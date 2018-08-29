using System;
using System.Diagnostics;

namespace Twinsanity
{
    /// <summary>
    /// Represents Adaptive Differential Pulse-Code Modulation
    /// </summary>
    public class ADPCM
    {
        const float k = ((22050.0f / 1881.0f) + (44100.0f / 3763.0f) + (48000.0f / 4096.0f)) / 3.0f;
        const int BUFFER_SIZE = 128 * 28;
        private float[][] F = new float[5][];
        private float _s_1 = 0, _s_2 = 0;
        private float mS_1 = 0, mS_2 = 0;

        internal void InitF()
        {
            for (int i = 0; i < 5; i++)
                F[i] = new float[2];
            F[0][0] = 0.0f;
            F[0][1] = 0.0f;
            F[1][0] = 60.0f / 64.0f;
            F[1][1] = 0.0f;
            F[2][0] = 115.0f / 64.0f;
            F[2][1] = -52.0f / 64.0f;
            F[3][0] = 98.0f / 64.0f;
            F[3][1] = -55.0f / 64.0f;
            F[4][0] = 122.0f / 64.0f;
            F[4][1] = -60.0f / 64.0f;
        }

        internal void InitF2()
        {
            for (int i = 0; i < 5; i++)
                F[i] = new float[2];
            F[0][0] = 0.0f;
            F[0][1] = 0.0f;
            F[1][0] = -60.0f / 64.0f;
            F[1][1] = 0.0f;
            F[2][0] = -115.0f / 64.0f;
            F[2][1] = 52.0f / 64.0f;
            F[3][0] = -98.0f / 64.0f;
            F[3][1] = 55.0f / 64.0f;
            F[4][0] = -122.0f / 64.0f;
            F[4][1] = 60.0f / 64.0f;
        }

        /// <summary>
        /// Demultiplexing given ADPCM file into the right and left audios
        /// </summary>
        /// <param name="ADPCM">Original ADPCM represented as a stream in RAM</param>
        /// <param name="ADPCM_R">Right ADPCM that will be returned</param>
        /// <param name="ADPCM_L">Left ADPCM that will be returned</param>
        /// <param name="Interleave">Determines how mixed up together left and right will be in the result</param>
        public void ADPCM_Demux(System.IO.MemoryStream ADPCM, ref System.IO.MemoryStream ADPCM_R, ref System.IO.MemoryStream ADPCM_L, uint Interleave)
        {
            System.IO.BinaryReader ADPCMReader = new System.IO.BinaryReader(ADPCM);
            System.IO.BinaryWriter ADPCM_RWriter = new System.IO.BinaryWriter(ADPCM_R);
            System.IO.BinaryWriter ADPCM_LWriter = new System.IO.BinaryWriter(ADPCM_L);
            ADPCM.Position = 0;
            ADPCM_L.Position = 0;
            ADPCM_R.Position = 0;
            while (ADPCM.Position < ADPCM.Length)
            {
                long BlockSize = (ADPCM.Length - ADPCM.Position < Interleave) ? ADPCM.Length - ADPCM.Position : Interleave;
                //Be sure that the block we are reading is an actual block
                Debug.Assert(BlockSize > 0);
                
                ADPCM_LWriter.Write(ADPCMReader.ReadBytes((int)BlockSize));
                byte b = 0;
                for (int i = 1; i <= Interleave - BlockSize; i++)
                    ADPCM_LWriter.Write(b);

                BlockSize = (ADPCM.Length - ADPCM.Position < Interleave) ? ADPCM.Length - ADPCM.Position : Interleave;
                //Be sure that the block we are reading is an actual block
                Debug.Assert(BlockSize > 0);

                ADPCM_RWriter.Write(ADPCMReader.ReadBytes((int)BlockSize));
                for (int i = 1; i <= Interleave - BlockSize; i++)
                    ADPCM_RWriter.Write(b);
            }
            ADPCM.Position = 0;
            ADPCM_L.Position = 0;
            ADPCM_R.Position = 0;
        }

        /// <summary>
        /// Multiplexing left and right ADPCM into single ADPCM file
        /// </summary>
        /// <param name="ADPCM">ADPCM file to get as a result</param>
        /// <param name="ADPCM_R">Right ADPCM represented as a stream in RAM</param>
        /// <param name="ADPCM_L">Left ADPCM represented as a stream in RAM</param>
        /// <param name="Interleave">Determines how mixed up the left and right of original ADPCM are</param>
        public void ADPCM_Mux(ref System.IO.MemoryStream ADPCM, System.IO.MemoryStream ADPCM_R, System.IO.MemoryStream ADPCM_L, uint Interleave)
        {
            System.IO.BinaryWriter ADPCMWriter = new System.IO.BinaryWriter(ADPCM);
            System.IO.BinaryReader ADPCM_RReader = new System.IO.BinaryReader(ADPCM_R);
            System.IO.BinaryReader ADPCM_LReader = new System.IO.BinaryReader(ADPCM_L);
            ADPCM.Position = 0;
            ADPCM_L.Position = 0;
            ADPCM_R.Position = 0;
            while ((ADPCM_L.Position < ADPCM_L.Length) | (ADPCM_L.Position < ADPCM_L.Length))
            {
                long BlockSize = (ADPCM.Length - ADPCM.Position < Interleave) ? ADPCM.Length - ADPCM.Position : Interleave;
                //Be sure that the block we are reading is an actual block
                Debug.Assert(BlockSize > 0);

                ADPCMWriter.Write(ADPCM_LReader.ReadBytes((int)BlockSize));
                byte b = 0;
                for (int i = 1; i <= Interleave - BlockSize; i++)
                    ADPCMWriter.Write(b);

                BlockSize = (ADPCM_R.Length - ADPCM_R.Position < Interleave) ? ADPCM_R.Length - ADPCM_R.Position : Interleave;
                //Be sure that the block we are reading is an actual block
                Debug.Assert(BlockSize > 0);

                ADPCMWriter.Write(ADPCM_RReader.ReadBytes((int)BlockSize));
                for (int i = 1; i <= Interleave - BlockSize; i++)
                    ADPCMWriter.Write(b);
            }
            ADPCM.Position = 0;
            ADPCM_L.Position = 0;
            ADPCM_R.Position = 0;
        }

        /// <summary>
        /// Demultiplexing given PCM into left and right PCMs
        /// </summary>
        /// <param name="PCM">PCM file represented as a stream in RAM</param>
        /// <param name="PCM_R">Right PCM to get as a result</param>
        /// <param name="PCM_L">Left PCM to get as a result</param>
        public void PCM_Demux(System.IO.MemoryStream PCM, ref System.IO.MemoryStream PCM_R, ref System.IO.MemoryStream PCM_L)
        {
            System.IO.BinaryReader PCMReader = new System.IO.BinaryReader(PCM);
            System.IO.BinaryWriter PCM_RWriter = new System.IO.BinaryWriter(PCM_R);
            System.IO.BinaryWriter PCM_LWriter = new System.IO.BinaryWriter(PCM_L);
            PCM.Position = 0;
            PCM_L.Position = 0;
            PCM_R.Position = 0;
            while (PCM.Position < PCM.Length)
            {
                PCM_LWriter.Write(PCMReader.ReadInt16());
                PCM_RWriter.Write(PCMReader.ReadInt16());
            }
            PCM.Position = 0;
            PCM_L.Position = 0;
            PCM_R.Position = 0;
        }

        /// <summary>
        /// Multiplexing given left and right PCMs into a single one
        /// </summary>
        /// <param name="PCM">PCM to receive as a result</param>
        /// <param name="PCM_R">Right PCM represented as a stream in RAM</param>
        /// <param name="PCM_L">Left PCM represented as a stream in RAM</param>
        public void PCM_Mux(ref System.IO.MemoryStream PCM, System.IO.MemoryStream PCM_R, System.IO.MemoryStream PCM_L)
        {
            System.IO.BinaryWriter PCMWriter = new System.IO.BinaryWriter(PCM);
            System.IO.BinaryReader PCM_RReader = new System.IO.BinaryReader(PCM_R);
            System.IO.BinaryReader PCM_LReader = new System.IO.BinaryReader(PCM_L);
            short Null = 0;
            PCM.Position = 0;
            PCM_L.Position = 0;
            PCM_R.Position = 0;
            while ((PCM_R.Position < PCM_R.Length) || (PCM_L.Position < PCM_L.Length))
            {
                if (PCM_L.Position < PCM_L.Length)
                    PCMWriter.Write(PCM_LReader.ReadInt16());
                else
                    PCMWriter.Write(Null);
                if (PCM_R.Position < PCM_R.Length)
                    PCMWriter.Write(PCM_RReader.ReadInt16());
                else
                    PCMWriter.Write(Null);
            }
            PCM.Position = 0;
            PCM_L.Position = 0;
            PCM_R.Position = 0;
        }

        /// <summary>
        /// Convert ADPCM to PCM
        /// </summary>
        /// <param name="ADPCM">ADPCM file to convert represented as a stream in RAM</param>
        /// <param name="Wav">WAV file to get as a result represented as a stream in RAM</param>
        public void ADPCM2PCM(System.IO.MemoryStream ADPCM, ref System.IO.MemoryStream Wav)
        {
            InitF();
            System.IO.BinaryReader Reader = new System.IO.BinaryReader(ADPCM);
            System.IO.BinaryWriter Writer = new System.IO.BinaryWriter(Wav);
            Wav.Position = 0;
            bool Flag = true;
            float s_1 = 0f, s_2 = 0f;
            while (Flag)
            {
                byte predict_nr;
                byte shift_factor;
                byte flags;
                int D, S;
                float[] Samples = new float[28];
                predict_nr = Reader.ReadByte();
                shift_factor = (byte)(predict_nr & 15);
                predict_nr >>= 4;
                flags = Reader.ReadByte();
                if (flags == 7 || ADPCM.Length - ADPCM.Position < 16)
                    Flag = false;
                else
                {
                    for (int i = 0; i <= 27; i += 2)
                    {
                        D = Reader.ReadByte();
                        S = (D & 15) << 12;
                        if ((S & 32768) != 0)
                            S = S - 65536;
                        int tmp = S >> shift_factor;
                        Samples[i] = S >> shift_factor;
                        S = (D & 240) << 8;
                        if ((S & 32768) != 0)
                            S = S - 65536;
                        Samples[i + 1] = S >> shift_factor;
                    }
                    for (int i = 0; i <= 27; i++)
                    {
                        Samples[i] = Samples[i] + s_1 * F[predict_nr][0] + s_2 * F[predict_nr][1];
                        s_2 = s_1;
                        s_1 = Samples[i];
                        short tmp;
                        if (Math.Round((Samples[i] + 0.5)) > short.MaxValue)
                            tmp = short.MaxValue;
                        else if (Math.Round((Samples[i] + 0.5)) < short.MinValue)
                            tmp = short.MinValue;
                        else
                            tmp = (short)Math.Round((Samples[i] + 0.5));
                        Writer.Write(tmp);
                    }
                }
            }
            Wav.Position = 0;
        }

        /// <summary>
        /// Convert PCM to ADPCM
        /// </summary>
        /// <param name="ADPCM">ADPCM to get as a result</param>
        /// <param name="PCM">PCM file to convert represented as a stream in RAM</param>
        public void PCM2ADPCM(ref System.IO.MemoryStream ADPCM, System.IO.MemoryStream PCM)
        {
            InitF2();
            System.IO.BinaryWriter ADPCMWriter = new System.IO.BinaryWriter(ADPCM);
            System.IO.BinaryReader WavReader = new System.IO.BinaryReader(PCM);
            int samples = (int)PCM.Length / 2;
            byte flags = 0;
            while (samples > 0 && PCM.Position < PCM.Length)
            {
                int work_size;
                short[] wave = new short[3584];
                if (samples >= BUFFER_SIZE)
                    work_size = BUFFER_SIZE;
                else
                    work_size = samples;
                for (int i = 0; i <= work_size - 1; i++)
                    wave[i] = WavReader.ReadInt16();
                for (int i = 0; i <= work_size / 28 - 1; i++)
                {
                    float[] d_samples = new float[28];
                    int[] four_bit = new int[28];
                    byte predict_nr = 0;
                    byte shift_factor = 0;
                    find_predict(wave, (short)(i * 28), ref predict_nr, ref shift_factor, ref d_samples);
                    pack(d_samples, ref four_bit, predict_nr, shift_factor);
                    byte D = (byte)((predict_nr << 4) | shift_factor);
                    ADPCMWriter.Write(D);
                    ADPCMWriter.Write(flags);
                    for (int j = 0; j <= 27; j += 2)
                    {
                        byte ebit = (byte)(((four_bit[j + 1] >> 8) & 240) | ((four_bit[j] >> 12) & 15));
                        ADPCMWriter.Write(ebit);
                    }
                    samples -= 28;
                    if (samples < 28)
                        flags = 1;
                }
            }
            byte _D = 0;
            ADPCMWriter.Write(_D);
            _D = 7;
            ADPCMWriter.Write(_D);
            _D = 119;
            for (int i = 0; i <= 13; i++)
                ADPCMWriter.Write(_D);
            PCM.Position = 0;
            ADPCM.Position = 0;
        }

        /// <summary>
        /// Converts Twinsanity's sound to WAV sound format
        /// </summary>
        /// <param name="ADPCM">ADPCM file represented as a stream in RAM</param>
        /// <param name="Wav">WAV file to save the sound into represented as a stream in RAM</param>
        /// <param name="Frequency">Frequence of the sound</param>
        /// <param name="Chan">Channel number of the sound, default is 1</param>
        public void Twin2WAV(System.IO.MemoryStream ADPCM, ref System.IO.MemoryStream Wav, uint Frequency, short Chan = 1)
        {
            char[] Header = new[] { 'R', 'I', 'F', 'F' };
            int FileSize = 0; // FinalSize - 8 position 4
            char[] WAVHeader = new[] { 'W', 'A', 'V', 'E' };
            char[] fmtHeader = new[] { 'f', 'm', 't', ' ' };
            int SubChunk1Size = 16;
            short Format = 1;
            short Chanells = Chan;
            int SampleRate;
            switch (Frequency)
            {
                case 682:
                    {
                        SampleRate = 8000;
                        break;
                    }

                case 1024:
                    {
                        SampleRate = 11025;
                        break;
                    }

                case 1365:
                    {
                        SampleRate = 16000;
                        break;
                    }

                case 1706:
                    {
                        SampleRate = 20000;
                        break;
                    }

                case 1536:
                    {
                        SampleRate = 18000;
                        break;
                    }

                case 1881:
                    {
                        SampleRate = 22050;
                        break;
                    }

                case 3763:
                    {
                        SampleRate = 44100;
                        break;
                    }

                case 4096:
                    {
                        SampleRate = 48000;
                        break;
                    }

                default:
                    {
                        SampleRate = (int)Math.Round(Frequency * k);
                        break;
                    }
            }
            int BitRate = SampleRate * Chanells * 2;
            short Align = (short)(Chanells * 2);
            short BPS = 16;
            char[] SubChunk2Id = new[] { 'd', 'a', 't', 'a' };
            int SubChunk2Size = 0; // FinalSize - 44 position 40
            ADPCM.Position = 0;
            Wav.Position = 0;
            System.IO.BinaryWriter Writer = new System.IO.BinaryWriter(Wav);
            System.IO.BinaryReader Reader = new System.IO.BinaryReader(ADPCM);
            // WAVE Header
            InitF();
            Writer.Write(Header);
            Writer.Write(FileSize);
            Writer.Write(WAVHeader);
            Writer.Write(fmtHeader);
            Writer.Write(SubChunk1Size);
            Writer.Write(Format);
            Writer.Write(Chanells);
            Writer.Write(SampleRate);
            Writer.Write(BitRate);
            Writer.Write(Align);
            Writer.Write(BPS);
            Writer.Write(SubChunk2Id);
            Writer.Write(SubChunk2Size);
            // DATA
            bool Flag = true;
            float s_1 = 0f, s_2 = 0f;
            while (Flag)
            {
                byte predict_nr;
                byte shift_factor;
                byte flags;
                int D, S;
                float[] Samples = new float[28];
                predict_nr = Reader.ReadByte();
                shift_factor = (byte)(predict_nr & 15);
                predict_nr >>= 4;
                flags = Reader.ReadByte();
                if (flags == 7 || ADPCM.Length - ADPCM.Position < 16)
                    Flag = false;
                else
                {
                    for (int i = 0; i <= 27; i += 2)
                    {
                        D = Reader.ReadByte();
                        S = (D & 15) << 12;
                        if ((S & 32768) != 0)
                            S = S - 65536;
                        int tmp = S >> shift_factor;
                        Samples[i] = S >> shift_factor;
                        S = (D & 240) << 8;
                        if ((S & 32768) != 0)
                            S = S - 65536;
                        Samples[i + 1] = S >> shift_factor;
                    }
                    for (int i = 0; i <= 27; i++)
                    {
                        Samples[i] = Samples[i] + s_1 * F[predict_nr][0] + s_2 * F[predict_nr][1];
                        s_2 = s_1;
                        s_1 = Samples[i];
                        short tmp;
                        if (Math.Round((Samples[i] + 0.5)) > short.MaxValue)
                            tmp = short.MaxValue;
                        else if (Math.Round((Samples[i] + 0.5)) < short.MinValue)
                            tmp = short.MinValue;
                        else
                            tmp = (short)Math.Round((Samples[i] + 0.5));
                        Writer.Write(tmp);
                    }
                }
            }
            // Post Processing
            Wav.Position = 4;
            uint Len = (uint)Wav.Length - 8;
            Writer.Write(Len);
            Wav.Position = 40;
            Len = (uint)Wav.Length - 44;
            Writer.Write(Len);
            ADPCM.Position = 0;
            Wav.Position = 0;
        }

        /// <summary>
        /// Converts WAV sound to Twinsanity's sound format
        /// </summary>
        /// <param name="Wav">WAV file represented as a stream in RAM</param>
        /// <param name="ADPCM">ADPCM file represented as a stream in RAM</param>
        public void WAV2Twin(System.IO.MemoryStream Wav, ref System.IO.MemoryStream ADPCM)
        {
            InitF2();
            char[] Header;
            int FileSize;
            char[] WAVHeader;
            char[] fmtHeader;
            int SubChunk1Size;
            short Format;
            short Chanells;
            uint SampleRate;
            uint BitRate;
            ushort Align;
            ushort BPS;
            char[] SubChunk2Id;
            int SubChunk2Size;
            System.IO.BinaryReader WavReader = new System.IO.BinaryReader(Wav);
            System.IO.BinaryWriter ADPCMWriter = new System.IO.BinaryWriter(ADPCM);
            Wav.Position = 0;
            ADPCM.Position = 0;
            Header = WavReader.ReadChars(4);
            FileSize = WavReader.ReadInt32();
            WAVHeader = WavReader.ReadChars(4);
            fmtHeader = WavReader.ReadChars(4);
            SubChunk1Size = WavReader.ReadInt32();
            Format = WavReader.ReadInt16();
            Chanells = WavReader.ReadInt16();
            SampleRate = WavReader.ReadUInt32();
            BitRate = WavReader.ReadUInt32();
            Align = WavReader.ReadUInt16();
            BPS = WavReader.ReadUInt16();
            SubChunk2Id = WavReader.ReadChars(4);
            SubChunk2Size = WavReader.ReadInt32();
            int samples = SubChunk2Size / 2;
            int size = samples / 28;
            if (samples % 28 > 0)
                size += 1;
            int ADPCM_Size = size * 16 + 16;
            ushort Frequency;
            switch (SampleRate)
            {
                case 8000:
                    {
                        Frequency = 682;
                        break;
                    }

                case 11025:
                    {
                        Frequency = 1024;
                        break;
                    }

                case 16000:
                    {
                        Frequency = 1365;
                        break;
                    }

                case 20000:
                    {
                        Frequency = 1706;
                        break;
                    }

                case 18000:
                    {
                        Frequency = 1536;
                        break;
                    }

                case 22050:
                    {
                        Frequency = 1881;
                        break;
                    }

                case 44100:
                    {
                        Frequency = 3763;
                        break;
                    }

                case 48000:
                    {
                        Frequency = 4096;
                        break;
                    }

                default:
                    {
                        Frequency = (ushort)Math.Round(SampleRate / (double)k);
                        break;
                    }
            }

            Random random = new Random();
            int I32 = (int)Math.Round(random.NextDouble() * 1500 + 2000);
            ADPCMWriter.Write(I32);
            ADPCMWriter.Write(Frequency);
            ADPCMWriter.Write(ADPCM_Size);
            byte flags = 0;
            while (samples > 0 && Wav.Position < Wav.Length)
            {
                int work_size;
                short[] wave = new short[3584];
                if (samples >= BUFFER_SIZE)
                    work_size = BUFFER_SIZE;
                else
                    work_size = samples;
                for (int i = 0; i <= work_size - 1; i++)
                    wave[i] = WavReader.ReadInt16();
                for (int i = 0; i <= work_size / 28 - 1; i++)
                {
                    float[] d_samples = new float[28];
                    int[] four_bit = new int[28];
                    byte predict_nr = 0;
                    byte shift_factor = 0;
                    find_predict(wave, (short)(i * 28), ref predict_nr, ref shift_factor, ref d_samples);
                    pack(d_samples, ref four_bit, predict_nr, shift_factor);
                    byte D = (byte)((predict_nr << 4) | shift_factor);
                    ADPCMWriter.Write(D);
                    ADPCMWriter.Write(flags);
                    for (int j = 0; j <= 27; j += 2)
                    {
                        byte ebit = (byte)(((four_bit[j + 1] >> 8) & 240) | ((four_bit[j] >> 12) & 15));
                        ADPCMWriter.Write(ebit);
                    }
                    samples -= 28;
                    if (samples < 28)
                        flags = 1;
                }
            }
            byte _D = 0;
            ADPCMWriter.Write(_D);
            _D = 7;
            ADPCMWriter.Write(_D);
            _D = 119;
            for (int i = 0; i <= 13; i++)
                ADPCMWriter.Write(_D);
            Wav.Position = 0;
            ADPCM.Position = 0;
        }

        internal void find_predict(short[] wave, short index, ref byte predict_nr, ref byte shift_factor, ref float[] d_samples)
        {
            float[][] buffer = new float[28][];
            float min = (float)Math.Pow(10, 10);
            float[] max = new float[5];
            float ds;
            int min2;
            int shift_mask;
        /* 
        *   Static _s_1, _s_2 As Single
        *   Impossible to convert due to this feature non-existing in C#, gotta add them as normal class members
        */
            float s_0 = 0, s_1 = 0, s_2 = 0;
            for (int i = 0; i <= 27; i++)
                Array.Resize(ref buffer[i], 5);
            for (int i = 0; i <= 4; i++)
            {
                max[i] = 0;
                s_1 = _s_1;
                s_2 = _s_2;
                for (int j = 0; j <= 27; j++)
                {
                    s_0 = wave[index + j];
                    if (s_0 > 30719)
                        s_0 = 30719;
                    else if (s_0 < -30720)
                        s_0 = -30720;
                    ds = s_0 + s_1 * F[i][0] + s_2 * F[i][1];
                    buffer[j][i] = ds;
                    if (Math.Abs(ds) > max[i])
                        max[i] = Math.Abs(ds);
                    s_2 = s_1;
                    s_1 = s_0;
                }
                if (max[i] < min)
                {
                    min = max[i];
                    predict_nr = (byte)i;
                }
                if (min <= 7)
                {
                    predict_nr = 0;
                    break;
                }
            }
            _s_1 = s_1;
            _s_2 = s_2;
            for (int i = 0; i <= 27; i++)
                d_samples[i] = buffer[i][predict_nr];
            if (min > 32767)
                min = 32767;
            min2 = (int)Math.Round(min);
            shift_mask = 16384;
            shift_factor = 0;
            while (shift_factor < 12)
            {
                var bit_condition = shift_mask & (min2 + (shift_mask >> 3));
                if (bit_condition != 0)
                    break;
                shift_factor += 1;
                shift_mask = shift_mask >> 1;
            }
        }

        internal void pack(float[] d_samples, ref int[] four_bit, byte predict_nr, byte shift_factor)
        {
            float ds;
            int di;
            float s_0;
    /*
     * Same thing
     * Static s_1 As Single
    */
    /*
     * Same stuff here
     * Static s_2 As Single
    */
            for (int i = 0; i <= 27; i++)
            {
                s_0 = d_samples[i] + mS_1 * F[predict_nr][0] + mS_2 * F[predict_nr][1];
                ds = s_0 * (1 << shift_factor);
                di = (int)(((uint)Math.Round(ds) + 2048) & 4294963200);
                if (di > short.MaxValue)
                    di = short.MaxValue;
                else if (di < short.MinValue)
                    di = short.MinValue;
                four_bit[i] = di;
                di = di >> shift_factor;
                mS_2 = mS_1;
                mS_1 = di - s_0;
            }
        }
    }
}
