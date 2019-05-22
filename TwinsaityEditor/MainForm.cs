using System;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public partial class MainForm : Form
    {
        private Form smForm, exeForm;

        private TreeNode previousNode;

        //private List<FileController> FilesOpened { get; }
        public FileController FilesController { get => (FileController)Tag; }
        public FileController CurCont { get => FilesController; } //get currently selected file controller
        public TwinsFile CurFile { get => CurCont.Data; } //get currently selected file
        //public FileController DefaultCont { get; private set; }
        //public TwinsFile DefaultFile { get => DefaultCont.Data; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void GenTree()
        {
            treeView1.BeginUpdate();
            treeView1.AfterSelect += treeView1_AfterSelect;
            treeView1.KeyDown += treeView1_KeyDown;
            if (treeView1.TopNode != null && treeView1.TopNode.Tag is Controller c)
                Controller.DisposeNode(treeView1.TopNode);
            if (ColDataController.importer != null)
                ColDataController.importer.Close();
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
            else if (a is SoundEffect)
                c = new SEController(this, (SoundEffect)a);
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

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            TreeView tree = (TreeView)sender;
            if (e.KeyCode == Keys.Enter && tree.SelectedNode.Tag is Controller c)
                CurCont.OpenEditor(c);
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
                if (CurCont != null)
                    CurCont.Dispose();
                var file = new TwinsFile();
                switch (ofd.FilterIndex)
                {
                    case 1:
                        file.LoadFile(ofd.FileName, TwinsFile.FileType.RM2);
                        rMViewerToolStripMenuItem.Enabled = true;
                        sMViewerToolStripMenuItem.Enabled = false;
                        break;
                    case 2:
                        file.LoadFile(ofd.FileName, TwinsFile.FileType.SM2);
                        sMViewerToolStripMenuItem.Enabled = true;
                        rMViewerToolStripMenuItem.Enabled = false;
                        break;
                    case 3:
                        file.LoadFile(ofd.FileName, TwinsFile.FileType.RMX);
                        rMViewerToolStripMenuItem.Enabled = true;
                        sMViewerToolStripMenuItem.Enabled = false;
                        break;
                    case 4:
                        file.LoadFile(ofd.FileName, TwinsFile.FileType.SMX);
                        sMViewerToolStripMenuItem.Enabled = true;
                        rMViewerToolStripMenuItem.Enabled = false;
                        break;
                    case 5:
                        file.LoadFile(ofd.FileName, TwinsFile.FileType.DemoRM2);
                        rMViewerToolStripMenuItem.Enabled = true;
                        sMViewerToolStripMenuItem.Enabled = false;
                        break;
                    case 6:
                        file.LoadFile(ofd.FileName, TwinsFile.FileType.DemoSM2);
                        sMViewerToolStripMenuItem.Enabled = true;
                        rMViewerToolStripMenuItem.Enabled = false;
                        break;
                }
                file.FileName = ofd.FileName;
                file.SafeFileName = ofd.SafeFileName;
                Tag = new FileController(this, file);
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
                CurCont.Data.FileName = sfd.FileName;
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
            CurCont.OpenRMViewer();
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
    }
}
