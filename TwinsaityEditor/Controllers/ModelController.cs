using System.Windows.Forms;
using System.IO;
using Twinsanity;

namespace TwinsaityEditor
{
    public class ModelController : Controller
    {
        private Model data;

        public ModelController(Model item)
        {
            Toolbar = ToolbarFlags.Hex | ToolbarFlags.Extract | ToolbarFlags.Replace | ToolbarFlags.Delete;
            data = item;
        }

        public override string GetName()
        {
            return "Model [ID: " + data.ID + "]";
        }

        public override void GenText()
        {
            TextPrev = new string[4 + data.MaterialIDs.Length];
            TextPrev[0] = "ID: " + data.ID;
            TextPrev[1] = "Offset: " + data.Offset + " Size: " + data.Size;
            TextPrev[2] = "Header: " + data.Header + " MaterialCount: " + data.MaterialIDs.Length;
            for (int i = 0; i < data.MaterialIDs.Length; ++i)
                TextPrev[3 + i] = FileController.GetMaterialName(data.MaterialIDs[i]);
            TextPrev[3 + data.MaterialIDs.Length] = "Mesh: " + data.MeshID;
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
