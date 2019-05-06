using Twinsanity;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TwinsaityEditor
{
    public class FileController : Controller
    {
        public TwinsFile Data { get; set; }

        public string FileName { get; set; }
        public string SafeFileName { get; set; }
        public Dictionary<uint, string> ObjectNames { get; set; } = new Dictionary<uint, string>();
        public Dictionary<uint, string> MaterialNames { get; set; } = new Dictionary<uint, string>();

        public TwinsItem SelectedItem { get; set; } = null;

        //Viewers
        private Form rmForm;
        public RMViewer RMViewer { get; private set; }

        public FileController(MainForm topform, TwinsFile item) : base(topform)
        {
            Data = item;
        }

        protected override string GetName()
        {
            return "File";
        }

        protected override void GenText()
        {
            TextPrev = new string[2];
            TextPrev[0] = "Size: " + Data.Size;
            TextPrev[1] = "ContentSize: " + Data.ContentSize + " Element Count: " + Data.Records.Count;
        }

        public void LoadFile(string path, TwinsFile.FileType type)
        {
            Data.LoadFile(path, type);
            ObjectNames.Clear();
            if (Data.RecordIDs.ContainsKey(10) && ((TwinsSection)Data.GetItem(10)).RecordIDs.ContainsKey(0))
            {
                foreach (GameObject obj in ((TwinsSection)((TwinsSection)Data.GetItem(10)).GetItem(0)).Records)
                {
                    ObjectNames.Add(obj.ID, obj.Name);
                }
            }
            MaterialNames.Clear();
            if (Data.RecordIDs.ContainsKey(11) && ((TwinsSection)Data.GetItem(11)).RecordIDs.ContainsKey(1))
            {
                foreach (Material mat in ((TwinsSection)((TwinsSection)Data.GetItem(11)).GetItem(1)).Records)
                {
                    MaterialNames.Add(mat.ID, mat.Name);
                }
            }
        }

        public void OpenRMViewer()
        {
            if (rmForm == null)
            {
                rmForm = new Form { Size = new System.Drawing.Size(480, 480), Text = "Initializing viewer..." };
                rmForm.FormClosed += delegate
                {
                    rmForm = null;
                    RMViewer = null;
                };
                rmForm.Show();
                RMViewer = new RMViewer(this, ref rmForm) { Dock = DockStyle.Fill };
                rmForm.Controls.Add(RMViewer);
                rmForm.Text = "RMViewer";
            }
            else
                rmForm.Select();
        }

        public void RMViewer_LoadInstances()
        {
            if (RMViewer != null)
                RMViewer.LoadInstances();
        }

        public void CloseRMViewer()
        {
            if (rmForm != null && !rmForm.IsDisposed)
                rmForm.Close();
        }

        public void SelectItem(TwinsItem item)
        {
            SelectedItem = item;
            if (RMViewer != null)
                RMViewer.UpdateSelected();
        }

        public string GetMaterialName(uint id)
        {
            if (MaterialNames.ContainsKey(id))
                return MaterialNames[id];
            else return string.Empty;
        }

        public string GetObjectName(uint id)
        {
            if (ObjectNames.ContainsKey(id))
                return ObjectNames[id];
            else return string.Empty;
        }

        public string GetScriptName(uint id)
        {
            try { return ((Script)((TwinsSection)((TwinsSection)Data.GetItem(10)).GetItem(1)).GetItem(id)).Name; } //lol
            catch { return string.Empty; }
        }

        public Instance GetInstance(uint sector, uint id)
        {
            if (Data.RecordIDs.ContainsKey(sector) && ((TwinsSection)Data.GetItem(sector)).RecordIDs.ContainsKey(6))// && ((TwinsSection)((TwinsSection)Data.GetItem(sector)).GetItem(6)).SecInfo.Records.ContainsKey(id))
            {
                int i = 0;
                foreach (Instance j in ((TwinsSection)((TwinsSection)Data.GetItem(sector)).GetItem(6)).Records)
                {
                    if (i++ == id)
                        return j;
                }
                throw new System.ArgumentException("The requested section does not have an instance in the specified position.");
            }
            else throw new System.ArgumentException("The requested section does not have an object instance section.");
        }
    }
}
