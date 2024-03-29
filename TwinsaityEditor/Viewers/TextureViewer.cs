using System.Drawing;
using System.Windows.Forms;
using System;
using Twinsanity;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace TwinsaityEditor
{
    public partial class TextureViewer
    {

        public TextureViewer()
        {
            InitializeComponent();
        }

        private int texInd;

        public Texture SelectedTexture;
        public TextureX SelectedTextureX;
        public TextureP SelectedTextureP;
        public int TextureIndex
        {
            set
            {
                if (SelectedTexture == null)
                {
                    if (SelectedTextureX == null)
                        lblTextureIndex.Text = (value + 1).ToString() + "/" + TexturesP.Count;
                    else
                        lblTextureIndex.Text = (value + 1).ToString() + "/" + TexturesX.Count;
                }
                else
                    lblTextureIndex.Text = (value + 1).ToString() + "/" + Textures.Count;
                texInd = value;
            }
            get
            {
                return texInd;
            }
        }
        public List<Texture> Textures = new List<Texture>();
        public List<TextureX> TexturesX = new List<TextureX>();
        public List<TextureP> TexturesP = new List<TextureP>();

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
            if (SelectedTexture == null)
            {
                if (SelectedTextureX == null)
                {
                    for (int i = 0; i < SelectedTextureP.RawData.Length; i++)
                    {
                        GL.Color4(SelectedTextureP.RawData[i]);
                        GL.Vertex2(i % (SelectedTextureP.Width), i / (SelectedTextureP.Width));
                    }
                    GL.End();
                }
                else
                {
                    for (int i = 0; i < SelectedTextureX.RawData.Length; i++)
                    {
                        GL.Color4(SelectedTextureX.RawData[i]);
                        GL.Vertex2(i % (SelectedTextureX.Width), i / (SelectedTextureX.Width));
                    }
                    GL.End();
                }
            }
            else
            {
                for (int i = 0; i < SelectedTexture.RawData.Length; i++)
                {
                    GL.Color4(SelectedTexture.RawData[i]);
                    GL.Vertex2(i % (SelectedTexture.Width), i / (SelectedTexture.Width));
                }
                GL.End();
                if (SelectedTexture.MipLevels > 0)
                {
                    var widthOffset = SelectedTexture.Width;
                    for (var i = 0; i < SelectedTexture.MipLevels; ++i)
                    {
                        var mip = SelectedTexture.GetMips(i);
                        var mipWidth = (SelectedTexture.Width / (1 << (i + 1)));
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
            }
            GlControl1.SwapBuffers();
            Application.DoEvents();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SavePNG.FileName = SelectedTexture.ID.ToString();
            if (SavePNG.ShowDialog() == DialogResult.OK)
            {
                if (SelectedTexture == null)
                {
                    if (SelectedTextureX == null)
                    {
                        Bitmap BMP = new Bitmap(Convert.ToInt32(SelectedTextureP.Width), Convert.ToInt32(SelectedTextureP.Height));
                        for (int i = 0; i < SelectedTextureP.RawData.Length; i++)
                            BMP.SetPixel((i % SelectedTextureP.Width), (i / SelectedTextureP.Width), SelectedTextureP.RawData[i]);
                        BMP.Save(SavePNG.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    else
                    {
                        Bitmap BMP = new Bitmap(Convert.ToInt32(SelectedTextureX.Width), Convert.ToInt32(SelectedTextureX.Height));
                        for (int i = 0; i < SelectedTextureX.RawData.Length; i++)
                            BMP.SetPixel((i % SelectedTextureX.Width), (i / SelectedTextureX.Width), SelectedTextureX.RawData[i]);
                        BMP.Save(SavePNG.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
                else
                {
                    Bitmap BMP = new Bitmap(Convert.ToInt32(SelectedTexture.Width), Convert.ToInt32(SelectedTexture.Height));
                    for (int i = 0; i < SelectedTexture.RawData.Length; i++)
                        BMP.SetPixel((i % SelectedTexture.Width), (i / SelectedTexture.Width), SelectedTexture.RawData[i]);
                    BMP.Save(SavePNG.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    if (cbSaveMips.Checked)
                    {
                        if (SelectedTexture.MipLevels > 0)
                        {
                            for (var i = 0; i < SelectedTexture.MipLevels; ++i)
                            {
                                var mip = SelectedTexture.GetMips(i);
                                var mipWidth = (SelectedTexture.Width / (1 << (i + 1)));
                                var mipHeight = (SelectedTexture.Height / (1 << (i + 1)));
                                Bitmap mipBmp = new Bitmap(Convert.ToInt32(mipWidth), Convert.ToInt32(mipHeight));
                                for (int j = 0; j < mip.Length; j++)
                                    mipBmp.SetPixel((j % mipWidth), (j / mipWidth), mip[j]);
                                mipBmp.Save(SavePNG.FileName.Substring(0, SavePNG.FileName.Length - 4) + "_mip_" + (i + 1).ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                            }
                        }
                    }
                }
            }
        }

        private void btnPrevTexture_Click(object sender, EventArgs e)
        {
            TextureIndex--;
            if (SelectedTexture == null)
            {
                if (SelectedTextureX == null)
                {
                    if (TextureIndex < 0)
                    {
                        TextureIndex = TexturesP.Count - 1;
                    }
                    SelectedTextureP = TexturesP[TextureIndex];
                }
                else
                {
                    if (TextureIndex < 0)
                    {
                        TextureIndex = TexturesX.Count - 1;
                    }
                    SelectedTextureX = TexturesX[TextureIndex];
                }
            }
            else
            {
                if (TextureIndex < 0)
                {
                    TextureIndex = Textures.Count - 1;
                }
                SelectedTexture = Textures[TextureIndex];
            }
            Refresh();
        }

        public void UpdateTextureLabel()
        {
            if (SelectedTexture == null)
            {
                if (SelectedTextureX == null)
                {
                    lblTextureIndex.Text = (TextureIndex + 1).ToString() + "/" + TexturesP.Count;
                }
                else
                {
                    lblTextureIndex.Text = (TextureIndex + 1).ToString() + "/" + TexturesX.Count;
                }
            }
            else
            {
                lblTextureIndex.Text = (TextureIndex + 1).ToString() + "/" + Textures.Count;
            }
        }

        private void btnNextTexture_Click(object sender, EventArgs e)
        {
            TextureIndex++;
            if (SelectedTexture == null)
            {
                if (SelectedTextureX == null)
                {
                    if (TextureIndex >= TexturesP.Count)
                    {
                        TextureIndex = 0;
                    }
                    SelectedTextureP = TexturesP[TextureIndex];
                }
                else
                {
                    if (TextureIndex >= TexturesX.Count)
                    {
                        TextureIndex = 0;
                    }
                    SelectedTextureX = TexturesX[TextureIndex];
                }
            }
            else
            {
                if (TextureIndex >= Textures.Count)
                {
                    TextureIndex = 0;
                }
                SelectedTexture = Textures[TextureIndex];
            }
            Refresh();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (SelectedTexture == null) return;
            if (LoadPNG.ShowDialog() == DialogResult.OK)
            {
                Bitmap temp = new Bitmap(LoadPNG.FileName);
                Bitmap map = new Bitmap(temp);
                temp.Dispose();

                int ogHeight = SelectedTexture.Height;
                int ogWidth = SelectedTexture.Width;
                int col = 0;
                int row = 0;
                int c = 0;
                for (int y = row * ogHeight; y < ogHeight + (row * ogHeight); y++)
                {
                    for (int x = col * ogWidth; x < ogWidth + (col * ogWidth); x++)
                    {
                        if (c < SelectedTexture.RawData.Length)
                        {
                            SelectedTexture.RawData[c] = map.GetPixel(x, y);
                        }
                        c++;
                    }
                }
                col++;
                if (col == 4)
                {
                    col = 0;
                    row++;
                }
                SelectedTexture.UpdateImageData();
                Refresh();
            }
        }
    }
}
