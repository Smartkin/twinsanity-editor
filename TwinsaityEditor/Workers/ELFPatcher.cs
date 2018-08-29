using Microsoft.VisualBasic;
using System;

namespace TwinsaityEditor
{
    public partial class ELFPatcher
    {
        const int PALStartUpOffset = 2057992;
        const int PALStartUpSize = 55;
        const int PALBootOffset = 2020368;
        const int PALBootSize = 7;
        private uint StartUpOffset;
        private uint StartUpSize;
        private uint BootOffset;
        private uint BootSize;
        const int NTCSStartUpOffset = 2055720;
        const int NTCSStartUpSize = 55;
        const int NTCSBootOffset = 2018064;
        const int NTCSBootSize = 7;

        public ELFPatcher()
        {
            InitializeComponent();
        }

        private void Cancel_Button_Click(System.Object sender, System.EventArgs e)
        {
            this.Close();
        }
        private System.IO.FileStream ELFFile;
        private System.IO.BinaryReader ELFReader;
        private System.IO.BinaryWriter ELFWriter;
        private void ELFPather_Load(object sender, EventArgs e)
        {
            if (ELFOpen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ELFFile = new System.IO.FileStream(ELFOpen.FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                if (ELFOpen.FilterIndex == 1)
                {
                    StartUpOffset = PALStartUpOffset;
                    StartUpSize = PALStartUpSize;
                    BootOffset = PALBootOffset;
                    BootSize = PALBootSize;
                }
                else if (ELFOpen.FilterIndex == 2)
                {
                    StartUpOffset = NTCSStartUpOffset;
                    StartUpSize = NTCSStartUpSize;
                    BootOffset = NTCSBootOffset;
                    BootSize = NTCSBootSize;
                }
                TextBox1.MaxLength = (int)StartUpSize;
                TextBox1.Text = "";
                TextBox2.MaxLength = (int)BootSize;
                TextBox2.Text = "";
                ELFReader = new System.IO.BinaryReader(ELFFile);
                ELFFile.Position = StartUpOffset;
                byte b;
                do
                {
                    b = ELFReader.ReadByte();
                    TextBox1.Text += Strings.Chr(b);
                }
                while (~b == 0);
                ELFFile.Position = BootOffset;
                do
                {
                    b = ELFReader.ReadByte();
                    TextBox2.Text += Strings.Chr(b);
                }
                while (~b == 0);
                ELFReader.Close();
                ELFFile.Close();
            }
            else
                this.Close();
        }

        private void OK_Button_Click_1(object sender, EventArgs e)
        {
            ELFFile = new System.IO.FileStream(ELFOpen.FileName, System.IO.FileMode.Open, System.IO.FileAccess.Write);
            ELFWriter = new System.IO.BinaryWriter(ELFFile);
            if (CheckBox2.Checked)
            {
                ELFFile.Position = StartUpOffset;
                byte b = 0;
                for (int i = 0; i <= StartUpSize - 1; i++)
                    ELFWriter.Write(b);
                ELFFile.Position = StartUpOffset;
                for (int i = 0; i <= TextBox1.Text.Length - 1; i++)
                {
                    char c = TextBox1.Text[i];
                    ELFWriter.Write(c);
                }
            }
            if (CheckBox3.Checked)
            {
                ELFFile.Position = BootOffset;
                byte b = 0;
                for (int i = 0; i <= BootSize - 1; i++)
                    ELFWriter.Write(b);
                ELFFile.Position = BootOffset;
                for (int i = 0; i <= TextBox2.Text.Length - 1; i++)
                {
                    char c = TextBox2.Text[i];
                    ELFWriter.Write(c);
                }
            }
            ELFWriter.Close();
            ELFFile.Close();
            this.Close();
        }
    }
}
