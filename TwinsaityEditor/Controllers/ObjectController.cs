using System.Windows.Forms;
using System.IO;
using System;
using Twinsanity;

namespace TwinsaityEditor
{
    public class ObjectController : Controller
    {
        private GameObject data;

        public ObjectController(GameObject item)
        {
            Toolbar = ToolbarFlags.Hex | ToolbarFlags.Extract | ToolbarFlags.Replace | ToolbarFlags.Delete;
            data = item;
        }

        public override string GetName()
        {
            return data.Name + " [ID: " + data.ID + "]";
        }

        public override void GenText()
        {
            TextPrev = new string[10 + data.UI32.Length + data.OGIs.Length + data.Anims.Length + data.Scripts.Length + data.Objects.Length + data.Sounds.Length];
            TextPrev[0] = "ID: " + data.ID;
            TextPrev[1] = "Offset: " + data.Offset + " Size: " + data.Size;
            TextPrev[2] = "Name: " + data.Name;
            TextPrev[3] = "Headers: " + Convert.ToString(data.Class1, 16) + " " + Convert.ToString(data.Class2, 16) + " " + Convert.ToString(data.Class3, 16);

            TextPrev[4] = "UnknownInt32Count: " + data.UI32.Length;
            for (int i = 0; i < data.UI32.Length; ++i)
                TextPrev[5 + i] = data.UI32[i].ToString();

            TextPrev[5 + data.UI32.Length] = "OGICount: " + data.OGIs.Length;
            for (int i = 0; i < data.OGIs.Length; ++i)
                TextPrev[6 + data.UI32.Length + i] = data.OGIs[i].ToString();

            TextPrev[6 + data.UI32.Length + data.OGIs.Length] = "AnimCount: " + data.Anims.Length;
            for (int i = 0; i < data.Anims.Length; ++i)
                TextPrev[7 + data.UI32.Length + data.OGIs.Length + i] = data.Anims[i].ToString();

            TextPrev[7 + data.UI32.Length + data.OGIs.Length + data.Anims.Length] = "ScriptCount: " + data.Scripts.Length;
            for (int i = 0; i < data.Scripts.Length; ++i)
                TextPrev[8 + data.UI32.Length + data.OGIs.Length + data.Anims.Length + i] = data.Scripts[i].ToString();

            TextPrev[8 + data.UI32.Length + data.OGIs.Length + data.Anims.Length + data.Scripts.Length] = "ObjectCount: " + data.Objects.Length;
            for (int i = 0; i < data.Objects.Length; ++i)
                TextPrev[9 + data.UI32.Length + data.OGIs.Length + data.Anims.Length + data.Scripts.Length + i] = data.Objects[i].ToString();

            TextPrev[9 + data.UI32.Length + data.OGIs.Length + data.Anims.Length + data.Scripts.Length + data.Objects.Length] = "SoundCount: " + data.Sounds.Length;
            for (int i = 0; i < data.Sounds.Length; ++i)
                TextPrev[10 + data.UI32.Length + data.OGIs.Length + data.Anims.Length + data.Scripts.Length + data.Objects.Length + i] = data.Sounds[i].ToString();
        }

        public override void ToolbarAction(ToolbarFlags button)
        {
            switch (button)
            {
                case ToolbarFlags.Hex:
                    //do hex stuff here
                    break;
                case ToolbarFlags.Extract:
                    {
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.FileName = GetName().Replace(":", string.Empty);
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            FileStream file = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                            BinaryWriter writer = new BinaryWriter(file);
                            data.Save(writer);
                            writer.Close();
                            file.Close();
                        }
                    }
                    break;
            }
        }
    }
}
