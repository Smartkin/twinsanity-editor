using System.Windows.Forms;
using System;
using Twinsanity;

namespace TwinsaityEditor
{
    public partial class TriggerTreeForm
    {
        public GeoData.VertexMaps[] Trigg;
        private void TriggerTree_Load(object sender, EventArgs e)
        {
            TreeView1.Nodes.Clear();
            TreeView1.Nodes.Add("Tree Root");
            TreeView1.BeginUpdate();
            AddNode(TreeView1.TopNode, 0);
            TreeView1.EndUpdate();
        }
        private void AddNode(TreeNode Node, int index)
        {
            Node.Nodes.Add("ID: " + index.ToString() + " Tier: " + (Node.Level).ToString());
            if (Trigg[index].Flag1 >= 0)
            {
                AddNode(Node.LastNode, Trigg[index].Flag1);
                AddNode(Node.LastNode, Trigg[index].Flag2);
            }
            else
                Node.LastNode.Nodes.Add("Groups: " + (Trigg[index].Flag1 * (-1)).ToString() + " and " + (Trigg[index].Flag2 * (-1)).ToString());
        }
    }
}
