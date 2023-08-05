namespace TwinsaityEditor
{
    partial class AnimationEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnimationEditor));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbAnimations = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tcProperties = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.tbJointTimelineView = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbTransformOffset = new System.Windows.Forms.TextBox();
            this.lbAnimatedTransforms = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tbTransformation = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lbTransformations = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbAddRotation = new System.Windows.Forms.CheckBox();
            this.cbParentScale = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbJointAnimatedTransformIndex = new System.Windows.Forms.TextBox();
            this.tbJointTransformIndex = new System.Windows.Forms.TextBox();
            this.tbJointTransformChoice = new System.Windows.Forms.TextBox();
            this.lbJointSettings = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.tbMorphTimeline = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.tbTransformOffset2 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.lbTwoPartTransforms2 = new System.Windows.Forms.ListBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.tbTransformation2 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.lbTransformations2 = new System.Windows.Forms.ListBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.tbJointTwoPartTransformIndex2 = new System.Windows.Forms.TextBox();
            this.tbJointTransformIndex2 = new System.Windows.Forms.TextBox();
            this.tbJointTransformChoice2 = new System.Windows.Forms.TextBox();
            this.tbShapesAmount = new System.Windows.Forms.TextBox();
            this.lbShapeSettings = new System.Windows.Forms.ListBox();
            this.tpPreview = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbAnimationTimeline = new System.Windows.Forms.TrackBar();
            this.cbLoop = new System.Windows.Forms.CheckBox();
            this.btnPlayAnim = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbPlaybackFps = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbOGIList = new System.Windows.Forms.ComboBox();
            this.cbShowSkeleton = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tcProperties.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.tpPreview.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbAnimationTimeline)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbAnimations);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(256, 583);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Animations";
            // 
            // lbAnimations
            // 
            this.lbAnimations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbAnimations.FormattingEnabled = true;
            this.lbAnimations.Location = new System.Drawing.Point(3, 16);
            this.lbAnimations.Name = "lbAnimations";
            this.lbAnimations.Size = new System.Drawing.Size(250, 564);
            this.lbAnimations.TabIndex = 0;
            this.lbAnimations.SelectedIndexChanged += new System.EventHandler(this.lbAnimations_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tcProperties);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(256, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(862, 583);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Properties";
            // 
            // tcProperties
            // 
            this.tcProperties.Controls.Add(this.tabPage1);
            this.tcProperties.Controls.Add(this.tabPage2);
            this.tcProperties.Controls.Add(this.tpPreview);
            this.tcProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcProperties.Location = new System.Drawing.Point(3, 16);
            this.tcProperties.Name = "tcProperties";
            this.tcProperties.SelectedIndex = 0;
            this.tcProperties.Size = new System.Drawing.Size(856, 564);
            this.tcProperties.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox10);
            this.tabPage1.Controls.Add(this.groupBox5);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(848, 538);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main Animation";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox10
            // 
            this.groupBox10.AutoSize = true;
            this.groupBox10.Controls.Add(this.tbJointTimelineView);
            this.groupBox10.Location = new System.Drawing.Point(270, 309);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(559, 214);
            this.groupBox10.TabIndex = 3;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Joint timeline view";
            // 
            // tbJointTimelineView
            // 
            this.tbJointTimelineView.AcceptsReturn = true;
            this.tbJointTimelineView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbJointTimelineView.Location = new System.Drawing.Point(3, 16);
            this.tbJointTimelineView.Multiline = true;
            this.tbJointTimelineView.Name = "tbJointTimelineView";
            this.tbJointTimelineView.ReadOnly = true;
            this.tbJointTimelineView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbJointTimelineView.Size = new System.Drawing.Size(553, 195);
            this.tbJointTimelineView.TabIndex = 1;
            // 
            // groupBox5
            // 
            this.groupBox5.AutoSize = true;
            this.groupBox5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.tbTransformOffset);
            this.groupBox5.Controls.Add(this.lbAnimatedTransforms);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox5.Location = new System.Drawing.Point(497, 3);
            this.groupBox5.MaximumSize = new System.Drawing.Size(0, 300);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(336, 300);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Animated transforms";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Right;
            this.label9.Location = new System.Drawing.Point(3, 215);
            this.label9.Name = "label9";
            this.label9.Padding = new System.Windows.Forms.Padding(3);
            this.label9.Size = new System.Drawing.Size(72, 19);
            this.label9.TabIndex = 5;
            this.label9.Text = "Value (Float)";
            // 
            // tbTransformOffset
            // 
            this.tbTransformOffset.Dock = System.Windows.Forms.DockStyle.Right;
            this.tbTransformOffset.Location = new System.Drawing.Point(75, 215);
            this.tbTransformOffset.Name = "tbTransformOffset";
            this.tbTransformOffset.Size = new System.Drawing.Size(258, 20);
            this.tbTransformOffset.TabIndex = 4;
            this.tbTransformOffset.TextChanged += new System.EventHandler(this.tbTransformOffset_TextChanged);
            // 
            // lbAnimatedTransforms
            // 
            this.lbAnimatedTransforms.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbAnimatedTransforms.FormattingEnabled = true;
            this.lbAnimatedTransforms.Location = new System.Drawing.Point(3, 16);
            this.lbAnimatedTransforms.Name = "lbAnimatedTransforms";
            this.lbAnimatedTransforms.Size = new System.Drawing.Size(330, 199);
            this.lbAnimatedTransforms.TabIndex = 0;
            this.lbAnimatedTransforms.SelectedIndexChanged += new System.EventHandler(this.lbRotations_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.AutoSize = true;
            this.groupBox4.Controls.Add(this.tbTransformation);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.lbTransformations);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox4.Location = new System.Drawing.Point(267, 3);
            this.groupBox4.MaximumSize = new System.Drawing.Size(0, 300);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(230, 300);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Static transforms";
            // 
            // tbTransformation
            // 
            this.tbTransformation.Dock = System.Windows.Forms.DockStyle.Left;
            this.tbTransformation.Location = new System.Drawing.Point(72, 215);
            this.tbTransformation.Name = "tbTransformation";
            this.tbTransformation.Size = new System.Drawing.Size(155, 20);
            this.tbTransformation.TabIndex = 20;
            this.tbTransformation.TextChanged += new System.EventHandler(this.tbTransformation_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Left;
            this.label10.Location = new System.Drawing.Point(3, 215);
            this.label10.Margin = new System.Windows.Forms.Padding(3);
            this.label10.Name = "label10";
            this.label10.Padding = new System.Windows.Forms.Padding(3);
            this.label10.Size = new System.Drawing.Size(69, 19);
            this.label10.TabIndex = 19;
            this.label10.Text = "Value(Float)";
            // 
            // lbTransformations
            // 
            this.lbTransformations.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbTransformations.FormattingEnabled = true;
            this.lbTransformations.Location = new System.Drawing.Point(3, 16);
            this.lbTransformations.Name = "lbTransformations";
            this.lbTransformations.Size = new System.Drawing.Size(224, 199);
            this.lbTransformations.TabIndex = 0;
            this.lbTransformations.SelectedIndexChanged += new System.EventHandler(this.lbScales_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSize = true;
            this.groupBox3.Controls.Add(this.cbAddRotation);
            this.groupBox3.Controls.Add(this.cbParentScale);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.tbJointAnimatedTransformIndex);
            this.groupBox3.Controls.Add(this.tbJointTransformIndex);
            this.groupBox3.Controls.Add(this.tbJointTransformChoice);
            this.groupBox3.Controls.Add(this.lbJointSettings);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.MinimumSize = new System.Drawing.Size(250, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(264, 532);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Joint settings";
            // 
            // cbAddRotation
            // 
            this.cbAddRotation.AutoSize = true;
            this.cbAddRotation.Location = new System.Drawing.Point(127, 378);
            this.cbAddRotation.Name = "cbAddRotation";
            this.cbAddRotation.Size = new System.Drawing.Size(131, 17);
            this.cbAddRotation.TabIndex = 19;
            this.cbAddRotation.Text = "Add additional rotation";
            this.cbAddRotation.UseVisualStyleBackColor = true;
            // 
            // cbParentScale
            // 
            this.cbParentScale.AutoSize = true;
            this.cbParentScale.Location = new System.Drawing.Point(7, 378);
            this.cbParentScale.Name = "cbParentScale";
            this.cbParentScale.Size = new System.Drawing.Size(113, 17);
            this.cbParentScale.TabIndex = 18;
            this.cbParentScale.Text = "Use parent\'s scale";
            this.cbParentScale.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 457);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(125, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Animated transform index";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 431);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Static transform index";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 405);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Transform choice";
            // 
            // tbJointAnimatedTransformIndex
            // 
            this.tbJointAnimatedTransformIndex.Location = new System.Drawing.Point(144, 452);
            this.tbJointAnimatedTransformIndex.Name = "tbJointAnimatedTransformIndex";
            this.tbJointAnimatedTransformIndex.Size = new System.Drawing.Size(100, 20);
            this.tbJointAnimatedTransformIndex.TabIndex = 9;
            this.tbJointAnimatedTransformIndex.TextChanged += new System.EventHandler(this.tbDisB7_TextChanged);
            // 
            // tbJointTransformIndex
            // 
            this.tbJointTransformIndex.Location = new System.Drawing.Point(144, 428);
            this.tbJointTransformIndex.Name = "tbJointTransformIndex";
            this.tbJointTransformIndex.Size = new System.Drawing.Size(100, 20);
            this.tbJointTransformIndex.TabIndex = 7;
            this.tbJointTransformIndex.TextChanged += new System.EventHandler(this.tbDisB5_TextChanged);
            // 
            // tbJointTransformChoice
            // 
            this.tbJointTransformChoice.Location = new System.Drawing.Point(144, 402);
            this.tbJointTransformChoice.Name = "tbJointTransformChoice";
            this.tbJointTransformChoice.Size = new System.Drawing.Size(100, 20);
            this.tbJointTransformChoice.TabIndex = 5;
            this.tbJointTransformChoice.TextChanged += new System.EventHandler(this.tbDisB3_TextChanged);
            // 
            // lbJointSettings
            // 
            this.lbJointSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbJointSettings.FormattingEnabled = true;
            this.lbJointSettings.Location = new System.Drawing.Point(3, 16);
            this.lbJointSettings.Name = "lbJointSettings";
            this.lbJointSettings.Size = new System.Drawing.Size(258, 355);
            this.lbJointSettings.TabIndex = 0;
            this.lbJointSettings.SelectedIndexChanged += new System.EventHandler(this.lbDisplacements_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox11);
            this.tabPage2.Controls.Add(this.groupBox6);
            this.tabPage2.Controls.Add(this.groupBox7);
            this.tabPage2.Controls.Add(this.groupBox8);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(848, 538);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Blend shape animation";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox11
            // 
            this.groupBox11.AutoSize = true;
            this.groupBox11.Controls.Add(this.tbMorphTimeline);
            this.groupBox11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox11.Location = new System.Drawing.Point(448, 351);
            this.groupBox11.MinimumSize = new System.Drawing.Size(180, 0);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(397, 184);
            this.groupBox11.TabIndex = 6;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Facial morph timeline";
            // 
            // tbMorphTimeline
            // 
            this.tbMorphTimeline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMorphTimeline.Location = new System.Drawing.Point(3, 16);
            this.tbMorphTimeline.Multiline = true;
            this.tbMorphTimeline.Name = "tbMorphTimeline";
            this.tbMorphTimeline.ReadOnly = true;
            this.tbMorphTimeline.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbMorphTimeline.Size = new System.Drawing.Size(391, 165);
            this.tbMorphTimeline.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.AutoSize = true;
            this.groupBox6.Controls.Add(this.tbTransformOffset2);
            this.groupBox6.Controls.Add(this.label12);
            this.groupBox6.Controls.Add(this.lbTwoPartTransforms2);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox6.Location = new System.Drawing.Point(448, 3);
            this.groupBox6.MinimumSize = new System.Drawing.Size(200, 0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(397, 348);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Animated transforms";
            // 
            // tbTransformOffset2
            // 
            this.tbTransformOffset2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbTransformOffset2.Location = new System.Drawing.Point(3, 325);
            this.tbTransformOffset2.Name = "tbTransformOffset2";
            this.tbTransformOffset2.Size = new System.Drawing.Size(391, 20);
            this.tbTransformOffset2.TabIndex = 8;
            this.tbTransformOffset2.TextChanged += new System.EventHandler(this.tbTransformOffset2_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Top;
            this.label12.Location = new System.Drawing.Point(3, 306);
            this.label12.Name = "label12";
            this.label12.Padding = new System.Windows.Forms.Padding(3);
            this.label12.Size = new System.Drawing.Size(72, 19);
            this.label12.TabIndex = 9;
            this.label12.Text = "Value (Float)";
            // 
            // lbTwoPartTransforms2
            // 
            this.lbTwoPartTransforms2.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbTwoPartTransforms2.FormattingEnabled = true;
            this.lbTwoPartTransforms2.Location = new System.Drawing.Point(3, 16);
            this.lbTwoPartTransforms2.MaximumSize = new System.Drawing.Size(4, 300);
            this.lbTwoPartTransforms2.MinimumSize = new System.Drawing.Size(194, 4);
            this.lbTwoPartTransforms2.Name = "lbTwoPartTransforms2";
            this.lbTwoPartTransforms2.Size = new System.Drawing.Size(194, 290);
            this.lbTwoPartTransforms2.TabIndex = 0;
            this.lbTwoPartTransforms2.SelectedIndexChanged += new System.EventHandler(this.lbRotations2_SelectedIndexChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.AutoSize = true;
            this.groupBox7.Controls.Add(this.tbTransformation2);
            this.groupBox7.Controls.Add(this.label14);
            this.groupBox7.Controls.Add(this.lbTransformations2);
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox7.Location = new System.Drawing.Point(298, 3);
            this.groupBox7.MinimumSize = new System.Drawing.Size(150, 0);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(150, 532);
            this.groupBox7.TabIndex = 4;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Static transforms";
            // 
            // tbTransformation2
            // 
            this.tbTransformation2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbTransformation2.Location = new System.Drawing.Point(3, 325);
            this.tbTransformation2.Name = "tbTransformation2";
            this.tbTransformation2.Size = new System.Drawing.Size(144, 20);
            this.tbTransformation2.TabIndex = 3;
            this.tbTransformation2.TextChanged += new System.EventHandler(this.tbTransformation2_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Dock = System.Windows.Forms.DockStyle.Top;
            this.label14.Location = new System.Drawing.Point(3, 306);
            this.label14.Margin = new System.Windows.Forms.Padding(0);
            this.label14.Name = "label14";
            this.label14.Padding = new System.Windows.Forms.Padding(3);
            this.label14.Size = new System.Drawing.Size(72, 19);
            this.label14.TabIndex = 19;
            this.label14.Text = "Value (Float)";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbTransformations2
            // 
            this.lbTransformations2.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbTransformations2.FormattingEnabled = true;
            this.lbTransformations2.Location = new System.Drawing.Point(3, 16);
            this.lbTransformations2.MaximumSize = new System.Drawing.Size(4, 300);
            this.lbTransformations2.MinimumSize = new System.Drawing.Size(144, 4);
            this.lbTransformations2.Name = "lbTransformations2";
            this.lbTransformations2.Size = new System.Drawing.Size(144, 290);
            this.lbTransformations2.TabIndex = 0;
            this.lbTransformations2.SelectedIndexChanged += new System.EventHandler(this.lbScales2_SelectedIndexChanged);
            // 
            // groupBox8
            // 
            this.groupBox8.AutoSize = true;
            this.groupBox8.Controls.Add(this.label16);
            this.groupBox8.Controls.Add(this.label18);
            this.groupBox8.Controls.Add(this.label20);
            this.groupBox8.Controls.Add(this.label22);
            this.groupBox8.Controls.Add(this.tbJointTwoPartTransformIndex2);
            this.groupBox8.Controls.Add(this.tbJointTransformIndex2);
            this.groupBox8.Controls.Add(this.tbJointTransformChoice2);
            this.groupBox8.Controls.Add(this.tbShapesAmount);
            this.groupBox8.Controls.Add(this.lbShapeSettings);
            this.groupBox8.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox8.Location = new System.Drawing.Point(3, 3);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(295, 532);
            this.groupBox8.TabIndex = 3;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Shape settings";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(4, 457);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(125, 13);
            this.label16.TabIndex = 17;
            this.label16.Text = "Animated transform index";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(4, 431);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(108, 13);
            this.label18.TabIndex = 15;
            this.label18.Text = "Static transform index";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(4, 405);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(89, 13);
            this.label20.TabIndex = 13;
            this.label20.Text = "Transform choice";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(4, 378);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(81, 13);
            this.label22.TabIndex = 11;
            this.label22.Text = "Shapes amount";
            // 
            // tbJointTwoPartTransformIndex2
            // 
            this.tbJointTwoPartTransformIndex2.Location = new System.Drawing.Point(149, 454);
            this.tbJointTwoPartTransformIndex2.Name = "tbJointTwoPartTransformIndex2";
            this.tbJointTwoPartTransformIndex2.Size = new System.Drawing.Size(140, 20);
            this.tbJointTwoPartTransformIndex2.TabIndex = 9;
            this.tbJointTwoPartTransformIndex2.TextChanged += new System.EventHandler(this.tbDis2B7_TextChanged);
            // 
            // tbJointTransformIndex2
            // 
            this.tbJointTransformIndex2.Location = new System.Drawing.Point(149, 428);
            this.tbJointTransformIndex2.Name = "tbJointTransformIndex2";
            this.tbJointTransformIndex2.Size = new System.Drawing.Size(140, 20);
            this.tbJointTransformIndex2.TabIndex = 7;
            this.tbJointTransformIndex2.TextChanged += new System.EventHandler(this.tbDis2B5_TextChanged);
            // 
            // tbJointTransformChoice2
            // 
            this.tbJointTransformChoice2.Location = new System.Drawing.Point(149, 402);
            this.tbJointTransformChoice2.Name = "tbJointTransformChoice2";
            this.tbJointTransformChoice2.Size = new System.Drawing.Size(140, 20);
            this.tbJointTransformChoice2.TabIndex = 5;
            this.tbJointTransformChoice2.TextChanged += new System.EventHandler(this.tbDis2B3_TextChanged);
            // 
            // tbShapesAmount
            // 
            this.tbShapesAmount.Location = new System.Drawing.Point(149, 376);
            this.tbShapesAmount.Name = "tbShapesAmount";
            this.tbShapesAmount.ReadOnly = true;
            this.tbShapesAmount.Size = new System.Drawing.Size(140, 20);
            this.tbShapesAmount.TabIndex = 3;
            // 
            // lbShapeSettings
            // 
            this.lbShapeSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbShapeSettings.FormattingEnabled = true;
            this.lbShapeSettings.Location = new System.Drawing.Point(3, 16);
            this.lbShapeSettings.Name = "lbShapeSettings";
            this.lbShapeSettings.Size = new System.Drawing.Size(289, 355);
            this.lbShapeSettings.TabIndex = 0;
            this.lbShapeSettings.SelectedIndexChanged += new System.EventHandler(this.lbDisplacements2_SelectedIndexChanged);
            // 
            // tpPreview
            // 
            this.tpPreview.Controls.Add(this.groupBox9);
            this.tpPreview.Location = new System.Drawing.Point(4, 22);
            this.tpPreview.Name = "tpPreview";
            this.tpPreview.Padding = new System.Windows.Forms.Padding(3);
            this.tpPreview.Size = new System.Drawing.Size(848, 538);
            this.tpPreview.TabIndex = 2;
            this.tpPreview.Text = "Preview";
            this.tpPreview.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.AutoSize = true;
            this.groupBox9.Controls.Add(this.panel1);
            this.groupBox9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox9.Location = new System.Drawing.Point(3, 446);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(842, 89);
            this.groupBox9.TabIndex = 1;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Playback controls";
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.cbShowSkeleton);
            this.panel1.Controls.Add(this.tbAnimationTimeline);
            this.panel1.Controls.Add(this.cbLoop);
            this.panel1.Controls.Add(this.btnPlayAnim);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.tbPlaybackFps);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbOGIList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.MinimumSize = new System.Drawing.Size(290, 70);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(836, 70);
            this.panel1.TabIndex = 6;
            // 
            // tbAnimationTimeline
            // 
            this.tbAnimationTimeline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbAnimationTimeline.Location = new System.Drawing.Point(5, 5);
            this.tbAnimationTimeline.Name = "tbAnimationTimeline";
            this.tbAnimationTimeline.Size = new System.Drawing.Size(336, 60);
            this.tbAnimationTimeline.TabIndex = 6;
            this.tbAnimationTimeline.Scroll += new System.EventHandler(this.tbAnimationTimeline_Scroll);
            // 
            // cbLoop
            // 
            this.cbLoop.AutoSize = true;
            this.cbLoop.Dock = System.Windows.Forms.DockStyle.Right;
            this.cbLoop.Location = new System.Drawing.Point(341, 5);
            this.cbLoop.Name = "cbLoop";
            this.cbLoop.Size = new System.Drawing.Size(50, 60);
            this.cbLoop.TabIndex = 1;
            this.cbLoop.Text = "Loop";
            this.cbLoop.UseVisualStyleBackColor = true;
            this.cbLoop.CheckedChanged += new System.EventHandler(this.cbLoop_CheckedChanged);
            // 
            // btnPlayAnim
            // 
            this.btnPlayAnim.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPlayAnim.Location = new System.Drawing.Point(391, 5);
            this.btnPlayAnim.Name = "btnPlayAnim";
            this.btnPlayAnim.Size = new System.Drawing.Size(75, 60);
            this.btnPlayAnim.TabIndex = 0;
            this.btnPlayAnim.Text = "Play";
            this.btnPlayAnim.UseVisualStyleBackColor = true;
            this.btnPlayAnim.Click += new System.EventHandler(this.btnPlayAnim_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Right;
            this.label4.Location = new System.Drawing.Point(466, 5);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(2);
            this.label4.Size = new System.Drawing.Size(78, 17);
            this.label4.TabIndex = 4;
            this.label4.Text = "Playback FPS";
            // 
            // tbPlaybackFps
            // 
            this.tbPlaybackFps.Dock = System.Windows.Forms.DockStyle.Right;
            this.tbPlaybackFps.Location = new System.Drawing.Point(544, 5);
            this.tbPlaybackFps.Name = "tbPlaybackFps";
            this.tbPlaybackFps.Size = new System.Drawing.Size(100, 20);
            this.tbPlaybackFps.TabIndex = 5;
            this.tbPlaybackFps.TextChanged += new System.EventHandler(this.tbPlaybackFps_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Location = new System.Drawing.Point(644, 5);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(2);
            this.label2.Size = new System.Drawing.Size(30, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "OGI";
            // 
            // cbOGIList
            // 
            this.cbOGIList.Dock = System.Windows.Forms.DockStyle.Right;
            this.cbOGIList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOGIList.FormattingEnabled = true;
            this.cbOGIList.Location = new System.Drawing.Point(674, 5);
            this.cbOGIList.Name = "cbOGIList";
            this.cbOGIList.Size = new System.Drawing.Size(157, 21);
            this.cbOGIList.TabIndex = 2;
            this.cbOGIList.SelectedIndexChanged += new System.EventHandler(this.cbOGIList_SelectedIndexChanged);
            // 
            // cbShowSkeleton
            // 
            this.cbShowSkeleton.AutoSize = true;
            this.cbShowSkeleton.Checked = true;
            this.cbShowSkeleton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowSkeleton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cbShowSkeleton.Location = new System.Drawing.Point(5, 48);
            this.cbShowSkeleton.Name = "cbShowSkeleton";
            this.cbShowSkeleton.Size = new System.Drawing.Size(336, 17);
            this.cbShowSkeleton.TabIndex = 7;
            this.cbShowSkeleton.Text = "Render skeleton outline";
            this.cbShowSkeleton.UseVisualStyleBackColor = true;
            this.cbShowSkeleton.CheckedChanged += new System.EventHandler(this.cbShowSkeleton_CheckedChanged);
            // 
            // AnimationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1118, 583);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AnimationEditor";
            this.Text = "AnimationEditor";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tcProperties.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.tpPreview.ResumeLayout(false);
            this.tpPreview.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbAnimationTimeline)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lbAnimations;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabControl tcProperties;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox lbJointSettings;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListBox lbTransformations;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ListBox lbAnimatedTransforms;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbJointAnimatedTransformIndex;
        private System.Windows.Forms.TextBox tbJointTransformIndex;
        private System.Windows.Forms.TextBox tbJointTransformChoice;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ListBox lbTwoPartTransforms2;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox tbTransformation2;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ListBox lbTransformations2;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox tbJointTwoPartTransformIndex2;
        private System.Windows.Forms.TextBox tbJointTransformIndex2;
        private System.Windows.Forms.TextBox tbJointTransformChoice2;
        private System.Windows.Forms.TextBox tbShapesAmount;
        private System.Windows.Forms.ListBox lbShapeSettings;
        private System.Windows.Forms.TextBox tbTransformation;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbTransformOffset;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbTransformOffset2;
        private System.Windows.Forms.TabPage tpPreview;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.CheckBox cbLoop;
        private System.Windows.Forms.Button btnPlayAnim;
        private System.Windows.Forms.ComboBox cbOGIList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.TextBox tbJointTimelineView;
        private System.Windows.Forms.TextBox tbPlaybackFps;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbAddRotation;
        private System.Windows.Forms.CheckBox cbParentScale;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.TextBox tbMorphTimeline;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TrackBar tbAnimationTimeline;
        private System.Windows.Forms.CheckBox cbShowSkeleton;
    }
}