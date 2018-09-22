using System;
using Twinsanity;

namespace TwinsaityEditor
{
    public class InstanceController : ItemController
    {
        public new Instance Data { get; set; }

        public InstanceController(Instance item) : base(item)
        {
            Data = item;
        }

        public override string GetName()
        {
            if (FileController.GetObjectName(Data.ObjectID) != string.Empty)
                return FileController.GetObjectName(Data.ObjectID) + " Instance [ID " + Data.ID + "]";
            else
                return "Instance [ID " + Data.ID + "]";
        }

        public override void GenText()
        {
            string obj_name = FileController.GetObjectName(Data.ObjectID);
            TextPrev = new string[12 + Data.S1.Count + Data.S2.Count + Data.S3.Count + Data.UnkI321.Length + Data.UnkI322.Length + Data.UnkI323.Length];
            TextPrev[0] = "ID: " + Data.ID;
            TextPrev[1] = "Offset: " + Data.Offset + " Size: " + Data.Size;
            TextPrev[2] = "Object ID " + Data.ObjectID + (obj_name != string.Empty ? " (" + FileController.GetObjectName(Data.ObjectID) + ")" : string.Empty);
            TextPrev[3] = "Position (" + Data.Pos.X + ", " + Data.Pos.Y + ", " + Data.Pos.Z + ", " + Data.Pos.W + ")";
            TextPrev[4] = "Rotation (" + Data.RotX + "/" + Data.COMRotX + " | " + Data.RotY + "/" + Data.COMRotY + " | " + Data.RotZ + "/" + Data.COMRotZ + ")";

            TextPrev[5] = "Instances: " + Data.S1.Count + " SomeNum1: " + Data.SomeNum1;
            for (int i = 0; i < Data.S1.Count; ++i)
                TextPrev[6 + i] = Data.S1[i].ToString();

            TextPrev[6 + Data.S1.Count] = "Positions: " + Data.S2.Count + " SomeNum2: " + Data.SomeNum2;
            for (int i = 0; i < Data.S2.Count; ++i)
                TextPrev[7 + Data.S1.Count + i] = Data.S2[i].ToString();

            TextPrev[7 + Data.S1.Count + Data.S2.Count] = "Paths: " + Data.S3.Count + " SomeNum3: " + Data.SomeNum3;
            for (int i = 0; i < Data.S3.Count; ++i)
                TextPrev[8 + Data.S1.Count + Data.S2.Count + i] = Data.S3[i].ToString();
            
            TextPrev[8 + Data.S1.Count + Data.S2.Count + Data.S3.Count] = "Properties: " + Convert.ToString(Data.UnkI32, 16).ToUpper();

            TextPrev[9 + Data.S1.Count + Data.S2.Count + Data.S3.Count] = "Integers: " + Data.UnkI321.Length;
            for (int i = 0; i < Data.UnkI321.Length; ++i)
                TextPrev[10 + Data.S1.Count + Data.S2.Count + Data.S3.Count + i] = Data.UnkI321[i].ToString();

            TextPrev[10 + Data.S1.Count + Data.S2.Count + Data.S3.Count + Data.UnkI321.Length] = "Floats: " + Data.UnkI322.Length;
            for (int i = 0; i < Data.UnkI322.Length; ++i)
                TextPrev[11 + Data.S1.Count + Data.S2.Count + Data.S3.Count + Data.UnkI321.Length + i] = Data.UnkI322[i].ToString();

            TextPrev[11 + Data.S1.Count + Data.S2.Count + Data.S3.Count + Data.UnkI321.Length + Data.UnkI322.Length] = "Integers: " + Data.UnkI323.Length;
            for (int i = 0; i < Data.UnkI323.Length; ++i)
                TextPrev[12 + Data.S1.Count + Data.S2.Count + Data.S3.Count + Data.UnkI321.Length + Data.UnkI322.Length + i] = Data.UnkI323[i].ToString();
        }
    }
}
