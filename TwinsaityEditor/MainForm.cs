using System;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    /// <summary>
    /// Flags corresponding to buttons in the toolbar to the right.
    /// </summary>
    [Flags]
    public enum ToolbarFlags
    {
        Hex     = 0x0001,
        Extract = 0x0002,
        Replace = 0x0004,
        Search  = 0x0008,
        Add     = 0x0010,
        Create  = 0x0020,
        Delete  = 0x0040,
        Export  = 0x0080,
        View    = 0x0100,
        Script  = 0x0200,
        Edit    = 0x0400
    }

    public partial class MainForm : Form
    {
        private TwinsFile fileData = new TwinsFile();
        private string fileName;
        private Form rmForm;

        public MainForm()
        {
            InitializeComponent();
        }

        private void GenTree()
        {
            groupBox1.Enabled = true;
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
                buttonHex.Enabled = (c.Toolbar & ToolbarFlags.Hex) != 0;
                buttonExt.Enabled = (c.Toolbar & ToolbarFlags.Extract) != 0;
                buttonRep.Enabled = (c.Toolbar & ToolbarFlags.Replace) != 0;
                buttonAdd.Enabled = (c.Toolbar & ToolbarFlags.Add) != 0;
                buttonCre.Enabled = (c.Toolbar & ToolbarFlags.Create) != 0;
                buttonDel.Enabled = (c.Toolbar & ToolbarFlags.Delete) != 0;
                buttonEdt.Enabled = (c.Toolbar & ToolbarFlags.Edit) != 0;
                buttonExp.Enabled = (c.Toolbar & ToolbarFlags.Export) != 0;
                buttonScr.Enabled = (c.Toolbar & ToolbarFlags.Script) != 0;
                buttonSrc.Enabled = (c.Toolbar & ToolbarFlags.Search) != 0;
                buttonViw.Enabled = (c.Toolbar & ToolbarFlags.View) != 0;
            }
            else if (e.Node == treeView1.TopNode)
            {
                buttonHex.Enabled = buttonExt.Enabled = buttonRep.Enabled = buttonAdd.Enabled = buttonCre.Enabled =
                buttonDel.Enabled = buttonEdt.Enabled = buttonExp.Enabled = buttonScr.Enabled = buttonSrc.Enabled =
                buttonViw.Enabled = false;
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
                GenTree();
                Text = "Twinsaity Editor by Neo_Kesha [" + ofd.FileName + "] ";
            }
        }

        private void buttonHex_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag is Controller c && (c.Toolbar & ToolbarFlags.Hex) != 0)
                c.ToolbarAction(ToolbarFlags.Hex);
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

        private void buttonExt_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag is Controller c && (c.Toolbar & ToolbarFlags.Extract) != 0)
                c.ToolbarAction(ToolbarFlags.Extract);
        }

        private void buttonRep_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag is Controller c && (c.Toolbar & ToolbarFlags.Replace) != 0)
                c.ToolbarAction(ToolbarFlags.Replace);
        }

        private void buttonSrc_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag is Controller c && (c.Toolbar & ToolbarFlags.Search) != 0)
                c.ToolbarAction(ToolbarFlags.Search);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag is Controller c && (c.Toolbar & ToolbarFlags.Add) != 0)
                c.ToolbarAction(ToolbarFlags.Add);
        }

        private void buttonCre_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag is Controller c && (c.Toolbar & ToolbarFlags.Create) != 0)
                c.ToolbarAction(ToolbarFlags.Create);
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag is Controller c && (c.Toolbar & ToolbarFlags.Delete) != 0)
                c.ToolbarAction(ToolbarFlags.Delete);
        }

        private void buttonExp_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag is Controller c && (c.Toolbar & ToolbarFlags.Export) != 0)
                c.ToolbarAction(ToolbarFlags.Export);
        }

        private void buttonViw_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag is Controller c && (c.Toolbar & ToolbarFlags.View) != 0)
                c.ToolbarAction(ToolbarFlags.View);
        }

        private void buttonScr_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag is Controller c && (c.Toolbar & ToolbarFlags.Script) != 0)
                c.ToolbarAction(ToolbarFlags.Script);
        }

        private void buttonEdt_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag is Controller c && (c.Toolbar & ToolbarFlags.Edit) != 0)
                c.ToolbarAction(ToolbarFlags.Edit);
        }

        private void rMViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rmForm == null)
            {
                rmForm = new Form { Size = new System.Drawing.Size(480, 480) };
                rmForm.FormClosed += delegate {
                    rmForm = null;
                };
                RMViewer viewer = new RMViewer(fileData.SecInfo.Records.ContainsKey(9) ? (ColData)fileData.SecInfo.Records[9] : null, ref FileController.GetFile()) { Dock = DockStyle.Fill };
                rmForm.Controls.Add(viewer);
                rmForm.Show();
            }
        }
    }
}
