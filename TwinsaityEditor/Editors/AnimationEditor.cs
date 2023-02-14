using OpenTK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TwinsaityEditor.Controllers;
using TwinsaityEditor.Viewers;
using Twinsanity;
using static OpenTK.Graphics.OpenGL.GL;

namespace TwinsaityEditor
{
    public partial class AnimationEditor : Form
    {
        private SectionController controller;
        private Animation animation;
        private Animation.JointSettings JointSettings;
        private Animation.JointSettings JointSettings2;
        private Animation.AnimatedTransform AnimatedTransform;
        private Animation.AnimatedTransform AnimatedTransform2;
        private Animation.Transformation StaticTransform;
        private Animation.Transformation StaticTransform2;
        private AnimationViewer viewer;

        private bool playing, loop;
        private int fps = 50;

        public AnimationEditor(SectionController c)
        {
            controller = c;
            InitializeComponent();
            Text = $"Animation editor";
            PopulateList();
        }

        private void PopulateList()
        {
            lbAnimations.SelectedIndex = -1;
            lbAnimations.Items.Clear();
            foreach (Animation anim in controller.Data.Records)
            {
                lbAnimations.Items.Add($"Animation ID {anim.ID}");
            }
        }

        private void PopulateWithAnimData(IList list, IList data, Action<IList, string[], int> adder, params string[] namePattern)
        {
            list.Clear();
            var index = 1;
            foreach (var d in data)
            {
                adder.Invoke(list, namePattern, index++);
            }
        }

        private void lbAnimations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbAnimations.SelectedIndex == -1) return;
            JointSettings = null;
            JointSettings2 = null;
            AnimatedTransform = null;
            AnimatedTransform2 = null;
            StaticTransform = null;
            StaticTransform2 = null;
            tbJointUnknown.Text = "";
            tbJointTransformChoice.Text = "";
            tbJointTransformIndex.Text = "";
            tbJointAnimatedTransformIndex.Text = "";
            tbJointUnused2.Text = "";
            tbJointTransformChoice2.Text = "";
            tbJointTransformIndex2.Text = "";
            tbJointTwoPartTransformIndex2.Text = "";
            tbTransformation.Text = "";
            tbTransformation2.Text = "";
            cbOGIList.Items.Clear();
            playing = false;
            animation = (Animation)controller.Data.Records[lbAnimations.SelectedIndex];
            viewer?.Dispose();
            viewer = null;
            fps = 50;
            tbPlaybackFps.Text = fps.ToString();
            cbLoop.Checked = false;
            playing = false;
            UpdateLists();
        }

        private void UpdateLists()
        {
            void listAdder(IList list, string[] name, int index)
            {
                list.Add($"{name[0]} {index}");
            }

            PopulateWithAnimData(lbJointSettings.Items, animation.JointsSettings, listAdder, "Joint setting");
            PopulateWithAnimData(lbTransformations.Items, animation.StaticTransforms, listAdder, "Transform");
            PopulateWithAnimData(lbAnimatedTransforms.Items, animation.AnimatedTransforms, listAdder, "Animated transform");
            PopulateWithAnimData(lbJointSettings2.Items, animation.JointsSettings2, listAdder, "Joint setting");
            PopulateWithAnimData(lbTransformations2.Items, animation.StaticTransforms2, listAdder, "Transform");
            PopulateWithAnimData(lbTwoPartTransforms2.Items, animation.AnimatedTransforms2, listAdder, "Animated transform");

            var ogis = controller.MainFile.GetItem<SectionController>(10).GetItem<SectionController>(3);
            foreach (GraphicsInfo ogi in ogis.Data.Records.Cast<GraphicsInfo>())
            {
                if (ogi.Joints.Length == lbJointSettings.Items.Count)
                {
                    cbOGIList.Items.Add(ogi);
                }
            }
            if (cbOGIList.Items.Count > 0)
            {
                cbOGIList.SelectedIndex = 0;
                var ogi = cbOGIList.SelectedItem as GraphicsInfo;
                var ogic = ogis.GetItem<GraphicsInfoController>(ogi.ID);
                var animViewer = new AnimationViewer(ogic, controller.GetItem<AnimationController>(animation.ID), controller.MainFile)
                {
                    Parent = tpPreview,
                    AutoSize = true,
                    Location = new System.Drawing.Point(3, 3),
                    Dock = DockStyle.Fill,
                    MinimumSize = new System.Drawing.Size(0, 450)
                };
                viewer = animViewer;
            }
            else
            {
                var animViewer = new AnimationViewer()
                {
                    Parent = tpPreview,
                    AutoSize = true,
                    Location = new System.Drawing.Point(3, 3),
                    Dock = DockStyle.Fill,
                    MinimumSize = new System.Drawing.Size(0, 450)
                };
                viewer = animViewer;
            }
            viewer.FPS = fps;
        }

        private void lbDisplacements_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list = (ListBox)sender;
            if (list.SelectedIndex == -1) return;

            Animation.JointSettings displacement = animation.JointsSettings[list.SelectedIndex];
            JointSettings = displacement;
            tbJointUnknown.Text = displacement.Unused.ToString();
            tbJointTransformChoice.Text = displacement.TransformationChoice.ToString();
            tbJointTransformIndex.Text = displacement.TransformationIndex.ToString();
            tbJointAnimatedTransformIndex.Text = displacement.AnimatedTransformIndex.ToString();

            var transformChoice = JointSettings.TransformationChoice;
            var timelineText = new List<string>();
            for (int i = 0; i < animation.TotalFrames - 1; i++)
            {
                var transformIndex = JointSettings.TransformationIndex;
                int currentFrameTransformIndex = JointSettings.AnimatedTransformIndex;
                var nextFrameTransformIndex = JointSettings.AnimatedTransformIndex;
                timelineText.Add($"Frame {i + 1}:\n");
                var translateXChoice = (transformChoice & 0x1) == 0;
                var translateYChoice = (transformChoice & 0x2) == 0;
                var translateZChoice = (transformChoice & 0x4) == 0;
                var rotXChoice = (transformChoice & 0x8) == 0;
                var rotYChoice = (transformChoice & 0x10) == 0;
                var rotZChoice = (transformChoice & 0x20) == 0;
                var scaleXChoice = (transformChoice & 0x40) == 0;
                var scaleYChoice = (transformChoice & 0x80) == 0;
                var scaleZChoice = (transformChoice & 0x100) == 0;
                

                if (translateXChoice)
                {
                    timelineText.Add($"Animate Translate from X {animation.AnimatedTransforms[i].GetOffset(currentFrameTransformIndex++)}\n");
                    timelineText.Add($"Animate Translate to X {animation.AnimatedTransforms[i+1].GetOffset(nextFrameTransformIndex++)}\n");
                }
                else
                {
                    timelineText.Add($"Static Translate X to {animation.StaticTransforms[transformIndex++].Value}\n");
                }

                if (translateYChoice)
                {
                    timelineText.Add($"Animate Translate from Y {animation.AnimatedTransforms[i].GetOffset(currentFrameTransformIndex++)}\n");
                    timelineText.Add($"Animate Translate to Y {animation.AnimatedTransforms[i+1].GetOffset(nextFrameTransformIndex++)}\n");
                }
                else
                {
                    timelineText.Add($"Static Translate Y to {animation.StaticTransforms[transformIndex++].Value}\n");
                }

                if (translateZChoice)
                {
                    timelineText.Add($"Animate Translate from Z {animation.AnimatedTransforms[i].GetOffset(currentFrameTransformIndex++)}\n");
                    timelineText.Add($"Animate Translate to Z {animation.AnimatedTransforms[i+1].GetOffset(nextFrameTransformIndex++)}\n");
                }
                else
                {
                    timelineText.Add($"Static Translate Z to {animation.StaticTransforms[transformIndex++].Value}\n");
                }

                if (rotXChoice)
                {
                    var rot1 = animation.AnimatedTransforms[i].GetPureOffset(currentFrameTransformIndex++) * 16;
                    var rot2 = animation.AnimatedTransforms[i+1].GetPureOffset(nextFrameTransformIndex++) * 16;
                    var diff = rot1 - rot2;
                    if (diff < -0x8000)
                    {
                        rot1 += 0x10000;
                    }
                    if (diff > 0x8000)
                    {
                        rot1 -= 0x10000;
                    }
                    var rot1Rad = rot1 / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi;
                    var rot2Rad = rot2 / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi;
                    timelineText.Add($"Animate Rotation from X {(rot1Rad)}");
                    timelineText.Add($"Animate Rotation to X {(rot2Rad)}");
                }
                else
                {
                    var rot = animation.StaticTransforms[transformIndex++].RotValue;
                    timelineText.Add($"Static Rotation X {(rot)}");
                }

                if (rotYChoice)
                {
                    var rot1 = animation.AnimatedTransforms[i].GetPureOffset(currentFrameTransformIndex++) * 16;
                    var rot2 = animation.AnimatedTransforms[i + 1].GetPureOffset(nextFrameTransformIndex++) * 16;
                    var diff = rot1 - rot2;
                    if (diff < -0x8000)
                    {
                        rot1 += 0x10000;
                    }
                    if (diff > 0x8000)
                    {
                        rot1 -= 0x10000;
                    }
                    var rot1Rad = rot1 / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi;
                    var rot2Rad = rot2 / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi;
                    timelineText.Add($"Animate Rotation from Y {(-rot1Rad)}");
                    timelineText.Add($"Animate Rotation to Y {(-rot2Rad)}");
                }
                else
                {
                    var rot = animation.StaticTransforms[transformIndex++].RotValue;
                    timelineText.Add($"Static Rotation Y {(-rot)}");
                }

                if (rotZChoice)
                {
                    var rot1 = animation.AnimatedTransforms[i].GetPureOffset(currentFrameTransformIndex++) * 16;
                    var rot2 = animation.AnimatedTransforms[i + 1].GetPureOffset(nextFrameTransformIndex++) * 16;
                    var diff = rot1 - rot2;
                    if (diff < -0x8000)
                    {
                        rot1 += 0x10000;
                    }
                    if (diff > 0x8000)
                    {
                        rot1 -= 0x10000;
                    }
                    var rot1Rad = rot1 / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi;
                    var rot2Rad = rot2 / (float)(ushort.MaxValue + 1) * MathHelper.TwoPi;
                    timelineText.Add($"Animate Rotation from Z {(-rot1Rad)}");
                    timelineText.Add($"Animate Rotation to Z {(-rot2Rad)}");
                }
                else
                {
                    var rot = animation.StaticTransforms[transformIndex++].RotValue;
                    timelineText.Add($"Static Rotation Z {(-rot)}");
                }

                if (scaleXChoice)
                {
                    timelineText.Add($"Animate Scale from X {animation.AnimatedTransforms[i].GetOffset(currentFrameTransformIndex++)}\n");
                    timelineText.Add($"Animate Scale to X {animation.AnimatedTransforms[i + 1].GetOffset(nextFrameTransformIndex++)}\n");
                }
                else
                {
                    timelineText.Add($"Static Scale X to {animation.StaticTransforms[transformIndex++].Value}\n");
                }

                if (scaleYChoice)
                {
                    timelineText.Add($"Animate Scale from Y {animation.AnimatedTransforms[i].GetOffset(currentFrameTransformIndex++)}\n");
                    timelineText.Add($"Animate Scale to Y {animation.AnimatedTransforms[i + 1].GetOffset(nextFrameTransformIndex++)}\n");
                }
                else
                {
                    timelineText.Add($"Static Scale Y to {animation.StaticTransforms[transformIndex++].Value}\n");
                }

                if (scaleZChoice)
                {
                    timelineText.Add($"Animate Scale from Z {animation.AnimatedTransforms[i].GetOffset(currentFrameTransformIndex++)}\n");
                    timelineText.Add($"Animate Scale to Z {animation.AnimatedTransforms[i + 1].GetOffset(nextFrameTransformIndex++)}\n");
                }
                else
                {
                    timelineText.Add($"Static Scale Z to {animation.StaticTransforms[transformIndex++].Value}\n");
                }
            }
            tbJointTimelineView.Lines = timelineText.ToArray();
        }

        private void lbScales_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list = (ListBox)sender;
            if (list.SelectedIndex == -1) return;

            Animation.Transformation scale = animation.StaticTransforms[list.SelectedIndex];
            StaticTransform = scale;
            tbTransformation.Text = scale.Value.ToString();
        }

        private void lbRotations_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list = (ListBox)sender;
            if (list.SelectedIndex == -1) return;

            Animation.AnimatedTransform timeline = animation.AnimatedTransforms[list.SelectedIndex];
            AnimatedTransform = timeline;
        }

        private void lbDisplacements2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list = (ListBox)sender;
            if (list.SelectedIndex == -1) return;

            Animation.JointSettings displacement = animation.JointsSettings2[list.SelectedIndex];
            JointSettings2 = displacement;
            tbJointUnused2.Text = displacement.Unused.ToString();
            tbJointTransformChoice2.Text = displacement.TransformationChoice.ToString();
            tbJointTransformIndex2.Text = displacement.TransformationIndex.ToString();
            tbJointTwoPartTransformIndex2.Text = displacement.AnimatedTransformIndex.ToString();
        }

        private void lbScales2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list = (ListBox)sender;
            if (list.SelectedIndex == -1) return;

            Animation.Transformation scale = animation.StaticTransforms2[list.SelectedIndex];
            StaticTransform2 = scale;
            tbTransformation2.Text = scale.Value.ToString();
        }

        private void lbRotations2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list = (ListBox)sender;
            if (list.SelectedIndex == -1) return;

            Animation.AnimatedTransform timeline = animation.AnimatedTransforms2[list.SelectedIndex];
            AnimatedTransform2 = timeline;
        }

        private void tbDisB1_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!ushort.TryParse(tb.Text, out ushort result) || JointSettings == null) return;
            JointSettings.Unused = result;
        }

        private void tbDisB3_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!ushort.TryParse(tb.Text, out ushort result) || JointSettings == null) return;
            JointSettings.TransformationChoice = result;
        }

        private void tbDisB5_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!ushort.TryParse(tb.Text, out ushort result) || JointSettings == null) return;
            JointSettings.TransformationIndex = result;
        }

        private void tbDisB7_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!ushort.TryParse(tb.Text, out ushort result) || JointSettings == null) return;
            JointSettings.AnimatedTransformIndex = result;
        }

        private void tbTransformation_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Single.TryParse(tb.Text, out Single result) || StaticTransform == null) return;
            StaticTransform.Value = result;
        }

        private void tbDis2B1_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!ushort.TryParse(tb.Text, out ushort result) || JointSettings2 == null) return;
            JointSettings2.Unused = result;
        }

        private void tbDis2B3_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!ushort.TryParse(tb.Text, out ushort result) || JointSettings2 == null) return;
            JointSettings2.TransformationChoice = result;
        }

        private void tbDis2B5_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!ushort.TryParse(tb.Text, out ushort result) || JointSettings2 == null) return;
            JointSettings2.TransformationIndex = result;
        }

        private void tbDis2B7_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!ushort.TryParse(tb.Text, out ushort result) || JointSettings2 == null) return;
            JointSettings2.AnimatedTransformIndex = result;
        }

        private void tbTransformation2_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Single.TryParse(tb.Text, out Single result) || StaticTransform2 == null) return;
            StaticTransform2.Value = result;
        }

        private void tbTransformOffset_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Int16.TryParse(tb.Text, out Int16 result) || AnimatedTransform == null) return;
            //TwoPartTransform.SetOffset(tbTimeline.Value, result);
        }

        private void tbTransformOffset2_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Int16.TryParse(tb.Text, out Int16 result) || AnimatedTransform2 == null) return;
            //TwoPartTransform2.SetOffset(tbTimeline2.Value, result);
        }

        private void btnAddTransformation_Click(object sender, EventArgs e)
        {
            if (animation == null) return;
            animation.StaticTransforms.Add(new Animation.Transformation());
            UpdateLists();
        }

        private void btnDeleteTransformation_Click(object sender, EventArgs e)
        {
            if (animation == null) return;
            animation.StaticTransforms.RemoveAt(animation.StaticTransforms.Count - 1);
            UpdateLists();
        }

        private void btnAddTimeline_Click(object sender, EventArgs e)
        {
            if (animation == null) return;
            animation.AnimatedTransforms.Add(new Animation.AnimatedTransform(animation.TotalFrames));
            for (var i = 0; i < animation.AnimatedTransforms[animation.AnimatedTransforms.Count - 1].Values.Capacity; ++i)
            {
                animation.AnimatedTransforms[animation.AnimatedTransforms.Count - 1].Values.Add(0);
            }
            UpdateLists();
        }

        private void btnDeleteTimeline_Click(object sender, EventArgs e)
        {
            if (animation == null) return;
            animation.AnimatedTransforms.RemoveAt(animation.AnimatedTransforms.Count - 1);
            UpdateLists();
        }

        private void btnAddTransformation2_Click(object sender, EventArgs e)
        {
            if (animation == null) return;
            animation.StaticTransforms2.Add(new Animation.Transformation());
            UpdateLists();
        }

        private void btnDeleteTransformation2_Click(object sender, EventArgs e)
        {
            if (animation == null) return;
            animation.StaticTransforms2.RemoveAt(animation.StaticTransforms2.Count - 1);
            UpdateLists();
        }

        private void btnAddTimeline2_Click(object sender, EventArgs e)
        {
            if (animation == null) return;
            if (animation.TotalFrames2 == 0)
            {
                animation.TotalFrames2 = 1;
            }
            animation.AnimatedTransforms2.Add(new Animation.AnimatedTransform(animation.TotalFrames2));
            for (var i = 0; i < animation.AnimatedTransforms2[animation.AnimatedTransforms2.Count - 1].Values.Capacity; ++i)
            {
                animation.AnimatedTransforms2[animation.AnimatedTransforms2.Count - 1].Values.Add(0);
            }
            UpdateLists();
        }

        private void cbOGIList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbOGIList.SelectedIndex == -1) return;

            var ogis = controller.MainFile.GetItem<SectionController>(10).GetItem<SectionController>(3);
            var ogi = cbOGIList.SelectedItem as GraphicsInfo;
            var ogic = ogis.GetItem<GraphicsInfoController>(ogi.ID);
            viewer?.ChangeGraphicsInfo(ogic);
        }

        private void btnPlayAnim_Click(object sender, EventArgs e)
        {
            if (viewer != null)
            {
                playing = viewer.Finished || !playing;
                viewer.Playing = playing;
            }
        }

        private void tbPlaybackFps_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!UInt16.TryParse(tb.Text, out UInt16 result) || viewer == null || result > 120 || result == 0) return;
            fps = result;
            viewer.FPS = fps;
        }

        private void cbLoop_CheckedChanged(object sender, EventArgs e)
        {
            if (viewer == null) return;
            loop = cbLoop.Checked;
            viewer.Loop = loop;
            
        }

        private void btnDeleteTimeline2_Click(object sender, EventArgs e)
        {
            if (animation == null) return;
            animation.AnimatedTransforms2.RemoveAt(animation.AnimatedTransforms2.Count - 1);
            UpdateLists();
        }

    }
}
