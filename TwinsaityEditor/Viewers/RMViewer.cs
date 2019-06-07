using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public class RMViewer : ThreeDViewer
    {
        private static readonly int circle_res = 16;

        private bool show_col_nodes, show_triggers, wire_col, sm2_links;
        private FileController file;
        private ChunkLinks links;

        public RMViewer(FileController file, ref Form pform)
        {
            //initialize variables here
            show_col_nodes = show_triggers = wire_col = false;
            sm2_links = true;
            this.file = file;
            Tag = pform;
            InitVBO(5);
            if (file.DataAux != null && file.DataAux.ContainsItem(5))
            {
                links = (ChunkLinks)file.DataAux.GetItem(5);
            }
            if (file.Data.ContainsItem(9))
            {
                if (file.Data.GetItem(9).Size >= 12)
                {
                    pform.Text = "Loading collision tree...";
                    LoadColTree();
                    pform.Text = "Loading collision nodes...";
                    LoadColNodes();
                }
            }
            pform.Text = "Loading instances...";
            LoadInstances();
            pform.Text = "Loading positions...";
            LoadPositions();
            pform.Text = "Loading AI positions...";
            LoadAIPositions();
            pform.Text = "Initializing...";
        }

        protected override void RenderHUD()
        {
            base.RenderHUD();
            RenderString2D("Press C to toggle collision nodes\nPress X to toggle collision tree wireframe", 0, Height, 12, TextAnchor.BotLeft);
        }

        protected override void RenderObjects()
        {
            //put all object rendering code here
            //draw collision
            if (file.Data.ContainsItem(9))
            {
                GL.Enable(EnableCap.Lighting);
                vtx[0].DrawAllElements(PrimitiveType.Triangles, BufferPointerFlags.Normal);
                GL.Disable(EnableCap.Lighting);

                if (wire_col)
                {
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                    GL.Color3(Color.Black);
                    vtx[0].DrawAllElements(PrimitiveType.Triangles, BufferPointerFlags.NormalNoCol);
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                }

                if (show_col_nodes)
                {
                    vtx[2].DrawMulti(PrimitiveType.LineStrip, BufferPointerFlags.Default);
                }
            }

            //instances
            vtx[1].DrawMulti(PrimitiveType.LineStrip, BufferPointerFlags.Default);

            //positions + ai positions
            vtx[3].DrawMulti(PrimitiveType.LineLoop, BufferPointerFlags.Default);
            vtx[4].DrawMulti(PrimitiveType.LineLoop, BufferPointerFlags.Default);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.PushMatrix();
            
            for (uint i = 0; i <= 7; ++i)
            {
                if (file.Data.ContainsItem(i))
                {
                    Color cur_color;
                    if (((TwinsSection)file.Data.GetItem(i)).ContainsItem(1)) //aipositions
                    {
                        foreach (AIPosition pos in ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(1)).Records)
                        {
                            if (file.SelectedItem != pos)
                            {
                                GL.PointSize(5);
                                cur_color = colors[colors.Length - i * 2 - 2];
                            }
                            else
                            {
                                GL.PointSize(10);
                                cur_color = Color.White;
                            }
                            GL.Color3(cur_color);
                            GL.Begin(PrimitiveType.Points);
                            GL.Vertex3(-pos.Pos.X, pos.Pos.Y, pos.Pos.Z);
                            GL.End();
                            RenderString3D(pos.ID.ToString(), cur_color, -pos.Pos.X, pos.Pos.Y, pos.Pos.Z, ref identity_mat, pos.Pos.W / 3);
                        }
                    }

                    if (((TwinsSection)file.Data.GetItem(i)).ContainsItem(2)) //aipaths
                    {
                        foreach (AIPath pth in ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(2)).Records)
                        {
                            AIPosition pth_begin = file.GetAIPos(i, pth.Arg[0]);
                            AIPosition pth_end = file.GetAIPos(i, pth.Arg[1]);

                            if (file.SelectedItem != pth)
                            {
                                GL.PointSize(5);
                                GL.LineWidth(1);
                                cur_color = colors[colors.Length - i * 2 - 2];
                            }
                            else
                            {
                                GL.PointSize(10);
                                GL.LineWidth(2);
                                cur_color = Color.White;
                            }
                            RenderString3D(pth.ID.ToString(), cur_color, -(pth_begin.Pos.X + pth_end.Pos.X) / 2, (pth_begin.Pos.Y + pth_end.Pos.Y) / 2, (pth_begin.Pos.Z + pth_end.Pos.Z) / 2, ref identity_mat, 0.5F);
                            GL.Color3(cur_color);
                            GL.Begin(PrimitiveType.Lines);
                            GL.Vertex3(-pth_begin.Pos.X, pth_begin.Pos.Y, pth_begin.Pos.Z);
                            GL.Vertex3(-pth_end.Pos.X, pth_end.Pos.Y, pth_end.Pos.Z);
                            GL.End();
                        }
                    }

                    if (((TwinsSection)file.Data.GetItem(i)).ContainsItem(3)) //positions
                    {
                        foreach (Position pos in ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(3)).Records)
                        {
                            if (file.SelectedItem != pos)
                            {
                                GL.PointSize(5);
                                cur_color = colors[colors.Length - i * 2 - 1];
                            }
                            else
                            {
                                GL.PointSize(10);
                                cur_color = Color.White;
                            }
                            GL.Color3(cur_color);
                            GL.Begin(PrimitiveType.Points);
                            GL.Vertex3(-pos.Pos.X, pos.Pos.Y, pos.Pos.Z);
                            GL.End();
                            RenderString3D(pos.ID.ToString(), cur_color, -pos.Pos.X, pos.Pos.Y, pos.Pos.Z, ref identity_mat, 0.5F);
                        }
                    }

                    if (((TwinsSection)file.Data.GetItem(i)).ContainsItem(4)) //paths
                    {
                        foreach (Path pth in ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(4)).Records)
                        {
                            for (int k = 0; k < pth.Positions.Count; ++k)
                            {
                                DrawAxes(-pth.Positions[k].X, pth.Positions[k].Y, pth.Positions[k].Z, 0.5f);
                                if (file.SelectedItem != pth || file.SelectedItemArg != k)
                                    cur_color = colors[colors.Length - i * 2 - 1];
                                else
                                    cur_color = Color.White;
                                RenderString3D($"{pth.ID.ToString()}:{k}", cur_color, -pth.Positions[k].X, pth.Positions[k].Y, pth.Positions[k].Z, ref identity_mat, 0.5F);
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
                                GL.Vertex3(-pth.Positions[k].X, pth.Positions[k].Y, pth.Positions[k].Z);
                            }
                            GL.End();
                        }
                    }

                    if (((TwinsSection)file.Data.GetItem(i)).ContainsItem(6)) //instances
                    {
                        foreach (Instance ins in ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(6)).Records)
                        {
                            Matrix3 rot_ins = Matrix3.Identity;
                            rot_ins *= Matrix3.CreateRotationX(ins.RotX / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi);
                            rot_ins *= Matrix3.CreateRotationY(-ins.RotY / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi);
                            rot_ins *= Matrix3.CreateRotationZ(-ins.RotZ / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi);
                            if (file.SelectedItem == ins)
                                cur_color = Color.White;
                            else
                                cur_color = colors[colors.Length - i * 2 - 1];
                            RenderString3D(ins.ID.ToString(), cur_color, -ins.Pos.X, ins.Pos.Y, ins.Pos.Z, ref rot_ins);
                        }
                    }
                }
            }

            //Draw triggers (transparent surfaces)
            for (uint i = 0; i <= 7; ++i)
            {
                if (file.Data.ContainsItem(i))
                {
                    if (((TwinsSection)file.Data.GetItem(i)).ContainsItem(7) && show_triggers)
                    {
                        foreach (Trigger trg in ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(7)).Records)
                        {
                            GL.PushMatrix();
                            GL.Scale(-1, 1, 1);
                            GL.Translate(trg.Coords[1].X, trg.Coords[1].Y, trg.Coords[1].Z);

                            var cur_color = file.SelectedItem == trg ? Color.White : colors[colors.Length - i * 2- 1];
                            GL.DepthMask(false);
                            GL.Enable(EnableCap.Lighting);
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
                            GL.DepthMask(true);
                            GL.Disable(EnableCap.Lighting);

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

                            GL.PopMatrix();
                        }
                    }
                }
            }

            //Draw chunk links if available
            if (sm2_links && links != null)
            {
                foreach (var l in links.Links)
                {
                    GL.PushMatrix();
                    GL.DepthMask(false);
                    GL.Scale(-1, 1, 1);
                    if (l.HasWall())
                    {
                        GL.Color4(Color.FromArgb(95, Color.DarkGray));
                        GL.Begin(PrimitiveType.Quads);
                        GL.Vertex4(l.LoadWall[0].ToArray());
                        GL.Vertex4(l.LoadWall[1].ToArray());
                        GL.Vertex4(l.LoadWall[2].ToArray());
                        GL.Vertex4(l.LoadWall[3].ToArray());
                        GL.End();
                        GL.Color4(Color.DarkGray);
                        GL.Begin(PrimitiveType.LineLoop);
                        GL.Vertex4(l.LoadWall[0].ToArray());
                        GL.Vertex4(l.LoadWall[1].ToArray());
                        GL.Vertex4(l.LoadWall[2].ToArray());
                        GL.Vertex4(l.LoadWall[3].ToArray());
                        GL.End();
                        RenderString3D(new string(l.Path), Color.DarkGray,
                            -(l.LoadWall[0].X + l.LoadWall[1].X + l.LoadWall[2].X + l.LoadWall[3].X) / 4,
                            (l.LoadWall[0].Y + l.LoadWall[1].Y + l.LoadWall[2].Y + l.LoadWall[3].Y) / 4,
                            (l.LoadWall[0].Z + l.LoadWall[1].Z + l.LoadWall[2].Z + l.LoadWall[3].Z) / 4,
                            ref identity_mat);
                    }
                    if (l.Type == 1 || l.Type == 3)
                    {
                        GL.Color4(Color.FromArgb(95, Color.DarkGray));
                        GL.Begin(PrimitiveType.QuadStrip);
                        GL.Vertex4(l.LoadArea[0].ToArray());
                        GL.Vertex4(l.LoadArea[1].ToArray());
                        GL.Vertex4(l.LoadArea[2].ToArray());
                        GL.Vertex4(l.LoadArea[3].ToArray());
                        GL.Vertex4(l.LoadArea[4].ToArray());
                        GL.Vertex4(l.LoadArea[5].ToArray());
                        GL.Vertex4(l.LoadArea[6].ToArray());
                        GL.Vertex4(l.LoadArea[7].ToArray());
                        GL.Vertex4(l.LoadArea[0].ToArray());
                        GL.Vertex4(l.LoadArea[1].ToArray());
                        GL.End();
                        GL.Begin(PrimitiveType.Quads);
                        GL.Vertex4(l.LoadArea[1].ToArray());
                        GL.Vertex4(l.LoadArea[3].ToArray());
                        GL.Vertex4(l.LoadArea[5].ToArray());
                        GL.Vertex4(l.LoadArea[7].ToArray());
                        GL.Vertex4(l.LoadArea[0].ToArray());
                        GL.Vertex4(l.LoadArea[2].ToArray());
                        GL.Vertex4(l.LoadArea[4].ToArray());
                        GL.Vertex4(l.LoadArea[6].ToArray());
                        GL.End();
                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                        GL.Begin(PrimitiveType.QuadStrip);
                        GL.Vertex4(l.LoadArea[0].ToArray());
                        GL.Vertex4(l.LoadArea[1].ToArray());
                        GL.Vertex4(l.LoadArea[2].ToArray());
                        GL.Vertex4(l.LoadArea[3].ToArray());
                        GL.Vertex4(l.LoadArea[4].ToArray());
                        GL.Vertex4(l.LoadArea[5].ToArray());
                        GL.Vertex4(l.LoadArea[6].ToArray());
                        GL.Vertex4(l.LoadArea[7].ToArray());
                        GL.Vertex4(l.LoadArea[0].ToArray());
                        GL.Vertex4(l.LoadArea[1].ToArray());
                        GL.End();
                        GL.Begin(PrimitiveType.Quads);
                        GL.Vertex4(l.LoadArea[1].ToArray());
                        GL.Vertex4(l.LoadArea[3].ToArray());
                        GL.Vertex4(l.LoadArea[5].ToArray());
                        GL.Vertex4(l.LoadArea[7].ToArray());
                        GL.Vertex4(l.LoadArea[0].ToArray());
                        GL.Vertex4(l.LoadArea[2].ToArray());
                        GL.Vertex4(l.LoadArea[4].ToArray());
                        GL.Vertex4(l.LoadArea[6].ToArray());
                        GL.End();
                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                        if (l.HasWall())
                        {
                            GL.Disable(EnableCap.DepthTest);
                            GL.LineWidth(2);
                            GL.Color4(Color.DarkGray);
                            GL.Begin(PrimitiveType.Lines);
                            GL.Vertex3((l.LoadArea[0].X + l.LoadArea[1].X + l.LoadArea[2].X + l.LoadArea[3].X + l.LoadArea[4].X + l.LoadArea[5].X + l.LoadArea[6].X + l.LoadArea[7].X) / 8,
                                (l.LoadArea[0].Y + l.LoadArea[1].Y + l.LoadArea[2].Y + l.LoadArea[3].Y + l.LoadArea[4].Y + l.LoadArea[5].Y + l.LoadArea[6].Y + l.LoadArea[7].Y) / 8,
                                (l.LoadArea[0].Z + l.LoadArea[1].Z + l.LoadArea[2].Z + l.LoadArea[3].Z + l.LoadArea[4].Z + l.LoadArea[5].Z + l.LoadArea[6].Z + l.LoadArea[7].Z) / 8);
                            GL.Vertex3((l.LoadWall[0].X + l.LoadWall[1].X + l.LoadWall[2].X + l.LoadWall[3].X) / 4,
                                (l.LoadWall[0].Y + l.LoadWall[1].Y + l.LoadWall[2].Y + l.LoadWall[3].Y) / 4,
                                (l.LoadWall[0].Z + l.LoadWall[1].Z + l.LoadWall[2].Z + l.LoadWall[3].Z) / 4);
                            GL.End();
                            GL.Enable(EnableCap.DepthTest);
                        }
                    }
                    GL.DepthMask(true);
                    GL.PopMatrix();
                }
            }

            GL.PopMatrix();

            GL.LineWidth(1);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.C:
                case Keys.L:
                case Keys.T:
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
                case Keys.C:
                    show_col_nodes = !show_col_nodes;
                    break;
                case Keys.L:
                    sm2_links = !sm2_links;
                    break;
                case Keys.T:
                    show_triggers = !show_triggers;
                    break;
                case Keys.X:
                    wire_col = !wire_col;
                    break;
            }
        }

        public void LoadColTree()
        {
            ColData data = (ColData)file.Data.GetItem(9);
            //vtx[0].Vtx = new Vertex[data.Vertices.Count];
            List<Vertex> vertices = new List<Vertex>(data.Vertices.Count);
            vtx[0].VtxInd = new uint[data.Tris.Count * 3];
            for (int i = 0; i < data.Vertices.Count; ++i)
            {
                var v = data.Vertices[i].ToVec3();
                v.X = -v.X;
                vertices.Add(new Vertex(v));
            }
            for (int i = 0; i < data.Tris.Count; ++i)
            {
                uint col = Vertex.ColorToABGR(colors[data.Tris[i].Surface % colors.Length]);
                int v1 = data.Tris[i].Vert1;
                if (vertices[v1].Col != 0 && vertices[v1].Col != col)
                {
                    vertices.Add(vertices[v1]);
                    v1 = vertices.Count-1;
                }
                int v2 = data.Tris[i].Vert2;
                if (vertices[v2].Col != 0 && vertices[v2].Col != col)
                {
                    vertices.Add(vertices[v2]);
                    v2 = vertices.Count-1;
                }
                int v3 = data.Tris[i].Vert3;
                if (vertices[v3].Col != 0 && vertices[v3].Col != col)
                {
                    vertices.Add(vertices[v3]);
                    v3 = vertices.Count-1;
                }
                vtx[0].VtxInd[i * 3 + 0] = (uint)v1;
                vtx[0].VtxInd[i * 3 + 1] = (uint)v2;
                vtx[0].VtxInd[i * 3 + 2] = (uint)v3;
                Vector3 normal = VectorFuncs.CalcNormal(vertices[v1].Pos, vertices[v2].Pos, vertices[v3].Pos);
                var v = vertices[v1];
                v.Nor += normal;
                v.Col = col;
                vertices[v1] = v;
                v = vertices[v2];
                v.Nor += normal;
                v.Col = col;
                vertices[v2] = v;
                v = vertices[v3];
                v.Nor += normal;
                v.Col = col;
                vertices[v3] = v;
            }
            vtx[0].Vtx = vertices.ToArray();
            //vtx[0].Vtx = new Vertex[data.Tris.Count * 3];
            //for (int i = 0; i < data.Tris.Count; ++i)
            //{
            //    Vector3 v1 = data.Vertices[data.Tris[i].Vert1].ToVec3();
            //    Vector3 v2 = data.Vertices[data.Tris[i].Vert2].ToVec3();
            //    Vector3 v3 = data.Vertices[data.Tris[i].Vert3].ToVec3();
            //    v1.X = -v1.X;
            //    v2.X = -v2.X;
            //    v3.X = -v3.X;
            //    Vector3 nor = VectorFuncs.CalcNormal(v1, v2, v3);
            //    vtx[0].Vtx[i * 3 + 0] = new Vertex(v1, nor, colors[data.Tris[i].Surface % colors.Length]);
            //    vtx[0].Vtx[i * 3 + 1] = new Vertex(v2, nor, colors[data.Tris[i].Surface % colors.Length]);
            //    vtx[0].Vtx[i * 3 + 2] = new Vertex(v3, nor, colors[data.Tris[i].Surface % colors.Length]);
            //}
            UpdateVBO(0);
        }

        public void LoadInstances()
        {
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            bool[] record_exists = new bool[8];
            int inst_count = 0;
            for (uint i = 0; i <= 7; ++i)
            {
                record_exists[i] = file.Data.ContainsItem(i);
                if (record_exists[i])
                {
                    if (((TwinsSection)file.Data.GetItem(i)).ContainsItem(6))
                        inst_count += ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(6)).Records.Count;
                    else record_exists[i] = false;
                }
            }
            if (vtx[1].Vtx == null || vtx[1].Vtx.Length != 22 * inst_count)
            {
                vtx[1].VtxCounts = new int[7 * inst_count];
                vtx[1].VtxOffs = new int[7 * inst_count];
                vtx[1].Vtx = new Vertex[22 * inst_count];
                for (int i = 0; i < inst_count; ++i)
                {
                    vtx[1].VtxCounts[i * 7 + 0] = 2;
                    vtx[1].VtxCounts[i * 7 + 1] = 2;
                    vtx[1].VtxCounts[i * 7 + 2] = 2;
                    vtx[1].VtxCounts[i * 7 + 3] = 8;
                    vtx[1].VtxCounts[i * 7 + 4] = 4;
                    vtx[1].VtxCounts[i * 7 + 5] = 2;
                    vtx[1].VtxCounts[i * 7 + 6] = 2;
                }
            }
            int l = 0, m = 0;
            for (uint i = 0; i <= 7; ++i)
            {
                if (!record_exists[i]) continue;
                if (((TwinsSection)file.Data.GetItem(i)).ContainsItem(6))
                {
                    foreach (Instance ins in ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(6)).Records)
                    {
                        Matrix3 rot_ins = Matrix3.Identity;
                        rot_ins *= Matrix3.CreateRotationX(ins.RotX / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi);
                        rot_ins *= Matrix3.CreateRotationY(-ins.RotY / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi);
                        rot_ins *= Matrix3.CreateRotationZ(-ins.RotZ / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi);
                        Vector3 pos_ins = ins.Pos.ToVec3();
                        pos_ins.X = -pos_ins.X;
                        vtx[1].VtxOffs[l++] = m;
                        vtx[1].Vtx[m++] = new Vertex(new Vector3(-indicator_size * 0.75f, 0, 0) + pos_ins, Color.Red);
                        vtx[1].Vtx[m++] = new Vertex(new Vector3(+indicator_size * 0.375f, 0, 0) + pos_ins, Color.Red);
                        vtx[1].VtxOffs[l++] = m;
                        vtx[1].Vtx[m++] = new Vertex(new Vector3(0, indicator_size * 0.75f, 0) + pos_ins, Color.Green);
                        vtx[1].Vtx[m++] = new Vertex(new Vector3(0, -indicator_size * 0.375f, 0) + pos_ins, Color.Green);
                        vtx[1].VtxOffs[l++] = m;
                        vtx[1].Vtx[m++] = new Vertex(new Vector3(0, 0, indicator_size * 0.75f) + pos_ins, Color.Blue);
                        vtx[1].Vtx[m++] = new Vertex(new Vector3(0, 0, -indicator_size * 0.375f) + pos_ins, Color.Blue);
                        vtx[1].VtxOffs[l++] = m;
                        Color cur_color = (file.SelectedItem == ins) ? Color.White : colors[colors.Length - i * 2 - 1];
                        vtx[1].Vtx[m++] = new Vertex(new Vector3(-indicator_size, -indicator_size + 0.5f, -indicator_size) * rot_ins + pos_ins, cur_color);
                        vtx[1].Vtx[m++] = new Vertex(new Vector3(+indicator_size, -indicator_size + 0.5f, -indicator_size) * rot_ins + pos_ins, cur_color);
                        vtx[1].Vtx[m++] = new Vertex(new Vector3(+indicator_size, +indicator_size + 0.5f, -indicator_size) * rot_ins + pos_ins, cur_color);
                        vtx[1].Vtx[m++] = new Vertex(new Vector3(-indicator_size, +indicator_size + 0.5f, -indicator_size) * rot_ins + pos_ins, cur_color);
                        vtx[1].Vtx[m++] = new Vertex(new Vector3(-indicator_size, -indicator_size + 0.5f, -indicator_size) * rot_ins + pos_ins, cur_color);
                        vtx[1].Vtx[m++] = new Vertex(new Vector3(-indicator_size, -indicator_size + 0.5f, +indicator_size) * rot_ins + pos_ins, cur_color);
                        vtx[1].Vtx[m++] = new Vertex(new Vector3(+indicator_size, -indicator_size + 0.5f, +indicator_size) * rot_ins + pos_ins, cur_color);
                        vtx[1].Vtx[m++] = new Vertex(new Vector3(+indicator_size, -indicator_size + 0.5f, -indicator_size) * rot_ins + pos_ins, cur_color);
                        vtx[1].VtxOffs[l++] = m;
                        vtx[1].Vtx[m++] = new Vertex(new Vector3(-indicator_size, -indicator_size + 0.5f, +indicator_size) * rot_ins + pos_ins, cur_color);
                        vtx[1].Vtx[m++] = new Vertex(new Vector3(-indicator_size, +indicator_size + 0.5f, +indicator_size) * rot_ins + pos_ins, cur_color);
                        vtx[1].Vtx[m++] = new Vertex(new Vector3(+indicator_size, +indicator_size + 0.5f, +indicator_size) * rot_ins + pos_ins, cur_color);
                        vtx[1].Vtx[m++] = new Vertex(new Vector3(+indicator_size, -indicator_size + 0.5f, +indicator_size) * rot_ins + pos_ins, cur_color);
                        vtx[1].VtxOffs[l++] = m;
                        vtx[1].Vtx[m++] = new Vertex(new Vector3(-indicator_size, +indicator_size + 0.5f, +indicator_size) * rot_ins + pos_ins, cur_color);
                        vtx[1].Vtx[m++] = new Vertex(new Vector3(-indicator_size, +indicator_size + 0.5f, -indicator_size) * rot_ins + pos_ins, cur_color);
                        vtx[1].VtxOffs[l++] = m;
                        vtx[1].Vtx[m++] = new Vertex(new Vector3(+indicator_size, +indicator_size + 0.5f, +indicator_size) * rot_ins + pos_ins, cur_color);
                        vtx[1].Vtx[m++] = new Vertex(new Vector3(+indicator_size, +indicator_size + 0.5f, -indicator_size) * rot_ins + pos_ins, cur_color);
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
            UpdateVBO(1);
        }

        public void LoadColNodes()
        {
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            ColData data = (ColData)file.Data.GetItem(9);
            vtx[2].Vtx = new Vertex[data.Triggers.Count * 16];
            vtx[2].VtxCounts = new int[4 * data.Triggers.Count];
            vtx[2].VtxOffs = new int[4 * data.Triggers.Count];
            for (int i = 0; i < data.Triggers.Count; ++i)
            {
                vtx[2].VtxCounts[i * 4 + 0] = 8;
                vtx[2].VtxCounts[i * 4 + 1] = 4;
                vtx[2].VtxCounts[i * 4 + 2] = 2;
                vtx[2].VtxCounts[i * 4 + 3] = 2;
            }
            int l = 0, m = 0;
            foreach (var i in data.Triggers)
            {
                Color cur_color = (i.Flag1 == i.Flag2 && i.Flag1 < 0) ? Color.Cyan : Color.Red;
                vtx[2].VtxOffs[l++] = m;
                vtx[2].Vtx[m++] = new Vertex(new Vector3(-i.X1, i.Y1, i.Z1), cur_color);
                vtx[2].Vtx[m++] = new Vertex(new Vector3(-i.X2, i.Y1, i.Z1), cur_color);
                vtx[2].Vtx[m++] = new Vertex(new Vector3(-i.X2, i.Y2, i.Z1), cur_color);
                vtx[2].Vtx[m++] = new Vertex(new Vector3(-i.X1, i.Y2, i.Z1), cur_color);
                vtx[2].Vtx[m++] = new Vertex(new Vector3(-i.X1, i.Y1, i.Z1), cur_color);
                vtx[2].Vtx[m++] = new Vertex(new Vector3(-i.X1, i.Y1, i.Z2), cur_color);
                vtx[2].Vtx[m++] = new Vertex(new Vector3(-i.X2, i.Y1, i.Z2), cur_color);
                vtx[2].Vtx[m++] = new Vertex(new Vector3(-i.X2, i.Y1, i.Z1), cur_color);
                vtx[2].VtxOffs[l++] = m;
                vtx[2].Vtx[m++] = new Vertex(new Vector3(-i.X1, i.Y1, i.Z2), cur_color);
                vtx[2].Vtx[m++] = new Vertex(new Vector3(-i.X1, i.Y2, i.Z2), cur_color);
                vtx[2].Vtx[m++] = new Vertex(new Vector3(-i.X2, i.Y2, i.Z2), cur_color);
                vtx[2].Vtx[m++] = new Vertex(new Vector3(-i.X2, i.Y1, i.Z2), cur_color);
                vtx[2].VtxOffs[l++] = m;
                vtx[2].Vtx[m++] = new Vertex(new Vector3(-i.X1, i.Y2, i.Z2), cur_color);
                vtx[2].Vtx[m++] = new Vertex(new Vector3(-i.X1, i.Y2, i.Z1), cur_color);
                vtx[2].VtxOffs[l++] = m;
                vtx[2].Vtx[m++] = new Vertex(new Vector3(-i.X2, i.Y2, i.Z2), cur_color);
                vtx[2].Vtx[m++] = new Vertex(new Vector3(-i.X2, i.Y2, i.Z1), cur_color);
                min_x = Math.Min(min_x, i.X1);
                min_y = Math.Min(min_y, i.Y1);
                min_z = Math.Min(min_z, i.Z1);
                max_x = Math.Max(max_x, i.X2);
                max_y = Math.Max(max_y, i.Y2);
                max_z = Math.Max(max_z, i.Z2);
            }
            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
            UpdateVBO(2);
        }

        public void LoadPositions()
        {
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            bool[] record_exists = new bool[8];
            int posi_count = 0;
            for (uint i = 0; i <= 7; ++i)
            {
                record_exists[i] = file.Data.ContainsItem(i);
                if (record_exists[i])
                {
                    if (((TwinsSection)file.Data.GetItem(i)).ContainsItem(3))
                    {
                        posi_count += ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(3)).Records.Count;
                        record_exists[i] = true;
                    }
                    else
                        record_exists[i] = false;
                }
            }
            if (vtx[3] == null || vtx.Length != (circle_res * 3 + 6) * posi_count)
            {
                vtx[3].VtxCounts = new int[6 * posi_count];
                vtx[3].VtxOffs = new int[6 * posi_count];
                vtx[3].Vtx = new Vertex[(circle_res * 3 + 6) * posi_count];
                for (int i = 0; i < posi_count; ++i)
                {
                    vtx[3].VtxCounts[i * 6 + 0] = 2;
                    vtx[3].VtxCounts[i * 6 + 1] = 2;
                    vtx[3].VtxCounts[i * 6 + 2] = 2;
                    vtx[3].VtxCounts[i * 6 + 3] = circle_res;
                    vtx[3].VtxCounts[i * 6 + 4] = circle_res;
                    vtx[3].VtxCounts[i * 6 + 5] = circle_res;
                }
            }
            int l = 0, m = 0;
            for (uint i = 0; i <= 7; ++i)
            {
                if (!record_exists[i]) continue;
                if (((TwinsSection)file.Data.GetItem(i)).ContainsItem(3))
                {
                    foreach (Position pos in ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(3)).Records)
                    {
                        Vector3 pos_pos = pos.Pos.ToVec3();
                        pos_pos.X = -pos_pos.X;
                        vtx[3].VtxOffs[l++] = m;
                        vtx[3].Vtx[m++] = new Vertex(new Vector3(-indicator_size * 0.75f * 0.5f, 0, 0) + pos_pos, Color.Red);
                        vtx[3].Vtx[m++] = new Vertex(new Vector3(+indicator_size * 0.375f * 0.5f, 0, 0) + pos_pos, Color.Red);
                        vtx[3].VtxOffs[l++] = m;
                        vtx[3].Vtx[m++] = new Vertex(new Vector3(0, indicator_size * 0.75f * 0.5f, 0) + pos_pos, Color.Green);
                        vtx[3].Vtx[m++] = new Vertex(new Vector3(0, -indicator_size * 0.375f * 0.5f, 0) + pos_pos, Color.Green);
                        vtx[3].VtxOffs[l++] = m;
                        vtx[3].Vtx[m++] = new Vertex(new Vector3(0, 0, indicator_size * 0.75f * 0.5f) + pos_pos, Color.Blue);
                        vtx[3].Vtx[m++] = new Vertex(new Vector3(0, 0, -indicator_size * 0.375f * 0.5f) + pos_pos, Color.Blue);
                        Color cur_color = (file.SelectedItem == pos) ? Color.White : colors[colors.Length - i * 2 - 1];
                        vtx[3].VtxOffs[l++] = m;
                        for (int j = 0; j < circle_res; ++j)
                        {
                            Vector3 vec = new Vector3(0, 0, indicator_size);
                            vec *= Matrix3.Identity * Matrix3.CreateRotationX(MathHelper.TwoPi / circle_res * j);
                            vtx[3].Vtx[m++] = new Vertex(pos_pos + vec, cur_color);
                        }
                        vtx[3].VtxOffs[l++] = m;
                        for (int j = 0; j < circle_res; ++j)
                        {
                            Vector3 vec = new Vector3(0, 0, indicator_size);
                            vec *= Matrix3.Identity * Matrix3.CreateRotationY(MathHelper.TwoPi / circle_res * j);
                            vtx[3].Vtx[m++] = new Vertex(pos_pos + vec, cur_color);
                        }
                        vtx[3].VtxOffs[l++] = m;
                        for (int j = 0; j < circle_res; ++j)
                        {
                            Vector3 vec = new Vector3(0, indicator_size, 0);
                            vec *= Matrix3.Identity * Matrix3.CreateRotationZ(MathHelper.TwoPi / circle_res * j);
                            vtx[3].Vtx[m++] = new Vertex(pos_pos + vec, cur_color);
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
            UpdateVBO(3);
        }

        public void LoadAIPositions()
        {
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            bool[] record_exists = new bool[8];
            int posi_count = 0;
            for (uint i = 0; i <= 7; ++i)
            {
                record_exists[i] = file.Data.ContainsItem(i);
                if (record_exists[i])
                {
                    if (((TwinsSection)file.Data.GetItem(i)).ContainsItem(1))
                    {
                        posi_count += ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(1)).Records.Count;
                        record_exists[i] = true;
                    }
                    else
                        record_exists[i] = false;
                }
            }
            if (vtx[4] == null || vtx.Length != (circle_res * 3 + 6) * posi_count)
            {
                vtx[4].VtxCounts = new int[6 * posi_count];
                vtx[4].VtxOffs = new int[6 * posi_count];
                vtx[4].Vtx = new Vertex[(circle_res * 3 + 6) * posi_count];
                for (int i = 0; i < posi_count; ++i)
                {
                    vtx[4].VtxCounts[i * 6 + 0] = 2;
                    vtx[4].VtxCounts[i * 6 + 1] = 2;
                    vtx[4].VtxCounts[i * 6 + 2] = 2;
                    vtx[4].VtxCounts[i * 6 + 3] = circle_res;
                    vtx[4].VtxCounts[i * 6 + 4] = circle_res;
                    vtx[4].VtxCounts[i * 6 + 5] = circle_res;
                }
            }
            int l = 0, m = 0;
            for (uint i = 0; i <= 7; ++i)
            {
                if (!record_exists[i]) continue;
                if (((TwinsSection)file.Data.GetItem(i)).ContainsItem(1))
                {
                    foreach (AIPosition pos in ((TwinsSection)((TwinsSection)file.Data.GetItem(i)).GetItem(1)).Records)
                    {
                        var ind_size = indicator_size * pos.Pos.W;
                        Vector3 pos_pos = pos.Pos.ToVec3();
                        pos_pos.X = -pos_pos.X;
                        vtx[4].VtxOffs[l++] = m;
                        vtx[4].Vtx[m++] = new Vertex(new Vector3(-ind_size * 0.75f * 0.5f, 0, 0) + pos_pos, Color.Red);
                        vtx[4].Vtx[m++] = new Vertex(new Vector3(+ind_size * 0.375f * 0.5f, 0, 0) + pos_pos, Color.Red);
                        vtx[4].VtxOffs[l++] = m;
                        vtx[4].Vtx[m++] = new Vertex(new Vector3(0, ind_size * 0.75f * 0.5f, 0) + pos_pos, Color.Green);
                        vtx[4].Vtx[m++] = new Vertex(new Vector3(0, -ind_size * 0.375f * 0.5f, 0) + pos_pos, Color.Green);
                        vtx[4].VtxOffs[l++] = m;
                        vtx[4].Vtx[m++] = new Vertex(new Vector3(0, 0, ind_size * 0.75f * 0.5f) + pos_pos, Color.Blue);
                        vtx[4].Vtx[m++] = new Vertex(new Vector3(0, 0, -ind_size * 0.375f * 0.5f) + pos_pos, Color.Blue);
                        Color cur_color = (file.SelectedItem == pos) ? Color.White : colors[colors.Length - i * 2 - 2];
                        vtx[4].VtxOffs[l++] = m;
                        for (int j = 0; j < circle_res; ++j)
                        {
                            Vector3 vec = new Vector3(0, 0, ind_size);
                            vec *= Matrix3.Identity * Matrix3.CreateRotationX(MathHelper.TwoPi / circle_res * j);
                            vtx[4].Vtx[m++] = new Vertex(pos_pos + vec, cur_color);
                        }
                        vtx[4].VtxOffs[l++] = m;
                        for (int j = 0; j < circle_res; ++j)
                        {
                            Vector3 vec = new Vector3(0, 0, ind_size);
                            vec *= Matrix3.Identity * Matrix3.CreateRotationY(MathHelper.TwoPi / circle_res * j);
                            vtx[4].Vtx[m++] = new Vertex(pos_pos + vec, cur_color);
                        }
                        vtx[4].VtxOffs[l++] = m;
                        for (int j = 0; j < circle_res; ++j)
                        {
                            Vector3 vec = new Vector3(0, ind_size, 0);
                            vec *= Matrix3.Identity * Matrix3.CreateRotationZ(MathHelper.TwoPi / circle_res * j);
                            vtx[4].Vtx[m++] = new Vertex(pos_pos + vec, cur_color);
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
            UpdateVBO(4);
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
