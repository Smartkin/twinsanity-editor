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
    public partial class IDEditor : Form
    {

        public ItemController item;

        private FileController File { get; set; }
        private bool ignore;
        private uint DataID;

        public IDEditor(ItemController i)
        {
            File = i.MainFile;
            item = i;
            DataID = i.Data.ID;
            InitializeComponent();
            textBox1.Text = $"{DataID:X8}";
            Text = $"ID Editor {i.GetItemName()}";
            numericUpDown2.Value = DataID;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (item.Data.ID == DataID)
            {
                Close();
                return;
            }
            var p = item.Data.Parent;
            if (p.ContainsItem(DataID))
            {
                MessageBox.Show("New ID already exists.");
                return;
            }
            var index = p.RecordIDs[item.Data.ID];
            p.RecordIDs.Remove(item.Data.ID);
            p.RecordIDs.Add(DataID, index);
            item.Data.ID = DataID;
            item.UpdateText();
            item.UpdateTextBox();
            item.UpdateName();
            Close();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (ignore) return;
            ignore = true;
            DataID = (uint)numericUpDown2.Value;
            textBox1.Text = $"{DataID:X8}";
            ignore = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (ignore) return;
            if (uint.TryParse(textBox1.Text, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out uint x))
            {
                ignore = true;
                DataID = x;
                numericUpDown2.Value = x;
                ignore = false;
            }
        }
    }
}
