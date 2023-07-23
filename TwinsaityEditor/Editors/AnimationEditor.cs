using OpenTK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TwinsaityEditor.Controllers;
using TwinsaityEditor.Viewers;
using Twinsanity;

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
            cbParentScale.Checked = false;
            cbAddRotation.Checked = false;
            tbJointTransformChoice.Text = "";
            tbJointTransformIndex.Text = "";
            tbJointAnimatedTransformIndex.Text = "";
            tbShapesAmount.Text = "";
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
            PopulateWithAnimData(lbShapeSettings.Items, animation.FacialJointsSettings, listAdder, "Joint setting");
            PopulateWithAnimData(lbTransformations2.Items, animation.FacialStaticTransforms, listAdder, "Transform");
            PopulateWithAnimData(lbTwoPartTransforms2.Items, animation.FacialAnimatedTransforms, listAdder, "Animated transform");

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

            Animation.JointSettings jointSettings = animation.JointsSettings[list.SelectedIndex];
            JointSettings = jointSettings;
            cbAddRotation.Checked = (jointSettings.Flags >> 0xC & 0x1) != 0;
            cbParentScale.Checked = (jointSettings.Flags >> 0xD & 0x1) != 0;
            tbJointTransformChoice.Text = jointSettings.TransformationChoice.ToString();
            tbJointTransformIndex.Text = jointSettings.TransformationIndex.ToString();
            tbJointAnimatedTransformIndex.Text = jointSettings.AnimatedTransformIndex.ToString();

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

            Animation.JointSettings jointSettings = animation.FacialJointsSettings[list.SelectedIndex];
            JointSettings2 = jointSettings;
            var flags = jointSettings.Flags;
            tbShapesAmount.Text = (flags >> 0x8 & 0xf).ToString();
            tbJointTransformChoice2.Text = jointSettings.TransformationChoice.ToString();
            tbJointTransformIndex2.Text = jointSettings.TransformationIndex.ToString();
            tbJointTwoPartTransformIndex2.Text = jointSettings.AnimatedTransformIndex.ToString();
            var timelineText = new List<string>();
            var joints = (flags >> 0x8 & 0xf);
            var floats = new float[joints];
            

            for (int j = 0; j < animation.FacialAnimationTotalFrames - 1; j++)
            {
                var transformIndex = jointSettings.TransformationIndex;
                int currentFrameTransformIndex = jointSettings.AnimatedTransformIndex;
                var nextFrameTransformIndex = jointSettings.AnimatedTransformIndex;
                var transformChoice = jointSettings.TransformationChoice;

                timelineText.Add($"Frame {j + 1}:\n");

                for (int i = 0; i < floats.Length; i++)
                {
                    if ((transformChoice & 0x1) == 0)
                    {
                        var f1 = animation.FacialAnimatedTransforms[j].GetOffset(currentFrameTransformIndex++);
                        var f2 = animation.FacialAnimatedTransforms[j + 1].GetOffset(nextFrameTransformIndex++);
                        timelineText.Add($"\tAnimation value {i} from {f1} to {f2}");
                    }
                    else
                    {
                        floats[i] = animation.FacialStaticTransforms[transformIndex++].Value;
                        timelineText.Add($"\tSet value {i} to {floats[i]}");
                    }
                    transformChoice >>= 1;
                }
            }
            
            tbMorphTimeline.Lines = timelineText.ToArray();
        }

        private void lbScales2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list = (ListBox)sender;
            if (list.SelectedIndex == -1) return;

            Animation.Transformation scale = animation.FacialStaticTransforms[list.SelectedIndex];
            StaticTransform2 = scale;
            tbTransformation2.Text = scale.Value.ToString();
        }

        private void lbRotations2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list = (ListBox)sender;
            if (list.SelectedIndex == -1) return;

            Animation.AnimatedTransform timeline = animation.FacialAnimatedTransforms[list.SelectedIndex];
            AnimatedTransform2 = timeline;
        }

        private void tbDisB1_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!ushort.TryParse(tb.Text, out ushort result) || JointSettings == null) return;
            JointSettings.Flags = result;
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
            animation.FacialStaticTransforms.Add(new Animation.Transformation());
            UpdateLists();
        }

        private void btnDeleteTransformation2_Click(object sender, EventArgs e)
        {
            if (animation == null) return;
            animation.FacialStaticTransforms.RemoveAt(animation.FacialStaticTransforms.Count - 1);
            UpdateLists();
        }

        private void btnAddTimeline2_Click(object sender, EventArgs e)
        {
            if (animation == null) return;
            if (animation.FacialAnimationTotalFrames == 0)
            {
                animation.FacialAnimationTotalFrames = 1;
            }
            animation.FacialAnimatedTransforms.Add(new Animation.AnimatedTransform(animation.FacialAnimationTotalFrames));
            for (var i = 0; i < animation.FacialAnimatedTransforms[animation.FacialAnimatedTransforms.Count - 1].Values.Capacity; ++i)
            {
                animation.FacialAnimatedTransforms[animation.FacialAnimatedTransforms.Count - 1].Values.Add(0);
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
            animation.FacialAnimatedTransforms.RemoveAt(animation.FacialAnimatedTransforms.Count - 1);
            UpdateLists();
        }

    }
}
