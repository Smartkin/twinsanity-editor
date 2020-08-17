namespace TwinsaityEditor
{
    partial class ObjectEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.objectList = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.animationIdSource = new System.Windows.Forms.TextBox();
            this.animationsSet = new System.Windows.Forms.Button();
            this.animationsAdd = new System.Windows.Forms.Button();
            this.animationsDown = new System.Windows.Forms.Button();
            this.animationsRemove = new System.Windows.Forms.Button();
            this.animationsUp = new System.Windows.Forms.Button();
            this.animationsListBox = new System.Windows.Forms.ListBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.ogiIdSource = new System.Windows.Forms.TextBox();
            this.ogiSet = new System.Windows.Forms.Button();
            this.ogiAdd = new System.Windows.Forms.Button();
            this.ogiDown = new System.Windows.Forms.Button();
            this.ogiRemove = new System.Windows.Forms.Button();
            this.ogiUp = new System.Windows.Forms.Button();
            this.ogiListBox = new System.Windows.Forms.ListBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.soundIdSource = new System.Windows.Forms.TextBox();
            this.soundsSet = new System.Windows.Forms.Button();
            this.soundsAdd = new System.Windows.Forms.Button();
            this.soundsDown = new System.Windows.Forms.Button();
            this.soundsRemove = new System.Windows.Forms.Button();
            this.soundsUp = new System.Windows.Forms.Button();
            this.soundsListBox = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.objectIdSource = new System.Windows.Forms.TextBox();
            this.objectsSet = new System.Windows.Forms.Button();
            this.objectsAdd = new System.Windows.Forms.Button();
            this.objectsDown = new System.Windows.Forms.Button();
            this.objectsRemove = new System.Windows.Forms.Button();
            this.objectsUp = new System.Windows.Forms.Button();
            this.objectsListBox = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.scrtiptIdSource = new System.Windows.Forms.TextBox();
            this.scriptsSet = new System.Windows.Forms.Button();
            this.scriptsAdd = new System.Windows.Forms.Button();
            this.scriptsDown = new System.Windows.Forms.Button();
            this.scriptsRemove = new System.Windows.Forms.Button();
            this.scriptsUp = new System.Windows.Forms.Button();
            this.scriptsListBox = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.nameSource = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.paramSource = new System.Windows.Forms.TextBox();
            this.paramsSet = new System.Windows.Forms.Button();
            this.paramsAdd = new System.Windows.Forms.Button();
            this.paramsDown = new System.Windows.Forms.Button();
            this.paramsRemove = new System.Windows.Forms.Button();
            this.paramsUp = new System.Windows.Forms.Button();
            this.paramsListBox = new System.Windows.Forms.ListBox();
            this.flagSource = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // objectList
            // 
            this.objectList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectList.FormattingEnabled = true;
            this.objectList.Location = new System.Drawing.Point(0, 0);
            this.objectList.Name = "objectList";
            this.objectList.Size = new System.Drawing.Size(1022, 619);
            this.objectList.TabIndex = 0;
            this.objectList.SelectedIndexChanged += new System.EventHandler(this.objectList_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Location = new System.Drawing.Point(281, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(741, 619);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Object Properties";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 57);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(735, 559);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Controls.Add(this.groupBox7);
            this.tabPage1.Controls.Add(this.groupBox5);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(727, 533);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Tab1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.animationIdSource);
            this.groupBox6.Controls.Add(this.animationsSet);
            this.groupBox6.Controls.Add(this.animationsAdd);
            this.groupBox6.Controls.Add(this.animationsDown);
            this.groupBox6.Controls.Add(this.animationsRemove);
            this.groupBox6.Controls.Add(this.animationsUp);
            this.groupBox6.Controls.Add(this.animationsListBox);
            this.groupBox6.Location = new System.Drawing.Point(483, 213);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(232, 195);
            this.groupBox6.TabIndex = 8;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Animation List";
            // 
            // animationIdSource
            // 
            this.animationIdSource.Location = new System.Drawing.Point(78, 148);
            this.animationIdSource.Name = "animationIdSource";
            this.animationIdSource.Size = new System.Drawing.Size(148, 20);
            this.animationIdSource.TabIndex = 6;
            // 
            // animationsSet
            // 
            this.animationsSet.Location = new System.Drawing.Point(10, 162);
            this.animationsSet.Name = "animationsSet";
            this.animationsSet.Size = new System.Drawing.Size(55, 23);
            this.animationsSet.TabIndex = 5;
            this.animationsSet.Text = "Set";
            this.animationsSet.UseVisualStyleBackColor = true;
            // 
            // animationsAdd
            // 
            this.animationsAdd.Location = new System.Drawing.Point(10, 133);
            this.animationsAdd.Name = "animationsAdd";
            this.animationsAdd.Size = new System.Drawing.Size(55, 23);
            this.animationsAdd.TabIndex = 4;
            this.animationsAdd.Text = "Add";
            this.animationsAdd.UseVisualStyleBackColor = true;
            // 
            // animationsDown
            // 
            this.animationsDown.Location = new System.Drawing.Point(10, 104);
            this.animationsDown.Name = "animationsDown";
            this.animationsDown.Size = new System.Drawing.Size(55, 23);
            this.animationsDown.TabIndex = 3;
            this.animationsDown.Text = "Down";
            this.animationsDown.UseVisualStyleBackColor = true;
            // 
            // animationsRemove
            // 
            this.animationsRemove.Location = new System.Drawing.Point(10, 60);
            this.animationsRemove.Name = "animationsRemove";
            this.animationsRemove.Size = new System.Drawing.Size(55, 23);
            this.animationsRemove.TabIndex = 2;
            this.animationsRemove.Text = "Remove";
            this.animationsRemove.UseVisualStyleBackColor = true;
            // 
            // animationsUp
            // 
            this.animationsUp.Location = new System.Drawing.Point(10, 19);
            this.animationsUp.Name = "animationsUp";
            this.animationsUp.Size = new System.Drawing.Size(55, 23);
            this.animationsUp.TabIndex = 1;
            this.animationsUp.Text = "Up";
            this.animationsUp.UseVisualStyleBackColor = true;
            // 
            // animationsListBox
            // 
            this.animationsListBox.FormattingEnabled = true;
            this.animationsListBox.Location = new System.Drawing.Point(78, 19);
            this.animationsListBox.Name = "animationsListBox";
            this.animationsListBox.Size = new System.Drawing.Size(148, 108);
            this.animationsListBox.TabIndex = 0;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.ogiIdSource);
            this.groupBox7.Controls.Add(this.ogiSet);
            this.groupBox7.Controls.Add(this.ogiAdd);
            this.groupBox7.Controls.Add(this.ogiDown);
            this.groupBox7.Controls.Add(this.ogiRemove);
            this.groupBox7.Controls.Add(this.ogiUp);
            this.groupBox7.Controls.Add(this.ogiListBox);
            this.groupBox7.Location = new System.Drawing.Point(245, 213);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(232, 195);
            this.groupBox7.TabIndex = 9;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "OGI List";
            // 
            // ogiIdSource
            // 
            this.ogiIdSource.Location = new System.Drawing.Point(78, 148);
            this.ogiIdSource.Name = "ogiIdSource";
            this.ogiIdSource.Size = new System.Drawing.Size(148, 20);
            this.ogiIdSource.TabIndex = 6;
            // 
            // ogiSet
            // 
            this.ogiSet.Location = new System.Drawing.Point(10, 162);
            this.ogiSet.Name = "ogiSet";
            this.ogiSet.Size = new System.Drawing.Size(55, 23);
            this.ogiSet.TabIndex = 5;
            this.ogiSet.Text = "Set";
            this.ogiSet.UseVisualStyleBackColor = true;
            // 
            // ogiAdd
            // 
            this.ogiAdd.Location = new System.Drawing.Point(10, 133);
            this.ogiAdd.Name = "ogiAdd";
            this.ogiAdd.Size = new System.Drawing.Size(55, 23);
            this.ogiAdd.TabIndex = 4;
            this.ogiAdd.Text = "Add";
            this.ogiAdd.UseVisualStyleBackColor = true;
            // 
            // ogiDown
            // 
            this.ogiDown.Location = new System.Drawing.Point(10, 104);
            this.ogiDown.Name = "ogiDown";
            this.ogiDown.Size = new System.Drawing.Size(55, 23);
            this.ogiDown.TabIndex = 3;
            this.ogiDown.Text = "Down";
            this.ogiDown.UseVisualStyleBackColor = true;
            // 
            // ogiRemove
            // 
            this.ogiRemove.Location = new System.Drawing.Point(10, 60);
            this.ogiRemove.Name = "ogiRemove";
            this.ogiRemove.Size = new System.Drawing.Size(55, 23);
            this.ogiRemove.TabIndex = 2;
            this.ogiRemove.Text = "Remove";
            this.ogiRemove.UseVisualStyleBackColor = true;
            // 
            // ogiUp
            // 
            this.ogiUp.Location = new System.Drawing.Point(10, 19);
            this.ogiUp.Name = "ogiUp";
            this.ogiUp.Size = new System.Drawing.Size(55, 23);
            this.ogiUp.TabIndex = 1;
            this.ogiUp.Text = "Up";
            this.ogiUp.UseVisualStyleBackColor = true;
            // 
            // ogiListBox
            // 
            this.ogiListBox.FormattingEnabled = true;
            this.ogiListBox.Location = new System.Drawing.Point(78, 19);
            this.ogiListBox.Name = "ogiListBox";
            this.ogiListBox.Size = new System.Drawing.Size(148, 108);
            this.ogiListBox.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.soundIdSource);
            this.groupBox5.Controls.Add(this.soundsSet);
            this.groupBox5.Controls.Add(this.soundsAdd);
            this.groupBox5.Controls.Add(this.soundsDown);
            this.groupBox5.Controls.Add(this.soundsRemove);
            this.groupBox5.Controls.Add(this.soundsUp);
            this.groupBox5.Controls.Add(this.soundsListBox);
            this.groupBox5.Location = new System.Drawing.Point(483, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(232, 195);
            this.groupBox5.TabIndex = 7;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Sound List";
            // 
            // soundIdSource
            // 
            this.soundIdSource.Location = new System.Drawing.Point(78, 148);
            this.soundIdSource.Name = "soundIdSource";
            this.soundIdSource.Size = new System.Drawing.Size(148, 20);
            this.soundIdSource.TabIndex = 6;
            // 
            // soundsSet
            // 
            this.soundsSet.Location = new System.Drawing.Point(10, 162);
            this.soundsSet.Name = "soundsSet";
            this.soundsSet.Size = new System.Drawing.Size(55, 23);
            this.soundsSet.TabIndex = 5;
            this.soundsSet.Text = "Set";
            this.soundsSet.UseVisualStyleBackColor = true;
            // 
            // soundsAdd
            // 
            this.soundsAdd.Location = new System.Drawing.Point(10, 133);
            this.soundsAdd.Name = "soundsAdd";
            this.soundsAdd.Size = new System.Drawing.Size(55, 23);
            this.soundsAdd.TabIndex = 4;
            this.soundsAdd.Text = "Add";
            this.soundsAdd.UseVisualStyleBackColor = true;
            // 
            // soundsDown
            // 
            this.soundsDown.Location = new System.Drawing.Point(10, 104);
            this.soundsDown.Name = "soundsDown";
            this.soundsDown.Size = new System.Drawing.Size(55, 23);
            this.soundsDown.TabIndex = 3;
            this.soundsDown.Text = "Down";
            this.soundsDown.UseVisualStyleBackColor = true;
            // 
            // soundsRemove
            // 
            this.soundsRemove.Location = new System.Drawing.Point(10, 60);
            this.soundsRemove.Name = "soundsRemove";
            this.soundsRemove.Size = new System.Drawing.Size(55, 23);
            this.soundsRemove.TabIndex = 2;
            this.soundsRemove.Text = "Remove";
            this.soundsRemove.UseVisualStyleBackColor = true;
            // 
            // soundsUp
            // 
            this.soundsUp.Location = new System.Drawing.Point(10, 19);
            this.soundsUp.Name = "soundsUp";
            this.soundsUp.Size = new System.Drawing.Size(55, 23);
            this.soundsUp.TabIndex = 1;
            this.soundsUp.Text = "Up";
            this.soundsUp.UseVisualStyleBackColor = true;
            // 
            // soundsListBox
            // 
            this.soundsListBox.FormattingEnabled = true;
            this.soundsListBox.Location = new System.Drawing.Point(78, 19);
            this.soundsListBox.Name = "soundsListBox";
            this.soundsListBox.Size = new System.Drawing.Size(148, 108);
            this.soundsListBox.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.objectIdSource);
            this.groupBox4.Controls.Add(this.objectsSet);
            this.groupBox4.Controls.Add(this.objectsAdd);
            this.groupBox4.Controls.Add(this.objectsDown);
            this.groupBox4.Controls.Add(this.objectsRemove);
            this.groupBox4.Controls.Add(this.objectsUp);
            this.groupBox4.Controls.Add(this.objectsListBox);
            this.groupBox4.Location = new System.Drawing.Point(245, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(232, 195);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Object List";
            // 
            // objectIdSource
            // 
            this.objectIdSource.Location = new System.Drawing.Point(78, 148);
            this.objectIdSource.Name = "objectIdSource";
            this.objectIdSource.Size = new System.Drawing.Size(148, 20);
            this.objectIdSource.TabIndex = 6;
            // 
            // objectsSet
            // 
            this.objectsSet.Location = new System.Drawing.Point(10, 162);
            this.objectsSet.Name = "objectsSet";
            this.objectsSet.Size = new System.Drawing.Size(55, 23);
            this.objectsSet.TabIndex = 5;
            this.objectsSet.Text = "Set";
            this.objectsSet.UseVisualStyleBackColor = true;
            // 
            // objectsAdd
            // 
            this.objectsAdd.Location = new System.Drawing.Point(10, 133);
            this.objectsAdd.Name = "objectsAdd";
            this.objectsAdd.Size = new System.Drawing.Size(55, 23);
            this.objectsAdd.TabIndex = 4;
            this.objectsAdd.Text = "Add";
            this.objectsAdd.UseVisualStyleBackColor = true;
            // 
            // objectsDown
            // 
            this.objectsDown.Location = new System.Drawing.Point(10, 104);
            this.objectsDown.Name = "objectsDown";
            this.objectsDown.Size = new System.Drawing.Size(55, 23);
            this.objectsDown.TabIndex = 3;
            this.objectsDown.Text = "Down";
            this.objectsDown.UseVisualStyleBackColor = true;
            // 
            // objectsRemove
            // 
            this.objectsRemove.Location = new System.Drawing.Point(10, 60);
            this.objectsRemove.Name = "objectsRemove";
            this.objectsRemove.Size = new System.Drawing.Size(55, 23);
            this.objectsRemove.TabIndex = 2;
            this.objectsRemove.Text = "Remove";
            this.objectsRemove.UseVisualStyleBackColor = true;
            // 
            // objectsUp
            // 
            this.objectsUp.Location = new System.Drawing.Point(10, 19);
            this.objectsUp.Name = "objectsUp";
            this.objectsUp.Size = new System.Drawing.Size(55, 23);
            this.objectsUp.TabIndex = 1;
            this.objectsUp.Text = "Up";
            this.objectsUp.UseVisualStyleBackColor = true;
            // 
            // objectsListBox
            // 
            this.objectsListBox.FormattingEnabled = true;
            this.objectsListBox.Location = new System.Drawing.Point(78, 19);
            this.objectsListBox.Name = "objectsListBox";
            this.objectsListBox.Size = new System.Drawing.Size(148, 108);
            this.objectsListBox.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.scrtiptIdSource);
            this.groupBox3.Controls.Add(this.scriptsSet);
            this.groupBox3.Controls.Add(this.scriptsAdd);
            this.groupBox3.Controls.Add(this.scriptsDown);
            this.groupBox3.Controls.Add(this.scriptsRemove);
            this.groupBox3.Controls.Add(this.scriptsUp);
            this.groupBox3.Controls.Add(this.scriptsListBox);
            this.groupBox3.Location = new System.Drawing.Point(7, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(232, 195);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Script List";
            // 
            // scrtiptIdSource
            // 
            this.scrtiptIdSource.Location = new System.Drawing.Point(78, 148);
            this.scrtiptIdSource.Name = "scrtiptIdSource";
            this.scrtiptIdSource.Size = new System.Drawing.Size(148, 20);
            this.scrtiptIdSource.TabIndex = 6;
            // 
            // scriptsSet
            // 
            this.scriptsSet.Location = new System.Drawing.Point(10, 162);
            this.scriptsSet.Name = "scriptsSet";
            this.scriptsSet.Size = new System.Drawing.Size(55, 23);
            this.scriptsSet.TabIndex = 5;
            this.scriptsSet.Text = "Set";
            this.scriptsSet.UseVisualStyleBackColor = true;
            // 
            // scriptsAdd
            // 
            this.scriptsAdd.Location = new System.Drawing.Point(10, 133);
            this.scriptsAdd.Name = "scriptsAdd";
            this.scriptsAdd.Size = new System.Drawing.Size(55, 23);
            this.scriptsAdd.TabIndex = 4;
            this.scriptsAdd.Text = "Add";
            this.scriptsAdd.UseVisualStyleBackColor = true;
            // 
            // scriptsDown
            // 
            this.scriptsDown.Location = new System.Drawing.Point(10, 104);
            this.scriptsDown.Name = "scriptsDown";
            this.scriptsDown.Size = new System.Drawing.Size(55, 23);
            this.scriptsDown.TabIndex = 3;
            this.scriptsDown.Text = "Down";
            this.scriptsDown.UseVisualStyleBackColor = true;
            // 
            // scriptsRemove
            // 
            this.scriptsRemove.Location = new System.Drawing.Point(10, 60);
            this.scriptsRemove.Name = "scriptsRemove";
            this.scriptsRemove.Size = new System.Drawing.Size(55, 23);
            this.scriptsRemove.TabIndex = 2;
            this.scriptsRemove.Text = "Remove";
            this.scriptsRemove.UseVisualStyleBackColor = true;
            // 
            // scriptsUp
            // 
            this.scriptsUp.Location = new System.Drawing.Point(10, 19);
            this.scriptsUp.Name = "scriptsUp";
            this.scriptsUp.Size = new System.Drawing.Size(55, 23);
            this.scriptsUp.TabIndex = 1;
            this.scriptsUp.Text = "Up";
            this.scriptsUp.UseVisualStyleBackColor = true;
            // 
            // scriptsListBox
            // 
            this.scriptsListBox.FormattingEnabled = true;
            this.scriptsListBox.Location = new System.Drawing.Point(78, 19);
            this.scriptsListBox.Name = "scriptsListBox";
            this.scriptsListBox.Size = new System.Drawing.Size(148, 108);
            this.scriptsListBox.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(727, 533);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Tab2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.flagSource);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.nameSource);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(735, 41);
            this.panel1.TabIndex = 1;
            // 
            // nameSource
            // 
            this.nameSource.Location = new System.Drawing.Point(50, 11);
            this.nameSource.Name = "nameSource";
            this.nameSource.Size = new System.Drawing.Size(193, 20);
            this.nameSource.TabIndex = 1;
            this.nameSource.TextChanged += new System.EventHandler(this.nameSource_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name: ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.paramSource);
            this.groupBox2.Controls.Add(this.paramsSet);
            this.groupBox2.Controls.Add(this.paramsAdd);
            this.groupBox2.Controls.Add(this.paramsDown);
            this.groupBox2.Controls.Add(this.paramsRemove);
            this.groupBox2.Controls.Add(this.paramsUp);
            this.groupBox2.Controls.Add(this.paramsListBox);
            this.groupBox2.Location = new System.Drawing.Point(6, 213);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(232, 195);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Script Parameters";
            // 
            // paramSource
            // 
            this.paramSource.Location = new System.Drawing.Point(78, 148);
            this.paramSource.Name = "paramSource";
            this.paramSource.Size = new System.Drawing.Size(148, 20);
            this.paramSource.TabIndex = 6;
            // 
            // paramsSet
            // 
            this.paramsSet.Location = new System.Drawing.Point(10, 162);
            this.paramsSet.Name = "paramsSet";
            this.paramsSet.Size = new System.Drawing.Size(55, 23);
            this.paramsSet.TabIndex = 5;
            this.paramsSet.Text = "Set";
            this.paramsSet.UseVisualStyleBackColor = true;
            // 
            // paramsAdd
            // 
            this.paramsAdd.Location = new System.Drawing.Point(10, 133);
            this.paramsAdd.Name = "paramsAdd";
            this.paramsAdd.Size = new System.Drawing.Size(55, 23);
            this.paramsAdd.TabIndex = 4;
            this.paramsAdd.Text = "Add";
            this.paramsAdd.UseVisualStyleBackColor = true;
            // 
            // paramsDown
            // 
            this.paramsDown.Location = new System.Drawing.Point(10, 104);
            this.paramsDown.Name = "paramsDown";
            this.paramsDown.Size = new System.Drawing.Size(55, 23);
            this.paramsDown.TabIndex = 3;
            this.paramsDown.Text = "Down";
            this.paramsDown.UseVisualStyleBackColor = true;
            // 
            // paramsRemove
            // 
            this.paramsRemove.Location = new System.Drawing.Point(10, 60);
            this.paramsRemove.Name = "paramsRemove";
            this.paramsRemove.Size = new System.Drawing.Size(55, 23);
            this.paramsRemove.TabIndex = 2;
            this.paramsRemove.Text = "Remove";
            this.paramsRemove.UseVisualStyleBackColor = true;
            // 
            // paramsUp
            // 
            this.paramsUp.Location = new System.Drawing.Point(10, 19);
            this.paramsUp.Name = "paramsUp";
            this.paramsUp.Size = new System.Drawing.Size(55, 23);
            this.paramsUp.TabIndex = 1;
            this.paramsUp.Text = "Up";
            this.paramsUp.UseVisualStyleBackColor = true;
            // 
            // paramsListBox
            // 
            this.paramsListBox.FormattingEnabled = true;
            this.paramsListBox.Location = new System.Drawing.Point(78, 19);
            this.paramsListBox.Name = "paramsListBox";
            this.paramsListBox.Size = new System.Drawing.Size(148, 108);
            this.paramsListBox.TabIndex = 0;
            // 
            // flagSource
            // 
            this.flagSource.Location = new System.Drawing.Point(294, 10);
            this.flagSource.Name = "flagSource";
            this.flagSource.Size = new System.Drawing.Size(193, 20);
            this.flagSource.TabIndex = 3;
            this.flagSource.TextChanged += new System.EventHandler(this.flagSource_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(247, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Flag:";
            // 
            // ObjectEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 619);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.objectList);
            this.Name = "ObjectEditor";
            this.Text = "ObjectEditor";
            this.Load += new System.EventHandler(this.ObjectEditor_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox objectList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox nameSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox soundIdSource;
        private System.Windows.Forms.Button soundsSet;
        private System.Windows.Forms.Button soundsAdd;
        private System.Windows.Forms.Button soundsDown;
        private System.Windows.Forms.Button soundsRemove;
        private System.Windows.Forms.Button soundsUp;
        private System.Windows.Forms.ListBox soundsListBox;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox objectIdSource;
        private System.Windows.Forms.Button objectsSet;
        private System.Windows.Forms.Button objectsAdd;
        private System.Windows.Forms.Button objectsDown;
        private System.Windows.Forms.Button objectsRemove;
        private System.Windows.Forms.Button objectsUp;
        private System.Windows.Forms.ListBox objectsListBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox scrtiptIdSource;
        private System.Windows.Forms.Button scriptsSet;
        private System.Windows.Forms.Button scriptsAdd;
        private System.Windows.Forms.Button scriptsDown;
        private System.Windows.Forms.Button scriptsRemove;
        private System.Windows.Forms.Button scriptsUp;
        private System.Windows.Forms.ListBox scriptsListBox;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox animationIdSource;
        private System.Windows.Forms.Button animationsSet;
        private System.Windows.Forms.Button animationsAdd;
        private System.Windows.Forms.Button animationsDown;
        private System.Windows.Forms.Button animationsRemove;
        private System.Windows.Forms.Button animationsUp;
        private System.Windows.Forms.ListBox animationsListBox;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox ogiIdSource;
        private System.Windows.Forms.Button ogiSet;
        private System.Windows.Forms.Button ogiAdd;
        private System.Windows.Forms.Button ogiDown;
        private System.Windows.Forms.Button ogiRemove;
        private System.Windows.Forms.Button ogiUp;
        private System.Windows.Forms.ListBox ogiListBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox paramSource;
        private System.Windows.Forms.Button paramsSet;
        private System.Windows.Forms.Button paramsAdd;
        private System.Windows.Forms.Button paramsDown;
        private System.Windows.Forms.Button paramsRemove;
        private System.Windows.Forms.Button paramsUp;
        private System.Windows.Forms.ListBox paramsListBox;
        private System.Windows.Forms.TextBox flagSource;
        private System.Windows.Forms.Label label2;
    }
}