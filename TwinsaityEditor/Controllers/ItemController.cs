using System.Windows.Forms;
using System.IO;
using Twinsanity;

namespace TwinsaityEditor
{
    public class ItemController : Controller
    {
        public TwinsItem Data { get; set; }

        public ItemController(TwinsItem item)
        {
            Data = item;
            AddMenu("Extract raw data to file", Menu_ExtractItem);
        }

        public override string GetName()
        {
            return "Item [ID " + Data.ID + "]";
        }

        public override void GenText()
        {
            TextPrev = new string[2];
            TextPrev[0] = "ID: " + Data.ID;
            TextPrev[1] = "Offset: " + Data.Offset + " Size: " + Data.Size;
        }

        private void Menu_ExtractItem()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = GetName().Replace(":", string.Empty);
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileStream file = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                BinaryWriter writer = new BinaryWriter(file);
                Data.Save(writer);
                writer.Close();
            }
        }
    }
}
