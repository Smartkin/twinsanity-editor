/**
 * Code taken from https://github.com/Robmaister/SharpFont/blob/master/Source/Examples/FontFormat.cs
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinsaityEditor.FontWrapper
{
    internal class FontFormat
    {
        public string Name { get; private set; }

        public string FileExtension { get; private set; }

        public FontFormat(string name, string ext)
        {
            if (!ext.StartsWith(".")) ext = "." + ext;
            this.Name = name; this.FileExtension = ext;
        }
    }

    internal class FontFormatCollection : Dictionary<string, FontFormat>
    {
        public void Add(string name, string ext)
        {
            if (!ext.StartsWith(".")) ext = "." + ext;
            this.Add(ext, new FontFormat(name, ext));
        }

        public bool ContainsExt(string ext)
        {
            return this.ContainsKey(ext);
        }
    }
}
