using System.Collections.Generic;
using Twinsanity;

namespace TwinsaityEditor
{
    public class ScriptController : ItemController
    {
        public new Script Data { get; set; }

        public ScriptController(MainForm topform, Script item) : base (topform, item)
        {
            Data = item;
            AddMenu("Open editor", Menu_OpenEditor);
        }

        protected override string GetName()
        {
            return $"{Data.Name ?? "Script"} [ID {Data.ID}]";
        }

        protected override void GenText()
        {
            List<string> text = new List<string>();
            text.Add($"ID: {Data.ID} {(Data.Name != null ? $" Name: {Data.Name}" : string.Empty)}");
            text.Add($"Size: {Data.Size}");
            if (Data?.Header != null)
            {
                text.Add($"Participants: {Data.Header.pairs.Count}");
                for (int i = 0; i < Data.Header.pairs.Count; i++)
                {
                    text.Add($"{i}: {Data.Header.pairs[i].AssignType} {Data.Header.pairs[i].AssignStatus} {Data.Header.pairs[i].AssignPreference} {Data.Header.pairs[i].AssignLocality} Object ID {Data.Header.pairs[i].ObjectID} - Script ID {Data.Header.pairs[i].mainScriptIndex - 1}");
                }
            }
            TextPrev = text.ToArray();
        }
        private void Menu_OpenEditor()
        {
            MainFile.OpenEditor(this);
        }
    }
}
