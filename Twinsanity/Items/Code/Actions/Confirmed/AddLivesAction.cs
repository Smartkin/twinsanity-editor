using System;
using System.Collections.Generic;

namespace Twinsanity.Actions
{
    [ActionID(DefaultEnums.CommandID.AddLives, DefaultEnums.CommandID.AddLives_Duplicate, DefaultEnums.CommandID.AddLives_Duplicate2)]
    public class AddLivesAction : ScriptAction
    {
        public int LivesToAdd { get; set; } = 1;

        public override void Load(Script.MainScript.ScriptCommand input)
        {
            LivesToAdd = (int)input.arguments[0];
        }

        public override void Save(Script.MainScript.ScriptCommand output)
        {
            output.arguments = new List<uint>() { (uint)LivesToAdd };
        }
    }
}
