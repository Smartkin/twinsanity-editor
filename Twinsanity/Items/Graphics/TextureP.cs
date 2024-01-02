using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Twinsanity
{
    public class TextureP : TwinsItem
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

        public int Width { get => 1 << w; }
        public int Height { get => 1 << h; }
        public int MipLevels { get => m - 1; }
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

            // PSP specific stuff
            HeaderAdd = reader.ReadBytes(0x2C);

            // PSP texture
            imageData = reader.ReadBytes(Width * Height);
            var Palette = new List<Color>();
            for (int i = 0; i < 256; i++)
            {
                byte R = reader.ReadByte();
                byte G = reader.ReadByte();
                byte B = reader.ReadByte();
                byte A = reader.ReadByte();
                Palette.Add(Color.FromArgb(A, R, G, B));
            }

            byte[] Decompressed = new byte[imageData.Length * 4];
            Decompressed = PSP_UnSwizzle(Width, Height, 8, imageData);

            RawData = new Color[Width * Height];
            int c = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    RawData[c] = Palette[Decompressed[c]];
                    c++;
                }
            }

            ItemSize = size;
            reader.BaseStream.Position = pre_pos;
            Data = reader.ReadBytes(size);
        }

        public Bitmap GetBmp()
        {
            Bitmap BMP = new Bitmap(Convert.ToInt32(Width), Convert.ToInt32(Height));
            for (int i = 0; i < RawData.Length; i++)
                BMP.SetPixel((i % Width), (i / Width), RawData[i]);
            return BMP;
        }

        protected override int GetSize()
        {
            return (int)ItemSize;
        }

        public byte[] PSP_Swizzle(int inwidth, int height, int bpp, byte[] texData)
        {
            int offset = 0;
            int width = (inwidth * bpp) >> 3;
            byte[] destination = new byte[width * height];
            int rowblocks = (width / 16);
            int magicNumber = 8;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int blockX = x / 16;
                    int blockY = y / magicNumber;

                    int blockIndex = blockX + ((blockY) * rowblocks);
                    int blockAddress = blockIndex * 16 * magicNumber;

                    int destinationOffset = blockAddress + (x - blockX * 16) + ((y - blockY * magicNumber) * 16);

                    destination[destinationOffset] = texData[offset];
                    offset++;
                }
            }

            return destination;
        }

        public byte[] PSP_UnSwizzle(int inwidth, int height, int bpp, byte[] texData)
        {
            int destinationOffset = 0;
            int width = (inwidth * bpp) >> 3;
            byte[] destination = new byte[width * height];
            int rowblocks = (width / 16);
            int magicNumber = 8;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int blockX = x / 16;
                    int blockY = y / magicNumber;

                    int blockIndex = blockX + ((blockY) * rowblocks);
                    int blockAddress = blockIndex * 16 * magicNumber;
                    int offset = blockAddress + (x - blockX * 16) + ((y - blockY * magicNumber) * 16);
                    destination[destinationOffset] = texData[offset];
                    destinationOffset++;
                }
            }

            return destination;
        }

    }
}
