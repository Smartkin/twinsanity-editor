using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Twinsanity;

namespace TwinsaityEditor
{
    public partial class LodEditor : Form
    {
        private SectionController controller;
        private LodModel pos;

        private FileController File { get; set; }
        private TwinsFile FileData { get => File.Data; }

        private bool ignore_value_change;

        public LodEditor(SectionController c)
        {
            File = c.MainFile;
            controller = c;
            InitializeComponent();
            Text = $"LOD Editor";
            PopulateList();
        }

        private void PopulateList()
        {
            listBox1.Items.Clear();
            foreach (LodModel i in controller.Data.Records)
            {
                listBox1.Items.Add($"ID {i.ID:X8}");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            ignore_value_change = true;

            this.SuspendDrawing();

            pos = (LodModel)controller.Data.Records[listBox1.SelectedIndex];
            File.SelectItem(pos);
            splitContainer1.Panel2.Enabled = true;

            numericUpDown5.Value = pos.ID;
            numericUpDown1.Value = pos.Header;
            numericUpDown2.Value = pos.Zero;
            numericUpDown3.Value = pos.ModelsAmount;
            numericUpDown4.Value = pos.LODDistance[0];
            numericUpDown6.Value = pos.LODDistance[1];
            numericUpDown7.Value = pos.LODDistance[2];
            numericUpDown8.Value = pos.LODDistance[3];
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            numericUpDown4.Enabled = false;
            numericUpDown6.Enabled = false;
            numericUpDown7.Enabled = false;
            numericUpDown8.Enabled = false;
            textBox1.Text = $"";
            textBox2.Text = $"";
            textBox3.Text = $"";
            textBox4.Text = $"";
            if (pos.ModelsAmount > 0)
            {
                textBox1.Text = $"{pos.LODModelIDs[0]:X8}";
                textBox1.Enabled = true;
                numericUpDown4.Enabled = true;
            }
            if (pos.ModelsAmount > 1)
            {
                textBox2.Text = $"{pos.LODModelIDs[1]:X8}";
                textBox2.Enabled = true;
                numericUpDown6.Enabled = true;
            }
            if (pos.ModelsAmount > 2)
            {
                textBox3.Text = $"{pos.LODModelIDs[2]:X8}";
                textBox3.Enabled = true;
                numericUpDown7.Enabled = true;
            }
            if (pos.ModelsAmount > 3)
            {
                textBox4.Text = $"{pos.LODModelIDs[3]:X8}";
                textBox4.Enabled = true;
                numericUpDown8.Enabled = true;
            }

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
            LodModel new_pos = new LodModel { ID = id, };
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

            LodModel old_pos = pos;

            uint id = 0;
            if (controller.Data.RecordIDs.Count != 0)
            {
                uint maxid = controller.Data.RecordIDs.Select(p => p.Key).Max();
                id = maxid;
            }
            ++id;
            LodModel new_pos = new LodModel { ID = id,
                Header = pos.Header,
                Zero = pos.Zero,
                ModelsAmount = pos.ModelsAmount,
                LODDistance = new uint[4] {pos.LODDistance[0], pos.LODDistance[1], pos.LODDistance[2], pos.LODDistance[3] },
                LODModelIDs = new List<uint>(pos.LODModelIDs).ToArray(),
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
            pos.Header = (uint)numericUpDown1.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown2_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.Zero = (uint)numericUpDown2.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown3_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint target = (uint)numericUpDown3.Value;
            if (target == pos.ModelsAmount) return;
            uint[] list = new uint[target];
            for (int i = 0; i < target && i < pos.LODModelIDs.Length; i++)
            {
                list[i] = pos.LODModelIDs[i];
            }
            pos.LODModelIDs = list;
            pos.ModelsAmount = target;

            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            numericUpDown4.Enabled = false;
            numericUpDown6.Enabled = false;
            numericUpDown7.Enabled = false;
            numericUpDown8.Enabled = false;
            textBox1.Text = $"";
            textBox2.Text = $"";
            textBox3.Text = $"";
            textBox4.Text = $"";
            if (pos.ModelsAmount > 0)
            {
                textBox1.Text = $"{pos.LODModelIDs[0]:X8}";
                textBox1.Enabled = true;
                numericUpDown4.Enabled = true;
            }
            if (pos.ModelsAmount > 1)
            {
                textBox2.Text = $"{pos.LODModelIDs[1]:X8}";
                textBox2.Enabled = true;
                numericUpDown6.Enabled = true;
            }
            if (pos.ModelsAmount > 2)
            {
                textBox3.Text = $"{pos.LODModelIDs[2]:X8}";
                textBox3.Enabled = true;
                numericUpDown7.Enabled = true;
            }
            if (pos.ModelsAmount > 3)
            {
                textBox4.Text = $"{pos.LODModelIDs[3]:X8}";
                textBox4.Enabled = true;
                numericUpDown8.Enabled = true;
            }
            ignore_value_change = false;

            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown4_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.LODDistance[0] = (uint)numericUpDown4.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown6_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.LODDistance[1] = (uint)numericUpDown6.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown7_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.LODDistance[2] = (uint)numericUpDown7.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown8_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.LODDistance[3] = (uint)numericUpDown8.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            if (uint.TryParse(textBox1.Text, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out uint x))
            {
                pos.LODModelIDs[0] = x;
                ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            if (uint.TryParse(textBox2.Text, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out uint x))
            {
                pos.LODModelIDs[1] = x;
                ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            if (uint.TryParse(textBox3.Text, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out uint x))
            {
                pos.LODModelIDs[2] = x;
                ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            if (uint.TryParse(textBox4.Text, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out uint x))
            {
                pos.LODModelIDs[3] = x;
                ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
            }
        }
    }
}
