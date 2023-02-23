using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using TwinsaityEditor.Animations;
using TwinsaityEditor.Controllers;
using Twinsanity;
using static System.Net.Mime.MediaTypeNames;

namespace TwinsaityEditor.Viewers
{
    public class AnimationViewer : ThreeDViewer
    {
        private GraphicsInfoController graphicsInfo;
        private SkinController skin;
        private BlendSkinController bskin;
        private ModelController mesh;
        private FileController targetFile;
        private FileController file;
        private AnimationController animation;
        private AnimationPlayer player;
        private readonly Dictionary<int, int> VbufferMap = new Dictionary<int, int>();

        public int FPS { get => player.FPS; set { player.FPS = value; if (animUpdateTimer != null) animUpdateTimer.Interval = (int)Math.Floor(1.0 / FPS * 1000); } }
        public bool Loop { get => player.Loop; set => player.Loop = value; }
        public bool Playing { get => player.Playing; set => player.Playing = value; }
        public bool Finished { get => player.Finished; }

        private Timer animUpdateTimer;
        private VertexBufferData skeletonBuffer;
        private Vector3[] tposeBuffer;

        private int bskinEndIndex = 0;
        private int skinEndIndex = 0;

        public AnimationViewer()
        {
            player = new AnimationPlayer();
        }

        public AnimationViewer(GraphicsInfoController mesh, AnimationController animation, FileController tFile)
        {
            targetFile = tFile;
            file = mesh.MainFile;
            graphicsInfo = mesh;
            this.animation = animation;
            player = new AnimationPlayer(animation);
            zFar = 50F;
            SetupVBORender();

            animUpdateTimer = new Timer
            {
                Interval = (int)Math.Floor(1.0 / 60 * 1000), //Set to ~60fps by default, TODO: Add to Preferences later
                Enabled = true
            };

            animUpdateTimer.Tick += delegate (object sender, EventArgs e)
            {
                UpdateAnimation(1.0f / FPS);
            };
        }

        private void SetupVBORender()
        {
            ModelController m;
            var vbos = 0;
            SectionController mesh_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(2);
            foreach(var pair in graphicsInfo.Data.ModelIDs)
            {
                foreach (RigidModel mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).Records)
                {
                    if (mod.ID == pair.Value.ModelID)
                    {
                        uint meshID = targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).GetItem<RigidModel>(mod.ID).MeshID;
                        m = mesh_sec.GetItem<ModelController>(meshID);
                        vbos += m.Data.SubModels.Count;
                        break;
                    }
                }
            }

            if (graphicsInfo.Data.SkinID != 0)
            {
                SectionController skin_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(4);
                foreach (Skin mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(4).Records)
                {
                    if (mod.ID == graphicsInfo.Data.SkinID)
                    {
                        skin = skin_sec.GetItem<SkinController>(mod.ID);
                        vbos += skin.Data.SubModels.Count;
                    }
                }
            }
            if (graphicsInfo.Data.BlendSkinID != 0)
            {
                SectionController blend_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(5);
                foreach (BlendSkin mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(5).Records)
                {
                    if (mod.ID == graphicsInfo.Data.BlendSkinID)
                    {
                        bskin = blend_sec.GetItem<BlendSkinController>(mod.ID);
                        vbos += bskin.Data.Models.Length;
                    }
                }
            }

            InitVBO(vbos, true);

            var skeleton = graphicsInfo.Data.Skeleton;
            ComputeTposeTransform(skeleton.Root, Matrix4.Identity);

            // Load skeleton in T-Pose
            skeletonBuffer = new VertexBufferData();
            skeletonBuffer.Vtx = new Vertex[graphicsInfo.Data.Joints.Length];
            skeletonBuffer.VtxInd = new uint[graphicsInfo.Data.Joints.Length];
            tposeBuffer = new Vector3[graphicsInfo.Data.Joints.Length];
            var skel = graphicsInfo.Skeleton;
            foreach (var joint in graphicsInfo.Data.Joints)
            {
                skeletonBuffer.Vtx[(int)joint.JointIndex].Pos = new Vector3(skel.BindPose.Position((int)joint.JointIndex));
                tposeBuffer[(int)joint.JointIndex] = skeletonBuffer.Vtx[(int)joint.JointIndex].Pos;
                skeletonBuffer.Vtx[(int)joint.JointIndex].Col = Vertex.ColorToABGR(Color.FromArgb(255, Color.White));
                skeletonBuffer.VtxInd[(int)joint.JointIndex] = joint.JointIndex;
            }
            UpdateSkeletonBuffer();

            if (targetFile.Data.Type == TwinsFile.FileType.RM2)
            {
                LoadOGI_PS2();
            }
        }

        protected override void RenderHUD()
        {
            base.RenderHUD();
        }

        protected override void RenderObjects()
        {
            if (vtx == null) return;

            JointTransforms.Clear();
            PoseTransforms.Clear();
            var skeleton = graphicsInfo.Data.Skeleton;
            ComputeJointTransformTree(skeleton.Root, Matrix4.Identity);

            GL.PushMatrix();

            AnimateSkeletonBuffer();
            UpdateSkeletonBuffer();

            DrawSkeleton();

            for (int i = 0; i < bskinEndIndex; i++)
            {
                vtx[i]?.DrawAllElements(PrimitiveType.Triangles, BufferPointerFlags.Normal);
            }
            for (int i = bskinEndIndex; i < skinEndIndex; i++)
            {
                vtx[i]?.DrawAllElements(PrimitiveType.Triangles, BufferPointerFlags.Normal);
            }
            for (int i = skinEndIndex; i < vtx.Count; i++)
            {
                vtx[i]?.DrawAllElements(PrimitiveType.Triangles, BufferPointerFlags.Normal);
            }

            
            GL.PopMatrix();
        }

        private void DrawSkeleton()
        {
            GL.LineWidth(10f);
            skeletonBuffer.DrawAllElements(PrimitiveType.Lines, BufferPointerFlags.Color);
            GL.LineWidth(1f);
        }

        private void AnimateSkeletonBuffer()
        {
            var skeleton = graphicsInfo.Data.Skeleton;
            AnimateSkeletonJointBuffer(skeleton.Root);
            if (skin != null)
            {
                AnimateSkeletonSkin();
            }
        }

        private void AnimateSkeletonSkin()
        {
            var vtxIndex = bskinEndIndex;
            var skeleton = graphicsInfo.Skeleton;
            for (int i = 0; i < skin.Vertices.Count; i++)
            {
                Vertex[] vbuffer = new Vertex[skin.TposeVertices[i].Length];
                for (int k = 0; k < skin.TposeVertices[i].Length; k++)
                {
                    vbuffer[k] = skin.TposeVertices[i][k];
                    var bindPose1 = skeleton.InverseBindPose[skin.JointInfos[i][k].JointIndex1] * JointTransforms[skin.JointInfos[i][k].JointIndex1];
                    var bindPose2 = skeleton.InverseBindPose[skin.JointInfos[i][k].JointIndex2] * JointTransforms[skin.JointInfos[i][k].JointIndex2];
                    var bindPose3 = skeleton.InverseBindPose[skin.JointInfos[i][k].JointIndex3] * JointTransforms[skin.JointInfos[i][k].JointIndex3];
                    Vector4 targetPos = new Vector4(skin.TposeVertices[i][k].Pos.X, skin.TposeVertices[i][k].Pos.Y, skin.TposeVertices[i][k].Pos.Z, 1);
                    var t1 = (targetPos * bindPose1) * skin.JointInfos[i][k].Weight1;
                    var t2 = (targetPos * bindPose2) * skin.JointInfos[i][k].Weight2;
                    var t3 = (targetPos * bindPose3) * skin.JointInfos[i][k].Weight3;
                    targetPos = t1 + t2 + t3;
                    skin.TposeVertices[i][k].Pos = new Vector3(targetPos.X, targetPos.Y, targetPos.Z);
                }
                vtx[vtxIndex].Vtx = skin.TposeVertices[i];
                vtx[vtxIndex].VtxInd = skin.Indices[i];
                skin.TposeVertices[i] = vbuffer;
                UpdateVBO(vtxIndex);
                vtxIndex++;
            }
        }

        private Dictionary<int, Matrix4> JointTransforms = new Dictionary<int, Matrix4>();
        private Dictionary<int, Matrix4> PoseTransforms = new Dictionary<int, Matrix4>();
        private void ComputeJointTransformTree(GraphicsInfo.JointNode joint, Matrix4 parentTransform)
        {
            var transform = ComputeJointTransform((int)joint.Joint.JointIndex, parentTransform);
            var poseTransform = ComputePoseTransform((int)joint.Joint.JointIndex, transform);
            JointTransforms.Add((int)joint.Joint.JointIndex, transform);
            PoseTransforms.Add((int)joint.Joint.JointIndex, poseTransform);

            foreach (var c in joint.Children)
            {
                ComputeJointTransformTree(c, transform);
            }
        }

        private void AnimateSkeletonJointBuffer(GraphicsInfo.JointNode joint)
        {
            var poseTransform = PoseTransforms[(int)joint.Joint.JointIndex];

            var newPosition = new Vector4(tposeBuffer[(int)joint.Joint.JointIndex], 1f) * poseTransform;
            skeletonBuffer.Vtx[(int)joint.Joint.JointIndex].Pos = newPosition.Xyz;
            skeletonBuffer.Vtx[(int)joint.Joint.JointIndex].Col = Vertex.ColorToABGR(Color.FromArgb(255, Color.White));
            skeletonBuffer.VtxInd[(int)joint.Joint.JointIndex] = joint.Joint.JointIndex;
            foreach (var c in joint.Children)
            {
                AnimateSkeletonJointBuffer(c);
            }

            var models = graphicsInfo.Data.ModelIDs.Where((v) => v.Value.JointIndex == joint.Joint.JointIndex);

            foreach (var model in models)
            {
                var vtxIndex = VbufferMap[model.Key];
                var modelId = model.Value.ModelID;

                SectionController mesh_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(2);
                foreach (RigidModel mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).Records)
                {
                    if (mod.ID == modelId)
                    {
                        uint meshID = targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).GetItem<RigidModel>(mod.ID).MeshID;
                        mesh = mesh_sec.GetItem<ModelController>(meshID);
                    }
                }

                for (int v = 0; v < mesh.Vertices.Count; v++)
                {
                    Vertex[] vbuffer = new Vertex[mesh.TposeVertices[v].Length];
                    for (int k = 0; k < mesh.TposeVertices[v].Length; k++)
                    {
                        vbuffer[k] = mesh.TposeVertices[v][k];
                        Vector4 targetPos = new Vector4(mesh.TposeVertices[v][k].Pos.X, mesh.TposeVertices[v][k].Pos.Y, mesh.TposeVertices[v][k].Pos.Z, 1);
                        targetPos *= poseTransform;
                        mesh.TposeVertices[v][k].Pos = new Vector3(targetPos.X, targetPos.Y, targetPos.Z);
                    }

                    vtx[vtxIndex].Vtx = mesh.TposeVertices[v];
                    vtx[vtxIndex].VtxInd = mesh.Indices[v];
                    mesh.TposeVertices[v] = vbuffer;
                    UpdateVBO(vtxIndex);

                    vtxIndex++;
                }
            }
        }

        private void UpdateSkeletonBuffer()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, skeletonBuffer.ID);
            if (skeletonBuffer.Vtx.Length > skeletonBuffer.LastSize)
                GL.BufferData(BufferTarget.ArrayBuffer, Vertex.SizeOf * skeletonBuffer.Vtx.Length, skeletonBuffer.Vtx, BufferUsageHint.StaticDraw);
            else
                GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)0, Vertex.SizeOf * skeletonBuffer.Vtx.Length, skeletonBuffer.Vtx);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            skeletonBuffer.LastSize = skeletonBuffer.Vtx.Length;
        }

        private Matrix4 ComputeJointTransform(int jointIndex, Matrix4 parentTransform)
        {
            var transforms = player.Play(jointIndex);
            
            var rotQuat = new Quaternion(transforms.Item1.Row2.Xyz);
            rotQuat.Y = -rotQuat.Y;
            rotQuat.Z = -rotQuat.Z;
            var rot = Matrix4.CreateFromQuaternion(rotQuat);
            if (transforms.Item2)
            {
                var jointAddRot = graphicsInfo.Data.Joints[jointIndex].Matrix[4];
                var addRot = new Quaternion(jointAddRot.X, -jointAddRot.Y, -jointAddRot.Z, jointAddRot.W);
                Quaternion.Multiply(ref addRot, ref rotQuat, out Quaternion resQuat);
                rot = Matrix4.CreateFromQuaternion(resQuat);
            }
            var localTransform = rot
               * Matrix4.CreateScale(transforms.Item1.Row1.Xyz)
                * Matrix4.CreateTranslation(transforms.Item1.Row0.Xyz);

            var jointTransform = localTransform * parentTransform;

            return jointTransform;
        }

        private void ComputeTposeTransform(GraphicsInfo.JointNode joint, Matrix4 parentTransform)
        {
            var index = joint.Joint.JointIndex;
            var localRot = Matrix4.CreateFromQuaternion(new Quaternion(
                graphicsInfo.Data.Joints[index].Matrix[2].X,
                -graphicsInfo.Data.Joints[index].Matrix[2].Y,
                -graphicsInfo.Data.Joints[index].Matrix[2].Z,
                graphicsInfo.Data.Joints[index].Matrix[2].W));
            var localTranslate = Matrix4.CreateTranslation(
                    -graphicsInfo.Data.Joints[index].Matrix[0].X,
                    graphicsInfo.Data.Joints[index].Matrix[0].Y,
                    graphicsInfo.Data.Joints[index].Matrix[0].Z);
            var localTransform = localRot * localTranslate;
            var jointTransform = localTransform * parentTransform;
            var bindMat = jointTransform;
            var skeleton = graphicsInfo.Skeleton;
            skeleton.BindPose.Set((int)index, bindMat.ExtractTranslation(), bindMat.ExtractRotation());
            bindMat.Invert();
            skeleton.InverseBindPose.Set((int)index, bindMat.ExtractTranslation(), bindMat.ExtractRotation());
            foreach (var c in joint.Children)
            {
                ComputeTposeTransform(c, jointTransform);
            }
        }

        private Matrix4 ComputePoseTransform(int jointIndex, Matrix4 jointTransform)
        {
            var skeleton = graphicsInfo.Skeleton;
            var inversePose = skeleton.InverseBindPose[jointIndex];
            var resTransform = inversePose * jointTransform;

            return resTransform;
        }

        private void UpdateAnimation(float deltaTime)
        {
            player.AdvanceClock(deltaTime);
        }

        public void LoadOGI_PS2()
        {
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;

            var vtxIndex = 0;
            if (graphicsInfo.Data.BlendSkinID != 0)
            {
                bskin.LoadMeshData();
                foreach (var list in bskin.Vertices)
                {
                    foreach (var v in list)
                    {
                        min_x = Math.Min(min_x, v.Pos.X);
                        min_y = Math.Min(min_y, v.Pos.Y);
                        min_z = Math.Min(min_z, v.Pos.Z);
                        max_x = Math.Max(max_x, v.Pos.X);
                        max_y = Math.Max(max_y, v.Pos.Y);
                        max_z = Math.Max(max_z, v.Pos.Z);
                    }
                }
                for (int i = 0; i < bskin.Vertices.Count; i++)
                {
                    vtx[vtxIndex].Vtx = bskin.Vertices[i];
                    vtx[vtxIndex].VtxInd = bskin.Indices[i];
                    Utils.TextUtils.LoadTexture(bskin.Data.Models.Select((subModel) =>
                    {
                        return subModel.MaterialID;
                    }).ToArray(), file, vtx[vtxIndex], i);
                    UpdateVBO(vtxIndex);
                    vtxIndex++;
                }
            }
            bskinEndIndex = vtxIndex;
            if (graphicsInfo.Data.SkinID != 0)
            {
                skin.LoadMeshData();
                foreach (var list in skin.Vertices)
                {
                    foreach (var v in list)
                    {
                        min_x = Math.Min(min_x, v.Pos.X);
                        min_y = Math.Min(min_y, v.Pos.Y);
                        min_z = Math.Min(min_z, v.Pos.Z);
                        max_x = Math.Max(max_x, v.Pos.X);
                        max_y = Math.Max(max_y, v.Pos.Y);
                        max_z = Math.Max(max_z, v.Pos.Z);
                    }
                }

                for (int i = 0; i < skin.Vertices.Count; i++)
                {
                    Vertex[] vbuffer = new Vertex[skin.TposeVertices[i].Length];
                    for (int k = 0; k < skin.TposeVertices[i].Length; k++)
                    {
                        vbuffer[k] = skin.TposeVertices[i][k];
                        Vector4 targetPos = new Vector4(skin.TposeVertices[i][k].Pos.X, skin.TposeVertices[i][k].Pos.Y, skin.TposeVertices[i][k].Pos.Z, 1);
                        skin.TposeVertices[i][k].Pos = new Vector3(targetPos.X, targetPos.Y, targetPos.Z);
                        skin.TposeVertices[i][k].Col = Vertex.ColorToABGR(Color.FromArgb(220, Color.White));
                    }
                    vtx[vtxIndex].Vtx = skin.TposeVertices[i];
                    vtx[vtxIndex].VtxInd = skin.Indices[i];
                    Utils.TextUtils.LoadTexture(skin.Data.SubModels.Select((subModel) =>
                    {
                        return subModel.MaterialID;
                    }).ToArray(), file, vtx[vtxIndex], i);
                    skin.TposeVertices[i] = vbuffer;
                    UpdateVBO(vtxIndex);
                    vtxIndex++;
                }
            }
            skinEndIndex = vtxIndex;
            foreach (var pair in graphicsInfo.Data.ModelIDs)
            {
                SectionController mesh_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(2);
                SectionController rigid_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(3);
                RigidModelController rigid = null;
                foreach (RigidModel mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).Records)
                {
                    if (mod.ID == pair.Value.ModelID)
                    {
                        uint meshID = targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).GetItem<RigidModel>(mod.ID).MeshID;
                        mesh = mesh_sec.GetItem<ModelController>(meshID);
                        rigid = rigid_sec.GetItem<RigidModelController>(mod.ID);
                    }
                }

                mesh.LoadMeshData();

                var skeleton = graphicsInfo.Skeleton;
                var bindPose = skeleton.BindPose[(int)pair.Value.JointIndex];
                VbufferMap.Add(pair.Key, vtxIndex);

                for (int v = 0; v < mesh.TposeVertices.Count; v++)
                {
                    for (int k = 0; k < mesh.TposeVertices[v].Length; k++)
                    {
                        Vector4 targetPos = new Vector4(mesh.TposeVertices[v][k].Pos.X, mesh.TposeVertices[v][k].Pos.Y, mesh.TposeVertices[v][k].Pos.Z, 1);
                        targetPos *= bindPose;
                        mesh.TposeVertices[v][k].Pos = new Vector3(targetPos.X, targetPos.Y, targetPos.Z);
                        mesh.TposeVertices[v][k].Col = Vertex.ColorToABGR(Color.FromArgb(220, Color.White));
                    }


                    foreach (var p in mesh.Vertices[v])
                    {
                        min_x = Math.Min(min_x, p.Pos.X);
                        min_y = Math.Min(min_y, p.Pos.Y);
                        min_z = Math.Min(min_z, p.Pos.Z);
                        max_x = Math.Max(max_x, p.Pos.X);
                        max_y = Math.Max(max_y, p.Pos.Y);
                        max_z = Math.Max(max_z, p.Pos.Z);
                    }
                    if (rigid != null)
                    {
                        Utils.TextUtils.LoadTexture(rigid.Data.MaterialIDs, file, vtx[vtxIndex], v);
                    }
                    vtx[vtxIndex].Vtx = mesh.TposeVertices[v];
                    vtx[vtxIndex].VtxInd = mesh.Indices[v];
                    UpdateVBO(vtxIndex);
                    
                    vtxIndex++;
                }


            }

            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
        }



        public void ChangeGraphicsInfo(GraphicsInfoController mesh)
        {
            graphicsInfo = mesh;
            VbufferMap.Clear();
            Utils.TextUtils.ClearTextureCache();
            SetupVBORender();
        }

        protected override void Dispose(bool disposing)
        {
            if (animUpdateTimer != null)
            {
                animUpdateTimer.Dispose();
            }
            
            base.Dispose(disposing);
        }
    }
}
