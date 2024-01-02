using OpenTK.Graphics.OpenGL;
using OpenTK;
using System;
using System.Windows.Forms;
using Twinsanity;
using System.Linq;

namespace TwinsaityEditor
{
    public class MeshViewer : ThreeDViewer
    {
        private FileController targetFile;
        private ModelController mesh;
        private RigidModelController rigid;
        private SkinController skin;
        private BlendSkinController bskin;
        private ModelXController meshX;
        private SkinXController skinX;
        private BlendSkinXController bskinX;
        private ModelPController meshP;
        private GraphicsInfoController model;
        private FileController file;
        private uint TargetBlendShape = 0;
        private bool BSkinActive = false;
        private bool FullModelActive = false;
        private bool Visible_Models = true;
        private bool Visible_Skin = true;
        private bool Visible_BSkin = true;

        private bool lighting, wire;
        private bool textures = true;
        private float curShapeWeight = 1.0f;

        private int bskinEndIndex = 0;
        private int skinEndIndex = 0;

        public MeshViewer(ModelController mesh, Form pform)
        {
            //initialize variables here
            this.mesh = mesh;
            zFar = 50F;
            file = mesh.MainFile;
            lighting = true;
            wire = false;
            Tag = pform;
            InitVBO(mesh.Data.SubModels.Count, true);
            pform.Text = "Loading mesh...";
            LoadMesh();
            pform.Text = "Initializing...";
        }
        public MeshViewer(RigidModelController rigid, Form pform)
        {
            if (rigid.MainFile.MeshSection.Data.Parent.Type == SectionType.GraphicsX)
            {
                this.meshX = rigid.MainFile.MeshSection.GetItem<ModelXController>(rigid.Data.MeshID);
                file = meshX.MainFile;
            }
            else
            {
                this.mesh = rigid.MainFile.MeshSection.GetItem<ModelController>(rigid.Data.MeshID);
                file = mesh.MainFile;
            }
            this.rigid = rigid;
            zFar = 50F;
            lighting = false;
            wire = false;
            Tag = pform;
            if (rigid.MainFile.MeshSection.Data.Parent.Type == SectionType.GraphicsX)
            {
                InitVBO(meshX.Data.SubModels.Count, true);
                pform.Text = "Loading mesh...";
                LoadMeshX();
                pform.Text = "Initializing...";
            }
            else
            {
                InitVBO(mesh.Data.SubModels.Count, true);
                pform.Text = "Loading mesh...";
                LoadMesh();
                pform.Text = "Initializing...";
            }
        }
        public MeshViewer(SkinController mesh, Form pform)
        {
            this.skin = mesh;
            file = mesh.MainFile;
            zFar = 50F;
            lighting = false;
            wire = false;
            Tag = pform;
            InitVBO(mesh.Data.SubModels.Count, true);
            pform.Text = "Loading mesh...";
            LoadSkin();
            pform.Text = "Initializing...";
        }
        public MeshViewer(BlendSkinController mesh, Form pform)
        {
            this.bskin = mesh;
            zFar = 50F;
            file = mesh.MainFile;
            targetFile = file;
            lighting = false;
            wire = false;
            Tag = pform;
            InitVBO(mesh.Data.Models.Length, true);
            pform.Text = "Loading mesh...";
            BSkinActive = true;
            LoadBSkin();
            pform.Text = "Initializing...";
        }
        public MeshViewer(ModelXController mesh, Form pform)
        {
            //initialize variables here
            this.meshX = mesh;
            file = mesh.MainFile;
            zFar = 50F;
            lighting = true;
            wire = false;
            Tag = pform;
            InitVBO(meshX.Data.SubModels.Count, true);
            pform.Text = "Loading mesh...";
            LoadMeshX();
            pform.Text = "Initializing...";
        }
        public MeshViewer(SkinXController mesh, Form pform)
        {
            //initialize variables here
            this.skinX = mesh;
            file = mesh.MainFile;
            targetFile = file;
            zFar = 50F;
            lighting = true;
            wire = false;
            Tag = pform;
            InitVBO(mesh.Data.SubModels.Count, true);
            pform.Text = "Loading mesh...";
            LoadSkinX();
            pform.Text = "Initializing...";
        }
        public MeshViewer(BlendSkinXController mesh, Form pform)
        {
            //initialize variables here
            this.bskinX = mesh;
            file = mesh.MainFile;
            targetFile = file;
            zFar = 50F;
            lighting = true;
            wire = false;
            Tag = pform;
            InitVBO(mesh.Data.SubModels.Count);
            pform.Text = "Loading mesh...";
            BSkinActive = true;
            LoadBSkinX();
            pform.Text = "Initializing...";
        }
        public MeshViewer(ModelPController mesh, Form pform)
        {
            //initialize variables here
            this.meshP = mesh;
            zFar = 50F;
            file = mesh.MainFile;
            lighting = true;
            wire = false;
            Tag = pform;
            InitVBO(mesh.Data.SubModels.Count, true);
            pform.Text = "Loading mesh...";
            LoadMeshP();
            pform.Text = "Initializing...";
        }
        public MeshViewer(GraphicsInfoController mesh, Form pform, FileController tFile)
        {
            //initialize variables here
            targetFile = tFile;
            file = mesh.MainFile;
            this.model = mesh;
            zFar = 50F;
            lighting = false;
            wire = false;
            Tag = pform;
            var vbos = 0;
            SectionController mesh_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(2);
            if (mesh_sec.Data.Type == SectionType.ModelX)
            {
                foreach (var pair in model.Data.ModelIDs)
                {
                    foreach (RigidModel mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).Records)
                    {
                        if (mod.ID == pair.Value.ModelID)
                        {
                            uint meshID = targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).GetItem<RigidModel>(mod.ID).MeshID;
                            ModelXController m = mesh_sec.GetItem<ModelXController>(meshID);
                            vbos += m.Data.SubModels.Count;
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (var pair in model.Data.ModelIDs)
                {
                    foreach (RigidModel mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).Records)
                    {
                        if (mod.ID == pair.Value.ModelID)
                        {
                            uint meshID = targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).GetItem<RigidModel>(mod.ID).MeshID;
                            ModelController m = mesh_sec.GetItem<ModelController>(meshID);
                            vbos += m.Data.SubModels.Count;
                            break;
                        }
                    }
                }
            }
            
            if (model.Data.SkinID != 0)
            {
                SectionController skin_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(4);
                if (skin_sec.Data.Type == SectionType.SkinX)
                {
                    foreach (SkinX mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(4).Records)
                    {
                        if (mod.ID == model.Data.SkinID)
                        {
                            skinX = skin_sec.GetItem<SkinXController>(mod.ID);
                            vbos += skinX.Data.SubModels.Count;
                        }
                    }
                }
                else
                {
                    foreach (Skin mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(4).Records)
                    {
                        if (mod.ID == model.Data.SkinID)
                        {
                            skin = skin_sec.GetItem<SkinController>(mod.ID);
                            vbos += skin.Data.SubModels.Count;
                        }
                    }
                }
            }
            if (mesh.Data.BlendSkinID != 0)
            {
                BSkinActive = true;
                SectionController blend_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(5);
                if (blend_sec.Data.Type == SectionType.BlendSkinX)
                {
                    foreach (BlendSkinX mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(5).Records)
                    {
                        if (mod.ID == model.Data.BlendSkinID)
                        {
                            bskinX = blend_sec.GetItem<BlendSkinXController>(mod.ID);
                            vbos += bskinX.Data.SubModels.Count;
                        }
                    }
                }
                else
                {
                    foreach (BlendSkin mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(5).Records)
                    {
                        if (mod.ID == model.Data.BlendSkinID)
                        {
                            bskin = blend_sec.GetItem<BlendSkinController>(mod.ID);
                            vbos += bskin.Data.Models.Length;
                        }
                    }
                }
                
            }
            FullModelActive = true;

            InitVBO(vbos, true);
            pform.Text = "Loading mesh...";
            if (targetFile.Data.Type == TwinsFile.FileType.RM2 || targetFile.Data.Type == TwinsFile.FileType.DemoRM2)
            {
                LoadOGI_PS2();
            }
            else if (targetFile.Data.Type == TwinsFile.FileType.RMX)
            {
                LoadOGI_Xbox();
            }
            pform.Text = "Initializing...";
        }

        protected override void RenderHUD()
        {
            base.RenderHUD();

            var renderString = $"L Lights {lighting}\nX Wireframe {wire}\nY Textures {textures}\n";
            if (FullModelActive)
            {
                renderString += $"V Model {Visible_Models}\nB Skin {Visible_Skin}\nN Blend Skin {Visible_BSkin}\n";
                if (BSkinActive)
                {
                    if (targetFile.Data.Type == TwinsFile.FileType.RMX)
                    {
                        renderString += $"Z BlendShape {TargetBlendShape}/{bskinX.Data.BlendShapeCount}\n";
                    }
                    else if (targetFile.Data.Type == TwinsFile.FileType.RM2)
                    {
                        renderString += $"Z BlendShape {TargetBlendShape}/{bskin.Data.BlendShapeCount}\n";
                    }
                }
            }
            else if (BSkinActive)
            {
                if (targetFile.Data.Type == TwinsFile.FileType.RMX)
                {
                    renderString += $"Z BlendShape {TargetBlendShape}/{bskinX.Data.BlendShapeCount}\n";
                }
                else if (targetFile.Data.Type == TwinsFile.FileType.RM2)
                {
                    renderString += $"Z BlendShape {TargetBlendShape}/{bskin.Data.BlendShapeCount}\n";
                }
            }
            RenderString2D(renderString, 0, Height, 12, System.Drawing.Color.White, TextAnchor.BotLeft);
        }

        protected override void RenderObjects()
        {
            //put all object rendering code here
            var flags = lighting ? BufferPointerFlags.Normal : BufferPointerFlags.Default;
            if (lighting)
                GL.Enable(EnableCap.Lighting);

            if (!wire)
            {
                if (FullModelActive)
                {
                    if (model.Data.BlendSkinID != 0 && Visible_BSkin)
                    {
                        for (int i = 0; i < bskinEndIndex; i++)
                        {
                            vtx[i].DrawAllElements(PrimitiveType.Triangles, flags);
                        }
                    }
                    if (model.Data.SkinID != 0 && Visible_Skin)
                    {
                        for (int i = bskinEndIndex; i < skinEndIndex; i++)
                        {
                            vtx[i].DrawAllElements(PrimitiveType.Triangles, flags);
                        }
                    }
                    if (Visible_Models)
                    {
                        for (int i = skinEndIndex; i < vtx.Count; i++)
                        {
                            vtx[i].DrawAllElements(PrimitiveType.Triangles, flags);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < vtx.Count; i++)
                    {
                        vtx[i].DrawAllElements(PrimitiveType.Triangles, flags);
                    }
                }
            }

            if (lighting)
                GL.Disable(EnableCap.Lighting);
            if (wire)
            {
                var wire_flags = textures ? BufferPointerFlags.TexCoord : BufferPointerFlags.None;
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                GL.Color3(System.Drawing.Color.Black);
                GL.LineWidth(2);
                if (FullModelActive)
                {
                    if (model.Data.BlendSkinID != 0 && Visible_BSkin)
                    {
                        vtx[0].DrawAllElements(PrimitiveType.Triangles, wire_flags, true);
                    }
                    if (model.Data.SkinID != 0 && Visible_Skin)
                    {
                        vtx[1].DrawAllElements(PrimitiveType.Triangles, wire_flags, true);
                    }
                    if (Visible_Models)
                    {
                        for (int i = 0; i < vtx.Count - 2; i++)
                        {
                            vtx[i + 2].DrawAllElements(PrimitiveType.Triangles, wire_flags, true);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < vtx.Count; i++)
                    {
                        
                        vtx[i].DrawAllElements(PrimitiveType.Triangles, wire_flags, true);
                    }
                }
                GL.LineWidth(1);
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
                case Keys.P:
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
                case Keys.Y:
                    textures = !textures;
                    SetTexturing(textures);
                    break;
                case Keys.P:
                    curShapeWeight += 0.05f;
                    if (curShapeWeight > 1f)
                    {
                        curShapeWeight = 0f;
                    }
                    LoadBlendShape(curShapeWeight);
                    break;
            }
        }

        private void SetTexturing(bool textures)
        {
            for (int i = 0; i < vtx.Count; i++)
            {
                if (vtx[i].Texture != -1)
                {
                    vtx[i].Textured = textures;
                }
            }
        }

        public void LoadMesh()
        {
            mesh.LoadMeshData();
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            foreach (var list in mesh.Vertices)
            {
                foreach (var v in list)
                {
                    min_x = Math.Min(min_x, v.Pos.X);
                    min_y = Math.Min(min_y, v.Pos.Y);
                    min_z = Math.Min(min_z, v.Pos.Z);
                    max_x = Math.Max(max_x, v.Pos.X);
                    max_y = Math.Max(max_y, v.Pos.Y);
                    max_z = Math.Max(max_z, v.Pos.Z);
                }
            }
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                vtx[i].Vtx = mesh.Vertices[i];
                vtx[i].VtxInd = mesh.Indices[i];
                if (this is ModelViewer)
                {
                    Utils.TextUtils.LoadTexture(rigid.Data.MaterialIDs, file, vtx[i], i);
                }
                UpdateVBO(i);
            }
            
            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
            
        }

        public void LoadSkin()
        {
            skin.LoadMeshData();
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            foreach (var list in skin.Vertices)
            {
                foreach (var v in list)
                {
                    min_x = Math.Min(min_x, v.Pos.X);
                    min_y = Math.Min(min_y, v.Pos.Y);
                    min_z = Math.Min(min_z, v.Pos.Z);
                    max_x = Math.Max(max_x, v.Pos.X);
                    max_y = Math.Max(max_y, v.Pos.Y);
                    max_z = Math.Max(max_z, v.Pos.Z);
                }
            }
            for (int i = 0; i < skin.Vertices.Count; i++)
            {
                vtx[i].Vtx = skin.Vertices[i];
                vtx[i].VtxInd = skin.Indices[i];
                Utils.TextUtils.LoadTexture(skin.Data.SubModels.Select((subModel) =>
                {
                    return subModel.MaterialID;
                }).ToArray(), file, vtx[i], i);
                UpdateVBO(i);
            }
            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
        }

        public void LoadBSkin()
        {
            bskin.LoadMeshData();
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            foreach (var list in bskin.Vertices)
            {
                foreach (var v in list)
                {
                    min_x = Math.Min(min_x, v.Pos.X);
                    min_y = Math.Min(min_y, v.Pos.Y);
                    min_z = Math.Min(min_z, v.Pos.Z);
                    max_x = Math.Max(max_x, v.Pos.X);
                    max_y = Math.Max(max_y, v.Pos.Y);
                    max_z = Math.Max(max_z, v.Pos.Z);
                }
            }
            for (int i = 0; i < bskin.Vertices.Count; i++)
            {
                vtx[i].Vtx = bskin.Vertices[i];
                vtx[i].VtxInd = bskin.Indices[i];
                Utils.TextUtils.LoadTexture(bskin.Data.Models[i].MaterialID, file, vtx[i]);
                UpdateVBO(i);
            }
            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
        }

        public void LoadMeshX()
        {
            meshX.LoadMeshData();
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            foreach (var list in meshX.Vertices)
            {
                foreach (var v in list)
                {
                    min_x = Math.Min(min_x, v.Pos.X);
                    min_y = Math.Min(min_y, v.Pos.Y);
                    min_z = Math.Min(min_z, v.Pos.Z);
                    max_x = Math.Max(max_x, v.Pos.X);
                    max_y = Math.Max(max_y, v.Pos.Y);
                    max_z = Math.Max(max_z, v.Pos.Z);
                }
            }
            for (int i = 0; i < meshX.Vertices.Count; i++)
            {
                vtx[i].Vtx = meshX.Vertices[i];
                vtx[i].VtxInd = meshX.Indices[i];
                if (this is ModelViewer)
                {
                    Utils.TextUtils.LoadTexture(rigid.Data.MaterialIDs, file, vtx[i], i);
                }
                UpdateVBO(i);
            }
            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
        }
        public void LoadSkinX()
        {
            skinX.LoadMeshData();
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            foreach (var list in skinX.Vertices)
            {
                foreach (var v in list)
                {
                    min_x = Math.Min(min_x, v.Pos.X);
                    min_y = Math.Min(min_y, v.Pos.Y);
                    min_z = Math.Min(min_z, v.Pos.Z);
                    max_x = Math.Max(max_x, v.Pos.X);
                    max_y = Math.Max(max_y, v.Pos.Y);
                    max_z = Math.Max(max_z, v.Pos.Z);
                }
            }
            for (int i = 0; i < skinX.Vertices.Count; i++)
            {
                vtx[i].Vtx = skinX.Vertices[i];
                vtx[i].VtxInd = skinX.Indices[i];
                Utils.TextUtils.LoadTexture(skinX.Data.SubModels.Select((subModel) =>
                {
                    return subModel.MaterialID;
                }).ToArray(), file, vtx[i], i);
                UpdateVBO(i);
            }
            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
        }
        public void LoadBSkinX()
        {
            bskinX.LoadMeshData();
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            foreach (var list in bskinX.Vertices)
            {
                foreach (var v in list)
                {
                    min_x = Math.Min(min_x, v.Pos.X);
                    min_y = Math.Min(min_y, v.Pos.Y);
                    min_z = Math.Min(min_z, v.Pos.Z);
                    max_x = Math.Max(max_x, v.Pos.X);
                    max_y = Math.Max(max_y, v.Pos.Y);
                    max_z = Math.Max(max_z, v.Pos.Z);
                }
            }
            for (int i = 0; i < bskinX.Vertices.Count; i++)
            {
                vtx[i].Vtx = bskinX.Vertices[i];
                vtx[i].VtxInd = bskinX.Indices[i];
                Utils.TextUtils.LoadTexture(bskinX.Data.SubModels.Select((subModel) =>
                {
                    return subModel.MaterialID;
                }).ToArray(), file, vtx[i], i);
                UpdateVBO(i);
            }
            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
        }

        public void LoadOGI_Xbox()
        {
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;

            var vtxIndex = 0;
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
                foreach (var list in bskinX.Vertices)
                {
                    foreach (var v in list)
                    {
                        min_x = Math.Min(min_x, v.Pos.X);
                        min_y = Math.Min(min_y, v.Pos.Y);
                        min_z = Math.Min(min_z, v.Pos.Z);
                        max_x = Math.Max(max_x, v.Pos.X);
                        max_y = Math.Max(max_y, v.Pos.Y);
                        max_z = Math.Max(max_z, v.Pos.Z);
                    }
                }
                for (int i = 0; i < bskinX.Vertices.Count; i++)
                {
                    vtx[vtxIndex].Vtx = bskinX.Vertices[i];
                    vtx[vtxIndex].VtxInd = bskinX.Indices[i];
                    Utils.TextUtils.LoadTexture(bskinX.Data.SubModels.Select((subModel) =>
                    {
                        return subModel.MaterialID;
                    }).ToArray(), file, vtx[vtxIndex], i);
                    UpdateVBO(vtxIndex);
                    vtxIndex++;
                }
            }
            bskinEndIndex = vtxIndex;
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
                foreach (var list in skinX.Vertices)
                {
                    foreach (var v in list)
                    {
                        min_x = Math.Min(min_x, v.Pos.X);
                        min_y = Math.Min(min_y, v.Pos.Y);
                        min_z = Math.Min(min_z, v.Pos.Z);
                        max_x = Math.Max(max_x, v.Pos.X);
                        max_y = Math.Max(max_y, v.Pos.Y);
                        max_z = Math.Max(max_z, v.Pos.Z);
                    }
                }
                for (int i = 0; i < skinX.Vertices.Count; i++)
                {
                    vtx[vtxIndex].Vtx = skinX.Vertices[i];
                    vtx[vtxIndex].VtxInd = skinX.Indices[i];
                    Utils.TextUtils.LoadTexture(skinX.Data.SubModels.Select((subModel) =>
                    {
                        return subModel.MaterialID;
                    }).ToArray(), file, vtx[vtxIndex], i);
                    UpdateVBO(vtxIndex);
                    vtxIndex++;
                }
            }
            skinEndIndex = vtxIndex;
            foreach (var pair in model.Data.ModelIDs)
            {
                SectionController mesh_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(2);
                SectionController rigid_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(3);
                RigidModelController rigid = null;
                foreach (RigidModel mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).Records)
                {
                    if (mod.ID == pair.Value.ModelID)
                    {
                        uint meshID = targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).GetItem<RigidModel>(mod.ID).MeshID;
                        meshX = mesh_sec.GetItem<ModelXController>(meshID);
                        rigid = rigid_sec.GetItem<RigidModelController>(mod.ID);
                    }
                }

                Matrix4 tempRot = Matrix4.Identity;

                // Rotation
                tempRot.M11 = -model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[0].X;
                tempRot.M12 = -model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[1].X;
                tempRot.M13 = -model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[2].X;

                tempRot.M21 = model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[0].Y;
                tempRot.M22 = model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[1].Y;
                tempRot.M23 = model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[2].Y;

                tempRot.M31 = model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[0].Z;
                tempRot.M32 = model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[1].Z;
                tempRot.M33 = model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[2].Z;

                tempRot.M14 = model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[0].W;
                tempRot.M24 = model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[1].W;
                tempRot.M34 = model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[2].W;

                //var rot = tempRot.ExtractRotation();
                //rot.X = -rot.X;
                //rot.Y = -rot.Y;
                //rot.Z = -rot.Z;
                //tempRot = Matrix4.CreateFromQuaternion(rot);

                // Position (Joint's world position)
                tempRot.M41 = model.Data.Joints[pair.Value.JointIndex].Matrix[1].X;
                tempRot.M42 = model.Data.Joints[pair.Value.JointIndex].Matrix[1].Y;
                tempRot.M43 = model.Data.Joints[pair.Value.JointIndex].Matrix[1].Z;
                tempRot.M44 = model.Data.Joints[pair.Value.JointIndex].Matrix[1].W;


                // Adjusted for OpenTK
                //tempRot *= Matrix4.CreateScale(-1, 1, 1);

                meshX.LoadMeshData();



                for (int v = 0; v < meshX.Vertices.Count; v++)
                {
                    Vertex[] vbuffer = new Vertex[meshX.Vertices[v].Length];
                    for (int k = 0; k < meshX.Vertices[v].Length; k++)
                    {
                        vbuffer[k] = meshX.Vertices[v][k];
                        Vector4 targetPos = new Vector4(meshX.Vertices[v][k].Pos.X, meshX.Vertices[v][k].Pos.Y, meshX.Vertices[v][k].Pos.Z, 1);

                        targetPos *= tempRot;

                        meshX.Vertices[v][k].Pos = new Vector3(targetPos.X, targetPos.Y, targetPos.Z);

                    }


                    foreach (var p in meshX.Vertices[v])
                    {
                        min_x = Math.Min(min_x, p.Pos.X);
                        min_y = Math.Min(min_y, p.Pos.Y);
                        min_z = Math.Min(min_z, p.Pos.Z);
                        max_x = Math.Max(max_x, p.Pos.X);
                        max_y = Math.Max(max_y, p.Pos.Y);
                        max_z = Math.Max(max_z, p.Pos.Z);
                    }
                    if (rigid != null)
                    {
                        Utils.TextUtils.LoadTexture(rigid.Data.MaterialIDs, file, vtx[vtxIndex], v);
                    }
                    vtx[vtxIndex].Vtx = meshX.Vertices[v];
                    vtx[vtxIndex].VtxInd = meshX.Indices[v];
                    meshX.Vertices[v] = vbuffer;
                    UpdateVBO(vtxIndex);
                    vtxIndex++;
                }



            }

            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
        }

        public void LoadOGI_PS2()
        {
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;

            var vtxIndex = 0;
            if (model.Data.BlendSkinID != 0)
            {
                SectionController mesh_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(5);
                foreach (BlendSkin mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(5).Records)
                {
                    if (mod.ID == model.Data.BlendSkinID)
                    {
                        bskin = mesh_sec.GetItem<BlendSkinController>(mod.ID);
                    }
                }

                bskin.LoadMeshData();
                foreach (var list in bskin.Vertices)
                {
                    foreach (var v in list)
                    {
                        min_x = Math.Min(min_x, v.Pos.X);
                        min_y = Math.Min(min_y, v.Pos.Y);
                        min_z = Math.Min(min_z, v.Pos.Z);
                        max_x = Math.Max(max_x, v.Pos.X);
                        max_y = Math.Max(max_y, v.Pos.Y);
                        max_z = Math.Max(max_z, v.Pos.Z);
                    }
                }
                for (int i = 0; i < bskin.Vertices.Count; i++)
                {
                    vtx[vtxIndex].Vtx = bskin.Vertices[i];
                    vtx[vtxIndex].VtxInd = bskin.Indices[i];
                    Utils.TextUtils.LoadTexture(bskin.Data.Models.Select((subModel) =>
                    {
                        return subModel.MaterialID;
                    }).ToArray(), file, vtx[vtxIndex], i);
                    UpdateVBO(vtxIndex);
                    vtxIndex++;
                }
            }
            bskinEndIndex = vtxIndex;
            if (model.Data.SkinID != 0)
            {
                SectionController mesh_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(4);
                foreach (Skin mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(4).Records)
                {
                    if (mod.ID == model.Data.SkinID)
                    {
                        skin = mesh_sec.GetItem<SkinController>(mod.ID);
                    }
                }

                skin.LoadMeshData();
                foreach (var list in skin.Vertices)
                {
                    foreach (var v in list)
                    {
                        min_x = Math.Min(min_x, v.Pos.X);
                        min_y = Math.Min(min_y, v.Pos.Y);
                        min_z = Math.Min(min_z, v.Pos.Z);
                        max_x = Math.Max(max_x, v.Pos.X);
                        max_y = Math.Max(max_y, v.Pos.Y);
                        max_z = Math.Max(max_z, v.Pos.Z);
                    }
                }
                for (int i = 0; i < skin.Vertices.Count; i++)
                {
                    vtx[vtxIndex].Vtx = skin.Vertices[i];
                    vtx[vtxIndex].VtxInd = skin.Indices[i];
                    Utils.TextUtils.LoadTexture(skin.Data.SubModels.Select((subModel) =>
                    {
                        return subModel.MaterialID;
                    }).ToArray(), file, vtx[vtxIndex], i);
                    UpdateVBO(vtxIndex);
                    vtxIndex++;
                }
            }
            skinEndIndex = vtxIndex;
            foreach (var pair in model.Data.ModelIDs)
            {
                SectionController mesh_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(2);
                SectionController rigid_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(3);
                RigidModelController rigid = null;
                foreach (RigidModel mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).Records)
                {
                    if (mod.ID == pair.Value.ModelID)
                    {
                        uint meshID = targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).GetItem<RigidModel>(mod.ID).MeshID;
                        mesh = mesh_sec.GetItem<ModelController>(meshID);
                        rigid = rigid_sec.GetItem<RigidModelController>(mod.ID);
                    }
                }

                Matrix4 tempRot = Matrix4.Identity;

                // Rotation
                tempRot.M11 = -model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[0].X;
                tempRot.M12 = -model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[1].X;
                tempRot.M13 = -model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[2].X;

                tempRot.M21 = model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[0].Y;
                tempRot.M22 = model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[1].Y;
                tempRot.M23 = model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[2].Y;

                tempRot.M31 = model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[0].Z;
                tempRot.M32 = model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[1].Z;
                tempRot.M33 = model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[2].Z;

                tempRot.M14 = model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[0].W;
                tempRot.M24 = model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[1].W;
                tempRot.M34 = model.Data.SkinTransforms[pair.Value.JointIndex].Matrix[2].W;

                //var rot = tempRot.ExtractRotation();
                //rot.X = -rot.X;
                //rot.Y = -rot.Y;
                //rot.Z = -rot.Z;
                //tempRot = Matrix4.CreateFromQuaternion(rot);

                // Position (Joint's world position)
                tempRot.M41 = model.Data.Joints[pair.Value.JointIndex].Matrix[1].X;
                tempRot.M42 = model.Data.Joints[pair.Value.JointIndex].Matrix[1].Y;
                tempRot.M43 = model.Data.Joints[pair.Value.JointIndex].Matrix[1].Z;
                tempRot.M44 = model.Data.Joints[pair.Value.JointIndex].Matrix[1].W;


                // Adjusted for OpenTK
                //tempRot *= Matrix4.CreateScale(-1, 1, 1);

                mesh.LoadMeshData();

                

                for (int v = 0; v < mesh.Vertices.Count; v++)
                {
                    Vertex[] vbuffer = new Vertex[mesh.Vertices[v].Length];
                    for (int k = 0; k < mesh.Vertices[v].Length; k++)
                    {
                        vbuffer[k] = mesh.Vertices[v][k];
                        Vector4 targetPos = new Vector4(mesh.Vertices[v][k].Pos.X, mesh.Vertices[v][k].Pos.Y, mesh.Vertices[v][k].Pos.Z, 1);

                        targetPos *= tempRot;

                        mesh.Vertices[v][k].Pos = new Vector3(targetPos.X, targetPos.Y, targetPos.Z);

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
                    if (rigid != null)
                    {
                        Utils.TextUtils.LoadTexture(rigid.Data.MaterialIDs, file, vtx[vtxIndex], v);
                    }
                    vtx[vtxIndex].Vtx = mesh.Vertices[v];
                    vtx[vtxIndex].VtxInd = mesh.Indices[v];
                    mesh.Vertices[v] = vbuffer;
                    UpdateVBO(vtxIndex);
                    vtxIndex++;
                }



            }

            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
        }

        public void LoadMeshP()
        {
            meshP.LoadMeshData();
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            foreach (var list in meshP.Vertices)
            {
                foreach (var v in list)
                {
                    min_x = Math.Min(min_x, v.Pos.X);
                    min_y = Math.Min(min_y, v.Pos.Y);
                    min_z = Math.Min(min_z, v.Pos.Z);
                    max_x = Math.Max(max_x, v.Pos.X);
                    max_y = Math.Max(max_y, v.Pos.Y);
                    max_z = Math.Max(max_z, v.Pos.Z);
                }
            }
            for (int i = 0; i < meshP.Vertices.Count; i++)
            {
                vtx[i].Vtx = meshP.Vertices[i];
                vtx[i].VtxInd = meshP.Indices[i];
                if (this is ModelViewer)
                {
                    Utils.TextUtils.LoadTexture(rigid.Data.MaterialIDs, file, vtx[i], i);
                }
                UpdateVBO(i);
            }
            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
        }

        public void NextBlendShape()
        {
            if (bskinX == null && bskin == null) return;

            TargetBlendShape++;
            if (bskinX != null)
            {
                if (TargetBlendShape > bskinX.Data.BlendShapeCount)
                {
                    TargetBlendShape = 0;
                }
            }
            if (bskin != null)
            {
                if (TargetBlendShape > bskin.Data.BlendShapeCount)
                {
                    TargetBlendShape = 0;
                }
            }

            LoadBlendShape();

        }

        private void LoadBlendShape(float shapeWeight = 1f)
        {
            if (TargetBlendShape == 0)
            {
                bskinX?.LoadMeshData();
                bskin?.LoadMeshData();
            }
            else
            {
                bskinX?.LoadMeshData_BlendShape((int)TargetBlendShape - 1);
                bskin?.LoadMeshData_BlendShape((int)TargetBlendShape - 1, shapeWeight);
            }
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            if (bskinX != null)
            {
                foreach (var list in bskinX.Vertices)
                {
                    foreach (var v in list)
                    {
                        min_x = Math.Min(min_x, v.Pos.X);
                        min_y = Math.Min(min_y, v.Pos.Y);
                        min_z = Math.Min(min_z, v.Pos.Z);
                        max_x = Math.Max(max_x, v.Pos.X);
                        max_y = Math.Max(max_y, v.Pos.Y);
                        max_z = Math.Max(max_z, v.Pos.Z);
                    }
                }
                for (int i = 0; i < bskinX.Vertices.Count; i++)
                {
                    vtx[i].Vtx = bskinX.Vertices[i];
                    vtx[i].VtxInd = bskinX.Indices[i];
                    Utils.TextUtils.LoadTexture(bskinX.Data.SubModels.Select((subModel) =>
                    {
                        return subModel.MaterialID;
                    }).ToArray(), file, vtx[i], i);
                    UpdateVBO(i);
                }
            }
            if (bskin != null)
            {
                foreach (var list in bskin.Vertices)
                {
                    foreach (var v in list)
                    {
                        min_x = Math.Min(min_x, v.Pos.X);
                        min_y = Math.Min(min_y, v.Pos.Y);
                        min_z = Math.Min(min_z, v.Pos.Z);
                        max_x = Math.Max(max_x, v.Pos.X);
                        max_y = Math.Max(max_y, v.Pos.Y);
                        max_z = Math.Max(max_z, v.Pos.Z);
                    }
                }
                for (int i = 0; i < bskin.Vertices.Count; i++)
                {
                    vtx[i].Vtx = bskin.Vertices[i];
                    vtx[i].VtxInd = bskin.Indices[i];
                    Utils.TextUtils.LoadTexture(bskin.Data.Models[i].MaterialID, file, vtx[i]);
                    UpdateVBO(i);
                }
            }
            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
        }


        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            Utils.TextUtils.ClearTextureCache();
            base.Dispose(disposing);
        }
    }
}
