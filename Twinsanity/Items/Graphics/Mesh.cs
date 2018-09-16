using System;
using System.IO;
using System.Collections.Generic;

namespace Twinsanity
{
    public class Mesh : TwinsItem
    {
        private List<SubModel> submodels = new List<SubModel>();

        public List<SubModel> SubModels { get => submodels; set => submodels = value; }

        public override void Load(BinaryReader reader, int size)
        {
            var sk = reader.BaseStream.Position;
            var count = reader.ReadInt32();

            SubModels.Clear();
            for (int i = 0; i < count; i++)
            {
                SubModel sub = new SubModel() {
                    VertexCount = reader.ReadInt32(),
                    BlockSize = reader.ReadUInt32(),
                    k = reader.ReadUInt16(),
                    c = reader.ReadUInt16(),
                    Null1 = reader.ReadUInt32(),
                    Something = reader.ReadUInt32(),
                    Null2 = reader.ReadUInt32()
                };
                int cnt = 0, offset = (int)(reader.BaseStream.Position - sk) - 12;
                sub.Groups = new List<Group>();
                while ((cnt < sub.VertexCount) && (reader.BaseStream.Position - sk < offset + sub.BlockSize))
                {
                    Group grp = new Group {
                        SomeNum1 = reader.ReadUInt32(),
                        VertexCount = reader.ReadByte(),
                        Some80h = reader.ReadByte(),
                        Null1 = reader.ReadUInt16(),
                        SomeNum2 = reader.ReadUInt32(),
                        SomeNum3 = reader.ReadUInt32(),
                        Null2 = reader.ReadUInt32(),
                        Signature1 = reader.ReadUInt32(),
                        SomeShit1 = reader.ReadUInt32(),
                        SomeShit2 = reader.ReadUInt32(),
                        SomeShit3 = reader.ReadUInt32(),
                        Signature2 = reader.ReadUInt32()
                    };
                    cnt += grp.VertexCount;
                    uint head = reader.ReadUInt32();
                    while (head != 0x14000000)
                    {
                        switch (head & 255)
                        {
                            case 3:
                                {
                                    grp.VertHead = head;
                                    grp.Vertex = new Position3[grp.VertexCount];
                                    for (int j = 0; j < grp.VertexCount; j++)
                                    {
                                        grp.Vertex[j].X = reader.ReadSingle();
                                        grp.Vertex[j].Y = reader.ReadSingle();
                                        grp.Vertex[j].Z = reader.ReadSingle();
                                    }

                                    break;
                                }

                            case 4:
                                {
                                    grp.WeightHead = head;
                                    grp.Weight = new Weight[grp.VertexCount];
                                    for (int j = 0; j < grp.VertexCount; j++)
                                    {
                                        grp.Weight[j].X = reader.ReadSingle();
                                        grp.Weight[j].Y = reader.ReadSingle();
                                        grp.Weight[j].Z = reader.ReadSingle();
                                        grp.Weight[j].SomeByte = reader.ReadByte();
                                        grp.Weight[j].CONN = reader.ReadByte();
                                        grp.Weight[j].Null1 = reader.ReadUInt16();
                                    }

                                    break;
                                }

                            case 5:
                                {
                                    grp.UVHead = head;
                                    grp.UV = new Position3[grp.VertexCount];
                                    for (int j = 0; j < grp.VertexCount; j++)
                                    {
                                        grp.UV[j].X = reader.ReadSingle();
                                        grp.UV[j].Y = reader.ReadSingle();
                                        grp.UV[j].Z = reader.ReadSingle();
                                    }

                                    break;
                                }

                            case 6:
                                {
                                    grp.ShiteHead = head;
                                    grp.Shit = new uint[grp.VertexCount];
                                    for (int j = 0; j < grp.VertexCount; j++)
                                        grp.Shit[j] = reader.ReadUInt32();
                                    break;
                                }
                        }
                        head = reader.ReadUInt32();
                    }
                    grp.EndSignature1 = head;
                    grp.EndSignature2 = reader.ReadUInt32();
                    grp.leftovers = new byte[] { };
                    sub.Groups.Add(grp);
                }
                Group group = sub.Groups[sub.Groups.Count - 1];
                group.leftovers = new byte[(int)(sub.BlockSize + offset - (reader.BaseStream.Position - sk))];
                group.leftovers = reader.ReadBytes((int)(sub.BlockSize + offset - (reader.BaseStream.Position - sk)));

                sub.Groups[sub.Groups.Count - 1] = group;
                SubModels.Add(sub);
            }
        }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(SubModels.Count);
            for (int i = 0; i < SubModels.Count; ++i)
            {
                {
                    var sub = SubModels[i];
                    writer.Write(sub.VertexCount);
                    writer.Write(sub.BlockSize);
                    writer.Write(sub.k);
                    writer.Write(sub.c);
                    writer.Write(sub.Null1);
                    writer.Write(sub.Something);
                    writer.Write(sub.Null2);
                    for (int a = 0; a < sub.Groups.Count; ++a)
                    {
                        {
                            var group = sub.Groups[a];
                            writer.Write(group.SomeNum1);
                            writer.Write(group.VertexCount);
                            writer.Write(group.Some80h);
                            writer.Write(group.Null1);
                            writer.Write(group.SomeNum2);
                            writer.Write(group.SomeNum3);
                            writer.Write(group.Null2);
                            writer.Write(group.Signature1);
                            writer.Write(group.SomeShit1);
                            writer.Write(group.SomeShit2);
                            writer.Write(group.SomeShit3);
                            writer.Write(group.Signature2);
                            if (group.VertHead > 0)
                            {
                                writer.Write(group.VertHead);
                                for (int j = 0; j < group.VertexCount; ++j)
                                {
                                    writer.Write(group.Vertex[j].X);
                                    writer.Write(group.Vertex[j].Y);
                                    writer.Write(group.Vertex[j].Z);
                                }
                            }
                            if (group.WeightHead > 0)
                            {
                                writer.Write(group.WeightHead);
                                for (int j = 0; j < group.VertexCount; ++j)
                                {
                                    writer.Write(group.Weight[j].X);
                                    writer.Write(group.Weight[j].Y);
                                    writer.Write(group.Weight[j].Z);
                                    writer.Write(group.Weight[j].SomeByte);
                                    writer.Write(group.Weight[j].CONN);
                                    writer.Write(group.Weight[j].Null1);
                                }
                            }
                            if (group.UVHead > 0)
                            {
                                writer.Write(group.UVHead);
                                for (int j = 0; j < group.VertexCount; ++j)
                                {
                                    writer.Write(group.UV[j].X);
                                    writer.Write(group.UV[j].Y);
                                    writer.Write(group.UV[j].Z);
                                }
                            }
                            if (group.ShiteHead > 0)
                            {
                                writer.Write(group.ShiteHead);
                                for (int j = 0; j < group.VertexCount; ++j)
                                    writer.Write(group.Shit[j]);
                            }
                            writer.Write(group.EndSignature1);
                            writer.Write(group.EndSignature2);
                        }
                    }
                    writer.Write(sub.Groups[sub.Groups.Count - 1].leftovers);
                }
            }
        }

        protected override int GetSize()
        {
            int size = 4;
            foreach(var i in SubModels)
            {
                size += 24;
                foreach (var j in i.Groups)
                {
                    size += 48;
                    if (j.VertHead > 0)
                    {
                        size += 4 + 12 * j.VertexCount;
                    }
                    if (j.WeightHead > 0)
                    {
                        size += 4 + 16 * j.VertexCount;
                    }
                    if (j.UVHead > 0)
                    {
                        size += 4 + 12 * j.VertexCount;
                    }
                    if (j.ShiteHead > 0)
                    {
                        size += 4 + 4 * j.VertexCount;
                    }
                    size += j.leftovers.Length;
                }
            }
            return size;
        }

        public void Import(RawData[][] RawData)
        {
            /*
            SubModels = RawData.Length;
            Array.Resize(ref SubModel, SubModels);
            for (int i = 0; i <= SubModels - 1; i++)
            {
                var Groups = Math.Ceiling(RawData[i].Length / (double)33);
                SubModel[i].VertexCount = RawData[i].Length;
                SubModel[i].BlockSize = (uint)(12 + Groups * (48 + 16) + RawData[i].Length * (12 + 16 + 12 + 4) + 20);
                SubModel[i].k = (ushort)((SubModel[i].BlockSize - 20) / 16);
                SubModel[i].c = 24576;
                SubModel[i].Null1 = 0;
                SubModel[i].Something = 0x70000FA;
                SubModel[i].Null2 = 0;
                Array.Resize(ref SubModel[i].Group, (int)Groups);
                uint offset = 0;
                for (int j = 0; j <= SubModel[i].Group.Length - 1; j++)
                {
                    {
                        var withBlock = SubModel[i].Group[j];
                        withBlock.SomeNum1 = 0x6C018000;
                        if (j < SubModel[i].Group.Length - 1)
                            withBlock.VertexCount = 33;
                        else
                            withBlock.VertexCount = (byte)(RawData[i].Length - 33 * (SubModel[i].Group.Length - 1));
                        Array.Resize(ref withBlock.Vertex, withBlock.VertexCount);
                        Array.Resize(ref withBlock.Weight, withBlock.VertexCount);
                        Array.Resize(ref withBlock.UV, withBlock.VertexCount);
                        Array.Resize(ref withBlock.Shit, withBlock.VertexCount);
                        withBlock.Some80h = 128;
                        withBlock.Null1 = 0;
                        withBlock.SomeNum2 = 0x30024000;
                        withBlock.SomeNum3 = 0x512;
                        withBlock.Null2 = 0;
                        withBlock.Signature1 = 0x1000101;
                        withBlock.SomeShit1 = 0x64018001;
                        withBlock.SomeShit2 = (uint)withBlock.VertexCount * 4;
                        withBlock.SomeShit3 = (uint)withBlock.VertexCount + withBlock.Some80h << 8;
                        withBlock.Signature2 = 0x1000104;
                        withBlock.VertHead = 0x68008003 + (uint)withBlock.VertexCount * 65536;  // 0x0380XX68
                        withBlock.WeightHead = 0x6C008004 + (uint)withBlock.VertexCount * 65536;
                        withBlock.UVHead = 0x68008005 + (uint)withBlock.VertexCount * 65536;
                        withBlock.ShiteHead = 0x6E008006 + (uint)withBlock.VertexCount * 65536;
                        for (int k = 0; k <= withBlock.VertexCount - 1; k++)
                        {
                            withBlock.Vertex[k].X = RawData[i][k + offset].X;
                            withBlock.Vertex[k].Y = RawData[i][k + offset].Y;
                            withBlock.Vertex[k].Z = RawData[i][k + offset].Z;
                            withBlock.Weight[k].X = RawData[i][k + offset].U;
                            withBlock.Weight[k].Y = RawData[i][k + offset].V;
                            withBlock.Weight[k].Z = RawData[i][k + offset].W;
                            withBlock.Weight[k].SomeByte = 127;
                            withBlock.Weight[k].CONN = RawData[i][k + offset].CONN == true ? (byte)0 : (byte)128;
                            withBlock.UV[k].X = RawData[i][k + offset].Nx;
                            withBlock.UV[k].Y = RawData[i][k + offset].Ny;
                            withBlock.UV[k].Z = RawData[i][k + offset].Nz;
                            withBlock.Shit[k] = RawData[i][k + offset].Diffuse;
                        }
                        offset += withBlock.VertexCount;
                        withBlock.EndSignature1 = 0x14000000;
                        withBlock.EndSignature2 = 0x1000101;
                        Array.Resize(ref withBlock.leftovers, 20);
                        SubModel[i].Group[j] = withBlock;
                    }
                }
            }
            UpdateStream();*/
        }

        #region STRUCTURES
        public struct SubModel
        {
            // Primary Header
            public int VertexCount;
            public uint BlockSize;
            public ushort k, c;
            public uint Null1;
            public uint Something;
            public uint Null2;
            public List<Group> Groups;
        }
        public struct Group
        {
            public uint SomeNum1;
            public byte VertexCount;
            public byte Some80h;
            public ushort Null1;
            public uint SomeNum2;
            public uint SomeNum3;
            public uint Null2;
            public uint Signature1;
            public uint SomeShit1;
            public uint SomeShit2;
            public uint SomeShit3;
            public uint Signature2;
            public uint VertHead;
            public Position3[] Vertex;
            public uint WeightHead;
            public Weight[] Weight;
            public uint UVHead;
            public Position3[] UV;
            public uint ShiteHead;
            public uint[] Shit;
            public uint EndSignature1;
            public uint EndSignature2;
            public byte[] leftovers;
        }
        public struct Position3
        {
            public float X, Y, Z;
        }
        public struct Weight
        {
            public float X, Y, Z;
            public byte SomeByte;
            public byte CONN;
            public ushort Null1;
        }
        public struct RawData
        {
            public float X, Y, Z;
            public float U, V, W;
            public bool CONN;
            public uint Diffuse;
            public float Nx, Ny, Nz;
        }
        #endregion
    }
}