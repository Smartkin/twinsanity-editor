using OpenTK;
using System.Drawing;
using System.Runtime.InteropServices;

namespace TwinsaityEditor
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        private Vector3 pos, nor;
        private uint col;

        public Vertex(Vector3 pos, Vector3 nor, Color col)
        {
            this.pos = pos;
            this.nor = nor;
            this.col = col.R | (uint)col.G << 8 | (uint)col.B << 16 | (uint)col.A << 24;
        }

        public static int SizeOf { get; } = Marshal.SizeOf(typeof(Vertex));
        public static System.IntPtr OffsetOfPos { get; } = Marshal.OffsetOf(typeof(Vertex), "pos");
        public static System.IntPtr OffsetOfNor { get; } = Marshal.OffsetOf(typeof(Vertex), "nor");
        public static System.IntPtr OffsetOfCol { get; } = Marshal.OffsetOf(typeof(Vertex), "col");

        public Vector3 Pos { get => pos; set => pos = value; }
        public Vector3 Nor { get => nor; set => nor = value; }
        public uint Col { get => col; set => col = value; }
    }

    public enum TextAnchor { TopLeft, TopMiddle, TopRight, BotLeft, BotMiddle, BotRight };
    public enum Editors
    {
        ChunkLinks,
        Position,
        Path,
        Instance,
        Trigger
    };
}
