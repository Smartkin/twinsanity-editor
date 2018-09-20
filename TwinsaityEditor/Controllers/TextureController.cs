using Twinsanity;

namespace TwinsaityEditor
{
    public class TextureController : ItemController
    {
        private Texture data;

        public TextureController(Texture item) : base(item)
        {
            data = item;
        }

        public override string GetName()
        {
            return "Texture [ID: " + data.ID + "]";
        }

        public override void GenText()
        {
            TextPrev = new string[3];
            TextPrev[0] = "ID: " + data.ID;
            TextPrev[1] = "Offset: " + data.Offset + " Size: " + data.Size;
            TextPrev[2] = "Image Size: " + data.Width + "x" + data.Height;
        }
    }
}
