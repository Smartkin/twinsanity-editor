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
                for (int j = 0; j < 4; ++j)
                {
                    writer.Write(i.LoadWall[j].X);
                    writer.Write(i.LoadWall[j].Y);
                    writer.Write(i.LoadWall[j].Z);
                    writer.Write(i.LoadWall[j].W);
                }
                if (i.Type == 1 || i.Type == 3) //type 1/3 continue here
                {
                    for (int j = 0; j < 4; ++j)
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
                ChunkLink link = new ChunkLink();
                link.Type = reader.ReadInt32();
                link.Path = reader.ReadChars(reader.ReadInt32());
                link.Flags = reader.ReadUInt32();
                link.ObjectMatrix = new Pos[4];
                for (int j = 0; j < 4; ++j)
                {
                    link.ObjectMatrix[j].X = reader.ReadSingle();
                    link.ObjectMatrix[j].Y = reader.ReadSingle();
                    link.ObjectMatrix[j].Z = reader.ReadSingle();
                    link.ObjectMatrix[j].W = reader.ReadSingle();
                }
                link.ChunkMatrix = new Pos[4];
                for (int j = 0; j < 4; ++j)
                {
                    link.ChunkMatrix[j].X = reader.ReadSingle();
                    link.ChunkMatrix[j].Y = reader.ReadSingle();
                    link.ChunkMatrix[j].Z = reader.ReadSingle();
                    link.ChunkMatrix[j].W = reader.ReadSingle();
                }
                if ((link.Flags & 0x80000) != 0)
                {
                    link.LoadWall = new Pos[4];
                    for (int j = 0; j < 4; ++j)
                    {
                        link.LoadWall[j].X = reader.ReadSingle();
                        link.LoadWall[j].Y = reader.ReadSingle();
                        link.LoadWall[j].Z = reader.ReadSingle();
                        link.LoadWall[j].W = reader.ReadSingle();
                    }
                }
                if (link.Type == 1 || link.Type == 3) //type 1/3 continue here
                {
                    link.Unknown = new short[15];
                    for (int j = 0; j < 15; ++j)
                    {
                        link.Unknown[j] = reader.ReadInt16();
                    }
                    link.LoadArea = new Pos[8];
                    for (int j = 0; j < 8; ++j)
                    {
                        link.LoadArea[j].X = reader.ReadSingle();
                        link.LoadArea[j].Y = reader.ReadSingle();
                        link.LoadArea[j].Z = reader.ReadSingle();
                        link.LoadArea[j].W = reader.ReadSingle();
                    }
                    link.AreaMatrix = new Pos[6];
                    for (int j = 0; j < 6; ++j)
                    {
                        link.AreaMatrix[j].X = reader.ReadSingle();
                        link.AreaMatrix[j].Y = reader.ReadSingle();
                        link.AreaMatrix[j].Z = reader.ReadSingle();
                        link.AreaMatrix[j].W = reader.ReadSingle();
                    }
                    link.UnknownMatrix = new Pos[6];
                    for (int j = 0; j < 6; ++j)
                    {
                        link.UnknownMatrix[j].X = reader.ReadSingle();
                        link.UnknownMatrix[j].Y = reader.ReadSingle();
                        link.UnknownMatrix[j].Z = reader.ReadSingle();
                        link.UnknownMatrix[j].W = reader.ReadSingle();
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
                size += i.Path.Length + 204;
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
