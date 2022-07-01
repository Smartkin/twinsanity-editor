using OpenTK;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public class BlendSkinXController : ItemController
    {
        public new BlendSkinX Data { get; set; }

        public Vertex[] Vertices { get; set; }
        public uint[] Indices { get; set; }
        public bool IsLoaded { get; private set; }

        public BlendSkinXController(MainForm topform, BlendSkinX item) : base(topform, item)
        {
            Data = item;
            AddMenu("Open mesh viewer", Menu_OpenViewer);
            AddMenu("Export as PLY", Menu_ExportPLY);
            //AddMenu("Export as COLLADA", Menu_ExportDAE);
            //AddMenu("Export as COLLADA + Textures", Menu_ExportDAE_Tex);
            //AddMenu("Export as COLLADA (Split Meshes)", Menu_ExportDAE_Split);
            AddMenu("Export as OBJ", Menu_ExportOBJ);
            IsLoaded = false;
            //LoadMeshData(); TODO preferences
        }

        protected override string GetName()
        {
            return string.Format("Blend Skinned Model X [ID {0:X8}]", Data.ID);
        }

        protected override void GenText()
        {
            List<string> Text = new List<string>();
            Text.Add(string.Format("ID: {0:X8}", Data.ID));
            Text.Add($"Size: {Data.Size}");
            Text.Add($"Blend Shape Count: {Data.BlendShapeCount}");
            Text.Add($"SubMesh Count: {Data.SubModels.Count}");
            for (int i = 0; i < Data.SubModels.Count; i++)
            {
                Text.Add($"SubMesh {i}: Material {Data.SubModels[i].MaterialID.ToString("X8")} Vertex Count {Data.SubModels[i].VData.Count} Groups {Data.SubModels[i].GroupList.Count}");

                for (int a = 0; a < Data.SubModels[i].GroupJoints.Count; a++)
                {
                    Text.Add($"Group #{a}: Vertex Count {Data.SubModels[i].GroupList[a]} Joints {Data.SubModels[i].GroupJoints[a].Count}");
                    /*
                    for (int b = 0; b < Data.SubModels[i].GroupJoints[a].Count ; b++)
                    {
                        Text.Add($"#{b}: {Data.SubModels[i].GroupJoints[a][b]}");
                    }
                    */
                }
            }


            TextPrev = Text.ToArray();
        }

        public void LoadMeshData()
        {
            List<Vertex> vtx = new List<Vertex>();
            List<uint> idx = new List<uint>();
            int off = 0;
            int gvert = 0;
            foreach (var s in Data.SubModels)
            {
                gvert = 0;
                for (int g = 0; g < s.GroupList.Count; g++)
                {
                    vtx.Add(new Vertex(new Vector3(-s.VData[gvert + 0].X, s.VData[gvert + 0].Y, s.VData[gvert + 0].Z), Color.FromArgb(s.VData[gvert + 0].R, s.VData[gvert + 0].G, s.VData[gvert + 0].B)));
                    vtx.Add(new Vertex(new Vector3(-s.VData[gvert + 1].X, s.VData[gvert + 1].Y, s.VData[gvert + 1].Z), Color.FromArgb(s.VData[gvert + 1].R, s.VData[gvert + 1].G, s.VData[gvert + 1].B)));
                    for (int i = 2; i < s.GroupList[g]; ++i)
                    {
                        vtx.Add(new Vertex(new Vector3(-s.VData[gvert + i].X, s.VData[gvert + i].Y, s.VData[gvert + i].Z), Color.FromArgb(s.VData[gvert + i].R, s.VData[gvert + i].G, s.VData[gvert + i].B)));
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
                    gvert += (int)s.GroupList[g];
                    off += (int)s.GroupList[g];
                }
            }
            Vertices = vtx.ToArray();
            Indices = idx.ToArray();
        }
        public void LoadMeshData_BlendShape(int id)
        {
            List<Vertex> vtx = new List<Vertex>();
            List<uint> idx = new List<uint>();
            int off = 0;
            int gvert = 0;
            foreach (var s in Data.SubModels)
            {
                gvert = 0;
                for (int g = 0; g < s.GroupList.Count; g++)
                {
                    vtx.Add(new Vertex(new Vector3(-s.VData[gvert + 0].BlendShapes[id].X, s.VData[gvert + 0].BlendShapes[id].Y, s.VData[gvert + 0].BlendShapes[id].Z), Color.FromArgb(s.VData[gvert + 0].R, s.VData[gvert + 0].G, s.VData[gvert + 0].B)));
                    vtx.Add(new Vertex(new Vector3(-s.VData[gvert + 1].BlendShapes[id].X, s.VData[gvert + 1].BlendShapes[id].Y, s.VData[gvert + 1].BlendShapes[id].Z), Color.FromArgb(s.VData[gvert + 1].R, s.VData[gvert + 1].G, s.VData[gvert + 1].B)));
                    for (int i = 2; i < s.GroupList[g]; ++i)
                    {
                        vtx.Add(new Vertex(new Vector3(-s.VData[gvert + i].BlendShapes[id].X, s.VData[gvert + i].BlendShapes[id].Y, s.VData[gvert + i].BlendShapes[id].Z), Color.FromArgb(s.VData[gvert + i].R, s.VData[gvert + i].G, s.VData[gvert + i].B)));
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
                    gvert += (int)s.GroupList[g];
                    off += (int)s.GroupList[g];
                }
            }

            Vertices = vtx.ToArray();
            Indices = idx.ToArray();
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
