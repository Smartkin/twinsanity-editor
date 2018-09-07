namespace TwinsaityEditor
{
    public abstract class Controller
    {
        public string[] TextPrev { get; set; }
        public bool Dirty { get; set; } = true;

        public abstract string GetName();
        protected abstract void GenText();
    }
}
