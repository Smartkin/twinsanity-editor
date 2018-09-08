using System;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    /// <summary>
    /// Flags corresponding to buttons in the toolbar to the right.
    /// </summary>
    [Flags]
    public enum ToolbarFlags
    {
        Hex     = 0x0001,
        Extract = 0x0002,
        Replace = 0x0004,
        Search  = 0x0008,
        Add     = 0x0010,
        Create  = 0x0020,
        Delete  = 0x0040,
        Export  = 0x0080,
        View    = 0x0100,
        Script  = 0x0200,
        Edit    = 0x0400
    }

    public partial class MainForm : Form
    {
        private TwinsFile fileData = new TwinsFile();

        public MainForm()
        {
            InitializeComponent();
        }

        private void GenTree()
        {
            groupBox1.Enabled = true;
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add("Root");
            foreach (var i in fileData.SecInfo.Records.Values)
            {
                GenTreeNode(i, treeView1.TopNode);
            }
            treeView1.TopNode.Expand();
            treeView1.AfterSelect += TreeNodeSelect;
        }

        private void TreeNodeSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is Controller)
            {
                Controller c = (Controller)e.Node.Tag;
                if (c.Dirty)
                {
                    e.Node.Text = c.GetName();
                    c.Dirty = false;
                }
                textBox1.Lines = c.TextPrev;
                buttonHex.Enabled = (c.Toolbar & ToolbarFlags.Hex) != 0;
                buttonExt.Enabled = (c.Toolbar & ToolbarFlags.Extract) != 0;
                buttonRep.Enabled = (c.Toolbar & ToolbarFlags.Replace) != 0;
                buttonAdd.Enabled = (c.Toolbar & ToolbarFlags.Add) != 0;
                buttonCre.Enabled = (c.Toolbar & ToolbarFlags.Create) != 0;
                buttonDel.Enabled = (c.Toolbar & ToolbarFlags.Delete) != 0;
                buttonEdt.Enabled = (c.Toolbar & ToolbarFlags.Edit) != 0;
                buttonExp.Enabled = (c.Toolbar & ToolbarFlags.Export) != 0;
                buttonScr.Enabled = (c.Toolbar & ToolbarFlags.Script) != 0;
                buttonSrc.Enabled = (c.Toolbar & ToolbarFlags.Search) != 0;
                buttonViw.Enabled = (c.Toolbar & ToolbarFlags.View) != 0;
            }
            else if (e.Node == treeView1.TopNode)
            {
                buttonHex.Enabled = buttonExt.Enabled = buttonRep.Enabled = buttonAdd.Enabled = buttonCre.Enabled = 
                buttonDel.Enabled = buttonEdt.Enabled = buttonExp.Enabled = buttonScr.Enabled = buttonSrc.Enabled = 
                buttonViw.Enabled = false;
            }
        }

        private void GenTreeNode(TwinsItem a, TreeNode node)
        {
            TreeNode new_node = new TreeNode();
            if (a is TwinsSection)
            {
                foreach (var i in ((TwinsSection)a).SecInfo.Records.Values)
                {
                    GenTreeNode(i, new_node);
                }
                new_node.Tag = new SectionController((TwinsSection)a);
            }
            else if (a is ColData)
                new_node.Tag = new ColDataController((ColData)a);
            else
                new_node.Tag = new ItemController(a);
            new_node.Text = ((Controller)new_node.Tag).GetName();
            node.Nodes.Add(new_node);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openRM2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "RM2/RMX files|*.rm*|SM2/SMX files|*.sm*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                switch (ofd.FilterIndex)
                {
                    case 1:
                        fileData.LoadFile(ofd.FileName, false);
                        break;
                    case 2:
                        fileData.LoadFile(ofd.FileName, true);
                        break;
                }
                GenTree();
                Text = "Twinsaity Editor by Neo_Kesha [" + ofd.FileName + "] ";
            }
        }

        private void graphicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fm = new Workers.TextureImport();
            fm.Show();
        }
    }
}
