using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twinsanity.VIF
{
    public class DMATag
    {
        public UInt16 QWC;
        public Byte PCE;
        public Byte ID;
        public Byte IRQ;
        public UInt32 ADDR;
        public Byte SPR;
        public UInt64 Extra;
        public void Read(BinaryReader reader)
        {
            var low = reader.ReadUInt64();
            QWC = (UInt16)(low & 0xFFFF);
            PCE = (Byte)(low >> 26 & 0b11);
            ID = (Byte)(low >> 28 & 0b111);
            IRQ = (Byte)(low >> 31 & 0b1);
            ADDR = (UInt32)(low >> 32 & 0x7FFFFFFF);
            SPR = (Byte)(low >> 63 & 0b1);
            Extra = reader.ReadUInt64();
        }

        public void Write(BinaryWriter writer)
        {
            UInt64 low = QWC;
            low |= (UInt64)PCE << 26;
            low |= (UInt64)ID << 28;
            low |= (UInt64)IRQ << 31;
            low |= (UInt64)ADDR << 32;
            low |= (UInt64)SPR << 63;
            writer.Write(low);
            writer.Write(Extra);
        }
    }
}
