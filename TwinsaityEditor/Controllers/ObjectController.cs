using System;
using Twinsanity;

namespace TwinsaityEditor
{
    public class ObjectController : ItemController
    {
        public new GameObject Data { get; set; }

        public ObjectController(MainForm topform, GameObject item) : base (topform, item)
        {
            Data = item;
        }

        protected override string GetName()
        {
            return $"{Data.Name} [ID {Data.ID}]";
        }

        protected override void GenText()
        {
            TextPrev = new string[11 + Data.UI32.Length + Data.OGIs.Length + Data.Anims.Length + Data.Scripts.Length + Data.Objects.Length + Data.Sounds.Length + Data.cCM.Length];
            TextPrev[0] = $"ID: {Data.ID}";
            TextPrev[1] = $"Offset: {Data.Offset} Size: {Data.Size}";
            TextPrev[2] = $"Name: {Data.Name}";
            TextPrev[3] = $"Headers: {Data.Class1.ToString("X")} {Data.Class2.ToString("X")} {Data.Class3.ToString("X")}";

            TextPrev[4] = $"UnknownInt32Count: {Data.UI32.Length}";
            for (int i = 0; i < Data.UI32.Length; ++i)
                TextPrev[5 + i] = Data.UI32[i].ToString("X");

            TextPrev[5 + Data.UI32.Length] = $"OGICount: {Data.OGIs.Length}";
            for (int i = 0; i < Data.OGIs.Length; ++i)
                TextPrev[6 + Data.UI32.Length + i] = Data.OGIs[i].ToString();

            TextPrev[6 + Data.UI32.Length + Data.OGIs.Length] = $"AnimCount: {Data.Anims.Length}";
            for (int i = 0; i < Data.Anims.Length; ++i)
                TextPrev[7 + Data.UI32.Length + Data.OGIs.Length + i] = Data.Anims[i].ToString();

            TextPrev[7 + Data.UI32.Length + Data.OGIs.Length + Data.Anims.Length] = $"ScriptCount: {Data.Scripts.Length}";
            for (int i = 0; i < Data.Scripts.Length; ++i)
                TextPrev[8 + Data.UI32.Length + Data.OGIs.Length + Data.Anims.Length + i] = Data.Scripts[i].ToString();

            TextPrev[8 + Data.UI32.Length + Data.OGIs.Length + Data.Anims.Length + Data.Scripts.Length] = $"ObjectCount: {Data.Objects.Length}";
            for (int i = 0; i < Data.Objects.Length; ++i)
                TextPrev[9 + Data.UI32.Length + Data.OGIs.Length + Data.Anims.Length + Data.Scripts.Length + i] = Data.Objects[i].ToString();

            TextPrev[9 + Data.UI32.Length + Data.OGIs.Length + Data.Anims.Length + Data.Scripts.Length + Data.Objects.Length] = $"SoundCount: {Data.Sounds.Length}";
            for (int i = 0; i < Data.Sounds.Length; ++i)
                TextPrev[10 + Data.UI32.Length + Data.OGIs.Length + Data.Anims.Length + Data.Scripts.Length + Data.Objects.Length + i] = Data.Sounds[i].ToString();

            TextPrev[10 + Data.UI32.Length + Data.OGIs.Length + Data.Anims.Length + Data.Scripts.Length + Data.Objects.Length + Data.Sounds.Length] = $"CMCount: {Data.cCM.Length}";
            for (int i = 0; i < Data.cCM.Length; ++i)
                TextPrev[11 + Data.UI32.Length + Data.OGIs.Length + Data.Anims.Length + Data.Scripts.Length + Data.Objects.Length + Data.Sounds.Length + i] = Data.cCM[i].ToString();
        }
    }
}
