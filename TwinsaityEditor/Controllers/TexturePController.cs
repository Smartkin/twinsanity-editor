﻿using System.Collections.Generic;
using Twinsanity;

namespace TwinsaityEditor
{
    public class TexturePController : ItemController
    {
        public new TextureP Data { get; set; }

        public TexturePController(MainForm topform, TextureP item) : base(topform, item)
        {
            Data = item;
            if (Data.RawData != null)
            {
                AddMenu("View texture", Menu_OpenViewer);
            }
        }

        protected override string GetName()
        {
            return string.Format("TextureP [ID {0:X8}]", Data.ID);
        }

        protected override void GenText()
        {
            List<string> text = new List<string>();
            text.Add(string.Format("ID: {0:X8}", Data.ID));
            text.Add($"Size: {Data.Size}");
            text.Add($"Image Size: {Data.Width}x{Data.Height}");
            text.Add($"Mip levels: {Data.MipLevels}");
            //text.Add($"Texture format: {Data.PixelFormat}");
            //text.Add($"GS destination format: {Data.DestinationPixelFormat}");
            //text.Add($"Texture function: {Data.TexFun}");
            //text.Add($"Color component : {Data.ColorComponent}");
            text.Add($"Texture buffer width(in words): {Data.TextureBufferWidth}");
            TextPrev = text.ToArray();
        }

        private void Menu_OpenViewer()
        {
            MainFile.OpenTextureViewer(this);
        }
    }
}
