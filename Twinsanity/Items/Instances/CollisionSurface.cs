using System.IO;

namespace Twinsanity
{
    public class CollisionSurface : TwinsItem
    {
        public uint Flags = 0x001FF0F0;
        public ushort SurfaceID;
        public ushort Sound_1 = 65535;
        public ushort Sound_2 = 65535;
        public ushort Particle_1 = 65535;
        public ushort Particle_2 = 65535;
        public ushort Sound_3 = 65535;
        public ushort Sound_4 = 65535;
        public ushort Particle_3 = 65535;
        public ushort Sound_5 = 65535;
        public ushort Sound_6 = 65535;
        public float UnkFloat1 = 0.2f;
        public float UnkFloat2 = -1f;
        public float UnkFloat3 = -1f;
        public float UnkFloat4 = 0.25f;
        public float UnkFloat5 = 0.15f;
        public float UnkFloat6 = 1000000f;
        public float UnkFloat7 = 1f;
        public float UnkFloat8 = 1f;
        public float UnkFloat9;
        public float UnkFloat10;
        public Pos UnkVector = new Pos(0, 0, 0, 1);
        public Pos UnkBoundingBox1 = new Pos(0, 0, 0, 0);
        public Pos UnkBoundingBox2 = new Pos(0, 0, 0, 0);

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Flags);
            writer.Write(SurfaceID);
            writer.Write(Sound_1);
            writer.Write(Sound_2);
            writer.Write(Particle_1);
            writer.Write(Particle_2);
            writer.Write(Sound_3);
            writer.Write(Sound_4);
            writer.Write(Particle_3);
            writer.Write(Sound_5);
            writer.Write(Sound_6);
            writer.Write(65535);
            writer.Write(UnkFloat1);
            writer.Write(UnkFloat2);
            writer.Write(UnkFloat3);
            writer.Write(UnkFloat4);
            writer.Write(UnkFloat5);
            writer.Write(UnkFloat6);
            writer.Write(UnkFloat7);
            writer.Write(UnkFloat8);
            writer.Write(UnkFloat9);
            writer.Write(UnkFloat10);
            writer.Write(UnkVector.X);
            writer.Write(UnkVector.Y);
            writer.Write(UnkVector.Z);
            writer.Write(UnkVector.W);
            writer.Write(UnkBoundingBox1.X);
            writer.Write(UnkBoundingBox1.Y);
            writer.Write(UnkBoundingBox1.Z);
            writer.Write(UnkBoundingBox1.W);
            writer.Write(UnkBoundingBox2.X);
            writer.Write(UnkBoundingBox2.Y);
            writer.Write(UnkBoundingBox2.Z);
            writer.Write(UnkBoundingBox2.W);
        }

        public override void Load(BinaryReader reader, int size)
        {
            Flags = reader.ReadUInt32();
            SurfaceID = reader.ReadUInt16();
            Sound_1 = reader.ReadUInt16();
            Sound_2 = reader.ReadUInt16();
            Particle_1 = reader.ReadUInt16();
            Particle_2 = reader.ReadUInt16();
            Sound_3 = reader.ReadUInt16();
            Sound_4 = reader.ReadUInt16();
            Particle_3 = reader.ReadUInt16();
            Sound_5 = reader.ReadUInt16();
            Sound_6 = reader.ReadUInt16();
            reader.ReadUInt16(); // not stored by the game, just skipped
            UnkFloat1 = reader.ReadSingle();
            UnkFloat2 = reader.ReadSingle();
            UnkFloat3 = reader.ReadSingle();
            UnkFloat4 = reader.ReadSingle();
            UnkFloat5 = reader.ReadSingle();
            UnkFloat6 = reader.ReadSingle();
            UnkFloat7 = reader.ReadSingle();
            UnkFloat8 = reader.ReadSingle();
            UnkFloat9 = reader.ReadSingle();
            UnkFloat10 = reader.ReadSingle();
            UnkVector = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            UnkBoundingBox1 = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            UnkBoundingBox2 = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        protected override int GetSize()
        {
            return 114;
        }
    }
}
