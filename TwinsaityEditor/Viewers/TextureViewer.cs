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

        public Texture Texture;

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
        }

        private void GlControl1_Paint(object sender, PaintEventArgs e)
        {
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Begin(PrimitiveType.Points);
            for (int i = 0; i < Texture.RawData.Length; i++)
            {
                GL.Color4(Texture.RawData[i]);
                GL.Vertex2(i % (Texture.Width), i / (Texture.Width));
            }
            GL.End();
            if (Texture.MipLevels > 0)
            {
                var widthOffset = Texture.Width;
                for (var i = 0; i < Texture.MipLevels; ++i)
                {
                    var mip = Texture.GetMips(i);
                    var mipWidth = (Texture.Width / (1 << (i + 1)));
                    GL.Begin(PrimitiveType.Points);
                    for (int j = 0; j < mip.Length; ++j)
                    {
                        GL.Color4(mip[j]);
                        GL.Vertex2(widthOffset + j % mipWidth, j / mipWidth);
                    }
                    GL.End();
                    widthOffset += mipWidth;
                }
            }
            GlControl1.SwapBuffers();
            Application.DoEvents();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            SavePNG.FileName = Texture.ID.ToString();
            if (SavePNG.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Bitmap BMP = new Bitmap(System.Convert.ToInt32(Texture.Width), System.Convert.ToInt32(Texture.Height));
                for (int i = 0; i < Texture.RawData.Length; i++)
                    BMP.SetPixel((int)(i % Texture.Width), (int)(i / Texture.Width), Texture.RawData[i]);
                BMP.Save(SavePNG.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }
    }
}
