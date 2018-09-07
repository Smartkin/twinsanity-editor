namespace TwinsaityEditor
{
    public abstract class Controller
    {
        public string[] TextPrev { get; set; }

        public abstract string GetName();
        protected abstract void GenText();
        public bool Dirty { get; set; } = true;
    }
}
