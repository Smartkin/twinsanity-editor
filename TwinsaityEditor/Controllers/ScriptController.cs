using Twinsanity;

namespace TwinsaityEditor
{
    public class ScriptController : ItemController
    {
        public new Script Data { get; set; }

        public ScriptController(MainForm topform, Script item) : base (topform, item)
        {
            Data = item;
        }

        protected override string GetName()
        {
            return (Data.Name ?? "Script") + " [ID " + Data.ID + "]";
        }

        protected override void GenText()
        {
            TextPrev = new string[2];
            TextPrev[0] = "ID: " + Data.ID + (Data.Name != null ? " Name: " + Data.Name : string.Empty);
            TextPrev[1] = "Offset: " + Data.Offset + " Size: " + Data.Size;
        }
    }
}
