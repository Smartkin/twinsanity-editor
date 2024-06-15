using System.IO;

namespace Twinsanity
{
    public class CollisionSurface : TwinsItem
    {
        public byte[] Flags = new byte[4]{
            0xF0,0xF0,0x1F,0x00
        }; //4
        public ushort SurfaceID;
        public ushort[] SoundIDs = new ushort[10] {
            65535, 65535, 65535, 65535, 65535, 65535, 65535, 65535, 65535, 65535,
        }; //10
        public Pos[] Floats = new Pos[4] {
            new Pos(0.2f,-1f,-1f,0.25f),
            new Pos(0.15f,1000000f,1f,1f),
            new Pos(0,0,0,0),
            new Pos(0,1f,0,0)
        }; //4
        public ushort[] UnkInts = new ushort[12]{
            0, 0, 0, 0, 0, 0, 52480, 52685, 52685, 52685, 52685, 52685
        }; //12

        public override void Save(BinaryWriter writer)
        {
            for (int i = 0; i < Flags.Length; i++)
            {
                writer.Write(Flags[i]);
            }
            writer.Write(SurfaceID);
            for (int i = 0; i < SoundIDs.Length; i++)
            {
                writer.Write(SoundIDs[i]);
            }
            for (int i = 0; i < Floats.Length; i++)
            {
                writer.Write(Floats[i].X);
                writer.Write(Floats[i].Y);
                writer.Write(Floats[i].Z);
                writer.Write(Floats[i].W);
            }
            for (int i = 0; i < UnkInts.Length; i++)
            {
                writer.Write(UnkInts[i]);
            }
        }

        public override void Load(BinaryReader reader, int size)
        {
            Flags = new byte[4];
            for (int i = 0; i < Flags.Length; i++)
            {
                Flags[i] = reader.ReadByte();
            }
            SurfaceID = reader.ReadUInt16();
            SoundIDs = new ushort[10];
            for (int i = 0; i < SoundIDs.Length; i++)
            {
                SoundIDs[i] = reader.ReadUInt16();
            }
            Floats = new Pos[4];
            for (int i = 0; i < Floats.Length; i++)
            {
                Floats[i] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            }
            UnkInts = new ushort[12];
            for (int i = 0; i < UnkInts.Length; i++)
            {
                UnkInts[i] = reader.ReadUInt16();
            }
        }

        protected override int GetSize()
        {
            return 114;
        }
    }
}
