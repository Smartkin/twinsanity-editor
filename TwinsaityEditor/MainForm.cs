using System;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public partial class MainForm : Form
    {
        private static TwinsFile fileData = new TwinsFile();
        private static Form rmForm, exeForm;
        private static string fileName;

        public static string SafeFileName { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void GenTree()
        {
            treeView1.BeginUpdate();
            treeView1.AfterSelect += TreeNodeSelect;
            if (treeView1.TopNode != null && treeView1.TopNode.Tag is Controller c)
                c.DisposeNode(treeView1.TopNode);
            treeView1.Nodes.Clear();
            FileController controller = new FileController(fileData);
            controller.Node.Text = controller.GetName();
            controller.GenText();
            treeView1.Nodes.Add(controller.Node);
            treeView1.Select();
            foreach (var i in fileData.SecInfo.Records.Values)
            {
                GenTreeNode(i, controller);
            }
            treeView1.TopNode.Expand();
            treeView1.EndUpdate();
        }

        private void TreeNodeSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is Controller c)
            {
                if (c.Dirty)
                {
                    e.Node.Text = c.GetName();
                    c.GenText();
                    c.Dirty = false;
                }
                textBox1.Lines = c.TextPrev;
            }
        }

        public void GenTreeNode(TwinsItem a, Controller controller)
        {
            Controller c;
            if (a is TwinsSection)
            {
                c = new SectionController((TwinsSection)a);
                foreach (var i in ((TwinsSection)a).SecInfo.Records.Values)
                {
                    GenTreeNode(i, c);
                }
            }
            else if (a is Texture)
                c = new TextureController((Texture)a);
            else if (a is Material)
                c = new MaterialController((Material)a);
            else if (a is Mesh)
                c = new MeshController((Mesh)a);
            else if (a is Model)
                c = new ModelController((Model)a);
            else if (a is GameObject)
                c = new ObjectController((GameObject)a);
            else if (a is Script)
                c = new ScriptController((Script)a);
            else if (a is Instance)
                c = new InstanceController((Instance)a);
            else if (a is Trigger && fileData.Type != TwinsFile.FileType.DemoRM2) //trigger controller assumes final instance format
                c = new TriggerController((Trigger)a);
            else if (a is ColData)
                c = new ColDataController((ColData)a);
            else if (a is ChunkLinks)
                c = new ChunkLinksController((ChunkLinks)a);
            else
                c = new ItemController(a);
            c.Node.Text = c.GetName();
            c.GenText();
            controller.AddNode(c);
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
                switch (ofd.FilterIndex)
                {
                    case 1:
                        fileData.LoadFile(ofd.FileName, TwinsFile.FileType.RM2);
                        rMViewerToolStripMenuItem.Enabled = true;
                        sMViewerToolStripMenuItem.Enabled = false;
                        break;
                    case 2:
                        fileData.LoadFile(ofd.FileName, TwinsFile.FileType.SM2);
                        sMViewerToolStripMenuItem.Enabled = true;
                        rMViewerToolStripMenuItem.Enabled = false;
                        break;
                    case 3:
                        fileData.LoadFile(ofd.FileName, TwinsFile.FileType.RMX);
                        rMViewerToolStripMenuItem.Enabled = true;
                        sMViewerToolStripMenuItem.Enabled = false;
                        break;
                    case 4:
                        fileData.LoadFile(ofd.FileName, TwinsFile.FileType.SMX);
                        sMViewerToolStripMenuItem.Enabled = true;
                        rMViewerToolStripMenuItem.Enabled = false;
                        break;
                    case 5:
                        fileData.LoadFile(ofd.FileName, TwinsFile.FileType.DemoRM2);
                        rMViewerToolStripMenuItem.Enabled = true;
                        sMViewerToolStripMenuItem.Enabled = false;
                        break;
                    case 6:
                        fileData.LoadFile(ofd.FileName, TwinsFile.FileType.DemoSM2);
                        sMViewerToolStripMenuItem.Enabled = true;
                        rMViewerToolStripMenuItem.Enabled = false;
                        break;
                }
                fileName = ofd.FileName;
                SafeFileName = ofd.SafeFileName;
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
                fileData.SaveFile(sfd.FileName);
                fileName = sfd.FileName;
                Text = "Twinsaity Editor by Neo_Kesha [" + sfd.FileName + "] ";
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileData.SaveFile(fileName);
        }

        private void rMViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenRMViewer();
        }

        public static void OpenRMViewer()
        {
            if (rmForm == null)
            {
                rmForm = new Form { Size = new System.Drawing.Size(480, 480), Text = "Initiating renderer..." };
                rmForm.FormClosed += delegate
                {
                    rmForm = null;
                };
                rmForm.Show();
                RMViewer viewer = new RMViewer(fileData.SecInfo.Records.ContainsKey(9) ? (ColData)fileData.SecInfo.Records[9] : null, ref FileController.GetFile()) { Dock = DockStyle.Fill };
                rmForm.Controls.Add(viewer);
                rmForm.Text = "RMViewer";
            }
            else
                rmForm.Select();
        }

        private void eLFPatcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenEXETool();
        }

        public static void OpenEXETool()
        {
            if (exeForm == null)
            {
                exeForm = new Workers.EXEPatcher();
                exeForm.FormClosed += delegate
                {
                    exeForm = null;
                };
                exeForm.Show();
            }
            else
                exeForm.Select();
        }
    }
}
