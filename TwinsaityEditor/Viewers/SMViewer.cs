using OpenTK;
using OpenTK.Graphics.OpenGL;
using Twinsanity;
using System.Drawing;
using System;

namespace TwinsaityEditor
{
    public partial class SMViewer : ThreeDViewer
    {
        private int displaylist;
        //private List<Mesh> data;
        private TwinsFile file;

        public SMViewer(ref TwinsFile file)//Mesh data)
        {
            //initialize variables here
            displaylist = -1;
            //this.data = new List<Mesh> { data };
            this.file = file;
        }

        protected override void RenderHUD()
        {
            return;
        }

        protected override void RenderObjects()
        {
            //put all object rendering code here
            GL.PushMatrix();
            GL.Scale(-1, 1, 1);
            if (displaylist == -1) //if model(s) display list is non-existant
            {
                displaylist = GL.GenLists(1);
                GL.NewList(displaylist, ListMode.CompileAndExecute);
                GL.Begin(PrimitiveType.Triangles);
                //foreach (Mesh mdl in data)
                {

                }
                GL.End();
                GL.EndList();
            }
            else
                GL.CallList(displaylist);

            if (file.ContainsItem(5))
            {
                ChunkLinks links = (ChunkLinks)file.GetItem(5);
                GL.Disable(EnableCap.Lighting);
                GL.Begin(PrimitiveType.Quads);
                for (int i = 0; i < links.Links.Count; ++i)
                {
                    if ((links.Links[i].Flags & 0x80000) != 0)
                    {
                        GL.Color3(colors[i % colors.Length]);
                        GL.Vertex4(links.Links[i].LoadWall[0].X, links.Links[i].LoadWall[0].Y, links.Links[i].LoadWall[0].Z, links.Links[i].LoadWall[0].W);
                        GL.Vertex4(links.Links[i].LoadWall[1].X, links.Links[i].LoadWall[1].Y, links.Links[i].LoadWall[1].Z, links.Links[i].LoadWall[1].W);
                        GL.Vertex4(links.Links[i].LoadWall[2].X, links.Links[i].LoadWall[2].Y, links.Links[i].LoadWall[2].Z, links.Links[i].LoadWall[2].W);
                        GL.Vertex4(links.Links[i].LoadWall[3].X, links.Links[i].LoadWall[3].Y, links.Links[i].LoadWall[3].Z, links.Links[i].LoadWall[3].W);
                    }
                }
                GL.End();
                for (int i = 0; i < links.Links.Count; ++i)
                {
                    if (links.Links[i].Type == 1 || links.Links[i].Type == 3)
                    {
                        Vector4 v1 = VectorFuncs.Pos2Vector4(links.Links[i].LoadArea[0]);
                        Vector4 v2 = VectorFuncs.Pos2Vector4(links.Links[i].LoadArea[1]);
                        Vector4 v3 = VectorFuncs.Pos2Vector4(links.Links[i].LoadArea[2]);
                        Vector4 v4 = VectorFuncs.Pos2Vector4(links.Links[i].LoadArea[3]);
                        Vector4 v5 = VectorFuncs.Pos2Vector4(links.Links[i].LoadArea[4]);
                        Vector4 v6 = VectorFuncs.Pos2Vector4(links.Links[i].LoadArea[5]);
                        Vector4 v7 = VectorFuncs.Pos2Vector4(links.Links[i].LoadArea[6]);
                        Vector4 v8 = VectorFuncs.Pos2Vector4(links.Links[i].LoadArea[7]);
                        GL.Color4(Color.FromArgb(0xFF, colors[i % colors.Length]));
                        GL.Begin(PrimitiveType.TriangleStrip);
                        //GL.Vertex4(v1); GL.Vertex4(v2); GL.Vertex4(v3); GL.Vertex4(v4); GL.Vertex4(v5); GL.Vertex4(v6); GL.Vertex4(v7); GL.Vertex4(v8);
                        GL.End();
                        GL.Begin(PrimitiveType.Quads);
                        //GL.Vertex4(v1); GL.Vertex4(v3); GL.Vertex4(v5); GL.Vertex4(v7);
                        //GL.Vertex4(v2); GL.Vertex4(v4); GL.Vertex4(v6); GL.Vertex4(v8);
                        //GL.Vertex4(v1); GL.Vertex4(v2); GL.Vertex4(v8); GL.Vertex4(v7);
                        GL.End();
                        float min_x = Math.Min(v1.X, Math.Min(v2.X, Math.Min(v3.X, Math.Min(v4.X, Math.Min(v5.X, Math.Min(v6.X, Math.Min(v7.X, v8.X)))))));
                        float min_y = Math.Min(v1.Y, Math.Min(v2.Y, Math.Min(v3.Y, Math.Min(v4.Y, Math.Min(v5.Y, Math.Min(v6.Y, Math.Min(v7.Y, v8.Y)))))));
                        float min_z = Math.Min(v1.Z, Math.Min(v2.Z, Math.Min(v3.Z, Math.Min(v4.Z, Math.Min(v5.Z, Math.Min(v6.Z, Math.Min(v7.Z, v8.Z)))))));
                        float max_x = Math.Max(v1.X, Math.Max(v2.X, Math.Max(v3.X, Math.Max(v4.X, Math.Max(v5.X, Math.Max(v6.X, Math.Max(v7.X, v8.X)))))));
                        float max_y = Math.Max(v1.Y, Math.Max(v2.Y, Math.Max(v3.Y, Math.Max(v4.Y, Math.Max(v5.Y, Math.Max(v6.Y, Math.Max(v7.Y, v8.Y)))))));
                        float max_z = Math.Max(v1.Z, Math.Max(v2.Z, Math.Max(v3.Z, Math.Max(v4.Z, Math.Max(v5.Z, Math.Max(v6.Z, Math.Max(v7.Z, v8.Z)))))));
                        GL.PushMatrix();
                        GL.Translate((min_x + max_x) / 2, (min_y + max_y) / 2, (min_z + max_z) / 2);
                        v1 = VectorFuncs.Pos2Vector4(links.Links[i].AreaMatrix[0]);
                        v2 = VectorFuncs.Pos2Vector4(links.Links[i].AreaMatrix[1]);
                        v3 = VectorFuncs.Pos2Vector4(links.Links[i].AreaMatrix[2]);
                        v4 = VectorFuncs.Pos2Vector4(links.Links[i].AreaMatrix[3]);
                        v5 = VectorFuncs.Pos2Vector4(links.Links[i].AreaMatrix[4]);
                        v6 = VectorFuncs.Pos2Vector4(links.Links[i].AreaMatrix[5]);
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex2(0, 0); GL.Vertex4(v1.NormalizeW());
                        GL.Vertex2(0, 0); GL.Vertex4(v2.NormalizeW());
                        GL.Vertex2(0, 0); GL.Vertex4(v3.NormalizeW());
                        GL.Vertex2(0, 0); GL.Vertex4(v4.NormalizeW());
                        GL.Vertex2(0, 0); GL.Vertex4(v5.NormalizeW());
                        GL.Vertex2(0, 0); GL.Vertex4(v6.NormalizeW());
                        GL.End();
                        GL.PopMatrix();
                    }
                }
                GL.Enable(EnableCap.Lighting);
            }
            GL.PopMatrix();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (displaylist != -1)
            {
                GL.DeleteLists(displaylist, 1);
                displaylist = -1;
            }
            base.Dispose(disposing);
        }
    }
}
