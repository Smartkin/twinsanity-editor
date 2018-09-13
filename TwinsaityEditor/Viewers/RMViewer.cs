using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public class RMViewer : ThreeDViewer
    {
        private int dlist_col = -1, dlist_trg = -1;
        private ColData data;
        private Color[] colors = new[] { Color.Gray, Color.Green, Color.Red, Color.DarkBlue, Color.Yellow, Color.Pink, Color.DarkCyan, Color.DarkGreen, Color.DarkRed, Color.Brown, Color.DarkMagenta, Color.Orange, Color.DarkSeaGreen, Color.Bisque, Color.Coral };
        private bool show_trigger;
        private TwinsFile file;

        private readonly float indicator_size = 0.5f;

        public RMViewer(ColData data, ref TwinsFile file)
        {
            //initialize variables here
            dlist_col = dlist_trg = -1;
            show_trigger = false;
            this.data = data;
            this.file = file;
        }

        protected override void RenderObjects()
        {
            //put all object rendering code here
            if (dlist_col == -1) //if collision tree display list is non-existant
            {
                dlist_col = GL.GenLists(1);
                GL.NewList(dlist_col, ListMode.CompileAndExecute);
                GL.Begin(PrimitiveType.Triangles);
                foreach (var tri in data.Tris)
                {
                    GL.Color3(colors[tri.Surface % colors.Length]);
                    Pos v1 = data.Vertices[tri.Vert1], v2 = data.Vertices[tri.Vert2], v3 = data.Vertices[tri.Vert3];
                    GL.Vertex3(-v1.X, v1.Y, v1.Z);
                    GL.Vertex3(-v2.X, v2.Y, v2.Z);
                    GL.Vertex3(-v3.X, v3.Y, v3.Z);
                    /*GL.Normal3(TwinsanityEditorForm.CalcNormal(new Vector3(data.Vertices[tri.Vert1].X, data.Vertices[tri.Vert1].Y, data.Vertices[tri.Vert1].Z),
                        new Vector3(data.Vertices[tri.Vert2].X, data.Vertices[tri.Vert2].Y, data.Vertices[tri.Vert2].Z),
                        new Vector3(data.Vertices[tri.Vert3].X, data.Vertices[tri.Vert3].Y, data.Vertices[tri.Vert3].Z)));*/
                }
                GL.End();
                GL.Color3(Color.Black);
                foreach (var tri in data.Tris)
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    Pos v1 = data.Vertices[tri.Vert1], v2 = data.Vertices[tri.Vert2], v3 = data.Vertices[tri.Vert3];
                    GL.Vertex3(-v1.X, v1.Y, v1.Z);
                    GL.Vertex3(-v2.X, v2.Y, v2.Z);
                    GL.Vertex3(-v3.X, v3.Y, v3.Z);
                    GL.End();
                }
                GL.EndList();
            }
            else
                GL.CallList(dlist_col);
            if (show_trigger)
            {
                if (dlist_trg == -1)
                {
                    dlist_trg = GL.GenLists(1);
                    GL.NewList(dlist_trg, ListMode.CompileAndExecute);
                    foreach (var i in data.Triggers)
                    {
                        if (i.Flag1 == i.Flag2 && i.Flag1 < 0)
                            GL.Color3(Color.Cyan);
                        else
                            GL.Color3(Color.Red);
                        GL.Begin(PrimitiveType.LineStrip);
                        GL.Vertex3(-i.X1, i.Y1, i.Z1);
                        GL.Vertex3(-i.X2, i.Y1, i.Z1);
                        GL.Vertex3(-i.X2, i.Y2, i.Z1);
                        GL.Vertex3(-i.X1, i.Y2, i.Z1);
                        GL.Vertex3(-i.X1, i.Y1, i.Z1);
                        GL.Vertex3(-i.X1, i.Y1, i.Z2);
                        GL.Vertex3(-i.X2, i.Y1, i.Z2);
                        GL.Vertex3(-i.X2, i.Y1, i.Z1);
                        GL.End();
                        GL.Begin(PrimitiveType.LineStrip);
                        GL.Vertex3(-i.X1, i.Y1, i.Z2);
                        GL.Vertex3(-i.X1, i.Y2, i.Z2);
                        GL.Vertex3(-i.X2, i.Y2, i.Z2);
                        GL.Vertex3(-i.X2, i.Y1, i.Z2);
                        GL.End();
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(-i.X1, i.Y2, i.Z2);
                        GL.Vertex3(-i.X1, i.Y2, i.Z1);
                        GL.Vertex3(-i.X2, i.Y2, i.Z2);
                        GL.Vertex3(-i.X2, i.Y2, i.Z1);
                        GL.End();
                    }
                    GL.EndList();
                }
                else
                    GL.CallList(dlist_trg);
            }

            for (uint i = 0; i <= 7; ++i)
            {
                if (file.SecInfo.Records.ContainsKey(i))
                {
                    if (((TwinsSection)file.SecInfo.Records[i]).SecInfo.Records.ContainsKey(6))
                    {
                        foreach (Instance j in ((TwinsSection)((TwinsSection)file.SecInfo.Records[i]).SecInfo.Records[6]).SecInfo.Records.Values)
                        {
                            GL.PushMatrix();
                            GL.Translate(-j.Pos.X, j.Pos.Y + 0.5, j.Pos.Z);
                            GL.Rotate(-j.RotX / (float)ushort.MaxValue * 360f, 1, 0, 0);
                            GL.Rotate(-j.RotY / (float)ushort.MaxValue * 360f, 0, 1, 0);
                            GL.Rotate(-j.RotZ / (float)ushort.MaxValue * 360f, 0, 0, 1);
                            GL.Begin(PrimitiveType.Lines);
                            GL.Color3(1f, 0f, 0f);
                            float new_ind_size = indicator_size / 2;
                            GL.Vertex3(+new_ind_size, 0, 0);
                            GL.Vertex3(-new_ind_size, 0, 0);
                            GL.Color3(0f, 1f, 0f);
                            GL.Vertex3(0, +new_ind_size, 0);
                            GL.Vertex3(0, -new_ind_size, 0);
                            GL.Color3(0f, 0f, 1f);
                            GL.Vertex3(0, 0, +new_ind_size);
                            GL.Vertex3(0, 0, -new_ind_size);
                            GL.End();
                            GL.Color3(colors[colors.Length - i - 1]);
                            GL.Begin(PrimitiveType.LineStrip);
                            GL.Vertex3(-indicator_size, -indicator_size, -indicator_size);
                            GL.Vertex3(+indicator_size, -indicator_size, -indicator_size);
                            GL.Vertex3(+indicator_size, +indicator_size, -indicator_size);
                            GL.Vertex3(-indicator_size, +indicator_size, -indicator_size);
                            GL.Vertex3(-indicator_size, -indicator_size, -indicator_size);
                            GL.Vertex3(-indicator_size, -indicator_size, +indicator_size);
                            GL.Vertex3(+indicator_size, -indicator_size, +indicator_size);
                            GL.Vertex3(+indicator_size, -indicator_size, -indicator_size);
                            GL.End();
                            GL.Begin(PrimitiveType.LineStrip);
                            GL.Vertex3(-indicator_size, -indicator_size, +indicator_size);
                            GL.Vertex3(-indicator_size, +indicator_size, +indicator_size);
                            GL.Vertex3(+indicator_size, +indicator_size, +indicator_size);
                            GL.Vertex3(+indicator_size, -indicator_size, +indicator_size);
                            GL.End();
                            GL.Begin(PrimitiveType.Lines);
                            GL.Vertex3(-indicator_size, +indicator_size, +indicator_size);
                            GL.Vertex3(-indicator_size, +indicator_size, -indicator_size);
                            GL.Vertex3(+indicator_size, +indicator_size, +indicator_size);
                            GL.Vertex3(+indicator_size, +indicator_size, -indicator_size);
                            GL.End();
                            GL.PopMatrix();
                        }
                    }
                }
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            base.IsInputKey(keyData);
            switch (keyData)
            {
                case Keys.T:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.T:
                    show_trigger = !show_trigger;
                    break;
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (dlist_col != -1)
            {
                GL.DeleteLists(dlist_col, 1);
                dlist_col = -1;
            }
            if (dlist_trg != -1)
            {
                GL.DeleteLists(dlist_trg, 1);
                dlist_trg = -1;
            }
            base.Dispose(disposing);
        }
    }
}
