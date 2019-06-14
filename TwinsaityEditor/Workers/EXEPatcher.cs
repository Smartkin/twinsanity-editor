using System;
using System.Windows.Forms;
using System.IO;

namespace TwinsaityEditor
{
    public partial class EXEPatcher : Form
    {
        private enum GameVersion { NTSC_U, PAL, NTSC_J, NTSC_U_2, PAL_2 };

        private GameVersion ver;
        private string fileName;

        private const int PAL_levelOff = 0x1F6708;
        private const int PAL_levelSize = 0x37;
        private const int PAL_archiveOff = 0x1ED410;
        private const int PAL_archiveSize = 0x7;
        private const int NTSCU_levelOff = 0x1F5E28;
        private const int NTSCU_levelSize = 0x37;
        private const int NTSCU_archiveOff = 0x1ECB10;
        private const int NTSCU_archiveSize = 0x7;

        public EXEPatcher()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "NTSC-U executable|SLUS_209.09|PAL executable|SLES_525.68";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileName = ofd.FileName;
                switch (ofd.FilterIndex)
                {
                    case 1:
                        LoadEXE(NTSCU_levelOff, NTSCU_levelSize, NTSCU_archiveOff, NTSCU_archiveSize);
                        ver = GameVersion.NTSC_U;
                        break;
                    case 2:
                        LoadEXE(PAL_levelOff, PAL_levelSize, PAL_archiveOff, PAL_archiveSize);
                        ver = GameVersion.PAL;
                        break;
                }
                InitializeComponent();
                Show();
            }
            else
                Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = checkBox2.Checked;
        }

        private void LoadEXE(int l_off, int l_size, int a_off, int a_size)
        {
            BinaryReader reader = new BinaryReader(new FileStream(fileName, FileMode.Open, FileAccess.Read));
            textBox1.MaxLength = a_size;
            textBox2.MaxLength = l_size;
            reader.BaseStream.Position = a_off;
            char ch = '\0';
            do
            {
                ch = reader.ReadChar();
                textBox1.Text += ch;
            }
            while (ch != '\0');
            reader.BaseStream.Position = l_off;
            do
            {
                ch = reader.ReadChar();
                textBox2.Text += ch;
            }
            while (ch != '\0');
            reader.Close();
        }

        private void PatchEXE(int l_off, int l_size, int a_off, int a_size)
        {
            label1.Visible = true;
            label1.Text = "Patching...";
            BinaryWriter writer = new BinaryWriter(new FileStream(fileName, FileMode.Open, FileAccess.Write));
            if (checkBox1.Checked)
            {
                writer.BaseStream.Position = a_off;
                while (writer.BaseStream.Position < a_off + a_size)
                    writer.Write((byte)0);
                writer.BaseStream.Position = a_off;
                for (int i = 0; i < a_size && writer.BaseStream.Position < a_off + a_size && i < textBox1.Text.Length; ++i)
                {
                    writer.Write(textBox1.Text[i]);
                }
            }
            if (checkBox2.Checked)
            {
                writer.BaseStream.Position = l_off;
                while (writer.BaseStream.Position < l_off + l_size)
                    writer.Write((byte)0);
                writer.BaseStream.Position = l_off;
                for (int i = 0; i < l_size && writer.BaseStream.Position < l_off + l_size && i < textBox2.Text.Length; ++i)
                {
                    writer.Write(textBox2.Text[i]);
                }
            }
            writer.Close();
            label1.Text = "Patched!";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ver == GameVersion.NTSC_U)
                PatchEXE(NTSCU_levelOff, NTSCU_levelSize, NTSCU_archiveOff, NTSCU_archiveSize);
            else if (ver == GameVersion.PAL)
                PatchEXE(PAL_levelOff, PAL_levelSize, PAL_archiveOff, PAL_archiveSize);
        }
    }
}
