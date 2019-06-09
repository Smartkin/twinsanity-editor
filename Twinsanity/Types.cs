namespace Twinsanity
{
    public class Pos
    {
        public Pos(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public float[] ToArray()
        {
            return new float[4] { X, Y, Z, W };
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }
    }

    public class CamRot
    {
        public CamRot(ushort pitch, ushort yaw, ushort roll)
        {
            Pitch = pitch;
            Yaw = yaw;
            Roll = roll;
        }

        public ushort Pitch { get; set; }
        public ushort Yaw { get; set; }
        public ushort Roll { get; set; }
    }
}
