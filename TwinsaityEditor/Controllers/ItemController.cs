using System.Windows.Forms;
using System.IO;
using Twinsanity;

namespace TwinsaityEditor
{
    public class ItemController : Controller
    {
        private TwinsItem data;

        public ItemController(TwinsItem item)
        {
            data = item;
            AddMenu("Extract to file", Menu_ExtractItem);
        }

        public override string GetName()
        {
            return "Item [ID " + data.ID + "]";
        }

        public override void GenText()
        {
            TextPrev = new string[2];
            TextPrev[0] = "ID: " + data.ID;
            TextPrev[1] = "Offset: " + data.Offset + " Size: " + data.Size;
        }

        private void Menu_ExtractItem()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = GetName().Replace(":", string.Empty);
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileStream file = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                BinaryWriter writer = new BinaryWriter(file);
                data.Save(writer);
                writer.Close();
            }
        }
    }
}
