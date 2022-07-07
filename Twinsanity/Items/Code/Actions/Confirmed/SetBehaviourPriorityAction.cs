using System;
using System.Collections.Generic;

namespace Twinsanity.Actions
{
    [ActionID(DefaultEnums.CommandID.SetBehaviourPriority)]
    public class SetBehaviourPriority : ScriptAction
    {
        public byte Priority { get; set; }

        public SetBehaviourPriority()
        {
            Priority = 50;
        }

        public override void Load(Script.MainScript.ScriptCommand input)
        {
            Priority = (byte)input.arguments[0];
        }

        public override void Save(Script.MainScript.ScriptCommand output)
        {
            output.arguments = new List<uint>() { 0xCDCDCD00 + Priority };
        }

        public override string ToString()
        {
            return $"SetBehaviourPriority {Priority}";
        }
    }
}
