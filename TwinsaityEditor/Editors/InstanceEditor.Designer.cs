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
    partial class InstanceEditor : System.Windows.Forms.Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstanceEditor));
            this.ObjectID = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.InstanceTree = new System.Windows.Forms.TreeView();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.InstanceX = new System.Windows.Forms.TextBox();
            this.InstanceY = new System.Windows.Forms.TextBox();
            this.InstanceZ = new System.Windows.Forms.TextBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.RotationX = new System.Windows.Forms.TrackBar();
            this.RotationY = new System.Windows.Forms.TrackBar();
            this.RotationZ = new System.Windows.Forms.TrackBar();
            this.Label9 = new System.Windows.Forms.Label();
            this.Flags = new System.Windows.Forms.TextBox();
            this.Label10 = new System.Windows.Forms.Label();
            this.Satan = new System.Windows.Forms.TextBox();
            this.Some = new System.Windows.Forms.ListBox();
            this.Floats = new System.Windows.Forms.ListBox();
            this.Integers = new System.Windows.Forms.ListBox();
            this.Apply = new System.Windows.Forms.Button();
            this.Button1 = new System.Windows.Forms.Button();
            this.Label11 = new System.Windows.Forms.Label();
            this.InstanceID = new System.Windows.Forms.TextBox();
            this.Button2 = new System.Windows.Forms.Button();
            this.Button3 = new System.Windows.Forms.Button();
            this.Button4 = new System.Windows.Forms.Button();
            this.SomeVal = new System.Windows.Forms.TextBox();
            this.Button5 = new System.Windows.Forms.Button();
            this.Button6 = new System.Windows.Forms.Button();
            this.Button7 = new System.Windows.Forms.Button();
            this.FloatVal = new System.Windows.Forms.TextBox();
            this.IntegerVal = new System.Windows.Forms.TextBox();
            this.Button8 = new System.Windows.Forms.Button();
            this.Button9 = new System.Windows.Forms.Button();
            this.Button10 = new System.Windows.Forms.Button();
            this.InstanceVal = new System.Windows.Forms.TextBox();
            this.Button11 = new System.Windows.Forms.Button();
            this.Button12 = new System.Windows.Forms.Button();
            this.Button13 = new System.Windows.Forms.Button();
            this.PathVal = new System.Windows.Forms.TextBox();
            this.Button14 = new System.Windows.Forms.Button();
            this.Button15 = new System.Windows.Forms.Button();
            this.Button16 = new System.Windows.Forms.Button();
            this.PositionVal = new System.Windows.Forms.TextBox();
            this.Button17 = new System.Windows.Forms.Button();
            this.Button18 = new System.Windows.Forms.Button();
            this.Button19 = new System.Windows.Forms.Button();
            this.Instance = new System.Windows.Forms.ListBox();
            this.Path = new System.Windows.Forms.ListBox();
            this.Position = new System.Windows.Forms.ListBox();
            this.Label12 = new System.Windows.Forms.Label();
            this.Label13 = new System.Windows.Forms.Label();
            this.Label14 = new System.Windows.Forms.Label();
            this.Label15 = new System.Windows.Forms.Label();
            this.Label16 = new System.Windows.Forms.Label();
            this.Label17 = new System.Windows.Forms.Label();
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.RotZText = new System.Windows.Forms.TextBox();
            this.RotYText = new System.Windows.Forms.TextBox();
            this.RotXText = new System.Windows.Forms.TextBox();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.RotationX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationZ)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.TabPage1.SuspendLayout();
            this.TabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ObjectID
            // 
            this.ObjectID.Location = new System.Drawing.Point(86, 35);
            this.ObjectID.Name = "ObjectID";
            this.ObjectID.Size = new System.Drawing.Size(100, 20);
            this.ObjectID.TabIndex = 0;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(7, 38);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(52, 13);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "ObjectID:";
            // 
            // InstanceTree
            // 
            this.InstanceTree.Location = new System.Drawing.Point(13, 9);
            this.InstanceTree.Name = "InstanceTree";
            this.InstanceTree.Size = new System.Drawing.Size(121, 241);
            this.InstanceTree.TabIndex = 2;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(11, 138);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(17, 13);
            this.Label3.TabIndex = 4;
            this.Label3.Text = "X:";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(11, 182);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(17, 13);
            this.Label4.TabIndex = 5;
            this.Label4.Text = "Y:";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(11, 237);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(17, 13);
            this.Label5.TabIndex = 6;
            this.Label5.Text = "Z:";
            // 
            // InstanceX
            // 
            this.InstanceX.Location = new System.Drawing.Point(34, 135);
            this.InstanceX.Name = "InstanceX";
            this.InstanceX.Size = new System.Drawing.Size(100, 20);
            this.InstanceX.TabIndex = 7;
            // 
            // InstanceY
            // 
            this.InstanceY.Location = new System.Drawing.Point(34, 179);
            this.InstanceY.Name = "InstanceY";
            this.InstanceY.Size = new System.Drawing.Size(100, 20);
            this.InstanceY.TabIndex = 8;
            // 
            // InstanceZ
            // 
            this.InstanceZ.Location = new System.Drawing.Point(34, 234);
            this.InstanceZ.Name = "InstanceZ";
            this.InstanceZ.Size = new System.Drawing.Size(100, 20);
            this.InstanceZ.TabIndex = 9;
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(288, 135);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(57, 13);
            this.Label6.TabIndex = 10;
            this.Label6.Text = "RotationX:";
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(288, 186);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(57, 13);
            this.Label7.TabIndex = 11;
            this.Label7.Text = "RotationY:";
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Location = new System.Drawing.Point(288, 234);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(57, 13);
            this.Label8.TabIndex = 12;
            this.Label8.Text = "RotationZ:";
            // 
            // RotationX
            // 
            this.RotationX.BackColor = System.Drawing.Color.White;
            this.RotationX.Location = new System.Drawing.Point(140, 127);
            this.RotationX.Maximum = 65535;
            this.RotationX.Name = "RotationX";
            this.RotationX.Size = new System.Drawing.Size(154, 45);
            this.RotationX.TabIndex = 13;
            this.RotationX.Scroll += new System.EventHandler(this.RotationX_Scroll);
            // 
            // RotationY
            // 
            this.RotationY.BackColor = System.Drawing.Color.White;
            this.RotationY.Location = new System.Drawing.Point(140, 174);
            this.RotationY.Maximum = 65535;
            this.RotationY.Name = "RotationY";
            this.RotationY.Size = new System.Drawing.Size(154, 45);
            this.RotationY.TabIndex = 14;
            this.RotationY.Scroll += new System.EventHandler(this.RotationY_Scroll);
            // 
            // RotationZ
            // 
            this.RotationZ.BackColor = System.Drawing.Color.White;
            this.RotationZ.Location = new System.Drawing.Point(140, 225);
            this.RotationZ.Maximum = 65535;
            this.RotationZ.Name = "RotationZ";
            this.RotationZ.Size = new System.Drawing.Size(154, 45);
            this.RotationZ.TabIndex = 15;
            this.RotationZ.Scroll += new System.EventHandler(this.RotationZ_Scroll);
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Location = new System.Drawing.Point(7, 69);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(35, 13);
            this.Label9.TabIndex = 16;
            this.Label9.Text = "Flags:";
            // 
            // Flags
            // 
            this.Flags.Location = new System.Drawing.Point(86, 66);
            this.Flags.Name = "Flags";
            this.Flags.Size = new System.Drawing.Size(100, 20);
            this.Flags.TabIndex = 17;
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Location = new System.Drawing.Point(7, 95);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(76, 13);
            this.Label10.TabIndex = 18;
            this.Label10.Text = "SatanVariable:";
            // 
            // Satan
            // 
            this.Satan.Location = new System.Drawing.Point(86, 92);
            this.Satan.Name = "Satan";
            this.Satan.Size = new System.Drawing.Size(100, 20);
            this.Satan.TabIndex = 19;
            // 
            // Some
            // 
            this.Some.FormattingEnabled = true;
            this.Some.Location = new System.Drawing.Point(9, 23);
            this.Some.Name = "Some";
            this.Some.Size = new System.Drawing.Size(80, 56);
            this.Some.TabIndex = 20;
            this.Some.SelectedIndexChanged += new System.EventHandler(this.Some_SelectedIndexChanged);
            // 
            // Floats
            // 
            this.Floats.FormattingEnabled = true;
            this.Floats.Location = new System.Drawing.Point(6, 101);
            this.Floats.Name = "Floats";
            this.Floats.Size = new System.Drawing.Size(80, 56);
            this.Floats.TabIndex = 21;
            this.Floats.SelectedIndexChanged += new System.EventHandler(this.Floats_SelectedIndexChanged);
            // 
            // Integers
            // 
            this.Integers.FormattingEnabled = true;
            this.Integers.Location = new System.Drawing.Point(6, 178);
            this.Integers.Name = "Integers";
            this.Integers.Size = new System.Drawing.Size(80, 56);
            this.Integers.TabIndex = 22;
            this.Integers.SelectedIndexChanged += new System.EventHandler(this.Integers_SelectedIndexChanged);
            // 
            // Apply
            // 
            this.Apply.Location = new System.Drawing.Point(13, 257);
            this.Apply.Name = "Apply";
            this.Apply.Size = new System.Drawing.Size(121, 23);
            this.Apply.TabIndex = 23;
            this.Apply.Text = "Apply";
            this.Apply.UseVisualStyleBackColor = true;
            this.Apply.Click += new System.EventHandler(this.Apply_Click);
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(13, 286);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(121, 23);
            this.Button1.TabIndex = 24;
            this.Button1.Text = "Revert";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Label11
            // 
            this.Label11.AutoSize = true;
            this.Label11.Location = new System.Drawing.Point(7, 15);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(21, 13);
            this.Label11.TabIndex = 25;
            this.Label11.Text = "ID:";
            // 
            // InstanceID
            // 
            this.InstanceID.Location = new System.Drawing.Point(86, 12);
            this.InstanceID.Name = "InstanceID";
            this.InstanceID.Size = new System.Drawing.Size(100, 20);
            this.InstanceID.TabIndex = 26;
            // 
            // Button2
            // 
            this.Button2.Location = new System.Drawing.Point(95, 23);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(34, 23);
            this.Button2.TabIndex = 27;
            this.Button2.Text = "Add";
            this.Button2.UseVisualStyleBackColor = true;
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // Button3
            // 
            this.Button3.Location = new System.Drawing.Point(135, 23);
            this.Button3.Name = "Button3";
            this.Button3.Size = new System.Drawing.Size(34, 23);
            this.Button3.TabIndex = 28;
            this.Button3.Text = "Edit";
            this.Button3.UseVisualStyleBackColor = true;
            this.Button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // Button4
            // 
            this.Button4.Location = new System.Drawing.Point(175, 23);
            this.Button4.Name = "Button4";
            this.Button4.Size = new System.Drawing.Size(34, 23);
            this.Button4.TabIndex = 29;
            this.Button4.Text = "Delete";
            this.Button4.UseVisualStyleBackColor = true;
            this.Button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // SomeVal
            // 
            this.SomeVal.Location = new System.Drawing.Point(95, 59);
            this.SomeVal.Name = "SomeVal";
            this.SomeVal.Size = new System.Drawing.Size(114, 20);
            this.SomeVal.TabIndex = 30;
            // 
            // Button5
            // 
            this.Button5.Location = new System.Drawing.Point(92, 101);
            this.Button5.Name = "Button5";
            this.Button5.Size = new System.Drawing.Size(34, 23);
            this.Button5.TabIndex = 31;
            this.Button5.Text = "Add";
            this.Button5.UseVisualStyleBackColor = true;
            this.Button5.Click += new System.EventHandler(this.Button5_Click);
            // 
            // Button6
            // 
            this.Button6.Location = new System.Drawing.Point(133, 101);
            this.Button6.Name = "Button6";
            this.Button6.Size = new System.Drawing.Size(34, 23);
            this.Button6.TabIndex = 32;
            this.Button6.Text = "Edit";
            this.Button6.UseVisualStyleBackColor = true;
            this.Button6.Click += new System.EventHandler(this.Button6_Click);
            // 
            // Button7
            // 
            this.Button7.Location = new System.Drawing.Point(173, 101);
            this.Button7.Name = "Button7";
            this.Button7.Size = new System.Drawing.Size(34, 23);
            this.Button7.TabIndex = 33;
            this.Button7.Text = "Delete";
            this.Button7.UseVisualStyleBackColor = true;
            this.Button7.Click += new System.EventHandler(this.Button7_Click);
            // 
            // FloatVal
            // 
            this.FloatVal.Location = new System.Drawing.Point(93, 137);
            this.FloatVal.Name = "FloatVal";
            this.FloatVal.Size = new System.Drawing.Size(114, 20);
            this.FloatVal.TabIndex = 34;
            // 
            // IntegerVal
            // 
            this.IntegerVal.Location = new System.Drawing.Point(93, 214);
            this.IntegerVal.Name = "IntegerVal";
            this.IntegerVal.Size = new System.Drawing.Size(114, 20);
            this.IntegerVal.TabIndex = 38;
            // 
            // Button8
            // 
            this.Button8.Location = new System.Drawing.Point(173, 178);
            this.Button8.Name = "Button8";
            this.Button8.Size = new System.Drawing.Size(34, 23);
            this.Button8.TabIndex = 37;
            this.Button8.Text = "Delete";
            this.Button8.UseVisualStyleBackColor = true;
            this.Button8.Click += new System.EventHandler(this.Button8_Click);
            // 
            // Button9
            // 
            this.Button9.Location = new System.Drawing.Point(133, 178);
            this.Button9.Name = "Button9";
            this.Button9.Size = new System.Drawing.Size(34, 23);
            this.Button9.TabIndex = 36;
            this.Button9.Text = "Edit";
            this.Button9.UseVisualStyleBackColor = true;
            this.Button9.Click += new System.EventHandler(this.Button9_Click);
            // 
            // Button10
            // 
            this.Button10.Location = new System.Drawing.Point(92, 178);
            this.Button10.Name = "Button10";
            this.Button10.Size = new System.Drawing.Size(34, 23);
            this.Button10.TabIndex = 35;
            this.Button10.Text = "Add";
            this.Button10.UseVisualStyleBackColor = true;
            this.Button10.Click += new System.EventHandler(this.Button10_Click);
            // 
            // InstanceVal
            // 
            this.InstanceVal.Location = new System.Drawing.Point(311, 214);
            this.InstanceVal.Name = "InstanceVal";
            this.InstanceVal.Size = new System.Drawing.Size(114, 20);
            this.InstanceVal.TabIndex = 53;
            // 
            // Button11
            // 
            this.Button11.Location = new System.Drawing.Point(391, 178);
            this.Button11.Name = "Button11";
            this.Button11.Size = new System.Drawing.Size(34, 23);
            this.Button11.TabIndex = 52;
            this.Button11.Text = "Delete";
            this.Button11.UseVisualStyleBackColor = true;
            this.Button11.Click += new System.EventHandler(this.Button11_Click);
            // 
            // Button12
            // 
            this.Button12.Location = new System.Drawing.Point(351, 178);
            this.Button12.Name = "Button12";
            this.Button12.Size = new System.Drawing.Size(34, 23);
            this.Button12.TabIndex = 51;
            this.Button12.Text = "Edit";
            this.Button12.UseVisualStyleBackColor = true;
            this.Button12.Click += new System.EventHandler(this.Button12_Click);
            // 
            // Button13
            // 
            this.Button13.Location = new System.Drawing.Point(310, 178);
            this.Button13.Name = "Button13";
            this.Button13.Size = new System.Drawing.Size(34, 23);
            this.Button13.TabIndex = 50;
            this.Button13.Text = "Add";
            this.Button13.UseVisualStyleBackColor = true;
            this.Button13.Click += new System.EventHandler(this.Button13_Click);
            // 
            // PathVal
            // 
            this.PathVal.Location = new System.Drawing.Point(311, 137);
            this.PathVal.Name = "PathVal";
            this.PathVal.Size = new System.Drawing.Size(114, 20);
            this.PathVal.TabIndex = 49;
            // 
            // Button14
            // 
            this.Button14.Location = new System.Drawing.Point(391, 101);
            this.Button14.Name = "Button14";
            this.Button14.Size = new System.Drawing.Size(34, 23);
            this.Button14.TabIndex = 48;
            this.Button14.Text = "Delete";
            this.Button14.UseVisualStyleBackColor = true;
            this.Button14.Click += new System.EventHandler(this.Button14_Click);
            // 
            // Button15
            // 
            this.Button15.Location = new System.Drawing.Point(351, 101);
            this.Button15.Name = "Button15";
            this.Button15.Size = new System.Drawing.Size(34, 23);
            this.Button15.TabIndex = 47;
            this.Button15.Text = "Edit";
            this.Button15.UseVisualStyleBackColor = true;
            this.Button15.Click += new System.EventHandler(this.Button15_Click);
            // 
            // Button16
            // 
            this.Button16.Location = new System.Drawing.Point(310, 101);
            this.Button16.Name = "Button16";
            this.Button16.Size = new System.Drawing.Size(34, 23);
            this.Button16.TabIndex = 46;
            this.Button16.Text = "Add";
            this.Button16.UseVisualStyleBackColor = true;
            this.Button16.Click += new System.EventHandler(this.Button16_Click);
            // 
            // PositionVal
            // 
            this.PositionVal.Location = new System.Drawing.Point(311, 59);
            this.PositionVal.Name = "PositionVal";
            this.PositionVal.Size = new System.Drawing.Size(114, 20);
            this.PositionVal.TabIndex = 45;
            // 
            // Button17
            // 
            this.Button17.Location = new System.Drawing.Point(391, 23);
            this.Button17.Name = "Button17";
            this.Button17.Size = new System.Drawing.Size(34, 23);
            this.Button17.TabIndex = 44;
            this.Button17.Text = "Delete";
            this.Button17.UseVisualStyleBackColor = true;
            this.Button17.Click += new System.EventHandler(this.Button17_Click);
            // 
            // Button18
            // 
            this.Button18.Location = new System.Drawing.Point(351, 23);
            this.Button18.Name = "Button18";
            this.Button18.Size = new System.Drawing.Size(34, 23);
            this.Button18.TabIndex = 43;
            this.Button18.Text = "Edit";
            this.Button18.UseVisualStyleBackColor = true;
            this.Button18.Click += new System.EventHandler(this.Button18_Click);
            // 
            // Button19
            // 
            this.Button19.Location = new System.Drawing.Point(311, 23);
            this.Button19.Name = "Button19";
            this.Button19.Size = new System.Drawing.Size(34, 23);
            this.Button19.TabIndex = 42;
            this.Button19.Text = "Add";
            this.Button19.UseVisualStyleBackColor = true;
            this.Button19.Click += new System.EventHandler(this.Button19_Click);
            // 
            // Instance
            // 
            this.Instance.FormattingEnabled = true;
            this.Instance.Location = new System.Drawing.Point(224, 178);
            this.Instance.Name = "Instance";
            this.Instance.Size = new System.Drawing.Size(80, 56);
            this.Instance.TabIndex = 41;
            this.Instance.SelectedIndexChanged += new System.EventHandler(this.Instance_SelectedIndexChanged);
            // 
            // Path
            // 
            this.Path.FormattingEnabled = true;
            this.Path.Location = new System.Drawing.Point(224, 101);
            this.Path.Name = "Path";
            this.Path.Size = new System.Drawing.Size(80, 56);
            this.Path.TabIndex = 40;
            this.Path.SelectedIndexChanged += new System.EventHandler(this.Path_SelectedIndexChanged);
            // 
            // Position
            // 
            this.Position.FormattingEnabled = true;
            this.Position.Location = new System.Drawing.Point(224, 23);
            this.Position.Name = "Position";
            this.Position.Size = new System.Drawing.Size(80, 56);
            this.Position.TabIndex = 39;
            this.Position.SelectedIndexChanged += new System.EventHandler(this.Position_SelectedIndexChanged);
            // 
            // Label12
            // 
            this.Label12.AutoSize = true;
            this.Label12.Location = new System.Drawing.Point(8, 7);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(57, 13);
            this.Label12.TabIndex = 54;
            this.Label12.Text = "Something";
            // 
            // Label13
            // 
            this.Label13.AutoSize = true;
            this.Label13.Location = new System.Drawing.Point(5, 85);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(30, 13);
            this.Label13.TabIndex = 55;
            this.Label13.Text = "Float";
            // 
            // Label14
            // 
            this.Label14.AutoSize = true;
            this.Label14.Location = new System.Drawing.Point(6, 162);
            this.Label14.Name = "Label14";
            this.Label14.Size = new System.Drawing.Size(40, 13);
            this.Label14.TabIndex = 56;
            this.Label14.Text = "Integer";
            // 
            // Label15
            // 
            this.Label15.AutoSize = true;
            this.Label15.Location = new System.Drawing.Point(224, 7);
            this.Label15.Name = "Label15";
            this.Label15.Size = new System.Drawing.Size(44, 13);
            this.Label15.TabIndex = 57;
            this.Label15.Text = "Position";
            // 
            // Label16
            // 
            this.Label16.AutoSize = true;
            this.Label16.Location = new System.Drawing.Point(224, 84);
            this.Label16.Name = "Label16";
            this.Label16.Size = new System.Drawing.Size(29, 13);
            this.Label16.TabIndex = 58;
            this.Label16.Text = "Path";
            // 
            // Label17
            // 
            this.Label17.AutoSize = true;
            this.Label17.Location = new System.Drawing.Point(224, 165);
            this.Label17.Name = "Label17";
            this.Label17.Size = new System.Drawing.Size(48, 13);
            this.Label17.TabIndex = 59;
            this.Label17.Text = "Instance";
            // 
            // TabControl1
            // 
            this.TabControl1.Controls.Add(this.TabPage1);
            this.TabControl1.Controls.Add(this.TabPage2);
            this.TabControl1.Location = new System.Drawing.Point(140, 9);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(469, 300);
            this.TabControl1.TabIndex = 60;
            // 
            // TabPage1
            // 
            this.TabPage1.Controls.Add(this.RotZText);
            this.TabPage1.Controls.Add(this.RotYText);
            this.TabPage1.Controls.Add(this.RotXText);
            this.TabPage1.Controls.Add(this.InstanceID);
            this.TabPage1.Controls.Add(this.ObjectID);
            this.TabPage1.Controls.Add(this.Label1);
            this.TabPage1.Controls.Add(this.Label3);
            this.TabPage1.Controls.Add(this.Label4);
            this.TabPage1.Controls.Add(this.Label5);
            this.TabPage1.Controls.Add(this.InstanceX);
            this.TabPage1.Controls.Add(this.InstanceY);
            this.TabPage1.Controls.Add(this.InstanceZ);
            this.TabPage1.Controls.Add(this.Label6);
            this.TabPage1.Controls.Add(this.Label7);
            this.TabPage1.Controls.Add(this.Label8);
            this.TabPage1.Controls.Add(this.RotationX);
            this.TabPage1.Controls.Add(this.RotationY);
            this.TabPage1.Controls.Add(this.RotationZ);
            this.TabPage1.Controls.Add(this.Label9);
            this.TabPage1.Controls.Add(this.Flags);
            this.TabPage1.Controls.Add(this.Label10);
            this.TabPage1.Controls.Add(this.Satan);
            this.TabPage1.Controls.Add(this.Label11);
            this.TabPage1.Location = new System.Drawing.Point(4, 22);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage1.Size = new System.Drawing.Size(461, 274);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "General";
            this.TabPage1.UseVisualStyleBackColor = true;
            // 
            // RotZText
            // 
            this.RotZText.Location = new System.Drawing.Point(395, 233);
            this.RotZText.Name = "RotZText";
            this.RotZText.Size = new System.Drawing.Size(60, 20);
            this.RotZText.TabIndex = 29;
            this.RotZText.TextChanged += new System.EventHandler(this.RotZText_TextChanged);
            // 
            // RotYText
            // 
            this.RotYText.Location = new System.Drawing.Point(395, 186);
            this.RotYText.Name = "RotYText";
            this.RotYText.Size = new System.Drawing.Size(60, 20);
            this.RotYText.TabIndex = 28;
            this.RotYText.TextChanged += new System.EventHandler(this.RotYText_TextChanged);
            // 
            // RotXText
            // 
            this.RotXText.Location = new System.Drawing.Point(395, 136);
            this.RotXText.Name = "RotXText";
            this.RotXText.Size = new System.Drawing.Size(60, 20);
            this.RotXText.TabIndex = 27;
            this.RotXText.TextChanged += new System.EventHandler(this.RotXText_TextChanged);
            // 
            // TabPage2
            // 
            this.TabPage2.Controls.Add(this.Position);
            this.TabPage2.Controls.Add(this.Label14);
            this.TabPage2.Controls.Add(this.Label17);
            this.TabPage2.Controls.Add(this.Label13);
            this.TabPage2.Controls.Add(this.Path);
            this.TabPage2.Controls.Add(this.Label12);
            this.TabPage2.Controls.Add(this.Label16);
            this.TabPage2.Controls.Add(this.IntegerVal);
            this.TabPage2.Controls.Add(this.Instance);
            this.TabPage2.Controls.Add(this.Button8);
            this.TabPage2.Controls.Add(this.Label15);
            this.TabPage2.Controls.Add(this.Button9);
            this.TabPage2.Controls.Add(this.Button19);
            this.TabPage2.Controls.Add(this.Button10);
            this.TabPage2.Controls.Add(this.Button18);
            this.TabPage2.Controls.Add(this.FloatVal);
            this.TabPage2.Controls.Add(this.Button17);
            this.TabPage2.Controls.Add(this.Button7);
            this.TabPage2.Controls.Add(this.PositionVal);
            this.TabPage2.Controls.Add(this.Button6);
            this.TabPage2.Controls.Add(this.InstanceVal);
            this.TabPage2.Controls.Add(this.Button5);
            this.TabPage2.Controls.Add(this.Button16);
            this.TabPage2.Controls.Add(this.SomeVal);
            this.TabPage2.Controls.Add(this.Button11);
            this.TabPage2.Controls.Add(this.Button4);
            this.TabPage2.Controls.Add(this.Button15);
            this.TabPage2.Controls.Add(this.Button3);
            this.TabPage2.Controls.Add(this.Button12);
            this.TabPage2.Controls.Add(this.Button2);
            this.TabPage2.Controls.Add(this.Button14);
            this.TabPage2.Controls.Add(this.Integers);
            this.TabPage2.Controls.Add(this.Button13);
            this.TabPage2.Controls.Add(this.Floats);
            this.TabPage2.Controls.Add(this.PathVal);
            this.TabPage2.Controls.Add(this.Some);
            this.TabPage2.Location = new System.Drawing.Point(4, 22);
            this.TabPage2.Name = "TabPage2";
            this.TabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage2.Size = new System.Drawing.Size(461, 274);
            this.TabPage2.TabIndex = 1;
            this.TabPage2.Text = "Collections";
            this.TabPage2.UseVisualStyleBackColor = true;
            // 
            // InstanceEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 322);
            this.Controls.Add(this.TabControl1);
            this.Controls.Add(this.InstanceTree);
            this.Controls.Add(this.Apply);
            this.Controls.Add(this.Button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InstanceEditor";
            this.Text = "Instance Editor";
            this.Load += new System.EventHandler(this.InstanceEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.RotationX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationZ)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.TabPage1.ResumeLayout(false);
            this.TabPage1.PerformLayout();
            this.TabPage2.ResumeLayout(false);
            this.TabPage2.PerformLayout();
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.TextBox ObjectID;
        private System.Windows.Forms.Label Label1;
        public System.Windows.Forms.TreeView InstanceTree;
        private System.Windows.Forms.Label Label3;
        private System.Windows.Forms.Label Label4;
        private System.Windows.Forms.Label Label5;
        private System.Windows.Forms.TextBox InstanceX;
        private System.Windows.Forms.TextBox InstanceY;
        private System.Windows.Forms.TextBox InstanceZ;
        private System.Windows.Forms.Label Label6;
        private System.Windows.Forms.Label Label7;
        private System.Windows.Forms.Label Label8;
        private System.Windows.Forms.TrackBar RotationX;
        private System.Windows.Forms.TrackBar RotationY;
        private System.Windows.Forms.TrackBar RotationZ;
        private System.Windows.Forms.Label Label9;
        private System.Windows.Forms.TextBox Flags;
        private System.Windows.Forms.Label Label10;
        private System.Windows.Forms.TextBox Satan;
        private System.Windows.Forms.ListBox Some;
        private System.Windows.Forms.ListBox Floats;
        private System.Windows.Forms.ListBox Integers;
        private System.Windows.Forms.Button Apply;
        private System.Windows.Forms.Button Button1;
        private System.Windows.Forms.Label Label11;
        private System.Windows.Forms.TextBox InstanceID;
        private System.Windows.Forms.Button Button2;
        private System.Windows.Forms.Button Button3;
        private System.Windows.Forms.Button Button4;
        private System.Windows.Forms.TextBox SomeVal;
        private System.Windows.Forms.Button Button5;
        private System.Windows.Forms.Button Button6;
        private System.Windows.Forms.Button Button7;
        private System.Windows.Forms.TextBox FloatVal;
        private System.Windows.Forms.TextBox IntegerVal;
        private System.Windows.Forms.Button Button8;
        private System.Windows.Forms.Button Button9;
        private System.Windows.Forms.Button Button10;
        private System.Windows.Forms.TextBox InstanceVal;
        private System.Windows.Forms.Button Button11;
        private System.Windows.Forms.Button Button12;
        private System.Windows.Forms.Button Button13;
        private System.Windows.Forms.TextBox PathVal;
        private System.Windows.Forms.Button Button14;
        private System.Windows.Forms.Button Button15;
        private System.Windows.Forms.Button Button16;
        private System.Windows.Forms.TextBox PositionVal;
        private System.Windows.Forms.Button Button17;
        private System.Windows.Forms.Button Button18;
        private System.Windows.Forms.Button Button19;
        private System.Windows.Forms.ListBox Instance;
        private System.Windows.Forms.ListBox Path;
        private System.Windows.Forms.ListBox Position;
        private System.Windows.Forms.Label Label12;
        private System.Windows.Forms.Label Label13;
        private System.Windows.Forms.Label Label14;
        private System.Windows.Forms.Label Label15;
        private System.Windows.Forms.Label Label16;
        private System.Windows.Forms.Label Label17;
        private System.Windows.Forms.TabControl TabControl1;
        private System.Windows.Forms.TabPage TabPage1;
        private System.Windows.Forms.TabPage TabPage2;
        private System.Windows.Forms.TextBox RotZText;
        private System.Windows.Forms.TextBox RotYText;
        private System.Windows.Forms.TextBox RotXText;
    }
}
