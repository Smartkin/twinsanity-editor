using System.Windows.Forms;
using System;

namespace TwinsaityEditor
{
    public delegate void ControllerAddMenuDelegate();

    public abstract class Controller : IDisposable
    {
        protected MainForm TopForm { get; private set; }
        public bool Selected { get; set; }
        public string[] TextPrev { get; set; }
        public TreeNode Node { get; set; }
        public ContextMenu ContextMenu { get; set; } = new ContextMenu();
        
        public Controller(MainForm topform)
        {
            TopForm = topform;
            Node = new TreeNode { Tag = this, ContextMenu = ContextMenu };
        }

        public void AddNode(Controller controller)
        {
            Node.Nodes.Add(controller.Node);
        }

        protected void AddMenu(string text, ControllerAddMenuDelegate func)
        {
            EventHandler handler = delegate (object sender, EventArgs e)
            {
                func();
            };
            ContextMenu.MenuItems.Add(text, handler);
        }

        /// <summary>
        /// Dispose of a node.
        /// </summary>
        /// <param name="node">Node to be disposed of.</param>
        public static void DisposeNode(TreeNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");
            ((Controller)node.Tag).Dispose();
            while (node.Nodes.Count > 0)
                DisposeNode(node.Nodes[0]);
            node.Remove();
        }

        protected abstract string GetName();
        protected abstract void GenText();

        public void UpdateText()
        {
            GenText();
            Node.Text = GetName();
            if (Selected)
                ((MainForm)Node.TreeView.TopLevelControl).ControllerNodeSelect(this);
        }

        public virtual void Dispose() { }
    }
}
