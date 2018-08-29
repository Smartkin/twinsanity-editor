using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Collections;
using System;

namespace TwinsaityEditor
{
    [global::Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    partial class TriggerEditor : System.Windows.Forms.Form
    {

        // Форма переопределяет dispose для очистки списка компонентов.
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                    components.Dispose();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Является обязательной для конструктора форм Windows Forms
        private System.ComponentModel.IContainer components;

        // Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
        // Для ее изменения используйте конструктор форм Windows Form.  
        // Не изменяйте ее в редакторе исходного кода.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TriggerEditor));
            this.TriggerTree = new System.Windows.Forms.TreeView();
            this.Label1 = new System.Windows.Forms.Label();
            this.TriggerID = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.Flags = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.SomeNumber = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.Instances = new System.Windows.Forms.ListBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.Value1 = new System.Windows.Forms.TextBox();
            this.Value2 = new System.Windows.Forms.TextBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.Value3 = new System.Windows.Forms.TextBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.Value4 = new System.Windows.Forms.TextBox();
            this.Label8 = new System.Windows.Forms.Label();
            this.NumberValue = new System.Windows.Forms.TextBox();
            this.Label9 = new System.Windows.Forms.Label();
            this.Label10 = new System.Windows.Forms.Label();
            this.Label11 = new System.Windows.Forms.Label();
            this.PosX = new System.Windows.Forms.TextBox();
            this.PosY = new System.Windows.Forms.TextBox();
            this.Label12 = new System.Windows.Forms.Label();
            this.PosZ = new System.Windows.Forms.TextBox();
            this.Label13 = new System.Windows.Forms.Label();
            this.SizeL = new System.Windows.Forms.TextBox();
            this.Label14 = new System.Windows.Forms.Label();
            this.SizeH = new System.Windows.Forms.TextBox();
            this.Label15 = new System.Windows.Forms.Label();
            this.SizeW = new System.Windows.Forms.TextBox();
            this.Label16 = new System.Windows.Forms.Label();
            this.Label17 = new System.Windows.Forms.Label();
            this.VecZ = new System.Windows.Forms.TextBox();
            this.Label18 = new System.Windows.Forms.Label();
            this.VecY = new System.Windows.Forms.TextBox();
            this.Label19 = new System.Windows.Forms.Label();
            this.VecX = new System.Windows.Forms.TextBox();
            this.Label20 = new System.Windows.Forms.Label();
            this.Label21 = new System.Windows.Forms.Label();
            this.Apply = new System.Windows.Forms.Button();
            this.Revert = new System.Windows.Forms.Button();
            this.Button1 = new System.Windows.Forms.Button();
            this.Button2 = new System.Windows.Forms.Button();
            this.Button3 = new System.Windows.Forms.Button();
            this.InstanceVal = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TriggerTree
            // 
            this.TriggerTree.Location = new System.Drawing.Point(13, 13);
            this.TriggerTree.Name = "TriggerTree";
            this.TriggerTree.Size = new System.Drawing.Size(127, 262);
            this.TriggerTree.TabIndex = 0;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(147, 13);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(21, 13);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "ID:";
            // 
            // TriggerID
            // 
            this.TriggerID.Location = new System.Drawing.Point(174, 10);
            this.TriggerID.Name = "TriggerID";
            this.TriggerID.Size = new System.Drawing.Size(62, 20);
            this.TriggerID.TabIndex = 2;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(243, 12);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(35, 13);
            this.Label2.TabIndex = 3;
            this.Label2.Text = "Flags:";
            // 
            // Flags
            // 
            this.Flags.Location = new System.Drawing.Point(284, 10);
            this.Flags.Name = "Flags";
            this.Flags.Size = new System.Drawing.Size(69, 20);
            this.Flags.TabIndex = 4;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(359, 13);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(47, 13);
            this.Label3.TabIndex = 5;
            this.Label3.Text = "Number:";
            // 
            // SomeNumber
            // 
            this.SomeNumber.Location = new System.Drawing.Point(413, 10);
            this.SomeNumber.Name = "SomeNumber";
            this.SomeNumber.Size = new System.Drawing.Size(75, 20);
            this.SomeNumber.TabIndex = 6;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(147, 33);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(153, 13);
            this.Label4.TabIndex = 7;
            this.Label4.Text = "Instances (Object is undefined)";
            // 
            // Instances
            // 
            this.Instances.FormattingEnabled = true;
            this.Instances.Location = new System.Drawing.Point(150, 49);
            this.Instances.Name = "Instances";
            this.Instances.Size = new System.Drawing.Size(91, 95);
            this.Instances.TabIndex = 8;
            this.Instances.SelectedIndexChanged += new System.EventHandler(this.Instances_SelectedIndexChanged);
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(247, 49);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(70, 13);
            this.Label5.TabIndex = 9;
            this.Label5.Text = "SomeValue1:";
            // 
            // Value1
            // 
            this.Value1.Location = new System.Drawing.Point(323, 46);
            this.Value1.Name = "Value1";
            this.Value1.Size = new System.Drawing.Size(100, 20);
            this.Value1.TabIndex = 10;
            // 
            // Value2
            // 
            this.Value2.Location = new System.Drawing.Point(323, 72);
            this.Value2.Name = "Value2";
            this.Value2.Size = new System.Drawing.Size(100, 20);
            this.Value2.TabIndex = 12;
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(247, 75);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(70, 13);
            this.Label6.TabIndex = 11;
            this.Label6.Text = "SomeValue2:";
            // 
            // Value3
            // 
            this.Value3.Location = new System.Drawing.Point(323, 98);
            this.Value3.Name = "Value3";
            this.Value3.Size = new System.Drawing.Size(100, 20);
            this.Value3.TabIndex = 14;
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(247, 101);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(70, 13);
            this.Label7.TabIndex = 13;
            this.Label7.Text = "SomeValue3:";
            // 
            // Value4
            // 
            this.Value4.Location = new System.Drawing.Point(323, 124);
            this.Value4.Name = "Value4";
            this.Value4.Size = new System.Drawing.Size(100, 20);
            this.Value4.TabIndex = 16;
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Location = new System.Drawing.Point(247, 127);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(70, 13);
            this.Label8.TabIndex = 15;
            this.Label8.Text = "SomeValue4:";
            // 
            // NumberValue
            // 
            this.NumberValue.Location = new System.Drawing.Point(227, 188);
            this.NumberValue.Name = "NumberValue";
            this.NumberValue.Size = new System.Drawing.Size(100, 20);
            this.NumberValue.TabIndex = 18;
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Location = new System.Drawing.Point(147, 191);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(74, 13);
            this.Label9.TabIndex = 17;
            this.Label9.Text = "SomeNumber:";
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Location = new System.Drawing.Point(150, 208);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(44, 13);
            this.Label10.TabIndex = 19;
            this.Label10.Text = "Position";
            // 
            // Label11
            // 
            this.Label11.AutoSize = true;
            this.Label11.Location = new System.Drawing.Point(150, 225);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(17, 13);
            this.Label11.TabIndex = 20;
            this.Label11.Text = "X:";
            // 
            // PosX
            // 
            this.PosX.Location = new System.Drawing.Point(173, 222);
            this.PosX.Name = "PosX";
            this.PosX.Size = new System.Drawing.Size(79, 20);
            this.PosX.TabIndex = 21;
            // 
            // PosY
            // 
            this.PosY.Location = new System.Drawing.Point(173, 248);
            this.PosY.Name = "PosY";
            this.PosY.Size = new System.Drawing.Size(79, 20);
            this.PosY.TabIndex = 23;
            // 
            // Label12
            // 
            this.Label12.AutoSize = true;
            this.Label12.Location = new System.Drawing.Point(150, 251);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(17, 13);
            this.Label12.TabIndex = 22;
            this.Label12.Text = "Y:";
            // 
            // PosZ
            // 
            this.PosZ.Location = new System.Drawing.Point(173, 274);
            this.PosZ.Name = "PosZ";
            this.PosZ.Size = new System.Drawing.Size(79, 20);
            this.PosZ.TabIndex = 25;
            // 
            // Label13
            // 
            this.Label13.AutoSize = true;
            this.Label13.Location = new System.Drawing.Point(150, 277);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(17, 13);
            this.Label13.TabIndex = 24;
            this.Label13.Text = "Z:";
            // 
            // SizeL
            // 
            this.SizeL.Location = new System.Drawing.Point(286, 277);
            this.SizeL.Name = "SizeL";
            this.SizeL.Size = new System.Drawing.Size(79, 20);
            this.SizeL.TabIndex = 32;
            // 
            // Label14
            // 
            this.Label14.AutoSize = true;
            this.Label14.Location = new System.Drawing.Point(263, 280);
            this.Label14.Name = "Label14";
            this.Label14.Size = new System.Drawing.Size(16, 13);
            this.Label14.TabIndex = 31;
            this.Label14.Text = "L:";
            // 
            // SizeH
            // 
            this.SizeH.Location = new System.Drawing.Point(286, 251);
            this.SizeH.Name = "SizeH";
            this.SizeH.Size = new System.Drawing.Size(79, 20);
            this.SizeH.TabIndex = 30;
            // 
            // Label15
            // 
            this.Label15.AutoSize = true;
            this.Label15.Location = new System.Drawing.Point(263, 254);
            this.Label15.Name = "Label15";
            this.Label15.Size = new System.Drawing.Size(18, 13);
            this.Label15.TabIndex = 29;
            this.Label15.Text = "H:";
            // 
            // SizeW
            // 
            this.SizeW.Location = new System.Drawing.Point(286, 225);
            this.SizeW.Name = "SizeW";
            this.SizeW.Size = new System.Drawing.Size(79, 20);
            this.SizeW.TabIndex = 28;
            // 
            // Label16
            // 
            this.Label16.AutoSize = true;
            this.Label16.Location = new System.Drawing.Point(263, 228);
            this.Label16.Name = "Label16";
            this.Label16.Size = new System.Drawing.Size(21, 13);
            this.Label16.TabIndex = 27;
            this.Label16.Text = "W:";
            // 
            // Label17
            // 
            this.Label17.AutoSize = true;
            this.Label17.Location = new System.Drawing.Point(263, 211);
            this.Label17.Name = "Label17";
            this.Label17.Size = new System.Drawing.Size(27, 13);
            this.Label17.TabIndex = 26;
            this.Label17.Text = "Size";
            // 
            // VecZ
            // 
            this.VecZ.Location = new System.Drawing.Point(401, 277);
            this.VecZ.Name = "VecZ";
            this.VecZ.Size = new System.Drawing.Size(79, 20);
            this.VecZ.TabIndex = 39;
            // 
            // Label18
            // 
            this.Label18.AutoSize = true;
            this.Label18.Location = new System.Drawing.Point(378, 280);
            this.Label18.Name = "Label18";
            this.Label18.Size = new System.Drawing.Size(17, 13);
            this.Label18.TabIndex = 38;
            this.Label18.Text = "Z:";
            // 
            // VecY
            // 
            this.VecY.Location = new System.Drawing.Point(401, 251);
            this.VecY.Name = "VecY";
            this.VecY.Size = new System.Drawing.Size(79, 20);
            this.VecY.TabIndex = 37;
            // 
            // Label19
            // 
            this.Label19.AutoSize = true;
            this.Label19.Location = new System.Drawing.Point(378, 254);
            this.Label19.Name = "Label19";
            this.Label19.Size = new System.Drawing.Size(17, 13);
            this.Label19.TabIndex = 36;
            this.Label19.Text = "Y:";
            // 
            // VecX
            // 
            this.VecX.Location = new System.Drawing.Point(401, 225);
            this.VecX.Name = "VecX";
            this.VecX.Size = new System.Drawing.Size(79, 20);
            this.VecX.TabIndex = 35;
            // 
            // Label20
            // 
            this.Label20.AutoSize = true;
            this.Label20.Location = new System.Drawing.Point(378, 228);
            this.Label20.Name = "Label20";
            this.Label20.Size = new System.Drawing.Size(17, 13);
            this.Label20.TabIndex = 34;
            this.Label20.Text = "X:";
            // 
            // Label21
            // 
            this.Label21.AutoSize = true;
            this.Label21.Location = new System.Drawing.Point(378, 211);
            this.Label21.Name = "Label21";
            this.Label21.Size = new System.Drawing.Size(44, 13);
            this.Label21.TabIndex = 33;
            this.Label21.Text = "Vector3";
            // 
            // Apply
            // 
            this.Apply.Location = new System.Drawing.Point(13, 282);
            this.Apply.Name = "Apply";
            this.Apply.Size = new System.Drawing.Size(127, 23);
            this.Apply.TabIndex = 40;
            this.Apply.Text = "Apply";
            this.Apply.UseVisualStyleBackColor = true;
            this.Apply.Click += new System.EventHandler(this.Apply_Click);
            // 
            // Revert
            // 
            this.Revert.Location = new System.Drawing.Point(13, 311);
            this.Revert.Name = "Revert";
            this.Revert.Size = new System.Drawing.Size(127, 23);
            this.Revert.TabIndex = 41;
            this.Revert.Text = "Revert";
            this.Revert.UseVisualStyleBackColor = true;
            this.Revert.Click += new System.EventHandler(this.Revert_Click);
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(150, 150);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(35, 23);
            this.Button1.TabIndex = 42;
            this.Button1.Text = "Add";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Button2
            // 
            this.Button2.Location = new System.Drawing.Point(191, 150);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(35, 23);
            this.Button2.TabIndex = 43;
            this.Button2.Text = "Edit";
            this.Button2.UseVisualStyleBackColor = true;
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // Button3
            // 
            this.Button3.Location = new System.Drawing.Point(232, 150);
            this.Button3.Name = "Button3";
            this.Button3.Size = new System.Drawing.Size(35, 23);
            this.Button3.TabIndex = 44;
            this.Button3.Text = "Delete";
            this.Button3.UseVisualStyleBackColor = true;
            this.Button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // InstanceVal
            // 
            this.InstanceVal.Location = new System.Drawing.Point(273, 152);
            this.InstanceVal.Name = "InstanceVal";
            this.InstanceVal.Size = new System.Drawing.Size(100, 20);
            this.InstanceVal.TabIndex = 45;
            // 
            // TriggerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 342);
            this.Controls.Add(this.InstanceVal);
            this.Controls.Add(this.Button3);
            this.Controls.Add(this.Button2);
            this.Controls.Add(this.Button1);
            this.Controls.Add(this.Revert);
            this.Controls.Add(this.Apply);
            this.Controls.Add(this.VecZ);
            this.Controls.Add(this.Label18);
            this.Controls.Add(this.VecY);
            this.Controls.Add(this.Label19);
            this.Controls.Add(this.VecX);
            this.Controls.Add(this.Label20);
            this.Controls.Add(this.Label21);
            this.Controls.Add(this.SizeL);
            this.Controls.Add(this.Label14);
            this.Controls.Add(this.SizeH);
            this.Controls.Add(this.Label15);
            this.Controls.Add(this.SizeW);
            this.Controls.Add(this.Label16);
            this.Controls.Add(this.Label17);
            this.Controls.Add(this.PosZ);
            this.Controls.Add(this.Label13);
            this.Controls.Add(this.PosY);
            this.Controls.Add(this.Label12);
            this.Controls.Add(this.PosX);
            this.Controls.Add(this.Label11);
            this.Controls.Add(this.Label10);
            this.Controls.Add(this.NumberValue);
            this.Controls.Add(this.Label9);
            this.Controls.Add(this.Value4);
            this.Controls.Add(this.Label8);
            this.Controls.Add(this.Value3);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.Value2);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.Value1);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.Instances);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.SomeNumber);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Flags);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.TriggerID);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.TriggerTree);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TriggerEditor";
            this.Text = "Trigger Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public System.Windows.Forms.TreeView TriggerTree;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.TextBox TriggerID;
        private System.Windows.Forms.Label Label2;
        private System.Windows.Forms.TextBox Flags;
        private System.Windows.Forms.Label Label3;
        private System.Windows.Forms.TextBox SomeNumber;
        private System.Windows.Forms.Label Label4;
        private System.Windows.Forms.ListBox Instances;
        private System.Windows.Forms.Label Label5;
        private System.Windows.Forms.TextBox Value1;
        private System.Windows.Forms.TextBox Value2;
        private System.Windows.Forms.Label Label6;
        private System.Windows.Forms.TextBox Value3;
        private System.Windows.Forms.Label Label7;
        private System.Windows.Forms.TextBox Value4;
        private System.Windows.Forms.Label Label8;
        private System.Windows.Forms.TextBox NumberValue;
        private System.Windows.Forms.Label Label9;
        private System.Windows.Forms.Label Label10;
        private System.Windows.Forms.Label Label11;
        private System.Windows.Forms.TextBox PosX;
        private System.Windows.Forms.TextBox PosY;
        private System.Windows.Forms.Label Label12;
        private System.Windows.Forms.TextBox PosZ;
        private System.Windows.Forms.Label Label13;
        private System.Windows.Forms.TextBox SizeL;
        private System.Windows.Forms.Label Label14;
        private System.Windows.Forms.TextBox SizeH;
        private System.Windows.Forms.Label Label15;
        private System.Windows.Forms.TextBox SizeW;
        private System.Windows.Forms.Label Label16;
        private System.Windows.Forms.Label Label17;
        private System.Windows.Forms.TextBox VecZ;
        private System.Windows.Forms.Label Label18;
        private System.Windows.Forms.TextBox VecY;
        private System.Windows.Forms.Label Label19;
        private System.Windows.Forms.TextBox VecX;
        private System.Windows.Forms.Label Label20;
        private System.Windows.Forms.Label Label21;
        private System.Windows.Forms.Button Apply;
        private System.Windows.Forms.Button Revert;
        private System.Windows.Forms.Button Button1;
        private System.Windows.Forms.Button Button2;
        private System.Windows.Forms.Button Button3;
        private System.Windows.Forms.TextBox InstanceVal;
    }
}
