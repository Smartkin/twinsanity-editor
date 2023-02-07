using OpenTK.Graphics.OpenGL;
using OpenTK;
using System;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public class MeshViewer : ThreeDViewer
    {
        private FileController targetFile;
        private ModelController mesh;
        private SkinController skin;
        private BlendSkinController bskin;
        private ModelXController meshX;
        private SkinXController skinX;
        private BlendSkinXController bskinX;
        private GraphicsInfoController model;
        private FileController file;
        private uint TargetBlendShape = 0;
        private bool BSkinActive = false;
        private bool FullModelActive = false;
        private bool Visible_Models = true;
        private bool Visible_Skin = true;
        private bool Visible_BSkin = true;

        private bool lighting, wire;

        public MeshViewer(ModelController mesh, Form pform)
        {
            //initialize variables here
            this.mesh = mesh;
            zFar = 50F;
            file = mesh.MainFile;
            lighting = true;
            wire = false;
            Tag = pform;
            InitVBO(1);
            pform.Text = "Loading mesh...";
            LoadMesh();
            pform.Text = "Initializing...";
        }
        public MeshViewer(SkinController mesh, Form pform)
        {
            this.skin = mesh;
            zFar = 50F;
            lighting = true;
            wire = false;
            Tag = pform;
            InitVBO(1);
            pform.Text = "Loading mesh...";
            LoadSkin();
            pform.Text = "Initializing...";
        }
        public MeshViewer(BlendSkinController mesh, Form pform)
        {
            this.bskin = mesh;
            zFar = 50F;
            lighting = true;
            wire = false;
            Tag = pform;
            InitVBO(1);
            pform.Text = "Loading mesh...";
            LoadBSkin();
            pform.Text = "Initializing...";
        }
        public MeshViewer(ModelXController mesh, Form pform)
        {
            //initialize variables here
            this.meshX = mesh;
            zFar = 50F;
            lighting = true;
            wire = false;
            Tag = pform;
            InitVBO(1);
            pform.Text = "Loading mesh...";
            LoadMeshX();
            pform.Text = "Initializing...";
        }
        public MeshViewer(SkinXController mesh, Form pform)
        {
            //initialize variables here
            this.skinX = mesh;
            zFar = 50F;
            lighting = true;
            wire = false;
            Tag = pform;
            InitVBO(1);
            pform.Text = "Loading mesh...";
            LoadSkinX();
            pform.Text = "Initializing...";
        }
        public MeshViewer(BlendSkinXController mesh, Form pform)
        {
            //initialize variables here
            this.bskinX = mesh;
            zFar = 50F;
            lighting = true;
            wire = false;
            Tag = pform;
            InitVBO(1);
            pform.Text = "Loading mesh...";
            BSkinActive = true;
            LoadBSkinX();
            pform.Text = "Initializing...";
        }
        public MeshViewer(GraphicsInfoController mesh, Form pform, FileController tFile)
        {
            //initialize variables here
            targetFile = tFile;
            this.model = mesh;
            zFar = 50F;
            lighting = true;
            wire = false;
            Tag = pform;
            InitVBO(2 + model.Data.ModelIDs.Length, true);
            pform.Text = "Loading mesh...";
            if (mesh.Data.BlendSkinID != 0)
            {
                BSkinActive = true;
            }
            FullModelActive = true;
            LoadOGI_Xbox();
            pform.Text = "Initializing...";
        }

        protected override void RenderHUD()
        {
            base.RenderHUD();

            if (FullModelActive)
            {
                if (BSkinActive)
                {
                    RenderString2D($"V Model {Visible_Models}\nB Skin {Visible_Skin}\nN Blend Skin {Visible_BSkin}\nZ BlendShape {TargetBlendShape}/{bskinX.Data.BlendShapeCount}\nL Lights\nX Wireframe", 0, Height, 12, System.Drawing.Color.White, TextAnchor.BotLeft);
                }
                else
                {
                    RenderString2D($"V Model {Visible_Models}\nB Skin {Visible_Skin}\nL Lights\nX Wireframe", 0, Height, 12, System.Drawing.Color.White, TextAnchor.BotLeft);
                }
            }
            else if (BSkinActive)
            {
                RenderString2D($"Z BlendShape {TargetBlendShape}/{bskinX.Data.BlendShapeCount}\nL Lights\nX Wireframe", 0, Height, 12, System.Drawing.Color.White, TextAnchor.BotLeft);
            }
            else
            {
                RenderString2D("L Lights\nX Wireframe", 0, Height, 12, System.Drawing.Color.White, TextAnchor.BotLeft);
            }
        }

        protected override void RenderObjects()
        {
            //put all object rendering code here
            var flags = lighting ? BufferPointerFlags.Normal : BufferPointerFlags.Default;
            if (lighting)
                GL.Enable(EnableCap.Lighting);

            if (FullModelActive)
            {
                if (model.Data.BlendSkinID != 0 && Visible_BSkin)
                {
                    vtx[0].DrawAllElements(PrimitiveType.Triangles, flags);
                }
                if (model.Data.SkinID != 0 && Visible_Skin)
                {
                    vtx[1].DrawAllElements(PrimitiveType.Triangles, flags);
                }
                if (Visible_Models)
                {
                    for (int i = 0; i < model.Data.ModelIDs.Length; i++)
                    {
                        vtx[i + 2].DrawAllElements(PrimitiveType.Triangles, flags);
                    }
                }
            }
            else
            {
                vtx[0].DrawAllElements(PrimitiveType.Triangles, flags);
            }

            if (lighting)
                GL.Disable(EnableCap.Lighting);
            if (wire)
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                GL.Color3(System.Drawing.Color.Black);
                if (FullModelActive)
                {
                    if (model.Data.BlendSkinID != 0 && Visible_BSkin)
                    {
                        vtx[0].DrawAllElements(PrimitiveType.Triangles, BufferPointerFlags.None);
                    }
                    if (model.Data.SkinID != 0 && Visible_Skin)
                    {
                        vtx[1].DrawAllElements(PrimitiveType.Triangles, BufferPointerFlags.None);
                    }
                    if (Visible_Models)
                    {
                        for (int i = 0; i < model.Data.ModelIDs.Length; i++)
                        {
                            vtx[i + 2].DrawAllElements(PrimitiveType.Triangles, BufferPointerFlags.None);
                        }
                    }
                }
                else
                {
                    vtx[0].DrawAllElements(PrimitiveType.Triangles, BufferPointerFlags.None);
                }
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.L:
                case Keys.X:
                case Keys.Z:
                case Keys.V:
                case Keys.B:
                case Keys.N:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.L:
                    lighting = !lighting;
                    break;
                case Keys.X:
                    wire = !wire;
                    break;
                case Keys.Z:
                    NextBlendShape();
                    break;
                case Keys.V:
                    Visible_Models = !Visible_Models;
                    break;
                case Keys.B:
                    Visible_Skin = !Visible_Skin;
                    break;
                case Keys.N:
                    Visible_BSkin = !Visible_BSkin;
                    break;
            }
        }

        public void LoadMesh()
        {
            mesh.LoadMeshData();
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            foreach (var v in mesh.Vertices)
            {
                min_x = Math.Min(min_x, v.Pos.X);
                min_y = Math.Min(min_y, v.Pos.Y);
                min_z = Math.Min(min_z, v.Pos.Z);
                max_x = Math.Max(max_x, v.Pos.X);
                max_y = Math.Max(max_y, v.Pos.Y);
                max_z = Math.Max(max_z, v.Pos.Z);
            }
            vtx[0].Vtx = mesh.Vertices;
            vtx[0].VtxInd = mesh.Indices;
            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
            UpdateVBO(0);
        }

        public void LoadSkin()
        {
            skin.LoadMeshData();
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            foreach (var v in skin.Vertices)
            {
                min_x = Math.Min(min_x, v.Pos.X);
                min_y = Math.Min(min_y, v.Pos.Y);
                min_z = Math.Min(min_z, v.Pos.Z);
                max_x = Math.Max(max_x, v.Pos.X);
                max_y = Math.Max(max_y, v.Pos.Y);
                max_z = Math.Max(max_z, v.Pos.Z);
            }
            vtx[0].Vtx = skin.Vertices;
            vtx[0].VtxInd = skin.Indices;
            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
            UpdateVBO(0);
        }

        public void LoadBSkin()
        {
            bskin.LoadMeshData();
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            foreach (var v in bskin.Vertices)
            {
                min_x = Math.Min(min_x, v.Pos.X);
                min_y = Math.Min(min_y, v.Pos.Y);
                min_z = Math.Min(min_z, v.Pos.Z);
                max_x = Math.Max(max_x, v.Pos.X);
                max_y = Math.Max(max_y, v.Pos.Y);
                max_z = Math.Max(max_z, v.Pos.Z);
            }
            vtx[0].Vtx = bskin.Vertices;
            vtx[0].VtxInd = bskin.Indices;
            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
            UpdateVBO(0);
        }

        public void LoadMeshX()
        {
            meshX.LoadMeshData();
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            foreach (var v in meshX.Vertices)
            {
                min_x = Math.Min(min_x, v.Pos.X);
                min_y = Math.Min(min_y, v.Pos.Y);
                min_z = Math.Min(min_z, v.Pos.Z);
                max_x = Math.Max(max_x, v.Pos.X);
                max_y = Math.Max(max_y, v.Pos.Y);
                max_z = Math.Max(max_z, v.Pos.Z);
            }
            vtx[0].Vtx = meshX.Vertices;
            vtx[0].VtxInd = meshX.Indices;
            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
            UpdateVBO(0);
        }
        public void LoadSkinX()
        {
            skinX.LoadMeshData();
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            foreach (var v in skinX.Vertices)
            {
                min_x = Math.Min(min_x, v.Pos.X);
                min_y = Math.Min(min_y, v.Pos.Y);
                min_z = Math.Min(min_z, v.Pos.Z);
                max_x = Math.Max(max_x, v.Pos.X);
                max_y = Math.Max(max_y, v.Pos.Y);
                max_z = Math.Max(max_z, v.Pos.Z);
            }
            vtx[0].Vtx = skinX.Vertices;
            vtx[0].VtxInd = skinX.Indices;
            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
            UpdateVBO(0);
        }
        public void LoadBSkinX()
        {
            bskinX.LoadMeshData();
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            foreach (var v in bskinX.Vertices)
            {
                min_x = Math.Min(min_x, v.Pos.X);
                min_y = Math.Min(min_y, v.Pos.Y);
                min_z = Math.Min(min_z, v.Pos.Z);
                max_x = Math.Max(max_x, v.Pos.X);
                max_y = Math.Max(max_y, v.Pos.Y);
                max_z = Math.Max(max_z, v.Pos.Z);
            }
            vtx[0].Vtx = bskinX.Vertices;
            vtx[0].VtxInd = bskinX.Indices;
            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
            UpdateVBO(0);
        }

        public void LoadOGI_Xbox()
        {
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;

            if (model.Data.BlendSkinID != 0)
            {
                SectionController mesh_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(5);
                foreach (BlendSkinX mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(5).Records)
                {
                    if (mod.ID == model.Data.BlendSkinID)
                    {
                        bskinX = mesh_sec.GetItem<BlendSkinXController>(mod.ID);
                    }
                }

                bskinX.LoadMeshData();
                foreach (var v in bskinX.Vertices)
                {
                    min_x = Math.Min(min_x, v.Pos.X);
                    min_y = Math.Min(min_y, v.Pos.Y);
                    min_z = Math.Min(min_z, v.Pos.Z);
                    max_x = Math.Max(max_x, v.Pos.X);
                    max_y = Math.Max(max_y, v.Pos.Y);
                    max_z = Math.Max(max_z, v.Pos.Z);
                }
                vtx[0].Vtx = bskinX.Vertices;
                vtx[0].VtxInd = bskinX.Indices;
                UpdateVBO(0);
            }
            if (model.Data.SkinID != 0)
            {
                SectionController mesh_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(4);
                foreach (SkinX mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(4).Records)
                {
                    if (mod.ID == model.Data.SkinID)
                    {
                        skinX = mesh_sec.GetItem<SkinXController>(mod.ID);
                    }
                }

                skinX.LoadMeshData();
                foreach (var v in skinX.Vertices)
                {
                    min_x = Math.Min(min_x, v.Pos.X);
                    min_y = Math.Min(min_y, v.Pos.Y);
                    min_z = Math.Min(min_z, v.Pos.Z);
                    max_x = Math.Max(max_x, v.Pos.X);
                    max_y = Math.Max(max_y, v.Pos.Y);
                    max_z = Math.Max(max_z, v.Pos.Z);
                }
                vtx[1].Vtx = skinX.Vertices;
                vtx[1].VtxInd = skinX.Indices;
                UpdateVBO(1);
            }
            for (int i = 0; i < model.Data.ModelIDs.Length; i++)
            {
                SectionController mesh_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(2);
                foreach (RigidModel mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).Records)
                {
                    if (mod.ID == model.Data.ModelIDs[i].ModelID)
                    {
                        uint meshID = targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).GetItem<RigidModel>(mod.ID).MeshID;
                        meshX = mesh_sec.GetItem<ModelXController>(meshID);
                    }
                }

                Matrix4 tempRot = Matrix4.Identity;

                // Rotation
                tempRot.M11 = -model.Data.Type3[model.Data.ModelIDs[i].ID].Matrix[0].X;
                tempRot.M12 = -model.Data.Type3[model.Data.ModelIDs[i].ID].Matrix[1].X;
                tempRot.M13 = -model.Data.Type3[model.Data.ModelIDs[i].ID].Matrix[2].X;

                tempRot.M21 = model.Data.Type3[model.Data.ModelIDs[i].ID].Matrix[0].Y;
                tempRot.M22 = model.Data.Type3[model.Data.ModelIDs[i].ID].Matrix[1].Y;
                tempRot.M23 = model.Data.Type3[model.Data.ModelIDs[i].ID].Matrix[2].Y;

                tempRot.M31 = model.Data.Type3[model.Data.ModelIDs[i].ID].Matrix[0].Z;
                tempRot.M32 = model.Data.Type3[model.Data.ModelIDs[i].ID].Matrix[1].Z;
                tempRot.M33 = model.Data.Type3[model.Data.ModelIDs[i].ID].Matrix[2].Z;

                tempRot.M14 = model.Data.Type3[model.Data.ModelIDs[i].ID].Matrix[0].W;
                tempRot.M24 = model.Data.Type3[model.Data.ModelIDs[i].ID].Matrix[1].W;
                tempRot.M34 = model.Data.Type3[model.Data.ModelIDs[i].ID].Matrix[2].W;

                // Position
                tempRot.M41 = model.Data.Type1[model.Data.ModelIDs[i].ID].Matrix[1].X;
                tempRot.M42 = model.Data.Type1[model.Data.ModelIDs[i].ID].Matrix[1].Y;
                tempRot.M43 = model.Data.Type1[model.Data.ModelIDs[i].ID].Matrix[1].Z;
                tempRot.M44 = model.Data.Type1[model.Data.ModelIDs[i].ID].Matrix[1].W;

                // Adjusted for OpenTK
                tempRot *= Matrix4.CreateScale(-1, 1, 1);

                meshX.LoadMeshData();

                Vertex[] vbuffer = new Vertex[meshX.Vertices.Length];

                for (int v = 0; v < meshX.Vertices.Length; v++)
                {
                    vbuffer[v] = meshX.Vertices[v];
                    Vector4 targetPos = new Vector4(meshX.Vertices[v].Pos.X, meshX.Vertices[v].Pos.Y, meshX.Vertices[v].Pos.Z, 1);

                    targetPos *= tempRot;

                    meshX.Vertices[v].Pos = new Vector3(targetPos.X, targetPos.Y, targetPos.Z);
                }

                foreach (var v in meshX.Vertices)
                {
                    min_x = Math.Min(min_x, v.Pos.X);
                    min_y = Math.Min(min_y, v.Pos.Y);
                    min_z = Math.Min(min_z, v.Pos.Z);
                    max_x = Math.Max(max_x, v.Pos.X);
                    max_y = Math.Max(max_y, v.Pos.Y);
                    max_z = Math.Max(max_z, v.Pos.Z);
                }
                vtx[i + 2].Vtx = meshX.Vertices;
                vtx[i + 2].VtxInd = meshX.Indices;
                meshX.Vertices = vbuffer;
                UpdateVBO(i + 2);

            }

            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
        }

        public void NextBlendShape()
        {
            if (bskinX == null) return;

            TargetBlendShape++;
            if (TargetBlendShape > bskinX.Data.BlendShapeCount)
            {
                TargetBlendShape = 0;
            }

            if (TargetBlendShape == 0)
            {
                bskinX.LoadMeshData();
            }
            else
            {
                bskinX.LoadMeshData_BlendShape((int)TargetBlendShape - 1);
            }
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            foreach (var v in bskinX.Vertices)
            {
                min_x = Math.Min(min_x, v.Pos.X);
                min_y = Math.Min(min_y, v.Pos.Y);
                min_z = Math.Min(min_z, v.Pos.Z);
                max_x = Math.Max(max_x, v.Pos.X);
                max_y = Math.Max(max_y, v.Pos.Y);
                max_z = Math.Max(max_z, v.Pos.Z);
            }
            vtx[0].Vtx = bskinX.Vertices;
            vtx[0].VtxInd = bskinX.Indices;
            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
            UpdateVBO(0);

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
