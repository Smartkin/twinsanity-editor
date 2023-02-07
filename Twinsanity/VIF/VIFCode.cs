using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twinsanity.VIF
{
    public class VIFCode
    {
        public Boolean Interrupt { get; set; }
        public VIFCodeEnum OP { get; set; }
        public Byte Amount { get; set; }
        public UInt16 Immediate { get; set; }
        public void Read(BinaryReader reader)
        {
            SetVIF(reader.ReadUInt32());
        }
        public void Write(BinaryWriter writer)
        {
            writer.Write(GetVIF());
        }

        public void SetVIF(UInt32 cmd)
        {
            Byte CMD = (Byte)((cmd & 0xFF000000) >> 24);
            Amount = (Byte)((cmd & 0x00FF0000) >> 16);
            Immediate = (UInt16)((cmd & 0x0000FFFF) >> 0);
            OP = (VIFCodeEnum)(CMD & 0b01111111);
            Interrupt = ((CMD & 0b10000000) != 0) ? true : false;
        }
        public UInt32 GetVIF()
        {
            Byte CMD = (Byte)OP;
            if (Interrupt)
            {
                CMD |= 0b10000000;
            }
            return (UInt32)CMD << 24 | (UInt32)Amount << 16 | (UInt32)Immediate << 0;
        }
        public bool isUnpack()
        {
            return (OP & VIFCodeEnum.UNPACK) == VIFCodeEnum.UNPACK;
        }
    }
    public enum VIFCodeEnum
    {
        NOP = 0b0000000,
        STCYCL = 0b0000001,
        OFFSET = 0b0000010,
        BASE = 0b0000011,
        ITOP = 0b0000100,
        STMOD = 0b0000101,
        MSKPATH3 = 0b0000110,
        MARK = 0b0000111,
        FLUSHE = 0b0010000,
        FLUSH = 0b0010001,
        FLUSHA = 0b0010011,
        MSCAL = 0b0010100,
        MSCNT = 0b0010111,
        MSCALF = 0b0010101,
        STMASK = 0b0100000,
        STROW = 0b0110000,
        STCOL = 0b0110001,
        MPG = 0b1001010,
        DIRECT = 0b1010000,
        DIRECTHL = 0b1010001,
        UNPACK = 0b1100000,
    }
}
