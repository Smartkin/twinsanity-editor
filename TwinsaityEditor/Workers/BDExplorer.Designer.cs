namespace TwinsaityEditor
{ 
    partial class BDExplorer
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
            this.archiveContentsTree = new System.Windows.Forms.TreeView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusBar = new System.Windows.Forms.ToolStripStatusLabel();
            this.OpenBDBH = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // archiveContentsTree
            // 
            this.archiveContentsTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.archiveContentsTree.Location = new System.Drawing.Point(0, 0);
            this.archiveContentsTree.Name = "archiveContentsTree";
            this.archiveContentsTree.Size = new System.Drawing.Size(550, 534);
            this.archiveContentsTree.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 512);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(550, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusBar
            // 
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(39, 17);
            this.statusBar.Text = "Ready";
            // 
            // OpenBDBH
            // 
            this.OpenBDBH.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.OpenBDBH.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.OpenBDBH.Font = new System.Drawing.Font("Twelve Ton Goldfish", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpenBDBH.Location = new System.Drawing.Point(321, 12);
            this.OpenBDBH.Name = "OpenBDBH";
            this.OpenBDBH.Size = new System.Drawing.Size(199, 85);
            this.OpenBDBH.TabIndex = 3;
            this.OpenBDBH.Text = "Open BD/BH";
            this.OpenBDBH.UseVisualStyleBackColor = false;
            this.OpenBDBH.Click += new System.EventHandler(this.button1_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SaveButton.Font = new System.Drawing.Font("Twelve Ton Goldfish", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveButton.Location = new System.Drawing.Point(321, 113);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(199, 85);
            this.SaveButton.TabIndex = 4;
            this.SaveButton.Text = "Save BD/BH";
            this.SaveButton.UseVisualStyleBackColor = false;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Font = new System.Drawing.Font("Twelve Ton Goldfish", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(321, 214);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(199, 85);
            this.button2.TabIndex = 5;
            this.button2.Text = "Extract All";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Font = new System.Drawing.Font("Twelve Ton Goldfish", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(321, 314);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(199, 85);
            this.button3.TabIndex = 6;
            this.button3.Text = "Extract Selected";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Firebrick;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button4.Font = new System.Drawing.Font("Twelve Ton Goldfish", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(321, 415);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(199, 85);
            this.button4.TabIndex = 7;
            this.button4.Text = "EXIT";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // BDExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 534);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.OpenBDBH);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.archiveContentsTree);
            this.MaximumSize = new System.Drawing.Size(566, 99999);
            this.MinimumSize = new System.Drawing.Size(566, 573);
            this.Name = "BDExplorer";
            this.Text = "BDExplorer";
            this.Load += new System.EventHandler(this.BDExplorer_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView archiveContentsTree;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusBar;
        private System.Windows.Forms.Button OpenBDBH;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}