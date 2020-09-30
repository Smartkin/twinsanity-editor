using Twinsanity;

namespace TwinsaityEditor
{
    public class SkydomeController : ItemController
    {
        public new Skydome Data { get; set; }

        public SkydomeController(MainForm topform, Skydome item) : base (topform, item)
        {
            Data = item;
            AddMenu("Open skydome viewer", Menu_OpenViewer);
        }

        protected override string GetName()
        {
            return string.Format("Skydome [ID {0:X8}]", Data.ID);
        }

        protected override void GenText()
        {
            TextPrev = new string[3 + Data.ModelIDs.Length];
            TextPrev[0] = string.Format("ID: {0:X8}", Data.ID);
            TextPrev[1] = $"Size: {Data.Size}";
            TextPrev[2] = $"Unknown: {Data.Unknown} ModelCount: {Data.ModelIDs.Length}";
            for (int i = 0; i < Data.ModelIDs.Length; ++i)
                TextPrev[3 + i] = string.Format("{0:X8}", Data.ModelIDs[i]);
        }

        private void Menu_OpenViewer()
        {
            MainFile.OpenSkydomeViewer(this);
        }
    }
}
