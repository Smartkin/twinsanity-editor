using System.Windows.Forms;
using System.IO;
using System;
using Twinsanity;

namespace TwinsaityEditor
{
    public class InstanceController : Controller
    {
        private Instance data;

        public InstanceController(Instance item)
        {
            Toolbar = ToolbarFlags.Hex | ToolbarFlags.Extract | ToolbarFlags.Replace | ToolbarFlags.Delete;
            data = item;
        }

        public override string GetName()
        {
            if (FileController.GetObjectName(data.ObjectID) != string.Empty)
                return FileController.GetObjectName(data.ObjectID) + " Instance [ID: " + data.ID + "]";
            else
                return "Instance [ID: " + data.ID + "]";
        }

        public override void GenText()
        {
            string obj_name = FileController.GetObjectName(data.ObjectID);
            TextPrev = new string[12 + data.S1.Count + data.S2.Count + data.S3.Count + data.UnkI321.Length + data.UnkI322.Length + data.UnkI323.Length];
            TextPrev[0] = "ID: " + data.ID;
            TextPrev[1] = "Offset: " + data.Offset + " Size: " + data.Size;
            TextPrev[2] = "Object ID " + data.ObjectID + (obj_name != string.Empty ? " (" + FileController.GetObjectName(data.ObjectID) + ")" : string.Empty);
            TextPrev[3] = "Position (" + data.Pos.X + ", " + data.Pos.Y + ", " + data.Pos.Z + ", " + data.Pos.W + ")";
            TextPrev[4] = "Rotation (" + data.RotX + "/" + data.COMRotX + " | " + data.RotY + "/" + data.COMRotY + " | " + data.RotZ + "/" + data.COMRotZ + ")";

            TextPrev[5] = "Instances: " + data.S1.Count + " SomeNum1: " + data.SomeNum1;
            for (int i = 0; i < data.S1.Count; ++i)
                TextPrev[6 + i] = data.S1[i].ToString();

            TextPrev[6 + data.S1.Count] = "Positions: " + data.S2.Count + " SomeNum2: " + data.SomeNum2;
            for (int i = 0; i < data.S2.Count; ++i)
                TextPrev[7 + data.S1.Count + i] = data.S2[i].ToString();

            TextPrev[7 + data.S1.Count + data.S2.Count] = "Paths: " + data.S3.Count + " SomeNum3: " + data.SomeNum3;
            for (int i = 0; i < data.S3.Count; ++i)
                TextPrev[8 + data.S1.Count + data.S2.Count + i] = data.S3[i].ToString();
            
            TextPrev[8 + data.S1.Count + data.S2.Count + data.S3.Count] = "Properties: " + Convert.ToString(data.UnkI32, 16).ToUpper();

            TextPrev[9 + data.S1.Count + data.S2.Count + data.S3.Count] = "Integers: " + data.UnkI321.Length;
            for (int i = 0; i < data.UnkI321.Length; ++i)
                TextPrev[10 + data.S1.Count + data.S2.Count + data.S3.Count + i] = data.UnkI321[i].ToString();

            TextPrev[10 + data.S1.Count + data.S2.Count + data.S3.Count + data.UnkI321.Length] = "Floats: " + data.UnkI322.Length;
            for (int i = 0; i < data.UnkI322.Length; ++i)
                TextPrev[11 + data.S1.Count + data.S2.Count + data.S3.Count + data.UnkI321.Length + i] = data.UnkI322[i].ToString();

            TextPrev[11 + data.S1.Count + data.S2.Count + data.S3.Count + data.UnkI321.Length + data.UnkI322.Length] = "Integers: " + data.UnkI323.Length;
            for (int i = 0; i < data.UnkI323.Length; ++i)
                TextPrev[12 + data.S1.Count + data.S2.Count + data.S3.Count + data.UnkI321.Length + data.UnkI322.Length + i] = data.UnkI323[i].ToString();
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
