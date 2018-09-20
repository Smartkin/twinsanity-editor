using System.Collections.Generic;
using System;
using Twinsanity;

namespace TwinsaityEditor
{
    public class MeshController : ItemController
    {
        private Mesh data;

        public MeshController(Mesh item) : base(item)
        {
            data = item;
        }

        public override string GetName()
        {
            return "Mesh [ID: " + data.ID + "]";
        }

        public override void GenText()
        {
            var ex_lines = new List<string>();
            foreach (var i in data.SubModels)
            {
                ex_lines.Add("SubMesh" + data.SubModels.IndexOf(i));
                ex_lines.Add("VertexCount: " + i.VertexCount + " BlockSize: " + i.BlockSize);
                ex_lines.Add("K: " + i.k + " C: " + i.c);
                ex_lines.Add("GroupCount: " + i.Groups.Count);
                foreach (var j in i.Groups)
                    ex_lines.Add("VertexCount: " + j.VertexCount);
            }
            TextPrev = new string[3 + ex_lines.Count];
            TextPrev[0] = "ID: " + data.ID;
            TextPrev[1] = "Offset: " + data.Offset + " Size: " + data.Size;
            TextPrev[2] = "SubMesh Count: " + data.SubModels.Count;
            Array.Copy(ex_lines.ToArray(), 0, TextPrev, 3, ex_lines.Count);
        }
    }
}
