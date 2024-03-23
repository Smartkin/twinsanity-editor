using System;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public partial class InstanceFlagsEditor : Form
    {
        private Instance inst;
        private GameObject agent;
        private NumericUpDown flagscontrol;


        public Instance Instance
        {
            set { inst = value; UpdateCheckBoxes(); }
        }
        public GameObject Agent
        {
            set { agent = value; UpdateCheckBoxes(); }
        }

        private CheckBox[] checkboxes;
        internal static string[] flagtext = new string[32] {
            "Inactive", // 0
            "Collidable", // 1
            "Visible", // 2
            "Shadow", // 3
            "", // 4
            "", // 5 Tangible?
            "HasLoadZoneState", // 6 Persistent state flag
            "", // 7
            "", // 8 Receive OnTrigger signals?
            "Harmful", // 9 Collision harmful
            "SolidToBodyslam", // 10
            "SolidToSlide", // 11
            "SolidToSpin", // 12
            "SolidToTwinSlam", // 13
            "", // 14 SolidTo something?
            "Targetable", // 15 Targetable with MultiTool and Nina
            "", // 16
            "", // 17
            "ScriptSoftFlag18", // 18
            "ScriptSoftFlag19", // 19
            "ScriptSoftFlag20", // 20
            "ScriptSoftFlag21", // 21
            "ScriptSoftFlag22", // 22
            "ScriptSoftFlag23", // 23
            "ScriptSoftFlag24", // 24
            "ScriptSoftFlag25", // 25
            "ScriptSoftFlag26", // 26
            "ScriptSoftFlag27", // 27
            "ScriptSoftFlag28", // 28
            "ScriptSoftFlag29", // 29
            "ScriptSoftFlag30", // 30
            "ScriptSoftFlag31" // 31
        };

        internal string GetFlagText(int id)
        {
            if (string.IsNullOrEmpty(flagtext[id])) return id.ToString();
            else return $"{id}: {flagtext[id]}";
        }

        public void UpdateCheckBoxes()
        {
            if (inst != null)
            {
                foreach (CheckBox checkbox in checkboxes)
                    checkbox.Checked = ((uint)checkbox.Tag & inst.Flags) != 0;
            }
            else
            {
                foreach (CheckBox checkbox in checkboxes)
                    checkbox.Checked = ((uint)checkbox.Tag & agent.PUI32) != 0;
            }
        }

        public InstanceFlagsEditor(Instance instance, NumericUpDown flagscontrol)
        {
            InitializeComponent();
            this.flagscontrol = flagscontrol;

            checkboxes = new CheckBox[32];
            for (int i = 0; i < 32; ++i)
            {
                checkboxes[i] = new CheckBox() {
                    Location = new System.Drawing.Point(i/16*203 + 12, i%16*23 + 12),
                    Size = new System.Drawing.Size(200, 20),
                    Text = GetFlagText(i),
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                    Tag = (uint)(1 << i) };
                checkboxes[i].CheckedChanged += cbFlag_CheckChanged;
                Controls.Add(checkboxes[i]);
            }
            Instance = instance;
        }
        public InstanceFlagsEditor(GameObject gobject, NumericUpDown flagscontrol)
        {
            InitializeComponent();
            this.flagscontrol = flagscontrol;

            checkboxes = new CheckBox[32];
            for (int i = 0; i < 32; ++i)
            {
                checkboxes[i] = new CheckBox()
                {
                    Location = new System.Drawing.Point(i / 16 * 203 + 12, i % 16 * 23 + 12),
                    Size = new System.Drawing.Size(200, 20),
                    Text = GetFlagText(i),
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                    Tag = (uint)(1 << i)
                };
                checkboxes[i].CheckedChanged += cbFlag_CheckChanged;
                Controls.Add(checkboxes[i]);
            }
            Agent = gobject;
        }

        private void cbFlag_CheckChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            uint flag = (uint)cb.Tag;
            if (inst != null)
            {
                if (cb.Checked)
                {
                    inst.Flags |= flag;
                }
                else
                {
                    inst.Flags &= ~flag;
                }
                flagscontrol.Value = inst.Flags;
            }
            else
            {
                if (cb.Checked)
                {
                    agent.PUI32 |= flag;
                }
                else
                {
                    agent.PUI32 &= ~flag;
                }
                flagscontrol.Value = agent.PUI32;
            }
            
        }
    }
}
