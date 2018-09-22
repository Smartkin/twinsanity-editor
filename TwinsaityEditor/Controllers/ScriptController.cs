using Twinsanity;

namespace TwinsaityEditor
{
    public class ScriptController : ItemController
    {
        public new Script Data { get; set; }

        public ScriptController(Script item) : base(item)
        {
            Data = item;
        }

        public override string GetName()
        {
            return (Data.Name != null ? Data.Name : "Script") + " [ID " + Data.ID + "]";
        }

        public override void GenText()
        {
            TextPrev = new string[2];
            TextPrev[0] = "ID: " + Data.ID + (Data.Name != null ? " Name: " + Data.Name : string.Empty);
            TextPrev[1] = "Offset: " + Data.Offset + " Size: " + Data.Size;
        }
    }
}
