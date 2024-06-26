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
        public string FileName { get; set; }
        public string SafeFileName { get; set; }

        public new FileType Type { get; set; }
        public ConsoleType Console { get; set; }

        public void LoadFile(string fileName, FileType type)
        {
            FileName = fileName;

            byte[] buffer;
            using (var br = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 0x10000, FileOptions.SequentialScan))
            {
                buffer = new byte[br.Length];
                br.Read(buffer, 0, buffer.Length);
            }
            using (var memoryStream = new MemoryStream(buffer))
            {
                using (BinaryReader reader = new BinaryReader(memoryStream))
                {
                    LoadFileStream(reader, fileName, type);
                }
            }
        }

        /// <summary>
        /// Load an RM/SM file.
        /// </summary>
        /// <param name="path">Path to the file to load from.</param>
        /// <param name="type">Filetype. RM2, SM2, etc.</param>
        public void LoadFileStream(BinaryReader reader, string path, FileType type)
        {
            if (!File.Exists(path))
                return;
            Records = new List<TwinsItem>();
            RecordIDs = new Dictionary<uint, int>();
            var file = new FileStream(path, FileMode.Open, FileAccess.Read);
            Type = type;
            Console = ConsoleType.PS2;
            if (type == FileType.RMX || type == FileType.SMX) Console = ConsoleType.XBOX;
            if (type == FileType.Frontend)
            {
                TwinsSection sec = new TwinsSection
                {
                    ID = 3,
                    Type = SectionType.SE,
                };
                var sk = reader.BaseStream.Position;
                sec.Load(reader, (int)file.Length);
                reader.BaseStream.Position = sk;
                RecordIDs.Add(3, Records.Count);
                Records.Add(sec);
                reader.Close();
                return;
            }
            if ((Magic = reader.ReadUInt32()) != magic)
                throw new Exception("LoadFile: Magic number is wrong.");
            FileName = path;
            int count = 0;
            bool miniFix = false;
            if (type == FileType.MonkeyBallRM || type == FileType.MonkeyBallSM)
            {
                count = reader.ReadInt16();
                uint test = reader.ReadUInt16();
                if (test != 0) // PS2 file and sections contain 0x0080 here
                {
                    miniFix = true;
                }
                else
                {
                    Console = ConsoleType.PSP;
                }
            }
            else
            {
                count = reader.ReadInt32();
            }
            var sec_size = reader.ReadUInt32();

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
                uint s_off = SubItems[i].Off;
                uint s_id = SubItems[i].ID;
                int s_size = SubItems[i].Size;
                reader.BaseStream.Position = s_off;

                BinaryReader secReader = reader;
                MemoryStream subMem = null;
                if (miniFix)
                {
                    int ItemSize = 0;
                    if (i != count - 1)
                        ItemSize = (int)(SubItems[i + 1].Off - SubItems[i].Off);
                    else
                        ItemSize = (int)(reader.BaseStream.Length - SubItems[i].Off);
                    if (SubItems[i].Size != ItemSize)
                    {
                        try
                        {
                            reader.ReadBytes(4); // PACK
                            byte[] outData = InteropUCL.DecompressNRV2B(reader.ReadBytes(ItemSize - 4));
                            subMem = new MemoryStream(outData);
                            secReader = new BinaryReader(subMem);
                            s_off = 0;
                        }
                        catch
                        {
                            System.Console.WriteLine($"Failed to unpack item {SubItems[i].ID} in {Type}");
                        }
                    }
                }

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
                                        TwinsSection sec = new TwinsSection() { ID = s_id, Parent = this };
                                        if (s_id <= 7)
                                            if (type == FileType.DemoRM2)
                                                sec.Type = SectionType.InstanceDemo;
                                            else
                                                sec.Type = SectionType.Instance;
                                        else if (s_id == 10)
                                            if (type == FileType.DemoRM2)
                                                sec.Type = SectionType.CodeDemo;
                                            else if (type == FileType.RMX)
                                                sec.Type = SectionType.CodeX;
                                            else
                                                sec.Type = SectionType.Code;
                                        else if (s_id == 11)
                                            if (type == FileType.RMX)
                                                sec.Type = SectionType.GraphicsX;
                                            else if (type == FileType.DemoRM2)
                                                sec.Type = SectionType.GraphicsD;
                                            else
                                                sec.Type = SectionType.Graphics;
                                        var sk = reader.BaseStream.Position;
                                        reader.BaseStream.Position = s_off;
                                        sec.Level = 1;
                                        sec.Load(reader, s_size);
                                        reader.BaseStream.Position = sk;
                                        RecordIDs.Add(s_id, Records.Count);
                                        Records.Add(sec);
                                        break;
                                    }
                                case 9:
                                    {
                                        ColData rec = new ColData() { ID = s_id, Parent = this };
                                        var sk = reader.BaseStream.Position;
                                        reader.BaseStream.Position = s_off;
                                        rec.Load(reader, s_size);
                                        reader.BaseStream.Position = sk;
                                        RecordIDs.Add(s_id, Records.Count);
                                        Records.Add(rec);
                                        break;
                                    }
                                case 8:
                                    {
                                        ParticleData rec = new ParticleData() { ID = s_id, Parent = this };
                                        var sk = reader.BaseStream.Position;
                                        reader.BaseStream.Position = s_off;
                                        rec.Load(reader, s_size);
                                        reader.BaseStream.Position = sk;
                                        RecordIDs.Add(s_id, Records.Count);
                                        Records.Add(rec);
                                        break;
                                    }
                                default:
                                    {
                                        TwinsItem rec = new TwinsItem { ID = s_id, Parent = this };
                                        var sk = reader.BaseStream.Position;
                                        reader.BaseStream.Position = s_off;
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
                                        SectionType targetType = SectionType.Graphics;
                                        if (type == FileType.SMX)
                                            targetType = SectionType.GraphicsX;
                                        if (type == FileType.DemoSM2)
                                            targetType = SectionType.GraphicsD;
                                        TwinsSection sec = new TwinsSection
                                        {
                                            ID = s_id,
                                            Type = targetType,
                                            Level = 1,
                                            Parent = this
                                        };
                                        var sk = reader.BaseStream.Position;
                                        reader.BaseStream.Position = s_off;
                                        sec.Load(reader, s_size);
                                        reader.BaseStream.Position = sk;
                                        RecordIDs.Add(s_id, Records.Count);
                                        Records.Add(sec);
                                        break;
                                    }
                                case 5:
                                    {
                                        ChunkLinks rec = new ChunkLinks { ID = s_id, Parent = this };
                                        var sk = reader.BaseStream.Position;
                                        reader.BaseStream.Position = s_off;
                                        rec.Load(reader, s_size);
                                        reader.BaseStream.Position = sk;
                                        RecordIDs.Add(s_id, Records.Count);
                                        Records.Add(rec);
                                        break;
                                    }
                                case 0:
                                    {
                                        SceneryData rec = new SceneryData { ID = s_id, Parent = this };
                                        var sk = reader.BaseStream.Position;
                                        reader.BaseStream.Position = s_off;
                                        rec.Load(reader, s_size);
                                        reader.BaseStream.Position = sk;
                                        RecordIDs.Add(s_id, Records.Count);
                                        Records.Add(rec);
                                        break;
                                    }
                                case 4:
                                    {
                                        DynamicSceneryData rec = new DynamicSceneryData { ID = s_id, Parent = this };
                                        var sk = reader.BaseStream.Position;
                                        reader.BaseStream.Position = s_off;
                                        rec.Load(reader, s_size);
                                        reader.BaseStream.Position = sk;
                                        RecordIDs.Add(s_id, Records.Count);
                                        Records.Add(rec);
                                        break;
                                    }
                                default:
                                    {
                                        TwinsItem rec = new TwinsItem { ID = s_id, Parent = this };
                                        var sk = reader.BaseStream.Position;
                                        reader.BaseStream.Position = s_off;
                                        rec.Load(reader, s_size);
                                        reader.BaseStream.Position = sk;
                                        RecordIDs.Add(s_id, Records.Count);
                                        Records.Add(rec);
                                        break;
                                    }
                            }
                        }
                        break;
                    case FileType.MonkeyBallRM:
                        {
                            switch (s_id)
                            {
                                case 1:
                                case 2:
                                case 3:
                                case 4:
                                case 5:
                                case 6:
                                case 7:
                                case 8:
                                case 11:
                                case 12:
                                    {
                                        TwinsSection sec = new TwinsSection() { ID = s_id, Parent = this };
                                        if (s_id == 12)
                                        {
                                            sec.Type = SectionType.GraphicsMB;
                                        }
                                        else if (s_id == 11)
                                        {
                                            sec.Type = SectionType.CodeMB;
                                        }
                                        else
                                        {
                                            sec.Type = SectionType.InstanceMB;
                                        }
                                        var sk = reader.BaseStream.Position;
                                        reader.BaseStream.Position = s_off;
                                        sec.Level = 1;
                                        sec.Load(reader, s_size, miniFix);
                                        reader.BaseStream.Position = sk;
                                        RecordIDs.Add(s_id, Records.Count);
                                        Records.Add(sec);
                                        break;
                                    }
                                case 9:
                                    {
                                        ParticleData rec = new ParticleData() { ID = s_id, Parent = this };
                                        var sk = secReader.BaseStream.Position;
                                        secReader.BaseStream.Position = s_off;
                                        rec.Load(secReader, s_size, true);
                                        secReader.BaseStream.Position = sk;
                                        RecordIDs.Add(s_id, Records.Count);
                                        Records.Add(rec);
                                        break;
                                    }
                                case 10:
                                    {
                                        if (!miniFix)
                                        {
                                            ColData rec = new ColData() { ID = s_id, Parent = this };
                                            var sk = secReader.BaseStream.Position;
                                            secReader.BaseStream.Position = s_off;
                                            rec.Load(secReader, s_size);
                                            secReader.BaseStream.Position = sk;
                                            RecordIDs.Add(s_id, Records.Count);
                                            Records.Add(rec);
                                        }
                                        else
                                        {
                                            ColDataMB rec = new ColDataMB() { ID = s_id, Parent = this };
                                            var sk = secReader.BaseStream.Position;
                                            secReader.BaseStream.Position = s_off;
                                            rec.Load(secReader, s_size);
                                            secReader.BaseStream.Position = sk;
                                            RecordIDs.Add(s_id, Records.Count);
                                            Records.Add(rec);
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        TwinsItem rec = new TwinsItem { ID = s_id, Parent = this };
                                        var sk = secReader.BaseStream.Position;
                                        secReader.BaseStream.Position = s_off;
                                        rec.Load(secReader, s_size);
                                        secReader.BaseStream.Position = sk;
                                        RecordIDs.Add(s_id, Records.Count);
                                        Records.Add(rec);
                                        break;
                                    }
                            }
                        }
                        break;
                    case FileType.MonkeyBallSM:
                        {
                            switch (s_id)
                            {
                                default:
                                    {
                                        TwinsItem rec = new TwinsItem { ID = s_id, Parent = this };
                                        var sk = secReader.BaseStream.Position;
                                        secReader.BaseStream.Position = s_off;
                                        rec.Load(secReader, s_size);
                                        secReader.BaseStream.Position = sk;
                                        RecordIDs.Add(s_id, Records.Count);
                                        Records.Add(rec);
                                        break;
                                    }
                                case 0:
                                    {
                                        if (miniFix)
                                        {
                                            SceneryData rec = new SceneryData { ID = s_id, IsMonkeyBall = true, Parent = this };
                                            var sk = secReader.BaseStream.Position;
                                            secReader.BaseStream.Position = s_off;
                                            rec.Load(secReader, s_size);
                                            secReader.BaseStream.Position = sk;
                                            RecordIDs.Add(s_id, Records.Count);
                                            Records.Add(rec);
                                            break;
                                        }
                                        else
                                        {
                                            TwinsItem rec = new TwinsItem { ID = s_id, Parent = this };
                                            var sk = secReader.BaseStream.Position;
                                            secReader.BaseStream.Position = s_off;
                                            rec.Load(secReader, s_size);
                                            secReader.BaseStream.Position = sk;
                                            RecordIDs.Add(s_id, Records.Count);
                                            Records.Add(rec);
                                            break;
                                        }
                                    }
                                case 8:
                                    {
                                        SectionType targetType = SectionType.SceneryMB;
                                        TwinsSection sec = new TwinsSection
                                        {
                                            ID = s_id,
                                            Type = targetType,
                                            Level = 1,
                                            Parent = this
                                        };
                                        var sk = secReader.BaseStream.Position;
                                        secReader.BaseStream.Position = s_off;
                                        sec.Load(secReader, s_size);
                                        secReader.BaseStream.Position = sk;
                                        RecordIDs.Add(s_id, Records.Count);
                                        Records.Add(sec);
                                        break;
                                    }
                                case 5:
                                    {
                                        DynamicSceneryDataMB rec = new DynamicSceneryDataMB { ID = s_id, Parent = this };
                                        var sk = secReader.BaseStream.Position;
                                        secReader.BaseStream.Position = s_off;
                                        rec.Load(secReader, s_size);
                                        secReader.BaseStream.Position = sk;
                                        RecordIDs.Add(s_id, Records.Count);
                                        Records.Add(rec);
                                        break;
                                    }
                                case 7:
                                    {
                                        // empty on PSP
                                        SectionType targetType = SectionType.GraphicsMB;
                                        TwinsSection sec = new TwinsSection
                                        {
                                            ID = s_id,
                                            Type = targetType,
                                            Level = 1,
                                            Parent = this
                                        };
                                        var sk = reader.BaseStream.Position;
                                        reader.BaseStream.Position = s_off;
                                        sec.Load(reader, s_size, miniFix);
                                        reader.BaseStream.Position = sk;
                                        RecordIDs.Add(s_id, Records.Count);
                                        Records.Add(sec);
                                        break;
                                    }
                                case 6:
                                    {
                                        ChunkLinks rec = new ChunkLinks { ID = s_id, Parent = this };
                                        var sk = secReader.BaseStream.Position;
                                        secReader.BaseStream.Position = s_off;
                                        rec.Load(secReader, s_size);
                                        secReader.BaseStream.Position = sk;
                                        RecordIDs.Add(s_id, Records.Count);
                                        Records.Add(rec);
                                        break;
                                    }
                            }
                        }
                        break;
                }

                if (subMem != null)
                {
                    subMem.Close();
                }
            }
        }

        public void Merge(TwinsFile package)
        {
            var importObjects = package.GetItem<TwinsSection>(10).GetItem<TwinsSection>(0);
            var existingObjects = GetItem<TwinsSection>(10).GetItem<TwinsSection>(0);
            foreach (GameObject importObject in importObjects.Records)
            {
                if (existingObjects.HasItem(importObject.ID))
                {
                    continue;
                }
                importObject.FillPackage(package, this);
            }
        }

        public void FillExportPackageStructure(ConsoleType console = ConsoleType.PS2)
        {
            Magic = magic;
            Console = console;
            var graphicsSection = CreateGraphicsSection();
            RecordIDs.Add(graphicsSection.ID, Records.Count);
            Records.Add(graphicsSection);
            var codeSection = CreateCodeSection();
            RecordIDs.Add(codeSection.ID, Records.Count);
            Records.Add(codeSection);
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
        public enum FileType { First = SectionType.Last, RM2, SM2, DemoRM2, DemoSM2, RMX, SMX, Frontend, MonkeyBallRM, MonkeyBallSM };

        public enum ConsoleType { First = SectionType.Last, PS2, PSP, XBOX }
    }
}
