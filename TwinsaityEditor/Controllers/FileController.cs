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
            if (data.SecInfo.Records.ContainsKey(11) && ((TwinsSection)data.SecInfo.Records[11]).SecInfo.Records.ContainsKey(1) && ((TwinsSection)((TwinsSection)data.SecInfo.Records[11]).SecInfo.Records[1]).SecInfo.Records.ContainsKey(id))
                return ((Material)((TwinsSection)((TwinsSection)data.SecInfo.Records[11]).SecInfo.Records[1]).SecInfo.Records[id]).Name; //lol
            else return string.Empty;
        }

        public static string GetObjectName(uint id)
        {
            if (data.SecInfo.Records.ContainsKey(10) && ((TwinsSection)data.SecInfo.Records[10]).SecInfo.Records.ContainsKey(0) && ((TwinsSection)((TwinsSection)data.SecInfo.Records[10]).SecInfo.Records[0]).SecInfo.Records.ContainsKey(id))
                return ((GameObject)((TwinsSection)((TwinsSection)data.SecInfo.Records[10]).SecInfo.Records[0]).SecInfo.Records[id]).Name; //lol
            else return string.Empty;
        }

        public static string GetScriptName(uint id)
        {
            if (data.SecInfo.Records.ContainsKey(10) && ((TwinsSection)data.SecInfo.Records[10]).SecInfo.Records.ContainsKey(1) && ((TwinsSection)((TwinsSection)data.SecInfo.Records[10]).SecInfo.Records[1]).SecInfo.Records.ContainsKey(id))
                return ((Script)((TwinsSection)((TwinsSection)data.SecInfo.Records[10]).SecInfo.Records[1]).SecInfo.Records[id]).Name; //lol
            else return string.Empty;
        }

        public static Instance GetInstance(uint sector, uint id)
        {
            if (data.SecInfo.Records.ContainsKey(sector) && ((TwinsSection)data.SecInfo.Records[sector]).SecInfo.Records.ContainsKey(6) && ((TwinsSection)((TwinsSection)data.SecInfo.Records[sector]).SecInfo.Records[6]).SecInfo.Records.ContainsKey(id))
                return ((Instance)((TwinsSection)((TwinsSection)data.SecInfo.Records[sector]).SecInfo.Records[6]).SecInfo.Records[id]);
            else throw new System.MissingFieldException("The requested instance could not be found.");
        }

        public static ref TwinsFile GetFile()
        {
            return ref data;
        }
    }
}
