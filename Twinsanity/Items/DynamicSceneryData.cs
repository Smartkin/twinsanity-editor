using System.Collections.Generic;
using System.IO;
using System;

namespace Twinsanity
{
    public sealed class DynamicSceneryData : TwinsItem
    {

        // Unfinished

        public uint Header1;
        public ushort ModelCount;
        public DynamicSceneryModel[] Models;

        public int DataSize;

        public DynamicSceneryData()
        {

        }

        /// <summary>
        /// Write converted binary data to file.
        /// </summary>
        public override void Save(BinaryWriter writer)
        {
            writer.Write(Data);
        }

        public override void Load(BinaryReader reader, int size)
        {
            long start_pos = reader.BaseStream.Position;

            Header1 = reader.ReadUInt32();
            ModelCount = reader.ReadUInt16();

            if (ModelCount != 0)
            {
                Models = new DynamicSceneryModel[ModelCount];
                for (int i = 0; i < ModelCount; i++)
                {
                    Models[i] = new DynamicSceneryModel();
                    char Name1 = Convert.ToChar(reader.ReadByte());
                    char Name2 = Convert.ToChar(reader.ReadByte());
                    char Name3 = Convert.ToChar(reader.ReadByte());
                    char Name4 = Convert.ToChar(reader.ReadByte());
                    string tempName = "" + Name1 + Name2 + Name3 + Name4;
                    Models[i].Name = tempName.Replace('\0', ' ');
                    Models[i].UnkOne = reader.ReadUInt32();

                    // Very similiar to the ones in GraphicsInfo and ChunkLink.Unknown - collision related?
                    Models[i].UnkVars = new ushort[13];
                    for (int a = 0; a < Models[i].UnkVars.Length; a++)
                    {
                        Models[i].UnkVars[a] = reader.ReadUInt16();
                    }

                    Models[i].UnkVectors = new Pos[Models[i].UnkVars[0]];
                    for (int a = 0; a < Models[i].UnkVectors.Length; a++)
                    {
                        Models[i].UnkVectors[a] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    }

                    Models[i].UnkVectors2 = new Pos[Models[i].UnkVars[2]];
                    for (int a = 0; a < Models[i].UnkVectors2.Length; a++)
                    {
                        Models[i].UnkVectors2[a] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    }

                    Models[i].UnkVectors3 = new Pos[Models[i].UnkVars[3] + Models[i].UnkVars[4]];
                    for (int a = 0; a < Models[i].UnkVectors3.Length; a++)
                    {
                        Models[i].UnkVectors3[a] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    }

                    Models[i].UnkVars2 = new ushort[3];
                    for (int a = 0; a < Models[i].UnkVars2.Length; a++)
                    {
                        // always 1280, 3850, 6420
                        Models[i].UnkVars2[a] = reader.ReadUInt16();
                    }

                    /*
                    reader.BaseStream.Position += 0x20;
                    bool check = false;

                    while (!check)
                    {
                        ushort test = reader.ReadByte();
                        if (test >= 0x20)
                        {
                            check = true;
                        }
                    }
                    reader.BaseStream.Position -= 1;
                    */

                    bool check = false;
                    ushort test, test2;

                    while (!check)
                    {
                        test = reader.ReadUInt16();
                        if (test == 0)
                        {
                            reader.BaseStream.Position += 4;
                            test = reader.ReadUInt16();
                            reader.BaseStream.Position -= 10;
                            test2 = reader.ReadUInt16();
                            if (test == test2)
                            {
                                check = true;
                                reader.BaseStream.Position += 6;
                            }
                            else
                            {
                                reader.BaseStream.Position += 1;
                            }
                        }
                        else
                        {
                            reader.BaseStream.Position -= 1;
                        }
                    }


                    //uint ArrayCount1 = reader.ReadUInt32();

                    //Models[i].UnkVar1 = reader.ReadUInt32();

                    ushort ArrayCount = reader.ReadUInt16(); // must be the same as ArrayCount1

                    reader.BaseStream.Position += 8;

                    Models[i].WorldPosition = new Pos(0, 0, 0, 1);
                    Models[i].LocalRotation = new float[3];

                    Models[i].WorldPosition.X = reader.ReadSingle();

                    float PosTest = reader.ReadSingle();
                    if (PosTest == 0f || PosTest == 1f)
                    {
                        /*
                        while (PosTest == 0f || PosTest == 1f)
                        {
                            PosTest = reader.ReadSingle();
                        }
                        */
                        reader.BaseStream.Position += 8;
                        PosTest = reader.ReadSingle();
                    }
                    Models[i].WorldPosition.Y = PosTest;

                    bool isElevator = false;
                    PosTest = reader.ReadSingle();
                    if (PosTest == 0f || PosTest == 1f)
                    {
                        isElevator = true;
                        Models[i].WorldPosition.Z = Models[i].WorldPosition.Y;
                        Models[i].WorldPosition.Y = PosTest;
                        /*
                        while (PosTest == 0f || PosTest == 1f)
                        {
                            PosTest = reader.ReadSingle();
                        }
                        */
                        reader.BaseStream.Position += 8;
                        //PosTest = reader.ReadSingle();
                    }
                    else
                    {
                        Models[i].WorldPosition.Z = PosTest;
                    }


                    Models[i].WorldPosition.W = reader.ReadSingle();
                    // this may also have a 3 float prefix


                    //Models[i].WorldPosition = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    Models[i].LocalRotation = new float[3] { reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle() };

                    if (isElevator)
                    {
                        Models[i].WorldPosition.Y = Models[i].LocalRotation[0];
                    }

                    reader.ReadByte();

                    check = false;
                    bool altcheck = false;
                    while (!check)
                    {
                        if (reader.ReadUInt32() == 0x0001)
                        {
                            check = true;
                        }
                        if (reader.BaseStream.Position >= start_pos + size)
                        {
                            check = true;
                            altcheck = true;
                        }
                    }
                    if (!altcheck)
                    {
                        reader.BaseStream.Position -= 44;
                    }
                    else
                    {
                        reader.BaseStream.Position = (start_pos + size) - 36;
                    }

                    Models[i].ModelID = reader.ReadUInt32();

                    Models[i].BoundingBoxVector1 = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    Models[i].BoundingBoxVector2 = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                }
            }

            reader.BaseStream.Position = start_pos;
            Data = reader.ReadBytes(size);
            DataSize = size;
        }

        public class DynamicSceneryModel
        {
            public string Name;
            public uint ModelID;
            public uint UnkOne;
            public ushort[] UnkVars; // 7
            public Pos[] UnkVectors;
            public Pos[] UnkVectors2;
            public Pos[] UnkVectors3;

            public uint UnkVar1;

            public byte UnkFlag;

            public byte[] UnkFlags;

            public ushort[] UnkVars2; //30

            public Pos WorldPosition;
            public float[] LocalRotation;

            public Pos BoundingBoxVector1;
            public Pos BoundingBoxVector2;
        }

    }
}
