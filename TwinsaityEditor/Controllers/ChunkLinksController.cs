using System;
using Twinsanity;

namespace TwinsaityEditor
{
    public class ChunkLinksController : ItemController
    {
        private ChunkLinks data;

        public ChunkLinksController(ChunkLinks item) : base(item)
        {
            data = item;
        }

        public override string GetName()
        {
            return "Chunk Links [ID " + data.ID + "]";
        }

        public override void GenText()
        {
            TextPrev = new string[3 + data.Links.Count * 5];
            TextPrev[0] = "ID: " + data.ID;
            TextPrev[1] = "Offset: " + data.Offset + " Size: " + data.Size;
            TextPrev[2] = "LinkCount: " + data.Links.Count;
            for (int i = 0; i < data.Links.Count; ++i)
            {
                TextPrev[4 + i * 5] = "Link" + i;
                TextPrev[5 + i * 5] = "Type: " + data.Links[i].Type;
                TextPrev[6 + i * 5] = "Directory: " + new string(data.Links[i].Path);
                TextPrev[7 + i * 5] = "Flags: " + Convert.ToString(data.Links[i].Flags, 16).ToUpper();
            }
        }
    }
}
