﻿using System;
using System.Collections.Generic;
using System.IO;
using Twinsanity.VIF;

namespace Twinsanity
{
    public class BlendSkin : TwinsItem
    {
        public BlendSkinRigidModel[] Models;
        public uint BlendShapeCount;

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Models.Length);
            writer.Write(BlendShapeCount);

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

                    for (int b = 0; b < BlendShapeCount; b++)
                    {
                        writer.Write(Models[sub].SubModels[t].BlendShapes[b].Blob.Length >> 4);
                        writer.Write(Models[sub].SubModels[t].BlendShapes[b].VertexesAmount);
                        writer.Write(Models[sub].SubModels[t].BlendShapes[b].Blob);
                    }
                }

            }

        }

        public override void Load(BinaryReader reader, int size)
        {
            long start_pos = reader.BaseStream.Position;

            uint rigidCount = reader.ReadUInt32();
            BlendShapeCount = reader.ReadUInt32();
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

                    Skin.BlendShapes = new BlendShape[BlendShapeCount];
                    for (int b = 0; b < BlendShapeCount; b++)
                    {
                        BlendShape BSkin = new BlendShape();
                        int BSize = reader.ReadInt32();
                        BSkin.VertexesAmount = reader.ReadUInt32();
                        BSkin.Blob = reader.ReadBytes(BSize << 4);
                        BSkin.ShapeVertecies = new BlendShapeVertex[BSkin.VertexesAmount];
                        var dma = new DMATag
                        {
                            QWC = (ushort)BSize,
                            Extra = (0x0) | ((BSkin.VertexesAmount + 1) << 0x10) | 0x6E000000 // Unpack vectors compressed in V4_8 format
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
                                    for (var i = 0; i < BSkin.VertexesAmount; i++)
                                    {
                                        var x_comp = (int)vData[0][i + 1].GetBinaryX();
                                        var y_comp = (int)vData[0][i + 1].GetBinaryY();
                                        var z_comp = (int)vData[0][i + 1].GetBinaryZ();
                                        BSkin.ShapeVertecies[i] = new BlendShapeVertex()
                                        {
                                            Offset = new Vector4(x_comp * Skin.BlendShapeX,
                                                y_comp * Skin.BlendShapeY,
                                                z_comp * Skin.BlendShapeZ,
                                                1f)
                                        };
                                        Skin.Vertexes[i].BlendShapes.Add(BSkin.ShapeVertecies[i]);
                                    }
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
                var jointInfos = new List<JointInfo>();
                for (int j = 0; j < verts; ++j)
                {
                    var v1 = vertex_batch_3[j];

                    var unkValue = v1.GetBinaryW() & 0xFFFF;
                    connections.Add((unkValue >> 8) != 128);

                    var weightAmount = v1.GetBinaryW() & 0xFF;
                    var weight1 = 0f;
                    var weight2 = 0f;
                    var weight3 = 0f;
                    var jointIndex1 = 0u;
                    var jointIndex2 = 0u;
                    var jointIndex3 = 0u;
                    if (weightAmount > 0)
                    {
                        jointIndex1 = v1.GetBinaryX() & 0xFF;
                        jointIndex1 /= 4;
                        v1.SetBinaryX(v1.GetBinaryX() & 0xFFFFFF00);
                        weight1 = v1.X;
                    }
                    if (weightAmount > 1)
                    {
                        jointIndex2 = v1.GetBinaryY() & 0xFF;
                        jointIndex2 /= 4;
                        v1.SetBinaryY(v1.GetBinaryY() & 0xFFFFFF00);
                        weight2 = v1.Y;
                    }
                    if (weightAmount > 2)
                    {
                        jointIndex3 = v1.GetBinaryZ() & 0xFF;
                        jointIndex3 /= 4;
                        v1.SetBinaryZ(v1.GetBinaryZ() & 0xFFFFFF00);
                        weight3 = v1.Z;
                    }

                    var joint = new JointInfo()
                    {
                        Weight1 = weight1,
                        Weight2 = weight2,
                        Weight3 = weight3,
                        JointIndex1 = (int)jointIndex1,
                        JointIndex2 = (int)jointIndex2,
                        JointIndex3 = (int)jointIndex3
                    };

                    jointInfos.Add(joint);
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
                        Joint = jointInfos[j],
                        Conn = connections[j],
                        BlendShapes = new List<BlendShapeVertex>()
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
                    for (int b = 0; b < BlendShapeCount; b++)
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
            public uint VertexesAmount;
            public BlendShapeVertex[] ShapeVertecies;
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

        public struct JointInfo
        {
            public float Weight1;
            public float Weight2;
            public float Weight3;
            public int JointIndex1;
            public int JointIndex2;
            public int JointIndex3;
        }

        public struct BlendShapeVertex
        {
            public Vector4 Offset;
        }

        public struct VertexData
        {
            public float X, Y, Z;
            public float U, V;
            public byte R, G, B, A;
            public JointInfo Joint;
            public bool Conn;
            public List<BlendShapeVertex> BlendShapes;
        }
    }
}