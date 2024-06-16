using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public partial class AIPositionEditor : Form
    {
        private SectionController controller;
        private AIPosition pos;

        private FileController File { get; set; }
        private TwinsFile FileData { get => File.Data; }

        private bool ignore_value_change;

        public AIPositionEditor(SectionController c)
        {
            File = c.MainFile;
            controller = c;
            InitializeComponent();
            Text = $"AI Position Editor (Section {c.Data.Parent.ID})";
            PopulateList();
            FormClosed += PositionEditor_FormClosed;
        }

        private void PositionEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            File.SelectItem(null);
        }

        private void PopulateList()
        {
            listBox1.Items.Clear();
            foreach (AIPosition i in controller.Data.Records)
            {
                listBox1.Items.Add($"ID {i.ID}");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            ignore_value_change = true;

            this.SuspendDrawing();

            pos = (AIPosition)controller.Data.Records[listBox1.SelectedIndex];
            File.SelectItem(pos);
            splitContainer1.Panel2.Enabled = true;

            numericUpDown1.Value = (decimal)pos.Pos.X;
            numericUpDown2.Value = (decimal)pos.Pos.Y;
            numericUpDown3.Value = (decimal)pos.Pos.Z;
            numericUpDown4.Value = (decimal)pos.Pos.W;
            numericUpDown5.Value = pos.ID;
            numericUpDownType.Value = pos.Num;
            switch (pos.Num)
            {
                default: comboBoxType.SelectedIndex = 0; break;
                case (ushort)AIPosition.NodeType.Air: comboBoxType.SelectedIndex = 1; break;
                case (ushort)AIPosition.NodeType.WormPath: comboBoxType.SelectedIndex = 2; break;
                case (ushort)AIPosition.NodeType.CortexPoint: comboBoxType.SelectedIndex = 3; break;
            }

            ignore_value_change = false;

            this.ResumeDrawing();
        }

        private void addToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (controller.Data.RecordIDs.Count >= ushort.MaxValue) return;
            uint id;
            for (id = 0; id < uint.MaxValue; ++id)
            {
                if (!controller.Data.ContainsItem(id))
                    break;
            }
            AIPosition new_pos = new AIPosition { ID = id, Pos = new Pos(0, 0, 0, 1) };
            controller.Data.AddItem(id, new_pos);
            ((MainForm)Tag).GenTreeNode(new_pos, controller);
            pos = new_pos;
            listBox1.Items.Add($"ID {pos.ID}");
            controller.UpdateText();
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateText();
        }

        private void removeToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var sel_i = listBox1.SelectedIndex;
            if (sel_i == -1)
                return;
            controller.RemoveItem(pos.ID);
            listBox1.BeginUpdate();
            listBox1.Items.RemoveAt(sel_i);
            for (int i = 0; i < controller.Data.Records.Count; ++i)
            {
                AIPosition new_pos = (AIPosition)controller.Data.Records[i];
                if (new_pos.ID != i)
                {
                    controller.ChangeID(new_pos.ID, (uint)i);
                    listBox1.Items[i] = $"ID {i}";
                    ((Controller)controller.Node.Nodes[i].Tag).UpdateText();
                }
            }
            if (sel_i >= listBox1.Items.Count) sel_i = listBox1.Items.Count - 1;
            listBox1.SelectedIndex = sel_i;
            listBox1.EndUpdate();
            if (listBox1.Items.Count == 0)
                splitContainer1.Panel2.Enabled = false;
            controller.UpdateText();
        }

        private void numericUpDown1_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.Pos.X = (float)numericUpDown1.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown2_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.Pos.Y = (float)numericUpDown2.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown3_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.Pos.Z = (float)numericUpDown3.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown4_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.Pos.W = (float)numericUpDown4.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void button_CopyViewerPos_Click(object sender, System.EventArgs e)
        {
            Pos currentPos = File.RMViewer_GetPos(pos.Pos);
            pos.Pos.X = currentPos.X;
            pos.Pos.Y = currentPos.Y;
            pos.Pos.Z = currentPos.Z;
            numericUpDown1.Value = (decimal)pos.Pos.X;
            numericUpDown2.Value = (decimal)pos.Pos.Y;
            numericUpDown3.Value = (decimal)pos.Pos.Z;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
            File.RMViewer_LoadPositions();
        }

        private void duplicateToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var sel_i = listBox1.SelectedIndex;
            if (sel_i == -1)
                return;

            AIPosition old_pos = pos;

            if (controller.Data.RecordIDs.Count >= ushort.MaxValue) return;
            uint id;
            for (id = 0; id < uint.MaxValue; ++id)
            {
                if (!controller.Data.ContainsItem(id))
                    break;
            }
            AIPosition new_pos = new AIPosition { ID = id, Pos = new Pos(old_pos.Pos.X, old_pos.Pos.Y + 1f, old_pos.Pos.Z, old_pos.Pos.W), Num = old_pos.Num };
            controller.Data.AddItem(id, new_pos);
            ((MainForm)Tag).GenTreeNode(new_pos, controller);
            pos = new_pos;
            listBox1.Items.Add($"ID {pos.ID}");
            controller.UpdateText();
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateText();
        }

        private void numericUpDownType_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            pos.Num = (ushort)numericUpDownType.Value;
            switch (pos.Num)
            {
                default: comboBoxType.SelectedIndex = 0; break;
                case (ushort)AIPosition.NodeType.Air: comboBoxType.SelectedIndex = 1; break;
                case (ushort)AIPosition.NodeType.WormPath: comboBoxType.SelectedIndex = 2; break;
                case (ushort)AIPosition.NodeType.CortexPoint: comboBoxType.SelectedIndex = 3; break;
            }
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
            ignore_value_change = false;
        }

        private void comboBoxType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            switch (comboBoxType.SelectedIndex)
            {
                default: pos.Num = (ushort)AIPosition.NodeType.Ground; break;
                case 1: pos.Num = (ushort)AIPosition.NodeType.Air; break;
                case 2: pos.Num = (ushort)AIPosition.NodeType.WormPath; break;
                case 3: pos.Num = (ushort)AIPosition.NodeType.CortexPoint; break;
            }
            numericUpDownType.Value = pos.Num;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
            ignore_value_change = false;
        }
    }
}
