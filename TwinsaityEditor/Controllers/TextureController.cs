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
            return string.Format("Texture [ID {0:X8}]", Data.ID);
        }

        protected override void GenText()
        {
            TextPrev = new string[5];
            TextPrev[0] = string.Format("ID: {0:X8}", Data.ID);
            TextPrev[1] = $"Offset: {Data.Offset} Size: {Data.Size}";
            TextPrev[2] = $"Image Size: {Data.Width}x{Data.Height}";
            TextPrev[3] = $"Type: {Data.type} Mips: {Data.Mips}";
            TextPrev[4] = $"Palette: {Data.Palette} Size: {Data.PaletteSize}";
        }
    }
}
