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
            TextPrev = new string[14 + Data.ModelIDs.Length + Data.Variables.Length + (Data.Type1.Length * 10) + (Data.Type2.Length * 5)];
            int cur_pos = 8;
            TextPrev[0] = $"ID: {Data.ID}";
            TextPrev[1] = $"Bounding Box Vector 1: {Data.Coord1.X}; {Data.Coord1.Y}; {Data.Coord1.Z}; {Data.Coord1.W}";
            TextPrev[2] = $"Bounding Box Vector 2: {Data.Coord2.X}; {Data.Coord2.Y}; {Data.Coord2.Z}; {Data.Coord2.W}";
            TextPrev[3] = $"Armature Model ID: {Data.ArmatureModelID}";
            TextPrev[4] = $"Actor Model ID: {Data.ActorModelID}";
            TextPrev[5] = $"Type 1 Size: {Data.Type1.Length}";
            TextPrev[6] = $"Type 2 Size: {Data.Type2.Length}";
            TextPrev[7] = $"Model Count: {Data.ModelIDs.Length}";
            if (Data.ModelIDs.Length > 0)
            {
                TextPrev[cur_pos] = $"Models Linked:";
                cur_pos++;
                for (int i = 0; i < Data.ModelIDs.Length; i++)
                {
                    TextPrev[cur_pos] = $"Model #{ Data.ModelIDs[i].ID } ID: { Data.ModelIDs[i].ModelID }";
                    cur_pos++;
                }
            }
            
            if (Data.Type1.Length > 0)
            {
                TextPrev[cur_pos] = $"Type 1 Structs:";
                cur_pos++;
                for (int i = 0; i < Data.Type1.Length; i++)
                {
                    TextPrev[cur_pos] = $"#{ i } Numbers: { Data.Type1[i].Numbers[0] }; { Data.Type1[i].Numbers[1] }; { Data.Type1[i].Numbers[2] }; { Data.Type1[i].Numbers[3] }; { Data.Type1[i].Numbers[4] }";
                    cur_pos++;
                    TextPrev[cur_pos] = $"#{ i } Local Position: { Data.Type1[i].LocalPosition.X }; { Data.Type1[i].LocalPosition.Y }; { Data.Type1[i].LocalPosition.Z }; { Data.Type1[i].LocalPosition.W }";
                    cur_pos++;
                    TextPrev[cur_pos] = $"#{ i } Vector 1: { Data.Type1[i].Matrix[0].X }; { Data.Type1[i].Matrix[0].Y }; { Data.Type1[i].Matrix[0].Z }; { Data.Type1[i].Matrix[0].W }";
                    cur_pos++;
                    TextPrev[cur_pos] = $"#{ i } Vector 2: { Data.Type1[i].Matrix[1].X }; { Data.Type1[i].Matrix[1].Y }; { Data.Type1[i].Matrix[1].Z }; { Data.Type1[i].Matrix[1].W }";
                    cur_pos++;
                    TextPrev[cur_pos] = $"#{ i } Vector 3: { Data.Type1[i].Matrix[2].X }; { Data.Type1[i].Matrix[2].Y }; { Data.Type1[i].Matrix[2].Z }; { Data.Type1[i].Matrix[2].W }";
                    cur_pos++;
                    TextPrev[cur_pos] = $"#{ i } Vector 4: { Data.Type1[i].Matrix[3].X }; { Data.Type1[i].Matrix[3].Y }; { Data.Type1[i].Matrix[3].Z }; { Data.Type1[i].Matrix[3].W }";
                    cur_pos++;
                    TextPrev[cur_pos] = $"#{ i } T3 Matrix 1: { Data.Type3[i].Matrix[0].X }; { Data.Type3[i].Matrix[0].Y }; { Data.Type3[i].Matrix[0].Z }; { Data.Type3[i].Matrix[0].W }";
                    cur_pos++;
                    TextPrev[cur_pos] = $"#{ i } T3 Matrix 2: { Data.Type3[i].Matrix[1].X }; { Data.Type3[i].Matrix[1].Y }; { Data.Type3[i].Matrix[1].Z }; { Data.Type3[i].Matrix[1].W }";
                    cur_pos++;
                    TextPrev[cur_pos] = $"#{ i } T3 Matrix 3: { Data.Type3[i].Matrix[2].X }; { Data.Type3[i].Matrix[2].Y }; { Data.Type3[i].Matrix[2].Z }; { Data.Type3[i].Matrix[2].W }";
                    cur_pos++;
                    TextPrev[cur_pos] = $"#{ i } T3 Matrix 4: { Data.Type3[i].Matrix[3].X }; { Data.Type3[i].Matrix[3].Y }; { Data.Type3[i].Matrix[3].Z }; { Data.Type3[i].Matrix[3].W }";
                    cur_pos++;
                }
            }
            if (Data.Type2.Length > 0)
            {
                TextPrev[cur_pos] = $"Type 2 Structs:";
                cur_pos++;
                for (int i = 0; i < Data.Type2.Length; i++)
                {
                    TextPrev[cur_pos] = $"#{ i } Numbers: { Data.Type2[i].Numbers[0] }; { Data.Type2[i].Numbers[1] }";
                    cur_pos++;
                    TextPrev[cur_pos] = $"#{ i } Matrix 1: { Data.Type2[i].Matrix[0].X }; { Data.Type2[i].Matrix[0].Y }; { Data.Type2[i].Matrix[0].Z }; { Data.Type2[i].Matrix[0].W }";
                    cur_pos++;
                    TextPrev[cur_pos] = $"#{ i } Matrix 2: { Data.Type2[i].Matrix[1].X }; { Data.Type2[i].Matrix[1].Y }; { Data.Type2[i].Matrix[1].Z }; { Data.Type2[i].Matrix[1].W }";
                    cur_pos++;
                    TextPrev[cur_pos] = $"#{ i } Matrix 3: { Data.Type2[i].Matrix[2].X }; { Data.Type2[i].Matrix[2].Y }; { Data.Type2[i].Matrix[2].Z }; { Data.Type2[i].Matrix[2].W }";
                    cur_pos++;
                    TextPrev[cur_pos] = $"#{ i } Matrix 4: { Data.Type2[i].Matrix[3].X }; { Data.Type2[i].Matrix[3].Y }; { Data.Type2[i].Matrix[3].Z }; { Data.Type2[i].Matrix[3].W }";
                    cur_pos++;
                }
            }

            if (Data.Variables.Length > 0)
            {
                TextPrev[cur_pos] = $"Variables: ";
                cur_pos++;
                for (int i = 0; i < Data.Variables.Length; i++)
                {
                    TextPrev[cur_pos] = $"Variable #{ i }: { Data.Variables[i] }";
                    cur_pos++;
                }
            }
        }
    }
}