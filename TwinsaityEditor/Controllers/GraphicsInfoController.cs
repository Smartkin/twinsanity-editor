using Twinsanity;

namespace TwinsaityEditor
{
    public class GraphicsInfoController : ItemController
    {
        public new GraphicsInfo Data { get; set; }

        public GraphicsInfoController(MainForm topform, GraphicsInfo item) : base(topform, item)
        {
            Data = item;
        }

        protected override string GetName()
        {
            return $"Graphics Info [ID {Data.ID}]";
        }

        protected override void GenText()
        {
            TextPrev = new string[9 + Data.ModelIDs.Length];
            TextPrev[0] = $"ID: {Data.ID}";
            TextPrev[1] = $"Type 1 Size: {Data.Type1.Length}";
            TextPrev[2] = $"Type 2 Size: {Data.Type2.Length}";
            TextPrev[3] = $"Model Count: {Data.ModelIDs.Length}";
            TextPrev[4] = $"Armature Model ID: {Data.ArmatureModelID}";
            TextPrev[5] = $"Actor Model ID: {Data.ActorModelID}";
            TextPrev[6] = $"Vector 1: {Data.Coord1.X}; {Data.Coord1.Y}; {Data.Coord1.Z}; {Data.Coord1.W}";
            TextPrev[7] = $"Vector 2: {Data.Coord2.X}; {Data.Coord2.Y}; {Data.Coord2.Z}; {Data.Coord2.W}";
            if (Data.ModelIDs.Length > 0)
            {
                for (int i = 0; i < Data.ModelIDs.Length; i++)
                {
                    TextPrev[i + 8] = $"Model #{ Data.ModelIDs[i].ID } ID: { Data.ModelIDs[i].ModelID }";
                }
            }
            else
            {
                TextPrev[8] = $"No models linked.";
            }
        }
    }
}