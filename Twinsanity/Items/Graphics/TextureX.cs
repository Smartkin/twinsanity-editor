using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Twinsanity
{
    public class TextureX : TwinsItem
    {

        private int texSize;
        private int unkInt;
        private short w, h;
        private byte m;
        private byte format;
        private byte destinationFormat;
        private byte texColComponent; // 0 - RGB, 1 - RGBA
        private byte unkByte;
        private byte textureFun;
        private byte[] unkBytes = new byte[2];
        private int textureBasePointer;
        private int[] mipLevelsTBP;
        private int textureBufferWidth;
        private int[] mipLevelsTBW;
        private int clutBufferBasePointer;
        private byte[] unkBytes2 = new byte[8];
        private byte[] unkBytes3 = new byte[2];
        private byte[] unusedMetadata = new byte[32]; // This metadata is read but discarded afterwards in game's code
        private byte[] vifBlock = new byte[96]; // This holds a VIF code block, which does some basic texture setup, we don't care about it
        private byte[] imageData;
        //private Color[] palette;
        //private Color[][] mips;
        //private int rrw;
        //private int rrh;

        public int Width { get => 1 << w; }
        public int Height { get => 1 << h; }
        public int MipLevels { get => m - 1; }
        //public TexturePixelFormat PixelFormat { get => (TexturePixelFormat)format; }
        //public TexturePixelFormat DestinationPixelFormat { get => (TexturePixelFormat)destinationFormat; }
        //public TextureColorComponent ColorComponent { get => (TextureColorComponent)texColComponent; }
        //public TextureFunction TexFun { get => (TextureFunction)textureFun; }
        public Color[] RawData { get; set; }
        public int TextureBufferWidth { get => textureBufferWidth * 64; }

        public byte[] HeaderAdd;
        public uint TextureType;
        public long ItemSize { get; set; }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Data);
        }

        public override void Load(BinaryReader reader, int size)
        {
            long pre_pos = reader.BaseStream.Position;

            texSize = reader.ReadInt32();
            // Texture header
            unkInt = reader.ReadInt32();
            w = reader.ReadInt16();
            h = reader.ReadInt16();
            m = reader.ReadByte();
            format = reader.ReadByte();
            destinationFormat = reader.ReadByte();
            texColComponent = reader.ReadByte();
            unkByte = reader.ReadByte();
            textureFun = reader.ReadByte();
            unkBytes = reader.ReadBytes(2);
            textureBasePointer = reader.ReadInt32();
            mipLevelsTBP = new int[6];
            for (var i = 0; i < 6; ++i)
            {
                mipLevelsTBP[i] = reader.ReadInt32();
            }
            textureBufferWidth = reader.ReadInt32();
            mipLevelsTBW = new int[6];
            for (var i = 0; i < 6; ++i)
            {
                mipLevelsTBW[i] = reader.ReadInt32();
            }
            clutBufferBasePointer = reader.ReadInt32();
            unkBytes2 = reader.ReadBytes(8);

            // Xbox specific stuff
            HeaderAdd = reader.ReadBytes(0x2C);
            TextureType = reader.ReadUInt32();

            
            if (TextureType != 0)
            {
                // BC3/DXT5 compression
                imageData = reader.ReadBytes(Width * Height);
                byte[] Decompressed = new byte[imageData.Length * 4];
                DecompressDXT5(imageData, Width, Height, Decompressed);

                RawData = new Color[Width * Height];
                int b = 0;
                int c = 0;
                for (int y = Height - 1; y >= 0; y--)
                {
                    c = (Width * y);
                    for (int x = 0; x < Width; x++)
                    {
                        RawData[c + x] = Color.FromArgb(Decompressed[b + 3], Decompressed[b + 2], Decompressed[b + 1], Decompressed[b + 0]);
                        b += 4;
                    }
                }

                ItemSize = size;
                reader.BaseStream.Position = pre_pos;
                Data = reader.ReadBytes(size);
            }
            else
            {
                // Uncompressed pixels (PSM)
                imageData = new byte[0];
                RawData = new Color[Width * Height];
                int c = 0;
                for (int y = Height - 1; y >= 0; y--)
                {
                    c = (Width * y);
                    for (int x = 0; x < Width; x++)
                    {
                        byte[] clr = reader.ReadBytes(4);
                        RawData[c + x] = Color.FromArgb(clr[3], clr[2], clr[1], clr[0]);
                    }
                }

                if (unkBytes2[4] != 0x84)
                {
                    reader.ReadBytes(unkBytes2[4] - 0x84); // 0x1C
                }
            }
        }

        protected override int GetSize()
        {
            return (int)ItemSize;
        }

        public static void DecompressDXT1(byte[] input, int width, int height, byte[] output)
        {
            int offset = 0;
            int bcw = (width + 3) / 4;
            int bch = (height + 3) / 4;
            int clen_last = (width + 3) % 4 + 1;
            uint[] buffer = new uint[16];
            int[] colors = new int[4];
            for (int t = 0; t < bch; t++)
            {
                for (int s = 0; s < bcw; s++, offset += 8)
                {
                    int r0, g0, b0, r1, g1, b1;
                    int q0 = input[offset + 0] | input[offset + 1] << 8;
                    int q1 = input[offset + 2] | input[offset + 3] << 8;
                    Rgb565(q0, out r0, out g0, out b0);
                    Rgb565(q1, out r1, out g1, out b1);
                    colors[0] = TColor(r0, g0, b0, 255);
                    colors[1] = TColor(r1, g1, b1, 255);
                    if (q0 > q1)
                    {
                        colors[2] = TColor((r0 * 2 + r1) / 3, (g0 * 2 + g1) / 3, (b0 * 2 + b1) / 3, 255);
                        colors[3] = TColor((r0 + r1 * 2) / 3, (g0 + g1 * 2) / 3, (b0 + b1 * 2) / 3, 255);
                    }
                    else
                    {
                        colors[2] = TColor((r0 + r1) / 2, (g0 + g1) / 2, (b0 + b1) / 2, 255);
                    }

                    uint d = BitConverter.ToUInt32(input, offset + 4);
                    for (int i = 0; i < 16; i++, d >>= 2)
                    {
                        buffer[i] = unchecked((uint)colors[d & 3]);
                    }

                    int clen = (s < bcw - 1 ? 4 : clen_last) * 4;
                    for (int i = 0, y = t * 4; i < 4 && y < height; i++, y++)
                    {
                        Buffer.BlockCopy(buffer, i * 4 * 4, output, (y * width + s * 4) * 4, clen);
                    }
                }
            }
        }

        public static void DecompressDXT3(byte[] input, int width, int height, byte[] output)
        {
            int offset = 0;
            int bcw = (width + 3) / 4;
            int bch = (height + 3) / 4;
            int clen_last = (width + 3) % 4 + 1;
            uint[] buffer = new uint[16];
            int[] colors = new int[4];
            int[] alphas = new int[16];
            for (int t = 0; t < bch; t++)
            {
                for (int s = 0; s < bcw; s++, offset += 16)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        int alpha = input[offset + i * 2] | input[offset + i * 2 + 1] << 8;
                        alphas[i * 4 + 0] = (((alpha >> 0) & 0xF) * 0x11) << 24;
                        alphas[i * 4 + 1] = (((alpha >> 4) & 0xF) * 0x11) << 24;
                        alphas[i * 4 + 2] = (((alpha >> 8) & 0xF) * 0x11) << 24;
                        alphas[i * 4 + 3] = (((alpha >> 12) & 0xF) * 0x11) << 24;
                    }

                    int r0, g0, b0, r1, g1, b1;
                    int q0 = input[offset + 8] | input[offset + 9] << 8;
                    int q1 = input[offset + 10] | input[offset + 11] << 8;
                    Rgb565(q0, out r0, out g0, out b0);
                    Rgb565(q1, out r1, out g1, out b1);
                    colors[0] = TColor(r0, g0, b0, 0);
                    colors[1] = TColor(r1, g1, b1, 0);
                    if (q0 > q1)
                    {
                        colors[2] = TColor((r0 * 2 + r1) / 3, (g0 * 2 + g1) / 3, (b0 * 2 + b1) / 3, 0);
                        colors[3] = TColor((r0 + r1 * 2) / 3, (g0 + g1 * 2) / 3, (b0 + b1 * 2) / 3, 0);
                    }
                    else
                    {
                        colors[2] = TColor((r0 + r1) / 2, (g0 + g1) / 2, (b0 + b1) / 2, 0);
                    }

                    uint d = BitConverter.ToUInt32(input, offset + 12);
                    for (int i = 0; i < 16; i++, d >>= 2)
                    {
                        buffer[i] = unchecked((uint)(colors[d & 3] | alphas[i]));
                    }

                    int clen = (s < bcw - 1 ? 4 : clen_last) * 4;
                    for (int i = 0, y = t * 4; i < 4 && y < height; i++, y++)
                    {
                        Buffer.BlockCopy(buffer, i * 4 * 4, output, (y * width + s * 4) * 4, clen);
                    }
                }
            }
        }

        public static void DecompressDXT5(byte[] input, int width, int height, byte[] output)
        {
            int offset = 0;
            int bcw = (width + 3) / 4;
            int bch = (height + 3) / 4;
            int clen_last = (width + 3) % 4 + 1;
            uint[] buffer = new uint[16];
            int[] colors = new int[4];
            int[] alphas = new int[8];
            for (int t = 0; t < bch; t++)
            {
                for (int s = 0; s < bcw; s++, offset += 16)
                {
                    alphas[0] = input[offset + 0];
                    alphas[1] = input[offset + 1];
                    if (alphas[0] > alphas[1])
                    {
                        alphas[2] = (alphas[0] * 6 + alphas[1]) / 7;
                        alphas[3] = (alphas[0] * 5 + alphas[1] * 2) / 7;
                        alphas[4] = (alphas[0] * 4 + alphas[1] * 3) / 7;
                        alphas[5] = (alphas[0] * 3 + alphas[1] * 4) / 7;
                        alphas[6] = (alphas[0] * 2 + alphas[1] * 5) / 7;
                        alphas[7] = (alphas[0] + alphas[1] * 6) / 7;
                    }
                    else
                    {
                        alphas[2] = (alphas[0] * 4 + alphas[1]) / 5;
                        alphas[3] = (alphas[0] * 3 + alphas[1] * 2) / 5;
                        alphas[4] = (alphas[0] * 2 + alphas[1] * 3) / 5;
                        alphas[5] = (alphas[0] + alphas[1] * 4) / 5;
                        alphas[7] = 255;
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        alphas[i] <<= 24;
                    }

                    int r0, g0, b0, r1, g1, b1;
                    int q0 = input[offset + 8] | input[offset + 9] << 8;
                    int q1 = input[offset + 10] | input[offset + 11] << 8;
                    Rgb565(q0, out r0, out g0, out b0);
                    Rgb565(q1, out r1, out g1, out b1);
                    colors[0] = TColor(r0, g0, b0, 0);
                    colors[1] = TColor(r1, g1, b1, 0);
                    if (q0 > q1)
                    {
                        colors[2] = TColor((r0 * 2 + r1) / 3, (g0 * 2 + g1) / 3, (b0 * 2 + b1) / 3, 0);
                        colors[3] = TColor((r0 + r1 * 2) / 3, (g0 + g1 * 2) / 3, (b0 + b1 * 2) / 3, 0);
                    }
                    else
                    {
                        colors[2] = TColor((r0 + r1) / 2, (g0 + g1) / 2, (b0 + b1) / 2, 0);
                    }

                    ulong da = BitConverter.ToUInt64(input, offset) >> 16;
                    uint dc = BitConverter.ToUInt32(input, offset + 12);
                    for (int i = 0; i < 16; i++, da >>= 3, dc >>= 2)
                    {
                        buffer[i] = unchecked((uint)(alphas[da & 7] | colors[dc & 3]));
                    }

                    int clen = (s < bcw - 1 ? 4 : clen_last) * 4;
                    for (int i = 0, y = t * 4; i < 4 && y < height; i++, y++)
                    {
                        Buffer.BlockCopy(buffer, i * 4 * 4, output, (y * width + s * 4) * 4, clen);
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Rgb565(int c, out int r, out int g, out int b)
        {
            r = (c & 0xf800) >> 8;
            g = (c & 0x07e0) >> 3;
            b = (c & 0x001f) << 3;
            r |= r >> 5;
            g |= g >> 6;
            b |= b >> 5;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int TColor(int r, int g, int b, int a)
        {
            return r << 16 | g << 8 | b | a << 24;
        }
    }
}
