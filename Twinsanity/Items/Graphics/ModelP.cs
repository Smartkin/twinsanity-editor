using System.IO;
using System.Collections.Generic;

namespace Twinsanity
{
    public class ModelP : TwinsItem
    {
        public List<SubModel> SubModels { get; set; } = new List<SubModel>();

        public override void Load(BinaryReader reader, int size)
        {
            var count = reader.ReadInt32();
            SubModels.Clear();
            for (int i = 0; i < count; i++)
            {
                SubModel sub = new SubModel();
                int VertexCount = reader.ReadInt32();
                uint DataSize = reader.ReadUInt32(); // vertex count * 0x10
                uint GroupCount = reader.ReadUInt32();
                sub.GroupList = new List<uint>();
                for (int c = 0; c < GroupCount; c++)
                {
                    sub.GroupList.Add(reader.ReadUInt32()); // list of vertex counts for each group
                }
                sub.UnkFloat1 = reader.ReadSingle();
                sub.UnkFloat2 = reader.ReadSingle();
                sub.UnkFloat3 = reader.ReadSingle();
                sub.UnkFloat4 = reader.ReadSingle();
                sub.UnkFloat5 = reader.ReadSingle();
                // load model (0x10 data per vertex)
                sub.VData = new List<VertexData>();
                for (int c = 0; c < VertexCount; c++)
                {
                    VertexData v = new VertexData();
                    v.UV_X = reader.ReadInt16();
                    v.UV_Y = reader.ReadInt16();
                    v.X = reader.ReadSingle();
                    v.Y = reader.ReadSingle();
                    v.Z = reader.ReadSingle();
                    sub.VData.Add(v);
                }
                sub.UnkInt = reader.ReadUInt32();

                SubModels.Add(sub);
            }


        }

        public override void Save(BinaryWriter writer)
        {
            writer.Write((uint)SubModels.Count);
            for (int i = 0; i < SubModels.Count; i++)
            {
                SubModel Sub = SubModels[i];
                writer.Write(Sub.VData.Count);
                writer.Write(Sub.VData.Count * 0x10);
                writer.Write(Sub.GroupList.Count);
                for (int c = 0; c < Sub.GroupList.Count; c++)
                {
                    writer.Write(Sub.GroupList[c]);
                }
                writer.Write(Sub.UnkFloat1);
                writer.Write(Sub.UnkFloat2);
                writer.Write(Sub.UnkFloat3);
                writer.Write(Sub.UnkFloat4);
                writer.Write(Sub.UnkFloat5);
                for (int c = 0; c < Sub.VData.Count; c++)
                {
                    VertexData v = Sub.VData[c];
                    writer.Write(v.UV_X);
                    writer.Write(v.UV_Y);
                    writer.Write(v.X);
                    writer.Write(v.Y);
                    writer.Write(v.Z);
                }
                writer.Write(Sub.UnkInt);
            }
        }

        protected override int GetSize()
        {
            int Size = 4;
            for (int i = 0; i < SubModels.Count; i++)
            {
                Size += 0x24;
                Size += SubModels[i].GroupList.Count * 4;
                Size += SubModels[i].VData.Count * 0x10;
            }
            return Size;
        }

        #region STRUCTURES
        public struct SubModel
        {
            public List<uint> GroupList; // Amount of Vertexes per group
            public List<VertexData> VData;
            public float UnkFloat1;
            public float UnkFloat2;
            public float UnkFloat3;
            public float UnkFloat4;
            public float UnkFloat5;
            public uint UnkInt;
        }
        public struct VertexData
        {
            public float X, Y, Z;
            public short UV_X, UV_Y;
        }
        #endregion

    }
}
