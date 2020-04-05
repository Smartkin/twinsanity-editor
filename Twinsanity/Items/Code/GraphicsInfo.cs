using System;
using System.IO;

namespace Twinsanity
{
    public class GraphicsInfo : TwinsItem
    {

        // Unfinished, doesn't work etc.

        public long ItemSize { get; set; }

        public GI_Type0[] ModelIDs { get; set; }
        public uint ArmatureModelID { get; set; }
        public uint ActorModelID { get; set; }
        public Pos Coord1 { get; set; }
        public Pos Coord2 { get; set; }
        public uint[] Variables { get; set; } //12
        public GI_Type1[] Type1 { get; set; }
        public GI_Type2[] Type2 { get; set; }
        public GI_Type3[] Type3 { get; set; }


        public override void Save(BinaryWriter writer)
        {
            writer.Write(ItemSize);
        }

        public override void Load(BinaryReader reader, int size)
        {
            Console.WriteLine("Intro Size: " + size);
            long pre_pos = reader.BaseStream.Position;

            uint Type1_Size = reader.ReadByte();
            Console.WriteLine("Type1 Size: " + Type1_Size);

            uint Type2_Size = reader.ReadByte();
            Console.WriteLine("Type2 Size: " + Type2_Size);

            // ??
            reader.ReadUInt16();
            reader.ReadByte();

            uint Model_Size = reader.ReadUInt16();
            Console.WriteLine("Model Size: " + Model_Size);


            // 10 bytes, with one number either 1 or 3

            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt16();


            Coord1 = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            Console.WriteLine("Coord1: " + Coord1.X + " " + Coord1.Y + " " + Coord1.Z + " " + Coord1.W);
            Coord2 = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            Console.WriteLine("Coord2: " + Coord2.X + " " + Coord2.Y + " " + Coord2.Z + " " + Coord2.W);
            
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
                    for (int a = 0; a < Type1_Matrix.Length; a++)
                    {
                        Type1_Matrix[a] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    }
                    GI_Type1 temp_Type1 = new GI_Type1() { Matrix = Type1_Matrix, Numbers = Type1_Numbers };
                    Type1[i] = temp_Type1;
                }
            }

            if (Model_Size > 0)
            {
                ModelIDs = new GI_Type0[Model_Size];
                for (int i = 0; i < Model_Size; i++)
                {
                    uint Type0_ID = reader.ReadUInt32();
                    uint Type0_ModelID = reader.ReadUInt32();
                    GI_Type0 Type0 = new GI_Type0() { ID = Type0_ID, ModelID = Type0_ModelID };
                    ModelIDs[i] = Type0;
                    Console.WriteLine("#" + i + " ID: " + Type0_ID);
                    Console.WriteLine("#" + i + " Model ID: " + Type0_ModelID);
                }
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

            ArmatureModelID = reader.ReadUInt32();
            Console.WriteLine("Armature Model ID: " + ArmatureModelID);
            ActorModelID = reader.ReadUInt32();
            Console.WriteLine("Actor Model ID: " + ActorModelID);

            Variables = new uint[12];
            for (int i = 0; i < Variables.Length; i++)
            {
                Variables[i] = reader.ReadUInt16();
                Console.WriteLine("Variable " + i + ": " + Variables[i]);
            }

            ItemSize = reader.BaseStream.Position - pre_pos;
            Console.WriteLine("End Size: " + ItemSize);

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
