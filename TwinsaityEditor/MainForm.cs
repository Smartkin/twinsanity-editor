using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public partial class MainForm : Form
    {
        private TwinsFile fileData = new TwinsFile();

        public MainForm()
        {
            InitializeComponent();
        }

        private void GenTree()
        {
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add("Root");
            foreach (var i in fileData.SecInfo.Records)
            {
                GenTreeNode(i, treeView1.TopNode);
            }
            treeView1.TopNode.Expand();
            treeView1.AfterSelect += TreeNodeSelect;
        }

        private void TreeNodeSelect(object sender, TreeViewEventArgs e)
        {
            if (!(e.Node.Tag is Controller))
                return;
            Controller c = (Controller)e.Node.Tag;
            if (c.Dirty)
            {
                e.Node.Text = c.GetName();
                c.Dirty = false;
            }
            textBox1.Lines = c.TextPrev;
        }

        private void GenTreeNode(KeyValuePair<uint, TwinsItem> a, TreeNode node)
        {
            TreeNode new_node = new TreeNode();
            if (a.Value is TwinsSection)
            {
                foreach (var i in ((TwinsSection)a.Value).SecInfo.Records)
                {
                    GenTreeNode(i, new_node);
                }
                new_node.Tag = new SectionController(a.Key, (TwinsSection)a.Value);
            }
            else
                new_node.Tag = new ItemController(a.Key, a.Value);
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
    }
}
