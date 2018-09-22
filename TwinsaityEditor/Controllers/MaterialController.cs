using Twinsanity;

namespace TwinsaityEditor
{
    public class MaterialController : ItemController
    {
        public new Material Data { get; set; }

        public MaterialController(Material item) : base(item)
        {
            Data = item;
        }

        public override string GetName()
        {
            return Data.Name + " [ID " + Data.ID + "]";
        }

        public override void GenText()
        {
            TextPrev = new string[5];
            TextPrev[0] = "ID: " + Data.ID;
            TextPrev[1] = "Offset: " + Data.Offset + " Size: " + Data.Size;
            TextPrev[2] = "Name: " + Data.Name + " Texture ID: " + Data.Tex;
            TextPrev[3] = "Integers: " + Data.ValuesI[0] + " " + Data.ValuesI[1] + " " + Data.ValuesI[2] + " " + Data.ValuesI[3];
            TextPrev[4] = "Floats: " + Data.ValuesF[0] + " " + Data.ValuesF[1] + " " + Data.ValuesF[2] + " " + Data.ValuesF[3];
        }
    }
}
