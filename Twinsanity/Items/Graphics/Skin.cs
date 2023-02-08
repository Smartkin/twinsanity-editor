using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Twinsanity.VIF;

namespace Twinsanity
{
    public class Skin : TwinsItem
    {

        public long ItemSize { get; set; }

        public List<SubModel> SubModels { get; set; }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Data);
        }

        public override void Load(BinaryReader reader, int size)
        {
            long pre_pos = reader.BaseStream.Position;

            var subModelAmt = reader.ReadUInt32();

            SubModels = new List<SubModel>();
            for (int i = 0; i < subModelAmt; i++)
            {
                var model = new SubModel
                {
                    MaterialID = reader.ReadUInt32()
                };

                var codeSize = reader.ReadInt32();
                model.VertexAmount = reader.ReadInt32();
                var interpreter = VIFInterpreter.InterpretCode(reader.ReadBytes(codeSize));
                var data = interpreter.GetMem();
                model.Vertexes = CalculateData(data);
                SubModels.Add(model);
            }

            ItemSize = size;
            reader.BaseStream.Position = pre_pos;
            Data = reader.ReadBytes(size);
        }

        protected override int GetSize()
        {
            return (int)ItemSize;
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

                var vertex_batch_1 = data[i + VERT_DATA_INDEX];     // Position vectors
                var vertex_batch_2 = data[i + VERT_DATA_INDEX + 1]; // UV vectors
                var vertex_batch_3 = data[i + VERT_DATA_INDEX + 3]; // Transform vectors, also contain specific pointers in VU memory to some matrices
                var vertex_batch_4 = data[i + VERT_DATA_INDEX + 2]; // Color vectors

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
                        // Vert coords
                        X = vertex_batch_1[j].X,
                        Y = vertex_batch_1[j].Y,
                        Z = vertex_batch_1[j].Z,
                        // UVs
                        U = vertex_batch_2[j].X,
                        V = vertex_batch_2[j].Y,
                        // Colors
                        R = (byte)(Math.Min(vertex_batch_4[j].X + 126, 255)),
                        G = (byte)(Math.Min(vertex_batch_4[j].Y + 126, 255)),
                        B = (byte)(Math.Min(vertex_batch_4[j].Z + 126, 255)),
                        A = (byte)(Math.Min(vertex_batch_4[j].W + 126, 255)),
                        Conn = connections[j]
                    };
                    vertexes.Add(vertData);
                }

                i += (int)fields + 3;
            }
            return vertexes;
        }

        internal void FillPackage(TwinsFile source, TwinsFile destination)
        {
            var sourceMaterials = source.GetItem<TwinsSection>(11).GetItem<TwinsSection>(1);
            var destinationMaterials = destination.GetItem<TwinsSection>(11).GetItem<TwinsSection>(1);
            foreach (var model in SubModels)
            {
                if (destinationMaterials.HasItem(model.MaterialID))
                {
                    continue;
                }
                var linkedMaterial = sourceMaterials.GetItem<Material>(model.MaterialID);
                destinationMaterials.AddItem(model.MaterialID, linkedMaterial);
                linkedMaterial.FillPackage(source, destination);
            }
        }

        public struct SubModel
        {
            public uint MaterialID;
            public int VertexAmount;
            public List<VertexData> Vertexes;
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
