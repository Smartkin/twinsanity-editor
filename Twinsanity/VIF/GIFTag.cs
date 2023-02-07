using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twinsanity.VIF
{
    public class GIFTag
    {
        public UInt16 NLOOP { get; set; }
        public Byte EOP { get; set; }
        public Byte PRE { get; set; }
        public UInt16 PRIM { get; set; }
        public GIFModeEnum FLG { get; set; }
        public Byte NREG { get; set; }
        public REGSEnum[] REGS { get; set; }
        public List<RegOutput> Data { get; set; }
        private UInt64 Q = 0x3F800000;
        public void Read(BinaryReader reader)
        {
            UInt64 low = reader.ReadUInt64();
            NLOOP = (UInt16)((low & (UInt64)0b111111111111111) >> 0);
            EOP = (Byte)((low & ((UInt64)0b1 << 15)) >> 15);
            PRE = (Byte)((low & ((UInt64)0b1 << 46)) >> 46);
            PRIM = (UInt16)((low & ((UInt64)0b11111111111 << 47)) >> 47);
            FLG = (GIFModeEnum)((low & (((UInt64)0b11) << 58)) >> 58);
            NREG = (Byte)((low & ((UInt64)0b1111 << 60)) >> 60);
            NREG = (NREG == 0) ? (Byte)16 : NREG;
            REGS = new REGSEnum[16];
            UInt64 high = reader.ReadUInt64();
            for (int i = 0; i < 16; ++i)
            {
                REGS[i] = (REGSEnum)(high & 0b1111);
                high >>= 4;
            }
            Data = new List<RegOutput>();
            if (PRE == 1)
            {
                RegOutput prim = new RegOutput();
                prim.REG = REGSEnum.PRIM;
                prim.Output = PRIM;
                Data.Add(prim);
            }
            switch (FLG)
            {
                case GIFModeEnum.IMAGE:
                    for (int i = 0; i < NLOOP; ++i)
                    {
                        Interpret(reader, REGSEnum.HWREG, Data);
                    }
                    break;
                case GIFModeEnum.REGLIST:
                    for (int i = 0; i < NLOOP; ++i)
                    {
                        for (int j = 0; j < NREG; ++j)
                        {
                            Interpret(reader, REGS[j], Data);
                        }
                    }
                    break;
                case GIFModeEnum.PACKED:
                    for (int i = 0; i < NLOOP; ++i)
                    {
                        for (int j = 0; j < NREG; ++j)
                        {
                            Interpret(reader, REGS[j], Data);
                        }
                    }
                    break;
            }

        }
        private void Interpret(BinaryReader reader, REGSEnum REG, List<RegOutput> list)
        {
            RegOutput output = new RegOutput();
            UInt64 low, high;
            switch (FLG)
            {
                case GIFModeEnum.PACKED:
                    high = reader.ReadUInt64();
                    low = reader.ReadUInt64();
                    output.REG = REG;
                    switch (REG)
                    {
                        case REGSEnum.RGBAQ:
                            UInt64 r = BitUtils.GetBits(low, 8, 0);
                            UInt64 g = BitUtils.GetBits(low, 8, 23);
                            UInt64 b = BitUtils.GetBits(low, 8, 46);
                            UInt64 a = BitUtils.GetBits(low, 8, 69);
                            output.Output = BitUtils.SetBits(BitUtils.SetBits(BitUtils.SetBits(BitUtils.SetBits(r, g, 8), b, 16), a, 24), Q, 32);
                            break;
                        case REGSEnum.ST:
                            UInt64 s = BitUtils.GetBits(low, 32, 0);
                            UInt64 t = BitUtils.GetBits(low, 32, 32);
                            UInt64 q = BitUtils.GetBits(high, 32, 0);
                            Q = q;
                            output.Output = BitUtils.SetBits(s, t, 32);
                            break;
                        case REGSEnum.UV:
                            UInt64 v = BitUtils.GetBits(low, 14, 0);
                            UInt64 u = BitUtils.GetBits(low, 14, 32);
                            output.Output = BitUtils.SetBits(v, u, 16);
                            break;
                        case REGSEnum.XYZF2:
                            {
                                UInt64 x = BitUtils.GetBits(low, 16, 0);
                                UInt64 y = BitUtils.GetBits(low, 16, 32);
                                UInt64 z = BitUtils.GetBits(high, 24, 4);
                                UInt64 f = BitUtils.GetBits(high, 8, 36);
                                UInt64 adc = BitUtils.GetBits(high, 1, 47);
                                if (adc == 0)
                                {
                                    output.REG = REGSEnum.XYZF2;
                                }
                                else
                                {
                                    output.REG = REGSEnum.XYZF3;
                                }
                                output.Output = BitUtils.SetBits(BitUtils.SetBits(BitUtils.SetBits(x, y, 16), z, 32), f, 56);
                            }
                            break;
                        case REGSEnum.XYZ2:
                            {
                                UInt64 x = BitUtils.GetBits(low, 16, 0);
                                UInt64 y = BitUtils.GetBits(low, 16, 32);
                                UInt64 z = BitUtils.GetBits(high, 32, 0);
                                UInt64 adc = BitUtils.GetBits(high, 1, 47);
                                if (adc == 0)
                                {
                                    output.REG = REGSEnum.XYZ2;
                                }
                                else
                                {
                                    output.REG = REGSEnum.XYZ3;
                                }
                                output.Output = BitUtils.SetBits(BitUtils.SetBits(x, y, 16), z, 32);
                            }

                            break;
                        case REGSEnum.FOG:
                            {
                                UInt64 f = BitUtils.GetBits(high, 8, 36);
                                output.Output = BitUtils.SetBits(0, f, 56);
                            }
                            break;
                        case REGSEnum.ApD:
                            UInt64 Data = high;
                            UInt64 Addr = BitUtils.GetBits(low, 7, 0);
                            output.Output = Data;
                            output.Address = Addr;
                            break;
                        case REGSEnum.TEX0_1:
                        case REGSEnum.TEX1_1:
                        case REGSEnum.CLAMP_1:
                        case REGSEnum.CLAMP_2:
                        case REGSEnum.XYZF3:
                        case REGSEnum.XYZ3:
                            output.Output = low;
                            break;
                        case REGSEnum.NOP:
                        default:
                            break;
                    }
                    list.Add(output);
                    break;
                case GIFModeEnum.REGLIST:
                    output.Output = reader.ReadUInt64();
                    output.REG = REG;
                    list.Add(output);
                    break;
                case GIFModeEnum.IMAGE:
                    RegOutput output1 = new RegOutput();
                    RegOutput output2 = new RegOutput();
                    high = reader.ReadUInt64();
                    output2.REG = REGSEnum.HWREG;
                    output2.Output = high;
                    low = reader.ReadUInt64();
                    output1.REG = REGSEnum.HWREG;
                    output1.Output = low;
                    list.Add(output1);
                    list.Add(output2);
                    break;
                case GIFModeEnum.DISABLE:
                    // Nothing
                    break;
            }
        }
        public void Write(BinaryWriter writer)
        {
            UInt64 low = 0;
            low |= ((UInt64)NLOOP & 0b111111111111111) << 0;
            low |= ((UInt64)EOP & 0b1) << 15;
            low |= ((UInt64)PRE & 0b1) << 46;
            low |= ((UInt64)PRIM & 0b11111111111) << 47;
            low |= ((UInt64)FLG & 0b11) << 58;
            low |= ((UInt64)NREG & 0b1111) << 60;
            writer.Write(low);
            UInt64 high = 0;
            for (int i = 0; i < 16; ++i)
            {
                high |= (UInt64)REGS[REGS.Length - i - 1] & 0b1111;
                if (i != 15)
                {
                    high <<= 4;
                }
            }
            writer.Write(high);
            switch (FLG)
            {
                case GIFModeEnum.PACKED:
                    // Twinsanity textures only use A+D with PACKED, so we can safely ignore all the other writes
                    for (var i = 0; i < Data.Count; ++i)
                    {
                        switch (Data[i].REG)
                        {
                            case REGSEnum.ApD:
                                writer.Write(Data[i].Output);
                                writer.Write(Data[i].Address);
                                break;
                        }
                    }
                    break;
                case GIFModeEnum.REGLIST:
                    for (var i = 0; i < Data.Count; ++i)
                    {
                        writer.Write(Data[i].Output);
                    }
                    break;
                case GIFModeEnum.IMAGE:
                    for (var i = 0; i < Data.Count; i += 2)
                    {
                        writer.Write(Data[i + 1].Output);
                        writer.Write(Data[i].Output);
                    }
                    break;
                case GIFModeEnum.DISABLE:
                    // Nothing
                    break;
            }
        }
        public Int32 GetLength()
        {
            switch (FLG)
            {
                case GIFModeEnum.PACKED:
                    return NREG * NLOOP; // QWORD
                case GIFModeEnum.REGLIST:
                    return NREG * NLOOP; // DWORD
                case GIFModeEnum.IMAGE:
                    return NLOOP; // QWORD
                case GIFModeEnum.DISABLE:
                    return 0;
            }
            return 0;
        }
    }
    public enum GIFModeEnum
    {
        PACKED = 0b00,
        REGLIST = 0b01,
        IMAGE = 0b10,
        DISABLE = 0b11
    }
    public enum REGSEnum
    {
        PRIM = 0x00,
        RGBAQ = 0x01,
        ST = 0x02,
        UV = 0x03,
        XYZF2 = 0x04,
        XYZ2 = 0x05,
        TEX0_1 = 0x06,
        TEX1_1 = 0x07,
        CLAMP_1 = 0x08,
        CLAMP_2 = 0x09,
        FOG = 0x0a,
        RESERVED = 0x0b,
        XYZF3 = 0x0c,
        XYZ3 = 0x0d,
        ApD = 0x0e,
        NOP = 0x0f,
        HWREG = 0xff
    }
    public class RegOutput
    {
        private UInt64 _Output;
        public UInt64 Output
        {
            get
            {
                return _Output;
            }
            set
            {
                _Output = value;
                switch (REG)
                {
                    case REGSEnum.RGBAQ:
                        R = (Byte)BitUtils.GetBits(_Output, 8, 0);
                        G = (Byte)BitUtils.GetBits(_Output, 8, 8);
                        B = (Byte)BitUtils.GetBits(_Output, 8, 16);
                        A = (Byte)BitUtils.GetBits(_Output, 8, 24);
                        Q = BitConverter.ToSingle(BitConverter.GetBytes((UInt32)BitUtils.GetBits(_Output, 32, 32)), 0);
                        break;
                    case REGSEnum.ST:
                        S = BitConverter.ToSingle(BitConverter.GetBytes((UInt32)BitUtils.GetBits(_Output, 32, 0)), 0);
                        T = BitConverter.ToSingle(BitConverter.GetBytes((UInt32)BitUtils.GetBits(_Output, 32, 32)), 0);
                        break;
                    case REGSEnum.UV:
                        UInt64 v = BitUtils.GetBits(_Output, 14, 0);
                        UInt64 v_int = BitUtils.GetBits(v, 10, 4);
                        UInt64 v_fract = BitUtils.GetBits(v, 4, 0);
                        UInt64 u = BitUtils.GetBits(_Output, 14, 16);
                        UInt64 u_int = BitUtils.GetBits(u, 10, 4);
                        UInt64 u_fract = BitUtils.GetBits(u, 4, 0);
                        U = BitUtils.FixedToSingle(u_int, u_fract, 4);
                        V = BitUtils.FixedToSingle(v_int, v_fract, 4);
                        break;
                    case REGSEnum.XYZF3:
                    case REGSEnum.XYZF2:
                        {
                            Z = (UInt32)BitUtils.GetBits(_Output, 24, 32);
                            UInt64 x = BitUtils.GetBits(_Output, 16, 0);
                            UInt64 x_int = BitUtils.GetBits(x, 12, 4);
                            UInt64 x_fract = BitUtils.GetBits(x, 4, 0);
                            UInt64 y = BitUtils.GetBits(_Output, 16, 16);
                            UInt64 y_int = BitUtils.GetBits(y, 12, 4);
                            UInt64 y_fract = BitUtils.GetBits(y, 4, 0);
                            X = BitUtils.FixedToSingle(x_int, x_fract, 4);
                            Y = BitUtils.FixedToSingle(y_int, y_fract, 4);
                            F = (Byte)BitUtils.GetBits(_Output, 8, 56);
                        }
                        break;
                    case REGSEnum.XYZ3:
                    case REGSEnum.XYZ2:
                        {
                            Z = (UInt32)(_Output >> 32);
                            UInt64 x = BitUtils.GetBits(_Output, 16, 0);
                            UInt64 x_int = BitUtils.GetBits(x, 12, 4);
                            UInt64 x_fract = BitUtils.GetBits(x, 4, 0);
                            UInt64 y = BitUtils.GetBits(_Output, 16, 16);
                            UInt64 y_int = BitUtils.GetBits(y, 12, 4);
                            UInt64 y_fract = BitUtils.GetBits(y, 4, 0);
                            X = BitUtils.FixedToSingle(x_int, x_fract, 4);
                            Y = BitUtils.FixedToSingle(y_int, y_fract, 4);
                        }
                        break;
                    case REGSEnum.FOG:
                        {
                            F = (Byte)(_Output >> 56);
                        }
                        break;
                }
            }
        }
        public REGSEnum REG { get; set; }
        public UInt64 Address { get; set; }
        public UInt16 PRIM { get; set; }
        public Byte A { get; set; }
        public Byte R { get; set; }
        public Byte G { get; set; }
        public Byte B { get; set; }
        public Single Q { get; set; }
        public Single T { get; set; }
        public Single S { get; set; }
        public Single V { get; set; }
        public Single U { get; set; }
        public Single X { get; set; }
        public Single Y { get; set; }
        public UInt32 Z { get; set; }
        public Byte F { get; set; }
    }
    public static class BitUtils
    {
        public static Single[] Fracts;

        static BitUtils()
        {
            Fracts = new Single[64];
            for (int i = 0; i < 64; ++i)
            {
                Fracts[i] = 1.0f / ((Single)Math.Pow(2, i));
            }
        }
        public static UInt64 GetBits(UInt64 src, Byte len, Byte offset)
        {
            UInt64 mask = 0;
            for (int i = 0; i < len; ++i)
            {
                mask = (mask << 1) | 1;
            }
            return ((src & ((UInt64)0b11111111 << offset)) >> offset);
        }
        public static UInt64 SetBits(UInt64 src, UInt64 val, Byte offset)
        {
            return src | (val << offset);
        }
        public static Single FixedToSingle(UInt64 I, UInt64 F, Byte fractLength)
        {
            Single result = (Single)I;
            for (int i = 1; i <= fractLength; ++i)
            {
                Single fract = Fracts[fractLength - i];
                if ((F & 0b1) != 0)
                {
                    result += fract;
                }
                F >>= 1;
            }
            return result;
        }
    }
}
