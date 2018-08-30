namespace Twinsanity
{
    public struct Pos
    {
        private float x, y, z, w;

        public Pos(float x, float y, float z, float w) : this()
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }
    }
}
