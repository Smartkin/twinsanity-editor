using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public partial class AIPathEditor : Form
    {
        private SectionController controller;
        private AIPath pos;

        private FileController File { get; set; }
        private TwinsFile FileData { get => File.Data; }

        private bool ignore_value_change;

        public AIPathEditor(SectionController c)
        {
            File = c.MainFile;
            controller = c;
            InitializeComponent();
            Text = $"AI Path Editor (Section {c.Data.Parent.ID})";
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
            foreach (AIPath i in controller.Data.Records)
            {
                listBox1.Items.Add($"ID {i.ID}");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            ignore_value_change = true;

            this.SuspendDrawing();

            pos = (AIPath)controller.Data.Records[listBox1.SelectedIndex];
            File.SelectItem(pos);
            splitContainer1.Panel2.Enabled = true;

            numericUpDown1.Value = pos.Arg[0];
            numericUpDown2.Value = pos.Arg[1];
            numericUpDown3.Value = pos.Arg[2];
            numericUpDown4.Value = pos.Arg[3];
            numericUpDown6.Value = pos.Arg[4];
            numericUpDown5.Value = pos.ID;

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
            AIPath new_pos = new AIPath { ID = id, Arg = new ushort[5] { 0,1,0,0,0 } };
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
                AIPath new_pos = (AIPath)controller.Data.Records[i];
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

        private void duplicateToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var sel_i = listBox1.SelectedIndex;
            if (sel_i == -1)
                return;

            AIPath old_pos = pos;

            if (controller.Data.RecordIDs.Count >= ushort.MaxValue) return;
            uint id;
            for (id = 0; id < uint.MaxValue; ++id)
            {
                if (!controller.Data.ContainsItem(id))
                    break;
            }
            AIPath new_pos = new AIPath { ID = id, Arg = new ushort[5] { pos.Arg[0], pos.Arg[1], pos.Arg[2], pos.Arg[3], pos.Arg[4] } };
            controller.Data.AddItem(id, new_pos);
            ((MainForm)Tag).GenTreeNode(new_pos, controller);
            pos = new_pos;
            listBox1.Items.Add($"ID {pos.ID}");
            controller.UpdateText();
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateText();
        }

        private void numericUpDown1_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.Arg[0] = (ushort)numericUpDown1.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown2_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.Arg[1] = (ushort)numericUpDown2.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown3_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.Arg[2] = (ushort)numericUpDown3.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown4_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.Arg[3] = (ushort)numericUpDown4.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown6_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.Arg[4] = (ushort)numericUpDown6.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }
    }
}
