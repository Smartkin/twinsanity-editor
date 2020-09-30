using System.Collections.Generic;
using System;
using System.IO;

namespace Twinsanity
{
    public sealed class ParticleData : TwinsItem
    {

        //Works for all versions AND Xbox's .PTL files, how nice. (may even work for TWOC with some adjustments)
        public bool IsStandalone = false;

        public long DataSize;

        public uint Header1;
        public uint ParticleTypeCount;
        public ParticleSystemDefinition[] ParticleTypes;
        public uint ParticleInstanceCount;
        public List<ParticleSystemInstance> ParticleInstances;
        public List<uint> PreHeader;
        public byte[] Remain;


        public ParticleData()
        {

        }

        protected override int GetSize()
        {
            if (Header1 == 0x616E6942)
            {
                return (int)DataSize;
            }
            int count = 12;
            count += PreHeader.Count * 4;
            count += Remain.Length;
            count += (int)ParticleInstanceCount * 0x44;
            if (ParticleTypeCount > 0)
            {
                for (int i = 0; i < ParticleTypes.Length; i++)
                {
                    count += 0x10;
                    count += ParticleTypes[i].Remain.Length;
                }
            }
            return count;
        }

        public override void Save(BinaryWriter writer)
        {
            if (Header1 == 0x616E6942)
            {
                writer.Write(Data);
                return;
            }

            if (PreHeader.Count > 0)
            {
                for (int i = 0; i < PreHeader.Count; i++)
                {
                    writer.Write(PreHeader[i]);
                }
            }

            writer.Write(Header1);
            writer.Write(ParticleTypeCount);

            if (ParticleTypeCount > 0)
            {
                for (int i = 0; i < ParticleTypeCount; i++)
                {
                    string tempName = ParticleTypes[i].Name.Replace(' ', '\0');
                    char[] tempName2 = tempName.ToCharArray();
                    writer.Write(tempName2);
                    writer.Write(ParticleTypes[i].Remain);
                }
            }

            writer.Write(ParticleInstanceCount);
            if (ParticleInstanceCount > 0 && ParticleInstanceCount < 65536)
            {
                for (int i = 0; i < ParticleInstanceCount; i++)
                {
                    writer.Write(ParticleInstances[i].Position.X);
                    writer.Write(ParticleInstances[i].Position.Y);
                    writer.Write(ParticleInstances[i].Position.Z);
                    writer.Write(ParticleInstances[i].Position.W);
                    writer.Write(ParticleInstances[i].Rot_X);
                    writer.Write(ParticleInstances[i].Rot_Y);
                    writer.Write(ParticleInstances[i].Rot_Z);
                    writer.Write(ParticleInstances[i].UnkZero);
                    string tempName = ParticleInstances[i].Name.Replace(' ', '\0');
                    char[] tempName2 = tempName.ToCharArray();
                    writer.Write(tempName2);
                    for (int a = 0; a < ParticleInstances[i].UnkShorts.Length; a++)
                    {
                        writer.Write(ParticleInstances[i].UnkShorts[a]);
                    }
                    writer.Write(ParticleInstances[i].EndZero);
                }
            }

            if (Remain.Length > 0)
            {
                writer.Write(Remain);
            }
        }

        public override void Load(BinaryReader reader, int size)
        {
            long start_pos = reader.BaseStream.Position;
            DataSize = size;

            Header1 = reader.ReadUInt32();

            // Some PTL files are "BinaryIntermediate" files
            if (Header1 == 0x616E6942)
            {
                ParticleInstanceCount = 0;
                ParticleTypeCount = 0;
                reader.BaseStream.Position = start_pos;
                Data = reader.ReadBytes(size);
                return;
            }

            PreHeader = new List<uint>();
            //Default.rm has some pre-header data
            if (Header1 > 0xFF)
            {
                PreHeader.Add(Header1);
                bool check = false;
                uint HeaderTest = 0;
                while (!check)
                {
                    HeaderTest = reader.ReadUInt32();
                    if (HeaderTest <= 0xFF)
                    {
                        check = true;
                        Header1 = HeaderTest;
                    }
                    else
                    {
                        PreHeader.Add(HeaderTest);
                    }
                }
            }

            ParticleTypeCount = reader.ReadUInt32();

            ParticleTypes = new ParticleSystemDefinition[ParticleTypeCount];

            // size 0x33C (0x330 if header is 0x1c)
            if (ParticleTypeCount > 0)
            {
                for (int i = 0; i < ParticleTypeCount; i++)
                {
                    ParticleTypes[i] = new ParticleSystemDefinition();
                    string tempName = new string(reader.ReadChars(0x10));
                    ParticleTypes[i].Name = tempName.Replace('\0', ' ');
                    int bufferSize = 0x320;
                    if (Header1 == 0x1E)
                    {
                        bufferSize += 0xC;
                    }
                    ParticleTypes[i].Remain = reader.ReadBytes(bufferSize);
                }
            }

            ParticleInstanceCount = reader.ReadUInt32();
            if (ParticleInstanceCount > 0 && ParticleInstanceCount < 65536)
            {
                ParticleInstances = new List<ParticleSystemInstance>();

                // size 0x44
                for (int i = 0; i < ParticleInstanceCount; i++)
                {
                    ParticleSystemInstance ParticleInstance = new ParticleSystemInstance();
                    ParticleInstance.Position = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    ParticleInstance.Rot_X = reader.ReadUInt16();
                    ParticleInstance.Rot_Y = reader.ReadUInt16();
                    ParticleInstance.Rot_Z = reader.ReadUInt16();
                    ParticleInstance.UnkZero = reader.ReadUInt32();
                    string tempName = new string(reader.ReadChars(0x10));
                    ParticleInstance.Name = tempName.Replace('\0', ' ');
                    ParticleInstance.UnkShorts = new ushort[12];
                    for (int a = 0; a < ParticleInstance.UnkShorts.Length; a++)
                    {
                        ParticleInstance.UnkShorts[a] = reader.ReadUInt16();
                    }
                    ParticleInstance.EndZero = reader.ReadUInt16();

                    ParticleInstances.Add(ParticleInstance);
                }
            }
            else
            {
                ParticleInstanceCount = 0;
            }

            // Default.rm has some extra data (water ripple/footstep stuff?)
            int RemainBytes = (int)((start_pos + size) - reader.BaseStream.Position);
            if (RemainBytes > 0)
            {
                Remain = reader.ReadBytes(RemainBytes);
            }
            else
            {
                Remain = new byte[0];
            }
        }

        public class ParticleSystemDefinition
        {
            public string Name;
            public byte[] Remain;
        }

        public class ParticleSystemInstance
        {
            public string Name;
            public Pos Position;
            public ushort Rot_X;
            public ushort Rot_Y;
            public ushort Rot_Z;
            public uint UnkZero;
            public ushort[] UnkShorts; //12
            public ushort EndZero;
        }

    }
}
