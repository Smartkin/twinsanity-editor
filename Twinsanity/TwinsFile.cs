using System;
using System.Collections.Generic;
using System.IO;

namespace Twinsanity
{
    /// <summary>
    /// Represents a Twinsanity RM/SM file, a full pair corresponds to a complete level "chunk"
    /// </summary>
    public class TwinsFile : TwinsSection
    {
        private readonly uint magic = 0x00010001;

        public string FileName { get; set; }
        public string SafeFileName { get; set; }

        public new FileType Type { get; set; }

        /// <summary>
        /// Load an RM/SM file.
        /// </summary>
        /// <param name="path">Path to the file to load from.</param>
        /// <param name="type">Filetype. RM2, SM2, etc.</param>
        public void LoadFile(string path, FileType type)
        {
            if (!File.Exists(path))
                return;
            Records = new List<TwinsItem>();
            RecordIDs = new Dictionary<uint, int>();
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(file);
            Type = type;
            if ((Magic = reader.ReadUInt32()) != magic)
                throw new ArgumentException("TwinsFile::LoadRM2File: Magic number is wrong.");
            var count = reader.ReadInt32();
            var sec_size = reader.ReadUInt32();
            uint s_off = 0, s_id = 0;
            int s_size = 0;
            for (int i = 0; i < count; i++)
            {
                s_off = reader.ReadUInt32();
                s_size = reader.ReadInt32();
                s_id = reader.ReadUInt32();
                switch (type)
                {
                    case FileType.DemoRM2:
                    case FileType.RMX:
                    case FileType.RM2:
                        {
                            switch (s_id)
                            {
                                case 0:
                                case 1:
                                case 2:
                                case 3:
                                case 4:
                                case 5:
                                case 6:
                                case 7:
                                case 10:
                                case 11:
                                    {
                                        TwinsSection sec = new TwinsSection() { ID = s_id };
                                        if (s_id <= 7)
                                            if (type == FileType.DemoRM2)
                                                sec.Type = SectionType.InstanceDemo;
                                            else
                                                sec.Type = SectionType.Instance;
                                        else if (s_id == 10)
                                            if (type == FileType.DemoRM2)
                                                sec.Type = SectionType.CodeDemo;
                                            else
                                                sec.Type = SectionType.Code;
                                        else if (s_id == 11)
                                            if (type == FileType.RMX)
                                                sec.Type = SectionType.GraphicsX;
                                            else
                                                sec.Type = SectionType.Graphics;
                                        var sk = reader.BaseStream.Position;
                                        reader.BaseStream.Position = sec.Offset = s_off;
                                        sec.Level = 1;
                                        sec.Load(reader, s_size);
                                        reader.BaseStream.Position = sk;
                                        RecordIDs.Add(s_id, Records.Count);
                                        Records.Add(sec);
                                        break;
                                    }
                                case 9:
                                    {
                                        ColData rec = new ColData() { ID = s_id };
                                        var sk = reader.BaseStream.Position;
                                        reader.BaseStream.Position = rec.Offset = s_off;
                                        rec.Load(reader, s_size);
                                        reader.BaseStream.Position = sk;
                                        RecordIDs.Add(s_id, Records.Count);
                                        Records.Add(rec);
                                        break;
                                    }
                                default:
                                    {
                                        TwinsItem rec = new TwinsItem { ID = s_id };
                                        var sk = reader.BaseStream.Position;
                                        reader.BaseStream.Position = rec.Offset = s_off;
                                        rec.Load(reader, s_size);
                                        reader.BaseStream.Position = sk;
                                        RecordIDs.Add(s_id, Records.Count);
                                        Records.Add(rec);
                                        break;
                                    }
                            }
                        }
                        break;
                    case FileType.DemoSM2:
                    case FileType.SM2:
                    case FileType.SMX:
                        {
                            switch (s_id)
                            {
                                case 6:
                                    {
                                        TwinsSection sec = new TwinsSection
                                        {
                                            ID = s_id,
                                            Type = type == FileType.SMX ? SectionType.GraphicsX : SectionType.Graphics,
                                            Level = 1
                                        };
                                        var sk = reader.BaseStream.Position;
                                        reader.BaseStream.Position = sec.Offset = s_off;
                                        sec.Load(reader, s_size);
                                        reader.BaseStream.Position = sk;
                                        RecordIDs.Add(s_id, Records.Count);
                                        Records.Add(sec);
                                        break;
                                    }
                                case 5:
                                    {
                                        ChunkLinks rec = new ChunkLinks { ID = s_id };
                                        var sk = reader.BaseStream.Position;
                                        reader.BaseStream.Position = rec.Offset = s_off;
                                        rec.Load(reader, s_size);
                                        reader.BaseStream.Position = sk;
                                        RecordIDs.Add(s_id, Records.Count);
                                        Records.Add(rec);
                                        break;
                                    }
                                default:
                                    {
                                        TwinsItem rec = new TwinsItem { ID = s_id };
                                        var sk = reader.BaseStream.Position;
                                        reader.BaseStream.Position = rec.Offset = s_off;
                                        rec.Load(reader, s_size);
                                        reader.BaseStream.Position = sk;
                                        RecordIDs.Add(s_id, Records.Count);
                                        Records.Add(rec);
                                        break;
                                    }
                            }
                        }
                        break;
                }
            }
            reader.Close();
        }

        /// <summary>
        /// Save the file.
        /// </summary>
        /// <param name="path">File directory to save to.</param>
        public void SaveFile(string path)
        {
            FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(file, System.Text.Encoding.ASCII);
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

            writer.Close();
        }
        
        private int GetContentSize()
        {
            int c_size = 0;
            foreach (var i in Records)
                c_size += i.Size;
            return c_size;
        }

        protected override int GetSize()
        {
            return ContentSize + Records.Count * 12 + 12;
        }

        //NOTE: Do NOT use "First"
        public enum FileType { First = SectionType.Last, RM2, SM2, DemoRM2, DemoSM2, RMX, SMX };
    }
}
