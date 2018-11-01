using System.IO;
using System.Collections.Generic;

namespace Twinsanity
{
    public class Path : TwinsItem
    {
        private List<Pos> positions = new List<Pos>();
        private List<PathParam> pathParams = new List<PathParam>();

        public List<Pos> Positions { get => positions; set => positions = value; }
        public List<PathParam> Params { get => pathParams; set => pathParams = value; }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Positions.Count);
            foreach (var p in Positions)
            {
                writer.Write(p.X);
                writer.Write(p.Y);
                writer.Write(p.Z);
                writer.Write(p.W);
            }
            writer.Write(Params.Count);
            foreach (var p in Params)
            {
                writer.Write(p.Int1);
                writer.Write(p.Int2);
            }
        }

        public override void Load(BinaryReader reader, int size)
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; ++i)
            {
                positions.Add(new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
            }
            count = reader.ReadInt32();
            for (int i = 0; i < count; ++i)
            {
                pathParams.Add(new PathParam { Int1 = reader.ReadUInt32(), Int2 = reader.ReadUInt32() });
            }
        }

        protected override int GetSize()
        {
            return 8 + Positions.Count * 16 + Params.Count * 8;
        }

        #region STRUCTURES
        public struct PathParam
        {
            public uint Int1;
            public uint Int2;
        }
        #endregion
    }
}
