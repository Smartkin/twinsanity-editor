using System;
using System.Collections.Generic;

namespace Twinsanity.Actions
{
    //[ActionID(DefaultEnums.CommandID.PosWarp)]
    public class PosWarpAction : ScriptAction
    {
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public float PositionW { get; set; }

        public bool WorldSpace { get; set; }
        public bool InitialSpace { get; set; }
        public bool CurrentSpace { get; set; }
        public bool TargetSpace { get; set; }
        public bool StoredSpace { get; set; }
        public bool CurrentKey { get; set; }
        public bool NextKey { get; set; }
        public bool CurrentNode { get; set; }
        public bool NextNode { get; set; }
        public bool Focus { get; set; }
        public bool FocusPos { get; set; }

        [Flags]
        public enum SpaceOptions
        {
            WorldSpace = 1,
            InitialSpace = 2,
            CurrentSpace = 4,
            TargetSpace = 8,
            StoredSpace = 16,
            CurrentKey = 32,
            NextKey = 64,
            CurrentNode = 128,
            NextNode = 256,
            Focus = 512,
            FocusPos = 1024,
        }

        public override void Load(Script.MainScript.ScriptCommand input)
        {
            
        }

        public override void Save(Script.MainScript.ScriptCommand output)
        {
            
        }
    }
}
