using System;
using Twinsanity;

namespace TwinsaityEditor
{
    public class ChunkLinksController : ItemController
    {
        public new ChunkLinks Data { get; set; }

        public ChunkLinksController(ChunkLinks item) : base(item)
        {
            Data = item;
        }

        protected override string GetName()
        {
            return "Chunk Links [ID " + Data.ID + "]";
        }

        protected override void GenText()
        {
            TextPrev = new string[3 + Data.Links.Count * 4];
            TextPrev[0] = "ID: " + Data.ID;
            TextPrev[1] = "Offset: " + Data.Offset + " Size: " + Data.Size;
            TextPrev[2] = "LinkCount: " + Data.Links.Count;
            for (int i = 0; i < Data.Links.Count; ++i)
            {
                TextPrev[3 + i * 5] = "Link" + i;
                TextPrev[4 + i * 5] = "Type: " + Data.Links[i].Type;
                TextPrev[5 + i * 5] = "Directory: " + new string(Data.Links[i].Path);
                TextPrev[6 + i * 5] = "Flags: " + Convert.ToString(Data.Links[i].Flags, 16).ToUpper();
            }
        }
    }
}
