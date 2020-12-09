using System.Collections.Generic;
using System.IO;
using System;

namespace Twinsanity
{
    public class ChunkLinks : TwinsItem
    {
        public List<ChunkLink> Links { get; set; } = new List<ChunkLink>();

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
                if (i.TreeRoot != null)
                {
                    SaveTree(writer, (ChunkLink.LinkTree)i.TreeRoot);
                }
            }
        }

        public override void Load(BinaryReader reader, int size)
        {
            long start_pos = reader.BaseStream.Position;

            Links.Clear();
            var count = reader.ReadInt32();
            for (int i = 0; i < count; ++i)
            {
                ChunkLink link = new ChunkLink() { };
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

                link.TreeRoot = ReadTree(reader, link.Type);

                Links.Add(link);
            }

            //Console.WriteLine("end pos: " + (reader.BaseStream.Position - start_pos) + " target: " + size);

        }

        private ChunkLink.LinkTree? ReadTree(BinaryReader reader, int Head)
        {
            if ((Head & 0x1) == 0)
            {
                return null;
            }

            ChunkLink.LinkTree Node = new ChunkLink.LinkTree();
            Node.Header = reader.ReadInt32();

            byte[] Header = reader.ReadBytes(0x16);
            int blobSize = reader.ReadInt32();

            Node.LoadArea = new Pos[8];
            Node.AreaMatrix = new Pos[6];
            Node.UnknownMatrix = new Pos[6];
            for (int j = 0; j < 8; ++j)
            {
                Node.LoadArea[j] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            }
            for (int j = 0; j < 6; ++j)
            {
                Node.AreaMatrix[j] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            }
            for (int j = 0; j < 6; ++j)
            {
                Node.UnknownMatrix[j] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            }
            byte[] Blob = reader.ReadBytes(blobSize - 320);
            Node.GI_Type = new GraphicsInfo.GI_Type4() { Header = Header, unkBlob = Blob };

            Node.Ptr = ReadTree(reader, Node.Header);

            return Node;
        }

        private void SaveTree(BinaryWriter writer, ChunkLink.LinkTree node)
        {
            writer.Write(node.Header);
            writer.Write(node.GI_Type.Header);
            writer.Write(node.GI_Type.unkBlob.Length + 320);

            for (int j = 0; j < node.LoadArea.Length; ++j)
            {
                writer.Write(node.LoadArea[j].X);
                writer.Write(node.LoadArea[j].Y);
                writer.Write(node.LoadArea[j].Z);
                writer.Write(node.LoadArea[j].W);
            }
            for (int j = 0; j < node.AreaMatrix.Length; ++j)
            {
                writer.Write(node.AreaMatrix[j].X);
                writer.Write(node.AreaMatrix[j].Y);
                writer.Write(node.AreaMatrix[j].Z);
                writer.Write(node.AreaMatrix[j].W);
            }
            for (int j = 0; j < node.UnknownMatrix.Length; ++j)
            {
                writer.Write(node.UnknownMatrix[j].X);
                writer.Write(node.UnknownMatrix[j].Y);
                writer.Write(node.UnknownMatrix[j].Z);
                writer.Write(node.UnknownMatrix[j].W);
            }

            writer.Write(node.GI_Type.unkBlob);

            if (node.Ptr != null)
            {
                SaveTree(writer, (ChunkLink.LinkTree)node.Ptr);
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
                if (i.TreeRoot != null)
                {
                    CountTree((ChunkLink.LinkTree)i.TreeRoot, ref size);
                }
            }
            return size;
        }

        private void CountTree(ChunkLink.LinkTree ptr, ref int size)
        {
            size += 350 + ptr.GI_Type.unkBlob.Length;
            if (ptr.Ptr != null)
            {
                CountTree((ChunkLink.LinkTree)ptr.Ptr, ref size);
            }
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
            public LinkTree? TreeRoot;

            public bool HasWall()
            {
                return (Flags & 0x80000) != 0;
            }

            public bool HasTree()
            {
                return (Type & 0x1) != 0;
            }

            public struct LinkTree
            {
                public int Header;
                public GraphicsInfo.GI_Type4 GI_Type;
                public Pos[] LoadArea; // 8
                public Pos[] AreaMatrix; // 6
                public Pos[] UnknownMatrix; // 6

                public object Ptr;
            }
        }
        #endregion
    }
}
