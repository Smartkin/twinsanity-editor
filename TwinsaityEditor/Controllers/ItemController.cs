using Twinsanity;

namespace TwinsaityEditor
{
    public class ItemController : Controller
    {
        private TwinsItem data;

        public ItemController(TwinsItem item)
        {
            Toolbar = ToolbarFlags.Hex | ToolbarFlags.Extract | ToolbarFlags.Replace | ToolbarFlags.Delete;
            data = item;
            GenText();
        }

        public override string GetName()
        {
            return "Item [ID: " + data.ID + "]";
        }

        protected override void GenText()
        {
            TextPrev = new string[2];
            TextPrev[0] = "ID: " + data.ID;
            TextPrev[1] = "Offset: " + data.Offset + " Size: " + data.Size;
        }
    }
}
