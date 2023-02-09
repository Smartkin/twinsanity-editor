using OpenTK;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public class ModelController : ItemController
    {
        public new Model Data { get; set; }

        public List<Vertex[]> Vertices { get; set; } = new List<Vertex[]>();
        public List<uint[]> Indices { get; set; } = new List<uint[]>();
        public bool IsLoaded { get; private set; }

        public ModelController(MainForm topform, Model item) : base (topform, item)
        {
            Data = item;
            AddMenu("Export to PLY", Menu_ExportPLY);
            AddMenu("Open model viewer", Menu_OpenViewer);
            IsLoaded = false;
            //LoadMeshData(); TODO preferences
        }

        protected override string GetName()
        {
            return string.Format("Model [ID {0:X8}/{0}]", Data.ID);
        }

        protected override void GenText()
        {
            //TODO use array
            var ex_lines = new List<string>();
            for (int i = 0; i < Data.SubModels.Count; ++i)
            {
                var sub = Data.SubModels[i];
                ex_lines.Add($"SubModel{i}");
                ex_lines.Add($"VertexCount: {sub.VertexCount} VIF code length: {sub.VifCode.Length}");
            }
            TextPrev = new string[3 + ex_lines.Count];
            TextPrev[0] = string.Format("ID: {0:X8}", Data.ID);
            TextPrev[1] = $"Size: {Data.Size}";
            TextPrev[2] = $"SubModel Count: {Data.SubModels.Count}";
            Array.Copy(ex_lines.ToArray(), 0, TextPrev, 3, ex_lines.Count);
        }

        public void LoadMeshData()
        {
            if (IsLoaded) return;

            var refIndex = 0U;
            var offset = 0;

            foreach (var model in Data.SubModels)
            {
                List<Vertex> vtx = new List<Vertex>();
                List<uint> idx = new List<uint>();

                for (var j = 0; j < model.Vertexes.Count; ++j)
                {
                    if (j < model.Vertexes.Count - 2)
                    {
                        if (model.Vertexes[j + 2].Conn)
                        {
                            if ((/*offset +*/ j) % 2 == 0)
                            {
                                idx.Add(refIndex);
                                idx.Add(refIndex + 1);
                                idx.Add(refIndex + 2);
                            }
                            else
                            {
                                idx.Add(refIndex + 1);
                                idx.Add(refIndex);
                                idx.Add(refIndex + 2);
                            }
                        }
                        ++refIndex;
                    }
                    Color col = Color.FromArgb(model.Vertexes[j].A, model.Vertexes[j].R, model.Vertexes[j].G, model.Vertexes[j].B);
                    vtx.Add(new Vertex(new Vector3(-model.Vertexes[j].X, model.Vertexes[j].Y, model.Vertexes[j].Z),
                            new Vector3(0, 0, 0), new Vector2(model.Vertexes[j].U, model.Vertexes[j].V),
                            col));
                }
                //offset += model.Vertexes.Count;
                refIndex = 0;

                for (int i = 0; i < idx.Count; i += 3)
                {
                    var n1 = idx[i];
                    var n2 = idx[i + 1];
                    var n3 = idx[i + 2];
                    var v1 = vtx[(int)n1];
                    var v2 = vtx[(int)n2];
                    var v3 = vtx[(int)n3];
                    var normal = VectorFuncs.CalcNormal(v1.Pos, v2.Pos, v3.Pos);
                    v1.Nor += normal;
                    v2.Nor += normal;
                    v3.Nor += normal;
                    vtx[(int)n1] = v1;
                    vtx[(int)n2] = v2;
                    vtx[(int)n3] = v3;
                }

                Vertices.Add(vtx.ToArray());
                Indices.Add(idx.ToArray());
            }

            IsLoaded = true;
        }

        private void Menu_ExportPLY()
        {
            SaveFileDialog sfd = new SaveFileDialog { Filter = "PLY files (*.ply)|*.ply", FileName = GetName() };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(sfd.FileName, Data.ToPLY());
            }
        }

        private void Menu_OpenViewer()
        {
            MainFile.OpenMeshViewer(this);
        }
    }
}
