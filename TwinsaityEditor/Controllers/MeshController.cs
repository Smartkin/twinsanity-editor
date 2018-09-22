using System.Collections.Generic;
using System;
using Twinsanity;

namespace TwinsaityEditor
{
    public class MeshController : ItemController
    {
        public new Mesh Data { get; set; }

        public MeshController(Mesh item) : base(item)
        {
            Data = item;
        }

        public override string GetName()
        {
            return "Mesh [ID " + Data.ID + "]";
        }

        public override void GenText()
        {
            var ex_lines = new List<string>();
            foreach (var i in Data.SubModels)
            {
                ex_lines.Add("SubMesh" + Data.SubModels.IndexOf(i));
                ex_lines.Add("VertexCount: " + i.VertexCount + " BlockSize: " + i.BlockSize);
                ex_lines.Add("K: " + i.k + " C: " + i.c);
                ex_lines.Add("GroupCount: " + i.Groups.Count);
                foreach (var j in i.Groups)
                    ex_lines.Add("VertexCount: " + j.VertexCount);
            }
            TextPrev = new string[3 + ex_lines.Count];
            TextPrev[0] = "ID: " + Data.ID;
            TextPrev[1] = "Offset: " + Data.Offset + " Size: " + Data.Size;
            TextPrev[2] = "SubMesh Count: " + Data.SubModels.Count;
            Array.Copy(ex_lines.ToArray(), 0, TextPrev, 3, ex_lines.Count);
        }
    }
}
