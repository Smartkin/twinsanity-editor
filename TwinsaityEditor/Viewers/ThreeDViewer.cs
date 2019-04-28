﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TwinsaityEditor
{
    public abstract class ThreeDViewer : GLControl
    {
        //add to preferences later
        protected static Color[] colors = new[] { Color.Gray, Color.SlateGray, Color.DodgerBlue, Color.OrangeRed, Color.Red, Color.Pink, Color.LimeGreen, Color.DarkSlateBlue, Color.SaddleBrown, Color.LightSteelBlue, Color.SandyBrown, Color.Peru, Color.RoyalBlue, Color.DimGray, Color.Coral, Color.AliceBlue, Color.LightGray, Color.Cyan, Color.MediumTurquoise, Color.DarkSlateGray, Color.DarkSalmon, Color.DarkRed, Color.DarkCyan, Color.MediumVioletRed, Color.MediumOrchid, Color.DarkGray, Color.Yellow, Color.Goldenrod };

        protected Vertex[][] vtx;

        private Vector3 pos, rot, sca;
        private float range;
        private Timer refresh;
        private bool k_w, k_a, k_s, k_d, k_e, k_q, m_l, m_r;
        private int m_x, m_y;
        private EventHandler _inputHandle;
        private FontWrapper.FontService _fntService;
        private Dictionary<char, int> textureCharMap = new Dictionary<char, int>();
        private float size = 48;
        protected int[] vbo_id;
        protected int vbo_count;
        private int[] vbo_sizes;


        public ThreeDViewer()
        {
            _fntService = new FontWrapper.FontService();
            List<FileInfo> fonts = (List<FileInfo>)_fntService.GetFontFiles(new DirectoryInfo("Fonts/"), false);
            _fntService.SetFont(fonts[0].FullName);
            _fntService.SetSize(size);

            pos = new Vector3(0, 0, 0);
            rot = new Vector3(0, 0, 0);
            sca = new Vector3(1.0f, 1.0f, 1.0f);
            range = 100;

            _inputHandle = (sender, e) =>
            {
                if (e is MouseEventArgs)
                {
                    Invalidate();
                }
                else
                {
                    float speed = range / 250;
                    int v = 0, h = 0, d = 0;
                    if (k_w)
                        d++;
                    if (k_a)
                        h++;
                    if (k_s)
                        d--;
                    if (k_d)
                        h--;
                    if (k_e)
                        v++;
                    if (k_q)
                        v--;
                    Vector3 delta = new Vector3(h, v, d) * speed;
                    Matrix4 rot_matrix = Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), rot.X);
                    rot_matrix *= Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), rot.Y);
                    rot_matrix *= Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), rot.Z);

                    Vector3 fin_delta = new Vector3(rot_matrix * new Vector4(delta, 1.0f));

                    pos -= fin_delta;

                    if ((h | v | d) != 0)
                        Invalidate();
                }
            };

            refresh = new Timer
            {
                Interval = (int)Math.Round(1.0/60*1000), //Set to 60fps by default, TODO: Add to Preferences later
                Enabled = true
            };

            refresh.Tick += delegate (object sender, EventArgs e)
            {
                _inputHandle(sender, e);
                Invalidate();
            };

            ParentChanged += ThreeDViewer_ParentChanged;
        }

        private void ThreeDViewer_ParentChanged(object sender, EventArgs e)
        {
            Form par = (Form)Parent;
            par.Icon = Properties.Resources.icon;
            ParentChanged -= ThreeDViewer_ParentChanged;
        }

        protected abstract void RenderObjects();

        private void ResetCamera()
        {
            pos = new Vector3(0, 0, 0);
            rot = new Vector3((float)Math.PI, 0, 0);
            Matrix4 view = Matrix4.Identity;
            Matrix4 proj = Matrix4.Identity;
            Matrix4 rot_matrix = Utils.MatrixWrapper.RotateMatrix4(rot.X, rot.Y, rot.Z);

            //Setup view and projection matrix
            Vector4 rot_vector = Vector4.Transform(new Vector4(0, 0, 1, 1), rot_matrix);
            view = Matrix4.LookAt(pos, new Vector3(pos.X + rot_vector.X, pos.Y + rot_vector.Y, pos.Z + rot_vector.Z), new Vector3(0, 1, 0));
            proj = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, (float)this.Width / this.Height, 1.0f, 10000.0f);

            //Apply the matrices
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref proj);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref view);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            switch (e.Button)
            {
                case MouseButtons.Left:
                    m_l = true;
                    break;
                case MouseButtons.Right:
                    m_r = true;
                    break;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            switch (e.Button)
            {
                case MouseButtons.Left:
                    m_l = false;
                    break;
                case MouseButtons.Right:
                    m_r = false;
                    break;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (m_l)
            {
                rot.X += (e.X - m_x) / 180.0f * (float)Math.PI / (Size.Width / 480f);
                rot.Y += (e.Y - m_y) / 180.0f * (float)Math.PI / (Size.Height / 480f);
                rot.X += rot.X > (float)Math.PI ? -2*(float)Math.PI : rot.X < -(float)Math.PI ? +2*(float)Math.PI : 0;
                if (rot.Y > (float)Math.PI / 2)
                    rot.Y = (float)Math.PI / 2;
                if (rot.Y < (float)-Math.PI / 2)
                    rot.Y = (float)-Math.PI / 2;
            }
            m_x = e.X;
            m_y = e.Y;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            range -= e.Delta / 120 * 30;
            if (range > 500f)
                range = 500f;
            else if (range < 50)
                range = 50;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.W:
                case Keys.A:
                case Keys.S:
                case Keys.D:
                case Keys.Q:
                case Keys.E:
                case Keys.R:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.W:
                    k_w = true;
                    break;
                case Keys.A:
                    k_a = true;
                    break;
                case Keys.S:
                    k_s = true;
                    break;
                case Keys.D:
                    k_d = true;
                    break;
                case Keys.Q:
                    k_q = true;
                    break;
                case Keys.E:
                    k_e = true;
                    break;
                case Keys.R:
                    ResetCamera();
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            switch (e.KeyCode)
            {
                case Keys.W:
                    k_w = false;
                    break;
                case Keys.A:
                    k_a = false;
                    break;
                case Keys.S:
                    k_s = false;
                    break;
                case Keys.D:
                    k_d = false;
                    break;
                case Keys.Q:
                    k_q = false;
                    break;
                case Keys.E:
                    k_e = false;
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            MakeCurrent();
            GL.Viewport(Location, Size);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Frustum(-0.75, +0.75, -0.75, +0.75, 0.8, 1000.0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Scale(sca);
            GL.Rotate(rot.Y * 180 / Math.PI, 1, 0, 0);
            GL.Rotate(rot.X * 180 / Math.PI, 0, 1, 0);
            GL.Rotate(rot.Z * 180 / Math.PI, 0, 0, 1);
            Vector3 delta = new Vector3(0, 0, -1) * range / 25f;
            Matrix4 rot_matrix = Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), rot.X);
            rot_matrix *= Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), rot.Y);
            rot_matrix *= Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), rot.Z);

            Vector3 fin_delta = new Vector3(rot_matrix * new Vector4(delta, 1.0f));
            GL.Translate(-pos + fin_delta);
            RenderObjects();
            DrawText();
            SwapBuffers();
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Texture2D);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.AlphaFunc(AlphaFunction.Greater, 0);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Fastest);
            GL.ClearColor(Color.MidnightBlue); //TODO: Add clear color to Preferences later
            GL.Enable(EnableCap.ColorMaterial);
            //GL.ShadeModel(ShadingModel.Flat); //TODO: Add to preferences
            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 0, 0, 0, 1 });
            GL.LightModel(LightModelParameter.LightModelTwoSide, 1);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.Normalize);
            InitVBO();
            base.OnLoad(e);
        }

        private void InitVBO()
        {
            vbo_id = new int[vbo_count];
            vbo_sizes = new int[vbo_count];
            //Generate a buffer
            GL.GenBuffers(vbo_count, vbo_id);
            for (int i = 0; i < vbo_count; ++i)
            {
                //Ignore this buffer if the vertex buffer is null
                if (vtx[i] == null) continue;
                //Store size in order to be compared with later
                vbo_sizes[i] = vtx[i].Length;
                //Bind newly-generated buffer to the array buffer
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_id[i]);
                //Allocate data for vertex buffer...
                GL.BufferData(BufferTarget.ArrayBuffer, Marshal.SizeOf(typeof(Vertex)) * vtx[i].Length, vtx[i], BufferUsageHint.StaticDraw);
                //unbind buffer (safety)
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }
        }

        protected void UpdateVBO(int id)
        {
            //Bind newly-generated buffer to the array buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_id[id]);
            //Allocate data for vertex buffer...
            if (vtx[id].Length > vbo_sizes[id])
                GL.BufferData(BufferTarget.ArrayBuffer, Marshal.SizeOf(typeof(Vertex)) * vtx[id].Length, vtx[id], BufferUsageHint.StaticDraw);
            else
                GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)0, Marshal.SizeOf(typeof(Vertex)) * vtx[id].Length, vtx[id]);
            //unbind buffer (safety)
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        protected virtual void DrawText()
        {
            //GL.Color3(Color.White);
            //RenderString("0123456789ABCDEFGHIJKLMNP1RSTUVWXYZabcdefghijklmnopqrstuvwxyz !_/:?");
        }

        protected int LoadTextTexture(ref Bitmap text, int quality = 1, bool flip_y = false)
        {
            if (flip_y)
                text.RotateFlip(RotateFlipType.RotateNoneFlipY);

            GL.GenTextures(1, out int texture);

            GL.BindTexture(TextureTarget.Texture2D, texture);

            switch (quality)
            {
                case 0:
                default:
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
                    break;
                case 1:
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Nearest);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Nearest);
                    break;
            }

            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.ClampToEdge);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.ClampToEdge);

            BitmapData data = text.LockBits(new Rectangle(0, 0, text.Width, text.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            text.UnlockBits(data);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            return texture;
        }

        protected void RenderString(string s)
        {
            float spacing = 2 / 3f;
            float x = (s.Length + 1) * (-spacing / 2f);
            foreach (char c in s)
            {
                x += spacing;
                if (c == ' ')
                    continue;
                if (!textureCharMap.ContainsKey(c))
                    GenCharTex(c);
                GL.BindTexture(TextureTarget.Texture2D, textureCharMap[c]);
                GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out float w);
                GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out float h);
                w /= size*2;
                h /= size;
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(0, 1); GL.Vertex2(x-w, 0);
                GL.TexCoord2(1, 1); GL.Vertex2(x+w, 0);
                GL.TexCoord2(1, 0); GL.Vertex2(x+w, h);
                GL.TexCoord2(0, 0); GL.Vertex2(x-w, h);
                GL.End();
            }
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private void GenCharTex(char c)
        {
            Bitmap bmp = _fntService.RenderString(c.ToString(), Color.White, Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
            textureCharMap.Add(c, LoadTextTexture(ref bmp));
            bmp.Dispose();
        }

        protected void SetPosition(Vector3 pos)
        {
            this.pos = pos;
        }

        protected override void Dispose(bool disposing)
        {
            refresh.Dispose();
            base.Dispose(disposing);
        }
    }
}
