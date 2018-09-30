using System.IO;
using System.Collections.Generic;

namespace Twinsanity
{
    public class Camera : TwinsItem
    {
        public int Header { get; set; }
        public int Type { get; set; }
        public float[] Floats { get; set; } = new float[5];
        public Pos CPos { get; set; }
        public Pos CSize { get; set; }
        public int[] Ints { get; set; } = new int[3];
        public short[] Shorts { get; set; } = new short[3];
        public float UnkFloat { get; set; }
        public int[] UnkInts { get; set; } = new int[2];

        //INCOMPLETE

        public override void Save(BinaryWriter writer)
        {
            base.Save(writer);
        }

        public override void Load(BinaryReader reader, int size)
        {
            base.Load(reader, size);
        }

        protected override int GetSize()
        {
            return 16;
        }
    }
}
