using System.Collections.Generic;
using System.IO;

namespace Twinsanity
{
    public class Material : TwinsItem
    {
        public string Name { get; set; } = "Unnamed\0";
        public ulong Header { get; set; } = 2;
        public int Unknown { get; set; } = 2;
        public List<TwinsShader> Shaders = new List<TwinsShader>();

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Header);
            writer.Write(Unknown);
            writer.Write(Name.Length);
            writer.Write(Name.ToCharArray());
            writer.Write(Shaders.Count);
            foreach (var shd in Shaders)
            {
                shd.Write(writer);
            }
        }

        public override void Load(BinaryReader reader, int size)
        {
            bool isMB = Parent.Parent.Type == SectionType.GraphicsMB;
            Header = reader.ReadUInt64();
            Unknown = reader.ReadInt32();
            var nameLen = reader.ReadInt32();
            Name = new string(reader.ReadChars(nameLen));
            var shdCnt = reader.ReadInt32();
            Shaders.Clear();
            for (var i = 0; i < shdCnt; ++i)
            {
                TwinsShader shd = new TwinsShader();
                shd.Read(reader, 0, false, isMB);
                Shaders.Add(shd);
            }
        }

        protected override int GetSize()
        {
            var shdLen = 0;
            foreach (var shd in Shaders)
            {
                shdLen += shd.GetLength();
            }
            return 20 + Name.Length + shdLen;
        }

        internal void FillPackage(TwinsFile source, TwinsFile destination)
        {
            var sourceTextures = source.GetItem<TwinsSection>(11).GetItem<TwinsSection>(0);
            var destinationTextures = destination.GetItem<TwinsSection>(11).GetItem<TwinsSection>(0);
            foreach (var shader in Shaders)
            {
                var textureId = shader.TextureId;
                if (destinationTextures.HasItem(textureId))
                {
                    continue;
                }
                var linkedTexture = sourceTextures.GetItem<Texture>(textureId);
                destinationTextures.AddItem(textureId, linkedTexture);
            }
        }
    }
}
