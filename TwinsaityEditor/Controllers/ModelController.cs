using Twinsanity;

namespace TwinsaityEditor
{
    public class ModelController : ItemController
    {
        private Model data;

        public ModelController(Model item) : base(item)
        {
            data = item;
        }

        public override string GetName()
        {
            return "Model [ID " + data.ID + "]";
        }

        public override void GenText()
        {
            TextPrev = new string[4 + data.MaterialIDs.Length];
            TextPrev[0] = "ID: " + data.ID;
            TextPrev[1] = "Offset: " + data.Offset + " Size: " + data.Size;
            TextPrev[2] = "Header: " + data.Header + " MaterialCount: " + data.MaterialIDs.Length;
            for (int i = 0; i < data.MaterialIDs.Length; ++i)
                TextPrev[3 + i] = FileController.GetMaterialName(data.MaterialIDs[i]);
            TextPrev[3 + data.MaterialIDs.Length] = "Mesh: " + data.MeshID;
        }
    }
}
