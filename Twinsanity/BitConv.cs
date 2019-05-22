namespace Twinsanity
{
    public static class BitConv
    {
        public static void ToInt16(byte[] arr, int off, short value)
        {
            arr[off] = (byte)(value & 0xFF);
            arr[off + 1] = (byte)((value >> 8) & 0xFF);
        }
    }
}
