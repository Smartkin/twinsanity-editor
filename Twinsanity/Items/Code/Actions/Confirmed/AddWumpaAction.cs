using System;
using System.Collections.Generic;

namespace Twinsanity.Actions
{
    [ActionID(DefaultEnums.CommandID.CA_PickUpWumpa)]
    public class AddWumpaAction : ScriptAction
    {
        public int WumpaToAdd { get; set; } = 1;

        public override void Load(Script.MainScript.ScriptCommand input)
        {
            WumpaToAdd = (int)input.arguments[0];
        }

        public override void Save(Script.MainScript.ScriptCommand output)
        {
            output.arguments = new List<uint>() { (uint)WumpaToAdd };
        }

        public override string ToString()
        {
            return $"AddWumpa {WumpaToAdd}";
        }
    }
}
