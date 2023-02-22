using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twinsanity;

namespace TwinsaityEditor.Controllers
{
    public class AnimationController : ItemController
    {
        public new Animation Data { get; set; }
        
        public AnimationController(MainForm topform, Animation item) : base(topform, item)
        {
            Data = item;
            AddMenu("Open editor", Menu_OpenEditor);
        }

        public Tuple<Matrix4, bool> GetMainAnimationTransform(int jointIndex, int curFrame, int nextFrame, float frameDisplacement)
        {
            Vector4 rotation = new Vector4();
            Vector4 translation = new Vector4();
            translation.W = 1.0f;
            Vector4 scale = new Vector4();
            scale.W = 1.0f;
            var jointSetting = Data.JointsSettings[jointIndex];
            var useAddRot = (jointSetting.Flags >> 0xC & 0x1) != 0;
            var transformIndex = jointSetting.TransformationIndex;
            int currentFrameTransformIndex = jointSetting.AnimatedTransformIndex;
            var nextFrameTransformIndex = jointSetting.AnimatedTransformIndex;
            var transformChoice = jointSetting.TransformationChoice;
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
                var x1 = Data.AnimatedTransforms[curFrame].GetOffset(currentFrameTransformIndex++);
                var x2 = Data.AnimatedTransforms[nextFrame].GetOffset(nextFrameTransformIndex++);
                translation.X = VectorFuncs.Lerp(x1, x2, frameDisplacement);
            }
            else
            {
                translation.X = Data.StaticTransforms[transformIndex++].Value;
            }

            if (translateYChoice)
            {
                var y1 = Data.AnimatedTransforms[curFrame].GetOffset(currentFrameTransformIndex++);
                var y2 = Data.AnimatedTransforms[nextFrame].GetOffset(nextFrameTransformIndex++);
                translation.Y = VectorFuncs.Lerp(y1, y2, frameDisplacement);
            }
            else
            {
                translation.Y = Data.StaticTransforms[transformIndex++].Value;
            }

            if (translateZChoice)
            {
                var z1 = Data.AnimatedTransforms[curFrame].GetOffset(currentFrameTransformIndex++);
                var z2 = Data.AnimatedTransforms[nextFrame].GetOffset(nextFrameTransformIndex++);
                translation.Z = VectorFuncs.Lerp(z1, z2, frameDisplacement);
            }
            else
            {
                translation.Z = Data.StaticTransforms[transformIndex++].Value;
            }

            if (rotXChoice)
            {
                var rot1 = Data.AnimatedTransforms[curFrame].GetPureOffset(currentFrameTransformIndex++) * 16;
                var rot2 = Data.AnimatedTransforms[nextFrame].GetPureOffset(nextFrameTransformIndex++) * 16;
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
                rotation.X = VectorFuncs.Lerp(rot1Rad, rot2Rad, frameDisplacement);
            }
            else
            {
                var rot = Data.StaticTransforms[transformIndex++].GetRot(false);
                rotation.X = rot;
            }

            if (rotYChoice)
            {
                var rot1 = Data.AnimatedTransforms[curFrame].GetPureOffset(currentFrameTransformIndex++) * 16;
                var rot2 = Data.AnimatedTransforms[nextFrame].GetPureOffset(nextFrameTransformIndex++) * 16;
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
                rotation.Y = VectorFuncs.Lerp(rot1Rad, rot2Rad, frameDisplacement);
            }
            else
            {
                var rot = Data.StaticTransforms[transformIndex++].GetRot(false);
                rotation.Y = rot;
            }

            if (rotZChoice)
            {
                var rot1 = Data.AnimatedTransforms[curFrame].GetPureOffset(currentFrameTransformIndex++) * 16;
                var rot2 = Data.AnimatedTransforms[nextFrame].GetPureOffset(nextFrameTransformIndex++) * 16;
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
                rotation.Z = VectorFuncs.Lerp(rot1Rad, rot2Rad, frameDisplacement);
            }
            else
            {
                var rot = Data.StaticTransforms[transformIndex++].GetRot(false);
                rotation.Z = rot;
            }

            if (scaleXChoice)
            {
                var x1 = Data.AnimatedTransforms[curFrame].GetOffset(currentFrameTransformIndex++);
                var x2 = Data.AnimatedTransforms[nextFrame].GetOffset(nextFrameTransformIndex++);
                scale.X = VectorFuncs.Lerp(x1, x2, frameDisplacement);
            }
            else
            {
                scale.X = Data.StaticTransforms[transformIndex++].Value;
            }

            if (scaleYChoice)
            {
                var y1 = Data.AnimatedTransforms[curFrame].GetOffset(currentFrameTransformIndex++);
                var y2 = Data.AnimatedTransforms[nextFrame].GetOffset(nextFrameTransformIndex++);
                scale.Y = VectorFuncs.Lerp(y1, y2, frameDisplacement);
            }
            else
            {
                scale.Y = Data.StaticTransforms[transformIndex++].Value;
            }

            if (scaleZChoice)
            {
                var z1 = Data.AnimatedTransforms[curFrame].GetOffset(currentFrameTransformIndex++);
                var z2 = Data.AnimatedTransforms[nextFrame].GetOffset(nextFrameTransformIndex++);
                scale.Z = VectorFuncs.Lerp(z1, z2, frameDisplacement);
            }
            else
            {
                scale.Z = Data.StaticTransforms[transformIndex++].Value;
            }

            /*var transMat = Matrix4.CreateTranslation(translation.Xyz);
            var scaleMat = Matrix4.CreateScale(scale.Xyz);
            var rotMat = Matrix4.CreateFromQuaternion(new Quaternion(rotation.Xyz));
            var transformMatrix = transMat * rotMat * scaleMat;*/
            var transformMatrix = Matrix4.Zero;
            transformMatrix.Row0 = translation;
            transformMatrix.Row1 = scale;
            transformMatrix.Row2 = rotation;
            return new Tuple<Matrix4, bool>(transformMatrix, useAddRot);
        }

        protected override string GetName()
        {
            return $"Animation [ID {Data.ID}]";
        }

        protected override void GenText()
        {
            List<string> text = new List<string>
            {
                $"ID: {Data.ID}",
                $"Size: {Data.Size}",
                $"Unknown bitfield: 0x{Data.Bitfield:X}",
                $"Total frames: {Data.TotalFrames}",
                $"Blob packed 1: 0x{Data.UnkBlobSizePacked1:X}",
                $"Joint settings 1: {Data.JointsSettings.Count}",
                $"Transformations 1: {Data.StaticTransforms.Count}",
                $"Interpolate transformations 1: {Data.AnimatedTransforms.Count}",
                $"Total frames 2: {Data.TotalFrames2}",
                $"Blob packed 2: 0x{Data.UnkBlobSizePacked2:X}",
                $"Joint settings 2: {Data.JointsSettings2.Count}",
                $"Transformations 2: {Data.StaticTransforms2.Count}",
                $"Interpolate transformations 2: {Data.AnimatedTransforms2.Count}",
            };
            TextPrev = text.ToArray();
        }

        private void Menu_OpenEditor()
        {
            MainFile.OpenEditor(this);
        }
    }
}
