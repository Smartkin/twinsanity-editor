using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public partial class InstanceEditor : Form
    {
        private const string angleFormat = "{0:0.000}º";
        private SectionController controller;
        private Instance ins;
        
        private FileController TFController { get; set; }
        private TwinsFile File { get => TFController.Data; }

        private bool ignore_value_change;

        public InstanceEditor(FileController file, SectionController c)
        {
            TFController = file;
            controller = c;
            InitializeComponent();
            Text = "Instance Editor (Section " + c.Data.Parent.ID + ")";
            PopulateList();
            comboBox1.TextChanged += comboBox1_TextChanged;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            FormClosed += InstanceEditor_FormClosed;
        }

        private void InstanceEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            TFController.SelectItem(null);
        }

        private void PopulateList()
        {
            listBox1.Items.Clear();
            foreach (Instance i in controller.Data.Records)
            {
                listBox1.Items.Add(GenTextForList(i));
            }
            comboBox1.Items.Clear();
            var s_dic = new SortedDictionary<uint, int>(((TwinsSection)((TwinsSection)File.GetItem(10)).GetItem(0)).RecordIDs);
            foreach (var i in s_dic)
            {
                comboBox1.Items.Add(i.Key + " (" + TFController.GetObjectName(i.Key) + ")");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            ignore_value_change = true;

            this.SuspendDrawing();

            TFController.SelectItem((Instance)controller.Data.Records[listBox1.SelectedIndex]);
            ins = (Instance)TFController.SelectedItem;
            tabControl1.Enabled = true;
            tabControl1.Tag = 0x00;
            if (tabControl1.SelectedIndex == 0)
            {
                UpdateTab1();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                UpdateTab2();
            }

            ignore_value_change = false;

            this.ResumeDrawing();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0 && ((int)tabControl1.Tag & 0x01) == 0)
            {
                UpdateTab1();
            }
            else if (tabControl1.SelectedIndex == 1 && ((int)tabControl1.Tag & 0x02) == 0)
            {
                UpdateTab2();
            }
        }

        private void UpdateTab1()
        {
            string obj_name = TFController.GetObjectName(ins.ObjectID);
            comboBox1.Text = ins.ObjectID.ToString() + ((obj_name == string.Empty) ? string.Empty : (" (" + obj_name + ")"));
            numericUpDown1.Value = ins.ID;
            numericUpDown12.Value = listBox1.SelectedIndex;
            numericUpDown2.Value = (decimal)ins.Pos.X;
            numericUpDown3.Value = (decimal)ins.Pos.Y;
            numericUpDown4.Value = (decimal)ins.Pos.Z;
            numericUpDown5.Value = (decimal)ins.Pos.W;
            GetXRot(false, false, false); GetYRot(false, false, false); GetZRot(false, false, false);
            textBox1.Text = Convert.ToString(ins.UnkI32, 16);
            tabControl1.Tag = (int)tabControl1.Tag | 0x01;
        }

        private void UpdateTab2()
        {
            numericUpDown9.Value = ins.SomeNum1;
            numericUpDown10.Value = ins.SomeNum2;
            numericUpDown11.Value = ins.SomeNum3;

            string[] lines = new string[ins.S1.Count];
            for (int i = 0; i < ins.S1.Count; ++i)
                lines[i] = ins.S1[i].ToString();
            textBox2.Lines = lines;
            lines = new string[ins.S2.Count];
            for (int i = 0; i < ins.S2.Count; ++i)
                lines[i] = ins.S2[i].ToString();
            textBox3.Lines = lines;
            lines = new string[ins.S3.Count];
            for (int i = 0; i < ins.S3.Count; ++i)
                lines[i] = ins.S3[i].ToString();
            textBox4.Lines = lines;

            lines = new string[ins.UnkI321.Count];
            for (int i = 0; i < ins.UnkI321.Count; ++i)
                lines[i] = Convert.ToString(ins.UnkI321[i], 16);
            textBox7.Lines = lines;
            lines = new string[ins.UnkI322.Count];
            for (int i = 0; i < ins.UnkI322.Count; ++i)
                lines[i] = ins.UnkI322[i].ToString();
            textBox6.Lines = lines;
            lines = new string[ins.UnkI323.Count];
            for (int i = 0; i < ins.UnkI323.Count; ++i)
                lines[i] = ins.UnkI323[i].ToString();
            textBox5.Lines = lines;
            tabControl1.Tag = (int)tabControl1.Tag | 0x02;
        }

        private void GetXRot(bool ignore_slider, bool ignore_rot1, bool ignore_rot2)
        {
            if (!ignore_slider)
                trackBar1.Value = ins.RotX;
            if (!ignore_rot1)
                numericUpDown6.Value = ins.RotX;
            if (!ignore_rot2)
                numericUpDown13.Value = ins.COMRotX;
            label6.Text = string.Format(angleFormat, ins.RotX / (float)(ushort.MaxValue + 1) * 360f);
        }

        private void GetYRot(bool ignore_slider, bool ignore_rot1, bool ignore_rot2)
        {
            if (!ignore_slider)
                trackBar2.Value = ins.RotY;
            if (!ignore_rot1)
                numericUpDown7.Value = ins.RotY;
            if (!ignore_rot2)
                numericUpDown14.Value = ins.COMRotY;
            label7.Text = string.Format(angleFormat, ins.RotY / (float)(ushort.MaxValue + 1) * 360f);
        }

        private void GetZRot(bool ignore_slider, bool ignore_rot1, bool ignore_rot2)
        {
            if (!ignore_slider)
                trackBar3.Value = ins.RotZ;
            if (!ignore_rot1)
                numericUpDown8.Value = ins.RotZ;
            if (!ignore_rot2)
                numericUpDown15.Value = ins.COMRotZ;
            label9.Text = string.Format(angleFormat, ins.RotZ / (float)(ushort.MaxValue + 1) * 360f);
        }

        private string GenTextForList(Instance instance)
        {
            string obj_name = TFController.GetObjectName(instance.ObjectID);
            return "ID " + instance.ID + ((obj_name == string.Empty) ? string.Empty : (" (" + obj_name + ")"));
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            if (controller.Data.RecordIDs.ContainsKey((uint)numericUpDown1.Value))
            {
                MessageBox.Show("The specified ID already exists.");
                ignore_value_change = true;
                numericUpDown1.Value = ins.ID;
                ignore_value_change = false;
                return;
            }
            controller.Data.RecordIDs.Remove(ins.ID);
            ins.ID = (uint)numericUpDown1.Value;
            controller.Data.RecordIDs.Add(ins.ID, listBox1.SelectedIndex);
            listBox1.Items[listBox1.SelectedIndex] = GenTextForList(ins);
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateText();
            TFController.RMViewer_LoadInstances();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            if (ushort.TryParse(comboBox1.Text.Split(new char[] { ' ' }, 2)[0], out ushort oid))
            {
                ins.ObjectID = oid;
                listBox1.Items[listBox1.SelectedIndex] = GenTextForList(ins);
            }
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateText();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            if (uint.TryParse(textBox1.Text, System.Globalization.NumberStyles.HexNumber, null, out uint o))
            {
                ins.UnkI32 = o;
            }
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateText();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ins.Pos.X = (float)numericUpDown2.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateTextBox();
            TFController.RMViewer_LoadInstances();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ins.Pos.Y = (float)numericUpDown3.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateTextBox();
            TFController.RMViewer_LoadInstances();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ins.Pos.Z = (float)numericUpDown4.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateTextBox();
            TFController.RMViewer_LoadInstances();
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ins.Pos.W = (float)numericUpDown5.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateTextBox();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            ins.RotX = (ushort)trackBar1.Value;
            GetXRot(true, false, false);
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateTextBox();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            ins.RotY = (ushort)trackBar2.Value;
            GetYRot(true, false, false);
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateTextBox();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            ins.RotZ = (ushort)trackBar3.Value;
            GetZRot(true, false, false);
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateTextBox();
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ins.RotX = (ushort)numericUpDown6.Value;
            GetXRot(false, true, false);
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateText();
            TFController.RMViewer_LoadInstances();
        }

        private void numericUpDown13_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ins.COMRotX = (ushort)numericUpDown13.Value;
            GetXRot(false, false, true);
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateText();
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ins.RotY = (ushort)numericUpDown7.Value;
            GetYRot(false, true, false);
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateText();
            TFController.RMViewer_LoadInstances();
        }

        private void numericUpDown14_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ins.COMRotY = (ushort)numericUpDown14.Value;
            GetYRot(false, false, true);
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateText();
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ins.SomeNum1 = (int)numericUpDown9.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateText();
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ins.SomeNum2 = (int)numericUpDown10.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateText();
        }

        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ins.SomeNum3 = (int)numericUpDown11.Value;
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateText();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ins.S1.Clear();
            for (int i = 0; i < textBox2.Lines.Length; ++i)
            {
                if (ushort.TryParse(textBox2.Lines[i], out ushort v))
                    ins.S1.Add(v);
            }
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateText();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ins.S2.Clear();
            for (int i = 0; i < textBox3.Lines.Length; ++i)
            {
                if (ushort.TryParse(textBox3.Lines[i], out ushort v))
                    ins.S2.Add(v);
            }
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateText();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ins.S3.Clear();
            for (int i = 0; i < textBox4.Lines.Length; ++i)
            {
                if (ushort.TryParse(textBox4.Lines[i], out ushort v))
                    ins.S3.Add(v);
            }
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateText();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ins.UnkI321.Clear();
            for (int i = 0; i < textBox7.Lines.Length; ++i)
            {
                if (uint.TryParse(textBox7.Lines[i], out uint v))
                    ins.UnkI321.Add(v);
            }
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateText();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ins.UnkI322.Clear();
            for (int i = 0; i < textBox6.Lines.Length; ++i)
            {
                if (float.TryParse(textBox6.Lines[i], out float v))
                    ins.UnkI322.Add(v);
            }
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateText();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ins.UnkI323.Clear();
            for (int i = 0; i < textBox5.Lines.Length; ++i)
            {
                if (uint.TryParse(textBox5.Lines[i], out uint v))
                    ins.UnkI323.Add(v);
            }
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateText();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (controller.Data.RecordIDs.Count >= ushort.MaxValue) return;
            uint id;
            for (id = 0; id < uint.MaxValue; ++id)
            {
                if (!controller.Data.RecordIDs.ContainsKey(id))
                    break;
            }
            Instance new_ins = new Instance { ID = id, AfterOID = 0xFFFFFFFF, Pos = new Pos(0, 0, 0, 1) };
            controller.Data.AddItem(id, new_ins);
            ((MainForm)Tag).GenTreeNode(new_ins, controller);
            ins = new_ins;
            listBox1.Items.Add(GenTextForList(ins));
            controller.UpdateText();
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateText();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sel_i = listBox1.SelectedIndex;
            if (sel_i == -1)
                return;
            Controller.DisposeNode(controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]]);
            controller.Data.RemoveItem(ins.ID);
            listBox1.Items.RemoveAt(sel_i);
            if (sel_i >= listBox1.Items.Count) sel_i = listBox1.Items.Count - 1;
            listBox1.SelectedIndex = sel_i;
            if (listBox1.Items.Count == 0)
                tabControl1.Enabled = false;
            controller.UpdateText();
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ins.RotZ = (ushort)numericUpDown8.Value;
            GetZRot(false, true, false);
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateText();
            TFController.RMViewer_LoadInstances();
        }

        private void numericUpDown15_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            ins.COMRotZ = (ushort)numericUpDown15.Value;
            GetZRot(false, false, true);
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[ins.ID]].Tag).UpdateText();
        }
    }
}
