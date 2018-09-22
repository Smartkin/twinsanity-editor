using Twinsanity;

namespace TwinsaityEditor
{
    public class FileController : Controller
    {
        public static TwinsFile Data { get; set; }

        public FileController(TwinsFile item)
        {
            Data = item;
        }

        public override string GetName()
        {
            return "File";
        }

        public override void GenText()
        {
            TextPrev = new string[2];
            TextPrev[0] = "Size: " + Data.Size;
            TextPrev[1] = "ContentSize: " + Data.ContentSize + " Element Count: " + Data.SecInfo.Records.Count;
        }

        public static string GetMaterialName(uint id)
        {
            if (Data.SecInfo.Records.ContainsKey(11) && ((TwinsSection)Data.SecInfo.Records[11]).SecInfo.Records.ContainsKey(1) && ((TwinsSection)((TwinsSection)Data.SecInfo.Records[11]).SecInfo.Records[1]).SecInfo.Records.ContainsKey(id))
                return ((Material)((TwinsSection)((TwinsSection)Data.SecInfo.Records[11]).SecInfo.Records[1]).SecInfo.Records[id]).Name; //lol
            else return string.Empty;
        }

        public static string GetObjectName(uint id)
        {
            if (Data.SecInfo.Records.ContainsKey(10) && ((TwinsSection)Data.SecInfo.Records[10]).SecInfo.Records.ContainsKey(0) && ((TwinsSection)((TwinsSection)Data.SecInfo.Records[10]).SecInfo.Records[0]).SecInfo.Records.ContainsKey(id))
                return ((GameObject)((TwinsSection)((TwinsSection)Data.SecInfo.Records[10]).SecInfo.Records[0]).SecInfo.Records[id]).Name; //lol
            else return string.Empty;
        }

        public static string GetScriptName(uint id)
        {
            if (Data.SecInfo.Records.ContainsKey(10) && ((TwinsSection)Data.SecInfo.Records[10]).SecInfo.Records.ContainsKey(1) && ((TwinsSection)((TwinsSection)Data.SecInfo.Records[10]).SecInfo.Records[1]).SecInfo.Records.ContainsKey(id))
                return ((Script)((TwinsSection)((TwinsSection)Data.SecInfo.Records[10]).SecInfo.Records[1]).SecInfo.Records[id]).Name; //lol
            else return string.Empty;
        }

        public static Instance GetInstance(uint sector, uint id)
        {
            if (Data.SecInfo.Records.ContainsKey(sector) && ((TwinsSection)Data.SecInfo.Records[sector]).SecInfo.Records.ContainsKey(6))// && ((TwinsSection)((TwinsSection)Data.SecInfo.Records[sector]).SecInfo.Records[6]).SecInfo.Records.ContainsKey(id))
            {
                int i = 0;
                foreach (var j in ((TwinsSection)((TwinsSection)Data.SecInfo.Records[sector]).SecInfo.Records[6]).SecInfo.Records)
                {
                    if (i++ == id)
                        return (Instance)j.Value;
                }
                throw new System.ArgumentException("The requested section does not have an instance in the specified position.");
            }
            else throw new System.ArgumentException("The requested section does not have an object instance section.");
        }

        public static TwinsFile GetFile()
        {
            return Data;
        }
    }
}
