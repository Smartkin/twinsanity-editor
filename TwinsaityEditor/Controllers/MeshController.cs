using System.Collections.Generic;
using System;
using System.IO;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public class MeshController : ItemController
    {
        public new Mesh Data { get; set; }

        public MeshController(Mesh item) : base(item)
        {
            Data = item;
            AddMenu("Export to PLY", Menu_ExportPLY);
        }

        protected override string GetName()
        {
            return "Mesh [ID " + Data.ID + "]";
        }

        protected override void GenText()
        {
            //TODO use array
            var ex_lines = new List<string>();
            for (int i = 0; i < Data.SubModels.Count; ++i)
            {
                var sub = Data.SubModels[i];
                ex_lines.Add("SubMesh" + i);
                ex_lines.Add("VertexCount: " + sub.VertexCount + " BlockSize: " + sub.BlockSize);
                ex_lines.Add("K: " + sub.k + " C: " + sub.c);
                ex_lines.Add("GroupCount: " + sub.Groups.Count);
                foreach (var j in sub.Groups)
                    ex_lines.Add("VertexCount: " + j.VertexCount);
            }
            TextPrev = new string[3 + ex_lines.Count];
            TextPrev[0] = "ID: " + Data.ID;
            TextPrev[1] = "Offset: " + Data.Offset + " Size: " + Data.Size;
            TextPrev[2] = "SubMesh Count: " + Data.SubModels.Count;
            Array.Copy(ex_lines.ToArray(), 0, TextPrev, 3, ex_lines.Count);
        }

        private void Menu_ExportPLY()
        {
            SaveFileDialog sfd = new SaveFileDialog { Filter = "PLY files (*.ply)|*.ply", FileName = GetName() };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(sfd.FileName, Data.ToPLY());
            }
        }
    }
}
