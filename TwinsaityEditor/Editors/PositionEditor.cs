using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public partial class PositionEditor : Form
    {
        private SectionController controller;
        private Position pos;

        private FileController TFController { get; set; }
        private TwinsFile File { get => TFController.Data; }

        private bool ignore_value_change;

        public PositionEditor(FileController file, SectionController c)
        {
            TFController = file;
            controller = c;
            InitializeComponent();
            Text = "Position Editor (Section " + c.Data.Parent.ID + ")";
            PopulateList();
            numericUpDown2.ValueChanged += numericUpDown1_ValueChanged;
            numericUpDown3.ValueChanged += numericUpDown1_ValueChanged;
            numericUpDown4.ValueChanged += numericUpDown1_ValueChanged;
            FormClosed += PositionEditor_FormClosed;
        }

        private void PositionEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            TFController.SelectItem(null);
        }

        private void PopulateList()
        {
            listBox1.Items.Clear();
            foreach (Position i in controller.Data.Records)
            {
                listBox1.Items.Add("ID " + i.ID);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            ignore_value_change = true;

            this.SuspendDrawing();

            pos = (Position)controller.Data.Records[listBox1.SelectedIndex];
            TFController.SelectItem(pos);
            splitContainer1.Panel2.Enabled = true;

            numericUpDown1.Value = (decimal)pos.Pos.X;
            numericUpDown2.Value = (decimal)pos.Pos.Y;
            numericUpDown3.Value = (decimal)pos.Pos.Z;
            numericUpDown4.Value = (decimal)pos.Pos.W;
            numericUpDown5.Value = pos.ID;

            ignore_value_change = false;

            this.ResumeDrawing();
        }

        private void numericUpDown1_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.Pos = new Pos((float)numericUpDown1.Value, (float)numericUpDown2.Value, (float)numericUpDown3.Value, (float)numericUpDown4.Value);
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateText();
        }

        private void addToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (controller.Data.RecordIDs.Count >= ushort.MaxValue) return;
            uint id;
            for (id = 0; id < uint.MaxValue; ++id)
            {
                if (!controller.Data.RecordIDs.ContainsKey(id))
                    break;
            }
            Position new_pos = new Position { ID = id };
            controller.Data.AddItem(id, new_pos);
            ((MainForm)Tag).GenTreeNode(new_pos, controller);
            pos = new_pos;
            listBox1.Items.Add("ID " + pos.ID);
            controller.UpdateText();
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateText();
        }

        private void removeToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var sel_i = listBox1.SelectedIndex;
            if (sel_i == -1)
                return;
            Controller.DisposeNode(controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]]);
            controller.Data.RemoveItem(pos.ID);
            listBox1.Items.RemoveAt(sel_i);
            if (sel_i >= listBox1.Items.Count) sel_i = listBox1.Items.Count - 1;
            listBox1.SelectedIndex = sel_i;
            if (listBox1.Items.Count == 0)
                splitContainer1.Panel2.Enabled = false;
            controller.UpdateText();
        }

        private void numericUpDown5_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            if (controller.Data.RecordIDs.ContainsKey((uint)numericUpDown5.Value))
            {
                MessageBox.Show("The specified ID already exists.");
                ignore_value_change = true;
                numericUpDown5.Value = pos.ID;
                ignore_value_change = false;
                return;
            }
            controller.Data.RecordIDs.Remove(pos.ID);
            pos.ID = (uint)numericUpDown5.Value;
            controller.Data.RecordIDs.Add(pos.ID, listBox1.SelectedIndex);
            listBox1.Items[listBox1.SelectedIndex] = "ID " + pos.ID;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateText();
        }
    }
}
