using System;
using System.Collections.Generic;

namespace Twinsanity.Actions
{
    //[ActionID(DefaultEnums.CommandID.CA_SetPickup)]
    public class SetPickupAction : ScriptAction
    {
        public bool WumpaHover { get; set; } = true;
        public float SuckRange { get; set; } = 3;
        public float SuckPower { get; set; } = 80;
        public float FlySpeed { get; set; } = 50;

        public override void Load(Script.MainScript.ScriptCommand input)
        {
            byte[] bytes = BitConverter.GetBytes(input.arguments[0]);
            SuckRange = BitConverter.ToSingle(bytes, 0);
            bytes = BitConverter.GetBytes(input.arguments[1]);
            SuckPower = BitConverter.ToSingle(bytes, 0);
            bytes = BitConverter.GetBytes(input.arguments[2]);
            FlySpeed = BitConverter.ToSingle(bytes, 0);
            // arg 4 - 0xCDCDCC40 wumpahover
        }

        public override void Save(Script.MainScript.ScriptCommand output)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"SetPickup";
        }
    }
}
