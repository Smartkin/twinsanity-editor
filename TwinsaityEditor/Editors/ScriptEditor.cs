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

        private Script.HeaderScriptStruct selectedHeaderScript;
        private Script.MainScriptStruct selectedMainScript;
        private Script.MainScriptStruct.SupportType1 selectedType1;
        private Script.MainScriptStruct.SupportType2 selectedType2;
        private Script.MainScriptStruct.SupportType3 selectedType3;
        private Script.MainScriptStruct.SupportType4 selectedType4;
        private Script.MainScriptStruct.LinkedScript selectedLinked;
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
            if (null != script)
            {
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
                    selectedHeaderScript = (Script.HeaderScriptStruct)tag;
                    UpdateHeaderPanel();
                }
                if (tag is Script.MainScriptStruct)
                {
                    panelMain.Visible = true;
                    selectedMainScript = (Script.MainScriptStruct)tag;
                    UpdateMainPanel();
                }
                if (tag is Script.MainScriptStruct.SupportType1)
                {
                    panelType1.Visible = true;
                    selectedType1 = (Script.MainScriptStruct.SupportType1)tag;
                    UpdateType1Panel();
                }
                if (tag is Script.MainScriptStruct.SupportType2)
                {
                    panelType2.Visible = true;
                    selectedType2 = (Script.MainScriptStruct.SupportType2)tag;
                    UpdateType2Panel();
                }
                if (tag is Script.MainScriptStruct.SupportType3)
                {
                    panelType3.Visible = true;
                    selectedType3 = (Script.MainScriptStruct.SupportType3)tag;
                    UpdateType3Panel();
                }
                if (tag is Script.MainScriptStruct.SupportType4)
                {
                    panelType4.Visible = true;
                    selectedType4 = (Script.MainScriptStruct.SupportType4)tag;
                    UpdateType4Panel();
                }
                if (tag is Script.MainScriptStruct.LinkedScript)
                {
                    panelLinked.Visible = true;
                    selectedLinked = (Script.MainScriptStruct.LinkedScript)tag;
                    UpdateLinkedPanel();
                }
            }
        }

        private void UpdateHeaderPanel()
        {
            headerSubScripts.Items.Clear();
            foreach (Script.HeaderScriptStruct.UnkIntPairs pair in selectedHeaderScript.pairs)
            {
                headerSubScripts.Items.Add(pair);
            }
            if (headerSubScripts.Items.Count > 0)
            {
                headerSubScripts.SelectedIndex = 0;
            }
        }
        private void headerSubScripts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (headerSubScripts.SelectedItem != null)
            {
                Script.HeaderScriptStruct.UnkIntPairs pair = selectedHeaderScript.pairs[headerSubScripts.SelectedIndex];
                headerSubscriptID.Text = pair.mainScriptIndex.ToString();
                headerSubscriptArg.Text = pair.unkInt2.ToString();
            }
        }
        private void headerSubscriptID_TextChanged(object sender, EventArgs e)
        {
            if (headerSubScripts.SelectedItem != null)
            {
                Script.HeaderScriptStruct.UnkIntPairs pair = selectedHeaderScript.pairs[headerSubScripts.SelectedIndex];
                TextBox textBox = (TextBox)sender;
                Int32 val = pair.mainScriptIndex;
                if (Int32.TryParse(textBox.Text, out val))
                {
                    textBox.BackColor = Color.White;
                    pair.mainScriptIndex = val;
                    headerSubScripts.SelectedItem = pair;
                    headerSubScripts.Text = headerSubScripts.SelectedItem.ToString();
                }
                else
                {
                    textBox.BackColor = Color.Red;
                }

            }
        }

        private void headerSubscriptArg_TextChanged(object sender, EventArgs e)
        {
            if (headerSubScripts.SelectedItem != null)
            {
                Script.HeaderScriptStruct.UnkIntPairs pair = selectedHeaderScript.pairs[headerSubScripts.SelectedIndex];
                TextBox textBox = (TextBox)sender;
                UInt32 val = pair.unkInt2;
                if (UInt32.TryParse(textBox.Text, out val))
                {
                    textBox.BackColor = Color.White;
                    pair.unkInt2 = val;
                    headerSubScripts.SelectedItem = pair;
                    headerSubScripts.Text = headerSubScripts.SelectedItem.ToString();
                }
                else
                {
                    textBox.BackColor = Color.Red;
                }

            }
        }
        private void UpdateMainPanel()
        {
            mainName.Text = selectedMainScript.name;
            mainLinkedCnt.Text = selectedMainScript.LinkedScriptsCount.ToString();
            mainUnk.Text = selectedMainScript.unkInt2.ToString();
            mainLinkedPos.Text = "0";
        }
        private void mainName_TextChanged(object sender, EventArgs e)
        {
            selectedMainScript.name = ((TextBox)sender).Text;
            scriptTree.TopNode.Text = selectedMainScript.name;
            //listBox1.Items[listBox1.SelectedIndex] = GenTextForList(script); Fuck this shit for being unstable piss that destroys my will to live
        }

        private void mainUnk_TextChanged(object sender, EventArgs e)
        {
            Int32 val = selectedMainScript.unkInt2;
            if (Int32.TryParse(((TextBox)sender).Text, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedMainScript.unkInt2 = val;
            } else
            {
                ((TextBox)sender).BackColor = Color.Red;
            }
        }

        private void mainLinkedPos_TextChanged(object sender, EventArgs e)
        {

        }

        private void mainAddLinked_Click(object sender, EventArgs e)
        {

        }

        private void mainDelLinked_Click(object sender, EventArgs e)
        {

        }
        private void UpdateType1Panel()
        {

        }
        private void UpdateType2Panel()
        {

        }
        private void UpdateType3Panel()
        {

        }
        private void UpdateType4Panel()
        {

        }
        private void UpdateLinkedPanel()
        {

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
            UpdatePanels();
        }

        private void scriptTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdatePanels();
        }
    }
}
