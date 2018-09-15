using System.Windows.Forms;
using System.IO;
using System;
using Twinsanity;

namespace TwinsaityEditor
{
    public class TriggerController : Controller
    {
        private Trigger data;

        public TriggerController(Trigger item)
        {
            Toolbar = ToolbarFlags.Hex | ToolbarFlags.Extract | ToolbarFlags.Replace | ToolbarFlags.Delete;
            data = item;
        }

        public override string GetName()
        {
            return "Trigger [ID: " + data.ID + "]";
        }

        public override void GenText()
        {
            TextPrev = new string[8 + data.Instances.Length];
            TextPrev[0] = "ID: " + data.ID;
            TextPrev[1] = "Offset: " + data.Offset + " Size: " + data.Size;
            TextPrev[2] = "Other (" + data.Coords[0].X + ", " + data.Coords[0].Y + ", " + data.Coords[0].Z + ", " + data.Coords[0].W + ")";
            TextPrev[3] = "Position (" + data.Coords[1].X + ", " + data.Coords[1].Y + ", " + data.Coords[1].Z + ", " + data.Coords[1].W + ")";
            TextPrev[4] = "Size (" + data.Coords[2].X + ", " + data.Coords[2].Y + ", " + data.Coords[2].Z + ", " + data.Coords[2].W + ")";
            TextPrev[5] = "SomeFloat: " + data.SomeFloat;

            TextPrev[6] = "Instances: " + data.Instances.Length;
            for (int i = 0; i < data.Instances.Length; ++i)
            {
                string obj_name = FileController.GetObjectName(FileController.GetInstance(data.Parent.Parent.ID, data.Instances[i]).ObjectID);
                TextPrev[7 + i] = "Instance " + data.Instances[i] + (obj_name != string.Empty ? " (" + obj_name + ")" : string.Empty);
            }

            TextPrev[7 + data.Instances.Length] = "Arguments: " + data.SomeUInt161 + " " + data.SomeUInt162 + " " + data.SomeUInt163 + " " + data.SomeUInt164;
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
