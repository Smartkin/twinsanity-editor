using System.IO;
using System;
using System.Drawing;
using Twinsanity.Properties;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Twinsanity
{
    /// <summary>
    /// Represents Twinsanity's Texture
    /// </summary>
    public class Texture : TwinsItem
    {
        Int32 Size;
        Int32 Signature;
        Int16 W;
        Int16 H;
        Byte Mips;
        Byte Palette;
        Int16 PaletteSize;

        List<Int32> header_leftover;

        Int32 typeIndex = 8;

        List<Byte[]> blocks;

        public int Width { get => 1 << W; }
        public int Height { get => 1 << H; }

        public void ChangeState(bool UnSwizzlePic, bool SwizzlePic, bool FlipPic, bool Fix)
        {
            /*
            if (type == 4)
                return;
            if (UnSwizzlePic)
                UnSwizzle(ref pixels, (ushort)Width, (ushort)Height);
            else if (SwizzlePic)
                Swizzle(ref pixels, (ushort)Width, (ushort)Height);
            if (FlipPic)
                Flip(ref pixels, (ushort)Width, (ushort)Height);
            if (Fix)
                SwapPalette(ref palette);
            for (int i = 0; i <= pixel_data.Length - 1; i++)
                pixel_data[i] = palette[pixels[i]];
            */
        }

        public void Import(Color[] pixel_data, uint pwidth, uint pheight, BlockFormats format, bool m)
        {
            throw new NotImplementedException();
            /*byte[] pixels = new byte[] { };
            Color[] palette = new Color[] { };
            byte[] mippixels = new byte[] { };
            ARGB2INDEX(pixel_data, ref pixels, ref palette);
            Flip(ref pixels, (ushort)pwidth, (ushort)pheight);
            Swizzle(ref pixels, (ushort)pwidth, (ushort)pheight);
            SwapPalette(ref palette);
            if (m)
                GenerateMips(pixels, pwidth, pheight, ref mippixels);
            MemoryStream Data = new MemoryStream();
            MemoryStream Header = new MemoryStream();
            MakeInterleave(ref Data, pixels, palette, mippixels, format);
            FormHeader(ref Header, format);
            MemoryStream NewStream = new MemoryStream();
            BinaryWriter NSWriter = new BinaryWriter(NewStream);
            NSWriter.Write(Header.ToArray());
            NSWriter.Write(Data.ToArray());
            FileStream File = new FileStream(@"C:\N", FileMode.Create, FileAccess.Write);
            BinaryWriter Writer = new BinaryWriter(File);
            Writer.Write(NewStream.ToArray());
            Writer.Close();
            File.Close();
            Size = (uint)NewStream.Length;
            DataUpdate();*/
        }

        public void ARGB2INDEX(Color[] pixel_data, ref byte[] pixels, ref Color[] palette)
        {
            Array.Resize(ref palette, 256);
            Array.Resize(ref pixels, pixel_data.Length);
            int cnt = -1;
            for (int n = 0; n <= pixel_data.Length - 1; n++)
            {
                Color c = pixel_data[n];
                bool flag = true;
                for (int i = 0; i <= cnt; i++)
                {
                    if (c == palette[i])
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    cnt += 1;
                    palette[cnt] = c;
                }
                if (cnt == 255)
                    break;
            }
            for (int n = 0; n <= pixel_data.Length - 1; n++)
            {
                float MinUnMath = -1.0F;
                byte MaxIndex = 0;
                Color c = pixel_data[n];
                bool flag = false;
                for (int i = 0; i < 256; i++)
                {
                    Color p = palette[i];
                    float cUnMath = Math.Abs(System.Convert.ToInt32(p.A) - System.Convert.ToInt32(c.A)) + Math.Abs(System.Convert.ToInt32(p.R) - System.Convert.ToInt32(c.R)) + Math.Abs(System.Convert.ToInt32(p.G) - System.Convert.ToInt32(c.G)) + Math.Abs(System.Convert.ToInt32(p.B) - System.Convert.ToInt32(c.B));
                    if (c == p)
                    {
                        flag = true;
                        pixels[n] = (byte)i;
                        break;
                    }
                    if ((cUnMath < MinUnMath) || MinUnMath == -1.0F)
                    {
                        MaxIndex = (byte)i;
                        MinUnMath = cUnMath;
                    }
                }
                if (!flag)
                    pixels[n] = MaxIndex;
            }
        }

        public override void Load(BinaryReader reader, int size)
        {
            header_leftover = new List<int>();
            blocks = new List<byte[]>();
            Size = reader.ReadInt32();
            Signature = reader.ReadInt32();
            W = reader.ReadInt16();
            H = reader.ReadInt16();
            Mips = reader.ReadByte();
            Palette = reader.ReadByte();
            PaletteSize = reader.ReadInt16();
            for (int i = 0; i < 53; ++i)
            {
                header_leftover.Add(reader.ReadInt32());
            }
            Int32 blocksCnt = (Size - 0xE0) / 0x100;
            for (int i = 0; i < blocksCnt; ++i)
            {
                blocks.Add(reader.ReadBytes(0x100));
            }
        }
        public override void Save(BinaryWriter writer)
        {
            long start = writer.BaseStream.Position;
            writer.Write(Size);
            writer.Write(Signature);
            writer.Write(W);
            writer.Write(H);
            writer.Write(Mips);
            writer.Write(Palette);
            writer.Write(PaletteSize);
            for (int i = 0; i < 53; ++i)
            {
                writer.Write(header_leftover[i]);
            }
            Int32 blocksCnt = (Size - 0xE0) / 0x100;
            for (int i = 0; i < blocksCnt; ++i)
            {
                writer.Write(blocks[i]);
            }
        }

        protected override int GetSize()
        {
            return Size + 4;
        }

        #region INTERNALS
        internal void UnSwizzle(ref byte[] indexes, ushort width, ushort height)
        {
            byte[] tmp = new byte[indexes.Length];
            indexes.CopyTo(tmp, 0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int block_location = (y & ~0xF) * width + (x & ~0xF) * 2;
                    int swap_selector = (((y + 2) >> 2) & 0x1) * 4;
                    int posY = (((y & ~3) >> 1) + (y & 1)) & 0x7;
                    int posX = posY * width * 2 + ((x + swap_selector) & 0x7) * 4;
                    var byte_num = ((y >> 1) & 1) + ((x >> 2) & 2);
                    indexes[Math.Min(y * width + x, indexes.Length - 1)] = tmp[Math.Min(block_location + posX + byte_num, tmp.Length - 1)];
                }
            }
        }
        internal void Swizzle(ref byte[] indexes, ushort width, ushort height)
        {
            byte[] tmp = new byte[indexes.Length - 1 + 1];
            indexes.CopyTo(tmp, 0);
            for (int y = 0; y <= height - 1; y++)
            {
                for (int x = 0; x <= width - 1; x++)
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

        internal void MakeInterleave(ref MemoryStream Data, byte[] pixels, Color[] palette, byte[] mippixels, BlockFormats Format)
        {
            BlockFormat[][] fmt = new BlockFormat[][] { };
            InitFMT(ref fmt);
            Data = new MemoryStream();
            BinaryWriter DWriter = new BinaryWriter(Data);
            uint ind_offset = 0;
            uint mip_offset = 0;
            uint pal_offset = 0;
            var format_ind = (int)Format;
            for (int i = 0; i <= fmt[format_ind].Length - 1; i++)
            {
                for (int j = 0; j <= fmt[format_ind][i].pixels - 1; j++)
                    DWriter.Write(pixels[j + ind_offset]);
                for (int j = 0; j <= fmt[format_ind][i].m - 1; j++)
                    DWriter.Write(mippixels[j + mip_offset]);
                for (int j = 0; j <= fmt[format_ind][i].palette / 4 - 1; j++)
                {
                    DWriter.Write(palette[j + pal_offset].R);
                    DWriter.Write(palette[j + pal_offset].G);
                    DWriter.Write(palette[j + pal_offset].B);
                    DWriter.Write(palette[j + pal_offset].A);
                }
                for (int j = 0; j <= fmt[format_ind][i].Space - 1; j++)
                    DWriter.Write(System.Convert.ToByte(255));
                ind_offset += fmt[format_ind][i].pixels;
                mip_offset += fmt[format_ind][i].m;
                pal_offset += (uint)fmt[format_ind][i].palette / 4;
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

        internal void FormHeader(ref MemoryStream Header, BlockFormats fmt)
        {
            Header = new MemoryStream(228);
            BinaryWriter DWriter = new BinaryWriter(Header);
            Header.Position = 0;
            switch (fmt)
            {
                case BlockFormats.fmt128x256:
                    {
                        
                        DWriter.Write(Resources._128x256);
                        break;
                    }

                case BlockFormats.fmt128x128:
                    {
                        DWriter.Write(Resources._128x128);
                        break;
                    }

                case BlockFormats.fmt128x128mip:
                    {
                        DWriter.Write(Resources._128x128mip);
                        break;
                    }

                case BlockFormats.fmt128x64:
                    {
                        break;
                    }

                case BlockFormats.fmt128x64mip:
                    {
                        DWriter.Write(Resources._128x64mip);
                        break;
                    }

                case BlockFormats.fmt128x32:
                    {
                        break;
                    }

                case BlockFormats.fmt128x32mip:
                    {
                        DWriter.Write(Resources._128x32mip);
                        break;
                    }

                case BlockFormats.fmt64x64:
                    {
                        DWriter.Write(Resources._64x64);
                        break;
                    }

                case BlockFormats.fmt64x64mip:
                    {
                        DWriter.Write(Resources._64x64mip);
                        break;
                    }

                case BlockFormats.fmt64x32:
                    {
                        break;
                    }

                case BlockFormats.fmt64x32mip:
                    {
                        DWriter.Write(Resources._64x32mip);
                        break;
                    }

                case BlockFormats.fmt32x64:
                    {
                        DWriter.Write(Resources._32x64);
                        break;
                    }

                case BlockFormats.fmt32x64mip:
                    {
                        DWriter.Write(Resources._32x64mip);
                        break;
                    }

                case BlockFormats.fmt32x32:
                    {
                        break;
                    }

                case BlockFormats.fmt32x32mip:
                    {
                        DWriter.Write(Resources._32x32mip);
                        break;
                    }

                case BlockFormats.fmt32x16:
                    {
                        break;
                    }

                case BlockFormats.fmt32x16mip:
                    {
                        DWriter.Write(Resources._32x16mip);
                        break;
                    }

                case BlockFormats.fmt32x8:
                    {
                        DWriter.Write(Resources._32x8);
                        break;
                    }

                case BlockFormats.fmt16x16:
                    {
                        break;
                    }

                case BlockFormats.fmt16x16mip:
                    {
                        DWriter.Write(Resources._16x16mip);
                        break;
                    }
            }
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
        public enum BlockFormats
        {
            fmt128x256 = 0,
            fmt128x128 = 1,
            fmt128x128mip = 2,
            fmt128x64 = 3,
            fmt128x64mip = 4,
            fmt128x32 = 5,
            fmt128x32mip = 6,
            fmt64x64 = 7,
            fmt64x64mip = 8,
            fmt64x32 = 9,
            fmt64x32mip = 10,
            fmt32x64 = 11,
            fmt32x64mip = 12,
            fmt32x32 = 13,
            fmt32x32mip = 14,
            fmt32x16 = 15,
            fmt32x16mip = 16,
            fmt32x8 = 17,
            fmt16x16 = 18,
            fmt16x16mip = 19
        }
        #endregion
    }
}
/*
 * 
 * var n = reader.ReadInt32();
            reader.BaseStream.Position -= 4;
            Data = reader.ReadBytes(n + 4);
            reader.BaseStream.Position -= n;
            var key = reader.ReadUInt32();
            w = reader.ReadInt16();
            h = reader.ReadInt16();
            m = reader.ReadByte();
            pal_flag = reader.ReadByte();
            pal_size = reader.ReadUInt16();
            tex_space = reader.ReadBytes(32);
            type = reader.ReadInt32();
            unk = reader.ReadBytes(176);
            switch (type)
            {
                case 1:
                    {
                        pixels = new byte[Width * Height];
                        mippixels = new byte[Width / 2 * Height];
                        palette = new Color[(n & 0xFFFFFF00) >> 4];
                        pixel_data = new Color[Width * Height];
                        if ((Width == 32) && (Height == 8))
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                for (int j = 0; j < 64; j++)
                                    pixels[j + i * 64] = reader.ReadByte();
                                reader.BaseStream.Position += 192;
                            }
                            reader.BaseStream.Position += 12 * 256;
                            for (int i = 0; i < 16; i++)
                            {
                                for (int j = 0; j < 16; j++)
                                {
                                    byte a, r, g, b;
                                    r = reader.ReadByte();
                                    g = reader.ReadByte();
                                    b = reader.ReadByte();
                                    a = (byte)(reader.ReadByte() << 1);
                                    palette[j + i * 16] = Color.FromArgb(a, r, g, b);
                                }
                                reader.BaseStream.Position += 192;
                            }
                        }
                        else if ((Width == 16) && (Height == 16))
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                for (int j = 0; j < 32; j++)
                                    pixels[j + i * 32] = reader.ReadByte();
                                reader.BaseStream.Position += 224;
                            }
                            for (int i = 0; i < 4; i++)
                            {
                                for (int j = 0; j < 32; j++)
                                    mippixels[j + i * 32] = reader.ReadByte();
                                reader.BaseStream.Position += 224;
                            }
                            reader.BaseStream.Position += 4 * 256;
                            for (int i = 0; i < 16; i++)
                            {
                                for (int j = 0; j < 16; j++)
                                {
                                    byte a, r, g, b;
                                    r = reader.ReadByte();
                                    g = reader.ReadByte();
                                    b = reader.ReadByte();
                                    a = (byte)(reader.ReadByte() << 1);
                                    palette[j + i * 16] = Color.FromArgb(a, r, g, b);
                                }
                                reader.BaseStream.Position += 192;
                            }
                        }
                        else if ((Width == 32) && (Height == 16))
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                for (int j = 0; j < 64; j++)
                                    pixels[j + i * 64] = reader.ReadByte();
                                reader.BaseStream.Position += 192;
                            }
                            for (int i = 0; i < 8; i++)
                            {
                                for (int j = 0; j < 32; j++)
                                    mippixels[j + i * 32] = reader.ReadByte();
                                reader.BaseStream.Position += 224;
                            }
                            for (int i = 0; i < 16; i++)
                            {
                                for (int j = 0; j < 16; j++)
                                {
                                    byte a, r, g, b;
                                    r = reader.ReadByte();
                                    g = reader.ReadByte();
                                    b = reader.ReadByte();
                                    a = (byte)(reader.ReadByte() << 1);
                                    palette[j + i * 16] = Color.FromArgb(a, r, g, b);
                                }
                                reader.BaseStream.Position += 192;
                            }
                        }
                        else if ((Width == 32) && (Height == 32))
                        {
                            if (m == 1)
                            {
                                for (int i = 0; i < 16; i++)
                                {
                                    for (int j = 0; j < 64; j++)
                                        pixels[j + i * 64] = reader.ReadByte();
                                    reader.BaseStream.Position += 192;
                                }
                                for (int i = 0; i < 16; i++)
                                {
                                    for (int j = 0; j < 16; j++)
                                    {
                                        byte a, r, g, b;
                                        r = reader.ReadByte();
                                        g = reader.ReadByte();
                                        b = reader.ReadByte();
                                        a = (byte)(reader.ReadByte() << 1);
                                        palette[j + i * 16] = Color.FromArgb(a, r, g, b);
                                    }
                                    reader.BaseStream.Position += 192;
                                }
                            }
                            else
                            {
                                for (int i = 0; i < 16; i++)
                                {
                                    for (int j = 0; j < 64; j++)
                                        pixels[j + i * 64] = reader.ReadByte();
                                    for (int j = 0; j < 16; j++)
                                    {
                                        byte a, r, g, b;
                                        r = reader.ReadByte();
                                        g = reader.ReadByte();
                                        b = reader.ReadByte();
                                        a = (byte)(reader.ReadByte() << 1);
                                        palette[j + i * 16] = Color.FromArgb(a, r, g, b);
                                    }
                                    reader.BaseStream.Position += 128;
                                }
                                for (int i = 0; i < 16; i++)
                                {
                                    for (int j = 0; j < 32; j++)
                                        mippixels[j + i * 32] = reader.ReadByte();
                                    reader.BaseStream.Position += 224;
                                }
                            }
                        }
                        else if ((Width == 32) && (Height == 64))
                        {
                            if (m == 1)
                            {
                                for (int i = 0; i < 32; i++)
                                {
                                    for (int j = 0; j < 64; j++)
                                        pixels[j + i * 64] = reader.ReadByte();
                                    for (int j = 0; j < 16; j++)
                                    {
                                        byte a, r, g, b;
                                        r = reader.ReadByte();
                                        g = reader.ReadByte();
                                        b = reader.ReadByte();
                                        a = (byte)(reader.ReadByte() << 1);
                                        palette[j + i * 16] = Color.FromArgb(a, r, g, b);
                                    }
                                    reader.BaseStream.Position += 128;
                                }
                            }
                            else
                            {
                                for (int i = 0; i < 16; i++)
                                {
                                    for (int j = 0; j < 64; j++)
                                        pixels[j + i * 64] = reader.ReadByte();
                                    for (int j = 0; j < 64; j++)
                                        mippixels[j + i * 64] = reader.ReadByte();
                                    for (int j = 0; j < 16; j++)
                                    {
                                        byte a, r, g, b;
                                        r = reader.ReadByte();
                                        g = reader.ReadByte();
                                        b = reader.ReadByte();
                                        a = (byte)(reader.ReadByte() << 1);
                                        palette[j + i * 16] = Color.FromArgb(a, r, g, b);
                                    }
                                    reader.BaseStream.Position += 64;
                                }
                                for (int i = 0; i < 16; i++)
                                {
                                    for (int j = 0; j < 64; j++)
                                        pixels[1024 + j + i * 64] = reader.ReadByte();
                                    reader.BaseStream.Position += 64;
                                    for (int j = 0; j < 16; j++)
                                    {
                                        byte a, r, g, b;
                                        r = reader.ReadByte();
                                        g = reader.ReadByte();
                                        b = reader.ReadByte();
                                        a = (byte)(reader.ReadByte() << 1);
                                        palette[256 + j + i * 16] = Color.FromArgb(a, r, g, b);
                                    }
                                    reader.BaseStream.Position += 64;
                                }
                            }
                        }
                        else if ((Width == 64) && (Height == 32))
                        {
                            if (m == 1)
                            {
                                for (int i = 0; i < 16; i++)
                                {
                                    for (int j = 0; j < 128; j++)
                                        pixels[j + i * 128] = reader.ReadByte();
                                    reader.BaseStream.Position += 128;
                                }
                                for (int i = 0; i < 16; i++)
                                {
                                    for (int j = 0; j < 16; j++)
                                    {
                                        byte a, r, g, b;
                                        r = reader.ReadByte();
                                        g = reader.ReadByte();
                                        b = reader.ReadByte();
                                        a = (byte)(reader.ReadByte() << 1);
                                        palette[j + i * 16] = Color.FromArgb(a, r, g, b);
                                    }
                                    reader.BaseStream.Position += 192;
                                }
                            }
                            else
                            {
                                for (int i = 0; i < 16; i++)
                                {
                                    for (int j = 0; j < 128; j++)
                                        pixels[j + i * 128] = reader.ReadByte();
                                    reader.BaseStream.Position += 128;
                                }
                                for (int i = 0; i < 16; i++)
                                {
                                    for (int j = 0; j < 64; j++)
                                        mippixels[j + i * 64] = reader.ReadByte();
                                    for (int j = 0; j < 16; j++)
                                    {
                                        byte a, r, g, b;
                                        r = reader.ReadByte();
                                        g = reader.ReadByte();
                                        b = reader.ReadByte();
                                        a = (byte)(reader.ReadByte() << 1);
                                        palette[j + i * 16] = Color.FromArgb(a, r, g, b);
                                    }
                                    reader.BaseStream.Position += 128;
                                }
                            }
                        }
                        else if ((Width == 64) && (Height == 64))
                        {
                            if (m == 1)
                            {
                                for (int i = 0; i < 32; i++)
                                {
                                    for (int j = 0; j < 128; j++)
                                        pixels[j + i * 128] = reader.ReadByte();
                                    for (int j = 0; j < 16; j++)
                                    {
                                        byte a, r, g, b;
                                        r = reader.ReadByte();
                                        g = reader.ReadByte();
                                        b = reader.ReadByte();
                                        a = (byte)(reader.ReadByte() << 1);
                                        palette[j + i * 16] = Color.FromArgb(a, r, g, b);
                                    }
                                    reader.BaseStream.Position += 64;
                                }
                            }
                            else
                                for (int i = 0; i < 32; i++)
                                {
                                    for (int j = 0; j < 128; j++)
                                        pixels[j + i * 128] = reader.ReadByte();
                                    for (int j = 0; j < 64; j++)
                                        mippixels[j + i * 64] = reader.ReadByte();
                                    for (int j = 0; j < 16; j++)
                                    {
                                        byte a, r, g, b;
                                        r = reader.ReadByte();
                                        g = reader.ReadByte();
                                        b = reader.ReadByte();
                                        a = (byte)(reader.ReadByte() << 1);
                                        palette[j + i * 16] = Color.FromArgb(a, r, g, b);
                                    }
                                }
                        }
                        //else
                        //    System.Windows.Forms.MessageBox.Show("ID:" + ID + " Width: " + Width + " Height: " + Height, "Kesha, we have a problem!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        UnSwizzle(ref pixels, (ushort)Width, (ushort)Height);
                        UnSwizzle(ref mippixels, (ushort)(Width / 2), (ushort)Height);
                        Flip(ref pixels, (ushort)Width, (ushort)Height);
                        SwapPalette(ref palette);
                        for (int i = 0; i <= pixel_data.Length - 1; i++)
                            pixel_data[i] = palette[pixels[i]];
                        break;
                    }

                case 2:
                    {
                        Array.Resize(ref pixels, Width * Height);
                        Array.Resize(ref mippixels, Width / 2 * Height);
                        Array.Resize(ref palette, 512);
                        if ((Width == 128) && (Height == 32))
                        {
                            pixels = reader.ReadBytes(4096);
                            for (int i = 0; i < 16; i++)
                            {
                                for (int j = 0; j < 128; j++)
                                    mippixels[j + i * 128] = reader.ReadByte();
                                for (int j = 0; j < 16; j++)
                                {
                                    byte a, r, g, b;
                                    r = reader.ReadByte();
                                    g = reader.ReadByte();
                                    b = reader.ReadByte();
                                    a = (byte)(reader.ReadByte() << 1);
                                    palette[j + i * 16] = Color.FromArgb(a, r, g, b);
                                }
                                reader.BaseStream.Position += 64;
                            }
                        }
                        else if ((Width == 128) && (Height == 64))
                        {
                            pixels = reader.ReadBytes(8192);
                            for (int i = 0; i < 16; i++)
                            {
                                for (int j = 0; j < 64; j++)
                                    mippixels[j + i * 64] = reader.ReadByte();
                                reader.BaseStream.Position += 192;
                            }
                            for (int i = 0; i < 16; i++)
                            {
                                for (int j = 0; j < 64; j++)
                                    mippixels[512 + j + i * 64] = reader.ReadByte();
                                for (int j = 0; j < 16; j++)
                                {
                                    byte a, r, g, b;
                                    r = reader.ReadByte();
                                    g = reader.ReadByte();
                                    b = reader.ReadByte();
                                    a = (byte)(reader.ReadByte() << 1);
                                    palette[j + i * 16] = Color.FromArgb(a, r, g, b);
                                }
                                reader.BaseStream.Position += 128;
                            }
                        }
                        else if ((Width == 128) && (Height == 128))
                        {
                            pixels = reader.ReadBytes(16384);
                            if (m > 1)
                            {
                                for (int i = 0; i < 32; i++)
                                {
                                    for (int j = 0; j <= 191; j++)
                                        mippixels[j + i * 192] = reader.ReadByte();
                                    for (int j = 0; j < 16; j++)
                                    {
                                        byte a, r, g, b;
                                        r = reader.ReadByte();
                                        g = reader.ReadByte();
                                        b = reader.ReadByte();
                                        a = (byte)(reader.ReadByte() << 1);
                                        palette[j + i * 16] = Color.FromArgb(a, r, g, b);
                                    }
                                }
                            }
                            else
                                for (int i = 0; i < 32; i++)
                                {
                                    for (int j = 0; j < 16; j++)
                                    {
                                        byte a, r, g, b;
                                        r = reader.ReadByte();
                                        g = reader.ReadByte();
                                        b = reader.ReadByte();
                                        a = (byte)(reader.ReadByte() << 1);
                                        palette[j + i * 16] = Color.FromArgb(a, r, g, b);
                                    }
                                    reader.BaseStream.Position += 192;
                                }
                        }
                        else if ((Width == 128) && (Height == 256))
                        {
                            pixels = reader.ReadBytes(32768);
                            for (int i = 0; i < 32; i++)
                            {
                                for (int j = 0; j < 16; j++)
                                {
                                    byte a, r, g, b;
                                    r = reader.ReadByte();
                                    g = reader.ReadByte();
                                    b = reader.ReadByte();
                                    a = (byte)(reader.ReadByte() << 1);
                                    palette[j + i * 16] = Color.FromArgb(a, r, g, b);
                                }
                                reader.BaseStream.Position += 192;
                            }
                        }
                        //else
                        //    System.Windows.Forms.MessageBox.Show("ID:" + ID + " Width: " + Width + " Height: " + Height, "Kesha, we have a problem! Abort now!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        UnSwizzle(ref pixels, (ushort)Width, (ushort)Height);
                        Flip(ref pixels, (ushort)Width, (ushort)Height);
                        SwapPalette(ref palette);
                        Array.Resize(ref pixel_data, pixels.Length);
                        for (int i = 0; i <= pixel_data.Length - 1; i++)
                            pixel_data[i] = palette[pixels[i]];
                        break;
                    }

                case 4:
                    {
                        Array.Resize(ref pixel_data, (int)(Width * Height));
                        for (int i = 0; i <= (Width) * Height - 1; i++)
                        {
                            byte a, r, g, b;
                            r = reader.ReadByte();
                            g = reader.ReadByte();
                            b = reader.ReadByte();
                            a = (byte)(reader.ReadByte() << 1);
                            pixel_data[i] = Color.FromArgb(a, r, g, b);
                        }

                        break;
                    }
            }

*/