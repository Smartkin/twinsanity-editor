using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twinsanity;

namespace TwinsaityEditor.Controllers
{
    public class AnimationController : ItemController
    {
        public new Animation Data { get; set; }
        
        public AnimationController(MainForm topform, Animation item) : base(topform, item)
        {
            Data = item;
            AddMenu("Open editor", Menu_OpenEditor);
        }

        protected override string GetName()
        {
            return $"Animation [ID {Data.ID}]";
        }

        protected override void GenText()
        {
            List<string> text = new List<string>
            {
                $"ID: {Data.ID}",
                $"Size: {Data.Size}",
                $"Unknown bitfield: 0x{Data.Bitfield:X}",
                $"Blob packed 1: 0x{Data.UnkBlobSizePacked1:X}",
                $"Displacements 1: {Data.Displacements.Count}",
                $"Scales 1: {Data.Scales.Count}",
                $"Rotations 1: {Data.Rotations.Count}",
                $"Blob packed 2: 0x{Data.UnkBlobSizePacked2:X}",
                $"Displacements 2: {Data.Displacements2.Count}",
                $"Scales 2: {Data.Scales2.Count}",
                $"Rotations 2: {Data.Rotations2.Count}",
            };
            TextPrev = text.ToArray();
        }

        private void Menu_OpenEditor()
        {
            MainFile.OpenEditor(this);
        }
    }
}
