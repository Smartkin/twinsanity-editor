using Twinsanity;

namespace TwinsaityEditor
{
    public class FileController : Controller
    {
        private static TwinsFile data;

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

        public static string GetMaterialName(uint id)
        {
            return ((Material)((TwinsSection)((TwinsSection)data.SecInfo.Records[11]).SecInfo.Records[1]).SecInfo.Records[id]).Name; //lol
        }

        public static string GetObjectName(uint id)
        {
            return ((GameObject)((TwinsSection)((TwinsSection)data.SecInfo.Records[10]).SecInfo.Records[0]).SecInfo.Records[id]).Name; //lol
        }

        //public static string GetScriptName(uint id)
        //{
        //    return ((Script)((TwinsSection)((TwinsSection)data.SecInfo.Records[10]).SecInfo.Records[0]).SecInfo.Records[id]).Name; //lol
        //}
    }
}
