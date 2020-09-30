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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.SaveAsButton = new System.Windows.Forms.Button();
            this.RM2ViewerButton = new System.Windows.Forms.Button();
            this.SaveButtonMain = new System.Windows.Forms.Button();
            this.OpenFileButton = new System.Windows.Forms.Button();
            this.AboutButton = new System.Windows.Forms.Button();
            this.EXEPatcherButton = new System.Windows.Forms.Button();
            this.ImageMakerButton = new System.Windows.Forms.Button();
            this.BDBHButton = new System.Windows.Forms.Button();
            this.MHMBToolButton = new System.Windows.Forms.Button();
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
            this.bHBDToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageMakerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rMViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sMViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Pref_TruncateNames_toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Pref_EnableAllNames_toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.thisIsFillerToFixLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Silver;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(124, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBox1);
            this.splitContainer1.Size = new System.Drawing.Size(771, 513);
            this.splitContainer1.SplitterDistance = 312;
            this.splitContainer1.TabIndex = 1;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(312, 513);
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
            this.textBox1.Size = new System.Drawing.Size(455, 513);
            this.textBox1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.BackColor = System.Drawing.Color.Silver;
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.SaveAsButton);
            this.panel2.Controls.Add(this.RM2ViewerButton);
            this.panel2.Controls.Add(this.SaveButtonMain);
            this.panel2.Controls.Add(this.OpenFileButton);
            this.panel2.Controls.Add(this.AboutButton);
            this.panel2.Controls.Add(this.EXEPatcherButton);
            this.panel2.Controls.Add(this.ImageMakerButton);
            this.panel2.Controls.Add(this.BDBHButton);
            this.panel2.Controls.Add(this.MHMBToolButton);
            this.panel2.Location = new System.Drawing.Point(0, 70);
            this.panel2.MaximumSize = new System.Drawing.Size(126, 9999);
            this.panel2.MinimumSize = new System.Drawing.Size(126, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(126, 9999);
            this.panel2.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Silver;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Twelve Ton Goldfish", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(7, 184);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 38);
            this.button1.TabIndex = 9;
            this.button1.Text = "SM Viewer";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SaveAsButton
            // 
            this.SaveAsButton.BackColor = System.Drawing.Color.Silver;
            this.SaveAsButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SaveAsButton.FlatAppearance.BorderSize = 0;
            this.SaveAsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveAsButton.Font = new System.Drawing.Font("Twelve Ton Goldfish", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveAsButton.Location = new System.Drawing.Point(7, 96);
            this.SaveAsButton.Name = "SaveAsButton";
            this.SaveAsButton.Size = new System.Drawing.Size(112, 38);
            this.SaveAsButton.TabIndex = 8;
            this.SaveAsButton.Text = "Save As...";
            this.SaveAsButton.UseVisualStyleBackColor = false;
            this.SaveAsButton.Click += new System.EventHandler(this.SaveAsButton_Click);
            // 
            // RM2ViewerButton
            // 
            this.RM2ViewerButton.BackColor = System.Drawing.Color.Silver;
            this.RM2ViewerButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RM2ViewerButton.FlatAppearance.BorderSize = 0;
            this.RM2ViewerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RM2ViewerButton.Font = new System.Drawing.Font("Twelve Ton Goldfish", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RM2ViewerButton.Location = new System.Drawing.Point(7, 140);
            this.RM2ViewerButton.Name = "RM2ViewerButton";
            this.RM2ViewerButton.Size = new System.Drawing.Size(112, 38);
            this.RM2ViewerButton.TabIndex = 2;
            this.RM2ViewerButton.Text = "RM Viewer";
            this.RM2ViewerButton.UseVisualStyleBackColor = false;
            this.RM2ViewerButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // SaveButtonMain
            // 
            this.SaveButtonMain.BackColor = System.Drawing.Color.Silver;
            this.SaveButtonMain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SaveButtonMain.FlatAppearance.BorderSize = 0;
            this.SaveButtonMain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveButtonMain.Font = new System.Drawing.Font("Twelve Ton Goldfish", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveButtonMain.Location = new System.Drawing.Point(7, 52);
            this.SaveButtonMain.Name = "SaveButtonMain";
            this.SaveButtonMain.Size = new System.Drawing.Size(112, 38);
            this.SaveButtonMain.TabIndex = 0;
            this.SaveButtonMain.Text = "Save File";
            this.SaveButtonMain.UseVisualStyleBackColor = false;
            this.SaveButtonMain.Click += new System.EventHandler(this.button2_Click);
            // 
            // OpenFileButton
            // 
            this.OpenFileButton.BackColor = System.Drawing.Color.Silver;
            this.OpenFileButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OpenFileButton.FlatAppearance.BorderSize = 0;
            this.OpenFileButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OpenFileButton.Font = new System.Drawing.Font("Twelve Ton Goldfish", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpenFileButton.Location = new System.Drawing.Point(7, 8);
            this.OpenFileButton.Name = "OpenFileButton";
            this.OpenFileButton.Size = new System.Drawing.Size(112, 38);
            this.OpenFileButton.TabIndex = 0;
            this.OpenFileButton.Text = "Open File";
            this.OpenFileButton.UseVisualStyleBackColor = false;
            this.OpenFileButton.Click += new System.EventHandler(this.OpenFileButton_Click);
            // 
            // AboutButton
            // 
            this.AboutButton.BackColor = System.Drawing.Color.Silver;
            this.AboutButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AboutButton.FlatAppearance.BorderSize = 0;
            this.AboutButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AboutButton.Font = new System.Drawing.Font("Twelve Ton Goldfish", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AboutButton.Location = new System.Drawing.Point(7, 404);
            this.AboutButton.Name = "AboutButton";
            this.AboutButton.Size = new System.Drawing.Size(112, 38);
            this.AboutButton.TabIndex = 7;
            this.AboutButton.Text = "About";
            this.AboutButton.UseVisualStyleBackColor = false;
            this.AboutButton.Click += new System.EventHandler(this.button8_Click);
            // 
            // EXEPatcherButton
            // 
            this.EXEPatcherButton.BackColor = System.Drawing.Color.Silver;
            this.EXEPatcherButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.EXEPatcherButton.FlatAppearance.BorderSize = 0;
            this.EXEPatcherButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EXEPatcherButton.Font = new System.Drawing.Font("Twelve Ton Goldfish", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EXEPatcherButton.Location = new System.Drawing.Point(7, 360);
            this.EXEPatcherButton.Name = "EXEPatcherButton";
            this.EXEPatcherButton.Size = new System.Drawing.Size(112, 38);
            this.EXEPatcherButton.TabIndex = 6;
            this.EXEPatcherButton.Text = "EXE Patcher";
            this.EXEPatcherButton.UseVisualStyleBackColor = false;
            this.EXEPatcherButton.Click += new System.EventHandler(this.EXEPatcherButton_Click);
            // 
            // ImageMakerButton
            // 
            this.ImageMakerButton.BackColor = System.Drawing.Color.Silver;
            this.ImageMakerButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ImageMakerButton.FlatAppearance.BorderSize = 0;
            this.ImageMakerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ImageMakerButton.Font = new System.Drawing.Font("Twelve Ton Goldfish", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ImageMakerButton.Location = new System.Drawing.Point(7, 272);
            this.ImageMakerButton.Name = "ImageMakerButton";
            this.ImageMakerButton.Size = new System.Drawing.Size(112, 38);
            this.ImageMakerButton.TabIndex = 5;
            this.ImageMakerButton.Text = "Image Maker";
            this.ImageMakerButton.UseVisualStyleBackColor = false;
            this.ImageMakerButton.Click += new System.EventHandler(this.button6_Click);
            // 
            // BDBHButton
            // 
            this.BDBHButton.BackColor = System.Drawing.Color.Silver;
            this.BDBHButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BDBHButton.FlatAppearance.BorderSize = 0;
            this.BDBHButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BDBHButton.Font = new System.Drawing.Font("Twelve Ton Goldfish", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BDBHButton.Location = new System.Drawing.Point(7, 228);
            this.BDBHButton.Name = "BDBHButton";
            this.BDBHButton.Size = new System.Drawing.Size(112, 38);
            this.BDBHButton.TabIndex = 4;
            this.BDBHButton.Text = "BD/BH Tool";
            this.BDBHButton.UseVisualStyleBackColor = false;
            this.BDBHButton.Click += new System.EventHandler(this.button5_Click);
            // 
            // MHMBToolButton
            // 
            this.MHMBToolButton.BackColor = System.Drawing.Color.Silver;
            this.MHMBToolButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MHMBToolButton.FlatAppearance.BorderSize = 0;
            this.MHMBToolButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MHMBToolButton.Font = new System.Drawing.Font("Twelve Ton Goldfish", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MHMBToolButton.Location = new System.Drawing.Point(7, 316);
            this.MHMBToolButton.Name = "MHMBToolButton";
            this.MHMBToolButton.Size = new System.Drawing.Size(112, 38);
            this.MHMBToolButton.TabIndex = 3;
            this.MHMBToolButton.Text = "MH/MB Tool";
            this.MHMBToolButton.UseVisualStyleBackColor = false;
            this.MHMBToolButton.Click += new System.EventHandler(this.MHMBToolButton_Click);
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
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(111, 19);
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
            this.mHMBToolToolStripMenuItem,
            this.bHBDToolToolStripMenuItem,
            this.imageMakerToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(111, 19);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // eLFPatcherToolStripMenuItem
            // 
            this.eLFPatcherToolStripMenuItem.Name = "eLFPatcherToolStripMenuItem";
            this.eLFPatcherToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.eLFPatcherToolStripMenuItem.Text = "EXE Patcher";
            this.eLFPatcherToolStripMenuItem.Click += new System.EventHandler(this.eLFPatcherToolStripMenuItem_Click);
            // 
            // mHMBToolToolStripMenuItem
            // 
            this.mHMBToolToolStripMenuItem.Name = "mHMBToolToolStripMenuItem";
            this.mHMBToolToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.mHMBToolToolStripMenuItem.Text = "MH/MB Tool";
            this.mHMBToolToolStripMenuItem.Click += new System.EventHandler(this.mHMBToolToolStripMenuItem_Click);
            // 
            // bHBDToolToolStripMenuItem
            // 
            this.bHBDToolToolStripMenuItem.Name = "bHBDToolToolStripMenuItem";
            this.bHBDToolToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.bHBDToolToolStripMenuItem.Text = "BH/BD Tool";
            this.bHBDToolToolStripMenuItem.Click += new System.EventHandler(this.bHBDToolToolStripMenuItem_Click);
            // 
            // imageMakerToolStripMenuItem
            // 
            this.imageMakerToolStripMenuItem.Name = "imageMakerToolStripMenuItem";
            this.imageMakerToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.imageMakerToolStripMenuItem.Text = "Image Maker";
            this.imageMakerToolStripMenuItem.Click += new System.EventHandler(this.imageMakerToolStripMenuItem_Click);
            // 
            // viewersToolStripMenuItem
            // 
            this.viewersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rMViewerToolStripMenuItem,
            this.sMViewerToolStripMenuItem});
            this.viewersToolStripMenuItem.Name = "viewersToolStripMenuItem";
            this.viewersToolStripMenuItem.Size = new System.Drawing.Size(111, 19);
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
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(111, 19);
            this.preferencesToolStripMenuItem.Text = "Preferences";
            // 
            // Pref_TruncateNames_toolStripMenuItem
            // 
            this.Pref_TruncateNames_toolStripMenuItem.Checked = true;
            this.Pref_TruncateNames_toolStripMenuItem.CheckOnClick = true;
            this.Pref_TruncateNames_toolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Pref_TruncateNames_toolStripMenuItem.Name = "Pref_TruncateNames_toolStripMenuItem";
            this.Pref_TruncateNames_toolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.Pref_TruncateNames_toolStripMenuItem.Text = "Truncate Object Names";
            this.Pref_TruncateNames_toolStripMenuItem.Click += new System.EventHandler(this.Pref_TruncateNames_Click);
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
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(111, 19);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // themesToolStripMenuItem
            // 
            this.themesToolStripMenuItem.Name = "themesToolStripMenuItem";
            this.themesToolStripMenuItem.Size = new System.Drawing.Size(111, 19);
            this.themesToolStripMenuItem.Text = "Themes";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Left;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.viewersToolStripMenuItem,
            this.preferencesToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.themesToolStripMenuItem,
            this.thisIsFillerToFixLayoutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(124, 513);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // thisIsFillerToFixLayoutToolStripMenuItem
            // 
            this.thisIsFillerToFixLayoutToolStripMenuItem.Name = "thisIsFillerToFixLayoutToolStripMenuItem";
            this.thisIsFillerToFixLayoutToolStripMenuItem.Size = new System.Drawing.Size(111, 19);
            this.thisIsFillerToFixLayoutToolStripMenuItem.Text = "ThisIsFillerToFixLay";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.textBox4);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.textBox3);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(126, 70);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox4.Font = new System.Drawing.Font("Twelve Ton Goldfish", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox4.ForeColor = System.Drawing.Color.White;
            this.textBox4.Location = new System.Drawing.Point(65, 44);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(48, 26);
            this.textBox4.TabIndex = 3;
            this.textBox4.TabStop = false;
            this.textBox4.Text = "v0.30";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(5, 23);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(46, 44);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox3.Font = new System.Drawing.Font("Twelve Ton Goldfish", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.ForeColor = System.Drawing.Color.White;
            this.textBox3.Location = new System.Drawing.Point(48, 23);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(65, 26);
            this.textBox3.TabIndex = 1;
            this.textBox3.TabStop = false;
            this.textBox3.Text = " Editor";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox2.Font = new System.Drawing.Font("Twelve Ton Goldfish", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.ForeColor = System.Drawing.Color.White;
            this.textBox2.Location = new System.Drawing.Point(3, 3);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(100, 26);
            this.textBox2.TabIndex = 0;
            this.textBox2.TabStop = false;
            this.textBox2.Text = "Twinsanity";
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(895, 513);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Twinsaity Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openRM2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem newRM2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newSM2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eLFPatcherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mHMBToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bHBDToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageMakerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rMViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sMViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Pref_TruncateNames_toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Pref_EnableAllNames_toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem themesToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button AboutButton;
        private System.Windows.Forms.Button EXEPatcherButton;
        private System.Windows.Forms.Button ImageMakerButton;
        private System.Windows.Forms.Button BDBHButton;
        private System.Windows.Forms.Button MHMBToolButton;
        private System.Windows.Forms.Button RM2ViewerButton;
        private System.Windows.Forms.Button SaveButtonMain;
        private System.Windows.Forms.Button OpenFileButton;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.ToolStripMenuItem thisIsFillerToFixLayoutToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button SaveAsButton;
    }
}