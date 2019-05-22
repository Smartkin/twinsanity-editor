using System.IO;

namespace Twinsanity
{
    public static class RIFF
    {
        public static byte[] SaveRiff(byte[] pcm, short channels, int samplerate)
        {
            byte[] data = new byte[pcm.Length + 46];
            BinaryWriter writer = new BinaryWriter(new MemoryStream(data));
            writer.Write("RIFF".ToCharArray());
            writer.Write(36 + data.Length);
            writer.Write("WAVE".ToCharArray());
            writer.Write("fmt ".ToCharArray());
            writer.Write(16);
            writer.Write((ushort)1);
            writer.Write(channels);
            writer.Write(samplerate);
            writer.Write(samplerate * channels * 2);
            writer.Write((short)(channels * 2));
            writer.Write((ushort)16);
            writer.Write("data".ToCharArray());
            writer.Write(data.Length);
            writer.Write(pcm);
            writer.Close();
            return data;
        }
    }
}
