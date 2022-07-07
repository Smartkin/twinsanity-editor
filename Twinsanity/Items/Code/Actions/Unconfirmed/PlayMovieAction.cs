using System;
using System.Collections.Generic;

namespace Twinsanity.Actions
{
    [ActionID(DefaultEnums.CommandID.PlayMovie)]
    public class PlayMovieAction : ScriptAction
    {
        public uint Movie { get; set; } = 1; // change to enum
        public float FadeDuration { get; set; } = 1f;

        public override void Load(Script.MainScript.ScriptCommand input)
        {
            Movie = input.arguments[0];
            byte[] bytes = BitConverter.GetBytes(input.arguments[1]);
            FadeDuration = BitConverter.ToSingle(bytes, 0);
        }

        public override void Save(Script.MainScript.ScriptCommand output)
        {
            byte[] bytes = BitConverter.GetBytes(FadeDuration);
            output.arguments = new List<uint>() { Movie, BitConverter.ToUInt32(bytes, 0) };
        }

        public override string ToString()
        {
            return $"PlayMovie {Movie}, Fade {FadeDuration}";
        }
    }
}
