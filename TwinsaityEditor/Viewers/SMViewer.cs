using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public partial class SMViewer : ThreeDViewer
    {
        private FileController file;
        private ChunkLinks links;
        private SceneryData scenery;
        private Skydome skydome;
        private DynamicSceneryData dynamicScenery;
        private List<VertexBufferData> sceneryObjects = new List<VertexBufferData>();

        private bool showScenery;


        public SMViewer(FileController file, Form pform)
        {
            this.file = file;
            showScenery = true;

            MakeCurrent();

            // Chunk links
            if (file.Data.ContainsItem(5))
            {
                links = file.Data.GetItem<ChunkLinks>(5);
            }
            // Scenery data
            if (file.Data.ContainsItem(0))
            {
                scenery = file.Data.GetItem<SceneryData>(0);
                LoadScenery(new Matrix4());
            }
            zFar = 2000F;
        }

        //protected override void RenderHUD()
        //{
        //    return;
        //}

        protected override void RenderObjects()
        {
            //put all object rendering code here
            GL.PushMatrix();

            if (showScenery)
            {
                GL.Enable(EnableCap.Lighting);
                for (int i = 0; i < sceneryObjects.Count; i++)
                {
                    sceneryObjects[i].DrawAllElements(PrimitiveType.Triangles, BufferPointerFlags.Normal);
                }
                GL.Disable(EnableCap.Lighting);
            }

            if (links != null)
            {
                GL.LineWidth(2);
                GL.DepthMask(false);
                foreach (var l in links.Links)
                {
                    Color cur_color = colors[(links.Links.IndexOf(l) + 2) % colors.Length];
                    GL.PushMatrix();
                    GL.Scale(-1, 1, 1);
                    if (l.HasWall)
                    {
                        GL.Color4(Color.FromArgb(95, cur_color));
                        GL.Begin(PrimitiveType.Quads);
                        GL.Vertex4(l.LoadWall[0].ToArray());
                        GL.Vertex4(l.LoadWall[1].ToArray());
                        GL.Vertex4(l.LoadWall[2].ToArray());
                        GL.Vertex4(l.LoadWall[3].ToArray());
                        GL.End();
                        GL.Color4(cur_color);
                        GL.Begin(PrimitiveType.LineLoop);
                        GL.Vertex4(l.LoadWall[0].ToArray());
                        GL.Vertex4(l.LoadWall[1].ToArray());
                        GL.Vertex4(l.LoadWall[2].ToArray());
                        GL.Vertex4(l.LoadWall[3].ToArray());
                        GL.End();
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex4(l.LoadWall[0].ToArray());
                        GL.Vertex4(l.LoadWall[2].ToArray());
                        GL.Vertex4(l.LoadWall[1].ToArray());
                        GL.Vertex4(l.LoadWall[3].ToArray());
                        GL.End();
                        Matrix3 rot_mat = Matrix3.Identity;
                        rot_mat *= Matrix3.CreateRotationX(-rot.Y / 180 * MathHelper.Pi);
                        rot_mat *= Matrix3.CreateRotationY(-rot.X / 180 * MathHelper.Pi);
                        rot_mat *= Matrix3.CreateRotationZ(rot.Z / 180 * MathHelper.Pi);
                        RenderString3D(l.Path, cur_color,
                            -(l.LoadWall[0].X + l.LoadWall[1].X + l.LoadWall[2].X + l.LoadWall[3].X) / 4,
                            (l.LoadWall[0].Y + l.LoadWall[1].Y + l.LoadWall[2].Y + l.LoadWall[3].Y) / 4,
                            (l.LoadWall[0].Z + l.LoadWall[1].Z + l.LoadWall[2].Z + l.LoadWall[3].Z) / 4,
                            ref rot_mat);
                    }
                    if (l.HasTree)
                    {
                        ChunkLinks.ChunkLink.LinkTree tree = l.TreeRoot;
                        while (tree != null)
                        {
                            GL.Begin(PrimitiveType.Lines);
                            for (int i = 0; i < 6; ++i)
                            {
                                switch (i)
                                {
                                    case 0: GL.Color4(Color.Red); break;
                                    case 1: GL.Color4(Color.Green); break;
                                    case 2: GL.Color4(Color.Blue); break;
                                    case 3: GL.Color4(Color.Yellow); break;
                                    case 4: GL.Color4(Color.Magenta); break;
                                    case 5: GL.Color4(Color.Cyan); break;
                                }
                                int i1 = i >= 4 ? 1 - (i - 4) : (0 + 2 * i) % 8;
                                int i2 = i >= 4 ? i1 + 2 : (1 + 2 * i) % 8;
                                int i3 = i >= 4 ? i2 + 2 : (2 + 2 * i) % 8;
                                int i4 = i >= 4 ? i3 + 2 : (3 + 2 * i) % 8;
                                Vector3 mid_vec = new Vector3(tree.LoadArea[i1].X + tree.LoadArea[i2].X + tree.LoadArea[i3].X + tree.LoadArea[i4].X,
                                    tree.LoadArea[i1].Y + tree.LoadArea[i2].Y + tree.LoadArea[i3].Y + tree.LoadArea[i4].Y,
                                    tree.LoadArea[i1].Z + tree.LoadArea[i2].Z + tree.LoadArea[i3].Z + tree.LoadArea[i4].Z) / 4;
                                Vector3 nor_vec = new Vector3(tree.AreaMatrix[i].X, tree.AreaMatrix[i].Y, tree.AreaMatrix[i].Z);
                                Vector3 unk_vec = new Vector3(tree.UnknownMatrix[i].X, tree.UnknownMatrix[i].Y, tree.UnknownMatrix[i].Z);
                                GL.Vertex3(mid_vec);
                                GL.Vertex3(mid_vec + nor_vec);
                                GL.Vertex3(mid_vec);
                                GL.Vertex3(mid_vec + unk_vec);
                            }
                            GL.End();
                            GL.Enable(EnableCap.Lighting);
                            GL.Color4(Color.FromArgb(95, cur_color));
                            GL.Begin(PrimitiveType.QuadStrip);
                            GL.Vertex4(tree.LoadArea[0].ToArray());
                            GL.Vertex4(tree.LoadArea[1].ToArray());
                            GL.Vertex4(tree.LoadArea[2].ToArray());
                            GL.Vertex4(tree.LoadArea[3].ToArray());
                            GL.Vertex4(tree.LoadArea[4].ToArray());
                            GL.Vertex4(tree.LoadArea[5].ToArray());
                            GL.Vertex4(tree.LoadArea[6].ToArray());
                            GL.Vertex4(tree.LoadArea[7].ToArray());
                            GL.Vertex4(tree.LoadArea[0].ToArray());
                            GL.Vertex4(tree.LoadArea[1].ToArray());
                            GL.End();
                            GL.Begin(PrimitiveType.Quads);
                            GL.Vertex4(tree.LoadArea[1].ToArray());
                            GL.Vertex4(tree.LoadArea[3].ToArray());
                            GL.Vertex4(tree.LoadArea[5].ToArray());
                            GL.Vertex4(tree.LoadArea[7].ToArray());
                            GL.Vertex4(tree.LoadArea[0].ToArray());
                            GL.Vertex4(tree.LoadArea[2].ToArray());
                            GL.Vertex4(tree.LoadArea[4].ToArray());
                            GL.Vertex4(tree.LoadArea[6].ToArray());
                            GL.End();
                            GL.Disable(EnableCap.Lighting);
                            GL.Color4(cur_color);
                            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                            GL.Begin(PrimitiveType.QuadStrip);
                            GL.Vertex4(tree.LoadArea[0].ToArray());
                            GL.Vertex4(tree.LoadArea[1].ToArray());
                            GL.Vertex4(tree.LoadArea[2].ToArray());
                            GL.Vertex4(tree.LoadArea[3].ToArray());
                            GL.Vertex4(tree.LoadArea[4].ToArray());
                            GL.Vertex4(tree.LoadArea[5].ToArray());
                            GL.Vertex4(tree.LoadArea[6].ToArray());
                            GL.Vertex4(tree.LoadArea[7].ToArray());
                            GL.Vertex4(tree.LoadArea[0].ToArray());
                            GL.Vertex4(tree.LoadArea[1].ToArray());
                            GL.End();
                            GL.Begin(PrimitiveType.Quads);
                            GL.Vertex4(tree.LoadArea[1].ToArray());
                            GL.Vertex4(tree.LoadArea[3].ToArray());
                            GL.Vertex4(tree.LoadArea[5].ToArray());
                            GL.Vertex4(tree.LoadArea[7].ToArray());
                            GL.Vertex4(tree.LoadArea[0].ToArray());
                            GL.Vertex4(tree.LoadArea[2].ToArray());
                            GL.Vertex4(tree.LoadArea[4].ToArray());
                            GL.Vertex4(tree.LoadArea[6].ToArray());
                            GL.End();
                            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                            GL.Begin(PrimitiveType.Lines);
                            GL.Vertex4(tree.LoadArea[0].ToArray());
                            GL.Vertex4(tree.LoadArea[3].ToArray());
                            GL.Vertex4(tree.LoadArea[1].ToArray());
                            GL.Vertex4(tree.LoadArea[2].ToArray());
                            GL.Vertex4(tree.LoadArea[2].ToArray());
                            GL.Vertex4(tree.LoadArea[5].ToArray());
                            GL.Vertex4(tree.LoadArea[3].ToArray());
                            GL.Vertex4(tree.LoadArea[4].ToArray());
                            GL.Vertex4(tree.LoadArea[4].ToArray());
                            GL.Vertex4(tree.LoadArea[7].ToArray());
                            GL.Vertex4(tree.LoadArea[5].ToArray());
                            GL.Vertex4(tree.LoadArea[6].ToArray());
                            GL.Vertex4(tree.LoadArea[6].ToArray());
                            GL.Vertex4(tree.LoadArea[1].ToArray());
                            GL.Vertex4(tree.LoadArea[7].ToArray());
                            GL.Vertex4(tree.LoadArea[0].ToArray());
                            GL.Vertex4(tree.LoadArea[0].ToArray());
                            GL.Vertex4(tree.LoadArea[4].ToArray());
                            GL.Vertex4(tree.LoadArea[2].ToArray());
                            GL.Vertex4(tree.LoadArea[6].ToArray());
                            GL.Vertex4(tree.LoadArea[1].ToArray());
                            GL.Vertex4(tree.LoadArea[5].ToArray());
                            GL.Vertex4(tree.LoadArea[3].ToArray());
                            GL.Vertex4(tree.LoadArea[7].ToArray());
                            GL.End();

                            if (tree.Next != null)
                            {
                                tree = tree.Next;
                            }
                            else
                                break;
                        }
                    }
                    GL.PopMatrix();
                }
                GL.DepthMask(true);
                GL.LineWidth(1);
            }

            GL.PopMatrix();
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.U:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.U: showScenery = !showScenery; break;
            }
        }

        private void LoadScenery(Matrix4 chunkMatrix)
        {
            if (scenery.SceneryRoot == null)
            {
                return;
            }

            LoadSceneryStruct(scenery.SceneryRoot, chunkMatrix);
        }

        private void LoadSceneryStruct(SceneryData.SceneryStruct branch, Matrix4 chunkMatrix)
        {
            LoadSceneryModel(branch.Model, chunkMatrix);

            for (int i = 0; i < branch.Links.Length; i++)
            {
                if (branch.Links[i] is SceneryData.SceneryModelStruct modelStruct)
                {
                    LoadSceneryModel(modelStruct, chunkMatrix);
                }
                else if (branch.Links[i] is SceneryData.SceneryStruct @struct)
                {
                    LoadSceneryStruct(@struct, chunkMatrix);
                }
            }
        }

        private void LoadSceneryModel(SceneryData.SceneryModelStruct leaf, Matrix4 chunkMatrix)
        {
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;

            uint gfx_section = 6;
            if (file.Data.Type == TwinsFile.FileType.MonkeyBallSM)
            {
                gfx_section = 7;
            }
            SectionController graphics_sec = file.GetItem<SectionController>(gfx_section);
            SectionController mesh_sec = graphics_sec.GetItem<SectionController>(2);
            SectionController model_sec = graphics_sec.GetItem<SectionController>(6);
            SectionController special_sec = graphics_sec.GetItem<SectionController>(7);

            if (file.Data.Type == TwinsFile.FileType.SM2 || file.Data.Type == TwinsFile.FileType.DemoSM2 || file.Data.Type == TwinsFile.FileType.MonkeyBallSM)
            {
                for (int m = 0; m < leaf.Models.Count; m++)
                {
                    ModelController mesh;
                    uint modelID;
                    if (!leaf.Models[m].isSpecial)
                    {
                        modelID = leaf.Models[m].ModelID;
                    }
                    else
                    {
                        //uint LODcount = special_sec.Data.GetItem<SpecialModel>(ptr.Models[m].ModelID).ModelsAmount;
                        //int targetLOD = LODcount == 1 ? 0 : 1;
                        modelID = special_sec.Data.GetItem<LodModel>(leaf.Models[m].ModelID).LODModelIDs[0];
                    }
                    if (modelID == 0xDDDDDDDD) continue;
                    mesh = mesh_sec.GetItem<ModelController>(model_sec.GetItem<RigidModelController>(modelID).Data.MeshID);

                    var rigid = model_sec.GetItem<RigidModelController>(modelID).Data;
                    mesh.LoadMeshData();

                    Matrix4 modelMatrix = Matrix4.Identity;

                    // closest: -M11, -M21, -M31, -X

                    // Rotation
                    modelMatrix.M11 = -leaf.Models[m].ModelMatrix[0].X;
                    modelMatrix.M12 = leaf.Models[m].ModelMatrix[1].X;
                    modelMatrix.M13 = leaf.Models[m].ModelMatrix[2].X;

                    modelMatrix.M21 = -leaf.Models[m].ModelMatrix[0].Y;
                    modelMatrix.M22 = leaf.Models[m].ModelMatrix[1].Y;
                    modelMatrix.M23 = leaf.Models[m].ModelMatrix[2].Y;

                    modelMatrix.M31 = -leaf.Models[m].ModelMatrix[0].Z;
                    modelMatrix.M32 = leaf.Models[m].ModelMatrix[1].Z;
                    modelMatrix.M33 = leaf.Models[m].ModelMatrix[2].Z;

                    modelMatrix.M14 = leaf.Models[m].ModelMatrix[0].W;
                    modelMatrix.M24 = leaf.Models[m].ModelMatrix[1].W;
                    modelMatrix.M34 = leaf.Models[m].ModelMatrix[2].W;

                    // Position
                    modelMatrix.M41 = leaf.Models[m].ModelMatrix[3].X;
                    modelMatrix.M42 = leaf.Models[m].ModelMatrix[3].Y;
                    modelMatrix.M43 = leaf.Models[m].ModelMatrix[3].Z;
                    modelMatrix.M44 = leaf.Models[m].ModelMatrix[3].W;

                    modelMatrix *= Matrix4.CreateScale(-1, 1, 1);


                    for (int v = 0; v < mesh.Vertices.Count; v++)
                    {
                        Vertex[] vbuffer = new Vertex[mesh.Vertices[v].Length];

                        for (int k = 0; k < mesh.Vertices[v].Length; k++)
                        {
                            vbuffer[k] = mesh.Vertices[v][k];
                            Vector4 vertexPos = new Vector4(mesh.Vertices[v][k].Pos.X, mesh.Vertices[v][k].Pos.Y, mesh.Vertices[v][k].Pos.Z, 1);
                            vertexPos *= modelMatrix;
                            mesh.Vertices[v][k].Pos = new Vector3(vertexPos.X, vertexPos.Y, vertexPos.Z);
                        }

                        foreach (var p in mesh.Vertices[v])
                        {
                            min_x = Math.Min(min_x, p.Pos.X);
                            min_y = Math.Min(min_y, p.Pos.Y);
                            min_z = Math.Min(min_z, p.Pos.Z);
                            max_x = Math.Max(max_x, p.Pos.X);
                            max_y = Math.Max(max_y, p.Pos.Y);
                            max_z = Math.Max(max_z, p.Pos.Z);
                        }

                        int vtx_id = GenerateVBOforScenery();

                        if (rigid != null)
                        {
                            Utils.TextUtils.LoadTexture(rigid.MaterialIDs, file, sceneryObjects[vtx_id], v);
                        }

                        sceneryObjects[vtx_id].Vtx = mesh.Vertices[v];
                        sceneryObjects[vtx_id].VtxInd = mesh.Indices[v];
                        mesh.Vertices[v] = vbuffer;

                        UpdateVBOforScenery(vtx_id);
                    }
                }
            }
        }

        protected void UpdateVBOforScenery(int id)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, sceneryObjects[id].ID);
            if (sceneryObjects[id].Vtx.Length > sceneryObjects[id].LastSize)
                GL.BufferData(BufferTarget.ArrayBuffer, Vertex.SizeOf * sceneryObjects[id].Vtx.Length, sceneryObjects[id].Vtx, BufferUsageHint.StaticDraw);
            else
                GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)0, Vertex.SizeOf * sceneryObjects[id].Vtx.Length, sceneryObjects[id].Vtx);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            sceneryObjects[id].LastSize = sceneryObjects[id].Vtx.Length;
        }

        private readonly int sceneryLimit = 4000;
        private int sceneryIndex;
        private int GenerateVBOforScenery()
        {
            if (sceneryIndex >= sceneryLimit)
            {
                sceneryIndex = 0;
            }
            if (sceneryObjects.Count >= sceneryLimit)
            {
                sceneryObjects[sceneryIndex] = new VertexBufferData();
            }
            else
            {
                sceneryObjects.Add(new VertexBufferData());
            }
            return sceneryIndex++;
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
