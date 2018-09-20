using Twinsanity;

namespace TwinsaityEditor
{
    public class MaterialController : ItemController
    {
        private Material data;

        public MaterialController(Material item) : base(item)
        {
            data = item;
        }

        public override string GetName()
        {
            return data.Name + " [ID: " + data.ID + "]";
        }

        public override void GenText()
        {
            TextPrev = new string[5];
            TextPrev[0] = "ID: " + data.ID;
            TextPrev[1] = "Offset: " + data.Offset + " Size: " + data.Size;
            TextPrev[2] = "Name: " + data.Name + " Texture ID: " + data.Tex;
            TextPrev[3] = "Integers: " + data.ValuesI[0] + " " + data.ValuesI[1] + " " + data.ValuesI[2] + " " + data.ValuesI[3];
            TextPrev[4] = "Floats: " + data.ValuesF[0] + " " + data.ValuesF[1] + " " + data.ValuesF[2] + " " + data.ValuesF[3];
        }
    }
}
