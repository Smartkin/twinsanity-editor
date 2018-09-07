using Twinsanity;

namespace TwinsaityEditor
{
    public class ItemController : Controller
    {
        private TwinsItem data;
        private uint id;

        public ItemController(uint id, TwinsItem item)
        {
            this.id = id;
            data = item;
            GenText();
        }

        public override string GetName()
        {
            return "Item [ID: " + id + "]";
        }

        protected override void GenText()
        {
            TextPrev = new string[2];
            TextPrev[0] = "ID: " + id;
            TextPrev[1] = "Offset: " + data.Offset + " Size: " + data.Size;
        }
    }
}
