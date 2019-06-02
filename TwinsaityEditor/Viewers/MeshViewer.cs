using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TwinsaityEditor
{
    public class MeshViewer : ThreeDViewer
    {
        private MeshController mesh;
        private FileController file;

        public MeshViewer(MeshController mesh, ref Form pform)
        {
            //initialize variables here
            this.mesh = mesh;
            zFar = 150F;
            file = mesh.MainFile;
            Tag = pform;
            InitVBO(1);
            pform.Text = "Loading mesh...";
            LoadMesh();
            pform.Text = "Initializing...";
        }

        protected override void RenderHUD()
        {
            GL.Color3(Color.White);
            RenderString2D($"RenderObjects: {timeRenderObj}ms (max: {timeRenderObj_max}ms, min: {timeRenderObj_min}ms)", 0, 0, 8);
            RenderString2D($"RenderHUD: {timeRenderHud}ms (max: {timeRenderHud_max}ms, min: {timeRenderHud_min}ms)", Width, Height, 8, TextAnchor.BotRight);
        }

        protected override void RenderObjects()
        {
            //put all object rendering code here
            GL.Enable(EnableCap.Lighting);
            vtx[0].DrawAll(PrimitiveType.Triangles, BufferPointerFlags.Normal);
            GL.Disable(EnableCap.Lighting);
        }

        public void LoadMesh()
        {
            //float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            List<Vertex> vtx_stack = new List<Vertex>();
            foreach (var s in mesh.Data.SubModels)
            {
                foreach (var g in s.Groups)
                {
                    if (g.VertHead > 0 && g.VDataHead > 0 && g.VertexCount >= 3)
                    {
                        for (int i = 2; i < g.VertexCount; ++i)
                        {
                            if (g.VData[i].CONN == 128) continue;
                            var v1 = (i & 0x1) == 0x1 ? i - 1 : i - 2;
                            var v2 = (i & 0x1) == 0x1 ? i - 2 : i - 1;
                            var v3 = i;
                            Vector3 vec_1 = new Vector3(-g.Vertex[v1].X, g.Vertex[v1].Y, g.Vertex[v1].Z);
                            Vector3 vec_2 = new Vector3(-g.Vertex[v2].X, g.Vertex[v2].Y, g.Vertex[v2].Z);
                            Vector3 vec_3 = new Vector3(-g.Vertex[v3].X, g.Vertex[v3].Y, g.Vertex[v3].Z);
                            Vector3 normal = VectorFuncs.CalcNormal(vec_1, vec_2, vec_3);
                            vtx_stack.Add(new Vertex(vec_1, normal, Color.FromArgb(g.VData[v1].R, g.VData[v1].G, g.VData[v1].B)));
                            vtx_stack.Add(new Vertex(vec_2, normal, Color.FromArgb(g.VData[v2].R, g.VData[v2].G, g.VData[v2].B)));
                            vtx_stack.Add(new Vertex(vec_3, normal, Color.FromArgb(g.VData[v3].R, g.VData[v3].G, g.VData[v3].B)));
                        }
                    }
                }
            }
            vtx[0].Vtx = vtx_stack.ToArray();
            //zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
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
