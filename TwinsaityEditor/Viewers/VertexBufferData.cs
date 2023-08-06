﻿using OpenTK.Graphics.OpenGL;
using System;

namespace TwinsaityEditor
{
    public class VertexBufferData
    {
        public int ID { get; set; }
        public int LastSize { get; set; }
        public bool Textured { get; set; }
        public int[] VtxOffs { get; set; }
        public int[] VtxCounts { get; set; }
        public uint[] VtxInd { get; set; }
        public Vertex[] Vtx { get; set; }
        public BufferPointerFlags Flags { get; set; }
        public int Texture { get; set; }

        public VertexBufferData()
        {
            ID = GL.GenBuffer();
            LastSize = 0;
            Texture = -1;
            Textured = true;
        }

        public void DrawAll(PrimitiveType primitive_type, BufferPointerFlags flags, bool useOnlyFlagsParam = false)
        {
            if (!useOnlyFlagsParam)
            {
                draw_func(0, primitive_type, flags | Flags);
            }
            else
            {
                draw_func(0, primitive_type, flags);
            }
        }

        public void DrawAllElements(PrimitiveType primitive_type, BufferPointerFlags flags, bool useOnlyFlagsParam = false)
        {
            if (!useOnlyFlagsParam)
            {
                draw_func(1, primitive_type, flags | Flags);
            }
            else
            {
                draw_func(1, primitive_type, flags);
            }
        }

        public void DrawMulti(PrimitiveType primitive_type, BufferPointerFlags flags, bool useOnlyFlagsParam = false)
        {
            if (!useOnlyFlagsParam)
            {
                draw_func(2, primitive_type, flags | Flags);
            }
            else
            {
                draw_func(2, primitive_type, flags);
            }
        }

        private void draw_func(int func, PrimitiveType prim, BufferPointerFlags flags)
        {
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, ID);
            if (Texture != -1 && Textured)
            {
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, Texture);
            }
            GL.VertexPointer(3, VertexPointerType.Float, Vertex.SizeOf, Vertex.OffsetOfPos);
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
            if ((flags & BufferPointerFlags.Color) != 0)
            {
                GL.EnableClientState(ArrayCap.ColorArray);
                GL.ColorPointer(4, ColorPointerType.UnsignedByte, Vertex.SizeOf, Vertex.OffsetOfCol);
            }
            switch (func)
            {
                case 0:
                    GL.DrawArrays(prim, 0, Vtx.Length);
                    break;
                case 1:
                    if (VtxInd != null)
                    {
                        GL.DrawElements(prim, VtxInd.Length, DrawElementsType.UnsignedInt, VtxInd);
                    }
                    break;
                case 2:
                    GL.MultiDrawArrays(prim, VtxOffs, VtxCounts, VtxCounts.Length);
                    break;
            }
            GL.Flush();
            if ((flags & BufferPointerFlags.Normal) != 0)
                GL.DisableClientState(ArrayCap.NormalArray);
            if ((flags & BufferPointerFlags.TexCoord) != 0)
                GL.DisableClientState(ArrayCap.TextureCoordArray);
            if ((flags & BufferPointerFlags.Color) != 0)
                GL.DisableClientState(ArrayCap.ColorArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            if (Texture != -1 && Textured)
            {
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
        }
    }

    [Flags]
    public enum BufferPointerFlags
    {
        None = 0,
        NormalNoCol = 1,
        TexCoordNoCol = 2,
        Color = 4,
        Normal,
        TexCoord,
        Default = Color
    }
}
