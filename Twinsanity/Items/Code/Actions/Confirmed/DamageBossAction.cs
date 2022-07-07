using System;
using System.Collections.Generic;

namespace Twinsanity.Actions
{
    [ActionID(DefaultEnums.CommandID.DamageBoss)]
    public class DamageBossAction : ScriptAction
    {
        public int DamageAdd { get; set; } = -1;

        public override void Load(Script.MainScript.ScriptCommand input)
        {
            DamageAdd = (int)input.arguments[0];
        }

        public override void Save(Script.MainScript.ScriptCommand output)
        {
            output.arguments = new List<uint>() { (uint)DamageAdd };
        }

        public override string ToString()
        {
            return $"DamageBoss {DamageAdd}";
        }
    }
}
