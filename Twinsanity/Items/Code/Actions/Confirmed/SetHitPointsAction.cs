using System;
using System.Collections.Generic;

namespace Twinsanity.Actions
{
    [ActionID(DefaultEnums.CommandID.SetHitPoints)]
    public class SetHitPointsAction : ScriptAction
    {
        public int HitPoints { get; set; } = 1;

        public override void Load(Script.MainScript.ScriptCommand input)
        {
            HitPoints = (int)input.arguments[0];
        }

        public override void Save(Script.MainScript.ScriptCommand output)
        {
            output.arguments = new List<uint>() { (uint)HitPoints };
        }
    }
}
