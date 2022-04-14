using System;
using System.Collections.Generic;

namespace Twinsanity.Actions
{
    [ActionID(DefaultEnums.CommandID.AddAmmo)]
    public class AddAmmoAction : ScriptAction
    {
        public int AmmoToAdd { get; set; } = 20;

        public override void Load(Script.MainScript.ScriptCommand input)
        {
            AmmoToAdd = (int)input.arguments[0];
        }

        public override void Save(Script.MainScript.ScriptCommand output)
        {
            output.arguments = new List<uint>() { (uint)AmmoToAdd };
        }
    }
}
