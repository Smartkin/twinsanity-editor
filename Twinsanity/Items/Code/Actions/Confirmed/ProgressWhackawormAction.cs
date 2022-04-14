using System;
using System.Collections.Generic;

namespace Twinsanity.Actions
{
    [ActionID(DefaultEnums.CommandID.ProgressWhackaworm)]
    public class ProgressWhackawormAction : ScriptAction
    {
        public int WormCounterAdd { get; set; } = -1;

        public override void Load(Script.MainScript.ScriptCommand input)
        {
            WormCounterAdd = (int)input.arguments[0];
        }

        public override void Save(Script.MainScript.ScriptCommand output)
        {
            output.arguments = new List<uint>() { (uint)WormCounterAdd };
        }
    }
}
