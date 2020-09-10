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
            List<string> text = new List<string>
            {
                $"ID: {Data.ID}",
                $"Size: {Data.Size}",
                $"Unknown bitfield: 0x{Data.Bitfield:X}",
                $"Blob packed 1: 0x{Data.UnkBlobSizePacked1:X}",
                $"Blob size helper 1: 0x{Data.UnkBlobSizeHelper1:X}",
                $"Blob 1 size: {Data.unkBlob1.Length}",
                $"Blob packed 2: 0x{Data.UnkBlobSizePacked2:X}",
                $"Blob size helper 2: 0x{Data.UnkBlobSizeHelper2:X}",
                $"Blob 2 size: {Data.unkBlob2.Length}"
            };
            TextPrev = text.ToArray();
        }
    }
}
