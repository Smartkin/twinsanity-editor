using System.Windows.Forms;
using System;

namespace TwinsaityEditor
{
    public partial class PositionEditor
    {

        private TwinsanityEditorForm twinsanityEditorForm;

        public PositionEditor(TwinsanityEditorForm TEF)
        {
            twinsanityEditorForm = TEF;
            InitializeComponent();
        }

        private int IISIndex;
        private uint ItemId;
        public void UpdateTree(ref Twinsanity.Positions POSs, uint Index)
        {
            PosTree.BeginUpdate();
            for (int i = 0; i <= POSs._Item.Length - 1; i++)
                PosTree.Nodes.Add("ID: " + POSs._Item[i].ID.ToString());
            PosTree.EndUpdate();
            IISIndex = (int)Index;
        }

        public void UpdatePos(int Index)
        {
            Twinsanity.Position Pos = (Twinsanity.Position)twinsanityEditorForm.LevelData.Get_Item(TwinsanityEditorForm.CalculateIndexes(twinsanityEditorForm.TreeView1.Nodes[0].Nodes[IISIndex].Nodes[3].Nodes[Index]));
            XVal.Text = Pos.Pos.X.ToString();
            YVal.Text = Pos.Pos.Y.ToString();
            ZVal.Text = Pos.Pos.Z.ToString();
            ItemId = Pos.ID;
            this.Text = "ID: " + Pos.ID.ToString();
        }

        public void ApplyPos(int Index)
        {
            Twinsanity.Position Pos = new Twinsanity.Position();
            Pos.Pos.X = float.Parse(XVal.Text);
            Pos.Pos.Y = float.Parse(YVal.Text);
            Pos.Pos.Z = float.Parse(ZVal.Text);
            Pos.ID = ItemId;
            twinsanityEditorForm.LevelData.Put_Item(Pos, TwinsanityEditorForm.CalculateIndexes(twinsanityEditorForm.TreeView1.Nodes[0].Nodes[IISIndex].Nodes[3].Nodes[Index]));
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            ApplyPos(PosTree.SelectedNode.Index);
        }

        private void Revert_Click(object sender, EventArgs e)
        {
            UpdatePos(PosTree.SelectedNode.Index);
        }

        private void PosTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdatePos(PosTree.SelectedNode.Index);
        }
    }
}
