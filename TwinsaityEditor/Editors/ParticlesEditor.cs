using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Twinsanity;

namespace TwinsaityEditor
{
    public partial class ParticlesEditor : Form
    {
        private ParticleDataController controller;
        ParticleData.ParticleSystemDefinition CurDef;
        ParticleData.ParticleSystemInstance CurInst;

        private FileController File { get; set; }
        private TwinsFile FileData { get => File.Data; }

        private bool ignore_value_change;

        public ParticlesEditor(ParticleDataController c)
        {
            File = c.MainFile;
            controller = c;
            InitializeComponent();
            Text = $"Particles Editor";
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
            int id = 0;
            foreach (var i in controller.Data.ParticleInstances)
            {
                listBox1.Items.Add($"{id:000}: {i.Name}");
                id++;
            }
            listBox2.Items.Clear();
            id = 0;
            foreach (var i in controller.Data.ParticleTypes)
            {
                listBox2.Items.Add($"{id:000}: {i.Name}");
                id++;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;

            this.SuspendDrawing();

            CurInst = controller.Data.ParticleInstances[listBox1.SelectedIndex];
            File.SelectParticle(controller.Data, CurInst);
            splitContainer1.Panel2.Enabled = true;

            ignore_value_change = true;

            textBox1.Text = CurInst.Name;
            numericUpDown10.Value = (decimal)CurInst.Position.X;
            numericUpDown11.Value = (decimal)CurInst.Position.Y;
            numericUpDown12.Value = (decimal)CurInst.Position.Z;
            numericUpDown14.Value = (decimal)CurInst.GravityRotX;
            numericUpDown15.Value = (decimal)CurInst.GravityRotY;
            numericUpDown18.Value = (decimal)CurInst.EmitRotX;
            numericUpDown19.Value = (decimal)CurInst.EmitRotY;
            numericUpDown1.Value = (decimal)CurInst.UnkShort5;
            numericUpDown2.Value = (decimal)CurInst.Offset;
            numericUpDown3.Value = (decimal)CurInst.SwitchType;
            numericUpDown4.Value = (decimal)CurInst.SwitchID;
            numericUpDown5.Value = (decimal)CurInst.SwitchValue;
            numericUpDown6.Value = (decimal)CurInst.UnkShort6;
            numericUpDown7.Value = (decimal)CurInst.UnkShort7;
            numericUpDown8.Value = (decimal)CurInst.PlaneOffset;
            numericUpDown9.Value = (decimal)CurInst.BounceFactor;
            numericUpDown13.Value = (decimal)CurInst.GroupID;

            ignore_value_change = false;

            this.ResumeDrawing();
        }

        private void addToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (controller.Data.ParticleInstances.Count == 255) return;
            string name = "Unnamed";
            if (controller.Data.ParticleTypes.Count != 0) name = controller.Data.ParticleTypes[0].Name;
            if (listBox2.SelectedIndex != -1) name = controller.Data.ParticleTypes[listBox2.SelectedIndex].Name;
            Pos targetPos = new Pos(0, 0, 0, 1);
            targetPos = File.RMViewer_GetPos(targetPos);
            var inst = new ParticleData.ParticleSystemInstance()
            {
                Name = name,
                Position = targetPos,
                SwitchID = -1,
                BounceFactor = 0.9f,
            };
            controller.Data.ParticleInstances.Add(inst);
            listBox1.Items.Clear();
            int id = 0;
            foreach (var i in controller.Data.ParticleInstances)
            {
                listBox1.Items.Add($"{id:000}: {i.Name}");
                id++;
            }
            controller.UpdateTextBox();
            File.RMViewer_LoadParticles();
            listBox1.SelectedIndex = controller.Data.ParticleInstances.Count - 1;
        }

        private void removeToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            controller.Data.ParticleInstances.RemoveAt(listBox1.SelectedIndex);
            listBox1.Items.Clear();
            int id = 0;
            foreach (var i in controller.Data.ParticleInstances)
            {
                listBox1.Items.Add($"{id:000}: {i.Name}");
                id++;
            }
            controller.UpdateTextBox();
            File.RMViewer_LoadParticles();
            splitContainer1.Panel2.Enabled = false;
        }

        private void numericUpDown10_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            CurInst.Position.X = (float)numericUpDown10.Value;
            File.RMViewer_LoadParticles();
        }

        private void numericUpDown11_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            CurInst.Position.Y = (float)numericUpDown11.Value;
            File.RMViewer_LoadParticles();
        }

        private void numericUpDown12_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            CurInst.Position.Z = (float)numericUpDown12.Value;
            File.RMViewer_LoadParticles();
        }

        private void numericUpDown14_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            CurInst.GravityRotX = (short)numericUpDown14.Value;
            File.RMViewer_LoadParticles();
        }

        private void numericUpDown15_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            CurInst.GravityRotY = (short)numericUpDown15.Value;
            File.RMViewer_LoadParticles();
        }

        private void numericUpDown18_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            CurInst.EmitRotX = (short)numericUpDown18.Value;
            File.RMViewer_LoadParticles();
        }

        private void numericUpDown19_ValueChanged(object sender, System.EventArgs e)
        {
            if (ignore_value_change) return;
            CurInst.EmitRotY = (short)numericUpDown19.Value;
            File.RMViewer_LoadParticles();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurInst.Name = textBox1.Text;
            listBox1.Items[listBox1.SelectedIndex] = $"{listBox1.SelectedIndex:000}: {CurInst.Name}";
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //if (controller.Data.ParticleTypes.Count == 255) return;
            string name = "Unnamed";
            var pdef = new ParticleData.ParticleSystemDefinition()
            {
                Name = name,
                UnkFloat1 = 25f,
                DrawCutOff = 9999.9f,
                UnkFloat6 = 0.5f,
                UnkFloat39 = 0.5f,
                ParticleLifeTime = 1f,
                UnkShorts = new short[4] {0,0,5,0},
            };
            controller.Data.ParticleTypes.Add(pdef);
            listBox2.Items.Clear();
            int id = 0;
            foreach (var i in controller.Data.ParticleTypes)
            {
                listBox2.Items.Add($"{id:000}: {i.Name}");
                id++;
            }
            controller.UpdateTextBox();
            listBox2.SelectedIndex = controller.Data.ParticleTypes.Count - 1;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1) return;
            controller.Data.ParticleTypes.RemoveAt(listBox2.SelectedIndex);
            listBox2.Items.Clear();
            int id = 0;
            foreach (var i in controller.Data.ParticleTypes)
            {
                listBox2.Items.Add($"{id:000}: {i.Name}");
                id++;
            }
            controller.UpdateTextBox();
            File.RMViewer_LoadParticles();
            splitContainer2.Panel2.Enabled = false;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurInst.UnkShort5 = (short)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurInst.Offset = (uint)numericUpDown2.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurInst.SwitchType = (int)numericUpDown3.Value;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurInst.SwitchID = (int)numericUpDown4.Value;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurInst.SwitchValue = (float)numericUpDown5.Value;
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurInst.UnkShort6 = (short)numericUpDown6.Value;
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurInst.UnkShort7 = (short)numericUpDown7.Value;
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurInst.PlaneOffset = (float)numericUpDown8.Value;
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurInst.BounceFactor = (float)numericUpDown9.Value;
        }

        private void numericUpDown13_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurInst.GroupID = (short)numericUpDown13.Value;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.Name = textBox2.Text;
            listBox2.Items[listBox2.SelectedIndex] = $"{listBox2.SelectedIndex:000}: {CurDef.Name}";
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1) return;

            this.SuspendDrawing();

            CurDef = controller.Data.ParticleTypes[listBox2.SelectedIndex];
            splitContainer2.Panel2.Enabled = true;

            ignore_value_change = true;

            textBox2.Text = CurDef.Name;

            ignore_value_change = false;

            this.ResumeDrawing();
        }
    }
}
