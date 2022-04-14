using System;
using System.Collections.Generic;

namespace Twinsanity.Actions
{
    [ActionID(DefaultEnums.CommandID.NowGoBackCollidable)]
    public class NowGoBackCollidableAction : ScriptAction
    {
        public float Distance { get; set; } = 1f;

        public override void Load(Script.MainScript.ScriptCommand input)
        {
            byte[] bytes = BitConverter.GetBytes(input.arguments[0]);
            Distance = BitConverter.ToSingle(bytes, 0);
        }

        public override void Save(Script.MainScript.ScriptCommand output)
        {
            byte[] bytes = BitConverter.GetBytes(Distance);
            output.arguments = new List<uint>() { BitConverter.ToUInt32(bytes, 0) };
        }
    }
}
