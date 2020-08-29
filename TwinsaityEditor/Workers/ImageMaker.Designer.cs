namespace TwinsaityEditor.Workers
{
    partial class ImageMaker
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbTwinsanityPath = new System.Windows.Forms.TextBox();
            this.tbImageName = new System.Windows.Forms.TextBox();
            this.tbOutputPath = new System.Windows.Forms.TextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnSelectTwinsPath = new System.Windows.Forms.Button();
            this.btnOutputPath = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslblCurrentFile = new System.Windows.Forms.ToolStripStatusLabel();
            this.tspbGenerationProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Twinsanity path:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Output path:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Image name:";
            // 
            // tbTwinsanityPath
            // 
            this.tbTwinsanityPath.Location = new System.Drawing.Point(103, 10);
            this.tbTwinsanityPath.Name = "tbTwinsanityPath";
            this.tbTwinsanityPath.ReadOnly = true;
            this.tbTwinsanityPath.Size = new System.Drawing.Size(339, 20);
            this.tbTwinsanityPath.TabIndex = 3;
            // 
            // tbImageName
            // 
            this.tbImageName.Location = new System.Drawing.Point(103, 36);
            this.tbImageName.MaxLength = 200;
            this.tbImageName.Name = "tbImageName";
            this.tbImageName.Size = new System.Drawing.Size(339, 20);
            this.tbImageName.TabIndex = 4;
            this.tbImageName.TextChanged += new System.EventHandler(this.tbImageName_TextChanged);
            // 
            // tbOutputPath
            // 
            this.tbOutputPath.Location = new System.Drawing.Point(103, 62);
            this.tbOutputPath.Name = "tbOutputPath";
            this.tbOutputPath.ReadOnly = true;
            this.tbOutputPath.Size = new System.Drawing.Size(339, 20);
            this.tbOutputPath.TabIndex = 5;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(13, 91);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 6;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnSelectTwinsPath
            // 
            this.btnSelectTwinsPath.Location = new System.Drawing.Point(448, 8);
            this.btnSelectTwinsPath.Name = "btnSelectTwinsPath";
            this.btnSelectTwinsPath.Size = new System.Drawing.Size(28, 23);
            this.btnSelectTwinsPath.TabIndex = 7;
            this.btnSelectTwinsPath.Text = "...";
            this.btnSelectTwinsPath.UseVisualStyleBackColor = true;
            this.btnSelectTwinsPath.Click += new System.EventHandler(this.btnSelectTwinsPath_Click);
            // 
            // btnOutputPath
            // 
            this.btnOutputPath.Location = new System.Drawing.Point(448, 60);
            this.btnOutputPath.Name = "btnOutputPath";
            this.btnOutputPath.Size = new System.Drawing.Size(28, 23);
            this.btnOutputPath.TabIndex = 8;
            this.btnOutputPath.Text = "...";
            this.btnOutputPath.UseVisualStyleBackColor = true;
            this.btnOutputPath.Click += new System.EventHandler(this.btnOutputPath_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslblCurrentFile,
            this.tspbGenerationProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 118);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(478, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslblCurrentFile
            // 
            this.tsslblCurrentFile.Name = "tsslblCurrentFile";
            this.tsslblCurrentFile.Size = new System.Drawing.Size(72, 17);
            this.tsslblCurrentFile.Text = "Current file: ";
            // 
            // tspbGenerationProgress
            // 
            this.tspbGenerationProgress.Name = "tspbGenerationProgress";
            this.tspbGenerationProgress.Size = new System.Drawing.Size(100, 16);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ImageMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 140);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnOutputPath);
            this.Controls.Add(this.btnSelectTwinsPath);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.tbOutputPath);
            this.Controls.Add(this.tbImageName);
            this.Controls.Add(this.tbTwinsanityPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ImageMaker";
            this.Text = "ImageMaker";
            this.Load += new System.EventHandler(this.ImageMaker_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbTwinsanityPath;
        private System.Windows.Forms.TextBox tbImageName;
        private System.Windows.Forms.TextBox tbOutputPath;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnSelectTwinsPath;
        private System.Windows.Forms.Button btnOutputPath;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslblCurrentFile;
        private System.Windows.Forms.ToolStripProgressBar tspbGenerationProgress;
        private System.Windows.Forms.Timer timer1;
    }
}