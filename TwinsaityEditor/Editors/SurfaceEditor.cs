using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public partial class SurfaceEditor : Form
    {
        private SectionController controller;
        private CollisionSurface pos;

        private FileController File { get; set; }
        private TwinsFile FileData { get => File.Data; }

        private bool ignore_value_change;

        public SurfaceEditor(SectionController c)
        {
            File = c.MainFile;
            controller = c;
            InitializeComponent();
            Text = $"Surface Editor (Section {c.Data.Parent.ID})";
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
            foreach (CollisionSurface i in controller.Data.Records)
            {
                listBox1.Items.Add($"ID {i.ID}");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            ignore_value_change = true;

            this.SuspendDrawing();

            pos = (CollisionSurface)controller.Data.Records[listBox1.SelectedIndex];
            File.SelectItem(pos);
            splitContainer1.Panel2.Enabled = true;

            numericUpDown5.Value = pos.ID;
            numericUpDown1.Value = pos.Sound_1;
            numericUpDown2.Value = pos.Sound_2;
            numericUpDown3.Value = pos.Sound_3;
            numericUpDown4.Value = pos.Sound_4;
            numericUpDown6.Value = pos.Sound_5;
            numericUpDown7.Value = pos.Sound_6;
            numericUpDown8.Value = pos.Particle_1;
            numericUpDown9.Value = pos.Particle_2;
            numericUpDown10.Value = pos.Particle_3;
            numericUpDown11.Value = (decimal)pos.UnkFloat1;
            numericUpDown12.Value = (decimal)pos.UnkFloat2;
            numericUpDown13.Value = (decimal)pos.UnkFloat3;
            numericUpDown14.Value = (decimal)pos.UnkFloat4;
            numericUpDown15.Value = (decimal)pos.UnkFloat5;
            numericUpDown16.Value = (decimal)pos.UnkFloat6;
            numericUpDown17.Value = (decimal)pos.UnkFloat7;
            numericUpDown18.Value = (decimal)pos.UnkFloat8;
            numericUpDown19.Value = (decimal)pos.UnkFloat9;
            numericUpDown20.Value = (decimal)pos.UnkFloat10;
            numericUpDown21.Value = (decimal)pos.UnkVector.X;
            numericUpDown22.Value = (decimal)pos.UnkVector.Y;
            numericUpDown23.Value = (decimal)pos.UnkVector.Z;
            numericUpDown24.Value = (decimal)pos.UnkVector.W;
            numericUpDown25.Value = (decimal)pos.UnkBoundingBox1.X;
            numericUpDown26.Value = (decimal)pos.UnkBoundingBox1.Y;
            numericUpDown27.Value = (decimal)pos.UnkBoundingBox1.Z;
            numericUpDown28.Value = (decimal)pos.UnkBoundingBox1.W;
            numericUpDown29.Value = (decimal)pos.UnkBoundingBox2.X;
            numericUpDown30.Value = (decimal)pos.UnkBoundingBox2.Y;
            numericUpDown31.Value = (decimal)pos.UnkBoundingBox2.Z;
            numericUpDown32.Value = (decimal)pos.UnkBoundingBox2.W;
            numericUpDown33.Value = pos.SurfaceID;
            checkBox1.Checked = (pos.Flags & (1 << 0)) != 0;
            checkBox2.Checked = (pos.Flags & (1 << 1)) != 0;
            checkBox3.Checked = (pos.Flags & (1 << 2)) != 0;
            checkBox4.Checked = (pos.Flags & (1 << 3)) != 0;
            checkBox5.Checked = (pos.Flags & (1 << 4)) != 0;
            checkBox6.Checked = (pos.Flags & (1 << 5)) != 0;
            checkBox7.Checked = (pos.Flags & (1 << 6)) != 0;
            checkBox8.Checked = (pos.Flags & (1 << 7)) != 0;
            checkBox9.Checked = (pos.Flags & (1 << 8)) != 0;
            checkBox10.Checked = (pos.Flags & (1 << 9)) != 0;
            checkBox11.Checked = (pos.Flags & (1 << 10)) != 0;
            checkBox12.Checked = (pos.Flags & (1 << 11)) != 0;
            checkBox13.Checked = (pos.Flags & (1 << 12)) != 0;
            checkBox14.Checked = (pos.Flags & (1 << 13)) != 0;
            checkBox15.Checked = (pos.Flags & (1 << 14)) != 0;
            checkBox16.Checked = (pos.Flags & (1 << 15)) != 0;
            checkBox17.Checked = (pos.Flags & (1 << 16)) != 0;
            checkBox18.Checked = (pos.Flags & (1 << 17)) != 0;
            checkBox19.Checked = (pos.Flags & (1 << 18)) != 0;
            checkBox20.Checked = (pos.Flags & (1 << 19)) != 0;
            checkBox21.Checked = (pos.Flags & (1 << 20)) != 0;
            checkBox22.Checked = (pos.Flags & (1 << 21)) != 0;
            checkBox23.Checked = (pos.Flags & (1 << 22)) != 0;
            checkBox24.Checked = (pos.Flags & (1 << 23)) != 0;
            checkBox25.Checked = (pos.Flags & (1 << 24)) != 0;
            checkBox26.Checked = (pos.Flags & (1 << 25)) != 0;
            checkBox27.Checked = (pos.Flags & (1 << 26)) != 0;
            checkBox28.Checked = (pos.Flags & (1 << 27)) != 0;
            checkBox29.Checked = (pos.Flags & (1 << 28)) != 0;
            checkBox30.Checked = (pos.Flags & (1 << 29)) != 0;
            checkBox31.Checked = (pos.Flags & (1 << 30)) != 0;
            checkBox32.Checked = (pos.Flags & (1 << 31)) != 0;

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
            CollisionSurface new_pos = new CollisionSurface { ID = id };
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
                CollisionSurface new_pos = (CollisionSurface)controller.Data.Records[i];
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

            CollisionSurface old_pos = pos;

            if (controller.Data.RecordIDs.Count >= ushort.MaxValue) return;
            uint id;
            for (id = 0; id < uint.MaxValue; ++id)
            {
                if (!controller.Data.ContainsItem(id))
                    break;
            }
            CollisionSurface new_pos = new CollisionSurface { ID = id,
                Flags = pos.Flags,
                SurfaceID = pos.SurfaceID,
                Sound_1 = pos.Sound_1,
                Sound_2 = pos.Sound_2,
                Sound_3 = pos.Sound_3,
                Sound_4 = pos.Sound_4,
                Sound_5 = pos.Sound_5,
                Sound_6 = pos.Sound_6,
                Particle_1 = pos.Particle_1,
                Particle_2 = pos.Particle_2,
                Particle_3 = pos.Particle_3,
                UnkFloat1 = pos.UnkFloat1,
                UnkFloat2 = pos.UnkFloat2,
                UnkFloat3 = pos.UnkFloat3,
                UnkFloat4 = pos.UnkFloat4,
                UnkFloat5 = pos.UnkFloat5,
                UnkFloat6 = pos.UnkFloat6,
                UnkFloat7 = pos.UnkFloat7,
                UnkFloat8 = pos.UnkFloat8,
                UnkFloat9 = pos.UnkFloat9,
                UnkFloat10 = pos.UnkFloat10,
                UnkVector = new Pos(pos.UnkVector.X, pos.UnkVector.Y, pos.UnkVector.Z, pos.UnkVector.W),
                UnkBoundingBox1 = new Pos(pos.UnkBoundingBox1.X, pos.UnkBoundingBox1.Y, pos.UnkBoundingBox1.Z, pos.UnkBoundingBox1.W),
                UnkBoundingBox2 = new Pos(pos.UnkBoundingBox2.X, pos.UnkBoundingBox2.Y, pos.UnkBoundingBox2.Z, pos.UnkBoundingBox2.W),
            };
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
            pos.Sound_1 = (ushort)numericUpDown1.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown2_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.Sound_2 = (ushort)numericUpDown2.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown3_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.Sound_3 = (ushort)numericUpDown3.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown4_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.Sound_4 = (ushort)numericUpDown4.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown6_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.Sound_5 = (ushort)numericUpDown6.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown7_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.Sound_6 = (ushort)numericUpDown7.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown8_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.Particle_1 = (ushort)numericUpDown8.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown9_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.Particle_2 = (ushort)numericUpDown9.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown10_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.Particle_3 = (ushort)numericUpDown10.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown11_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.UnkFloat1 = (float)numericUpDown11.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown12_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.UnkFloat2 = (float)numericUpDown12.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown13_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.UnkFloat3 = (float)numericUpDown13.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown14_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.UnkFloat4 = (float)numericUpDown14.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown15_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.UnkFloat5 = (float)numericUpDown15.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown16_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.UnkFloat6 = (float)numericUpDown16.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown17_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.UnkFloat7 = (float)numericUpDown17.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown18_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.UnkFloat8 = (float)numericUpDown18.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown19_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.UnkFloat9 = (float)numericUpDown19.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown20_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.UnkFloat10 = (float)numericUpDown20.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown21_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.UnkVector.X = (float)numericUpDown21.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown22_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.UnkVector.Y = (float)numericUpDown22.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown23_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.UnkVector.Z = (float)numericUpDown23.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown24_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.UnkVector.W = (float)numericUpDown24.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown25_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.UnkBoundingBox1.X = (float)numericUpDown25.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown26_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.UnkBoundingBox1.Y = (float)numericUpDown26.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown27_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.UnkBoundingBox1.Z = (float)numericUpDown27.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown28_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.UnkBoundingBox1.W = (float)numericUpDown28.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown29_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.UnkBoundingBox2.X = (float)numericUpDown29.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown30_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.UnkBoundingBox2.Y = (float)numericUpDown30.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown31_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.UnkBoundingBox2.Z = (float)numericUpDown31.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown32_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.UnkBoundingBox2.W = (float)numericUpDown32.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox1_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 0;
            if (checkBox1.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox2_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 1;
            if (checkBox2.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox3_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 2;
            if (checkBox3.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox4_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 3;
            if (checkBox4.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox5_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 4;
            if (checkBox5.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox6_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 5;
            if (checkBox6.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox7_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 6;
            if (checkBox7.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox8_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 7;
            if (checkBox8.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox9_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 8;
            if (checkBox9.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox10_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 9;
            if (checkBox10.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox11_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 10;
            if (checkBox11.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox12_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 11;
            if (checkBox12.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox13_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 12;
            if (checkBox13.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox14_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 13;
            if (checkBox14.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox15_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 14;
            if (checkBox15.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox16_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 15;
            if (checkBox16.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox17_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 16;
            if (checkBox17.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox18_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 17;
            if (checkBox18.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox19_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 18;
            if (checkBox19.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox20_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 19;
            if (checkBox20.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox21_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 20;
            if (checkBox21.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox22_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 21;
            if (checkBox22.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox23_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 22;
            if (checkBox23.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox24_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 23;
            if (checkBox24.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox25_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 24;
            if (checkBox25.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox26_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 25;
            if (checkBox26.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox27_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 26;
            if (checkBox27.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox28_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 27;
            if (checkBox28.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox29_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 28;
            if (checkBox29.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox30_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 29;
            if (checkBox30.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox31_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            uint mask = 1 << 30;
            if (checkBox31.Checked)
                pos.Flags |= mask;
            else
                pos.Flags &= ~mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void checkBox32_CheckedChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            int mask = 1 << 31;
            if (checkBox32.Checked)
                pos.Flags |= (uint)mask;
            else
                pos.Flags &= ~(uint)mask;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown33_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            pos.SurfaceID = (ushort)numericUpDown33.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[pos.ID]].Tag).UpdateTextBox();
        }
    }
}
