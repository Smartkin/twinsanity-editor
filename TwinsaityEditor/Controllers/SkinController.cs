using OpenTK;
using System.Collections.Generic;
using System.Drawing;
using Twinsanity;

namespace TwinsaityEditor
{
    public class SkinController : ItemController
    {
        public new Skin Data { get; set; }

        public List<Vertex[]> Vertices { get; set; } = new List<Vertex[]>();
        public List<uint[]> Indices { get; set; } = new List<uint[]>();

        public bool IsLoaded { get; private set; }

        public SkinController(MainForm topform, Skin item) : base(topform, item)
        {
            Data = item;
            AddMenu("Open mesh viewer", Menu_OpenViewer);
        }

        protected override string GetName()
        {
            return $"Skin [ID {Data.ID:X}/{Data.ID}]";
        }

        private void Menu_OpenViewer()
        {
            MainFile.OpenMeshViewer(this);
        }

        protected override void GenText()
        {
            TextPrev = new string[2 + (Data.SubModels.Count * 3)];
            TextPrev[0] = $"ID: {Data.ID}";
            TextPrev[1] = $"SubModels {Data.SubModels.Count}";
            int line = 2;
            var index = 0;
            foreach (var model in Data.SubModels)
            {
                TextPrev[line + 0] = $"SubModel {index}";
                TextPrev[line + 1] = $"MaterialID {model.MaterialID}/0x{model.MaterialID:X}";
                TextPrev[line + 2] = $"Vertexes {model.Vertexes.Count}";
                line += 3;
                index++;
            }
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
                            if ((offset + j) % 2 == 0)
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
                            new Vector3(0, 0, 0), new Vector2(model.Vertexes[j].U, 1 - model.Vertexes[j].V),
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
    }
}