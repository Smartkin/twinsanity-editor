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
        private SkinXController skinX;
        private BlendSkinXController bskinX;
        private FileController targetFile;
        private FileController file;
        private AnimationPlayer player;
        private readonly Dictionary<int, int> VbufferMap = new Dictionary<int, int>();

        public event EventHandler FrameChanged;

        public int FPS { get => player.FPS; set { player.FPS = value; } }
        public bool Loop { get => player.Loop; set => player.Loop = value; }
        public bool Playing { get => player.Playing; set => player.Playing = value; }
        public bool Finished { get => player.Finished; }
        public int CurrentFrame { get => player.Frame; }
        public bool DrawSkeletonOutline { get; set; } = true;

        private Timer animUpdateTimer;
        private VertexBufferData skeletonBuffer;
        private Vector3[] tposeBuffer;

        private int bskinEndIndex = 0;
        private int skinEndIndex = 0;

        public AnimationViewer()
        {
            player = new AnimationPlayer();
            player.FrameChanged += OnFrameChanged;
        }

        ~AnimationViewer()
        {
            player.FrameChanged -= OnFrameChanged;
        }

        public AnimationViewer(GraphicsInfoController mesh, AnimationController animation, FileController tFile)
        {
            targetFile = tFile;
            file = mesh.MainFile;
            graphicsInfo = mesh;
            player = new AnimationPlayer(animation);
            player.FrameChanged += OnFrameChanged;
            zFar = 50F;
            SetupVBORender();

            animUpdateTimer = new Timer
            {
                Interval = (int)Math.Floor(1.0 / 60 * 1000), //Set to ~60fps by default, TODO: Add to Preferences later
                Enabled = true
            };

            animUpdateTimer.Tick += delegate (object sender, EventArgs e)
            {
                UpdateAnimation(1.0f / 60);
            };
        }

        private void SetupVBORender()
        {
            var vbos = 0;
            skin = null;
            bskin = null;
            skinX = null;
            bskinX = null;
            uint gfx_section = 11;
            if (targetFile.Data.Type == TwinsFile.FileType.MonkeyBallRM)
            {
                gfx_section = 12;
            }
            SectionController mesh_sec = targetFile.GetItem<SectionController>(gfx_section).GetItem<SectionController>(2);

            if (targetFile.Data.Type == TwinsFile.FileType.RMX)
            {
                foreach (var pair in graphicsInfo.Data.ModelIDs)
                {
                    foreach (RigidModel mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).Records)
                    {
                        if (mod.ID == pair.Value.ModelID)
                        {
                            uint meshID = targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).GetItem<RigidModel>(mod.ID).MeshID;
                            ModelXController m = mesh_sec.GetItem<ModelXController>(meshID);
                            vbos += m.Data.SubModels.Count;
                            break;
                        }
                    }
                }

                if (graphicsInfo.Data.SkinID != 0)
                {
                    SectionController skin_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(4);
                    foreach (SkinX mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(4).Records)
                    {
                        if (mod.ID == graphicsInfo.Data.SkinID)
                        {
                            skinX = skin_sec.GetItem<SkinXController>(mod.ID);
                            vbos += skinX.Data.SubModels.Count;
                            break;
                        }
                    }
                }
                if (graphicsInfo.Data.BlendSkinID != 0)
                {
                    SectionController blend_sec = targetFile.GetItem<SectionController>(11).GetItem<SectionController>(5);
                    foreach (BlendSkinX mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(5).Records)
                    {
                        if (mod.ID == graphicsInfo.Data.BlendSkinID)
                        {
                            bskinX = blend_sec.GetItem<BlendSkinXController>(mod.ID);
                            vbos += bskinX.Data.SubModels.Count;
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (var pair in graphicsInfo.Data.ModelIDs)
                {
                    foreach (RigidModel mod in targetFile.Data.GetItem<TwinsSection>(gfx_section).GetItem<TwinsSection>(3).Records)
                    {
                        if (mod.ID == pair.Value.ModelID)
                        {
                            uint meshID = targetFile.Data.GetItem<TwinsSection>(gfx_section).GetItem<TwinsSection>(3).GetItem<RigidModel>(mod.ID).MeshID;
                            ModelController m = mesh_sec.GetItem<ModelController>(meshID);
                            vbos += m.Data.SubModels.Count;
                            break;
                        }
                    }
                }

                if (graphicsInfo.Data.SkinID != 0)
                {
                    SectionController skin_sec = targetFile.GetItem<SectionController>(gfx_section).GetItem<SectionController>(4);
                    foreach (Skin mod in targetFile.Data.GetItem<TwinsSection>(gfx_section).GetItem<TwinsSection>(4).Records)
                    {
                        if (mod.ID == graphicsInfo.Data.SkinID)
                        {
                            skin = skin_sec.GetItem<SkinController>(mod.ID);
                            vbos += skin.Data.SubModels.Count;
                            break;
                        }
                    }
                }
                if (graphicsInfo.Data.BlendSkinID != 0)
                {
                    SectionController blend_sec = targetFile.GetItem<SectionController>(gfx_section).GetItem<SectionController>(5);
                    foreach (BlendSkin mod in targetFile.Data.GetItem<TwinsSection>(gfx_section).GetItem<TwinsSection>(5).Records)
                    {
                        if (mod.ID == graphicsInfo.Data.BlendSkinID)
                        {
                            bskin = blend_sec.GetItem<BlendSkinController>(mod.ID);
                            vbos += bskin.Data.Models.Length;
                            break;
                        }
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

            if (targetFile.Data.Type == TwinsFile.FileType.RM2 || targetFile.Data.Type == TwinsFile.FileType.DemoRM2 || targetFile.Data.Type == TwinsFile.FileType.MonkeyBallRM)
            {
                LoadOGI_PS2();
            }
            else if (targetFile.Data.Type == TwinsFile.FileType.RMX)
            {
                LoadOGI_Xbox();
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
            ComputeJointTransformTree(skeleton.Root, Vector4.One, Matrix4.Identity);

            GL.PushMatrix();

            AnimateSkeletonBuffer();
            UpdateSkeletonBuffer();

            if (DrawSkeletonOutline)
            {
                DrawSkeleton();
            }

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
            if (skinX != null)
            {
                AnimateSkeletonSkinX();
            }
            if (bskin != null)
            {
                AnimateSkeletonBskin();
            }
            if (bskinX != null)
            {
                AnimateSkeletonBskinX();
            }
        }

        private void AnimateSkeletonBskin()
        {
            var faceProgress = player.PlayFacial();
            var vtxIndex = 0;
            var skeleton = graphicsInfo.Skeleton;
            for (int i = 0; i < bskin.Vertices.Count; i++)
            {
                var faceVerts = new Vector3[bskin.Vertices[i].Length];
                if (faceProgress.Length > 0)
                {
                    for (var j = 0; j < faceProgress.Length; ++j)
                    {
                        var face = bskin.GetFacialPositions(j, faceProgress[j]);
                        for (int k = 0; k < bskin.Vertices[i].Length; ++k)
                        {
                            faceVerts[k] += face[i][k];
                        }
                    }
                }
                Vertex[] vbuffer = new Vertex[bskin.Vertices[i].Length];
                for (int k = 0; k < bskin.Vertices[i].Length; k++)
                {
                    vbuffer[k] = bskin.Vertices[i][k];
                    var bindPose1 = skeleton.InverseBindPose[bskin.JointInfos[i][k].JointIndex1] * JointTransforms[bskin.JointInfos[i][k].JointIndex1];
                    var bindPose2 = skeleton.InverseBindPose[bskin.JointInfos[i][k].JointIndex2] * JointTransforms[bskin.JointInfos[i][k].JointIndex2];
                    var bindPose3 = skeleton.InverseBindPose[bskin.JointInfos[i][k].JointIndex3] * JointTransforms[bskin.JointInfos[i][k].JointIndex3];
                    Vector4 targetPos = new Vector4(bskin.Vertices[i][k].Pos.X + faceVerts[k].X, bskin.Vertices[i][k].Pos.Y + faceVerts[k].Y, bskin.Vertices[i][k].Pos.Z + faceVerts[k].Z, 1);
                    var t1 = (targetPos * bindPose1) * bskin.JointInfos[i][k].Weight1;
                    var t2 = (targetPos * bindPose2) * bskin.JointInfos[i][k].Weight2;
                    var t3 = (targetPos * bindPose3) * bskin.JointInfos[i][k].Weight3;
                    targetPos = t1 + t2 + t3;
                    bskin.Vertices[i][k].Pos = new Vector3(targetPos.X, targetPos.Y, targetPos.Z);
                }
                vtx[vtxIndex].Vtx = bskin.Vertices[i];
                vtx[vtxIndex].VtxInd = bskin.Indices[i];
                bskin.Vertices[i] = vbuffer;
                UpdateVBO(vtxIndex);
                vtxIndex++;
            }
        }
        private void AnimateSkeletonBskinX()
        {
            var faceProgress = player.PlayFacial();
            var vtxIndex = 0;
            var skeleton = graphicsInfo.Skeleton;
            for (int i = 0; i < bskinX.Vertices.Count; i++)
            {
                var faceVerts = new Vector3[bskinX.Vertices[i].Length];
                if (faceProgress.Length > 0)
                {
                    for (var j = 0; j < faceProgress.Length; ++j)
                    {
                        var face = bskinX.GetFacialPositions(j, faceProgress[j]);
                        for (int k = 0; k < bskinX.Vertices[i].Length; ++k)
                        {
                            faceVerts[k] += face[i][k];
                        }
                    }
                }

                int GroupID = 0;
                int GroupVert = 0;
                Vertex[] vbuffer = new Vertex[bskinX.Vertices[i].Length];
                for (int k = 0; k < bskinX.Vertices[i].Length; k++)
                {
                    ushort Bone1 = 0;
                    ushort Bone2 = 0;
                    ushort Bone3 = 0;
                    int Joint1 = (bskinX.Data.SubModels[i].VData[k].Joint1 - 16) / 4;
                    int Joint2 = (bskinX.Data.SubModels[i].VData[k].Joint2 - 16) / 4;
                    int Joint3 = (bskinX.Data.SubModels[i].VData[k].Joint3 - 16) / 4;
                    if (Joint1 < bskinX.Data.SubModels[i].GroupJoints[GroupID].Count)
                    {
                        Bone1 = (ushort)bskinX.Data.SubModels[i].GroupJoints[GroupID][Joint1];
                        if (Bone1 > JointTransforms.Count - 1)
                            Bone1 = 0;
                    }
                    if (Joint2 < bskinX.Data.SubModels[i].GroupJoints[GroupID].Count)
                    {
                        Bone2 = (ushort)bskinX.Data.SubModels[i].GroupJoints[GroupID][Joint2];
                        if (Bone2 > JointTransforms.Count - 1)
                            Bone2 = 0;
                    }
                    if (Joint3 < bskinX.Data.SubModels[i].GroupJoints[GroupID].Count)
                    {
                        Bone3 = (ushort)bskinX.Data.SubModels[i].GroupJoints[GroupID][Joint3];
                        if (Bone3 > JointTransforms.Count - 1)
                            Bone3 = 0;
                    }

                    vbuffer[k] = bskinX.Vertices[i][k];
                    var bindPose1 = skeleton.InverseBindPose[Bone1] * JointTransforms[Bone1];
                    var bindPose2 = skeleton.InverseBindPose[Bone2] * JointTransforms[Bone2];
                    var bindPose3 = skeleton.InverseBindPose[Bone3] * JointTransforms[Bone3];
                    Vector4 targetPos = new Vector4(bskinX.Vertices[i][k].Pos.X + faceVerts[k].X, bskinX.Vertices[i][k].Pos.Y + faceVerts[k].Y, bskinX.Vertices[i][k].Pos.Z + faceVerts[k].Z, 1);
                    var t1 = (targetPos * bindPose1) * bskinX.Data.SubModels[i].VData[k].Weight1;
                    var t2 = (targetPos * bindPose2) * bskinX.Data.SubModels[i].VData[k].Weight2;
                    var t3 = (targetPos * bindPose3) * bskinX.Data.SubModels[i].VData[k].Weight3;
                    targetPos = t1 + t2 + t3;
                    bskinX.Vertices[i][k].Pos = new Vector3(targetPos.X, targetPos.Y, targetPos.Z);

                    GroupVert++;
                    if (GroupVert > bskinX.Data.SubModels[i].GroupList[GroupID] - 1)
                    {
                        GroupVert = 0;
                        GroupID++;
                    }
                }
                vtx[vtxIndex].Vtx = bskinX.Vertices[i];
                vtx[vtxIndex].VtxInd = bskinX.Indices[i];
                bskinX.Vertices[i] = vbuffer;
                UpdateVBO(vtxIndex);
                vtxIndex++;
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
        private void AnimateSkeletonSkinX()
        {
            var vtxIndex = bskinEndIndex;
            var skeleton = graphicsInfo.Skeleton;
            for (int i = 0; i < skinX.Vertices.Count; i++)
            {
                int GroupID = 0;
                int GroupVert = 0;
                Vertex[] vbuffer = new Vertex[skinX.Vertices[i].Length];
                for (int k = 0; k < skinX.Vertices[i].Length; k++)
                {
                    ushort Bone1 = 0;
                    ushort Bone2 = 0;
                    ushort Bone3 = 0;
                    int Joint1 = (skinX.Data.SubModels[i].VData[k].Joint1 - 16) / 4;
                    int Joint2 = (skinX.Data.SubModels[i].VData[k].Joint2 - 16) / 4;
                    int Joint3 = (skinX.Data.SubModels[i].VData[k].Joint3 - 16) / 4;
                    if (Joint1 < skinX.Data.SubModels[i].GroupJoints[GroupID].Count)
                    {
                        Bone1 = (ushort)skinX.Data.SubModels[i].GroupJoints[GroupID][Joint1];
                        if (Bone1 > JointTransforms.Count - 1)
                            Bone1 = 0;
                    }
                    if (Joint2 < skinX.Data.SubModels[i].GroupJoints[GroupID].Count)
                    {
                        Bone2 = (ushort)skinX.Data.SubModels[i].GroupJoints[GroupID][Joint2];
                        if (Bone2 > JointTransforms.Count - 1)
                            Bone2 = 0;
                    }
                    if (Joint3 < skinX.Data.SubModels[i].GroupJoints[GroupID].Count)
                    {
                        Bone3 = (ushort)skinX.Data.SubModels[i].GroupJoints[GroupID][Joint3];
                        if (Bone3 > JointTransforms.Count - 1)
                            Bone3 = 0;
                    }

                    vbuffer[k] = skinX.Vertices[i][k];
                    var bindPose1 = skeleton.InverseBindPose[Bone1] * JointTransforms[Bone1];
                    var bindPose2 = skeleton.InverseBindPose[Bone2] * JointTransforms[Bone2];
                    var bindPose3 = skeleton.InverseBindPose[Bone3] * JointTransforms[Bone3];
                    Vector4 targetPos = new Vector4(skinX.Vertices[i][k].Pos.X, skinX.Vertices[i][k].Pos.Y, skinX.Vertices[i][k].Pos.Z, 1);
                    var t1 = (targetPos * bindPose1) * skinX.Data.SubModels[i].VData[k].Weight1;
                    var t2 = (targetPos * bindPose2) * skinX.Data.SubModels[i].VData[k].Weight2;
                    var t3 = (targetPos * bindPose3) * skinX.Data.SubModels[i].VData[k].Weight3;
                    targetPos = t1 + t2 + t3;
                    skinX.Vertices[i][k].Pos = new Vector3(targetPos.X, targetPos.Y, targetPos.Z);

                    GroupVert++;
                    if (GroupVert > skinX.Data.SubModels[i].GroupList[GroupID] - 1)
                    {
                        GroupVert = 0;
                        GroupID++;
                    }
                }
                vtx[vtxIndex].Vtx = skinX.Vertices[i];
                vtx[vtxIndex].VtxInd = skinX.Indices[i];
                skinX.Vertices[i] = vbuffer;
                UpdateVBO(vtxIndex);
                vtxIndex++;
            }
        }

        private readonly Dictionary<int, Matrix4> JointTransforms = new Dictionary<int, Matrix4>();
        private readonly Dictionary<int, Matrix4> PoseTransforms = new Dictionary<int, Matrix4>();
        private void ComputeJointTransformTree(GraphicsInfo.JointNode joint, Vector4 parentScale, Matrix4 parentTransform)
        {
            var transform = ComputeJointTransform((int)joint.Joint.JointIndex, parentScale, parentTransform);
            var poseTransform = ComputePoseTransform((int)joint.Joint.JointIndex, transform.Item1);
            JointTransforms.Add((int)joint.Joint.JointIndex, transform.Item1);
            PoseTransforms.Add((int)joint.Joint.JointIndex, poseTransform);

            foreach (var c in joint.Children)
            {
                ComputeJointTransformTree(c, transform.Item2, transform.Item1);
            }
        }

        private void AnimateSkeletonJointBuffer(GraphicsInfo.JointNode joint)
        {
            var poseTransform = PoseTransforms[(int)joint.Joint.JointIndex];
            uint gfx_section = 11;
            if (targetFile.Data.Type == TwinsFile.FileType.MonkeyBallRM)
            {
                gfx_section = 12;
            }

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

                
                SectionController mesh_sec = targetFile.GetItem<SectionController>(gfx_section).GetItem<SectionController>(2);
                if (mesh_sec.Data.Type == SectionType.ModelX)
                {
                    ModelXController mesh = null;
                    foreach (RigidModel mod in targetFile.Data.GetItem<TwinsSection>(gfx_section).GetItem<TwinsSection>(3).Records)
                    {
                        if (mod.ID == modelId)
                        {
                            uint meshID = targetFile.Data.GetItem<TwinsSection>(gfx_section).GetItem<TwinsSection>(3).GetItem<RigidModel>(mod.ID).MeshID;
                            mesh = mesh_sec.GetItem<ModelXController>(meshID);
                            break;
                        }
                    }

                    for (int v = 0; v < mesh.Vertices.Count; v++)
                    {
                        Vertex[] vbuffer = new Vertex[mesh.Vertices[v].Length];
                        for (int k = 0; k < mesh.Vertices[v].Length; k++)
                        {
                            vbuffer[k] = mesh.Vertices[v][k];
                            Vector4 targetPos = new Vector4(mesh.Vertices[v][k].Pos.X, mesh.Vertices[v][k].Pos.Y, mesh.Vertices[v][k].Pos.Z, 1);
                            targetPos *= poseTransform;
                            mesh.Vertices[v][k].Pos = new Vector3(targetPos.X, targetPos.Y, targetPos.Z);
                        }

                        vtx[vtxIndex].Vtx = mesh.Vertices[v];
                        vtx[vtxIndex].VtxInd = mesh.Indices[v];
                        mesh.Vertices[v] = vbuffer;
                        UpdateVBO(vtxIndex);

                        vtxIndex++;
                    }
                }
                else
                {
                    ModelController mesh = null;
                    foreach (RigidModel mod in targetFile.Data.GetItem<TwinsSection>(gfx_section).GetItem<TwinsSection>(3).Records)
                    {
                        if (mod.ID == modelId)
                        {
                            uint meshID = targetFile.Data.GetItem<TwinsSection>(gfx_section).GetItem<TwinsSection>(3).GetItem<RigidModel>(mod.ID).MeshID;
                            mesh = mesh_sec.GetItem<ModelController>(meshID);
                            break;
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

        private Tuple<Matrix4, Vector4> ComputeJointTransform(int jointIndex, Vector4 parentScale, Matrix4 parentTransform)
        {
            var transforms = player.Play(jointIndex);

            var slerpedRot = transforms.Item2;
            slerpedRot.X = -slerpedRot.X;

            var rot = Matrix4.CreateFromQuaternion(slerpedRot);
            if (transforms.Item3)
            {
                var jointAddRot = graphicsInfo.Data.Joints[jointIndex].Matrix[4];
                var addRot = new Quaternion(-jointAddRot.X, jointAddRot.Y, jointAddRot.Z, -jointAddRot.W);
                Quaternion.Multiply(ref addRot, ref slerpedRot, out Quaternion resQuat);
                rot = Matrix4.CreateFromQuaternion(resQuat);
            }

            var jointScale = new Vector4(transforms.Item1.Row1.Xyz, 1.0f);
            var resultScale = new Vector3(jointScale.X / parentScale.X, jointScale.Y / parentScale.Y, jointScale.Z / parentScale.Z);
            var scale = Matrix4.CreateScale(resultScale);

            var localTransform = rot * scale * Matrix4.CreateTranslation(transforms.Item1.Row0.Xyz);
            var jointTransform = localTransform * parentTransform;

            return new Tuple<Matrix4, Vector4>(jointTransform, new Vector4(jointTransform.ExtractScale(), 1.0f));
        }

        private void ComputeTposeTransform(GraphicsInfo.JointNode joint, Matrix4 parentTransform)
        {
            var index = joint.Joint.JointIndex;
            var localRot = Matrix4.CreateFromQuaternion(new Quaternion(
                -graphicsInfo.Data.Joints[index].Matrix[2].X,
                graphicsInfo.Data.Joints[index].Matrix[2].Y,
                graphicsInfo.Data.Joints[index].Matrix[2].Z,
                -graphicsInfo.Data.Joints[index].Matrix[2].W));
            var localTranslate = Matrix4.CreateTranslation(
                    -graphicsInfo.Data.Joints[index].Matrix[0].X,
                    graphicsInfo.Data.Joints[index].Matrix[0].Y,
                    graphicsInfo.Data.Joints[index].Matrix[0].Z);
            var localTransform = localRot * localTranslate;
            var jointTransform = localTransform * parentTransform;
            var bindMat = jointTransform;
            var skeleton = graphicsInfo.Skeleton;
            skeleton.BindPose.Set((int)index, bindMat);
            bindMat.Invert();
            skeleton.InverseBindPose.Set((int)index, bindMat);
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
            uint gfx_section = 11;
            if (targetFile.Data.Type == TwinsFile.FileType.MonkeyBallRM)
            {
                gfx_section = 12;
            }

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
                        skin.TposeVertices[i][k].Col = Vertex.ColorToABGR(Color.FromArgb(255, Color.White));
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
                SectionController mesh_sec = targetFile.GetItem<SectionController>(gfx_section).GetItem<SectionController>(2);
                SectionController rigid_sec = targetFile.GetItem<SectionController>(gfx_section).GetItem<SectionController>(3);
                RigidModelController rigid = null;
                ModelController mesh = null;
                foreach (RigidModel mod in targetFile.Data.GetItem<TwinsSection>(gfx_section).GetItem<TwinsSection>(3).Records)
                {
                    if (mod.ID == pair.Value.ModelID)
                    {
                        uint meshID = targetFile.Data.GetItem<TwinsSection>(gfx_section).GetItem<TwinsSection>(3).GetItem<RigidModel>(mod.ID).MeshID;
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
                        mesh.TposeVertices[v][k].Col = Vertex.ColorToABGR(Color.FromArgb(255, Color.White));
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

        public void LoadOGI_Xbox()
        {
            float min_x = float.MaxValue, min_y = float.MaxValue, min_z = float.MaxValue, max_x = float.MinValue, max_y = float.MinValue, max_z = float.MinValue;

            var vtxIndex = 0;
            if (graphicsInfo.Data.BlendSkinID != 0)
            {
                bskinX.LoadMeshData();
                foreach (var list in bskinX.Vertices)
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
                for (int i = 0; i < bskinX.Vertices.Count; i++)
                {
                    vtx[vtxIndex].Vtx = bskinX.Vertices[i];
                    vtx[vtxIndex].VtxInd = bskinX.Indices[i];
                    Utils.TextUtils.LoadTexture(bskinX.Data.SubModels.Select((subModel) =>
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
                skinX.LoadMeshData();
                foreach (var list in skinX.Vertices)
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

                for (int i = 0; i < skinX.Vertices.Count; i++)
                {
                    Vertex[] vbuffer = new Vertex[skinX.Vertices[i].Length];
                    for (int k = 0; k < skinX.Vertices[i].Length; k++)
                    {
                        vbuffer[k] = skinX.Vertices[i][k];
                        Vector4 targetPos = new Vector4(skinX.Vertices[i][k].Pos.X, skinX.Vertices[i][k].Pos.Y, skinX.Vertices[i][k].Pos.Z, 1);
                        skinX.Vertices[i][k].Pos = new Vector3(targetPos.X, targetPos.Y, targetPos.Z);
                        //skinX.Vertices[i][k].Col = Vertex.ColorToABGR(Color.FromArgb(255, Color.White));
                    }
                    vtx[vtxIndex].Vtx = skinX.Vertices[i];
                    vtx[vtxIndex].VtxInd = skinX.Indices[i];
                    Utils.TextUtils.LoadTexture(skinX.Data.SubModels.Select((subModel) =>
                    {
                        return subModel.MaterialID;
                    }).ToArray(), file, vtx[vtxIndex], i);
                    skinX.Vertices[i] = vbuffer;
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
                ModelXController mesh = null;
                foreach (RigidModel mod in targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).Records)
                {
                    if (mod.ID == pair.Value.ModelID)
                    {
                        uint meshID = targetFile.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3).GetItem<RigidModel>(mod.ID).MeshID;
                        mesh = mesh_sec.GetItem<ModelXController>(meshID);
                        rigid = rigid_sec.GetItem<RigidModelController>(mod.ID);
                    }
                }

                mesh.LoadMeshData();

                var skeleton = graphicsInfo.Skeleton;
                var bindPose = skeleton.BindPose[(int)pair.Value.JointIndex];
                VbufferMap.Add(pair.Key, vtxIndex);

                for (int v = 0; v < mesh.Vertices.Count; v++)
                {
                    for (int k = 0; k < mesh.Vertices[v].Length; k++)
                    {
                        Vector4 targetPos = new Vector4(mesh.Vertices[v][k].Pos.X, mesh.Vertices[v][k].Pos.Y, mesh.Vertices[v][k].Pos.Z, 1);
                        targetPos *= bindPose;
                        mesh.Vertices[v][k].Pos = new Vector3(targetPos.X, targetPos.Y, targetPos.Z);
                        //mesh.Vertices[v][k].Col = Vertex.ColorToABGR(Color.FromArgb(255, Color.White));
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
                    vtx[vtxIndex].Vtx = mesh.Vertices[v];
                    vtx[vtxIndex].VtxInd = mesh.Indices[v];
                    UpdateVBO(vtxIndex);

                    vtxIndex++;
                }


            }

            zFar = Math.Max(zFar, Math.Max(max_x - min_x, Math.Max(max_y - min_y, max_z - min_z)));
        }

        private void OnFrameChanged(object sender, EventArgs e)
        {
            FrameChanged?.Invoke(this, e);
        }

        public void ChangeGraphicsInfo(GraphicsInfoController mesh)
        {
            graphicsInfo = mesh;
            vtx.Clear();
            VbufferMap.Clear();
            Utils.TextUtils.ClearTextureCache();
            SetupVBORender();
        }

        public void ChangeAnimationFrame(int frame)
        {
            player.Frame = frame;
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
