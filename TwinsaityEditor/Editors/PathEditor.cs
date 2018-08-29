using System.Windows.Forms;
using System;

namespace TwinsaityEditor
{
    public partial class PathEditor
    {
        private TwinsanityEditorForm twinsanityEditorForm;

        public PathEditor(TwinsanityEditorForm TEF)
        {
            twinsanityEditorForm = TEF;
            InitializeComponent();
        }

        private int IISIndex;
        private Twinsanity.Path Path;
        public void UpdateTree(ref Twinsanity.Paths PATHs, uint Index)
        {
            Pathes.BeginUpdate();
            for (int i = 0; i <= PATHs._Item.Length - 1; i++)
                Pathes.Nodes.Add("ID: " + PATHs._Item[i].ID.ToString());
            Pathes.EndUpdate();
            IISIndex = (int)Index;
        }

        public void UpdatePath(int Index)
        {
            Path = (Twinsanity.Path)twinsanityEditorForm.LevelData.Get_Item(TwinsanityEditorForm.CalculateIndexes(twinsanityEditorForm.TreeView1.Nodes[0].Nodes[IISIndex].Nodes[4].Nodes[Index]));
            Positinos.Items.Clear();
            for (int i = 0; i <= Path.Pos.Length - 1; i++)
                Positinos.Items.Add(i);
            Somethings.Items.Clear();
            for (int i = 0; i <= Path.SomeInts.Length - 1; i++)
                Somethings.Items.Add(i);
            if (Positinos.SelectedIndex == -1 & Path.Pos.Length > 0)
                Positinos.SelectedIndex = 0;
            if (Somethings.SelectedIndex == -1 & Path.SomeInts.Length > 0)
                Somethings.SelectedIndex = 0;
            if (Positinos.SelectedIndex > Path.Pos.Length - 1)
            {
                if (Path.Pos.Length > 0)
                    Positinos.SelectedIndex = 0;
                else
                    Positinos.SelectedIndex = -1;
            }
            if (Somethings.SelectedIndex > Path.SomeInts.Length - 1)
            {
                if (Path.SomeInts.Length > 0)
                    Somethings.SelectedIndex = 0;
                else
                    Somethings.SelectedIndex = -1;
            }
            this.Text = "ID: " + Path.ID.ToString();
            UpdatePositions(Index);
        }
        public void UpdatePositions(int index)
        {
            if (Positinos.SelectedIndex >= 0)
            {
                XPos.Text = Path.Pos[index].X.ToString();
                YPos.Text = Path.Pos[index].Y.ToString();
                ZPos.Text = Path.Pos[index].Z.ToString();
            }
            else
            {
                XPos.Text = "";
                YPos.Text = "";
                ZPos.Text = "";
            }
        }
        public void UpdateSomethings(int index)
        {
            if (Somethings.SelectedIndex >= 0)
            {
                Int1Val.Text = Path.SomeInts[index].Int1.ToString();
                Int2Val.Text = Path.SomeInts[index].Int2.ToString();
            }
            else
            {
                Int1Val.Text = "";
                Int2Val.Text = "";
            }
        }
        public void ApplyPosition(int index)
        {
            if (Positinos.SelectedIndex == -1)
                return;
            Path.Pos[index].X = float.Parse(XPos.Text);
            Path.Pos[index].Y = float.Parse(YPos.Text);
            Path.Pos[index].Z = float.Parse(ZPos.Text);
        }
        public void ApplySomethings(int index)
        {
            if (Positinos.SelectedIndex == -1)
                return;
            Path.SomeInts[index].Int1 = uint.Parse(Int1Val.Text);
            Path.SomeInts[index].Int2 = uint.Parse(Int2Val.Text);
        }
        public void ApplyPath(int Index)
        {
            twinsanityEditorForm.LevelData.Put_Item(Path, TwinsanityEditorForm.CalculateIndexes(twinsanityEditorForm.TreeView1.Nodes[0].Nodes[IISIndex].Nodes[4].Nodes[Index]));
        }

        private void Pathes_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdatePath(Pathes.SelectedNode.Index);
        }

        private void Positinos_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePositions(Positinos.SelectedIndex);
        }

        private void Somethings_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSomethings(Somethings.SelectedIndex);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            UpdatePath(Pathes.SelectedNode.Index);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            ApplyPath(Pathes.SelectedNode.Index);
        }

        private void Save_Click(object sender, EventArgs e)
        {
            ApplyPosition(Positinos.SelectedIndex);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            ApplySomethings(Somethings.SelectedIndex);
        }

        private void DelPosition_Click(object sender, EventArgs e)
        {
            if (Positinos.SelectedIndex >= 0)
            {
                for (int i = Positinos.SelectedIndex + 1; i <= Path.Pos.Length - 1; i++)
                    Path.Pos[i - 1] = Path.Pos[i];
                Array.Resize(ref Path.Pos, Path.Pos.Length - 1);
                Positinos.Items.RemoveAt(Positinos.SelectedIndex);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (Somethings.SelectedIndex >= 0)
            {
                for (int i = Somethings.SelectedIndex + 1; i <= Path.SomeInts.Length - 1; i++)
                    Path.SomeInts[i - 1] = Path.SomeInts[i];
                Array.Resize(ref Path.SomeInts, Path.SomeInts.Length - 1);
                Somethings.Items.RemoveAt(Somethings.SelectedIndex);
            }
        }

        private void AddPosition_Click(object sender, EventArgs e)
        {
            Array.Resize(ref Path.Pos, Path.Pos.Length + 1);
            Path.Pos[Path.Pos.Length - 1].X = float.Parse(XPos.Text);
            Path.Pos[Path.Pos.Length - 1].Y = float.Parse(YPos.Text);
            Path.Pos[Path.Pos.Length - 1].Z = float.Parse(ZPos.Text);
            Positinos.Items.Add(Positinos.Items.Count);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Array.Resize(ref Path.SomeInts, Path.SomeInts.Length + 1);
            Path.SomeInts[Path.SomeInts.Length - 1].Int1 = uint.Parse(Int1Val.Text);
            Path.SomeInts[Path.SomeInts.Length - 1].Int2 = uint.Parse(Int2Val.Text);
            Somethings.Items.Add(Somethings.Items.Count);
        }
    }
}
