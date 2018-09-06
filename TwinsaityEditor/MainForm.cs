using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
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
        }

        private void GenTreeNode(KeyValuePair<uint, TwinsItem> a, TreeNode node)
        {
            TreeNode new_node = new TreeNode("TwinsItem");
            if (a.Value is TwinsSection)
            {
                foreach (var i in ((TwinsSection)a.Value).SecInfo.Records)
                {
                    GenTreeNode(i, new_node);
                }
                new_node.Text = "TwinsSection";
            }
            new_node.Text += " [ID: " + a.Key + "]";
            node.Nodes.Add(new_node);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openRM2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "RM2/RMX files|*.rm*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                switch (ofd.FilterIndex)
                {
                    case 1:
                        fileData.LoadRM(ofd.FileName);
                        break;
                }
                GenTree();
                Text = "Twinsaity Editor by Neo_Kesha [" + ofd.FileName + "] ";
            }
        }
    }
}
