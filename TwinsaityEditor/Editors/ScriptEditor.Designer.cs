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
            this.groupBox1.SuspendLayout();
            this.panelHeader.SuspendLayout();
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
            this.groupBox1.Controls.Add(this.panelHeader);
            this.groupBox1.Controls.Add(this.panelMain);
            this.groupBox1.Controls.Add(this.panelType1);
            this.groupBox1.Controls.Add(this.panelType2);
            this.groupBox1.Controls.Add(this.panelType3);
            this.groupBox1.Controls.Add(this.panelType4);
            this.groupBox1.Controls.Add(this.panelLinked);
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
    }
}