using Twinsanity;
using System.Collections.Generic;

namespace TwinsaityEditor
{
    public class GraphicsInfoMBController : ItemController
    {
        public GraphicsInfoMB Container { get; set; }
        public new GraphicsInfo Data { get; set; }
        public GHG_Actor_MB Actor { get; set; }

        public GraphicsInfoMBController(MainForm topform, GraphicsInfoMB item) : base(topform, item)
        {
            Container = item;
            Data = Container.OGI;
            Actor = Container.GHG;
        }

        protected override string GetName()
        {
            return $"Graphics Info [ID {Container.ID}]";
        }

        protected override void GenText()
        {
            List<string> text = new List<string>();
            text.Add($"ID: {Data.ID}");
            text.Add($"Bounding Box Vector 1: {Data.Coord1.X}; {Data.Coord1.Y}; {Data.Coord1.Z}; {Data.Coord1.W}");
            text.Add($"Bounding Box Vector 2: {Data.Coord2.X}; {Data.Coord2.Y}; {Data.Coord2.Z}; {Data.Coord2.W}");
            text.Add($"Skin ID: {Data.SkinID}");
            text.Add($"Blend Skin Model ID: {Data.BlendSkinID}");
            text.Add($"Joints: {Data.Type1.Length}");
            text.Add($"Exit Points: {Data.Type2.Length}");
            text.Add($"Collision Datas Size: {Data.CollisionData.Length}");
            text.Add($"Rigid Model Count: {Data.ModelIDs.Length}");
            text.Add(string.Empty);
            if (Data.ModelIDs.Length > 0)
            {
                for (int i = 0; i < Data.ModelIDs.Length; i++)
                {
                    text.Add(string.Format("Rigid Model ID: {1:X8} - Attached to Joint {0}", Data.ModelIDs[i].ID, Data.ModelIDs[i].ModelID));
                }
            }
            text.Add(string.Empty);
            if (Data.Type1.Length > 0)
            {
                text.Add($"Joints:");
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
            text.Add(string.Empty);
            if (Data.Type2.Length > 0)
            {
                text.Add($"Exit Points:");
                for (int i = 0; i < Data.Type2.Length; i++)
                {
                    text.Add($"#{ i } Attached to Joint { Data.Type2[i].Numbers[0] }; ID: { Data.Type2[i].Numbers[1] }");
                    text.Add($"#{ i } Matrix 1: { Data.Type2[i].Matrix[0].X }; { Data.Type2[i].Matrix[0].Y }; { Data.Type2[i].Matrix[0].Z }; { Data.Type2[i].Matrix[0].W }");
                    text.Add($"#{ i } Matrix 2: { Data.Type2[i].Matrix[1].X }; { Data.Type2[i].Matrix[1].Y }; { Data.Type2[i].Matrix[1].Z }; { Data.Type2[i].Matrix[1].W }");
                    text.Add($"#{ i } Matrix 3: { Data.Type2[i].Matrix[2].X }; { Data.Type2[i].Matrix[2].Y }; { Data.Type2[i].Matrix[2].Z }; { Data.Type2[i].Matrix[2].W }");
                    text.Add($"#{ i } Matrix 4: { Data.Type2[i].Matrix[3].X }; { Data.Type2[i].Matrix[3].Y }; { Data.Type2[i].Matrix[3].Z }; { Data.Type2[i].Matrix[3].W }");
                }
            }
            text.Add(string.Empty);
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
            }

            TextPrev = text.ToArray();
            return;

            text.Add(string.Empty);
            text.Add($"GHG Data");
            text.Add(string.Empty);

            text.Add($"Struct2 Count: {Actor.Structs2.Count}");

            for (int i = 0; i < Actor.Header2.Count; i++)
            {
                text.Add($"FLOAT1: " + Actor.Header2[i]);
            }
            for (int i = 0; i < Actor.Header3.Count; i++)
            {
                text.Add($"FLOAT2: " + Actor.Header3[i]);
            }

            text.Add(string.Empty);
            text.Add($"Bounding Box Vector 1: {Actor.BoundingBoxVector1.X}; {Actor.BoundingBoxVector1.Y}; {Actor.BoundingBoxVector1.Z};");
            text.Add($"Bounding Box Vector 2: {Actor.BoundingBoxVector2.X}; {Actor.BoundingBoxVector2.Y}; {Actor.BoundingBoxVector2.Z};");

            text.Add(string.Empty);
            text.Add($"Texture/Material? Count: {Actor.Textures.Count}");
            for (int i = 0; i < Actor.Textures.Count; i++)
            {
                //text.Add($"Texture: {Actor.Textures[i].Width}x{Actor.Textures[i].Height}");
            }

            text.Add(string.Empty);
            text.Add($"Exit Points: " + Actor.ExitPoints.Count);
            for (int i = 0; i < Actor.ExitPoints.Count; i++)
            {
                text.Add($"ID {Actor.ExitPoints[i].ID}: {Actor.ExitPoints[i].Name} - Attached to: {Actor.Joints[(int)Actor.ExitPoints[i].Joint].Name}");
                /*
                for (int d = 0; d < Data.ExitPoints[i].Matrix.Length; d++)
                {
                    text.Add($"{Data.ExitPoints[i].Matrix[d].X}; {Data.ExitPoints[i].Matrix[d].Y}; {Data.ExitPoints[i].Matrix[d].X}; {Data.ExitPoints[i].Matrix[d].W}");
                }
                */
                //text.Add($"{Data.ExitPoints[i].UnkInt2} {Data.ExitPoints[i].UnkInt3}");
                //text.Add(string.Empty);
            }

            /*
            text.Add(string.Empty);
            text.Add($"Locators: " + Data.Locators.Count);
            for (int i = 0; i < Data.Locators.Count; i ++)
            {
                text.Add(Data.Locators[i]);
            }
            */

            text.Add(string.Empty);
            text.Add($"Joint Count: {Actor.Joints.Count}");
            if (Actor.Joints.Count > 0)
            {
                for (int i = 0; i < Actor.Joints.Count; i++)
                {
                    // There CAN be more than one root joint like in spdog, but the game won't load it
                    if (Actor.Joints[i].ParentJoint == 255)
                    {
                        PrintJointTree(i, "", text);
                    }
                }
            }
            /*
            for (int i = 0; i < Data.Joints.Count; i++)
            {
                if (Data.Joints[i].ParentJoint != 255)
                {
                    text.Add($"#{i}: {Data.Joints[i].Name} - Parent Joint: {Data.Joints[Data.Joints[i].ParentJoint].Name}");
                }
                else
                {
                    text.Add($"#{i}: {Data.Joints[i].Name} - Root Joint");
                }
            }
            */
            /*
            for (int d = 0; d < Data.Joints[i].Matrix.Length; d++)
            {
                text.Add($"1: {Data.Joints[i].Matrix[d].X}; {Data.Joints[i].Matrix[d].Y}; {Data.Joints[i].Matrix[d].X}; {Data.Joints[i].Matrix[d].W}");
            }
            for (int d = 0; d < Data.Joints[i].Matrix2.Length; d++)
            {
                text.Add($"2: {Data.Joints[i].Matrix2[d].X}; {Data.Joints[i].Matrix2[d].Y}; {Data.Joints[i].Matrix2[d].X}; {Data.Joints[i].Matrix2[d].W}");
            }
            text.Add($"{Data.Joints[i].UnkInt1} {Data.Joints[i].UnkInt2} {Data.Joints[i].UnkInt3}");
            text.Add($"{Data.Joints[i].UnkByte} {Data.Joints[i].UnkInt5} {Data.Joints[i].UnkInt6} {Data.Joints[i].UnkInt7}");
            */
            //text.Add(string.Empty);
            //}

            text.Add(string.Empty);
            text.Add($"Layers: {Actor.Layers.Count}");

            /*
            text.Add(string.Empty);
            for (int a = 0; a < Actor.Layers.Count; a++)
            {
                GHG_Actor.Layer layer = Data.Layers[a];
                text.Add($"Layer {a}: {layer.LayerName}");
                text.Add($"Rigid Models: {Data.Layers[a].RigidModels.Count}");
                for (int i = 0; i < layer.RigidModels.Count; i++)
                {
                    GHG_Actor.RigidModel model = layer.RigidModels[i];
                    text.Add($"Rigid Model {i}: Verts {model.Vertices.Count} | Groups {model.GroupCount} | Attached to joint {Data.Joints[model.Joint].Name} ");

                }
                text.Add(string.Empty);
                text.Add($"Skins: {Data.Layers[a].Skins.Count}");
                for (int i = 0; i < layer.Skins.Count; i++)
                {
                    GHG_Actor.Skin skin = layer.Skins[i];
                    text.Add($"Skin {i}: Verts {skin.Vertices.Count} Groups {skin.GroupCount} ");
                }
                text.Add(string.Empty);
                text.Add($"Blend Skins: {Data.Layers[a].BlendSkins.Count}");
                for (int i = 0; i < layer.BlendSkins.Count; i++)
                {
                    GHG_Actor.BlendSkin skin = layer.BlendSkins[i];
                    text.Add($"Blend Skin {i}: Verts {skin.Vertices.Count} Groups {skin.GroupCount} ");

                }
            }
            */


            TextPrev = text.ToArray();
        }

        void PrintJointTree(int Joint, string Add, List<string> Text)
        {
            if (Actor.Joints[Joint].ID != null)
            {
                Text.Add($"{Add}{Actor.Joints[Joint].Name} [ID: {Actor.Joints[Joint].ID}]");
            }
            else
            {
                Text.Add($"{Add}{Actor.Joints[Joint].Name}");
            }
            for (int i = 0; i < Actor.Joints.Count; i++)
            {
                if (Actor.Joints[i].ParentJoint == Joint)
                {
                    PrintJointTree(i, Add + "--", Text);
                }
            }
        }
    }
}
 
 