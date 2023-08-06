﻿using Twinsanity;
using System.Collections.Generic;
using OpenTK;
using TwinsaityEditor.Animations;

namespace TwinsaityEditor
{
    public class GraphicsInfoController : ItemController
    {
        public new GraphicsInfo Data { get; set; }

        public Skeleton Skeleton { get; private set; }

        public GraphicsInfoController(MainForm topform, GraphicsInfo item) : base(topform, item)
        {
            Data = item;
            //if (MainFile.Data.Type == TwinsFile.FileType.RMX)
            {
                AddMenu("Open mesh viewer", Menu_OpenViewer);
            }

            Skeleton = new Skeleton(item.Joints.Length);
            for (int i = 0; i < item.Joints.Length; i++)
            {
                var joint = item.Joints[i];
                var animJoint = new SkeletalJoint() { Index = (int)joint.JointIndex };
                if (Skeleton.Joints.FindIndex(j => j.Index == (int)joint.ParentJointIndex) != -1)
                {
                    animJoint.Parent = Skeleton.Joints[(int)joint.ParentJointIndex];
                }
                Skeleton.Joints.Add(animJoint);
                var bindMat = GetJointToWorldTransform((int)joint.JointIndex);
                bindMat.Invert();
                Skeleton.InverseBindPose[(int)joint.JointIndex] = bindMat;
                Skeleton.BindPose[(int)joint.JointIndex] = GetJointToWorldTransform((int)joint.JointIndex);
            }
        }



        protected override string GetName()
        {
            return $"Graphics Info [ID {Data.ID}]";
        }

        protected override void GenText()
        {
            List<string> text = new List<string>
            {
                $"ID: {Data.ID}",
                $"Bounding Box Vector 1: {Data.Coord1.X}; {Data.Coord1.Y}; {Data.Coord1.Z}; {Data.Coord1.W}",
                $"Bounding Box Vector 2: {Data.Coord2.X}; {Data.Coord2.Y}; {Data.Coord2.Z}; {Data.Coord2.W}",
                $"Skin ID: {Data.SkinID:X8}",
                $"Blend Skin Model ID: {Data.BlendSkinID:X8}",
                $"Joint Count: {Data.Joints.Length}",
                $"Exit Point Count: {Data.ExitPoints.Length}",
                $"Parent Joint Count: {Data.HeaderVars[2]}",
                $"Collision Datas Size: {Data.CollisionData.Length}",
                $"Rigid Model Count: {Data.ModelIDs.Count}"
            };
            if (Data.ModelIDs.Count > 0)
            {
                foreach(var pair in Data.ModelIDs)
                {
                    text.Add(string.Format("Rigid Model ID: {1:X8} Attached to Joint #{0}", pair.Value.JointIndex, pair.Value.ModelID));
                }
            }

            if (Data.Joints.Length > 0)
            {
                text.Add($"Joints:");
                for (int i = 0; i < Data.Joints.Length; i++)
                {
                    text.Add($"#{ i } React Joint-ID: { Data.Joints[i].ReactJointID }; Joint index: { Data.Joints[i].JointIndex }; Parent Joint: { Data.Joints[i].ParentJointIndex }; Child joints: { Data.Joints[i].ChildJointAmount }; { Data.Joints[i].ChildJointAmount2 }");
                    //text.Add($"#{ i } Numbers: { Data.Type1[i].Numbers[0] }; { Data.Type1[i].Numbers[1] }; { Data.Type1[i].Numbers[2] }; { Data.Type1[i].Numbers[3] }; { Data.Type1[i].Numbers[4] }");
                    text.Add($"\t#{ i } Bind Position: { Data.Joints[i].Matrix[0].X }; { Data.Joints[i].Matrix[0].Y }; { Data.Joints[i].Matrix[0].Z }; { Data.Joints[i].Matrix[0].W }");
                    text.Add($"\t#{ i } World Space Position: { Data.Joints[i].Matrix[1].X }; { Data.Joints[i].Matrix[1].Y }; { Data.Joints[i].Matrix[1].Z }; { Data.Joints[i].Matrix[1].W }");
                    text.Add($"\t#{ i } Bind Rotation: { Data.Joints[i].Matrix[2].X }; { Data.Joints[i].Matrix[2].Y }; { Data.Joints[i].Matrix[2].Z }; { Data.Joints[i].Matrix[2].W }");
                    text.Add($"\t#{ i } Unused rotation: { Data.Joints[i].Matrix[3].X }; { Data.Joints[i].Matrix[3].Y }; { Data.Joints[i].Matrix[3].Z }; { Data.Joints[i].Matrix[3].W }");
                    text.Add($"\t#{ i } Additional rotation for animations: { Data.Joints[i].Matrix[4].X }; { Data.Joints[i].Matrix[4].Y }; { Data.Joints[i].Matrix[4].Z }; { Data.Joints[i].Matrix[4].W }");
                    text.Add($"\t#{ i } Skin Matrix Row 1: { Data.SkinTransforms[i].Matrix[0].X }; { Data.SkinTransforms[i].Matrix[1].X }; { Data.SkinTransforms[i].Matrix[2].X }; { Data.SkinTransforms[i].Matrix[3].X }");
                    text.Add($"\t#{ i } Skin Matrix Row 2: { Data.SkinTransforms[i].Matrix[0].Y }; { Data.SkinTransforms[i].Matrix[1].Y }; { Data.SkinTransforms[i].Matrix[2].Y }; { Data.SkinTransforms[i].Matrix[3].Y }");
                    text.Add($"\t#{ i } Skin Matrix Row 3: { Data.SkinTransforms[i].Matrix[0].Z }; { Data.SkinTransforms[i].Matrix[1].Z }; { Data.SkinTransforms[i].Matrix[2].Z }; { Data.SkinTransforms[i].Matrix[3].Z }");
                    text.Add($"\t#{ i } Skin Matrix Row 4: { Data.SkinTransforms[i].Matrix[0].W }; { Data.SkinTransforms[i].Matrix[1].W }; { Data.SkinTransforms[i].Matrix[2].W }; { Data.SkinTransforms[i].Matrix[3].W }");
                }
            }
            if (Data.ExitPoints.Length > 0)
            {
                text.Add($"Exit Points:");
                for (int i = 0; i < Data.ExitPoints.Length; i++)
                {
                    text.Add($"#{ i } Parent Joint: { Data.ExitPoints[i].ParentJointIndex }; ID: { Data.ExitPoints[i].ID }");
                    //text.Add($"#{ i } Numbers: { Data.Type2[i].Numbers[0] }; { Data.Type2[i].Numbers[1] }");
                    text.Add($"\t#{ i } Matrix 1: { Data.ExitPoints[i].Matrix[0].X }; { Data.ExitPoints[i].Matrix[0].Y }; { Data.ExitPoints[i].Matrix[0].Z }; { Data.ExitPoints[i].Matrix[0].W }");
                    text.Add($"\t#{ i } Matrix 2: { Data.ExitPoints[i].Matrix[1].X }; { Data.ExitPoints[i].Matrix[1].Y }; { Data.ExitPoints[i].Matrix[1].Z }; { Data.ExitPoints[i].Matrix[1].W }");
                    text.Add($"\t#{ i } Matrix 3: { Data.ExitPoints[i].Matrix[2].X }; { Data.ExitPoints[i].Matrix[2].Y }; { Data.ExitPoints[i].Matrix[2].Z }; { Data.ExitPoints[i].Matrix[2].W }");
                    text.Add($"\t#{ i } Matrix 4: { Data.ExitPoints[i].Matrix[3].X }; { Data.ExitPoints[i].Matrix[3].Y }; { Data.ExitPoints[i].Matrix[3].Z }; { Data.ExitPoints[i].Matrix[3].W }");
                }
            }
            if (Data.CollisionData.Length > 0)
            {
                text.Add($"Collision data information:");
                for (var i = 0; i < Data.CollisionData.Length; ++i)
                {
                    var type4 = Data.CollisionData[i];
                    text.Add($"#{ i } Header:");
                    for (var j = 0; j < 11; ++j)
                    {
                        text.Add($" {type4.Header[j]}");
                    }
                    for (var j = 0; j < 7; ++j)
                    {
                        text.Add($"Blob block {j + 1}");
                        switch (j)
                        {
                            case 0:
                                text.Add($"\tSize: {type4.Header[5]}");
                                for (var k = 0; k < type4.Header[j]; ++k)
                                {
                                    text.Add($"\t{type4.UnkVectors1[k]}");
                                }
                                break;
                            case 1:
                                text.Add($"\tSize: {type4.Header[6] - type4.Header[5]}");
                                break;
                            case 2:
                                text.Add($"\tSize: {type4.Header[7] - type4.Header[6]}");
                                break;
                            case 3:
                                text.Add($"\tSize: {type4.Header[8] - type4.Header[7]}");
                                break;
                            case 4:
                                text.Add($"\tSize: {type4.Header[9] - type4.Header[8]}");
                                break;
                            case 5:
                                text.Add($"\tSize: {type4.Header[10] - type4.Header[9]}");
                                break;
                            case 6:
                                text.Add($"\tSize: {type4.collisionDataBlob.Length - type4.Header[10]}");
                                break;
                        }
                    }
                }
                text.Add("Collision data indices:\n");
                foreach (var b in Data.CollisionDataRelated)
                {
                    text.Add($"\t{b}");
                }
            }

            text.Add("");
            TraverseSkeleton(text);

            TextPrev = text.ToArray();
        }

        private void TraverseSkeleton(List<string> text)
        {
            text.Add("Skeleton:");
            TraverseJoint(Data.Skeleton.Root, text, Matrix4.Identity, 0);
        }

        private void TraverseJoint(GraphicsInfo.JointNode joint, List<string> text, Matrix4 parentTransform, int traverseLevel)
        {
            var jointString = "";
            for (int i = 0; i < traverseLevel; i++)
            {
                jointString += "--";
            }
            var index = joint.Joint.JointIndex;
            text.Add($"{jointString}Joint {index}");
            var localRot = Matrix4.CreateFromQuaternion(new Quaternion(
                Data.Joints[index].Matrix[2].X,
                Data.Joints[index].Matrix[2].Y,
                Data.Joints[index].Matrix[2].Z,
                Data.Joints[index].Matrix[2].W));
            var localTranslate = Matrix4.CreateTranslation(
                    Data.Joints[index].Matrix[0].X,
                    Data.Joints[index].Matrix[0].Y,
                    Data.Joints[index].Matrix[0].Z);
            var jointTransform = localRot * localTranslate * parentTransform;

            var skinMat = jointTransform.Inverted();
            skinMat.Transpose();
            text.Add($"{jointString} Skin Matrix Row 1: {Data.SkinTransforms[index].Matrix[0].X}; {Data.SkinTransforms[index].Matrix[1].X}; {Data.SkinTransforms[index].Matrix[2].X}; {Data.SkinTransforms[index].Matrix[3].X}");
            text.Add($"{jointString} Skin Matrix Row 2: {Data.SkinTransforms[index].Matrix[0].Y}; {Data.SkinTransforms[index].Matrix[1].Y}; {Data.SkinTransforms[index].Matrix[2].Y}; {Data.SkinTransforms[index].Matrix[3].Y}");
            text.Add($"{jointString} Skin Matrix Row 3: {Data.SkinTransforms[index].Matrix[0].Z}; {Data.SkinTransforms[index].Matrix[1].Z}; {Data.SkinTransforms[index].Matrix[2].Z}; {Data.SkinTransforms[index].Matrix[3].Z}");
            text.Add($"{jointString} Skin Matrix Row 4: {Data.SkinTransforms[index].Matrix[0].W}; {Data.SkinTransforms[index].Matrix[1].W}; {Data.SkinTransforms[index].Matrix[2].W}; {Data.SkinTransforms[index].Matrix[3].W}");
            text.Add($"{jointString} Computed Skin Matrix Row 1: {skinMat.Row0}");
            text.Add($"{jointString} Computed Skin Matrix Row 2: {skinMat.Row1}");
            text.Add($"{jointString} Computed Skin Matrix Row 3: {skinMat.Row2}");
            text.Add($"{jointString} Computed Skin Matrix Row 4: {skinMat.Row3}");
            foreach (var c in joint.Children)
            {
                TraverseJoint(c, text, jointTransform, traverseLevel + 1);
            }
        }

        public Matrix4 GetJointToWorldTransform(int jointIndex)
        {
            Matrix4 tempRot = Matrix4.Identity;

            // Rotation
            tempRot.M11 = Data.SkinTransforms[jointIndex].Matrix[0].X;
            tempRot.M12 = Data.SkinTransforms[jointIndex].Matrix[1].X;
            tempRot.M13 = Data.SkinTransforms[jointIndex].Matrix[2].X;

            tempRot.M21 = Data.SkinTransforms[jointIndex].Matrix[0].Y;
            tempRot.M22 = Data.SkinTransforms[jointIndex].Matrix[1].Y;
            tempRot.M23 = Data.SkinTransforms[jointIndex].Matrix[2].Y;

            tempRot.M31 = Data.SkinTransforms[jointIndex].Matrix[0].Z;
            tempRot.M32 = Data.SkinTransforms[jointIndex].Matrix[1].Z;
            tempRot.M33 = Data.SkinTransforms[jointIndex].Matrix[2].Z;

            tempRot.M14 = Data.SkinTransforms[jointIndex].Matrix[0].W;
            tempRot.M24 = Data.SkinTransforms[jointIndex].Matrix[1].W;
            tempRot.M34 = Data.SkinTransforms[jointIndex].Matrix[2].W;

            // Position (Joint's world position)
            tempRot.M41 = Data.SkinTransforms[jointIndex].Matrix[3].X;//Data.Joints[jointIndex].Matrix[1].X;
            tempRot.M42 = Data.SkinTransforms[jointIndex].Matrix[3].Y;//Data.Joints[jointIndex].Matrix[1].Y;
            tempRot.M43 = Data.SkinTransforms[jointIndex].Matrix[3].Z;//Data.Joints[jointIndex].Matrix[1].Z;
            tempRot.M44 = Data.SkinTransforms[jointIndex].Matrix[3].W;//Data.Joints[jointIndex].Matrix[1].W;
            /*var addRot = Matrix4.CreateFromQuaternion(new Quaternion(
                Data.Joints[jointIndex].Matrix[3].X,
                Data.Joints[jointIndex].Matrix[3].Y,
                Data.Joints[jointIndex].Matrix[3].Z,
                Data.Joints[jointIndex].Matrix[3].W));
            tempRot *= addRot;*/
            // Adjusted for OpenTK
            //tempRot *= Matrix4.CreateScale(-1, 1, 1);

            return tempRot;
        }

        private void Menu_OpenViewer()
        {
            MainFile.OpenMeshViewer(this, MainFile);
        }
    }
}