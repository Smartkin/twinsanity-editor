using System.Collections.Generic;
using System.IO;

namespace Twinsanity
{
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

        public TwinsSecInfo SecInfo { get => sec_info; set => sec_info = value; }

        public int Level { get; set; }

        public override void Load(BinaryReader reader, int size)
        {
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
                if (Level >= 2) //if already on the 2nd level of sections, item
                {
                    TwinsItem rec = new TwinsItem();
                    rec.Offset = (uint)reader.BaseStream.Position;
                    rec.Load(reader, sub.Size);
                    sec_info.Records.Add(sub.ID, rec);
                }
                else //section
                {
                    TwinsSection sec = new TwinsSection();
                    sec.Level = Level + 1;
                    sec.Offset = (uint)reader.BaseStream.Position;
                    sec.Load(reader, sub.Size);
                    sec_info.Records.Add(sub.ID, sec);
                }
                reader.BaseStream.Position = sk;
            }
        }

        protected override int GetSize()
        {
            int size = SecInfo.Records.Count * 12 + 12;
            foreach (var item in SecInfo.Records.Values)
            {
                size += item.Size;
            }
            return size;
        }
    }
}
