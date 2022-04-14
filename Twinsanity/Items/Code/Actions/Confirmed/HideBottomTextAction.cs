using System;
using System.Collections.Generic;

namespace Twinsanity.Actions
{
    [ActionID(DefaultEnums.CommandID.HideBottomText)]
    public class HideBottomTextAction : ScriptAction
    {
        public float FadeDuration { get; set; } = 1f;

        public override void Load(Script.MainScript.ScriptCommand input)
        {
            byte[] bytes = BitConverter.GetBytes(input.arguments[0]);
            FadeDuration = BitConverter.ToSingle(bytes, 0);
        }

        public override void Save(Script.MainScript.ScriptCommand output)
        {
            byte[] bytes = BitConverter.GetBytes(FadeDuration);
            output.arguments = new List<uint>() { BitConverter.ToUInt32(bytes, 0) };
        }
    }
}
