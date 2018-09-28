using Twinsanity;

namespace TwinsaityEditor
{
    public class TriggerController : ItemController
    {
        public new Trigger Data { get; set; }

        public TriggerController(Trigger item) : base(item)
        {
            Data = item;
        }

        protected override string GetName()
        {
            return "Trigger [ID " + Data.ID + "]";
        }

        protected override void GenText()
        {
            TextPrev = new string[8 + Data.Instances.Length];
            TextPrev[0] = "ID: " + Data.ID;
            TextPrev[1] = "Offset: " + Data.Offset + " Size: " + Data.Size;
            TextPrev[2] = "Other (" + Data.Coords[0].X + ", " + Data.Coords[0].Y + ", " + Data.Coords[0].Z + ", " + Data.Coords[0].W + ")";
            TextPrev[3] = "Position (" + Data.Coords[1].X + ", " + Data.Coords[1].Y + ", " + Data.Coords[1].Z + ", " + Data.Coords[1].W + ")";
            TextPrev[4] = "Size (" + Data.Coords[2].X + ", " + Data.Coords[2].Y + ", " + Data.Coords[2].Z + ", " + Data.Coords[2].W + ")";
            TextPrev[5] = "SomeFloat: " + Data.SomeFloat;

            TextPrev[6] = "Instances: " + Data.Instances.Length;
            for (int i = 0; i < Data.Instances.Length; ++i)
            {
                string obj_name = FileController.GetObjectName(FileController.GetInstance(Data.Parent.Parent.ID, Data.Instances[i]).ObjectID);
                TextPrev[7 + i] = "Instance " + Data.Instances[i] + (obj_name != string.Empty ? " (" + obj_name + ")" : string.Empty);
            }

            TextPrev[7 + Data.Instances.Length] = "Arguments: " + Data.SomeUInt161 + " " + Data.SomeUInt162 + " " + Data.SomeUInt163 + " " + Data.SomeUInt164;
        }
    }
}
