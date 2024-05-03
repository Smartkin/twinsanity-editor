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
            numericUpDown16.Value = (decimal)CurDef.GenRate;
            numericUpDown17.Value = (decimal)CurDef.MaxParticleCount;
            numericUpDown20.Value = (decimal)CurDef.UnkUShort3;
            numericUpDown21.Value = (decimal)CurDef.Emitter_OverTime;
            numericUpDown22.Value = (decimal)CurDef.Emitter_OverTimeRandom;
            numericUpDown23.Value = (decimal)CurDef.Emitter_OffTime;
            numericUpDown24.Value = (decimal)CurDef.Emitter_OffTimeRandom;
            comboBox1.SelectedIndex = (int)CurDef.GSort;
            numericUpDown25.Value = (decimal)CurDef.UnkByte3;
            comboBox2.SelectedIndex = (int)CurDef.TextureFilter;
            numericUpDown26.Value = (decimal)CurDef.UnkByte5;
            numericUpDown27.Value = (decimal)CurDef.UnkFloat1;
            numericUpDown28.Value = (decimal)CurDef.CutOnRadius;
            numericUpDown29.Value = (decimal)CurDef.CutOffRadius;
            numericUpDown30.Value = (decimal)CurDef.DrawCutOff;
            numericUpDown31.Value = (decimal)CurDef.UnkFloat5;
            numericUpDown32.Value = (decimal)CurDef.UnkFloat6;
            numericUpDown33.Value = (decimal)CurDef.Velocity;
            numericUpDown34.Value = (decimal)CurDef.Random_Emit_X;
            numericUpDown35.Value = (decimal)CurDef.Random_Emit_Y;
            numericUpDown36.Value = (decimal)CurDef.Random_Emit_Z;
            numericUpDown37.Value = (decimal)CurDef.Random_Start_X;
            numericUpDown38.Value = (decimal)CurDef.Random_Start_Y;
            numericUpDown39.Value = (decimal)CurDef.Random_Start_Z;
            numericUpDown40.Value = (decimal)CurDef.UnkFloat8;
            numericUpDown41.Value = (decimal)CurDef.UnkFloat9;
            numericUpDown42.Value = (decimal)CurDef.UnkFloat10;
            numericUpDown43.Value = (decimal)CurDef.UnkFloat11;
            numericUpDown44.Value = (decimal)CurDef.UnkFloat12;
            numericUpDown45.Value = (decimal)CurDef.UnkFloat13;
            numericUpDown46.Value = (decimal)CurDef.UnkFloat14;
            numericUpDown47.Value = (decimal)CurDef.UnkFloat15;
            numericUpDown48.Value = (decimal)CurDef.UnkFloat16;
            numericUpDown49.Value = (decimal)CurDef.UnkFloat17;
            numericUpDown50.Value = (decimal)CurDef.UnkFloat18;
            numericUpDown51.Value = (decimal)CurDef.UnkFloat19;
            numericUpDown52.Value = (decimal)CurDef.Gravity;
            numericUpDown53.Value = (decimal)CurDef.ParticleLifeTime;
            numericUpDown89.Value = (decimal)CurDef.UnkUShort8;
            numericUpDown88.Value = (decimal)CurDef.UnkByte6;
            numericUpDown54.Value = (decimal)CurDef.UnkByte7;
            numericUpDown55.Value = (decimal)CurDef.UnkFloat22;
            numericUpDown56.Value = (decimal)CurDef.JibberXFreq;
            numericUpDown57.Value = (decimal)CurDef.JibberXAmp;
            numericUpDown58.Value = (decimal)CurDef.JibberYFreq;
            numericUpDown59.Value = (decimal)CurDef.JibberYAmp;
            numericUpDown60.Value = (decimal)CurDef.UnkVecs[0].X;
            numericUpDown61.Value = (decimal)CurDef.UnkVecs[0].Y;
            numericUpDown62.Value = (decimal)CurDef.UnkVecs[0].Z;
            numericUpDown63.Value = (decimal)CurDef.UnkVecs[0].W;
            numericUpDown64.Value = (decimal)CurDef.UnkVecs[1].X;
            numericUpDown65.Value = (decimal)CurDef.UnkVecs[1].Y;
            numericUpDown66.Value = (decimal)CurDef.UnkVecs[1].Z;
            numericUpDown67.Value = (decimal)CurDef.UnkVecs[1].W;
            numericUpDown68.Value = (decimal)CurDef.UnkVecs[2].X;
            numericUpDown69.Value = (decimal)CurDef.UnkVecs[2].Y;
            numericUpDown70.Value = (decimal)CurDef.UnkVecs[2].Z;
            numericUpDown71.Value = (decimal)CurDef.UnkVecs[2].W;
            numericUpDown72.Value = (decimal)CurDef.UnkVecs[3].X;
            numericUpDown73.Value = (decimal)CurDef.UnkVecs[3].Y;
            numericUpDown74.Value = (decimal)CurDef.UnkVecs[3].Z;
            numericUpDown75.Value = (decimal)CurDef.UnkVecs[3].W;
            numericUpDown76.Value = (decimal)CurDef.UnkVecs[4].X;
            numericUpDown77.Value = (decimal)CurDef.UnkVecs[4].Y;
            numericUpDown78.Value = (decimal)CurDef.UnkVecs[4].Z;
            numericUpDown79.Value = (decimal)CurDef.UnkVecs[4].W;
            numericUpDown80.Value = (decimal)CurDef.UnkVecs[5].X;
            numericUpDown81.Value = (decimal)CurDef.UnkVecs[5].Y;
            numericUpDown82.Value = (decimal)CurDef.UnkVecs[5].Z;
            numericUpDown83.Value = (decimal)CurDef.UnkVecs[5].W;
            numericUpDown84.Value = (decimal)CurDef.UnkVecs[6].X;
            numericUpDown85.Value = (decimal)CurDef.UnkVecs[6].Y;
            numericUpDown86.Value = (decimal)CurDef.UnkVecs[6].Z;
            numericUpDown87.Value = (decimal)CurDef.UnkVecs[6].W;
            numericUpDown90.Value = (decimal)CurDef.UnkVecs[7].X;
            numericUpDown91.Value = (decimal)CurDef.UnkVecs[7].Y;
            numericUpDown92.Value = (decimal)CurDef.UnkVecs[7].Z;
            numericUpDown93.Value = (decimal)CurDef.UnkVecs[7].W;
            numericUpDown94.Value = (decimal)CurDef.UnkFloat27;
            numericUpDown95.Value = (decimal)CurDef.UnkFloat28;
            numericUpDown96.Value = (decimal)CurDef.UnkFloat29;
            numericUpDown97.Value = (decimal)CurDef.UnkFloat30;
            numericUpDown98.Value = (decimal)CurDef.UnkFloat31;
            numericUpDown99.Value = (decimal)CurDef.UnkFloat32;
            numericUpDown100.Value = (decimal)CurDef.UnkFloat33;
            numericUpDown101.Value = (decimal)CurDef.UnkFloat34;
            numericUpDown102.Value = (decimal)CurDef.UnkFloat35;
            numericUpDown103.Value = (decimal)CurDef.UnkFloat36;
            numericUpDown104.Value = (decimal)CurDef.UnkByte8;
            numericUpDown105.Value = (decimal)CurDef.UnkByte9;
            numericUpDown106.Value = (decimal)CurDef.UnkFloat37;
            numericUpDown107.Value = (decimal)CurDef.UnkShorts[1];
            numericUpDown108.Value = (decimal)CurDef.UnkShorts[2];
            numericUpDown109.Value = (decimal)CurDef.UnkFloat38;
            numericUpDown110.Value = (decimal)CurDef.UnkFloat39;
            numericUpDown111.Value = (decimal)CurDef.UnkFloat40;
            numericUpDown112.Value = (decimal)CurDef.TexturePage;
            numericUpDown113.Value = (decimal)CurDef.UnkVec3.X;
            numericUpDown114.Value = (decimal)CurDef.UnkVec3.Y;
            numericUpDown115.Value = (decimal)CurDef.UnkVec3.Z;
            numericUpDown116.Value = (decimal)CurDef.UnkVec3.W;

            ignore_value_change = false;

            this.ResumeDrawing();
        }

        private void numericUpDown16_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.GenRate = (short)numericUpDown16.Value;
        }

        private void numericUpDown17_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.MaxParticleCount = (ushort)numericUpDown17.Value;
        }

        private void numericUpDown20_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkUShort3 = (ushort)numericUpDown20.Value;
        }

        private void numericUpDown21_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.Emitter_OverTime = (ushort)numericUpDown21.Value;
        }

        private void numericUpDown22_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.Emitter_OverTimeRandom = (ushort)numericUpDown22.Value;
        }

        private void numericUpDown23_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.Emitter_OffTime = (ushort)numericUpDown23.Value;
        }

        private void numericUpDown24_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.Emitter_OffTimeRandom = (ushort)numericUpDown24.Value;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.GSort = (ParticleData.ParticleSystemDefinition.GenSort)comboBox1.SelectedIndex;
        }

        private void numericUpDown25_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkByte3 = (byte)numericUpDown25.Value;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.TextureFilter = (ParticleData.ParticleSystemDefinition.TextureFiltering)comboBox2.SelectedIndex;
        }

        private void numericUpDown26_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkByte5 = (byte)numericUpDown26.Value;
        }

        private void numericUpDown27_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat1 = (float)numericUpDown27.Value;
        }

        private void numericUpDown28_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.CutOnRadius = (float)numericUpDown28.Value;
        }

        private void numericUpDown29_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.CutOffRadius = (float)numericUpDown29.Value;
        }

        private void numericUpDown30_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.DrawCutOff = (float)numericUpDown30.Value;
        }

        private void numericUpDown31_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat5 = (float)numericUpDown31.Value;
        }

        private void numericUpDown32_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat6 = (float)numericUpDown32.Value;
        }

        private void numericUpDown33_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.Velocity = (float)numericUpDown33.Value;
        }

        private void numericUpDown34_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.Random_Emit_X = (float)numericUpDown34.Value;
        }

        private void numericUpDown35_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.Random_Emit_Y = (float)numericUpDown35.Value;
        }

        private void numericUpDown36_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.Random_Emit_Z = (float)numericUpDown36.Value;
        }

        private void numericUpDown37_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.Random_Start_X = (float)numericUpDown37.Value;
        }

        private void numericUpDown38_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.Random_Start_Y = (float)numericUpDown38.Value;
        }

        private void numericUpDown39_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.Random_Start_Z = (float)numericUpDown39.Value;
        }

        private void numericUpDown40_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat8 = (float)numericUpDown40.Value;
        }

        private void numericUpDown41_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat9 = (float)numericUpDown41.Value;
        }

        private void numericUpDown42_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat10 = (float)numericUpDown42.Value;
        }

        private void numericUpDown43_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat11 = (float)numericUpDown43.Value;
        }

        private void numericUpDown44_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat12 = (float)numericUpDown44.Value;
        }

        private void numericUpDown45_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat13 = (float)numericUpDown45.Value;
        }

        private void numericUpDown46_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat14 = (float)numericUpDown46.Value;
        }

        private void numericUpDown47_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat15 = (float)numericUpDown47.Value;
        }

        private void numericUpDown48_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat16 = (float)numericUpDown48.Value;
        }

        private void numericUpDown49_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat17 = (float)numericUpDown49.Value;
        }

        private void numericUpDown50_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat18 = (float)numericUpDown50.Value;
        }

        private void numericUpDown51_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat19 = (float)numericUpDown51.Value;
        }

        private void numericUpDown52_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.Gravity = (float)numericUpDown52.Value;
        }

        private void numericUpDown53_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ParticleLifeTime = (float)numericUpDown53.Value;
        }

        private void numericUpDown89_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkUShort8 = (ushort)numericUpDown89.Value;
        }

        private void numericUpDown88_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkByte6 = (byte)numericUpDown88.Value;
        }

        private void numericUpDown54_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkByte7 = (byte)numericUpDown54.Value;
        }

        private void numericUpDown55_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat22 = (float)numericUpDown55.Value;
        }

        private void numericUpDown56_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.JibberXFreq = (float)numericUpDown56.Value;
        }

        private void numericUpDown57_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.JibberXAmp = (float)numericUpDown57.Value;
        }

        private void numericUpDown58_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.JibberYFreq = (float)numericUpDown58.Value;
        }

        private void numericUpDown59_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.JibberYAmp = (float)numericUpDown59.Value;
        }

        private void numericUpDown60_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[0].X = (float)numericUpDown60.Value;
        }

        private void numericUpDown61_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[0].Y = (float)numericUpDown61.Value;
        }

        private void numericUpDown62_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[0].Z = (float)numericUpDown62.Value;
        }

        private void numericUpDown63_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[0].W = (float)numericUpDown63.Value;
        }

        private void numericUpDown64_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[1].X = (float)numericUpDown64.Value;
        }

        private void numericUpDown65_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[1].Y = (float)numericUpDown65.Value;
        }

        private void numericUpDown66_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[1].Z = (float)numericUpDown66.Value;
        }

        private void numericUpDown67_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[1].W = (float)numericUpDown67.Value;
        }

        private void numericUpDown68_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[2].X = (float)numericUpDown68.Value;
        }

        private void numericUpDown69_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[2].Y = (float)numericUpDown69.Value;
        }

        private void numericUpDown70_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[2].Z = (float)numericUpDown70.Value;
        }

        private void numericUpDown71_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[2].W = (float)numericUpDown71.Value;
        }

        private void numericUpDown72_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[3].X = (float)numericUpDown72.Value;
        }

        private void numericUpDown73_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[3].Y = (float)numericUpDown73.Value;
        }

        private void numericUpDown74_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[3].Z = (float)numericUpDown74.Value;
        }

        private void numericUpDown75_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[3].W = (float)numericUpDown75.Value;
        }

        private void numericUpDown76_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[4].X = (float)numericUpDown76.Value;
        }

        private void numericUpDown77_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[4].Y = (float)numericUpDown77.Value;
        }

        private void numericUpDown78_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[4].Z = (float)numericUpDown78.Value;
        }

        private void numericUpDown79_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[4].W = (float)numericUpDown79.Value;
        }

        private void numericUpDown80_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[5].X = (float)numericUpDown80.Value;
        }

        private void numericUpDown81_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[5].Y = (float)numericUpDown81.Value;
        }

        private void numericUpDown82_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[5].Z = (float)numericUpDown82.Value;
        }

        private void numericUpDown83_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[5].W = (float)numericUpDown83.Value;
        }

        private void numericUpDown84_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[6].X = (float)numericUpDown84.Value;
        }

        private void numericUpDown85_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[6].Y = (float)numericUpDown85.Value;
        }

        private void numericUpDown86_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[6].Z = (float)numericUpDown86.Value;
        }

        private void numericUpDown87_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[6].W = (float)numericUpDown87.Value;
        }

        private void numericUpDown90_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[7].X = (float)numericUpDown90.Value;
        }

        private void numericUpDown91_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[7].Y = (float)numericUpDown91.Value;
        }

        private void numericUpDown92_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[7].Z = (float)numericUpDown92.Value;
        }

        private void numericUpDown93_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVecs[7].W = (float)numericUpDown93.Value;
        }

        private void numericUpDown94_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat27 = (float)numericUpDown94.Value;
        }

        private void numericUpDown95_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat28 = (float)numericUpDown95.Value;
        }

        private void numericUpDown96_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat29 = (float)numericUpDown96.Value;
        }

        private void numericUpDown97_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat30 = (float)numericUpDown97.Value;
        }

        private void numericUpDown98_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat31 = (float)numericUpDown98.Value;
        }

        private void numericUpDown99_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat32 = (float)numericUpDown99.Value;
        }

        private void numericUpDown100_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat33 = (float)numericUpDown100.Value;
        }

        private void numericUpDown101_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat34 = (float)numericUpDown101.Value;
        }

        private void numericUpDown102_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat35 = (float)numericUpDown102.Value;
        }

        private void numericUpDown103_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat36 = (float)numericUpDown103.Value;
        }

        private void numericUpDown104_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkByte8 = (byte)numericUpDown104.Value;
        }

        private void numericUpDown105_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkByte9 = (byte)numericUpDown105.Value;
        }

        private void numericUpDown106_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat37 = (float)numericUpDown94.Value;
        }

        private void numericUpDown107_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkShorts[1] = (short)numericUpDown107.Value;
        }

        private void numericUpDown108_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkShorts[2] = (short)numericUpDown108.Value;
        }

        private void numericUpDown109_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat38 = (float)numericUpDown109.Value;
        }

        private void numericUpDown110_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat39 = (float)numericUpDown110.Value;
        }

        private void numericUpDown111_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkFloat40 = (float)numericUpDown111.Value;
        }

        private void numericUpDown112_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.TexturePage = (int)numericUpDown112.Value;
        }

        private void numericUpDown113_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVec3.X = (float)numericUpDown113.Value;
        }

        private void numericUpDown114_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVec3.Y = (float)numericUpDown114.Value;
        }

        private void numericUpDown115_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVec3.Z = (float)numericUpDown115.Value;
        }

        private void numericUpDown116_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkVec3.W = (float)numericUpDown116.Value;
        }
    }
}
