using Twinsanity;

namespace TwinsaityEditor
{
    public class SectionController : Controller
    {
        private TwinsSection data;

        public SectionController(TwinsSection item)
        {
            Toolbar = ToolbarFlags.Search | ToolbarFlags.Add | ToolbarFlags.Create | ToolbarFlags.Export;
            data = item;
        }

        public override string GetName()
        {
            return data.Type + " Section [ID: " + data.ID + "]";
        }

        public override void GenText()
        {
            TextPrev = new string[3];
            TextPrev[0] = "ID: " + data.ID;
            TextPrev[1] = "Offset: " + data.Offset + " Size: " + data.Size;
            TextPrev[2] = "ContentSize: " + data.ContentSize + " Element Count: " + data.SecInfo.Records.Count;
        }
    }
}
