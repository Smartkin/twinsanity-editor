using Twinsanity;

namespace TwinsaityEditor
{
    public class PositionController : ItemController
    {
        public new Position Data { get; set; }

        public PositionController(Position item) : base(item)
        {
            Data = item;
        }

        protected override string GetName()
        {
            return "Position [ID " + Data.ID + "]";
        }

        protected override void GenText()
        {
            TextPrev = new string[3];
            TextPrev[0] = "ID: " + Data.ID;
            TextPrev[1] = "Offset: " + Data.Offset + " Size: " + Data.Size;
            TextPrev[2] = "Position (" + Data.Pos.X + ", " + Data.Pos.Y + ", " + Data.Pos.Z + ", " + Data.Pos.W + ")";
        }
    }
}
