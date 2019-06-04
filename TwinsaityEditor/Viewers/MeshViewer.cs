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
            zFar = 100F;
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
            vtx[0].DrawAllElements(PrimitiveType.Triangles, BufferPointerFlags.Normal);
            GL.Disable(EnableCap.Lighting);
        }

        public void LoadMesh()
        {
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            List<Vertex> vtx_stack = new List<Vertex>();
            List<uint> indx = new List<uint>();
            int off = 0;
            foreach (var s in mesh.Data.SubModels)
            {
                foreach (var g in s.Groups)
                {
                    if (g.VertHead > 0 && g.VDataHead > 0 && g.VertexCount >= 3)
                    {
                        vtx_stack.Add(new Vertex(new Vector3(-g.Vertex[0].X, g.Vertex[0].Y, g.Vertex[0].Z), Color.FromArgb(g.VData[0].R, g.VData[0].G, g.VData[0].B)));
                        vtx_stack.Add(new Vertex(new Vector3(-g.Vertex[1].X, g.Vertex[1].Y, g.Vertex[1].Z), Color.FromArgb(g.VData[1].R, g.VData[1].G, g.VData[1].B)));
                        min_x = Math.Min(min_x, Math.Min(vtx_stack[0].Pos.X, vtx_stack[1].Pos.X));
                        min_y = Math.Min(min_y, Math.Min(vtx_stack[0].Pos.Y, vtx_stack[1].Pos.Y));
                        min_z = Math.Min(min_z, Math.Min(vtx_stack[0].Pos.Z, vtx_stack[1].Pos.Z));
                        max_x = Math.Max(max_x, Math.Max(vtx_stack[0].Pos.X, vtx_stack[1].Pos.X));
                        max_y = Math.Max(max_y, Math.Max(vtx_stack[0].Pos.Y, vtx_stack[1].Pos.Y));
                        max_z = Math.Max(max_z, Math.Max(vtx_stack[0].Pos.Z, vtx_stack[1].Pos.Z));
                        for (int i = 2; i < g.VertexCount; ++i)
                        {
                            vtx_stack.Add(new Vertex(new Vector3(-g.Vertex[i].X, g.Vertex[i].Y, g.Vertex[i].Z), Color.FromArgb(g.VData[i].R, g.VData[i].G, g.VData[i].B)));
                            int v1 = off + i - 2 + (i & 1);
                            int v2 = off + i - 1 - (i & 1);
                            int v3 = off + i;
                            Vector3 normal = VectorFuncs.CalcNormal(vtx_stack[v1].Pos, vtx_stack[v2].Pos, vtx_stack[v3].Pos);
                            var v = vtx_stack[v1];
                            v.Nor += normal;
                            vtx_stack[v1] = v;
                            v = vtx_stack[v2];
                            v.Nor += normal;
                            vtx_stack[v2] = v;
                            v = vtx_stack[v3];
                            v.Nor += normal;
                            vtx_stack[v3] = v;
                            if (g.VData[i].CONN == 128) continue;
                            indx.Add((uint)v1);
                            indx.Add((uint)v2);
                            indx.Add((uint)v3);
                            min_x = Math.Min(min_x, vtx_stack[v3].Pos.X);
                            min_y = Math.Min(min_y, vtx_stack[v3].Pos.Y);
                            min_z = Math.Min(min_z, vtx_stack[v3].Pos.Z);
                            max_x = Math.Max(max_x, vtx_stack[v3].Pos.X);
                            max_y = Math.Max(max_y, vtx_stack[v3].Pos.Y);
                            max_z = Math.Max(max_z, vtx_stack[v3].Pos.Z);
                        }
                        off += g.VertexCount;
                    }
                }
            }
            //sort out duplicates
            for (int i = 0; i < vtx_stack.Count; ++i)
            {
                Vector3 pos = vtx_stack[i].Pos;
                uint col = vtx_stack[i].Col;
                for (int j = i+1; j < vtx_stack.Count; ++j)
                {
                    if (vtx_stack[j].Col == col && vtx_stack[j].Pos == pos)
                    {
                        for (int k = 0; k < indx.Count; ++k)
                        {
                            if (indx[k] == j)
                                indx[k] = (uint)i;
                            else if (indx[k] > j)
                                indx[k]--;
                        }
                        var v = vtx_stack[i];
                        v.Nor += vtx_stack[j].Nor;
                        vtx_stack[i] = v;
                        vtx_stack.RemoveAt(j);
                    }
                }
            }
            vtx[0].Vtx = vtx_stack.ToArray();
            vtx[0].VtxInd = indx.ToArray();
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
