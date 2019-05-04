using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public class RMViewer : ThreeDViewer
    {
        private bool show_col_nodes, show_triggers;
        private FileController file;
        private int[] inst_vtx_counts, inst_vtx_offs;
        private int[] coln_vtx_counts, coln_vtx_offs;

        private readonly float indicator_size = 0.5f;

        public RMViewer(FileController file)
        {
            //initialize variables here
            show_col_nodes = show_triggers = false;
            this.file = file;
            vbo_count = 3;
            vtx = new Vertex[vbo_count][];
            if (file.Data.RecordIDs.ContainsKey(9))
            {
                LoadColTree();
                LoadColNodes();
            }
            LoadInstances();
        }

        protected override void RenderHUD()
        {
            GL.Color3(Color.White);
            RenderString2D($"RenderObjects: {timeRenderObj}ms (max: {timeRenderObj_max}ms, min: {timeRenderObj_min}ms)", 0, 0, 10f);
        }

        protected override void RenderObjects()
        {
            //put all object rendering code here
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.NormalArray);
            //draw collision
            if (file.Data.RecordIDs.ContainsKey(9))
            {
                GL.PushMatrix();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_id[0]);
                GL.VertexPointer(3, VertexPointerType.Float, Marshal.SizeOf(typeof(Vertex)), Marshal.OffsetOf(typeof(Vertex), "pos"));
                GL.ColorPointer(4, ColorPointerType.UnsignedByte, Marshal.SizeOf(typeof(Vertex)), Marshal.OffsetOf(typeof(Vertex), "col"));
                GL.NormalPointer(NormalPointerType.Float, Marshal.SizeOf(typeof(Vertex)), Marshal.OffsetOf(typeof(Vertex), "nor"));
                GL.DrawArrays(PrimitiveType.Triangles, 0, vtx[0].Length);
                GL.PopMatrix();

                if (show_col_nodes)
                {
                    GL.Disable(EnableCap.Lighting);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_id[2]);
                    GL.VertexPointer(3, VertexPointerType.Float, Marshal.SizeOf(typeof(Vertex)), Marshal.OffsetOf(typeof(Vertex), "pos"));
                    GL.ColorPointer(4, ColorPointerType.UnsignedByte, Marshal.SizeOf(typeof(Vertex)), Marshal.OffsetOf(typeof(Vertex), "col"));
                    GL.NormalPointer(NormalPointerType.Float, Marshal.SizeOf(typeof(Vertex)), Marshal.OffsetOf(typeof(Vertex), "nor"));
                    GL.MultiDrawArrays(PrimitiveType.LineStrip, coln_vtx_offs, coln_vtx_counts, coln_vtx_offs.Length);
                    GL.Enable(EnableCap.Lighting);
                }
            }

            GL.Disable(EnableCap.Lighting);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_id[1]);
            GL.VertexPointer(3, VertexPointerType.Float, Marshal.SizeOf(typeof(Vertex)), Marshal.OffsetOf(typeof(Vertex), "pos"));
            GL.ColorPointer(4, ColorPointerType.UnsignedByte, Marshal.SizeOf(typeof(Vertex)), Marshal.OffsetOf(typeof(Vertex), "col"));
            GL.NormalPointer(NormalPointerType.Float, Marshal.SizeOf(typeof(Vertex)), Marshal.OffsetOf(typeof(Vertex), "nor"));
            GL.MultiDrawArrays(PrimitiveType.LineStrip, inst_vtx_offs, inst_vtx_counts, inst_vtx_offs.Length);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.ColorArray);
            GL.DisableClientState(ArrayCap.NormalArray);
            //Immediate mode drawing BEGIN - note that this is slower than using a buffer!
            GL.PushMatrix();

            GL.Scale(-1, 1, 1);
            for (uint i = 0; i <= 7; ++i)
            {
                if (file.Data.RecordIDs.ContainsKey(i))
                {
                    if (((TwinsSection)file.Data.GetItem(i)).RecordIDs.ContainsKey(3)) //positions
                    {
                        foreach (Position pos in ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(3)).Records)
                        {
                            GL.PushMatrix();
                            GL.Translate(pos.Pos.X, pos.Pos.Y, pos.Pos.Z);
                            DrawAxes(0, 0, 0, 0.5f);
                            if (file.SelectedItem != pos)
                            {
                                GL.PointSize(5);
                                GL.Color3(colors[colors.Length - i * 2 - 2]);
                            }
                            else
                            {
                                GL.PointSize(10);
                                GL.Color3(Color.White);
                            }
                            GL.Begin(PrimitiveType.Points);
                            GL.Vertex3(0, 0, 0);
                            GL.End();
                            GL.Scale(0.5, 0.5, 0.5);
                            RenderString(pos.ID.ToString());
                            GL.PopMatrix();
                        }
                    }

                    if (((TwinsSection)file.Data.GetItem(i)).RecordIDs.ContainsKey(4)) //paths
                    {
                        foreach (Path pth in ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(4)).Records)
                        {
                            for (int k = 0; k < pth.Positions.Count; ++k)
                            {
                                GL.PushMatrix();
                                GL.Translate(pth.Positions[k].X, pth.Positions[k].Y, pth.Positions[k].Z);
                                DrawAxes(0, 0, 0, 0.5f);
                                GL.Scale(0.5, 0.5, 0.5);
                                if (file.SelectedItem != pth)
                                    GL.Color3(colors[colors.Length - i * 2 - 2]);
                                else
                                    GL.Color3(Color.White);
                                RenderString(pth.ID.ToString()+":"+k);
                                GL.PopMatrix();
                            }
                            if (file.SelectedItem != pth)
                            {
                                GL.PointSize(5);
                                GL.LineWidth(1);
                            }
                            else
                            {
                                GL.PointSize(10);
                                GL.LineWidth(2);
                            }
                            GL.Begin(PrimitiveType.LineStrip);
                            for (int k = 0; k < pth.Positions.Count; ++k)
                            {
                                if (file.SelectedItem != pth)
                                    GL.Color3(colors[colors.Length - i * 2 - 2]);
                                else
                                    GL.Color3(Color.White);
                                GL.Vertex3(pth.Positions[k].X, pth.Positions[k].Y, pth.Positions[k].Z);
                            }
                            GL.End();
                        }
                    }

                    if (((TwinsSection)file.Data.GetItem(i)).RecordIDs.ContainsKey(6)) //instances
                    {
                        foreach (Instance ins in ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(6)).Records)
                        {
                            GL.PushMatrix();
                            GL.Scale(-1, 1, 1);
                            GL.Translate(-ins.Pos.X, ins.Pos.Y, ins.Pos.Z);
                            //DrawAxes(0, 0, 0, 0.5f);
                            Matrix4 rot_ins = Matrix4.Identity;
                            rot_ins *= Matrix4.CreateRotationX(ins.RotX / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi);
                            rot_ins *= Matrix4.CreateRotationY(-ins.RotY / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi);
                            rot_ins *= Matrix4.CreateRotationZ(-ins.RotZ / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi);
                            GL.MultMatrix(ref rot_ins);
                            if (file.SelectedItem == ins)
                            {
                                GL.Color3(Color.White);
                                //GL.LineWidth(2);
                            }
                            else
                            {
                                GL.Color3(colors[colors.Length - i * 2 - 1]);
                                //GL.LineWidth(1);
                            }
                            RenderString(ins.ID.ToString());
                            GL.PopMatrix();
                        }
                    }
                }
            }
            GL.Enable(EnableCap.Lighting);

            //Draw triggers (transparent surfaces)
            for (uint i = 0; i <= 7; ++i)
            {
                if (file.Data.RecordIDs.ContainsKey(i))
                {
                    if (((TwinsSection)file.Data.GetItem(i)).RecordIDs.ContainsKey(7) && show_triggers)
                    {
                        foreach (Trigger trg in ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(7)).Records)
                        {
                            GL.PushMatrix();
                            GL.Translate(trg.Coords[1].X, trg.Coords[1].Y, trg.Coords[1].Z);

                            GL.Begin(PrimitiveType.Quads);
                            GL.Color4(colors[colors.Length - i - 1].R, colors[colors.Length - i - 1].G, colors[colors.Length - i - 1].B, (byte)128);

                            GL.Vertex3(-trg.Coords[2].X, -trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, -trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, trg.Coords[2].Y, -trg.Coords[2].Z);

                            GL.Vertex3(-trg.Coords[2].X, -trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, -trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, trg.Coords[2].Y, trg.Coords[2].Z);

                            GL.Vertex3(-trg.Coords[2].X, -trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, -trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, trg.Coords[2].Y, -trg.Coords[2].Z);

                            GL.Vertex3(trg.Coords[2].X, -trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, -trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, trg.Coords[2].Y, -trg.Coords[2].Z);

                            GL.Vertex3(-trg.Coords[2].X, -trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, -trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, -trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, -trg.Coords[2].Y, trg.Coords[2].Z);

                            GL.Vertex3(-trg.Coords[2].X, trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, trg.Coords[2].Y, trg.Coords[2].Z);

                            GL.End();

                            GL.Disable(EnableCap.Lighting);
                            GL.Disable(EnableCap.DepthTest);
                            GL.LineWidth(2);
                            GL.Begin(PrimitiveType.Lines);
                            for (int k = 0; k < trg.Instances.Length; ++k)
                            {
                                Instance inst = file.GetInstance(trg.Parent.Parent.ID, trg.Instances[k]);
                                GL.Vertex3(0, 0, 0);
                                GL.Vertex3(inst.Pos.X - trg.Coords[1].X, inst.Pos.Y - trg.Coords[1].Y, inst.Pos.Z - trg.Coords[1].Z);
                            }
                            GL.End();
                            GL.LineWidth(1);

                            GL.Begin(PrimitiveType.LineLoop);
                            GL.Vertex3(-trg.Coords[2].X, -trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, -trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.End();

                            GL.Begin(PrimitiveType.LineLoop);
                            GL.Vertex3(-trg.Coords[2].X, -trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, -trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.End();

                            GL.Begin(PrimitiveType.LineLoop);
                            GL.Vertex3(-trg.Coords[2].X, -trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, -trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.End();

                            GL.Begin(PrimitiveType.LineLoop);
                            GL.Vertex3(trg.Coords[2].X, -trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, -trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.End();

                            GL.Begin(PrimitiveType.LineLoop);
                            GL.Vertex3(-trg.Coords[2].X, -trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, -trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, -trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, -trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.End();

                            GL.Begin(PrimitiveType.LineLoop);
                            GL.Vertex3(-trg.Coords[2].X, trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.End();
                            
                            DrawAxes(0, 0, 0, Math.Min(trg.Coords[2].X, Math.Min(trg.Coords[2].Y, trg.Coords[2].Z)) / 2);
                            GL.Enable(EnableCap.DepthTest);
                            GL.Enable(EnableCap.Lighting);

                            GL.PopMatrix();
                        }
                    }
                }
            }

            GL.PopMatrix();
            //Immediate mode drawing END
        }

        protected override bool IsInputKey(Keys keyData)
        {
            base.IsInputKey(keyData);
            switch (keyData)
            {
                case Keys.C:
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
                case Keys.C:
                    show_col_nodes = !show_col_nodes;
                    break;
                case Keys.T:
                    show_triggers = !show_triggers;
                    break;
            }
        }

        private void DrawAxes(float x, float y, float z, float size)
        {
            float new_ind_size = indicator_size * size;
            GL.PushMatrix();
            GL.Translate(x, y, z);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(1f, 0f, 0f);
            GL.Vertex3(+new_ind_size, 0, 0);
            GL.Vertex3(-new_ind_size/2, 0, 0);
            GL.Color3(0f, 1f, 0f);
            GL.Vertex3(0, +new_ind_size, 0);
            GL.Vertex3(0, -new_ind_size/2, 0);
            GL.Color3(0f, 0f, 1f);
            GL.Vertex3(0, 0, +new_ind_size);
            GL.Vertex3(0, 0, -new_ind_size/2);
            GL.End();
            GL.PopMatrix();
        }

        public void LoadColTree()
        {
            ColData data = (ColData)file.Data.GetItem(9);
            vtx[0] = new Vertex[data.Tris.Count * 3];
            for (int i = 0; i < data.Tris.Count; ++i)
            {
                Vector3 v1 = data.Vertices[data.Tris[i].Vert1].ToVec3();
                Vector3 v2 = data.Vertices[data.Tris[i].Vert2].ToVec3();
                Vector3 v3 = data.Vertices[data.Tris[i].Vert3].ToVec3();
                v1.X = -v1.X;
                v2.X = -v2.X;
                v3.X = -v3.X;
                Vector3 nor = VectorFuncs.CalcNormal(v1, v2, v3);
                vtx[0][i * 3 + 0] = new Vertex(v1, nor, colors[data.Tris[i].Surface % colors.Length]);
                vtx[0][i * 3 + 1] = new Vertex(v2, nor, colors[data.Tris[i].Surface % colors.Length]);
                vtx[0][i * 3 + 2] = new Vertex(v3, nor, colors[data.Tris[i].Surface % colors.Length]);
            }
            if (vbo_id != null)
            {
                UpdateVBO(0);
            }
        }

        public void LoadInstances()
        {
            bool[] record_exists = new bool[8];
            int inst_count = 0;
            for (uint i = 0; i <= 7; ++i)
            {
                record_exists[i] = file.Data.RecordIDs.ContainsKey(i);
                if (record_exists[i])
                {
                    if (((TwinsSection)file.Data.GetItem(i)).RecordIDs.ContainsKey(6))
                        inst_count += ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(6)).Records.Count;
                    else record_exists[i] = false;
                }
            }
            if (vtx[1] == null || vtx.Length != 22 * inst_count)
            {
                inst_vtx_counts = new int[7 * inst_count];
                inst_vtx_offs = new int[7 * inst_count];
                vtx[1] = new Vertex[22 * inst_count];
                for (int i = 0; i < inst_count; ++i)
                {
                    inst_vtx_counts[i * 7 + 0] = 2;
                    inst_vtx_counts[i * 7 + 1] = 2;
                    inst_vtx_counts[i * 7 + 2] = 2;
                    inst_vtx_counts[i * 7 + 3] = 8;
                    inst_vtx_counts[i * 7 + 4] = 4;
                    inst_vtx_counts[i * 7 + 5] = 2;
                    inst_vtx_counts[i * 7 + 6] = 2;
                }
            }
            int l = 0, m = 0;
            for (uint i = 0; i <= 7; ++i)
            {
                if (!record_exists[i]) continue;
                if (((TwinsSection)file.Data.GetItem(i)).RecordIDs.ContainsKey(6))
                {
                    foreach (Instance ins in ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(6)).Records)
                    {
                        Matrix3 rot_ins = Matrix3.Identity;
                        rot_ins *= Matrix3.CreateRotationX(ins.RotX / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi);
                        rot_ins *= Matrix3.CreateRotationY(-ins.RotY / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi);
                        rot_ins *= Matrix3.CreateRotationZ(-ins.RotZ / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi);
                        Vector3 pos_ins = ins.Pos.ToVec3();
                        pos_ins.X = -pos_ins.X;
                        inst_vtx_offs[l++] = m;
                        vtx[1][m++] = new Vertex(new Vector3(-indicator_size * 0.75f, 0, 0) + pos_ins, new Vector3(), Color.Red);
                        vtx[1][m++] = new Vertex(new Vector3(+indicator_size * 0.375f, 0, 0) + pos_ins, new Vector3(), Color.Red);
                        inst_vtx_offs[l++] = m;
                        vtx[1][m++] = new Vertex(new Vector3(0, indicator_size * 0.75f, 0) + pos_ins, new Vector3(), Color.Green);
                        vtx[1][m++] = new Vertex(new Vector3(0, -indicator_size * 0.375f, 0) + pos_ins, new Vector3(), Color.Green);
                        inst_vtx_offs[l++] = m;
                        vtx[1][m++] = new Vertex(new Vector3(0, 0, indicator_size * 0.75f) + pos_ins, new Vector3(), Color.Blue);
                        vtx[1][m++] = new Vertex(new Vector3(0, 0, -indicator_size * 0.375f) + pos_ins, new Vector3(), Color.Blue);
                        inst_vtx_offs[l++] = m;
                        Color cur_color = (file.SelectedItem == ins) ? Color.White : colors[colors.Length - i * 2 - 1];
                        vtx[1][m++] = new Vertex(new Vector3(-indicator_size, -indicator_size + 0.5f, -indicator_size) * rot_ins + pos_ins, new Vector3(), cur_color);
                        vtx[1][m++] = new Vertex(new Vector3(+indicator_size, -indicator_size + 0.5f, -indicator_size) * rot_ins + pos_ins, new Vector3(), cur_color);
                        vtx[1][m++] = new Vertex(new Vector3(+indicator_size, +indicator_size + 0.5f, -indicator_size) * rot_ins + pos_ins, new Vector3(), cur_color);
                        vtx[1][m++] = new Vertex(new Vector3(-indicator_size, +indicator_size + 0.5f, -indicator_size) * rot_ins + pos_ins, new Vector3(), cur_color);
                        vtx[1][m++] = new Vertex(new Vector3(-indicator_size, -indicator_size + 0.5f, -indicator_size) * rot_ins + pos_ins, new Vector3(), cur_color);
                        vtx[1][m++] = new Vertex(new Vector3(-indicator_size, -indicator_size + 0.5f, +indicator_size) * rot_ins + pos_ins, new Vector3(), cur_color);
                        vtx[1][m++] = new Vertex(new Vector3(+indicator_size, -indicator_size + 0.5f, +indicator_size) * rot_ins + pos_ins, new Vector3(), cur_color);
                        vtx[1][m++] = new Vertex(new Vector3(+indicator_size, -indicator_size + 0.5f, -indicator_size) * rot_ins + pos_ins, new Vector3(), cur_color);
                        inst_vtx_offs[l++] = m;
                        vtx[1][m++] = new Vertex(new Vector3(-indicator_size, -indicator_size + 0.5f, +indicator_size) * rot_ins + pos_ins, new Vector3(), cur_color);
                        vtx[1][m++] = new Vertex(new Vector3(-indicator_size, +indicator_size + 0.5f, +indicator_size) * rot_ins + pos_ins, new Vector3(), cur_color);
                        vtx[1][m++] = new Vertex(new Vector3(+indicator_size, +indicator_size + 0.5f, +indicator_size) * rot_ins + pos_ins, new Vector3(), cur_color);
                        vtx[1][m++] = new Vertex(new Vector3(+indicator_size, -indicator_size + 0.5f, +indicator_size) * rot_ins + pos_ins, new Vector3(), cur_color);
                        inst_vtx_offs[l++] = m;
                        vtx[1][m++] = new Vertex(new Vector3(-indicator_size, +indicator_size + 0.5f, +indicator_size) * rot_ins + pos_ins, new Vector3(), cur_color);
                        vtx[1][m++] = new Vertex(new Vector3(-indicator_size, +indicator_size + 0.5f, -indicator_size) * rot_ins + pos_ins, new Vector3(), cur_color);
                        inst_vtx_offs[l++] = m;
                        vtx[1][m++] = new Vertex(new Vector3(+indicator_size, +indicator_size + 0.5f, +indicator_size) * rot_ins + pos_ins, new Vector3(), cur_color);
                        vtx[1][m++] = new Vertex(new Vector3(+indicator_size, +indicator_size + 0.5f, -indicator_size) * rot_ins + pos_ins, new Vector3(), cur_color);
                    }
                }
            }
            if (vbo_id != null)
            {
                UpdateVBO(1);
            }
        }

        public void LoadColNodes()
        {
            ColData data = (ColData)file.Data.GetItem(9);
            vtx[2] = new Vertex[data.Triggers.Count * 16];
            coln_vtx_counts = new int[4 * data.Triggers.Count];
            coln_vtx_offs = new int[4 * data.Triggers.Count];
            for (int i = 0; i < data.Triggers.Count; ++i)
            {
                coln_vtx_counts[i * 4 + 0] = 8;
                coln_vtx_counts[i * 4 + 1] = 4;
                coln_vtx_counts[i * 4 + 2] = 2;
                coln_vtx_counts[i * 4 + 3] = 2;
            }
            int l = 0, m = 0;
            foreach (var i in data.Triggers)
            {
                Color cur_color = (i.Flag1 == i.Flag2 && i.Flag1 < 0) ? Color.Cyan : Color.Red;
                coln_vtx_offs[l++] = m;
                vtx[2][m++] = new Vertex(new Vector3(-i.X1, i.Y1, i.Z1), new Vector3(), cur_color);
                vtx[2][m++] = new Vertex(new Vector3(-i.X2, i.Y1, i.Z1), new Vector3(), cur_color);
                vtx[2][m++] = new Vertex(new Vector3(-i.X2, i.Y2, i.Z1), new Vector3(), cur_color);
                vtx[2][m++] = new Vertex(new Vector3(-i.X1, i.Y2, i.Z1), new Vector3(), cur_color);
                vtx[2][m++] = new Vertex(new Vector3(-i.X1, i.Y1, i.Z1), new Vector3(), cur_color);
                vtx[2][m++] = new Vertex(new Vector3(-i.X1, i.Y1, i.Z2), new Vector3(), cur_color);
                vtx[2][m++] = new Vertex(new Vector3(-i.X2, i.Y1, i.Z2), new Vector3(), cur_color);
                vtx[2][m++] = new Vertex(new Vector3(-i.X2, i.Y1, i.Z1), new Vector3(), cur_color);
                coln_vtx_offs[l++] = m;
                vtx[2][m++] = new Vertex(new Vector3(-i.X1, i.Y1, i.Z2), new Vector3(), cur_color);
                vtx[2][m++] = new Vertex(new Vector3(-i.X1, i.Y2, i.Z2), new Vector3(), cur_color);
                vtx[2][m++] = new Vertex(new Vector3(-i.X2, i.Y2, i.Z2), new Vector3(), cur_color);
                vtx[2][m++] = new Vertex(new Vector3(-i.X2, i.Y1, i.Z2), new Vector3(), cur_color);
                coln_vtx_offs[l++] = m;
                vtx[2][m++] = new Vertex(new Vector3(-i.X1, i.Y2, i.Z2), new Vector3(), cur_color);
                vtx[2][m++] = new Vertex(new Vector3(-i.X1, i.Y2, i.Z1), new Vector3(), cur_color);
                coln_vtx_offs[l++] = m;
                vtx[2][m++] = new Vertex(new Vector3(-i.X2, i.Y2, i.Z2), new Vector3(), cur_color);
                vtx[2][m++] = new Vertex(new Vector3(-i.X2, i.Y2, i.Z1), new Vector3(), cur_color);
            }
            if (vbo_id != null)
            {
                UpdateVBO(2);
            }
        }

        public void UpdateSelected()
        {
            if (file.SelectedItem == null)
            {
                if (file.SelectedItem is Instance)
                {
                    LoadInstances();
                }
                else if (file.SelectedItem is Position)
                {
                }
            }
            else if (file.SelectedItem is Instance ins)
            {
                SetPosition(new Vector3(-ins.Pos.X, ins.Pos.Y, ins.Pos.Z));
                LoadInstances();
            }
            else if (file.SelectedItem is Position pos)
            {
                SetPosition(new Vector3(-pos.Pos.X, pos.Pos.Y, pos.Pos.Z));
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
