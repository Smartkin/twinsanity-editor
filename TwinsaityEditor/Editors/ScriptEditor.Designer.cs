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
            this.groupBox1.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelMain.SuspendLayout();
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
            this.groupBox1.Controls.Add(this.panelMain);
            this.groupBox1.Controls.Add(this.panelType1);
            this.groupBox1.Controls.Add(this.panelType2);
            this.groupBox1.Controls.Add(this.panelType3);
            this.groupBox1.Controls.Add(this.panelType4);
            this.groupBox1.Controls.Add(this.panelLinked);
            this.groupBox1.Controls.Add(this.panelHeader);
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
            this.panelType1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelType1.Location = new System.Drawing.Point(3, 16);
            this.panelType1.Name = "panelType1";
            this.panelType1.Size = new System.Drawing.Size(303, 609);
            this.panelType1.TabIndex = 0;
            // 
            // panelType2
            // 
            this.panelType2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelType2.Location = new System.Drawing.Point(3, 16);
            this.panelType2.Name = "panelType2";
            this.panelType2.Size = new System.Drawing.Size(303, 609);
            this.panelType2.TabIndex = 0;
            // 
            // panelType3
            // 
            this.panelType3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelType3.Location = new System.Drawing.Point(3, 16);
            this.panelType3.Name = "panelType3";
            this.panelType3.Size = new System.Drawing.Size(303, 609);
            this.panelType3.TabIndex = 0;
            // 
            // panelType4
            // 
            this.panelType4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelType4.Location = new System.Drawing.Point(3, 16);
            this.panelType4.Name = "panelType4";
            this.panelType4.Size = new System.Drawing.Size(303, 609);
            this.panelType4.TabIndex = 0;
            // 
            // panelLinked
            // 
            this.panelLinked.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLinked.Location = new System.Drawing.Point(3, 16);
            this.panelLinked.Name = "panelLinked";
            this.panelLinked.Size = new System.Drawing.Size(303, 609);
            this.panelLinked.TabIndex = 0;
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
    }
}