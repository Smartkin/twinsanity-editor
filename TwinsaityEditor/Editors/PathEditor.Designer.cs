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
    partial class PathEditor : System.Windows.Forms.Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PathEditor));
            this.Pathes = new System.Windows.Forms.TreeView();
            this.Positinos = new System.Windows.Forms.ListBox();
            this.Somethings = new System.Windows.Forms.ListBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.AddPosition = new System.Windows.Forms.Button();
            this.DelPosition = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.Button1 = new System.Windows.Forms.Button();
            this.Button2 = new System.Windows.Forms.Button();
            this.Button3 = new System.Windows.Forms.Button();
            this.XPos = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.YPos = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.ZPos = new System.Windows.Forms.TextBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.Int2Val = new System.Windows.Forms.TextBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.Int1Val = new System.Windows.Forms.TextBox();
            this.Button4 = new System.Windows.Forms.Button();
            this.Button5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Pathes
            // 
            this.Pathes.Location = new System.Drawing.Point(12, 12);
            this.Pathes.Name = "Pathes";
            this.Pathes.Size = new System.Drawing.Size(155, 238);
            this.Pathes.TabIndex = 0;
            // 
            // Positinos
            // 
            this.Positinos.FormattingEnabled = true;
            this.Positinos.Location = new System.Drawing.Point(173, 33);
            this.Positinos.Name = "Positinos";
            this.Positinos.Size = new System.Drawing.Size(73, 69);
            this.Positinos.TabIndex = 1;
            // 
            // Somethings
            // 
            this.Somethings.FormattingEnabled = true;
            this.Somethings.Location = new System.Drawing.Point(173, 165);
            this.Somethings.Name = "Somethings";
            this.Somethings.Size = new System.Drawing.Size(73, 82);
            this.Somethings.TabIndex = 2;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(173, 17);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(49, 13);
            this.Label1.TabIndex = 3;
            this.Label1.Text = "Positions";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(173, 149);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(62, 13);
            this.Label2.TabIndex = 4;
            this.Label2.Text = "Somethings";
            // 
            // AddPosition
            // 
            this.AddPosition.Location = new System.Drawing.Point(252, 33);
            this.AddPosition.Name = "AddPosition";
            this.AddPosition.Size = new System.Drawing.Size(52, 23);
            this.AddPosition.TabIndex = 5;
            this.AddPosition.Text = "Add";
            this.AddPosition.UseVisualStyleBackColor = true;
            this.AddPosition.Click += new System.EventHandler(this.AddPosition_Click);
            // 
            // DelPosition
            // 
            this.DelPosition.Location = new System.Drawing.Point(252, 63);
            this.DelPosition.Name = "DelPosition";
            this.DelPosition.Size = new System.Drawing.Size(52, 23);
            this.DelPosition.TabIndex = 6;
            this.DelPosition.Text = "Del";
            this.DelPosition.UseVisualStyleBackColor = true;
            this.DelPosition.Click += new System.EventHandler(this.DelPosition_Click);
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(252, 92);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(52, 23);
            this.Save.TabIndex = 7;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(252, 165);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(52, 23);
            this.Button1.TabIndex = 8;
            this.Button1.Text = "Add";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Button2
            // 
            this.Button2.Location = new System.Drawing.Point(252, 194);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(52, 23);
            this.Button2.TabIndex = 9;
            this.Button2.Text = "Del";
            this.Button2.UseVisualStyleBackColor = true;
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // Button3
            // 
            this.Button3.Location = new System.Drawing.Point(252, 223);
            this.Button3.Name = "Button3";
            this.Button3.Size = new System.Drawing.Size(52, 23);
            this.Button3.TabIndex = 10;
            this.Button3.Text = "Save";
            this.Button3.UseVisualStyleBackColor = true;
            this.Button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // XPos
            // 
            this.XPos.Location = new System.Drawing.Point(334, 35);
            this.XPos.Name = "XPos";
            this.XPos.Size = new System.Drawing.Size(100, 20);
            this.XPos.TabIndex = 11;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(311, 39);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(17, 13);
            this.Label3.TabIndex = 12;
            this.Label3.Text = "X:";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(311, 69);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(17, 13);
            this.Label4.TabIndex = 14;
            this.Label4.Text = "Y:";
            // 
            // YPos
            // 
            this.YPos.Location = new System.Drawing.Point(334, 65);
            this.YPos.Name = "YPos";
            this.YPos.Size = new System.Drawing.Size(100, 20);
            this.YPos.TabIndex = 13;
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(311, 95);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(17, 13);
            this.Label5.TabIndex = 16;
            this.Label5.Text = "Z:";
            // 
            // ZPos
            // 
            this.ZPos.Location = new System.Drawing.Point(334, 91);
            this.ZPos.Name = "ZPos";
            this.ZPos.Size = new System.Drawing.Size(100, 20);
            this.ZPos.TabIndex = 15;
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(310, 199);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(28, 13);
            this.Label6.TabIndex = 20;
            this.Label6.Text = "Int2:";
            // 
            // Int2Val
            // 
            this.Int2Val.Location = new System.Drawing.Point(345, 198);
            this.Int2Val.Name = "Int2Val";
            this.Int2Val.Size = new System.Drawing.Size(100, 20);
            this.Int2Val.TabIndex = 19;
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(310, 170);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(28, 13);
            this.Label7.TabIndex = 18;
            this.Label7.Text = "Int1:";
            // 
            // Int1Val
            // 
            this.Int1Val.Location = new System.Drawing.Point(345, 167);
            this.Int1Val.Name = "Int1Val";
            this.Int1Val.Size = new System.Drawing.Size(100, 20);
            this.Int1Val.TabIndex = 17;
            // 
            // Button4
            // 
            this.Button4.Location = new System.Drawing.Point(12, 256);
            this.Button4.Name = "Button4";
            this.Button4.Size = new System.Drawing.Size(75, 23);
            this.Button4.TabIndex = 21;
            this.Button4.Text = "Apply";
            this.Button4.UseVisualStyleBackColor = true;
            this.Button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // Button5
            // 
            this.Button5.Location = new System.Drawing.Point(92, 256);
            this.Button5.Name = "Button5";
            this.Button5.Size = new System.Drawing.Size(75, 23);
            this.Button5.TabIndex = 22;
            this.Button5.Text = "Revert";
            this.Button5.UseVisualStyleBackColor = true;
            this.Button5.Click += new System.EventHandler(this.Button5_Click);
            // 
            // PathEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 288);
            this.Controls.Add(this.Button5);
            this.Controls.Add(this.Button4);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.Int2Val);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.Int1Val);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.ZPos);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.YPos);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.XPos);
            this.Controls.Add(this.Button3);
            this.Controls.Add(this.Button2);
            this.Controls.Add(this.Button1);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.DelPosition);
            this.Controls.Add(this.AddPosition);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.Somethings);
            this.Controls.Add(this.Positinos);
            this.Controls.Add(this.Pathes);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PathEditor";
            this.Text = "Path Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public System.Windows.Forms.TreeView Pathes;
        private System.Windows.Forms.ListBox Positinos;
        private System.Windows.Forms.ListBox Somethings;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Label Label2;
        private System.Windows.Forms.Button AddPosition;
        private System.Windows.Forms.Button DelPosition;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Button Button1;
        private System.Windows.Forms.Button Button2;
        private System.Windows.Forms.Button Button3;
        private TextBox XPos;
        private Label Label3;
        private Label Label4;
        private TextBox YPos;
        private Label Label5;
        private TextBox ZPos;
        private Label Label6;
        private TextBox Int2Val;
        private Label Label7;
        private TextBox Int1Val;
        private Button Button4;
        private Button Button5;
    }
}
