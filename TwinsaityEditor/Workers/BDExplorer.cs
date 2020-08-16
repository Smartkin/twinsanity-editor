using SharpFont.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwinsaityEditor.Properties;
using WK.Libraries.BetterFolderBrowserNS;
using WK.Libraries.BetterFolderBrowserNS.Helpers;

namespace TwinsaityEditor
{
    public partial class BDExplorer : Form
    {
        private Data data = null;
        private String path;
        private String name;
        public BDExplorer()
        {
            InitializeComponent();
            Show();
        }

        private void BDExplorer_Load(object sender, EventArgs e)
        {

        }
        private void openBHBDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.InitialDirectory = Settings.Default.BDFilePath;
                    ofd.Filter = "Bandicoot Header|*.BH";
                    if (DialogResult.OK == ofd.ShowDialog())
                    {
                        var file = ofd.FileName;
                        path = Path.GetDirectoryName(file);
                        name = Path.GetFileNameWithoutExtension(file);
                        Settings.Default.BDFilePath = file.Substring(0, file.LastIndexOf('\\'));
                        LoadData(path, name);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }

        }
        private void LoadData(String path, String name)
        {
            data = null;
            using (FileStream fileStream = new FileStream(String.Format("{0}\\{1}.BH", path, name), FileMode.Open, FileAccess.Read))
            using (BinaryReader reader = new BinaryReader(fileStream))
            {
                data = new Data(reader);
            }
            if (null != data)
            {
                UpdateView();
            }
            else
            {
                throw new Exception("Error loading BH file");
            }
        }
        private void UpdateView()
        {
            archiveContentsTree.BeginUpdate();
            archiveContentsTree.Nodes.Clear();
            archiveContentsTree.Nodes.Add(new TreeNode("Contents"));
            foreach (BH_Record record in data.FileList)
            {
                AddNode(record);
            }
            archiveContentsTree.EndUpdate();
        }
        private void AddNode(BH_Record record)
        {
            String[] hierarchy = record.Path.Split('\\');
            TreeNode node = archiveContentsTree.TopNode;
            for (int i = 0; i < hierarchy.Length; ++i)
            {
                String nodeName = hierarchy[i];
                if (node.Nodes.ContainsKey(nodeName))
                {
                    node = node.Nodes.Find(nodeName, false)[0];
                }
                else
                {
                    node = node.Nodes.Add(nodeName, nodeName);
                    if (i == hierarchy.Length - 1)
                    {
                        node.Tag = record;
                    }
                }
            }

        }
        private class Data
        {
            public Data(BinaryReader reader)
            {
                Header = reader.ReadInt32();
                FileList = new List<BH_Record>();
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    FileList.Add(new BH_Record(reader));
                }
            }
            public Data(String folderToPack)
            {
                Header = 0x501;
                FileList = new List<BH_Record>();
                String[] files = Directory.GetFiles(folderToPack, "*.*", SearchOption.AllDirectories);
                Int32 offset = 0;
                foreach (String file in files)
                {
                    BH_Record last = new BH_Record(folderToPack, file, offset);
                    offset += last.Length;
                    FileList.Add(last);
                }
            }
            public Int32 Header { get; private set; }
            public List<BH_Record> FileList { get; private set; }
            public void WriteDataBH(BinaryWriter writer, Action<String> callback)
            {
                writer.Write(Header);
                foreach (BH_Record record in FileList)
                {
                    record.WriteDataBH(writer, callback);
                }
            }
            public void WriteDataBD(String source, BinaryWriter writer, Action<String> callback)
            {
                foreach (BH_Record record in FileList)
                {
                    record.WriteDataBD(source, writer, callback);
                }
            }
        }
        private class BH_Record
        {
            public BH_Record(String root, String fileName, Int32 offset)
            {
                if (!root.EndsWith("\\")) root += "\\";
                Path = fileName.Replace(root, "");
                FileInfo info = new FileInfo(fileName);
                Length = (Int32)info.Length;
                Offset = offset;
            }
            public BH_Record(BinaryReader reader)
            {
                Int32 nameLength = reader.ReadInt32();
                Path = new string(reader.ReadChars(nameLength));
                Offset = reader.ReadInt32();
                Length = reader.ReadInt32();
            }
            public String Path { get; private set; }
            public Int32 Offset { get; private set; }
            public Int32 Length { get; private set; }
            public void WriteDataBH(BinaryWriter writer, Action<String> callback)
            {
                callback.Invoke(String.Format("Writing Header: {0}", Path));
                writer.Write((Int32)Path.Length);
                writer.Write(Path.ToCharArray());
                writer.Write(Offset);
                writer.Write(Length);

            }
            public void WriteDataBD(String source, BinaryWriter writer, Action<String> callback)
            {
                callback.Invoke(String.Format("Writing Data: {0}", Path));
                using (FileStream fileStream = new FileStream(System.IO.Path.Combine(source,Path), FileMode.Open, FileAccess.Read))
                using (BinaryReader reader = new BinaryReader(fileStream))
                {
                    writer.BaseStream.Position = Offset;
                    writer.Write(reader.ReadBytes(Length));
                }
            }
        }

        private void extractAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (BetterFolderBrowser ofd = new BetterFolderBrowser
                {
                    Title = "Select destination folder",
                    RootFolder = Settings.Default.BDExtractPath
                })
                {
                    if (null != data)
                    {
                        using (FileStream fileStream = new FileStream(String.Format("{0}\\{1}.BD", path, name), FileMode.Open, FileAccess.Read))
                        using (BinaryReader reader = new BinaryReader(fileStream))
                        {
                            if (DialogResult.OK == ofd.ShowDialog(this))
                            {
                                Settings.Default.BDExtractPath = ofd.SelectedPath;
                                ExtractRecursively(reader, archiveContentsTree.TopNode, ofd.SelectedPath);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
            CallBack("Ready");
        }
        private void extractSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (BetterFolderBrowser ofd = new BetterFolderBrowser
                {
                    Title = "Select destination folder",
                    RootFolder = Settings.Default.BDExtractPath
                })
                {
                    if (null != data && null != archiveContentsTree.SelectedNode)
                    {
                        using (FileStream fileStream = new FileStream(String.Format("{0}\\{1}.BD", path, name), FileMode.Open, FileAccess.Read))
                        using (BinaryReader reader = new BinaryReader(fileStream))
                        {
                            if (DialogResult.OK == ofd.ShowDialog(this))
                            {
                                Settings.Default.BDExtractPath = ofd.SelectedPath;
                                ExtractRecursively(reader, archiveContentsTree.SelectedNode, ofd.SelectedPath);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
            CallBack("Ready");
        }
        private void ExtractRecursively(BinaryReader source, TreeNode node, String extractionPath)
        {
            BH_Record record = (BH_Record)node.Tag;
            if (null != record)
            {
                ExtractRecord(source, record, extractionPath);
            }
            else
            {
                foreach (TreeNode childNode in node.Nodes)
                {
                    ExtractRecursively(source, childNode, extractionPath);
                }
            }
        }

        private void ExtractRecord(BinaryReader source, BH_Record record, String extractionPath)
        {
            String fullPath = Path.Combine(extractionPath, record.Path);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            using (FileStream fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            using (BinaryWriter writer = new BinaryWriter(fileStream))
            {
                CallBack(String.Format("Extracting: {0}", record.Path));
                source.BaseStream.Position = record.Offset;
                writer.Write(source.ReadBytes(record.Length));
            }
        }

        private void ShowError(String msg)
        {
            MessageBox.Show(String.Format("Unexpected exception happened\nMessage: {0}", msg), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void saveBHBDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                String sourcePath = null;
                String destinationPath = null;
                String name = "CRASH"; //Hardcoded name
                using (BetterFolderBrowser ofd = new BetterFolderBrowser
                {
                    Title = "Select source folder",
                    RootFolder = Settings.Default.BDSaveSrcPath
                })
                {
                    if (DialogResult.OK == ofd.ShowDialog(this))
                    {
                        Settings.Default.BDSaveSrcPath = ofd.SelectedPath;
                        sourcePath = ofd.SelectedPath;
                    }
                    else
                    {
                        return;
                    }
                }
                using (BetterFolderBrowser ofd = new BetterFolderBrowser
                {
                    Title = "Select source folder",
                    RootFolder = Settings.Default.BDSaveSrcPath
                })
                {
                    ofd.Title = "Select destination folder";
                    ofd.RootFolder = Settings.Default.BDSaveDstPath;
                    if (DialogResult.OK == ofd.ShowDialog(this))
                    {
                        Settings.Default.BDSaveDstPath = ofd.SelectedPath;
                        destinationPath = ofd.SelectedPath;
                        if (
                            File.Exists(Path.Combine(destinationPath, String.Format("{0}.BH", name))) ||
                            File.Exists(Path.Combine(destinationPath, String.Format("{0}.BD", name)))
                            )
                        {
                            DialogResult result = MessageBox.Show(String.Format("Archive with name {0} already in destination folder. Overwrite?", name), "Attention",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (DialogResult.Yes != result)
                            {
                                destinationPath = null;
                            }
                        }

                    }
                }
                if (null != sourcePath && null != destinationPath)
                {
                    PackArchive(sourcePath, destinationPath, name);
                    UpdateView();
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
            CallBack("Ready");
        }
        private void PackArchive(String source, String destination, String name)
        {
            data = new Data(source);
            String fullPathBH = Path.Combine(destination, String.Format("{0}.BH", name));
            String fullPathBD = Path.Combine(destination, String.Format("{0}.BD", name));
            using (FileStream fileStream = new FileStream(fullPathBH, FileMode.Create, FileAccess.Write))
            using (BinaryWriter writer = new BinaryWriter(fileStream))
            {
                data.WriteDataBH(writer, CallBack);
            }
            using (FileStream fileStream = new FileStream(fullPathBD, FileMode.Create, FileAccess.Write))
            using (BinaryWriter writer = new BinaryWriter(fileStream))
            {
                data.WriteDataBD(source, writer, CallBack);
            }
        }

        private void CallBack(String message)
        {
            statusBar.Text = message;
            Application.DoEvents();
        }
    }
}
