﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Twinsanity
{
    public class GraphicsInfo : TwinsItem
    {

        public Dictionary<uint, int> ModelIDs { get; set; } = new Dictionary<uint, int>();
        public uint SkinID { get; set; }
        public uint BlendSkinID { get; set; }
        public Pos Coord1 { get; set; } // Bounding box?
        public Pos Coord2 { get; set; } // Bounding box?
        public Joint[] Joints { get; set; }
        public ExitPoint[] ExitPoints { get; set; }
        public GI_Type3[] Type3 { get; set; }
        public GI_CollisionData[] CollisionData { get; set; }
        public JointTree Skeleton { get; private set; }

        public byte[] HeaderVars;
        public byte[] CollisionDataRelated;

        public override void Save(BinaryWriter writer)
        {
            writer.Write(HeaderVars);
            writer.Write(Coord1.X);
            writer.Write(Coord1.Y);
            writer.Write(Coord1.Z);
            writer.Write(Coord1.W);
            writer.Write(Coord2.X);
            writer.Write(Coord2.Y);
            writer.Write(Coord2.Z);
            writer.Write(Coord2.W);

            if (Joints.Length > 0)
            {
                for (int i = 0; i < Joints.Length; i++)
                {
                    writer.Write(Joints[i].ReactJointID);
                    writer.Write(Joints[i].JointIndex);
                    writer.Write(Joints[i].ParentJointIndex);
                    writer.Write(Joints[i].ChildJointAmount);
                    writer.Write(Joints[i].ChildJointAmount2);
                    for (int a = 0; a < Joints[i].Matrix.Length; a++)
                    {
                        writer.Write(Joints[i].Matrix[a].X);
                        writer.Write(Joints[i].Matrix[a].Y);
                        writer.Write(Joints[i].Matrix[a].Z);
                        writer.Write(Joints[i].Matrix[a].W);
                    }
                }
            }

            if (ExitPoints.Length > 0)
            {
                for (int i = 0; i < ExitPoints.Length; i++)
                {
                    for (int a = 0; a < ExitPoints[i].Numbers.Length; a++)
                    {
                        writer.Write(ExitPoints[i].Numbers[a]);
                    }
                    for (int a = 0; a < ExitPoints[i].Matrix.Length; a++)
                    {
                        writer.Write(ExitPoints[i].Matrix[a].X);
                        writer.Write(ExitPoints[i].Matrix[a].Y);
                        writer.Write(ExitPoints[i].Matrix[a].Z);
                        writer.Write(ExitPoints[i].Matrix[a].W);
                    }
                }
            }

            if (ModelIDs.Count > 0)
            {
                foreach (var pair in ModelIDs)
                {
                    writer.Write((byte)pair.Value);
                }
                foreach (var pair in ModelIDs)
                {
                    writer.Write(pair.Key);
                }
            }

            if (Joints.Length > 0)
            {
                for (int a = 0; a < Type3.Length; a++)
                {
                    for (int i = 0; i < Type3[a].Matrix.Length; i++)
                    {
                        writer.Write(Type3[a].Matrix[i].X);
                        writer.Write(Type3[a].Matrix[i].Y);
                        writer.Write(Type3[a].Matrix[i].Z);
                        writer.Write(Type3[a].Matrix[i].W);
                    }
                }
            }

            writer.Write(SkinID);
            writer.Write(BlendSkinID);

            if (CollisionData.Length > 0)
            {
                for (int a = 0; a < CollisionData.Length; a++)
                {
                    for (var i = 0; i < 11; ++i)
                    {
                        writer.Write(CollisionData[a].Header[i]);
                    }
                    writer.Write(CollisionData[a].collisionDataBlob.Length);
                    writer.Write(CollisionData[a].collisionDataBlob);
                }
            }

            writer.Write(CollisionDataRelated);

        }

        public override string ToString()
        {
            return DefaultHashes.Hash_OGI[ID];
        }

        public override void Load(BinaryReader reader, int size)
        {
            long pre_pos = reader.BaseStream.Position;

            HeaderVars = new byte[0x10];
            HeaderVars = reader.ReadBytes(0x10);

            uint jointsAmt = HeaderVars[0];
            uint exitPointsAmt = HeaderVars[1];
            uint reactJointsAmt = HeaderVars[2]; // These joints rotate and react to camera movement
            uint Model_Size = HeaderVars[5];
            uint SkinFlag = HeaderVars[6];
            uint BlendSkinFlag = HeaderVars[7];
            int collisionDataAmount = HeaderVars[8];

            Coord1 = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            Coord2 = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

            if (jointsAmt > 0)
            {
                Joints = new Joint[jointsAmt];
                for (int i = 0; i < jointsAmt; i++)
                {
                    Pos[] Type1_Matrix = new Pos[5];
                    uint[] Type1_Numbers = new uint[5];
                    for (int a = 0; a < Type1_Numbers.Length; a++)
                    {
                        Type1_Numbers[a] = reader.ReadUInt32();
                    }
                    Type1_Matrix[0] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    Type1_Matrix[1] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    Type1_Matrix[2] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    Type1_Matrix[3] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    Type1_Matrix[4] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    Joint temp_Type1 = new Joint()
                    {
                        Matrix = Type1_Matrix,
                        ReactJointID = Type1_Numbers[0],
                        JointIndex = Type1_Numbers[1],
                        ParentJointIndex = Type1_Numbers[2],
                        ChildJointAmount = Type1_Numbers[3],
                        ChildJointAmount2 = Type1_Numbers[4]
                    };
                    Joints[i] = temp_Type1;
                }
            }
            else
            {
                Joints = new Joint[0];
            }

            if (exitPointsAmt > 0)
            {
                ExitPoints = new ExitPoint[exitPointsAmt];
                for (int i = 0; i < exitPointsAmt; i++)
                {
                    Pos[] Type2_Matrix = new Pos[4];
                    uint[] Type2_Numbers = new uint[2];
                    Type2_Numbers[0] = reader.ReadUInt32();
                    Type2_Numbers[1] = reader.ReadUInt32();
                    for (int a = 0; a < Type2_Matrix.Length; a++)
                    {
                        Type2_Matrix[a] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    }
                    ExitPoint temp_Type2 = new ExitPoint() { Matrix = Type2_Matrix, Numbers = Type2_Numbers };
                    ExitPoints[i] = temp_Type2;
                }
            }
            else
            {
                ExitPoints = new ExitPoint[0];
            }

            if (Model_Size > 0)
            {
                ModelIDs = new Dictionary<uint, int>();
                uint[] IDs = new uint[Model_Size];
                uint[] IDs_m = new uint[Model_Size];
                for (int i = 0; i < Model_Size; i++)
                {
                    IDs[i] = reader.ReadByte();
                }
                for (int i = 0; i < Model_Size; i++)
                {
                    IDs_m[i] = reader.ReadUInt32();
                }
                for (int i = 0; i < Model_Size; i++)
                {
                    ModelIDs.Add(IDs_m[i], (int)IDs[i]);
                }

            }
            else
            {
                ModelIDs = new Dictionary<uint, int>();
            }

            if (jointsAmt > 0)
            {
                Type3 = new GI_Type3[jointsAmt];
                for (int a = 0; a < jointsAmt; a++)
                {
                    Pos[] Type3_Matrix = new Pos[4];
                    for (int i = 0; i < Type3_Matrix.Length; i++)
                    {
                        Type3_Matrix[i] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    }
                    Type3[a] = new GI_Type3() { Matrix = Type3_Matrix };
                }
            }
            else
            {
                Type3 = new GI_Type3[0];
            }

            SkinID = reader.ReadUInt32();

            BlendSkinID = reader.ReadUInt32();

            if (collisionDataAmount > 0)
            {
                CollisionData = new GI_CollisionData[collisionDataAmount];
                for (int a = 0; a < collisionDataAmount; a++)
                {
                    ushort[] header = new ushort[11];
                    for (var i = 0; i < 11; ++i)
                    {
                        header[i] = reader.ReadUInt16();
                    }
                    int blobSize = reader.ReadInt32();
                    byte[] Blob = reader.ReadBytes(blobSize);
                    CollisionData[a] = new GI_CollisionData() { Header = header, collisionDataBlob = Blob };
                    using (MemoryStream memoryStream = new MemoryStream(Blob))
                    {
                        using (BinaryReader blobReader = new BinaryReader(memoryStream))
                        {
                            var firstBlockElemAmt = header[0];
                            var secondBlockElemAmt = header[1];
                            var thirdBlockElemAmt = header[2];
                            var fourthBlockElemAmt = header[3];
                            var fifthBlockElemAmt = header[4];
                            var sixthBlockElemAmt = header[10] - header[9];
                            var seventhBlockElemAmt = blobSize - header[10];
                            var secondBlockPos = header[5];
                            var thirdBlockPos = header[6];
                            var fourthBlockPos = header[7];
                            var fifthBlockPos = header[8];
                            var sixthBlockPos = header[9];
                            var seventhBlockPos = header[10];
                            CollisionData[a].UnkVectors1 = new TwinsVector4[firstBlockElemAmt];
                            for (var i = 0; i < firstBlockElemAmt; ++i)
                            {
                                CollisionData[a].UnkVectors1[i] = new TwinsVector4();
                                CollisionData[a].UnkVectors1[i].Load(blobReader, 16);
                            }
                        }
                    }
                }
            }
            else
            {
                CollisionData = new GI_CollisionData[0];
            }

            CollisionDataRelated = reader.ReadBytes(collisionDataAmount);

            BuildSkeleton();
        }

        private void BuildSkeleton()
        {
            Skeleton = new JointTree
            {
                Root = new JointNode(Joints[0]),
                Nodes = new Dictionary<int, JointNode>()
            };
            Skeleton.Nodes.Add(0, Skeleton.Root);

            for (int i = 1; i < Joints.Length; i++)
            {
                var joint = Joints[i];
                var node = new JointNode(joint);
                Skeleton.Nodes.Add((int)joint.JointIndex, node);
                if (Skeleton.Nodes.ContainsKey((int)joint.ParentJointIndex))
                {
                    var parent = Skeleton.Nodes[(int)joint.ParentJointIndex];
                    parent.Children.Add(node);
                }
            }

            // Children amount validation
            foreach (var node in Skeleton.Nodes)
            {
                if (node.Value.Joint.ChildJointAmount != node.Value.Children.Count)
                {
                    Console.WriteLine($"WARNING: Joint {node.Value.Joint.JointIndex} in OGI 0x{ID:X} {ToString()} was supposed to have {node.Value.Joint.ChildJointAmount} children, instead has {node.Value.Children.Count}");
                }
            }
        }

        protected override int GetSize()
        {
            int count = 0x10 + 16 + 16 + 4 + 4 + CollisionDataRelated.Length;

            if (Joints.Length > 0)
            {
                for (int i = 0; i < Joints.Length; i++)
                {
                    count += 5 * 4;
                    count += Joints[i].Matrix.Length * 16;
                }
            }

            if (ExitPoints.Length > 0)
            {
                for (int i = 0; i < ExitPoints.Length; i++)
                {
                    count += ExitPoints[i].Numbers.Length * 4;
                    count += ExitPoints[i].Matrix.Length * 16;
                }
            }

            if (ModelIDs.Count > 0)
            {
                count += ModelIDs.Count * 5;
            }

            if (Type3.Length > 0)
            {
                for (int i = 0; i < Type3.Length; i++)
                {
                    count += Type3[i].Matrix.Length * 16;
                }
            }

            if (CollisionData.Length > 0)
            {
                for (int i = 0; i < CollisionData.Length; i++)
                {
                    count += CollisionData[i].collisionDataBlob.Length + 4 + 0x16;
                }
            }

            return count;
        }

        public struct JointTree
        {
            public JointNode Root;
            public Dictionary<int, JointNode> Nodes;
        }

        public struct JointNode
        {
            public List<JointNode> Children;
            public Joint Joint;
            public JointNode(Joint joint)
            {
                Children = new List<JointNode>();
                Joint = joint;
            }
        }

        public struct ModelLink
        {
            public uint JointIndex;
            public uint ModelID;
        }

        public struct Joint
        {
            public uint ReactJointID;
            public uint JointIndex;
            public uint ParentJointIndex;
            public uint ChildJointAmount;
            public uint ChildJointAmount2;
            public Pos[] Matrix; // 5
        }

        public struct ExitPoint
        {
            public uint[] Numbers; // 2
            public Pos[] Matrix; // 4
        }

        public struct GI_Type3
        {
            public Pos[] Matrix; // 4
        }

        public class GI_CollisionData
        {
            public ushort[] Header; //0x16
            public byte[] collisionDataBlob; //blobSize
            public TwinsVector4[] UnkVectors1;
        }

        public void FillPackage(TwinsFile source, TwinsFile destination)
        {
            var sourceModels = source.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3);
            var destinationModels = destination.GetItem<TwinsSection>(11).GetItem<TwinsSection>(3);
            var sourceSkins = source.GetItem<TwinsSection>(11).GetItem<TwinsSection>(4);
            var destinationSkins = destination.GetItem<TwinsSection>(11).GetItem<TwinsSection>(4);
            var sourceBlend = source.GetItem<TwinsSection>(11).GetItem<TwinsSection>(5);
            var destinationBlend = destination.GetItem<TwinsSection>(11).GetItem<TwinsSection>(5);
            foreach (var modelID in ModelIDs)
            {
                if (destinationModels.HasItem(modelID.Key))
                {
                    continue;
                }
                var linkedModel = sourceModels.GetItem<RigidModel>(modelID.Key);
                destinationModels.AddItem(modelID.Key, linkedModel);
                linkedModel.FillPackage(source, destination);
            }
            if (sourceSkins.HasItem(SkinID))
            {
                var linkedSkin = sourceSkins.GetItem<Skin>(SkinID);
                destinationSkins.AddItem(SkinID, linkedSkin);
                linkedSkin.FillPackage(source, destination);
            }
            if (sourceBlend.HasItem(BlendSkinID))
            {
                var linkedBlend = sourceBlend.GetItem<BlendSkin>(BlendSkinID);
                destinationBlend.AddItem(BlendSkinID, linkedBlend);
                linkedBlend.FillPackage(source, destination);
            }
        }
    }
}
