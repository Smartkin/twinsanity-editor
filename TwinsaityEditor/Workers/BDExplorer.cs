using SharpFont.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                    ofd.Filter = "Bandicoot Header|*.BH";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        path = Path.GetDirectoryName(ofd.FileName);
                        name = Path.GetFileNameWithoutExtension(ofd.FileName);
                        LoadData(path, name);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Unexpected exception happened\nMessage: {0}", ex.Message), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    node = node.Nodes.Add(nodeName,nodeName);
                    if (i == hierarchy.Length -1)
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
            public Int32 Header { get; private set; }
            public List<BH_Record> FileList { get; private set; }
        }
        private class BH_Record
        {
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
        }
    }
}
