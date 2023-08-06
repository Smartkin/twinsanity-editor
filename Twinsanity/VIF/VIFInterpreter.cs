using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twinsanity.VIF
{
    public class VIFInterpreter
    {
        public UInt32[] VIFn_R = { 0, 0, 0, 0 };
        public UInt32[] VIFn_C = { 0, 0, 0, 0 };
        public UInt32 VIFn_CYCLE;
        public UInt32 VIFn_MASK;
        public UInt32 VIFn_MODE;
        public UInt32 VIFn_ITOP;
        public UInt32 VIFn_ITOPS;
        public UInt32 VIF1_BASE;
        public UInt32 VIF1_OFST;
        public UInt32 VIF1_TOP;
        public UInt32 VIF1_TOPS;
        public UInt32 VIFn_MARK;
        public UInt32 VIFn_NUM;
        public UInt32 VIFn_CODE;

        private List<List<Vector4>> VUMem = new List<List<Vector4>>();
        private List<GIFTag> GifBuffer = new List<GIFTag>();
        private List<UInt32> tmpStack = new List<UInt32>();
        private List<List<UInt16>> AddressOuput = new List<List<UInt16>>();

        // Wrapper function for generating Interpreter instances
        public static VIFInterpreter InterpretCode(BinaryReader reader)
        {
            DMATag tag = new DMATag();
            tag.Read(reader);
            using (var mem = new MemoryStream())
            using (var writer = new BinaryWriter(mem))
            {
                // Transfer tag's extra data and its QWC data to VIF
                writer.Write(tag.Extra);
                writer.Write(reader.ReadBytes(tag.QWC * 0x10));
                mem.Position = 0;
                using (var vifReader = new BinaryReader(mem))
                {
                    var vifCode = new VIFInterpreter();
                    vifCode.Execute(vifReader);
                    return vifCode;
                }
            }
        }

        // Wrapper function for generating Interpreter instances using pure bytecode
        public static VIFInterpreter InterpretCode(Byte[] code)
        {
            using (MemoryStream codeStr = new MemoryStream(code))
            {
                using (BinaryReader codeReader = new BinaryReader(codeStr))
                {
                    return InterpretCode(codeReader);
                }
            }

        }

        public List<List<Vector4>> GetMem()
        {
            return VUMem;
        }

        public List<List<UInt16>> GetAddressOutput()
        {
            return AddressOuput;
        }

        public List<GIFTag> GetGifMem()
        {
            return GifBuffer;
        }

        private void Execute(BinaryReader reader)
        {
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                VIFCode vif = new VIFCode();
                vif.Read(reader);
                if (vif.isUnpack())
                {
                    Byte cmd = (Byte)vif.OP;
                    Byte vn = (Byte)((cmd & 0b1100) >> 2);
                    Byte vl = (Byte)((cmd & 0b0011) >> 0);
                    Byte m = (Byte)((cmd & 0b10000) >> 4);
                    Byte amount = vif.Amount;
                    UInt16 addr = (UInt16)(vif.Immediate & 0b111111111);
                    Byte usn = (Byte)(vif.Immediate & 0b0100000000000000);
                    Byte flg = (Byte)(vif.Immediate & 0b1000000000000000);
                    Byte WL = (Byte)((VIFn_CYCLE >> 8) & 0xFF);
                    Byte CL = (Byte)((VIFn_CYCLE >> 0) & 0xFF);
                    UInt32 dimensions = (UInt32)(vn + 1);
                    UInt32 packet_length = 0;
                    Boolean fill = WL > CL;
                    Console.WriteLine($"Total cycle specifier {CL}");
                    Console.WriteLine($"Write cycle specifier {WL}");
                    if (!fill)
                    {
                        UInt32 a = (UInt32)(32 >> vl);
                        UInt32 b = dimensions;
                        Single c = (Single)(a * b * amount);
                        Single d = c / 32.0f;
                        Single e = (Single)Math.Ceiling(d);
                        UInt32 f = (UInt32)e;
                        packet_length = 1 + f;
                    }
                    else
                    {
                        UInt32 n = (UInt32)(CL * (amount / WL) + ((amount % WL) > CL ? CL : (amount % WL)));
                        UInt32 a = (UInt32)(32 >> vl);
                        UInt32 b = dimensions;
                        Single c = (Single)(a * b * n);
                        Single d = c / 32.0f;
                        Single e = (Single)Math.Ceiling(d);
                        UInt32 f = (UInt32)e;
                        packet_length = 1 + f;
                    }
                    Console.WriteLine($"VU memory address 0x{addr:x}");
                    if (AddressOuput.Count == 0)
                    {
                        AddressOuput.Add(new List<ushort>());
                    }
                    AddressOuput[AddressOuput.Count - 1].Add(addr);
                    PackFormat fmt = (PackFormat)(vl | (vn << 2));
                    List<Vector4> vectors = new List<Vector4>(new Vector4[1024]);
                    tmpStack.Clear();
                    for (int i = 0; i < packet_length - 1; ++i)
                    {
                        tmpStack.Add(reader.ReadUInt32());
                    }
                    Unpack(tmpStack, vectors, fmt, amount, usn, false, 1, 1, 0);
                    VUMem.Add(vectors);
                    Console.WriteLine($"UNPACK {((int)packet_length - 1) * 4} bytes into {amount} 128bit vectors using {fmt} format");
                }
                else
                {
                    Console.WriteLine(vif.OP.ToString());
                    switch (vif.OP)
                    {
                        case VIFCodeEnum.NOP:
                            // Skip, no operation
                            break;
                        case VIFCodeEnum.STCYCL:
                            VIFn_CYCLE = vif.Immediate;
                            break;
                        case VIFCodeEnum.OFFSET:
                            VIF1_OFST = (uint)(vif.Immediate & 0b1111111111);
                            break;
                        case VIFCodeEnum.BASE:
                            VIF1_BASE = (uint)(vif.Immediate & 0b1111111111);
                            break;
                        case VIFCodeEnum.ITOP:
                            VIFn_ITOPS = (uint)(vif.Immediate & 0b1111111111);
                            break;
                        case VIFCodeEnum.STMOD:
                            VIFn_MODE = (uint)(vif.Immediate & 0b11);
                            break;
                        case VIFCodeEnum.MSKPATH3:
                            //throw new NotImplementedException();
                            break;
                        case VIFCodeEnum.MARK:
                            VIFn_MARK = vif.Immediate;
                            break;
                        case VIFCodeEnum.FLUSHE:

                            break;
                        case VIFCodeEnum.FLUSH:

                            break;
                        case VIFCodeEnum.FLUSHA:

                            break;
                        case VIFCodeEnum.MSCAL:
                            //throw new NotImplementedException();
                            AddressOuput.Add(new List<ushort>());
                            break;
                        case VIFCodeEnum.MSCNT:
                            //throw new NotImplementedException();
                            break;
                        case VIFCodeEnum.MSCALF:
                            //throw new NotImplementedException();
                            break;
                        case VIFCodeEnum.STMASK:
                            VIFn_MASK = reader.ReadUInt32();
                            break;
                        case VIFCodeEnum.STROW:
                            VIFn_R[0] = reader.ReadUInt32();
                            VIFn_R[1] = reader.ReadUInt32();
                            VIFn_R[2] = reader.ReadUInt32();
                            VIFn_R[3] = reader.ReadUInt32();
                            break;
                        case VIFCodeEnum.STCOL:
                            VIFn_C[0] = reader.ReadUInt32();
                            VIFn_C[1] = reader.ReadUInt32();
                            VIFn_C[2] = reader.ReadUInt32();
                            VIFn_C[3] = reader.ReadUInt32();
                            break;
                        case VIFCodeEnum.MPG:
                            //throw new NotImplementedException();
                            break;
                        case VIFCodeEnum.DIRECT:
                            UInt32 amount = (UInt32)((vif.Immediate == 0) ? 65536 * 16 : vif.Immediate * 16);
                            GifBuffer.Clear();
                            bool flag = false;
                            int len = 0;
                            do
                            {
                                GIFTag tag = new GIFTag();
                                tag.Read(reader);
                                GifBuffer.Add(tag);
                                flag = tag.EOP != 1;
                                int tagLen = tag.GetLength();
                                len += tagLen;
                            } while (flag);
                            break;
                        case VIFCodeEnum.DIRECTHL:

                            break;
                    }
                }
            }
        }

        private void SEXT(ref UInt32 n)
        {
            n = ((n & 0x8000) != 0) ? n | 0xFFFF0000 : n;
        }

        private void SEXT8(ref UInt32 n)
        {
            n = ((n & 0x80) != 0) ? n | 0xFFFFFF00 : n;
        }

        private void Unpack(List<UInt32> src, List<Vector4> dst, PackFormat fmt, Byte amount, Byte unsigned, Boolean fill, Byte write, Byte cycle, UInt16 addr)
        {
            var srcIdx = 0;
            switch (fmt)
            {
                case PackFormat.S_32:
                    for (int i = 0; i < amount; ++i)
                    {
                        Vector4 v = new Vector4();
                        v.SetBinaryX(src[i] + (IsInOffsetMode() ? VIFn_R[0] : 0));
                        v.SetBinaryY(src[i] + (IsInOffsetMode() ? VIFn_R[1] : 0));
                        v.SetBinaryZ(src[i] + (IsInOffsetMode() ? VIFn_R[2] : 0));
                        v.SetBinaryW(src[i] + (IsInOffsetMode() ? VIFn_R[3] : 0));

                        Fill(dst, v, i, fill, write, cycle, ref addr);
                    }
                    break;
                case PackFormat.S_16:
                    for (int i = 0; i < amount; ++i)
                    {
                        Vector4 v1 = new Vector4();
                        var mask = (i % 2 == 0) ? 0x0000FFFF : 0xFFFF0000;
                        UInt32 w1 = src[srcIdx] & mask;
                        if (i % 2 != 0)
                        {
                            w1 >>= 16;
                            srcIdx++;
                        }
                        if (unsigned == 0)
                        {
                            SEXT(ref w1);
                        }
                        v1.SetBinaryX(w1 + (IsInOffsetMode() ? VIFn_R[0] : 0));
                        v1.SetBinaryY(w1 + (IsInOffsetMode() ? VIFn_R[1] : 0));
                        v1.SetBinaryZ(w1 + (IsInOffsetMode() ? VIFn_R[2] : 0));
                        v1.SetBinaryW(w1 + (IsInOffsetMode() ? VIFn_R[3] : 0));
                        Fill(dst, v1, i, fill, write, cycle, ref addr);
                    }
                    break;
                case PackFormat.S_8:
                    for (int i = 0; i < amount; ++i)
                    {
                        Vector4 v1 = new Vector4();
                        UInt32 mask = 0x000000FF;
                        var shift = 0;
                        switch (i % 4)
                        {
                            case 1:
                                mask = 0x0000FF00;
                                shift = 8;
                                break;
                            case 2:
                                mask = 0x00FF0000;
                                shift = 16;
                                break;
                            case 3:
                                mask = 0xFF000000;
                                shift = 24;
                                break;
                        }
                        UInt32 w1 = (src[srcIdx] & mask) >> shift;
                        if (unsigned == 0)
                        {
                            SEXT8(ref w1);
                        }
                        v1.SetBinaryX(w1 + (IsInOffsetMode() ? VIFn_R[0] : 0));
                        v1.SetBinaryY(w1 + (IsInOffsetMode() ? VIFn_R[1] : 0));
                        v1.SetBinaryZ(w1 + (IsInOffsetMode() ? VIFn_R[2] : 0));
                        v1.SetBinaryW(w1 + (IsInOffsetMode() ? VIFn_R[3] : 0));
                        Fill(dst, v1, i, fill, write, cycle, ref addr);
                        if (i % 4 == 3)
                        {
                            srcIdx++;
                        }
                    }
                    break;
                case PackFormat.V2_32:
                    for (int i = 0; i < src.Count / 2; ++i)
                    {
                        Vector4 v = new Vector4();
                        v.SetBinaryX(src[i * 2 + 0] + (IsInOffsetMode() ? VIFn_R[0] : 0));
                        v.SetBinaryY(src[i * 2 + 1] + (IsInOffsetMode() ? VIFn_R[1] : 0));
                        Fill(dst, v, i, fill, write, cycle, ref addr);
                    }
                    break;
                case PackFormat.V2_16:
                    for (int i = 0; i < src.Count; ++i)
                    {
                        Vector4 v = new Vector4();
                        UInt32 w1 = src[i] & 0x0000FFFF;
                        UInt32 w2 = (src[i] & 0xFFFF0000) >> 16;
                        if (unsigned == 0)
                        {
                            SEXT(ref w1);
                            SEXT(ref w2);
                        }
                        v.SetBinaryX(w1 + (IsInOffsetMode() ? VIFn_R[0] : 0));
                        v.SetBinaryY(w2 + (IsInOffsetMode() ? VIFn_R[1] : 0));
                        Fill(dst, v, i, fill, write, cycle, ref addr);
                    }
                    break;
                case PackFormat.V2_8:
                    for (int i = 0; i < amount; ++i)
                    {
                        Vector4 v1 = new Vector4();
                        UInt32[] mask = { 0x000000FF, 0x0000FF00 };
                        Int32[] shift = { 0, 8 };
                        if (i % 2 != 0)
                        {
                            mask[0] = 0x00FF0000;
                            mask[1] = 0xFF000000;
                            shift[0] = 16;
                            shift[1] = 24;
                        }
                        UInt32 w1 = (Byte)((src[srcIdx] & mask[0]) >> shift[0]);
                        UInt32 w2 = (Byte)((src[srcIdx] & mask[1]) >> shift[1]);
                        if (unsigned == 0)
                        {
                            SEXT8(ref w1);
                            SEXT8(ref w2);
                        }

                        v1.SetBinaryX(w1 + (IsInOffsetMode() ? VIFn_R[0] : 0));
                        v1.SetBinaryY(w2 + (IsInOffsetMode() ? VIFn_R[1] : 0));
                        Fill(dst, v1, i, fill, write, cycle, ref addr);
                        if (i % 2 != 0)
                        {
                            srcIdx++;
                        }
                    }
                    break;
                case PackFormat.V3_32:
                    for (int i = 0; i < src.Count / 3; ++i)
                    {
                        Vector4 v = new Vector4();
                        v.SetBinaryX(src[i * 3 + 0] + (IsInOffsetMode() ? VIFn_R[0] : 0));
                        v.SetBinaryY(src[i * 3 + 1] + (IsInOffsetMode() ? VIFn_R[1] : 0));
                        v.SetBinaryZ(src[i * 3 + 2] + (IsInOffsetMode() ? VIFn_R[2] : 0));
                        Fill(dst, v, i, fill, write, cycle, ref addr);
                    }
                    break;
                case PackFormat.V3_16:
                    for (int i = 0; i < amount; ++i)
                    {
                        UInt32[] mask = { 0x0000FFFF, 0xFFFF0000 };
                        Int32[] shift = { 0, 16 };
                        if (i % 2 != 0)
                        {
                            mask[0] = 0xFFFF0000;
                            mask[1] = 0x0000FFFF;
                            shift[0] = 16;
                            shift[1] = 0;
                        }
                        Vector4 v1 = new Vector4();
                        UInt32 w1 = (src[srcIdx] & mask[0]) >> shift[0];
                        if (i % 2 != 0)
                        {
                            srcIdx++;
                        }
                        UInt32 w2 = (src[srcIdx] & mask[1]) >> shift[1];
                        if (i % 2 == 0)
                        {
                            srcIdx++;
                        }
                        UInt32 w3 = (src[srcIdx] & mask[0]) >> shift[0];
                        if (i % 2 != 0)
                        {
                            srcIdx++;
                        }
                        if (unsigned == 0)
                        {
                            SEXT(ref w1);
                            SEXT(ref w2);
                            SEXT(ref w3);
                        }
                        v1.SetBinaryX(w1 + (IsInOffsetMode() ? VIFn_R[0] : 0));
                        v1.SetBinaryY(w2 + (IsInOffsetMode() ? VIFn_R[1] : 0));
                        v1.SetBinaryZ(w3 + (IsInOffsetMode() ? VIFn_R[2] : 0));
                        Fill(dst, v1, i, fill, write, cycle, ref addr);
                    }
                    break;
                case PackFormat.V3_8:
                    for (int i = 0; i < amount; ++i)
                    {
                        Vector4 v1 = new Vector4();
                        // Is this scuffed? This is 100% scuffed
                        UInt32[] mask = { 0x000000FF, 0x0000FF00, 0x00FF0000 };
                        Int32[] shift = { 0, 8, 16 };
                        Boolean[] incIdx = { false, false, false };
                        switch (i % 4)
                        {
                            case 1:
                                mask[0] = 0xFF000000;
                                mask[1] = 0x000000FF;
                                mask[2] = 0x0000FF00;
                                shift[0] = 24;
                                shift[1] = 0;
                                shift[2] = 8;
                                incIdx[0] = true;
                                break;
                            case 2:
                                mask[0] = 0x00FF0000;
                                mask[1] = 0xFF000000;
                                mask[2] = 0x000000FF;
                                shift[0] = 16;
                                shift[1] = 24;
                                shift[2] = 0;
                                incIdx[1] = true;
                                break;
                            case 3:
                                mask[0] = 0x0000FF00;
                                mask[1] = 0x00FF0000;
                                mask[2] = 0xFF000000;
                                shift[0] = 8;
                                shift[1] = 16;
                                shift[2] = 24;
                                incIdx[2] = true;
                                break;
                        }
                        UInt32 w1 = (Byte)((src[srcIdx] & mask[0]) >> shift[0]);
                        if (incIdx[0]) srcIdx++;
                        UInt32 w2 = (Byte)((src[srcIdx] & mask[1]) >> shift[1]);
                        if (incIdx[1]) srcIdx++;
                        UInt32 w3 = (Byte)((src[srcIdx] & mask[2]) >> shift[2]);
                        if (incIdx[2]) srcIdx++;
                        if (unsigned == 0)
                        {
                            SEXT8(ref w1);
                            SEXT8(ref w2);
                            SEXT8(ref w3);
                        }
                        v1.SetBinaryX(w1 + (IsInOffsetMode() ? VIFn_R[0] : 0));
                        v1.SetBinaryY(w2 + (IsInOffsetMode() ? VIFn_R[1] : 0));
                        v1.SetBinaryZ(w3 + (IsInOffsetMode() ? VIFn_R[2] : 0));
                        Fill(dst, v1, i, fill, write, cycle, ref addr);
                    }
                    break;
                case PackFormat.V4_32:
                    for (int i = 0; i < src.Count / 4; ++i)
                    {
                        Vector4 v = new Vector4();
                        v.SetBinaryX(src[i * 4 + 0] + (IsInOffsetMode() ? VIFn_R[0] : 0));
                        v.SetBinaryY(src[i * 4 + 1] + (IsInOffsetMode() ? VIFn_R[1] : 0));
                        v.SetBinaryZ(src[i * 4 + 2] + (IsInOffsetMode() ? VIFn_R[2] : 0));
                        v.SetBinaryW(src[i * 4 + 3] + (IsInOffsetMode() ? VIFn_R[3] : 0));
                        Fill(dst, v, i, fill, write, cycle, ref addr);
                    }
                    break;
                case PackFormat.V4_16:
                    for (int i = 0; i < src.Count / 2; ++i)
                    {
                        Vector4 v = new Vector4();
                        UInt32 w1 = src[i * 2] & 0x0000FFFF;
                        UInt32 w2 = (src[i * 2] & 0xFFFF0000) >> 16;
                        UInt32 w3 = src[i * 2 + 1] & 0x0000FFFF;
                        UInt32 w4 = (src[i * 2 + 1] & 0xFFFF0000) >> 16;
                        if (unsigned == 0)
                        {
                            SEXT(ref w1);
                            SEXT(ref w2);
                            SEXT(ref w3);
                            SEXT(ref w4);
                        }
                        v.SetBinaryX(w1 + (IsInOffsetMode() ? VIFn_R[0] : 0));
                        v.SetBinaryY(w2 + (IsInOffsetMode() ? VIFn_R[1] : 0));
                        v.SetBinaryZ(w3 + (IsInOffsetMode() ? VIFn_R[2] : 0));
                        v.SetBinaryW(w4 + (IsInOffsetMode() ? VIFn_R[3] : 0));
                        Fill(dst, v, i, fill, write, cycle, ref addr);
                    }
                    break;
                case PackFormat.V4_8:
                    for (int i = 0; i < src.Count; ++i)
                    {
                        Vector4 v = new Vector4();
                        UInt32 w1 = (Byte)(src[i] & 0x000000FF);
                        UInt32 w2 = (Byte)((src[i] & 0x0000FF00) >> 8);
                        UInt32 w3 = (Byte)((src[i] & 0x00FF0000) >> 16);
                        UInt32 w4 = (Byte)((src[i] & 0xFF000000) >> 24);
                        if (unsigned == 0)
                        {
                            SEXT8(ref w1);
                            SEXT8(ref w2);
                            SEXT8(ref w3);
                            SEXT8(ref w4);
                        }
                        v.SetBinaryX(w1 + (IsInOffsetMode() ? VIFn_R[0] : 0));
                        v.SetBinaryY(w2 + (IsInOffsetMode() ? VIFn_R[1] : 0));
                        v.SetBinaryZ(w3 + (IsInOffsetMode() ? VIFn_R[2] : 0));
                        v.SetBinaryW(w4 + (IsInOffsetMode() ? VIFn_R[3] : 0));
                        Fill(dst, v, i, fill, write, cycle, ref addr);
                    }
                    break;
                case PackFormat.V4_5:
                    for (int i = 0; i < amount; ++i)
                    {
                        var mask = (i % 2 == 0) ? 0x0000FFFF : 0xFFFF0000;
                        UInt32 rgba1 = src[srcIdx] & mask;
                        if (i % 2 != 0)
                        {
                            rgba1 >>= 16;
                            srcIdx++;
                        }
                        Color c1 = new Color
                        {
                            R = (Byte)(rgba1 & 0b11111 << 3),
                            G = (Byte)((rgba1 & (0b11111 << 5)) >> 5 << 3),
                            B = (Byte)((rgba1 & (0b11111 << 10)) >> 10 << 3),
                            A = (Byte)((rgba1 & (0b1 << 15)) >> 15 << 7)
                        };
                        Fill(dst, c1.GetVector(), i, fill, write, cycle, ref addr);
                    }
                    break;
            }
        }

        private void Pack(List<Vector4> src, List<UInt32> dst, PackFormat fmt)
        {
            UInt32 resUInt = 0;
            switch (fmt)
            {
                case PackFormat.S_32:
                    for (var i = 0; i < src.Count; ++i)
                    {
                        dst.Add(src[i].GetBinaryX());
                    }
                    break;
                case PackFormat.S_16:
                    for (var i = 0; i < src.Count; ++i)
                    {
                        var vecPart = (src[i].GetBinaryX() & 0xFFFF);
                        if (i % 2 != 0)
                        {
                            resUInt |= (vecPart << 16);
                            dst.Add(resUInt);
                            resUInt = 0; // Reset bits
                        }
                        else
                        {
                            resUInt |= vecPart;
                            // Add last vector with padding 0 bits
                            if (i == src.Count - 1)
                            {
                                dst.Add(resUInt);
                            }
                        }
                    }
                    break;
                case PackFormat.S_8:
                    for (var i = 0; i < src.Count; ++i)
                    {
                        var vecPart = (src[i].GetBinaryX() & 0xFF);
                        switch (i % 4)
                        {
                            case 0:
                                resUInt |= vecPart;
                                break;
                            case 1:
                                resUInt |= (vecPart << 8);
                                break;
                            case 2:
                                resUInt |= (vecPart << 16);
                                break;
                            case 3:
                                resUInt |= (vecPart << 24);
                                dst.Add(resUInt);
                                resUInt = 0; // Reset bits
                                break;
                        }
                        // Add last vector with padding 0 bits
                        if (i == src.Count - 1 && i % 4 != 3)
                        {
                            dst.Add(resUInt);
                        }
                    }
                    break;
                case PackFormat.V2_32:
                    for (var i = 0; i < src.Count; ++i)
                    {
                        dst.Add(src[i].GetBinaryX());
                        dst.Add(src[i].GetBinaryY());
                    }
                    break;
                case PackFormat.V2_16:
                    for (var i = 0; i < src.Count; ++i)
                    {
                        resUInt |= src[i].GetBinaryX() & 0xFFFF;
                        resUInt |= (src[i].GetBinaryY() & 0xFFFF) << 16;
                        dst.Add(resUInt);
                        resUInt = 0; // Reset bits
                    }
                    break;
                case PackFormat.V2_8:
                    for (var i = 0; i < src.Count; ++i)
                    {
                        var vecX = src[i].GetBinaryX() & 0xFF;
                        var vecY = src[i].GetBinaryY() & 0xFF;
                        if (i % 2 != 0)
                        {
                            resUInt |= (vecX << 16);
                            resUInt |= (vecY << 24);
                            dst.Add(resUInt);
                            resUInt = 0; // Reset bits
                        }
                        else
                        {
                            resUInt |= vecX;
                            resUInt |= (vecY << 8);
                            // Add last vector with padding 0 bits
                            if (i == src.Count - 1)
                            {
                                dst.Add(resUInt);
                            }
                        }
                    }
                    break;
                case PackFormat.V3_32:
                    for (var i = 0; i < src.Count; ++i)
                    {
                        dst.Add(src[i].GetBinaryX());
                        dst.Add(src[i].GetBinaryY());
                        dst.Add(src[i].GetBinaryZ());
                    }
                    break;
                case PackFormat.V3_16:
                    for (var i = 0; i < src.Count; ++i)
                    {
                        var vecX = src[i].GetBinaryX() & 0xFFFF;
                        var vecY = src[i].GetBinaryY() & 0xFFFF;
                        var vecZ = src[i].GetBinaryZ() & 0xFFFF;
                        if (i % 2 != 0)
                        {
                            resUInt |= (vecX << 16);
                            dst.Add(resUInt);
                            resUInt = 0; // Reset bits
                            resUInt |= vecY;
                            resUInt |= (vecZ << 16);
                            dst.Add(resUInt);
                            resUInt = 0; // Reset bits
                        }
                        else
                        {
                            resUInt |= vecX;
                            resUInt |= (vecY << 16);
                            dst.Add(resUInt);
                            resUInt = 0; // Reset bits
                            resUInt |= vecZ;
                            // Add last vector with padding 0 bits
                            if (i == src.Count - 1)
                            {
                                dst.Add(resUInt);
                            }
                        }
                    }
                    break;
                case PackFormat.V3_8:
                    for (var i = 0; i < src.Count; ++i)
                    {
                        var vecX = src[i].GetBinaryX() & 0xFF;
                        var vecY = src[i].GetBinaryY() & 0xFF;
                        var vecZ = src[i].GetBinaryZ() & 0xFF;
                        switch (i % 4)
                        {
                            case 0:
                                resUInt |= vecX;
                                resUInt |= (vecY << 8);
                                resUInt |= (vecZ << 16);
                                break;
                            case 1:
                                resUInt |= (vecX << 24);
                                dst.Add(resUInt);
                                resUInt = 0; // Reset bits
                                resUInt |= vecY;
                                resUInt |= (vecZ << 8);
                                break;
                            case 2:
                                resUInt |= (vecX << 16);
                                resUInt |= (vecY << 24);
                                dst.Add(resUInt);
                                resUInt = 0; // Reset bits
                                resUInt |= vecZ;
                                break;
                            case 3:
                                resUInt |= (vecX << 8);
                                resUInt |= (vecY << 16);
                                resUInt |= (vecZ << 24);
                                dst.Add(resUInt);
                                resUInt = 0; // Reset bits
                                break;
                        }
                        // Add last vector with padding 0 bits
                        if (i == src.Count - 1 && i % 4 != 3)
                        {
                            dst.Add(resUInt);
                        }
                    }
                    break;
                case PackFormat.V4_32:
                    for (var i = 0; i < src.Count; ++i)
                    {
                        dst.Add(src[i].GetBinaryX());
                        dst.Add(src[i].GetBinaryY());
                        dst.Add(src[i].GetBinaryZ());
                        dst.Add(src[i].GetBinaryW());
                    }
                    break;
                case PackFormat.V4_16:
                    for (var i = 0; i < src.Count; ++i)
                    {
                        var vecX = src[i].GetBinaryX() & 0xFFFF;
                        var vecY = src[i].GetBinaryY() & 0xFFFF;
                        var vecZ = src[i].GetBinaryZ() & 0xFFFF;
                        var vecW = src[i].GetBinaryW() & 0xFFFF;
                        resUInt |= vecX;
                        resUInt |= (vecY << 16);
                        dst.Add(resUInt);
                        resUInt = 0; // Reset bits
                        resUInt |= vecZ;
                        resUInt |= (vecW << 16);
                        dst.Add(resUInt);
                        resUInt = 0; // Reset bits
                    }
                    break;
                case PackFormat.V4_8:
                    for (var i = 0; i < src.Count; ++i)
                    {
                        var vecX = src[i].GetBinaryX() & 0xFF;
                        var vecY = src[i].GetBinaryY() & 0xFF;
                        var vecZ = src[i].GetBinaryZ() & 0xFF;
                        var vecW = src[i].GetBinaryW() & 0xFF;
                        resUInt |= vecX;
                        resUInt |= (vecY << 8);
                        resUInt |= (vecZ << 16);
                        resUInt |= (vecW << 24);
                        dst.Add(resUInt);
                        resUInt = 0; // Reset bits
                    }
                    break;
                case PackFormat.V4_5:
                    for (var i = 0; i < src.Count; ++i)
                    {
                        var c = src[i].GetColor();
                        UInt32 r = (UInt32)c.R >> 3 & 0b11111;
                        UInt32 g = (UInt32)c.G >> 3 & 0b11111;
                        UInt32 b = (UInt32)c.B >> 3 & 0b11111;
                        UInt32 a = (UInt32)c.A >> 7 & 0b1;
                        if (i % 2 != 0)
                        {
                            resUInt |= (r << 16);
                            resUInt |= (g << 21);
                            resUInt |= (b << 26);
                            resUInt |= (a << 31);
                            dst.Add(resUInt);
                            resUInt = 0; // Reset bits
                        }
                        else
                        {
                            resUInt |= r;
                            resUInt |= (g << 5);
                            resUInt |= (b << 10);
                            resUInt |= (a << 15);
                            // Add last vector with padding 0 bits
                            if (i == src.Count - 1)
                            {
                                dst.Add(resUInt);
                            }
                        }
                    }
                    break;
            }
        }

        private void Fill(List<Vector4> dst, Vector4 vec, Int32 index, Boolean fill, Byte wl, Byte cl, ref UInt16 addr)
        {
            // Fill writing
            if (fill)
            {
                var doFill = ((index + 1) % cl == 0);
                dst[addr++] = vec;
                if (doFill)
                {
                    var fillVec = new Vector4();
                    fillVec.SetBinaryX(VIFn_R[0]);
                    fillVec.SetBinaryY(VIFn_R[1]);
                    fillVec.SetBinaryZ(VIFn_R[2]);
                    fillVec.SetBinaryW(VIFn_R[3]);
                    for (int i = 0; i < wl - cl; i++)
                    {
                        dst[addr++] = fillVec;
                    }
                }
                return;
            }
            // Skip writing
            var nullVec = new Vector4();
            var skipAmt = cl - wl;
            dst[addr++] = vec;
            if (wl != 0)
            {
                var doSkip = ((index + 1) % wl == 0);
                if (doSkip)
                {
                    addr += (UInt16)skipAmt;
                }
            }
        }

        private Boolean IsInOffsetMode()
        {
            return (VIFn_MODE & 0b01) == 1;
        }
    }
    public enum PackFormat
    {
        S_32 = 0b0000,
        S_16 = 0b0001,
        S_8 = 0b0010,
        V2_32 = 0b0100,
        V2_16 = 0b0101,
        V2_8 = 0b0110,
        V3_32 = 0b1000,
        V3_16 = 0b1001,
        V3_8 = 0b1010,
        V4_32 = 0b1100,
        V4_16 = 0b1101,
        V4_8 = 0b1110,
        V4_5 = 0b1111,
    }
}
