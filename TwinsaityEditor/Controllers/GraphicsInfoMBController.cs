using Twinsanity;
using System.Collections.Generic;

namespace TwinsaityEditor
{
    public class GraphicsInfoMBController : GraphicsInfoController
    {
        public new GraphicsInfoMB Data { get; set; }

        public GraphicsInfoMBController(MainForm topform, GraphicsInfoMB item) : base(topform, item)
        {
            Data = item;
        }

        private void Menu_OpenViewer()
        {
            MainFile.OpenMeshViewer(this, MainFile);
        }
    }
}