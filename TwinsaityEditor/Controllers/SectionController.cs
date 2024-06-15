using Twinsanity;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Linq;

namespace TwinsaityEditor
{
    public class SectionController : Controller
    {
        public TwinsSection Data { get; set; }
        public FileController MainFile { get; private set; }

        public SectionController(MainForm topform, TwinsSection item) : base (topform)
        {
            MainFile = TopForm.CurCont;
            Data = item;
            if (item is TwinsFile f)
            {
                if (f.Type == TwinsFile.FileType.RM2)
                {
                    AddMenu("Add remaining instance sections", Menu_FileFillInstanceSections);
                    AddMenu("Import object package (RM2)", Menu_ImportPackage);
                }
                AddMenu($"Toggle default asset names", Menu_ToggleDefaultNames);
            }
            else
            {
                AddMenu("Open editor", Menu_OpenEditor);
                AddMenu("Add new item", Menu_AddNew);
                AddMenu("Add item from raw data file", Menu_AddFromFile);
                AddMenu("Re-ID by order", Menu_ReIDByOrder);
                AddMenu("Re-order by ID (asc.)", Menu_ReOrderByID_Asc);
                AddMenu("Re-order by ID (desc.)", Menu_ReOrderByID_Desc);
                if (item.Type == SectionType.Model || item.Type == SectionType.ModelX || item.Type == SectionType.RigidModel || item.Type == SectionType.Mesh)
                {
                    AddMenu("Export all meshes to PLY", Menu_ExportAllPLY);
                }
                else if (item.Type == SectionType.Instance)
                {
                    AddMenu("Clear instance section", Menu_ClearInstanceSection);
                    AddMenu("Fill instance section", Menu_FillInstanceSection);
                }
                else if (item.Type >= SectionType.SE && item.Type <= SectionType.SE_Jpn)
                {
                    AddMenu("Extract extra data", Menu_ExtractExtraData);
                }
            }
            
        }

        protected override string GetName()
        {
            return $"{Data.Type} Section [ID {Data.ID}]";
        }

        protected override void GenText()
        {
            TextPrev = new string[Data.ExtraData == null ? 3 : 4];
            TextPrev[0] = $"ID: {Data.ID}";
            TextPrev[1] = $"Size: {Data.Size}";
            TextPrev[2] = $"ContentSize: {Data.ContentSize} Element Count: {Data.Records.Count}";
            if (Data.ExtraData != null)
                TextPrev[3] = $"ExtraDataSize: {Data.ExtraData.Length}";
        }

        public void AddItem(uint id, TwinsItem item)
        {
            Data.AddItem(id, item);
            TopForm.GenTreeNode(item, this);
            UpdateText();
            ((Controller)Node.Nodes[Data.RecordIDs[item.ID]].Tag).UpdateText();
        }

        public void RemoveItem(TwinsItem item)
        {
            RemoveItem(item.ID);
        }

        public void RemoveItem(uint id)
        {
            Node.Nodes[Data.RecordIDs[id]].Remove();
            Data.RemoveItem(id);
            UpdateText();
        }

        public void ChangeID(uint old_id, uint new_id)
        {
            if (Data.ContainsItem(new_id))
                throw new System.ArgumentException("New ID already exists.");
            var index = Data.RecordIDs[old_id];
            Data.GetItem<TwinsItem>(old_id).ID = new_id;
            Data.RecordIDs.Remove(old_id);
            Data.RecordIDs.Add(new_id, index);
        }

        private Controller GetItem(uint id)
        {
            return (Controller)Node.Nodes[Data.RecordIDs[id]].Tag;
        }

        public T GetItem<T>(uint id) where T : Controller
        {
            return Node.Nodes[Data.RecordIDs[id]].Tag as T;
        }


        private void Menu_AddNew()
        {
            TwinsItem newItem = null;
            switch (Data.Type)
            {
                case SectionType.Texture: break;
                case SectionType.TextureX: break;
                case SectionType.TextureP: break;
                case SectionType.Material: newItem = new Material(); break;
                case SectionType.MaterialD: break;
                case SectionType.Model:  break;
                case SectionType.ModelX: break;
                case SectionType.ModelP: break;
                case SectionType.RigidModel: newItem = new RigidModel(); break;
                case SectionType.Skin: break;
                case SectionType.SkinX: break;
                case SectionType.BlendSkin: break;
                case SectionType.BlendSkinX: break;
                case SectionType.Mesh: break;
                case SectionType.LodModel: newItem = new LodModel(); break;
                case SectionType.LodModelMB: break;
                case SectionType.Skydome: newItem = new Skydome(); break;

                case SectionType.Object: newItem = new GameObject(); break;
                case SectionType.ObjectDemo: break;
                case SectionType.ObjectMB: break;
                case SectionType.ScriptDemo: newItem = new Script(); break;
                case SectionType.ScriptX: newItem = new Script(); break;
                case SectionType.Script: newItem = new Script(); break;
                case SectionType.Animation: break;
                case SectionType.OGI: newItem = new GraphicsInfo(); break;
                case SectionType.GraphicsInfoMB: break;
                case SectionType.GraphicsInfoP: break;
                case SectionType.CustomAgent: break;
                case SectionType.CustomAgentX: break;
                case SectionType.CustomAgentDemo: break;
                case SectionType.SE:
                case SectionType.SE_Eng:
                case SectionType.SE_Fre:
                case SectionType.SE_Ger:
                case SectionType.SE_Ita:
                case SectionType.SE_Spa:
                case SectionType.SE_Jpn:
                    newItem = new SoundEffect();
                    ((SoundEffect)newItem).SoundOffset = (uint)Data.ExtraData.Length;
                    break;
                case SectionType.Xbox_SE:
                case SectionType.Xbox_SE_Eng:
                case SectionType.Xbox_SE_Fre:
                case SectionType.Xbox_SE_Ger:
                case SectionType.Xbox_SE_Ita:
                case SectionType.Xbox_SE_Jpn:
                case SectionType.Xbox_SE_Spa:
                    break;

                case SectionType.AIPosition: newItem = new AIPosition(); break;
                case SectionType.AIPath: newItem = new AIPath(); break;
                case SectionType.Position: newItem = new Position(); break;
                case SectionType.Path: newItem = new Twinsanity.Path(); break;
                case SectionType.CollisionSurface: newItem = new CollisionSurface(); break;
                case SectionType.ObjectInstance: newItem = new Instance(); break;
                case SectionType.ObjectInstanceDemo: break;
                case SectionType.ObjectInstanceMB: break;
                case SectionType.Trigger: newItem = new Trigger(); break;
                case SectionType.Camera: newItem = new Camera(); break;
                case SectionType.CameraDemo: break;
                
                default: break;
            }

            if (newItem == null)
            {
                MessageBox.Show("Adding this item type is unsupported.");
            }
            else
            {
                uint newId = 0;
                if (Data.RecordIDs.Count != 0)
                    newId = Data.RecordIDs.Keys.Max() + 1;
                newItem.ID = newId;
                newItem.Parent = Data;
                AddItem(newId, newItem);
            }
            
        }

        private void Menu_ExtractExtraData()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = GetName().Replace(":", string.Empty);
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileStream file = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                BinaryWriter writer = new BinaryWriter(file);
                writer.Write(Data.ExtraData);
                writer.Close();
            }
        }

        private void Menu_ClearInstanceSection()
        {
            for (uint i = 0; i <= 8; ++i)
            {
                if (Data.ContainsItem(i))
                {
                    RemoveItem(i);
                }
            }
        }

        private void Menu_FillInstanceSection()
        {
            for (uint i = 0; i <= 8; ++i)
            {
                SectionType type = SectionType.Null;
                switch (i)
                {
                    case 0: type = SectionType.InstanceTemplate; break;
                    case 1: type = SectionType.AIPosition; break;
                    case 2: type = SectionType.AIPath; break;
                    case 3: type = SectionType.Position; break;
                    case 4: type = SectionType.Path; break;
                    case 5: type = SectionType.CollisionSurface; break;
                    case 6: type = SectionType.ObjectInstance; break;
                    case 7: type = SectionType.Trigger; break;
                    case 8: type = SectionType.Camera; break;
                }
                if (!Data.ContainsItem(i))
                {
                    TwinsSection sec = new TwinsSection
                    {
                        ID = i,
                        Level = Data.Level + 1,
                        Type = type,
                        Parent = Data
                    };
                    AddItem(i, sec);
                }
            }
        }

        private void Menu_FileFillInstanceSections()
        {
            for (uint i = 0; i <= 7; ++i)
            {
                if (!Data.ContainsItem(i))
                {
                    TwinsSection sec = new TwinsSection
                    {
                        ID = i,
                        Level = Data.Level + 1,
                        Type = SectionType.Instance,
                        Parent = Data
                    };
                    AddItem(i, sec);
                }
            }
        }

        public void Menu_ImportPackage()
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "RM2 file|*.rm2";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var package = new TwinsFile();
                    package.LoadFile(ofd.FileName, TwinsFile.FileType.RM2);
                    TopForm.CurCont.Data.Merge(package);
                    Node.TreeView.BeginUpdate();
                    Node.Nodes.Clear();
                    foreach (var i in Data.RecordIDs)
                    {
                        TopForm.GenTreeNode(Data.Records[i.Value], this);
                    }
                    Node.TreeView.EndUpdate();
                }
            }
        }

        private void Menu_ReOrderByID_Asc()
        {
            if (Data.Type == SectionType.ObjectInstance)
            {
                MainFile.CloseEditor(Editors.Instance, (int)Data.Parent.ID);
            }
            else if (Data.Type == SectionType.Position)
            {
                MainFile.CloseEditor(Editors.Position, (int)Data.Parent.ID);
            }
            Node.TreeView.BeginUpdate();
            Node.Nodes.Clear();
            SortedDictionary<uint, int> sdic = new SortedDictionary<uint, int>(Data.RecordIDs);
            List<TwinsItem> slist = new List<TwinsItem>();
            foreach (var i in sdic)
            {
                slist.Add(Data.Records[i.Value]);
                TopForm.GenTreeNode(Data.Records[i.Value], this);
            }
            Data.Records = slist;
            Node.TreeView.EndUpdate();
        }

        private void Menu_ReOrderByID_Desc()
        {
            if (Data.Type == SectionType.ObjectInstance)
            {
                MainFile.CloseEditor(Editors.Instance, (int)Data.Parent.ID);
            }
            else if (Data.Type == SectionType.Position)
            {
                MainFile.CloseEditor(Editors.Position, (int)Data.Parent.ID);
            }
            Node.TreeView.BeginUpdate();
            Node.Nodes.Clear();
            SortedDictionary<uint, int> sdic = new SortedDictionary<uint, int>(new Utils.DescendingComparer<uint>());
            foreach (var i in Data.RecordIDs)
                sdic.Add(i.Key, i.Value);
            List<TwinsItem> slist = new List<TwinsItem>();
            foreach (var i in sdic)
            {
                slist.Add(Data.Records[i.Value]);
                TopForm.GenTreeNode(Data.Records[i.Value], this);
            }
            Data.Records = slist;
            Node.TreeView.EndUpdate();
        }

        private void Menu_ReIDByOrder()
        {
            if (Data.Type == SectionType.ObjectInstance)
            {
                MainFile.CloseEditor(Editors.Instance, (int)Data.Parent.ID);
            }
            else if (Data.Type == SectionType.Position)
            {
                MainFile.CloseEditor(Editors.Position, (int)Data.Parent.ID);
            }
            Node.TreeView.BeginUpdate();
            Node.Nodes.Clear();
            Data.RecordIDs.Clear();
            for (int i = 0; i < Data.Records.Count; ++i)
            {
                Data.Records[i].ID = (uint)i;
                Data.RecordIDs.Add((uint)i, i);
                TopForm.GenTreeNode(Data.Records[i], this);
            }
            Node.TreeView.EndUpdate();
        }

        public void RefreshSection()
        {
            if (Data.Type == SectionType.ObjectInstance)
            {
                MainFile.CloseEditor(Editors.Instance, (int)Data.Parent.ID);
            }
            else if (Data.Type == SectionType.Position)
            {
                MainFile.CloseEditor(Editors.Position, (int)Data.Parent.ID);
            }
            Node.TreeView.BeginUpdate();
            Node.Nodes.Clear();
            for (int i = 0; i < Data.Records.Count; ++i)
            {
                TopForm.GenTreeNode(Data.Records[i], this);
            }
            Node.TreeView.EndUpdate();
        }

        private void Menu_OpenEditor()
        {
            MainFile.OpenEditor(this);
        }

        private void Menu_ExportAllPLY()
        {
            if (Data.Type == SectionType.RigidModel || Data.Type == SectionType.Mesh)
                if (MessageBox.Show("PLY export is experimental, material and texture information will not be exported. Continue anyway?", "Export Warning", MessageBoxButtons.YesNo) == DialogResult.No) return;
            var fdbSave = new CommonOpenFileDialog { IsFolderPicker = true };
            if (fdbSave.ShowDialog() == CommonFileDialogResult.Ok)
            {
                foreach (TreeNode n in Node.Nodes)
                {
                    string fname = fdbSave.FileName + @"\{n.Text}.ply";
                    if (n.Tag is ModelController c)
                    {
                        File.WriteAllBytes(fname, c.Data.ToPLY());
                    }
                    else if (n.Tag is RigidModelController d)
                    {
                        File.WriteAllBytes(fname, Data.Parent.GetItem<TwinsSection>(2).GetItem<Model>(d.Data.MeshID).ToPLY());
                    }
                }
            }
        }

        public void Menu_ToggleDefaultNames()
        {
            DefaultHashes.DefaultNames = !DefaultHashes.DefaultNames;
            RefreshSection();
        }

        private void Menu_AddFromFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                uint id = 0;
                if (Data.Records.Count > 0)
                {
                    id = Data.Records[Data.Records.Count - 1].ID + 1;
                }
                TwinsItem item = new TwinsItem() { ID = id };

                BinaryReader reader = new BinaryReader(new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read));
                item.Load(reader, (int)reader.BaseStream.Length);
                reader.Close();

                Data.AddItem(id, item);
                TopForm.GenTreeNode(item, this);
                UpdateText();
                ((Controller)Node.Nodes[Data.RecordIDs[id]].Tag).UpdateText();
            }
        }

    }
}
