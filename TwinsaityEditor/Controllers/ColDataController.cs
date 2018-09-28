using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public class ColDataController : ItemController
    {
        public new ColData Data { get; set; }

        public ColDataController(ColData item) : base(item)
        {
            Data = item;
            AddMenu("Open RMViewer", Menu_OpenRMViewer);
            AddMenu("Export Collision Model", Menu_Export);
        }

        protected override string GetName()
        {
            return "Collision Data [ID " + Data.ID + "]";
        }

        protected override void GenText()
        {
            TextPrev = new string[6 + Data.Triggers.Count + Data.Groups.Count + Data.Tris.Count + Data.Vertices.Count];
            TextPrev[0] = "ID: " + Data.ID;
            TextPrev[1] = "Offset: " + Data.Offset + " Size: " + Data.Size;

            TextPrev[2] = "TriggerCount: " + Data.Triggers.Count;
            for (int i = 0; i < Data.Triggers.Count; ++i)
                TextPrev[3 + i] = "Trigger" + i + ": " + "(" + Data.Triggers[i].X1 + ", " + Data.Triggers[i].Y1 + ", " + Data.Triggers[i].Z1 + ")~(" + Data.Triggers[i].X2 + ", " + Data.Triggers[i].Y2 + ", " + Data.Triggers[i].Z2 + ") | Nodes: " + Data.Triggers[i].Flag1 + "~" + Data.Triggers[i].Flag2;

            TextPrev[3 + Data.Triggers.Count] = "GroupCount: " + Data.Groups.Count;
            for (int i = 0; i < Data.Groups.Count; ++i)
                TextPrev[4 + Data.Triggers.Count + i] = "Group" + i + ": " + "PolygonOffset: " + Data.Groups[i].Offset + " PolygonCount: " + Data.Groups[i].Size;

            TextPrev[4 + Data.Triggers.Count + Data.Groups.Count] = "PolyCount: " + Data.Tris.Count;
            for (int i = 0; i < Data.Tris.Count; ++i)
                TextPrev[5 + Data.Triggers.Count + Data.Groups.Count + i] = "Polygon" + i + ": " + Data.Tris[i].Vert1 + "|" + Data.Tris[i].Vert2 + "|" + Data.Tris[i].Vert3 + "|" + Data.Tris[i].Surface;

            TextPrev[5 + Data.Triggers.Count + Data.Groups.Count + Data.Tris.Count] = "VertexCount: " + Data.Vertices.Count;
            for (int i = 0; i < Data.Vertices.Count; ++i)
                TextPrev[6 + Data.Triggers.Count + Data.Groups.Count + Data.Tris.Count + i] = "Vertex" + i + ": (" + Data.Vertices[i].X + ", " + Data.Vertices[i].Y + ", " + Data.Vertices[i].Z + ", " + Data.Vertices[i].W + ")";
        }

        private void Menu_OpenRMViewer()
        {
            MainForm.OpenRMViewer();
        }

        private void Menu_Export()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Wavefront OBJ file (*.obj)|*.obj";
            sfd.FileName = MainForm.SafeFileName;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write));
                writer.WriteLine("# Generated using the TwinsanityEditor @ https://github.com/Smartkin/twinsanity-editor");
                writer.WriteLine();
                foreach (var i in Data.Vertices)
                {
                    writer.WriteLine("v {0} {1} {2}", i.X, i.Y, i.Z);
                }
                Dictionary<int, List<ColData.ColTri>> polys = new Dictionary<int, List<ColData.ColTri>>();
                foreach (var i in Data.Tris)
                {
                    if (!polys.ContainsKey(i.Surface))
                        polys.Add(i.Surface, new List<ColData.ColTri>());
                    polys[i.Surface].Add(i);
                }
                //???
                foreach (var d in polys)
                {
                    writer.WriteLine("o Surface {0}", d.Key);
                    foreach (var i in d.Value)
                    {
                        writer.WriteLine("f {0} {1} {2}", i.Vert1 + 1, i.Vert2 + 1, i.Vert3 + 1);
                    }
                }
                writer.Close();
            }
        }
    }
}
