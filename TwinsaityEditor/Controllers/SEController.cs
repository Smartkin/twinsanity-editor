using System;
using System.IO;
using System.Media;
using Twinsanity;

namespace TwinsaityEditor
{
    public class SEController : ItemController
    {
        public new SoundEffect Data { get; set; }

        public byte[] RawData { get; set; }
        public byte[] SoundData { get; set; }

        private static SoundPlayer player = new SoundPlayer();

        public SEController(MainForm topform, SoundEffect item) : base (topform, item)
        {
            Data = item;
            LoadSoundData();
            AddMenu("Play sound", Menu_PlaySound);
        }

        protected override string GetName()
        {
            return $"Sound Effect [ID {Data.ID}]";
        }

        protected override void GenText()
        {
            TextPrev = new string[3];
            TextPrev[0] = $"ID: {Data.ID}";
            TextPrev[1] = $"Offset: {Data.Offset} Size: {Data.Size}";
            TextPrev[2] = $"Frequency: {Data.Frequency} ({Data.Freq})";
        }

        public void LoadSoundData()
        {
            RawData = new byte[Data.SoundSize];
            Array.Copy(Data.Parent.ExtraData, Data.SoundOffset, RawData, 0, Data.SoundSize);
            SoundData = RIFF.SaveRiff(ADPCM.ToPCMMono(RawData, (int)Data.SoundSize), 1, Data.Freq);
        }

        private void Menu_PlaySound()
        {
            player.Stream = new MemoryStream(SoundData);
            player.Play();
        }
    }
}
