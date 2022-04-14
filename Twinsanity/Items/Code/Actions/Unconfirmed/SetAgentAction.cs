using System;
using System.Collections.Generic;

namespace Twinsanity.Actions
{
    //[ActionID(DefaultEnums.CommandID.SetAgent)]
    public class SetAgentAction : ScriptAction
    {
        public FlagVal Visible { get; set; }
        public FlagVal Active { get; set; }
        public FlagVal Tangible { get; set; }
        public FlagVal Shadow { get; set; }
        public FlagVal Collidable { get; set; }
        public FlagVal Clamping { get; set; }
        public FlagVal Harmful { get; set; }
        public FlagVal UnkFlag1 { get; set; }
        public FlagVal UnkFlag2 { get; set; }
        public FlagVal UnkFlag3 { get; set; }

        public enum FlagVal
        {
            NoChange = 0,
            Off,
            On,
        }

        [Flags]
        public enum AgentFlags
        {
            Active = 1,
            Visible = 2, // ok
            Collidable = 4, // ok
            Tangible = 8, // ok
            Shadow = 16, // ok
            Clamping = 32,
            Harmful = 64,
            UnkFlag1 = 128,
            UnkFlag2 = 256,
            UnkFlag3 = 512,  // GENERIC_CRATE_EXPLODE
        }

        public override void Load(Script.MainScript.ScriptCommand input)
        {
            ushort FlagsWatch = (ushort)(input.arguments[0]);
            ushort FlagsChange = (ushort)(input.arguments[0] / 65536);
            //Console.WriteLine("f " + FlagsWatch + " " + FlagsChange);
            AgentFlags AFlags1 = (AgentFlags)FlagsWatch;
            AgentFlags AFlags2 = (AgentFlags)FlagsChange;

            Visible = AFlags1.HasFlag(AgentFlags.Visible) ? (AFlags2.HasFlag(AgentFlags.Visible) ? FlagVal.On : FlagVal.Off) : FlagVal.NoChange;
            Active = AFlags1.HasFlag(AgentFlags.Active) ? (AFlags2.HasFlag(AgentFlags.Active) ? FlagVal.On : FlagVal.Off) : FlagVal.NoChange;
            Tangible = AFlags1.HasFlag(AgentFlags.Tangible) ? (AFlags2.HasFlag(AgentFlags.Tangible) ? FlagVal.On : FlagVal.Off) : FlagVal.NoChange;
            Shadow = AFlags1.HasFlag(AgentFlags.Shadow) ? (AFlags2.HasFlag(AgentFlags.Shadow) ? FlagVal.On : FlagVal.Off) : FlagVal.NoChange;
            Collidable = AFlags1.HasFlag(AgentFlags.Collidable) ? (AFlags2.HasFlag(AgentFlags.Collidable) ? FlagVal.On : FlagVal.Off) : FlagVal.NoChange;
            Clamping = AFlags1.HasFlag(AgentFlags.Clamping) ? (AFlags2.HasFlag(AgentFlags.Clamping) ? FlagVal.On : FlagVal.Off) : FlagVal.NoChange;
            Harmful = AFlags1.HasFlag(AgentFlags.Harmful) ? (AFlags2.HasFlag(AgentFlags.Harmful) ? FlagVal.On : FlagVal.Off) : FlagVal.NoChange;
            UnkFlag1 = AFlags1.HasFlag(AgentFlags.UnkFlag1) ? (AFlags2.HasFlag(AgentFlags.UnkFlag1) ? FlagVal.On : FlagVal.Off) : FlagVal.NoChange;
            UnkFlag2 = AFlags1.HasFlag(AgentFlags.UnkFlag2) ? (AFlags2.HasFlag(AgentFlags.UnkFlag2) ? FlagVal.On : FlagVal.Off) : FlagVal.NoChange;
            UnkFlag3 = AFlags1.HasFlag(AgentFlags.UnkFlag3) ? (AFlags2.HasFlag(AgentFlags.UnkFlag3) ? FlagVal.On : FlagVal.Off) : FlagVal.NoChange;
        }

        public override void Save(Script.MainScript.ScriptCommand output)
        {
            throw new NotImplementedException();
        }
    }
}
