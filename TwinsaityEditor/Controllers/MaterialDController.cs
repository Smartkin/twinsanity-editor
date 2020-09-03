using Twinsanity;

namespace TwinsaityEditor
{
    public class MaterialDController : ItemController
    {
        public new MaterialDemo Data { get; set; }

        public MaterialDController(MainForm topform, MaterialDemo item) : base(topform, item)
        {
            Data = item;
        }

        protected override string GetName()
        {
            return string.Format("{1} [ID {0:X8}]", Data.ID, Data.Name);
        }

        protected override void GenText()
        {
            TextPrev = new string[5];
            TextPrev[0] = string.Format("ID: {0:X8}", Data.ID);
            TextPrev[1] = $"Size: {Data.Size}";
            TextPrev[2] = $"Name: {Data.Name} Texture ID: {Data.Tex}";
            TextPrev[3] = $"Integers: {Data.ValuesI[0]} {Data.ValuesI[1]} {Data.ValuesI[2]} {Data.ValuesI[3]}";
            TextPrev[4] = $"Floats: {Data.ValuesF[0]} {Data.ValuesF[1]} {Data.ValuesF[2]} {Data.ValuesF[3]}";
        }
    }
}
