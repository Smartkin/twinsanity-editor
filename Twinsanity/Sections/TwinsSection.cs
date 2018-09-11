using System.Collections.Generic;
using System.IO;

namespace Twinsanity
{
    /// <summary>
    /// Enumerator that determines what type of section this TwinsSection is. Preferable to making new classes for each section since they basically all have the same format.
    /// 
    /// Please append more section types at the END of this list.
    /// </summary>
    public enum SectionType {
        Null,
        Graphics,
        Code,
        Instance,

        Texture,
        Material,
        Mesh,
        Model,
        ArmatureModel,
        ActorModel,
        StaticModel,
        SpecialModel,
        Skybox,

        Object,
        Script,
        Animation,
        OGI,
        CodeModel,
        SE_Eng,
        SE_Fre,
        SE_Ger,
        SE_Spa,
        SE_Ita,
        SE_Jpn,

        UnknownInstance,
        AIPosition,
        AIPath,
        Position,
        Path,
        CollisionSurface,
        ObjectInstance,
        Trigger,
        Camera
    }

    public struct TwinsSecInfo
    {
        public uint Magic;
        public Dictionary<uint, TwinsItem> Records;
    }

    public class TwinsSection : TwinsItem
    {
        private TwinsSecInfo sec_info;
        private readonly uint magic = 0x00010001;
        private readonly uint magicV2 = 0x00010003;
        private int size;

        public TwinsSecInfo SecInfo { get => sec_info; set => sec_info = value; }
        public SectionType Type { get; set; }
        public int Level { get; set; }
        public int ContentSize { get => GetContentSize(); }

        /// <summary>
        /// Loads the section from a file.
        /// </summary>
        /// <param name="reader">BinaryReader already seeked to where the section begins.</param>
        /// <param name="size">Size of the section.</param>
        public override void Load(BinaryReader reader, int size)
        {
            this.size = size;
            sec_info.Records = new Dictionary<uint, TwinsItem>();
            if (size < 0xC || ((sec_info.Magic = reader.ReadUInt32()) != magic && sec_info.Magic != magicV2))
                return;
            var count = reader.ReadInt32();
            var sec_size = reader.ReadUInt32();
            for (int i = 0; i < count; i++)
            {
                TwinsSubInfo sub = new TwinsSubInfo();
                sub.Off = reader.ReadUInt32();
                sub.Size = reader.ReadInt32();
                sub.ID = reader.ReadUInt32();
                var sk = reader.BaseStream.Position;
                reader.BaseStream.Position = sk - (i + 2) * 0xC + sub.Off;
                //var m = reader.ReadUInt32(); //get magic number [obsolete?]
                //reader.BaseStream.Position -= 4;
                if (Level > 2) //if over the 2nd level of sections, assume item for safety
                {
                    LoadItem(reader, sub);
                }
                else //section
                {
                    switch (Type)
                    {
                        case SectionType.Graphics:
                            switch (sub.ID)
                            {
                                case 0:
                                    LoadSection(reader, sub, SectionType.Texture);
                                    break;
                                case 1:
                                    LoadSection(reader, sub, SectionType.Material);
                                    break;
                                case 2:
                                    LoadSection(reader, sub, SectionType.Mesh);
                                    break;
                                case 3:
                                    LoadSection(reader, sub, SectionType.Model);
                                    break;
                                case 4:
                                    LoadSection(reader, sub, SectionType.ArmatureModel);
                                    break;
                                case 5:
                                    LoadSection(reader, sub, SectionType.ActorModel);
                                    break;
                                case 6:
                                    LoadSection(reader, sub, SectionType.StaticModel);
                                    break;
                                case 7:
                                    LoadSection(reader, sub, SectionType.SpecialModel);
                                    break;
                                case 8:
                                    LoadSection(reader, sub, SectionType.Skybox);
                                    break;
                                default:
                                    LoadItem(reader, sub);
                                    break;
                            }
                            break;
                        case SectionType.Instance:
                            switch (sub.ID)
                            {
                                case 0:
                                    LoadSection(reader, sub, SectionType.UnknownInstance);
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
                                    LoadSection(reader, sub, SectionType.ObjectInstance);
                                    break;
                                case 7:
                                    LoadSection(reader, sub, SectionType.Trigger);
                                    break;
                                case 8:
                                    LoadSection(reader, sub, SectionType.Camera);
                                    break;
                                default:
                                    LoadItem(reader, sub);
                                    break;
                            }
                            break;
                        case SectionType.Code:
                            switch (sub.ID)
                            {
                                case 0:
                                    LoadSection(reader, sub, SectionType.Object);
                                    break;
                                case 1:
                                    LoadSection(reader, sub, SectionType.Script);
                                    break;
                                case 2:
                                    LoadSection(reader, sub, SectionType.Animation);
                                    break;
                                case 3:
                                    LoadSection(reader, sub, SectionType.OGI);
                                    break;
                                case 4:
                                    LoadSection(reader, sub, SectionType.CodeModel);
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
                                    LoadItem(reader, sub);
                                    break;
                            }
                            break;
                        default:
                            LoadItem(reader, sub);
                            break;
                    }
                }
                reader.BaseStream.Position = sk;
            }
        }

        private void LoadItem(BinaryReader reader, TwinsSubInfo sub)
        {
            TwinsItem rec = new TwinsItem {
                ID = sub.ID,
                Offset = (uint)reader.BaseStream.Position
            };
            rec.Load(reader, sub.Size);
            sec_info.Records.Add(sub.ID, rec);
        }

        private void LoadSection(BinaryReader reader, TwinsSubInfo sub, SectionType type)
        {
            TwinsSection sec = new TwinsSection {
                ID = sub.ID,
                Level = Level + 1,
                Offset = (uint)reader.BaseStream.Position,
                Type = type
            };
            sec.Load(reader, sub.Size);
            sec_info.Records.Add(sub.ID, sec);
        }

        public override void Save(BinaryWriter writer)
        {
            if (size == 0)
                return;
            writer.Write(sec_info.Magic);
            writer.Write(sec_info.Records.Count);
            writer.Write(ContentSize);

            var sec_off = sec_info.Records.Count * 12 + 12;
            foreach (var i in sec_info.Records)
            {
                writer.Write(sec_off);
                writer.Write(i.Value.Size);
                writer.Write(i.Key);
                sec_off += i.Value.Size;
            }

            foreach (var i in sec_info.Records.Values)
            {
                i.Save(writer);
            }
        }

        protected override int GetSize()
        {
            return size;
        }

        private int GetContentSize()
        {
            int c_size = 0;
            foreach (var i in SecInfo.Records.Values)
                c_size += i.Size;
            return c_size;
        }
    }
}
