using System.Windows.Forms;
using System.IO;
using Twinsanity;

namespace TwinsaityEditor
{
    public class ScriptController : Controller
    {
        private Script data;

        public ScriptController(Script item)
        {
            Toolbar = ToolbarFlags.Hex | ToolbarFlags.Extract | ToolbarFlags.Replace | ToolbarFlags.Delete;
            data = item;
        }

        public override string GetName()
        {
            return (data.Name != null ? data.Name : "Script") + " [ID: " + data.ID + "]";
        }

        public override void GenText()
        {
            TextPrev = new string[2];
            TextPrev[0] = "ID: " + data.ID + (data.Name != null ? " Name: " + data.Name : string.Empty);
            TextPrev[1] = "Offset: " + data.Offset + " Size: " + data.Size;
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
