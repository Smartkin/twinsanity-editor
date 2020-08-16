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
    public partial class ObjectEditor : Form
    {
        private SectionController controller;
        private GameObject gameObject;
        private FileController File { get; set; }
        private TwinsFile FileData { get => File.Data; }
        public ObjectEditor(SectionController c)
        {
            InitializeComponent();
        }

        private void ObjectEditor_Load(object sender, EventArgs e)
        {

        }
    }
}
