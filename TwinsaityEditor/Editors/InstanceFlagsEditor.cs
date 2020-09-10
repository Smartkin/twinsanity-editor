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
    public partial class InstanceFlagsEditor : Form
    {
        private Instance inst;
        public Instance Instance
        {
            set
            {
                inst = value;
                UpdateFlags();
            }
            get
            {
                return inst;
            }
        }
        private TextBox tbFlags;
        public InstanceFlagsEditor(Instance instance, TextBox tbFlags)
        {
            inst = instance;
            InitializeComponent();
            this.tbFlags = tbFlags;
            cbFlag1.Tag = (UInt32)0x1;
            cbFlag2.Tag = (UInt32)0x2;
            cbFlagVisible.Tag = (UInt32)0x4;
            cbFlag4.Tag = (UInt32)0x8;
            cbFlag5.Tag = (UInt32)0x10;
            cbFlag6.Tag = (UInt32)0x20;
            cbFlag7.Tag = (UInt32)0x40;
            cbFlag8.Tag = (UInt32)0x80;
            cbFlag9.Tag = (UInt32)0x100;
            cbFlag10.Tag = (UInt32)0x200;
            cbFlag11.Tag = (UInt32)0x400;
            cbFlag12.Tag = (UInt32)0x800;
            cbFlag13.Tag = (UInt32)0x1000;
            cbFlag14.Tag = (UInt32)0x2000;
            cbFlag15.Tag = (UInt32)0x4000;
            cbFlag16.Tag = (UInt32)0x8000;
            cbFlag17.Tag = (UInt32)0x10000;
            cbFlag18.Tag = (UInt32)0x20000;
            cbFlag19.Tag = (UInt32)0x40000;
            cbFlag20.Tag = (UInt32)0x80000;
            cbFlag21.Tag = (UInt32)0x100000;
            cbFlag22.Tag = (UInt32)0x200000;
            cbFlag23.Tag = (UInt32)0x400000;
            cbFlag24.Tag = (UInt32)0x800000;
            cbFlag25.Tag = (UInt32)0x1000000;
            cbFlag26.Tag = (UInt32)0x2000000;
            cbFlag27.Tag = (UInt32)0x4000000;
            cbFlag28.Tag = (UInt32)0x8000000;
            cbFlag29.Tag = (UInt32)0x10000000;
            cbFlag30.Tag = (UInt32)0x20000000;
            cbFlag31.Tag = (UInt32)0x40000000;
            cbFlag32.Tag = (UInt32)0x80000000;
            UpdateFlags();
        }

        private void UpdateFlags()
        {
            cbFlag1.Checked = (inst.Flags & 0x1) != 0;
            cbFlag2.Checked = (inst.Flags & 0x2) != 0;
            cbFlagVisible.Checked = (inst.Flags & 0x4) != 0;
            cbFlag4.Checked = (inst.Flags & 0x8) != 0;
            cbFlag5.Checked = (inst.Flags & 0x10) != 0;
            cbFlag6.Checked = (inst.Flags & 0x20) != 0;
            cbFlag7.Checked = (inst.Flags & 0x40) != 0;
            cbFlag8.Checked = (inst.Flags & 0x80) != 0;
            cbFlag9.Checked = (inst.Flags & 0x100) != 0;
            cbFlag10.Checked = (inst.Flags & 0x200) != 0;
            cbFlag11.Checked = (inst.Flags & 0x400) != 0;
            cbFlag12.Checked = (inst.Flags & 0x800) != 0;
            cbFlag13.Checked = (inst.Flags & 0x1000) != 0;
            cbFlag14.Checked = (inst.Flags & 0x2000) != 0;
            cbFlag15.Checked = (inst.Flags & 0x4000) != 0;
            cbFlag16.Checked = (inst.Flags & 0x8000) != 0;
            cbFlag17.Checked = (inst.Flags & 0x10000) != 0;
            cbFlag18.Checked = (inst.Flags & 0x20000) != 0;
            cbFlag19.Checked = (inst.Flags & 0x40000) != 0;
            cbFlag20.Checked = (inst.Flags & 0x80000) != 0;
            cbFlag21.Checked = (inst.Flags & 0x100000) != 0;
            cbFlag22.Checked = (inst.Flags & 0x200000) != 0;
            cbFlag23.Checked = (inst.Flags & 0x400000) != 0;
            cbFlag24.Checked = (inst.Flags & 0x800000) != 0;
            cbFlag25.Checked = (inst.Flags & 0x1000000) != 0;
            cbFlag26.Checked = (inst.Flags & 0x2000000) != 0;
            cbFlag27.Checked = (inst.Flags & 0x4000000) != 0;
            cbFlag28.Checked = (inst.Flags & 0x8000000) != 0;
            cbFlag29.Checked = (inst.Flags & 0x10000000) != 0;
            cbFlag30.Checked = (inst.Flags & 0x20000000) != 0;
            cbFlag31.Checked = (inst.Flags & 0x40000000) != 0;
            cbFlag32.Checked = (inst.Flags & 0x80000000) != 0;
        }

        private void cbFlag_CheckChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            UInt32 flag = (UInt32)cb.Tag;
            if (cb.Checked)
            {
                inst.Flags |= flag;
            }
            else
            {
                inst.Flags &= ~flag;
            }
            tbFlags.Text = inst.Flags.ToString("X");
        }
    }
}
