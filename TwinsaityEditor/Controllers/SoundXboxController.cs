using System;
using System.IO;
using System.Media;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public class SoundXboxController : ItemController
    {
        public new SoundEffectX Data { get; set; }
        public byte[] SoundData { get; set; }
        private bool Loaded;

        private static SoundPlayer player = new SoundPlayer();

        public SoundXboxController(MainForm topform, SoundEffectX item) : base(topform, item)
        {
            Data = item;
            //LoadSoundData();
            AddMenu("Play sound", Menu_PlaySound);
            AddMenu("Export to .WAV", Menu_ExportWAV);
            AddMenu("Export to .VAG", Menu_ExportVAG);
            AddMenu("Replace sound from .WAV", Menu_ReplaceSoundWav);
        }

        protected override string GetName()
        {
            return $"Sound Effect [ID {Data.ID}]";
        }

        protected override void GenText()
        {
            TextPrev = new string[3];
            TextPrev[0] = $"ID: {Data.ID}";
            TextPrev[1] = $"Size: {Data.Size}";
            TextPrev[2] = $"Frequency: {Data.Freq} ({Data.Freq}Hz)";
        }

        public void LoadSoundData()
        {
            if (Loaded) return;
            Loaded = true;
            try
            {
                SoundData = RIFF.SaveRiff(Data.SoundData, 1, Data.Freq);
            }
            catch
            {
                Console.WriteLine("Error loading sound data: ID " + Data.ID);
            }
        }

        private void Menu_PlaySound()
        {
            LoadSoundData();
            player.Stop();
            player.Stream = new MemoryStream(SoundData);
            player.Play();
        }

        private void Menu_ExportWAV()
        {
            LoadSoundData();
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "WAV|*.wav";
            sfd.FileName = Data.ID.ToString();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileStream file = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                BinaryWriter writer = new BinaryWriter(file);
                writer.Write(SoundData);
                writer.Close();
            }
        }

        private void Menu_ExportVAG()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "VAG|*.vag";
            var id_str = Data.ID.ToString();
            sfd.FileName = id_str;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileStream file = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                BinaryWriter writer = new BinaryWriter(file);
                writer.Write("VAGp".ToCharArray());
                writer.Write(20);
                writer.Write(0);
                writer.Write(Data.SoundData.Length);
                writer.Write(BitConv.FlipBytes(Data.Freq));
                writer.Write(0); writer.Write(0); writer.Write(0);
                char[] name = new char[16] { (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0, (char)0 };
                Array.Copy(id_str.ToCharArray(), name, Math.Min(id_str.Length, 16));
                writer.Write(name);
                //writer.Write(0); writer.Write(0); writer.Write(0); writer.Write(0);
                writer.Write(Data.SoundData);
                writer.Close();
            }
        }

        private void Menu_ReplaceSoundWav()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "WAV|*.wav";
            ofd.FileName = Data.ID.ToString();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using (FileStream file = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                using (BinaryReader reader = new BinaryReader(file))
                {
                    file.Position = 0x16;
                    UInt16 channels = reader.ReadUInt16();
                    UInt32 frequency = reader.ReadUInt32();
                    file.Position = 0x28;
                    int len = reader.ReadInt32();
                    Byte[] PCM = reader.ReadBytes(len);
                    if (channels == 1)
                    {
                        Data.Freq = (ushort)frequency;
                        //Byte[] newData = ADPCM.FromPCMMono(PCM);
                        UInt32 newSize = (uint)PCM.Length;
                        Data.SoundData = PCM;
                        Data.UnkInt = (int)newSize;
                    }
                    else
                    {
                        throw new ArgumentException("ATM only mono, sorry fam");
                    }
                    Loaded = false;
                    LoadSoundData();
                    GenText();
                }
            }
        }
    }
}
