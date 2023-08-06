using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public class ModelViewer : MeshViewer
    {
        private RigidModelController model;
        private FileController file;

        public ModelViewer(RigidModelController model, Form pform) : base(model, pform)
        {
            //initialize variables here
            this.model = model;
            file = model.MainFile;
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
