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
        private UInt16 RotationKeysRelated1;
        public List<DisplacementKeys> Displacements = new List<DisplacementKeys>();
        public List<ScaleKeys> Scales = new List<ScaleKeys>();
        public List<RotationKeys> Rotations = new List<RotationKeys>();
        public UInt32 UnkBlobSizePacked2;
        private UInt16 RotationKeysRelated2;
        public List<DisplacementKeys> Displacements2 = new List<DisplacementKeys>();
        public List<ScaleKeys> Scales2 = new List<ScaleKeys>();
        public List<RotationKeys> Rotations2 = new List<RotationKeys>();

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Bitfield);
            UInt32 packed1 = (UInt32)Displacements.Count & 0x1F;
            packed1 |= (UInt32)((Scales.Count * 2) << 0xA) & 0xFFE;
            packed1 |= (UInt32)(Rotations.Count << 0x16);
            UnkBlobSizePacked1 ^= packed1;
            packed1 |= UnkBlobSizePacked1;
            writer.Write(packed1);
            writer.Write(RotationKeysRelated1);
            foreach (var displacement in Displacements)
            {
                displacement.Write(writer);
            }
            foreach (var scale in Scales)
            {
                scale.Write(writer);
            }
            foreach (var rotation in Rotations)
            {
                rotation.Write(writer);
            }
            UInt32 packed2 = (UInt32)Displacements.Count & 0x1F;
            packed2 |= (UInt32)((Scales.Count * 2) << 0xA) & 0xFFE;
            packed2 |= (UInt32)(Rotations.Count << 0x16);
            UnkBlobSizePacked2 ^= packed2;
            packed2 |= UnkBlobSizePacked2;
            writer.Write(packed2);
            writer.Write(RotationKeysRelated2);
            foreach (var displacement in Displacements2)
            {
                displacement.Write(writer);
            }
            foreach (var scale in Scales2)
            {
                scale.Write(writer);
            }
            foreach (var rotation in Rotations2)
            {
                rotation.Write(writer);
            }
        }

        public override void Load(BinaryReader reader, int size)
        {
            Bitfield = reader.ReadUInt32();
            UnkBlobSizePacked1 = reader.ReadUInt32();
            RotationKeysRelated1 = reader.ReadUInt16();
            var displacements = (UnkBlobSizePacked1 & 0x7F);
            var scales = (UnkBlobSizePacked1 >> 0xA & 0xFFE) / 2;
            var rotations = (UnkBlobSizePacked1 >> 0x16);
            for (var i = 0; i < displacements; ++i)
            {
                Displacements.Add(new DisplacementKeys());
                Displacements[i].Read(reader);
            }
            for (var i = 0; i < scales; ++i)
            {
                Scales.Add(new ScaleKeys());
                Scales[i].Read(reader);
            }
            for (var i = 0; i < rotations; ++i)
            {
                Rotations.Add(new RotationKeys(RotationKeysRelated1));
                Rotations[i].Read(reader);
            }
            UnkBlobSizePacked2 = reader.ReadUInt32();
            RotationKeysRelated2 = reader.ReadUInt16();
            var blobSize = (UnkBlobSizePacked2 & 0x7F) * 0x8 + (UnkBlobSizePacked2 >> 0xA & 0xFFE) + (UnkBlobSizePacked2 >> 0x16) * RotationKeysRelated2 * 0x2;

            displacements = (UnkBlobSizePacked2 & 0x7F);
            scales = (UnkBlobSizePacked2 >> 0xA & 0xFFE) / 2;
            rotations = (UnkBlobSizePacked2 >> 0x16);

            if (blobSize > 0)
            {
                for (var i = 0; i < displacements; ++i)
                {
                    Displacements2.Add(new DisplacementKeys());
                    Displacements2[i].Read(reader);
                }
                for (var i = 0; i < scales; ++i)
                {
                    Scales2.Add(new ScaleKeys());
                    Scales2[i].Read(reader);
                }
                for (var i = 0; i < rotations; ++i)
                {
                    Rotations2.Add(new RotationKeys(RotationKeysRelated2));
                    Rotations2[i].Read(reader);
                }
            }
        }

        public class DisplacementKeys
        {
            public Byte[] Unknown;
            public DisplacementKeys()
            {
                Unknown = new Byte[8];
            }
            public void Read(BinaryReader reader)
            {
                Unknown = reader.ReadBytes(Unknown.Length);
            }
            public void Write(BinaryWriter writer)
            {
                writer.Write(Unknown);
            }
        }

        public class ScaleKeys
        {
            public Byte[] Unknown;
            public ScaleKeys()
            {
                Unknown = new Byte[2];
            }
            public void Read(BinaryReader reader)
            {
                Unknown = reader.ReadBytes(Unknown.Length);
            }
            public void Write(BinaryWriter writer)
            {
                writer.Write(Unknown);
            }
        }

        public class RotationKeys
        {
            public Byte[] Unknown;
            public RotationKeys(UInt16 rotationKeysSize)
            {
                Unknown = new Byte[rotationKeysSize * 2];
            }
            public void Read(BinaryReader reader)
            {
                Unknown = reader.ReadBytes(Unknown.Length);
            }
            public void Write(BinaryWriter writer)
            {
                writer.Write(Unknown);
            }
        }

        protected override int GetSize()
        {
            var totalSize = 10; // Bitfield, blob packed, blob size helper
            totalSize += Displacements.Sum(d => d.Unknown.Length) + Scales.Sum(s => s.Unknown.Length) + Rotations.Sum(r => r.Unknown.Length);
            totalSize += 6; // blob packed 2, blob size helper 2
            totalSize += Displacements2.Sum(d => d.Unknown.Length) + Scales2.Sum(s => s.Unknown.Length) + Rotations2.Sum(r => r.Unknown.Length);
            return totalSize;
        }
    }
}
