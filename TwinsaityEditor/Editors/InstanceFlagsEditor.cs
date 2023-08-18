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
            "Deactivated?", // 0
            "Collidable", // 1
            "Visible", // 2
            "Shadow", // 3
            string.Empty, // 4
            "Tangible", // 5
            string.Empty, // 6
            string.Empty, // 7
            "Receive OnTrigger signals?", // 8
            "Can damage character?", // 9
            string.Empty, // 10
            string.Empty, // 11
            string.Empty, // 12
            string.Empty, // 13
            string.Empty, // 14
            "Harmful?", // 15
            "Can always damage character?", // 16
            string.Empty, // 17
            string.Empty, // 18
            string.Empty, // 19
            string.Empty, // 20
            string.Empty, // 21
            string.Empty, // 22
            string.Empty, // 23
            string.Empty, // 24
            string.Empty, // 25
            string.Empty, // 26
            string.Empty, // 27
            string.Empty, // 28
            string.Empty, // 29
            string.Empty, // 30
            string.Empty // 31
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
