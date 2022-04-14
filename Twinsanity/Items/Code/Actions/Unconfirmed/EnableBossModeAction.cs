using System;
using System.Collections.Generic;

namespace Twinsanity.Actions
{
    //[ActionID(DefaultEnums.CommandID.EnableBossMode)]
    public class EnableBossModeAction : ScriptAction
    {
        public uint Health { get; set; } = 24;
        public uint IconSlot { get; set; } = 0x1A; // This should be an OGI ID reference instead
        public float FadeDuration { get; set; } = 1f;

        public override void Load(Script.MainScript.ScriptCommand input)
        {
            IconSlot = input.arguments[0];
            Health = input.arguments[1];
            byte[] bytes = BitConverter.GetBytes(input.arguments[2]);
            FadeDuration = BitConverter.ToSingle(bytes, 0);
        }

        public override void Save(Script.MainScript.ScriptCommand output)
        {
            byte[] bytes = BitConverter.GetBytes(FadeDuration);
            output.arguments = new List<uint>() { IconSlot, Health, BitConverter.ToUInt32(bytes, 0) };
        }
    }
}
