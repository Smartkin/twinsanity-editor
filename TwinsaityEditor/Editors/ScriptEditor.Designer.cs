namespace TwinsaityEditor
{
    partial class ScriptEditor
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.scriptTree = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelType1 = new System.Windows.Forms.Panel();
            this.panelType2 = new System.Windows.Forms.Panel();
            this.panelType3 = new System.Windows.Forms.Panel();
            this.panelType4 = new System.Windows.Forms.Panel();
            this.panelLinked = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.headerSubScripts = new System.Windows.Forms.ComboBox();
            this.headerSubscriptID = new System.Windows.Forms.TextBox();
            this.headerSubscriptArg = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.mainName = new System.Windows.Forms.TextBox();
            this.mainLinkedCnt = new System.Windows.Forms.TextBox();
            this.mainUnk = new System.Windows.Forms.TextBox();
            this.mainAddLinked = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.mainLinkedPos = new System.Windows.Forms.TextBox();
            this.mainDelLinked = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.type1Array = new System.Windows.Forms.TextBox();
            this.type1UnkByte1 = new System.Windows.Forms.TextBox();
            this.type1UnkByte2 = new System.Windows.Forms.TextBox();
            this.type1UnkShort = new System.Windows.Forms.TextBox();
            this.type1UnkInt = new System.Windows.Forms.TextBox();
            this.type1Warning = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.type2Bitfield = new System.Windows.Forms.TextBox();
            this.type2Slot = new System.Windows.Forms.TextBox();
            this.type2CreateType3 = new System.Windows.Forms.Button();
            this.type2DeleteType3 = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.type2SelectedType4Pos = new System.Windows.Forms.TextBox();
            this.type2AddType4 = new System.Windows.Forms.Button();
            this.type2DeleteType4 = new System.Windows.Forms.Button();
            this.type2BitfieldWarning = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.type3VTable = new System.Windows.Forms.TextBox();
            this.type3UnkShort = new System.Windows.Forms.TextBox();
            this.type3X = new System.Windows.Forms.TextBox();
            this.type3Y = new System.Windows.Forms.TextBox();
            this.type3Z = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.type4Array = new System.Windows.Forms.TextBox();
            this.type4VTableIndex = new System.Windows.Forms.TextBox();
            this.type4BitField = new System.Windows.Forms.TextBox();
            this.type4ExpectedLength = new System.Windows.Forms.Label();
            this.type4Warning = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.linkedBitField = new System.Windows.Forms.TextBox();
            this.linkedSlotIndex = new System.Windows.Forms.TextBox();
            this.linkedCreateType1 = new System.Windows.Forms.Button();
            this.linkedDeleteType1 = new System.Windows.Forms.Button();
            this.linkedDeleteType2 = new System.Windows.Forms.Button();
            this.linkedCreateType2 = new System.Windows.Forms.Button();
            this.linkedWarning = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelType1.SuspendLayout();
            this.panelType2.SuspendLayout();
            this.panelType3.SuspendLayout();
            this.panelType4.SuspendLayout();
            this.panelLinked.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(213, 628);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // scriptTree
            // 
            this.scriptTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scriptTree.Location = new System.Drawing.Point(213, 0);
            this.scriptTree.Name = "scriptTree";
            this.scriptTree.Size = new System.Drawing.Size(243, 628);
            this.scriptTree.TabIndex = 1;
            this.scriptTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.scriptTree_AfterSelect);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panelLinked);
            this.groupBox1.Controls.Add(this.panelHeader);
            this.groupBox1.Controls.Add(this.panelMain);
            this.groupBox1.Controls.Add(this.panelType1);
            this.groupBox1.Controls.Add(this.panelType2);
            this.groupBox1.Controls.Add(this.panelType3);
            this.groupBox1.Controls.Add(this.panelType4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Location = new System.Drawing.Point(456, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(309, 628);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parameters";
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.label3);
            this.panelHeader.Controls.Add(this.label2);
            this.panelHeader.Controls.Add(this.headerSubscriptArg);
            this.panelHeader.Controls.Add(this.headerSubscriptID);
            this.panelHeader.Controls.Add(this.headerSubScripts);
            this.panelHeader.Controls.Add(this.label1);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHeader.Location = new System.Drawing.Point(3, 16);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(303, 609);
            this.panelHeader.TabIndex = 0;
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.mainDelLinked);
            this.panelMain.Controls.Add(this.mainLinkedPos);
            this.panelMain.Controls.Add(this.label8);
            this.panelMain.Controls.Add(this.mainAddLinked);
            this.panelMain.Controls.Add(this.mainUnk);
            this.panelMain.Controls.Add(this.mainLinkedCnt);
            this.panelMain.Controls.Add(this.mainName);
            this.panelMain.Controls.Add(this.label7);
            this.panelMain.Controls.Add(this.label6);
            this.panelMain.Controls.Add(this.label5);
            this.panelMain.Controls.Add(this.label4);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(3, 16);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(303, 609);
            this.panelMain.TabIndex = 0;
            // 
            // panelType1
            // 
            this.panelType1.Controls.Add(this.type1Warning);
            this.panelType1.Controls.Add(this.type1UnkInt);
            this.panelType1.Controls.Add(this.type1UnkShort);
            this.panelType1.Controls.Add(this.type1UnkByte2);
            this.panelType1.Controls.Add(this.type1UnkByte1);
            this.panelType1.Controls.Add(this.type1Array);
            this.panelType1.Controls.Add(this.label13);
            this.panelType1.Controls.Add(this.label12);
            this.panelType1.Controls.Add(this.label11);
            this.panelType1.Controls.Add(this.label10);
            this.panelType1.Controls.Add(this.label9);
            this.panelType1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelType1.Location = new System.Drawing.Point(3, 16);
            this.panelType1.Name = "panelType1";
            this.panelType1.Size = new System.Drawing.Size(303, 609);
            this.panelType1.TabIndex = 0;
            // 
            // panelType2
            // 
            this.panelType2.Controls.Add(this.type2BitfieldWarning);
            this.panelType2.Controls.Add(this.type2DeleteType4);
            this.panelType2.Controls.Add(this.type2AddType4);
            this.panelType2.Controls.Add(this.type2SelectedType4Pos);
            this.panelType2.Controls.Add(this.label16);
            this.panelType2.Controls.Add(this.type2DeleteType3);
            this.panelType2.Controls.Add(this.type2CreateType3);
            this.panelType2.Controls.Add(this.type2Slot);
            this.panelType2.Controls.Add(this.type2Bitfield);
            this.panelType2.Controls.Add(this.label15);
            this.panelType2.Controls.Add(this.label14);
            this.panelType2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelType2.Location = new System.Drawing.Point(3, 16);
            this.panelType2.Name = "panelType2";
            this.panelType2.Size = new System.Drawing.Size(303, 609);
            this.panelType2.TabIndex = 0;
            // 
            // panelType3
            // 
            this.panelType3.Controls.Add(this.type3Z);
            this.panelType3.Controls.Add(this.type3Y);
            this.panelType3.Controls.Add(this.type3X);
            this.panelType3.Controls.Add(this.type3UnkShort);
            this.panelType3.Controls.Add(this.type3VTable);
            this.panelType3.Controls.Add(this.label19);
            this.panelType3.Controls.Add(this.label18);
            this.panelType3.Controls.Add(this.label17);
            this.panelType3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelType3.Location = new System.Drawing.Point(3, 16);
            this.panelType3.Name = "panelType3";
            this.panelType3.Size = new System.Drawing.Size(303, 609);
            this.panelType3.TabIndex = 0;
            // 
            // panelType4
            // 
            this.panelType4.Controls.Add(this.type4Warning);
            this.panelType4.Controls.Add(this.type4ExpectedLength);
            this.panelType4.Controls.Add(this.type4BitField);
            this.panelType4.Controls.Add(this.type4VTableIndex);
            this.panelType4.Controls.Add(this.type4Array);
            this.panelType4.Controls.Add(this.label22);
            this.panelType4.Controls.Add(this.label21);
            this.panelType4.Controls.Add(this.label20);
            this.panelType4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelType4.Location = new System.Drawing.Point(3, 16);
            this.panelType4.Name = "panelType4";
            this.panelType4.Size = new System.Drawing.Size(303, 609);
            this.panelType4.TabIndex = 0;
            // 
            // panelLinked
            // 
            this.panelLinked.Controls.Add(this.textBox1);
            this.panelLinked.Controls.Add(this.label25);
            this.panelLinked.Controls.Add(this.linkedWarning);
            this.panelLinked.Controls.Add(this.linkedDeleteType2);
            this.panelLinked.Controls.Add(this.linkedCreateType2);
            this.panelLinked.Controls.Add(this.linkedDeleteType1);
            this.panelLinked.Controls.Add(this.linkedCreateType1);
            this.panelLinked.Controls.Add(this.linkedSlotIndex);
            this.panelLinked.Controls.Add(this.linkedBitField);
            this.panelLinked.Controls.Add(this.label24);
            this.panelLinked.Controls.Add(this.label23);
            this.panelLinked.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLinked.Location = new System.Drawing.Point(3, 16);
            this.panelLinked.Name = "panelLinked";
            this.panelLinked.Size = new System.Drawing.Size(303, 609);
            this.panelLinked.TabIndex = 0;
            this.panelLinked.Paint += new System.Windows.Forms.PaintEventHandler(this.panelLinked_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Header Script Editor";
            // 
            // headerSubScripts
            // 
            this.headerSubScripts.FormattingEnabled = true;
            this.headerSubScripts.Location = new System.Drawing.Point(7, 21);
            this.headerSubScripts.Name = "headerSubScripts";
            this.headerSubScripts.Size = new System.Drawing.Size(121, 21);
            this.headerSubScripts.TabIndex = 1;
            this.headerSubScripts.SelectedIndexChanged += new System.EventHandler(this.headerSubScripts_SelectedIndexChanged);
            // 
            // headerSubscriptID
            // 
            this.headerSubscriptID.Location = new System.Drawing.Point(194, 21);
            this.headerSubscriptID.Name = "headerSubscriptID";
            this.headerSubscriptID.Size = new System.Drawing.Size(100, 20);
            this.headerSubscriptID.TabIndex = 2;
            this.headerSubscriptID.TextChanged += new System.EventHandler(this.headerSubscriptID_TextChanged);
            // 
            // headerSubscriptArg
            // 
            this.headerSubscriptArg.Location = new System.Drawing.Point(194, 47);
            this.headerSubscriptArg.Name = "headerSubscriptArg";
            this.headerSubscriptArg.Size = new System.Drawing.Size(100, 20);
            this.headerSubscriptArg.TabIndex = 3;
            this.headerSubscriptArg.TextChanged += new System.EventHandler(this.headerSubscriptArg_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(143, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "ScriptID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(143, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "ScriptArg";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Main Script Editor";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Name:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "LinkedScripits";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Unknown Value:";
            // 
            // mainName
            // 
            this.mainName.Location = new System.Drawing.Point(104, 21);
            this.mainName.Name = "mainName";
            this.mainName.Size = new System.Drawing.Size(100, 20);
            this.mainName.TabIndex = 4;
            this.mainName.TextChanged += new System.EventHandler(this.mainName_TextChanged);
            // 
            // mainLinkedCnt
            // 
            this.mainLinkedCnt.Location = new System.Drawing.Point(104, 43);
            this.mainLinkedCnt.Name = "mainLinkedCnt";
            this.mainLinkedCnt.ReadOnly = true;
            this.mainLinkedCnt.Size = new System.Drawing.Size(100, 20);
            this.mainLinkedCnt.TabIndex = 5;
            // 
            // mainUnk
            // 
            this.mainUnk.Location = new System.Drawing.Point(104, 64);
            this.mainUnk.Name = "mainUnk";
            this.mainUnk.Size = new System.Drawing.Size(100, 20);
            this.mainUnk.TabIndex = 6;
            this.mainUnk.TextChanged += new System.EventHandler(this.mainUnk_TextChanged);
            // 
            // mainAddLinked
            // 
            this.mainAddLinked.Location = new System.Drawing.Point(7, 125);
            this.mainAddLinked.Name = "mainAddLinked";
            this.mainAddLinked.Size = new System.Drawing.Size(75, 23);
            this.mainAddLinked.TabIndex = 7;
            this.mainAddLinked.Text = "Add";
            this.mainAddLinked.UseVisualStyleBackColor = true;
            this.mainAddLinked.Click += new System.EventHandler(this.mainAddLinked_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 91);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(112, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Linked Script Position:";
            // 
            // mainLinkedPos
            // 
            this.mainLinkedPos.Location = new System.Drawing.Point(116, 88);
            this.mainLinkedPos.Name = "mainLinkedPos";
            this.mainLinkedPos.Size = new System.Drawing.Size(100, 20);
            this.mainLinkedPos.TabIndex = 9;
            this.mainLinkedPos.TextChanged += new System.EventHandler(this.mainLinkedPos_TextChanged);
            // 
            // mainDelLinked
            // 
            this.mainDelLinked.Location = new System.Drawing.Point(88, 125);
            this.mainDelLinked.Name = "mainDelLinked";
            this.mainDelLinked.Size = new System.Drawing.Size(75, 23);
            this.mainDelLinked.TabIndex = 10;
            this.mainDelLinked.Text = "Delete";
            this.mainDelLinked.UseVisualStyleBackColor = true;
            this.mainDelLinked.Click += new System.EventHandler(this.mainDelLinked_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "UnkByte1:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 37);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "UnkByte2:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(4, 63);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "UnkUShort:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(22, 88);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(42, 13);
            this.label12.TabIndex = 3;
            this.label12.Text = "UnkInt:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 123);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(55, 13);
            this.label13.TabIndex = 4;
            this.label13.Text = "ByteArray:";
            // 
            // type1Array
            // 
            this.type1Array.Location = new System.Drawing.Point(7, 139);
            this.type1Array.Name = "type1Array";
            this.type1Array.Size = new System.Drawing.Size(284, 20);
            this.type1Array.TabIndex = 5;
            this.type1Array.TextChanged += new System.EventHandler(this.type1Array_TextChanged);
            // 
            // type1UnkByte1
            // 
            this.type1UnkByte1.Location = new System.Drawing.Point(70, 8);
            this.type1UnkByte1.Name = "type1UnkByte1";
            this.type1UnkByte1.Size = new System.Drawing.Size(100, 20);
            this.type1UnkByte1.TabIndex = 6;
            this.type1UnkByte1.TextChanged += new System.EventHandler(this.type1UnkByte1_TextChanged);
            // 
            // type1UnkByte2
            // 
            this.type1UnkByte2.Location = new System.Drawing.Point(70, 34);
            this.type1UnkByte2.Name = "type1UnkByte2";
            this.type1UnkByte2.Size = new System.Drawing.Size(100, 20);
            this.type1UnkByte2.TabIndex = 7;
            this.type1UnkByte2.TextChanged += new System.EventHandler(this.type1UnkByte2_TextChanged);
            // 
            // type1UnkShort
            // 
            this.type1UnkShort.Location = new System.Drawing.Point(70, 60);
            this.type1UnkShort.Name = "type1UnkShort";
            this.type1UnkShort.Size = new System.Drawing.Size(100, 20);
            this.type1UnkShort.TabIndex = 8;
            this.type1UnkShort.TextChanged += new System.EventHandler(this.type1UnkShort_TextChanged);
            // 
            // type1UnkInt
            // 
            this.type1UnkInt.Location = new System.Drawing.Point(70, 86);
            this.type1UnkInt.Name = "type1UnkInt";
            this.type1UnkInt.Size = new System.Drawing.Size(100, 20);
            this.type1UnkInt.TabIndex = 9;
            this.type1UnkInt.TextChanged += new System.EventHandler(this.type1UnkInt_TextChanged);
            // 
            // type1Warning
            // 
            this.type1Warning.AutoSize = true;
            this.type1Warning.ForeColor = System.Drawing.Color.Red;
            this.type1Warning.Location = new System.Drawing.Point(10, 171);
            this.type1Warning.Name = "type1Warning";
            this.type1Warning.Size = new System.Drawing.Size(254, 26);
            this.type1Warning.TabIndex = 10;
            this.type1Warning.Text = "Bytes 1 and 2 do not match with length of ByteArray.\r\nIt must be B1 + 4*B2!";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(51, 11);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(47, 13);
            this.label14.TabIndex = 0;
            this.label14.Text = "Bit Field:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(10, 36);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(93, 13);
            this.label15.TabIndex = 1;
            this.label15.Text = "Linked Script Slot:";
            // 
            // type2Bitfield
            // 
            this.type2Bitfield.Location = new System.Drawing.Point(109, 8);
            this.type2Bitfield.Name = "type2Bitfield";
            this.type2Bitfield.Size = new System.Drawing.Size(100, 20);
            this.type2Bitfield.TabIndex = 2;
            this.type2Bitfield.TextChanged += new System.EventHandler(this.type2Bitfield_TextChanged);
            // 
            // type2Slot
            // 
            this.type2Slot.Location = new System.Drawing.Point(109, 33);
            this.type2Slot.Name = "type2Slot";
            this.type2Slot.Size = new System.Drawing.Size(100, 20);
            this.type2Slot.TabIndex = 3;
            this.type2Slot.TextChanged += new System.EventHandler(this.type2Slot_TextChanged);
            // 
            // type2CreateType3
            // 
            this.type2CreateType3.Location = new System.Drawing.Point(10, 60);
            this.type2CreateType3.Name = "type2CreateType3";
            this.type2CreateType3.Size = new System.Drawing.Size(88, 23);
            this.type2CreateType3.TabIndex = 4;
            this.type2CreateType3.Text = "Create Type3";
            this.type2CreateType3.UseVisualStyleBackColor = true;
            this.type2CreateType3.Click += new System.EventHandler(this.type2CreateType3_Click);
            // 
            // type2DeleteType3
            // 
            this.type2DeleteType3.Location = new System.Drawing.Point(10, 86);
            this.type2DeleteType3.Name = "type2DeleteType3";
            this.type2DeleteType3.Size = new System.Drawing.Size(88, 23);
            this.type2DeleteType3.TabIndex = 5;
            this.type2DeleteType3.Text = "Delete Type3";
            this.type2DeleteType3.UseVisualStyleBackColor = true;
            this.type2DeleteType3.Click += new System.EventHandler(this.type2DeleteType3_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(14, 123);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(80, 13);
            this.label16.TabIndex = 6;
            this.label16.Text = "Type4 Position:";
            // 
            // type2SelectedType4Pos
            // 
            this.type2SelectedType4Pos.Location = new System.Drawing.Point(109, 120);
            this.type2SelectedType4Pos.Name = "type2SelectedType4Pos";
            this.type2SelectedType4Pos.Size = new System.Drawing.Size(100, 20);
            this.type2SelectedType4Pos.TabIndex = 7;
            this.type2SelectedType4Pos.TextChanged += new System.EventHandler(this.type2SelectedType4Pos_TextChanged);
            // 
            // type2AddType4
            // 
            this.type2AddType4.Location = new System.Drawing.Point(13, 154);
            this.type2AddType4.Name = "type2AddType4";
            this.type2AddType4.Size = new System.Drawing.Size(88, 23);
            this.type2AddType4.TabIndex = 8;
            this.type2AddType4.Text = "Add Type4";
            this.type2AddType4.UseVisualStyleBackColor = true;
            this.type2AddType4.Click += new System.EventHandler(this.type2AddType4_Click);
            // 
            // type2DeleteType4
            // 
            this.type2DeleteType4.Location = new System.Drawing.Point(109, 154);
            this.type2DeleteType4.Name = "type2DeleteType4";
            this.type2DeleteType4.Size = new System.Drawing.Size(88, 23);
            this.type2DeleteType4.TabIndex = 9;
            this.type2DeleteType4.Text = "Delete Type4";
            this.type2DeleteType4.UseVisualStyleBackColor = true;
            this.type2DeleteType4.Click += new System.EventHandler(this.type2DeleteType4_Click);
            // 
            // type2BitfieldWarning
            // 
            this.type2BitfieldWarning.AutoSize = true;
            this.type2BitfieldWarning.ForeColor = System.Drawing.Color.Red;
            this.type2BitfieldWarning.Location = new System.Drawing.Point(7, 197);
            this.type2BitfieldWarning.Name = "type2BitfieldWarning";
            this.type2BitfieldWarning.Size = new System.Drawing.Size(279, 52);
            this.type2BitfieldWarning.TabIndex = 10;
            this.type2BitfieldWarning.Text = "Bit Field does not corresponds with held items!\r\n0xFF not zero - has type4\r\n0x200" +
    " not zero - has type3\r\n0x800 not zero - has next type2 (edit Linked Script paren" +
    "t)";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(20, 17);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(70, 13);
            this.label17.TabIndex = 0;
            this.label17.Text = "VTableIndex:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(35, 41);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(55, 13);
            this.label18.TabIndex = 1;
            this.label18.Text = "UnkShort:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(8, 67);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(98, 13);
            this.label19.TabIndex = 2;
            this.label19.Text = "Vector3 Parameter:";
            // 
            // type3VTable
            // 
            this.type3VTable.Location = new System.Drawing.Point(104, 11);
            this.type3VTable.Name = "type3VTable";
            this.type3VTable.Size = new System.Drawing.Size(100, 20);
            this.type3VTable.TabIndex = 3;
            this.type3VTable.TextChanged += new System.EventHandler(this.type3VTable_TextChanged);
            // 
            // type3UnkShort
            // 
            this.type3UnkShort.Location = new System.Drawing.Point(104, 38);
            this.type3UnkShort.Name = "type3UnkShort";
            this.type3UnkShort.Size = new System.Drawing.Size(100, 20);
            this.type3UnkShort.TabIndex = 4;
            this.type3UnkShort.TextChanged += new System.EventHandler(this.type3UnkShort_TextChanged);
            // 
            // type3X
            // 
            this.type3X.Location = new System.Drawing.Point(19, 86);
            this.type3X.Name = "type3X";
            this.type3X.Size = new System.Drawing.Size(75, 20);
            this.type3X.TabIndex = 5;
            this.type3X.TextChanged += new System.EventHandler(this.type3X_TextChanged);
            // 
            // type3Y
            // 
            this.type3Y.Location = new System.Drawing.Point(100, 85);
            this.type3Y.Name = "type3Y";
            this.type3Y.Size = new System.Drawing.Size(75, 20);
            this.type3Y.TabIndex = 6;
            this.type3Y.TextChanged += new System.EventHandler(this.type3Y_TextChanged);
            // 
            // type3Z
            // 
            this.type3Z.Location = new System.Drawing.Point(181, 85);
            this.type3Z.Name = "type3Z";
            this.type3Z.Size = new System.Drawing.Size(75, 20);
            this.type3Z.TabIndex = 7;
            this.type3Z.TextChanged += new System.EventHandler(this.type3Z_TextChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(9, 11);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(73, 13);
            this.label20.TabIndex = 0;
            this.label20.Text = "VTable Index:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(35, 38);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(47, 13);
            this.label21.TabIndex = 1;
            this.label21.Text = "Bit Field:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(32, 64);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(58, 13);
            this.label22.TabIndex = 2;
            this.label22.Text = "Byte Array:";
            // 
            // type4Array
            // 
            this.type4Array.Location = new System.Drawing.Point(10, 89);
            this.type4Array.Name = "type4Array";
            this.type4Array.Size = new System.Drawing.Size(287, 20);
            this.type4Array.TabIndex = 3;
            this.type4Array.TextChanged += new System.EventHandler(this.type4Array_TextChanged);
            // 
            // type4VTableIndex
            // 
            this.type4VTableIndex.Location = new System.Drawing.Point(88, 7);
            this.type4VTableIndex.Name = "type4VTableIndex";
            this.type4VTableIndex.Size = new System.Drawing.Size(100, 20);
            this.type4VTableIndex.TabIndex = 4;
            this.type4VTableIndex.TextChanged += new System.EventHandler(this.type4VTableIndex_TextChanged);
            // 
            // type4BitField
            // 
            this.type4BitField.Location = new System.Drawing.Point(88, 34);
            this.type4BitField.Name = "type4BitField";
            this.type4BitField.Size = new System.Drawing.Size(100, 20);
            this.type4BitField.TabIndex = 5;
            this.type4BitField.TextChanged += new System.EventHandler(this.type4BitField_TextChanged);
            // 
            // type4ExpectedLength
            // 
            this.type4ExpectedLength.AutoSize = true;
            this.type4ExpectedLength.Location = new System.Drawing.Point(23, 116);
            this.type4ExpectedLength.Name = "type4ExpectedLength";
            this.type4ExpectedLength.Size = new System.Drawing.Size(94, 13);
            this.type4ExpectedLength.TabIndex = 6;
            this.type4ExpectedLength.Text = "Expected Length: ";
            // 
            // type4Warning
            // 
            this.type4Warning.AutoSize = true;
            this.type4Warning.ForeColor = System.Drawing.Color.Red;
            this.type4Warning.Location = new System.Drawing.Point(23, 135);
            this.type4Warning.Name = "type4Warning";
            this.type4Warning.Size = new System.Drawing.Size(251, 39);
            this.type4Warning.TabIndex = 7;
            this.type4Warning.Text = "Bit Field value and next Type4 presence in parent\r\ndo not correspond (Bit 0x0100 " +
    "set) or \r\nBit Array length and Expected Length do not match.\r\n";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(20, 11);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(47, 13);
            this.label23.TabIndex = 0;
            this.label23.Text = "Bit Field:";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(10, 36);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(57, 13);
            this.label24.TabIndex = 1;
            this.label24.Text = "Slot Index:";
            // 
            // linkedBitField
            // 
            this.linkedBitField.Location = new System.Drawing.Point(88, 8);
            this.linkedBitField.Name = "linkedBitField";
            this.linkedBitField.Size = new System.Drawing.Size(100, 20);
            this.linkedBitField.TabIndex = 2;
            this.linkedBitField.TextChanged += new System.EventHandler(this.linkedBitField_TextChanged);
            // 
            // linkedSlotIndex
            // 
            this.linkedSlotIndex.Location = new System.Drawing.Point(88, 33);
            this.linkedSlotIndex.Name = "linkedSlotIndex";
            this.linkedSlotIndex.Size = new System.Drawing.Size(100, 20);
            this.linkedSlotIndex.TabIndex = 3;
            this.linkedSlotIndex.TextChanged += new System.EventHandler(this.linkedSlotIndex_TextChanged);
            // 
            // linkedCreateType1
            // 
            this.linkedCreateType1.Location = new System.Drawing.Point(10, 67);
            this.linkedCreateType1.Name = "linkedCreateType1";
            this.linkedCreateType1.Size = new System.Drawing.Size(84, 23);
            this.linkedCreateType1.TabIndex = 4;
            this.linkedCreateType1.Text = "Create Type 1";
            this.linkedCreateType1.UseVisualStyleBackColor = true;
            this.linkedCreateType1.Click += new System.EventHandler(this.linkedCreateType1_Click);
            // 
            // linkedDeleteType1
            // 
            this.linkedDeleteType1.Location = new System.Drawing.Point(100, 67);
            this.linkedDeleteType1.Name = "linkedDeleteType1";
            this.linkedDeleteType1.Size = new System.Drawing.Size(84, 23);
            this.linkedDeleteType1.TabIndex = 5;
            this.linkedDeleteType1.Text = "Delete Type 1";
            this.linkedDeleteType1.UseVisualStyleBackColor = true;
            this.linkedDeleteType1.Click += new System.EventHandler(this.linkedDeleteType1_Click);
            // 
            // linkedDeleteType2
            // 
            this.linkedDeleteType2.Location = new System.Drawing.Point(101, 120);
            this.linkedDeleteType2.Name = "linkedDeleteType2";
            this.linkedDeleteType2.Size = new System.Drawing.Size(84, 23);
            this.linkedDeleteType2.TabIndex = 9;
            this.linkedDeleteType2.Text = "Delete Type 2";
            this.linkedDeleteType2.UseVisualStyleBackColor = true;
            this.linkedDeleteType2.Click += new System.EventHandler(this.linkedDeleteType2_Click);
            // 
            // linkedCreateType2
            // 
            this.linkedCreateType2.Location = new System.Drawing.Point(11, 120);
            this.linkedCreateType2.Name = "linkedCreateType2";
            this.linkedCreateType2.Size = new System.Drawing.Size(84, 23);
            this.linkedCreateType2.TabIndex = 8;
            this.linkedCreateType2.Text = "Add Type 2";
            this.linkedCreateType2.UseVisualStyleBackColor = true;
            this.linkedCreateType2.Click += new System.EventHandler(this.linkedCreateType2_Click);
            // 
            // linkedWarning
            // 
            this.linkedWarning.AutoSize = true;
            this.linkedWarning.ForeColor = System.Drawing.Color.Red;
            this.linkedWarning.Location = new System.Drawing.Point(10, 146);
            this.linkedWarning.Name = "linkedWarning";
            this.linkedWarning.Size = new System.Drawing.Size(238, 52);
            this.linkedWarning.TabIndex = 10;
            this.linkedWarning.Text = "Bit Field don\'t correspond with presense of items:\r\n0x4000 bit for Type1 presence" +
    "\r\n0x8000 bit for having next Linked Script in parent\r\n0x1F not zero for presence" +
    " of Type 2";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(97, 99);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 12;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(10, 102);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(83, 13);
            this.label25.TabIndex = 11;
            this.label25.Text = "Type 2 Position:";
            // 
            // ScriptEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 628);
            this.Controls.Add(this.scriptTree);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listBox1);
            this.Name = "ScriptEditor";
            this.Text = "ScriptEditor";
            this.Load += new System.EventHandler(this.ScriptEditor_Load);
            this.groupBox1.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.panelType1.ResumeLayout(false);
            this.panelType1.PerformLayout();
            this.panelType2.ResumeLayout(false);
            this.panelType2.PerformLayout();
            this.panelType3.ResumeLayout(false);
            this.panelType3.PerformLayout();
            this.panelType4.ResumeLayout(false);
            this.panelType4.PerformLayout();
            this.panelLinked.ResumeLayout(false);
            this.panelLinked.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TreeView scriptTree;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelType1;
        private System.Windows.Forms.Panel panelType2;
        private System.Windows.Forms.Panel panelType3;
        private System.Windows.Forms.Panel panelType4;
        private System.Windows.Forms.Panel panelLinked;
        private System.Windows.Forms.TextBox headerSubscriptID;
        private System.Windows.Forms.ComboBox headerSubScripts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox headerSubscriptArg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button mainDelLinked;
        private System.Windows.Forms.TextBox mainLinkedPos;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button mainAddLinked;
        private System.Windows.Forms.TextBox mainUnk;
        private System.Windows.Forms.TextBox mainLinkedCnt;
        private System.Windows.Forms.TextBox mainName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox type1UnkInt;
        private System.Windows.Forms.TextBox type1UnkShort;
        private System.Windows.Forms.TextBox type1UnkByte2;
        private System.Windows.Forms.TextBox type1UnkByte1;
        private System.Windows.Forms.TextBox type1Array;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label type1Warning;
        private System.Windows.Forms.TextBox type2Slot;
        private System.Windows.Forms.TextBox type2Bitfield;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button type2DeleteType3;
        private System.Windows.Forms.Button type2CreateType3;
        private System.Windows.Forms.Button type2DeleteType4;
        private System.Windows.Forms.Button type2AddType4;
        private System.Windows.Forms.TextBox type2SelectedType4Pos;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label type2BitfieldWarning;
        private System.Windows.Forms.TextBox type3Z;
        private System.Windows.Forms.TextBox type3Y;
        private System.Windows.Forms.TextBox type3X;
        private System.Windows.Forms.TextBox type3UnkShort;
        private System.Windows.Forms.TextBox type3VTable;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox type4BitField;
        private System.Windows.Forms.TextBox type4VTableIndex;
        private System.Windows.Forms.TextBox type4Array;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label type4Warning;
        private System.Windows.Forms.Label type4ExpectedLength;
        private System.Windows.Forms.Button linkedDeleteType1;
        private System.Windows.Forms.Button linkedCreateType1;
        private System.Windows.Forms.TextBox linkedSlotIndex;
        private System.Windows.Forms.TextBox linkedBitField;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button linkedDeleteType2;
        private System.Windows.Forms.Button linkedCreateType2;
        private System.Windows.Forms.Label linkedWarning;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label25;
    }
}