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
                $"Total frames: {Data.TotalFrames}",
                $"Blob packed 1: 0x{Data.UnkBlobSizePacked1:X}",
                $"Joint settings 1: {Data.JointsSettings.Count}",
                $"Transformations 1: {Data.StaticTransforms.Count}",
                $"Interpolate transformations 1: {Data.AnimatedTransforms.Count}",
                $"Total frames 2: {Data.TotalFrames2}",
                $"Blob packed 2: 0x{Data.UnkBlobSizePacked2:X}",
                $"Joint settings 2: {Data.JointsSettings2.Count}",
                $"Transformations 2: {Data.StaticTransforms2.Count}",
                $"Interpolate transformations 2: {Data.AnimatedTransforms2.Count}",
            };
            TextPrev = text.ToArray();
        }

        private void Menu_OpenEditor()
        {
            MainFile.OpenEditor(this);
        }
    }
}
