using System.Drawing;
using System.Windows.Forms;
using System;
using Twinsanity;
using OpenTK.Graphics.OpenGL;

namespace TwinsaityEditor
{
    public partial class TextureViewer
    {

        public TextureViewer()
        {
            InitializeComponent();
        }

        public Twinsanity.Texture Texture;
        public Twinsanity.Textures Textures;
        public Twinsanity.Materials Materials;
        public bool Mat = false;
        public uint CurTex = 0;
        public void Init()
        {
            int[] viewPort = new int[5];
            GL.GetInteger(GetPName.Viewport, viewPort);
            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Ortho(viewPort[0], viewPort[0] + viewPort[2], viewPort[1] + viewPort[3], viewPort[1], -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Translate(0.0, 0.0, 0.0);
            GL.PushAttrib(AttribMask.DepthBufferBit);
            GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }
        public bool Draw = true;
        private void TextureViewer_Load(object sender, EventArgs e)
        {
            Init();
            UpdateTexture();
        }

        private void GlControl1_Paint(object sender, PaintEventArgs e)
        {
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Begin(PrimitiveType.Points);
            for (int i = 0; i <= Texture.RawData.Length - 1; i++)
            {
                if (CheckBox1.Checked == true)
                    GL.Color4(Color.FromArgb(255, Texture.Index[i], Texture.Index[i], Texture.Index[i]));
                else
                    GL.Color4(Texture.RawData[i]);
                GL.Vertex2(i % (Texture.Width), i / (Texture.Width));
            }
            GL.End();
            GlControl1.SwapBuffers();
            Application.DoEvents();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            SavePNG.FileName = Texture.ID.ToString();
            if (SavePNG.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Bitmap BMP = new Bitmap(System.Convert.ToInt32(Texture.Width), System.Convert.ToInt32(Texture.Height));
                for (int i = 0; i <= Texture.RawData.Length - 1; i++)
                    BMP.SetPixel((int)(i % Texture.Width), (int)(i / Texture.Width), Texture.RawData[i]);
                BMP.Save(SavePNG.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            GlControl1.Invalidate();
        }

        private void UpdateTexture()
        {
            if (Mat)
            {
                Twinsanity.Material Material = (Twinsanity.Material)Materials._Item[CurTex];
                uint TexId = Material.Texture;
                for (int i = 0; i <= Textures._Item.Length - 1; i++)
                {
                    if (Textures._Item[i].ID == TexId)
                    {
                        Texture = (Twinsanity.Texture)Textures._Item[i];
                        break;
                    }
                }
                Label1.Text = (CurTex + 1).ToString() + @"\" + Materials._Item.Length.ToString();
                this.Text = Material.Name + " Texture: " + Texture.ID.ToString();
            }
            else
            {
                Texture = (Twinsanity.Texture)Textures._Item[CurTex];
                Label1.Text = (CurTex + 1).ToString() + @"\" + Textures._Item.Length.ToString();
                this.Text = "ID: " + Texture.ID.ToString();
            }
            GlControl1.Invalidate();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (Mat)
            {
                if (CurTex < Materials._Item.Length - 1)
                    CurTex += 1;
                else
                    CurTex = 0;
            }
            else if (CurTex < Textures._Item.Length - 1)
                CurTex += 1;
            else
                CurTex = 0;
            UpdateTexture();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (Mat)
            {
                if (CurTex > 0)
                    CurTex -= 1;
                else
                    CurTex = (uint)Materials._Item.Length - 1;
            }
            else if (CurTex > 0)
                CurTex -= 1;
            else
                CurTex = (uint)Textures._Item.Length - 1;
            UpdateTexture();
        }
    }
}
