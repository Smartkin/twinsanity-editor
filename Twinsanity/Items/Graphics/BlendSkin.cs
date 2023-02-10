using System;
using System.Collections.Generic;
using System.IO;
using Twinsanity.VIF;

namespace Twinsanity
{
    public class BlendSkin : TwinsItem
    {
        public BlendSkinRigidModel[] Models;
        public uint BlendShapesCount;

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Models.Length);
            writer.Write(BlendShapesCount);

            for (int sub = 0; sub < Models.Length; sub++)
            {
                writer.Write(Models[sub].SubModels.Length);
                writer.Write(Models[sub].MaterialID);
                for (int t = 0; t < Models[sub].SubModels.Length; t++)
                {
                    writer.Write(Models[sub].SubModels[t].VifCode.Length);
                    writer.Write(Models[sub].SubModels[t].VertexesAmount);
                    writer.Write(Models[sub].SubModels[t].VifCode);
                    writer.Write(Models[sub].SubModels[t].BlendShapeX);
                    writer.Write(Models[sub].SubModels[t].BlendShapeY);
                    writer.Write(Models[sub].SubModels[t].BlendShapeZ);

                    for (int b = 0; b < BlendShapesCount; b++)
                    {
                        writer.Write(Models[sub].SubModels[t].BlendShapes[b].Blob.Length >> 4);
                        writer.Write(Models[sub].SubModels[t].BlendShapes[b].UnkInt);
                        writer.Write(Models[sub].SubModels[t].BlendShapes[b].Blob);
                    }
                }

            }

        }

        public override void Load(BinaryReader reader, int size)
        {
            long start_pos = reader.BaseStream.Position;

            uint rigidCount = reader.ReadUInt32();
            BlendShapesCount = reader.ReadUInt32();
            Models = new BlendSkinRigidModel[rigidCount];

            for (int rigidIndex = 0; rigidIndex < rigidCount; rigidIndex++)
            {
                Models[rigidIndex] = new BlendSkinRigidModel();
                uint subModelCount = reader.ReadUInt32();
                Models[rigidIndex].MaterialID = reader.ReadUInt32();
                Models[rigidIndex].SubModels = new BlendSkinSubModel[subModelCount];
                for (int t = 0; t < subModelCount; t++)
                {
                    BlendSkinSubModel Skin = new BlendSkinSubModel();
                    int BlobSize = reader.ReadInt32();
                    Skin.VertexesAmount = reader.ReadUInt32();
                    Skin.VifCode = reader.ReadBytes(BlobSize);
                    Skin.BlendShapeX = reader.ReadSingle();
                    Skin.BlendShapeY = reader.ReadSingle();
                    Skin.BlendShapeZ = reader.ReadSingle();

                    var interpreter = VIFInterpreter.InterpretCode(Skin.VifCode);
                    var data = interpreter.GetMem();
                    Skin.Vertexes = CalculateData(data);

                    Skin.BlendShapes = new BlendShape[BlendShapesCount];
                    for (int b = 0; b < BlendShapesCount; b++)
                    {
                        BlendShape BSkin = new BlendShape();
                        int BSize = reader.ReadInt32();
                        BSkin.UnkInt = reader.ReadUInt32();
                        BSkin.Blob = reader.ReadBytes(BSize << 4);

                        var dma = new DMATag
                        {
                            QWC = (ushort)BSize,
                            Extra = (ulong)(0x6E000000 | (BSize << 0x10) | 0x12B)
                        };

                        using (var mem = new MemoryStream())
                        {
                            using (var writer = new BinaryWriter(mem))
                            {
                                dma.Write(writer);
                                writer.Write(BSkin.Blob);

                                mem.Position = 0;
                                using (var memReader = new BinaryReader(mem))
                                {
                                    var interp = VIFInterpreter.InterpretCode(memReader);
                                    var vData = interp.GetMem();
                                    
                                }
                            }
                        }
                  
                        Skin.BlendShapes[b] = BSkin;
                    }

                    Models[rigidIndex].SubModels[t] = Skin;
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
                    var vertData = new VertexData
                    {
                        X = vertex_batch_1[j].X,
                        Y = vertex_batch_1[j].Y,
                        Z = vertex_batch_1[j].Z,
                        U = vertex_batch_2[j].X,
                        V = vertex_batch_2[j].Y,
                        R = (byte)(Math.Min((int)(vertex_batch_4[j].GetBinaryX() & 0xFF) + 127, 255)),
                        G = (byte)(Math.Min((int)(vertex_batch_4[j].GetBinaryY() & 0xFF) + 127, 255)),
                        B = (byte)(Math.Min((int)(vertex_batch_4[j].GetBinaryZ() & 0xFF) + 127, 255)),
                        A = (byte)(Math.Min((int)(vertex_batch_4[j].GetBinaryW() & 0xFF) + 127, 255)),
                        Conn = connections[j]
                    };
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
                    for (int b = 0; b < BlendShapesCount; b++)
                    {
                        size += 8 + Models[sub].SubModels[t].BlendShapes[b].Blob.Length;
                    }
                }
            }

            return size;
        }

        public class BlendSkinRigidModel
        {
            public uint MaterialID;
            public BlendSkinSubModel[] SubModels; // Type1_Count
        }

        public class BlendSkinSubModel
        {
            public uint VertexesAmount;
            public byte[] VifCode; //blobSize
            public float BlendShapeX, BlendShapeY, BlendShapeZ; // Used for animating blend shapes
            public List<VertexData> Vertexes;
            public BlendShape[] BlendShapes; //Blend shapes
        }

        public class BlendShape
        {
            public uint UnkInt;
            public byte[] Blob; //Vector 4 stored in V4_8 format
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
            public byte R, G, B, A;
            public bool Conn;
        }
    }
}