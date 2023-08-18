using System;
using System.Collections.Generic;
using Twinsanity;
using System.Text;

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
            text.Add($"Model Count: {Data.Models.Count}");

            if (Data.Models.Count > 0)
            {
                for (int i = 0; i < Data.Models.Count; i++)
                {
                    text.Add($"Model {i} ID: {string.Format("{0:X8}", Data.Models[i].ModelID)} Var: {Data.Models[i].UnkInt1}, Frame Count: {Data.Models[i].unkInt2}, Zero: {Data.Models[i].unkByte}");

                    if (Data.Models[i].GI_Types.Count > 0)
                    {
                        for (int g = 0; g < Data.Models[i].GI_Types.Count; g++)
                        {
                            text.Add($"GI {g}: Blob Size: {Data.Models[i].GI_Types[g].unkBlob.Length}");
                            /*
                            string HBlob = "";
                            for (int b = 0; b < Data.Models[i].GI_Types[g].Header.Length; b++)
                            {
                                HBlob += string.Format("{0:X}", Data.Models[i].GI_Types[g].Header[b]);
                            }
                            text.Add($"GI Header: {HBlob}");
                            
                            string Blob = "";
                            for (int b = 0; b < Data.Models[i].GI_Types[g].unkBlob.Length; b++)
                            {
                                Blob += string.Format("{0:X}", Data.Models[i].GI_Types[g].unkBlob[b]);
                            }
                            text.Add($"Blob: {Blob}");
                            */

                        }
                    }

                    text.Add($"DynBlob Length: {Data.Models[i].dynBlob.Length} Header: {Data.Models[i].AnimFlags:X2} / {Data.Models[i].AnimInt} / {Data.Models[i].AnimByte1} / {Data.Models[i].AnimByte2} ");
                    text.Add($"Model {i} Initial Position: {Data.Models[i].WorldPosition.X}; {Data.Models[i].WorldPosition.Y}; {Data.Models[i].WorldPosition.Z}; {Data.Models[i].WorldPosition.W}; ");
                    text.Add($"Model {i} Initial Rotation: {Data.Models[i].WorldRotation.X}; {Data.Models[i].WorldRotation.Y}; {Data.Models[i].WorldRotation.Z}; {Data.Models[i].WorldRotation.W}; ");
                }
            }

            TextPrev = text.ToArray();
        }
    }
}