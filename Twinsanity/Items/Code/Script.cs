using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Twinsanity
{
    public class Script : TwinsItem
    {
        public Script() {
            script = new byte[0];
        }
        public class HeaderScript
        {
            public HeaderScript(Int32 id)
            {
                unkIntPairs = 1;
                pairs = new UnkIntPairs[1];
                pairs[0] = new UnkIntPairs();
                pairs[0].mainScriptIndex = id + 1;
                pairs[0].unkInt2 = 4294922800;
            }
            public HeaderScript(BinaryReader reader)
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

        public class MainScript
        {
            public MainScript()
            {

            }
            public MainScript(BinaryReader reader)
            {
                int len = reader.ReadInt32();
                name = new string(reader.ReadChars(len));
                StatesAmount = reader.ReadInt32();
                unkInt2 = reader.ReadInt32();
                if (StatesAmount > 0)
                {
                    scriptState1 = new ScriptState(reader);
                    ScriptState ptr = scriptState1;
                    while (null != ptr)
                    {
                        if ((ptr.bitfield & 0x1F) != 0)
                        {
                            ptr.scriptStateBody = new ScriptStateBody(reader);
                        }
                        ptr = ptr.nextState;
                    }
                }
            }
            public void Write(BinaryWriter writer)
            {
                writer.Write(name.Length);
                writer.Write(name.ToCharArray());
                writer.Write(StatesAmount);
                writer.Write(unkInt2);
                if (StatesAmount > 0)
                {
                    scriptState1.Write(writer);
                    ScriptState ptr = scriptState1;
                    while (ptr != null)
                    {
                        if (null != ptr.scriptStateBody)
                        {
                            ptr.scriptStateBody.Write(writer);
                        }
                        ptr = ptr.nextState;
                    }
                }
            }
            public Int32 GetLength()
            {
                Int32 headerSize = 4 + name.Length + 4 + 4;
                Int32 linkedSize = ((scriptState1 != null)?scriptState1.GetLength():0);
                Int32 scriptStateBodySize = 0;
                ScriptState ptr = scriptState1;
                while (ptr != null)
                {
                    if (null != ptr.scriptStateBody)
                    {
                        scriptStateBodySize += ptr.scriptStateBody.GetLength();
                    }
                    ptr = ptr.nextState;
                }
                return headerSize + linkedSize + scriptStateBodySize;
            }
            public String name { get; set; }
            public Int32 StatesAmount { get; set; }
            public Int32 unkInt2 { get; set; }
            public ScriptState scriptState1 { get; set; }
            public ScriptState scriptState2 { get; set; }

            public class SupportType1
            {
                public SupportType1()
                {
                    bytes = new List<Byte>();
                    floats = new List<Single>();
                    unkByte1 = 0;
                    unkByte2 = 0;
                    unkUShort1 = 0;
                    unkInt1 = 0;
                }
                public SupportType1(BinaryReader reader)
                {
                    bytes = new List<Byte>();
                    floats = new List<Single>();
                    _unkByte1 = reader.ReadByte();
                    _unkByte2 = reader.ReadByte();
                    unkUShort1 = reader.ReadUInt16();
                    unkInt1 = reader.ReadInt32();
                    Int32 byteArrayLen = unkByte1 + unkByte2 * 4;
                    for (int i = 0; i < unkByte2; ++i)
                    {
                        floats.Add(reader.ReadSingle());
                    }
                    for (int i = 0; i < unkByte1; ++i)
                    {
                        bytes.Add(reader.ReadByte());
                    }
                }
                public void Write(BinaryWriter writer)
                {
                    writer.Write(unkByte1);
                    writer.Write(unkByte2);
                    writer.Write(unkUShort1);
                    writer.Write(unkInt1);
                    foreach (Single f in floats)
                    {
                        writer.Write(f);
                    }
                    foreach (Byte b in bytes)
                    {
                        writer.Write(b);
                    }
                }
                public Int32 GetLength()
                {
                    return 8 + floats.Count * 4 + bytes.Count;
                }
                private byte _unkByte1;
                public byte unkByte1 { 
                    get 
                    {
                        return _unkByte1;
                    }
                    set
                    {
                        _unkByte1 = value;
                        while (_unkByte1 > bytes.Count)
                        {
                            bytes.Add(0);
                        }
                        while (_unkByte1 < bytes.Count)
                        {
                            bytes.RemoveAt(bytes.Count - 1);
                        }
                    }
                }
                private byte _unkByte2;
                public byte unkByte2
                {
                    get
                    {
                        return _unkByte2;
                    }
                    set
                    {
                        _unkByte2 = value;
                        while (_unkByte2 > floats.Count)
                        {
                            floats.Add(0);
                        }
                        while (_unkByte2 < floats.Count)
                        {
                            floats.RemoveAt(floats.Count - 1);
                        }
                    }
                }
                public UInt16 unkUShort1 { get; set; }
                public Int32 unkInt1 { get; set; }
                public List<Byte> bytes { get; set; }
                public List<Single> floats { get; set; }
                public bool isValidArraySize()
                {
                    return true;
                }
            }
            public class ScriptStateBody
            {
                public ScriptStateBody()
                {
                    bitfield = 0;
                    scriptStateListIndex = 0;
                    condition = null;
                    command = null;
                    nextScriptStateBody = null;
                }
                public ScriptStateBody(BinaryReader reader)
                {
                    bitfield = reader.ReadInt32();
                    if ((bitfield & 0x400) != 0)
                    {
                        scriptStateListIndex = reader.ReadInt32();
                    }
                    if ((bitfield & 0x200) != 0)
                    {
                        condition = new ScriptCondition(reader);
                    }
                    if ((bitfield & 0xFF) != 0)
                    {
                        command = new ScriptCommand(reader);
                    }
                    if ((bitfield & 0x800) != 0)
                    {
                        nextScriptStateBody = new ScriptStateBody(reader);
                    }
                }
                public void Write(BinaryWriter writer)
                {
                    writer.Write(bitfield);
                    if ((bitfield & 0x400) != 0)
                    {
                        writer.Write(scriptStateListIndex);
                    }
                    if ((bitfield & 0x200) != 0)
                    {
                        condition.Write(writer);
                    }
                    if ((bitfield & 0xFF) != 0)
                    {
                        command.Write(writer);
                    }
                    if ((bitfield & 0x800) != 0)
                    {
                        nextScriptStateBody.Write(writer);
                    }
                }
                public Int32 GetLength()
                {
                    return 4 + (((bitfield & 0x400) != 0) ? 4 : 0)
                        + (((bitfield & 0x200) != 0) ? condition.GetLength() : 0)
                        + (((bitfield & 0xFF) != 0) ? command.GetLength() : 0)
                        + (((bitfield & 0x800) != 0) ? nextScriptStateBody.GetLength() : 0);
                }
                public Int32 bitfield { get; set; }
                public Int32 scriptStateListIndex { get; set; }
                public ScriptCondition condition { get; set; }
                public ScriptCommand command { get; set; }
                public ScriptStateBody nextScriptStateBody { get; set; }
                public bool isBitFieldValid()
                {
                    if (((bitfield & 0x200) == 0) && (condition != null))
                    {
                        return false;
                    }
                    if (((bitfield & 0x200) != 0) && (condition == null))
                    {
                        return false;
                    }
                    if (((bitfield & 0xFF) == 0) && (command != null))
                    {
                        return false;
                    }
                    if (((bitfield & 0xFF) != 0) && (command == null))
                    {
                        return false;
                    }
                    if (((bitfield & 0x800) == 0) && (nextScriptStateBody != null))
                    {
                        return false;
                    }
                    if (((bitfield & 0x800) != 0) && (nextScriptStateBody == null))
                    {
                        return false;
                    }
                    return true;
                }
                public bool IsEnabled
                {
                    get
                    {
                        return (bitfield & 0x400) != 0;
                    }
                    set
                    {
                        if (value)
                        {
                            bitfield = (Int16)(bitfield | 0x400);
                        }
                        else
                        {
                            bitfield = (Int16)(bitfield & ~0x400);
                        }
                    }
                }
                public Byte commandCount
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
                public bool CreateCondition()
                {
                    if (condition == null)
                    {
                        condition = new ScriptCondition();
                        bitfield = (Int16)(bitfield | 0x200);
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                public bool DeleteCondition()
                {
                    if (condition != null)
                    {
                        condition = null;
                        bitfield = (Int16)(bitfield & ~0x200);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                public bool AddCommand(Int32 position)
                {
                    if (position > commandCount || position < 0)
                    {
                        return false;
                    }
                    if (commandCount == 0)
                    {
                        command = new ScriptCommand();
                    }
                    else if (position == commandCount)
                    {
                        ScriptCommand ptr = command;
                        while (ptr.nextCommand != null)
                        {
                            ptr = ptr.nextCommand;
                        }
                        ptr.internalIndex = (Int16)(ptr.internalIndex | 0x1000000);
                        ptr.nextCommand = new ScriptCommand();
                    }
                    else
                    {
                        int pos = 0;
                        ScriptCommand prevPtr = null;
                        ScriptCommand ptr = command;
                        ScriptCommand newCommand = new ScriptCommand();
                        while (pos < position)
                        {
                            prevPtr = ptr;
                            ptr = ptr.nextCommand;
                            ++pos;
                        }
                        if (prevPtr != null)
                        {
                            prevPtr.nextCommand = newCommand;
                            prevPtr.nextCommand.nextCommand = ptr;
                        }
                        else
                        {
                            newCommand.nextCommand = command;
                            command = newCommand;
                        }

                        if (newCommand.nextCommand != null)
                        {
                            newCommand.internalIndex = (Int32)(newCommand.internalIndex | 0x1000000);
                        }
                    }
                    ++commandCount;
                    return true;
                }
                public bool DeleteCommand(Int32 position)
                {
                    if (position >= commandCount || position < 0)
                    {
                        return false;
                    }
                    if (position == 0)
                    {
                        command = command.nextCommand;
                    }
                    else
                    {
                        int pos = 0;
                        ScriptCommand prevPtr = null;
                        ScriptCommand ptr = command;
                        while (pos < position)
                        {
                            prevPtr = ptr;
                            ptr = ptr.nextCommand;
                            ++pos;
                        }
                        prevPtr.nextCommand = ptr.nextCommand;
                        if (prevPtr.nextCommand == null)
                        {
                            prevPtr.internalIndex = (Int32)(prevPtr.internalIndex & ~0x1000000);
                        }
                    }
                    --commandCount;
                    return true;
                }
            }
            public class ScriptCondition
            {
                public ScriptCondition()
                {
                    unkInt1 = 0;
                    X = 0.0f;
                    Y = 0.0f;
                    Z = 0.0f;
                }
                public ScriptCondition(BinaryReader reader)
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
            public class ScriptCommand
            {
                public ScriptCommand()
                {
                    internalIndex = 0;
                    arguments = new List<uint>();
                }
                public ScriptCommand(BinaryReader reader)
                {
                    internalIndex = reader.ReadInt32();
                    arguments = new List<uint>();
                    length = GetCommandSize(internalIndex & 0x0000FFFF);
                    if (length - 0xC > 0x0)
                    {
                        int sz = (length - 0xC) / 4;
                        for (int i = 0; i < sz; ++i)
                        {
                            arguments.Add(reader.ReadUInt32());
                        }
                    } 
                    if ((internalIndex & 0x1000000) != 0)
                    {
                        nextCommand = new ScriptCommand(reader);
                        int flag = (length != 0) ? 1 : 0;
                        unkUInt = (UInt32)(((UInt64)unkUInt & 0xFEFFFFFF) | (UInt64)flag << 0x18);
                    }
                    else
                    {
                        unkUInt &= 0xFEFFFFFF;
                    }
                    if (!isValidBits())
                    {
                        Console.WriteLine("Command " + (internalIndex & 0xffff)  + ": Invalid bits, check command size mapper");
                    }
                }
                public void Write(BinaryWriter writer)
                {
                    writer.Write(internalIndex);
                    if (null != arguments)
                    {
                       foreach (UInt32 arg in arguments)
                        {
                            writer.Write(arg);
                        }
                    }
                    if ((internalIndex & 0x1000000) != 0)
                    {
                        nextCommand.Write(writer);
                    }
                }
                public Int32 GetLength()
                {
                    return 4 + ((arguments != null) ? arguments.Count * 4 : 0) + (((internalIndex & 0x1000000) != 0) ? nextCommand.GetLength() : 0);
                }
                public UInt32 unkUInt { get; set; }
                public Int32 vTableAddress;
                private void UpdateArguments()
                {
                    int sz = GetExpectedSize() / 4;
                    while (sz > arguments.Count)
                    {
                        arguments.Add(0);
                    }
                    while (sz < arguments.Count)
                    {
                        arguments.RemoveAt(arguments.Count - 1);
                    }
                }
                public Int32 internalIndex { get; set; }
                public Int32 length { get; set; }
                public List<UInt32> arguments { get; set; }
                public ScriptCommand nextCommand { get; set; }

                public UInt16 VTableIndex
                {
                    get
                    {
                        return (UInt16)(internalIndex & 0xffff);
                    }
                    set
                    {
                        internalIndex = (Int32)(internalIndex & 0xffff0000) | (value & 0xffff);
                        UpdateArguments();
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
                    if (((internalIndex & 0x1000000) != 0) && nextCommand == null)
                    {
                        return false;
                    }
                    if (((internalIndex & 0x1000000) == 0) && nextCommand != null)
                    {
                        return false;
                    }
                    if (arguments == null && (GetCommandSize(internalIndex & 0xffff) > 0))
                    {
                        return false;
                    }
                    if (arguments != null && (GetCommandSize(internalIndex & 0xffff) == 0))
                    {
                        return false;
                    }
                    if (arguments != null && arguments.Count * 4 != GetExpectedSize())
                    {
                        return false;
                    }
                    return true;
                }
                public Int32 GetExpectedSize()
                {
                    Int32 sz = GetCommandSize(internalIndex & 0xffff);
                    if (sz - 0xC > 0)
                    {
                        return sz - 0xC;
                    }
                    else
                    {
                        return 0;
                    }
                }
                static Int32 GetCommandSize(Int32 index)
                {
                    if (index < 0 || index >= CommandSizeMapper.Length)
                    {
                        return 0;
                    }
                    return CommandSizeMapper[index];
                }
                static Int32[] CommandSizeMapper = {
                        0x00, 0x80, 0x0C, 0x20, 0x10, 0x0C, 0x00, 0x0C, 0x30, 0x24, 0x30, 0x48, 0x94, 0x0C, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x00, 0x00, 0x00, 0x10, 0x10, 0x00, 0x00, 0x10, 0x10, 0x20, 0x00, 0x10,
                        0x00, 0x10, 0x10, 0x0C, 0x0C, 0x00, 0x00, 0x0C, 0x14, 0x00, 0x10, 0x00, 0x50, 0x10, 0x00, 0x30, 0x30, 0x30, 0x0C, 0x20, 0x0C, 0x0C, 0x1C, 0x40, 0x14, 0x10, 0x00, 0x10, 0x60, 0x0C, 0x20, 0x0C,
                        0x30, 0x1C, 0x0C, 0x10, 0x14, 0x18, 0x00, 0x0C, 0x50, 0x00, 0x10, 0x10, 0x30, 0x0C, 0x14, 0x10, 0x50, 0x0C, 0x94, 0x94, 0x0C, 0x10, 0x28, 0x1C, 0x20, 0x10, 0x10, 0x10, 0x10, 0x10, 0x30, 0x10,
                        0xC0, 0x0C, 0x0C, 0x10, 0x10, 0x10, 0x10, 0x10, 0x10, 0x20, 0x10, 0x00, 0x60, 0x20, 0x0C, 0x0C, 0x30, 0x1C, 0x0C, 0x0C, 0x0C, 0x14, 0x14, 0x0C, 0x0C, 0x14, 0x10, 0x0C, 0x10, 0x20, 0x0C, 0x10,
                        0x0C, 0x0C, 0x1C, 0x0C, 0x10, 0x0C, 0x0C, 0x0C, 0x14, 0x14, 0x14, 0x10, 0x10, 0x10, 0x10, 0x0C, 0x0C, 0x10, 0x10, 0x0C, 0x1C, 0x14, 0x18, 0x0C, 0x1C, 0x20, 0x10, 0x10, 0x10, 0x10, 0x98, 0x0C,
                        0x0C, 0x0C, 0x14, 0x10, 0x18, 0x40, 0x10, 0x10, 0x30, 0x14, 0x18, 0x14, 0x10, 0x10, 0x0C, 0x0C, 0x14, 0x30, 0x30, 0x30, 0x14, 0x0C, 0x0C, 0x10, 0x10, 0x14, 0x0C, 0x1C, 0x24, 0x20, 0x24, 0x10,
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
                        0x14, 0x10, 0x40, 0x10, 0x10, 0x0C, 0x14, 0x0C, 0x0C, 0x14, 0x0C, 0x3C, 0x18, 0x40, 0x2C, 0x10, 0x10, 0x10, 0x20, 0x0C, 0x10, 0x10, 0x14, 0x0C, 0x10, 0x10, 0x0C, 0x1C, 0x24, 0x80, 0x0C, 0x24,
                        0x30, 0x48, 0x40, 0x00, 0x30, 0x50, 0x10, 0x0C, 0x0C, 0x10, 0x0C, 0x18, 0x0C, 0x40, 0x10, 0x18, 0x0C, 0x10, 0x0C, 0x40, 0x40, 0x40, 0x0C, 0x0C, 0x00, 0x10, 0x10, 0x10, 0x00, 0x18, 0x54, 0x14,
                        0x10, 0x1C, 0x10, 0x10, 0x20, 0x10, 0x4C, 0x54, 0x0C, 0x10, 0x10, 0x10, 0x0C, 0x10, 0x10, 0x3C, 0x10, 0x10, 0x14, 0x18, 0x18, 0x10, 0x0C, 0x0C, 0x0C, 0x0C, 0x24, 0x28, 0x0C, 0x10, 0x0C, 0x0C,
                        0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x10, 0x10, 0x10, 0x18, 0x0C, 0x10, 0x10, 0x0C, 0x10, 0x0C, 0x0C, 0x14, 0x0C, 0x0C, 0x14, 0x18, 0x10, 0x10, 0x10, 0x18, 0x14, 0x00, 0x10, 0x0C, 0x18, 0x10,
                        0x0C, 0x24, 0x24, 0x24, 0x24, 0x10, 0x00, 0x14, 0x10, 0x0C, 0x10, 0x10, 0x0C, 0x24, 0x0C, 0x28, 0x0C, 0x24, 0x28, 0x10, 0x10, 0x68, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
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
            public class ScriptState
            {
                public ScriptState()
                {
                    bitfield = 0;
                    scriptIndexOrSlot = -1;
                    type1 = null;
                    scriptStateBody = null;
                    nextState = null;
                    scriptStateBodyCount = 0;
                }
                public ScriptState(BinaryReader reader)
                {
                    bitfield = reader.ReadInt16();
                    scriptIndexOrSlot = reader.ReadInt16();
                    if ((bitfield & 0x4000) != 0)
                    {
                        type1 = new SupportType1(reader);
                    }
                    if ((bitfield & 0x8000) != 0)
                    {
                        nextState = new ScriptState(reader);
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
                        nextState.Write(writer);
                    }
                }
                public Int32 GetLength()
                {
                    return 4 + (((bitfield & 0x4000) != 0) ? type1.GetLength() : 0) + (((bitfield & 0x8000) != 0) ? nextState.GetLength() : 0);
                }
                public Int16 bitfield { get; set; }
                private Int16 scriptStateBodyCount {
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
                public bool IsSlot
                {
                    get
                    {
                        return (bitfield & 0x1000) != 0;
                    }
                    set
                    {
                        if (value)
                        {
                            bitfield = (Int16)(bitfield | 0x1000);
                        }
                        else
                        {
                            bitfield = (Int16)(bitfield & ~0x1000);
                        }
                    }
                }
                public SupportType1 type1 { get; set; }
                public ScriptStateBody scriptStateBody { get; set; }
                public ScriptState nextState { get; set; }
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
                    if (((bitfield & 0x8000) != 0) && nextState == null)
                    {
                        return false;
                    }
                    if (((bitfield & 0x8000) == 0) && nextState != null)
                    {
                        return false;
                    }
                    if (((bitfield & 0x1F) != 0) && scriptStateBody == null)
                    {
                        return false;
                    }
                    if (((bitfield & 0x1F) == 0) && scriptStateBody != null)
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
                public bool AddScriptStateBody(Int32 position)
                {
                    if (position > scriptStateBodyCount || position < 0)
                    {
                        return false;
                    }
                    if (scriptStateBodyCount == 0)
                    {
                        scriptStateBody = new ScriptStateBody();
                    }
                    else if (position == scriptStateBodyCount)
                    {
                        ScriptStateBody ptr = scriptStateBody;
                        while (ptr.nextScriptStateBody != null)
                        {
                            ptr = ptr.nextScriptStateBody;
                        }
                        ptr.bitfield = (Int16)(ptr.bitfield | 0x800);
                        ptr.nextScriptStateBody = new ScriptStateBody();
                    }
                    else
                    {
                        int pos = 0;
                        ScriptStateBody prevPtr = null;
                        ScriptStateBody ptr = scriptStateBody;
                        ScriptStateBody newType2 = new ScriptStateBody();
                        while (pos < position)
                        {
                            prevPtr = ptr;
                            ptr = ptr.nextScriptStateBody;
                            ++pos;
                        }
                        if (prevPtr != null)
                        {
                            prevPtr.nextScriptStateBody = newType2;
                            prevPtr.nextScriptStateBody.nextScriptStateBody = ptr;
                        }
                        else
                        {
                            newType2.nextScriptStateBody = scriptStateBody;
                            scriptStateBody = newType2;
                        }

                        if (newType2.nextScriptStateBody != null)
                        {
                            newType2.bitfield = (Int32)(newType2.bitfield | 0x800);
                        }
                    }
                    ++scriptStateBodyCount;
                    return true;
                }
                public bool DeleteScriptStateBody(Int32 position)
                {
                    if (position >= scriptStateBodyCount || position < 0)
                    {
                        return false;
                    }
                    if (position == 0)
                    {
                        scriptStateBody = scriptStateBody.nextScriptStateBody;
                    }
                    else
                    {
                        int pos = 0;
                        ScriptStateBody prevPtr = null;
                        ScriptStateBody ptr = scriptStateBody;
                        while (pos < position)
                        {
                            prevPtr = ptr;
                            ptr = ptr.nextScriptStateBody;
                            ++pos;
                        }
                        prevPtr.nextScriptStateBody = ptr.nextScriptStateBody;
                        if (prevPtr.nextScriptStateBody == null)
                        {
                            prevPtr.bitfield = (Int32)(prevPtr.bitfield & ~0x800);
                        }
                    }
                    --scriptStateBodyCount;
                    return true;
                }
            }

            public bool DeleteLinkedScript(Int32 position)
            {
                if (position >= StatesAmount || position < 0)
                {
                    return false;
                }
                if (position == 0)
                {
                    scriptState1 = scriptState1.nextState;
                }
                else
                {
                    int pos = 0;
                    ScriptState prevPtr = null;
                    ScriptState ptr = scriptState1;
                    while (pos < position)
                    {
                        prevPtr = ptr;
                        ptr = ptr.nextState;
                        ++pos;
                    }
                    prevPtr.nextState = ptr.nextState;
                    if (prevPtr.nextState == null)
                    {
                        prevPtr.bitfield = (Int16)(prevPtr.bitfield & ~0x8000);
                    }
                }
                --StatesAmount;
                return true;
            }
            public bool AddLinkedScript(Int32 position)
            {
                if (position > StatesAmount || position < 0)
                {
                    return false;
                }
                if (StatesAmount == 0)
                {
                    scriptState1 = new ScriptState();
                } 
                else if (position == StatesAmount)
                {
                    ScriptState ptr = scriptState1;
                    while (ptr.nextState != null)
                    {
                        ptr = ptr.nextState;
                    }
                    ptr.bitfield = (Int16)(ptr.bitfield | 0x8000);
                    ptr.nextState = new ScriptState();
                }
                else
                {
                    int pos = 0;
                    ScriptState prevPtr = null;
                    ScriptState ptr = scriptState1;
                    ScriptState newState = new ScriptState();
                    while (pos < position)
                    {
                        prevPtr = ptr;
                        ptr = ptr.nextState;
                        ++pos;
                    }
                    if (prevPtr != null)
                    {
                        prevPtr.nextState = newState;
                        prevPtr.nextState.nextState = ptr;
                    }
                    else
                    {
                        newState.nextState = scriptState1;
                        scriptState1 = newState;
                    }
                    
                    if (newState.nextState != null)
                    {
                        newState.bitfield = (Int16)(newState.bitfield | 0x8000);
                    }
                }
                ++StatesAmount;
                return true;
            }
        }
        public string Name
        {
            get
            {
                if (Main != null)
                {
                    return Main.name;
                }
                else
                {
                    return "Header script";
                }
            }
            set
            {
                if (Main != null)
                {
                    Main.name = value;
                }
            }
        }

        private ushort id;
        private byte mask;
        public byte flag;
        public HeaderScript Header { get; set; }
        public MainScript Main { get; set; }
        public byte[] script;
        public byte[] data;

        public override void Save(BinaryWriter writer)
        {
            id = (ushort)ID;
            writer.Write(id);
            writer.Write(mask);
            writer.Write(flag);
            if (data != null && data.Length > 0)
            {
                writer.Write(data);
                return;
            }
            if (flag == 0)
            {
                Main.Write(writer);
            }
            else
            {
                Header.Write(writer);
            }
            writer.Write(script);
        }
        public override void Load(BinaryReader reader, int size)
        {
            var sk = reader.BaseStream.Position;
            id = reader.ReadUInt16();
            mask = reader.ReadByte();
            flag = reader.ReadByte();
            var datapos = reader.BaseStream.Position;
            if (flag == 0)
            {
                Main = new MainScript(reader);
            }
            else
            {
                Header = new HeaderScript(reader);
            }
            try
            {
                script = reader.ReadBytes(size - (int)(reader.BaseStream.Position - sk));
                if (Main != null && script != null && script.Length > 0)
                {
                    Console.WriteLine("Script has leftovers (check command size mapper): " + Main.name);
                }
            }
            catch
            {
                if (flag == 0 && Main != null)
                {
                    Console.WriteLine("Failed to load script: " + Main.name);
                }
                script = null;
                reader.BaseStream.Position = datapos;
                data = reader.ReadBytes(size - 4);
            }
        }
        protected override int GetSize()
        {
            if (flag != 0)
            {
                return Header.GetLength() + 4 + script.Length;
            }
            else
            {
                if (data != null && data.Length > 0)
                {
                    return 4 + data.Length;
                }
                int a = Main.GetLength();
                int b = 4;
                int c = script.Length;
                return Main.GetLength() + 4 + script.Length;
            }

        }
    }
}
