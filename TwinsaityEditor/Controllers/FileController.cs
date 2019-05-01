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
        public Dictionary<uint, string> ObjectNames { get; set; }

        //Viewers
        private Form rmForm;
        private RMViewer rmViewer;
        public RMViewer RMViewer { get => rmViewer; }

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
            ObjectNames = new Dictionary<uint, string>();
            if (Data.RecordIDs.ContainsKey(10) && ((TwinsSection)Data.GetItem(10)).RecordIDs.ContainsKey(0))
            {
                foreach (GameObject obj in ((TwinsSection)((TwinsSection)Data.GetItem(10)).GetItem(0)).Records)
                {
                    ObjectNames.Add(obj.ID, obj.Name);
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
                    rmViewer = null;
                };
                rmForm.Show();
                rmViewer = new RMViewer(this) { Dock = DockStyle.Fill, Tag = TopForm };
                rmForm.Controls.Add(rmViewer);
                rmForm.Text = "RMViewer";
            }
            else
                rmForm.Select();
        }

        public void RMViewer_LoadInstances()
        {
            if (rmViewer != null)
                rmViewer.LoadInstances();
        }

        public void RMSelectItem(TwinsItem item)
        {
            if (rmViewer != null)
                rmViewer.SelectItem(item);
        }

        public void CloseRMViewer()
        {
            if (rmForm != null && !rmForm.IsDisposed)
                rmForm.Close();
        }

        public string GetMaterialName(uint id)
        {
            if (Data.RecordIDs.ContainsKey(11) && ((TwinsSection)Data.GetItem(11)).RecordIDs.ContainsKey(1) && ((TwinsSection)((TwinsSection)Data.GetItem(11)).GetItem(1)).RecordIDs.ContainsKey(id))
                return ((Material)((TwinsSection)((TwinsSection)Data.GetItem(11)).GetItem(1)).GetItem(id)).Name; //lol
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
