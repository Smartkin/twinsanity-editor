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
using Twinsanity.Actions;
using System.ComponentModel.Design;

namespace TwinsaityEditor
{
    public partial class ScriptEditor : Form
    {
        private SectionController controller;
        private Script script;

        private Script.HeaderScript selectedHeaderScript;
        private Script.MainScript selectedMainScript;
        private Script.MainScript.SupportType1 selectedType1;
        private Script.MainScript.ScriptStateBody selectedType2;
        private Script.MainScript.ScriptCondition selectedType3;
        private Script.MainScript.ScriptCommand selectedType4;
        private Script.MainScript.ScriptState selectedLinked;
        private ScriptAction selectedAction;
        private FileController File { get; set; }
        private TwinsFile FileData { get => File.Data; }
        private Func<Script, bool> scriptPredicate;
        private List<int> scriptIndices = new List<int>();
        private bool ignoreChange = false;
        private int scriptGameVersion = 0;
        public ScriptEditor(SectionController c)
        {
            File = c.MainFile;
            controller = c;
            if (controller.MainFile.Data.Type == TwinsFile.FileType.RMX) scriptGameVersion = 1;
            if (controller.MainFile.Data.Type == TwinsFile.FileType.DemoRM2) scriptGameVersion = 2;
            Text = $"Instance Editor (Section {c.Data.Parent.ID})";
            scriptPredicate = s => { return s.Name.Contains(scriptNameFilter.Text) && s.Main != null; };
            ScriptAction.GetSupported();
            InitializeComponent();
            PopulateList(scriptPredicate);
            UpdatePanels();
            PopulateCommandList();
            PopulatePerceptList();
            cbCommandIndex.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_perceptID.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_propGrid_ActionID.DropDownStyle = ComboBoxStyle.DropDownList;
            filterSelection.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_StartUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_packet_space.Items.Clear();
            for (int i = 0; i < (int)Script.MainScript.SupportType1.SpaceType.STORED_SPACE + 1; i++)
            {
                comboBox_packet_space.Items.Add((Script.MainScript.SupportType1.SpaceType)i);
            }
            comboBox_packet_motion.Items.Clear();
            for (int i = 0; i < (int)Script.MainScript.SupportType1.MotionType.AIR_CHASE + 1; i++)
            {
                comboBox_packet_motion.Items.Add((Script.MainScript.SupportType1.MotionType)i);
            }
            comboBox_packet_rotation.Items.Clear();
            for (int i = 0; i < (int)Script.MainScript.SupportType1.ContinuousRotate.NATURAL_ROLL + 1; i++)
            {
                comboBox_packet_rotation.Items.Add((Script.MainScript.SupportType1.ContinuousRotate)i);
            }
            comboBox_packet_axes.Items.Clear();
            for (int i = 0; i < (int)Script.MainScript.SupportType1.NaturalAxes.ALL_NATURAL + 1; i++)
            {
                comboBox_packet_axes.Items.Add((Script.MainScript.SupportType1.NaturalAxes)i);
            }
            comboBox_packet_accel.Items.Clear();
            for (int i = 0; i < (int)Script.MainScript.SupportType1.AccelFunction.SMOOTH_CURVE + 1; i++)
            {
                comboBox_packet_accel.Items.Add((Script.MainScript.SupportType1.AccelFunction)i);
            }
            comboBox_participant_locality.Items.Clear();
            for (int i = 0; i < (int)Script.HeaderScript.AssignLocalityID.ANYWHERE + 1; i++)
            {
                comboBox_participant_locality.Items.Add((Script.HeaderScript.AssignLocalityID)i);
            }
            comboBox_participant_status.Items.Clear();
            for (int i = 0; i < (int)Script.HeaderScript.AssignStatusID.ANYSTATE + 1; i++)
            {
                comboBox_participant_status.Items.Add((Script.HeaderScript.AssignStatusID)i);
            }
            comboBox_participant_preference.Items.Clear();
            for (int i = 0; i < (int)Script.HeaderScript.AssignPreferenceID.ANYHOW + 1; i++)
            {
                comboBox_participant_preference.Items.Add((Script.HeaderScript.AssignPreferenceID)i);
            }
            comboBox_participant_type.Items.Clear();
            for (int i = 0; i < (int)Script.HeaderScript.AssignTypeID.ORIGINATOR + 1; i++)
            {
                comboBox_participant_type.Items.Add((Script.HeaderScript.AssignTypeID)i);
            }
        }

        private void ScriptEditor_Load(object sender, EventArgs e)
        {
            filterSelection.SelectedIndex = 0;
        }
        private void PopulateCommandList()
        {
            // Populate with current script command knowledge
            for (ushort i = 0; i < 663; ++i) //Script.MainScript.ScriptCommand.ScriptCommandTableSize
            {
                string Label = string.Empty;
                if (Script.MainScript.ScriptCommand.GetCommandSize(i, scriptGameVersion) == 0x00)
                {
                    if (Enum.IsDefined(typeof(DefaultEnums.CommandID), i))
                    {
                        Label = $"-DELETED {i} ({((DefaultEnums.CommandID)i).ToString()})";
                    }
                    else
                    {
                        Label = $"-DELETED {i}";
                    }
                }
                else if (Script.MainScript.ScriptCommand.GetCommandSize(i, scriptGameVersion) == 0x0C)
                {
                    if (Enum.IsDefined(typeof(DefaultEnums.CommandID), i))
                    {
                        Label = $"{i:000}: {((DefaultEnums.CommandID)i).ToString()} (No args)";
                    }
                    else
                    {
                        Label = $"{i:000}: UNKNOWN (No args)";
                    }
                }
                else if (Enum.IsDefined(typeof(DefaultEnums.CommandID), i))
                {
                    Label = $"{i:000}: {((DefaultEnums.CommandID)i).ToString()}";
                }
                else
                {
                    Label = $"{i:000}: UNKNOWN";
                }
                cbCommandIndex.Items.Add(Label);
                comboBox_propGrid_ActionID.Items.Add(Label);
            }
        }
        private void PopulatePerceptList()
        {
            for (ushort i = 0; i < 645; ++i)
            {
                if (Enum.IsDefined(typeof(DefaultEnums.ConditionID), i))
                {
                    comboBox_perceptID.Items.Add($"{i:000}: {((DefaultEnums.ConditionID)i).ToString()}");
                }
                else if (i >= 177 && i <= 511)
                {
                    comboBox_perceptID.Items.Add($"----- {i}");
                }
                else
                {
                    comboBox_perceptID.Items.Add($"{i:000}: UNKNOWN");
                }
            }
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
            return $"{script.ID:0000}{(script.Name == string.Empty ? string.Empty : $": {script.Name}")}";
            /*
            if (script.script != null && script.script.Length > 0) // warn if there are leftovers
            {
                return $"(!)ID {script.ID} {(script.Name == string.Empty ? string.Empty : $" - {script.Name}")}";
            }
            else
            {
                
            }
            */
        }
        private void BuildTree()
        {
            scriptTree.BeginUpdate();
            scriptTree.Nodes.Clear();
            if (null != script)
            {
                scriptTree.Nodes.Add($"Priority {script.mask} - {script.Name}").Tag = script;
                if (script.Header != null)
                {
                    scriptTree.TopNode.Nodes.Add($"Participants: {script.Header.pairs.Count}").Tag = script.Header;
                }
                if (script.Main != null)
                {
                    TreeNode mainScriptNode = scriptTree.TopNode.Nodes.Add("Layer Script - Unit " + script.Main.StartUnit);
                    mainScriptNode.Tag = script.Main;
                    Script.MainScript mainScript = script.Main;
                    selectedMainScript = mainScript;
                    Script.MainScript.ScriptState ptr = mainScript.scriptState1;
                    while (ptr != null)
                    {
                        AddLinked(mainScriptNode, ptr);
                        ptr = ptr.nextState;
                    }
                }
                scriptTree.Nodes[0].ExpandAll();
                scriptTree.Nodes[0].EnsureVisible();
                scriptTree.SelectedNode = scriptTree.Nodes[0];
                scriptTree.EndUpdate();
            }
        }
        private void AddLinked(TreeNode parent, Script.MainScript.ScriptState ptr)
        {
            string Name = $"Unit {parent.Nodes.Count}";
            if (ptr.type1 != null)
            {
                Name += $" + Control Packet";
            }
            if (ptr.scriptIndexOrSlot != -1)
            {
                if (ptr.IsSlot)
                {
                    Name += $" - Script Slot #{ptr.scriptIndexOrSlot}";
                }
                else
                {
                    if (Enum.IsDefined(typeof(DefaultEnums.ScriptID), (ushort)ptr.scriptIndexOrSlot))
                    {
                        Name += $" - ID: {ptr.scriptIndexOrSlot} {(DefaultEnums.ScriptID)(ushort)ptr.scriptIndexOrSlot}";
                    }
                    else
                    {
                        Name += $" - Unit {ptr.scriptIndexOrSlot}";
                    }
                }
            }
            TreeNode node = parent.Nodes.Add(Name);
            node.Tag = ptr;
            if (null != ptr.type1)
            {
                //AddType1(node, ptr.type1);
            }
            Script.MainScript.ScriptStateBody ptrType2 = ptr.scriptStateBody;
            while (ptrType2 != null)
            {
                AddType2(node, ptrType2);
                ptrType2 = ptrType2.nextScriptStateBody;
            }
        }
        private void AddType1(TreeNode parent, Script.MainScript.SupportType1 ptr)
        {
            TreeNode node = parent.Nodes.Add($"Header");
            node.Tag = ptr;
        }
        private void AddType2(TreeNode parent, Script.MainScript.ScriptStateBody ptr)
        {
            TreeNode node = parent.Nodes.Add($"To Unit {ptr.scriptStateListIndex}");
            node.Tag = ptr;
            if (null != ptr.condition)
            {
                AddType3(node, ptr.condition);
            }
            if (!ptr.IsEnabled)
            {
                node.Text = string.Format("{1} {0}", node.Text, "(OFF)");
            }
            Script.MainScript.ScriptCommand ptrType4 = ptr.command;
            while (ptrType4 != null)
            {
                AddType4(node, ptrType4);
                ptrType4 = ptrType4.nextCommand;
            }
        }
        private void AddType3(TreeNode parent, Script.MainScript.ScriptCondition ptr)
        {
            string Name = $"Percept {ptr.VTableIndex}";
            //bool IsDefined = false;
            if (Enum.IsDefined(typeof(DefaultEnums.ConditionID), ptr.VTableIndex))
            {
                Name = ((DefaultEnums.ConditionID)ptr.VTableIndex).ToString();
                //IsDefined = true;
                //if (ScriptPercept.Args.ContainsKey((DefaultEnums.ConditionID)ptr.VTableIndex))
                //{
                //    Name += " " + ptr.UnkData;
                //}
            }
            Name += $" {ptr.Parameter}/{ptr.Interval}/{ptr.Threshold}";
            if (ptr.NotGate)
            {
                Name = "NOT " + Name;
            }

            parent.Text = string.Format("{1} - {0}", parent.Text, Name);
            /*
            TreeNode node = parent.Nodes.Add(Name);
            node.Tag = ptr;
            if (!IsDefined)
            {
                node.ForeColor = Color.FromKnownColor(KnownColor.ControlDarkDark);
            }
            */
            
        }
        private void AddType4(TreeNode parent, Script.MainScript.ScriptCommand ptr)
        {
            string Name = $"Action {ptr.VTableIndex}";
            bool IsDefined = false;
            bool IsNamed = false;
            if (Enum.IsDefined(typeof(DefaultEnums.CommandID), ptr.VTableIndex))
            {
                Name = ((DefaultEnums.CommandID)ptr.VTableIndex).ToString();
                IsDefined = true;
                DefaultEnums.CommandID ActID = (DefaultEnums.CommandID)ptr.VTableIndex;
                if (ScriptAction.SupportedTypes.ContainsKey(ActID))
                {
                    ScriptAction CacheAction = (ScriptAction)Activator.CreateInstance(ScriptAction.SupportedTypes[ActID]);
                    CacheAction.Load(ptr);
                    Name = CacheAction.ToString();
                    IsNamed = true;
                }
            }
            if (!IsNamed)
            {
                if (ptr.arguments.Count == 1)
                {
                    Name += $" 0x{ptr.arguments[0].ToString("X8")}";
                }
                else if (ptr.arguments.Count == 2)
                {
                    Name += $" 0x{ptr.arguments[0].ToString("X8")}, 0x{ptr.arguments[1].ToString("X8")}";
                }
                else if (ptr.arguments.Count > 2)
                {
                    Name += $" ... ({ptr.arguments.Count} args)";
                }
            }

            TreeNode node = parent.Nodes.Add(Name);
            node.Tag = ptr;
            if (!ptr.isValidBits())
            {
                node.ForeColor = Color.FromKnownColor(KnownColor.Red);
            }
            else if (!IsDefined)
            {
                node.ForeColor = Color.FromKnownColor(KnownColor.ControlDarkDark);
            }
        }
        private void UpdatePanels()
        {
            panelHeader.Visible = false;
            panelMain.Visible = false;
            panelType1.Visible = false;
            panel_LinkEditor.Visible = false;
            panelType3.Visible = false;
            panelType4.Visible = false;
            panelLinked.Visible = false;
            panelGeneral.Visible = false;
            panel_propGrid.Visible = false;
            if (null != scriptTree.SelectedNode)
            {
                Object tag = scriptTree.SelectedNode.Tag;
                if (tag == null || tag is Script)
                {
                    panelGeneral.Visible = true;
                    UpdateGeneralPanel();
                }
                if (tag is Script.HeaderScript)
                {
                    panelHeader.Visible = true;
                    selectedHeaderScript = (Script.HeaderScript)tag;
                    UpdateHeaderPanel();
                }
                if (tag is Script.MainScript)
                {
                    panelMain.Visible = true;
                    selectedMainScript = (Script.MainScript)tag;
                    UpdateMainPanel();
                }
                if (tag is Script.MainScript.SupportType1)
                {
                    panelType1.Visible = true;
                    selectedType1 = (Script.MainScript.SupportType1)tag;
                    UpdateType1Panel();
                }
                if (tag is Script.MainScript.ScriptStateBody)
                {
                    panel_LinkEditor.Visible = true;
                    selectedType2 = (Script.MainScript.ScriptStateBody)tag;
                    UpdateType2Panel();

                    ignoreChange = true;
                    if (selectedType2.condition != null)
                    {
                        //panelType2.Visible = false;
                        checkBox_type2_cond_toggle.Checked = true;
                        panelType3.Visible = true;
                        selectedType3 = selectedType2.condition;
                        UpdateType3Panel();
                    }
                    else
                    {
                        checkBox_type2_cond_toggle.Checked = false;
                        panelType3.Visible = false;
                    }
                    ignoreChange = false;
                    
                }
                if (tag is Script.MainScript.ScriptCondition)
                {
                    panelType3.Visible = true;
                    selectedType3 = (Script.MainScript.ScriptCondition)tag;
                    UpdateType3Panel();
                }
                if (tag is Script.MainScript.ScriptCommand)
                {
                    selectedType4 = (Script.MainScript.ScriptCommand)tag;
                    selectedAction = null;

                    if (ScriptAction.ArglessTypes.Contains((int)selectedType4.VTableIndex))
                    {
                        propertyGrid1.Enabled = false;
                        propertyGrid1.Visible = false;
                        panel_propGrid.Visible = true;
                        UpdatePropPanel();
                    }
                    else if (Enum.IsDefined(typeof(DefaultEnums.CommandID), selectedType4.VTableIndex))
                    {
                        DefaultEnums.CommandID ActID = (DefaultEnums.CommandID)selectedType4.VTableIndex;
                        if (ScriptAction.SupportedTypes.ContainsKey(ActID) && (Control.ModifierKeys & Keys.Shift) == 0)
                        {
                            panel_propGrid.Visible = true;

                            selectedAction = (ScriptAction)Activator.CreateInstance(ScriptAction.SupportedTypes[ActID]);
                            selectedAction.Load(selectedType4);
                            propertyGrid1.SelectedObject = selectedAction;
                            propertyGrid1.Enabled = true;
                            propertyGrid1.Visible = true;
                            UpdatePropPanel();
                        }
                        else
                        {
                            panelType4.Visible = true;
                            UpdateType4Panel();
                        }
                    }
                    else
                    {
                        panelType4.Visible = true;
                        UpdateType4Panel();
                    }
                }
                if (tag is Script.MainScript.ScriptState)
                {
                    panelLinked.Visible = true;
                    selectedLinked = (Script.MainScript.ScriptState)tag;
                    UpdateLinkedPanel();

                    ignoreChange = true;
                    if (selectedLinked.type1 != null)
                    {
                        checkBox_state_header_toggle.Checked = true;
                        panelType1.Visible = true;
                        selectedType1 = selectedLinked.type1;
                        UpdateType1Panel();
                    }
                    else
                    {
                        checkBox_state_header_toggle.Checked = false;
                        panelType1.Visible = false;
                    }
                    ignoreChange = false;
                }
                UpdateNodeName();
            }
        }

        private void UpdateHeaderPanel()
        {
            /*
            headerSubScripts.Items.Clear();
            foreach (Script.HeaderScript.UnkIntPairs pair in selectedHeaderScript.pairs)
            {
                headerSubScripts.Items.Add(pair);
            }
            if (headerSubScripts.Items.Count > 0)
            {
                headerSubScripts.SelectedIndex = 0;
            }
            */
            listBox_header_participants.Items.Clear();
            foreach (Script.HeaderScript.UnkIntPairs pair in selectedHeaderScript.pairs)
            {
                listBox_header_participants.Items.Add(pair);
            }
            if (listBox_header_participants.Items.Count > 0)
            {
                listBox_header_participants.SelectedIndex = 0;
            }
        }
        private void headerSubscriptID_TextChanged(object sender, EventArgs e)
        {
            if (listBox_header_participants.SelectedItem != null)
            {
                Script.HeaderScript.UnkIntPairs pair = selectedHeaderScript.pairs[listBox_header_participants.SelectedIndex];
                TextBox textBox = (TextBox)sender;
                Int32 val = pair.mainScriptIndex;
                if (Int32.TryParse(textBox.Text, out val))
                {
                    textBox.BackColor = Color.White;
                    pair.mainScriptIndex = val + 1;
                    listBox_header_participants.Items[listBox_header_participants.SelectedIndex] = listBox_header_participants.Items[listBox_header_participants.SelectedIndex];
                }
                else
                {
                    textBox.BackColor = Color.Red;
                }

            }
        }

        private void headerSubscriptArg_TextChanged(object sender, EventArgs e)
        {
            if (listBox_header_participants.SelectedItem != null)
            {
                Script.HeaderScript.UnkIntPairs pair = selectedHeaderScript.pairs[listBox_header_participants.SelectedIndex];
                TextBox textBox = (TextBox)sender;
                UInt16 val = pair.ObjectID;
                if (UInt16.TryParse(textBox.Text, out val))
                {
                    textBox.BackColor = Color.White;
                    //pair.unkInt2 = val;
                    pair.ObjectID = val;
                    listBox_header_participants.Items[listBox_header_participants.SelectedIndex] = listBox_header_participants.Items[listBox_header_participants.SelectedIndex];
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
            mainLinkedCnt.Visible = false;
            label6.Visible = false;
            //mainLinkedCnt.Text = selectedMainScript.StatesAmount.ToString();
            //mainUnk.Text = selectedMainScript.unkInt2.ToString();
            //mainLinkedPos.Text = "0";
            listBox_UnitList.Items.Clear();
            comboBox_StartUnit.Items.Clear();
            if (selectedMainScript.scriptState1 != null)
            {
                int i = 0;
                Script.MainScript.ScriptState com = selectedMainScript.scriptState1;
                while (com != null)
                {
                    string Name = $"Unit {i}"; // add link count?
                    i++;
                    listBox_UnitList.Items.Add(Name);
                    comboBox_StartUnit.Items.Add(Name);
                    com = com.nextState;
                }
            }
            if (selectedMainScript.StartUnit < comboBox_StartUnit.Items.Count)
            {
                comboBox_StartUnit.SelectedIndex = selectedMainScript.StartUnit;
            }
            else
            {
                comboBox_StartUnit.SelectedIndex = -1;
            }
            UpdateNodeName();
        }
        private void mainName_TextChanged(object sender, EventArgs e)
        {
            selectedMainScript.name = ((TextBox)sender).Text;
            scriptTree.TopNode.Text = selectedMainScript.name;
            //scriptListBox.Items[scriptListBox.SelectedIndex] = GenTextForList(script); Fuck this shit for being unstable piss that destroys my will to live
        }

        private void mainUnk_TextChanged(object sender, EventArgs e)
        {
            Int32 val = selectedMainScript.StartUnit;
            if (Int32.TryParse(((TextBox)sender).Text, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedMainScript.StartUnit = val;
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
            }
            UpdateNodeName();
        }

        private void mainLinkedPos_TextChanged(object sender, EventArgs e)
        {

        }

        private void mainAddLinked_Click(object sender, EventArgs e)
        {
            //Int32 val = 0;
            //if (Int32.TryParse(mainLinkedPos.Text, out val))
            //{
            //    
            //}
            int val = listBox_UnitList.SelectedIndex + 1;
            if (val == -1) val = selectedMainScript.GetStatesAmount();

            if (selectedMainScript.AddLinkedScript(val))
            {
                TreeNode mainNode = scriptTree.SelectedNode;
                mainNode.Nodes.Clear();
                Script.MainScript.ScriptState ptr = selectedMainScript.scriptState1;
                while (ptr != null)
                {
                    AddLinked(mainNode, ptr);
                    ptr = ptr.nextState;
                }
                UpdateMainPanel();
                scriptTree.Nodes[0].ExpandAll();
                scriptTree.Nodes[0].EnsureVisible();
            }
        }

        private void mainDelLinked_Click(object sender, EventArgs e)
        {
            //Int32 val = 0;
            //if (Int32.TryParse(mainLinkedPos.Text, out val))
            //{
            //   
            //}
            int val = listBox_UnitList.SelectedIndex;
            if (val == -1) return;

            if (selectedMainScript.DeleteLinkedScript(val))
            {
                TreeNode mainNode = scriptTree.SelectedNode;
                mainNode.Nodes.Clear();
                Script.MainScript.ScriptState ptr = selectedMainScript.scriptState1;
                while (ptr != null)
                {
                    AddLinked(mainNode, ptr);
                    ptr = ptr.nextState;
                }
                UpdateMainPanel();
                scriptTree.Nodes[0].ExpandAll();
                scriptTree.Nodes[0].EnsureVisible();
            }
        }
        bool blockType1IndexChanged = false;
        private void UpdateType1Panel()
        {
            type1UnkByte1.Text = selectedType1.unkByte1.ToString();
            type1UnkByte2.Text = selectedType1.unkByte2.ToString();

            comboBox_packet_accel.SelectedIndex = (int)selectedType1.AccelFunc;
            comboBox_packet_axes.SelectedIndex = (int)selectedType1.Axes;
            comboBox_packet_motion.SelectedIndex = (int)selectedType1.Motion;
            comboBox_packet_rotation.SelectedIndex = (int)selectedType1.ContRotate;
            comboBox_packet_space.SelectedIndex = (int)selectedType1.Space;

            checkBox_packet_translates.Checked = selectedType1.Translates;
            checkBox_packet_rotates.Checked = selectedType1.Rotates;
            checkBox_packet_usesphysics.Checked = selectedType1.UsesPhysics;
            checkBox_packet_usesrotator.Checked = selectedType1.UsesRotator;
            checkBox_packet_usesinterpolator.Checked = selectedType1.UsesInterpolator;
            checkBox_packet_interpolatesangles.Checked = selectedType1.InterpolatesAngles;
            checkBox_packet_translationcont.Checked = selectedType1.TranslationContinues;
            checkBox_packet_yawfaces.Checked = selectedType1.YawFaces;
            checkBox_packet_pitchfaces.Checked = selectedType1.PitchFaces;
            checkBox_packet_orients.Checked = selectedType1.OrientsPredicts;
            checkBox_packet_tracksdest.Checked = selectedType1.TracksDestination;
            checkBox_packet_keyislocal.Checked = selectedType1.KeyIsLocal;
            checkBox_packet_controtates.Checked = selectedType1.ContRotatesInWorldSpace;
            checkBox_packet_stalls.Checked = selectedType1.Stalls;
            checkBox_packet_hasValidData.Checked = selectedType1.HasValidData;

            blockType1IndexChanged = true;
            UpdateType1Bytes();
            UpdateType1Floats();
            blockType1IndexChanged = false;
            UpdateNodeName();
        }

        List<string> ByteOrderVariables = new List<string>()
        {
            "int SELECTOR/SYNC_INDEX",
            "int KEY_INDEX/FOCUS_DATA",
            "f   MOVE_SPEED/RISE_HEIGHT",
            "ang TURN_SPEED",
            "f   RAWPOS_X",
            "f   RAWPOS_Y",
            "f   RAWPOS_Z",
            "ang RAWANGS_X/PITCH",
            "ang RAWANGS_Y/YAW",
            "ang RAWANGS_Z/ROLL",
            "f   DELAY",
            "f   DURATION/CURVY/HOMEPOWER",
            "f   TUMBLE_DATA",
            "f   SPIN_DATA",
            "f   TWIST_DATA",
            "f   SQR_TOLERANCE/RANDRANGE",
            "f   POWER/GRAVITY/BANKING",
            "f   DAMPING/SPEEDLIM/BRAKING",
            "f   AC_DIST/RT_OPT/SHIFT FREQ",
            "f   DEC_DIST/PHYS_OPT/SHIFT",
            "f   BOUNCE/BANK_LIMIT",
            "ptr SYNC_UNIT",
            "int JOINT_INDEX",
        };

        private void UpdateType1Bytes()
        {
            type1Bytes.BeginUpdate();
            type1Bytes.Items.Clear();
            int i = 0;
            foreach (Byte b in selectedType1.bytes)
            {
                string byteID = string.Empty; //$"{i:000}";

                if (i < ByteOrderVariables.Count)
                {
                    byteID = ByteOrderVariables[i]; //+= $"{ByteOrderVariables[i]}";
                }

                if (b == 255)
                {
                    type1Bytes.Items.Add($"{byteID}: N/A");
                }
                else if (b > 127)
                {
                    type1Bytes.Items.Add($"{byteID}: Inst. Float #{b - 128}");
                }
                else
                {
                    type1Bytes.Items.Add($"{byteID}: {b}");
                }
                
                ++i;
            }
            type1Bytes.EndUpdate();
            comboBox_controlPacketByteType.SelectedIndex = 0;
        }
        private void UpdateType1Floats()
        {
            type1Floats.BeginUpdate();
            type1Floats.Items.Clear();
            int i = 0;
            foreach (Single f in selectedType1.floats)
            {
                type1Floats.Items.Add($"{i:000}: {f}");
                ++i;
            }
            type1Floats.EndUpdate();
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
                blockType1IndexChanged = true;
                UpdateType1Bytes();
                blockType1IndexChanged = false;
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
            }
        }

        private void type1UnkByte2_TextChanged(object sender, EventArgs e)
        {
            Byte val = selectedType1.unkByte2;
            if (Byte.TryParse(((TextBox)sender).Text, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedType1.unkByte2 = val;
                blockType1IndexChanged = true;
                UpdateType1Floats();
                blockType1IndexChanged = false;
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
            }
        }

        private void type1UnkInt_TextChanged(object sender, EventArgs e)
        {
            return;
            /*
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
            */
        }

        private void type1Array_TextChanged(object sender, EventArgs e)
        {

        }

        private void type1Bytes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!blockType1IndexChanged)
            {
                ListBox list = (ListBox)sender;
                if (list.SelectedItem != null)
                {
                    ignoreChange = true;
                    type1Byte.Text = selectedType1.bytes[list.SelectedIndex].ToString();
                    if (selectedType1.bytes[list.SelectedIndex] == 255)
                    {
                        type1Byte.Enabled = false;
                        comboBox_controlPacketByteType.SelectedIndex = 0;
                    }
                    else if (selectedType1.bytes[list.SelectedIndex] > 127)
                    {
                        type1Byte.Enabled = true;
                        comboBox_controlPacketByteType.SelectedIndex = 2;
                        type1Byte.Text = (selectedType1.bytes[list.SelectedIndex] - 128).ToString();
                    }
                    else
                    {
                        type1Byte.Enabled = true;
                        comboBox_controlPacketByteType.SelectedIndex = 1;
                    }
                    ignoreChange = false;
                }
            }
        }

        private void type1Floats_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!blockType1IndexChanged)
            {
                ListBox list = (ListBox)sender;
                if (list.SelectedItem != null)
                {
                    type1Float.Text = selectedType1.floats[list.SelectedIndex].ToString();
                }
            }
        }
        private void UpdateType2Panel()
        {
            type2Bitfield.Text = Convert.ToString(selectedType2.bitfield, 16);
            //type2Slot.Text = selectedType2.scriptStateListIndex.ToString();
            type2TransitionEnabled.Checked = (selectedType2.bitfield & 0x400) != 0;
            listBox_LinkActions.Items.Clear();
            if (selectedType2.commandCount > 0)
            {
                int i = 0;
                Script.MainScript.ScriptCommand com = selectedType2.command;
                while (i < selectedType2.commandCount && com != null)
                {
                    string Name = $"Action {com.VTableIndex}";
                    bool IsNamed = false;
                    if (Enum.IsDefined(typeof(DefaultEnums.CommandID), com.VTableIndex))
                    {
                        Name = ((DefaultEnums.CommandID)com.VTableIndex).ToString();
                        DefaultEnums.CommandID ActID = (DefaultEnums.CommandID)com.VTableIndex;
                        if (ScriptAction.SupportedTypes.ContainsKey(ActID))
                        {
                            ScriptAction CacheAction = (ScriptAction)Activator.CreateInstance(ScriptAction.SupportedTypes[ActID]);
                            CacheAction.Load(com);
                            Name = CacheAction.ToString();
                            IsNamed = true;
                        }
                    }
                    if (!IsNamed)
                    {
                        if (com.arguments.Count == 1)
                        {
                            Name += $" 0x{com.arguments[0].ToString("X8")}";
                        }
                        else if (com.arguments.Count == 2)
                        {
                            Name += $" 0x{com.arguments[0].ToString("X8")}, 0x{com.arguments[1].ToString("X8")}";
                        }
                        else if (com.arguments.Count > 2)
                        {
                            Name += $" ... ({com.arguments.Count} args)";
                        }
                    }
                    i++;
                    listBox_LinkActions.Items.Add(Name);
                    com = com.nextCommand;
                }
            }
            comboBox_LinkedUnit.Items.Clear();
            Script.MainScript.ScriptState state = selectedMainScript.scriptState1;
            int iter = 0;
            while (state != null)
            {
                string Name = $"Unit {iter}";
                comboBox_LinkedUnit.Items.Add(Name);
                iter++;
                state = state.nextState;
            }
            if (selectedType2.scriptStateListIndex < comboBox_LinkedUnit.Items.Count)
            {
                comboBox_LinkedUnit.SelectedIndex = selectedType2.scriptStateListIndex;
            }
            else
            {
                comboBox_LinkedUnit.SelectedIndex = -1;
            }
            UpdateNodeName();
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
            Int32 val = selectedType2.scriptStateListIndex;
            if (Int32.TryParse(((TextBox)sender).Text, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedType2.scriptStateListIndex = val;
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
                return;
            }
            UpdateNodeName();
        }

        private void type2CreateType3_Click(object sender, EventArgs e)
        {
            if (selectedType2.CreateCondition())
            {
                TreeNode node = scriptTree.SelectedNode;
                node.Nodes.Clear();
                if (selectedType2.condition != null)
                {
                    AddType3(node, selectedType2.condition);
                }
                Script.MainScript.ScriptCommand ptr = selectedType2.command;
                while (ptr != null)
                {
                    AddType4(node, ptr);
                    ptr = ptr.nextCommand;
                }
                UpdateType2Panel();
            }
        }

        private void type2DeleteType3_Click(object sender, EventArgs e)
        {
            if (selectedType2.DeleteCondition())
            {
                TreeNode node = scriptTree.SelectedNode;
                node.Nodes.Clear();
                if (selectedType2.condition != null)
                {
                    AddType3(node, selectedType2.condition);
                }
                Script.MainScript.ScriptCommand ptr = selectedType2.command;
                while (ptr != null)
                {
                    AddType4(node, ptr);
                    ptr = ptr.nextCommand;
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
            UpdateNodeName();
        }

        private void type2SelectedType4Pos_TextChanged(object sender, EventArgs e)
        {

        }

        private void type2AddType4_Click(object sender, EventArgs e)
        {
            //Int32 val = 0;
            //if (Int32.TryParse(type2SelectedType4Pos.Text, out val))
            //{
            //    
            //}
            int val = listBox_LinkActions.SelectedIndex + 1;
            if (val == -1) val = selectedType2.commandCount;

            if (selectedType2.AddCommand(val))
            {
                TreeNode node = scriptTree.SelectedNode;
                node.Nodes.Clear();
                if (selectedType2.condition != null)
                {
                    AddType3(node, selectedType2.condition);
                }
                Script.MainScript.ScriptCommand ptr = selectedType2.command;
                while (ptr != null)
                {
                    AddType4(node, ptr);
                    ptr = ptr.nextCommand;
                }
                UpdateType2Panel();
            }
        }

        private void type2DeleteType4_Click(object sender, EventArgs e)
        {
            //Int32 val = 0;
            //if (Int32.TryParse(type2SelectedType4Pos.Text, out val))
            //{
            //    
            //}
            int val = listBox_LinkActions.SelectedIndex;
            if (val == -1) return;

            if (selectedType2.DeleteCommand(val))
            {
                TreeNode node = scriptTree.SelectedNode;
                node.Nodes.Clear();
                if (selectedType2.condition != null)
                {
                    AddType3(node, selectedType2.condition);
                }
                Script.MainScript.ScriptCommand ptr = selectedType2.command;
                while (ptr != null)
                {
                    AddType4(node, ptr);
                    ptr = ptr.nextCommand;
                }
                UpdateType2Panel();
            }
        }
        private void UpdateType3Panel()
        {
            type3VTable.Text = selectedType3.VTableIndex.ToString();
            type3Parameter.Text = selectedType3.Parameter.ToString();
            type3CbNotGate.Checked = selectedType3.NotGate;
            type3Interval.Text = selectedType3.Interval.ToString(CultureInfo.InvariantCulture);
            type3Threshold.Text = selectedType3.Threshold.ToString(CultureInfo.InvariantCulture);
            type3ThresholdInverse.Text = selectedType3.ThresholdInverse.ToString(CultureInfo.InvariantCulture);
            comboBox_perceptID.SelectedIndex = selectedType3.VTableIndex;
            //type3ThresholdInverse.Enabled = false;
            UpdateNodeName();
        }
        private void type3VTable_TextChanged(object sender, EventArgs e)
        {
            UInt16 val = selectedType3.VTableIndex;
            if (UInt16.TryParse(((TextBox)sender).Text, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedType3.VTableIndex = val;
                UpdateNodeName();
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
                return;
            }
        }

        private void type3UnkShort_TextChanged(object sender, EventArgs e)
        {
            UInt16 val = selectedType3.Parameter;
            if (UInt16.TryParse(((TextBox)sender).Text, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedType3.Parameter = val;
                UpdateNodeName();
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
                return;
            }
        }

        private void type3X_TextChanged(object sender, EventArgs e)
        {
            Single val = selectedType3.Interval;
            if (Single.TryParse(((TextBox)sender).Text, NumberStyles.Float, CultureInfo.InvariantCulture, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedType3.Interval = val;
                UpdateNodeName();
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
                return;
            }
        }

        private void type3Y_TextChanged(object sender, EventArgs e)
        {
            Single val = selectedType3.Threshold;
            if (Single.TryParse(((TextBox)sender).Text, NumberStyles.Float, CultureInfo.InvariantCulture, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedType3.Threshold = val;
                if (val == 0f)
                {
                    // see: COM_CORTEX_GAME_SPAWNER_ACTIVATED
                    // editor would otherwise interpret it as infinity, but the game wants NaN, probably because the PS2 interprets NaN as zero?
                    selectedType3.ThresholdInverse = Single.NaN;
                }
                else
                {
                    selectedType3.ThresholdInverse = 1f / val;
                }
                ignoreChange = true;
                type3ThresholdInverse.Text = selectedType3.ThresholdInverse.ToString(CultureInfo.InvariantCulture);
                ignoreChange = false;
                UpdateNodeName();
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
                return;
            }
        }

        private void type3Z_TextChanged(object sender, EventArgs e)
        {
            if (ignoreChange) return;
            Single val = selectedType3.ThresholdInverse;
            if (Single.TryParse(((TextBox)sender).Text, NumberStyles.Float, CultureInfo.InvariantCulture, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                selectedType3.ThresholdInverse = val;
                UpdateNodeName();
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
                return;
            }
        }
        private void UpdateType4Panel()
        {
            ignoreChange = true;
            cbCommandIndex.SelectedItem = cbCommandIndex.Items[selectedType4.VTableIndex];
            ignoreChange = false;
            
            type4BitField.Text = selectedType4.UnkShort.ToString("X4");
            type4Arguments.BeginUpdate();
            type4Arguments.Items.Clear();
            int i = 0;
            foreach (UInt32 arg in selectedType4.arguments)
            {
                type4Arguments.Items.Add($"{i:000}: {arg:X8}");
                ++i;
            }
            type4Arguments.EndUpdate();
            if (selectedType4.arguments.Count > 0)
            {
                ignoreUpdate = 0;
                type4Arguments.SelectedIndex = 0;
            }
            UpdateNodeName();
        }
        private void UpdatePropPanel()
        {
            ignoreChange = true;
            comboBox_propGrid_ActionID.SelectedIndex = selectedType4.VTableIndex;
            ignoreChange = false;
            UpdateNodeName();
        }
        private void type1Byte_TextChanged(object sender, EventArgs e)
        {
            if (type1Bytes.SelectedIndex >= 0 && !stopChanged && !ignoreChange)
            {
                String text = ((TextBox)sender).Text;
                Byte val = 0;
                if (Byte.TryParse(text, out val) && val < 128)
                {
                    if (comboBox_controlPacketByteType.SelectedIndex == 1)
                    {
                        selectedType1.bytes[type1Bytes.SelectedIndex] = val;
                    }
                    else
                    {
                        selectedType1.bytes[type1Bytes.SelectedIndex] = (byte)(val + 128);
                    }
                    ((TextBox)sender).BackColor = Color.White;
                    int index = type1Bytes.SelectedIndex;
                    blockType1IndexChanged = true;
                    string byteName = $"{index:000}";
                    if (index < ByteOrderVariables.Count)
                    {
                        byteName = ByteOrderVariables[index];
                    }

                    if (selectedType1.bytes[type1Bytes.SelectedIndex] == 255)
                    {
                        type1Bytes.Items[index] = $"{byteName}: N/A";
                    }
                    else if (selectedType1.bytes[type1Bytes.SelectedIndex] > 127)
                    {
                        type1Bytes.Items[index] = $"{byteName}: Inst. Float #{selectedType1.bytes[index] - 128}";
                    }
                    else
                    {
                        type1Bytes.Items[index] = $"{byteName}: {selectedType1.bytes[index]}";
                    }
                    
                    type1Bytes.SelectedIndex = index;
                    blockType1IndexChanged = false;
                }
                else
                {
                    ((TextBox)sender).BackColor = Color.Red;
                }
            }
        }

        private void type1Float_TextChanged(object sender, EventArgs e)
        {
            if (type1Floats.SelectedIndex >= 0 && !stopChanged)
            {
                String text = ((TextBox)sender).Text;
                Single val = 0;
                if (Single.TryParse(text, out val))
                {
                    selectedType1.floats[type1Floats.SelectedIndex] = val;
                    ((TextBox)sender).BackColor = Color.White;
                    int index = type1Floats.SelectedIndex;
                    blockType1IndexChanged = true;
                    type1Floats.Items[index] = $"{index:000}: {selectedType1.floats[index]}";
                    type1Floats.SelectedIndex = index;
                    blockType1IndexChanged = false;
                }
                else
                {
                    ((TextBox)sender).BackColor = Color.Red;
                }
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
            UpdateNodeName();
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
                    UpdateArgRepresentations(selectedType4.arguments[listBox.SelectedIndex]);
                }
            }
        }
        private void UpdateArgRepresentations(UInt32 val)
        {
            if (ignoreUpdate != 0 && type4Arguments.SelectedIndex >= 0)
            {
                int index = type4Arguments.SelectedIndex;
                type4Arguments.Items[index] = $"{index:000}: {val:X8}";
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
            if (ignoreUpdate != 10) type4ArgSignedInt32.Text = ((Int32)val).ToString();
            if (ignoreUpdate != 11) type4ArgSignedInt16_1.Text = ((Int16)(val & 0xFFFF)).ToString();
            if (ignoreUpdate != 12) type4ArgSignedInt16_2.Text = ((Int16)((val & 0xFFFF0000) >> 16)).ToString();
            if (ignoreUpdate != 13) type4ArgBinary.Text = Convert.ToString(val,2).PadLeft(32,'0');
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
        private void type4ArgSignedInt32_TextChanged(object sender, EventArgs e)
        {
            if (type4Arguments.SelectedIndex >= 0 && !stopChanged)
            {
                String text = ((TextBox)sender).Text;
                Int32 val = 0;
                if (Int32.TryParse(text, out val))
                {
                    selectedType4.arguments[type4Arguments.SelectedIndex] = (UInt32)val;
                    ((TextBox)sender).BackColor = Color.White;
                    stopChanged = true;
                    ignoreUpdate = 2;
                    UpdateArgRepresentations(selectedType4.arguments[type4Arguments.SelectedIndex]);
                    ignoreUpdate = 10;
                    stopChanged = false;
                }
                else
                {
                    ((TextBox)sender).BackColor = Color.Red;
                }
            }
        }

        private void type4ArgSignedInt16_1_TextChanged(object sender, EventArgs e)
        {
            if (type4Arguments.SelectedIndex >= 0 && !stopChanged)
            {
                String text = ((TextBox)sender).Text;
                Int16 val = 0;
                if (Int16.TryParse(text, out val))
                {
                    selectedType4.arguments[type4Arguments.SelectedIndex] = (selectedType4.arguments[type4Arguments.SelectedIndex] & 0xFFFF0000) | (UInt32)((UInt16)val & 0xFFFF);
                    ((TextBox)sender).BackColor = Color.White;
                    stopChanged = true;
                    ignoreUpdate = 11;
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

        private void type4ArgSignedInt16_2_TextChanged(object sender, EventArgs e)
        {
            if (type4Arguments.SelectedIndex >= 0 && !stopChanged)
            {
                String text = ((TextBox)sender).Text;
                Int16 val = 0;
                if (Int16.TryParse(text, out val))
                {
                    selectedType4.arguments[type4Arguments.SelectedIndex] = (UInt32)((UInt16)val << 16) | (selectedType4.arguments[type4Arguments.SelectedIndex] & 0xFFFF);
                    ((TextBox)sender).BackColor = Color.White;
                    stopChanged = true;
                    ignoreUpdate = 12;
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
        private void type4ArgBinary_TextChanged(object sender, EventArgs e)
        {
            if (type4Arguments.SelectedIndex >= 0 && !stopChanged)
            {
                try
                {
                    String text = ((TextBox)sender).Text;
                    UInt32 val = Convert.ToUInt32(text, 2);
                    selectedType4.arguments[type4Arguments.SelectedIndex] = val;
                    ((TextBox)sender).BackColor = Color.White;
                    stopChanged = true;
                    ignoreUpdate = 13;
                    UpdateArgRepresentations(selectedType4.arguments[type4Arguments.SelectedIndex]);
                    ignoreUpdate = 0;
                    stopChanged = false;
                } 
                catch
                {
                    ((TextBox)sender).BackColor = Color.Red;
                }
            }
        }
        private void UpdateLinkedPanel()
        {
            linkedBitField.Text = selectedLinked.bitfield.ToString("X4");
            linkedSlotIndex.Text = selectedLinked.scriptIndexOrSlot.ToString();
            checkBox_localScriptSlot.Checked = selectedLinked.IsSlot;
            listBox_LinkList.Items.Clear();
            int i = 0;
            Script.MainScript.ScriptStateBody com = selectedLinked.scriptStateBody;
            while (com != null)
            {
                string Name = $"To Unit {com.scriptStateListIndex}";

                if (null != com.condition)
                {
                    if (Enum.IsDefined(typeof(DefaultEnums.ConditionID), com.condition.VTableIndex))
                    {
                        if (com.condition.NotGate)
                        {
                            Name = string.Format("NOT {1} {2}/{3}/{4} - {0}", Name, ((DefaultEnums.ConditionID)com.condition.VTableIndex).ToString(), com.condition.Parameter, com.condition.Interval, com.condition.Threshold);
                        }
                        else
                        {
                            Name = string.Format("{1} {2}/{3}/{4} - {0}", Name, ((DefaultEnums.ConditionID)com.condition.VTableIndex).ToString(), com.condition.Parameter, com.condition.Interval, com.condition.Threshold);
                        }
                    }
                    else
                    {
                        if (com.condition.NotGate)
                        {
                            Name = string.Format("NOT {1} {2}/{3}/{4} - {0}", Name, $"Percept {com.condition.VTableIndex}", com.condition.Parameter, com.condition.Interval, com.condition.Threshold);
                        }
                        else
                        {
                            Name = string.Format("{1} {2}/{3}/{4} - {0}", Name, $"Percept {com.condition.VTableIndex}", com.condition.Parameter, com.condition.Interval, com.condition.Threshold);
                        }
                    }
                }
                if (!com.IsEnabled)
                {
                    Name = string.Format("{1} {0}", Name, "(OFF)");
                }
                i++;
                listBox_LinkList.Items.Add(Name);
                com = com.nextScriptStateBody;
            }
            UpdateNodeName();
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
            UpdateNodeName();
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
                Script.MainScript.ScriptStateBody ptr = selectedLinked.scriptStateBody;
                while (ptr != null)
                {
                    AddType2(node, ptr);
                    ptr = ptr.nextScriptStateBody;
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
                Script.MainScript.ScriptStateBody ptr = selectedLinked.scriptStateBody;
                while (ptr != null)
                {
                    AddType2(node, ptr);
                    ptr = ptr.nextScriptStateBody;
                }
                UpdateLinkedPanel();
            }
        }

        private void linkedCreateType2_Click(object sender, EventArgs e)
        {
            //Int32 val = 0;
            //if (Int32.TryParse(linkedType2Pos.Text, out val))
            //{
            //    
            //}
            int val = listBox_LinkList.SelectedIndex + 1;
            if (val == -1) val = selectedMainScript.GetStatesAmount();

            if (selectedLinked.AddScriptStateBody(val))
            {
                TreeNode node = scriptTree.SelectedNode;
                node.Nodes.Clear();
                if (selectedLinked.type1 != null)
                {
                    AddType1(node, selectedLinked.type1);
                }
                Script.MainScript.ScriptStateBody ptr = selectedLinked.scriptStateBody;
                while (ptr != null)
                {
                    AddType2(node, ptr);
                    ptr = ptr.nextScriptStateBody;
                }
                UpdateLinkedPanel();
                scriptTree.Nodes[0].ExpandAll();
                scriptTree.Nodes[0].EnsureVisible();
            }
        }

        private void linkedDeleteType2_Click(object sender, EventArgs e)
        {
            //Int32 val = 0;
            //if (Int32.TryParse(linkedType2Pos.Text, out val))
            //{
            //    
            //}
            int val = listBox_LinkList.SelectedIndex;
            if (val == -1) return;

            if (selectedLinked.DeleteScriptStateBody(val))
            {
                TreeNode node = scriptTree.SelectedNode;
                node.Nodes.Clear();
                if (selectedLinked.type1 != null)
                {
                    AddType1(node, selectedLinked.type1);
                }
                Script.MainScript.ScriptStateBody ptr = selectedLinked.scriptStateBody;
                while (ptr != null)
                {
                    AddType2(node, ptr);
                    ptr = ptr.nextScriptStateBody;
                }
                UpdateLinkedPanel();
                scriptTree.Nodes[0].ExpandAll();
                scriptTree.Nodes[0].EnsureVisible();
            }
        }
        private void UpdateGeneralPanel()
        {
            generalId.Text = script.ID.ToString();
            generalArray.Text = GetTextFromArray(script.script);
            textBox_scriptMask.Text = script.mask.ToString();
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
                ScriptAction.SetupVersion(script.scriptGameVersion);
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
            if (checkBox_showHeaderScripts.Checked)
            {
                switch (filterSelection.SelectedIndex)
                {
                    case 0:
                        scriptPredicate = s =>
                        {
                            return s.Name.ToUpper().Contains(scriptNameFilter.Text.ToUpper());
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
            else
            {
                switch (filterSelection.SelectedIndex)
                {
                    case 0:
                        scriptPredicate = s =>
                        {
                            return s.Name.ToUpper().Contains(scriptNameFilter.Text.ToUpper()) && s.Main != null;
                        };
                        break;
                    case 1:
                        scriptPredicate = s =>
                        {
                            return scriptNameFilter.TextLength == 0 ||
                                    (scriptNameFilter.Text.All(c => { return char.IsDigit(c); }) &&
                                    s.ID == int.Parse(scriptNameFilter.Text)) &&
                                    s.Main != null;
                        };
                        break;
                }
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
            newScriptHeader.Header = new Script.HeaderScript((int)id2);
            newScriptHeader.ID = id1;
            newScriptHeader.Name = "Behaviour Starter";
            newScriptHeader.flag = 1;
            newScriptHeader.mask = 50;
            controller.Data.AddItem(id1, newScriptHeader);
            scriptIndices.Add(id1);
            //((MainForm)Tag).GenTreeNode(newScriptHeader, controller);

            script = newScriptHeader;
            scriptListBox.Items.Add(GenTextForList(newScriptHeader));
            controller.UpdateText();
            //((Controller)controller.Node.Nodes[controller.Data.RecordIDs[newScriptHeader.ID]].Tag).UpdateText();
            

            Script newScriptMain = new Script();
            newScriptMain.Main = new Script.MainScript();
            newScriptMain.ID = id2;
            newScriptMain.Name = "New Script";
            controller.Data.AddItem(id2, newScriptMain);
            scriptIndices.Add(id2);
            //((MainForm)Tag).GenTreeNode(newScriptMain, controller);

            scriptListBox.Items.Add(GenTextForList(newScriptMain));
            
            controller.UpdateText();
            //((Controller)controller.Node.Nodes[controller.Data.RecordIDs[newScriptMain.ID]].Tag).UpdateText();
            PopulateList();
            scriptListBox.SelectedIndex = scriptListBox.Items.Count - 1;

            controller.RefreshSection();
        }

        private void panelType4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox_showHeaderScripts_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_showHeaderScripts.Checked)
            {
                filterSelection_SelectedIndexChanged(null, null);
            }
            else
            {
                switch (filterSelection.SelectedIndex)
                {
                    case 0:
                        scriptPredicate = s =>
                        {
                            return s.Name.ToUpper().Contains(scriptNameFilter.Text.ToUpper()) && s.Main != null;
                        };
                        break;
                    case 1:
                        scriptPredicate = s =>
                        {
                            return scriptNameFilter.TextLength == 0 ||
                                    (scriptNameFilter.Text.All(c => { return char.IsDigit(c); }) &&
                                    s.ID == int.Parse(scriptNameFilter.Text)) &&
                                    s.Main != null;
                        };
                        break;
                }
            }
            scriptNameFilter_TextChanged(null, null);
        }

        private void checkBox_localScriptSlot_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_localScriptSlot.Checked)
            {
                selectedLinked.IsSlot = true;
            }
            else
            {
                selectedLinked.IsSlot = false;
            }
            if (sender != this && sender != null)
            {
                UpdateLinkedPanel();
            }
        }

        private void checkBox_type2_cond_toggle_CheckedChanged(object sender, EventArgs e)
        {
            if (ignoreChange)
            {
                ignoreChange = false;
                return;
            }
            if (selectedType2.condition != null)
            {
                selectedType2.DeleteCondition();
                selectedType3 = null;
                panelType3.Visible = false;
            }
            else
            {
                selectedType2.CreateCondition();
                selectedType3 = selectedType2.condition;
                UpdateType3Panel();
                panelType3.Visible = true;
            }
            UpdateType2Panel();
        }

        private void checkBox_state_header_toggle_CheckedChanged(object sender, EventArgs e)
        {
            if (ignoreChange)
            {
                ignoreChange = false;
                return;
            }
            if (selectedLinked.type1 != null)
            {
                selectedLinked.DeleteType1();
                selectedType1 = null;
                panelType1.Visible = false;
            }
            else
            {
                selectedLinked.CreateType1();
                selectedType1 = selectedLinked.type1;
                UpdateType1Panel();
                panelType1.Visible = true;
            }
            UpdateLinkedPanel();
        }

        void UpdateNodeName()
        {
            if (null != scriptTree.SelectedNode)
            {
                Object tag = scriptTree.SelectedNode.Tag;
                TreeNode node = scriptTree.SelectedNode;
                if (tag is Script.MainScript)
                {
                    scriptTree.SelectedNode.Text = "Layer Script - Unit " + script.Main.StartUnit;
                }
                if (tag is Script.MainScript.ScriptStateBody)
                {
                    string Name = $"To Unit {selectedType2.scriptStateListIndex}";

                    if (null != selectedType2.condition)
                    {
                        Script.MainScript.ScriptCondition condition = selectedType2.condition;
                        if (Enum.IsDefined(typeof(DefaultEnums.ConditionID), condition.VTableIndex))
                        {
                            if (condition.NotGate)
                            {
                                Name = string.Format("NOT {1} {2}/{3}/{4} - {0}", Name, ((DefaultEnums.ConditionID)condition.VTableIndex).ToString(), condition.Parameter, condition.Interval, condition.Threshold);
                            }
                            else
                            {
                                Name = string.Format("{1} {2}/{3}/{4} - {0}", Name, ((DefaultEnums.ConditionID)condition.VTableIndex).ToString(), condition.Parameter, condition.Interval, condition.Threshold);
                            }
                        }
                        else
                        {
                            if (condition.NotGate)
                            {
                                Name = string.Format("NOT {1} {2}/{3}/{4} - {0}", Name, $"Percept {condition.VTableIndex}", condition.Parameter, condition.Interval, condition.Threshold);
                            }
                            else
                            {
                                Name = string.Format("{1} {2}/{3}/{4} - {0}", Name, $"Percept {condition.VTableIndex}", condition.Parameter, condition.Interval, condition.Threshold);
                            }
                        }
                    }
                    if (!selectedType2.IsEnabled)
                    {
                        Name = string.Format("{1} {0}", Name, "(OFF)");
                    }

                    node.Text = Name;
                }
                if (tag is Script.MainScript.ScriptCommand)
                {
                    string Name = $"Action {selectedType4.VTableIndex}";
                    bool IsDefined = false;
                    if (Enum.IsDefined(typeof(DefaultEnums.CommandID), selectedType4.VTableIndex))
                    {
                        Name = ((DefaultEnums.CommandID)selectedType4.VTableIndex).ToString();
                        IsDefined = true;
                    }
                    if (selectedAction != null)
                    {
                        Name = selectedAction.ToString();
                    }
                    else if (selectedType4.arguments.Count == 1)
                    {
                        Name += $" 0x{selectedType4.arguments[0].ToString("X8")}";
                    }
                    else if (selectedType4.arguments.Count == 2)
                    {
                        Name += $" 0x{selectedType4.arguments[0].ToString("X8")}, 0x{selectedType4.arguments[1].ToString("X8")}";
                    }
                    else if (selectedType4.arguments.Count > 2)
                    {
                        Name += $" ... ({selectedType4.arguments.Count} args)";
                    }

                    if (!selectedType4.isValidBits())
                    {
                        node.ForeColor = Color.FromKnownColor(KnownColor.Red);
                    }
                    else if (!IsDefined)
                    {
                        node.ForeColor = Color.FromKnownColor(KnownColor.ControlDarkDark);
                    }
                    else
                    {
                        node.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                    }
                    node.Text = Name;
                }
                if (tag is Script.MainScript.ScriptState)
                {
                    TreeNode parentNode = node.Parent;
                    int index = 0;
                    for (int n = 0; n < parentNode.Nodes.Count; n++)
                    {
                        if (parentNode.Nodes[n].Text == node.Text)
                        {
                            index = n;
                        }
                    }

                    string Name = $"Unit {index}";
                    if (selectedLinked.type1 != null)
                    {
                        Name += $" + Control Packet";
                    }
                    if (selectedLinked.scriptIndexOrSlot != -1)
                    {
                        if (selectedLinked.IsSlot)
                        {
                            Name += $" - Script Slot #{selectedLinked.scriptIndexOrSlot}";
                        }
                        else
                        {
                            if (Enum.IsDefined(typeof(DefaultEnums.ScriptID), (ushort)selectedLinked.scriptIndexOrSlot))
                            {
                                Name += $" - ID: {selectedLinked.scriptIndexOrSlot} {(DefaultEnums.ScriptID)(ushort)selectedLinked.scriptIndexOrSlot}";
                            }
                            else
                            {
                                Name += $" - Script {selectedLinked.scriptIndexOrSlot}";
                            }
                        }
                    }

                    node.Text = Name;
                }
                if (tag is Script.HeaderScript header)
                {
                    node.Text = $"Participants: {header.pairs.Count}";
                }
                if (tag is Script general)
                {
                    node.Text = $"Priority {general.mask} - {general.Name}";
                }
            }
        }

        private void cbCommandIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ignoreChange) return;
            UInt16 val = selectedType4.VTableIndex;
            if (UInt16.TryParse(((ComboBox)sender).SelectedIndex.ToString(), out val))
            {
                ((ComboBox)sender).BackColor = Color.White;
                selectedType4.VTableIndex = val;
            }
            else
            {
                ((ComboBox)sender).BackColor = Color.Red;
                return;
            }

            selectedType4.arguments = new List<uint>();
            if (ScriptAction.ArglessTypes.Contains((int)selectedType4.VTableIndex))
            {
                panelType4.Visible = false;
                propertyGrid1.Enabled = false;
                propertyGrid1.Visible = false;
                panel_propGrid.Visible = true;
                UpdatePropPanel();
                return;
            }
            else if (Enum.IsDefined(typeof(DefaultEnums.CommandID), selectedType4.VTableIndex))
            {
                DefaultEnums.CommandID ActID = (DefaultEnums.CommandID)selectedType4.VTableIndex;
                if (ScriptAction.SupportedTypes.ContainsKey(ActID))
                {
                    if ((Control.ModifierKeys & Keys.Shift) == 0)
                    {
                        panelType4.Visible = false;
                        panel_propGrid.Visible = true;

                        selectedAction = (ScriptAction)Activator.CreateInstance(ScriptAction.SupportedTypes[ActID]);
                        //selectedAction.Load(selectedType4);
                        selectedAction.Save(selectedType4); // to set the default parameters
                        propertyGrid1.SelectedObject = selectedAction;
                        propertyGrid1.Enabled = true;
                        propertyGrid1.Visible = true;
                        UpdatePropPanel();
                        return;
                    }
                }
                else
                {
                    uint argCount = (uint)(selectedType4.GetExpectedSize() / 4);
                    for (int i = 0; i < argCount; i++)
                    {
                        selectedType4.arguments.Add(0);
                    }
                    UpdateNodeName();
                }
            }
            else
            {
                uint argCount = (uint)(selectedType4.GetExpectedSize() / 4);
                for (int i = 0; i < argCount; i++)
                {
                    selectedType4.arguments.Add(0);
                }
                UpdateNodeName();
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
            UpdateNodeName();
            UpdateType4Panel();
        }

        private void cbNotGate_CheckedChanged(object sender, EventArgs e)
        {
            selectedType3.NotGate = type3CbNotGate.Checked;
            UpdateNodeName();
        }

        private void comboBox_propGrid_ActionID_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedType4.VTableIndex = (ushort)comboBox_propGrid_ActionID.SelectedIndex;
            if (ignoreChange) return;
            selectedType4.arguments = new List<uint>();
            uint argCount = (uint)(selectedType4.GetExpectedSize() / 4);
            for (int i = 0; i < argCount; i++)
            {
                selectedType4.arguments.Add(0);
            }

            panelType4.Visible = false;
            panel_propGrid.Visible = false;
            if (ScriptAction.ArglessTypes.Contains((int)selectedType4.VTableIndex))
            {
                selectedAction = null;
                propertyGrid1.Enabled = false;
                propertyGrid1.Visible = false;
                panel_propGrid.Visible = true;
                UpdatePropPanel();
            }
            else if (Enum.IsDefined(typeof(DefaultEnums.CommandID), selectedType4.VTableIndex))
            {
                DefaultEnums.CommandID ActID = (DefaultEnums.CommandID)selectedType4.VTableIndex;
                if (ScriptAction.SupportedTypes.ContainsKey(ActID))
                {
                    panel_propGrid.Visible = true;

                    selectedAction = (ScriptAction)Activator.CreateInstance(ScriptAction.SupportedTypes[ActID]);
                    //selectedAction.Load(selectedType4);
                    selectedAction.Save(selectedType4); // to set the default parameters
                    propertyGrid1.SelectedObject = selectedAction;
                    propertyGrid1.Enabled = true;
                    propertyGrid1.Visible = true;
                    UpdatePropPanel();
                }
                else
                {
                    selectedAction = null;
                    panelType4.Visible = true;
                    UpdateType4Panel();
                }
            }
            else
            {
                selectedAction = null;
                panelType4.Visible = true;
                UpdateType4Panel();
            }
            UpdateNodeName();
        }

        private void button_propGrid_apply_Click(object sender, EventArgs e)
        {
            selectedAction.Save(selectedType4);
        }

        private void comboBox_perceptID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_perceptID.SelectedIndex < 0) return;

            selectedType3.VTableIndex = (ushort)comboBox_perceptID.SelectedIndex;
            UpdateNodeName();
        }

        private void listBox_LinkActions_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox_StartUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedMainScript.StartUnit = comboBox_StartUnit.SelectedIndex;
            UpdateNodeName();
        }

        private void listBox_LinkList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox_LinkedUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedType2.scriptStateListIndex = comboBox_LinkedUnit.SelectedIndex;
            UpdateNodeName();
        }

        private void comboBox_controlPacketByteType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (type1Bytes.SelectedIndex < 0) return;
            if (ignoreChange) return;
            int index = type1Bytes.SelectedIndex;
            string byteName = $"{index:000}";
            if (index < ByteOrderVariables.Count)
            {
                byteName = ByteOrderVariables[index];
            }
            ignoreChange = true;
            if (comboBox_controlPacketByteType.SelectedIndex == 0)
            {
                selectedType1.bytes[index] = 255;
                type1Byte.Enabled = false;
                type1Byte.Text = "255";
                type1Bytes.Items[index] = $"{byteName}: N/A";
            }
            else if (comboBox_controlPacketByteType.SelectedIndex == 2)
            {
                selectedType1.bytes[index] = 128;
                type1Byte.Enabled = true;
                type1Byte.Text = "128";
                type1Bytes.Items[index] = $"{byteName}: Inst. Float #{selectedType1.bytes[index] - 128}";
            }
            else
            {
                selectedType1.bytes[index] = 0;
                type1Byte.Enabled = true;
                type1Byte.Text = "0";
                type1Bytes.Items[index] = $"{byteName}: {selectedType1.bytes[index]}";
            }
            ignoreChange = false;
        }

        private void textBox_scriptMask_TextChanged(object sender, EventArgs e)
        {
            byte val = 0;
            if (byte.TryParse(((TextBox)sender).Text, out val))
            {
                ((TextBox)sender).BackColor = Color.White;
                script.mask = val;
                UpdateNodeName();
            }
            else
            {
                ((TextBox)sender).BackColor = Color.Red;
                return;
            }
        }

        private void listBox_header_participants_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_header_participants.SelectedItem != null)
            {
                Script.HeaderScript.UnkIntPairs pair = selectedHeaderScript.pairs[listBox_header_participants.SelectedIndex];
                headerSubscriptID.Text = (pair.mainScriptIndex - 1).ToString();
                headerSubscriptArg.Text = pair.ObjectID.ToString();//pair.unkInt2.ToString();  //Convert.ToString(pair.unkInt2, 2).PadLeft(32, '0');
                UpdateNodeName();
                comboBox_participant_locality.SelectedIndex = (int)pair.AssignLocality;
                comboBox_participant_type.SelectedIndex = (int)pair.AssignType;
                comboBox_participant_status.SelectedIndex = (int)pair.AssignStatus;
                comboBox_participant_preference.SelectedIndex = (int)pair.AssignPreference;
            }
        }

        private void button_header_participant_add_Click(object sender, EventArgs e)
        {
            Script.HeaderScript.UnkIntPairs pair = new Script.HeaderScript.UnkIntPairs();
            pair.mainScriptIndex = (int)script.ID + 1;
            pair.unkInt2 = 4294922800;
            selectedHeaderScript.pairs.Add(pair);
            listBox_header_participants.Items.Add(selectedHeaderScript.pairs[selectedHeaderScript.pairs.Count - 1]);
            UpdateNodeName();
        }

        private void button_header_participant_remove_Click(object sender, EventArgs e)
        {
            int index = listBox_header_participants.SelectedIndex;
            if (index < 0) return;
            listBox_header_participants.Items.RemoveAt(index);
            selectedHeaderScript.pairs.RemoveAt(index);
            UpdateNodeName();
        }

        private void comboBox_packet_space_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox s = (ComboBox)sender;
            if (s.SelectedIndex < 0) return;
            selectedType1.Space = (Script.MainScript.SupportType1.SpaceType)s.SelectedIndex;
        }

        private void comboBox_packet_motion_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox s = (ComboBox)sender;
            if (s.SelectedIndex < 0) return;
            selectedType1.Motion = (Script.MainScript.SupportType1.MotionType)s.SelectedIndex;
        }

        private void comboBox_packet_rotation_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox s = (ComboBox)sender;
            if (s.SelectedIndex < 0) return;
            selectedType1.ContRotate = (Script.MainScript.SupportType1.ContinuousRotate)s.SelectedIndex;
        }

        private void comboBox_packet_axes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox s = (ComboBox)sender;
            if (s.SelectedIndex < 0) return;
            selectedType1.Axes = (Script.MainScript.SupportType1.NaturalAxes)s.SelectedIndex;
        }

        private void comboBox_packet_accel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox s = (ComboBox)sender;
            if (s.SelectedIndex < 0) return;
            selectedType1.AccelFunc = (Script.MainScript.SupportType1.AccelFunction)s.SelectedIndex;
        }

        private void checkBox_packet_translates_CheckedChanged(object sender, EventArgs e)
        {
            selectedType1.Translates = checkBox_packet_translates.Checked;
        }

        private void checkBox_packet_rotates_CheckedChanged(object sender, EventArgs e)
        {
            selectedType1.Rotates = checkBox_packet_rotates.Checked;
        }

        private void checkBox_packet_usesphysics_CheckedChanged(object sender, EventArgs e)
        {
            selectedType1.UsesPhysics = checkBox_packet_usesphysics.Checked;
        }

        private void checkBox_packet_usesrotator_CheckedChanged(object sender, EventArgs e)
        {
            selectedType1.UsesRotator = checkBox_packet_usesrotator.Checked;
        }

        private void checkBox_packet_usesinterpolator_CheckedChanged(object sender, EventArgs e)
        {
            selectedType1.UsesInterpolator = checkBox_packet_usesinterpolator.Checked;
        }

        private void checkBox_packet_interpolatesangles_CheckedChanged(object sender, EventArgs e)
        {
            selectedType1.InterpolatesAngles = checkBox_packet_interpolatesangles.Checked;
        }

        private void checkBox_packet_translationcont_CheckedChanged(object sender, EventArgs e)
        {
            selectedType1.TranslationContinues = checkBox_packet_translationcont.Checked;
        }

        private void checkBox_packet_yawfaces_CheckedChanged(object sender, EventArgs e)
        {
            selectedType1.YawFaces = checkBox_packet_yawfaces.Checked;
        }

        private void checkBox_packet_pitchfaces_CheckedChanged(object sender, EventArgs e)
        {
            selectedType1.PitchFaces = checkBox_packet_pitchfaces.Checked;
        }

        private void checkBox_packet_orients_CheckedChanged(object sender, EventArgs e)
        {
            selectedType1.OrientsPredicts = checkBox_packet_orients.Checked;
        }

        private void checkBox_packet_tracksdest_CheckedChanged(object sender, EventArgs e)
        {
            selectedType1.TracksDestination = checkBox_packet_tracksdest.Checked;
        }

        private void checkBox_packet_keyislocal_CheckedChanged(object sender, EventArgs e)
        {
            selectedType1.KeyIsLocal = checkBox_packet_keyislocal.Checked;
        }

        private void checkBox_packet_controtates_CheckedChanged(object sender, EventArgs e)
        {
            selectedType1.ContRotatesInWorldSpace = checkBox_packet_controtates.Checked;
        }

        private void checkBox_packet_stalls_CheckedChanged(object sender, EventArgs e)
        {
            selectedType1.Stalls = checkBox_packet_stalls.Checked;
        }

        private void checkBox_packet_hasValidData_CheckedChanged(object sender, EventArgs e)
        {
            selectedType1.HasValidData = checkBox_packet_hasValidData.Checked;
        }

        private void comboBox_participant_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedHeaderScript == null) return;
            Script.HeaderScript.UnkIntPairs pair = selectedHeaderScript.pairs[listBox_header_participants.SelectedIndex];
            if (pair == null) return;
            pair.AssignType = (Script.HeaderScript.AssignTypeID)comboBox_participant_type.SelectedIndex;
        }

        private void comboBox_participant_locality_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedHeaderScript == null) return;
            Script.HeaderScript.UnkIntPairs pair = selectedHeaderScript.pairs[listBox_header_participants.SelectedIndex];
            if (pair == null) return;
            pair.AssignLocality = (Script.HeaderScript.AssignLocalityID)comboBox_participant_locality.SelectedIndex;
        }

        private void comboBox_participant_status_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedHeaderScript == null) return;
            Script.HeaderScript.UnkIntPairs pair = selectedHeaderScript.pairs[listBox_header_participants.SelectedIndex];
            if (pair == null) return;
            pair.AssignStatus = (Script.HeaderScript.AssignStatusID)comboBox_participant_status.SelectedIndex;
        }

        private void comboBox_participant_preference_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedHeaderScript == null) return;
            Script.HeaderScript.UnkIntPairs pair = selectedHeaderScript.pairs[listBox_header_participants.SelectedIndex];
            if (pair == null) return;
            pair.AssignPreference = (Script.HeaderScript.AssignPreferenceID)comboBox_participant_preference.SelectedIndex;
        }
    }
}
