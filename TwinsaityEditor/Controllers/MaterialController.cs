using System.Windows.Forms;
using System.IO;
using Twinsanity;

namespace TwinsaityEditor
{
    public class MaterialController : Controller
    {
        private Material data;

        public MaterialController(Material item)
        {
            Toolbar = ToolbarFlags.Hex | ToolbarFlags.Extract | ToolbarFlags.Replace | ToolbarFlags.Delete | ToolbarFlags.View;
            data = item;
        }

        public override string GetName()
        {
            return data.Name + " [ID: " + data.ID + "]";
        }

        public override void GenText()
        {
            TextPrev = new string[5];
            TextPrev[0] = "ID: " + data.ID;
            TextPrev[1] = "Offset: " + data.Offset + " Size: " + data.Size;
            TextPrev[2] = "Name: " + data.Name + " Texture ID: " + data.Tex;
            TextPrev[3] = "Integers: " + data.ValuesI[0] + " " + data.ValuesI[1] + " " + data.ValuesI[2] + " " + data.ValuesI[3];
            TextPrev[4] = "Floats: " + data.ValuesF[0] + " " + data.ValuesF[1] + " " + data.ValuesF[2] + " " + data.ValuesF[3];
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
