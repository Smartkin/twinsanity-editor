using OpenTK.Graphics.OpenGL;
using System;

namespace TwinsaityEditor
{
    public class VertexBufferData
    {
        public int ID { get; set; }
        public int LastSize { get; set; }
        public int[] VtxOffs { get; set; }
        public int[] VtxCounts { get; set; }
        public uint[] VtxInd { get; set; }
        public Vertex[] Vtx { get; set; }

        public VertexBufferData()
        {
            ID = GL.GenBuffer();
            LastSize = 0;
        }

        public void DrawAll(PrimitiveType primitive_type, BufferPointerFlags flags)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, ID);
            GL.VertexPointer(3, VertexPointerType.Float, Vertex.SizeOf, Vertex.OffsetOfPos);
            GL.ColorPointer(4, ColorPointerType.UnsignedByte, Vertex.SizeOf, Vertex.OffsetOfCol);
            if ((flags & BufferPointerFlags.Normal) != 0)
            {
                GL.EnableClientState(ArrayCap.NormalArray);
                GL.NormalPointer(NormalPointerType.Float, Vertex.SizeOf, Vertex.OffsetOfNor);
            }
            if ((flags & BufferPointerFlags.TexCoord) != 0)
            {
                GL.EnableClientState(ArrayCap.TextureCoordArray);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex.SizeOf, Vertex.OffsetOfTex);
            }
            GL.DrawArrays(primitive_type, 0, Vtx.Length);
            if ((flags & BufferPointerFlags.Normal) != 0)
                GL.DisableClientState(ArrayCap.NormalArray);
            if ((flags & BufferPointerFlags.TexCoord) != 0)
                GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void DrawAllElements(PrimitiveType primitive_type, BufferPointerFlags flags)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, ID);
            GL.VertexPointer(3, VertexPointerType.Float, Vertex.SizeOf, Vertex.OffsetOfPos);
            GL.ColorPointer(4, ColorPointerType.UnsignedByte, Vertex.SizeOf, Vertex.OffsetOfCol);
            if ((flags & BufferPointerFlags.Normal) != 0)
            {
                GL.EnableClientState(ArrayCap.NormalArray);
                GL.NormalPointer(NormalPointerType.Float, Vertex.SizeOf, Vertex.OffsetOfNor);
            }
            if ((flags & BufferPointerFlags.TexCoord) != 0)
            {
                GL.EnableClientState(ArrayCap.TextureCoordArray);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex.SizeOf, Vertex.OffsetOfTex);
            }
            GL.DrawElements(primitive_type, VtxInd.Length, DrawElementsType.UnsignedInt, VtxInd);
            if ((flags & BufferPointerFlags.Normal) != 0)
                GL.DisableClientState(ArrayCap.NormalArray);
            if ((flags & BufferPointerFlags.TexCoord) != 0)
                GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void DrawMulti(PrimitiveType primitive_type, BufferPointerFlags flags)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, ID);
            GL.VertexPointer(3, VertexPointerType.Float, Vertex.SizeOf, Vertex.OffsetOfPos);
            GL.ColorPointer(4, ColorPointerType.UnsignedByte, Vertex.SizeOf, Vertex.OffsetOfCol);
            if ((flags & BufferPointerFlags.Normal) != 0)
            {
                GL.EnableClientState(ArrayCap.NormalArray);
                GL.NormalPointer(NormalPointerType.Float, Vertex.SizeOf, Vertex.OffsetOfNor);
            }
            if ((flags & BufferPointerFlags.TexCoord) != 0)
            {
                GL.EnableClientState(ArrayCap.TextureCoordArray);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex.SizeOf, Vertex.OffsetOfTex);
            }
            GL.MultiDrawArrays(primitive_type, VtxOffs, VtxCounts, VtxCounts.Length);
            if ((flags & BufferPointerFlags.Normal) != 0)
                GL.DisableClientState(ArrayCap.NormalArray);
            if ((flags & BufferPointerFlags.TexCoord) != 0)
                GL.DisableClientState(ArrayCap.TextureCoordArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
    }

    [Flags]
    public enum BufferPointerFlags
    {
        None = 0,
        Normal = 1,
        TexCoord = 2,
    }
}
