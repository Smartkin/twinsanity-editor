using System;
using System.Collections.Generic;
using Twinsanity;

namespace TwinsaityEditor
{
    public class SceneryDataController : ItemController
    {
        public new SceneryData Data { get; set; }

        public SceneryDataController(MainForm topform, SceneryData item) : base(topform, item)
        {
            Data = item;
            //AddMenu("Open editor", Menu_OpenEditor);
        }

        protected override string GetName()
        {
            return $"Scenery Data [ID {Data.ID}]";
        }

        protected override void GenText()
        {
            List<string> text = new List<string>();

            text.Add($"ID: {Data.ID}");
            text.Add($"Size: {Data.Size}");
            text.Add(string.Format("Chunk Name: {0}", Data.ChunkName));
            text.Add(string.Format("Header Version: {0}", Data.HeaderVersion));
            text.Add(string.Format("Skydome ID: {0:X8}", Data.SkydomeID));
            text.Add(string.Format("Header: {0}; {1}; {2}; {3}", Data.HeaderUnk1, Data.HeaderUnk2, Data.HeaderUnk3, Data.HeaderUnk4));
            text.Add(string.Format("Vars: {0}; {1}; {2}; {3}; {4}; {5}", Data.HeaderUnkVars[0], Data.HeaderUnkVars[1], Data.HeaderUnkVars[2], Data.HeaderUnkVars[3], Data.HeaderUnkVars[4], Data.HeaderUnkVars[5]));
            text.Add(string.Format("Light Count: {0}", Data.LightsNum));
            text.Add(string.Format("Ambient {0}; Directional {1}; Point {2}; Negative {3}", Data.LightAmbientNum, Data.LightDirectionalNum, Data.LightPointNum, Data.LightNegativeNum));
            for (int i = 0; i < Data.LightsAmbient.Count; i++)
            {
                text.Add(string.Format("Ambient {0} Flags: {1:X}; {2:X}; {3:X}; {4:X};", i, Data.LightsAmbient[i].Flags[0], Data.LightsAmbient[i].Flags[1], Data.LightsAmbient[i].Flags[2], Data.LightsAmbient[i].Flags[3]));
                text.Add(string.Format("Ambient {0} Radius: {1}; R: {2}; G: {3}; B: {4}; F: {5}", i, Data.LightsAmbient[i].Radius, Data.LightsAmbient[i].Color_R, Data.LightsAmbient[i].Color_G, Data.LightsAmbient[i].Color_B, Data.LightsAmbient[i].UnkFloat));
                text.Add(string.Format("Ambient {0} Position: {1}; {2}; {3}; {4};", i, Data.LightsAmbient[i].Position.X, Data.LightsAmbient[i].Position.Y, Data.LightsAmbient[i].Position.Z, Data.LightsAmbient[i].Position.W));
                for (int v = 0; v < Data.LightsAmbient[i].Vectors.Length; v++)
                {
                    text.Add(string.Format("Ambient {0} Vector {1}: {2}; {3}; {4}; {5};", i, v + 1, Data.LightsAmbient[i].Vectors[v].X, Data.LightsAmbient[i].Vectors[v].Y, Data.LightsAmbient[i].Vectors[v].Z, Data.LightsAmbient[i].Vectors[v].W));
                }
            }
            for (int i = 0; i < Data.LightsDirectional.Count; i++)
            {
                text.Add(string.Format("Directional {0} Flags: {1:X}; {2:X}; {3:X}; {4:X}; Flags2: {5:X}; {6:X}", i, Data.LightsDirectional[i].Flags[0], Data.LightsDirectional[i].Flags[1], Data.LightsDirectional[i].Flags[2], Data.LightsDirectional[i].Flags[3], Data.LightsDirectional[i].Flags2[0], Data.LightsDirectional[i].Flags2[1]));
                text.Add(string.Format("Directional {0} Radius: {1}; R: {2}; G: {3}; B: {4}; F: {5}", i, Data.LightsDirectional[i].Radius, Data.LightsDirectional[i].Color_R, Data.LightsDirectional[i].Color_G, Data.LightsDirectional[i].Color_B, Data.LightsDirectional[i].UnkFloat));
                text.Add(string.Format("Directional {0} Position: {1}; {2}; {3}; {4};", i, Data.LightsDirectional[i].Position.X, Data.LightsDirectional[i].Position.Y, Data.LightsDirectional[i].Position.Z, Data.LightsDirectional[i].Position.W));
                for (int v = 0; v < Data.LightsDirectional[i].Vectors.Length; v++)
                {
                    text.Add(string.Format("Directional {0} Vector {1}: {2}; {3}; {4}; {5};", i, v + 1, Data.LightsDirectional[i].Vectors[v].X, Data.LightsDirectional[i].Vectors[v].Y, Data.LightsDirectional[i].Vectors[v].Z, Data.LightsDirectional[i].Vectors[v].W));
                }
            }
            for (int i = 0; i < Data.LightsPoint.Count; i++)
            {
                text.Add(string.Format("Point {0} Flags: {1:X}; {2:X}; {3:X}; {4:X}; Flags2: {5:X}; {6:X}", i, Data.LightsPoint[i].Flags[0], Data.LightsPoint[i].Flags[1], Data.LightsPoint[i].Flags[2], Data.LightsPoint[i].Flags[3], Data.LightsPoint[i].Flags2[0], Data.LightsPoint[i].Flags2[1]));
                text.Add(string.Format("Point {0} Radius: {1}; R: {2}; G: {3}; B: {4}; F: {5}", i, Data.LightsPoint[i].Radius, Data.LightsPoint[i].Color_R, Data.LightsPoint[i].Color_G, Data.LightsPoint[i].Color_B, Data.LightsPoint[i].UnkFloat));
                text.Add(string.Format("Point {0} Position: {1}; {2}; {3}; {4};", i, Data.LightsPoint[i].Position.X, Data.LightsPoint[i].Position.Y, Data.LightsPoint[i].Position.Z, Data.LightsPoint[i].Position.W));
                for (int v = 0; v < Data.LightsPoint[i].Vectors.Length; v++)
                {
                    text.Add(string.Format("Directional {0} Vector {1}: {2}; {3}; {4}; {5};", i, v + 1, Data.LightsPoint[i].Vectors[v].X, Data.LightsPoint[i].Vectors[v].Y, Data.LightsPoint[i].Vectors[v].Z, Data.LightsPoint[i].Vectors[v].W));
                }
            }
            for (int i = 0; i < Data.LightsNegative.Count; i++)
            {
                text.Add(string.Format("Negative {0} Flags: {1:X}; {2:X}; {3:X}; {4:X};", i, Data.LightsNegative[i].Flags[0], Data.LightsNegative[i].Flags[1], Data.LightsNegative[i].Flags[2], Data.LightsNegative[i].Flags[3]));
                text.Add(string.Format("Negative {0} Radius: {1}; R: {2}; G: {3}; B: {4}; F: {5}", i, Data.LightsNegative[i].Radius, Data.LightsNegative[i].Color_R, Data.LightsNegative[i].Color_G, Data.LightsNegative[i].Color_B, Data.LightsNegative[i].UnkFloat));
                text.Add(string.Format("Negative {0} Position: {1}; {2}; {3}; {4};", i, Data.LightsNegative[i].Position.X, Data.LightsNegative[i].Position.Y, Data.LightsNegative[i].Position.Z, Data.LightsNegative[i].Position.W));
                for (int v = 0; v < Data.LightsNegative[i].Vectors.Length; v++)
                {
                    text.Add(string.Format("Negative {0} Vector {1}: {2}; {3}; {4}; {5};", i, v + 1, Data.LightsNegative[i].Vectors[v].X, Data.LightsNegative[i].Vectors[v].Y, Data.LightsNegative[i].Vectors[v].Z, Data.LightsNegative[i].Vectors[v].W));
                }
                text.Add(string.Format("Negative {0} Floats: {1}; {2}; {3}; {4}; {5}", i, Data.LightsNegative[i].Floats[0], Data.LightsNegative[i].Floats[1], Data.LightsNegative[i].Floats[2], Data.LightsNegative[i].Floats[3], Data.LightsNegative[i].Floats[4]));
            }
            text.Add(string.Format("UnkVar: {0}", Data.unkVar5));

            if (Data.sceneryModels.Count > 0)
            {
                for (int i = 0; i < Data.sceneryModels.Count; i++)
                {
                    text.Add("\n");
                    text.Add("Scenery " + i);
                    /*
                    text.Add($"Model Count: { Data.sceneryModels[i].ModelIDs.Length }");
                    text.Add($"Special Model Count: { Data.sceneryModels[i].SpecialModelIDs.Length }");
                    */
                    for (int a = 0; a < Data.sceneryModels[i].Models.Count; a++)
                    {
                        if (!Data.sceneryModels[i].Models[a].isSpecial)
                        {
                            text.Add(string.Format("Model {0} ID: {1:X8}", a, Data.sceneryModels[i].Models[a].ModelID));
                        }
                        else
                        {
                            text.Add(string.Format("Special Model {0} ID: {1:X8}", a, Data.sceneryModels[i].Models[a].ModelID));
                        }
                        //text.Add($"Model {a} Bounding Box Vector 1: { Data.sceneryModels[i].Models[a].ModelBoundingBoxVector1.X }; { Data.sceneryModels[i].Models[a].ModelBoundingBoxVector1.Y }; { Data.sceneryModels[i].Models[a].ModelBoundingBoxVector1.Z }; { Data.sceneryModels[i].Models[a].ModelBoundingBoxVector1.W }");
                        //text.Add($"Model {a} Bounding Box Vector 2: { Data.sceneryModels[i].Models[a].ModelBoundingBoxVector2.X }; { Data.sceneryModels[i].Models[a].ModelBoundingBoxVector2.Y }; { Data.sceneryModels[i].Models[a].ModelBoundingBoxVector2.Z }; { Data.sceneryModels[i].Models[a].ModelBoundingBoxVector2.W }");
                        text.Add($"Model {a} Matrix 1: { Data.sceneryModels[i].Models[a].ModelMatrix[0].X }; { Data.sceneryModels[i].Models[a].ModelMatrix[0].Y }; { Data.sceneryModels[i].Models[a].ModelMatrix[0].Z }; { Data.sceneryModels[i].Models[a].ModelMatrix[0].W }");
                        text.Add($"Model {a} Matrix 2: { Data.sceneryModels[i].Models[a].ModelMatrix[1].X }; { Data.sceneryModels[i].Models[a].ModelMatrix[1].Y }; { Data.sceneryModels[i].Models[a].ModelMatrix[1].Z }; { Data.sceneryModels[i].Models[a].ModelMatrix[1].W }");
                        text.Add($"Model {a} Matrix 3: { Data.sceneryModels[i].Models[a].ModelMatrix[2].X }; { Data.sceneryModels[i].Models[a].ModelMatrix[2].Y }; { Data.sceneryModels[i].Models[a].ModelMatrix[2].Z }; { Data.sceneryModels[i].Models[a].ModelMatrix[2].W }");
                        text.Add($"Model {a} Matrix 4 (Position): { Data.sceneryModels[i].Models[a].ModelMatrix[3].X }; { Data.sceneryModels[i].Models[a].ModelMatrix[3].Y }; { Data.sceneryModels[i].Models[a].ModelMatrix[3].Z }; { Data.sceneryModels[i].Models[a].ModelMatrix[3].W }");
                    }
                    for (int a = 0; a < Data.sceneryModels[i].UnkStruct.UnkPos.Length; a++)
                    {
                        text.Add($"Scenery {i} Vector {a}: { Data.sceneryModels[i].UnkStruct.UnkPos[a].X }; { Data.sceneryModels[i].UnkStruct.UnkPos[a].Y }; { Data.sceneryModels[i].UnkStruct.UnkPos[a].Z }; { Data.sceneryModels[i].UnkStruct.UnkPos[a].W };");
                    }
                }
            }

            if (Data.unkStructs.Count > 0)
            {
                for (int i = 0; i < Data.unkStructs.Count; i++)
                {
                    text.Add("\n");
                    text.Add("UnkStruct " + i);
                    text.Add($"Struct {i} Matrix 1: { Data.unkStructs[i].UnkPos[0].X }; { Data.unkStructs[i].UnkPos[0].Y }; { Data.unkStructs[i].UnkPos[0].Z }; { Data.unkStructs[i].UnkPos[0].W }");
                    text.Add($"Struct {i} Matrix 2: { Data.unkStructs[i].UnkPos[1].X }; { Data.unkStructs[i].UnkPos[1].Y }; { Data.unkStructs[i].UnkPos[1].Z }; { Data.unkStructs[i].UnkPos[1].W }");
                    text.Add($"Struct {i} Matrix 3: { Data.unkStructs[i].UnkPos[2].X }; { Data.unkStructs[i].UnkPos[2].Y }; { Data.unkStructs[i].UnkPos[2].Z }; { Data.unkStructs[i].UnkPos[2].W }");
                    text.Add($"Struct {i} Matrix 4: { Data.unkStructs[i].UnkPos[3].X }; { Data.unkStructs[i].UnkPos[3].Y }; { Data.unkStructs[i].UnkPos[3].Z }; { Data.unkStructs[i].UnkPos[3].W }");
                    //text.Add($"Struct {i} Matrix 5: { Data.unkStructs[i].UnkPos[4].X }; { Data.unkStructs[i].UnkPos[4].Y }; { Data.unkStructs[i].UnkPos[4].Z }; { Data.unkStructs[i].UnkPos[4].W }");
                    //text.Add($"Struct {i} Vars: { Data.unkStructs[i].UnkInt[0] }; { Data.unkStructs[i].UnkInt[1] }; { Data.unkStructs[i].UnkInt[2] }; { Data.unkStructs[i].UnkInt[3] }; { Data.unkStructs[i].UnkInt[4] }; { Data.unkStructs[i].UnkInt[5] }; { Data.unkStructs[i].UnkInt[6] }; { Data.unkStructs[i].UnkInt[7] };");
                }
            }

            TextPrev = text.ToArray();
        }

        private void Menu_OpenEditor()
        {
            MainFile.OpenEditor((ItemController)Node.Tag);
        }
    }
}