using System.Collections.Generic;
using System;
using System.IO;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public class MeshController : ItemController
    {
        public new Mesh Data { get; set; }

        public MeshController(Mesh item) : base(item)
        {
            Data = item;
            AddMenu("Export to PLY", Menu_ExportPLY);
        }

        protected override string GetName()
        {
            return "Mesh [ID " + Data.ID + "]";
        }

        protected override void GenText()
        {
            //TODO use array
            var ex_lines = new List<string>();
            for (int i = 0; i < Data.SubModels.Count; ++i)
            {
                var sub = Data.SubModels[i];
                ex_lines.Add("SubMesh" + i);
                ex_lines.Add("VertexCount: " + sub.VertexCount + " BlockSize: " + sub.BlockSize);
                ex_lines.Add("K: " + sub.k + " C: " + sub.c);
                ex_lines.Add("GroupCount: " + sub.Groups.Count);
                foreach (var j in sub.Groups)
                    ex_lines.Add("VertexCount: " + j.VertexCount);
            }
            TextPrev = new string[3 + ex_lines.Count];
            TextPrev[0] = "ID: " + Data.ID;
            TextPrev[1] = "Offset: " + Data.Offset + " Size: " + Data.Size;
            TextPrev[2] = "SubMesh Count: " + Data.SubModels.Count;
            Array.Copy(ex_lines.ToArray(), 0, TextPrev, 3, ex_lines.Count);
        }

        private void Menu_ExportPLY()
        {
            SaveFileDialog sfd = new SaveFileDialog { Filter = "PLY files (*.ply)|*.ply", FileName = GetName() };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(sfd.FileName, ExportPLY());
            }
        }

        public byte[] ExportPLY()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter ply = new StreamWriter(stream) { AutoFlush = true })
                {
                    int vertexcount = 0, polycount = 0;
                    for (int i = 0; i < Data.SubModels.Count; ++i)
                    {
                        for (int a = 0; a < Data.SubModels[i].Groups.Count; ++a)
                        {
                            if (Data.SubModels[i].Groups[a].VertHead > 0 && Data.SubModels[i].Groups[a].VDataHead > 0)
                            {
                                vertexcount += Data.SubModels[i].Groups[a].VertexCount;
                                for (int f = 0; f < Data.SubModels[i].Groups[a].VertexCount - 2; ++f)
                                {
                                    if (Data.SubModels[i].Groups[a].VData[f + 2].CONN != 128)
                                        ++polycount;
                                }
                            }
                        }
                    }
                    ply.WriteLine("ply");
                    ply.WriteLine("format ascii 1.0");
                    ply.WriteLine("element vertex {0}", vertexcount);
                    ply.WriteLine("property float x");
                    ply.WriteLine("property float y");
                    ply.WriteLine("property float z");
                    ply.WriteLine("property uchar red");
                    ply.WriteLine("property uchar green");
                    ply.WriteLine("property uchar blue");
                    ply.WriteLine("element face {0}", polycount);
                    ply.WriteLine("property list uchar int vertex_index");
                    ply.WriteLine("end_header");
                    foreach (var s in Data.SubModels) //vertices
                    {
                        foreach (var g in s.Groups)
                        {
                            if (g.VertHead > 0 && g.VDataHead > 0)
                                for (int i = 0; i < g.VertexCount; ++i)
                                {
                                    byte red, green, blue;
                                    red = g.VData[i].R;
                                    green = g.VData[i].G;
                                    blue = g.VData[i].B;
                                    //if (g.ShiteHead > 0)
                                    //{
                                    //    red = (byte)((g.Shit[i] & 0xFF00) >> 8);
                                    //    green = (byte)((g.Shit[i] & 0xFF0000) >> 16);
                                    //    blue = (byte)((g.Shit[i] & 0xFF000000) >> 24);
                                    //}
                                    ply.WriteLine("{0} {1} {2} {3} {4} {5}", -g.Vertex[i].X, g.Vertex[i].Y, g.Vertex[i].Z, red, green, blue);
                                }
                        }
                    }
                    vertexcount = 0;
                    foreach (var s in Data.SubModels) //polys
                    {
                        foreach (var g in s.Groups)
                        {
                            if (g.VertHead > 0 && g.VDataHead > 0)
                            {
                                for (int i = 0; i < g.VertexCount - 2; ++i)
                                {
                                    if (g.VData[i + 2].CONN != 128)
                                        ply.WriteLine("3 {0} {1} {2}", vertexcount + ((i & 0x1) == 0x1 ? i + 1 : i + 0), vertexcount + ((i & 0x1) == 0x1 ? i + 0 : i + 1), vertexcount + ((i & 0x1) == 0x1 ? i + 2 : i + 2));
                                }
                                vertexcount += g.VertexCount;
                            }
                        }
                    }
                    return stream.ToArray();
                }
            }
        }
    }
}
