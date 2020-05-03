using System;
using System.IO;

namespace Twinsanity
{
    public class GraphicsInfo : TwinsItem
    {

        public long ItemSize { get; set; }

        public GI_Type0[] ModelIDs { get; set; }
        public uint ArmatureModelID { get; set; }
        public uint ActorModelID { get; set; }
        public Pos Coord1 { get; set; } // Bounding box?
        public Pos Coord2 { get; set; } // Bounding box?
        public uint[] Variables { get; set; } //13
        public GI_Type1[] Type1 { get; set; }
        public GI_Type2[] Type2 { get; set; }
        public GI_Type3[] Type3 { get; set; }
        public byte[] Remain { get; set; }

        public ushort Var1;
        public byte Var2;
        public ushort Var3;
        public ushort Var4;
        public ushort Var5;
        public uint Var6;

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Data);

            /*
            writer.Write((byte)Type1.Length);
            writer.Write((byte)Type2.Length);
            writer.Write(Var1);
            writer.Write(Var2);
            writer.Write((byte)ModelIDs.Length);
            writer.Write(Var3);
            writer.Write(Var4);
            writer.Write(Var5);
            writer.Write(Var6);
            writer.Write(Coord1.X);
            writer.Write(Coord1.Y);
            writer.Write(Coord1.Z);
            writer.Write(Coord1.W);
            writer.Write(Coord2.X);
            writer.Write(Coord2.Y);
            writer.Write(Coord2.Z);
            writer.Write(Coord2.W);
            */
        }

        public override void Load(BinaryReader reader, int size)
        {
            long pre_pos = reader.BaseStream.Position;

            uint Type1_Size = reader.ReadByte();

            uint Type2_Size = reader.ReadByte();

            Var1 = reader.ReadUInt16();
            Var2 = reader.ReadByte();

            uint Model_Size = reader.ReadByte();

            Var3 = reader.ReadUInt16();
            Var4 = reader.ReadUInt16();
            Var5 = reader.ReadUInt16();
            Var6 = reader.ReadUInt32();

            Coord1 = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            Coord2 = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            
            if (Type1_Size > 0)
            {
                Type1 = new GI_Type1[Type1_Size];
                for (int i = 0; i < Type1_Size; i++)
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
                    GI_Type1 temp_Type1 = new GI_Type1() { Matrix = Type1_Matrix, Numbers = Type1_Numbers };
                    Type1[i] = temp_Type1;
                }
            }
            else
            {
                Type1 = new GI_Type1[0];
            }

            if (Type2_Size > 0)
            {
                Type2 = new GI_Type2[Type2_Size];
                for (int i = 0; i < Type2_Size; i++)
                {
                    Pos[] Type2_Matrix = new Pos[4];
                    uint[] Type2_Numbers = new uint[2];
                    Type2_Numbers[0] = reader.ReadUInt32();
                    Type2_Numbers[1] = reader.ReadUInt32();
                    for (int a = 0; a < Type2_Matrix.Length; a++)
                    {
                        Type2_Matrix[a] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    }
                    GI_Type2 temp_Type2 = new GI_Type2() { Matrix = Type2_Matrix, Numbers = Type2_Numbers };
                    Type2[i] = temp_Type2;
                }
            }
            else
            {
                Type2 = new GI_Type2[0];
            }

            if (Model_Size > 0)
            {
                ModelIDs = new GI_Type0[Model_Size];
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
                    GI_Type0 Type0 = new GI_Type0() { ID = IDs[i], ModelID = IDs_m[i] };
                    ModelIDs[i] = Type0;
                }

            }
            else
            {
                ModelIDs = new GI_Type0[0];
            }

            if (Type1_Size > 0)
            {
                Type3 = new GI_Type3[Type1_Size];
                for (int a = 0; a < Type1_Size; a++)
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

            ArmatureModelID = reader.ReadUInt32();

            ActorModelID = reader.ReadUInt32();

            Variables = new uint[13];
            for (int i = 0; i < Variables.Length; i++)
            {
                Variables[i] = reader.ReadUInt16();
            }

            long headersize = reader.BaseStream.Position;
            Remain = reader.ReadBytes(size - (int)(headersize - pre_pos));

            //ItemSize = reader.BaseStream.Position - pre_pos;
            //Console.WriteLine("Target Size: " + size);
            //Console.WriteLine("End Size: " + ItemSize);
            //Console.WriteLine("\n");

            ItemSize = size;
            reader.BaseStream.Position = pre_pos;
            Data = reader.ReadBytes(size);
        }

        protected override int GetSize()
        {
            return (int)ItemSize;
        }

        public struct GI_Type0
        {
            public uint ID;
            public uint ModelID;
        }

        public struct GI_Type1
        {
            public uint[] Numbers; // 5
            public Pos[] Matrix; // 5
        }

        public struct GI_Type2
        {
            public uint[] Numbers; // 2
            public Pos[] Matrix; // 4
        }

        public struct GI_Type3
        {
            public Pos[] Matrix; // 4
        }
    }
}
