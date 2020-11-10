using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace Twinsanity
{
	public static class EzSwizzle
	{
		static byte[] gs = new byte[1024 * 1024 * 4];

		#region Constants
		static readonly int[] block32 = new int[32] {
			 0,  1,  4,  5, 16, 17, 20, 21,
			 2,  3,  6,  7, 18, 19, 22, 23,
			 8,  9, 12, 13, 24, 25, 28, 29,
			10, 11, 14, 15, 26, 27, 30, 31
		};

		static readonly int[][] blockTable32 = new int[4][]
		{
			new int[8] { 0,  1,  4,  5, 16, 17, 20, 21 },
			new int[8] { 2,  3,  6,  7, 18, 19, 22, 23 },
			new int[8] { 8,  9, 12, 13, 24, 25, 28, 29 },
			new int[8] { 10, 11, 14, 15, 26, 27, 30, 31 }
		};

		static readonly int[] clut8 = new int[32] {
			0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17,
			0x8, 0x9, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F
		};

		static readonly int[] columnWord32 = new int[16] {
			 0,  1,  4,  5,  8,  9, 12, 13,
			 2,  3,  6,  7, 10, 11, 14, 15
		};

		static readonly int[] block16 = new int[32] {
			 0,  2,  8, 10,
			 1,  3,  9, 11,
			 4,  6, 12, 14,
			 5,  7, 13, 15,
			16, 18, 24, 26,
			17, 19, 25, 27,
			20, 22, 28, 30,
			21, 23, 29, 31
		};

		static readonly int[] columnWord16 = new int[32] {
			 0,  1,  4,  5,  8,  9, 12, 13,   0,  1,  4,  5,  8,  9, 12, 13,
			 2,  3,  6,  7, 10, 11, 14, 15,   2,  3,  6,  7, 10, 11, 14, 15
		};

		static readonly int[] columnHalf16 = new int[32] {
			0, 0, 0, 0, 0, 0, 0, 0,  1, 1, 1, 1, 1, 1, 1, 1,
			0, 0, 0, 0, 0, 0, 0, 0,  1, 1, 1, 1, 1, 1, 1, 1
		};


		static readonly int[] block8 = new int[32] {
			 0,  1,  4,  5, 16, 17, 20, 21,
			 2,  3,  6,  7, 18, 19, 22, 23,
			 8,  9, 12, 13, 24, 25, 28, 29,
			10, 11, 14, 15, 26, 27, 30, 31
		};

		static readonly int[][] columnWord8 = new int[2][] {
			new int[64] {
				 0,  1,  4,  5,  8,  9, 12, 13,   0,  1,  4,  5,  8,  9, 12, 13,
				 2,  3,  6,  7, 10, 11, 14, 15,   2,  3,  6,  7, 10, 11, 14, 15,

				 8,  9, 12, 13,  0,  1,  4,  5,   8,  9, 12, 13,  0,  1,  4,  5,
				10, 11, 14, 15,  2,  3,  6,  7,  10, 11, 14, 15,  2,  3,  6,  7
			},
			new int[64] {
				 8,  9, 12, 13,  0,  1,  4,  5,   8,  9, 12, 13,  0,  1,  4,  5,
				10, 11, 14, 15,  2,  3,  6,  7,  10, 11, 14, 15,  2,  3,  6,  7,

				 0,  1,  4,  5,  8,  9, 12, 13,   0,  1,  4,  5,  8,  9, 12, 13,
				 2,  3,  6,  7, 10, 11, 14, 15,   2,  3,  6,  7, 10, 11, 14, 15
			}
		};

		static readonly int[] columnByte8 = new int[64] {
			0, 0, 0, 0, 0, 0, 0, 0,  2, 2, 2, 2, 2, 2, 2, 2,
			0, 0, 0, 0, 0, 0, 0, 0,  2, 2, 2, 2, 2, 2, 2, 2,

			1, 1, 1, 1, 1, 1, 1, 1,  3, 3, 3, 3, 3, 3, 3, 3,
			1, 1, 1, 1, 1, 1, 1, 1,  3, 3, 3, 3, 3, 3, 3, 3
		};

		static readonly int[] block4 = new int[32] {
			0,  2,  8, 10,
			1,  3,  9, 11,
			4,  6, 12, 14,
			5,  7, 13, 15,
			16, 18, 24, 26,
			17, 19, 25, 27,
			20, 22, 28, 30,
			21, 23, 29, 31
		};

		static readonly int[][] columnWord4 = new int[2][] {
			new int[128] {
				 0,  1,  4,  5,  8,  9, 12, 13,   0,  1,  4,  5,  8,  9, 12, 13,   0,  1,  4,  5,  8,  9, 12, 13,   0,  1,  4,  5,  8,  9, 12, 13,
				 2,  3,  6,  7, 10, 11, 14, 15,   2,  3,  6,  7, 10, 11, 14, 15,   2,  3,  6,  7, 10, 11, 14, 15,   2,  3,  6,  7, 10, 11, 14, 15,

				 8,  9, 12, 13,  0,  1,  4,  5,   8,  9, 12, 13,  0,  1,  4,  5,   8,  9, 12, 13,  0,  1,  4,  5,   8,  9, 12, 13,  0,  1,  4,  5,
				10, 11, 14, 15,  2,  3,  6,  7,  10, 11, 14, 15,  2,  3,  6,  7,  10, 11, 14, 15,  2,  3,  6,  7,  10, 11, 14, 15,  2,  3,  6,  7
			},
			new int[128] {
				 8,  9, 12, 13,  0,  1,  4,  5,   8,  9, 12, 13,  0,  1,  4,  5,   8,  9, 12, 13,  0,  1,  4,  5,   8,  9, 12, 13,  0,  1,  4,  5,
				10, 11, 14, 15,  2,  3,  6,  7,  10, 11, 14, 15,  2,  3,  6,  7,  10, 11, 14, 15,  2,  3,  6,  7,  10, 11, 14, 15,  2,  3,  6,  7,

				 0,  1,  4,  5,  8,  9, 12, 13,   0,  1,  4,  5,  8,  9, 12, 13,   0,  1,  4,  5,  8,  9, 12, 13,   0,  1,  4,  5,  8,  9, 12, 13,
				 2,  3,  6,  7, 10, 11, 14, 15,   2,  3,  6,  7, 10, 11, 14, 15,   2,  3,  6,  7, 10, 11, 14, 15,   2,  3,  6,  7, 10, 11, 14, 15
			}
		};

		static readonly int[] columnByte4 = new int[128] {
			0, 0, 0, 0, 0, 0, 0, 0,  2, 2, 2, 2, 2, 2, 2, 2,  4, 4, 4, 4, 4, 4, 4, 4,  6, 6, 6, 6, 6, 6, 6, 6,
			0, 0, 0, 0, 0, 0, 0, 0,  2, 2, 2, 2, 2, 2, 2, 2,  4, 4, 4, 4, 4, 4, 4, 4,  6, 6, 6, 6, 6, 6, 6, 6,

			1, 1, 1, 1, 1, 1, 1, 1,  3, 3, 3, 3, 3, 3, 3, 3,  5, 5, 5, 5, 5, 5, 5, 5,  7, 7, 7, 7, 7, 7, 7, 7,
			1, 1, 1, 1, 1, 1, 1, 1,  3, 3, 3, 3, 3, 3, 3, 3,  5, 5, 5, 5, 5, 5, 5, 5,  7, 7, 7, 7, 7, 7, 7, 7
		};

		#endregion

		static uint BlockNumber32(int x, int y, uint dbp, uint dbw)
        {
			return (uint)(dbp + (y & ~0x1f) * dbw + ((x >> 1) & ~0x1f) + blockTable32[(y >> 3) & 3][(x >> 3) & 7]);
        }

		static void WriteImageBlock32(int l, int r, int y, int h, int dataIndex, int srcpitch, int dbp, int dbw, byte[] data)
		{
			var bsx = 8;
			var bsy = 8;
			var dataMemStr = new MemoryStream(data);
			var dataBinReader = new BinaryReader(dataMemStr);

			for (int offset = srcpitch * bsy; h >= bsy; h -= bsy, y += bsy, dataIndex += offset)
			{
				for (int x = l; x < r; x += bsx)
                {
					var gsMemIndex = BlockNumber32(x, y, (uint)dbp, (uint)dbw) << 8;
					WriteBlock32((int)gsMemIndex, dataIndex + x * 4, data, ref gs, srcpitch);
                }
			}

			dataBinReader.Close();
		}

		static void WriteBlock32(int dstIndex, int srcIndex, byte[] src, ref byte[] dst, int srcpitch)
		{
			WriteColumn32(0, dstIndex, srcIndex, srcpitch, src, ref dst);
			srcIndex += srcpitch * 2;
			WriteColumn32(1 << 1, dstIndex, srcIndex, srcpitch, src, ref dst);
			srcIndex += srcpitch * 2;
			WriteColumn32(2 << 1, dstIndex, srcIndex, srcpitch, src, ref dst);
			srcIndex += srcpitch * 2;
			WriteColumn32(3 << 1, dstIndex, srcIndex, srcpitch, src, ref dst);
		}

		static void WriteColumn32(int y, int dstIndex, int srcIndex, int srcpitch, byte[] src, ref byte[] dst)
        {
			var i = ((y >> 1) & 3);
			var srcMemStr = new MemoryStream(src);
			var dstMemStr = new MemoryStream(dst);
			var srcBinRead = new BinaryReader(srcMemStr);
			var dstBinWriter = new BinaryWriter(dstMemStr);
			dstBinWriter.BaseStream.Position = i * 4 * 16 + dstIndex;
			srcBinRead.BaseStream.Position = srcIndex;

			Util.GSVector4i v0 = Util.GSVector4i.Read(srcBinRead);
			Util.GSVector4i v1 = Util.GSVector4i.Read(srcBinRead);
			srcBinRead.BaseStream.Position = srcIndex + srcpitch;
			Util.GSVector4i v2 = Util.GSVector4i.Read(srcBinRead);
			Util.GSVector4i v3 = Util.GSVector4i.Read(srcBinRead);

			Util.GSVector4i.sw64(ref v0, ref v2, ref v1, ref v3);

			v0.Write(dstBinWriter);
			v1.Write(dstBinWriter);
			v2.Write(dstBinWriter);
			v3.Write(dstBinWriter);

			srcBinRead.Close();
			dstBinWriter.Close();
		}

		public static void cleanGs()
        {
			gs = new byte[1024 * 1024 * 4];
        }

		public static void writeDirect(byte[] data)
        {
			data.CopyTo(gs, 0);
        }

		public static void dumpMemory(string path, bool dumpImage = false, string imgPath = "")
        {
			var gsDump = File.Create(path, gs.Length);
			gsDump.Write(gs, 0, gs.Length);
			gsDump.Close();

			if (!dumpImage) return;
			// Use this strictly for debugging as this is a processor heavy operation to visualize the entire GS map
			Color[] paletteEntries = new Color[256];
			for (int i = 0; i < 256; ++i)
            {
				paletteEntries[i] = Color.FromArgb(i, i, i);
            }
			var bitmap = new Bitmap(16, 262144, PixelFormat.Format32bppArgb);
			for (int i = 0; i < 262144; ++i)
            {
				for (int j = 0; j < 16; ++j)
                {
					bitmap.SetPixel(j, i, paletteEntries[255 - gs[j + i * 16]]);
				}
            }
			using (MemoryStream memory = new MemoryStream())
			{
				using (FileStream fs = new FileStream(imgPath, FileMode.Create, FileAccess.ReadWrite))
				{
					bitmap.Save(memory, ImageFormat.Bmp);
					byte[] bytes = memory.ToArray();
					fs.Write(bytes, 0, bytes.Length);
				}
			}
		}

		public static void writeSelfPSMCT32(int dbp, int dbw, int dsax, int dsay, int rrw, int rrh)
		{
			var gsCopy = (byte[])gs.Clone();
			cleanGs();
			writeTexPSMCT32(dbp, dbw, dsax, dsay, rrw, rrh, gsCopy);
		}

		public static void writeSelfPSMCT16(int dbp, int dbw, int dsax, int dsay, int rrw, int rrh)
		{
			var gsCopy = (byte[])gs.Clone();
			cleanGs();
			writeTexPSMCT16(dbp, dbw, dsax, dsay, rrw * 2, rrh * 2, gsCopy);
		}

		public static void writeTexPSMT4(int dbp, int dbw, int dsax, int dsay, int rrw, int rrh, byte[] data)
		{
			dbw >>= 1;
			int src = 0;
			int startBlockPos = dbp * 64;

			bool odd = false;

			for (int y = dsay; y < dsay + rrh; y++)
			{
				for (int x = dsax; x < dsax + rrw; x++)
				{
					int pageX = x / 128;
					int pageY = y / 128;
					int page = pageX + pageY * dbw;

					int px = x - (pageX * 128);
					int py = y - (pageY * 128);

					int blockX = px / 32;
					int blockY = py / 16;
					int block = block4[blockX + blockY * 4];

					int bx = px - blockX * 32;
					int by = py - blockY * 16;

					int column = by / 4;

					int cx = bx;
					int cy = by - column * 4;
					int cw = columnWord4[column & 1][cx + cy * 32];
					int cb = columnByte4[cx + cy * 32];

					int dst = startBlockPos + page * 2048 + block * 64 + column * 16 + cw;

					if ((cb & 1) != 0)
					{
						if (odd)
							gs[4 * dst + cb >> 1] = (byte)((gs[4 * dst + cb >> 1] & 0x0f) | ((data[src]) & 0xf0));
						else
							gs[4 * dst + cb >> 1] = (byte)((gs[4 * dst + cb >> 1] & 0x0f) | (((data[src]) << 4) & 0xf0));
					}
					else
					{
						if (odd)
							gs[4 * dst + cb >> 1] = (byte)((gs[4 * dst + cb >> 1] & 0xf0) | (((data[src]) >> 4) & 0x0f));
						else
							gs[4 * dst + cb >> 1] = (byte)((gs[4 * dst + cb >> 1] & 0xf0) | ((data[src]) & 0x0f));
					}

					if (odd)
						src++;

					odd = !odd;
				}
			}
		}

		public static void readTexPSMT4(int dbp, int dbw, int dsax, int dsay, int rrw, int rrh, ref byte[] data)
		{
			dbw >>= 1;
			int src = 0;
			int startBlockPos = dbp * 64;

			bool odd = false;

			for (int y = dsay; y < dsay + rrh; y++)
			{
				for (int x = dsax; x < dsax + rrw; x++)
				{
					int pageX = x / 128;
					int pageY = y / 128;
					int page = pageX + pageY * dbw;

					int px = x - (pageX * 128);
					int py = y - (pageY * 128);

					int blockX = px / 32;
					int blockY = py / 16;
					int block = block4[blockX + blockY * 4];

					int bx = px - blockX * 32;
					int by = py - blockY * 16;

					int column = by / 4;

					int cx = bx;
					int cy = by - column * 4;
					int cw = columnWord4[column & 1][cx + cy * 32];
					int cb = columnByte4[cx + cy * 32];

					int dst = startBlockPos + page * 2048 + block * 64 + column * 16 + cw + cb >> 1;
					//unsigned char* dst = (unsigned char*)&gsmem[startBlockPos + page * 2048 + block * 64 + column * 16 + cw];

					if ((cb & 1) != 0)
					{
						if (odd)
							data[src] = (byte)(((data[src]) & 0x0f) | (byte)(gs[4 * dst + cb >> 1] & 0xf0));
						else
							data[src] = (byte)(((data[src]) & 0xf0) | ((byte)(gs[4 * dst + cb >> 1] >> 4) & 0x0f));
					}
					else
					{
						if (odd)
							data[src] = (byte)(((data[src]) & 0x0f) | (((byte)gs[4 * dst + cb >> 1] << 4) & 0xf0));
						else
							data[src] = (byte)(((data[src]) & 0xf0) | ((byte)gs[4 * dst + cb >> 1] & 0x0f));
					}

					if (odd)
						src++;

					odd = !odd;
				}
			}
		}

		public static void readTexPSMT4_mod(int dbp, int dbw, int dsax, int dsay, int rrw, int rrh, ref byte[] data)
		{
			dbw >>= 1;
			int src = 0;
			int startBlockPos = dbp * 64;

			bool odd = false;

			for (int y = dsay; y < dsay + rrh; y++)
			{
				for (int x = dsax; x < dsax + rrw; x++)
				{
					int pageX = x / 128;
					int pageY = y / 128;
					int page = pageX + pageY * dbw;

					int px = x - (pageX * 128);
					int py = y - (pageY * 128);

					int blockX = px / 32;
					int blockY = py / 16;
					int block = block4[blockX + blockY * 4];

					int bx = px - blockX * 32;
					int by = py - blockY * 16;

					int column = by / 4;

					int cx = bx;
					int cy = by - column * 4;
					int cb = columnByte4[cx + cy * 32];
					int cw = columnWord4[column & 1][cx + cy * 32];

					int dst = startBlockPos + page * 2048 + block * 64 + column * 16 + cw;

					if ((cb & 1) != 0)
					{
						if (odd)
							data[src] = (byte)((gs[4 * dst + (cb >> 1)] >> 4) & 0x0f);
						else
							data[src] = (byte)((gs[4 * dst + (cb >> 1)] >> 4) & 0x0f);
					}
					else
					{
						if (odd)
							data[src] = (byte)(gs[4 * dst + (cb >> 1)] & 0x0f);
						else
							data[src] = (byte)(gs[4 * dst + (cb >> 1)] & 0x0f);
					}

					src++;

					odd = !odd;
				}
			}
		}

		public static void readTexPSMT8(int dbp, int dbw, int dsax, int dsay, int rrw, int rrh, ref byte[] data)
		{
			dbw >>= 1;
			int src = 0;
			int startBlockPos = dbp * 64;

			for (int y = dsay; y < dsay + rrh; y++)
			{
				for (int x = dsax; x < dsax + rrw; x++)
				{
					int pageX = x / 128;
					int pageY = y / 64;
					int page = pageX + pageY * dbw;

					int px = x - (pageX * 128);
					int py = y - (pageY * 64);

					int blockX = px / 16;
					int blockY = py / 16;
					int block = block8[blockX + blockY * 8];

					int bx = px - blockX * 16;
					int by = py - blockY * 16;

					int column = by / 4;

					int cx = bx;
					int cy = by - column * 4;
					int cw = columnWord8[column & 1][cx + cy * 16];
					int cb = columnByte8[cx + cy * 16];

					int dst = startBlockPos + page * 2048 + block * 64 + column * 16 + cw;
					data[src] = gs[4 * dst + cb];
					src++;
				}
			}
		}

		public static void writeTexPSMT8(int dbp, int dbw, int dsax, int dsay, int rrw, int rrh, byte[] data)
		{
			dbw >>= 1;
			int src = 0;
			int startBlockPos = dbp * 64;

			for (int y = dsay; y < dsay + rrh; y++)
			{
				for (int x = dsax; x < dsax + rrw; x++)
				{
					int pageX = x / 128;
					int pageY = y / 64;
					int page = pageX + pageY * dbw;

					int px = x - (pageX * 128);
					int py = y - (pageY * 64);

					int blockX = px / 16;
					int blockY = py / 16;
					int block = block8[blockX + blockY * 8];

					int bx = px - (blockX * 16);
					int by = py - (blockY * 16);

					int column = by / 4;

					int cx = bx;
					int cy = by - column * 4;
					int cw = columnWord8[column & 1][cx + cy * 16];
					int cb = columnByte8[cx + cy * 16];

					int dst = startBlockPos + page * 2048 + block * 64 + column * 16 + cw;
					gs[4 * dst + cb] = data[src];
					src++;
				}
			}
		}

		public static void writeTexPSMCT16(int dbp, int dbw, int dsax, int dsay, int rrw, int rrh, byte[] data)
		{
			//dbw >>= 1;
			int src = 0;
			int startBlockPos = dbp * 64;

			for (int y = dsay; y < dsay + rrh; y++)
			{
				for (int x = dsax; x < dsax + rrw; x++)
				{
					int pageX = x / 64;
					int pageY = y / 64;
					int page = pageX + pageY * dbw;

					int px = x - (pageX * 64);
					int py = y - (pageY * 64);

					int blockX = px / 16;
					int blockY = py / 8;
					int block = block16[blockX + blockY * 4];

					int bx = px - blockX * 16;
					int by = py - blockY * 8;

					int column = by / 2;

					int cx = bx;
					int cy = by - column * 2;
					int cw = columnWord16[cx + cy * 16];
					int ch = columnHalf16[cx + cy * 16];

					int dst = startBlockPos + page * 2048 + block * 64 + column * 16 + cw;
					for (int i = 0; i < 2; i++)
					{
						gs[4 * dst + 2 * ch + i] = data[src + i];
					}
					src += 2;
				}
			}
		}

		public static void readTexPSMCT16(int dbp, int dbw, int dsax, int dsay, int rrw, int rrh, ref byte[] data)
		{
			//dbw >>= 1;
			int src = 0;
			int startBlockPos = dbp * 64;

			for (int y = dsay; y < dsay + rrh; y++)
			{
				for (int x = dsax; x < dsax + rrw; x++)
				{
					int pageX = x / 64;
					int pageY = y / 64;
					int page = pageX + pageY * dbw;

					int px = x - (pageX * 64);
					int py = y - (pageY * 64);

					int blockX = px / 16;
					int blockY = py / 8;
					int block = block16[blockX + blockY * 4];

					int bx = px - blockX * 16;
					int by = py - blockY * 8;

					int column = by / 2;

					int cx = bx;
					int cy = by - column * 2;
					int cw = columnWord16[cx + cy * 16];
					int ch = columnHalf16[cx + cy * 16];

					int dst = startBlockPos + page * 2048 + block * 64 + column * 16 + cw;
					for (int i = 0; i < 2; i++)
					{
						data[src + i] = gs[4 * dst + 2 * ch + i];
					}
					src += 2;
				}
			}
		}

		public static void writeTexPSMCT32(int dbp, int dbw, int dsax, int dsay, int rrw, int rrh, byte[] data)
		{
			int src = 0;
			int startBlockPos = dbp * 64;

			for (int y = dsay; y < dsay + rrh; y++)
			{
				for (int x = dsax; x < dsax + rrw; x++)
				{
					int pageX = x / 64;
					int pageY = y / 32;
					int page = pageX + pageY * dbw;

					int px = x - (pageX * 64);
					int py = y - (pageY * 32);

					int blockX = px / 8;
					int blockY = py / 8;
					int block = block32[blockX + blockY * 8];

					int bx = px - blockX * 8;
					int by = py - blockY * 8;

					int column = by / 2;

					int cx = bx;
					int cy = by - column * 2;
					int cw = columnWord32[cx + cy * 8];

					int dst = startBlockPos + page * 2048 + block * 64 + column * 16 + cw;
					for (int i = 0; i < 4; i++)
					{
						gs[4 * dst + i] = data[src + i];
					}
					src += 4;
				}
			}
		}

		public static void writeTexPSMCT32_mod(int dbp, int dbw, int dsax, int dsay, int rrw, int rrh, byte[] data)
		{
			// Most of this code is courtesy of PCSX2's source code and ported from C++ to C#, God please never make me do this ever again
			// For PSMCT32 8, 8 and 32 values are used. For other formats look into more of PCSX2's code
			// This is a very small part of the code as THANKFULLY at least Twins textures keep the same values for TRXPOS, TRXREG and TRXDIR
			var bsx = 8;
			var bsy = 8;
			var trbpp = 32;
			var ty = 0;

			int l = dsax;
			int r = l + rrw;

			// What is even happening here?
			int la = 0;
			int ra = r & ~(bsx - 1);
			int srcpitch = (r - l) * trbpp >> 3;
			var len = data.Length;
			int h = len / srcpitch;

			if (ra - la >= bsx && h > 0)
			{
				// But wait what???
				var dataIndex = -l * trbpp >> 3;
				len -= srcpitch * h;

				if (la < ra)
                {
					int h2 = h & ~(bsy - 1);

					if (h2 > 0)
                    {
						WriteImageBlock32(la, ra, ty, h2, dataIndex, srcpitch, dbp, dbw, data);
						h -= h2;
                    }

					if (h > 0)
                    {
						throw new Exception("Houston we got a problem! We need to write more code!");
                    }
				}
			}

			if (len > 0)
            {
				throw new Exception("Houston we got a problem! We need to write more code!");
			}
		}

		public static void readTexPSMCT32(int dbp, int dbw, int dsax, int dsay, int rrw, int rrh, ref byte[] data)
		{
			int src = 0;
			int startBlockPos = dbp * 64;

			for (int y = dsay; y < dsay + rrh; y++)
			{
				for (int x = dsax; x < dsax + rrw; x++)
				{
					int pageX = x / 64;
					int pageY = y / 32;
					int page = pageX + pageY * dbw;

					int px = x - (pageX * 64);
					int py = y - (pageY * 32);

					int blockX = px / 8;
					int blockY = py / 8;
					int block = block32[blockX + blockY * 8];

					int bx = px - blockX * 8;
					int by = py - blockY * 8;

					int column = by / 2;

					int cx = bx;
					int cy = by - column * 2;
					int cw = columnWord32[cx + cy * 8];


					int dst = startBlockPos + page * 2048 + block * 64 + column * 16 + cw;
					for (int i = 0; i < 4; i++)
					{
						data[src + i] = gs[4 * dst + i];
					}
					src += 4;
				}
			}
		}
	}
}