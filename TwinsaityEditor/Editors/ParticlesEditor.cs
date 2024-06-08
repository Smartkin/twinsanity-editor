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
            var partdef = new ParticleData.ParticleSystemDefinition()
            {
                Name = name,
                GenRate = 1,
                MaxParticleCount = 10,
                Emitter_OverTime = 1,
                UnkFloat1 = 25f, // 40000f
                CutOffRadius = 25f,
                DrawCutOff = 9999.9f,
                UnkFloat6 = 0.5f,
                Velocity = 1f,
                ParticleLifeTime = 1f,
                UnkByte7 = 1,
                DistortionX = 0.125f,
                DistortionY = 0.125f,
                MinSize = 0f,
                MaxSize = 500f,
                MinRotation = -360f,
                MaxRotation = 360f,
                TextureStartX = 524288f,
                TextureStartY = 524288f,
                TextureEndX = 524416f,
                TextureEndY = 524416f,
                StarRadialPoints = 5,
                StarRadiusRatio = 0.5f,
                UnkVec3 = new TwinsVector4(0.7500094f, 0.7500094f, 0.7500094f, 0f),
            };
            partdef.ColorGradient[0] = new TwinsVector4(0f, 64f, 64f, 64f);
            partdef.ColorGradient[1] = new TwinsVector4(1f, 0f, 0f, 0f);
            partdef.AlphaGradientTime[1] = 1f;
            partdef.AlphaGradientValue[0] = 64;
            partdef.SizeWidthTime[1] = 1f;
            partdef.SizeWidthValue[0] = 500f;
            partdef.SizeWidthValue[1] = 500f;
            partdef.SizeHeightTime[1] = 1f;
            partdef.SizeHeightValue[0] = 500f;
            partdef.SizeHeightValue[1] = 500f;
            partdef.RotationTime[1] = 1f;
            partdef.UnkGradient1Time[1] = 1f;
            partdef.UnkGradient2Time[1] = 1f;
            controller.Data.ParticleTypes.Add(partdef);
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
            numericUpDown60.Value = (decimal)CurDef.ColorGradient[0].X;
            numericUpDown61.Value = (decimal)CurDef.ColorGradient[0].Y;
            numericUpDown62.Value = (decimal)CurDef.ColorGradient[0].Z;
            numericUpDown63.Value = (decimal)CurDef.ColorGradient[0].W;
            numericUpDown64.Value = (decimal)CurDef.ColorGradient[1].X;
            numericUpDown65.Value = (decimal)CurDef.ColorGradient[1].Y;
            numericUpDown66.Value = (decimal)CurDef.ColorGradient[1].Z;
            numericUpDown67.Value = (decimal)CurDef.ColorGradient[1].W;
            numericUpDown68.Value = (decimal)CurDef.ColorGradient[2].X;
            numericUpDown69.Value = (decimal)CurDef.ColorGradient[2].Y;
            numericUpDown70.Value = (decimal)CurDef.ColorGradient[2].Z;
            numericUpDown71.Value = (decimal)CurDef.ColorGradient[2].W;
            numericUpDown72.Value = (decimal)CurDef.ColorGradient[3].X;
            numericUpDown73.Value = (decimal)CurDef.ColorGradient[3].Y;
            numericUpDown74.Value = (decimal)CurDef.ColorGradient[3].Z;
            numericUpDown75.Value = (decimal)CurDef.ColorGradient[3].W;
            numericUpDown76.Value = (decimal)CurDef.ColorGradient[4].X;
            numericUpDown77.Value = (decimal)CurDef.ColorGradient[4].Y;
            numericUpDown78.Value = (decimal)CurDef.ColorGradient[4].Z;
            numericUpDown79.Value = (decimal)CurDef.ColorGradient[4].W;
            numericUpDown80.Value = (decimal)CurDef.ColorGradient[5].X;
            numericUpDown81.Value = (decimal)CurDef.ColorGradient[5].Y;
            numericUpDown82.Value = (decimal)CurDef.ColorGradient[5].Z;
            numericUpDown83.Value = (decimal)CurDef.ColorGradient[5].W;
            numericUpDown84.Value = (decimal)CurDef.ColorGradient[6].X;
            numericUpDown85.Value = (decimal)CurDef.ColorGradient[6].Y;
            numericUpDown86.Value = (decimal)CurDef.ColorGradient[6].Z;
            numericUpDown87.Value = (decimal)CurDef.ColorGradient[6].W;
            numericUpDown90.Value = (decimal)CurDef.ColorGradient[7].X;
            numericUpDown91.Value = (decimal)CurDef.ColorGradient[7].Y;
            numericUpDown92.Value = (decimal)CurDef.ColorGradient[7].Z;
            numericUpDown93.Value = (decimal)CurDef.ColorGradient[7].W;
            numericUpDown94.Value = (decimal)CurDef.DistortionX;
            numericUpDown95.Value = (decimal)CurDef.DistortionY;
            numericUpDown96.Value = (decimal)CurDef.MinSize;
            numericUpDown97.Value = (decimal)CurDef.MaxSize;
            numericUpDown98.Value = (decimal)CurDef.MinRotation;
            numericUpDown99.Value = (decimal)CurDef.MaxRotation;

            if (CurDef.TextureStartX >= 524288f)
                numericUpDown100.Value = (decimal)(CurDef.TextureStartX - 524288f);
            else if (CurDef.TextureStartX >= 262144f)
                numericUpDown100.Value = (decimal)(CurDef.TextureStartX - 262144f);
            else
                numericUpDown100.Value = (decimal)CurDef.TextureStartX;

            if (CurDef.TextureStartY >= 524288f)
                numericUpDown101.Value = (decimal)(CurDef.TextureStartY - 524288f);
            else if (CurDef.TextureStartY >= 262144f)
                numericUpDown101.Value = (decimal)(CurDef.TextureStartY - 262144f);
            else
                numericUpDown101.Value = (decimal)CurDef.TextureStartY;

            if (CurDef.TextureEndX >= 524288f)
                numericUpDown102.Value = (decimal)(CurDef.TextureEndX - 524288f);
            else if (CurDef.TextureEndX >= 262144f)
                numericUpDown102.Value = (decimal)(CurDef.TextureEndX - 262144f);
            else
                numericUpDown102.Value = (decimal)CurDef.TextureEndX;

            if (CurDef.TextureEndY >= 524288f)
                numericUpDown103.Value = (decimal)(CurDef.TextureEndY - 524288f);
            else if (CurDef.TextureEndY >= 262144f)
                numericUpDown103.Value = (decimal)(CurDef.TextureEndY - 262144f);
            else
                numericUpDown103.Value = (decimal)CurDef.TextureEndY;

            numericUpDown104.Value = (decimal)CurDef.CollisionNumSpheres;
            //numericUpDown105.Value = (decimal)CurDef.DrawFlag;
            comboBox3.SelectedIndex = (int)CurDef.DrawFlag;
            numericUpDown106.Value = (decimal)CurDef.ScaleFactor;
            numericUpDown107.Value = (decimal)CurDef.ParticleGhostsNum;
            numericUpDown108.Value = (decimal)CurDef.StarRadialPoints;
            numericUpDown109.Value = (decimal)CurDef.GhostSeparation;
            numericUpDown110.Value = (decimal)CurDef.StarRadiusRatio;
            numericUpDown111.Value = (decimal)CurDef.RampTime;
            numericUpDown112.Value = (decimal)CurDef.TexturePage;
            numericUpDown113.Value = (decimal)CurDef.UnkVec3.X;
            numericUpDown114.Value = (decimal)CurDef.UnkVec3.Y;
            numericUpDown115.Value = (decimal)CurDef.UnkVec3.Z;
            numericUpDown116.Value = (decimal)CurDef.UnkVec3.W;

            numericUpDown117.Value = (decimal)CurDef.AlphaGradientTime[0];
            numericUpDown118.Value = (decimal)CurDef.AlphaGradientTime[1];
            numericUpDown119.Value = (decimal)CurDef.AlphaGradientTime[2];
            numericUpDown120.Value = (decimal)CurDef.AlphaGradientTime[3];
            numericUpDown121.Value = (decimal)CurDef.AlphaGradientTime[4];
            numericUpDown122.Value = (decimal)CurDef.AlphaGradientTime[5];
            numericUpDown123.Value = (decimal)CurDef.AlphaGradientTime[6];
            numericUpDown124.Value = (decimal)CurDef.AlphaGradientTime[7];
            numericUpDown125.Value = (decimal)CurDef.AlphaGradientValue[0];
            numericUpDown126.Value = (decimal)CurDef.AlphaGradientValue[1];
            numericUpDown127.Value = (decimal)CurDef.AlphaGradientValue[2];
            numericUpDown128.Value = (decimal)CurDef.AlphaGradientValue[3];
            numericUpDown129.Value = (decimal)CurDef.AlphaGradientValue[4];
            numericUpDown130.Value = (decimal)CurDef.AlphaGradientValue[5];
            numericUpDown131.Value = (decimal)CurDef.AlphaGradientValue[6];
            numericUpDown132.Value = (decimal)CurDef.AlphaGradientValue[7];
            numericUpDown133.Value = (decimal)CurDef.RotationTime[0];
            numericUpDown134.Value = (decimal)CurDef.RotationTime[1];
            numericUpDown135.Value = (decimal)CurDef.RotationTime[2];
            numericUpDown136.Value = (decimal)CurDef.RotationTime[3];
            numericUpDown137.Value = (decimal)CurDef.RotationTime[4];
            numericUpDown138.Value = (decimal)CurDef.RotationTime[5];
            numericUpDown139.Value = (decimal)CurDef.RotationTime[6];
            numericUpDown140.Value = (decimal)CurDef.RotationTime[7];
            numericUpDown141.Value = (decimal)(CurDef.RotationValue[0] / 65535f * 360f);
            numericUpDown142.Value = (decimal)(CurDef.RotationValue[1] / 65535f * 360f);
            numericUpDown143.Value = (decimal)(CurDef.RotationValue[2] / 65535f * 360f);
            numericUpDown144.Value = (decimal)(CurDef.RotationValue[3] / 65535f * 360f);
            numericUpDown145.Value = (decimal)(CurDef.RotationValue[4] / 65535f * 360f);
            numericUpDown146.Value = (decimal)(CurDef.RotationValue[5] / 65535f * 360f);
            numericUpDown147.Value = (decimal)(CurDef.RotationValue[6] / 65535f * 360f);
            numericUpDown148.Value = (decimal)(CurDef.RotationValue[7] / 65535f * 360f);
            numericUpDown149.Value = (decimal)CurDef.SizeWidthTime[0];
            numericUpDown150.Value = (decimal)CurDef.SizeWidthTime[1];
            numericUpDown151.Value = (decimal)CurDef.SizeWidthTime[2];
            numericUpDown152.Value = (decimal)CurDef.SizeWidthTime[3];
            numericUpDown153.Value = (decimal)CurDef.SizeWidthTime[4];
            numericUpDown154.Value = (decimal)CurDef.SizeWidthTime[5];
            numericUpDown155.Value = (decimal)CurDef.SizeWidthTime[6];
            numericUpDown156.Value = (decimal)CurDef.SizeWidthTime[7];
            numericUpDown157.Value = (decimal)CurDef.SizeWidthValue[0];
            numericUpDown158.Value = (decimal)CurDef.SizeWidthValue[1];
            numericUpDown159.Value = (decimal)CurDef.SizeWidthValue[2];
            numericUpDown160.Value = (decimal)CurDef.SizeWidthValue[3];
            numericUpDown161.Value = (decimal)CurDef.SizeWidthValue[4];
            numericUpDown162.Value = (decimal)CurDef.SizeWidthValue[5];
            numericUpDown163.Value = (decimal)CurDef.SizeWidthValue[6];
            numericUpDown164.Value = (decimal)CurDef.SizeWidthValue[7];
            numericUpDown165.Value = (decimal)CurDef.SizeHeightTime[0];
            numericUpDown166.Value = (decimal)CurDef.SizeHeightTime[1];
            numericUpDown167.Value = (decimal)CurDef.SizeHeightTime[2];
            numericUpDown168.Value = (decimal)CurDef.SizeHeightTime[3];
            numericUpDown169.Value = (decimal)CurDef.SizeHeightTime[4];
            numericUpDown170.Value = (decimal)CurDef.SizeHeightTime[5];
            numericUpDown171.Value = (decimal)CurDef.SizeHeightTime[6];
            numericUpDown172.Value = (decimal)CurDef.SizeHeightTime[7];
            numericUpDown173.Value = (decimal)CurDef.SizeHeightValue[0];
            numericUpDown174.Value = (decimal)CurDef.SizeHeightValue[1];
            numericUpDown175.Value = (decimal)CurDef.SizeHeightValue[2];
            numericUpDown176.Value = (decimal)CurDef.SizeHeightValue[3];
            numericUpDown177.Value = (decimal)CurDef.SizeHeightValue[4];
            numericUpDown178.Value = (decimal)CurDef.SizeHeightValue[5];
            numericUpDown179.Value = (decimal)CurDef.SizeHeightValue[6];
            numericUpDown180.Value = (decimal)CurDef.SizeHeightValue[7];
            numericUpDown181.Value = (decimal)CurDef.CollisionTime[0];
            numericUpDown182.Value = (decimal)CurDef.CollisionTime[1];
            numericUpDown183.Value = (decimal)CurDef.CollisionTime[2];
            numericUpDown184.Value = (decimal)CurDef.CollisionTime[3];
            numericUpDown185.Value = (decimal)CurDef.CollisionTime[4];
            numericUpDown186.Value = (decimal)CurDef.CollisionTime[5];
            numericUpDown187.Value = (decimal)CurDef.CollisionTime[6];
            numericUpDown188.Value = (decimal)CurDef.CollisionTime[7];
            numericUpDown189.Value = (decimal)CurDef.CollisionValue[0];
            numericUpDown190.Value = (decimal)CurDef.CollisionValue[1];
            numericUpDown191.Value = (decimal)CurDef.CollisionValue[2];
            numericUpDown192.Value = (decimal)CurDef.CollisionValue[3];
            numericUpDown193.Value = (decimal)CurDef.CollisionValue[4];
            numericUpDown194.Value = (decimal)CurDef.CollisionValue[5];
            numericUpDown195.Value = (decimal)CurDef.CollisionValue[6];
            numericUpDown196.Value = (decimal)CurDef.CollisionValue[7];
            numericUpDown197.Value = (decimal)CurDef.UnkGradient1Time[0];
            numericUpDown198.Value = (decimal)CurDef.UnkGradient1Time[1];
            numericUpDown199.Value = (decimal)CurDef.UnkGradient1Time[2];
            numericUpDown200.Value = (decimal)CurDef.UnkGradient1Time[3];
            numericUpDown201.Value = (decimal)CurDef.UnkGradient1Time[4];
            numericUpDown202.Value = (decimal)CurDef.UnkGradient1Time[5];
            numericUpDown203.Value = (decimal)CurDef.UnkGradient1Time[6];
            numericUpDown204.Value = (decimal)CurDef.UnkGradient1Time[7];
            numericUpDown205.Value = (decimal)CurDef.UnkGradient1Value[0];
            numericUpDown206.Value = (decimal)CurDef.UnkGradient1Value[1];
            numericUpDown207.Value = (decimal)CurDef.UnkGradient1Value[2];
            numericUpDown208.Value = (decimal)CurDef.UnkGradient1Value[3];
            numericUpDown209.Value = (decimal)CurDef.UnkGradient1Value[4];
            numericUpDown210.Value = (decimal)CurDef.UnkGradient1Value[5];
            numericUpDown211.Value = (decimal)CurDef.UnkGradient1Value[6];
            numericUpDown212.Value = (decimal)CurDef.UnkGradient1Value[7];
            numericUpDown213.Value = (decimal)CurDef.UnkGradient2Time[0];
            numericUpDown214.Value = (decimal)CurDef.UnkGradient2Time[1];
            numericUpDown215.Value = (decimal)CurDef.UnkGradient2Time[2];
            numericUpDown216.Value = (decimal)CurDef.UnkGradient2Time[3];
            numericUpDown217.Value = (decimal)CurDef.UnkGradient2Time[4];
            numericUpDown218.Value = (decimal)CurDef.UnkGradient2Time[5];
            numericUpDown219.Value = (decimal)CurDef.UnkGradient2Time[6];
            numericUpDown220.Value = (decimal)CurDef.UnkGradient2Time[7];
            numericUpDown221.Value = (decimal)CurDef.UnkGradient2Value[0];
            numericUpDown222.Value = (decimal)CurDef.UnkGradient2Value[1];
            numericUpDown223.Value = (decimal)CurDef.UnkGradient2Value[2];
            numericUpDown224.Value = (decimal)CurDef.UnkGradient2Value[3];
            numericUpDown225.Value = (decimal)CurDef.UnkGradient2Value[4];
            numericUpDown226.Value = (decimal)CurDef.UnkGradient2Value[5];
            numericUpDown227.Value = (decimal)CurDef.UnkGradient2Value[6];
            numericUpDown228.Value = (decimal)CurDef.UnkGradient2Value[7];

            UpdateGenSort();
            UpdateTexFiltering();

            ignore_value_change = false;

            this.ResumeDrawing();
        }

        void UpdateGenSort()
        {
            ignore_value_change = true;

            switch (CurDef.GSort)
            {
                default:
                    {
                        label36.Text = "Random Emit X";
                        label37.Text = "Random Emit Y";
                        label38.Text = "Random Emit Z";
                        label39.Text = "Random Start X";
                        label40.Text = "Random Start Y";
                        label41.Text = "Random Start Z";
                        numericUpDown34.Value = (decimal)CurDef.Random_Emit_X;
                        numericUpDown35.Value = (decimal)CurDef.Random_Emit_Y;
                        numericUpDown36.Value = (decimal)CurDef.Random_Emit_Z;
                        numericUpDown37.Value = (decimal)CurDef.Random_Start_X;
                        numericUpDown38.Value = (decimal)CurDef.Random_Start_Y;
                        numericUpDown39.Value = (decimal)CurDef.Random_Start_Z;
                        break;
                    }
                case ParticleData.ParticleSystemDefinition.GenSort.Radial:
                case ParticleData.ParticleSystemDefinition.GenSort.ImprovedRadial:
                case ParticleData.ParticleSystemDefinition.GenSort.StarRadial:
                    {
                        label36.Text = "Random Magnitude X";
                        label37.Text = "Random Rotation Y";
                        label38.Text = "Random Rotation Z";
                        label39.Text = "Base Magnitude";
                        label40.Text = "Base Rotation Y";
                        label41.Text = "Base Rotation Z";
                        numericUpDown34.Value = (decimal)CurDef.Random_Emit_X;
                        numericUpDown35.Value = (decimal)(CurDef.Random_Emit_Y / 65535f * 360f);
                        numericUpDown36.Value = (decimal)(CurDef.Random_Emit_Z / 65535f * 360f);
                        numericUpDown37.Value = (decimal)CurDef.Random_Start_X;
                        numericUpDown38.Value = (decimal)(CurDef.Random_Start_Y / 65535f * 360f);
                        numericUpDown39.Value = (decimal)(CurDef.Random_Start_Z / 65535f * 360f);
                        break;
                    }
                case ParticleData.ParticleSystemDefinition.GenSort.RadialRotor:
                    {
                        label36.Text = "Random Magnitude X";
                        label37.Text = "Step Rotation Y";
                        label38.Text = "Step Rotation Z";
                        label39.Text = "Base Magnitude";
                        label40.Text = "Base Rotation Y";
                        label41.Text = "Base Rotation Z";
                        numericUpDown34.Value = (decimal)CurDef.Random_Emit_X;
                        numericUpDown35.Value = (decimal)(CurDef.Random_Emit_Y / 65535f * 360f);
                        numericUpDown36.Value = (decimal)(CurDef.Random_Emit_Z / 65535f * 360f);
                        numericUpDown37.Value = (decimal)CurDef.Random_Start_X;
                        numericUpDown38.Value = (decimal)(CurDef.Random_Start_Y / 65535f * 360f);
                        numericUpDown39.Value = (decimal)(CurDef.Random_Start_Z / 65535f * 360f);
                        break;
                    }
            }

            ignore_value_change = false;
        }

        void UpdateTexFiltering()
        {
            ignore_value_change = true;

            switch (CurDef.TextureFilter)
            {
                default:
                    label84.Text = "Alpha:  Time / Value";
                    break;
                case ParticleData.ParticleSystemDefinition.TextureFiltering.Glass:
                    label84.Text = "Distortion: Time / Value";
                    break;
            }

            ignore_value_change = false;
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
            UpdateGenSort();
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
            UpdateTexFiltering();
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
            switch (CurDef.GSort)
            {
                default:
                    CurDef.Random_Emit_Y = (float)numericUpDown35.Value;
                    break;
                case ParticleData.ParticleSystemDefinition.GenSort.Radial:
                case ParticleData.ParticleSystemDefinition.GenSort.RadialRotor:
                case ParticleData.ParticleSystemDefinition.GenSort.StarRadial:
                case ParticleData.ParticleSystemDefinition.GenSort.ImprovedRadial:
                    CurDef.Random_Emit_Y = (float)numericUpDown35.Value / 360f * 65535f;
                    break;
            }
        }

        private void numericUpDown36_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            switch (CurDef.GSort)
            {
                default:
                    CurDef.Random_Emit_Z = (float)numericUpDown36.Value;
                    break;
                case ParticleData.ParticleSystemDefinition.GenSort.Radial:
                case ParticleData.ParticleSystemDefinition.GenSort.RadialRotor:
                case ParticleData.ParticleSystemDefinition.GenSort.StarRadial:
                case ParticleData.ParticleSystemDefinition.GenSort.ImprovedRadial:
                    CurDef.Random_Emit_Z = (float)numericUpDown36.Value / 360f * 65535f;
                    break;
            }
        }

        private void numericUpDown37_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.Random_Start_X = (float)numericUpDown37.Value;
        }

        private void numericUpDown38_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            switch (CurDef.GSort)
            {
                default:
                    CurDef.Random_Start_Y = (float)numericUpDown38.Value;
                    break;
                case ParticleData.ParticleSystemDefinition.GenSort.Radial:
                case ParticleData.ParticleSystemDefinition.GenSort.RadialRotor:
                case ParticleData.ParticleSystemDefinition.GenSort.StarRadial:
                case ParticleData.ParticleSystemDefinition.GenSort.ImprovedRadial:
                    CurDef.Random_Start_Y = (float)numericUpDown38.Value / 360f * 65535f;
                    break;
            }
        }

        private void numericUpDown39_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            switch (CurDef.GSort)
            {
                default:
                    CurDef.Random_Start_Z = (float)numericUpDown39.Value;
                    break;
                case ParticleData.ParticleSystemDefinition.GenSort.Radial:
                case ParticleData.ParticleSystemDefinition.GenSort.RadialRotor:
                case ParticleData.ParticleSystemDefinition.GenSort.StarRadial:
                case ParticleData.ParticleSystemDefinition.GenSort.ImprovedRadial:
                    CurDef.Random_Start_Z = (float)numericUpDown39.Value / 360f * 65535f;
                    break;
            }
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
            CurDef.ColorGradient[0].X = (float)numericUpDown60.Value;
        }

        private void numericUpDown61_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[0].Y = (float)numericUpDown61.Value;
        }

        private void numericUpDown62_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[0].Z = (float)numericUpDown62.Value;
        }

        private void numericUpDown63_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[0].W = (float)numericUpDown63.Value;
        }

        private void numericUpDown64_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[1].X = (float)numericUpDown64.Value;
        }

        private void numericUpDown65_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[1].Y = (float)numericUpDown65.Value;
        }

        private void numericUpDown66_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[1].Z = (float)numericUpDown66.Value;
        }

        private void numericUpDown67_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[1].W = (float)numericUpDown67.Value;
        }

        private void numericUpDown68_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[2].X = (float)numericUpDown68.Value;
        }

        private void numericUpDown69_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[2].Y = (float)numericUpDown69.Value;
        }

        private void numericUpDown70_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[2].Z = (float)numericUpDown70.Value;
        }

        private void numericUpDown71_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[2].W = (float)numericUpDown71.Value;
        }

        private void numericUpDown72_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[3].X = (float)numericUpDown72.Value;
        }

        private void numericUpDown73_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[3].Y = (float)numericUpDown73.Value;
        }

        private void numericUpDown74_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[3].Z = (float)numericUpDown74.Value;
        }

        private void numericUpDown75_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[3].W = (float)numericUpDown75.Value;
        }

        private void numericUpDown76_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[4].X = (float)numericUpDown76.Value;
        }

        private void numericUpDown77_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[4].Y = (float)numericUpDown77.Value;
        }

        private void numericUpDown78_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[4].Z = (float)numericUpDown78.Value;
        }

        private void numericUpDown79_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[4].W = (float)numericUpDown79.Value;
        }

        private void numericUpDown80_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[5].X = (float)numericUpDown80.Value;
        }

        private void numericUpDown81_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[5].Y = (float)numericUpDown81.Value;
        }

        private void numericUpDown82_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[5].Z = (float)numericUpDown82.Value;
        }

        private void numericUpDown83_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[5].W = (float)numericUpDown83.Value;
        }

        private void numericUpDown84_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[6].X = (float)numericUpDown84.Value;
        }

        private void numericUpDown85_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[6].Y = (float)numericUpDown85.Value;
        }

        private void numericUpDown86_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[6].Z = (float)numericUpDown86.Value;
        }

        private void numericUpDown87_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[6].W = (float)numericUpDown87.Value;
        }

        private void numericUpDown90_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[7].X = (float)numericUpDown90.Value;
        }

        private void numericUpDown91_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[7].Y = (float)numericUpDown91.Value;
        }

        private void numericUpDown92_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[7].Z = (float)numericUpDown92.Value;
        }

        private void numericUpDown93_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ColorGradient[7].W = (float)numericUpDown93.Value;
        }

        private void numericUpDown94_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.DistortionX = (float)numericUpDown94.Value;
        }

        private void numericUpDown95_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.DistortionY = (float)numericUpDown95.Value;
        }

        private void numericUpDown96_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.MinSize = (float)numericUpDown96.Value;
        }

        private void numericUpDown97_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.MaxSize = (float)numericUpDown97.Value;
        }

        private void numericUpDown98_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.MinRotation = (float)numericUpDown98.Value;
        }

        private void numericUpDown99_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.MaxRotation = (float)numericUpDown99.Value;
        }

        private void numericUpDown100_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            if (CurDef.TextureStartX >= 524288f)
                CurDef.TextureStartX = (float)numericUpDown100.Value + 524288f;
            else if (CurDef.TextureStartX >= 262144f)
                CurDef.TextureStartX = (float)numericUpDown100.Value + 262144f;
            else
                CurDef.TextureStartX = (float)numericUpDown100.Value;
        }

        private void numericUpDown101_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            if (CurDef.TextureStartY >= 524288f)
                CurDef.TextureStartY = (float)numericUpDown101.Value + 524288f;
            else if (CurDef.TextureStartY >= 262144f)
                CurDef.TextureStartY = (float)numericUpDown101.Value + 262144f;
            else
                CurDef.TextureStartY = (float)numericUpDown101.Value;
        }

        private void numericUpDown102_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            if (CurDef.TextureEndX >= 524288f)
                CurDef.TextureEndX = (float)numericUpDown102.Value + 524288f;
            else if (CurDef.TextureEndX >= 262144f)
                CurDef.TextureEndX = (float)numericUpDown102.Value + 262144f;
            else
                CurDef.TextureEndX = (float)numericUpDown102.Value;
        }

        private void numericUpDown103_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            if (CurDef.TextureEndY >= 524288f)
                CurDef.TextureEndY = (float)numericUpDown103.Value + 524288f;
            else if (CurDef.TextureEndY >= 262144f)
                CurDef.TextureEndY = (float)numericUpDown103.Value + 262144f;
            else
                CurDef.TextureEndY = (float)numericUpDown103.Value;
        }

        private void numericUpDown104_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.CollisionNumSpheres = (byte)numericUpDown104.Value;
        }

        private void numericUpDown105_ValueChanged(object sender, EventArgs e)
        {
            //if (ignore_value_change) return;
            //CurDef.DrawFlag = (ParticleData.ParticleSystemDefinition.DrawFlags)numericUpDown105.Value;
        }

        private void numericUpDown106_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ScaleFactor = (float)numericUpDown106.Value;
        }

        private void numericUpDown107_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.ParticleGhostsNum = (short)numericUpDown107.Value;
        }

        private void numericUpDown108_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.StarRadialPoints = (short)numericUpDown108.Value;
        }

        private void numericUpDown109_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.GhostSeparation = (float)numericUpDown109.Value;
        }

        private void numericUpDown110_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.StarRadiusRatio = (float)numericUpDown110.Value;
        }

        private void numericUpDown111_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.RampTime = (float)numericUpDown111.Value;
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

        private void numericUpDown117_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.AlphaGradientTime[0] = (float)numericUpDown117.Value;
        }

        private void numericUpDown118_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.AlphaGradientTime[1] = (float)numericUpDown118.Value;
        }

        private void numericUpDown119_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.AlphaGradientTime[2] = (float)numericUpDown119.Value;
        }

        private void numericUpDown120_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.AlphaGradientTime[3] = (float)numericUpDown120.Value;
        }

        private void numericUpDown121_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.AlphaGradientTime[4] = (float)numericUpDown121.Value;
        }

        private void numericUpDown122_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.AlphaGradientTime[5] = (float)numericUpDown122.Value;
        }

        private void numericUpDown123_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.AlphaGradientTime[6] = (float)numericUpDown123.Value;
        }

        private void numericUpDown124_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.AlphaGradientTime[7] = (float)numericUpDown124.Value;
        }

        private void numericUpDown125_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.AlphaGradientValue[0] = (float)numericUpDown125.Value;
        }

        private void numericUpDown126_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.AlphaGradientValue[1] = (float)numericUpDown126.Value;
        }

        private void numericUpDown127_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.AlphaGradientValue[2] = (float)numericUpDown127.Value;
        }

        private void numericUpDown128_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.AlphaGradientValue[3] = (float)numericUpDown128.Value;
        }

        private void numericUpDown129_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.AlphaGradientValue[4] = (float)numericUpDown129.Value;
        }

        private void numericUpDown130_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.AlphaGradientValue[5] = (float)numericUpDown130.Value;
        }

        private void numericUpDown131_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.AlphaGradientValue[6] = (float)numericUpDown131.Value;
        }

        private void numericUpDown132_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.AlphaGradientValue[7] = (float)numericUpDown132.Value;
        }

        private void numericUpDown133_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.RotationTime[0] = (float)numericUpDown133.Value;
        }

        private void numericUpDown134_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.RotationTime[1] = (float)numericUpDown134.Value;
        }

        private void numericUpDown135_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.RotationTime[2] = (float)numericUpDown135.Value;
        }

        private void numericUpDown136_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.RotationTime[3] = (float)numericUpDown136.Value;
        }

        private void numericUpDown137_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.RotationTime[4] = (float)numericUpDown137.Value;
        }

        private void numericUpDown138_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.RotationTime[5] = (float)numericUpDown138.Value;
        }

        private void numericUpDown139_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.RotationTime[6] = (float)numericUpDown139.Value;
        }

        private void numericUpDown140_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.RotationTime[7] = (float)numericUpDown140.Value;
        }

        private void numericUpDown141_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.RotationValue[0] = (float)numericUpDown141.Value / 360f * 65535f;
        }

        private void numericUpDown142_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.RotationValue[1] = (float)numericUpDown142.Value / 360f * 65535f;
        }

        private void numericUpDown143_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.RotationValue[2] = (float)numericUpDown143.Value / 360f * 65535f;
        }

        private void numericUpDown144_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.RotationValue[3] = (float)numericUpDown144.Value / 360f * 65535f;
        }

        private void numericUpDown145_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.RotationValue[4] = (float)numericUpDown145.Value / 360f * 65535f;
        }

        private void numericUpDown146_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.RotationValue[5] = (float)numericUpDown146.Value / 360f * 65535f;
        }

        private void numericUpDown147_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.RotationValue[6] = (float)numericUpDown147.Value / 360f * 65535f;
        }

        private void numericUpDown148_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.RotationValue[7] = (float)numericUpDown148.Value / 360f * 65535f;
        }

        private void numericUpDown149_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeWidthTime[0] = (float)numericUpDown149.Value;
        }

        private void numericUpDown150_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeWidthTime[1] = (float)numericUpDown150.Value;
        }

        private void numericUpDown151_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeWidthTime[2] = (float)numericUpDown151.Value;
        }

        private void numericUpDown152_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeWidthTime[3] = (float)numericUpDown152.Value;
        }

        private void numericUpDown153_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeWidthTime[4] = (float)numericUpDown153.Value;
        }

        private void numericUpDown154_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeWidthTime[5] = (float)numericUpDown154.Value;
        }

        private void numericUpDown155_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeWidthTime[6] = (float)numericUpDown155.Value;
        }

        private void numericUpDown156_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeWidthTime[7] = (float)numericUpDown156.Value;
        }

        private void numericUpDown157_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeWidthValue[0] = (float)numericUpDown157.Value;
        }

        private void numericUpDown158_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeWidthValue[1] = (float)numericUpDown158.Value;
        }

        private void numericUpDown159_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeWidthValue[2] = (float)numericUpDown159.Value;
        }

        private void numericUpDown160_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeWidthValue[3] = (float)numericUpDown160.Value;
        }

        private void numericUpDown161_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeWidthValue[4] = (float)numericUpDown161.Value;
        }

        private void numericUpDown162_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeWidthValue[5] = (float)numericUpDown162.Value;
        }

        private void numericUpDown163_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeWidthValue[6] = (float)numericUpDown163.Value;
        }

        private void numericUpDown164_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeWidthValue[7] = (float)numericUpDown164.Value;
        }

        private void numericUpDown165_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeHeightTime[0] = (float)numericUpDown165.Value;
        }

        private void numericUpDown166_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeHeightTime[1] = (float)numericUpDown166.Value;
        }

        private void numericUpDown167_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeHeightTime[2] = (float)numericUpDown167.Value;
        }

        private void numericUpDown168_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeHeightTime[3] = (float)numericUpDown168.Value;
        }

        private void numericUpDown169_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeHeightTime[4] = (float)numericUpDown169.Value;
        }

        private void numericUpDown170_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeHeightTime[5] = (float)numericUpDown170.Value;
        }

        private void numericUpDown171_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeHeightTime[6] = (float)numericUpDown171.Value;
        }

        private void numericUpDown172_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeHeightTime[7] = (float)numericUpDown172.Value;
        }

        private void numericUpDown173_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeHeightValue[0] = (float)numericUpDown173.Value;
        }

        private void numericUpDown174_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeHeightValue[1] = (float)numericUpDown174.Value;
        }

        private void numericUpDown175_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeHeightValue[2] = (float)numericUpDown175.Value;
        }

        private void numericUpDown176_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeHeightValue[3] = (float)numericUpDown176.Value;
        }

        private void numericUpDown177_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeHeightValue[4] = (float)numericUpDown177.Value;
        }

        private void numericUpDown178_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeHeightValue[5] = (float)numericUpDown178.Value;
        }

        private void numericUpDown179_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeHeightValue[6] = (float)numericUpDown179.Value;
        }

        private void numericUpDown180_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.SizeHeightValue[7] = (float)numericUpDown180.Value;
        }

        private void numericUpDown181_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.CollisionTime[0] = (float)numericUpDown181.Value;
        }

        private void numericUpDown182_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.CollisionTime[1] = (float)numericUpDown182.Value;
        }

        private void numericUpDown183_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.CollisionTime[2] = (float)numericUpDown183.Value;
        }

        private void numericUpDown184_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.CollisionTime[3] = (float)numericUpDown184.Value;
        }

        private void numericUpDown185_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.CollisionTime[4] = (float)numericUpDown185.Value;
        }

        private void numericUpDown186_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.CollisionTime[5] = (float)numericUpDown186.Value;
        }

        private void numericUpDown187_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.CollisionTime[6] = (float)numericUpDown187.Value;
        }

        private void numericUpDown188_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.CollisionTime[7] = (float)numericUpDown188.Value;
        }

        private void numericUpDown189_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.CollisionValue[0] = (float)numericUpDown189.Value;
        }

        private void numericUpDown190_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.CollisionValue[1] = (float)numericUpDown190.Value;
        }

        private void numericUpDown191_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.CollisionValue[2] = (float)numericUpDown191.Value;
        }

        private void numericUpDown192_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.CollisionValue[3] = (float)numericUpDown192.Value;
        }

        private void numericUpDown193_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.CollisionValue[4] = (float)numericUpDown193.Value;
        }

        private void numericUpDown194_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.CollisionValue[5] = (float)numericUpDown194.Value;
        }

        private void numericUpDown195_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.CollisionValue[6] = (float)numericUpDown195.Value;
        }

        private void numericUpDown196_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.CollisionValue[7] = (float)numericUpDown196.Value;
        }

        private void numericUpDown197_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient1Time[0] = (float)numericUpDown197.Value;
        }

        private void numericUpDown198_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient1Time[1] = (float)numericUpDown198.Value;
        }

        private void numericUpDown199_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient1Time[2] = (float)numericUpDown199.Value;
        }

        private void numericUpDown200_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient1Time[3] = (float)numericUpDown200.Value;
        }

        private void numericUpDown201_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient1Time[4] = (float)numericUpDown201.Value;
        }

        private void numericUpDown202_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient1Time[5] = (float)numericUpDown202.Value;
        }

        private void numericUpDown203_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient1Time[6] = (float)numericUpDown203.Value;
        }

        private void numericUpDown204_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient1Time[7] = (float)numericUpDown204.Value;
        }

        private void numericUpDown205_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient1Value[0] = (float)numericUpDown205.Value;
        }

        private void numericUpDown206_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient1Value[1] = (float)numericUpDown206.Value;
        }

        private void numericUpDown207_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient1Value[2] = (float)numericUpDown207.Value;
        }

        private void numericUpDown208_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient1Value[3] = (float)numericUpDown208.Value;
        }

        private void numericUpDown209_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient1Value[4] = (float)numericUpDown209.Value;
        }

        private void numericUpDown210_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient1Value[5] = (float)numericUpDown210.Value;
        }

        private void numericUpDown211_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient1Value[6] = (float)numericUpDown211.Value;
        }

        private void numericUpDown212_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient1Value[7] = (float)numericUpDown212.Value;
        }

        private void numericUpDown213_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient2Time[0] = (float)numericUpDown213.Value;
        }

        private void numericUpDown214_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient2Time[1] = (float)numericUpDown214.Value;
        }

        private void numericUpDown215_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient2Time[2] = (float)numericUpDown215.Value;
        }

        private void numericUpDown216_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient2Time[3] = (float)numericUpDown216.Value;
        }

        private void numericUpDown217_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient2Time[4] = (float)numericUpDown217.Value;
        }

        private void numericUpDown218_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient2Time[5] = (float)numericUpDown218.Value;
        }

        private void numericUpDown219_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient2Time[6] = (float)numericUpDown219.Value;
        }

        private void numericUpDown220_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient2Time[7] = (float)numericUpDown220.Value;
        }

        private void numericUpDown221_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient2Value[0] = (float)numericUpDown221.Value;
        }

        private void numericUpDown222_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient2Value[1] = (float)numericUpDown222.Value;
        }

        private void numericUpDown223_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient2Value[2] = (float)numericUpDown223.Value;
        }

        private void numericUpDown224_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient2Value[3] = (float)numericUpDown224.Value;
        }

        private void numericUpDown225_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient2Value[4] = (float)numericUpDown225.Value;
        }

        private void numericUpDown226_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient2Value[5] = (float)numericUpDown226.Value;
        }

        private void numericUpDown227_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient2Value[6] = (float)numericUpDown227.Value;
        }

        private void numericUpDown228_ValueChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.UnkGradient2Value[7] = (float)numericUpDown228.Value;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            CurDef.DrawFlag = (ParticleData.ParticleSystemDefinition.DrawFlags)comboBox3.SelectedIndex;
        }
    }
}
