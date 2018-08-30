using System;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace TwinsaityEditor.Viewers
{
    public abstract class ThreeDViewer : GLControl
    {
        private Vector3 pos, rot, sca;
        private float range;
        private Timer input, refresh;
        private bool k_w, k_a, k_s, k_d, k_e, k_q, m_l, m_r;
        private int m_x, m_y;

        public ThreeDViewer()
        {
            pos = new Vector3(0, 0, 0);
            rot = new Vector3(0, 0, 0);
            sca = new Vector3(1.0f, 1.0f, 1.0f);
            input = new Timer
            {
                Interval = 15,
                Enabled = true
            };
            input.Tick += delegate (object sender, EventArgs e)
            {
                float speed = 1.0f; //range / 100;
                int v = 0, h = 0, d = 0;
                if (k_w)
                    d--;
                if (k_a)
                    h--;
                if (k_s)
                    d++;
                if (k_d)
                    h++;
                if (k_e)
                    v++;
                if (k_q)
                    v--;
                Matrix3 rot_x_matrix = Matrix3.CreateFromAxisAngle(new Vector3(speed, 0, 0), rot.X);
                Matrix3 rot_y_matrix = Matrix3.CreateFromAxisAngle(new Vector3(0, speed, 0), rot.Y);
                Matrix3 rot_z_matrix = Matrix3.CreateFromAxisAngle(new Vector3(0, 0, speed), rot.Z);
                Vector3 rot_x_vector = Vector3.Transform(new Vector3(speed, 0, 0), rot_x_matrix);
                Vector3 rot_y_vector = Vector3.Transform(new Vector3(0, speed, 0), rot_y_matrix);
                Vector3 rot_z_vector = Vector3.Transform(new Vector3(0, 0, speed), rot_z_matrix);
                pos.X += rot_y_vector.X * d + rot_x_vector.X * h;
                pos.Y += rot_z_vector.Y * d + v;
                pos.Z += rot_y_vector.Z * d + rot_x_vector.Z * h;
                if ((h | v | d) != 0)
                    Invalidate();
            };
            refresh = new Timer
            {
                Interval = 100,
                Enabled = true
            };
            refresh.Tick += delegate (object sender, EventArgs e)
            {
                Invalidate();
            };
        }

        protected abstract void RenderObjects();

        private void ResetCamera()
        {
            pos = new Vector3(0, 0, 0);
            rot = new Vector3(0, 0, 0);
            Invalidate();
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
                rot.X += (e.X - m_x) / 180.0f * (float)Math.PI;
                rot.Y += (e.Y - m_y) / 180.0f * (float)Math.PI;
                rot.X += rot.X > (float)Math.PI ? -2*(float)Math.PI : rot.X < -(float)Math.PI ? +2*(float)Math.PI : 0;
                if (rot.Y > (float)Math.PI / 2)
                    rot.Y = (float)Math.PI / 2;
                if (rot.Y < (float)-Math.PI / 2)
                    rot.Y = (float)-Math.PI / 2;
                Invalidate();
            }
            m_x = e.X;
            m_y = e.Y;
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
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.AlphaTest);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.AlphaFunc(AlphaFunction.Greater, 0);
            GL.Viewport(Location, Size);
            GL.ClearColor(0, 0, 1.0f, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Frustum(-0.01, +0.01, -0.01, +0.01, 0.01, ushort.MaxValue);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            float[] mat_diffuse = { 1.0f, 1.0f, 1.0f };
            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 1.0f, 1.0f, 0.0f, 1.0f });
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            GL.Translate(0, 0, -1);
            GL.Scale(sca);
            GL.Rotate(rot.Y * 180 / Math.PI, 1, 0, 0);
            GL.Rotate(rot.X * 180 / Math.PI, 0, 1, 0);
            GL.Translate(-pos.X, -pos.Y, -pos.Z);
            RenderObjects();
            SwapBuffers();
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.AlphaTest);
        }

        protected override void Dispose(bool disposing)
        {
            input.Dispose();
            refresh.Dispose();
            base.Dispose(disposing);
        }
    }
}
