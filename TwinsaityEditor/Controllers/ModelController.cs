using Twinsanity;

namespace TwinsaityEditor
{
    public class ModelController : ItemController
    {
        public new Model Data { get; set; }

        public ModelController(Model item) : base(item)
        {
            Data = item;
        }

        protected override string GetName()
        {
            return "Model [ID " + Data.ID + "]";
        }

        protected override void GenText()
        {
            TextPrev = new string[4 + Data.MaterialIDs.Length];
            TextPrev[0] = "ID: " + Data.ID;
            TextPrev[1] = "Offset: " + Data.Offset + " Size: " + Data.Size;
            TextPrev[2] = "Header: " + Data.Header + " MaterialCount: " + Data.MaterialIDs.Length;
            for (int i = 0; i < Data.MaterialIDs.Length; ++i)
                TextPrev[3 + i] = FileController.GetMaterialName(Data.MaterialIDs[i]);
            TextPrev[3 + Data.MaterialIDs.Length] = "Mesh: " + Data.MeshID;
        }
    }
}
