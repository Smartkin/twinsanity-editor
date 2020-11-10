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
    partial class TextureViewer : System.Windows.Forms.Form
    {

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

        private System.ComponentModel.IContainer components;


        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextureViewer));
            this.GlControl1 = new OpenTK.GLControl();
            this.Button1 = new System.Windows.Forms.Button();
            this.SavePNG = new System.Windows.Forms.SaveFileDialog();
            this.btnPrevTexture = new System.Windows.Forms.Button();
            this.btnNextTexture = new System.Windows.Forms.Button();
            this.lblTextureIndex = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // GlControl1
            // 
            this.GlControl1.BackColor = System.Drawing.Color.Black;
            this.GlControl1.Location = new System.Drawing.Point(13, 13);
            this.GlControl1.Name = "GlControl1";
            this.GlControl1.Size = new System.Drawing.Size(256, 256);
            this.GlControl1.TabIndex = 0;
            this.GlControl1.VSync = false;
            this.GlControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.GlControl1_Paint);
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(13, 275);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(75, 23);
            this.Button1.TabIndex = 1;
            this.Button1.Text = "Save";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // SavePNG
            // 
            this.SavePNG.Filter = "PNG Image (*.png)|*.png";
            // 
            // btnPrevTexture
            // 
            this.btnPrevTexture.Location = new System.Drawing.Point(133, 275);
            this.btnPrevTexture.Name = "btnPrevTexture";
            this.btnPrevTexture.Size = new System.Drawing.Size(45, 23);
            this.btnPrevTexture.TabIndex = 2;
            this.btnPrevTexture.Text = "Prev";
            this.btnPrevTexture.UseVisualStyleBackColor = true;
            this.btnPrevTexture.Click += new System.EventHandler(this.btnPrevTexture_Click);
            // 
            // btnNextTexture
            // 
            this.btnNextTexture.Location = new System.Drawing.Point(225, 275);
            this.btnNextTexture.Name = "btnNextTexture";
            this.btnNextTexture.Size = new System.Drawing.Size(43, 23);
            this.btnNextTexture.TabIndex = 3;
            this.btnNextTexture.Text = "Next";
            this.btnNextTexture.UseVisualStyleBackColor = true;
            this.btnNextTexture.Click += new System.EventHandler(this.btnNextTexture_Click);
            // 
            // lblTextureIndex
            // 
            this.lblTextureIndex.AutoSize = true;
            this.lblTextureIndex.Location = new System.Drawing.Point(190, 280);
            this.lblTextureIndex.Name = "lblTextureIndex";
            this.lblTextureIndex.Size = new System.Drawing.Size(24, 13);
            this.lblTextureIndex.TabIndex = 4;
            this.lblTextureIndex.Text = "0/0";
            this.lblTextureIndex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextureViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 306);
            this.Controls.Add(this.lblTextureIndex);
            this.Controls.Add(this.btnNextTexture);
            this.Controls.Add(this.btnPrevTexture);
            this.Controls.Add(this.Button1);
            this.Controls.Add(this.GlControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TextureViewer";
            this.Text = "TextureViewer";
            this.Load += new System.EventHandler(this.TextureViewer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.Panel Panel1;
        private OpenTK.GLControl GlControl1;
        private System.Windows.Forms.Button Button1;
        private System.Windows.Forms.SaveFileDialog SavePNG;
        private Button btnPrevTexture;
        private Button btnNextTexture;
        private Label lblTextureIndex;
    }
}
