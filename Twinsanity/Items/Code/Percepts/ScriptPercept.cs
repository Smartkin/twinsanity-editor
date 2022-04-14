using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace Twinsanity.Actions
{
    public class PerceptID : Attribute
    {
        public DefaultEnums.ConditionID Type { get; set; }

        public PerceptID(DefaultEnums.ConditionID type)
        {
            Type = type;
        }
    }

    public abstract class ScriptPercept
    {
        public enum PerceptArgument
        {
            ExitPoint = 0, // Point of interest defined in GameObject
            Actor,
            Subtype,
            Message,
            IntegerPlus,
            Surface,
            Counter,
            SoftFlag,
        }

        public static Dictionary<DefaultEnums.ConditionID, PerceptArgument> Args = new Dictionary<DefaultEnums.ConditionID, PerceptArgument>()
        {
            [DefaultEnums.ConditionID.HeadLookingAtFocus] = PerceptArgument.ExitPoint,
            [DefaultEnums.ConditionID.HeadCanSeeFocus] = PerceptArgument.ExitPoint,
            [DefaultEnums.ConditionID.HeadLookingAtRouteNode] = PerceptArgument.ExitPoint,
            [DefaultEnums.ConditionID.FocusHeadLookingAtMe] = PerceptArgument.ExitPoint,
            [DefaultEnums.ConditionID.FocusHeadCanSeeMe] = PerceptArgument.ExitPoint,
            [DefaultEnums.ConditionID.FocusActorEquals] = PerceptArgument.Actor,
            [DefaultEnums.ConditionID.ActorSubtypeEquals] = PerceptArgument.Subtype,
            [DefaultEnums.ConditionID.GotUserMessageEquals] = PerceptArgument.Message,
            [DefaultEnums.ConditionID.CurrentKey] = PerceptArgument.IntegerPlus,
            [DefaultEnums.ConditionID.TouchingTerrain] = PerceptArgument.Surface,
            [DefaultEnums.ConditionID.GotAttachmentOnExit] = PerceptArgument.ExitPoint,
            [DefaultEnums.ConditionID.CounterValue] = PerceptArgument.Counter,
            [DefaultEnums.ConditionID.CounterValueEqualsThreshold] = PerceptArgument.Counter,
            [DefaultEnums.ConditionID.SoftFlagSet] = PerceptArgument.SoftFlag,
            [DefaultEnums.ConditionID.MeToKeySqrDist] = PerceptArgument.IntegerPlus,
            [DefaultEnums.ConditionID.SpeedTowardsKey] = PerceptArgument.IntegerPlus,

            [DefaultEnums.ConditionID.HeadLookingAtPlayer] = PerceptArgument.ExitPoint,
            [DefaultEnums.ConditionID.HeadCanSeePlayer] = PerceptArgument.ExitPoint,
            [DefaultEnums.ConditionID.PlayerHeadLookingAtMe] = PerceptArgument.ExitPoint,
            [DefaultEnums.ConditionID.PlayerHeadCanSeeMe] = PerceptArgument.ExitPoint,
            [DefaultEnums.ConditionID.HeadCanSeePlayerUnblocked] = PerceptArgument.ExitPoint,
        };

        public abstract void Load(Script.MainScript.ScriptCondition input);

        public abstract void Save(Script.MainScript.ScriptCondition output);

        public static Dictionary<DefaultEnums.ConditionID, Type> SupportedTypes;

        public static void GetSupported()
        {
            SupportedTypes = new Dictionary<DefaultEnums.ConditionID, Type>();

            Assembly assembly = Assembly.GetAssembly(typeof(PerceptID));
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsAbstract || !typeof(ScriptPercept).IsAssignableFrom(type)) // only get non-abstract modders
                    continue;

                PerceptID tName = (PerceptID)type.GetCustomAttribute(typeof(PerceptID), false);

                if (tName == null)
                    continue;

                SupportedTypes.Add(tName.Type, type);
            }
        }
    }
}
