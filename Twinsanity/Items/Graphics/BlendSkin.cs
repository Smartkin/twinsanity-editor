using System;
using System.Collections.Generic;
using System.IO;
using Twinsanity.VIF;

namespace Twinsanity
{
    public class BlendSkin : TwinsItem
    {
        public BlendSkin_Type0[] Models;
        public uint Bone_Count;

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Models.Length);
            writer.Write(Bone_Count);

            for (int sub = 0; sub < Models.Length; sub++)
            {
                writer.Write(Models[sub].SubModels.Length);
                writer.Write(Models[sub].MaterialID);
                for (int t = 0; t < Models[sub].SubModels.Length; t++)
                {
                    writer.Write(Models[sub].SubModels[t].VifCode.Length);
                    writer.Write(Models[sub].SubModels[t].UnkInt);
                    writer.Write(Models[sub].SubModels[t].VifCode);
                    writer.Write(Models[sub].SubModels[t].UnkData);

                    for (int b = 0; b < Bone_Count; b++)
                    {
                        writer.Write(Models[sub].SubModels[t].Bones[b].Blob.Length >> 4);
                        writer.Write(Models[sub].SubModels[t].Bones[b].UnkInt);
                        writer.Write(Models[sub].SubModels[t].Bones[b].Blob);
                    }
                }

            }

        }

        public override void Load(BinaryReader reader, int size)
        {
            long start_pos = reader.BaseStream.Position;

            uint SubModel_Count = reader.ReadUInt32();
            Bone_Count = reader.ReadUInt32();
            Models = new BlendSkin_Type0[SubModel_Count];

            for (int sub = 0; sub < SubModel_Count; sub++)
            {
                Models[sub] = new BlendSkin_Type0();
                uint Type1_Count = reader.ReadUInt32();
                Models[sub].MaterialID = reader.ReadUInt32();
                Models[sub].SubModels = new BlendSkin_Type1[Type1_Count];
                for (int t = 0; t < Type1_Count; t++)
                {
                    BlendSkin_Type1 Skin = new BlendSkin_Type1();
                    int BlobSize = reader.ReadInt32();
                    Skin.UnkInt = reader.ReadUInt32();
                    Skin.VifCode = reader.ReadBytes(BlobSize);
                    Skin.UnkData = reader.ReadBytes(0xC);

                    var interpreter = VIFInterpreter.InterpretCode(Skin.VifCode);
                    var data = interpreter.GetMem();
                    Skin.Vertexes = CalculateData(data);

                    Skin.Bones = new BlendSkin_Type2[Bone_Count];
                    for (int b = 0; b < Bone_Count; b++)
                    {
                        BlendSkin_Type2 BSkin = new BlendSkin_Type2();
                        int BSize = reader.ReadInt32();
                        BSkin.UnkInt = reader.ReadUInt32();
                        BSkin.Blob = reader.ReadBytes(BSize << 4);
                        Skin.Bones[b] = BSkin;
                    }

                    Models[sub].SubModels[t] = Skin;
                }
            }

            //Console.WriteLine("end pos: " + (reader.BaseStream.Position - start_pos) + " target: " + size);

        }

        private List<VertexData> CalculateData(List<List<Vector4>> data)
        {
            var vertexes = new List<VertexData>();
            const int VERT_DATA_INDEX = 3;
            for (int i = 0; i < data.Count;)
            {

                var verts = (data[i][0].GetBinaryX() & 0xFF);
                var fields = (data[i + 1][0].GetBinaryX() & 0xFF) / verts;
                var scaleVec = data[i + 2][0];

                var vertex_batch_1 = data[i + VERT_DATA_INDEX];
                var vertex_batch_2 = data[i + VERT_DATA_INDEX + 1];
                var vertex_batch_3 = data[i + VERT_DATA_INDEX + 3];
                var vertex_batch_4 = data[i + VERT_DATA_INDEX + 2];

                // Vertex conversion
                for (int j = 0; j < verts; ++j)
                {
                    var v1 = new Vector4(vertex_batch_1[j]);
                    var v2 = new Vector4(vertex_batch_2[j]);
                    v1.X = (int)v1.GetBinaryX();
                    v1.Y = (int)v1.GetBinaryY();
                    v1.Z = (int)v1.GetBinaryZ();
                    v1.W = (int)v1.GetBinaryW();
                    v2.X = (int)v2.GetBinaryX();
                    v2.Y = (int)v2.GetBinaryY();
                    v2.Z = (int)v2.GetBinaryZ();
                    v2.W = (int)v2.GetBinaryW();
                    v1 = v1.Multiply(scaleVec.X);
                    v2 = v2.Multiply(scaleVec.Y);
                    vertex_batch_1[j] = v1;
                    vertex_batch_2[j] = v2;
                }
                var connections = new List<bool>();
                for (int j = 0; j < verts; ++j)
                {
                    var v1 = vertex_batch_3[j];

                    var unkValue = v1.GetBinaryW() & 0xFFFF;
                    connections.Add((unkValue >> 8) != 128);
                }

                for (int j = 0; j < verts; j++)
                {
                    var vertData = new VertexData();
                    vertData.X = vertex_batch_1[j].X;
                    vertData.Y = vertex_batch_1[j].Y;
                    vertData.Z = vertex_batch_1[j].Z;
                    vertData.U = vertex_batch_2[j].X;
                    vertData.V = vertex_batch_2[j].Y;
                    vertData.R = vertex_batch_4[j].X / 255f;
                    vertData.G = vertex_batch_4[j].X / 255f;
                    vertData.B = vertex_batch_4[j].X / 255f;
                    vertData.A = vertex_batch_4[j].X / 255f;
                    vertData.Conn = connections[j];
                    vertexes.Add(vertData);
                }

                i += (int)fields + 3;
            }
            return vertexes;
        }

        protected override int GetSize()
        {
            int size = 8;
            for (int sub = 0; sub < Models.Length; sub++)
            {
                size += 8;
                for (int t = 0; t < Models[sub].SubModels.Length; t++)
                {
                    size += 8 + 0xC + Models[sub].SubModels[t].VifCode.Length;
                    for (int b = 0; b < Bone_Count; b++)
                    {
                        size += 8 + Models[sub].SubModels[t].Bones[b].Blob.Length;
                    }
                }
            }

            return size;
        }

        public class BlendSkin_Type0
        {
            public uint MaterialID;
            public BlendSkin_Type1[] SubModels; // Type1_Count
        }

        public class BlendSkin_Type1
        {
            public uint UnkInt;
            public byte[] VifCode; //blobSize
            public byte[] UnkData; //0xC
            public List<VertexData> Vertexes;
            public BlendSkin_Type2[] Bones; //Bone_Count
        }

        public class BlendSkin_Type2
        {
            public uint UnkInt;
            public byte[] Blob; //blobSize << 4
        }

        public void FillPackage(TwinsFile source, TwinsFile destination)
        {
            var sourceMaterials = source.GetItem<TwinsSection>(11).GetItem<TwinsSection>(1);
            var destinationMaterials = destination.GetItem<TwinsSection>(11).GetItem<TwinsSection>(1);
            foreach (var model in Models)
            {
                var materialId = model.MaterialID;
                if (destinationMaterials.HasItem(materialId))
                {
                    continue;
                }
                var linkedMaterial = sourceMaterials.GetItem<Material>(materialId);
                destinationMaterials.AddItem(materialId, linkedMaterial);
                linkedMaterial.FillPackage(source, destination);
            }
        }

        public struct VertexData
        {
            public float X, Y, Z;
            public float U, V;
            public float R, G, B, A;
            public bool Conn;
        }
    }
}