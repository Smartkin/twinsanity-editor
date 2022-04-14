using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace Twinsanity.Actions
{
    public class ActionID : Attribute
    {
        public List<DefaultEnums.CommandID> Types { get; set; }

        public ActionID(params DefaultEnums.CommandID[] type)
        {
            Types = type.ToList();
        }
    }

    public abstract class ScriptAction
    {

        public ScriptAction() { }

        public abstract void Load(Script.MainScript.ScriptCommand input);

        public abstract void Save(Script.MainScript.ScriptCommand output);

        public static Dictionary<DefaultEnums.CommandID, Type> SupportedTypes;
        public static List<int> ArglessTypes;

        public static void GetSupported()
        {
            SupportedTypes = new Dictionary<DefaultEnums.CommandID, Type>();

            Assembly assembly = Assembly.GetAssembly(typeof(ActionID));
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsAbstract || !typeof(ScriptAction).IsAssignableFrom(type)) // only get non-abstract modders
                    continue;

                ActionID tName = (ActionID)type.GetCustomAttribute(typeof(ActionID), false);

                if (tName == null)
                    continue;

                for (int i = 0; i < tName.Types.Count; i++)
                {
                    SupportedTypes.Add(tName.Types[i], type);
                }
            }
        }
        public static void SetupVersion(int ver)
        {
            ArglessTypes = new List<int>();
            for (int i = 0; i < Script.MainScript.ScriptCommand.ScriptCommandTableSize; i++)
            {
                if (Script.MainScript.ScriptCommand.GetCommandSize(i, ver) == 0x0C)
                {
                    ArglessTypes.Add(i);
                }
            }
        }
    }
}
