using System;
using System.Collections.Generic;
using System.IO;

namespace Twinsanity
{
    public struct TwinsSubInfo
    {
        public uint Off;
        public int Size;
        public uint ID;
    }

    /// <summary>
    /// Represents a Twinsanity RM/SM file, a full pair (+ .ptl in Xbox) corresponds to a full "chunk"
    /// </summary>
    public class TwinsFile
    {
        private readonly uint magic = 0x00010001;
        private TwinsSecInfo sec_info;

        public TwinsSecInfo SecInfo { get => sec_info; set => sec_info = value; }
        public int ContentSize { get => GetContentSize(); }
        public int Size { get => ContentSize + SecInfo.Records.Count * 12 + 12; }

        /// <summary>
        /// Load an RM/SM file. "rs" is a boolean that determines if the file being loaded is an RM (false) or an SM (true)
        /// </summary>
        public void LoadFile(string path, bool rs)
        {
            if (!File.Exists(path))
                return;
            sec_info.Records = new Dictionary<uint, TwinsItem>();
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(file);
            if ((sec_info.Magic = reader.ReadUInt32()) != magic)
                throw new ArgumentException("TwinsFile::LoadRM2File: Magic number is wrong.");
            var count = reader.ReadInt32();
            var sec_size = reader.ReadUInt32();
            TwinsSubInfo sub = new TwinsSubInfo();
            for (int i = 0; i < count; i++)
            {
                sub.Off = reader.ReadUInt32();
                sub.Size = reader.ReadInt32();
                sub.ID = reader.ReadUInt32();
                if (!rs)
                {
                    switch (sub.ID)
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
                                TwinsSection sec = new TwinsSection() { ID = sub.ID };
                                if (sub.ID <= 7)
                                    sec.Type = SectionType.Instance;
                                else if (sub.ID == 10)
                                    sec.Type = SectionType.Code;
                                else if (sub.ID == 11)
                                    sec.Type = SectionType.Graphics;
                                var sk = reader.BaseStream.Position;
                                reader.BaseStream.Position = sec.Offset = sub.Off;
                                sec.Level = 1;
                                sec.Load(reader, sub.Size);
                                reader.BaseStream.Position = sk;
                                sec_info.Records.Add(sub.ID, sec);
                                break;
                            }
                        case 9:
                            {
                                ColData rec = new ColData() { ID = sub.ID };
                                var sk = reader.BaseStream.Position;
                                reader.BaseStream.Position = rec.Offset = sub.Off;
                                rec.Load(reader, sub.Size);
                                reader.BaseStream.Position = sk;
                                sec_info.Records.Add(sub.ID, rec);
                                break;
                            }
                        default:
                            {
                                TwinsItem rec = new TwinsItem { ID = sub.ID };
                                var sk = reader.BaseStream.Position;
                                reader.BaseStream.Position = rec.Offset = sub.Off;
                                rec.Load(reader, sub.Size);
                                reader.BaseStream.Position = sk;
                                sec_info.Records.Add(sub.ID, rec);
                                break;
                            }
                    }
                }
                else
                {
                    switch (sub.ID)
                    {
                        case 6:
                            {
                                TwinsSection sec = new TwinsSection {
                                    ID = sub.ID,
                                    Type = SectionType.Graphics,
                                    Level = 1
                                };
                                var sk = reader.BaseStream.Position;
                                reader.BaseStream.Position = sec.Offset = sub.Off;
                                sec.Load(reader, sub.Size);
                                reader.BaseStream.Position = sk;
                                sec_info.Records.Add(sub.ID, sec);
                                break;
                            }
                        default:
                            {
                                TwinsItem rec = new TwinsItem { ID = sub.ID };
                                var sk = reader.BaseStream.Position;
                                reader.BaseStream.Position = rec.Offset = sub.Off;
                                rec.Load(reader, sub.Size);
                                reader.BaseStream.Position = sk;
                                sec_info.Records.Add(sub.ID, rec);
                                break;
                            }
                    }
                }
            }
            reader.Close();
            file.Close();
        }

        /// <summary>
        /// Save the file.
        /// </summary>
        /// <param name="path">File directory to save to.</param>
        public void SaveFile(string path)
        {
            FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(file);
            writer.Write(sec_info.Magic);
            writer.Write(sec_info.Records.Count);
            int size = 0;
            foreach (var i in sec_info.Records.Values)
                size += i.Size;
            writer.Write(size);

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

            writer.Close();
            file.Close();
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
