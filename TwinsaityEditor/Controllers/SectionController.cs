using Twinsanity;

namespace TwinsaityEditor
{
    public class SectionController : Controller
    {
        private TwinsSection data;

        public SectionController(TwinsSection item)
        {
            data = item;
            GenText();
        }

        public override string GetName()
        {
            return "Section [ID: " + data.ID + "]";
        }

        protected override void GenText()
        {
            TextPrev = new string[3];
            TextPrev[0] = "ID: " + data.ID;
            TextPrev[1] = "Offset: " + data.Offset + " Size: " + data.Size;
            TextPrev[2] = "ContentSize: " + (data.Size - (data.SecInfo.Records.Count + 1) * 12) + " Element Count: " + data.SecInfo.Records.Count;
        }
    }
}
