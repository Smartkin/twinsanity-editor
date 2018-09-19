using System.Windows.Forms;
using System.IO;
using System;
using Twinsanity;

namespace TwinsaityEditor
{
    public class ChunkLinksController : Controller
    {
        private ChunkLinks data;

        public ChunkLinksController(ChunkLinks item)
        {
            Toolbar = ToolbarFlags.Hex | ToolbarFlags.Extract | ToolbarFlags.Replace | ToolbarFlags.Delete;
            data = item;
        }

        public override string GetName()
        {
            return "Chunk Links [ID: " + data.ID + "]";
        }

        public override void GenText()
        {
            TextPrev = new string[3 + data.Links.Count * 5];
            TextPrev[0] = "ID: " + data.ID;
            TextPrev[1] = "Offset: " + data.Offset + " Size: " + data.Size;
            TextPrev[2] = "LinkCount: " + data.Links.Count;
            for (int i = 0; i < data.Links.Count; ++i)
            {
                TextPrev[3 + i * 5] = "Link" + i;
                TextPrev[4 + i * 5] = "Type: " + data.Links[i].Type;
                TextPrev[5 + i * 5] = "Directory: " + new string(data.Links[i].Path);
                TextPrev[6 + i * 5] = "Flags: " + Convert.ToString(data.Links[i].Flags, 16).ToUpper();
            }
        }

        public override void ToolbarAction(ToolbarFlags button)
        {
            switch (button)
            {
                case ToolbarFlags.Hex:
                    //do hex stuff here
                    break;
                case ToolbarFlags.Extract:
                    {
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.FileName = GetName().Replace(":", string.Empty);
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            FileStream file = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                            BinaryWriter writer = new BinaryWriter(file);
                            data.Save(writer);
                            writer.Close();
                            file.Close();
                        }
                    }
                    break;
            }
        }
    }
}
