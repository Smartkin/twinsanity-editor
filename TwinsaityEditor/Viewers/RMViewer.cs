using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using Twinsanity;

namespace TwinsaityEditor.Viewers
{
    public partial class RMViewer : ThreeDViewer
    {
        private int dlist_col = -1, dlist_trg = -1;
        private GeoData data;
        private Color[] colors = new[] { Color.Gray, Color.Green, Color.Red, Color.DarkBlue, Color.Yellow, Color.Pink, Color.DarkCyan, Color.DarkGreen, Color.DarkRed, Color.Brown, Color.DarkMagenta, Color.Orange, Color.DarkSeaGreen, Color.Bisque, Color.Coral };
        //private int displaylist;

        public RMViewer(GeoData data)
        {
            //initialize variables here
            dlist_col = dlist_trg = -1;
            this.data = data;
        }

        protected override void RenderObjects()
        {
            //put all object rendering code here
            if (dlist_col == -1) //if collision tree display list is non-existant
            {
                dlist_col = GL.GenLists(1);
                GL.NewList(dlist_col, ListMode.CompileAndExecute);
                GL.Begin(PrimitiveType.Triangles);
                foreach (GeoData.ColTri tri in data.Tris)
                {
                    GL.Color3(colors[tri.Surface % colors.Length]);
                    Pos v1 = data.Vertices[tri.Vert1], v2 = data.Vertices[tri.Vert2], v3 = data.Vertices[tri.Vert3];
                    GL.Vertex3(-v1.X, v1.Y, v1.Z);
                    GL.Vertex3(-v2.X, v2.Y, v2.Z);
                    GL.Vertex3(-v3.X, v3.Y, v3.Z);
                }
                GL.End();
                GL.EndList();
            }
            else
                GL.CallList(dlist_col);
            if (dlist_trg == -1)
            {
                dlist_trg = GL.GenLists(1);
                GL.NewList(dlist_trg, ListMode.CompileAndExecute);

                GL.EndList();
            }
            else
                GL.CallList(dlist_trg);
            //throw new NotImplementedException();
        }
    }
}
