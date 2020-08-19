using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
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
        private Func<Script, bool> scriptPredicate;
        private List<int> scriptIndices = new List<int>();
        public ScriptEditor(SectionController c)
        {
            File = c.MainFile;
            controller = c;
            Text = $"Instance Editor (Section {c.Data.Parent.ID})";
            scriptPredicate = s => { return s.Name.Contains(scriptNameFilter.Text); };
            InitializeComponent();
            PopulateList();
            UpdatePanels();
        }

        private void ScriptEditor_Load(object sender, EventArgs e)
        {
            filterSelection.SelectedIndex = 0;
        }
        private void PopulateList()
        {
            PopulateList(s => true);
        }
        private void PopulateList(Func<Script, bool> predicate)
        {
            scriptListBox.BeginUpdate();
            scriptListBox.Items.Clear();
            scriptIndices.Clear();
            var index = 0;
            foreach (Script i in controller.Data.Records)
            {
                if (predicate.Invoke(i))
                {
                    scriptIndices.Add(index);
                    scriptListBox.Items.Add(GenTextForList(i));
                }
                ++index;
            }
            scriptListBox.EndUpdate();
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
                    TreeNode mainScriptNode = scriptTree.TopNode.Nodes.Add("Main Script - State: " + script.MainScript.unkInt2);
                    mainScriptNode.Tag = script.MainScript;
                    Script.MainScriptStruct mainScript = script.MainScript;
                    Script.MainScriptStruct.LinkedScript ptr = mainScript.linkedScript1;
                    while (ptr != null)
                    {
                        AddLinked(mainScriptNode, ptr);
                        ptr = ptr.nextLinked;
                    }
                }
                scriptTree.Nodes[0].ExpandAll();
                scriptTree.EndUpdate();
            }
        }
        private void AddLinked(TreeNode parent, Script.MainScriptStruct.LinkedScript ptr)
        {
            string Name = $"State {parent.Nodes.Count}";
            if (ptr.scriptIndexOrSlot != -1)
            {
                if (Enum.IsDefined(typeof(DefaultEnums.ScriptID), (ushort)ptr.scriptIndexOrSlot))
                {
                    Name += $" - ID: {ptr.scriptIndexOrSlot} {(DefaultEnums.ScriptID)(ushort)ptr.scriptIndexOrSlot}";
                }
                else
                {
                    Name += $" - Script {ptr.scriptIndexOrSlot}";
                }
            }
            TreeNode node = parent.Nodes.Add(Name);
            node.Tag = ptr;
            if (null != ptr.type1)
            {
                AddType1(node, ptr.type1);
            }
            Script.MainScriptStruct.SupportType2 ptrType2 = ptr.type2;
            while (ptrType2 != null)
            {
                AddType2(node, ptrType2);
                ptrType2 = ptrType2.nextType2;
            }
        }
        private void AddType1(TreeNode parent, Script.MainScriptStruct.SupportType1 ptr)
        {
            TreeNode node = parent.Nodes.Add($"Header");
            node.Tag = ptr;
        }
        private void AddType2(TreeNode parent, Script.MainScriptStruct.SupportType2 ptr)
        {
            TreeNode node = parent.Nodes.Add($"Switch - Go To State: {ptr.linkedScriptListIndex}");
            node.Tag = ptr;
            if (null != ptr.type3)
            {
                AddType3(node, ptr.type3);
            }
            Script.MainScriptStruct.SupportType4 ptrType4 = ptr.type4;
            while (ptrType4 != null)
            {
                AddType4(node, ptrType4);
                ptrType4 = ptrType4.nextType4;
            }
        }
        private void AddType3(TreeNode parent, Script.MainScriptStruct.SupportType3 ptr)
        {
            string Name = $"Condition {ptr.VTableIndex}";
            if (Enum.IsDefined(typeof(DefaultEnums.ConditionID), ptr.VTableIndex))
            {
                Name = ((DefaultEnums.ConditionID)ptr.VTableIndex).ToString();
            }

            TreeNode node = parent.Nodes.Add(Name);
            node.Tag = ptr;
        }
        private void AddType4(TreeNode parent, Script.MainScriptStruct.SupportType4 ptr)
        {
            string Name = $"Command {ptr.VTableIndex}";
            if (Enum.IsDefined(typeof(DefaultEnums.CommandID), ptr.VTableIndex))
            {
                Name = ((DefaultEnums.CommandID)ptr.VTableIndex).ToString();
            }

            TreeNode node = parent.Nodes.Add(Name);
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
            panelGeneral.Visible = false;
            if (null != scriptTree.SelectedNode)
            {
                Object tag = scriptTree.SelectedNode.Tag;
                if (tag == null)
                {
                    panelGeneral.Visible = true;
                    UpdateGeneralPanel();
                }
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
                headerSubscriptID.Text = (pair.mainScriptIndex - 1).ToString();
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
                    pair.mainScriptIndex = val + 1;
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
            //scriptListBox.Items[scriptListBox.SelectedIndex] = GenTextForList(script); Fuck this shit for being unstable piss that destroys my will to live
        }

        private void mainUnk_TextChanged(object sender, EventArgs e)
        {
            Int32 val = selectedMainScript.unkInt2;
            if (Int32.TryParse(((TextBox)sender).Text, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedMainScript.unkInt2 = val;
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
            }
        }

        private void mainLinkedPos_TextChanged(object sender, EventArgs e)
        {

        }

        private void mainAddLinked_Click(object sender, EventArgs e)
        {
            Int32 val = 0;
            if (Int32.TryParse(mainLinkedPos.Text, out val))
            {
                if (selectedMainScript.AddLinkedScript(val))
                {
                    TreeNode mainNode = scriptTree.SelectedNode;
                    mainNode.Nodes.Clear();
                    Script.MainScriptStruct.LinkedScript ptr = selectedMainScript.linkedScript1;
                    while (ptr != null)
                    {
                        AddLinked(mainNode, ptr);
                        ptr = ptr.nextLinked;
                    }
                    UpdateMainPanel();
                }
            }
        }

        private void mainDelLinked_Click(object sender, EventArgs e)
        {
            Int32 val = 0;
            if (Int32.TryParse(mainLinkedPos.Text, out val))
            {
                if (selectedMainScript.DeleteLinkedScript(val))
                {
                    TreeNode mainNode = scriptTree.SelectedNode;
                    mainNode.Nodes.Clear();
                    Script.MainScriptStruct.LinkedScript ptr = selectedMainScript.linkedScript1;
                    while (ptr != null)
                    {
                        AddLinked(mainNode, ptr);
                        ptr = ptr.nextLinked;
                    }
                    UpdateMainPanel();
                }
            }
        }
        private void UpdateType1Panel()
        {
            type1UnkByte1.Text = selectedType1.unkByte1.ToString();
            type1UnkByte2.Text = selectedType1.unkByte2.ToString();
            type1UnkShort.Text = selectedType1.unkUShort1.ToString();
            type1UnkInt.Text = selectedType1.unkInt1.ToString();
            type1Array.Text = GetTextFromArray(selectedType1.byteArray);
        }

        private String GetTextFromArray(Byte[] array)
        {
            StringBuilder builder = new StringBuilder();
            foreach (Byte b in array)
            {
                builder.Append($"{b:X2} ");
            }
            return builder.ToString();
        }
        private void type1UnkByte1_TextChanged(object sender, EventArgs e)
        {
            Byte val = selectedType1.unkByte1;
            if (Byte.TryParse(((TextBox)sender).Text, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedType1.unkByte1 = val;
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
            }
            if (selectedType1.isValidArraySize())
            {
                type1Warning.Visible = false;
            }
            else
            {
                type1Warning.Visible = true;

            }
        }

        private void type1UnkByte2_TextChanged(object sender, EventArgs e)
        {
            Byte val = selectedType1.unkByte2;
            if (Byte.TryParse(((TextBox)sender).Text, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedType1.unkByte2 = val;
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
            }
            if (selectedType1.isValidArraySize())
            {
                type1Warning.Visible = false;
            }
            else
            {
                type1Warning.Visible = true;

            }
        }

        private void type1UnkShort_TextChanged(object sender, EventArgs e)
        {
            UInt16 val = selectedType1.unkUShort1;
            if (UInt16.TryParse(((TextBox)sender).Text, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedType1.unkUShort1 = val;
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
            }
        }

        private void type1UnkInt_TextChanged(object sender, EventArgs e)
        {
            Int32 val = selectedType1.unkInt1;
            if (Int32.TryParse(((TextBox)sender).Text, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedType1.unkInt1 = val;
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
            }
        }

        private void type1Array_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(((TextBox)sender).Text))
            {
                String[] strs = ((TextBox)sender).Text.Trim(' ').Split(' ');
                Byte[] byteArray = new Byte[strs.Length];
                Int32 i = 0;
                foreach (String str in strs)
                {
                    Byte val = 0;
                    if (Byte.TryParse(str, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out val))
                    {
                        ((TextBox)sender).BackColor = Color.White;
                        byteArray[i] = val;
                    }
                    else
                    {
                        ((TextBox)sender).BackColor = Color.Red;
                        return;
                    }
                    ++i;
                }
                selectedType1.byteArray = byteArray;
            }
            else
            {
                selectedType1.byteArray = new byte[0];
            }
            if (selectedType1.isValidArraySize())
            {
                type1Warning.Visible = false;
            }
            else
            {
                type1Warning.Visible = true;

            }
        }
        private void UpdateType2Panel()
        {
            type2Bitfield.Text = Convert.ToString(selectedType2.bitfield, 16);
            type2Slot.Text = selectedType2.linkedScriptListIndex.ToString();
            type2TransitionEnabled.Checked = (selectedType2.bitfield & 0x400) != 0;
        }
        private void type2Bitfield_TextChanged(object sender, EventArgs e)
        {
            Int32 val = selectedType2.bitfield;
            if (Int32.TryParse(((TextBox)sender).Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedType2.bitfield = val;
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
                return;
            }
            if (selectedType2.isBitFieldValid())
            {
                type2BitfieldWarning.Visible = false;
            }
            else
            {
                type2BitfieldWarning.Visible = true;
            }
        }

        private void type2Slot_TextChanged(object sender, EventArgs e)
        {
            Int32 val = selectedType2.linkedScriptListIndex;
            if (Int32.TryParse(((TextBox)sender).Text, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedType2.linkedScriptListIndex = val;
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
                return;
            }
        }

        private void type2CreateType3_Click(object sender, EventArgs e)
        {
            if (selectedType2.CreateType3())
            {
                TreeNode node = scriptTree.SelectedNode;
                node.Nodes.Clear();
                if (selectedType2.type3 != null)
                {
                    AddType3(node, selectedType2.type3);
                }
                Script.MainScriptStruct.SupportType4 ptr = selectedType2.type4;
                while (ptr != null)
                {
                    AddType4(node, ptr);
                    ptr = ptr.nextType4;
                }
                UpdateType2Panel();
            }
        }

        private void type2DeleteType3_Click(object sender, EventArgs e)
        {
            if (selectedType2.DeleteType3())
            {
                TreeNode node = scriptTree.SelectedNode;
                node.Nodes.Clear();
                if (selectedType2.type3 != null)
                {
                    AddType3(node, selectedType2.type3);
                }
                Script.MainScriptStruct.SupportType4 ptr = selectedType2.type4;
                while (ptr != null)
                {
                    AddType4(node, ptr);
                    ptr = ptr.nextType4;
                }
                UpdateType2Panel();
            }
        }
        private void type2TransitionEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                selectedType2.bitfield |= 0x400;
            }
            else
            {
                selectedType2.bitfield &= ~0x400;
            }
            type2Bitfield.Text = Convert.ToString(selectedType2.bitfield, 16);
        }

        private void type2SelectedType4Pos_TextChanged(object sender, EventArgs e)
        {

        }

        private void type2AddType4_Click(object sender, EventArgs e)
        {
            Int32 val = 0;
            if (Int32.TryParse(type2SelectedType4Pos.Text, out val))
            {
                if (selectedType2.AddType4(val))
                {
                    TreeNode node = scriptTree.SelectedNode;
                    node.Nodes.Clear();
                    if (selectedType2.type3 != null)
                    {
                        AddType3(node, selectedType2.type3);
                    }
                    Script.MainScriptStruct.SupportType4 ptr = selectedType2.type4;
                    while (ptr != null)
                    {
                        AddType4(node, ptr);
                        ptr = ptr.nextType4;
                    }
                    UpdateType2Panel();
                }
            }
        }

        private void type2DeleteType4_Click(object sender, EventArgs e)
        {
            Int32 val = 0;
            if (Int32.TryParse(type2SelectedType4Pos.Text, out val))
            {
                if (selectedType2.DeleteType4(val))
                {
                    TreeNode node = scriptTree.SelectedNode;
                    node.Nodes.Clear();
                    if (selectedType2.type3 != null)
                    {
                        AddType3(node, selectedType2.type3);
                    }
                    Script.MainScriptStruct.SupportType4 ptr = selectedType2.type4;
                    while (ptr != null)
                    {
                        AddType4(node, ptr);
                        ptr = ptr.nextType4;
                    }
                    UpdateType2Panel();
                }
            }
        }
        private void UpdateType3Panel()
        {
            type3VTable.Text = selectedType3.VTableIndex.ToString();
            type3UnkShort.Text = selectedType3.UnkShort.ToString();
            type3X.Text = selectedType3.X.ToString(CultureInfo.InvariantCulture);
            type3Y.Text = selectedType3.Y.ToString(CultureInfo.InvariantCulture);
            type3Z.Text = selectedType3.Z.ToString(CultureInfo.InvariantCulture);
        }
        private void type3VTable_TextChanged(object sender, EventArgs e)
        {
            UInt16 val = selectedType3.VTableIndex;
            if (UInt16.TryParse(((TextBox)sender).Text, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedType3.VTableIndex = val;
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
                return;
            }
        }

        private void type3UnkShort_TextChanged(object sender, EventArgs e)
        {
            UInt16 val = selectedType3.UnkShort;
            if (UInt16.TryParse(((TextBox)sender).Text, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedType3.UnkShort = val;
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
                return;
            }
        }

        private void type3X_TextChanged(object sender, EventArgs e)
        {
            Single val = selectedType3.X;
            if (Single.TryParse(((TextBox)sender).Text, NumberStyles.Float, CultureInfo.InvariantCulture, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedType3.X = val;
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
                return;
            }
        }

        private void type3Y_TextChanged(object sender, EventArgs e)
        {
            Single val = selectedType3.Y;
            if (Single.TryParse(((TextBox)sender).Text, NumberStyles.Float, CultureInfo.InvariantCulture, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedType3.Y = val;
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
                return;
            }
        }

        private void type3Z_TextChanged(object sender, EventArgs e)
        {
            Single val = selectedType3.Z;
            if (Single.TryParse(((TextBox)sender).Text, NumberStyles.Float, CultureInfo.InvariantCulture, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedType3.Z = val;
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
                return;
            }
        }
        private void UpdateType4Panel()
        {
            type4VTableIndex.Text = selectedType4.VTableIndex.ToString();
            type4BitField.Text = selectedType4.UnkShort.ToString("X4");
            type4Arguments.BeginUpdate();
            type4Arguments.Items.Clear();
            foreach (UInt32 arg in selectedType4.arguments)
            {
                type4Arguments.Items.Add(arg.ToString("X8"));
            }
            type4Arguments.EndUpdate();
            if (selectedType4.arguments.Count > 0)
            {
                ignoreUpdate = 0;
                type4Arguments.SelectedIndex = 0;
            }
            
        }
        private void type4VTableIndex_TextChanged(object sender, EventArgs e)
        {
            UInt16 val = selectedType4.VTableIndex;
            if (UInt16.TryParse(((TextBox)sender).Text, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedType4.VTableIndex = val;
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
                return;
            }
            type4ExpectedLength.Text = $"Arguments: {selectedType4.GetExpectedSize() / 4}";
            if (selectedType4.isValidBits())
            {
                type4Warning.Visible = false;
            }
            else
            {
                type4Warning.Visible = true;

            }
        }

        private void type4BitField_TextChanged(object sender, EventArgs e)
        {
            UInt16 val = selectedType4.UnkShort;
            if (UInt16.TryParse(((TextBox)sender).Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedType4.UnkShort = val;
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
                return;
            }
            if (selectedType4.isValidBits())
            {
                type4Warning.Visible = false;
            }
            else
            {
                type4Warning.Visible = true;

            }
        }

        private void type4Array_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(((TextBox)sender).Text))
            {
                String[] strs = ((TextBox)sender).Text.Trim(' ').Split(' ');
                Byte[] byteArray = new Byte[strs.Length];
                Int32 i = 0;
                foreach (String str in strs)
                {
                    Byte val = 0;
                    if (Byte.TryParse(str, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out val))
                    {
                        ((TextBox)sender).BackColor = Color.White;
                        byteArray[i] = val;
                    }
                    else
                    {
                        ((TextBox)sender).BackColor = Color.Red;
                        return;
                    }
                    ++i;
                }
                //selectedType4.byteArray = byteArray;
            }
            else
            {
                //selectedType4.byteArray = new byte[0];
            }

            if (selectedType4.isValidBits())
            {
                type4Warning.Visible = false;
            }
            else
            {
                type4Warning.Visible = true;

            }
        }
        private void type4Arguments_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ignoreUpdate == 0)
            {
                ListBox listBox = (ListBox)sender;
                if (listBox.SelectedItem != null)
                {
                    UInt32 val = 0;
                    if (UInt32.TryParse((String)listBox.SelectedItem, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out val))
                    {
                        UpdateArgRepresentations(val);
                    }
                }
            }
        }
        private void UpdateArgRepresentations(UInt32 val)
        {
            if (ignoreUpdate != 0 && type4Arguments.SelectedIndex >= 0)
            {
                int index = type4Arguments.SelectedIndex;
                type4Arguments.Items[index] = val.ToString("X8");
                type4Arguments.SelectedIndex = index;
            }
            if (ignoreUpdate != 1) type4ArgHEX.Text = val.ToString("X8");
            if (ignoreUpdate != 2) type4ArgInt32.Text = val.ToString();
            if (ignoreUpdate != 3) type4ArgFloat.Text = (BitConverter.ToSingle(BitConverter.GetBytes(val), 0)).ToString();
            if (ignoreUpdate != 4) type4ArgInt16_1.Text = (val & 0xFFFF).ToString();
            if (ignoreUpdate != 5) type4ArgInt16_2.Text = ((val & 0xFFFF0000) >> 16).ToString();
            if (ignoreUpdate != 6) type4ArgByte1.Text = ((val & 0xFF) >> 0).ToString();
            if (ignoreUpdate != 7) type4ArgByte2.Text = ((val & 0xFF00) >> 8).ToString();
            if (ignoreUpdate != 8) type4ArgByte3.Text = ((val & 0xFF0000) >> 16).ToString();
            if (ignoreUpdate != 9) type4ArgByte4.Text = ((val & 0xFF000000) >> 24).ToString();
        }
        private bool stopChanged = false;
        private int ignoreUpdate = 0;
        private void type4ArgHEX_TextChanged(object sender, EventArgs e)
        {
            if (type4Arguments.SelectedIndex >= 0 && !stopChanged)
            {
                String text = ((TextBox)sender).Text;
                UInt32 val = 0;
                if (UInt32.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out val))
                {
                    selectedType4.arguments[type4Arguments.SelectedIndex] = val;
                    ((TextBox)sender).BackColor = Color.White;
                    stopChanged = true;
                    ignoreUpdate = 1;
                    UpdateArgRepresentations(selectedType4.arguments[type4Arguments.SelectedIndex]);
                    ignoreUpdate = 0;
                    stopChanged = false;
                } else
                {
                    ((TextBox)sender).BackColor = Color.Red;
                }
            }
        }

        private void type4ArgInt32_TextChanged(object sender, EventArgs e)
        {
            if (type4Arguments.SelectedIndex >= 0 && !stopChanged)
            {
                String text = ((TextBox)sender).Text;
                UInt32 val = 0;
                if (UInt32.TryParse(text, out val))
                {
                    selectedType4.arguments[type4Arguments.SelectedIndex] = val;
                    ((TextBox)sender).BackColor = Color.White;
                    stopChanged = true;
                    ignoreUpdate = 2;
                    UpdateArgRepresentations(selectedType4.arguments[type4Arguments.SelectedIndex]);
                    ignoreUpdate = 0;
                    stopChanged = false;
                }
                else
                {
                    ((TextBox)sender).BackColor = Color.Red;
                }
            }
        }

        private void type4ArgFloat_TextChanged(object sender, EventArgs e)
        {
            if (type4Arguments.SelectedIndex >= 0 && !stopChanged)
            {
                String text = ((TextBox)sender).Text;
                Single val = 0;
                if (Single.TryParse(text, out val))
                {
                    selectedType4.arguments[type4Arguments.SelectedIndex] = BitConverter.ToUInt32(BitConverter.GetBytes(val), 0);
                    ((TextBox)sender).BackColor = Color.White;
                    stopChanged = true;
                    ignoreUpdate = 3;
                    UpdateArgRepresentations(selectedType4.arguments[type4Arguments.SelectedIndex]);
                    ignoreUpdate = 0;
                    stopChanged = false;
                }
                else
                {
                    ((TextBox)sender).BackColor = Color.Red;
                }
            }
        }

        private void type4ArgInt16_1_TextChanged(object sender, EventArgs e)
        {
            if (type4Arguments.SelectedIndex >= 0 && !stopChanged)
            {
                String text = ((TextBox)sender).Text;
                UInt16 val = 0;
                if (UInt16.TryParse(text, out val))
                {
                    selectedType4.arguments[type4Arguments.SelectedIndex] =  (selectedType4.arguments[type4Arguments.SelectedIndex] & 0xFFFF0000) | (UInt32)(val & 0xFFFF);
                    ((TextBox)sender).BackColor = Color.White;
                    stopChanged = true;
                    ignoreUpdate = 4;
                    UpdateArgRepresentations(selectedType4.arguments[type4Arguments.SelectedIndex]);
                    ignoreUpdate = 0;
                    stopChanged = false;
                }
                else
                {
                    ((TextBox)sender).BackColor = Color.Red;
                }
            }
        }

        private void type4ArgInt16_2_TextChanged(object sender, EventArgs e)
        {
            if (type4Arguments.SelectedIndex >= 0 && !stopChanged)
            {
                String text = ((TextBox)sender).Text;
                UInt16 val = 0;
                if (UInt16.TryParse(text, out val))
                {
                    selectedType4.arguments[type4Arguments.SelectedIndex] = (UInt32)(val << 16) | (selectedType4.arguments[type4Arguments.SelectedIndex] & 0xFFFF);
                    ((TextBox)sender).BackColor = Color.White;
                    stopChanged = true;
                    ignoreUpdate = 5;
                    UpdateArgRepresentations(selectedType4.arguments[type4Arguments.SelectedIndex]);
                    ignoreUpdate = 0;
                    stopChanged = false;
                }
                else
                {
                    ((TextBox)sender).BackColor = Color.Red;
                }
            }
        }

        private void type4ArgByte1_TextChanged(object sender, EventArgs e)
        {
            if (type4Arguments.SelectedIndex >= 0 && !stopChanged)
            {
                String text = ((TextBox)sender).Text;
                Byte val = 0;
                if (Byte.TryParse(text, out val))
                {
                    selectedType4.arguments[type4Arguments.SelectedIndex] = (selectedType4.arguments[type4Arguments.SelectedIndex] & 0xFFFFFF00) | (UInt32)((val & 0xFF) << 0);
                    ((TextBox)sender).BackColor = Color.White;
                    stopChanged = true;
                    ignoreUpdate = 6;
                    UpdateArgRepresentations(selectedType4.arguments[type4Arguments.SelectedIndex]);
                    ignoreUpdate = 0;
                    stopChanged = false;
                }
                else
                {
                    ((TextBox)sender).BackColor = Color.Red;
                }
            }
        }

        private void type4ArgByte2_TextChanged(object sender, EventArgs e)
        {
            if (type4Arguments.SelectedIndex >= 0 && !stopChanged)
            {
                String text = ((TextBox)sender).Text;
                Byte val = 0;
                if (Byte.TryParse(text, out val))
                {
                    selectedType4.arguments[type4Arguments.SelectedIndex] = (selectedType4.arguments[type4Arguments.SelectedIndex] & 0xFFFF00FF) | (UInt32)((val & 0xFF) << 8);
                    ((TextBox)sender).BackColor = Color.White;
                    stopChanged = true;
                    ignoreUpdate = 7;
                    UpdateArgRepresentations(selectedType4.arguments[type4Arguments.SelectedIndex]);
                    ignoreUpdate = 0;
                    stopChanged = false;
                }
                else
                {
                    ((TextBox)sender).BackColor = Color.Red;
                }
            }
        }

        private void type4ArgByte3_TextChanged(object sender, EventArgs e)
        {
            if (type4Arguments.SelectedIndex >= 0 && !stopChanged)
            {
                String text = ((TextBox)sender).Text;
                Byte val = 0;
                if (Byte.TryParse(text, out val))
                {
                    selectedType4.arguments[type4Arguments.SelectedIndex] = (selectedType4.arguments[type4Arguments.SelectedIndex] & 0xFF00FFFF) | (UInt32)((val & 0xFF) << 16);
                    ((TextBox)sender).BackColor = Color.White;
                    stopChanged = true;
                    ignoreUpdate = 8;
                    UpdateArgRepresentations(selectedType4.arguments[type4Arguments.SelectedIndex]);
                    ignoreUpdate = 0;
                    stopChanged = false;
                }
                else
                {
                    ((TextBox)sender).BackColor = Color.Red;
                }
            }
        }

        private void type4ArgByte4_TextChanged(object sender, EventArgs e)
        {
            if (type4Arguments.SelectedIndex >= 0 && !stopChanged)
            {
                String text = ((TextBox)sender).Text;
                Byte val = 0;
                if (Byte.TryParse(text, out val))
                {
                    selectedType4.arguments[type4Arguments.SelectedIndex] = (selectedType4.arguments[type4Arguments.SelectedIndex] & 0x00FFFFFF) | (UInt32)((val & 0xFF) << 24);
                    ((TextBox)sender).BackColor = Color.White;
                    stopChanged = true;
                    ignoreUpdate = 9;
                    UpdateArgRepresentations(selectedType4.arguments[type4Arguments.SelectedIndex]);
                    ignoreUpdate = 0;
                    stopChanged = false;
                }
                else
                {
                    ((TextBox)sender).BackColor = Color.Red;
                }
            }
        }
        private void UpdateLinkedPanel()
        {
            linkedBitField.Text = selectedLinked.bitfield.ToString("X4");
            linkedSlotIndex.Text = selectedLinked.scriptIndexOrSlot.ToString();
        }
        private void linkedBitField_TextChanged(object sender, EventArgs e)
        {
            Int16 val = selectedLinked.bitfield;
            if (Int16.TryParse(((TextBox)sender).Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedLinked.bitfield = val;
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
                return;
            }
            if (selectedLinked.isValidBits())
            {
                linkedWarning.Visible = false;
            }
            else
            {
                linkedWarning.Visible = true;
            }
        }

        private void linkedSlotIndex_TextChanged(object sender, EventArgs e)
        {
            Int16 val = selectedLinked.scriptIndexOrSlot;
            if (Int16.TryParse(((TextBox)sender).Text, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedLinked.scriptIndexOrSlot = val;
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
                return;
            }
        }

        private void linkedCreateType1_Click(object sender, EventArgs e)
        {
            if (selectedLinked.CreateType1())
            {
                TreeNode node = scriptTree.SelectedNode;
                node.Nodes.Clear();
                if (selectedLinked.type1 != null)
                {
                    AddType1(node, selectedLinked.type1);
                }
                Script.MainScriptStruct.SupportType2 ptr = selectedLinked.type2;
                while (ptr != null)
                {
                    AddType2(node, ptr);
                    ptr = ptr.nextType2;
                }
                UpdateLinkedPanel();
            }
        }

        private void linkedDeleteType1_Click(object sender, EventArgs e)
        {
            if (selectedLinked.DeleteType1())
            {
                TreeNode node = scriptTree.SelectedNode;
                node.Nodes.Clear();
                if (selectedLinked.type1 != null)
                {
                    AddType1(node, selectedLinked.type1);
                }
                Script.MainScriptStruct.SupportType2 ptr = selectedLinked.type2;
                while (ptr != null)
                {
                    AddType2(node, ptr);
                    ptr = ptr.nextType2;
                }
                UpdateLinkedPanel();
            }
        }

        private void linkedCreateType2_Click(object sender, EventArgs e)
        {
            Int32 val = 0;
            if (Int32.TryParse(linkedType2Pos.Text, out val))
            {
                if (selectedLinked.AddType2(val))
                {
                    TreeNode node = scriptTree.SelectedNode;
                    node.Nodes.Clear();
                    if (selectedLinked.type1 != null)
                    {
                        AddType1(node, selectedLinked.type1);
                    }
                    Script.MainScriptStruct.SupportType2 ptr = selectedLinked.type2;
                    while (ptr != null)
                    {
                        AddType2(node, ptr);
                        ptr = ptr.nextType2;
                    }
                    UpdateLinkedPanel();
                }
            }
        }

        private void linkedDeleteType2_Click(object sender, EventArgs e)
        {
            Int32 val = 0;
            if (Int32.TryParse(linkedType2Pos.Text, out val))
            {
                if (selectedLinked.DeleteType2(val))
                {
                    TreeNode node = scriptTree.SelectedNode;
                    node.Nodes.Clear();
                    if (selectedLinked.type1 != null)
                    {
                        AddType1(node, selectedLinked.type1);
                    }
                    Script.MainScriptStruct.SupportType2 ptr = selectedLinked.type2;
                    while (ptr != null)
                    {
                        AddType2(node, ptr);
                        ptr = ptr.nextType2;
                    }
                    UpdateLinkedPanel();
                }
            }
        }
        private void UpdateGeneralPanel()
        {
            generalId.Text = script.ID.ToString();
            generalArray.Text = GetTextFromArray(script.script);
            if (script.script.Length == 0)
            {
                generalWarning.Visible = false;
            }
            else
            {
                generalWarning.Visible = true;
            }
        }
        private void generalArray_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(((TextBox)sender).Text))
            {
                String[] strs = ((TextBox)sender).Text.Trim(' ').Split(' ');
                Byte[] byteArray = new Byte[strs.Length];
                Int32 i = 0;
                foreach (String str in strs)
                {
                    Byte val = 0;
                    if (Byte.TryParse(str, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out val))
                    {
                        ((TextBox)sender).BackColor = Color.White;
                        byteArray[i] = val;
                    }
                    else
                    {
                        ((TextBox)sender).BackColor = Color.Red;
                        return;
                    }
                    ++i;
                }
                script.script = byteArray;
            }
            else
            {
                script.script = new Byte[0];
            }
            if (script.script.Length == 0)
            {
                generalWarning.Visible = false;
            }
            else
            {
                generalWarning.Visible = true;
            }
        }
        private void generalId_TextChanged_1(object sender, EventArgs e)
        {
            UInt32 val = 0;
            if (UInt32.TryParse(((TextBox)sender).Text, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                script.ID = val;
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
                return;
            }
        }
        private void scriptListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (null != scriptListBox.SelectedItem)
            {
                File.SelectItem((Script)controller.Data.Records[scriptIndices[scriptListBox.SelectedIndex]]);
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

        private void panelLinked_Paint(object sender, PaintEventArgs e)
        {

        }

        private void scriptNameFilter_TextChanged(object sender, EventArgs e)
        {
            PopulateList(scriptPredicate);
        }

        private void filterSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (filterSelection.SelectedIndex)
            {
                case 0:
                    scriptPredicate = s =>
                    {
                        return s.Name.Contains(scriptNameFilter.Text);
                    };
                    break;
                case 1:
                    scriptPredicate = s =>
                    {
                        return scriptNameFilter.TextLength == 0 ||
                                (scriptNameFilter.Text.All(c => { return char.IsDigit(c); }) &&
                                s.ID == int.Parse(scriptNameFilter.Text));
                    };
                    break;
            }
        }

        private void deleteScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sel_i = scriptListBox.SelectedIndex;
            if (sel_i == -1)
                return;
            controller.RemoveItem(script.ID);
            scriptListBox.BeginUpdate();
            scriptListBox.Items.RemoveAt(sel_i);
            if (sel_i >= scriptListBox.Items.Count) sel_i = scriptListBox.Items.Count - 1;
            scriptListBox.SelectedIndex = sel_i;
            scriptListBox.EndUpdate();
            controller.UpdateTextBox();
        }

        private void createScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ushort maxid = (ushort)controller.Data.RecordIDs.Select(p => p.Key).Max();
            ushort id1 = Math.Max((ushort)(8191), maxid);
            ++id1;
            id1 += (ushort)(id1 % 2);
            ushort id2 = id1;
            ++id2;
            Script newScriptHeader = new Script();
            newScriptHeader.HeaderScript = new Script.HeaderScriptStruct((int)id2);
            newScriptHeader.ID = id1;
            newScriptHeader.Name = "Header Script";
            newScriptHeader.flag = 1;
            controller.Data.AddItem(id1, newScriptHeader);
            scriptIndices.Add(id1);
            ((MainForm)Tag).GenTreeNode(newScriptHeader, controller);

            script = newScriptHeader;
            scriptListBox.Items.Add(GenTextForList(newScriptHeader));
            controller.UpdateText();
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[newScriptHeader.ID]].Tag).UpdateText();
            

            Script newScriptMain = new Script();
            newScriptMain.MainScript = new Script.MainScriptStruct();
            newScriptMain.ID = id2;
            newScriptMain.Name = "New Script";
            controller.Data.AddItem(id2, newScriptMain);
            scriptIndices.Add(id2);
            ((MainForm)Tag).GenTreeNode(newScriptMain, controller);

            scriptListBox.Items.Add(GenTextForList(newScriptMain));
            
            controller.UpdateText();
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[newScriptMain.ID]].Tag).UpdateText();
            PopulateList();
            scriptListBox.SelectedIndex = scriptListBox.Items.Count - 1;
        }
    }
}
