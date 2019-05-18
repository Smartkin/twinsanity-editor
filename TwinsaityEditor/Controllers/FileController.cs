using Twinsanity;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TwinsaityEditor
{
    public class FileController : SectionController
    {
        public new TwinsFile Data { get; set; }

        public string FileName { get => Data.FileName; }
        public string SafeFileName { get => Data.SafeFileName; }
        public Dictionary<uint, string> ObjectNames { get; set; } = new Dictionary<uint, string>();
        public Dictionary<uint, string> MaterialNames { get; set; } = new Dictionary<uint, string>();

        public TwinsItem SelectedItem { get; set; } = null;
        public int SelectedItemArg { get; set; } = -1;

        //Editors
        private Form editChunkLinks;
        private readonly Form[] editInstances = new Form[8], editPositions = new Form[8], editPaths = new Form[8], editTriggers = new Form[8];

        //Viewers
        private Form rmForm;
        private RMViewer RMViewer { get; set; }

        public FileController(MainForm topform, TwinsFile item) : base(topform, item)
        {
            Data = item;
            LoadFileInfo();
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

        private void LoadFileInfo()
        {
            if (Data.RecordIDs.ContainsKey(10) && ((TwinsSection)Data.GetItem(10)).RecordIDs.ContainsKey(0))
            {
                foreach (GameObject obj in ((TwinsSection)((TwinsSection)Data.GetItem(10)).GetItem(0)).Records)
                {
                    ObjectNames.Add(obj.ID, obj.Name);
                }
            }
            if (Data.RecordIDs.ContainsKey(11) && ((TwinsSection)Data.GetItem(11)).RecordIDs.ContainsKey(1))
            {
                foreach (Material mat in ((TwinsSection)((TwinsSection)Data.GetItem(11)).GetItem(1)).Records)
                {
                    MaterialNames.Add(mat.ID, mat.Name);
                }
            }
        }

        public void CloseFile()
        {
            ObjectNames.Clear();
            MaterialNames.Clear();
            Data = null;
            CloseRMViewer();
            CloseEditor(Editors.ChunkLinks);
            for (int i = 0; i <= 7; ++i)
            {
                CloseEditor(Editors.Instance, i);
                CloseEditor(Editors.Position, i);
                CloseEditor(Editors.Path, i);
                CloseEditor(Editors.Trigger, i);
            }
        }

        public void CheckOpenEditor(Controller c)
        {
            if (c is ChunkLinksController)
                OpenEditor(ref editChunkLinks, Editors.ChunkLinks, c);
            else if (c is PositionController)
                OpenEditor(ref editPositions[((PositionController)c).Data.Parent.Parent.ID], Editors.Position, (Controller)c.Node.Parent.Tag);
            else if (c is PathController)
                OpenEditor(ref editPaths[((PathController)c).Data.Parent.Parent.ID], Editors.Path, (Controller)c.Node.Parent.Tag);
            else if (c is InstanceController)
                OpenEditor(ref editInstances[((InstanceController)c).Data.Parent.Parent.ID], Editors.Instance, (Controller)c.Node.Parent.Tag);
            else if (c is TriggerController)
                OpenEditor(ref editTriggers[((TriggerController)c).Data.Parent.Parent.ID], Editors.Trigger, (Controller)c.Node.Parent.Tag);
            else if (c is SectionController s)
            {
                if (s.Data.Type == SectionType.ObjectInstance)
                    OpenEditor(ref editInstances[s.Data.Parent.ID], Editors.Instance, c);
                else if (s.Data.Type == SectionType.Position)
                    OpenEditor(ref editPositions[s.Data.Parent.ID], Editors.Position, c);
                else if (s.Data.Type == SectionType.Path)
                    OpenEditor(ref editPaths[s.Data.Parent.ID], Editors.Path, c);
                else if (s.Data.Type == SectionType.Trigger)
                    OpenEditor(ref editTriggers[s.Data.Parent.ID], Editors.Trigger, c);
            }
        }

        public void OpenEditor(ref Form editor_var, Editors editor, Controller cont)
        {
            if (editor_var == null || editor_var.IsDisposed)
            {
                switch (editor)
                {
                    case Editors.ChunkLinks: editor_var = new ChunkLinksEditor((ChunkLinksController)cont) { Tag = TopForm }; break;
                    case Editors.Position: editor_var = new PositionEditor(this, (SectionController)cont) { Tag = TopForm }; break;
                    case Editors.Path: editor_var = new PathEditor(this, (SectionController)cont) { Tag = TopForm }; break;
                    case Editors.Instance: editor_var = new InstanceEditor(this, (SectionController)cont) { Tag = TopForm }; break;
                    case Editors.Trigger: editor_var = new TriggerEditor(this, (SectionController)cont) { Tag = TopForm }; break;
                }
                editor_var.Show();
            }
            else
                editor_var.Select();
        }

        public void CloseEditor(Editors editor, int arg = -1)
        {
            Form editorForm = null;
            switch (editor)
            {
                case Editors.ChunkLinks: editorForm = editChunkLinks; break;
                case Editors.Instance: editorForm = editInstances[arg]; break; //since arg is -1 by default, an exception will be thrown unless it is specified
                case Editors.Position: editorForm = editPositions[arg]; break;
                case Editors.Path: editorForm = editPaths[arg]; break;
                case Editors.Trigger: editorForm = editTriggers[arg]; break;
            }
            if (editorForm != null && !editorForm.IsDisposed)
                editorForm.Close();
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
                RMViewer rmViewer = new RMViewer(this, ref rmForm) { Dock = DockStyle.Fill };
                rmForm.Controls.Add(rmViewer);
                rmForm.Text = "RMViewer";
                RMViewer = rmViewer;
            }
            else
                rmForm.Select();
        }

        public void RMViewer_LoadInstances()
        {
            if (RMViewer != null)
                RMViewer.LoadInstances();
        }

        public void RMViewer_LoadPositions()
        {
            if (RMViewer != null)
                RMViewer.LoadPositions();
        }

        public void CloseForm(ref Form form)
        {
            if (form != null && form.IsDisposed)
                form.Close();
        }

        public void CloseRMViewer()
        {
            CloseForm(ref rmForm);
        }

        public void SelectItem(TwinsItem item, int arg = -1)
        {
            var prev_item = SelectedItem;
            SelectedItem = item;
            SelectedItemArg = arg;
            if (RMViewer != null)
            {
                if (item == null && prev_item != null)
                {
                    if (prev_item is Instance)
                        RMViewer.LoadInstances();
                    else if (prev_item is Position)
                        RMViewer.LoadPositions();
                }
                else
                    RMViewer.UpdateSelected();
            }
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
                //throw new System.ArgumentException("The requested section does not have an instance in the specified position.");
                return null;
            }
            else throw new System.ArgumentException("The requested section does not have an object instance section.");
        }

        public override void Dispose()
        {
            CloseFile();
            base.Dispose();
        }
    }
}
