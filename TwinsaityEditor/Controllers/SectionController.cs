using Twinsanity;

namespace TwinsaityEditor
{
    public class SectionController : Controller
    {
        private TwinsSection data;
        private uint id;

        public SectionController(uint id, TwinsSection item)
        {
            this.id = id;
            data = item;
            GenText();
        }

        public override string GetName()
        {
            return "Section [ID: " + id + "]";
        }

        protected override void GenText()
        {
            TextPrev = new string[3];
            TextPrev[0] = "ID: " + id;
            TextPrev[1] = "Offset: " + data.Offset + " Size: " + data.Size;
            TextPrev[2] = "ContentSize: " + (data.Size - (data.SecInfo.Records.Count + 1) * 12) + " Element Count: " + data.SecInfo.Records.Count;
        }
    }
}
