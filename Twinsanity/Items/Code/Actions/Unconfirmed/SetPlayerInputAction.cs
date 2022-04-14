using System;
using System.Collections.Generic;

namespace Twinsanity.Actions
{
    //[ActionID(DefaultEnums.CommandID.SetPlayerInput)]
    public class SetPlayerInputAction : ScriptAction
    {
        public FlagVal Input { get; set; }
        public FlagVal Rotation { get; set; }
        public FlagVal ForwardMovement { get; set; }
        public FlagVal LateralMovement { get; set; }
        public FlagVal JumpKey { get; set; }
        public FlagVal CrouchKey { get; set; }
        public FlagVal SpinKey { get; set; }
        public FlagVal FoofieKey { get; set; }
        public FlagVal AffectedByGravity { get; set; }
        //public FlagVal UnkFlag { get; set; }

        public enum FlagVal
        {
            NoChange = 0,
            Off,
            On,
        }

        [Flags]
        public enum AgentFlags
        {
            Input = 1,
            Rotation = 2,
            ForwardMovement = 4,
            LateralMovement = 8,
            JumpKey = 16,
            CrouchKey = 32,
            SpinKey = 64,
            FoofieKey = 128,
            AffectedByGravity = 256,
        }

        public override void Load(Script.MainScript.ScriptCommand input)
        {
            ushort FlagsWatch = (ushort)(input.arguments[0]);
            ushort FlagsChange = (ushort)(input.arguments[0] / 65536);
            //Console.WriteLine("f " + FlagsWatch + " " + FlagsChange);
            AgentFlags AFlags1 = (AgentFlags)FlagsWatch;
            AgentFlags AFlags2 = (AgentFlags)FlagsChange;

            Input = AFlags1.HasFlag(AgentFlags.Input) ? (AFlags2.HasFlag(AgentFlags.Input) ? FlagVal.On : FlagVal.Off) : FlagVal.NoChange;
            Rotation = AFlags1.HasFlag(AgentFlags.Rotation) ? (AFlags2.HasFlag(AgentFlags.Rotation) ? FlagVal.On : FlagVal.Off) : FlagVal.NoChange;
            ForwardMovement = AFlags1.HasFlag(AgentFlags.ForwardMovement) ? (AFlags2.HasFlag(AgentFlags.ForwardMovement) ? FlagVal.On : FlagVal.Off) : FlagVal.NoChange;
            LateralMovement = AFlags1.HasFlag(AgentFlags.LateralMovement) ? (AFlags2.HasFlag(AgentFlags.LateralMovement) ? FlagVal.On : FlagVal.Off) : FlagVal.NoChange;
            JumpKey = AFlags1.HasFlag(AgentFlags.JumpKey) ? (AFlags2.HasFlag(AgentFlags.JumpKey) ? FlagVal.On : FlagVal.Off) : FlagVal.NoChange;
            CrouchKey = AFlags1.HasFlag(AgentFlags.CrouchKey) ? (AFlags2.HasFlag(AgentFlags.CrouchKey) ? FlagVal.On : FlagVal.Off) : FlagVal.NoChange;
            SpinKey = AFlags1.HasFlag(AgentFlags.SpinKey) ? (AFlags2.HasFlag(AgentFlags.SpinKey) ? FlagVal.On : FlagVal.Off) : FlagVal.NoChange;
            FoofieKey = AFlags1.HasFlag(AgentFlags.FoofieKey) ? (AFlags2.HasFlag(AgentFlags.FoofieKey) ? FlagVal.On : FlagVal.Off) : FlagVal.NoChange;
            AffectedByGravity = AFlags1.HasFlag(AgentFlags.AffectedByGravity) ? (AFlags2.HasFlag(AgentFlags.AffectedByGravity) ? FlagVal.On : FlagVal.Off) : FlagVal.NoChange;
        }

        public override void Save(Script.MainScript.ScriptCommand output)
        {
            throw new NotImplementedException();
        }
    }
}
