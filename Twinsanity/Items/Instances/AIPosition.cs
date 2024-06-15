using System.IO;

namespace Twinsanity
{
    public class AIPosition : TwinsItem
    {
        public Pos Pos { get; set; } = new Pos(0, 0, 0, 1); // W is node weight?
        public ushort Num { get; set; }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Pos.X);
            writer.Write(Pos.Y);
            writer.Write(Pos.Z);
            writer.Write(Pos.W);
            writer.Write(Num);
        }

        public override void Load(BinaryReader reader, int size)
        {
            Pos = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            Num = reader.ReadUInt16();
        }

        protected override int GetSize()
        {
            return 18;
        }

        public enum NodeType : ushort
        {
            Ground = 0, // Default AI node
            Air = 2, // For jetpack ant / bat
            WormPath = 4, // Earth worm / farm chickens
            CortexPoint = 16, // Cortex interest point, also used for bird paths in L03
        }
    }
}
