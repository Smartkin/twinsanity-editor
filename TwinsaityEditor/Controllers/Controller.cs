namespace TwinsaityEditor
{
    public abstract class Controller
    {
        public string[] TextPrev { get; set; }
        /// <summary>
        /// Determines whether text preview needs to be regenerated or not.
        /// </summary>
        public bool Dirty { get; set; } = true;
        /// <summary>
        /// FLAGS. Determines which buttons are enabled on the toolbar for this controller.
        /// </summary>
        public ToolbarFlags Toolbar { get; set; }

        public abstract string GetName();
        public abstract void GenText();

        /// <summary>
        /// Function called when a toolbar button is clicked.
        /// </summary>
        /// <param name="button">The button clicked.</param>
        public virtual void ToolbarAction(ToolbarFlags button) { }

        public virtual void Dispose() { }
    }
}
