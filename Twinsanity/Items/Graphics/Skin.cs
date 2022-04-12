using System;
using System.IO;

namespace Twinsanity
{
    public class Skin : TwinsItem
    {

        public long ItemSize { get; set; }

        public uint SubModels { get; set; }
        public uint[] MaterialIDs { get; set; }
        public uint[] Vertexes { get; set; }
        public uint[] BlockSize { get; set; }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Data);
        }

        public override void Load(BinaryReader reader, int size)
        {
            long pre_pos = reader.BaseStream.Position;

            SubModels = reader.ReadUInt32();

            MaterialIDs = new uint[SubModels];
            Vertexes = new uint[SubModels];
            BlockSize = new uint[SubModels];
            for (int i = 0; i < SubModels; i++)
            {
                MaterialIDs[i] = reader.ReadUInt32();
                BlockSize[i] = reader.ReadUInt32();
                Vertexes[i] = reader.ReadUInt32();
                reader.BaseStream.Position = reader.BaseStream.Position + BlockSize[i];
            }

            ItemSize = size;
            reader.BaseStream.Position = pre_pos;
            Data = reader.ReadBytes(size);
        }

        protected override int GetSize()
        {
            return (int)ItemSize;
        }

        internal void FillPackage(TwinsFile source, TwinsFile destination)
        {
            var sourceMaterials = source.GetItem<TwinsSection>(11).GetItem<TwinsSection>(1);
            var destinationMaterials = destination.GetItem<TwinsSection>(11).GetItem<TwinsSection>(1);
            foreach (var materialId in MaterialIDs)
            {
                if (destinationMaterials.HasItem(materialId))
                {
                    continue;
                }
                var linkedMaterial = sourceMaterials.GetItem<Material>(materialId);
                destinationMaterials.AddItem(materialId, linkedMaterial);
                linkedMaterial.FillPackage(source, destination);
            }
        }
    }
}
