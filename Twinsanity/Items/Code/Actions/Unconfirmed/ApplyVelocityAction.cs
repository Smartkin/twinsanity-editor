using System;
using System.Collections.Generic;

namespace Twinsanity.Actions
{
    //[ActionID(DefaultEnums.CommandID.ApplyVelocity)]
    public class ApplyVelocityAction : ScriptAction
    {
        public float Gravity { get; set; }
        public float Velocity_X { get; set; }
        public float Velocity_Y { get; set; }
        public float Velocity_Z { get; set; }
        public float Distance_X { get; set; }
        public float Distance_Y { get; set; }
        public float Distance_Z { get; set; }
        public float Apply_X { get; set; }
        public float Apply_Y { get; set; }
        public float Apply_Z { get; set; }
        public byte Agent { get; set; }

        public override void Load(Script.MainScript.ScriptCommand input)
        {
            byte[] bytes = BitConverter.GetBytes(input.arguments[0]);
            Gravity = BitConverter.ToSingle(bytes, 0);
            bytes = BitConverter.GetBytes(input.arguments[1]);
            Velocity_X = BitConverter.ToSingle(bytes, 0);
            bytes = BitConverter.GetBytes(input.arguments[2]);
            Velocity_Y = BitConverter.ToSingle(bytes, 0);
            bytes = BitConverter.GetBytes(input.arguments[3]);
            Velocity_Z = BitConverter.ToSingle(bytes, 0);
            bytes = BitConverter.GetBytes(input.arguments[4]);
            Distance_X = BitConverter.ToSingle(bytes, 0);
            bytes = BitConverter.GetBytes(input.arguments[5]);
            Distance_Y = BitConverter.ToSingle(bytes, 0);
            bytes = BitConverter.GetBytes(input.arguments[6]);
            Distance_Z = BitConverter.ToSingle(bytes, 0);

            bytes = BitConverter.GetBytes(input.arguments[8]);
            Apply_X = BitConverter.ToSingle(bytes, 0);
            bytes = BitConverter.GetBytes(input.arguments[9]);
            Apply_Y = BitConverter.ToSingle(bytes, 0);
            bytes = BitConverter.GetBytes(input.arguments[10]);
            Apply_Z = BitConverter.ToSingle(bytes, 0);
        }

        public override void Save(Script.MainScript.ScriptCommand output)
        {
            throw new NotImplementedException();
        }
    }
}
