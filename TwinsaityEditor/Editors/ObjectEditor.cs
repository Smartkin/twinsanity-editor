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
        public ObjectEditor(SectionController c)
        {
            File = c.MainFile;
            controller = c;
            Text = $"Instance Editor (Section {c.Data.Parent.ID})";
            InitializeComponent();
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
            }
            else
            {
                File.SelectItem(null);
                gameObject = null;
            }
            InitLists();
        }
        private void InitLists()
        {
            ScriptsManipulator = new ListManipulatorUInt16(gameObject.Scripts, scriptsListBox, scrtiptIdSource,
                scriptsAdd, scriptsRemove, scriptsSet, scriptsUp, scriptsDown);
            ScriptsManipulator.PopulateList();
            ObjectsManipulator = new ListManipulatorUInt16(gameObject.Objects, objectsListBox, objectIdSource,
                objectsAdd, objectsRemove, objectsSet, objectsUp, objectsDown);
            ObjectsManipulator.PopulateList();
            OGIManipulator = new ListManipulatorUInt16(gameObject.OGIs, ogiListBox, ogiIdSource,
                ogiAdd, ogiRemove, ogiSet, ogiUp, ogiDown);
            OGIManipulator.PopulateList();
            AnimationsManipulator = new ListManipulatorUInt16(gameObject.Anims, animationsListBox, animationIdSource,
                animationsAdd, animationsRemove, animationsSet, animationsUp, animationsDown);
            AnimationsManipulator.PopulateList();
            SoundManipulator = new ListManipulatorUInt16(gameObject.Sounds, soundsListBox, soundIdSource,
                soundsAdd, soundsRemove, soundsSet, soundsUp, soundsDown);
            SoundManipulator.PopulateList();
            ParamsManipulator = new ListManipulatorUInt16(gameObject.scriptParams, paramsListBox, paramSource,
                paramsAdd, paramsRemove, paramsSet, paramsUp, paramsDown);
            ParamsManipulator.PopulateList();

            cScriptsManipulator = new ListManipulatorUInt16(gameObject.cScripts, cscriptsList, cscriptIdSource,
                cscriptsAdd, cscriptsRemove, cscriptsSet, cscriptsUp, cscriptsDown);
            cScriptsManipulator.PopulateList();
            cObjectsManipulator = new ListManipulatorUInt16(gameObject.cObjects, cobjectList, cobjectIdSource,
                cobjectAdd, cobjectRemove, cobjectSet, cobjectUp, cobjectDown);
            cObjectsManipulator.PopulateList();
            cOGIManipulator = new ListManipulatorUInt16(gameObject.cOGIs, cogiList, cogiIdSource,
                cogiAdd, cogiRemove, cogiSet, cogiUp, cogiDown);
            cOGIManipulator.PopulateList();
            cAnimationsManipulator = new ListManipulatorUInt16(gameObject.cAnims, canimationsList, canimationIdSource,
                canimationAdd, canimationRemove, canimationSet, canimationUp, canimationDown);
            cAnimationsManipulator.PopulateList();
            cSoundManipulator = new ListManipulatorUInt16(gameObject.cSounds, csoundsList, csoundIdSource,
                csoundAdd, csoundRemove, csoundSet, csoundUp, csoundDown);
            cSoundManipulator.PopulateList();
            cCmManipulator = new ListManipulatorUInt16(gameObject.cCM, ccmList, ccmIdSource,
                ccmAdd, ccmRemove, ccmSet, ccmUp, ccmDown);
            cCmManipulator.PopulateList();

            nameSource.Text = gameObject.Name;
            flagSource.Text = Convert.ToString(gameObject.flag, 16);
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
    }
}
