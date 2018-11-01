using Twinsanity;
using System.IO;
using System.Windows.Forms;

namespace TwinsaityEditor
{
    public class ModelController : ItemController
    {
        public new Model Data { get; set; }

        public ModelController(Model item) : base(item)
        {
            Data = item;
            AddMenu("Export mesh to PLY", Menu_ExportPLY);
        }

        protected override string GetName()
        {
            return "Model [ID " + Data.ID + "]";
        }

        protected override void GenText()
        {
            TextPrev = new string[4 + Data.MaterialIDs.Length];
            TextPrev[0] = "ID: " + Data.ID;
            TextPrev[1] = "Offset: " + Data.Offset + " Size: " + Data.Size;
            TextPrev[2] = "Header: " + Data.Header + " MaterialCount: " + Data.MaterialIDs.Length;
            for (int i = 0; i < Data.MaterialIDs.Length; ++i)
                TextPrev[3 + i] = FileController.GetMaterialName(Data.MaterialIDs[i]);
            TextPrev[3 + Data.MaterialIDs.Length] = "Mesh: " + Data.MeshID;
        }

        private void Menu_ExportPLY()
        {
            if (MessageBox.Show("PLY export is experimental, material and texture information will not be exported. Continue anyway?", "Export Warning", MessageBoxButtons.YesNo) == DialogResult.No) return;
            SaveFileDialog sfd = new SaveFileDialog { Filter = "PLY files (*.ply)|*.ply", FileName = GetName() };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(sfd.FileName, ((Mesh)((TwinsSection)Data.Parent.Parent.GetItem(2)).GetItem(Data.MeshID)).ToPLY());
            }
        }
    }
}
