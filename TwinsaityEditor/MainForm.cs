using System;
using System.Windows.Forms;
using TwinsaityEditor.Controllers;
using TwinsaityEditor.Properties;
using TwinsaityEditor.Workers;
using Twinsanity;

namespace TwinsaityEditor
{
    public partial class MainForm : Form
    {
        private Form mhForm, exeForm, bdForm, imageMakerForm;

        private TreeNode nodeLastSelected;

        //private List<FileController> FilesOpened { get; }
        public FileController FilesController { get => (FileController)Tag; }
        public FileController CurCont { get => FilesController; } //get currently selected file controller
        public TwinsFile CurFile { get => CurCont.Data; } //get currently selected file
        //public FileController DefaultCont { get; private set; }
        //public TwinsFile DefaultFile { get => DefaultCont.Data; }

        public MainForm()
        {
            InitializeComponent();
            treeView1.AfterSelect += treeView1_AfterSelect;
            treeView1.KeyDown += treeView1_KeyDown;
        }

        private void GenTree()
        {
            nodeLastSelected = null;
            treeView1.BeginUpdate();
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
            if (nodeLastSelected != null && nodeLastSelected.Tag is Controller c1)
                c1.Selected = false;
            if (e.Node.Tag is Controller c2)
                ControllerNodeSelect(c2);
            nodeLastSelected = e.Node;
        }

        public void ControllerNodeSelect(Controller c)
        {
            c.Selected = true;
            textBox1.Lines = c.TextPrev;
        }

        public void GenTreeNode(TwinsItem a, Controller controller, bool cached = false)
        {
            Controller c;
            if (a is TwinsSection)
            {
                c = new SectionController(this, (TwinsSection)a);
                foreach (var i in ((TwinsSection)a).Records)
                {
                    GenTreeNode(i, c, cached);
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
            else if (a is Skydome)
                c = new SkydomeController(this, (Skydome)a);
            else if (a is GameObject)
                c = new ObjectController(this, (GameObject)a);
            else if (a is CodeModel)
                c = new CodeModelController(this, (CodeModel)a);
            else if (a is Script)
                c = new ScriptController(this, (Script)a);
            else if (a is Animation)
                c = new AnimationController(this, (Animation)a);
            else if (a is SoundEffect)
                c = new SEController(this, (SoundEffect)a);
            else if (a is AIPosition)
                c = new AIPositionController(this, (AIPosition)a);
            else if (a is AIPath)
                c = new AIPathController(this, (AIPath)a);
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
            else if (a is GraphicsInfo)
                c = new GraphicsInfoController(this, (GraphicsInfo)a);
            else if (a is ArmatureModel)
                c = new ArmatureModelController(this, (ArmatureModel)a);
            else if (a is ArmatureModelX)
                c = new ArmatureModelXController(this, (ArmatureModelX)a);
            else if (a is MaterialDemo)
                c = new MaterialDController(this, (MaterialDemo)a);
            else if (a is SceneryData)
                c = new SceneryDataController(this, (SceneryData)a);
            else if (a is SpecialModel)
                c = new SpecialModelController(this, (SpecialModel)a);
            else if (a is ParticleData)
                c = new ParticleDataController(this, (ParticleData)a);
            else if (a is DynamicSceneryData)
                c = new DynamicSceneryDataController(this, (DynamicSceneryData)a);
            else if (a is CollisionSurface)
                c = new CollisionSurfaceController(this, (CollisionSurface)a);
            else if (a is Camera)
                c = new CameraController(this, (Camera)a);
            else if (a is InstanceTemplate)
                c = new InstaceTemplateController(this, (InstanceTemplate)a);
            else
                c = new ItemController(this, a);

            if (!cached)
                c.UpdateText();
            controller.AddNode(c);
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            TreeView tree = (TreeView)sender;
            if (e.KeyCode == Keys.Enter && tree.SelectedNode != null && tree.SelectedNode.Tag is Controller c)
                CurCont.OpenEditor(c);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openRM2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = Settings.Default.ChunkFilePath,
                Filter = "RM2 files|*.rm2|SM2 files|*.sm2|RMX files|*.rmx|SMX files|*.smx|Demo RM2 files|*.rm2|Demo SM2 files|*.sm2|Frontend|Frontend.bin"
                //Filter = "PS2 files (.rm2; .sm2)|*.rm2;*.sm2|XBOX files (.rmx; .smx)|*.rmx;*.smx|Demo files (.rm2; .sm2)|*.rm2; *.sm2";
            })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (CurCont != null)
                        CurCont.CloseFile();
                    Tag = null;
                    Settings.Default.ChunkFilePath = ofd.FileName.Substring(0, ofd.FileName.LastIndexOf('\\'));
                    TwinsFile file = new TwinsFile();
                    TwinsFile aux_file = null;
                    TwinsFile default_file = null;
                    bool IsScenery = ofd.FileName.Contains(".sm");
                    switch (ofd.FilterIndex)
                    {
                        case 1:
                        case 2:
                            if (IsScenery)
                                file.LoadFile(ofd.FileName, TwinsFile.FileType.SM2);
                            else
                            {
                                file.LoadFile(ofd.FileName, TwinsFile.FileType.RM2);
                                aux_file = new TwinsFile();
                                aux_file.LoadFile(ofd.FileName.Substring(0, ofd.FileName.LastIndexOf('.')) + ".sm2", TwinsFile.FileType.SM2);
                                if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/Default.rm2"))
                                {
                                    default_file = new TwinsFile();
                                    default_file.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "/Default.rm2", TwinsFile.FileType.RM2);
                                }
                            }
                            break;
                        case 3:
                        case 4:
                            if (IsScenery)
                                file.LoadFile(ofd.FileName, TwinsFile.FileType.SMX);
                            else
                            {
                                file.LoadFile(ofd.FileName, TwinsFile.FileType.RMX);
                                aux_file = new TwinsFile();
                                aux_file.LoadFile(ofd.FileName.Substring(0, ofd.FileName.LastIndexOf('.')) + ".smx", TwinsFile.FileType.SMX);
                            }
                            break;
                        case 5:
                        case 6:
                            if (IsScenery)
                                file.LoadFile(ofd.FileName, TwinsFile.FileType.DemoSM2);
                            else
                            {
                                file.LoadFile(ofd.FileName, TwinsFile.FileType.DemoRM2);
                                aux_file = new TwinsFile();
                                aux_file.LoadFile(ofd.FileName.Substring(0, ofd.FileName.LastIndexOf('.')) + ".sm2", TwinsFile.FileType.DemoSM2);
                            }
                            break;
                        case 7:
                            file.LoadFile(ofd.FileName, TwinsFile.FileType.Frontend);
                            break;
                    }
                    if (IsScenery)
                    {
                        sMViewerToolStripMenuItem.Enabled = true;
                        rMViewerToolStripMenuItem.Enabled = false;
                    }
                    else
                    {
                        rMViewerToolStripMenuItem.Enabled = true;
                        sMViewerToolStripMenuItem.Enabled = false;
                    }
                    file.SafeFileName = ofd.SafeFileName;
                    Tag = new FileController(this, file);
                    ((FileController)Tag).DataAux = aux_file;
                    ((FileController)Tag).DataDefault = default_file;
                    if (default_file != null)
                    {
                        ((FileController)Tag).DefaultCont = new FileController(this, default_file);
                        foreach (var i in ((FileController)Tag).DataDefault.Records)
                        {
                            GenTreeNode(i, ((FileController)Tag).DefaultCont, true);
                        }
                    }
                    GenTree();
                    Text = $"Twinsaity Editor [{ofd.FileName}]";
                }
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
                Text = $"Twinsaity Editor [{sfd.FileName}] ";
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

        private void eLFPatcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenEXETool();
        }

        private void sMViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurCont.OpenSMViewer();
        }

        private void mHMBToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMHTool();
        }

        public void OpenEXETool()
        {
            if (exeForm == null)
            {
                exeForm = new EXEPatcher();
                exeForm.FormClosed += delegate
                {
                    exeForm = null;
                };
            }
            else
                exeForm.Select();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TwinsaityEditor + Twinsanity API v0.24\nDeveloped by Neo_Kesha, Smartkin, ManDude, Marko, BetaM\nSource code available at: https://github.com/smartkin/twinsanity-editor", "About", MessageBoxButtons.OK);
        }

        public void OpenMHTool()
        {
            if (mhForm == null)
            {
                mhForm = new MHViewer();
                mhForm.FormClosed += delegate
                {
                    mhForm = null;
                };
            }
            else
                mhForm.Select();
        }

        public void Pref_TruncateNames_Click(object sender, EventArgs e)
        {
            Utils.TextUtils.Pref_TruncateObjectNames = !Utils.TextUtils.Pref_TruncateObjectNames;
            Pref_TruncateNames_toolStripMenuItem.Checked = Utils.TextUtils.Pref_TruncateObjectNames;
        }

        private void bHBDToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenBDTool();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Save();
        }

        private void imageMakerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenImageMaker();
        }

        public void OpenBDTool()
        {
            if (bdForm == null)
            {
                bdForm = new BDExplorer();
                bdForm.FormClosed += delegate
                {
                    bdForm = null;
                };
            }
            else
            {
                bdForm.Select();
            }
                
        }

        public void OpenImageMaker()
        {
            if (imageMakerForm == null)
            {
                imageMakerForm = new ImageMaker();
                imageMakerForm.FormClosed += delegate
                {
                    imageMakerForm = null;
                };
            } else
            {
                imageMakerForm.Select();
            }
        }

        public void Pref_EnableAllNames_Click(object sender, EventArgs e)
        {
            Utils.TextUtils.Pref_EnableAnyObjectNames = !Utils.TextUtils.Pref_EnableAnyObjectNames;
            Pref_EnableAllNames_toolStripMenuItem.Checked = Utils.TextUtils.Pref_EnableAnyObjectNames;
        }

    }
}
