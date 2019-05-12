using System.Collections.Generic;
using System.IO;

namespace Twinsanity
{
    public class ChunkLinks : TwinsItem
    {
        private List<ChunkLink> links = new List<ChunkLink>();

        public List<ChunkLink> Links { get => links; set => links = value; }


        /////////PARENTS FUNCTION//////////
        public override void Save(BinaryWriter writer)
        {
            writer.Write(Links.Count);
            foreach (var i in Links)
            {
                writer.Write(i.Type);
                writer.Write(i.Path.Length);
                writer.Write(i.Path);
                writer.Write(i.Flags);
                for (int j = 0; j < 4; ++j)
                {
                    writer.Write(i.ObjectMatrix[j].X);
                    writer.Write(i.ObjectMatrix[j].Y);
                    writer.Write(i.ObjectMatrix[j].Z);
                    writer.Write(i.ObjectMatrix[j].W);
                }
                for (int j = 0; j < 4; ++j)
                {
                    writer.Write(i.ChunkMatrix[j].X);
                    writer.Write(i.ChunkMatrix[j].Y);
                    writer.Write(i.ChunkMatrix[j].Z);
                    writer.Write(i.ChunkMatrix[j].W);
                }
                if ((i.Flags & 0x80000) != 0)
                {
                    for (int j = 0; j < 4; ++j)
                    {
                        writer.Write(i.LoadWall[j].X);
                        writer.Write(i.LoadWall[j].Y);
                        writer.Write(i.LoadWall[j].Z);
                        writer.Write(i.LoadWall[j].W);
                    }
                }
                if (i.Type == 1 || i.Type == 3) //type 1/3 continue here
                {
                    for (int j = 0; j < 15; ++j)
                    {
                        writer.Write(i.Unknown[j]);
                    }
                    for (int j = 0; j < 8; ++j)
                    {
                        writer.Write(i.LoadArea[j].X);
                        writer.Write(i.LoadArea[j].Y);
                        writer.Write(i.LoadArea[j].Z);
                        writer.Write(i.LoadArea[j].W);
                    }
                    for (int j = 0; j < 6; ++j)
                    {
                        writer.Write(i.AreaMatrix[j].X);
                        writer.Write(i.AreaMatrix[j].Y);
                        writer.Write(i.AreaMatrix[j].Z);
                        writer.Write(i.AreaMatrix[j].W);
                    }
                    for (int j = 0; j < 6; ++j)
                    {
                        writer.Write(i.UnknownMatrix[j].X);
                        writer.Write(i.UnknownMatrix[j].Y);
                        writer.Write(i.UnknownMatrix[j].Z);
                        writer.Write(i.UnknownMatrix[j].W);
                    }
                    writer.Write(i.Bytes);
                }
            }
        }

        public override void Load(BinaryReader reader, int size)
        {
            Links.Clear();
            var count = reader.ReadInt32();
            for (int i = 0; i < count; ++i)
            {
                ChunkLink link = new ChunkLink() { Unknown = new short[15] { 0, 0, 8, 12, 6, 3, 3, 128, 224, 272, 320, 326, 356, 380, 0 }, Bytes = new byte[60] { 0, 5, 10, 15, 20, 25, 4, 2, 3, 1, 0, 4, 4, 5, 3, 2, 4, 6, 7, 5, 4, 4, 0, 1, 7, 6, 4, 3, 5, 7, 1, 4, 4, 2, 0, 6, 0, 1, 1, 3, 3, 2, 2, 0, 3, 5, 5, 4, 4, 2, 5, 7, 7, 6, 6, 4, 7, 1, 0, 6, } };
                link.Type = reader.ReadInt32();
                link.Path = reader.ReadChars(reader.ReadInt32());
                link.Flags = reader.ReadUInt32();
                link.ObjectMatrix = new Pos[4];
                for (int j = 0; j < 4; ++j)
                {
                    link.ObjectMatrix[j] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                }
                link.ChunkMatrix = new Pos[4];
                for (int j = 0; j < 4; ++j)
                {
                    link.ChunkMatrix[j] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                }
                link.LoadWall = new Pos[4];
                if ((link.Flags & 0x80000) != 0)
                {
                    for (int j = 0; j < 4; ++j)
                    {
                        link.LoadWall[j] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    }
                }
                link.Unknown = new short[15];
                link.LoadArea = new Pos[8];
                link.AreaMatrix = new Pos[6];
                link.UnknownMatrix = new Pos[6];
                link.Bytes = new byte[60];
                if (link.Type == 1 || link.Type == 3) //type 1/3 continue here
                {
                    for (int j = 0; j < 15; ++j)
                    {
                        link.Unknown[j] = reader.ReadInt16();
                    }
                    for (int j = 0; j < 8; ++j)
                    {
                        link.LoadArea[j] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    }
                    for (int j = 0; j < 6; ++j)
                    {
                        link.AreaMatrix[j] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    }
                    for (int j = 0; j < 6; ++j)
                    {
                        link.UnknownMatrix[j] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    }
                    link.Bytes = reader.ReadBytes(60);
                }
                Links.Add(link);
            }
        }

        protected override int GetSize()
        {
            int size = 4;
            foreach (var i in Links)
            {
                size += i.Path.Length + 8 + 132;
                if ((i.Flags & 0x80000) != 0)
                    size += 64;
                if (i.Type == 1 || i.Type == 3)
                    size += 410;
            }
            return size;
        }

        #region STRUCTURES
        public struct ChunkLink
        {
            public int Type;
            public char[] Path;
            public uint Flags;
            public Pos[] ObjectMatrix; // 4
            public Pos[] ChunkMatrix; // 4
            public Pos[] LoadWall; // 4
            // Type 1 stuff
            public short[] Unknown; // 15
            public Pos[] LoadArea; // 8
            public Pos[] AreaMatrix; // 6
            public Pos[] UnknownMatrix; // 6
            public byte[] Bytes; // 60
        }
        #endregion
    }
}
