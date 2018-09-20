using Twinsanity;

namespace TwinsaityEditor
{
    public class FileController : Controller
    {
        private static TwinsFile data;

        public FileController(TwinsFile item)
        {
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
            if (data.SecInfo.Records.ContainsKey(sector) && ((TwinsSection)data.SecInfo.Records[sector]).SecInfo.Records.ContainsKey(6))// && ((TwinsSection)((TwinsSection)data.SecInfo.Records[sector]).SecInfo.Records[6]).SecInfo.Records.ContainsKey(id))
            {
                int i = 0;
                foreach (var j in ((TwinsSection)((TwinsSection)data.SecInfo.Records[sector]).SecInfo.Records[6]).SecInfo.Records)
                {
                    if (i++ == id)
                        return (Instance)j.Value;
                }
                throw new System.ArgumentException("The requested section does not have an instance in the specified position.");
            }
            else throw new System.ArgumentException("The requested section does not have an object instance section.");
        }

        public static ref TwinsFile GetFile()
        {
            return ref data;
        }
    }
}
