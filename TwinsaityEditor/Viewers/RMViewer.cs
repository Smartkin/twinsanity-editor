using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public class RMViewer : ThreeDViewer
    {
        private static readonly int circle_res = 16;

        private bool show_col_nodes, show_triggers, show_cams, wire_col, sm2_links, obj_models, collisions, show_scenery;
        private FileController file;
        private ChunkLinks links;
        private SceneryData scenery;
        private List<VertexBufferData> sceneryObjects = new List<VertexBufferData>();

        private List<DefaultEnums.ObjectID> WoodCrates = new List<DefaultEnums.ObjectID>()
        {
            DefaultEnums.ObjectID.AKUAKUCRATE, DefaultEnums.ObjectID.AMMOCRATESMALL, DefaultEnums.ObjectID.BASICCRATE, DefaultEnums.ObjectID.CHECKPOINTCRATE, DefaultEnums.ObjectID.EXTRALIFECRATE, DefaultEnums.ObjectID.EXTRALIFECRATECORTEX, DefaultEnums.ObjectID.EXTRALIFECRATENINA,
            DefaultEnums.ObjectID.LEVELCRATE, DefaultEnums.ObjectID.MULTIPLEHITCRATE, DefaultEnums.ObjectID.SURPRISECRATE, DefaultEnums.ObjectID.WOODENSPRINGCRATE
        };

        public RMViewer(FileController file, Form pform)
        {
            //initialize variables here
            show_col_nodes = show_triggers = wire_col = show_cams = show_scenery = false;
            sm2_links = true;
            obj_models = true;
            collisions = true;
            this.file = file;
            Tag = pform;
            if (file.Data.Type == TwinsFile.FileType.RM2 || file.Data.Type == TwinsFile.FileType.RMX)
            {
                int ObjectModelPool = 3000; // model limit
                InitVBO(ObjectModelPool);
            }
            else
            {
                InitVBO(6);
            }
            uint link_section = 5;
            if (file.DataAux != null && file.DataAux.Type == TwinsFile.FileType.MonkeyBallSM) link_section = 6;
            if (file.DataAux != null && file.DataAux.ContainsItem(link_section))
            {
                links = file.DataAux.GetItem<ChunkLinks>(link_section);
            }
            if (file.DataAux != null && file.DataAux.ContainsItem(0))
            {
                scenery = file.DataAux.GetItem<SceneryData>(0);
                LoadScenery(new Matrix4());
            }
            uint col_section = 9;
            if (file.Data.Type == TwinsFile.FileType.MonkeyBallRM) col_section = 10;
            if (file.Data.ContainsItem(col_section))
            {
                if (file.Data.GetItem<ColData>(col_section).Size >= 12)
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
            RenderString2D("Press C to toggle collision nodes\nPress X to toggle collision tree wireframe\nPress T to toggle object triggers\nPress Y to toggle camera triggers\nPress V to toggle object models\nPress F to toggle collision rendering\nPress U to toggle scenery", 0, Height, 10, Color.White, TextAnchor.BotLeft);
            RenderString2D("X: " + (-pos.X) + "\n\nY: " + pos.Y + "\n\nZ: " + pos.Z, 0, Height - 104, 12, Color.White, TextAnchor.BotLeft);
        }

        public Vector3 GetViewerPos()
        {
            return new Vector3(-pos.X,pos.Y,pos.Z);
        }

        protected override void RenderObjects()
        {
            //put all object rendering code here
            //draw collision
            if (file.Data.ContainsItem(9))
            {
                if (collisions)
                {
                    GL.Enable(EnableCap.Lighting);
                    vtx[0].DrawAllElements(PrimitiveType.Triangles, BufferPointerFlags.Normal);
                    GL.Disable(EnableCap.Lighting);
                }

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

            //Scenery
            if (show_scenery && scenery != null)
            {
                GL.Enable(EnableCap.Lighting);
                for (int i = 0; i < sceneryObjects.Count; i++)
                {
                    sceneryObjects[i].DrawAllElements(PrimitiveType.Triangles, BufferPointerFlags.Normal);
                }
                GL.Disable(EnableCap.Lighting);
            }

            //object visuals
            if ((file.Data.Type == TwinsFile.FileType.RM2 || file.Data.Type == TwinsFile.FileType.RMX) && obj_models)
            {
                //GL.Enable(EnableCap.Lighting);
                for (int i = 5; i < vtx.Count; i++)
                {
                    vtx[i]?.DrawAllElements(PrimitiveType.Triangles, BufferPointerFlags.Normal);
                }
                //GL.Disable(EnableCap.Lighting);
            }

            //instances
            vtx[1].DrawMulti(PrimitiveType.LineStrip, BufferPointerFlags.Default);

            //positions + ai positions
            vtx[3].DrawMulti(PrimitiveType.LineLoop, BufferPointerFlags.Default);
            vtx[4].DrawMulti(PrimitiveType.LineLoop, BufferPointerFlags.Default);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.PushMatrix();

            

            uint mb_add = 0;
            if (file.Data.Type == TwinsFile.FileType.MonkeyBallRM) mb_add = 1;
            for (uint i = mb_add; i <= mb_add + 7; ++i)
            {
                if (file.Data.ContainsItem(i))
                {
                    Color cur_color;
                    if (file.Data.GetItem<TwinsSection>(i).ContainsItem(1)) //aipositions
                    {
                        foreach (AIPosition pos in file.Data.GetItem<TwinsSection>(i).GetItem<TwinsSection>(1).Records)
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

                    if (file.Data.GetItem<TwinsSection>(i).ContainsItem(2)) //aipaths
                    {
                        foreach (AIPath pth in file.Data.GetItem<TwinsSection>(i).GetItem<TwinsSection>(2).Records)
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

                    if (file.Data.GetItem<TwinsSection>(i).ContainsItem(3)) //positions
                    {
                        foreach (Position pos in file.Data.GetItem<TwinsSection>(i).GetItem<TwinsSection>(3).Records)
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

                    if (file.Data.GetItem<TwinsSection>(i).ContainsItem(4)) //paths
                    {
                        foreach (Path pth in file.Data.GetItem<TwinsSection>(i).GetItem<TwinsSection>(4).Records)
                        {
                            for (int k = 0; k < pth.Positions.Count; ++k)
                            {
                                DrawAxes(-pth.Positions[k].X, pth.Positions[k].Y, pth.Positions[k].Z, 0.5f);
                                if (file.SelectedItem != pth || file.SelectedItemArg != k)
                                    cur_color = colors[colors.Length - i * 2 - 1];
                                else
                                    cur_color = Color.White;
                                RenderString3D($"{pth.ID}:{k}", cur_color, -pth.Positions[k].X, pth.Positions[k].Y, pth.Positions[k].Z, ref identity_mat, 0.5F);
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

                    if (file.Data.Type != TwinsFile.FileType.DemoRM2 && file.Data.GetItem<TwinsSection>(i).ContainsItem(6)) //instances
                    {
                        if (file.Data.Type == TwinsFile.FileType.MonkeyBallRM)
                        {
                            foreach (InstanceMB ins in file.Data.GetItem<TwinsSection>(i).GetItem<TwinsSection>(6).Records)
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
                        else
                        {
                            foreach (Instance ins in file.Data.GetItem<TwinsSection>(i).GetItem<TwinsSection>(6).Records)
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

                    if (show_triggers && file.Data.GetItem<TwinsSection>(i).ContainsItem(7))
                    {
                        foreach (Trigger trg in file.Data.GetItem<TwinsSection>(i).GetItem<TwinsSection>(7).Records)
                        {
                            GL.PushMatrix();
                            GL.Translate(-trg.Coords[1].X, trg.Coords[1].Y, trg.Coords[1].Z);
                            Quaternion quat = new Quaternion(trg.Coords[0].X, -trg.Coords[0].Y, -trg.Coords[0].Z, trg.Coords[0].W);
                            Matrix4 new_mat = Matrix4.CreateFromQuaternion(quat);
                            GL.MultMatrix(ref new_mat);


                            cur_color = file.SelectedItem == trg ? Color.White : colors[colors.Length - i * 2 - 1];
                            GL.DepthMask(false);
                            GL.Enable(EnableCap.Lighting);
                            GL.Color4(cur_color.R, cur_color.G, cur_color.B, (byte)95);
                            GL.Begin(PrimitiveType.QuadStrip);
                            GL.Vertex3(-trg.Coords[2].X, -trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, -trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, -trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, -trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, -trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, -trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.End();
                            GL.Begin(PrimitiveType.Quads);

                            GL.Vertex3(-trg.Coords[2].X, -trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, -trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, trg.Coords[2].Y, -trg.Coords[2].Z);

                            GL.Vertex3(trg.Coords[2].X, -trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, -trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, trg.Coords[2].Y, -trg.Coords[2].Z);

                            GL.End();
                            GL.DepthMask(true);
                            GL.Disable(EnableCap.Lighting);

                            GL.Color4(cur_color);
                            GL.LineWidth(1);

                            GL.Begin(PrimitiveType.LineStrip);
                            GL.Vertex3(-trg.Coords[2].X, trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, -trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, -trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, -trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, -trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, -trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.End();
                            GL.Begin(PrimitiveType.Lines);
                            GL.Vertex3(-trg.Coords[2].X, -trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(-trg.Coords[2].X, trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, -trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, trg.Coords[2].Y, -trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, -trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.Vertex3(trg.Coords[2].X, trg.Coords[2].Y, trg.Coords[2].Z);
                            GL.End();
                            
                            GL.PopMatrix();
                            GL.LineWidth(2);
                            GL.Begin(PrimitiveType.Lines);
                            if (file.Data.Type != TwinsFile.FileType.MonkeyBallRM)
                            {
                                foreach (var id in trg.Instances)
                                {
                                    Pos inst = file.GetInstancePos(trg.Parent.Parent.ID, id);
                                    GL.Vertex3(-trg.Coords[1].X, trg.Coords[1].Y, trg.Coords[1].Z);
                                    GL.Vertex3(-inst.X, inst.Y, inst.Z);
                                }
                            }
                            GL.End();
                            GL.LineWidth(1);
                            DrawAxes(-trg.Coords[1].X, trg.Coords[1].Y, trg.Coords[1].Z, Math.Min(trg.Coords[2].X, Math.Min(trg.Coords[2].Y, trg.Coords[2].Z)) / 2);
                            Matrix3 rot_mat = Matrix3.CreateFromQuaternion(quat);
                            RenderString3D(trg.ID.ToString(), cur_color, -trg.Coords[1].X, trg.Coords[1].Y, trg.Coords[1].Z, ref rot_mat);
                        }
                    }

                    if (show_cams && file.Data.GetItem<TwinsSection>(i).ContainsItem(8))
                    {
                        if (file.Data.Type != TwinsFile.FileType.DemoSM2)
                        {
                            foreach (Camera cam in file.Data.GetItem<TwinsSection>(i).GetItem<TwinsSection>(8).Records)
                            {
                                GL.PushMatrix();
                                GL.Translate(-cam.Coords[1].X, cam.Coords[1].Y, cam.Coords[1].Z);
                                Quaternion quat = new Quaternion(cam.Coords[0].X, -cam.Coords[0].Y, -cam.Coords[0].Z, cam.Coords[0].W);
                                Matrix4 new_mat = Matrix4.CreateFromQuaternion(quat);
                                GL.MultMatrix(ref new_mat);

                                cur_color = file.SelectedItem == cam ? Color.White : colors[colors.Length - i * 2 - 2];
                                GL.DepthMask(false);
                                GL.Enable(EnableCap.Lighting);
                                GL.Color4(cur_color.R, cur_color.G, cur_color.B, (byte)95);
                                GL.Begin(PrimitiveType.QuadStrip);
                                GL.Vertex3(-cam.Coords[2].X, -cam.Coords[2].Y, -cam.Coords[2].Z);
                                GL.Vertex3(cam.Coords[2].X, -cam.Coords[2].Y, -cam.Coords[2].Z);
                                GL.Vertex3(-cam.Coords[2].X, cam.Coords[2].Y, -cam.Coords[2].Z);
                                GL.Vertex3(cam.Coords[2].X, cam.Coords[2].Y, -cam.Coords[2].Z);
                                GL.Vertex3(-cam.Coords[2].X, cam.Coords[2].Y, cam.Coords[2].Z);
                                GL.Vertex3(cam.Coords[2].X, cam.Coords[2].Y, cam.Coords[2].Z);
                                GL.Vertex3(-cam.Coords[2].X, -cam.Coords[2].Y, cam.Coords[2].Z);
                                GL.Vertex3(cam.Coords[2].X, -cam.Coords[2].Y, cam.Coords[2].Z);
                                GL.Vertex3(-cam.Coords[2].X, -cam.Coords[2].Y, -cam.Coords[2].Z);
                                GL.Vertex3(cam.Coords[2].X, -cam.Coords[2].Y, -cam.Coords[2].Z);
                                GL.End();
                                GL.Begin(PrimitiveType.Quads);

                                GL.Vertex3(-cam.Coords[2].X, -cam.Coords[2].Y, -cam.Coords[2].Z);
                                GL.Vertex3(-cam.Coords[2].X, -cam.Coords[2].Y, cam.Coords[2].Z);
                                GL.Vertex3(-cam.Coords[2].X, cam.Coords[2].Y, cam.Coords[2].Z);
                                GL.Vertex3(-cam.Coords[2].X, cam.Coords[2].Y, -cam.Coords[2].Z);

                                GL.Vertex3(cam.Coords[2].X, -cam.Coords[2].Y, -cam.Coords[2].Z);
                                GL.Vertex3(cam.Coords[2].X, -cam.Coords[2].Y, cam.Coords[2].Z);
                                GL.Vertex3(cam.Coords[2].X, cam.Coords[2].Y, cam.Coords[2].Z);
                                GL.Vertex3(cam.Coords[2].X, cam.Coords[2].Y, -cam.Coords[2].Z);

                                GL.End();
                                GL.DepthMask(true);
                                GL.Disable(EnableCap.Lighting);

                                GL.Color4(cur_color);
                                GL.LineWidth(1);

                                GL.Begin(PrimitiveType.LineStrip);
                                GL.Vertex3(-cam.Coords[2].X, cam.Coords[2].Y, -cam.Coords[2].Z);
                                GL.Vertex3(cam.Coords[2].X, cam.Coords[2].Y, -cam.Coords[2].Z);
                                GL.Vertex3(cam.Coords[2].X, cam.Coords[2].Y, cam.Coords[2].Z);
                                GL.Vertex3(-cam.Coords[2].X, cam.Coords[2].Y, cam.Coords[2].Z);
                                GL.Vertex3(-cam.Coords[2].X, cam.Coords[2].Y, -cam.Coords[2].Z);
                                GL.Vertex3(-cam.Coords[2].X, -cam.Coords[2].Y, -cam.Coords[2].Z);
                                GL.Vertex3(cam.Coords[2].X, -cam.Coords[2].Y, -cam.Coords[2].Z);
                                GL.Vertex3(cam.Coords[2].X, -cam.Coords[2].Y, cam.Coords[2].Z);
                                GL.Vertex3(-cam.Coords[2].X, -cam.Coords[2].Y, cam.Coords[2].Z);
                                GL.Vertex3(-cam.Coords[2].X, -cam.Coords[2].Y, -cam.Coords[2].Z);
                                GL.End();
                                GL.Begin(PrimitiveType.Lines);
                                GL.Vertex3(-cam.Coords[2].X, -cam.Coords[2].Y, cam.Coords[2].Z);
                                GL.Vertex3(-cam.Coords[2].X, cam.Coords[2].Y, cam.Coords[2].Z);
                                GL.Vertex3(cam.Coords[2].X, -cam.Coords[2].Y, -cam.Coords[2].Z);
                                GL.Vertex3(cam.Coords[2].X, cam.Coords[2].Y, -cam.Coords[2].Z);
                                GL.Vertex3(cam.Coords[2].X, -cam.Coords[2].Y, cam.Coords[2].Z);
                                GL.Vertex3(cam.Coords[2].X, cam.Coords[2].Y, cam.Coords[2].Z);
                                GL.End();

                                GL.PopMatrix();
                                //DrawAxes(-cam.Coords[1].X, cam.Coords[1].Y, cam.Coords[1].Z, Math.Min(cam.Coords[2].X, Math.Min(cam.Coords[2].Y, cam.Coords[2].Z)) / 2);
                                Matrix3 rot_mat = Matrix3.CreateFromQuaternion(quat);
                                RenderString3D(cam.ID.ToString(), cur_color, -cam.Coords[1].X, cam.Coords[1].Y, cam.Coords[1].Z, ref rot_mat);

                                GL.PushMatrix();
                                GL.Translate(0, 0, 0);
                                Matrix4 resetMat = Matrix4.Identity;
                                GL.MultMatrix(ref resetMat);
                                foreach (object camera in cam.Cameras)
                                {

                                    GL.LineWidth(1);
                                    cur_color = colors[colors.Length - i * 2 - 2];
                                    GL.Color3(cur_color);
                                    if (camera is Camera.Camera_Path CameraPolyline)
                                    {
                                        GL.PointSize(5);
                                        for (int p = 0; p < CameraPolyline.unkVectors.Length - 1; p++)
                                        {
                                            Pos Point1 = CameraPolyline.unkVectors[p];
                                            Pos Point2 = CameraPolyline.unkVectors[p + 1];
                                            GL.Begin(PrimitiveType.Lines);
                                            GL.Vertex3(-Point1.X, Point1.Y, Point1.Z);
                                            GL.Vertex3(-Point2.X, Point2.Y, Point2.Z);
                                            GL.End();
                                            GL.Begin(PrimitiveType.Points);
                                            GL.Vertex3(-Point1.X, Point1.Y, Point1.Z);
                                            if (p == CameraPolyline.unkVectors.Length - 1)
                                            {
                                                GL.Vertex3(-Point2.X, Point2.Y, Point2.Z);
                                            }
                                            GL.End();
                                        }
                                    }
                                    else if (camera is Camera.Camera_Point CameraPoint)
                                    {
                                        GL.PointSize(10);
                                        Pos Point = CameraPoint.unkVector;
                                        Pos Source = new Pos(-cam.Coords[1].X, cam.Coords[1].Y, cam.Coords[1].Z, cam.Coords[1].W);
                                        GL.Begin(PrimitiveType.Points);
                                        GL.Vertex3(-Point.X, Point.Y, Point.Z);
                                        GL.End();
                                        GL.Begin(PrimitiveType.Lines);
                                        GL.Vertex3(-Point.X, Point.Y, Point.Z);
                                        GL.Vertex3(Source.X, Source.Y, Source.Z);
                                        GL.End();
                                    }
                                    else if (camera is Camera.Camera_Spline CameraSpline)
                                    {
                                        GL.PointSize(5);
                                        for (int p = 0; p < CameraSpline.unkVectors.Length - 3; p++)
                                        {
                                            Pos Point1 = CameraSpline.unkVectors[p];
                                            Pos Point2 = CameraSpline.unkVectors[p + 2];
                                            GL.Begin(PrimitiveType.Lines);
                                            GL.Vertex3(-Point1.X, Point1.Y, Point1.Z);
                                            GL.Vertex3(-Point2.X, Point2.Y, Point2.Z);
                                            GL.End();
                                            GL.Begin(PrimitiveType.Points);
                                            GL.Vertex3(-Point1.X, Point1.Y, Point1.Z);
                                            if (p == CameraSpline.unkVectors.Length - 2)
                                            {
                                                GL.Vertex3(-Point2.X, Point2.Y, Point2.Z);
                                            }
                                            GL.End();
                                            p++;
                                        }
                                    }
                                    else if (camera is Camera.Camera_Point2 CameraPoint2)
                                    {
                                        GL.PointSize(10);
                                        Pos Point = CameraPoint2.unkVector;
                                        Pos Source = new Pos(-cam.Coords[1].X, cam.Coords[1].Y, cam.Coords[1].Z, cam.Coords[1].W);
                                        GL.Begin(PrimitiveType.Points);
                                        GL.Vertex3(-Point.X, Point.Y, Point.Z);
                                        GL.End();
                                        GL.Begin(PrimitiveType.Lines);
                                        GL.Vertex3(-Point.X, Point.Y, Point.Z);
                                        GL.Vertex3(Source.X, Source.Y, Source.Z);
                                        GL.End();
                                    }
                                    else if (camera is Camera.Camera_Line CameraLine)
                                    {
                                        GL.PointSize(10);
                                        Pos Point1 = CameraLine.unkBoundingBoxVector1;
                                        Pos Point2 = CameraLine.unkBoundingBoxVector2;
                                        GL.Begin(PrimitiveType.Points);
                                        GL.Vertex3(-Point1.X, Point1.Y, Point1.Z);
                                        GL.Vertex3(-Point2.X, Point2.Y, Point2.Z);
                                        GL.End();
                                        GL.Begin(PrimitiveType.Lines);
                                        GL.Vertex3(-Point1.X, Point1.Y, Point1.Z);
                                        GL.Vertex3(-Point2.X, Point2.Y, Point2.Z);
                                        GL.End();
                                    }
                                    else if (camera is Camera.Camera_Line2 CameraLine2)
                                    {
                                        GL.PointSize(10);
                                        Pos Point1 = CameraLine2.unkBoundingBoxVector1;
                                        Pos Point2 = CameraLine2.unkBoundingBoxVector2;
                                        GL.Begin(PrimitiveType.Points);
                                        GL.Vertex3(-Point1.X, Point1.Y, Point1.Z);
                                        GL.Vertex3(-Point2.X, Point2.Y, Point2.Z);
                                        GL.End();
                                        GL.Begin(PrimitiveType.Lines);
                                        GL.Vertex3(-Point1.X, Point1.Y, Point1.Z);
                                        GL.Vertex3(-Point2.X, Point2.Y, Point2.Z);
                                        GL.End();
                                    }
                                    else if (camera is Camera.Camera_Zone CameraZone)
                                    {
                                        // May be wrong?
                                        GL.PointSize(10);
                                        Pos Point1 = CameraZone.Data1_Vectors[3];
                                        Pos Point2 = CameraZone.Data2_Vectors[3];
                                        GL.Begin(PrimitiveType.Points);
                                        GL.Vertex3(-Point1.X, Point1.Y, Point1.Z);
                                        GL.Vertex3(-Point2.X, Point2.Y, Point2.Z);
                                        GL.End();
                                        GL.Begin(PrimitiveType.Lines);
                                        GL.Vertex3(-Point1.X, Point1.Y, Point1.Z);
                                        GL.Vertex3(-Point2.X, Point2.Y, Point2.Z);
                                        GL.End();
                                    }

                                }
                                GL.PopMatrix();

                            }
                        }
                    }
                }
            }

            //Draw chunk links if available
            if (sm2_links && links != null)
            {
                GL.LineWidth(2);
                GL.DepthMask(false);
                foreach (var l in links.Links)
                {
                    Color cur_color = colors[(links.Links.IndexOf(l) + 2) % colors.Length];
                    GL.PushMatrix();
                    GL.Scale(-1, 1, 1);
                    if (l.HasWall)
                    {
                        GL.Color4(Color.FromArgb(95, cur_color));
                        GL.Begin(PrimitiveType.Quads);
                        GL.Vertex4(l.LoadWall[0].ToArray());
                        GL.Vertex4(l.LoadWall[1].ToArray());
                        GL.Vertex4(l.LoadWall[2].ToArray());
                        GL.Vertex4(l.LoadWall[3].ToArray());
                        GL.End();
                        GL.Color4(cur_color);
                        GL.Begin(PrimitiveType.LineLoop);
                        GL.Vertex4(l.LoadWall[0].ToArray());
                        GL.Vertex4(l.LoadWall[1].ToArray());
                        GL.Vertex4(l.LoadWall[2].ToArray());
                        GL.Vertex4(l.LoadWall[3].ToArray());
                        GL.End();
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex4(l.LoadWall[0].ToArray());
                        GL.Vertex4(l.LoadWall[2].ToArray());
                        GL.Vertex4(l.LoadWall[1].ToArray());
                        GL.Vertex4(l.LoadWall[3].ToArray());
                        GL.End();
                        Matrix3 rot_mat = Matrix3.Identity;
                        rot_mat *= Matrix3.CreateRotationX(-rot.Y / 180 * MathHelper.Pi);
                        rot_mat *= Matrix3.CreateRotationY(-rot.X / 180 * MathHelper.Pi);
                        rot_mat *= Matrix3.CreateRotationZ(rot.Z / 180 * MathHelper.Pi);
                        RenderString3D(l.Path, cur_color,
                            -(l.LoadWall[0].X + l.LoadWall[1].X + l.LoadWall[2].X + l.LoadWall[3].X) / 4,
                            (l.LoadWall[0].Y + l.LoadWall[1].Y + l.LoadWall[2].Y + l.LoadWall[3].Y) / 4,
                            (l.LoadWall[0].Z + l.LoadWall[1].Z + l.LoadWall[2].Z + l.LoadWall[3].Z) / 4,
                            ref rot_mat);
                    }
                    if (l.HasTree)
                    {
                        ChunkLinks.ChunkLink.LinkTree tree = l.TreeRoot;
                        while (tree != null)
                        {
                            GL.Begin(PrimitiveType.Lines);
                            for (int i = 0; i < 6; ++i)
                            {
                                switch (i)
                                {
                                    case 0: GL.Color4(Color.Red); break;
                                    case 1: GL.Color4(Color.Green); break;
                                    case 2: GL.Color4(Color.Blue); break;
                                    case 3: GL.Color4(Color.Yellow); break;
                                    case 4: GL.Color4(Color.Magenta); break;
                                    case 5: GL.Color4(Color.Cyan); break;
                                }
                                int i1 = i >= 4 ? 1 - (i - 4) : (0 + 2 * i) % 8;
                                int i2 = i >= 4 ? i1 + 2 : (1 + 2 * i) % 8;
                                int i3 = i >= 4 ? i2 + 2 : (2 + 2 * i) % 8;
                                int i4 = i >= 4 ? i3 + 2 : (3 + 2 * i) % 8;
                                Vector3 mid_vec = new Vector3(tree.LoadArea[i1].X + tree.LoadArea[i2].X + tree.LoadArea[i3].X + tree.LoadArea[i4].X,
                                    tree.LoadArea[i1].Y + tree.LoadArea[i2].Y + tree.LoadArea[i3].Y + tree.LoadArea[i4].Y,
                                    tree.LoadArea[i1].Z + tree.LoadArea[i2].Z + tree.LoadArea[i3].Z + tree.LoadArea[i4].Z) / 4;
                                Vector3 nor_vec = new Vector3(tree.AreaMatrix[i].X, tree.AreaMatrix[i].Y, tree.AreaMatrix[i].Z);
                                Vector3 unk_vec = new Vector3(tree.UnknownMatrix[i].X, tree.UnknownMatrix[i].Y, tree.UnknownMatrix[i].Z);
                                GL.Vertex3(mid_vec);
                                GL.Vertex3(mid_vec + nor_vec);
                                GL.Vertex3(mid_vec);
                                GL.Vertex3(mid_vec + unk_vec);
                            }
                            GL.End();
                            GL.Enable(EnableCap.Lighting);
                            GL.Color4(Color.FromArgb(95, cur_color));
                            GL.Begin(PrimitiveType.QuadStrip);
                            GL.Vertex4(tree.LoadArea[0].ToArray());
                            GL.Vertex4(tree.LoadArea[1].ToArray());
                            GL.Vertex4(tree.LoadArea[2].ToArray());
                            GL.Vertex4(tree.LoadArea[3].ToArray());
                            GL.Vertex4(tree.LoadArea[4].ToArray());
                            GL.Vertex4(tree.LoadArea[5].ToArray());
                            GL.Vertex4(tree.LoadArea[6].ToArray());
                            GL.Vertex4(tree.LoadArea[7].ToArray());
                            GL.Vertex4(tree.LoadArea[0].ToArray());
                            GL.Vertex4(tree.LoadArea[1].ToArray());
                            GL.End();
                            GL.Begin(PrimitiveType.Quads);
                            GL.Vertex4(tree.LoadArea[1].ToArray());
                            GL.Vertex4(tree.LoadArea[3].ToArray());
                            GL.Vertex4(tree.LoadArea[5].ToArray());
                            GL.Vertex4(tree.LoadArea[7].ToArray());
                            GL.Vertex4(tree.LoadArea[0].ToArray());
                            GL.Vertex4(tree.LoadArea[2].ToArray());
                            GL.Vertex4(tree.LoadArea[4].ToArray());
                            GL.Vertex4(tree.LoadArea[6].ToArray());
                            GL.End();
                            GL.Disable(EnableCap.Lighting);
                            GL.Color4(cur_color);
                            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                            GL.Begin(PrimitiveType.QuadStrip);
                            GL.Vertex4(tree.LoadArea[0].ToArray());
                            GL.Vertex4(tree.LoadArea[1].ToArray());
                            GL.Vertex4(tree.LoadArea[2].ToArray());
                            GL.Vertex4(tree.LoadArea[3].ToArray());
                            GL.Vertex4(tree.LoadArea[4].ToArray());
                            GL.Vertex4(tree.LoadArea[5].ToArray());
                            GL.Vertex4(tree.LoadArea[6].ToArray());
                            GL.Vertex4(tree.LoadArea[7].ToArray());
                            GL.Vertex4(tree.LoadArea[0].ToArray());
                            GL.Vertex4(tree.LoadArea[1].ToArray());
                            GL.End();
                            GL.Begin(PrimitiveType.Quads);
                            GL.Vertex4(tree.LoadArea[1].ToArray());
                            GL.Vertex4(tree.LoadArea[3].ToArray());
                            GL.Vertex4(tree.LoadArea[5].ToArray());
                            GL.Vertex4(tree.LoadArea[7].ToArray());
                            GL.Vertex4(tree.LoadArea[0].ToArray());
                            GL.Vertex4(tree.LoadArea[2].ToArray());
                            GL.Vertex4(tree.LoadArea[4].ToArray());
                            GL.Vertex4(tree.LoadArea[6].ToArray());
                            GL.End();
                            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                            GL.Begin(PrimitiveType.Lines);
                            GL.Vertex4(tree.LoadArea[0].ToArray());
                            GL.Vertex4(tree.LoadArea[3].ToArray());
                            GL.Vertex4(tree.LoadArea[1].ToArray());
                            GL.Vertex4(tree.LoadArea[2].ToArray());
                            GL.Vertex4(tree.LoadArea[2].ToArray());
                            GL.Vertex4(tree.LoadArea[5].ToArray());
                            GL.Vertex4(tree.LoadArea[3].ToArray());
                            GL.Vertex4(tree.LoadArea[4].ToArray());
                            GL.Vertex4(tree.LoadArea[4].ToArray());
                            GL.Vertex4(tree.LoadArea[7].ToArray());
                            GL.Vertex4(tree.LoadArea[5].ToArray());
                            GL.Vertex4(tree.LoadArea[6].ToArray());
                            GL.Vertex4(tree.LoadArea[6].ToArray());
                            GL.Vertex4(tree.LoadArea[1].ToArray());
                            GL.Vertex4(tree.LoadArea[7].ToArray());
                            GL.Vertex4(tree.LoadArea[0].ToArray());
                            GL.Vertex4(tree.LoadArea[0].ToArray());
                            GL.Vertex4(tree.LoadArea[4].ToArray());
                            GL.Vertex4(tree.LoadArea[2].ToArray());
                            GL.Vertex4(tree.LoadArea[6].ToArray());
                            GL.Vertex4(tree.LoadArea[1].ToArray());
                            GL.Vertex4(tree.LoadArea[5].ToArray());
                            GL.Vertex4(tree.LoadArea[3].ToArray());
                            GL.Vertex4(tree.LoadArea[7].ToArray());
                            GL.End();

                            if (tree.Next != null)
                            {
                                tree = tree.Next;
                            }
                            else
                                break;
                        }
                    }
                    GL.PopMatrix();
                }
                GL.DepthMask(true);
                GL.LineWidth(1);
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
                case Keys.Y:
                case Keys.F:
                case Keys.U:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.C: show_col_nodes = !show_col_nodes; break;
                case Keys.L: sm2_links = !sm2_links; break;
                case Keys.T: show_triggers = !show_triggers; break;
                case Keys.X: wire_col = !wire_col; break;
                case Keys.V: obj_models = !obj_models; break;
                case Keys.Y: show_cams = !show_cams; break;
                case Keys.F: collisions = !collisions; break;
                case Keys.U: show_scenery = !show_scenery; break;
            }
        }

        private void LoadScenery(Matrix4 chunkMatrix)
        {
            if (scenery.SceneryRoot == null)
            {
                return;
            }

            LoadSceneryStruct(scenery.SceneryRoot, chunkMatrix);
        }

        private void LoadSceneryStruct(SceneryData.SceneryStruct branch, Matrix4 chunkMatrix)
        {
            LoadSceneryModel(branch.Model, chunkMatrix);

            for (int i = 0; i < branch.Links.Length; i++)
            {
                if (branch.Links[i] is SceneryData.SceneryModelStruct modelStruct)
                {
                    LoadSceneryModel(modelStruct, chunkMatrix);
                }
                else if (branch.Links[i] is SceneryData.SceneryStruct @struct)
                {
                    LoadSceneryStruct(@struct, chunkMatrix);
                }
            }
        }

        private void LoadSceneryModel(SceneryData.SceneryModelStruct leaf, Matrix4 chunkMatrix)
        {
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;

            SectionController graphics_sec = file.DataAuxCont.GetItem<SectionController>(6);
            SectionController mesh_sec = graphics_sec.GetItem<SectionController>(2);
            SectionController model_sec = graphics_sec.GetItem<SectionController>(6);
            SectionController special_sec = graphics_sec.GetItem<SectionController>(7);

            if (file.DataAux.Type == TwinsFile.FileType.SM2 || file.DataAux.Type == TwinsFile.FileType.DemoSM2)
            {
                for (int m = 0; m < leaf.Models.Count; m++)
                {
                    ModelController mesh;
                    uint modelID;
                    if (!leaf.Models[m].isSpecial)
                    {
                        modelID = leaf.Models[m].ModelID;
                    }
                    else
                    {
                        //uint LODcount = special_sec.Data.GetItem<SpecialModel>(ptr.Models[m].ModelID).ModelsAmount;
                        //int targetLOD = LODcount == 1 ? 0 : 1;
                        modelID = special_sec.Data.GetItem<LodModel>(leaf.Models[m].ModelID).LODModelIDs[0];
                    }
                    mesh = mesh_sec.GetItem<ModelController>(model_sec.GetItem<RigidModelController>(modelID).Data.MeshID);

                    var rigid = model_sec.GetItem<RigidModelController>(modelID).Data;
                    mesh.LoadMeshData();

                    Matrix4 modelMatrix = Matrix4.Identity;

                    // closest: -M11, -M21, -M31, -X

                    // Rotation
                    modelMatrix.M11 = -leaf.Models[m].ModelMatrix[0].X;
                    modelMatrix.M12 = leaf.Models[m].ModelMatrix[1].X;
                    modelMatrix.M13 = leaf.Models[m].ModelMatrix[2].X;

                    modelMatrix.M21 = -leaf.Models[m].ModelMatrix[0].Y;
                    modelMatrix.M22 = leaf.Models[m].ModelMatrix[1].Y;
                    modelMatrix.M23 = leaf.Models[m].ModelMatrix[2].Y;

                    modelMatrix.M31 = -leaf.Models[m].ModelMatrix[0].Z;
                    modelMatrix.M32 = leaf.Models[m].ModelMatrix[1].Z;
                    modelMatrix.M33 = leaf.Models[m].ModelMatrix[2].Z;

                    modelMatrix.M14 = leaf.Models[m].ModelMatrix[0].W;
                    modelMatrix.M24 = leaf.Models[m].ModelMatrix[1].W;
                    modelMatrix.M34 = leaf.Models[m].ModelMatrix[2].W;

                    // Position
                    modelMatrix.M41 = leaf.Models[m].ModelMatrix[3].X;
                    modelMatrix.M42 = leaf.Models[m].ModelMatrix[3].Y;
                    modelMatrix.M43 = leaf.Models[m].ModelMatrix[3].Z;
                    modelMatrix.M44 = leaf.Models[m].ModelMatrix[3].W;

                    modelMatrix *= Matrix4.CreateScale(-1, 1, 1);


                    for (int v = 0; v < mesh.Vertices.Count; v++)
                    {
                        Vertex[] vbuffer = new Vertex[mesh.Vertices[v].Length];

                        for (int k = 0; k < mesh.Vertices[v].Length; k++)
                        {
                            vbuffer[k] = mesh.Vertices[v][k];
                            Vector4 vertexPos = new Vector4(mesh.Vertices[v][k].Pos.X, mesh.Vertices[v][k].Pos.Y, mesh.Vertices[v][k].Pos.Z, 1);
                            vertexPos *= modelMatrix;
                            mesh.Vertices[v][k].Pos = new Vector3(vertexPos.X, vertexPos.Y, vertexPos.Z);
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

                        int vtx_id = GenerateVBOforScenery();

                        if (rigid != null)
                        {
                            Utils.TextUtils.LoadTexture(rigid.MaterialIDs, file.DataAuxCont, sceneryObjects[vtx_id], v);
                        }

                        sceneryObjects[vtx_id].Vtx = mesh.Vertices[v];
                        sceneryObjects[vtx_id].VtxInd = mesh.Indices[v];
                        mesh.Vertices[v] = vbuffer;

                        UpdateVBOforScenery(vtx_id);
                    }
                }
            }
        }

        private readonly int sceneryLimit = 4000;
        private int sceneryIndex;
        private int GenerateVBOforScenery()
        {
            if (sceneryIndex >= sceneryLimit)
            {
                sceneryIndex = 0;
            }
            if (sceneryObjects.Count >= sceneryLimit)
            {
                sceneryObjects[sceneryIndex] = new VertexBufferData();
            }
            else
            {
                sceneryObjects.Add(new VertexBufferData());
            }
            return sceneryIndex++;
        }

        protected void UpdateVBOforScenery(int id)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, sceneryObjects[id].ID);
            if (sceneryObjects[id].Vtx.Length > sceneryObjects[id].LastSize)
                GL.BufferData(BufferTarget.ArrayBuffer, Vertex.SizeOf * sceneryObjects[id].Vtx.Length, sceneryObjects[id].Vtx, BufferUsageHint.StaticDraw);
            else
                GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)0, Vertex.SizeOf * sceneryObjects[id].Vtx.Length, sceneryObjects[id].Vtx);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            sceneryObjects[id].LastSize = sceneryObjects[id].Vtx.Length;
        }

        public void LoadColTree()
        {
            uint col_section = 9;
            if (file.Data.Type == TwinsFile.FileType.MonkeyBallRM) col_section = 10;
            ColData data = file.Data.GetItem<ColData>(col_section);
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
            UpdateVBO(0);
        }

        public void LoadInstances()
        {
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            bool[] record_exists = new bool[9];
            int inst_count = 0;
            uint mb_add = 0;
            if (file.Data.Type == TwinsFile.FileType.MonkeyBallRM) mb_add = 1;
            for (uint i = mb_add; i <= mb_add + 7; ++i)
            {
                record_exists[i] = file.Data.ContainsItem(i);
                if (record_exists[i])
                {
                    if (file.Data.GetItem<TwinsSection>(i).ContainsItem(6))
                        inst_count += file.Data.GetItem<TwinsSection>(i).GetItem<TwinsSection>(6).Records.Count;
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
            int l = 0, m = 0, cur_instance = 0;
            for (uint i = mb_add; i <= mb_add + 7; ++i)
            {
                if (!record_exists[i]) continue;
                if (file.Data.GetItem<TwinsSection>(i).ContainsItem(6))
                {
                    if (file.Data.Type == TwinsFile.FileType.MonkeyBallRM)
                    {
                        foreach (InstanceMB ins in file.Data.GetItem<TwinsSection>(i).GetItem<TwinsSection>(6).Records)
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
                    else if (file.Data.Type != TwinsFile.FileType.DemoRM2)
                    {
                        foreach (Instance ins in file.Data.GetItem<TwinsSection>(i).GetItem<TwinsSection>(6).Records)
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

                            if (file.Data.Type == TwinsFile.FileType.RM2 || file.Data.Type == TwinsFile.FileType.RMX)
                            {
                                bool HasMesh = false;
                                ushort TargetGI = 65535;
                                List<uint> ModelList = new List<uint>();
                                List<Matrix4> LocalRotList = new List<Matrix4>();
                                uint TargetModel = 65535;
                                uint TargetSkin = 0;
                                uint TargetBSkin = 0;
                                bool HasSkin = false;
                                bool HasBlendSkin = false;
                                Matrix4 LocalRot = Matrix4.Identity;
                                FileController targetFile = file;
                                Vector4 pos_ins_4 = ins.Pos.ToVec4();
                                pos_ins_4.X = -pos_ins_4.X;
                                Matrix4 rot_ins_4 = Matrix4.Identity;
                                rot_ins_4 *= Matrix4.CreateRotationX(ins.RotX / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi);
                                rot_ins_4 *= Matrix4.CreateRotationY(-ins.RotY / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi);
                                rot_ins_4 *= Matrix4.CreateRotationZ(-ins.RotZ / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi);

                                foreach (GameObject gameObject in targetFile.Data.GetItem<TwinsSection>(10).GetItem<TwinsSection>(0).Records)
                                {
                                    if (gameObject.ID == ins.ObjectID)
                                    {
                                        if (gameObject.OGIs.Count > 0 && gameObject.OGIs[0] != 65535)
                                        {
                                            if (ins.UnkI323[0] != 0)
                                            {
                                                if (gameObject.OGIs.Count > ins.UnkI323[0] && gameObject.OGIs[(int)ins.UnkI323[0]] != 65535)
                                                {
                                                    TargetGI = gameObject.OGIs[(int)ins.UnkI323[0]];
                                                }
                                                else
                                                {
                                                    TargetGI = gameObject.OGIs[0];
                                                }
                                            }
                                            else
                                            {
                                                TargetGI = gameObject.OGIs[0];
                                            }
                                        }
                                    }
                                }

                                if (TargetGI == 65535)
                                {
                                    if (file.DataDefault != null)
                                    {
                                        targetFile = file.DefaultCont;
                                        foreach (GameObject gameObject in file.DataDefault.GetItem<TwinsSection>(10).GetItem<TwinsSection>(0).Records)
                                        {
                                            if (gameObject.ID == ins.ObjectID)
                                            {
                                                if (gameObject.OGIs.Count > 0 && gameObject.OGIs[0] != 65535)
                                                {
                                                    TargetGI = gameObject.OGIs[0];
                                                }
                                            }
                                        }
                                    }
                                }

                                if (TargetGI != 65535)
                                {
                                    foreach (GraphicsInfo GI in targetFile.Data.GetItem<TwinsSection>(10).GetItem<TwinsSection>(3).Records)
                                    {
                                        if (GI.ID == TargetGI)
                                        {
                                            if (GI.BlendSkinID != 0)
                                            {
                                                HasBlendSkin = true;
                                                TargetBSkin = GI.BlendSkinID;
                                            }
                                            if (GI.SkinID != 0)
                                            {
                                                HasSkin = true;
                                                TargetSkin = GI.SkinID;
                                            }
                                            if (GI.ModelIDs.Count > 0)
                                            {
                                                foreach (var pair in GI.ModelIDs)
                                                {
                                                    ModelList.Add(pair.Value.ModelID);
                                                    Matrix4 tempRot = Matrix4.Identity;

                                                    // Rotation
                                                    tempRot.M11 = -GI.SkinTransforms[pair.Value.JointIndex].Matrix[0].X;
                                                    tempRot.M12 = -GI.SkinTransforms[pair.Value.JointIndex].Matrix[1].X;
                                                    tempRot.M13 = -GI.SkinTransforms[pair.Value.JointIndex].Matrix[2].X;

                                                    tempRot.M21 = GI.SkinTransforms[pair.Value.JointIndex].Matrix[0].Y;
                                                    tempRot.M22 = GI.SkinTransforms[pair.Value.JointIndex].Matrix[1].Y;
                                                    tempRot.M23 = GI.SkinTransforms[pair.Value.JointIndex].Matrix[2].Y;

                                                    tempRot.M31 = GI.SkinTransforms[pair.Value.JointIndex].Matrix[0].Z;
                                                    tempRot.M32 = GI.SkinTransforms[pair.Value.JointIndex].Matrix[1].Z;
                                                    tempRot.M33 = GI.SkinTransforms[pair.Value.JointIndex].Matrix[2].Z;

                                                    tempRot.M14 = GI.SkinTransforms[pair.Value.JointIndex].Matrix[0].W;
                                                    tempRot.M24 = GI.SkinTransforms[pair.Value.JointIndex].Matrix[1].W;
                                                    tempRot.M34 = GI.SkinTransforms[pair.Value.JointIndex].Matrix[2].W;

                                                    // Position
                                                    tempRot.M41 = GI.Joints[pair.Value.JointIndex].Matrix[1].X;
                                                    tempRot.M42 = GI.Joints[pair.Value.JointIndex].Matrix[1].Y;
                                                    tempRot.M43 = GI.Joints[pair.Value.JointIndex].Matrix[1].Z;
                                                    tempRot.M44 = GI.Joints[pair.Value.JointIndex].Matrix[1].W;

                                                    // Adjusted for OpenTK
                                                    tempRot *= Matrix4.CreateScale(-1, 1, 1);

                                                    LocalRotList.Add(tempRot);
                                                }
                                            }
                                        }
                                    }

                                    if (ModelList.Count > 0)
                                    {
                                        for (int modelID = 0; modelID < ModelList.Count; modelID++)
                                        {
                                            HasMesh = false;
                                            TargetModel = ModelList[modelID];
                                            LocalRot = LocalRotList[modelID];

                                            if (file.Data.Type == TwinsFile.FileType.RM2)
                                            {
                                                ModelController modelCont = null;
                                                SectionController mesh_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(2);
                                                RigidModel rigid = null;
                                                foreach (RigidModel model in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).Records)
                                                {
                                                    if (model.ID == TargetModel)
                                                    {
                                                        rigid = targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).GetItem<RigidModel>(TargetModel);
                                                        uint meshID = rigid.MeshID;
                                                        modelCont = mesh_sec.GetItem<ModelController>(meshID);
                                                        HasMesh = true;
                                                    }
                                                }

                                                if (HasMesh)
                                                {
                                                    modelCont.LoadMeshData();
                                                    

                                                    for (int v = 0; v < modelCont.Vertices.Count; v++)
                                                    {
                                                        Vertex[] vbuffer = new Vertex[modelCont.Vertices[v].Length];
                                                        for (int p = 0; p < modelCont.Vertices[v].Length; p++)
                                                        {
                                                            vbuffer[p] = modelCont.Vertices[v][p];
                                                            Vector4 targetPos = new Vector4(modelCont.Vertices[v][p].Pos.X, modelCont.Vertices[v][p].Pos.Y, modelCont.Vertices[v][p].Pos.Z, 1);

                                                            targetPos *= LocalRot;

                                                            bool rotationOverride = false;

                                                            if (!rotationOverride)
                                                            {
                                                                targetPos *= rot_ins_4;
                                                            }

                                                            targetPos += pos_ins_4;
                                                            modelCont.Vertices[v][p].Pos = new Vector3(targetPos.X, targetPos.Y, targetPos.Z);
                                                            if (ins.ObjectID == (ushort)DefaultEnums.ObjectID.TNTCRATE)
                                                            {
                                                                modelCont.Vertices[v][p].Col = Vertex.ColorToABGR(Color.Red);
                                                            }
                                                            else if (ins.ObjectID == (ushort)DefaultEnums.ObjectID.NITROCRATE)
                                                            {
                                                                modelCont.Vertices[v][p].Col = Vertex.ColorToABGR(Color.Green);
                                                            }
                                                            else if (WoodCrates.Contains((DefaultEnums.ObjectID)ins.ObjectID))
                                                            {
                                                                modelCont.Vertices[v][p].Col = Vertex.ColorToABGR(Color.SandyBrown);
                                                            }
                                                        }
                                                        
                                                        vtx[5 + cur_instance] = new VertexBufferData();
                                                        vtx[5 + cur_instance].Vtx = modelCont.Vertices[v];
                                                        vtx[5 + cur_instance].VtxInd = modelCont.Indices[v];
                                                        modelCont.Vertices[v] = vbuffer;
                                                        Utils.TextUtils.LoadTexture(rigid.MaterialIDs, file, vtx[5 + cur_instance], v);
                                                        UpdateVBO(5 + cur_instance);
                                                        cur_instance++;
                                                    }
                                                    
                                                    
                                                    
                                                }
                                                else
                                                {
                                                    vtx[5 + cur_instance] = null;
                                                }
                                            }
                                            else if (file.Data.Type == TwinsFile.FileType.RMX)
                                            {
                                                ModelXController modelCont = null;
                                                SectionController mesh_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(2);
                                                foreach (Twinsanity.RigidModel model in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).Records)
                                                {
                                                    if (model.ID == TargetModel)
                                                    {
                                                        uint meshID = targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).GetItem<Twinsanity.RigidModel>(TargetModel).MeshID;
                                                        modelCont = mesh_sec.GetItem<ModelXController>(meshID);
                                                        HasMesh = true;
                                                    }
                                                }

                                                if (HasMesh)
                                                {
                                                    modelCont.LoadMeshData();
                                                    Vertex[] vbuffer = new Vertex[modelCont.Vertices.Length];

                                                    for (int v = 0; v < modelCont.Vertices.Length; v++)
                                                    {
                                                        vbuffer[v] = modelCont.Vertices[v];
                                                        Vector4 targetPos = new Vector4(modelCont.Vertices[v].Pos.X, modelCont.Vertices[v].Pos.Y, modelCont.Vertices[v].Pos.Z, 1);

                                                        targetPos *= LocalRot;

                                                        bool rotationOverride = false;
                                                        if (ins.ObjectID == (ushort)DefaultEnums.ObjectID.ICICLE)
                                                        {
                                                            if (ins.UnkI323[0] == 1)
                                                            {
                                                                Matrix4 rot_ins_fix = Matrix4.Identity;
                                                                rot_ins_fix *= Matrix4.CreateRotationZ((32768) / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi);
                                                                targetPos *= rot_ins_fix;
                                                                rotationOverride = true;
                                                            }
                                                        }

                                                        if (!rotationOverride)
                                                        {
                                                            targetPos *= rot_ins_4;
                                                        }

                                                        targetPos += pos_ins_4;
                                                        modelCont.Vertices[v].Pos = new Vector3(targetPos.X, targetPos.Y, targetPos.Z);
                                                        if (ins.ObjectID == (ushort)DefaultEnums.ObjectID.TNTCRATE)
                                                        {
                                                            modelCont.Vertices[v].Col = Vertex.ColorToABGR(Color.Red);
                                                        }
                                                        else if (ins.ObjectID == (ushort)DefaultEnums.ObjectID.NITROCRATE)
                                                        {
                                                            modelCont.Vertices[v].Col = Vertex.ColorToABGR(Color.Green);
                                                        }
                                                        else if (WoodCrates.Contains((DefaultEnums.ObjectID)ins.ObjectID))
                                                        {
                                                            modelCont.Vertices[v].Col = Vertex.ColorToABGR(Color.SandyBrown);
                                                        }
                                                    }
                                                    vtx[5 + cur_instance] = new VertexBufferData();
                                                    vtx[5 + cur_instance].Vtx = modelCont.Vertices;
                                                    vtx[5 + cur_instance].VtxInd = modelCont.Indices;
                                                    modelCont.Vertices = vbuffer;
                                                    UpdateVBO(5 + cur_instance);
                                                }
                                                else
                                                {
                                                    vtx[5 + cur_instance] = null;
                                                }
                                            }

                                            cur_instance++;
                                        }
                                    }

                                    if (HasSkin && file.Data.Type == TwinsFile.FileType.RM2)
                                    {
                                        SkinController modelCont = null;
                                        SectionController meshSec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(4);
                                        foreach (Twinsanity.Skin mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(4).Records.Cast<Twinsanity.Skin>())
                                        {
                                            if (mod.ID == TargetSkin)
                                            {
                                                modelCont = meshSec.GetItem<SkinController>(mod.ID);
                                            }
                                        }

                                        modelCont.LoadMeshData();

                                        for (int v = 0; v < modelCont.Vertices.Count; v++)
                                        {
                                            Vertex[] vbuffer = new Vertex[modelCont.Vertices[v].Length];
                                            for (int p = 0; p < modelCont.Vertices[v].Length; p++)
                                            {
                                                vbuffer[p] = modelCont.Vertices[v][p];
                                                Vector4 targetPos = new Vector4(modelCont.Vertices[v][p].Pos.X, modelCont.Vertices[v][p].Pos.Y, modelCont.Vertices[v][p].Pos.Z, 1);

                                                //targetPos *= LocalRot;

                                                bool rotationOverride = false;

                                                if (!rotationOverride)
                                                {
                                                    targetPos *= rot_ins_4;
                                                }

                                                targetPos += pos_ins_4;
                                                modelCont.Vertices[v][p].Pos = new Vector3(targetPos.X, targetPos.Y, targetPos.Z);
                                                if (ins.ObjectID == (ushort)DefaultEnums.ObjectID.TNTCRATE)
                                                {
                                                    modelCont.Vertices[v][p].Col = Vertex.ColorToABGR(Color.Red);
                                                }
                                                else if (ins.ObjectID == (ushort)DefaultEnums.ObjectID.NITROCRATE)
                                                {
                                                    modelCont.Vertices[v][p].Col = Vertex.ColorToABGR(Color.Green);
                                                }
                                                else if (WoodCrates.Contains((DefaultEnums.ObjectID)ins.ObjectID))
                                                {
                                                    modelCont.Vertices[v][p].Col = Vertex.ColorToABGR(Color.SandyBrown);
                                                }
                                            }
                                            
                                            vtx[5 + cur_instance] = new VertexBufferData();
                                            vtx[5 + cur_instance].Vtx = modelCont.Vertices[v];
                                            vtx[5 + cur_instance].VtxInd = modelCont.Indices[v];
                                            Utils.TextUtils.LoadTexture(modelCont.Data.SubModels.Select((subModel) =>
                                            {
                                                return subModel.MaterialID;
                                            }).ToArray(), file, vtx[5 + cur_instance], v);
                                            modelCont.Vertices[v] = vbuffer;
                                            UpdateVBO(5 + cur_instance);
                                            cur_instance++;
                                        }
                                    }

                                    if (HasSkin && file.Data.Type == TwinsFile.FileType.RMX)
                                    {
                                        SkinXController modelCont = null;
                                        SectionController mesh_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(4);
                                        foreach (SkinX mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(4).Records)
                                        {
                                            if (mod.ID == TargetSkin)
                                            {
                                                modelCont = mesh_sec.GetItem<SkinXController>(mod.ID);
                                            }
                                        }

                                        modelCont.LoadMeshData();
                                        Vertex[] vbuffer = new Vertex[modelCont.Vertices.Length];

                                        for (int v = 0; v < modelCont.Vertices.Length; v++)
                                        {
                                            vbuffer[v] = modelCont.Vertices[v];
                                            Vector4 targetPos = new Vector4(modelCont.Vertices[v].Pos.X, modelCont.Vertices[v].Pos.Y, modelCont.Vertices[v].Pos.Z, 1);

                                            //targetPos *= LocalRot;

                                            bool rotationOverride = false;
                                            if (!rotationOverride)
                                            {
                                                targetPos *= rot_ins_4;
                                            }

                                            targetPos += pos_ins_4;
                                            modelCont.Vertices[v].Pos = new Vector3(targetPos.X, targetPos.Y, targetPos.Z);
                                        }
                                        vtx[5 + cur_instance] = new VertexBufferData();
                                        vtx[5 + cur_instance].Vtx = modelCont.Vertices;
                                        vtx[5 + cur_instance].VtxInd = modelCont.Indices;
                                        modelCont.Vertices = vbuffer;
                                        UpdateVBO(5 + cur_instance);

                                        cur_instance++;
                                    }

                                    if (HasBlendSkin && file.Data.Type == TwinsFile.FileType.RM2)
                                    {
                                        BlendSkinController modelCont = null;
                                        SectionController mesh_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(5);
                                        foreach (Twinsanity.BlendSkin mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(5).Records)
                                        {
                                            if (mod.ID == TargetBSkin)
                                            {
                                                modelCont = mesh_sec.GetItem<BlendSkinController>(mod.ID);
                                            }
                                        }

                                        modelCont.LoadMeshData();

                                        for (int v = 0; v < modelCont.Vertices.Count; v++)
                                        {
                                            Vertex[] vbuffer = new Vertex[modelCont.Vertices[v].Length];
                                            for (int p = 0; p < modelCont.Vertices[v].Length; p++)
                                            {
                                                vbuffer[p] = modelCont.Vertices[v][p];
                                                Vector4 targetPos = new Vector4(modelCont.Vertices[v][p].Pos.X, modelCont.Vertices[v][p].Pos.Y, modelCont.Vertices[v][p].Pos.Z, 1);

                                                //targetPos *= LocalRot;

                                                bool rotationOverride = false;

                                                if (!rotationOverride)
                                                {
                                                    targetPos *= rot_ins_4;
                                                }

                                                targetPos += pos_ins_4;
                                                modelCont.Vertices[v][p].Pos = new Vector3(targetPos.X, targetPos.Y, targetPos.Z);
                                                if (ins.ObjectID == (ushort)DefaultEnums.ObjectID.TNTCRATE)
                                                {
                                                    modelCont.Vertices[v][p].Col = Vertex.ColorToABGR(Color.Red);
                                                }
                                                else if (ins.ObjectID == (ushort)DefaultEnums.ObjectID.NITROCRATE)
                                                {
                                                    modelCont.Vertices[v][p].Col = Vertex.ColorToABGR(Color.Green);
                                                }
                                                else if (WoodCrates.Contains((DefaultEnums.ObjectID)ins.ObjectID))
                                                {
                                                    modelCont.Vertices[v][p].Col = Vertex.ColorToABGR(Color.SandyBrown);
                                                }
                                            }
                                            
                                            vtx[5 + cur_instance] = new VertexBufferData();
                                            vtx[5 + cur_instance].Vtx = modelCont.Vertices[v];
                                            vtx[5 + cur_instance].VtxInd = modelCont.Indices[v];
                                            Utils.TextUtils.LoadTexture(modelCont.Data.Models.Select((subModel) =>
                                            {
                                                return subModel.MaterialID;
                                            }).ToArray(), file, vtx[5 + cur_instance], v);
                                            modelCont.Vertices[v] = vbuffer;
                                            UpdateVBO(5 + cur_instance);
                                            cur_instance++;
                                        }
                                    }

                                    if (HasBlendSkin && file.Data.Type == TwinsFile.FileType.RMX)
                                    {
                                        BlendSkinXController modelCont = null;
                                        SectionController mesh_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(5);
                                        foreach (BlendSkinX mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(5).Records)
                                        {
                                            if (mod.ID == TargetBSkin)
                                            {
                                                modelCont = mesh_sec.GetItem<BlendSkinXController>(mod.ID);
                                            }
                                        }

                                        modelCont.LoadMeshData();
                                        for (int v = 0; v < modelCont.Vertices.Count; v++)
                                        {
                                            Vertex[] vbuffer = new Vertex[modelCont.Vertices[v].Length];
                                            for (int p = 0; p < modelCont.Vertices[v].Length; p++)
                                            {
                                                vbuffer[p] = modelCont.Vertices[v][p];
                                                Vector4 targetPos = new Vector4(modelCont.Vertices[v][p].Pos.X, modelCont.Vertices[v][p].Pos.Y, modelCont.Vertices[v][p].Pos.Z, 1);

                                                //targetPos *= LocalRot;

                                                bool rotationOverride = false;

                                                if (!rotationOverride)
                                                {
                                                    targetPos *= rot_ins_4;
                                                }

                                                targetPos += pos_ins_4;
                                                modelCont.Vertices[v][p].Pos = new Vector3(targetPos.X, targetPos.Y, targetPos.Z);
                                                if (ins.ObjectID == (ushort)DefaultEnums.ObjectID.TNTCRATE)
                                                {
                                                    modelCont.Vertices[v][p].Col = Vertex.ColorToABGR(Color.Red);
                                                }
                                                else if (ins.ObjectID == (ushort)DefaultEnums.ObjectID.NITROCRATE)
                                                {
                                                    modelCont.Vertices[v][p].Col = Vertex.ColorToABGR(Color.Green);
                                                }
                                                else if (WoodCrates.Contains((DefaultEnums.ObjectID)ins.ObjectID))
                                                {
                                                    modelCont.Vertices[v][p].Col = Vertex.ColorToABGR(Color.SandyBrown);
                                                }
                                            }

                                            vtx[5 + cur_instance] = new VertexBufferData();
                                            vtx[5 + cur_instance].Vtx = modelCont.Vertices[v];
                                            vtx[5 + cur_instance].VtxInd = modelCont.Indices[v];
                                            Utils.TextUtils.LoadTexture(modelCont.Data.SubModels.Select((subModel) =>
                                            {
                                                return subModel.MaterialID;
                                            }).ToArray(), file, vtx[5 + cur_instance], v);
                                            modelCont.Vertices[v] = vbuffer;
                                            UpdateVBO(5 + cur_instance);
                                            cur_instance++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
            UpdateVBO(1);
        }

        public void LoadColNodes()
        {
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;
            uint col_section = 9;
            if (file.Data.Type == TwinsFile.FileType.MonkeyBallRM) col_section = 10;
            ColData data = file.Data.GetItem<ColData>(col_section);
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
            bool[] record_exists = new bool[9];
            int posi_count = 0;
            uint mb_add = 0;
            if (file.Data.Type == TwinsFile.FileType.MonkeyBallRM) mb_add = 1;
            for (uint i = mb_add; i <= mb_add + 7; ++i)
            {
                record_exists[i] = file.Data.ContainsItem(i);
                if (record_exists[i])
                {
                    if (file.Data.GetItem<TwinsSection>(i).ContainsItem(3))
                    {
                        posi_count += file.Data.GetItem<TwinsSection>(i).GetItem<TwinsSection>(3).Records.Count;
                        record_exists[i] = true;
                    }
                    else
                        record_exists[i] = false;
                }
            }
            if (vtx[3] == null || vtx.Count != (circle_res * 3 + 6) * posi_count)
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
            for (uint i = mb_add; i <= mb_add + 7; ++i)
            {
                if (!record_exists[i]) continue;
                if (file.Data.GetItem<TwinsSection>(i).ContainsItem(3))
                {
                    foreach (Position pos in file.Data.GetItem<TwinsSection>(i).GetItem<TwinsSection>(3).Records)
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
            bool[] record_exists = new bool[9];
            int posi_count = 0;
            uint mb_add = 0;
            if (file.Data.Type == TwinsFile.FileType.MonkeyBallRM) mb_add = 1;
            for (uint i = mb_add; i <= mb_add + 7; ++i)
            {
                record_exists[i] = file.Data.ContainsItem(i);
                if (record_exists[i])
                {
                    if (file.Data.GetItem<TwinsSection>(i).ContainsItem(1))
                    {
                        posi_count += file.Data.GetItem<TwinsSection>(i).GetItem<TwinsSection>(1).Records.Count;
                        record_exists[i] = true;
                    }
                    else
                        record_exists[i] = false;
                }
            }
            if (vtx[4] == null || vtx.Count != (circle_res * 3 + 6) * posi_count)
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
            for (uint i = mb_add; i <= mb_add + 7; ++i)
            {
                if (!record_exists[i]) continue;
                if (file.Data.GetItem<TwinsSection>(i).ContainsItem(1))
                {
                    foreach (AIPosition pos in file.Data.GetItem<TwinsSection>(i).GetItem<TwinsSection>(1).Records)
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
            else if (file.SelectedItem is InstanceMB insm)
            {
                SetPosition(new Vector3(-insm.Pos.X, insm.Pos.Y, insm.Pos.Z));
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
