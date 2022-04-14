using System;
using System.Collections.Generic;

namespace Twinsanity.Actions
{
    [ActionID(DefaultEnums.CommandID.AddGem)]
    public class AddGemAction : ScriptAction
    {
        public GemType Gem { get; set; } = GemType.Red;

        public enum GemType
        {
            Blue = 0,
            Clear,
            Green,
            Purple,
            Red,
            Yellow,
        }

        public override void Load(Script.MainScript.ScriptCommand input)
        {
            Gem = (GemType)input.arguments[0];
        }

        public override void Save(Script.MainScript.ScriptCommand output)
        {
            output.arguments = new List<uint>() { (uint)Gem };
        }
    }
}
