using System.Drawing;
using System;

namespace TwinsaityEditor
{
    public partial class TextureImport
    {
        private void Button1_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Bitmap BMP = new Bitmap(OpenFileDialog1.FileName);
                Color[] RawData = new Color[BMP.Width * BMP.Height - 1 + 1];
                for (int i = 0; i <= RawData.Length - 1; i++)
                    RawData[i] = BMP.GetPixel(i % BMP.Width, i / BMP.Width);
                Twinsanity.Texture Texture = new Twinsanity.Texture();
                uint width = uint.Parse(ComboBox1.Text.Split('x')[0]);
                uint height = uint.Parse(ComboBox1.Text.Split('x')[1].Split(' ')[0]);
                bool mip = ComboBox1.Text.Split('x')[1].Split(' ')[1] == "mip" ? true : false;
                Twinsanity.Texture.BlockFormats blockFormat = (Twinsanity.Texture.BlockFormats)ComboBox1.SelectedIndex;
                Texture.Import(RawData, width, height, blockFormat, mip);
                System.IO.FileStream File = new System.IO.FileStream(OpenFileDialog1.FileName.Remove(OpenFileDialog1.FileName.Length - 4, 4), System.IO.FileMode.Create, System.IO.FileAccess.Write);
                System.IO.BinaryWriter FileWriter = new System.IO.BinaryWriter(File);
                FileWriter.Write(Texture.ByteStream.ToArray());
                FileWriter.Close();
                File.Close();
            }
        }
    }
}
