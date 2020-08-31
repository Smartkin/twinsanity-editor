using Twinsanity;
using System.Collections.Generic;
using System;

namespace TwinsaityEditor
{
    public class CameraController : ItemController
    {
        public new Camera Data { get; set; }

        public CameraController(MainForm topform, Camera item) : base(topform, item)
        {
            Data = item;
            //AddMenu("Open editor", Menu_OpenEditor);
        }

        protected override string GetName()
        {
            return string.Format("Camera [ID {0}]", Data.ID);
        }

        protected override void GenText()
        {
            List<string> text = new List<string>();

            text.Add(string.Format("ID: {0:X8}", Data.ID));
            text.Add($"Size: {Data.Size}");
            text.Add($"Header: {Data.Header}");
            text.Add($"Enabled: {Data.Enabled}");
            text.Add($"UnkFac1: {Data.UnkFac1}");
            text.Add($"Trigger Rot: {Data.TriggerRot.X}; {Data.TriggerRot.Y}; {Data.TriggerRot.Z}; {Data.TriggerRot.W}; ");
            text.Add($"Trigger Pos: {Data.TriggerPos.X}; {Data.TriggerPos.Y}; {Data.TriggerPos.Z}; {Data.TriggerPos.W}; ");
            text.Add($"TriggerSize: {Data.TriggerSize.X}; {Data.TriggerSize.Y}; {Data.TriggerSize.Z}; {Data.TriggerSize.W}; ");
            text.Add($"Ints: {Data.Int0}; {Data.Int1}; {Data.Int2};");
            text.Add($"Cam Rot: {Data.CamRot.Pitch}; {Data.CamRot.Yaw}; {Data.CamRot.Roll};");
            text.Add($"UnkFac2: {Data.UnkFac2}");
            text.Add($"SecHeader1: {Data.SecHeader1}");
            text.Add($"SecHeader2: {Data.SecHeader2}");
            text.Add($"Type: {Data.Type}");

            TextPrev = text.ToArray();

        }

        private void Menu_OpenEditor()
        {
            MainFile.OpenEditor((SectionController)Node.Parent.Tag);
        }
    }
}