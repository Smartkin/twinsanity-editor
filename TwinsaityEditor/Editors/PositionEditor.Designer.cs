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
    partial class PositionEditor : System.Windows.Forms.Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PositionEditor));
            this.PosTree = new System.Windows.Forms.TreeView();
            this.Apply = new System.Windows.Forms.Button();
            this.Revert = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.XVal = new System.Windows.Forms.TextBox();
            this.YVal = new System.Windows.Forms.TextBox();
            this.ZVal = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // PosTree
            // 
            this.PosTree.Location = new System.Drawing.Point(13, 13);
            this.PosTree.Name = "PosTree";
            this.PosTree.Size = new System.Drawing.Size(121, 147);
            this.PosTree.TabIndex = 0;
            // 
            // Apply
            // 
            this.Apply.Location = new System.Drawing.Point(143, 104);
            this.Apply.Name = "Apply";
            this.Apply.Size = new System.Drawing.Size(121, 23);
            this.Apply.TabIndex = 1;
            this.Apply.Text = "Apply";
            this.Apply.UseVisualStyleBackColor = true;
            this.Apply.Click += new System.EventHandler(this.Apply_Click);
            // 
            // Revert
            // 
            this.Revert.Location = new System.Drawing.Point(143, 134);
            this.Revert.Name = "Revert";
            this.Revert.Size = new System.Drawing.Size(121, 23);
            this.Revert.TabIndex = 2;
            this.Revert.Text = "Revert";
            this.Revert.UseVisualStyleBackColor = true;
            this.Revert.Click += new System.EventHandler(this.Revert_Click);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(141, 28);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(17, 13);
            this.Label1.TabIndex = 3;
            this.Label1.Text = "X:";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(140, 54);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(17, 13);
            this.Label2.TabIndex = 4;
            this.Label2.Text = "Y:";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(140, 81);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(17, 13);
            this.Label3.TabIndex = 5;
            this.Label3.Text = "Z:";
            // 
            // XVal
            // 
            this.XVal.Location = new System.Drawing.Point(164, 25);
            this.XVal.Name = "XVal";
            this.XVal.Size = new System.Drawing.Size(100, 20);
            this.XVal.TabIndex = 6;
            // 
            // YVal
            // 
            this.YVal.Location = new System.Drawing.Point(164, 51);
            this.YVal.Name = "YVal";
            this.YVal.Size = new System.Drawing.Size(100, 20);
            this.YVal.TabIndex = 7;
            // 
            // ZVal
            // 
            this.ZVal.Location = new System.Drawing.Point(164, 78);
            this.ZVal.Name = "ZVal";
            this.ZVal.Size = new System.Drawing.Size(100, 20);
            this.ZVal.TabIndex = 8;
            // 
            // PositionEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 175);
            this.Controls.Add(this.ZVal);
            this.Controls.Add(this.YVal);
            this.Controls.Add(this.XVal);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.Revert);
            this.Controls.Add(this.Apply);
            this.Controls.Add(this.PosTree);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PositionEditor";
            this.Text = "Position Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public System.Windows.Forms.TreeView PosTree;
        private System.Windows.Forms.Button Apply;
        private System.Windows.Forms.Button Revert;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Label Label2;
        private System.Windows.Forms.Label Label3;
        private System.Windows.Forms.TextBox XVal;
        private System.Windows.Forms.TextBox YVal;
        private System.Windows.Forms.TextBox ZVal;
    }
}
