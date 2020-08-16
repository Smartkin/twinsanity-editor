using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Twinsanity
{
    public class Script : TwinsItem
    {
        public class HeaderScriptStruct
        {
            public HeaderScriptStruct(BinaryReader reader)
            {
                unkIntPairs = reader.ReadUInt32();
                pairs = new UnkIntPairs[unkIntPairs];
                for (int i = 0; i < unkIntPairs; i++)
                {
                    pairs[i] = new UnkIntPairs();
                    pairs[i].mainScriptIndex = reader.ReadInt32();
                    pairs[i].unkInt2 = reader.ReadUInt32();
                }
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(unkIntPairs);
                for (int i = 0; i < unkIntPairs; i++)
                {
                    writer.Write(pairs[i].mainScriptIndex);
                    writer.Write(pairs[i].unkInt2);
                }
            }
            public Int32 GetLength()
            {
                return (Int32)(4 + unkIntPairs * 8);
            }
            public class UnkIntPairs
            {
                public int mainScriptIndex;
                public uint unkInt2;
                public override string ToString()
                {
                    return $"ID: {mainScriptIndex} ({unkInt2})";
                }
            }
            public uint unkIntPairs;
            public UnkIntPairs[] pairs;
        }

        public class MainScriptStruct
        {
            public MainScriptStruct(BinaryReader reader)
            {
                int len = reader.ReadInt32();
                name = new string(reader.ReadChars(len));
                LinkedScriptsCount = reader.ReadInt32();
                unkInt2 = reader.ReadInt32();
                linkedScript1 = new LinkedScript(reader);
                LinkedScript ptr = linkedScript1;
                while (null != ptr)
                {
                    if ((ptr.bitfield & 0x1F) != 0)
                    {
                        ptr.type2 = new SupportType2(reader);
                    }
                    ptr = ptr.nextLinked;
                }
            }
            public void Write(BinaryWriter writer)
            {
                writer.Write(name.Length);
                writer.Write(name.ToCharArray());
                writer.Write(LinkedScriptsCount);
                writer.Write(unkInt2);
                linkedScript1.Write(writer);
                LinkedScript ptr = linkedScript1;
                while (ptr != null)
                {
                    if (null != ptr.type2)
                    {
                        ptr.type2.Write(writer);
                    }
                    ptr = ptr.nextLinked;
                }
            }
            public Int32 GetLength()
            {
                Int32 headerSize = 4 + name.Length + 4 + 4;
                Int32 linkedSize = linkedScript1.GetLength();
                Int32 type2Size = 0;
                LinkedScript ptr = linkedScript1;
                while (ptr != null)
                {
                    if (null != ptr.type2)
                    {
                        type2Size += ptr.type2.GetLength();
                    }
                    ptr = ptr.nextLinked;
                }
                return headerSize + linkedSize + type2Size;
            }
            public String name { get; set; }
            public Int32 LinkedScriptsCount { get; set; }
            public Int32 unkInt2 { get; set; }
            public LinkedScript linkedScript1 { get; set; }
            public LinkedScript linkedScript2 { get; set; }

            public class SupportType1
            {
                public SupportType1()
                {
                    unkByte1 = 0;
                    unkByte2 = 0;
                    unkUShort1 = 0;
                    unkInt1 = 0;
                    byteArray = new byte[0];
                }
                public SupportType1(BinaryReader reader)
                {
                    unkByte1 = reader.ReadByte();
                    unkByte2 = reader.ReadByte();
                    unkUShort1 = reader.ReadUInt16();
                    unkInt1 = reader.ReadInt32();
                    Int32 byteArrayLen = unkByte1 + unkByte2 * 4;
                    byteArray = reader.ReadBytes(unkByte1 + unkByte2 * 4);
                }
                public void Write(BinaryWriter writer)
                {
                    writer.Write(unkByte1);
                    writer.Write(unkByte2);
                    writer.Write(unkUShort1);
                    writer.Write(unkInt1);
                    writer.Write(byteArray);
                }
                public Int32 GetLength()
                {
                    return 8 + byteArray.Length;
                }
                public byte unkByte1 { get; set; }
                public byte unkByte2 { get; set; }
                public UInt16 unkUShort1 { get; set; }
                public Int32 unkInt1 { get; set; }
                public Byte[] byteArray { get; set; }
                public bool isValidArraySize()
                {
                    return byteArray.Length == unkByte1 + 4 * unkByte2;
                }
            }
            public class SupportType2
            {
                public SupportType2()
                {
                    bitfield = 0;
                    linkedScriptListIndex = 0;
                    type3 = null;
                    type4 = null;
                    nextType2 = null;
                }
                public SupportType2(BinaryReader reader)
                {
                    bitfield = reader.ReadInt32();
                    if ((bitfield & 0x400) != 0)
                    {
                        linkedScriptListIndex = reader.ReadInt32();
                    }
                    if ((bitfield & 0x200) != 0)
                    {
                        type3 = new SupportType3(reader);
                    }
                    if ((bitfield & 0xFF) != 0)
                    {
                        type4 = new SupportType4(reader);
                    }
                    if ((bitfield & 0x800) != 0)
                    {
                        nextType2 = new SupportType2(reader);
                    }
                }
                public void Write(BinaryWriter writer)
                {
                    writer.Write(bitfield);
                    if ((bitfield & 0x400) != 0)
                    {
                        writer.Write(linkedScriptListIndex);
                    }
                    if ((bitfield & 0x200) != 0)
                    {
                        type3.Write(writer);
                    }
                    if ((bitfield & 0xFF) != 0)
                    {
                        type4.Write(writer);
                    }
                    if ((bitfield & 0x800) != 0)
                    {
                        nextType2.Write(writer);
                    }
                }
                public Int32 GetLength()
                {
                    return 4 + (((bitfield & 0x400) != 0) ? 4 : 0)
                        + (((bitfield & 0x200) != 0) ? type3.GetLength() : 0)
                        + (((bitfield & 0xFF) != 0) ? type4.GetLength() : 0)
                        + (((bitfield & 0x800) != 0) ? nextType2.GetLength() : 0);
                }
                public Int32 bitfield { get; set; }
                public Int32 linkedScriptListIndex { get; set; }
                public SupportType3 type3 { get; set; }
                public SupportType4 type4 { get; set; }
                public SupportType2 nextType2 { get; set; }
                public bool isBitFieldValid()
                {
                    if (((bitfield & 0x200) == 0) && (type3 != null))
                    {
                        return false;
                    }
                    if (((bitfield & 0x200) != 0) && (type3 == null))
                    {
                        return false;
                    }
                    if (((bitfield & 0xFF) == 0) && (type4 != null))
                    {
                        return false;
                    }
                    if (((bitfield & 0xFF) != 0) && (type4 == null))
                    {
                        return false;
                    }
                    if (((bitfield & 0x800) == 0) && (nextType2 != null))
                    {
                        return false;
                    }
                    if (((bitfield & 0x800) != 0) && (nextType2 == null))
                    {
                        return false;
                    }
                    return true;
                }
                public Byte type4Count
                {
                    get
                    {
                        return (Byte)(bitfield & 0xFF);
                    }
                    set
                    {
                        bitfield = (Int32)(bitfield & 0xFFFFFF00) | (value & 0xFF);
                    }
                }
                public bool CreateType3()
                {
                    if (type3 == null)
                    {
                        type3 = new SupportType3();
                        bitfield = (Int16)(bitfield | 0x200);
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                public bool DeleteType3()
                {
                    if (type3 != null)
                    {
                        type3 = null;
                        bitfield = (Int16)(bitfield & ~0x200);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                public bool AddType4(Int32 position)
                {
                    if (position > type4Count || position < 0)
                    {
                        return false;
                    }
                    if (type4Count == 0)
                    {
                        type4 = new SupportType4();
                    }
                    else if (position == type4Count)
                    {
                        SupportType4 ptr = type4;
                        while (ptr.nextType4 != null)
                        {
                            ptr = ptr.nextType4;
                        }
                        ptr.internalIndex = (Int16)(ptr.internalIndex | 0x1000000);
                        ptr.nextType4 = new SupportType4();
                    }
                    else
                    {
                        int pos = 0;
                        SupportType4 prevPtr = null;
                        SupportType4 ptr = type4;
                        SupportType4 newType2 = new SupportType4();
                        while (pos < position)
                        {
                            prevPtr = ptr;
                            ptr = ptr.nextType4;
                            ++pos;
                        }
                        if (prevPtr != null)
                        {
                            prevPtr.nextType4 = newType2;
                            prevPtr.nextType4.nextType4 = ptr;
                        }
                        else
                        {
                            newType2.nextType4 = type4;
                            type4 = newType2;
                        }

                        if (newType2.nextType4 != null)
                        {
                            newType2.internalIndex = (Int32)(newType2.internalIndex | 0x1000000);
                        }
                    }
                    ++type4Count;
                    return true;
                }
                public bool DeleteType4(Int32 position)
                {
                    if (position >= type4Count || position < 0)
                    {
                        return false;
                    }
                    if (position == 0)
                    {
                        type4 = type4.nextType4;
                    }
                    else
                    {
                        int pos = 0;
                        SupportType4 prevPtr = null;
                        SupportType4 ptr = type4;
                        while (pos < position)
                        {
                            prevPtr = ptr;
                            ptr = ptr.nextType4;
                            ++pos;
                        }
                        prevPtr.nextType4 = ptr.nextType4;
                        if (prevPtr.nextType4 == null)
                        {
                            prevPtr.internalIndex = (Int32)(prevPtr.internalIndex & ~0x1000000);
                        }
                    }
                    --type4Count;
                    return true;
                }
            }
            public class SupportType3
            {
                public SupportType3()
                {
                    unkInt1 = 0;
                    X = 0.0f;
                    Y = 0.0f;
                    Z = 0.0f;
                }
                public SupportType3(BinaryReader reader)
                {
                    unkInt1 = reader.ReadInt32();
                    vTableAddress = 0x0;
                    X = reader.ReadSingle();
                    Y = reader.ReadSingle();
                    Z = reader.ReadSingle();
                }
                public void Write(BinaryWriter writer)
                {
                    writer.Write(unkInt1);
                    writer.Write(X);
                    writer.Write(Y);
                    writer.Write(Z);
                }
                public Int32 GetLength()
                {
                    return 16;
                }
                public Int32 unkInt1 { get; set; }
                public Int32 vTableAddress { get; set; }
                public float X { get; set; }
                public float Y { get; set; }
                public float Z { get; set; }
                public UInt16 VTableIndex
                {
                    get
                    {
                        return (UInt16)(unkInt1 & 0xffff);
                    }
                    set
                    {
                        unkInt1 = (Int32)(unkInt1 & 0xffff0000) | (value & 0xffff);
                    }
                }
                public UInt16 UnkShort
                {
                    get
                    {
                        return (UInt16)((unkInt1 & 0xffff0000) >> 16);
                    }
                    set
                    {
                        unkInt1 = (unkInt1 & 0xffff) | (Int32)((value << 16) & 0xffff0000);
                    }
                }
            }
            public class SupportType4
            {
                public SupportType4()
                {
                    internalIndex = 0;
                    byteArray = new byte[0];
                }
                public SupportType4(BinaryReader reader)
                {
                    internalIndex = reader.ReadInt32();
                    length = GetType4Size(internalIndex & 0x0000FFFF);
                    if (length - 0xC > 0x0)
                    {
                        byteArray = reader.ReadBytes(length - 0xC);
                    } 
                    else
                    {
                        byteArray = new Byte[0];
                    }
                    if ((internalIndex & 0x1000000) != 0)
                    {
                        nextType4 = new SupportType4(reader);
                        int flag = (length != 0) ? 1 : 0;
                        unkUInt = (UInt32)(((UInt64)unkUInt & 0xFEFFFFFF) | (UInt64)flag << 0x18);
                    }
                    else
                    {
                        unkUInt &= 0xFEFFFFFF;
                    }

                }
                public void Write(BinaryWriter writer)
                {
                    writer.Write(internalIndex);
                    if (null != byteArray)
                    {
                        writer.Write(byteArray);
                    }
                    if ((internalIndex & 0x1000000) != 0)
                    {
                        nextType4.Write(writer);
                    }
                }
                public Int32 GetLength()
                {
                    return 4 + ((byteArray != null) ? byteArray.Length : 0) + (((internalIndex & 0x1000000) != 0) ? nextType4.GetLength() : 0);
                }
                public UInt32 unkUInt { get; set; }
                public Int32 vTableAddress { get; set; }
                public Int32 internalIndex { get; set; }
                public Int32 length { get; set; }
                public Byte[] byteArray { get; set; }
                public SupportType4 nextType4 { get; set; }

                public UInt16 VTableIndex
                {
                    get
                    {
                        return (UInt16)(internalIndex & 0xffff);
                    }
                    set
                    {
                        internalIndex = (Int32)(internalIndex & 0xffff0000) | (value & 0xffff);
                    }
                }
                public UInt16 UnkShort
                {
                    get
                    {
                        return (UInt16)((internalIndex & 0xffff0000) >> 16);
                    }
                    set
                    {
                        internalIndex = (internalIndex & 0xffff) | (Int32)((value << 16) & 0xffff0000);
                    }
                }

                public bool isValidBits()
                {
                    if (((internalIndex & 0x1000000) != 0) && nextType4 == null)
                    {
                        return false;
                    }
                    if (((internalIndex & 0x1000000) == 0) && nextType4 != null)
                    {
                        return false;
                    }
                    if (byteArray == null && (GetType4Size(internalIndex & 0xffff) > 0))
                    {
                        return false;
                    }
                    if (byteArray != null && (GetType4Size(internalIndex & 0xffff) == 0))
                    {
                        return false;
                    }
                    if (byteArray != null && byteArray.Length != GetExpectedSize())
                    {
                        return false;
                    }
                    return true;
                }
                public Int32 GetExpectedSize()
                {
                    Int32 sz = GetType4Size(internalIndex & 0xffff);
                    if (sz - 0xC > 0)
                    {
                        return sz - 0xC;
                    }
                    else
                    {
                        return 0;
                    }
                }
                static Int32 GetType4Size(Int32 index)
                {
                    if (index < 0 || index > Type4SizeMapper.Length)
                    {
                        return 0;
                    }
                    return Type4SizeMapper[index];
                }
                static Int32[] Type4SizeMapper = {
                        0x00, 0x80, 0x0C, 0x20, 0x10, 0x0C, 0x00, 0x0C, 0x30, 0x24, 0x30, 0x48, 0x94, 0x0C, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x00, 0x00, 0x00, 0x10, 0x10, 0x00, 0x00, 0x10, 0x10, 0x20, 0x00, 0x10,
                        0x00, 0x10, 0x10, 0x0C, 0x0C, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x10, 0x00, 0x50, 0x10, 0x00, 0x30, 0x30, 0x30, 0x0C, 0x20, 0x0C, 0x00, 0x1C, 0x40, 0x14, 0x10, 0x00, 0x10, 0x60, 0x0C, 0x20, 0x0C,
                        0x30, 0x1C, 0x0C, 0x10, 0x14, 0x18, 0x00, 0x0C, 0x50, 0x00, 0x10, 0x10, 0x30, 0x0C, 0x14, 0x10, 0x50, 0x0C, 0x94, 0x94, 0x0C, 0x10, 0x28, 0x1C, 0x20, 0x10, 0x10, 0x10, 0x10, 0x10, 0x30, 0x10,
                        0xC0, 0x0C, 0x0C, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x20, 0x10, 0x00, 0x60, 0x20, 0x0C, 0x0C, 0x30, 0x1C, 0x0C, 0x0C, 0x0C, 0x14, 0x14, 0x00, 0x00, 0x14, 0x10, 0x0C, 0x10, 0x20, 0x0C, 0x10,
                        0x0C, 0x0C, 0x1C, 0x0C, 0x10, 0x0C, 0x0C, 0x0C, 0x14, 0x14, 0x14, 0x10, 0x10, 0x10, 0x10, 0x0C, 0x0C, 0x10, 0x10, 0x0C, 0x1C, 0x14, 0x18, 0x0C, 0x1C, 0x20, 0x10, 0x10, 0x10, 0x10, 0x98, 0x0C,
                        0x0C, 0x0C, 0x14, 0x10, 0x18, 0x40, 0x00, 0x10, 0x30, 0x14, 0x18, 0x14, 0x10, 0x10, 0x0C, 0x0C, 0x14, 0x30, 0x30, 0x30, 0x14, 0x0C, 0x0C, 0x10, 0x10, 0x14, 0x0C, 0x1C, 0x24, 0x20, 0x24, 0x10,
                        0x10, 0x30, 0x14, 0x0C, 0x0C, 0x30, 0x18, 0x20, 0x18, 0x18, 0x0C, 0x10, 0x2C, 0x14, 0x0C, 0x0C, 0x10, 0x10, 0x10, 0x0C, 0x0C, 0x0C, 0x10, 0x18, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x14, 0x10, 0x40, 0x10, 0x00, 0x0C, 0x14, 0x0C, 0x0C, 0x14, 0x0C, 0x3C, 0x18, 0x40, 0x2C, 0x10, 0x10, 0x10, 0x20, 0x0C, 0x10, 0x10, 0x14, 0x0C, 0x10, 0x10, 0x0C, 0x1C, 0x24, 0x80, 0x0C, 0x24,
                        0x30, 0x48, 0x40, 0x00, 0x30, 0x50, 0x10, 0x0C, 0x0C, 0x10, 0x0C, 0x00, 0x0C, 0x40, 0x10, 0x18, 0x0C, 0x10, 0x0C, 0x40, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x18, 0x54, 0x14,
                        0x10, 0x1C, 0x10, 0x10, 0x20, 0x10, 0x4C, 0x54, 0x00, 0x10, 0x00, 0x10, 0x0C, 0x10, 0x10, 0x3C, 0x10, 0x10, 0x14, 0x18, 0x00, 0x00, 0x00, 0x0C, 0x0C, 0x00, 0x24, 0x28, 0x0C, 0x10, 0x0C, 0x0C,
                        0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x10, 0x10, 0x10, 0x18, 0x0C, 0x10, 0x10, 0x0C, 0x10, 0x0C, 0x0C, 0x14, 0x0C, 0x0C, 0x14, 0x18, 0x10, 0x10, 0x00, 0x18, 0x14, 0x00, 0x10, 0x0C, 0x00, 0x00,
                        0x00, 0x24, 0x24, 0x24, 0x24, 0x10, 0x00, 0x14, 0x10, 0x0C, 0x00, 0x00, 0x0C, 0x24, 0x0C, 0x00, 0x0C, 0x24, 0x28, 0x10, 0x10, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            }
            public class LinkedScript
            {
                public LinkedScript()
                {
                    bitfield = 0;
                    scriptIndexOrSlot = -1;
                    type1 = null;
                    type2 = null;
                    nextLinked = null;
                    type2Count = 0;
                }
                public LinkedScript(BinaryReader reader)
                {
                    bitfield = reader.ReadInt16();
                    scriptIndexOrSlot = reader.ReadInt16();
                    if ((bitfield & 0x4000) != 0)
                    {
                        type1 = new SupportType1(reader);
                    }
                    if ((bitfield & 0x8000) != 0)
                    {
                        nextLinked = new LinkedScript(reader);
                    }
                }
                public void Write(BinaryWriter writer)
                {
                    writer.Write(bitfield);
                    writer.Write(scriptIndexOrSlot);
                    if ((bitfield & 0x4000) != 0)
                    {
                        type1.Write(writer);
                    }
                    if ((bitfield & 0x8000) != 0)
                    {
                        nextLinked.Write(writer);
                    }
                }
                public Int32 GetLength()
                {
                    return 4 + (((bitfield & 0x4000) != 0) ? type1.GetLength() : 0) + (((bitfield & 0x8000) != 0) ? nextLinked.GetLength() : 0);
                }
                public Int16 bitfield { get; set; }
                private Int16 type2Count {
                    get
                    {
                        return (Int16)(((UInt16)bitfield) & 0x1F);
                    }
                    set
                    {
                        bitfield = (Int16)((((UInt16)bitfield) & 0xFFE0) | (value & 0x1F));
                    }
                }
                public Int16 scriptIndexOrSlot { get; set; }
                public SupportType1 type1 { get; set; }
                public SupportType2 type2 { get; set; }
                public LinkedScript nextLinked { get; set; }
                public bool isValidBits()
                {
                    if (((bitfield & 0x4000) != 0) && type1 == null)
                    {
                        return false;
                    }
                    if (((bitfield & 0x4000) == 0) && type1 != null)
                    {
                        return false;
                    }
                    if (((bitfield & 0x8000) != 0) && nextLinked == null)
                    {
                        return false;
                    }
                    if (((bitfield & 0x8000) == 0) && nextLinked != null)
                    {
                        return false;
                    }
                    if (((bitfield & 0x1F) != 0) && type2 == null)
                    {
                        return false;
                    }
                    if (((bitfield & 0x1F) == 0) && type2 != null)
                    {
                        return false;
                    }
                    return true;
                }
                public bool CreateType1()
                {
                    if (type1 == null)
                    {
                        type1 = new SupportType1();
                        bitfield = (Int16)(bitfield | 0x4000);
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                public bool DeleteType1()
                {
                    if (type1 != null)
                    {
                        type1 = null;
                        bitfield = (Int16)(bitfield & ~0x4000);
                        return true;
                    } 
                    else
                    {
                        return false;
                    }
                }
                public bool AddType2(Int32 position)
                {
                    if (position > type2Count || position < 0)
                    {
                        return false;
                    }
                    if (type2Count == 0)
                    {
                        type2 = new SupportType2();
                    }
                    else if (position == type2Count)
                    {
                        SupportType2 ptr = type2;
                        while (ptr.nextType2 != null)
                        {
                            ptr = ptr.nextType2;
                        }
                        ptr.bitfield = (Int16)(ptr.bitfield | 0x800);
                        ptr.nextType2 = new SupportType2();
                    }
                    else
                    {
                        int pos = 0;
                        SupportType2 prevPtr = null;
                        SupportType2 ptr = type2;
                        SupportType2 newType2 = new SupportType2();
                        while (pos < position)
                        {
                            prevPtr = ptr;
                            ptr = ptr.nextType2;
                            ++pos;
                        }
                        if (prevPtr != null)
                        {
                            prevPtr.nextType2 = newType2;
                            prevPtr.nextType2.nextType2 = ptr;
                        }
                        else
                        {
                            newType2.nextType2 = type2;
                            type2 = newType2;
                        }

                        if (newType2.nextType2 != null)
                        {
                            newType2.bitfield = (Int32)(newType2.bitfield | 0x800);
                        }
                    }
                    ++type2Count;
                    return true;
                }
                public bool DeleteType2(Int32 position)
                {
                    if (position >= type2Count || position < 0)
                    {
                        return false;
                    }
                    if (position == 0)
                    {
                        type2 = type2.nextType2;
                    }
                    else
                    {
                        int pos = 0;
                        SupportType2 prevPtr = null;
                        SupportType2 ptr = type2;
                        while (pos < position)
                        {
                            prevPtr = ptr;
                            ptr = ptr.nextType2;
                            ++pos;
                        }
                        prevPtr.nextType2 = ptr.nextType2;
                        if (prevPtr.nextType2 == null)
                        {
                            prevPtr.bitfield = (Int32)(prevPtr.bitfield & ~0x800);
                        }
                    }
                    --type2Count;
                    return true;
                }
            }

            public bool DeleteLinkedScript(Int32 position)
            {
                if (position >= LinkedScriptsCount || position < 0)
                {
                    return false;
                }
                if (position == 0)
                {
                    linkedScript1 = linkedScript1.nextLinked;
                }
                else
                {
                    int pos = 0;
                    LinkedScript prevPtr = null;
                    LinkedScript ptr = linkedScript1;
                    while (pos < position)
                    {
                        prevPtr = ptr;
                        ptr = ptr.nextLinked;
                        ++pos;
                    }
                    prevPtr.nextLinked = ptr.nextLinked;
                    if (prevPtr.nextLinked == null)
                    {
                        prevPtr.bitfield = (Int16)(prevPtr.bitfield & ~0x8000);
                    }
                }
                --LinkedScriptsCount;
                return true;
            }
            public bool AddLinkedScript(Int32 position)
            {
                if (position > LinkedScriptsCount || position < 0)
                {
                    return false;
                }
                if (LinkedScriptsCount == 0)
                {
                    linkedScript1 = new LinkedScript();
                } 
                else if (position == LinkedScriptsCount)
                {
                    LinkedScript ptr = linkedScript1;
                    while (ptr.nextLinked != null)
                    {
                        ptr = ptr.nextLinked;
                    }
                    ptr.bitfield = (Int16)(ptr.bitfield | 0x8000);
                    ptr.nextLinked = new LinkedScript();
                }
                else
                {
                    int pos = 0;
                    LinkedScript prevPtr = null;
                    LinkedScript ptr = linkedScript1;
                    LinkedScript newLinked = new LinkedScript();
                    while (pos < position)
                    {
                        prevPtr = ptr;
                        ptr = ptr.nextLinked;
                        ++pos;
                    }
                    if (prevPtr != null)
                    {
                        prevPtr.nextLinked = newLinked;
                        prevPtr.nextLinked.nextLinked = ptr;
                    }
                    else
                    {
                        newLinked.nextLinked = linkedScript1;
                        linkedScript1 = newLinked;
                    }
                    
                    if (newLinked.nextLinked != null)
                    {
                        newLinked.bitfield = (Int16)(newLinked.bitfield | 0x8000);
                    }
                }
                ++LinkedScriptsCount;
                return true;
            }
        }
        public string Name
        {
            get
            {
                if (MainScript != null)
                {
                    return MainScript.name;
                }
                else
                {
                    return "Header script";
                }
            }
            set
            {
                if (MainScript != null)
                {
                    MainScript.name = value;
                }
            }
        }

        private ushort id;
        private byte mask;
        private byte flag;
        public HeaderScriptStruct HeaderScript { get; set; }
        public MainScriptStruct MainScript { get; set; }
        public byte[] script;

        public override void Save(BinaryWriter writer)
        {
            writer.Write(id);
            writer.Write(mask);
            writer.Write(flag);
            if (flag == 0)
            {
                MainScript.Write(writer);
            }
            else
            {
                HeaderScript.Write(writer);
            }
            writer.Write(script);
        }
        public override void Load(BinaryReader reader, int size)
        {
            var sk = reader.BaseStream.Position;
            id = reader.ReadUInt16();
            mask = reader.ReadByte();
            flag = reader.ReadByte();
            if (flag == 0)
            {
                MainScript = new MainScriptStruct(reader);
            }
            else
            {
                HeaderScript = new HeaderScriptStruct(reader);

            }
            script = reader.ReadBytes(size - (int)(reader.BaseStream.Position - sk));
        }
        protected override int GetSize()
        {
            if (flag != 0)
            {
                return HeaderScript.GetLength() + 4 + script.Length;
            }
            else
            {
                int a = MainScript.GetLength();
                int b = 4;
                int c = script.Length;
                return MainScript.GetLength() + 4 + script.Length;
            }

        }
    }
}
