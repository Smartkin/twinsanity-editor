using System.Collections.Generic;
using System;
using System.IO;

namespace Twinsanity
{
    public sealed class ParticleData : TwinsItem
    {
        public bool IsStandalone = false;
        public bool IsDefault = false;

        public long DataSize;

        public uint Version;
        public List<ParticleSystemDefinition> ParticleTypes = new List<ParticleSystemDefinition>();
        public List<ParticleSystemInstance> ParticleInstances = new List<ParticleSystemInstance>();

        public uint ParticleTextureID_1;
        public uint ParticleMaterialID_1;
        public uint ParticleTextureID_2;
        public uint ParticleMaterialID_2;
        public uint ParticleTextureID_3;
        public uint ParticleMaterialID_3;
        public byte[] Remain = new byte[0];
        public uint DecalTextureID;
        public uint DecalMaterialID;

        public uint UnkTextureID_1;
        public uint UnkMaterialID_1;
        public uint UnkTextureID_2;
        public uint UnkMaterialID_2;
        public uint UnkTextureID_3;
        public uint UnkMaterialID_3;

        public ParticleData()
        {

        }

        protected override int GetSize()
        {
            if (Version == 0x616E6942) return (int)DataSize;
            int count = 12;
            if (IsDefault) count += 28;
            count += Remain.Length;

            int instSize = 36;
            if (Version >= 0x16) instSize += 2;
            if (Version >= 0x08) instSize += 4;
            if (Version >= 0x09) instSize += 12;
            if (Version >= 0x0C) instSize += 8;
            if (Version >= 0x0D) instSize += 4;
            if (Version >= 0x0F) instSize += 2;
            count += (int)ParticleInstances.Count * instSize;

            int defSize = 690; 
            if (Version == 0x20) defSize += 76;
            if (Version >= 0x06) defSize += 8;
            if (Version >= 0x0A) defSize += 4;
            if (Version > 0x16 && Version != 0x20) defSize += 4;
            if (Version >= 0x18 && Version != 0x20) defSize += 4;
            if (Version < 0x07) defSize += 8;
            if (Version < 0x12) defSize += 24;
            if (Version > 0x15) defSize += 8;
            if (Version >= 0x03) defSize += 65;
            if (Version >= 0x11) defSize += 1;
            if (Version >= 0x10) defSize += 8;
            if (Version >= 0x19 && Version != 0x20) defSize += 8;
            if (Version >= 0x1A && Version != 0x20) defSize += 4;
            if (Version > 0x1A && Version != 0x20) defSize += 4;
            if (Version > 0x1B && Version != 0x20) defSize += 4;
            if (Version >= 0x1E) defSize += 16;
            count += (int)ParticleTypes.Count * defSize;
            if (Version > 0x16 && Version < 0x1D && Version != 0x20)
            {
                count += (int)ParticleTypes.Count * 4;
                for (int i = 0; i < ParticleTypes.Count; i++)
                {
                    count += ParticleTypes[i].padAmount * 24;
                }
            }

            for (int i = 0; i < ParticleTypes.Count; i++)
            {
                count += ParticleTypes[i].Remain.Length;
            }
            return count;
        }

        public override void Save(BinaryWriter writer)
        {
            if (Version == 0x616E6942)
            {
                writer.Write(Data);
                return;
            }

            if (IsDefault)
            {
                writer.Write(ParticleTextureID_1);
                writer.Write(ParticleMaterialID_1);
                writer.Write(ParticleTextureID_2);
                writer.Write(ParticleMaterialID_2);
                writer.Write(ParticleTextureID_3);
                writer.Write(ParticleMaterialID_3);
                if (isMonkeyBall)
                {
                    writer.Write(UnkMaterialID_1);
                    writer.Write(UnkTextureID_1);
                    writer.Write(UnkMaterialID_2);
                    writer.Write(UnkTextureID_2);
                    writer.Write(UnkMaterialID_3);
                    writer.Write(UnkTextureID_3);
                }
            }

            writer.Write(Version);
            writer.Write(ParticleTypes.Count);

            if (ParticleTypes.Count > 0)
            {
                for (int a = 0; a < ParticleTypes.Count; a++)
                {
                    ParticleSystemDefinition PS = ParticleTypes[a];
                    string tempName = PS.Name + "";
                    if (tempName.Length != 0x10)
                    {
                        if (tempName.Length > 0x10)
                        {
                            tempName = tempName.Substring(0, 0x10);
                        }
                        else
                        {
                            while (tempName.Length < 0x10)
                            {
                                tempName += '\0';
                            }
                        }
                    }
                    writer.Write(tempName.ToCharArray());
                    if (Version == 0x20)
                    {
                        writer.Write((Byte)0);
                        writer.Write(PS.UnkByte1);
                    }
                    writer.Write(PS.GenRate);
                    writer.Write(PS.MaxParticleCount);
                    writer.Write(PS.UnkUShort3);
                    writer.Write(PS.Emitter_OverTime);
                    writer.Write(PS.Emitter_OverTimeRandom);
                    writer.Write(PS.Emitter_OffTime);
                    writer.Write(PS.Emitter_OffTimeRandom);
                    writer.Write((byte)PS.GSort);
                    writer.Write(PS.UnkByte3);
                    writer.Write((byte)PS.TextureFilter);
                    writer.Write(PS.UnkByte5);
                    writer.Write(PS.UnkFloat1);
                    if (Version >= 0x6)
                    {
                        writer.Write(PS.CutOnRadius);
                        writer.Write(PS.CutOffRadius);
                    }
                    if (Version >= 0xA)
                    {
                        writer.Write(PS.DrawCutOff);
                    }
                    if (!(Version <= 0x16 || Version == 0x20))
                    {
                        writer.Write(PS.UnkFloat5);
                    }
                    if (!(Version < 0x18 || Version == 0x20))
                    {
                        writer.Write(PS.UnkFloat6);
                    }
                    if (Version < 0x7)
                    {
                        writer.Write(0);
                        writer.Write(0);
                    }
                    writer.Write(PS.Velocity);
                    writer.Write(PS.Random_Emit_X);
                    writer.Write(PS.Random_Emit_Y);
                    writer.Write(PS.Random_Emit_Z);
                    if (Version < 0x12)
                    {
                        writer.Write(0);
                        writer.Write(0);
                        writer.Write(0);
                    }
                    writer.Write(PS.Random_Start_X);
                    writer.Write(PS.Random_Start_Y);
                    writer.Write(PS.Random_Start_Z);
                    if (Version < 0x12)
                    {
                        writer.Write(0);
                        writer.Write(0);
                        writer.Write(0);
                    }
                    writer.Write(PS.UnkFloat8);
                    writer.Write(PS.UnkFloat9);
                    writer.Write(PS.UnkFloat10);
                    writer.Write(PS.UnkFloat11);
                    writer.Write(PS.UnkFloat12);
                    writer.Write(PS.UnkFloat13);
                    writer.Write(PS.UnkFloat14);
                    writer.Write(PS.UnkFloat15);
                    writer.Write(PS.UnkFloat16);
                    writer.Write(PS.UnkFloat17);
                    writer.Write(PS.UnkFloat18);
                    writer.Write(PS.UnkFloat19);
                    writer.Write(PS.Gravity);
                    writer.Write(PS.ParticleLifeTime);
                    writer.Write(PS.UnkUShort8);
                    writer.Write(PS.UnkByte6);
                    writer.Write(PS.UnkByte7);
                    writer.Write(PS.UnkFloat22);
                    writer.Write(PS.JibberXFreq);
                    writer.Write(PS.JibberXAmp);
                    writer.Write(PS.JibberYFreq);
                    writer.Write(PS.JibberYAmp);
                    for (var i = 0; i < 8; ++i)
                    {
                        writer.Write(PS.UnkVecs[i].X);
                        writer.Write(PS.UnkVecs[i].Y);
                        writer.Write(PS.UnkVecs[i].Z);
                        writer.Write(PS.UnkVecs[i].W);
                    }
                    for (var i = 0; i < 8; ++i)
                    {
                        writer.Write(PS.UnkLongs1[i]);
                    }
                    if (Version >= 0x15)
                    {
                        writer.Write(PS.UnkFloat27);
                        writer.Write(PS.UnkFloat28);
                    }
                    writer.Write(PS.UnkFloat29);
                    writer.Write(PS.UnkFloat30);
                    for (var i = 0; i < 8; ++i)
                    {
                        writer.Write(PS.UnkLongs2[i]);
                    }
                    for (var i = 0; i < 8; ++i)
                    {
                        writer.Write(PS.UnkLongs3[i]);
                    }
                    writer.Write(PS.UnkFloat31);
                    writer.Write(PS.UnkFloat32);
                    for (var i = 0; i < 8; ++i)
                    {
                        writer.Write(PS.UnkLongs4[i]);
                    }
                    for (var i = 0; i < 8; ++i)
                    {
                        writer.Write(PS.UnkLongs5[i]);
                    }
                    for (var i = 0; i < 8; ++i)
                    {
                        writer.Write(PS.UnkLongs6[i]);
                    }
                    writer.Write(PS.UnkFloat33);
                    writer.Write(PS.UnkFloat34);
                    writer.Write(PS.UnkFloat35);
                    writer.Write(PS.UnkFloat36);
                    if (Version == 0x20)
                    {
                        writer.Write(0);
                    }
                    if (Version >= 0x3)
                    {
                        for (var i = 0; i < 8; ++i)
                        {
                            writer.Write(PS.UnkLongs7[i]);
                        }
                        writer.Write(PS.UnkByte8);
                    }
                    if (Version >= 0x11)
                    {
                        writer.Write(PS.UnkByte9);
                    }
                    if (Version == 0x20)
                    {
                        writer.Write((Byte)0);
                        writer.Write((Byte)0);
                        writer.Write(0);
                    }
                    if (Version > 0x16 && Version != 0x20)
                    {
                        if (Version < 0x1D)
                        {
                            writer.Write(PS.padAmount);
                            for (var i = 0; i < PS.padAmount * 6; ++i)
                            {
                                writer.Write(0);
                            }
                        }
                    }
                    else
                    {
                        if (Version == 0x20)
                        {
                            writer.Write(PS.UnkFloat37);
                            for (var i = 0; i < 11; ++i)
                            {
                                writer.Write(0);
                            }
                        }
                    }
                    if (Version >= 0x10)
                    {
                        if (Version == 0x20)
                        {
                            writer.Write(PS.UnkShorts[1]);
                            writer.Write((Byte)0);
                            writer.Write((Byte)0);
                        }
                        else
                        {
                            writer.Write((Int32)PS.UnkShorts[1]);
                        }
                        writer.Write(PS.UnkFloat38);
                    }
                    if (Version >= 0x19 && Version != 0x20)
                    {
                        writer.Write((Int32)PS.UnkShorts[2]);
                        writer.Write(PS.UnkFloat39);
                    }
                    if (Version >= 0x1A && Version != 0x20)
                    {
                        writer.Write(PS.UnkFloat40);
                    }
                    if (Version != 0x20)
                    {
                        if (Version > 0x1A)
                        {
                            writer.Write(PS.TexturePage);
                        }
                        if (Version > 0x1B)
                        {
                            writer.Write(PS.UnkFloat37);
                        }
                    }
                    if (Version >= 0x1E)
                    {
                        writer.Write(PS.UnkVec3.X);
                        writer.Write(PS.UnkVec3.Y);
                        writer.Write(PS.UnkVec3.Z);
                        writer.Write(PS.UnkVec3.W);
                    }
                }
            }

            if (!IsDefault)
            {
                writer.Write(ParticleInstances.Count);
            }
            if (ParticleInstances.Count > 0 && ParticleInstances.Count < 65536)
            {
                for (int i = 0; i < ParticleInstances.Count; i++)
                {
                    writer.Write(ParticleInstances[i].Position.X);
                    writer.Write(ParticleInstances[i].Position.Y);
                    writer.Write(ParticleInstances[i].Position.Z);
                    if (Version >= 0x7)
                    {
                        writer.Write(ParticleInstances[i].GravityRotX);
                        writer.Write(ParticleInstances[i].GravityRotY);
                        writer.Write(ParticleInstances[i].EmitRotX);
                        writer.Write(ParticleInstances[i].EmitRotY);
                    }
                    else
                    {
                        writer.Write((Int32)ParticleInstances[i].EmitRotX);
                        writer.Write((Int32)ParticleInstances[i].EmitRotY);
                    }
                    if (Version >= 0x16)
                    {
                        writer.Write(ParticleInstances[i].UnkShort5);
                    }
                    if (Version >= 0x8)
                    {
                        writer.Write(ParticleInstances[i].Offset);
                    }
                    string tempName = ParticleInstances[i].Name + "";
                    if (tempName.Length != 0x10)
                    {
                        if (tempName.Length > 0x10)
                        {
                            tempName = tempName.Substring(0, 0x10);
                        }
                        else
                        {
                            while (tempName.Length < 0x10)
                            {
                                tempName += '\0';
                            }
                        }
                    }
                    writer.Write(tempName.ToCharArray());
                    if (Version >= 0x9)
                    {
                        writer.Write(ParticleInstances[i].SwitchType);
                        writer.Write(ParticleInstances[i].SwitchID);
                        writer.Write(ParticleInstances[i].SwitchValue);
                    }
                    if (Version >= 0xC)
                    {
                        writer.Write(ParticleInstances[i].UnkShort6);
                        writer.Write(ParticleInstances[i].UnkShort7);
                        writer.Write(ParticleInstances[i].PlaneOffset);
                    }
                    if (Version >= 0xD)
                    {
                        writer.Write(ParticleInstances[i].BounceFactor);
                    }
                    if (Version >= 0xF)
                    {
                        writer.Write(ParticleInstances[i].GroupID);
                    }

                }
            }

            if (IsDefault)
            {
                writer.Write(DecalTextureID);
                writer.Write(DecalMaterialID);
            }

            if (Remain.Length > 0)
            {
                writer.Write(Remain);
            }
        }

        public bool isMonkeyBall = false;
        public void Load(BinaryReader reader, int size, bool isMB)
        {
            isMonkeyBall = isMB;
            Load(reader, size);
        }

        public override void Load(BinaryReader reader, int size)
        {
            long start_pos = reader.BaseStream.Position;
            DataSize = size;

            ParticleTypes = new List<ParticleSystemDefinition>();
            ParticleInstances = new List<ParticleSystemInstance>();

            Version = reader.ReadUInt32();

            // Some PTL files are "BinaryIntermediate" files
            if (Version == 0x616E6942)
            {
                reader.BaseStream.Position = start_pos;
                Data = reader.ReadBytes(size);
                return;
            }

            //Default.rm2 has some pre-header data: 3x (texture ID + material ID)
            if (Version > 0xFF)
            {
                IsDefault = true;
                ParticleTextureID_1 = Version;
                ParticleMaterialID_1 = reader.ReadUInt32();
                ParticleTextureID_2 = reader.ReadUInt32();
                ParticleMaterialID_2 = reader.ReadUInt32();
                ParticleTextureID_3 = reader.ReadUInt32();
                ParticleMaterialID_3 = reader.ReadUInt32();
                Version = reader.ReadUInt32();

                if (isMonkeyBall)
                {
                    UnkMaterialID_1 = Version;
                    UnkTextureID_1 = reader.ReadUInt32();
                    UnkMaterialID_2 = reader.ReadUInt32();
                    UnkTextureID_2 = reader.ReadUInt32();
                    UnkMaterialID_3 = reader.ReadUInt32();
                    UnkTextureID_3 = reader.ReadUInt32();
                    Version = reader.ReadUInt32();
                }
            }

            uint ParticleTypeCount = reader.ReadUInt32();

            if (ParticleTypeCount != 0)
            {
                for (int i = 0; i < ParticleTypeCount; i++)
                {
                    ParticleSystemDefinition PS = new ParticleSystemDefinition();
                    string tempName = string.Empty;
                    while (tempName.Length < 0x10)
                    {
                        char namechar = reader.ReadChar();
                        if (namechar == '\0')
                        {
                            reader.ReadBytes(0x0F - tempName.Length);
                            break;
                        }
                        tempName += namechar;
                    }
                    PS.Name = tempName;
                    if (Version == 0x20)
                    {
                        reader.ReadByte();
                        PS.UnkByte1 = reader.ReadByte();
                    }

                    PS.GenRate = reader.ReadInt16();
                    PS.MaxParticleCount = reader.ReadUInt16();
                    PS.UnkUShort3 = reader.ReadUInt16();
                    PS.Emitter_OverTime = reader.ReadUInt16();
                    PS.Emitter_OverTimeRandom = reader.ReadUInt16();
                    PS.Emitter_OffTime = reader.ReadUInt16();
                    PS.Emitter_OffTimeRandom = reader.ReadUInt16();
                    PS.GSort = (ParticleSystemDefinition.GenSort)reader.ReadByte();
                    PS.UnkByte3 = reader.ReadByte();
                    PS.TextureFilter = (ParticleSystemDefinition.TextureFiltering)reader.ReadByte();
                    PS.UnkByte5 = reader.ReadByte();
                    PS.UnkFloat1 = reader.ReadSingle();
                    if (Version == 0x20) PS.UnkFloat1 = 25f;
                    PS.CutOnRadius = 0f;
                    PS.CutOffRadius = 25f;
                    if (Version >= 0x6)
                    {
                        PS.CutOnRadius = reader.ReadSingle();
                        PS.CutOffRadius = reader.ReadSingle();
                    }
                    PS.DrawCutOff = 0f;
                    if (Version >= 0xA)
                    {
                        PS.DrawCutOff = reader.ReadSingle();
                        if (PS.DrawCutOff <= 2f)
                        {
                            PS.DrawCutOff = 999999.875f;
                        }
                    }
                    if (Version <= 0x16 || Version == 0x20)
                    {
                        PS.UnkFloat5 = 0f;
                    }
                    else
                    {
                        PS.UnkFloat5 = reader.ReadSingle();
                    }
                    if (Version < 0x18 || Version == 0x20)
                    {
                        PS.UnkFloat6 = 0.5f;
                    }
                    else
                    {
                        PS.UnkFloat6 = reader.ReadSingle();
                    }
                    if (Version < 0x7)
                    {
                        reader.ReadBytes(8);
                    }
                    PS.Velocity = reader.ReadSingle();
                    PS.Random_Emit_X = reader.ReadSingle();
                    PS.Random_Emit_Y = reader.ReadSingle();
                    PS.Random_Emit_Z = reader.ReadSingle();
                    if (Version < 0x12)
                    {
                        reader.ReadBytes(0xC);
                    }
                    PS.Random_Start_X = reader.ReadSingle();
                    PS.Random_Start_Y = reader.ReadSingle();
                    PS.Random_Start_Z = reader.ReadSingle();
                    if (Version < 0x12)
                    {
                        reader.ReadBytes(0xC);
                    }
                    PS.UnkFloat8 = reader.ReadSingle();
                    PS.UnkFloat9 = reader.ReadSingle();
                    PS.UnkFloat10 = reader.ReadSingle();
                    PS.UnkFloat11 = reader.ReadSingle();
                    PS.UnkFloat12 = reader.ReadSingle();
                    PS.UnkFloat13 = reader.ReadSingle();
                    PS.UnkFloat14 = reader.ReadSingle();
                    PS.UnkFloat15 = reader.ReadSingle();
                    PS.UnkFloat16 = reader.ReadSingle();
                    PS.UnkFloat17 = reader.ReadSingle();
                    PS.UnkFloat18 = reader.ReadSingle();
                    PS.UnkFloat19 = reader.ReadSingle();
                    PS.Gravity = reader.ReadSingle();
                    PS.ParticleLifeTime = reader.ReadSingle();
                    PS.UnkUShort8 = reader.ReadUInt16();
                    PS.UnkByte6 = reader.ReadByte();
                    PS.UnkByte7 = reader.ReadByte();
                    PS.UnkFloat22 = reader.ReadSingle();
                    PS.JibberXFreq = reader.ReadSingle();
                    PS.JibberXAmp = reader.ReadSingle();
                    PS.JibberYFreq = reader.ReadSingle();
                    PS.JibberYAmp = reader.ReadSingle();
                    for (int a = 0; a < 8; a++)
                    {
                        PS.UnkVecs[a] = new TwinsVector4();
                        PS.UnkVecs[a].X = reader.ReadSingle();
                        PS.UnkVecs[a].Y = reader.ReadSingle();
                        PS.UnkVecs[a].Z = reader.ReadSingle();
                        PS.UnkVecs[a].W = reader.ReadSingle();
                    }
                    for (int a = 0; a < 8; a++)
                    {
                        PS.UnkLongs1[a] = reader.ReadInt64();
                    }
                    PS.UnkFloat27 = 0.125f;
                    PS.UnkFloat28 = 0.125f;
                    if (Version > 0x15)
                    {
                        PS.UnkFloat27 = reader.ReadSingle();
                        PS.UnkFloat28 = reader.ReadSingle();
                    }
                    PS.UnkFloat29 = reader.ReadSingle();
                    PS.UnkFloat30 = reader.ReadSingle();
                    for (var a = 0; a < 8; ++a)
                    {
                        PS.UnkLongs2[a] = reader.ReadInt64();
                    }
                    for (var a = 0; a < 8; ++a)
                    {
                        PS.UnkLongs3[a] = reader.ReadInt64();
                    }
                    PS.UnkFloat31 = reader.ReadSingle();
                    PS.UnkFloat32 = reader.ReadSingle();
                    for (var a = 0; a < 8; ++a)
                    {
                        PS.UnkLongs4[a] = reader.ReadInt64();
                    }
                    for (var a = 0; a < 8; ++a)
                    {
                        PS.UnkLongs5[a] = reader.ReadInt64();
                    }
                    for (var a = 0; a < 8; ++a)
                    {
                        PS.UnkLongs6[a] = reader.ReadInt64();
                    }
                    PS.UnkFloat33 = reader.ReadSingle();
                    PS.UnkFloat34 = reader.ReadSingle();
                    PS.UnkFloat35 = reader.ReadSingle();
                    PS.UnkFloat36 = reader.ReadSingle();
                    if (Version == 0x20)
                    {
                        reader.ReadBytes(4);
                    }
                    if (Version >= 0x3)
                    {
                        for (var a = 0; a < 8; ++a)
                        {
                            PS.UnkLongs7[a] = reader.ReadInt64();
                        }
                        PS.UnkByte8 = reader.ReadByte();
                    }
                    if (Version >= 0x11)
                    {
                        PS.UnkByte9 = reader.ReadByte();
                    }
                    if (PS.TextureFilter == ParticleSystemDefinition.TextureFiltering.Glass)
                    {
                        PS.UnkByte9 = 2;
                    }
                    if (Version == 0x20)
                    {
                        reader.ReadBytes(6);
                    }
                    if (Version > 0x16 && Version != 0x20)
                    {
                        if (Version < 0x1D)
                        {
                            PS.padAmount = reader.ReadInt32();
                            reader.ReadBytes(PS.padAmount * 24);
                        }
                    }
                    else
                    {
                        if (Version == 0x20)
                        {
                            PS.UnkFloat37 = reader.ReadSingle();
                            reader.ReadBytes(44);
                        }
                    }
                    PS.UnkShorts[1] = 0;
                    PS.UnkFloat38 = 0;
                    if (Version >= 0x10)
                    {
                        if (Version == 0x20)
                        {
                            PS.UnkShorts[1] = reader.ReadInt16();
                            reader.ReadBytes(2);
                        }
                        else
                        {
                            PS.UnkShorts[1] = (Int16)reader.ReadInt32();
                        }
                        PS.UnkFloat38 = reader.ReadSingle();
                    }
                    PS.UnkShorts[2] = 5;
                    PS.UnkFloat39 = 0.5f;
                    if (Version >= 0x19 && Version != 0x20)
                    {
                        PS.UnkShorts[2] = (Int16)reader.ReadInt32();
                        PS.UnkFloat39 = reader.ReadSingle();
                    }
                    PS.UnkFloat40 = 0;
                    if (Version >= 0x1A && Version != 0x20)
                    {
                        PS.UnkFloat40 = reader.ReadSingle();
                    }
                    if (Version != 0x20)
                    {
                        if (Version > 0x1A)
                        {
                            PS.TexturePage = reader.ReadInt32();
                        }
                        if (Version > 0x1B)
                        {
                            PS.UnkFloat37 = reader.ReadSingle();
                        }
                    }
                    if (Version >= 0x1E)
                    {
                        PS.UnkVec3.X = reader.ReadSingle();
                        PS.UnkVec3.Y = reader.ReadSingle();
                        PS.UnkVec3.Z = reader.ReadSingle();
                        PS.UnkVec3.W = reader.ReadSingle();
                    }
                    else
                    {
                        PS.UnkVec3.X = 10f;
                        PS.UnkVec3.Y = 10f;
                        PS.UnkVec3.Z = 10f;
                        PS.UnkVec3.W = 0f;
                        if (PS.GSort == ParticleSystemDefinition.GenSort.Normal)
                        {
                            var f1 = PS.UnkFloat30 * 0.0001f;
                            PS.UnkVec3.X = ((PS.Velocity + PS.Random_Emit_X) * PS.ParticleLifeTime + PS.Random_Start_X + f1) * 0.75f;
                            PS.UnkVec3.Y = ((PS.Velocity + PS.Random_Emit_Y) * PS.ParticleLifeTime + PS.Random_Start_Y + f1) * 0.75f;
                            PS.UnkVec3.Z = ((PS.Velocity + PS.Random_Emit_Z) * PS.ParticleLifeTime + PS.Random_Start_Z + f1) * 0.75f;
                        }
                    }


                    ParticleTypes.Add(PS);
                }
            }

            if (reader.BaseStream.Position == start_pos + DataSize)
            {
                Remain = new byte[0];
                return;
            }

            uint InstanceCheck = reader.ReadUInt32();
            uint ParticleInstanceCount = InstanceCheck;
            if (!IsDefault && ParticleInstanceCount != 0 && ParticleInstanceCount < 65536 && Version != 0x20)
            {
                ParticleInstances = new List<ParticleSystemInstance>();

                for (int i = 0; i < ParticleInstanceCount; i++)
                {
                    ParticleSystemInstance PI = new ParticleSystemInstance();
                    PI.Position = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), 1f);
                    if (Version >= 0x7)
                    {
                        PI.GravityRotX = reader.ReadInt16();
                        PI.GravityRotY = reader.ReadInt16();
                        PI.EmitRotX = reader.ReadInt16();
                        PI.EmitRotY = reader.ReadInt16();
                    }
                    else
                    {
                        PI.GravityRotX = 0;
                        PI.GravityRotY = 0;
                        PI.EmitRotX = (short)reader.ReadInt32();
                        PI.EmitRotY = (short)reader.ReadInt32();
                    }
                    if (Version >= 0x16)
                    {
                        PI.UnkShort5 = reader.ReadInt16();
                    }
                    if (Version >= 0x08)
                    {
                        PI.Offset = reader.ReadUInt32();
                    }
                    string tempName = string.Empty;
                    while (tempName.Length < 0x10)
                    {
                        char namechar = reader.ReadChar();
                        if (namechar == '\0')
                        {
                            reader.ReadBytes(0x0F - tempName.Length);
                            break;
                        }
                        tempName += namechar;
                    }
                    PI.Name = tempName;
                    if (Version >= 0x9)
                    {
                        PI.SwitchType = reader.ReadInt32();
                        PI.SwitchID = reader.ReadInt32();
                        PI.SwitchValue = reader.ReadSingle();
                    }
                    if (Version >= 0xC)
                    {
                        PI.UnkShort6 = reader.ReadInt16();
                        PI.UnkShort7 = reader.ReadInt16();
                        PI.PlaneOffset = reader.ReadSingle();
                    }
                    PI.BounceFactor = 0.89999998f;
                    if (Version >= 0xD)
                    {
                        PI.BounceFactor = reader.ReadSingle();
                    }
                    if (Version >= 0xF)
                    {
                        PI.GroupID = reader.ReadInt16();
                    }

                    ParticleInstances.Add(PI);
                }
            }
            else
            {
                ParticleInstanceCount = 0;
            }

            // Default.rm has some extra data (decal stuff)
            if (IsDefault)
            {
                DecalTextureID = InstanceCheck;
                DecalMaterialID = reader.ReadUInt32();
            }

            // todo: more data after this in default (more decal stuff?)

            int RemainBytes = (int)((start_pos + size) - reader.BaseStream.Position);
            if (RemainBytes > 0)
            {
                Remain = reader.ReadBytes(RemainBytes);
            }
            else if (RemainBytes < 0)
            {
                throw new Exception("Invalid particle parsing");
            }
            else
            {
                Remain = new byte[0];
            }
        }

        public class ParticleSystemDefinition
        {
            public string Name;
            public byte[] Remain = new byte[0];

            public byte UnkByte1;
            public short GenRate; // -10 - 10
            public ushort MaxParticleCount;
            public UInt16 UnkUShort3; // always 0?
            public ushort Emitter_OverTime;
            public ushort Emitter_OverTimeRandom;
            public ushort Emitter_OffTime;
            public ushort Emitter_OffTimeRandom;
            public GenSort GSort;
            public Byte UnkByte3; // always 0?
            public TextureFiltering TextureFilter;
            public Byte UnkByte5; // always 0?
            public Single UnkFloat1; // always 25?
            public float CutOnRadius; // Version >= 0x6
            public float CutOffRadius; // Version >= 0x6
            public float DrawCutOff; // Version >= 0xA
            public Single UnkFloat5; // Version > 0x16, always 0?
            public Single UnkFloat6; // Version >= 0x18, always 0.5?
            public float Velocity;
            public float Random_Emit_X;
            public float Random_Emit_Y;
            public float Random_Emit_Z;
            public float Random_Start_X;
            public float Random_Start_Y;
            public float Random_Start_Z;
            public Single UnkFloat8;
            public Single UnkFloat9;
            public Single UnkFloat10;
            public Single UnkFloat11;
            public Single UnkFloat12;
            public Single UnkFloat13;
            public Single UnkFloat14;
            public Single UnkFloat15;
            public Single UnkFloat16;
            public Single UnkFloat17;
            public Single UnkFloat18;
            public Single UnkFloat19;
            public float Gravity;
            public float ParticleLifeTime;
            public UInt16 UnkUShort8; // usually 0 or 16 (together with the next two)
            public Byte UnkByte6; // usually 0 or 1
            public Byte UnkByte7; // usually 1 or 3
            public Single UnkFloat22; // usually 0 or 320
            public float JibberXFreq;
            public float JibberXAmp;
            public float JibberYFreq;
            public float JibberYAmp;
            public TwinsVector4[] UnkVecs; // Color timeline (Time 0.0 - 1.0 / RGB 0.0 - 255.0)
            public Int64[] UnkLongs1;
            public Single UnkFloat27; // Version > 0x15
            public Single UnkFloat28; // Version > 0x15
            public Single UnkFloat29;
            public Single UnkFloat30;
            public Int64[] UnkLongs2;
            public Int64[] UnkLongs3;
            public Single UnkFloat31;
            public Single UnkFloat32;
            public Int64[] UnkLongs4;
            public Int64[] UnkLongs5;
            public Int64[] UnkLongs6;
            public Single UnkFloat33;
            public Single UnkFloat34;
            public Single UnkFloat35;
            public Single UnkFloat36;
            public Int64[] UnkLongs7; // Version >= 0x3
            public Byte UnkByte8; // Version >= 0x3
            public Byte UnkByte9; // Version >= 0x11
            public Int32 padAmount; // Version > 0x16 && Version < 0x1D
            public Single UnkFloat37; // Version > 0x1B
            public Int16[] UnkShorts; // [1]: Version >= 0x10, [2]: Version >= 0x19, [0] and [3] always 0, [2] always 5?
            public Single UnkFloat38; // Version >= 0x10
            public Single UnkFloat39; // Version >= 0x19, always 0.5?
            public Single UnkFloat40; // Version >= 0x1A, always 0?
            public int TexturePage; // Version > 0x1A, (0-2)
            public TwinsVector4 UnkVec3; // Version >= 0x1E, default/editor spawn position? W always 0

            public enum GenSort
            {
                Normal = 0,
                Radial = 0x06,
                RadialRotor = 0x07,
                Spheroid = 0x08,
                BounceY = 0x09,
                BounceXZ = 0x0A,
                ImprovedRadial = 0x0B,
            }

            public enum TextureFiltering
            {
                Additive = 0,
                Unknown = 1, // negative? inverts colors, not the same as sub, exists in 5 particles in default
                Modulation = 0x02,
                Subtractive = 0x03,
                Glass = 0x07,
            }

            public ParticleSystemDefinition()
            {
                UnkVecs = new TwinsVector4[8]
                {
                    new TwinsVector4(),
                    new TwinsVector4(),
                    new TwinsVector4(),
                    new TwinsVector4(),
                    new TwinsVector4(),
                    new TwinsVector4(),
                    new TwinsVector4(),
                    new TwinsVector4(),
                };
                UnkLongs1 = new Int64[8];
                UnkLongs2 = new Int64[8];
                UnkLongs3 = new Int64[8];
                UnkLongs4 = new Int64[8];
                UnkLongs5 = new Int64[8];
                UnkLongs6 = new Int64[8];
                UnkLongs7 = new Int64[8];
                UnkShorts = new Int16[4];
                UnkVec3 = new TwinsVector4();
            }
        }

        public class ParticleSystemInstance
        {
            public Pos Position;
            public short GravityRotX; // Version >= 0x7
            public short GravityRotY; // Version >= 0x7
            public short EmitRotX;
            public short EmitRotY;
            public Int16 UnkShort5; // Version >= 0x16
            public uint Offset; // Version >= 0x08
            public string Name;
            public int SwitchType; // (0 - none, 1 - global switch) // Version >= 0x9
            public int SwitchID; // (default -1) // Version >= 0x9
            public float SwitchValue; // (0.0 - 20.0) // Version >= 0x9
            public Int16 UnkShort6; // Version >= 0xC
            public Int16 UnkShort7; // Version >= 0xC
            public float PlaneOffset; // -28.0 - 28.0 // Version >= 0xC
            public float BounceFactor; // 0.0 - 2.0 default 0.9 // Version >= 0xD
            public short GroupID; // (0-32) // Version >= 0xF
        }

    }
}
