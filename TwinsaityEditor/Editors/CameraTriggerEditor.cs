using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Twinsanity;

namespace TwinsaityEditor
{
    public partial class CameraTriggerEditor : Form
    {
        private SectionController controller;
        private Camera trigger;

        private FileController File { get; set; }
        private TwinsFile FileData { get => File.Data; }
        private Controller CurCont { get => (Controller)controller.Node.Nodes[controller.Data.RecordIDs[trigger.ID]].Tag; }

        private bool ignore_value_change;

        public CameraTriggerEditor(SectionController c)
        {
            File = c.MainFile;
            controller = c;
            InitializeComponent();
            Text = $"Camera Trigger Editor (Section {c.Data.Parent.ID})";
            PopulateList();
            FormClosed += TriggerEditor_FormClosed;
        }

        private void TriggerEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            File.SelectItem(null);
        }

        private void PopulateList()
        {
            listBox1.Items.Clear();
            foreach (Camera i in controller.Data.Records)
            {
                listBox1.Items.Add($"ID {i.ID}");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;

            this.SuspendDrawing();

            trigger = (Camera)controller.Data.Records[listBox1.SelectedIndex];
            File.SelectItem(trigger);
            splitContainer1.Panel2.Enabled = true;

            ignore_value_change = true;

            numericUpDown1.Value = trigger.Header;
            numericUpDown2.Value = trigger.Enabled;
            numericUpDown3.Value = (decimal)trigger.SomeFloat;
            numericUpDown4.Value = trigger.SectionHead;
            numericUpDown5.Value = trigger.ID;
            numericUpDown10.Value = (decimal)trigger.Coords[1].X;
            numericUpDown11.Value = (decimal)trigger.Coords[1].Y;
            numericUpDown12.Value = (decimal)trigger.Coords[1].Z;
            numericUpDown13.Value = (decimal)trigger.Coords[1].W;
            numericUpDown14.Value = (decimal)trigger.Coords[2].X;
            numericUpDown15.Value = (decimal)trigger.Coords[2].Y;
            numericUpDown16.Value = (decimal)trigger.Coords[2].Z;
            numericUpDown17.Value = (decimal)trigger.Coords[2].W;
            double angle = Math.Acos(trigger.Coords[0].W);
            double angle_modify = Math.Sin(angle);
            if (angle_modify == 0)
            {
                numericUpDown18.Value = 0;
                numericUpDown19.Value = 0;
                numericUpDown20.Value = 0;
                numericUpDown21.Value = 0;
            }
            else
            {
                numericUpDown18.Value = (decimal)(trigger.Coords[0].X / angle_modify);
                numericUpDown19.Value = (decimal)(trigger.Coords[0].Y / angle_modify);
                numericUpDown20.Value = (decimal)(trigger.Coords[0].Z / angle_modify);
                numericUpDown21.Value = (decimal)angle * 2;
            }

            bool[] TrigMask = trigger.Mask;

            checkBox5.Checked = TrigMask[0];
            checkBox6.Checked = TrigMask[1];
            checkBox7.Checked = TrigMask[2];
            checkBox8.Checked = TrigMask[3];
            checkBox9.Checked = TrigMask[4];
            checkBox10.Checked = TrigMask[5];
            checkBox11.Checked = TrigMask[6];
            checkBox12.Checked = TrigMask[7];
            checkBox13.Checked = TrigMask[8];

            var lines = new string[trigger.Instances.Count];
            for (int i = 0; i < trigger.Instances.Count; ++i)
            {
                lines[i] = trigger.Instances[i].ToString();
            }
            textBox1.Lines = lines;

            numericUpDowncamFlags.Value = trigger.CamHeader;
            numericUpDowncamByte.Value = trigger.UnkByte;
            numericUpDowncamShort.Value = trigger.UnkShort;
            numericUpDowncamFloat.Value = (decimal)trigger.UnkFloat1;

            checkBoxFlag0.Checked = (trigger.CamHeader & (1 << 0)) != 0;
            checkBoxFlag1.Checked = (trigger.CamHeader & (1 << 1)) != 0;
            checkBoxFlag2.Checked = (trigger.CamHeader & (1 << 2)) != 0;
            checkBoxFlag3.Checked = (trigger.CamHeader & (1 << 3)) != 0;
            checkBoxFlag4.Checked = (trigger.CamHeader & (1 << 4)) != 0;
            checkBoxFlag5.Checked = (trigger.CamHeader & (1 << 5)) != 0;
            checkBoxFlag6.Checked = (trigger.CamHeader & (1 << 6)) != 0;
            checkBoxFlag7.Checked = (trigger.CamHeader & (1 << 7)) != 0;
            checkBoxFlag8.Checked = (trigger.CamHeader & (1 << 8)) != 0;
            checkBoxFlag9.Checked = (trigger.CamHeader & (1 << 9)) != 0;
            checkBoxFlag10.Checked = (trigger.CamHeader & (1 << 10)) != 0;
            checkBoxFlag11.Checked = (trigger.CamHeader & (1 << 11)) != 0;
            checkBoxFlag12.Checked = (trigger.CamHeader & (1 << 12)) != 0;
            checkBoxFlag13.Checked = (trigger.CamHeader & (1 << 13)) != 0;
            checkBoxFlag14.Checked = (trigger.CamHeader & (1 << 14)) != 0;
            checkBoxFlag15.Checked = (trigger.CamHeader & (1 << 15)) != 0;
            checkBoxFlag16.Checked = (trigger.CamHeader & (1 << 16)) != 0;
            checkBoxFlag17.Checked = (trigger.CamHeader & (1 << 17)) != 0;
            checkBoxFlag18.Checked = (trigger.CamHeader & (1 << 18)) != 0;
            checkBoxFlag19.Checked = (trigger.CamHeader & (1 << 19)) != 0;
            checkBoxFlag20.Checked = (trigger.CamHeader & (1 << 20)) != 0;
            checkBoxFlag21.Checked = (trigger.CamHeader & (1 << 21)) != 0;
            checkBoxFlag22.Checked = (trigger.CamHeader & (1 << 22)) != 0;
            checkBoxFlag23.Checked = (trigger.CamHeader & (1 << 23)) != 0;
            checkBoxFlag24.Checked = (trigger.CamHeader & (1 << 24)) != 0;
            checkBoxFlag25.Checked = (trigger.CamHeader & (1 << 25)) != 0;
            checkBoxFlag26.Checked = (trigger.CamHeader & (1 << 26)) != 0;
            checkBoxFlag27.Checked = (trigger.CamHeader & (1 << 27)) != 0;
            checkBoxFlag28.Checked = (trigger.CamHeader & (1 << 28)) != 0;
            checkBoxFlag29.Checked = (trigger.CamHeader & (1 << 29)) != 0;
            checkBoxFlag30.Checked = (trigger.CamHeader & (1 << 30)) != 0;
            checkBoxFlag31.Checked = (trigger.CamHeader & (1 << 31)) != 0;

            checkBoxTFlag0.Checked = trigger.UnkFlag0;
            checkBoxTFlag1.Checked = trigger.UnkFlag1;
            checkBoxTFlag2.Checked = trigger.UnkFlag2;
            checkBoxTFlag3.Checked = trigger.UnkFlag3;
            checkBoxTFlag4.Checked = trigger.UnkFlag4;
            checkBoxTFlag5.Checked = trigger.UnkFlag5;
            checkBoxTFlag6.Checked = trigger.UnkFlag6;
            checkBoxTFlag18.Checked = trigger.UnkFlag18;
            checkBoxTFlag20.Checked = trigger.UnkFlag20;

            numericUpDownUInt3.Value = trigger.UnkUInt3;
            numericUpDownUInt4.Value = trigger.UnkUInt4;
            numericUpDownFloat4.Value = (decimal)trigger.UnkFloat4;
            numericUpDownFloat5.Value = (decimal)trigger.UnkFloat5;
            numericUpDownInt5.Value = trigger.UnkInt5;
            numericUpDownInt6.Value = trigger.UnkInt6;
            numericUpDownUInt1.Value = trigger.UnkUInt1;
            numericUpDownUInt2.Value = trigger.UnkUInt2;
            numericUpDownUnkCoord1X.Value = (decimal)trigger.UnkCoords1.X;
            numericUpDownUnkCoord1Y.Value = (decimal)trigger.UnkCoords1.Y;
            numericUpDownUnkCoord1Z.Value = (decimal)trigger.UnkCoords1.Z;
            numericUpDownUnkCoord1W.Value = (decimal)trigger.UnkCoords1.W;
            numericUpDownUnkCoord2X.Value = (decimal)trigger.UnkCoords2.X;
            numericUpDownUnkCoord2Y.Value = (decimal)trigger.UnkCoords2.Y;
            numericUpDownUnkCoord2Z.Value = (decimal)trigger.UnkCoords2.Z;
            numericUpDownUnkCoord2W.Value = (decimal)trigger.UnkCoords2.W;
            numericUpDownFloat2.Value = (decimal)trigger.UnkFloat2;
            numericUpDownFloat3.Value = (decimal)trigger.UnkFloat3;
            numericUpDownFloat6.Value = (decimal)trigger.UnkFloat6;
            numericUpDownFloat7.Value = (decimal)trigger.UnkFloat7;
            numericUpDownUInt7.Value = trigger.UnkUInt7;
            numericUpDownInt8.Value = trigger.UnkInt8;
            numericUpDownUInt9.Value = trigger.UnkUInt9;
            numericUpDownFloat8.Value = (decimal)trigger.UnkFloat8;
            numericUpDownUInt3.Enabled = checkBoxFlag2.Checked;
            numericUpDownUInt4.Enabled = checkBoxFlag2.Checked;
            numericUpDownFloat4.Enabled = checkBoxFlag3.Checked;
            numericUpDownFloat5.Enabled = checkBoxFlag3.Checked;
            numericUpDownInt5.Enabled = checkBoxFlag6.Checked;
            numericUpDownInt6.Enabled = checkBoxFlag6.Checked;
            numericUpDownUInt1.Enabled = checkBoxFlag7.Checked;
            numericUpDownUInt2.Enabled = checkBoxFlag7.Checked;
            numericUpDownUnkCoord1X.Enabled = checkBoxFlag8.Checked || checkBoxFlag28.Checked;
            numericUpDownUnkCoord1Y.Enabled = checkBoxFlag8.Checked || checkBoxFlag28.Checked;
            numericUpDownUnkCoord1Z.Enabled = checkBoxFlag8.Checked || checkBoxFlag28.Checked;
            numericUpDownUnkCoord1W.Enabled = checkBoxFlag28.Checked;
            numericUpDownUnkCoord2X.Enabled = checkBoxFlag8.Checked || checkBoxFlag28.Checked;
            numericUpDownUnkCoord2Y.Enabled = checkBoxFlag8.Checked || checkBoxFlag28.Checked;
            numericUpDownUnkCoord2Z.Enabled = checkBoxFlag8.Checked || checkBoxFlag28.Checked;
            numericUpDownUnkCoord2W.Enabled = checkBoxFlag28.Checked;
            numericUpDownFloat2.Enabled = checkBoxFlag9.Checked || checkBoxFlag10.Checked;
            numericUpDownFloat3.Enabled = checkBoxFlag9.Checked || checkBoxFlag10.Checked;
            numericUpDownFloat6.Enabled = checkBoxFlag12.Checked;
            numericUpDownFloat7.Enabled = checkBoxFlag13.Checked;
            numericUpDownUInt7.Enabled = checkBoxFlag15.Checked;
            numericUpDownInt8.Enabled = checkBoxFlag16.Checked;
            numericUpDownUInt9.Enabled = checkBoxFlag17.Checked;
            numericUpDownFloat8.Enabled = checkBoxFlag18.Checked;

            UpdateCamItemBoxes();

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
            Camera new_trigger = new Camera { ID = id, Enabled = 1, Header = 1310720, SomeFloat = 0.3f, SectionHead = 10, Instances = new List<ushort>(),
                Coords = new Pos[] { new Pos(0, 0, 0, 1), new Pos(0, 0, 0, 1), new Pos(0, 0, 0, 1) }, UnkCoords1 = new Pos(0,0,0,1), UnkCoords2 = new Pos(0,0,0,1),
                CameraType1 = 3, CameraType2 = 3, UnkFloat1 = 1f};
            controller.Data.AddItem(id, new_trigger);
            ((MainForm)Tag).GenTreeNode(new_trigger, controller);
            trigger = new_trigger;
            listBox1.Items.Add($"ID {trigger.ID}");
            controller.UpdateTextBox();
            CurCont.UpdateText();
        }

        private void removeToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var sel_i = listBox1.SelectedIndex;
            if (sel_i == -1)
                return;
            controller.RemoveItem(trigger.ID);
            listBox1.BeginUpdate();
            listBox1.Items.RemoveAt(sel_i);
            for (int i = 0; i < controller.Data.Records.Count; ++i)
            {
                Camera new_trg = (Camera)controller.Data.Records[i];
                if (new_trg.ID != i)
                {
                    controller.ChangeID(new_trg.ID, (uint)i);
                    listBox1.Items[i] = $"ID {i}";
                    ((Controller)controller.Node.Nodes[i].Tag).UpdateText();
                }
            }
            if (sel_i >= listBox1.Items.Count) sel_i = listBox1.Items.Count - 1;
            listBox1.SelectedIndex = sel_i;
            listBox1.EndUpdate();
            if (listBox1.Items.Count == 0)
                splitContainer1.Panel2.Enabled = false;
            controller.UpdateTextBox();
        }

        private void numericUpDown1_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            trigger.Header = (uint)numericUpDown1.Value;
            checkBoxTFlag0.Checked = trigger.UnkFlag0;
            checkBoxTFlag1.Checked = trigger.UnkFlag1;
            checkBoxTFlag2.Checked = trigger.UnkFlag2;
            checkBoxTFlag3.Checked = trigger.UnkFlag3;
            checkBoxTFlag4.Checked = trigger.UnkFlag4;
            checkBoxTFlag5.Checked = trigger.UnkFlag5;
            checkBoxTFlag6.Checked = trigger.UnkFlag6;
            checkBoxTFlag18.Checked = trigger.UnkFlag18;
            checkBoxTFlag20.Checked = trigger.UnkFlag20;
            CurCont.UpdateTextBox();
            ignore_value_change = false;
        }

        private void numericUpDown2_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.Enabled = (uint)numericUpDown2.Value;

            bool[] TrigMask = trigger.Mask;
            checkBox5.Checked = TrigMask[0];
            checkBox6.Checked = TrigMask[1];
            checkBox7.Checked = TrigMask[2];
            checkBox8.Checked = TrigMask[3];
            checkBox9.Checked = TrigMask[4];
            checkBox10.Checked = TrigMask[5];
            checkBox11.Checked = TrigMask[6];
            checkBox12.Checked = TrigMask[7];
            checkBox13.Checked = TrigMask[8];
        }

        private void numericUpDown3_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.SomeFloat = (float)numericUpDown3.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDown4_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.SectionHead = (uint)numericUpDown4.Value;
            CurCont.UpdateTextBox();
        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.Instances.Clear();
            for (int i = 0; i < textBox1.Lines.Length; ++i)
            {
                if (ushort.TryParse(textBox1.Lines[i], out ushort v))
                {
                    if (File.GetInstanceID(controller.Data.Parent.ID, v) != null)
                        trigger.Instances.Add(v);
                }
            }
            controller.UpdateTextBox();
            CurCont.UpdateTextBox();
        }

        private void numericUpDown10_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.Coords[1].X = (float)numericUpDown10.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDown11_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.Coords[1].Y = (float)numericUpDown11.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDown12_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.Coords[1].Z = (float)numericUpDown12.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDown13_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.Coords[1].W = (float)numericUpDown13.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDown14_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.Coords[2].X = (float)numericUpDown14.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDown15_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.Coords[2].Y = (float)numericUpDown15.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDown16_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.Coords[2].Z = (float)numericUpDown16.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDown17_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.Coords[2].W = (float)numericUpDown17.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDown18_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.Coords[0].X = (float)((float)numericUpDown18.Value * Math.Sin((float)numericUpDown21.Value * 2));
            CurCont.UpdateTextBox();
        }

        private void numericUpDown19_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.Coords[0].Y = (float)((float)numericUpDown19.Value * Math.Sin((float)numericUpDown21.Value * 2));
            CurCont.UpdateTextBox();
        }

        private void numericUpDown20_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.Coords[0].Z = (float)((float)numericUpDown20.Value * Math.Sin((float)numericUpDown21.Value * 2));
            CurCont.UpdateTextBox();
        }

        private void numericUpDown21_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.Coords[0].W = (float)Math.Cos((float)numericUpDown21.Value / 2);
            trigger.Coords[0].X = (float)((float)numericUpDown18.Value * Math.Sin((float)numericUpDown21.Value * 2));
            trigger.Coords[0].Y = (float)((float)numericUpDown19.Value * Math.Sin((float)numericUpDown21.Value * 2));
            trigger.Coords[0].Z = (float)((float)numericUpDown20.Value * Math.Sin((float)numericUpDown21.Value * 2));
            CurCont.UpdateTextBox();
        }

        private void UpdateTrigMask()
        {
            bool[] TrigMask = trigger.Mask;
            TrigMask[0] = checkBox5.Checked;
            TrigMask[1] = checkBox6.Checked;
            TrigMask[2] = checkBox7.Checked;
            TrigMask[3] = checkBox8.Checked;
            TrigMask[4] = checkBox9.Checked;
            TrigMask[5] = checkBox10.Checked;
            TrigMask[6] = checkBox11.Checked;
            TrigMask[7] = checkBox12.Checked;
            TrigMask[8] = checkBox13.Checked;
            trigger.Mask = TrigMask;

            numericUpDown2.Value = trigger.Enabled;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            UpdateTrigMask();
            CurCont.UpdateTextBox();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            UpdateTrigMask();
            CurCont.UpdateTextBox();
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            UpdateTrigMask();
            CurCont.UpdateTextBox();
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            UpdateTrigMask();
            CurCont.UpdateTextBox();
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            UpdateTrigMask();
            CurCont.UpdateTextBox();
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            UpdateTrigMask();
            CurCont.UpdateTextBox();
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            UpdateTrigMask();
            CurCont.UpdateTextBox();
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            UpdateTrigMask();
            CurCont.UpdateTextBox();
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            UpdateTrigMask();
            CurCont.UpdateTextBox();
        }

        private void numericUpDowncamFlags_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.CamHeader = (uint)numericUpDowncamFlags.Value;
            CurCont.UpdateTextBox();

            checkBoxFlag0.Checked = (trigger.CamHeader & (1 << 0)) != 0;
            checkBoxFlag1.Checked = (trigger.CamHeader & (1 << 1)) != 0;
            checkBoxFlag2.Checked = (trigger.CamHeader & (1 << 2)) != 0;
            checkBoxFlag3.Checked = (trigger.CamHeader & (1 << 3)) != 0;
            checkBoxFlag4.Checked = (trigger.CamHeader & (1 << 4)) != 0;
            checkBoxFlag5.Checked = (trigger.CamHeader & (1 << 5)) != 0;
            checkBoxFlag6.Checked = (trigger.CamHeader & (1 << 6)) != 0;
            checkBoxFlag7.Checked = (trigger.CamHeader & (1 << 7)) != 0;
            checkBoxFlag8.Checked = (trigger.CamHeader & (1 << 8)) != 0;
            checkBoxFlag9.Checked = (trigger.CamHeader & (1 << 9)) != 0;
            checkBoxFlag10.Checked = (trigger.CamHeader & (1 << 10)) != 0;
            checkBoxFlag11.Checked = (trigger.CamHeader & (1 << 11)) != 0;
            checkBoxFlag12.Checked = (trigger.CamHeader & (1 << 12)) != 0;
            checkBoxFlag13.Checked = (trigger.CamHeader & (1 << 13)) != 0;
            checkBoxFlag14.Checked = (trigger.CamHeader & (1 << 14)) != 0;
            checkBoxFlag15.Checked = (trigger.CamHeader & (1 << 15)) != 0;
            checkBoxFlag16.Checked = (trigger.CamHeader & (1 << 16)) != 0;
            checkBoxFlag17.Checked = (trigger.CamHeader & (1 << 17)) != 0;
            checkBoxFlag18.Checked = (trigger.CamHeader & (1 << 18)) != 0;
            checkBoxFlag19.Checked = (trigger.CamHeader & (1 << 19)) != 0;
            checkBoxFlag20.Checked = (trigger.CamHeader & (1 << 20)) != 0;
            checkBoxFlag21.Checked = (trigger.CamHeader & (1 << 21)) != 0;
            checkBoxFlag22.Checked = (trigger.CamHeader & (1 << 22)) != 0;
            checkBoxFlag23.Checked = (trigger.CamHeader & (1 << 23)) != 0;
            checkBoxFlag24.Checked = (trigger.CamHeader & (1 << 24)) != 0;
            checkBoxFlag25.Checked = (trigger.CamHeader & (1 << 25)) != 0;
            checkBoxFlag26.Checked = (trigger.CamHeader & (1 << 26)) != 0;
            checkBoxFlag27.Checked = (trigger.CamHeader & (1 << 27)) != 0;
            checkBoxFlag28.Checked = (trigger.CamHeader & (1 << 28)) != 0;
            checkBoxFlag29.Checked = (trigger.CamHeader & (1 << 29)) != 0;
            checkBoxFlag30.Checked = (trigger.CamHeader & (1 << 30)) != 0;
            checkBoxFlag31.Checked = (trigger.CamHeader & (1 << 31)) != 0;
        }

        private void numericUpDowncamFloat_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkFloat1 = (float)numericUpDowncamFloat.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDowncamShort_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkShort = (ushort)numericUpDowncamShort.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDowncamByte_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkByte = (byte)numericUpDowncamByte.Value;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag0_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 0;
            if (checkBoxFlag0.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag1_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 1;
            if (checkBoxFlag1.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag2_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 2;
            if (checkBoxFlag2.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            numericUpDownUInt3.Enabled = checkBoxFlag2.Checked;
            numericUpDownUInt4.Enabled = checkBoxFlag2.Checked;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag3_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 3;
            if (checkBoxFlag3.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            numericUpDownFloat4.Enabled = checkBoxFlag3.Checked;
            numericUpDownFloat5.Enabled = checkBoxFlag3.Checked;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag4_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 4;
            if (checkBoxFlag4.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag5_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 5;
            if (checkBoxFlag5.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag6_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 6;
            if (checkBoxFlag6.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            numericUpDownInt5.Enabled = checkBoxFlag6.Checked;
            numericUpDownInt6.Enabled = checkBoxFlag6.Checked;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag7_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 7;
            if (checkBoxFlag7.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            numericUpDownUInt1.Enabled = checkBoxFlag7.Checked;
            numericUpDownUInt2.Enabled = checkBoxFlag7.Checked;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag8_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 8;
            if (checkBoxFlag8.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            numericUpDownUnkCoord1X.Enabled = checkBoxFlag8.Checked || checkBoxFlag28.Checked;
            numericUpDownUnkCoord1Y.Enabled = checkBoxFlag8.Checked || checkBoxFlag28.Checked;
            numericUpDownUnkCoord1Z.Enabled = checkBoxFlag8.Checked || checkBoxFlag28.Checked;
            numericUpDownUnkCoord1W.Enabled = checkBoxFlag28.Checked;
            numericUpDownUnkCoord2X.Enabled = checkBoxFlag8.Checked || checkBoxFlag28.Checked;
            numericUpDownUnkCoord2Y.Enabled = checkBoxFlag8.Checked || checkBoxFlag28.Checked;
            numericUpDownUnkCoord2Z.Enabled = checkBoxFlag8.Checked || checkBoxFlag28.Checked;
            numericUpDownUnkCoord2W.Enabled = checkBoxFlag28.Checked;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag9_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 9;
            if (checkBoxFlag9.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            numericUpDownFloat2.Enabled = checkBoxFlag9.Checked || checkBoxFlag10.Checked;
            numericUpDownFloat3.Enabled = checkBoxFlag9.Checked || checkBoxFlag10.Checked;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag10_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 10;
            if (checkBoxFlag10.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            numericUpDownFloat2.Enabled = checkBoxFlag9.Checked || checkBoxFlag10.Checked;
            numericUpDownFloat3.Enabled = checkBoxFlag9.Checked || checkBoxFlag10.Checked;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag11_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 11;
            if (checkBoxFlag11.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag12_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 12;
            if (checkBoxFlag12.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            numericUpDownFloat6.Enabled = checkBoxFlag12.Checked;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag13_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 13;
            if (checkBoxFlag13.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            numericUpDownFloat7.Enabled = checkBoxFlag13.Checked;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag14_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 14;
            if (checkBoxFlag14.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag15_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 15;
            if (checkBoxFlag15.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            numericUpDownUInt7.Enabled = checkBoxFlag15.Checked;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag16_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 16;
            if (checkBoxFlag16.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            numericUpDownInt8.Enabled = checkBoxFlag16.Checked;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag17_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 17;
            if (checkBoxFlag17.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            numericUpDownUInt9.Enabled = checkBoxFlag17.Checked;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag18_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 18;
            if (checkBoxFlag18.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            numericUpDownFloat8.Enabled = checkBoxFlag18.Checked;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag19_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 19;
            if (checkBoxFlag19.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag20_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 20;
            if (checkBoxFlag20.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag21_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 21;
            if (checkBoxFlag21.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag22_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 22;
            if (checkBoxFlag22.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag23_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 23;
            if (checkBoxFlag23.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag24_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 24;
            if (checkBoxFlag24.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag25_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 25;
            if (checkBoxFlag25.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag26_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 26;
            if (checkBoxFlag26.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag27_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 27;
            if (checkBoxFlag27.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag28_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 28;
            if (checkBoxFlag28.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            numericUpDownUnkCoord1X.Enabled = checkBoxFlag8.Checked || checkBoxFlag28.Checked;
            numericUpDownUnkCoord1Y.Enabled = checkBoxFlag8.Checked || checkBoxFlag28.Checked;
            numericUpDownUnkCoord1Z.Enabled = checkBoxFlag8.Checked || checkBoxFlag28.Checked;
            numericUpDownUnkCoord1W.Enabled = checkBoxFlag28.Checked;
            numericUpDownUnkCoord2X.Enabled = checkBoxFlag8.Checked || checkBoxFlag28.Checked;
            numericUpDownUnkCoord2Y.Enabled = checkBoxFlag8.Checked || checkBoxFlag28.Checked;
            numericUpDownUnkCoord2Z.Enabled = checkBoxFlag8.Checked || checkBoxFlag28.Checked;
            numericUpDownUnkCoord2W.Enabled = checkBoxFlag28.Checked;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag29_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 29;
            if (checkBoxFlag29.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag30_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            uint mask = 1 << 30;
            if (checkBoxFlag30.Checked)
                trigger.CamHeader |= mask;
            else
                trigger.CamHeader &= ~mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxFlag31_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            int mask = 1 << 31;
            if (checkBoxFlag31.Checked)
                trigger.CamHeader |= (uint)mask;
            else
                trigger.CamHeader &= ~(uint)mask;
            numericUpDowncamFlags.Value = trigger.CamHeader;
            ignore_value_change = false;
            CurCont.UpdateTextBox();
        }

        private void checkBoxTFlag0_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            trigger.UnkFlag0 = checkBoxTFlag0.Checked;
            numericUpDown1.Value = trigger.Header;
            CurCont.UpdateTextBox();
            ignore_value_change = false;
        }

        private void checkBoxTFlag1_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            trigger.UnkFlag1 = checkBoxTFlag1.Checked;
            numericUpDown1.Value = trigger.Header;
            CurCont.UpdateTextBox();
            ignore_value_change = false;
        }

        private void checkBoxTFlag2_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            trigger.UnkFlag2 = checkBoxTFlag2.Checked;
            numericUpDown1.Value = trigger.Header;
            CurCont.UpdateTextBox();
            ignore_value_change = false;
        }

        private void checkBoxTFlag3_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            trigger.UnkFlag3 = checkBoxTFlag3.Checked;
            numericUpDown1.Value = trigger.Header;
            CurCont.UpdateTextBox();
            ignore_value_change = false;
        }

        private void checkBoxTFlag4_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            trigger.UnkFlag4 = checkBoxTFlag4.Checked;
            numericUpDown1.Value = trigger.Header;
            CurCont.UpdateTextBox();
            ignore_value_change = false;
        }

        private void checkBoxTFlag5_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            trigger.UnkFlag5 = checkBoxTFlag5.Checked;
            numericUpDown1.Value = trigger.Header;
            CurCont.UpdateTextBox();
            ignore_value_change = false;
        }

        private void checkBoxTFlag6_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            trigger.UnkFlag6 = checkBoxTFlag6.Checked;
            numericUpDown1.Value = trigger.Header;
            CurCont.UpdateTextBox();
            ignore_value_change = false;
        }

        private void checkBoxTFlag18_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            trigger.UnkFlag18 = checkBoxTFlag18.Checked;
            numericUpDown1.Value = trigger.Header;
            CurCont.UpdateTextBox();
            ignore_value_change = false;
        }

        private void checkBoxTFlag20_CheckedChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ignore_value_change = true;
            trigger.UnkFlag20 = checkBoxTFlag20.Checked;
            numericUpDown1.Value = trigger.Header;
            CurCont.UpdateTextBox();
            ignore_value_change = false;
        }

        private void numericUpDownUInt3_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkUInt3 = (uint)numericUpDownUInt3.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownUInt4_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkUInt4 = (uint)numericUpDownUInt4.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownFloat4_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkFloat4 = (float)numericUpDownFloat4.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownFloat5_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkFloat5 = (float)numericUpDownFloat5.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownInt5_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkInt5 = (int)numericUpDownInt5.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownInt6_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkInt6 = (int)numericUpDownInt6.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownUInt1_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkUInt1 = (uint)numericUpDownUInt1.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownUInt2_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkUInt2 = (uint)numericUpDownUInt2.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownFloat2_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkFloat2 = (float)numericUpDownFloat2.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownFloat3_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkFloat3 = (float)numericUpDownFloat3.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownFloat6_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkFloat6 = (float)numericUpDownFloat6.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownFloat7_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkFloat7 = (float)numericUpDownFloat7.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownUInt7_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkUInt7 = (uint)numericUpDownUInt7.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownInt8_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkInt8 = (int)numericUpDownInt8.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownUInt9_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkUInt9 = (uint)numericUpDownUInt9.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownFloat8_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkFloat8 = (float)numericUpDownFloat8.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownUnkCoord1X_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkCoords1.X = (float)numericUpDownUnkCoord1X.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownUnkCoord1Y_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkCoords1.Y = (float)numericUpDownUnkCoord1Y.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownUnkCoord1Z_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkCoords1.Z = (float)numericUpDownUnkCoord1Z.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownUnkCoord1W_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkCoords1.W = (float)numericUpDownUnkCoord1W.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownUnkCoord2X_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkCoords2.X = (float)numericUpDownUnkCoord2X.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownUnkCoord2Y_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkCoords2.Y = (float)numericUpDownUnkCoord2Y.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownUnkCoord2Z_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkCoords2.Z = (float)numericUpDownUnkCoord2Z.Value;
            CurCont.UpdateTextBox();
        }

        private void numericUpDownUnkCoord2W_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            trigger.UnkCoords2.W = (float)numericUpDownUnkCoord2W.Value;
            CurCont.UpdateTextBox();
        }

        void UpdateCamItemBoxes()
        {
            string CamName1 = "None";
            string CamName2 = "None";
            switch (trigger.CameraType1)
            {
                default: break;
                case 0xA19: CamName1 = "Boss"; break;
                case 0x1C02: CamName1 = "Point"; break;
                case 0x1C03: CamName1 = "Line"; break;
                case 0x1C04: CamName1 = "Path"; break;
                case 0x1C05: CamName1 = "Unk1C05"; break;
                case 0x1C06: CamName1 = "Spline"; break;
                case 0x1C09: CamName1 = "Unk1C09"; break;
                case 0x1C0B: CamName1 = "Point2"; break;
                case 0x1C0C: CamName1 = "Unk1C0C"; break;
                case 0x1C0D: CamName1 = "Line2"; break;
                case 0x1C0E: CamName1 = "Unk1C0E"; break;
                case 0x1C0F: CamName1 = "Zone"; break;
            }
            switch (trigger.CameraType2)
            {
                default: break;
                case 0xA19: CamName2 = "Boss"; break;
                case 0x1C02: CamName2 = "Point"; break;
                case 0x1C03: CamName2 = "Line"; break;
                case 0x1C04: CamName2 = "Path"; break;
                case 0x1C05: CamName2 = "Unk1C05"; break;
                case 0x1C06: CamName2 = "Spline"; break;
                case 0x1C09: CamName2 = "Unk1C09"; break;
                case 0x1C0B: CamName2 = "Point2"; break;
                case 0x1C0C: CamName2 = "Unk1C0C"; break;
                case 0x1C0D: CamName2 = "Line2"; break;
                case 0x1C0E: CamName2 = "Unk1C0E"; break;
                case 0x1C0F: CamName2 = "Zone"; break;
            }
            groupBox_CamItem1.Text = $"Cam Item 1: {CamName1}";
            groupBox_CamItem2.Text = $"Cam Item 2: {CamName2}";
            button_camitem1add.Enabled = false; //trigger.CameraType1 == 3;
            button_camitem2add.Enabled = false; //trigger.CameraType2 == 3;
            button_camitem1edit.Enabled = false;//trigger.CameraType1 != 3;
            button_camitem2edit.Enabled = false; //trigger.CameraType2 != 3;
            button_camitem1delete.Enabled = trigger.CameraType1 != 3;
            button_camitem2delete.Enabled = trigger.CameraType2 != 3;
        }

        private void button_camitem1add_Click(object sender, EventArgs e)
        {

        }

        private void button_camitem1delete_Click(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            if (trigger.CameraType1 == 3) return;
            trigger.CameraType1 = 3;
            trigger.Cameras[0] = null;
            CurCont.UpdateTextBox();
            UpdateCamItemBoxes();
        }

        private void button_camitem1edit_Click(object sender, EventArgs e)
        {

        }

        private void button_camitem2add_Click(object sender, EventArgs e)
        {

        }

        private void button_camitem2delete_Click(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            if (trigger.CameraType2 == 3) return;
            trigger.CameraType2 = 3;
            trigger.Cameras[1] = null;
            CurCont.UpdateTextBox();
            UpdateCamItemBoxes();
        }

        private void button_camitem2edit_Click(object sender, EventArgs e)
        {

        }
    }
}
