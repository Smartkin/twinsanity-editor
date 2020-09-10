using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twinsanity
{
    public class Animation : TwinsItem
    {
        public UInt32 Bitfield;
        public UInt32 UnkBlobSizePacked1;
        public UInt16 UnkBlobSizeHelper1;
        public byte[] unkBlob1 = new byte[0];
        public UInt32 UnkBlobSizePacked2;
        public UInt16 UnkBlobSizeHelper2;
        public byte[] unkBlob2 = new byte[0];

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Bitfield);
            writer.Write(UnkBlobSizePacked1);
            writer.Write(UnkBlobSizeHelper1);
            writer.Write(unkBlob1);
            writer.Write(UnkBlobSizePacked2);
            writer.Write(UnkBlobSizeHelper2);
            if (unkBlob2.Length > 0)
            {
                writer.Write(unkBlob2);
            }
        }

        public override void Load(BinaryReader reader, int size)
        {
            Bitfield = reader.ReadUInt32();
            UnkBlobSizePacked1 = reader.ReadUInt32();
            UnkBlobSizeHelper1 = reader.ReadUInt16();
            var blobSize = (UnkBlobSizePacked1 & 0x7F) * 0x8 + (UnkBlobSizePacked1 >> 0xA & 0xFFE) + (UnkBlobSizePacked1 >> 0x16) * UnkBlobSizeHelper1 * 0x2;
            unkBlob1 = reader.ReadBytes((int)blobSize);
            UnkBlobSizePacked2 = reader.ReadUInt32();
            UnkBlobSizeHelper2 = reader.ReadUInt16();
            blobSize = (UnkBlobSizePacked2 & 0x7F) * 0x8 + (UnkBlobSizePacked2 >> 0xA & 0xFFE) + (UnkBlobSizePacked2 >> 0x16) * UnkBlobSizeHelper2 * 0x2;
            if (blobSize > 0)
            {
                unkBlob2 = reader.ReadBytes((int)blobSize);
            }
        }

        protected override int GetSize()
        {
            var totalSize = 10; // Bitfield, blob packed, blob size helper
            totalSize += unkBlob1.Length;
            totalSize += 6; // blob packed 2, blob size helper 2
            totalSize += unkBlob2.Length;
            return totalSize;
        }
    }
}
