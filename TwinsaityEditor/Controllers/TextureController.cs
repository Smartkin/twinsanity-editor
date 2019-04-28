using Twinsanity;

namespace TwinsaityEditor
{
    public class TextureController : ItemController
    {
        public new Texture Data { get; set; }

        public TextureController(MainForm topform, Texture item) : base (topform, item)
        {
            Data = item;
        }

        protected override string GetName()
        {
            return "Texture [ID " + Data.ID + "]";
        }

        protected override void GenText()
        {
            TextPrev = new string[3];
            TextPrev[0] = "ID: " + Data.ID;
            TextPrev[1] = "Offset: " + Data.Offset + " Size: " + Data.Size;
            TextPrev[2] = "Image Size: " + Data.Width + "x" + Data.Height;
        }
    }
}
