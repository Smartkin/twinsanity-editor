using OpenTK;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public class ModelXController : ItemController
    {
        public new ModelX Data { get; set; }

        public List<Vertex[]> Vertices { get; set; } = new List<Vertex[]>();
        public List<uint[]> Indices { get; set; } = new List<uint[]>();
        public bool IsLoaded { get; private set; }

        public ModelXController(MainForm topform, ModelX item) : base(topform, item)
        {
            Data = item;
            AddMenu("Open mesh viewer", Menu_OpenViewer);
            //AddMenu("Export as COLLADA", Menu_ExportDAE);
            AddMenu("Export as PLY", Menu_ExportPLY);
            //AddMenu("Export as OBJ", Menu_ExportOBJ);
            IsLoaded = false;
            //LoadMeshData(); TODO preferences
        }

        protected override string GetName()
        {
            return string.Format("Model X [ID {0:X8}]", Data.ID);
        }

        protected override void GenText()
        {
            List<string> Text = new List<string>();
            Text.Add(string.Format("ID: {0:X8}", Data.ID));
            Text.Add($"Size: {Data.Size}");
            Text.Add($"SubMesh Count: {Data.SubModels.Count}");
            for (int i = 0; i < Data.SubModels.Count; i++)
            {
                Text.Add($"SubMesh {i}: Vertex Count {Data.SubModels[i].VData.Count} Groups {Data.SubModels[i].GroupList.Count}");
                for (int g = 0; g < Data.SubModels[i].GroupList.Count; g++)
                {
                    Text.Add($"Group {g}: Vertex Count {Data.SubModels[i].GroupList[g]}");
                }
            }


            TextPrev = Text.ToArray();
        }

        public void LoadMeshData()
        {
            Vertices.Clear();
            Indices.Clear();

            foreach (var s in Data.SubModels)
            {
                List<Vertex> vtx = new List<Vertex>();
                List<uint> idx = new List<uint>();
                int off = 0;
                for (int v = 0; v < s.VData.Count; v++)
                {
                    vtx.Add(new Vertex(new Vector3(-s.VData[v].X, s.VData[v].Y, s.VData[v].Z),
                        new Vector2(s.VData[v].UV_X, 1 - s.VData[v].UV_Y),
                        Color.FromArgb(s.VData[v].R, s.VData[v].G, s.VData[v].B)));
                }
                for (int g = 0; g < s.GroupList.Count; g++)
                {
                    for (int i = 2; i < s.GroupList[g]; ++i)
                    {
                        int v1 = off + i - 2 + (i & 1);
                        int v2 = off + i - 1 - (i & 1);
                        int v3 = off + i;
                        Vector3 normal = VectorFuncs.CalcNormal(vtx[v1].Pos, vtx[v2].Pos, vtx[v3].Pos);
                        var v = vtx[v1];
                        v.Nor += normal;
                        vtx[v1] = v;
                        v = vtx[v2];
                        v.Nor += normal;
                        vtx[v2] = v;
                        v = vtx[v3];
                        v.Nor += normal;
                        vtx[v3] = v;
                        idx.Add((uint)v1);
                        idx.Add((uint)v2);
                        idx.Add((uint)v3);
                    }
                    off += (int)s.GroupList[g];
                }
                Vertices.Add(vtx.ToArray());
                Indices.Add(idx.ToArray());
            }
            IsLoaded = true;
        }

        private void Menu_OpenViewer()
        {
            MainFile.OpenMeshViewer(this);
        }

        private void Menu_ExportPLY()
        {
            SaveFileDialog sfd = new SaveFileDialog { Filter = "PLY files (*.ply)|*.ply", FileName = GetName() };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < Data.SubModels.Count; i++)
                {
                    File.WriteAllBytes(sfd.FileName.Replace(".ply", $"_{i}.ply"), Data.ToPLY(i, true));
                }
            }
        }

        private void Menu_ExportOBJ()
        {
            SaveFileDialog sfd = new SaveFileDialog { Filter = "OBJ files (*.obj)|*.obj", FileName = GetName() };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(sfd.FileName, Data.ToOBJ());
            }
        }

    }
}
