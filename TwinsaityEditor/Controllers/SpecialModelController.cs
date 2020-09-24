using Twinsanity;
using System.Collections.Generic;

namespace TwinsaityEditor
{
    public class SpecialModelController : ItemController
    {
        public new SpecialModel Data { get; set; }

        public SpecialModelController(MainForm topform, SpecialModel item) : base(topform, item)
        {
            Data = item;
            AddMenu("Open model viewer", Menu_OpenViewer);
        }

        protected override string GetName()
        {
            return string.Format("Special Model [ID {0:X8}]", Data.ID);
        }

        protected override void GenText()
        {
            List<string> text = new List<string>();

            text.Add(string.Format("ID: {0:X8}", Data.ID));
            text.Add($"Size: {Data.Size}");
            text.Add($"Models in LOD: {Data.ModelsAmount} ");
            for (int i = 0; i < Data.ModelsAmount; ++i)
            {
                text.Add($"Model ID {i} (LOD {(i == 0 ? (int)Data.ModelsAmount - 1 : i - 1)}): {Data.LODModelIDs[i]:X8} - Max Distance To Camera: {Data.LODDistance[i]}");
            }

            TextPrev = text.ToArray();

        }

        private void Menu_OpenViewer()
        {
            MainFile.OpenModelViewer(this);
        }
    }
}
