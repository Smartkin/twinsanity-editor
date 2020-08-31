using System.IO;

namespace Twinsanity
{
    public class Position : TwinsItem
    {
        protected override int? ConstantSize => 16;

        public Pos Pos { get; set; }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Pos.X);
            writer.Write(Pos.Y);
            writer.Write(Pos.Z);
            writer.Write(Pos.W);
        }

        public override void Load(BinaryReader reader, int size)
        {
            Pos = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }
    }
}
