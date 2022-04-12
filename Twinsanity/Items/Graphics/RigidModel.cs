using System.IO;

namespace Twinsanity
{
    public class RigidModel : TwinsItem
    {
        public uint Header { get; set; }
        public uint[] MaterialIDs { get; set; }
        public uint MeshID { get; set; }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Header);
            writer.Write(MaterialIDs.Length);
            for (int i = 0; i < MaterialIDs.Length; ++i)
                writer.Write(MaterialIDs[i]);
            writer.Write(MeshID);
        }

        public override void Load(BinaryReader reader, int size)
        {
            Header = reader.ReadUInt32();
            var count = reader.ReadInt32();
            MaterialIDs = new uint[count];
            for (int i = 0; i < count; ++i)
                MaterialIDs[i] = reader.ReadUInt32();
            MeshID = reader.ReadUInt32();
        }

        protected override int GetSize()
        {
            return 12 + MaterialIDs.Length * 4;
        }

        internal void FillPackage(TwinsFile source, TwinsFile destination)
        {
            var sourceMaterials = source.GetItem<TwinsSection>(11).GetItem<TwinsSection>(1);
            var destinationMaterials = destination.GetItem<TwinsSection>(11).GetItem<TwinsSection>(1);
            var sourceMeshes = source.GetItem<TwinsSection>(11).GetItem<TwinsSection>(2);
            var destinationMeshes = destination.GetItem<TwinsSection>(11).GetItem<TwinsSection>(2);
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
            if (!destinationMeshes.HasItem(MeshID))
            {
                var linkedMesh = sourceMeshes.GetItem<Model>(MeshID);
                destinationMeshes.AddItem(MeshID, linkedMesh);
            }
        }
    }
}
