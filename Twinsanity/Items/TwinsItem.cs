using System;
using System.IO;

namespace Twinsanity
{
    /// <summary>
    /// Represents a generic item in the chunk tree
    /// </summary>
    public class TwinsItem
    {
        public virtual int GetSize()
        {
            if (Data == null) return -1;
            else return Data.Length;
        }

        public virtual byte[] Save()
        {
            return Data;
        }

        public virtual void Load(BinaryReader reader)
        {
            throw new NotImplementedException("TwinsItem::Load: Function Load(BinaryReader reader) cannot be used in TwinsItem.");
        }

        public virtual void Load(BinaryReader reader, int size)
        {
            Data = reader.ReadBytes(size);
        }

        public byte[] Data { get; set; }
        public uint Offset { get; set; }
        public int Size { get => GetSize(); }
    }
}
