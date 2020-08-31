using System;
using System.Collections.Generic;
using Twinsanity;

namespace TwinsaityEditor
{
    public class DynamicSceneryDataController : ItemController
    {
        public new DynamicSceneryData Data { get; set; }

        public DynamicSceneryDataController(MainForm topform, DynamicSceneryData item) : base(topform, item)
        {
            Data = item;
        }

        protected override string GetName()
        {
            return $"Dynamic Scenery Data [ID {Data.ID}]";
        }

        protected override void GenText()
        {
            List<string> text = new List<string>();

            text.Add($"ID: {Data.ID}");
            text.Add($"Size: {Data.Size}");
            text.Add($"Model Count: {Data.ModelCount}");

            if (Data.ModelCount > 0)
            {
                for (int i = 0; i < Data.ModelCount; i++)
                {
                    text.Add($"Model {i} ID: {string.Format("{0:X8}", Data.Models[i].ModelID)} Name: {Data.Models[i].Name}");
                    //text.Add($"Model {i} Pos: {Data.Models[i].Position.X}; {Data.Models[i].Position.Y}; {Data.Models[i].Position.Z}; {Data.Models[i].Position.W} ");
                    if (Data.Models[i].UnkVectors.Length > 0)
                    {
                        /*
                        for (int a = 0; a < Data.Models[i].UnkVectors.Length; a++)
                        {
                            text.Add($"Model {i} Vector1 {a}: {Data.Models[i].UnkVectors[a].X}; {Data.Models[i].UnkVectors[a].Y}; {Data.Models[i].UnkVectors[a].Z}; {Data.Models[i].UnkVectors[a].W}; ");
                        }
                        for (int a = 0; a < Data.Models[i].UnkVectors2.Length; a++)
                        {
                            text.Add($"Model {i} Vector2 {a}: {Data.Models[i].UnkVectors2[a].X}; {Data.Models[i].UnkVectors2[a].Y}; {Data.Models[i].UnkVectors2[a].Z}; {Data.Models[i].UnkVectors2[a].W}; ");
                        }
                        for (int a = 0; a < Data.Models[i].UnkVectors3.Length; a++)
                        {
                            text.Add($"Model {i} Vector3 {a}: {Data.Models[i].UnkVectors3[a].X}; {Data.Models[i].UnkVectors3[a].Y}; {Data.Models[i].UnkVectors3[a].Z}; {Data.Models[i].UnkVectors3[a].W}; ");
                        }
                        */

                        text.Add($"Model {i} Position: {Data.Models[i].WorldPosition.X}; {Data.Models[i].WorldPosition.Y}; {Data.Models[i].WorldPosition.Z}; {Data.Models[i].WorldPosition.W}; ");
                        text.Add($"Model {i} Rotation?: {Data.Models[i].LocalRotation[0]}; {Data.Models[i].LocalRotation[1]}; {Data.Models[i].LocalRotation[2]}; ");
                        //text.Add($"Model {i} Bounding Box Vector 1: {Data.Models[i].BoundingBoxVector1.X}; {Data.Models[i].BoundingBoxVector1.Y}; {Data.Models[i].BoundingBoxVector1.Z}; {Data.Models[i].BoundingBoxVector1.W}; ");
                        //text.Add($"Model {i} Bounding Box Vector 2: {Data.Models[i].BoundingBoxVector2.X}; {Data.Models[i].BoundingBoxVector2.Y}; {Data.Models[i].BoundingBoxVector2.Z}; {Data.Models[i].BoundingBoxVector2.W}; ");
                    }
                    //text.Add(string.Format("Model {0} Var1: {1:X8}", i, Data.Models[i].UnkVar1));
                    //text.Add($"Model {i} One: {Data.Models[i].UnkOne}");
                    /*
                    if (Data.Models[i].UnkVars.Length > 0)
                    {
                        text.Add($"Model {i} Vars: {Data.Models[i].UnkVars[0]}; {Data.Models[i].UnkVars[1]}; {Data.Models[i].UnkVars[2]}; {Data.Models[i].UnkVars[3]}; {Data.Models[i].UnkVars[4]}; {Data.Models[i].UnkVars[5]}; {Data.Models[i].UnkVars[6]}; {Data.Models[i].UnkVars[7]}; {Data.Models[i].UnkVars[8]}; {Data.Models[i].UnkVars[9]}; {Data.Models[i].UnkVars[10]}; {Data.Models[i].UnkVars[11]}; {Data.Models[i].UnkVars[12]}; ");
                    }
                    */
                    text.Add("");
                }
            }

            TextPrev = text.ToArray();
        }
    }
}