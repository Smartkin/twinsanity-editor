using System.Diagnostics;
using System.IO;

namespace Twinsanity
{
    /// <summary>
    /// Represents a generic item in the chunk tree
    /// </summary>
    public class TwinsItem
    {
        public virtual void Save(BinaryWriter writer)
        {
            writer.Write(Data);
        }

        public virtual void Load(BinaryReader reader, int size)
        {
            Data = reader.ReadBytes(size);
        }

        public byte[] Data { get; set; }
        public uint ID { get; set; }
        protected virtual int? ConstantSize => null;
        public int Size { get
            {
                if (ConstantSize.HasValue)
                {
                    return ConstantSize.Value;
                }
                using (BinaryWriter writer = new BinaryWriter(new MemoryStream()))
                {
                    Save(writer);
                    return (int)writer.BaseStream.Position;
                }
            }
        }
        public TwinsSection Parent { get; set; }
        public SectionType ParentType { get; set; }
    }
}
