using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using Twinsanity;
using TwinsaityEditor.Utils;
using System.IO;

namespace TwinsaityEditor
{
    public partial class ObjectEditor : Form
    {
        private SectionController controller;
        private GameObject gameObject;
        private FileController File { get; set; }
        private TwinsFile FileData { get => File.Data; }

        private ListManipulatorUInt16 ScriptsManipulator;
        private ListManipulatorUInt16 ObjectsManipulator;
        private ListManipulatorUInt16 AnimationsManipulator;
        private ListManipulatorUInt16 OGIManipulator;
        private ListManipulatorUInt16 SoundManipulator;
        private ListManipulatorUInt16 ParamsManipulator;

        private ListManipulatorUInt16 cScriptsManipulator;
        private ListManipulatorUInt16 cObjectsManipulator;
        private ListManipulatorUInt16 cAnimationsManipulator;
        private ListManipulatorUInt16 cOGIManipulator;
        private ListManipulatorUInt16 cSoundManipulator;
        private ListManipulatorUInt16 cCmManipulator;

        private ListManipulatorUInt32 unk1Manipulator;
        private ListManipulatorSingle unk2Manipulator;
        private ListManipulatorUInt32 unk3Manipulator;
        private ListManipulatorUInt16 unk4Manipulator;
        public ObjectEditor(SectionController c)
        {
            File = c.MainFile;
            controller = c;
            Text = $"Instance Editor (Section {c.Data.Parent.ID})";
            InitializeComponent();
            ScriptsManipulator = new ListManipulatorUInt16(scriptsAdd, scriptsRemove, scriptsSet, scriptsUp, scriptsDown, scriptsListBox, scrtiptIdSource);
            ObjectsManipulator = new ListManipulatorUInt16(objectsAdd, objectsRemove, objectsSet, objectsUp, objectsDown, objectsListBox, objectIdSource);
            OGIManipulator = new ListManipulatorUInt16(ogiAdd, ogiRemove, ogiSet, ogiUp, ogiDown, ogiListBox, ogiIdSource);
            AnimationsManipulator = new ListManipulatorUInt16(animationsAdd, animationsRemove, animationsSet, animationsUp, animationsDown, animationsListBox, animationIdSource);
            SoundManipulator = new ListManipulatorUInt16(soundsAdd, soundsRemove, soundsSet, soundsUp, soundsDown, soundsListBox, soundIdSource);
            ParamsManipulator = new ListManipulatorUInt16(paramsAdd, paramsRemove, paramsSet, paramsUp, paramsDown, paramsListBox, paramSource);

            cScriptsManipulator = new ListManipulatorUInt16(cscriptsAdd, cscriptsRemove, cscriptsSet, cscriptsUp, cscriptsDown, cscriptsList, cscriptIdSource);
            cObjectsManipulator = new ListManipulatorUInt16(cobjectAdd, cobjectRemove, cobjectSet, cobjectUp, cobjectDown, cobjectList, cobjectIdSource);
            cOGIManipulator = new ListManipulatorUInt16(cogiAdd, cogiRemove, cogiSet, cogiUp, cogiDown, cogiList, cogiIdSource);
            cAnimationsManipulator = new ListManipulatorUInt16(canimationAdd, canimationRemove, canimationSet, canimationUp, canimationDown, canimationsList, canimationIdSource);
            cSoundManipulator = new ListManipulatorUInt16(csoundAdd, csoundRemove, csoundSet, csoundUp, csoundDown, csoundsList, csoundIdSource);
            cCmManipulator = new ListManipulatorUInt16(ccmAdd, ccmRemove, ccmSet, ccmUp, ccmDown, ccmList, ccmIdSource);

            unk1Manipulator = new ListManipulatorUInt32(unk1Add, unk1Remove, unk1Set, unk1Up, unk1Down, unk1List, unk1Source);
            unk2Manipulator = new ListManipulatorSingle(unk2Add, unk2Remove, unk2Set, unk2Up, unk2Down, unk2List, unk2Source);
            unk3Manipulator = new ListManipulatorUInt32(unk3Add, unk3Remove, unk3Set, unk3Up, unk3Down, unk3List, unk3Source);
            unk4Manipulator = new ListManipulatorUInt16(unk4Add, unk4Remove, unk4Set, unk4Up, unk4Down, unk4List, unk4Source);
            PopulateList();
        }
        public void PopulateList()
        {
            objectList.BeginUpdate();
            objectList.Items.Clear();
            foreach (GameObject i in controller.Data.Records)
            {
                objectList.Items.Add(GenTextForList(i));
            }
            objectList.EndUpdate();

        }
        private string GenTextForList(GameObject gameObject)
        {
            return $"ID {gameObject.ID} {(gameObject.Name == string.Empty ? string.Empty : $" - {gameObject.Name}")}";
        }
        private void ObjectEditor_Load(object sender, EventArgs e)
        {

        }

        private void objectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (null != objectList.SelectedItem)
            {
                File.SelectItem((GameObject)controller.Data.Records[objectList.SelectedIndex]);
                gameObject = (GameObject)File.SelectedItem;
                InitLists();
            }
            else
            {
                File.SelectItem(null);
                gameObject = null;
            }
        }
        private void InitLists()
        {
            ScriptsManipulator.SetSource(gameObject.Scripts);
            ScriptsManipulator.PopulateList();
            ObjectsManipulator.SetSource(gameObject.Objects);
            ObjectsManipulator.PopulateList();
            OGIManipulator.SetSource(gameObject.OGIs);
            OGIManipulator.PopulateList();
            AnimationsManipulator.SetSource(gameObject.Anims);
            AnimationsManipulator.PopulateList();
            SoundManipulator.SetSource(gameObject.Sounds);
            SoundManipulator.PopulateList();
            ParamsManipulator.SetSource(gameObject.scriptParams);
            ParamsManipulator.PopulateList();

            cScriptsManipulator.SetSource(gameObject.cScripts);
            cScriptsManipulator.PopulateList();
            cObjectsManipulator.SetSource(gameObject.cObjects);
            cObjectsManipulator.PopulateList();
            cOGIManipulator.SetSource(gameObject.cOGIs);
            cOGIManipulator.PopulateList();
            cAnimationsManipulator.SetSource(gameObject.cAnims);
            cAnimationsManipulator.PopulateList();
            cSoundManipulator.SetSource(gameObject.cSounds);
            cSoundManipulator.PopulateList();
            cCmManipulator.SetSource(gameObject.cCM);
            cCmManipulator.PopulateList();

            unk1Manipulator.SetSource(gameObject.pUi321);
            unk1Manipulator.PopulateList();
            unk2Manipulator.SetSource(gameObject.pUi322);
            unk2Manipulator.PopulateList();
            unk3Manipulator.SetSource(gameObject.pUi323);
            unk3Manipulator.PopulateList();
            unk4Manipulator.SetSource(gameObject.cUnk);
            unk4Manipulator.PopulateList();

            nameSource.Text = gameObject.Name;
            objectId.Text = Convert.ToString(gameObject.ID, 10);
        }

        private void nameSource_TextChanged(object sender, EventArgs e)
        {
            if (gameObject != null)
            {
                gameObject.Name = ((TextBox)sender).Text;
                //objectList.Items[objectList.SelectedIndex] = gameObject.Name; //for the sake of not breaking anything - don't uncomment. And the deal is not in NPE. At all.
            }
        }

        private void flagSource_TextChanged(object sender, EventArgs e)
        {
            if (gameObject != null)
            {
                UInt32 val = 0;
                if (UInt32.TryParse(((TextBox)sender).Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out val))
                {
                    gameObject.flag = val;
                    ((TextBox)sender).BackColor = Color.White;
                } else
                {
                    ((TextBox)sender).BackColor = Color.Red;
                }
                
            }
        }

        private void objectId_TextChanged(object sender, EventArgs e)
        {
            if (gameObject != null)
            {
                UInt32 val = 0;
                if (UInt32.TryParse(((TextBox)sender).Text,  out val))
                {
                    gameObject.ID = val;
                    ((TextBox)sender).BackColor = Color.White;
                }
                else
                {
                    ((TextBox)sender).BackColor = Color.Red;
                }

            }
        }

        private void deleteObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sel_i = objectList.SelectedIndex;
            if (sel_i == -1)
                return;
            controller.RemoveItem(gameObject.ID);
            objectList.BeginUpdate();
            objectList.Items.RemoveAt(sel_i);
            if (sel_i >= objectList.Items.Count) sel_i = objectList.Items.Count - 1;
            objectList.SelectedIndex = sel_i;
            objectList.EndUpdate();
            controller.UpdateTextBox();
        }

        private void createObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ushort maxid = (ushort)controller.Data.RecordIDs.Select(p => p.Key).Max();
            ushort id = Math.Max((ushort)(32 * 1024), maxid);
            ++id;
            GameObject newGameObject = new GameObject();
            newGameObject.ID = id;
            newGameObject.Name = "New Game Object";
            controller.Data.AddItem(id, newGameObject);
            ((MainForm)Tag).GenTreeNode(newGameObject, controller);
            gameObject = newGameObject;
<<<<<<< HEAD
            int i = objectList.Items.Add(GenTextForList(newGameObject));
            objectList.SelectedIndex = i;
=======
            objectList.Items.Add(GenTextForList(newGameObject));
>>>>>>> 4c3ec50355e2df7d1bc6482e0fc13b265228bcab
            controller.UpdateText();
            ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[newGameObject.ID]].Tag).UpdateText();
        }

        private void duplicateObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gameObject != null)
            {
                ushort maxid = (ushort)controller.Data.RecordIDs.Select(p => p.Key).Max();
                ushort id = Math.Max((ushort)(32 * 1024), maxid);
                ++id;
                GameObject newGameObject = new GameObject();
                using (MemoryStream stream = new MemoryStream())
                using (BinaryWriter writer = new BinaryWriter(stream))
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    gameObject.Save(writer);
                    reader.BaseStream.Position = 0;
                    newGameObject.Load(reader, (int)reader.BaseStream.Length);
                }
                newGameObject.ID = id;
                controller.Data.AddItem(id, newGameObject);
                ((MainForm)Tag).GenTreeNode(newGameObject, controller);
                gameObject = newGameObject;
<<<<<<< HEAD
                int i = objectList.Items.Add(GenTextForList(newGameObject));
                objectList.SelectedIndex = i;
=======
                objectList.Items.Add(GenTextForList(newGameObject));
>>>>>>> 4c3ec50355e2df7d1bc6482e0fc13b265228bcab
                controller.UpdateText();
                ((Controller)controller.Node.Nodes[controller.Data.RecordIDs[newGameObject.ID]].Tag).UpdateText();
            }
        }
    }
}
