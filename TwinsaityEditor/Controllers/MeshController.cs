using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System;
using Twinsanity;

namespace TwinsaityEditor
{
    public class MeshController : Controller
    {
        private Mesh data;

        public MeshController(Mesh item)
        {
            Toolbar = ToolbarFlags.Hex | ToolbarFlags.Extract | ToolbarFlags.Replace | ToolbarFlags.Delete | ToolbarFlags.View;
            data = item;
        }

        public override string GetName()
        {
            return "Mesh [ID: " + data.ID + "]";
        }

        public override void GenText()
        {
            var ex_lines = new List<string>();
            foreach (var i in data.SubModels)
            {
                ex_lines.Add("SubMesh" + data.SubModels.IndexOf(i));
                ex_lines.Add("VertexCount: " + i.VertexCount + " BlockSize: " + i.BlockSize);
                ex_lines.Add("K: " + i.k + " C: " + i.c);
                ex_lines.Add("GroupCount: " + i.Groups.Count);
                foreach (var j in i.Groups)
                    ex_lines.Add("VertexCount: " + j.VertexCount);
            }
            TextPrev = new string[3 + ex_lines.Count];
            TextPrev[0] = "ID: " + data.ID;
            TextPrev[1] = "Offset: " + data.Offset + " Size: " + data.Size;
            TextPrev[2] = "SubMesh Count: " + data.SubModels.Count;
            Array.Copy(ex_lines.ToArray(), 0, TextPrev, 3, ex_lines.Count);
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
