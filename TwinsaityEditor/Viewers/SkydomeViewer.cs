using OpenTK.Graphics.OpenGL;
using System;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public class SkydomeViewer : ThreeDViewer
    {
        private SkydomeController sky;
        private FileController file;

        private bool lighting, wire;

        public SkydomeController Sky { get => sky; }

        public SkydomeViewer(SkydomeController sky, Form pform)
        {
            //initialize variables here
            this.sky = sky;
            file = sky.MainFile;
            zFar = 100F;
            var vbos = 0;
            SectionController mesh_sec = file.GetItem<SectionController>(6).GetItem<SectionController>(6);
            for (var i = 0; i < sky.Data.MeshIDs.Length; i++)
            {
                foreach (RigidModel mod in mesh_sec.Data.Records)
                {
                    if (mod.ID == sky.Data.MeshIDs[i])
                    {
                        var m = mesh_sec.GetItem<RigidModelController>(sky.Data.MeshIDs[i]);
                        vbos += m.Data.MaterialIDs.Length;
                        break;
                    }
                }
            }
            InitVBO(vbos);
            pform.Text = "Loading models...";
            LoadModels();
        }

        protected override void RenderHUD()
        {
            base.RenderHUD();
            RenderString2D("Press L to toggle lighting\nPress X to toggle wireframe", 0, Height, 12, System.Drawing.Color.White, TextAnchor.BotLeft);
        }

        protected override void RenderObjects()
        {
            //put all object rendering code here
            for (int i = 0; i < vtx.Count; ++i)
            {
                if (vtx[i].Vtx == null) continue;
                var flags = lighting ? BufferPointerFlags.Normal : BufferPointerFlags.Default;
                if (lighting)
                    GL.Enable(EnableCap.Lighting);
                vtx[i].DrawAllElements(PrimitiveType.Triangles, flags);
                if (lighting)
                    GL.Disable(EnableCap.Lighting);
                if (wire)
                {
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                    GL.Color3(System.Drawing.Color.Black);
                    vtx[i].DrawAllElements(PrimitiveType.Triangles, BufferPointerFlags.None);
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                }
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.L:
                case Keys.X:
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
            }
        }

        private void LoadModels()
        {
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            SectionController mesh_sec = file.GetItem<SectionController>(6).GetItem<SectionController>(2);
            SectionController model_sec = file.GetItem<SectionController>(6).GetItem<SectionController>(6);
            SectionController special_sec = file.GetItem<SectionController>(6).GetItem<SectionController>(7);
            var vtxIndex = 0;
            for (int i = 0; i < sky.Data.MeshIDs.Length; ++i)
            {
                if (special_sec.Data.ContainsItem(sky.Data.MeshIDs[i]))
                    continue;
                ModelController mesh = mesh_sec.GetItem<ModelController>(model_sec.GetItem<RigidModelController>(sky.Data.MeshIDs[i]).Data.MeshID);
                mesh.LoadMeshData();
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
                for (int j = 0; j < mesh.Vertices.Count; j++)
                {
                    vtx[vtxIndex].Vtx = mesh.Vertices[j];
                    vtx[vtxIndex].VtxInd = mesh.Indices[j];
                    Utils.TextUtils.LoadTexture(model_sec.GetItem<RigidModelController>(sky.Data.MeshIDs[i]).Data.MaterialIDs, file, vtx[vtxIndex], j);
                    UpdateVBO(vtxIndex);
                    vtxIndex++;
                }
                
                
            }
            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
        }

        protected void InitVBO(int count)
        {
            MakeCurrent();
            vtx = new System.Collections.Generic.List<VertexBufferData>();
            for (int i = 0; i < count; ++i)
            {
                vtx.Add(new VertexBufferData());
            }
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
