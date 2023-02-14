﻿using System;
using System.Collections.Generic;
using System.IO;
using Twinsanity;

namespace Twinsanity
{
    #region ENUMS & STRUCTS
    /// <summary>
    /// Enumerator that determines what type of section this TwinsSection is. Preferable to making new classes for each section since they basically all have the same format.
    /// 
    /// Please append more section types at the END of this list, BEFORE "Last".
    /// </summary>
    public enum SectionType {
        Null,
        Graphics, GraphicsX, GraphicsD, GraphicsMB,
        Code, CodeDemo, CodeX, CodeMB,
        Instance, InstanceDemo, InstanceMB,
        SceneryMB,
        ParticleData,
        Unknown,

        Texture, TextureX, TextureMB,
        Material, MaterialD,
        Model, ModelX, ModelMB,
        RigidModel,
        Skin, SkinX,
        BlendSkin, BlendSkinX,
        Mesh,
        LodModel,
        Skydome,

        Object, ObjectDemo, ObjectMB,
        Script, ScriptX, ScriptDemo, ScriptMB,
        Animation,
        OGI, GraphicsInfo, GraphicsInfoMB,
        CodeModel, CodeModelX, CodeModelDemo,
        SE, Xbox_SE, MB_SE,
        SE_Eng, Xbox_SE_Eng,
        SE_Fre, Xbox_SE_Fre,
        SE_Ger, Xbox_SE_Ger,
        SE_Spa, Xbox_SE_Spa,
        SE_Ita, Xbox_SE_Ita,
        SE_Jpn, Xbox_SE_Jpn,

        InstanceTemplate, InstanceTemplateDemo, InstanceTemplateMB,
        AIPosition,
        AIPath,
        Position,
        Path,
        CollisionSurface,
        ObjectInstance, ObjectInstanceDemo, ObjectInstanceMB,
        Trigger,
        Camera, CameraDemo,

        Last
    }

    public class TwinsSubInfo
    {
        public uint Off;
        public int Size;
        public uint ID;
    }
    #endregion

    public class TwinsSection : TwinsItem
    {
        protected static readonly uint magic = 0x00010001;
        protected static readonly uint magicV2 = 0x00010003;
        protected int size;

        public uint Magic { get; set; }
        public List<TwinsItem> Records = new List<TwinsItem>();
        public Dictionary<uint, int> RecordIDs = new Dictionary<uint, int>();
        public SectionType Type { get; set; }
        public int Level { get; set; }
        public int ContentSize { get => GetContentSize(); }

        public byte[] ExtraData { get; set; } = new byte[0];

        private bool isMonkeyBallPS2 { get; set; }

        public void Load(BinaryReader reader, int size, bool isMB)
        {
            isMonkeyBallPS2 = isMB;
            Load(reader, size);
        }

        /// <summary>
        /// Loads the section from a file.
        /// </summary>
        /// <param name="reader">BinaryReader already seeked to where the section begins.</param>
        /// <param name="size">Size of the section.</param>
        public override void Load(BinaryReader reader, int size)
        {
            this.size = size;
            Records = new List<TwinsItem>();
            RecordIDs = new Dictionary<uint, int>();
            var MagicStart = reader.BaseStream.Position;
            if (size < 0xC || ((Magic = reader.ReadUInt32()) != magic && Magic != magicV2))
                return;
            int count = 0;
            if (isMonkeyBallPS2)
            {
                count = reader.ReadInt16();
                reader.ReadByte();
            }
            else
            {
                count = reader.ReadInt32();
            }
            var sec_size = reader.ReadUInt32();
            if (isMonkeyBallPS2)
            {
                reader.ReadByte();
            }
            var start_sk = reader.BaseStream.Position - 12;
            long extra_begin = 12;
            if (isMonkeyBallPS2 && Level == 2)
            {
                List<TwinsSubInfo> SubItems = new List<TwinsSubInfo>();
                for (int i = 0; i < count; i++)
                {
                    TwinsSubInfo sub = new TwinsSubInfo
                    {
                        Off = reader.ReadUInt32(),
                        Size = reader.ReadInt32(),
                        ID = reader.ReadUInt32()
                    };
                    SubItems.Add(sub);
                }
                for (int i = 0; i < count; i++)
                {
                    int ItemSize = 0;
                    bool compressionCheck = true;
                    var sk = reader.BaseStream.Position;
                    if (i != count - 1)
                    {
                        ItemSize = (int)(SubItems[i + 1].Off - SubItems[i].Off);
                    }
                    else
                    {
                        ItemSize = (int)(size - SubItems[i].Off);
                    }
                    if (SubItems[i].Size == ItemSize)
                    {
                        //Console.WriteLine($"Section {Type} Item {i} ID {SubItems[i].ID:X8} Offset {(MagicStart + SubItems[i].Off):X8} Size {SubItems[i].Size} Compressed {ItemSize}");
                        compressionCheck = false;
                    }
                    else
                    {
                        //Console.WriteLine($"Section {Type} Item {i} ID {SubItems[i].ID:X8} Offset {(MagicStart + SubItems[i].Off):X8} Size {SubItems[i].Size} Compressed {ItemSize}");
                    }
                    SubItems[i].Size = ItemSize;
                    reader.BaseStream.Position = MagicStart + SubItems[i].Off;
                    extra_begin = Math.Max(SubItems[i].Off + SubItems[i].Size, extra_begin);
                    if (compressionCheck)
                    {
                        LoadItem<TwinsItem>(reader, SubItems[i], Type, true);
                    }
                    else
                    {
                        // some items are left uncompressed...
                        LoadSectionItem(reader, SubItems[i]);
                    }
                    reader.BaseStream.Position = sk;
                }
                reader.BaseStream.Position = start_sk + extra_begin;
                ExtraData = reader.ReadBytes((int)(size - extra_begin));
                return;
            }

            for (int i = 0; i < count; i++)
            {
                TwinsSubInfo sub = new TwinsSubInfo
                {
                    Off = reader.ReadUInt32(),
                    Size = reader.ReadInt32(),
                    ID = reader.ReadUInt32()
                };
                var sk = reader.BaseStream.Position;
                reader.BaseStream.Position = sk - (i + 2) * 0xC + sub.Off;
                extra_begin = Math.Max(sub.Off + sub.Size, extra_begin);
                //var m = reader.ReadUInt32(); //get magic number [obsolete?]
                //reader.BaseStream.Position -= 4;
                LoadSectionItem(reader, sub);
                reader.BaseStream.Position = sk;
            }
            reader.BaseStream.Position = start_sk + extra_begin;
            ExtraData = reader.ReadBytes((int)(size - extra_begin));
        }

        protected void LoadSectionItem(BinaryReader reader, TwinsSubInfo sub)
        {
            switch (Type)
            {
                case SectionType.Graphics:
                case SectionType.GraphicsX:
                case SectionType.GraphicsD:
                case SectionType.GraphicsMB:
                    switch (sub.ID)
                    {
                        case 0:
                            if (Type == SectionType.GraphicsX)
                                LoadSection(reader, sub, SectionType.TextureX);
                            else if (Type == SectionType.GraphicsMB && !isMonkeyBallPS2)
                                LoadSection(reader, sub, SectionType.TextureMB);
                            else
                                LoadSection(reader, sub, SectionType.Texture);
                            break;
                        case 1:
                            if (Type == SectionType.GraphicsD)
                                LoadSection(reader, sub, SectionType.MaterialD);
                            else
                                LoadSection(reader, sub, SectionType.Material);
                            break;
                        case 2:
                            if (Type == SectionType.GraphicsX)
                                LoadSection(reader, sub, SectionType.ModelX);
                            else if (Type == SectionType.GraphicsMB)
                                LoadSection(reader, sub, SectionType.ModelMB);
                            else
                                LoadSection(reader, sub, SectionType.Model);
                            break;
                        case 3:
                            LoadSection(reader, sub, SectionType.RigidModel);
                            break;
                        case 4:
                            if (Type == SectionType.GraphicsX)
                                LoadSection(reader, sub, SectionType.SkinX);
                            else
                                LoadSection(reader, sub, SectionType.Skin);
                            break;
                        case 5:
                            if (Type == SectionType.GraphicsX)
                                LoadSection(reader, sub, SectionType.BlendSkinX);
                            else
                                LoadSection(reader, sub, SectionType.BlendSkin);
                            break;
                        case 6:
                            LoadSection(reader, sub, SectionType.Mesh);
                            break;
                        case 7:
                            LoadSection(reader, sub, SectionType.LodModel);
                            break;
                        case 8:
                            LoadSection(reader, sub, SectionType.Skydome);
                            break;
                        default:
                            LoadItem<TwinsItem>(reader, sub, Type);
                            break;
                    }
                    break;
                case SectionType.Instance:
                case SectionType.InstanceDemo:
                    switch (sub.ID)
                    {
                        case 0:
                            if (Type == SectionType.InstanceDemo)
                                LoadSection(reader, sub, SectionType.InstanceTemplateDemo);
                            else
                                LoadSection(reader, sub, SectionType.InstanceTemplate);
                            break;
                        case 1:
                            LoadSection(reader, sub, SectionType.AIPosition);
                            break;
                        case 2:
                            LoadSection(reader, sub, SectionType.AIPath);
                            break;
                        case 3:
                            LoadSection(reader, sub, SectionType.Position);
                            break;
                        case 4:
                            LoadSection(reader, sub, SectionType.Path);
                            break;
                        case 5:
                            LoadSection(reader, sub, SectionType.CollisionSurface);
                            break;
                        case 6:
                            if (Type == SectionType.InstanceDemo)
                                LoadSection(reader, sub, SectionType.ObjectInstanceDemo);
                            else
                                LoadSection(reader, sub, SectionType.ObjectInstance);
                            break;
                        case 7:
                            LoadSection(reader, sub, SectionType.Trigger);
                            break;
                        case 8:
                            if (Type == SectionType.InstanceDemo)
                                LoadSection(reader, sub, SectionType.CameraDemo);
                            else
                                LoadSection(reader, sub, SectionType.Camera);
                            break;
                        default:
                            LoadItem<TwinsItem>(reader, sub, Type);
                            break;
                    }
                    break;
                case SectionType.InstanceMB:
                    switch (sub.ID)
                    {
                        case 0:
                            LoadSection(reader, sub, SectionType.InstanceTemplateMB);
                            break;
                        case 1:
                            LoadSection(reader, sub, SectionType.AIPosition);
                            break;
                        case 2:
                            LoadSection(reader, sub, SectionType.AIPath);
                            break;
                        case 3:
                            LoadSection(reader, sub, SectionType.Position);
                            break;
                        case 4:
                            LoadSection(reader, sub, SectionType.Path);
                            break;
                        case 5:
                            LoadSection(reader, sub, SectionType.CollisionSurface);
                            break;
                        case 6:
                            LoadSection(reader, sub, SectionType.ObjectInstanceMB);
                            break;
                        case 7:
                            LoadSection(reader, sub, SectionType.Trigger);
                            break;
                        case 8:
                            LoadSection(reader, sub, SectionType.Camera);
                            break;
                        default:
                            LoadItem<TwinsItem>(reader, sub, Type);
                            break;
                    }
                    break;
                case SectionType.Code:
                case SectionType.CodeX:
                case SectionType.CodeDemo:
                    switch (sub.ID)
                    {
                        case 0:
                            if (Type == SectionType.CodeDemo)
                                LoadSection(reader, sub, SectionType.ObjectDemo);
                            else
                                LoadSection(reader, sub, SectionType.Object);
                            break;
                        case 1:
                            if (Type == SectionType.Code)
                                LoadSection(reader, sub, SectionType.Script);
                            else if (Type == SectionType.CodeX)
                                LoadSection(reader, sub, SectionType.ScriptX);
                            else
                                LoadSection(reader, sub, SectionType.ScriptDemo);
                            break;
                        case 2:
                            LoadSection(reader, sub, SectionType.Animation);
                            break;
                        case 3:
                            LoadSection(reader, sub, SectionType.OGI);
                            break;
                        case 4:
                            if (Type == SectionType.Code)
                                LoadSection(reader, sub, SectionType.CodeModel);
                            else if (Type == SectionType.CodeX)
                                LoadSection(reader, sub, SectionType.CodeModelX);
                            else
                                LoadSection(reader, sub, SectionType.CodeModelDemo);
                            break;
                        case 6:
                            if (Type == SectionType.CodeX)
                                LoadSection(reader, sub, SectionType.Xbox_SE);
                            else
                                LoadSection(reader, sub, SectionType.SE);
                            break;
                        case 7:
                            if (Type == SectionType.CodeX)
                                LoadSection(reader, sub, SectionType.Xbox_SE_Eng);
                            else
                                LoadSection(reader, sub, SectionType.SE_Eng);
                            break;
                        case 8:
                            if (Type == SectionType.CodeX)
                                LoadSection(reader, sub, SectionType.Xbox_SE_Fre);
                            else
                                LoadSection(reader, sub, SectionType.SE_Fre);
                            break;
                        case 9:
                            if (Type == SectionType.CodeX)
                                LoadSection(reader, sub, SectionType.Xbox_SE_Ger);
                            else
                                LoadSection(reader, sub, SectionType.SE_Ger);
                            break;
                        case 10:
                            if (Type == SectionType.CodeX)
                                LoadSection(reader, sub, SectionType.Xbox_SE_Spa);
                            else
                                LoadSection(reader, sub, SectionType.SE_Spa);
                            break;
                        case 11:
                            if (Type == SectionType.CodeX)
                                LoadSection(reader, sub, SectionType.Xbox_SE_Ita);
                            else
                                LoadSection(reader, sub, SectionType.SE_Ita);
                            break;
                        case 12:
                            if (Type == SectionType.CodeX)
                                LoadSection(reader, sub, SectionType.Xbox_SE_Jpn);
                            else
                                LoadSection(reader, sub, SectionType.SE_Jpn);
                            break;
                        default:
                            LoadItem<TwinsItem>(reader, sub, Type);
                            break;
                    }
                    break;
                case SectionType.CodeMB:
                    switch (sub.ID)
                    {
                        case 0:
                            LoadSection(reader, sub, SectionType.ObjectMB);
                            break;
                        case 1:
                            LoadSection(reader, sub, SectionType.ScriptMB);
                            break;
                        case 2:
                            LoadSection(reader, sub, SectionType.Animation);
                            break;
                        case 3:
                            LoadSection(reader, sub, SectionType.GraphicsInfoMB);
                            break;
                        case 4:
                            LoadSection(reader, sub, SectionType.CodeModel);
                            break;
                        case 5:
                            LoadSection(reader, sub, SectionType.Unknown);
                            break;
                        case 6:
                            //loads forever on PSP
                            if (isMonkeyBallPS2)
                            {
                                LoadSection(reader, sub, SectionType.MB_SE);
                            }
                            else
                            {
                                LoadItem<TwinsItem>(reader, sub, Type);
                            }
                            break;
                        case 7:
                            LoadSection(reader, sub, SectionType.SE_Eng);
                            break;
                        case 8:
                            LoadSection(reader, sub, SectionType.SE_Fre);
                            break;
                        case 9:
                            LoadSection(reader, sub, SectionType.SE_Ger);
                            break;
                        case 10:
                            LoadSection(reader, sub, SectionType.SE_Spa);
                            break;
                        case 11:
                            LoadSection(reader, sub, SectionType.SE_Ita);
                            break;
                        case 12:
                            LoadSection(reader, sub, SectionType.SE_Jpn);
                            break;
                        default:
                            LoadItem<TwinsItem>(reader, sub, Type);
                            break;
                    }
                    break;
                case SectionType.Texture:
                    LoadItem<Texture>(reader, sub, Type);
                    break;
                case SectionType.TextureX: //XBOX textures
                    LoadItem<TextureX>(reader, sub, Type);
                    break;
                case SectionType.Material:
                    LoadItem<Material>(reader, sub, Type);
                    break;
                case SectionType.MaterialD: //PS2 DEMO Materials
                    LoadItem<MaterialDemo>(reader, sub, Type);
                    break;
                case SectionType.Model:
                    LoadItem<Model>(reader, sub, Type);
                    break;
                case SectionType.ModelX:
                    LoadItem<ModelX>(reader, sub, Type);
                    break;
                case SectionType.RigidModel:
                case SectionType.Mesh:
                    LoadItem<RigidModel>(reader, sub, Type);
                    break;
                case SectionType.Skydome:
                    LoadItem<Skydome>(reader, sub, Type);
                    break;
                case SectionType.Object:
                    LoadItem<GameObject>(reader, sub, Type);
                    break;
                case SectionType.ObjectDemo: //PS2 DEMO objects
                    LoadItem<GameObjectDemo>(reader, sub, Type);
                    break;
                case SectionType.CodeModel:
                    LoadItem<CodeModel>(reader, sub, Type);
                    break;
                case SectionType.CodeModelX:
                    LoadItem<CodeModel>(reader, sub, Type);
                    break;
                case SectionType.CodeModelDemo:
                    LoadItem<CodeModel>(reader, sub, Type);
                    break;
                case SectionType.Script:
                    LoadItem<Script>(reader, sub, Type);
                    break;
                case SectionType.ScriptX:
                    LoadItem<Script>(reader, sub, Type);
                    break;
                case SectionType.ScriptDemo:
                    LoadItem<Script>(reader, sub, Type);
                    break;
                case SectionType.ScriptMB:
                    LoadItem<Script>(reader, sub, Type);
                    break;
                case SectionType.Animation:
                    LoadItem<Animation>(reader, sub, Type);
                    break;
                case SectionType.SE:
                case SectionType.SE_Eng:
                case SectionType.SE_Fre:
                case SectionType.SE_Ger:
                case SectionType.SE_Ita:
                case SectionType.SE_Spa:
                case SectionType.SE_Jpn:
                    LoadItem<SoundEffect>(reader, sub, Type);
                    break;
                case SectionType.Xbox_SE:
                case SectionType.Xbox_SE_Eng:
                case SectionType.Xbox_SE_Fre:
                case SectionType.Xbox_SE_Ger:
                case SectionType.Xbox_SE_Ita:
                case SectionType.Xbox_SE_Jpn:
                case SectionType.Xbox_SE_Spa:
                    LoadItem<SoundEffectX>(reader, sub, Type);
                    break;
                case SectionType.AIPosition:
                    LoadItem<AIPosition>(reader, sub, Type);
                    break;
                case SectionType.AIPath:
                    LoadItem<AIPath>(reader, sub, Type);
                    break;
                case SectionType.Position:
                    LoadItem<Position>(reader, sub, Type);
                    break;
                case SectionType.Path:
                    LoadItem<Path>(reader, sub, Type);
                    break;
                case SectionType.ObjectInstance:
                    LoadItem<Instance>(reader, sub, Type);
                    break;
                case SectionType.ObjectInstanceDemo: //PS2 DEMO instances
                    LoadItem<InstanceDemo>(reader, sub, Type);
                    break;
                case SectionType.ObjectInstanceMB:
                    LoadItem<InstanceMB>(reader, sub, Type);
                    break;
                case SectionType.Trigger:
                    LoadItem<Trigger>(reader, sub, Type);
                    break;
                case SectionType.Camera:
                    LoadItem<Camera>(reader, sub, Type);
                    break;
                case SectionType.CameraDemo:
                    LoadItem<Camera>(reader, sub, Type);
                    break;
                case SectionType.OGI:
                    LoadItem<GraphicsInfo>(reader, sub, Type);
                    break;
                case SectionType.GraphicsInfoMB:
                    LoadItem<GraphicsInfoMB>(reader, sub, Type);
                    break;
                case SectionType.Skin:
                    LoadItem<Skin>(reader, sub, Type);
                    break;
                case SectionType.SkinX: //XBOX Skins
                    LoadItem<SkinX>(reader, sub, Type);
                    break;
                case SectionType.LodModel:
                    LoadItem<LodModel>(reader, sub, Type);
                    break;
                case SectionType.ParticleData:
                    LoadItem<ParticleData>(reader, sub, Type);
                    break;
                case SectionType.CollisionSurface:
                    LoadItem<CollisionSurface>(reader, sub, Type);
                    break;
                case SectionType.InstanceTemplate:
                    LoadItem<InstanceTemplate>(reader, sub, Type);
                    break;
                case SectionType.InstanceTemplateDemo:
                    LoadItem<InstanceTemplateDemo>(reader, sub, Type);
                    break;
                case SectionType.BlendSkin:
                    LoadItem<BlendSkin>(reader, sub, Type);
                    break;
                case SectionType.BlendSkinX:
                    LoadItem<BlendSkinX>(reader, sub, Type);
                    break;
                default:
                    LoadItem<TwinsItem>(reader, sub, Type);
                    break;
            }
        }

        protected void LoadItem<T>(BinaryReader reader, TwinsSubInfo sub, SectionType type, bool compressed = false) where T : TwinsItem, new()
        {
            T rec = new T
            {
                ID = sub.ID,
                Parent = this
            };
            rec.WasCompressed = compressed;
            rec.ParentType = type;
            rec.Load(reader, sub.Size);
            RecordIDs.Add(sub.ID, Records.Count);
            Records.Add(rec);
        }

        private void LoadSection(BinaryReader reader, TwinsSubInfo sub, SectionType type)
        {
            TwinsSection sec = new TwinsSection {
                ID = sub.ID,
                Level = Level + 1,
                Type = type,
                Parent = this,
                isMonkeyBallPS2 = this.isMonkeyBallPS2,
            };
            sec.Load(reader, sub.Size);
            RecordIDs.Add(sub.ID, Records.Count);
            Records.Add(sec);
        }

        private void CreateSection(SectionType type, uint id)
        {
            TwinsSection sec = new TwinsSection
            {
                ID = id,
                Level = Level + 1,
                Type = type,
                Parent = this,
                size = 12,
                Magic = magic
            };
            RecordIDs.Add(id, Records.Count);
            Records.Add(sec);
        }

        public override void Save(BinaryWriter writer)
        {
            if (size == 0)
                return;
            writer.Write(Magic);
            writer.Write(Records.Count);
            writer.Write(ContentSize);

            var sec_off = Records.Count * 12 + 12;
            foreach (var i in Records)
            {
                writer.Write(sec_off);
                writer.Write(i.Size);
                writer.Write(i.ID);
                sec_off += i.Size;
            }

            foreach (var i in Records)
            {
                i.Save(writer);
            }

            writer.Write(ExtraData);
        }

        protected override int GetSize()
        {
            if (size < 0xC)
                return size;
            else
                return (Records.Count + 1) * 12 + ContentSize + ExtraData.Length;
        }

        private int GetContentSize()
        {
            int c_size = 0;
            foreach (var i in Records)
                c_size += i.Size;
            return c_size;
        }

        public T GetItem<T>(uint id) where T : TwinsItem
        {
            return Records[RecordIDs[id]] as T;
        }

        public bool HasItem(uint id)
        {
            return RecordIDs.ContainsKey(id);
        }

        public void AddItem(uint id, TwinsItem item)
        {
            item.Parent = this;
            RecordIDs.Add(id, Records.Count);
            Records.Add(item);
        }

        public bool TryAddItem(uint id, TwinsItem item)
        {
            if (RecordIDs.ContainsKey(id))
                return false;
            RecordIDs.Add(id, Records.Count);
            Records.Add(item);
            return true;
        }

        public void RemoveItem(uint id)
        {
            var index = RecordIDs[id];
            RecordIDs.Remove(id);
            Records.RemoveAt(index);
            var new_recs = new Dictionary<uint, int>(RecordIDs);
            RecordIDs.Clear();
            foreach (var i in new_recs)
            {
                if (i.Value >= index)
                    RecordIDs.Add(i.Key, i.Value - 1);
                else
                    RecordIDs.Add(i.Key, i.Value);
            }
        }

        public bool ContainsItem(uint id)
        {
            return RecordIDs.ContainsKey(id);
        }

        public static TwinsSection CreateGraphicsSection()
        {
            TwinsSection section = new TwinsSection() { ID = 11, Magic = magic, size = 12 };
            section.Type = SectionType.Graphics;
            section.CreateSection(SectionType.Texture, 0);
            section.CreateSection(SectionType.Material, 1);
            section.CreateSection(SectionType.Model, 2);
            section.CreateSection(SectionType.RigidModel, 3);
            section.CreateSection(SectionType.Skin, 4);
            section.CreateSection(SectionType.BlendSkin, 5);
            section.CreateSection(SectionType.Mesh, 6);
            section.CreateSection(SectionType.LodModel, 7);
            section.CreateSection(SectionType.Skydome, 8);
            section.size = section.GetSize();
            return section;
        }

        public static TwinsSection CreateCodeSection()
        {
            TwinsSection section = new TwinsSection() { ID = 10, Magic = magic, size = 12 };
            section.Type = SectionType.Graphics;
            section.CreateSection(SectionType.Object, 0);
            section.CreateSection(SectionType.Script, 1);
            section.CreateSection(SectionType.Animation, 2);
            section.CreateSection(SectionType.OGI, 3);
            section.CreateSection(SectionType.CodeModel, 4);
            section.CreateSection(SectionType.Unknown, 5);
            section.CreateSection(SectionType.SE, 6);
            section.CreateSection(SectionType.SE_Eng, 7);
            section.CreateSection(SectionType.SE_Fre, 8);
            section.CreateSection(SectionType.SE_Ger, 9);
            section.CreateSection(SectionType.SE_Spa, 10);
            section.CreateSection(SectionType.SE_Ita, 11);
            section.CreateSection(SectionType.SE_Jpn, 12);
            section.size = section.GetSize();
            return section;
        }
    }
}
