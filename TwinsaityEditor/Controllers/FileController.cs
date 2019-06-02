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

        public SectionController MeshSection { get; private set; }
        public Dictionary<uint, string> ObjectNames { get; set; }
        public Dictionary<uint, string> MaterialNames { get; set; }

        public TwinsItem SelectedItem { get; set; } = null;
        public int SelectedItemArg { get; set; } = -1;

        //Editors
        private Form editChunkLinks;
        private readonly Form[] editInstances = new Form[8], editPositions = new Form[8], editPaths = new Form[8], editTriggers = new Form[8];

        //Viewers
        private Form rmForm, colForm;
        private RMViewer RMViewer { get; set; }
        private Dictionary<uint, Form> MeshViewers { get; set; }

        public FileController(MainForm topform, TwinsFile item) : base(topform, item)
        {
            Data = item;
            ObjectNames = new Dictionary<uint, string>();
            MaterialNames = new Dictionary<uint, string>();
            MeshViewers = new Dictionary<uint, Form>();
            LoadFileInfo();
        }

        protected override string GetName()
        {
            return "File";
        }

        protected override void GenText()
        {
            TextPrev = new string[2];
            TextPrev[0] = $"Size: {Data.Size}";
            TextPrev[1] = $"ContentSize: {Data.ContentSize} Element Count: {Data.Records.Count}";
        }

        private void LoadFileInfo()
        {
            if (Data.ContainsItem(10) && ((TwinsSection)Data.GetItem(10)).ContainsItem(0))
            {
                foreach (GameObject obj in ((TwinsSection)((TwinsSection)Data.GetItem(10)).GetItem(0)).Records)
                {
                    ObjectNames.Add(obj.ID, obj.Name);
                }
            }
            if (Data.ContainsItem(11) && ((TwinsSection)Data.GetItem(11)).ContainsItem(1))
            {
                foreach (Material mat in ((TwinsSection)((TwinsSection)Data.GetItem(11)).GetItem(1)).Records)
                {
                    MaterialNames.Add(mat.ID, mat.Name);
                }
            }
            if (Data.ContainsItem(11) && ((TwinsSection)Data.GetItem(11)).ContainsItem(2))
            {
                
            }
        }

        public void CloseFile()
        {
            CloseRMViewer();
            CloseAllMeshViewers();
            CloseEditor(Editors.ChunkLinks);
            CloseEditor(Editors.ColData);
            for (int i = 0; i <= 7; ++i)
            {
                CloseEditor(Editors.Instance, i);
                CloseEditor(Editors.Position, i);
                CloseEditor(Editors.Path, i);
                CloseEditor(Editors.Trigger, i);
            }
        }

        public void OpenEditor(Controller c)
        {
            if (c is ChunkLinksController)
                OpenEditor(ref editChunkLinks, Editors.ChunkLinks, c);
            else if (c is ColDataController)
                OpenEditor(ref colForm, Editors.ColData, c);
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

        private void OpenEditor(ref Form editor_var, Editors editor, Controller cont)
        {
            if (editor_var == null || editor_var.IsDisposed)
            {
                switch (editor)
                {
                    case Editors.ColData:
                        {
                            if (Data.ContainsItem(9)) editor_var = new ColDataEditor((ColData)Data.GetItem(9)) { Tag = TopForm };
                            else return;
                        }
                        break;
                    case Editors.ChunkLinks: editor_var = new ChunkLinksEditor((ChunkLinksController)cont) { Tag = TopForm }; break;
                    case Editors.Position: editor_var = new PositionEditor((SectionController)cont) { Tag = TopForm }; break;
                    case Editors.Path: editor_var = new PathEditor((SectionController)cont) { Tag = TopForm }; break;
                    case Editors.Instance: editor_var = new InstanceEditor((SectionController)cont) { Tag = TopForm }; break;
                    case Editors.Trigger: editor_var = new TriggerEditor((SectionController)cont) { Tag = TopForm }; break;
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
                case Editors.ColData: editorForm = colForm; break;
                case Editors.ChunkLinks: editorForm = editChunkLinks; break;
                case Editors.Instance: editorForm = editInstances[arg]; break; //since arg is -1 by default, an exception will be thrown unless it is specified
                case Editors.Position: editorForm = editPositions[arg]; break;
                case Editors.Path: editorForm = editPaths[arg]; break;
                case Editors.Trigger: editorForm = editTriggers[arg]; break;
            }
            CloseForm(ref editorForm);
        }

        public void OpenMeshViewer(MeshController c)
        {
            var id = c.Data.ID;
            if (!MeshViewers.ContainsKey(id))
            {
                var f = new Form { Size = new System.Drawing.Size(480, 480), Text = "Initializing viewer..." };
                f.FormClosed += delegate
                {
                    MeshViewers.Remove(id);
                };
                f.Show();
                MeshViewer v = new MeshViewer(c, ref f) { Dock = DockStyle.Fill };
                f.Controls.Add(v);
                f.Text = "MeshViewer";
                MeshViewers.Add(id, f);
            }
            else
                MeshViewers[id].Select();
        }

        public void CloseMeshViewer(uint mesh_id)
        {
            var f = MeshViewers[mesh_id];
            CloseForm(ref f);
            MeshViewers.Remove(mesh_id);
        }

        public void CloseAllMeshViewers()
        {
            var a = new uint[MeshViewers.Count];
            MeshViewers.Keys.CopyTo(a, 0);
            foreach (var p in a)
            {
                CloseMeshViewer(p);
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
            if (form != null && !form.IsDisposed)
            {
                form.Close();
                form = null;
            }
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
            if (Data.ContainsItem(sector) && ((TwinsSection)Data.GetItem(sector)).ContainsItem(6))
            {
                //int i = 0;
                //foreach (Instance j in ((TwinsSection)((TwinsSection)Data.GetItem(sector)).GetItem(6)).Records)
                //{
                //    if (i++ == id)
                //        return j;
                //}
                //throw new System.ArgumentException("The requested section does not have an instance in the specified position.");
                if (id < ((TwinsSection)((TwinsSection)Data.GetItem(sector)).GetItem(6)).Records.Count)
                    return (Instance)((TwinsSection)((TwinsSection)Data.GetItem(sector)).GetItem(6)).Records[(int)id];
                else
                    return null;
            }
            else throw new System.ArgumentException("The requested section does not have an object instance section.");
        }

        public AIPosition GetAIPos(uint sector, uint id)
        {
            if (Data.ContainsItem(sector) && ((TwinsSection)Data.GetItem(sector)).ContainsItem(1))
            {
                //int i = 0;
                //foreach (Instance j in ((TwinsSection)((TwinsSection)Data.GetItem(sector)).GetItem(6)).Records)
                //{
                //    if (i++ == id)
                //        return j;
                //}
                //throw new System.ArgumentException("The requested section does not have an instance in the specified position.");
                if (id < ((TwinsSection)((TwinsSection)Data.GetItem(sector)).GetItem(1)).Records.Count)
                    return (AIPosition)((TwinsSection)((TwinsSection)Data.GetItem(sector)).GetItem(1)).Records[(int)id];
                else
                    return null;
            }
            else throw new System.ArgumentException("The requested section does not have an object instance section.");
        }
    }
}
