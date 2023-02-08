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

        public void LoadTexture(RigidModelController model, FileController file, VertexBufferData vtx, int index)
        {
            var material = model.Data.MaterialIDs[index];
            {
                var matSec = file.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(1);
                var texSec = file.Data.GetItem<TwinsSection>(11).GetItem<TwinsSection>(0);
                if (matSec.ContainsItem(material))
                {
                    var mat = matSec.GetItem<Material>(material);
                    Texture texture = null;
                    foreach (var shader in mat.Shaders)
                    {
                        if (shader.TxtMapping == TwinsShader.TextureMapping.ON)
                        {
                            texture = texSec.GetItem<Texture>(shader.TextureId);
                            break;
                        }
                    }
                    if (texture != null)
                    {
                        var bmp = texture.GetBmp();
                        var texId = LoadTexture(ref bmp);
                        vtx.Texture = texId;
                        vtx.Flags |= BufferPointerFlags.TexCoord;
                    }
                }
            }
        }
    }
}
