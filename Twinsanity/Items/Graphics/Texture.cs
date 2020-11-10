using System.IO;
using System;
using System.Drawing;
using Twinsanity.Properties;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace Twinsanity
{
    /// <summary>
    /// Represents Twinsanity's Texture
    /// </summary>
    public class Texture : TwinsItem
    {
        private int texSize;
        private int unkInt;
        private short w, h;
        private byte m;
        private byte format;
        private int type;
        private byte destinationFormat;
        private byte texColComponent; // 0 - RGB, 1 - RGBA
        private byte unkByte;
        private byte textureFun;
        private byte[] unkBytes = new byte[2];
        private int textureBasePointer;
        private int mipLevel1TBP;
        private int mipLevel2TBP;
        private int mipLevel3TBP;
        private int mipLevel4TBP;
        private int mipLevel5TBP;
        private int mipLevel6TBP;
        private int textureBufferWidth;
        private int mipLevel1TBW;
        private int mipLevel2TBW;
        private int mipLevel3TBW;
        private int mipLevel4TBW;
        private int mipLevel5TBW;
        private int mipLevel6TBW;
        private int clutBufferBasePointer;
        private byte[] unkBytes2 = new byte[8];
        private byte[] unkBytes3 = new byte[2];
        private byte[] unusedMetadata = new byte[32]; // This metadata is read but discarded afterwards in game's code
        private byte[] vifBlock = new byte[96]; // This holds a VIF code block, which does some basic texture setup, we don't care about it
        private byte[] imageData;
        private Color[] palette;

        public int Width { get => 1 << w; }
        public int Height { get => 1 << h; }
        public int MipLevels { get => m; }
        public TexturePixelFormat PixelFormat { get => (TexturePixelFormat)format; }
        public TexturePixelFormat DestinationPixelFormat { get => (TexturePixelFormat)destinationFormat; }
        public TextureColorComponent ColorComponent { get => (TextureColorComponent)texColComponent; }
        public TextureFunction TexFun { get => (TextureFunction)textureFun; }
        public Color[] RawData { get; private set; }
        public int TextureBufferWidth { get => textureBufferWidth * 64; }

        public override void Load(BinaryReader reader, int size)
        {
            texSize = reader.ReadInt32();
            var allTexDataPos = reader.BaseStream.Position;
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
            mipLevel1TBP = reader.ReadInt32();
            mipLevel2TBP = reader.ReadInt32();
            mipLevel3TBP = reader.ReadInt32();
            mipLevel4TBP = reader.ReadInt32();
            mipLevel5TBP = reader.ReadInt32();
            mipLevel6TBP = reader.ReadInt32();
            textureBufferWidth = reader.ReadInt32();
            mipLevel1TBW = reader.ReadInt32();
            mipLevel2TBW = reader.ReadInt32();
            mipLevel3TBW = reader.ReadInt32();
            mipLevel4TBW = reader.ReadInt32();
            mipLevel5TBW = reader.ReadInt32();
            mipLevel6TBW = reader.ReadInt32();
            clutBufferBasePointer = reader.ReadInt32();
            unkBytes2 = reader.ReadBytes(8);
            reader.ReadInt32(); // Reserved, in game's code refers to an index of vifCodeBlock
            reader.ReadInt32(); // Reserved, in game's code refers to an unknown pointer
            unkBytes3 = reader.ReadBytes(2);
            reader.ReadBytes(2); // Reserved, unknown
            // The rest of data
            unusedMetadata = reader.ReadBytes(32);
            vifBlock = reader.ReadBytes(96);
            var afterVifPos = reader.BaseStream.Position;
            reader.BaseStream.Position -= 96;
            reader.ReadBytes(48);
            var rrw = reader.ReadInt32();
            var rrh = reader.ReadInt32();
            reader.BaseStream.Position = afterVifPos;
            //imageData = reader.ReadBytes(texSize - 224);
            // TODO: Implement proper formats readers
            switch(PixelFormat)
            {
                case TexturePixelFormat.PSMCT32: // Easiest one, raw color data
                    imageData = reader.ReadBytes(texSize - 224);
                    RawData = new Color[Width * Height];
                    var pxInd = 0;
                    for (var i = 0; i < texSize - 224; i += 4)
                    {
                        var r = imageData[i];
                        var g = imageData[i + 1];
                        var b = imageData[i + 2];
                        var a = (byte)(imageData[i + 3] << 1);
                        RawData[pxInd++] = Color.FromArgb(a, r, g, b);
                    }
                    break;
                case TexturePixelFormat.PSMT8: // End me
                    imageData = reader.ReadBytes(texSize - 224);
                    var dumpPath = @"D:\Twinsanity Discs\ScriptModding_Tests\tex_raw_data_extract\";
                    var texMeta = ID.ToString("X") + "_" + Width + "x" + Height;
                    
                    reader.BaseStream.Position = allTexDataPos;
                    var rawTex = reader.ReadBytes(texSize);
                    var rawTexDump = File.Create(dumpPath + "rawTex_" + texMeta, texSize);
                    rawTexDump.Write(rawTex, 0, texSize);
                    rawTexDump.Close();
                    EzSwizzle.InitGs();
                    EzSwizzle.cleanGs();
                    EzSwizzle.writeTexPSMCT32_mod(0, 1, 0, 0, rrw, rrh, imageData);
                    EzSwizzle.dumpMemory(dumpPath + "gsDump_ct32_" + texMeta, false, dumpPath + "gsDumpVisual_" + texMeta + ".bmp");
                    //EzSwizzle.dumpMemoryRearranged(dumpPath + "gsDump_Rearranged_ct32_" + texMeta);
                    /*EzSwizzle.cleanGs();
                    EzSwizzle.writeTexPSMCT32(0, 1, 0, 0, rrw, rrh, imageData);*/
                    // Main texture image data extraction (just indices)
                    var texData = new byte[Width * Height];
                    EzSwizzle.readTexPSMT8(0, textureBufferWidth, 0, 0, Width, Height, ref texData);
                    /*if (MipLevels > 1)
                    {
                        var mipLevel1 = new byte[(Width / 2) * (Height / 2)];
                        EzSwizzle.readTexPSMT8(mipLevel1TBP, mipLevel1TBW, 0, 0, rrw / 2, rrh / 2, ref mipLevel1);
                        var mipDataDump = File.Create(@"D:\Twinsanity Discs\ScriptModding_Tests\tex_raw_data_extract\mip_1_" + ID.ToString("X") + "_" + Width / 2 + "x" + Height / 2, (Width / 2) * (Height / 2));
                        mipDataDump.Write(mipLevel1, 0, (Width / 2) * (Height / 2));
                        mipDataDump.Close();
                    }*/
                    // Palette extraction
                    var palette = new byte[256 * 4];
                    EzSwizzle.readTexPSMCT32(clutBufferBasePointer, 1, 0, 0, 16, 16, ref palette);
                    this.palette = new Color[16 * 16];
                    var palInd = 0;
                    for (var i = 0; i < 256 * 4; i += 4)
                    {
                        var r = palette[i];
                        var g = palette[i + 1];
                        var b = palette[i + 2];
                        var a = (byte)(palette[i + 3] << 1);
                        this.palette[palInd] = Color.FromArgb(a, r, g, b);
                    }

                    break;
                default:
                    imageData = reader.ReadBytes(texSize - 224);
                    break;
            }
        }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(texSize);
            writer.Write(unkInt);
            writer.Write(w);
            writer.Write(h);
            writer.Write(m);
            writer.Write(format);
            writer.Write(destinationFormat);
            writer.Write(texColComponent);
            writer.Write(unkByte);
            writer.Write(textureFun);
            writer.Write(unkBytes);
            writer.Write(textureBasePointer);
            writer.Write(mipLevel1TBP);
            writer.Write(mipLevel2TBP);
            writer.Write(mipLevel3TBP);
            writer.Write(mipLevel4TBP);
            writer.Write(mipLevel5TBP);
            writer.Write(mipLevel6TBP);
            writer.Write(textureBufferWidth);
            writer.Write(mipLevel1TBW);
            writer.Write(mipLevel2TBW);
            writer.Write(mipLevel3TBW);
            writer.Write(mipLevel4TBW);
            writer.Write(mipLevel5TBW);
            writer.Write(mipLevel6TBW);
            writer.Write(clutBufferBasePointer);
            writer.Write(unkBytes2);
            writer.Write(0);
            writer.Write(0);
            writer.Write(unkBytes3);
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write(unusedMetadata);
            writer.Write(vifBlock);
            writer.Write(imageData);
        }

        protected override int GetSize()
        {
            return texSize + 4;
        }

        #region INTERNALS
        private byte[] UnSwizzle(byte[] indexes, ushort width, ushort height)
        {
            byte[] tmp = new byte[width * height];
            indexes.CopyTo(tmp, 0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var block_location = (x + y) % 256;
                }
            }
            return tmp;
        }
        internal void Swizzle(ref byte[] indexes, ushort width, ushort height)
        {
            byte[] tmp = new byte[indexes.Length - 1 + 1];
            indexes.CopyTo(tmp, 0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int block_location = (y & (~0xF)) * width + (x & (~0xF)) * 2;
                    int swap_selector = (((y + 2) >> 2) & 0x1) * 4;
                    int posY = (((y & (~3)) >> 1) + (y & 1)) & 0x7;
                    int posX = posY * width * 2 + ((x + swap_selector) & 0x7) * 4;
                    var byte_num = ((y >> 1) & 1) + ((x >> 2) & 2);
                    indexes[Math.Min(block_location + posX + byte_num, tmp.Length - 1)] = tmp[Math.Min((y * width) + x, indexes.Length - 1)];
                }
            }
        }
        internal void Flip(ref byte[] Indexes, ushort width, ushort height)
        {
            height = (ushort)(Indexes.Length / width);
            for (uint y = 0; y <= height / (double)2 - 1; y++)
            {
                for (uint x = 0; x <= width - 1; x++)
                {
                    byte tmp = Indexes[y * width + x];
                    Indexes[y * width + x] = Indexes[(height - y - 1) * width + x];
                    Indexes[(height - y - 1) * width + x] = tmp;
                }
            }
        }
        internal byte SwapBit(byte num, byte b1, byte b2)
        {
            byte mask = (byte)(1 << b1 + 1 << b2);
            byte new_num = (byte)(num & (~mask));
            byte swap = (byte)(((num & (1 << b1)) >> b1) << b2 + ((num & (1 << b2)) >> b2) << b1);
            return (byte)(swap + new_num);
        }
        internal void SwapPalette(ref Color[] palette)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 8 + i * 32; j < 16 + i * 32; j++)
                {
                    Color tmp = palette[j];
                    palette[j] = palette[j + 8];
                    palette[j + 8] = tmp;
                }
            }
        }
        
        internal void GenerateMips(byte[] pixels, uint Width, uint Height, ref byte[] mippixels)
        {
            byte mips = (byte)(Math.Log(Width, 2) - 2);
            byte[][] tmp = new byte[pixels.Length][];
            Array.Resize(ref tmp[0], pixels.Length);
            pixels.CopyTo(tmp[0], 0);
            Array.Resize(ref mippixels, (int)(Width * Height / 2));
            ushort w, h, yoffset = 0;
            w = (ushort)Width;
            h = (ushort)Height;
            for (int i = 0; i < mippixels.Length; i++)
                mippixels[i] = 255;
            for (int i = 1; i <= mips; i++)
            {
                w /= 2;
                h /= 2;
                MakeMip(tmp[i - 1], ref tmp[i]);
                Swizzle(ref tmp[i], w, h);
                Flip(ref tmp[i], w, h);
                MergeSurfaces(Width / 2, Height, w, h, 0, yoffset, ref mippixels, ref tmp[i]);
            }
        }

        internal void MakeMip(byte[] pixels, ref byte[] mippixels)
        {
            Array.Resize(ref mippixels, pixels.Length / 2);
            for (int i = 0; i <= pixels.Length - 1; i += 4)
                mippixels[i / 2] = pixels[i];
        }

        internal void MergeSurfaces(uint Width, uint Height, uint MWidth, uint MHeigth, uint x, uint y, ref byte[] pixels, ref byte[] mippixels)
        {
            for (int i = 0; i <= MHeigth - 1; i++)
            {
                for (int j = 0; j <= MWidth - 1; j++)
                    pixels[i * Width + j] = mippixels[i * MWidth + j];
            }
        }

        internal void InitFMT(ref BlockFormat[][] fmt)
        {
            Array.Resize(ref fmt, 19);
            // fmt128x256 128I256 16P64S192 16S256
            Array.Resize(ref fmt[0], 160);
            for (int i = 0; i < 128; i++)
                fmt[0][i] = new BlockFormat(256, 0, 0, 0);
            for (int i = 128; i <= 143; i++)
                fmt[0][i] = new BlockFormat(0, 0, 64, 192);
            for (int i = 144; i <= 159; i++)
                fmt[0][i] = new BlockFormat(0, 0, 0, 256);
            // fmt128x128 64I256 16P64S192 16S256
            Array.Resize(ref fmt[1], 96);
            for (int i = 0; i < 64; i++)
                fmt[1][i] = new BlockFormat(256, 0, 0, 0);
            for (int i = 64; i <= 79; i++)
                fmt[1][i] = new BlockFormat(0, 0, 64, 192);
            for (int i = 80; i <= 95; i++)
                fmt[1][i] = new BlockFormat(0, 0, 0, 256);
            // fmt128x128mip 64I256 16M192P64 16S256 
            Array.Resize(ref fmt[2], 96);
            for (int i = 0; i < 64; i++)
                fmt[2][i] = new BlockFormat(256, 0, 0, 0);
            for (int i = 64; i <= 79; i++)
                fmt[2][i] = new BlockFormat(0, 192, 64, 0);
            for (int i = 80; i <= 95; i++)
                fmt[2][i] = new BlockFormat(0, 0, 0, 256);
            // fmt128x64 32I256 16P64S192 16S256
            Array.Resize(ref fmt[3], 64);
            for (int i = 0; i < 32; i++)
                fmt[3][i] = new BlockFormat(256, 0, 0, 0);
            for (int i = 32; i <= 47; i++)
                fmt[3][i] = new BlockFormat(0, 0, 64, 192);
            for (int i = 48; i < 64; i++)
                fmt[3][i] = new BlockFormat(0, 0, 0, 256);
            // fmt128x64mip 32I256 16M64S192 16M64P64S128
            Array.Resize(ref fmt[4], 64);
            for (int i = 0; i < 32; i++)
                fmt[4][i] = new BlockFormat(256, 0, 0, 0);
            for (int i = 32; i <= 47; i++)
                fmt[4][i] = new BlockFormat(0, 64, 0, 192);
            for (int i = 48; i < 64; i++)
                fmt[4][i] = new BlockFormat(0, 64, 64, 128);
            // fmt128x32 16I256 16P64S192 16S256
            Array.Resize(ref fmt[5], 48);
            for (int i = 0; i < 16; i++)
                fmt[5][i] = new BlockFormat(256, 0, 0, 0);
            for (int i = 16; i < 32; i++)
                fmt[5][i] = new BlockFormat(0, 0, 64, 192);
            for (int i = 32; i <= 47; i++)
                fmt[5][i] = new BlockFormat(0, 0, 0, 256);
            // fmt128x32mip 16I256 16M128P64S64 16S256
            Array.Resize(ref fmt[6], 48);
            for (int i = 0; i < 16; i++)
                fmt[6][i] = new BlockFormat(256, 0, 0, 0);
            for (int i = 16; i < 32; i++)
                fmt[6][i] = new BlockFormat(0, 128, 64, 64);
            for (int i = 32; i <= 47; i++)
                fmt[6][i] = new BlockFormat(0, 0, 0, 256);
            // fmt64x64 16I128P64S64 16I128S128
            Array.Resize(ref fmt[7], 32);
            for (int i = 0; i < 16; i++)
                fmt[7][i] = new BlockFormat(128, 0, 64, 64);
            for (int i = 16; i < 32; i++)
                fmt[7][i] = new BlockFormat(128, 0, 0, 128);
            // fmt64x64mip 16I128M64P64 16I128M64S64
            Array.Resize(ref fmt[8], 32);
            for (int i = 0; i < 16; i++)
                fmt[8][i] = new BlockFormat(128, 0, 64, 64);
            for (int i = 16; i < 32; i++)
                fmt[8][i] = new BlockFormat(128, 64, 0, 64);
            // fmt64x32 16I128S128 16P64S192
            Array.Resize(ref fmt[9], 32);
            for (int i = 0; i < 16; i++)
                fmt[9][i] = new BlockFormat(128, 0, 0, 128);
            for (int i = 16; i < 32; i++)
                fmt[9][i] = new BlockFormat(0, 0, 64, 192);
            // fmt64x32mip 16I128S128 16M64P64S128
            Array.Resize(ref fmt[10], 32);
            for (int i = 0; i < 16; i++)
                fmt[10][i] = new BlockFormat(128, 0, 0, 128);
            for (int i = 16; i < 32; i++)
                fmt[10][i] = new BlockFormat(0, 64, 64, 128);
            // fmt32x64 16I64P64S128 16I64S192
            Array.Resize(ref fmt[11], 32);
            for (int i = 0; i < 16; i++)
                fmt[11][i] = new BlockFormat(64, 0, 64, 128);
            for (int i = 16; i < 32; i++)
                fmt[11][i] = new BlockFormat(64, 0, 0, 192);
            // fmt32x64mip 16I64M64P64S64 16I64S192
            Array.Resize(ref fmt[12], 32);
            for (int i = 0; i < 16; i++)
                fmt[12][i] = new BlockFormat(64, 64, 64, 64);
            for (int i = 16; i < 32; i++)
                fmt[12][i] = new BlockFormat(64, 0, 0, 192);
            // fmt32x32 16I64S192 16P64S192
            Array.Resize(ref fmt[13], 32);
            for (int i = 0; i < 16; i++)
                fmt[13][i] = new BlockFormat(64, 0, 0, 192);
            for (int i = 16; i < 32; i++)
                fmt[13][i] = new BlockFormat(0, 0, 64, 192);
            // fmt32x32mip 16I64P64S128 16M32S224
            Array.Resize(ref fmt[14], 32);
            for (int i = 0; i < 16; i++)
                fmt[14][i] = new BlockFormat(64, 0, 64, 128);
            for (int i = 16; i < 32; i++)
                fmt[14][i] = new BlockFormat(0, 32, 0, 224);
            // fmt32x16 8I64S192 16P64S192 8S256 EXPEREMENTAL!
            Array.Resize(ref fmt[15], 32);
            for (int i = 0; i < 8; i++)
                fmt[15][i] = new BlockFormat(64, 0, 0, 192);
            for (int i = 8; i < 16; i++)
                fmt[15][i] = new BlockFormat(0, 0, 64, 224);
            for (int i = 16; i < 32; i++)
                fmt[15][i] = new BlockFormat(0, 0, 0, 256);
            // fmt32x16mip 8I64S192 4M32S224 4S256 16P64S192
            Array.Resize(ref fmt[16], 32);
            for (int i = 0; i < 8; i++)
                fmt[16][i] = new BlockFormat(64, 0, 0, 192);
            for (int i = 8; i <= 11; i++)
                fmt[16][i] = new BlockFormat(0, 32, 0, 224);
            for (int i = 12; i < 16; i++)
                fmt[16][i] = new BlockFormat(0, 0, 0, 256);
            for (int i = 16; i < 32; i++)
                fmt[16][i] = new BlockFormat(0, 0, 64, 192);
            // fmt32x8 4I64S192 12S256 16P64S192
            Array.Resize(ref fmt[17], 32);
            for (int i = 0; i < 4; i++)
                fmt[17][i] = new BlockFormat(64, 0, 0, 192);
            for (int i = 4; i < 16; i++)
                fmt[17][i] = new BlockFormat(0, 0, 0, 256);
            for (int i = 16; i < 32; i++)
                fmt[17][i] = new BlockFormat(0, 0, 64, 192);
            // fmt16x16mip 8I32S224 4M32S224 4S256 16P64
            Array.Resize(ref fmt[18], 32);
            for (int i = 0; i < 8; i++)
                fmt[18][i] = new BlockFormat(32, 0, 0, 224);
            for (int i = 8; i <= 11; i++)
                fmt[18][i] = new BlockFormat(0, 32, 0, 224);
            for (int i = 12; i < 16; i++)
                fmt[18][i] = new BlockFormat(0, 0, 0, 256);
            for (int i = 16; i < 32; i++)
                fmt[18][i] = new BlockFormat(0, 0, 64, 192);
        }
        #endregion

        #region STRUCTURES
        public struct BlockFormat
        {
            public ushort pixels;
            public ushort m;
            public ushort palette;
            public ushort Space;
            public BlockFormat(ushort pIndex, ushort pMip, ushort pPalette, ushort pSpace)
            {
                pixels = pIndex;
                m = pMip;
                palette = pPalette;
                Space = pSpace;
            }
        }
        
        public enum TexturePixelFormat
        {
            PSMCT32     = 0b000000,
            PSMCT24     = 0b000001,
            PSMCT16     = 0b000010,
            PSMCT16S    = 0b001010,
            PSMT8       = 0b010011,
            PSMT4       = 0b010100,
            PSMT8H      = 0b011011,
            PSMT4HL     = 0b100100,
            PSMT4HH     = 0b101100,
            PSMZ32      = 0b110000,
            PSMZ24      = 0b110001,
            PSMZ16      = 0b110010,
            PSMZ16S     = 0b111010
        }
        public enum TextureColorComponent
        {
            RGB = 0,
            RGBA = 1
        }
        public enum TextureFunction
        {
            MODULATE = 0b00,
            DECAL = 0b01,
            HIGHLIGHT = 0b10,
            HIGHLIGHT2 = 0b11
        }
        #endregion
    }
}
