using Twinsanity;

namespace TwinsaityEditor
{
    public class FileController : Controller
    {
        private TwinsFile data;

        public FileController(TwinsFile item)
        {
            Toolbar = ToolbarFlags.Search;
            data = item;
        }

        public override string GetName()
        {
            return "File";
        }

        public override void GenText()
        {
            TextPrev = new string[2];
            TextPrev[0] = "Size: " + data.Size;
            TextPrev[1] = "ContentSize: " + data.ContentSize + " Element Count: " + data.SecInfo.Records.Count;
        }
    }
}
