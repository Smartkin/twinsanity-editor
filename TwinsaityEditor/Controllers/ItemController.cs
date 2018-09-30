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
            AddMenu("Replace raw data with new file", Menu_ReplaceItem);
        }

        protected override string GetName()
        {
            return "Item [ID " + Data.ID + "]";
        }

        protected override void GenText()
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

        private void Menu_ReplaceItem()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                BinaryReader reader = new BinaryReader(new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read));
                if (Data is ChunkLinks)
                {
                    MainForm.CloseEditor(MainForm.Editors.ChunkLinks);
                    ((ChunkLinks)Data).Load(reader, (int)reader.BaseStream.Length);
                }
                else if (Data is Instance)
                {
                    MainForm.CloseInstanceEditor((int)Data.Parent.Parent.ID);
                    ((Instance)Data).Load(reader, (int)reader.BaseStream.Length);
                }
                else if (Data is Position)
                {
                    MainForm.CloseInstanceEditor((int)Data.Parent.Parent.ID);
                    ((Position)Data).Load(reader, (int)reader.BaseStream.Length);
                }
                else
                    Data.Load(reader, (int)reader.BaseStream.Length);
                reader.Close();
                UpdateText();
            }
        }
    }
}
