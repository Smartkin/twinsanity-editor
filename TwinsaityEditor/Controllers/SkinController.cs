using OpenTK;
using System.Collections.Generic;
using Twinsanity;

namespace TwinsaityEditor
{
    public class SkinController : ItemController
    {
        public new Skin Data { get; set; }

        public Vertex[] Vertices { get; set; }
        public uint[] Indices { get; set; }

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
                TextPrev[line + 1] = $"MaterialID {model.MaterialID}/0x{model.MaterialID:x}";
                TextPrev[line + 2] = $"Vertexes {model.Vertexes.Count}";
                line += 3;
                index++;
            }
        }

        public void LoadMeshData()
        {
            List<Vertex> vtx = new List<Vertex>();
            List<uint> idx = new List<uint>();

            var refIndex = 0U;
            var offset = 0;

            foreach (var model in Data.SubModels)
            {
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
                    vtx.Add(new Vertex(new Vector3(model.Vertexes[j].X, model.Vertexes[j].Y, model.Vertexes[j].Z),
                            new Vector3(0, 0, 0), new Vector2(model.Vertexes[j].U, model.Vertexes[j].V), System.Drawing.Color.White));
                }
                offset += model.Vertexes.Count;
                refIndex += 2;
            }

            Vertices = vtx.ToArray();
            Indices = idx.ToArray();
        }
    }
}