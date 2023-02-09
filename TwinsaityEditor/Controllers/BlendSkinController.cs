﻿using Twinsanity;
using System;
using System.IO;
using System.Collections.Generic;
using OpenTK;
using System.Drawing;

namespace TwinsaityEditor
{
    public class BlendSkinController : ItemController
    {
        public new BlendSkin Data { get; set; }

        public List<Vertex[]> Vertices { get; set; } = new List<Vertex[]>();
        public List<uint[]> Indices { get; set; } = new List<uint[]>();

        public bool IsLoaded { get; private set; }

        public BlendSkinController(MainForm topform, BlendSkin item) : base(topform, item)
        {
            Data = item;
            //AddMenu("Export mesh to PLY", Menu_ExportPLY);
            AddMenu("Open mesh viewer", Menu_OpenViewer);
        }

        protected override string GetName()
        {
            return string.Format("Blend Skin [ID {0:X8}]", Data.ID);
        }

        protected override void GenText()
        {
            List<string> text = new List<string>();
            text.Add($"ID: {Data.ID} ");
            text.Add($"Size: {Data.Size}");

            for (int i = 0; i < Data.Models.Length; i++)
            {
                text.Add($"Model {i}: Material - {MainFile.GetMaterialName(Data.Models[i].MaterialID)} [ID: {Data.Models[i].MaterialID}]");
                for (int a = 0; a < Data.Models[i].SubModels.Length; a++)
                {
                    text.Add($"SubModel {a}: Vertexes {Data.Models[i].SubModels[a].VertexesAmount}, Bone Count {Data.Models[i].SubModels[a].Bones.Length}");
                    for (int b = 0; b < Data.Models[i].SubModels[a].Bones.Length; b++)
                    {
                        //text.Add($"Bone {b}: Int {Data.Models[i].SubModels[a].Bones[b].UnkInt}");
                    }
                }
            }

            TextPrev = text.ToArray();
        }

        public void LoadMeshData()
        {
            if (IsLoaded) return;

            var refIndex = 0U;
            var offset = 0;

            foreach (var rigidModel in Data.Models)
            {
                List<Vertex> vtx = new List<Vertex>();
                List<uint> idx = new List<uint>();
                foreach (var model in rigidModel.SubModels)
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
                        Color col = Color.FromArgb(model.Vertexes[j].A, model.Vertexes[j].R, model.Vertexes[j].G, model.Vertexes[j].B);
                        vtx.Add(new Vertex(new Vector3(-model.Vertexes[j].X, model.Vertexes[j].Y, model.Vertexes[j].Z),
                                new Vector3(0, 0, 0), new Vector2(model.Vertexes[j].U, 1 - model.Vertexes[j].V),
                                col));
                    }
                    //offset += model.Vertexes.Count;
                    refIndex += 2;
                }
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

        private void Menu_OpenViewer()
        {
            MainFile.OpenMeshViewer(this);
        }
    }
}
