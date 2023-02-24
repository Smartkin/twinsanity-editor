using Twinsanity;
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
        public List<BlendSkin.JointInfo[]> JointInfos { get; set; } = new List<BlendSkin.JointInfo[]>();

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
            List<string> text = new List<string>
            {
                $"ID: {Data.ID} ",
                $"Size: {Data.Size}"
            };

            for (int i = 0; i < Data.Models.Length; i++)
            {
                text.Add($"Model {i}: Material - {MainFile.GetMaterialName(Data.Models[i].MaterialID)} [ID: {Data.Models[i].MaterialID}]");
                for (int a = 0; a < Data.Models[i].SubModels.Length; a++)
                {
                    text.Add($"SubModel {a}: Vertexes {Data.Models[i].SubModels[a].VertexesAmount}, Blend Shapes Count {Data.Models[i].SubModels[a].BlendShapes.Length}");
                    text.Add($"Blend vertex: {Data.Models[i].SubModels[a].BlendShapeX}, {Data.Models[i].SubModels[a].BlendShapeY}, {Data.Models[i].SubModels[a].BlendShapeZ}");
                    for (int b = 0; b < Data.Models[i].SubModels[a].BlendShapes.Length; b++)
                    {
                        text.Add($"Shape {b}:");
                        text.Add($"\t Vertexes {Data.Models[i].SubModels[a].BlendShapes[b].VertexesAmount}");
                        for (int k = 0; k < Data.Models[i].SubModels[a].BlendShapes[b].ShapeVertecies.Length; k++)
                        {
                            text.Add($"\t Vec {k}: {Data.Models[i].SubModels[a].BlendShapes[b].ShapeVertecies[k].Position}");
                        }
                    }
                    for (var j = 0; j < Data.Models[i].SubModels[a].Vertexes.Count; j++)
                    {
                        text.Add($"Vertex {j}");
                        text.Add($"\tJoint index 1 {Data.Models[i].SubModels[a].Vertexes[j].Joint.JointIndex1}; Weight 1 {Data.Models[i].SubModels[a].Vertexes[j].Joint.Weight1}");
                        text.Add($"\tJoint index 2 {Data.Models[i].SubModels[a].Vertexes[j].Joint.JointIndex2}; Weight 2 {Data.Models[i].SubModels[a].Vertexes[j].Joint.Weight2}");
                        text.Add($"\tJoint index 3 {Data.Models[i].SubModels[a].Vertexes[j].Joint.JointIndex3}; Weight 3 {Data.Models[i].SubModels[a].Vertexes[j].Joint.Weight3}");
                    }
                }
            }

            TextPrev = text.ToArray();
        }

        public void LoadMeshData()
        {
            Vertices.Clear();
            Indices.Clear();
            JointInfos.Clear();

            var refIndex = 0U;
            var offset = 0;

            var isSpyroModel = DefaultHashes.Hash_BlendSkins[Data.ID] == "Spyro";

            foreach (var rigidModel in Data.Models)
            {
                List<Vertex> vtx = new List<Vertex>();
                List<uint> idx = new List<uint>();
                List<BlendSkin.JointInfo> jointInfos = new List<BlendSkin.JointInfo>();
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
                        if (isSpyroModel)
                        { // Spyro model specifically doesn't need UV flipping
                            vtx.Add(new Vertex(new Vector3(-model.Vertexes[j].X, model.Vertexes[j].Y, model.Vertexes[j].Z),
                                    new Vector3(0, 0, 0), new Vector2(model.Vertexes[j].U, model.Vertexes[j].V),
                                    col));
                        }
                        else
                        {
                            vtx.Add(new Vertex(new Vector3(-model.Vertexes[j].X, model.Vertexes[j].Y, model.Vertexes[j].Z),
                                    new Vector3(0, 0, 0), new Vector2(model.Vertexes[j].U, 1 - model.Vertexes[j].V),
                                    col));
                        }
                        jointInfos.Add(model.Vertexes[j].Joint);
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
                JointInfos.Add(jointInfos.ToArray());
            }

        }

        public void LoadMeshData_BlendShape(int blendShape)
        {
            Vertices.Clear();
            Indices.Clear();
            JointInfos.Clear();

            var refIndex = 0U;

            var isSpyroModel = DefaultHashes.Hash_BlendSkins[Data.ID] == "Spyro";

            foreach (var rigidModel in Data.Models)
            {
                List<Vertex> vtx = new List<Vertex>();
                List<uint> idx = new List<uint>();
                List<BlendSkin.JointInfo> jointInfos = new List<BlendSkin.JointInfo>();
                foreach (var model in rigidModel.SubModels)
                {
                    for (var j = 0; j < model.Vertexes.Count; ++j)
                    {
                        if (j < model.Vertexes.Count - 2)
                        {
                            if (model.Vertexes[j + 2].Conn)
                            {
                                idx.Add(refIndex + (uint)(j % 2));
                                idx.Add(refIndex + 1 - (uint)(j % 2));
                                idx.Add(refIndex + 2);
                            }
                            ++refIndex;
                        }
                        Color col = Color.FromArgb(model.Vertexes[j].A, model.Vertexes[j].R, model.Vertexes[j].G, model.Vertexes[j].B);
                        var blendVec = new Vector3(-model.Vertexes[j].BlendShapes[blendShape].Position.X,
                            model.Vertexes[j].BlendShapes[blendShape].Position.Y,
                            model.Vertexes[j].BlendShapes[blendShape].Position.Z);
                        blendVec *= model.BlendShapes[blendShape].ShapeVertecies[j].Position.W;
                        var x_comp = blendVec.X + -model.Vertexes[j].X;
                        var y_comp = blendVec.Y + model.Vertexes[j].Y;
                        var z_comp = blendVec.Z + model.Vertexes[j].Z;
                        if (isSpyroModel)
                        { // Spyro model specifically doesn't need UV flipping
                            vtx.Add(new Vertex(new Vector3(x_comp, y_comp, z_comp),
                                    new Vector3(0, 0, 0), new Vector2(model.Vertexes[j].U, model.Vertexes[j].V),
                                    col));
                        }
                        else
                        {
                            vtx.Add(new Vertex(new Vector3(x_comp, y_comp, z_comp),
                                    new Vector3(0, 0, 0), new Vector2(model.Vertexes[j].U, 1 - model.Vertexes[j].V),
                                    col));
                        }
                        jointInfos.Add(model.Vertexes[j].Joint);
                    }
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
                JointInfos.Add(jointInfos.ToArray());
            }

        }

        public List<Vector3[]> GetFacialPositions(int blendShape, float faceProgress)
        {
            var faceOffsets = new List<Vector3[]>();
            foreach (var rigidModel in Data.Models)
            {
                var modelFace = new List<Vector3>();
                foreach (var model in rigidModel.SubModels)
                {
                    for (var j = 0; j < model.Vertexes.Count; ++j)
                    {
                        var blendVec = new Vector3(-model.Vertexes[j].BlendShapes[blendShape].Position.X,
                            model.Vertexes[j].BlendShapes[blendShape].Position.Y,
                            model.Vertexes[j].BlendShapes[blendShape].Position.Z);
                        blendVec *= faceProgress;
                        modelFace.Add(blendVec);
                    }
                }
                faceOffsets.Add(modelFace.ToArray());
            }

            return faceOffsets;
        }

        private void Menu_OpenViewer()
        {
            MainFile.OpenMeshViewer(this);
        }
    }
}
