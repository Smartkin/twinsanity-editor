using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public class RMViewer : ThreeDViewer
    {
        private static readonly int circle_res = 16;

        private bool show_col_nodes, show_triggers;
        private FileController file;
        private int[] inst_vtx_counts, inst_vtx_offs;
        private int[] coln_vtx_counts, coln_vtx_offs;
        private int[] posi_vtx_counts, posi_vtx_offs;

        public RMViewer(FileController file, ref Form pform)
        {
            //initialize variables here
            show_col_nodes = show_triggers = false;
            this.file = file;
            Tag = pform;
            vbo_count = 4;
            vtx = new Vertex[vbo_count][];
            if (file.Data.RecordIDs.ContainsKey(9))
            {
                pform.Text = "Loading collision tree...";
                LoadColTree();
                pform.Text = "Loading collision nodes...";
                LoadColNodes();
            }
            pform.Text = "Loading instances...";
            LoadInstances();
            pform.Text = "Loading positions...";
            LoadPositions();
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
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.NormalArray);
            //draw collision
            if (file.Data.RecordIDs.ContainsKey(9))
            {
                GL.Enable(EnableCap.Lighting);
                GL.PushMatrix();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_id[0]);
                GL.VertexPointer(3, VertexPointerType.Float, Vertex.SizeOf, Vertex.OffsetOfPos);
                GL.ColorPointer(4, ColorPointerType.UnsignedByte, Vertex.SizeOf, Vertex.OffsetOfCol);
                GL.NormalPointer(NormalPointerType.Float, Vertex.SizeOf, Vertex.OffsetOfNor);
                GL.DrawArrays(PrimitiveType.Triangles, 0, vtx[0].Length);
                GL.PopMatrix();
                GL.Disable(EnableCap.Lighting);

                if (show_col_nodes)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_id[2]);
                    GL.VertexPointer(3, VertexPointerType.Float, Vertex.SizeOf, Vertex.OffsetOfPos);
                    GL.ColorPointer(4, ColorPointerType.UnsignedByte, Vertex.SizeOf, Vertex.OffsetOfCol);
                    GL.NormalPointer(NormalPointerType.Float, Vertex.SizeOf, Vertex.OffsetOfNor);
                    GL.MultiDrawArrays(PrimitiveType.LineStrip, coln_vtx_offs, coln_vtx_counts, coln_vtx_offs.Length);
                }
            }
            
            //instances
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_id[1]);
            GL.VertexPointer(3, VertexPointerType.Float, Vertex.SizeOf, Vertex.OffsetOfPos);
            GL.ColorPointer(4, ColorPointerType.UnsignedByte, Vertex.SizeOf, Vertex.OffsetOfCol);
            GL.NormalPointer(NormalPointerType.Float, Vertex.SizeOf, Vertex.OffsetOfNor);
            GL.MultiDrawArrays(PrimitiveType.LineStrip, inst_vtx_offs, inst_vtx_counts, inst_vtx_offs.Length);

            //positions
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_id[3]);
            GL.VertexPointer(3, VertexPointerType.Float, Vertex.SizeOf, Vertex.OffsetOfPos);
            GL.ColorPointer(4, ColorPointerType.UnsignedByte, Vertex.SizeOf, Vertex.OffsetOfCol);
            GL.NormalPointer(NormalPointerType.Float, Vertex.SizeOf, Vertex.OffsetOfNor);
            GL.MultiDrawArrays(PrimitiveType.LineLoop, posi_vtx_offs, posi_vtx_counts, posi_vtx_offs.Length);

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
                    if (((TwinsSection)file.Data.GetItem(i)).RecordIDs.ContainsKey(1)) //aipositions
                    {
                        foreach (AIPosition pos in ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(1)).Records)
                        {
                            GL.PushMatrix();
                            GL.Translate(pos.Pos.X, pos.Pos.Y, pos.Pos.Z);
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
                            GL.Scale(pos.Pos.W, pos.Pos.W, pos.Pos.W);
                            RenderString(pos.ID.ToString());
                            GL.PopMatrix();
                        }
                    }

                    if (((TwinsSection)file.Data.GetItem(i)).RecordIDs.ContainsKey(2)) //aipaths
                    {
                        foreach (AIPath pth in ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(2)).Records)
                        {
                            AIPosition pth_begin = file.GetAIPos(i, pth.Arg[0]);
                            AIPosition pth_end = file.GetAIPos(i, pth.Arg[1]);

                            if (file.SelectedItem != pth)
                            {
                                GL.PointSize(5);
                                GL.LineWidth(1);
                                GL.Color3(colors[colors.Length - i * 2 - 2]);
                            }
                            else
                            {
                                GL.PointSize(10);
                                GL.LineWidth(2);
                                GL.Color3(Color.White);
                            }
                            GL.PushMatrix();
                            GL.Translate((pth_begin.Pos.X + pth_end.Pos.X) / 2, (pth_begin.Pos.Y + pth_end.Pos.Y) / 2, (pth_begin.Pos.Z + pth_end.Pos.Z) / 2);
                            DrawAxes(0, 0, 0, 0.5f);
                            GL.Scale(0.5, 0.5, 0.5);
                            RenderString(pth.ID.ToString());
                            GL.PopMatrix();
                            GL.Begin(PrimitiveType.Lines);
                            GL.Vertex3(pth_begin.Pos.X, pth_begin.Pos.Y, pth_begin.Pos.Z);
                            GL.Vertex3(pth_end.Pos.X, pth_end.Pos.Y, pth_end.Pos.Z);
                            GL.End();
                        }
                    }

                    if (((TwinsSection)file.Data.GetItem(i)).RecordIDs.ContainsKey(3)) //positions
                    {
                        foreach (Position pos in ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(3)).Records)
                        {
                            GL.PushMatrix();
                            GL.Translate(pos.Pos.X, pos.Pos.Y, pos.Pos.Z);
                            if (file.SelectedItem != pos)
                            {
                                GL.PointSize(5);
                                GL.Color3(colors[colors.Length - i * 2 - 1]);
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
                                if (file.SelectedItem != pth || file.SelectedItemArg != k)
                                    GL.Color3(colors[colors.Length - i * 2 - 1]);
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
                                if (file.SelectedItem != pth || file.SelectedItemArg != k)
                                    GL.Color3(colors[colors.Length - i * 2 - 1]);
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
                            Matrix4 rot_ins = Matrix4.Identity;
                            rot_ins *= Matrix4.CreateRotationX(ins.RotX / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi);
                            rot_ins *= Matrix4.CreateRotationY(-ins.RotY / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi);
                            rot_ins *= Matrix4.CreateRotationZ(-ins.RotZ / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi);
                            GL.MultMatrix(ref rot_ins);
                            if (file.SelectedItem == ins)
                                GL.Color3(Color.White);
                            else
                                GL.Color3(colors[colors.Length - i * 2 - 1]);
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

                            var cur_color = file.SelectedItem == trg ? Color.White : colors[colors.Length - i * 2- 1];
                            GL.Begin(PrimitiveType.Quads);
                            GL.Color4(cur_color.R, cur_color.G, cur_color.B, (byte)127);

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
                            GL.Color4(cur_color);
                            GL.LineWidth(2);
                            GL.Begin(PrimitiveType.Lines);
                            foreach (var id in trg.Instances)
                            {
                                Instance inst = file.GetInstance(trg.Parent.Parent.ID, id);
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
                            
                            DrawAxes(0, 0, 0, Math.Min(trg.Coords[2].X / 2, Math.Min(trg.Coords[2].Y, trg.Coords[2].Z)) / 2);
                            GL.Enable(EnableCap.DepthTest);
                            GL.Enable(EnableCap.Lighting);

                            GL.PopMatrix();
                        }
                    }
                }
            }
            GL.Disable(EnableCap.Lighting);

            GL.PopMatrix();
            //Immediate mode drawing END
            GL.LineWidth(1);
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
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
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
                        min_x = Math.Min(min_x, pos_ins.X);
                        min_y = Math.Min(min_y, pos_ins.Y);
                        min_z = Math.Min(min_z, pos_ins.Z);
                        max_x = Math.Max(max_x, pos_ins.X);
                        max_y = Math.Max(max_y, pos_ins.Y);
                        max_z = Math.Max(max_z, pos_ins.Z);
                    }
                }
            }
            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
            if (vbo_id != null)
            {
                UpdateVBO(1);
            }
        }

        public void LoadColNodes()
        {
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
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
                min_x = Math.Min(min_x, i.X1);
                min_y = Math.Min(min_y, i.Y1);
                min_z = Math.Min(min_z, i.Z1);
                max_x = Math.Max(max_x, i.X2);
                max_y = Math.Max(max_y, i.Y2);
                max_z = Math.Max(max_z, i.Z2);
            }
            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
            if (vbo_id != null)
            {
                UpdateVBO(2);
            }
        }

        public void LoadPositions()
        {
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            bool[] record_exists = new bool[8];
            int posi_count = 0;
            for (uint i = 0; i <= 7; ++i)
            {
                record_exists[i] = file.Data.RecordIDs.ContainsKey(i);
                if (record_exists[i])
                {
                    record_exists[i] = false;
                    if (((TwinsSection)file.Data.GetItem(i)).RecordIDs.ContainsKey(3))
                    {
                        posi_count += ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(3)).Records.Count;
                        record_exists[i] = true;
                    }
                    if (((TwinsSection)file.Data.GetItem(i)).RecordIDs.ContainsKey(1))
                    {
                        posi_count += ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(1)).Records.Count;
                        record_exists[i] = true;
                    }
                }
            }
            if (vtx[3] == null || vtx.Length != (circle_res * 3 + 6) * posi_count)
            {
                posi_vtx_counts = new int[6 * posi_count];
                posi_vtx_offs = new int[6 * posi_count];
                vtx[3] = new Vertex[(circle_res * 3 + 6) * posi_count];
                for (int i = 0; i < posi_count; ++i)
                {
                    posi_vtx_counts[i * 6 + 0] = 2;
                    posi_vtx_counts[i * 6 + 1] = 2;
                    posi_vtx_counts[i * 6 + 2] = 2;
                    posi_vtx_counts[i * 6 + 3] = circle_res;
                    posi_vtx_counts[i * 6 + 4] = circle_res;
                    posi_vtx_counts[i * 6 + 5] = circle_res;
                }
            }
            int l = 0, m = 0;
            for (uint i = 0; i <= 7; ++i)
            {
                if (!record_exists[i]) continue;
                if (((TwinsSection)file.Data.GetItem(i)).RecordIDs.ContainsKey(1))
                {
                    foreach (AIPosition pos in ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(1)).Records)
                    {
                        indicator_size *= pos.Pos.W;
                        Vector3 pos_pos = pos.Pos.ToVec3();
                        pos_pos.X = -pos_pos.X;
                        posi_vtx_offs[l++] = m;
                        vtx[3][m++] = new Vertex(new Vector3(-indicator_size * 0.75f * 0.5f, 0, 0) + pos_pos, new Vector3(), Color.Red);
                        vtx[3][m++] = new Vertex(new Vector3(+indicator_size * 0.375f * 0.5f, 0, 0) + pos_pos, new Vector3(), Color.Red);
                        posi_vtx_offs[l++] = m;
                        vtx[3][m++] = new Vertex(new Vector3(0, indicator_size * 0.75f * 0.5f, 0) + pos_pos, new Vector3(), Color.Green);
                        vtx[3][m++] = new Vertex(new Vector3(0, -indicator_size * 0.375f * 0.5f, 0) + pos_pos, new Vector3(), Color.Green);
                        posi_vtx_offs[l++] = m;
                        vtx[3][m++] = new Vertex(new Vector3(0, 0, indicator_size * 0.75f * 0.5f) + pos_pos, new Vector3(), Color.Blue);
                        vtx[3][m++] = new Vertex(new Vector3(0, 0, -indicator_size * 0.375f * 0.5f) + pos_pos, new Vector3(), Color.Blue);
                        Color cur_color = (file.SelectedItem == pos) ? Color.White : colors[colors.Length - i * 2 - 2];
                        posi_vtx_offs[l++] = m;
                        for (int j = 0; j < circle_res; ++j)
                        {
                            Vector3 vec = new Vector3(0, 0, indicator_size);
                            vec *= Matrix3.Identity * Matrix3.CreateRotationX(MathHelper.TwoPi / circle_res * j);
                            vtx[3][m++] = new Vertex(pos_pos + vec, new Vector3(), cur_color);
                        }
                        posi_vtx_offs[l++] = m;
                        for (int j = 0; j < circle_res; ++j)
                        {
                            Vector3 vec = new Vector3(0, 0, indicator_size);
                            vec *= Matrix3.Identity * Matrix3.CreateRotationY(MathHelper.TwoPi / circle_res * j);
                            vtx[3][m++] = new Vertex(pos_pos + vec, new Vector3(), cur_color);
                        }
                        posi_vtx_offs[l++] = m;
                        for (int j = 0; j < circle_res; ++j)
                        {
                            Vector3 vec = new Vector3(0, indicator_size, 0);
                            vec *= Matrix3.Identity * Matrix3.CreateRotationZ(MathHelper.TwoPi / circle_res * j);
                            vtx[3][m++] = new Vertex(pos_pos + vec, new Vector3(), cur_color);
                        }
                        indicator_size /= pos.Pos.W;
                        min_x = Math.Min(min_x, pos_pos.X);
                        min_y = Math.Min(min_y, pos_pos.Y);
                        min_z = Math.Min(min_z, pos_pos.Z);
                        max_x = Math.Max(max_x, pos_pos.X);
                        max_y = Math.Max(max_y, pos_pos.Y);
                        max_z = Math.Max(max_z, pos_pos.Z);
                    }
                }
                if (((TwinsSection)file.Data.GetItem(i)).RecordIDs.ContainsKey(3))
                {
                    foreach (Position pos in ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(3)).Records)
                    {
                        Vector3 pos_pos = pos.Pos.ToVec3();
                        pos_pos.X = -pos_pos.X;
                        posi_vtx_offs[l++] = m;
                        vtx[3][m++] = new Vertex(new Vector3(-indicator_size * 0.75f * 0.5f, 0, 0) + pos_pos, new Vector3(), Color.Red);
                        vtx[3][m++] = new Vertex(new Vector3(+indicator_size * 0.375f * 0.5f, 0, 0) + pos_pos, new Vector3(), Color.Red);
                        posi_vtx_offs[l++] = m;
                        vtx[3][m++] = new Vertex(new Vector3(0, indicator_size * 0.75f * 0.5f, 0) + pos_pos, new Vector3(), Color.Green);
                        vtx[3][m++] = new Vertex(new Vector3(0, -indicator_size * 0.375f * 0.5f, 0) + pos_pos, new Vector3(), Color.Green);
                        posi_vtx_offs[l++] = m;
                        vtx[3][m++] = new Vertex(new Vector3(0, 0, indicator_size * 0.75f * 0.5f) + pos_pos, new Vector3(), Color.Blue);
                        vtx[3][m++] = new Vertex(new Vector3(0, 0, -indicator_size * 0.375f * 0.5f) + pos_pos, new Vector3(), Color.Blue);
                        Color cur_color = (file.SelectedItem == pos) ? Color.White : colors[colors.Length - i * 2 - 1];
                        posi_vtx_offs[l++] = m;
                        for (int j = 0; j < circle_res; ++j)
                        {
                            Vector3 vec = new Vector3(0, 0, indicator_size);
                            vec *= Matrix3.Identity * Matrix3.CreateRotationX(MathHelper.TwoPi / circle_res * j);
                            vtx[3][m++] = new Vertex(pos_pos + vec, new Vector3(), cur_color);
                        }
                        posi_vtx_offs[l++] = m;
                        for (int j = 0; j < circle_res; ++j)
                        {
                            Vector3 vec = new Vector3(0, 0, indicator_size);
                            vec *= Matrix3.Identity * Matrix3.CreateRotationY(MathHelper.TwoPi / circle_res * j);
                            vtx[3][m++] = new Vertex(pos_pos + vec, new Vector3(), cur_color);
                        }
                        posi_vtx_offs[l++] = m;
                        for (int j = 0; j < circle_res; ++j)
                        {
                            Vector3 vec = new Vector3(0, indicator_size, 0);
                            vec *= Matrix3.Identity * Matrix3.CreateRotationZ(MathHelper.TwoPi / circle_res * j);
                            vtx[3][m++] = new Vertex(pos_pos + vec, new Vector3(), cur_color);
                        }
                        min_x = Math.Min(min_x, pos_pos.X);
                        min_y = Math.Min(min_y, pos_pos.Y);
                        min_z = Math.Min(min_z, pos_pos.Z);
                        max_x = Math.Max(max_x, pos_pos.X);
                        max_y = Math.Max(max_y, pos_pos.Y);
                        max_z = Math.Max(max_z, pos_pos.Z);
                    }
                }
            }
            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
            if (vbo_id != null)
            {
                UpdateVBO(3);
            }
        }

        public void UpdateSelected()
        {
            if (file.SelectedItem is Instance ins)
            {
                SetPosition(new Vector3(-ins.Pos.X, ins.Pos.Y, ins.Pos.Z));
                LoadInstances();
            }
            else if (file.SelectedItem is Position pos)
            {
                SetPosition(new Vector3(-pos.Pos.X, pos.Pos.Y, pos.Pos.Z));
                LoadPositions();
            }
            else if (file.SelectedItem is Trigger trig)
            {
                SetPosition(new Vector3(-trig.Coords[1].X, trig.Coords[1].Y, trig.Coords[1].Z));
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
