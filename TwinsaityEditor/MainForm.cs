using System;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public partial class MainForm : Form
    {
        public enum Editors
        {
            ChunkLinks,
            Position,
            Instance
        };

        //private static TwinsFile fileData = new TwinsFile();
        private Form rmForm, smForm, exeForm;
        private Form editChunkLinks;
        private Form[] editInstances = new Form[8], editPositions = new Form[8];
        //private static string fileName;

        private TreeNode previousNode;

        public FileController FilesController { get => (FileController)Tag; }
        public FileController CurCont { get => FilesController; } //get currently selected file controller
        public TwinsFile CurFile { get => CurCont.Data; } //get currently selected file

        private RMViewer rmViewer;
        public RMViewer RMViewer { get => rmViewer; }
        //public static string SafeFileName { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void GenTree()
        {
            treeView1.BeginUpdate();
            treeView1.AfterSelect += treeView1_AfterSelect;
            treeView1.NodeMouseDoubleClick += TreeNodeOpenEditor;
            treeView1.KeyDown += treeView1_KeyDown;
            if (treeView1.TopNode != null && treeView1.TopNode.Tag is Controller c)
                Controller.DisposeNode(treeView1.TopNode);
            if (ColDataController.importer != null)
                ColDataController.importer.Close();
            CloseEditor(Editors.ChunkLinks);
            for (int i = 0; i <= 7; ++i)
            {
                CloseInstanceEditor(i);
                ClosePositionEditor(i);
            }
            CloseRMViewer();
            treeView1.Nodes.Clear();
            CurCont.UpdateText();
            treeView1.Nodes.Add(CurCont.Node);
            treeView1.Select();
            foreach (var i in CurFile.Records)
            {
                GenTreeNode(i, CurCont);
            }
            treeView1.TopNode.Expand();
            treeView1.EndUpdate();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (previousNode != null && previousNode.Tag is Controller c1)
                c1.Selected = false;
            if (e.Node.Tag is Controller c2)
                ControllerNodeSelect(c2);
            previousNode = e.Node;
        }

        public void ControllerNodeSelect(Controller c)
        {
            c.Selected = true;
            textBox1.Lines = c.TextPrev;
        }

        public void GenTreeNode(TwinsItem a, Controller controller)
        {
            Controller c;
            if (a is TwinsSection)
            {
                c = new SectionController(this, (TwinsSection)a);
                foreach (var i in ((TwinsSection)a).Records)
                {
                    GenTreeNode(i, c);
                }
            }
            else if (a is Texture)
                c = new TextureController(this, (Texture)a);
            else if (a is Material)
                c = new MaterialController(this, (Material)a);
            else if (a is Mesh)
                c = new MeshController(this, (Mesh)a);
            else if (a is Model)
                c = new ModelController(this, (Model)a);
            else if (a is GameObject)
                c = new ObjectController(this, (GameObject)a);
            else if (a is Script)
                c = new ScriptController(this, (Script)a);
            else if (a is Position)
                c = new PositionController(this, (Position)a);
            else if (a is Path)
                c = new PathController(this, (Path)a);
            else if (a is Instance)
                c = new InstanceController(this, (Instance)a);
            else if (a is Trigger && CurFile.Type != TwinsFile.FileType.DemoRM2) //trigger controller assumes final instance format
                c = new TriggerController(this, (Trigger)a);
            else if (a is ColData)
                c = new ColDataController(this, (ColData)a);
            else if (a is ChunkLinks)
                c = new ChunkLinksController(this, (ChunkLinks)a);
            else
                c = new ItemController(this, a);
            c.UpdateText();
            controller.AddNode(c);
        }

        private void TreeNodeOpenEditor(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is Controller c)
                CheckOpenEditor(c);
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            TreeView tree = (TreeView)sender;
            if (e.KeyCode == Keys.Enter && tree.SelectedNode.Tag is Controller c)
                CheckOpenEditor(c);
        }

        private void CheckOpenEditor(Controller c)
        {
            if (c is ChunkLinksController)
                OpenEditor(ref editChunkLinks, Editors.ChunkLinks, c);
            else if (c is PositionController)
                OpenEditor(ref editPositions[((PositionController)c).Data.Parent.Parent.ID], Editors.Position, (Controller)c.Node.Parent.Tag);
            else if (c is InstanceController)
                OpenEditor(ref editInstances[((InstanceController)c).Data.Parent.Parent.ID], Editors.Instance, (Controller)c.Node.Parent.Tag);
            else if (c is SectionController s && !(c.Node.IsExpanded && c.Node.Nodes.Count > 0))
            {
                if (s.Data.Type == SectionType.ObjectInstance)
                    OpenEditor(ref editInstances[s.Data.Parent.ID], Editors.Instance, c);
                else if (s.Data.Type == SectionType.Position)
                    OpenEditor(ref editPositions[s.Data.Parent.ID], Editors.Position, c);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openRM2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "RM2 files|*.rm2|SM2 files|*.sm2|RMX files|*.rmx|SMX files|*.smx|Demo RM2 files|*.rm2|Demo SM2 files|*.sm2";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Tag = new FileController(this, new TwinsFile());
                switch (ofd.FilterIndex)
                {
                    case 1:
                        CurFile.LoadFile(ofd.FileName, TwinsFile.FileType.RM2);
                        rMViewerToolStripMenuItem.Enabled = true;
                        sMViewerToolStripMenuItem.Enabled = false;
                        break;
                    case 2:
                        CurFile.LoadFile(ofd.FileName, TwinsFile.FileType.SM2);
                        sMViewerToolStripMenuItem.Enabled = true;
                        rMViewerToolStripMenuItem.Enabled = false;
                        break;
                    case 3:
                        CurFile.LoadFile(ofd.FileName, TwinsFile.FileType.RMX);
                        rMViewerToolStripMenuItem.Enabled = true;
                        sMViewerToolStripMenuItem.Enabled = false;
                        break;
                    case 4:
                        CurFile.LoadFile(ofd.FileName, TwinsFile.FileType.SMX);
                        sMViewerToolStripMenuItem.Enabled = true;
                        rMViewerToolStripMenuItem.Enabled = false;
                        break;
                    case 5:
                        CurFile.LoadFile(ofd.FileName, TwinsFile.FileType.DemoRM2);
                        rMViewerToolStripMenuItem.Enabled = true;
                        sMViewerToolStripMenuItem.Enabled = false;
                        break;
                    case 6:
                        CurFile.LoadFile(ofd.FileName, TwinsFile.FileType.DemoSM2);
                        sMViewerToolStripMenuItem.Enabled = true;
                        rMViewerToolStripMenuItem.Enabled = false;
                        break;
                }
                CurCont.FileName = ofd.FileName;
                CurCont.SafeFileName = ofd.SafeFileName;
                GenTree();
                Text = "Twinsaity Editor by Neo_Kesha [" + ofd.FileName + "] ";
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "RM2/RMX files|*.rm*|SM2/SMX files|*.sm*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                CurFile.SaveFile(sfd.FileName);
                CurCont.FileName = sfd.FileName;
                Text = "Twinsaity Editor by Neo_Kesha [" + sfd.FileName + "] ";
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Overwrite original file?", "Save", MessageBoxButtons.OKCancel) == DialogResult.OK)
                CurFile.SaveFile(CurCont.FileName);
        }

        private void rMViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenRMViewer();
        }

        public void OpenRMViewer()
        {
            if (rmForm == null)
            {
                rmForm = new Form { Size = new System.Drawing.Size(480, 480), Text = "Initiating viewer..." };
                rmForm.FormClosed += delegate
                {
                    rmForm = null;
                    rmViewer = null;
                };
                rmForm.Show();
                rmViewer = new RMViewer(CurCont) { Dock = DockStyle.Fill, Tag = this };
                rmForm.Controls.Add(rmViewer);
                rmForm.Text = "RMViewer";
            }
            else
                rmForm.Select();
        }

        public void OpenSMViewer()
        {
            if (smForm == null)
            {
                smForm = new Form { Size = new System.Drawing.Size(480, 480), Text = "Initiating renderer..." };
                smForm.FormClosed += delegate
                {
                    smForm = null;
                };
                smForm.Show();
                TwinsFile file = CurFile;
                SMViewer viewer = new SMViewer(ref file) { Dock = DockStyle.Fill, Tag = this };
                smForm.Controls.Add(viewer);
                smForm.Text = "SMViewer";
            }
            else
                smForm.Select();
        }

        private void eLFPatcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenEXETool();
        }

        private void sMViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSMViewer();
        }

        public void OpenEXETool()
        {
            if (exeForm == null)
            {
                exeForm = new Workers.EXEPatcher();
                exeForm.FormClosed += delegate
                {
                    exeForm = null;
                };
            }
            else
                exeForm.Select();
        }

        public void OpenEditor(ref Form editor_var, Editors editor, Controller cont)
        {
            if (editor_var == null || editor_var.IsDisposed)
            {
                switch (editor)
                {
                    case Editors.ChunkLinks: editor_var = new ChunkLinksEditor((ChunkLinksController)cont) { Tag = this }; break;
                    case Editors.Position: editor_var = new PositionEditor((SectionController)cont) { Tag = this }; break;
                    case Editors.Instance: editor_var = new InstanceEditor(CurCont, (SectionController)cont) { Tag = this }; break;
                }
                editor_var.Show();
            }
            else
                editor_var.Select();
        }

        public void CloseEditor(Editors editor)
        {
            Form editorForm = null;
            switch(editor)
            {
                case Editors.ChunkLinks: editorForm = editChunkLinks; break;
            }
            if (editorForm != null && !editorForm.IsDisposed)
                editorForm.Close();
        }

        public void CloseInstanceEditor(int id)
        {
            if (editInstances[id] != null && !editInstances[id].IsDisposed)
                editInstances[id].Close();
        }

        public void ClosePositionEditor(int id)
        {
            if (editPositions[id] != null && !editPositions[id].IsDisposed)
                editPositions[id].Close();
        }

        public void CloseRMViewer()
        {
            if (rmForm != null && !rmForm.IsDisposed)
                rmForm.Close();
        }

        public void RMSelectItem(TwinsItem item)
        {
            if (rmViewer != null)
                rmViewer.SelectItem(item);
        }
    }
}
