using System;
using System.Collections.Generic;

namespace Twinsanity.Actions
{
    [ActionID(DefaultEnums.CommandID.ReduceHitPoints)]
    public class ReduceHitPointsAction : ScriptAction
    {
        public int Damage { get; set; } = 10;

        public override void Load(Script.MainScript.ScriptCommand input)
        {
            Damage = (int)input.arguments[0];
        }

        public override void Save(Script.MainScript.ScriptCommand output)
        {
            output.arguments = new List<uint>() { (uint)Damage };
        }

        public override string ToString()
        {
            return $"ReduceHitPoints {Damage}";
        }
    }
}
