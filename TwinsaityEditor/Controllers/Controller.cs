using System.Windows.Forms;

namespace TwinsaityEditor
{
    public abstract class Controller
    {
        private readonly string name = "PEEPEE POOPOO";

        public string TextPrev { get; set; }

        public abstract string GetName();
        public abstract string GetText();
    }
}
