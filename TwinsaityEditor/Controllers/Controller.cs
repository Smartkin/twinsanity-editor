using System.Windows.Forms;
using System;

namespace TwinsaityEditor
{
    public delegate void ControllerAddMenuDelegate();

    public abstract class Controller
    {
        public string[] TextPrev { get; set; }
        /// <summary>
        /// Determines whether text preview needs to be regenerated or not.
        /// </summary>
        public bool Dirty { get; set; }
        public TreeNode Node { get; set; }
        public ContextMenu ContextMenu { get; set; } = new ContextMenu();
        
        public Controller()
        {
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
        public void DisposeNode(TreeNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");
            ((Controller)node.Tag).Dispose();
            while (node.Nodes.Count > 0)
                DisposeNode(node.Nodes[0]);
            node.Remove();
        }

        public abstract string GetName();
        public abstract void GenText();

        public virtual void Dispose() { }
    }
}
