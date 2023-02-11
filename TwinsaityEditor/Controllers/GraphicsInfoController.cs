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
            //if (MainFile.Data.Type == TwinsFile.FileType.RMX)
            {
                AddMenu("Open mesh viewer", Menu_OpenViewer);
            }
        }



        protected override string GetName()
        {
            return $"Graphics Info [ID {Data.ID}]";
        }

        protected override void GenText()
        {
            List<string> text = new List<string>
            {
                $"ID: {Data.ID}",
                $"Bounding Box Vector 1: {Data.Coord1.X}; {Data.Coord1.Y}; {Data.Coord1.Z}; {Data.Coord1.W}",
                $"Bounding Box Vector 2: {Data.Coord2.X}; {Data.Coord2.Y}; {Data.Coord2.Z}; {Data.Coord2.W}",
                $"Skin ID: {Data.SkinID.ToString("X8")}",
                $"Blend Skin Model ID: {Data.BlendSkinID.ToString("X8")}",
                $"Joint Count: {Data.Joints.Length}",
                $"Exit Point Count: {Data.ExitPoints.Length}",
                $"Parent Joint Count: {Data.HeaderVars[2]}",
                $"Collision Datas Size: {Data.CollisionData.Length}",
                $"Rigid Model Count: {Data.ModelIDs.Count}"
            };
            if (Data.ModelIDs.Count > 0)
            {
                foreach(var pair in Data.ModelIDs)
                {
                    text.Add(string.Format("Rigid Model ID: {1:X8} Attached to Joint #{0}", pair.Value, pair.Key));
                }
            }

            if (Data.Joints.Length > 0)
            {
                text.Add($"Joints:");
                for (int i = 0; i < Data.Joints.Length; i++)
                {
                    text.Add($"#{ i } React Joint-ID: { Data.Joints[i].ReactJointID }; Joint index: { Data.Joints[i].JointIndex }; Parent Joint: { Data.Joints[i].ParentJointIndex }; Child joints: { Data.Joints[i].ChildJointAmount }; { Data.Joints[i].ChildJointAmount2 }");
                    //text.Add($"#{ i } Numbers: { Data.Type1[i].Numbers[0] }; { Data.Type1[i].Numbers[1] }; { Data.Type1[i].Numbers[2] }; { Data.Type1[i].Numbers[3] }; { Data.Type1[i].Numbers[4] }");
                    text.Add($"\t#{ i } Local Space Position: { Data.Joints[i].Matrix[0].X }; { Data.Joints[i].Matrix[0].Y }; { Data.Joints[i].Matrix[0].Z }; { Data.Joints[i].Matrix[0].W }");
                    text.Add($"\t#{ i } World Space Position: { Data.Joints[i].Matrix[1].X }; { Data.Joints[i].Matrix[1].Y }; { Data.Joints[i].Matrix[1].Z }; { Data.Joints[i].Matrix[1].W }");
                    text.Add($"\t#{ i } Local Space Rotation: { Data.Joints[i].Matrix[2].X }; { Data.Joints[i].Matrix[2].Y }; { Data.Joints[i].Matrix[2].Z }; { Data.Joints[i].Matrix[2].W }");
                    text.Add($"\t#{ i } Vector 4: { Data.Joints[i].Matrix[3].X }; { Data.Joints[i].Matrix[3].Y }; { Data.Joints[i].Matrix[3].Z }; { Data.Joints[i].Matrix[3].W }");
                    text.Add($"\t#{ i } Vector 5: { Data.Joints[i].Matrix[4].X }; { Data.Joints[i].Matrix[4].Y }; { Data.Joints[i].Matrix[4].Z }; { Data.Joints[i].Matrix[4].W }");
                    text.Add($"\t#{ i } T3 Matrix 1: { Data.Type3[i].Matrix[0].X }; { Data.Type3[i].Matrix[0].Y }; { Data.Type3[i].Matrix[0].Z }; { Data.Type3[i].Matrix[0].W }");
                    text.Add($"\t#{ i } T3 Matrix 2: { Data.Type3[i].Matrix[1].X }; { Data.Type3[i].Matrix[1].Y }; { Data.Type3[i].Matrix[1].Z }; { Data.Type3[i].Matrix[1].W }");
                    text.Add($"\t#{ i } T3 Matrix 3: { Data.Type3[i].Matrix[2].X }; { Data.Type3[i].Matrix[2].Y }; { Data.Type3[i].Matrix[2].Z }; { Data.Type3[i].Matrix[2].W }");
                    text.Add($"\t#{ i } T3 Matrix 4: { Data.Type3[i].Matrix[3].X }; { Data.Type3[i].Matrix[3].Y }; { Data.Type3[i].Matrix[3].Z }; { Data.Type3[i].Matrix[3].W }");
                }
            }
            if (Data.ExitPoints.Length > 0)
            {
                text.Add($"Exit Points:");
                for (int i = 0; i < Data.ExitPoints.Length; i++)
                {
                    text.Add($"#{ i } Parent Joint: { Data.ExitPoints[i].Numbers[0] }; ID: { Data.ExitPoints[i].Numbers[1] }");
                    //text.Add($"#{ i } Numbers: { Data.Type2[i].Numbers[0] }; { Data.Type2[i].Numbers[1] }");
                    text.Add($"\t#{ i } Matrix 1: { Data.ExitPoints[i].Matrix[0].X }; { Data.ExitPoints[i].Matrix[0].Y }; { Data.ExitPoints[i].Matrix[0].Z }; { Data.ExitPoints[i].Matrix[0].W }");
                    text.Add($"\t#{ i } Matrix 2: { Data.ExitPoints[i].Matrix[1].X }; { Data.ExitPoints[i].Matrix[1].Y }; { Data.ExitPoints[i].Matrix[1].Z }; { Data.ExitPoints[i].Matrix[1].W }");
                    text.Add($"\t#{ i } Matrix 3: { Data.ExitPoints[i].Matrix[2].X }; { Data.ExitPoints[i].Matrix[2].Y }; { Data.ExitPoints[i].Matrix[2].Z }; { Data.ExitPoints[i].Matrix[2].W }");
                    text.Add($"\t#{ i } Matrix 4: { Data.ExitPoints[i].Matrix[3].X }; { Data.ExitPoints[i].Matrix[3].Y }; { Data.ExitPoints[i].Matrix[3].Z }; { Data.ExitPoints[i].Matrix[3].W }");
                }
            }
            if (Data.CollisionData.Length > 0)
            {
                text.Add($"Collision data information:");
                for (var i = 0; i < Data.CollisionData.Length; ++i)
                {
                    var type4 = Data.CollisionData[i];
                    text.Add($"#{ i } Header:");
                    for (var j = 0; j < 11; ++j)
                    {
                        text.Add($" {type4.Header[j]}");
                    }
                    for (var j = 0; j < 7; ++j)
                    {
                        text.Add($"Blob block {j + 1}");
                        switch (j)
                        {
                            case 0:
                                text.Add($"\tSize: {type4.Header[5]}");
                                for (var k = 0; k < type4.Header[j]; ++k)
                                {
                                    text.Add($"\t{type4.UnkVectors1[k]}");
                                }
                                break;
                            case 1:
                                text.Add($"\tSize: {type4.Header[6] - type4.Header[5]}");
                                break;
                            case 2:
                                text.Add($"\tSize: {type4.Header[7] - type4.Header[6]}");
                                break;
                            case 3:
                                text.Add($"\tSize: {type4.Header[8] - type4.Header[7]}");
                                break;
                            case 4:
                                text.Add($"\tSize: {type4.Header[9] - type4.Header[8]}");
                                break;
                            case 5:
                                text.Add($"\tSize: {type4.Header[10] - type4.Header[9]}");
                                break;
                            case 6:
                                text.Add($"\tSize: {type4.collisionDataBlob.Length - type4.Header[10]}");
                                break;
                        }
                    }
                }
                text.Add("Collision data indices:\n");
                foreach (var b in Data.CollisionDataRelated)
                {
                    text.Add($"\t{b}");
                }
            }

            TextPrev = text.ToArray();
        }

        private void Menu_OpenViewer()
        {
            MainFile.OpenMeshViewer(this, MainFile);
        }
    }
}