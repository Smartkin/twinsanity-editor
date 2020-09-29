﻿using System;
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
        private UInt16 TimelineRelated1;
        public List<BoneSettings> BonesSettings = new List<BoneSettings>();
        public List<EndTransformations> Transformations = new List<EndTransformations>();
        public List<Timeline> Timelines = new List<Timeline>();
        public UInt32 UnkBlobSizePacked2;
        private UInt16 TimelineRelated2;
        public List<BoneSettings> BonesSettings2 = new List<BoneSettings>();
        public List<EndTransformations> Transformations2 = new List<EndTransformations>();
        public List<Timeline> Timelines2 = new List<Timeline>();

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Bitfield);
            UInt32 packed1 = (UInt32)BonesSettings.Count & 0x1F;
            packed1 |= (UInt32)((Transformations.Count * 2) << 0xA) & 0xFFE;
            packed1 |= (UInt32)(Timelines.Count << 0x16);
            UnkBlobSizePacked1 ^= packed1;
            packed1 |= UnkBlobSizePacked1;
            writer.Write(packed1);
            writer.Write(TimelineRelated1);
            foreach (var displacement in BonesSettings)
            {
                displacement.Write(writer);
            }
            foreach (var scale in Transformations)
            {
                scale.Write(writer);
            }
            foreach (var rotation in Timelines)
            {
                rotation.Write(writer);
            }
            UInt32 packed2 = (UInt32)BonesSettings.Count & 0x1F;
            packed2 |= (UInt32)((Transformations.Count * 2) << 0xA) & 0xFFE;
            packed2 |= (UInt32)(Timelines.Count << 0x16);
            UnkBlobSizePacked2 ^= packed2;
            packed2 |= UnkBlobSizePacked2;
            writer.Write(packed2);
            writer.Write(TimelineRelated2);
            foreach (var displacement in BonesSettings2)
            {
                displacement.Write(writer);
            }
            foreach (var scale in Transformations2)
            {
                scale.Write(writer);
            }
            foreach (var rotation in Timelines2)
            {
                rotation.Write(writer);
            }
        }

        public override void Load(BinaryReader reader, int size)
        {
            Bitfield = reader.ReadUInt32();
            UnkBlobSizePacked1 = reader.ReadUInt32();
            TimelineRelated1 = reader.ReadUInt16();
            var bones = (UnkBlobSizePacked1 & 0x7F);
            var transformations = (UnkBlobSizePacked1 >> 0xA & 0xFFE) / 2;
            var timelines = (UnkBlobSizePacked1 >> 0x16);
            BonesSettings.Clear();
            for (var i = 0; i < bones; ++i)
            {
                BonesSettings.Add(new BoneSettings());
                BonesSettings[i].Read(reader);
            }
            Transformations.Clear();
            for (var i = 0; i < transformations; ++i)
            {
                Transformations.Add(new EndTransformations());
                Transformations[i].Read(reader);
            }
            Timelines.Clear();
            for (var i = 0; i < timelines; ++i)
            {
                Timelines.Add(new Timeline(TimelineRelated1));
                Timelines[i].Read(reader);
            }
            UnkBlobSizePacked2 = reader.ReadUInt32();
            TimelineRelated2 = reader.ReadUInt16();
            var blobSize = (UnkBlobSizePacked2 & 0x7F) * 0x8 + (UnkBlobSizePacked2 >> 0xA & 0xFFE) + (UnkBlobSizePacked2 >> 0x16) * TimelineRelated2 * 0x2;

            bones = (UnkBlobSizePacked2 & 0x7F);
            transformations = (UnkBlobSizePacked2 >> 0xA & 0xFFE) / 2;
            timelines = (UnkBlobSizePacked2 >> 0x16);
            BonesSettings2.Clear();
            Transformations2.Clear();
            Timelines2.Clear();
            if (blobSize > 0)
            {
                for (var i = 0; i < bones; ++i)
                {
                    BonesSettings2.Add(new BoneSettings());
                    BonesSettings2[i].Read(reader);
                }
                for (var i = 0; i < transformations; ++i)
                {
                    Transformations2.Add(new EndTransformations());
                    Transformations2[i].Read(reader);
                }
                for (var i = 0; i < timelines; ++i)
                {
                    Timelines2.Add(new Timeline(TimelineRelated2));
                    Timelines2[i].Read(reader);
                }
            }
        }

        public class BoneSettings
        {
            public Byte[] Unknown;
            public BoneSettings()
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

        public class EndTransformations
        {
            public Byte[] Unknown;
            public EndTransformations()
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

        public class Timeline
        {
            public Byte[] Unknown;
            public Timeline(UInt16 rotationKeysSize)
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
            totalSize += BonesSettings.Sum(d => d.Unknown.Length) + Transformations.Sum(s => s.Unknown.Length) + Timelines.Sum(r => r.Unknown.Length);
            totalSize += 6; // blob packed 2, blob size helper 2
            totalSize += BonesSettings2.Sum(d => d.Unknown.Length) + Transformations2.Sum(s => s.Unknown.Length) + Timelines2.Sum(r => r.Unknown.Length);
            return totalSize;
        }
    }
}
