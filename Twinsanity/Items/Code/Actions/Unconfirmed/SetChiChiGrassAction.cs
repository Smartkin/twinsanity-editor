using System;
using System.Collections.Generic;

namespace Twinsanity.Actions
{
    //[ActionID(DefaultEnums.CommandID.SetChiChiGrass)]
    public class SetChiChiGrassAction : ScriptAction
    {
        public FlagVal Selectable { get; set; }

        public enum FlagVal
        {
            NoChange = 0,
            Off,
            On,
        }

        [Flags]
        public enum AgentFlags
        {
            Selectable = 1,
        }

        public override void Load(Script.MainScript.ScriptCommand input)
        {
            ushort FlagsWatch = (ushort)(input.arguments[0]);
            ushort FlagsChange = (ushort)(input.arguments[0] / 65536);
            AgentFlags AFlags1 = (AgentFlags)FlagsWatch;
            AgentFlags AFlags2 = (AgentFlags)FlagsChange;

            Selectable = AFlags1.HasFlag(AgentFlags.Selectable) ? (AFlags2.HasFlag(AgentFlags.Selectable) ? FlagVal.On : FlagVal.Off) : FlagVal.NoChange;
        }

        public override void Save(Script.MainScript.ScriptCommand output)
        {
            throw new NotImplementedException();
        }
    }
}
