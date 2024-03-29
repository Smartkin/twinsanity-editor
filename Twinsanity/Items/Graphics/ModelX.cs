﻿using System.IO;
using System.Collections.Generic;

namespace Twinsanity
{
    public class ModelX : TwinsItem
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
                uint DataSize = reader.ReadUInt32(); // vertex count * 0x1C
                uint GroupCount = reader.ReadUInt32();
                sub.GroupList = new List<uint>();
                for (int c = 0; c < GroupCount; c++)
                {
                    sub.GroupList.Add(reader.ReadUInt32()); // list of vertex counts for each group
                }
                // load model (0x1C data per vertex)
                sub.VData = new List<VertexData>();
                for (int c = 0; c < VertexCount; c++)
                {
                    VertexData v = new VertexData();
                    v.X = reader.ReadSingle();
                    v.Y = reader.ReadSingle();
                    v.Z = reader.ReadSingle();
                    v.PackedNormals = reader.ReadUInt32();
                    v.R = reader.ReadByte();
                    v.G = reader.ReadByte();
                    v.B = reader.ReadByte();
                    v.A = reader.ReadByte();
                    v.UV_X = reader.ReadSingle();
                    v.UV_Y = reader.ReadSingle();
                    sub.VData.Add(v);
                }
                uint Zero = reader.ReadUInt32(); // confirmed always zero

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
                writer.Write(Sub.VData.Count * 0x1C);
                writer.Write(Sub.GroupList.Count);
                for (int c = 0; c < Sub.GroupList.Count; c++)
                {
                    writer.Write(Sub.GroupList[c]);
                }
                for (int c = 0; c < Sub.VData.Count; c++)
                {
                    VertexData v = Sub.VData[c];
                    writer.Write(v.X);
                    writer.Write(v.Y);
                    writer.Write(v.Z);
                    writer.Write(v.PackedNormals);
                    writer.Write(v.R);
                    writer.Write(v.G);
                    writer.Write(v.B);
                    writer.Write(v.A);
                    writer.Write(v.UV_X);
                    writer.Write(v.UV_Y);
                }
                writer.Write((uint)0);
            }
        }

        protected override int GetSize()
        {
            int Size = 4;
            for (int i = 0; i < SubModels.Count; i++)
            {
                Size += 16;
                Size += SubModels[i].GroupList.Count * 4;
                Size += SubModels[i].VData.Count * 0x1C;
            }
            return Size;
        }

        #region STRUCTURES
        public struct SubModel
        {
            public List<uint> GroupList; // Amount of Vertexes per group
            public List<VertexData> VData;
        }
        public struct VertexData
        {
            public float X, Y, Z;
            public uint PackedNormals; // Packed normals? (into 10-bit x3 + 2?)
            public byte R, G, B, A;
            public float UV_X, UV_Y;
        }
        #endregion

        #region Export
        public byte[] ToPLY(int ModelID, bool blender = false)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter ply = new StreamWriter(stream) { AutoFlush = true })
                {
                    int vertexcount = 0, polycount = 0;
                    vertexcount += SubModels[ModelID].VData.Count;
                    for (int g = 0; g < SubModels[ModelID].GroupList.Count; g++)
                    {
                        for (int f = 0; f < SubModels[ModelID].GroupList[g] - 2; ++f)
                        {
                            ++polycount;
                        }
                    }
                    ply.WriteLine("ply");
                    ply.WriteLine("format ascii 1.0");
                    ply.WriteLine("element vertex {0}", vertexcount);
                    ply.WriteLine("property float x");
                    ply.WriteLine("property float y");
                    ply.WriteLine("property float z");
                    ply.WriteLine("property float s");
                    ply.WriteLine("property float t");
                    ply.WriteLine("property uchar red");
                    ply.WriteLine("property uchar green");
                    ply.WriteLine("property uchar blue");
                    ply.WriteLine("element face {0}", polycount);
                    if (!blender)
                    {
                        ply.WriteLine("property list uchar int vertex_index");
                    }
                    else
                    {
                        ply.WriteLine("property list uchar int vertex_indices");
                    }
                    ply.WriteLine("end_header");
                    for (int i = 0; i < SubModels[ModelID].VData.Count; i++)
                    {
                        byte red, green, blue;
                        red = SubModels[ModelID].VData[i].R;
                        green = SubModels[ModelID].VData[i].G;
                        blue = SubModels[ModelID].VData[i].B;
                        string Line = string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", -SubModels[ModelID].VData[i].X, SubModels[ModelID].VData[i].Y, SubModels[ModelID].VData[i].Z, SubModels[ModelID].VData[i].UV_X, SubModels[ModelID].VData[i].UV_Y, red, green, blue);
                        Line = Line.Replace(',', '.');
                        ply.WriteLine(Line);
                    }
                    vertexcount = 0;
                    for (int g = 0; g < SubModels[ModelID].GroupList.Count; g++)
                    {
                        for (int i = 0; i < SubModels[ModelID].GroupList[g] - 2; ++i)
                        {
                            ply.WriteLine("3 {0} {1} {2}", vertexcount + ((i & 0x1) == 0x1 ? i + 1 : i + 0), vertexcount + ((i & 0x1) == 0x1 ? i + 0 : i + 1), vertexcount + ((i & 0x1) == 0x1 ? i + 2 : i + 2));
                        }
                        vertexcount += (int)SubModels[ModelID].GroupList[g];
                    }
                    return stream.ToArray();
                }
            }
        }

        public byte[] ToPLY(bool blender = false)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter ply = new StreamWriter(stream) { AutoFlush = true })
                {
                    int vertexcount = 0, polycount = 0;
                    for (int i = 0; i < SubModels.Count; i++)
                    {
                        vertexcount += SubModels[i].VData.Count;
                        for (int g = 0; g < SubModels[i].GroupList.Count; g++)
                        {
                            for (int f = 0; f < SubModels[i].GroupList[g] - 2; ++f)
                            {
                                ++polycount;
                            }
                        }
                    }
                    ply.WriteLine("ply");
                    ply.WriteLine("format ascii 1.0");
                    ply.WriteLine("element vertex {0}", vertexcount);
                    ply.WriteLine("property float x");
                    ply.WriteLine("property float y");
                    ply.WriteLine("property float z");
                    ply.WriteLine("property float s");
                    ply.WriteLine("property float t");
                    ply.WriteLine("property uchar red");
                    ply.WriteLine("property uchar green");
                    ply.WriteLine("property uchar blue");
                    ply.WriteLine("element face {0}", polycount);
                    if (!blender)
                    {
                        ply.WriteLine("property list uchar int vertex_index");
                    }
                    else
                    {
                        ply.WriteLine("property list uchar int vertex_indices");
                    }
                    ply.WriteLine("end_header");
                    foreach (var s in SubModels)
                    {
                        for (int i = 0; i < s.VData.Count; i++)
                        {
                            byte red, green, blue;
                            red = s.VData[i].R;
                            green = s.VData[i].G;
                            blue = s.VData[i].B;
                            string Line = string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", -s.VData[i].X, s.VData[i].Y, s.VData[i].Z, s.VData[i].UV_X, s.VData[i].UV_Y, red, green, blue);
                            Line = Line.Replace(',', '.');
                            ply.WriteLine(Line);
                        }
                    }
                    vertexcount = 0;
                    foreach (var s in SubModels) //polys
                    {
                        for (int g = 0; g < s.GroupList.Count; g++)
                        {
                            for (int i = 0; i < s.GroupList[g] - 2; ++i)
                            {
                                ply.WriteLine("3 {0} {1} {2}", vertexcount + ((i & 0x1) == 0x1 ? i + 1 : i + 0), vertexcount + ((i & 0x1) == 0x1 ? i + 0 : i + 1), vertexcount + ((i & 0x1) == 0x1 ? i + 2 : i + 2));
                            }
                            vertexcount += (int)s.GroupList[g];
                        }
                    }
                    return stream.ToArray();
                }
            }
        }

        public byte[] ToOBJ()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter obj = new StreamWriter(stream))
                {
                    obj.WriteLine("# Vertices");
                    foreach (var s in SubModels)
                    {
                        for (int i = 0; i < s.VData.Count; i++)
                        {
                            byte red, green, blue;
                            red = s.VData[i].R;
                            green = s.VData[i].G;
                            blue = s.VData[i].B;
                            string Line = string.Format("v {0} {1} {2} {3} {4} {5}", -s.VData[i].X, s.VData[i].Y, s.VData[i].Z, red / 255f, green / 255f, blue / 255f);
                            Line = Line.Replace(',', '.');
                            obj.WriteLine(Line);
                        }
                    }
                    foreach (var s in SubModels)
                    {
                        for (int i = 0; i < s.VData.Count; i++)
                        {
                            string Line = string.Format("vt {0} {1}", s.VData[i].UV_X, s.VData[i].UV_Y);
                            Line = Line.Replace(',', '.');
                            obj.WriteLine(Line);
                        }
                    }
                    obj.WriteLine();

                    int vertexcount = 1;
                    for (int a = 0; a < SubModels.Count; a++)
                    {
                        obj.WriteLine($"o Mesh_{a + 1}");

                        for (int g = 0; g < SubModels[a].GroupList.Count; g++)
                        {
                            for (int i = 0; i < SubModels[a].GroupList[g] - 2; ++i)
                            {
                                obj.WriteLine("f {0} {1} {2}", vertexcount + ((i & 0x1) == 0x1 ? i + 1 : i + 0), vertexcount + ((i & 0x1) == 0x1 ? i + 0 : i + 1), vertexcount + ((i & 0x1) == 0x1 ? i + 2 : i + 2));
                            }
                            vertexcount += (int)SubModels[a].GroupList[g];
                        }
                    }

                }
                return stream.ToArray();
            }
        }
        #endregion

    }
}
