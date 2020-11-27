using System.Drawing;
using System.Windows.Forms;
using System;
using Twinsanity;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using Twinsanity.Items;

namespace TwinsaityEditor
{
    public partial class PTCViewer
    {

        public PTCViewer()
        {
            InitializeComponent();
        }

        private int texInd;

        public TwinsPTC SelectedPTC;

        public int TextureIndex
        {
            set
            {
                lblTextureIndex.Text = (value + 1).ToString() + "/" + PTCs.Count;
                texInd = value;
            }
            get
            {
                return texInd;
            }
        }
        public List<TwinsPTC> PTCs = new List<TwinsPTC>();

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
            for (int i = 0; i < SelectedPTC.Texture.RawData.Length; i++)
            {
                GL.Color4(SelectedPTC.Texture.RawData[i]);
                GL.Vertex2(i % (SelectedPTC.Texture.Width), i / (SelectedPTC.Texture.Width));
            }
            GL.End();
            GlControl1.SwapBuffers();
            Application.DoEvents();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SavePNG.FileName = SelectedPTC.TexID.ToString();
            if (SavePNG.ShowDialog() == DialogResult.OK)
            {
                Bitmap BMP = new Bitmap(Convert.ToInt32(SelectedPTC.Texture.Width), Convert.ToInt32(SelectedPTC.Texture.Height));
                for (int i = 0; i < SelectedPTC.Texture.RawData.Length; i++)
                    BMP.SetPixel((i % SelectedPTC.Texture.Width), (i / SelectedPTC.Texture.Width), SelectedPTC.Texture.RawData[i]);
                BMP.Save(SavePNG.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private void btnPrevTexture_Click(object sender, EventArgs e)
        {
            TextureIndex--;
            if (TextureIndex < 0)
            {
                TextureIndex = PTCs.Count - 1;
            }
            SelectedPTC = PTCs[TextureIndex];
            Refresh();
        }

        public void UpdateTextureLabel()
        {
            lblTextureIndex.Text = (TextureIndex + 1).ToString() + "/" + PTCs.Count;
        }

        private void btnNextTexture_Click(object sender, EventArgs e)
        {
            TextureIndex++;
            if (TextureIndex >= PTCs.Count)
            {
                TextureIndex = 0;
            }
            SelectedPTC = PTCs[TextureIndex];
            Refresh();
        }
    }
}
