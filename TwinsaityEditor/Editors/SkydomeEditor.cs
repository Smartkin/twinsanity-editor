using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Twinsanity;

namespace TwinsaityEditor
{
    public partial class SkydomeEditor : Form
    {
        private SectionController controller;
        private Skydome pos;

        private FileController File { get; set; }
        private TwinsFile FileData { get => File.Data; }

        private bool ignore_value_change;

        public SkydomeEditor(SectionController c)
        {
            File = c.MainFile;
            controller = c;
            InitializeComponent();
            Text = $"Skydome Editor";
            PopulateList();
        }

        private void PopulateList()
        {
            listBox1.Items.Clear();
            foreach (Skydome i in controller.Data.Records)
            {
                listBox1.Items.Add($"ID {i.ID:X8}");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            ignore_value_change = true;

            this.SuspendDrawing();

            pos = (Skydome)controller.Data.Records[listBox1.SelectedIndex];
            File.SelectItem(pos);
            splitContainer1.Panel2.Enabled = true;

            numericUpDown5.Value = pos.ID;
            numericUpDown1.Value = pos.Unknown;

            var lines = new string[pos.MeshIDs.Length];
            for (int i = 0; i < pos.MeshIDs.Length; ++i)
            {
                lines[i] = $"{pos.MeshIDs[i]:X8}";
            }
            textBox1.Lines = lines;

            ignore_value_change = false;

            this.ResumeDrawing();
        }

        private void addToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            uint id = 0;
            if (controller.Data.RecordIDs.Count != 0)
            {
                uint maxid = controller.Data.RecordIDs.Select(p => p.Key).Max();
                id = maxid;
            }
            ++id;
            Skydome new_pos = new Skydome { ID = id, };
            controller.Data.AddItem(id, new_pos);
            ((MainForm)Tag).GenTreeNode(new_pos, controller);
            pos = new_pos;
            listBox1.Items.Add($"ID {pos.ID:X8}");
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
            PopulateList();
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

            Skydome old_pos = pos;

            uint id = 0;
            if (controller.Data.RecordIDs.Count != 0)
            {
                uint maxid = controller.Data.RecordIDs.Select(p => p.Key).Max();
                id = maxid;
            }
            ++id;
            Skydome new_pos = new Skydome { ID = id,
                Unknown = pos.Unknown,
                MeshIDs = new List<uint>(pos.MeshIDs).ToArray(),
            };
            controller.Data.AddItem(id, new_pos);
            ((MainForm)Tag).GenTreeNode(new_pos, controller);
            pos = new_pos;
            listBox1.Items.Add($"ID {pos.ID:X8}");
            controller.UpdateText();
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateText();
        }

        private void numericUpDown1_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.Unknown = (uint)numericUpDown1.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            List<uint> meshes = new List<uint>();
            for (int i = 0; i < textBox1.Lines.Length; ++i)
            {
                if (uint.TryParse(textBox1.Lines[i], System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out uint x))
                {
                    meshes.Add(x);
                }
            }
            pos.MeshIDs = meshes.ToArray();
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }
    }
}
