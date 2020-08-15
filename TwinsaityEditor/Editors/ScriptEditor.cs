using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public partial class ScriptEditor : Form
    {
        private SectionController controller;
        private Script script;

        private FileController File { get; set; }
        private TwinsFile FileData { get => File.Data; }
        public ScriptEditor(SectionController c)
        {
            File = c.MainFile;
            controller = c;
            Text = $"Instance Editor (Section {c.Data.Parent.ID})";
            InitializeComponent();
            PopulateList();
            UpdatePanels();
        }

        private void ScriptEditor_Load(object sender, EventArgs e)
        {

        }
        private void PopulateList()
        {
            listBox1.BeginUpdate();
            listBox1.Items.Clear();
            foreach (Script i in controller.Data.Records)
            {
                listBox1.Items.Add(GenTextForList(i));
            }
            listBox1.EndUpdate();
        }
        private string GenTextForList(Script script)
        {
            return $"ID {script.ID} {(script.Name == string.Empty ? string.Empty : $" - {script.Name}")}";
        }
        private void BuildTree()
        {
            scriptTree.BeginUpdate();
            scriptTree.Nodes.Clear();
            scriptTree.Nodes.Add(script.Name);
            if (script.HeaderScript != null)
            {
                scriptTree.TopNode.Nodes.Add("Header Script").Tag = script.HeaderScript;
            } 
            if (script.MainScript != null)
            {
                TreeNode mainScriptNode = scriptTree.TopNode.Nodes.Add("Main Script");
                mainScriptNode.Tag = script.MainScript;
                Script.MainScriptStruct mainScript = script.MainScript;
                Script.MainScriptStruct.LinkedScript ptr = mainScript.linkedScript1;
                while (ptr != null)
                {
                    AddLinked(mainScriptNode, ptr);
                    ptr = ptr.nextLinked;
                }
            }
            scriptTree.EndUpdate();
        }
        private void AddLinked(TreeNode parent, Script.MainScriptStruct.LinkedScript ptr)
        {
            TreeNode node = parent.Nodes.Add($"Linked Script {ptr.scriptIndexOrSlot}");
            node.Tag = ptr;
            AddType1(node, ptr.type1);
            Script.MainScriptStruct.SupportType2 ptrType2 = ptr.type2;
            while (ptrType2 != null)
            {
                AddType2(node, ptrType2);
                ptrType2 = ptrType2.nextType2;
            }
        }
        private void AddType1(TreeNode parent, Script.MainScriptStruct.SupportType1 ptr)
        {
            TreeNode node = parent.Nodes.Add($"Type 1");
            node.Tag = ptr;
        }
        private void AddType2(TreeNode parent, Script.MainScriptStruct.SupportType2 ptr)
        {
            TreeNode node = parent.Nodes.Add($"Type 2 Linked Slot: {ptr.linkedScriptListIndex}");
            node.Tag = ptr;
            AddType3(node, ptr.type3);
            Script.MainScriptStruct.SupportType4 ptrType4 = ptr.type4;
            while (ptrType4 != null)
            {
                AddType4(node, ptrType4);
                ptrType4 = ptrType4.nextType4;
            }
        }
        private void AddType3(TreeNode parent, Script.MainScriptStruct.SupportType3 ptr)
        {
            TreeNode node = parent.Nodes.Add($"Type 3");
            node.Tag = ptr;
        }
        private void AddType4(TreeNode parent, Script.MainScriptStruct.SupportType4 ptr)
        {
            TreeNode node = parent.Nodes.Add($"Type 4 ");
            node.Tag = ptr;
        }
        private void UpdatePanels()
        {
            panelHeader.Visible = false;
            panelMain.Visible = false;
            panelType1.Visible = false;
            panelType2.Visible = false;
            panelType3.Visible = false;
            panelType4.Visible = false;
            panelLinked.Visible = false;
            if (null != scriptTree.SelectedNode)
            {
                Object tag = scriptTree.SelectedNode.Tag;
                if (tag is Script.HeaderScriptStruct)
                {
                    panelHeader.Visible = true;
                }
                if (tag is Script.MainScriptStruct)
                {
                    panelMain.Visible = true;
                }
                if (tag is Script.MainScriptStruct.SupportType1)
                {
                    panelType1.Visible = true;
                }
                if (tag is Script.MainScriptStruct.SupportType2)
                {
                    panelType2.Visible = true;
                }
                if (tag is Script.MainScriptStruct.SupportType3)
                {
                    panelType3.Visible = true;
                }
                if (tag is Script.MainScriptStruct.SupportType4)
                {
                    panelType4.Visible = true;
                }
                if (tag is Script.MainScriptStruct.LinkedScript)
                {
                    panelLinked.Visible = true;
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (null != listBox1.SelectedItem)
            {
                File.SelectItem((Script)controller.Data.Records[listBox1.SelectedIndex]);
                script = (Script)File.SelectedItem;
            } 
            else
            {
                File.SelectItem(null);
                script = null;
            }
            BuildTree();
        }

        private void scriptTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdatePanels();
        }
    }
}
