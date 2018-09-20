using System.Windows.Forms;
using System.IO;
using Twinsanity;

namespace TwinsaityEditor
{
    public class ScriptController : ItemController
    {
        private Script data;

        public ScriptController(Script item) : base(item)
        {
            data = item;
        }

        public override string GetName()
        {
            return (data.Name != null ? data.Name : "Script") + " [ID: " + data.ID + "]";
        }

        public override void GenText()
        {
            TextPrev = new string[2];
            TextPrev[0] = "ID: " + data.ID + (data.Name != null ? " Name: " + data.Name : string.Empty);
            TextPrev[1] = "Offset: " + data.Offset + " Size: " + data.Size;
        }
    }
}
