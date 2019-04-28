using System;
using Twinsanity;

namespace TwinsaityEditor
{
    public class InstanceController : ItemController
    {
        public new Instance Data { get; set; }

        public InstanceController(MainForm topform, Instance item) : base (topform, item)
        {
            Data = item;
        }

        protected override string GetName()
        {
            if (MainFile.GetObjectName(Data.ObjectID) != string.Empty)
                return MainFile.GetObjectName(Data.ObjectID) + " Instance [ID " + Data.ID + "]";
            else
                return "Instance [ID " + Data.ID + "]";
        }

        protected override void GenText()
        {
            string obj_name = MainFile.GetObjectName(Data.ObjectID);
            TextPrev = new string[12 + Data.S1.Count + Data.S2.Count + Data.S3.Count + Data.UnkI321.Count + Data.UnkI322.Count + Data.UnkI323.Count];
            TextPrev[0] = "ID: " + Data.ID;
            TextPrev[1] = "Offset: " + Data.Offset + " Size: " + Data.Size;
            TextPrev[2] = "Object ID " + Data.ObjectID + (obj_name != string.Empty ? " (" + MainFile.GetObjectName(Data.ObjectID) + ")" : string.Empty);
            TextPrev[3] = "Position (" + Data.Pos.X + ", " + Data.Pos.Y + ", " + Data.Pos.Z + ", " + Data.Pos.W + ")";
            TextPrev[4] = "Rotation (" + Data.RotX + " | " + Data.RotY + " | " + Data.RotZ + ")";

            TextPrev[5] = "Instances: " + Data.S1.Count;
            for (int i = 0; i < Data.S1.Count; ++i)
                TextPrev[6 + i] = Data.S1[i].ToString();

            TextPrev[6 + Data.S1.Count] = "Positions: " + Data.S2.Count;
            for (int i = 0; i < Data.S2.Count; ++i)
                TextPrev[7 + Data.S1.Count + i] = Data.S2[i].ToString();

            TextPrev[7 + Data.S1.Count + Data.S2.Count] = "Paths: " + Data.S3.Count;
            for (int i = 0; i < Data.S3.Count; ++i)
                TextPrev[8 + Data.S1.Count + Data.S2.Count + i] = Data.S3[i].ToString();
            
            TextPrev[8 + Data.S1.Count + Data.S2.Count + Data.S3.Count] = "Properties: " + Convert.ToString(Data.UnkI32, 16).ToUpper();

            TextPrev[9 + Data.S1.Count + Data.S2.Count + Data.S3.Count] = "Integers: " + Data.UnkI321.Count;
            for (int i = 0; i < Data.UnkI321.Count; ++i)
                TextPrev[10 + Data.S1.Count + Data.S2.Count + Data.S3.Count + i] = Data.UnkI321[i].ToString();

            TextPrev[10 + Data.S1.Count + Data.S2.Count + Data.S3.Count + Data.UnkI321.Count] = "Floats: " + Data.UnkI322.Count;
            for (int i = 0; i < Data.UnkI322.Count; ++i)
                TextPrev[11 + Data.S1.Count + Data.S2.Count + Data.S3.Count + Data.UnkI321.Count + i] = Data.UnkI322[i].ToString();

            TextPrev[11 + Data.S1.Count + Data.S2.Count + Data.S3.Count + Data.UnkI321.Count + Data.UnkI322.Count] = "Integers: " + Data.UnkI323.Count;
            for (int i = 0; i < Data.UnkI323.Count; ++i)
                TextPrev[12 + Data.S1.Count + Data.S2.Count + Data.S3.Count + Data.UnkI321.Count + Data.UnkI322.Count + i] = Data.UnkI323[i].ToString();
        }
    }
}
