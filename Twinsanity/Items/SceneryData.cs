using System.Collections.Generic;
using System.IO;
using System;

namespace Twinsanity
{
    public sealed class SceneryData : TwinsItem
    {

        //Unfinished after lights

        public ushort HeaderUnk1;
        public ushort HeaderVersion;
        public string ChunkName;
        public uint HeaderUnk2;
        public uint HeaderUnk3;
        public byte HeaderUnk4;
        public byte[] HeaderBuffer;
        public uint SkydomeID;
        public uint[] HeaderUnkVars;
        public List<byte> HeaderAfter;
        public List<SceneryStruct> sceneryModels;
        public List<UnkStruct1> unkStructs;
        public List<UnkStruct2> unkTables;

        public uint LightsNum;
        public uint LightAmbientNum;
        public uint LightDirectionalNum;
        public uint LightPointNum;
        public uint LightNegativeNum;
        public List<LightAmbient> LightsAmbient;
        public List<LightDirectional> LightsDirectional;
        public List<LightPoint> LightsPoint;
        public List<LightNegative> LightsNegative;

        public uint unkVar5;

        public byte[] Remain;
        public int DataSize;

        public SceneryData()
        {

        }

        protected override int GetSize()
        {
            int count = 2 + 2 + 4 + ChunkName.Length + 4 + 4 + 1 + 4;
            count += 4 * HeaderUnkVars.Length;
            count += HeaderBuffer.Length;
            count += 4 + 4 + 4 + 4 + 4;

            if (LightsAmbient.Count > 0)
            {
                count += LightsAmbient.Count * (4 + 4 + 4 + 4 + 4 + 4 + 16 + 32);
            }
            if (LightsDirectional.Count > 0)
            {
                count += LightsDirectional.Count * (4 + 4 + 4 + 4 + 4 + 4 + 16 + 48 + 2);
            }
            if (LightsPoint.Count > 0)
            {
                count += LightsPoint.Count * (4 + 4 + 4 + 4 + 4 + 4 + 16 + 32 + 2);
            }
            if (LightsNegative.Count > 0)
            {
                count += LightsNegative.Count * (4 + 4 + 4 + 4 + 4 + 4 + 16 + 48 + 20);
            }

            count += Remain.Length;
            return count;
        }

        public override void Save(BinaryWriter writer)
        {

            writer.Write(HeaderUnk1);
            writer.Write(HeaderVersion);
            writer.Write((uint)ChunkName.Length);
            writer.Write(ChunkName.ToCharArray());
            writer.Write(HeaderUnk2);
            writer.Write(HeaderUnk3);
            writer.Write(HeaderUnk4);
            writer.Write(SkydomeID);
            for (int i = 0; i < HeaderUnkVars.Length; i++)
            {
                writer.Write(HeaderUnkVars[i]);
            }
            writer.Write(HeaderBuffer);

            LightAmbientNum = (uint)LightsAmbient.Count;
            LightDirectionalNum = (uint)LightsDirectional.Count;
            LightPointNum = (uint)LightsPoint.Count;
            LightNegativeNum = (uint)LightsNegative.Count;
            LightsNum = LightAmbientNum + LightDirectionalNum + LightPointNum + LightNegativeNum;

            writer.Write(LightsNum);
            writer.Write(LightAmbientNum);
            writer.Write(LightDirectionalNum);
            writer.Write(LightPointNum);
            writer.Write(LightNegativeNum);

            if (LightAmbientNum > 0)
            {
                for (int i = 0; i < LightAmbientNum; i++)
                {
                    writer.Write(LightsAmbient[i].Flags);
                    writer.Write(LightsAmbient[i].Radius);
                    writer.Write(LightsAmbient[i].Color_R);
                    writer.Write(LightsAmbient[i].Color_G);
                    writer.Write(LightsAmbient[i].Color_B);
                    writer.Write(LightsAmbient[i].UnkFloat);
                    writer.Write(LightsAmbient[i].Position.X); writer.Write(LightsAmbient[i].Position.Y); writer.Write(LightsAmbient[i].Position.Z); writer.Write(LightsAmbient[i].Position.W);
                    for (int v = 0; v < LightsAmbient[i].Vectors.Length; v++)
                    {
                        writer.Write(LightsAmbient[i].Vectors[v].X);
                        writer.Write(LightsAmbient[i].Vectors[v].Y);
                        writer.Write(LightsAmbient[i].Vectors[v].Z);
                        writer.Write(LightsAmbient[i].Vectors[v].W);
                    }
                }
            }
            if (LightDirectionalNum > 0)
            {
                for (int i = 0; i < LightDirectionalNum; i++)
                {
                    writer.Write(LightsDirectional[i].Flags);
                    writer.Write(LightsDirectional[i].Radius);
                    writer.Write(LightsDirectional[i].Color_R);
                    writer.Write(LightsDirectional[i].Color_G);
                    writer.Write(LightsDirectional[i].Color_B);
                    writer.Write(LightsDirectional[i].UnkFloat);
                    writer.Write(LightsDirectional[i].Position.X); writer.Write(LightsDirectional[i].Position.Y); writer.Write(LightsDirectional[i].Position.Z); writer.Write(LightsDirectional[i].Position.W);
                    for (int v = 0; v < LightsDirectional[i].Vectors.Length; v++)
                    {
                        writer.Write(LightsDirectional[i].Vectors[v].X);
                        writer.Write(LightsDirectional[i].Vectors[v].Y);
                        writer.Write(LightsDirectional[i].Vectors[v].Z);
                        writer.Write(LightsDirectional[i].Vectors[v].W);
                    }
                    writer.Write(LightsDirectional[i].Flags2);
                }
            }
            if (LightPointNum > 0)
            {
                for (int i = 0; i < LightPointNum; i++)
                {
                    writer.Write(LightsPoint[i].Flags);
                    writer.Write(LightsPoint[i].Radius);
                    writer.Write(LightsPoint[i].Color_R);
                    writer.Write(LightsPoint[i].Color_G);
                    writer.Write(LightsPoint[i].Color_B);
                    writer.Write(LightsPoint[i].UnkFloat);
                    writer.Write(LightsPoint[i].Position.X); writer.Write(LightsPoint[i].Position.Y); writer.Write(LightsPoint[i].Position.Z); writer.Write(LightsPoint[i].Position.W);
                    for (int v = 0; v < LightsPoint[i].Vectors.Length; v++)
                    {
                        writer.Write(LightsPoint[i].Vectors[v].X);
                        writer.Write(LightsPoint[i].Vectors[v].Y);
                        writer.Write(LightsPoint[i].Vectors[v].Z);
                        writer.Write(LightsPoint[i].Vectors[v].W);
                    }
                    writer.Write(LightsPoint[i].Flags2);
                }
            }
            if (LightNegativeNum > 0)
            {
                for (int i = 0; i < LightNegativeNum; i++)
                {
                    writer.Write(LightsNegative[i].Flags);
                    writer.Write(LightsNegative[i].Radius);
                    writer.Write(LightsNegative[i].Color_R);
                    writer.Write(LightsNegative[i].Color_G);
                    writer.Write(LightsNegative[i].Color_B);
                    writer.Write(LightsNegative[i].UnkFloat);
                    writer.Write(LightsNegative[i].Position.X); writer.Write(LightsNegative[i].Position.Y); writer.Write(LightsNegative[i].Position.Z); writer.Write(LightsNegative[i].Position.W);
                    for (int v = 0; v < LightsNegative[i].Vectors.Length; v++)
                    {
                        writer.Write(LightsNegative[i].Vectors[v].X);
                        writer.Write(LightsNegative[i].Vectors[v].Y);
                        writer.Write(LightsNegative[i].Vectors[v].Z);
                        writer.Write(LightsNegative[i].Vectors[v].W);
                    }
                    for (int v = 0; v < LightsNegative[i].Floats.Length; v++)
                    {
                        writer.Write(LightsNegative[i].Floats[v]);
                    }
                }
            }

            writer.Write(Remain);

        }

        public override void Load(BinaryReader reader, int size)
        {
            long start_pos = reader.BaseStream.Position;

            HeaderUnk1 = reader.ReadUInt16();
            HeaderVersion = reader.ReadUInt16();
            uint chunkNameLength = reader.ReadUInt32();
            ChunkName = new string(reader.ReadChars((int)chunkNameLength));
            HeaderUnk2 = reader.ReadUInt32();
            HeaderUnk3 = reader.ReadUInt32();
            HeaderUnk4 = reader.ReadByte();
            SkydomeID = reader.ReadUInt32();

            HeaderUnkVars = new uint[6];
            for (int i = 0; i < HeaderUnkVars.Length; i++)
            {
                HeaderUnkVars[i] = reader.ReadUInt32();
            }

            //filled with 0xCD, but has numbers at the start sometimes (a list of some kind with a fixed limit?)
            if (HeaderVersion == 2)
            {
                HeaderBuffer = reader.ReadBytes(0x3E4);
            }
            else
            {
                HeaderBuffer = reader.ReadBytes(0x3E8);
            }

            LightsNum = reader.ReadUInt32();

            LightAmbientNum = reader.ReadUInt32();
            LightDirectionalNum = reader.ReadUInt32();
            LightPointNum = reader.ReadUInt32();
            LightNegativeNum = reader.ReadUInt32();

            LightsAmbient = new List<LightAmbient>();
            LightsDirectional = new List<LightDirectional>();
            LightsPoint = new List<LightPoint>();
            LightsNegative = new List<LightNegative>();

            if (LightAmbientNum > 0)
            {
                for (int i = 0; i < LightAmbientNum; i++)
                {
                    LightAmbient light = new LightAmbient();

                    light.Flags = reader.ReadBytes(4);
                    light.Radius = reader.ReadSingle();
                    light.Color_R = reader.ReadSingle();
                    light.Color_G = reader.ReadSingle();
                    light.Color_B = reader.ReadSingle();
                    light.UnkFloat = reader.ReadSingle();
                    light.Position = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    light.Vectors = new Pos[2];
                    light.Vectors[0] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    light.Vectors[1] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                    LightsAmbient.Add(light);
                }
            }
            if (LightDirectionalNum > 0)
            {
                for (int i = 0; i < LightDirectionalNum; i++)
                {
                    LightDirectional light = new LightDirectional();

                    light.Flags = reader.ReadBytes(4);
                    light.Radius = reader.ReadSingle();
                    light.Color_R = reader.ReadSingle();
                    light.Color_G = reader.ReadSingle();
                    light.Color_B = reader.ReadSingle();
                    light.UnkFloat = reader.ReadSingle();
                    light.Position = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    light.Vectors = new Pos[3];
                    light.Vectors[0] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    light.Vectors[1] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    light.Vectors[2] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    light.Flags2 = reader.ReadBytes(2);

                    LightsDirectional.Add(light);
                }
            }
            if (LightPointNum > 0)
            {
                for (int i = 0; i < LightPointNum; i++)
                {
                    LightPoint light = new LightPoint();

                    light.Flags = reader.ReadBytes(4);
                    light.Radius = reader.ReadSingle();
                    light.Color_R = reader.ReadSingle();
                    light.Color_G = reader.ReadSingle();
                    light.Color_B = reader.ReadSingle();
                    light.UnkFloat = reader.ReadSingle();
                    light.Position = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    light.Vectors = new Pos[2];
                    light.Vectors[0] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    light.Vectors[1] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    light.Flags2 = reader.ReadBytes(2);

                    LightsPoint.Add(light);
                }
            }
            if (LightNegativeNum > 0)
            {
                for (int i = 0; i < LightNegativeNum; i++)
                {
                    LightNegative light = new LightNegative();

                    light.Flags = reader.ReadBytes(4);
                    light.Radius = reader.ReadSingle();
                    light.Color_R = reader.ReadSingle();
                    light.Color_G = reader.ReadSingle();
                    light.Color_B = reader.ReadSingle();
                    light.UnkFloat = reader.ReadSingle();
                    light.Position = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    light.Vectors = new Pos[3];
                    light.Vectors[0] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    light.Vectors[1] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    light.Vectors[2] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    light.Floats = new float[5];
                    light.Floats[0] = reader.ReadSingle();
                    light.Floats[1] = reader.ReadSingle();
                    light.Floats[2] = reader.ReadSingle();
                    light.Floats[3] = reader.ReadSingle();
                    light.Floats[4] = reader.ReadSingle();

                    LightsNegative.Add(light);
                }
            }

            // Unfinished at this point
            long cur_pos = reader.BaseStream.Position;
            Remain = reader.ReadBytes((int)((start_pos + size) - cur_pos));
            reader.BaseStream.Position = cur_pos;

            unkVar5 = reader.ReadUInt32();


            uint AftHeader = 0;
            sceneryModels = new List<SceneryStruct>();
            unkStructs = new List<UnkStruct1>();
            unkTables = new List<UnkStruct2>();

            int cur_mod = 0;
            int off = 0;

            while (reader.BaseStream.Position < start_pos + size)
            {
                AftHeader = reader.ReadUInt32();
                if (reader.BaseStream.Position > start_pos + size)
                {
                    break;
                }
                if (AftHeader == 0x00001613)
                {
                    //Console.WriteLine("found model " + sceneryModels.Count);
                    cur_mod++;
                    off = 0;
                    SceneryStruct newStruct = new SceneryStruct();
                    ushort modelCount = reader.ReadUInt16();
                    ushort specialModelCount = reader.ReadUInt16();
                    newStruct.Header = AftHeader;
                    newStruct.Models = new List<SceneryModel>();

                    for (int i = 0; i < modelCount + specialModelCount; i++)
                    {
                        SceneryModel newModel = new SceneryModel();
                        newModel.ModelMatrix = new Pos[4];
                        newModel.ModelBoundingBoxVector1 = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                        newModel.ModelBoundingBoxVector2 = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                        newStruct.Models.Add(newModel);
                    }
                    for (int i = 0; i < modelCount + specialModelCount; i++)
                    {
                        if (i > modelCount - 1)
                        {
                            newStruct.Models[i].ModelID = reader.ReadUInt32();
                            newStruct.Models[i].isSpecial = true;
                        }
                        else
                        {
                            newStruct.Models[i].ModelID = reader.ReadUInt32();
                            newStruct.Models[i].isSpecial = false;
                        }
                    }
                    for (int i = 0; i < modelCount + specialModelCount; i++)
                    {
                        newStruct.Models[i].ModelMatrix[0] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                        newStruct.Models[i].ModelMatrix[1] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                        newStruct.Models[i].ModelMatrix[2] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                        newStruct.Models[i].ModelMatrix[3] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    }

                    newStruct.UnkStruct = new UnkStruct0();
                    newStruct.UnkStruct.UnkPos = new Pos[5];
                    newStruct.UnkStruct.UnkPos[0] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    newStruct.UnkStruct.UnkPos[1] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    newStruct.UnkStruct.UnkPos[2] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    newStruct.UnkStruct.UnkPos[3] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    newStruct.UnkStruct.UnkPos[4] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                    sceneryModels.Add(newStruct);
                }
                else
                {
                    off += 1;
                }
                /*
                else if (AftHeader == 0x00000003)
                {
                    Console.WriteLine("found struct " + unkStructs.Count);
                    cur_mod++;
                    off = 0;
                    UnkStruct1 newStruct = new UnkStruct1();
                    newStruct.Header = AftHeader;
                    newStruct.UnkPos = new Pos[4];
                    newStruct.UnkPos[0] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    newStruct.UnkPos[1] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    newStruct.UnkPos[2] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    newStruct.UnkPos[3] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    //newStruct.UnkPos[4] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                    unkStructs.Add(newStruct);
                }
                else
                {
                    Console.WriteLine("found table " + unkTables.Count);
                    cur_mod++;
                    off = 0;
                    UnkStruct2 newStruct = new UnkStruct2();
                    newStruct.Header = AftHeader;

                    newStruct.UnkInt = new uint[11];
                    newStruct.UnkInt[0] = reader.ReadUInt32();
                    newStruct.UnkInt[1] = reader.ReadUInt32();
                    newStruct.UnkInt[2] = reader.ReadUInt32();
                    newStruct.UnkInt[3] = reader.ReadUInt32();
                    newStruct.UnkInt[4] = reader.ReadUInt32();
                    newStruct.UnkInt[5] = reader.ReadUInt32();
                    newStruct.UnkInt[6] = reader.ReadUInt32();
                    newStruct.UnkInt[7] = reader.ReadUInt32();
                    newStruct.UnkInt[8] = reader.ReadUInt32();
                    newStruct.UnkInt[9] = reader.ReadUInt32();
                    newStruct.UnkInt[10] = reader.ReadUInt32();

                    unkTables.Add(newStruct);
                }
                */


                /*
                else
                {
                    off += 1;
                }
                */
            }

            //Console.WriteLine("end pos: " + (reader.BaseStream.Position - start_pos) + " target: " + size);
        }

        public class SceneryStruct
        {
            public uint Header;
            public List<SceneryModel> Models;
            public byte[] Remain; // The rest of the data per struct
            public UnkStruct0 UnkStruct;
        }

        public class UnkStructRoot
        {
            public uint[] Zeros; //3
            public uint UnkID;
            public Pos[] UnkPos; //4
            public uint Flag;
            public uint[] RemainZeros; //3
        }

        public class UnkStruct0
        {
            public Pos[] UnkPos; //4
        }
        public class UnkStruct1
        {
            public uint Header;
            public Pos[] UnkPos; //4
        }
        public class UnkStruct2
        {
            public uint Header;
            public uint[] UnkInt; //9
        }

        public class SceneryModel
        {
            public bool isSpecial;
            public uint ModelID;
            public Pos ModelBoundingBoxVector1;
            public Pos ModelBoundingBoxVector2;
            public Pos[] ModelMatrix; // 4
        }

        public class LightAmbient
        {
            public byte[] Flags; //4
            public float Radius;
            public float Color_R;
            public float Color_G;
            public float Color_B;
            public float UnkFloat;
            public Pos Position;
            public Pos[] Vectors; //2
        }

        public class LightDirectional
        {
            public byte[] Flags; //4
            public float Radius;
            public float Color_R;
            public float Color_G;
            public float Color_B;
            public float UnkFloat;
            public Pos Position;
            public Pos[] Vectors; //3

            public byte[] Flags2; //2
        }

        public class LightPoint
        {
            public byte[] Flags; //4
            public float Radius;
            public float Color_R;
            public float Color_G;
            public float Color_B;
            public float UnkFloat;
            public Pos Position;
            public Pos[] Vectors; //2

            public byte[] Flags2; //2
        }

        public class LightNegative
        {
            public byte[] Flags; //4
            public float Radius;
            public float Color_R;
            public float Color_G;
            public float Color_B;
            public float UnkFloat;
            public Pos Position;
            public Pos[] Vectors; //3

            public float[] Floats; //5
        }

    }

}
