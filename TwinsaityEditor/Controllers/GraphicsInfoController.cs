using Twinsanity;
using System.Collections.Generic;

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
            List<string> text = new List<string>();
            text.Add($"ID: {Data.ID}");
            text.Add($"Bounding Box Vector 1: {Data.Coord1.X}; {Data.Coord1.Y}; {Data.Coord1.Z}; {Data.Coord1.W}");
            text.Add($"Bounding Box Vector 2: {Data.Coord2.X}; {Data.Coord2.Y}; {Data.Coord2.Z}; {Data.Coord2.W}");
            text.Add($"Armature Model ID: {Data.ArmatureModelID}");
            text.Add($"Actor Model ID: {Data.ActorModelID}");
            text.Add($"Type 1 Size: {Data.Type1.Length}");
            text.Add($"Type 2 Size: {Data.Type2.Length}");
            text.Add($"Type 4 Size: {Data.Type4.Length}");
            text.Add($"Model Count: {Data.ModelIDs.Length}");
            if (Data.ModelIDs.Length > 0)
            {
                text.Add($"Models Linked:");
                for (int i = 0; i < Data.ModelIDs.Length; i++)
                {
                    text.Add(string.Format("Model #{0} ID: {1:X8}", Data.ModelIDs[i].ID, Data.ModelIDs[i].ModelID));
                }
            }

            if (Data.Type1.Length > 0)
            {
                text.Add($"Type 1 Structs:");
                for (int i = 0; i < Data.Type1.Length; i++)
                {
                    text.Add($"#{ i } Numbers: { Data.Type1[i].Numbers[0] }; { Data.Type1[i].Numbers[1] }; { Data.Type1[i].Numbers[2] }; { Data.Type1[i].Numbers[3] }; { Data.Type1[i].Numbers[4] }");
                    text.Add($"#{ i } Vector 1: { Data.Type1[i].Matrix[0].X }; { Data.Type1[i].Matrix[0].Y }; { Data.Type1[i].Matrix[0].Z }; { Data.Type1[i].Matrix[0].W }");
                    text.Add($"#{ i } Vector 2: { Data.Type1[i].Matrix[1].X }; { Data.Type1[i].Matrix[1].Y }; { Data.Type1[i].Matrix[1].Z }; { Data.Type1[i].Matrix[1].W }");
                    text.Add($"#{ i } Vector 3: { Data.Type1[i].Matrix[2].X }; { Data.Type1[i].Matrix[2].Y }; { Data.Type1[i].Matrix[2].Z }; { Data.Type1[i].Matrix[2].W }");
                    text.Add($"#{ i } Vector 4: { Data.Type1[i].Matrix[3].X }; { Data.Type1[i].Matrix[3].Y }; { Data.Type1[i].Matrix[3].Z }; { Data.Type1[i].Matrix[3].W }");
                    text.Add($"#{ i } Vector 5: { Data.Type1[i].Matrix[4].X }; { Data.Type1[i].Matrix[4].Y }; { Data.Type1[i].Matrix[4].Z }; { Data.Type1[i].Matrix[4].W }");
                    text.Add($"#{ i } T3 Matrix 1: { Data.Type3[i].Matrix[0].X }; { Data.Type3[i].Matrix[0].Y }; { Data.Type3[i].Matrix[0].Z }; { Data.Type3[i].Matrix[0].W }");
                    text.Add($"#{ i } T3 Matrix 2: { Data.Type3[i].Matrix[1].X }; { Data.Type3[i].Matrix[1].Y }; { Data.Type3[i].Matrix[1].Z }; { Data.Type3[i].Matrix[1].W }");
                    text.Add($"#{ i } T3 Matrix 3: { Data.Type3[i].Matrix[2].X }; { Data.Type3[i].Matrix[2].Y }; { Data.Type3[i].Matrix[2].Z }; { Data.Type3[i].Matrix[2].W }");
                    text.Add($"#{ i } T3 Matrix 4: { Data.Type3[i].Matrix[3].X }; { Data.Type3[i].Matrix[3].Y }; { Data.Type3[i].Matrix[3].Z }; { Data.Type3[i].Matrix[3].W }");
                }
            }
            if (Data.Type2.Length > 0)
            {
                text.Add($"Type 2 Structs:");
                for (int i = 0; i < Data.Type2.Length; i++)
                {
                    text.Add($"#{ i } Numbers: { Data.Type2[i].Numbers[0] }; { Data.Type2[i].Numbers[1] }");
                    text.Add($"#{ i } Matrix 1: { Data.Type2[i].Matrix[0].X }; { Data.Type2[i].Matrix[0].Y }; { Data.Type2[i].Matrix[0].Z }; { Data.Type2[i].Matrix[0].W }");
                    text.Add($"#{ i } Matrix 2: { Data.Type2[i].Matrix[1].X }; { Data.Type2[i].Matrix[1].Y }; { Data.Type2[i].Matrix[1].Z }; { Data.Type2[i].Matrix[1].W }");
                    text.Add($"#{ i } Matrix 3: { Data.Type2[i].Matrix[2].X }; { Data.Type2[i].Matrix[2].Y }; { Data.Type2[i].Matrix[2].Z }; { Data.Type2[i].Matrix[2].W }");
                    text.Add($"#{ i } Matrix 4: { Data.Type2[i].Matrix[3].X }; { Data.Type2[i].Matrix[3].Y }; { Data.Type2[i].Matrix[3].Z }; { Data.Type2[i].Matrix[3].W }");
                }
            }

            TextPrev = text.ToArray();
        }
    }
}