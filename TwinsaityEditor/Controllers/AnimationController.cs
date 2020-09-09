using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twinsanity;

namespace TwinsaityEditor.Controllers
{
    class AnimationController : ItemController
    {
        public new Animation Data { get; set; }
        
        public AnimationController(MainForm topform, Animation item) : base(topform, item)
        {
            Data = item;
        }

        protected override string GetName()
        {
            return $"Animation [ID {Data.ID}]";
        }

        protected override void GenText()
        {
            List<string> text = new List<string>();
            text.Add($"ID: {Data.ID}");
            text.Add($"Size: {Data.Size}");
            text.Add($"Unknown bitfield: 0x{Data.Bitfield:X}");
            text.Add($"Blob packed 1: 0x{Data.UnkBlobSizePacked1:X}");
            text.Add($"Blob size helper 1: 0x{Data.UnkBlobSizeHelper1:X}");
            text.Add($"Blob 1 size: {Data.unkBlob1.Length}");
            text.Add($"Blob packed 2: 0x{Data.UnkBlobSizePacked2:X}");
            text.Add($"Blob size helper 2: 0x{Data.UnkBlobSizeHelper2:X}");
            text.Add($"Blob 2 size: {Data.unkBlob2.Length}");
            TextPrev = text.ToArray();
        }
    }
}
