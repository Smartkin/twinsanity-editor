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
        public UInt32 AnimationDataPacker;
        public UInt16 TotalFrames;
        public List<JointSettings> JointsSettings = new List<JointSettings>();
        public List<Transformation> StaticTransforms = new List<Transformation>();
        public List<AnimatedTransform> AnimatedTransforms = new List<AnimatedTransform>();
        public UInt32 FacialAnimationDataPacker;
        public UInt16 FacialAnimationTotalFrames;
        public List<JointSettings> FacialJointsSettings = new List<JointSettings>();
        public List<Transformation> FacialStaticTransforms = new List<Transformation>();
        public List<AnimatedTransform> FacialAnimatedTransforms = new List<AnimatedTransform>();

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Bitfield);
            AnimationDataPacker &= ~0x7FU;
            AnimationDataPacker &= ~(0xFFEU << 0xA);
            AnimationDataPacker &= ~(0x3FFU << 0x16);
            UInt32 packed1 = (UInt32)JointsSettings.Count & 0x7F;
            packed1 |= (UInt32)(((StaticTransforms.Count * 2) & 0xFFE) << 0xA);
            packed1 |= (UInt32)(AnimatedTransforms.Count << 0x16);
            packed1 |= AnimationDataPacker;
            writer.Write(packed1);
            AnimationDataPacker = packed1;
            writer.Write(TotalFrames);
            foreach (var jointSetting in JointsSettings)
            {
                jointSetting.Write(writer);
            }
            foreach (var transformation in StaticTransforms)
            {
                transformation.Write(writer);
            }
            foreach (var timeline in AnimatedTransforms)
            {
                timeline.Write(writer);
            }
            FacialAnimationDataPacker &= ~0x7FU;
            FacialAnimationDataPacker &= ~(0xFFEU << 0xA);
            FacialAnimationDataPacker &= ~(0x3FFU << 0x16);
            UInt32 packed2 = (UInt32)FacialJointsSettings.Count & 0x7F;
            packed2 |= (UInt32)(((FacialStaticTransforms.Count * 2) & 0xFFE) << 0xA);
            packed2 |= (UInt32)(FacialAnimatedTransforms.Count << 0x16);
            packed2 |= FacialAnimationDataPacker;
            writer.Write(packed2);
            FacialAnimationDataPacker = packed2;
            writer.Write(FacialAnimationTotalFrames);
            foreach (var boneSetting in FacialJointsSettings)
            {
                boneSetting.Write(writer);
            }
            foreach (var transformation in FacialStaticTransforms)
            {
                transformation.Write(writer);
            }
            foreach (var twoPartTransform in FacialAnimatedTransforms)
            {
                twoPartTransform.Write(writer);
            }
        }

        public override void Load(BinaryReader reader, int size)
        {
            Bitfield = reader.ReadUInt32();
            AnimationDataPacker = reader.ReadUInt32();
            TotalFrames = reader.ReadUInt16();
            var joints = (AnimationDataPacker & 0x7F);
            var transformations = (AnimationDataPacker >> 0xA & 0xFFE) / 2;
            var twoPartTransforms = (AnimationDataPacker >> 0x16);
            JointsSettings.Clear();
            for (var i = 0; i < joints; ++i)
            {
                JointsSettings.Add(new JointSettings());
                JointsSettings[i].Read(reader);
            }
            StaticTransforms.Clear();
            for (var i = 0; i < transformations; ++i)
            {
                StaticTransforms.Add(new Transformation());
                StaticTransforms[i].Read(reader);
            }
            AnimatedTransforms.Clear();
            for (var i = 0; i < TotalFrames; ++i)
            {
                AnimatedTransforms.Add(new AnimatedTransform((ushort)twoPartTransforms));
                AnimatedTransforms[i].Read(reader);
            }
            FacialAnimationDataPacker = reader.ReadUInt32();
            FacialAnimationTotalFrames = reader.ReadUInt16();
            var facialAnimationDataSize = (FacialAnimationDataPacker & 0x7F) * 0x8
                + (FacialAnimationDataPacker >> 0xA & 0xFFE)
                + (FacialAnimationDataPacker >> 0x16) * FacialAnimationTotalFrames * 0x2;

            joints = (FacialAnimationDataPacker & 0x7F);
            transformations = (FacialAnimationDataPacker >> 0xA & 0xFFE) / 2;
            twoPartTransforms = (FacialAnimationDataPacker >> 0x16);
            FacialJointsSettings.Clear();
            FacialStaticTransforms.Clear();
            FacialAnimatedTransforms.Clear();
            if (facialAnimationDataSize > 0)
            {
                for (var i = 0; i < joints; ++i)
                {
                    FacialJointsSettings.Add(new JointSettings());
                    FacialJointsSettings[i].Read(reader);
                }
                for (var i = 0; i < transformations; ++i)
                {
                    FacialStaticTransforms.Add(new Transformation());
                    FacialStaticTransforms[i].Read(reader);
                }
                for (var i = 0; i < FacialAnimationTotalFrames; ++i)
                {
                    FacialAnimatedTransforms.Add(new AnimatedTransform((ushort)twoPartTransforms));
                    FacialAnimatedTransforms[i].Read(reader);
                }
            }
        }

        public uint ComponentsUsedPerFrame
        {
            get => (AnimationDataPacker >> 0x16);
        }

        public class JointSettings
        {
            public ushort Flags;
            public ushort TransformationChoice;
            public ushort TransformationIndex;
            public ushort AnimatedTransformIndex;
            public JointSettings()
            {
            }
            public void Read(BinaryReader reader)
            {
                Flags = reader.ReadUInt16();
                TransformationChoice = reader.ReadUInt16();
                TransformationIndex = reader.ReadUInt16();
                AnimatedTransformIndex = reader.ReadUInt16();
            }
            public void Write(BinaryWriter writer)
            {
                writer.Write(Flags);
                writer.Write(TransformationChoice);
                writer.Write(TransformationIndex);
                writer.Write(AnimatedTransformIndex);
            }
        }

        public class Transformation
        {
            public Int16 StoredTransformValue;

            public Single Value
            {
                get
                {
                    return StoredTransformValue / 4096f;
                }
                set
                {
                    StoredTransformValue = (Int16)(value * 4096);
                }
            }

            public Single RotValue
            {
                get
                {
                    return (StoredTransformValue * 16) / (float)(ushort.MaxValue + 1) * (float)Math.PI * 2;
                }
            }

            public Single GetRot(bool negate)
            {
                if (negate)
                {
                    return (-StoredTransformValue * 16) / (float)(ushort.MaxValue + 1) * (float)Math.PI * 2;
                }
                return RotValue;
            }

            public void Read(BinaryReader reader)
            {
                StoredTransformValue = reader.ReadInt16();
            }
            public void Write(BinaryWriter writer)
            {
                writer.Write(StoredTransformValue);
            }
        }

        public class AnimatedTransform
        {
            public List<Int16> Values;

            public Int16 GetPureOffset(int index)
            {
                return Values[index];
            }

            public float GetOffset(int index)
            {
                return Values[index] / 4096f;
            }

            public void SetOffset(int index, float value)
            {
                Values[index] = (Int16)(value * 4096);
            }

            public AnimatedTransform(UInt16 timelineLength)
            {
                Values = new List<Int16>(timelineLength);
            }
            public void Read(BinaryReader reader)
            {
                for (var i = 0; i < Values.Capacity; ++i)
                {
                    Values.Add(reader.ReadInt16());
                }
            }
            public void Write(BinaryWriter writer)
            {
                foreach (var offset in Values)
                {
                    writer.Write(offset);
                }
            }
        }

        protected override int GetSize()
        {
            var totalSize = 10; // Bitfield, blob packed, blob size helper
            totalSize += JointsSettings.Sum(d => 8) + StaticTransforms.Count * 2 + AnimatedTransforms.Sum(r => r.Values.Count * 2);
            totalSize += 6; // blob packed 2, blob size helper 2
            totalSize += FacialJointsSettings.Sum(d => 8) + FacialStaticTransforms.Count * 2 + FacialAnimatedTransforms.Sum(r => r.Values.Count * 2);
            return totalSize;
        }
    }
}
