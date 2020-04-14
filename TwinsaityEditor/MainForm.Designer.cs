namespace TwinsaityEditor
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openRM2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.newRM2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSM2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eLFPatcherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mHMBToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rMViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sMViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Pref_TruncateNames_toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Pref_EnableAllNames_toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.viewersToolStripMenuItem,
            this.preferencesToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(721, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openRM2ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.newRM2ToolStripMenuItem,
            this.newSM2ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripMenuItem3,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openRM2ToolStripMenuItem
            // 
            this.openRM2ToolStripMenuItem.Name = "openRM2ToolStripMenuItem";
            this.openRM2ToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.openRM2ToolStripMenuItem.Text = "Open";
            this.openRM2ToolStripMenuItem.Click += new System.EventHandler(this.openRM2ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(122, 6);
            // 
            // newRM2ToolStripMenuItem
            // 
            this.newRM2ToolStripMenuItem.Enabled = false;
            this.newRM2ToolStripMenuItem.Name = "newRM2ToolStripMenuItem";
            this.newRM2ToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.newRM2ToolStripMenuItem.Text = "New RM2";
            // 
            // newSM2ToolStripMenuItem
            // 
            this.newSM2ToolStripMenuItem.Enabled = false;
            this.newSM2ToolStripMenuItem.Name = "newSM2ToolStripMenuItem";
            this.newSM2ToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.newSM2ToolStripMenuItem.Text = "New SM2";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(122, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.saveAsToolStripMenuItem.Text = "Save as...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(122, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eLFPatcherToolStripMenuItem,
            this.mHMBToolToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // eLFPatcherToolStripMenuItem
            // 
            this.eLFPatcherToolStripMenuItem.Name = "eLFPatcherToolStripMenuItem";
            this.eLFPatcherToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.eLFPatcherToolStripMenuItem.Text = "EXE Patcher";
            this.eLFPatcherToolStripMenuItem.Click += new System.EventHandler(this.eLFPatcherToolStripMenuItem_Click);
            // 
            // mHMBToolToolStripMenuItem
            // 
            this.mHMBToolToolStripMenuItem.Name = "mHMBToolToolStripMenuItem";
            this.mHMBToolToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.mHMBToolToolStripMenuItem.Text = "MH/MB Tool";
            this.mHMBToolToolStripMenuItem.Click += new System.EventHandler(this.mHMBToolToolStripMenuItem_Click);
            // 
            // viewersToolStripMenuItem
            // 
            this.viewersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rMViewerToolStripMenuItem,
            this.sMViewerToolStripMenuItem});
            this.viewersToolStripMenuItem.Name = "viewersToolStripMenuItem";
            this.viewersToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.viewersToolStripMenuItem.Text = "Viewers";
            // 
            // rMViewerToolStripMenuItem
            // 
            this.rMViewerToolStripMenuItem.Enabled = false;
            this.rMViewerToolStripMenuItem.Name = "rMViewerToolStripMenuItem";
            this.rMViewerToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.rMViewerToolStripMenuItem.Text = "RM Viewer";
            this.rMViewerToolStripMenuItem.Click += new System.EventHandler(this.rMViewerToolStripMenuItem_Click);
            // 
            // sMViewerToolStripMenuItem
            // 
            this.sMViewerToolStripMenuItem.Enabled = false;
            this.sMViewerToolStripMenuItem.Name = "sMViewerToolStripMenuItem";
            this.sMViewerToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.sMViewerToolStripMenuItem.Text = "SM Viewer";
            this.sMViewerToolStripMenuItem.Click += new System.EventHandler(this.sMViewerToolStripMenuItem_Click);
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Pref_TruncateNames_toolStripMenuItem,
            this.Pref_EnableAllNames_toolStripMenuItem});
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.preferencesToolStripMenuItem.Text = "Preferences";
            // 
            // Pref_TruncateNames_toolStripMenuItem
            // 
            this.Pref_TruncateNames_toolStripMenuItem.Checked = true;
            this.Pref_TruncateNames_toolStripMenuItem.CheckOnClick = true;
            this.Pref_TruncateNames_toolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Pref_TruncateNames_toolStripMenuItem.Name = "Pref_TruncateNames_toolStripMenuItem";
            this.Pref_TruncateNames_toolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.Pref_TruncateNames_toolStripMenuItem.Text = "Truncate Instance Names";
            this.Pref_TruncateNames_toolStripMenuItem.Click += new System.EventHandler(this.Pref_TruncateNames_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(5);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBox1);
            this.splitContainer1.Size = new System.Drawing.Size(721, 345);
            this.splitContainer1.SplitterDistance = 280;
            this.splitContainer1.TabIndex = 1;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(280, 345);
            this.treeView1.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.AcceptsReturn = true;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(437, 345);
            this.textBox1.TabIndex = 0;
            // 
            // Pref_EnableAllNames_toolStripMenuItem
            // 
            this.Pref_EnableAllNames_toolStripMenuItem.Checked = true;
            this.Pref_EnableAllNames_toolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Pref_EnableAllNames_toolStripMenuItem.Name = "Pref_EnableAllNames_toolStripMenuItem";
            this.Pref_EnableAllNames_toolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.Pref_EnableAllNames_toolStripMenuItem.Text = "Enable All Instance Names";
            this.Pref_EnableAllNames_toolStripMenuItem.Click += new System.EventHandler(this.Pref_EnableAllNames_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 369);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Twinsaity Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openRM2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem newRM2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newSM2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eLFPatcherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rMViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sMViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mHMBToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Pref_TruncateNames_toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Pref_EnableAllNames_toolStripMenuItem;
    }
}