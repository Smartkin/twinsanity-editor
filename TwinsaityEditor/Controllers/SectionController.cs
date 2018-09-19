using Twinsanity;
using System.Collections.Generic;

namespace TwinsaityEditor
{
    public class SectionController : Controller
    {
        private TwinsSection data;

        public SectionController(TwinsSection item)
        {
            Toolbar = ToolbarFlags.Search | ToolbarFlags.Add | ToolbarFlags.Create | ToolbarFlags.Export;
            data = item;
            if (item.Type == SectionType.ObjectInstance)
            {
                AddMenu("Re-order by ID (asc.)", Menu_ReOrderByID_Asc);
                AddMenu("Re-ID by order", Menu_ReIDByOrder);
            }
        }

        public override string GetName()
        {
            return data.Type + " Section [ID: " + data.ID + "]";
        }

        public override void GenText()
        {
            TextPrev = new string[3];
            TextPrev[0] = "ID: " + data.ID;
            TextPrev[1] = "Offset: " + data.Offset + " Size: " + data.Size;
            TextPrev[2] = "ContentSize: " + data.ContentSize + " Element Count: " + data.SecInfo.Records.Count;
        }

        private void Menu_ReOrderByID_Asc()
        {
            Node.TreeView.BeginUpdate();
            while (Node.Nodes.Count > 0)
                DisposeNode(Node.Nodes[0]);
            SortedDictionary<uint, TwinsItem> sdic = new SortedDictionary<uint, TwinsItem>(data.SecInfo.Records);
            data.SecInfo.Records.Clear();
            foreach (var i in sdic)
            {
                data.SecInfo.Records.Add(i.Key, i.Value);
                ((MainForm)Node.TreeView.FindForm()).GenTreeNode(i.Value, this);
            }
            Node.TreeView.EndUpdate();
        }

        private void Menu_ReIDByOrder()
        {
            Node.TreeView.BeginUpdate();
            while (Node.Nodes.Count > 0)
                DisposeNode(Node.Nodes[0]);
            Dictionary<uint, TwinsItem> dic = new Dictionary<uint, TwinsItem>(data.SecInfo.Records);
            data.SecInfo.Records.Clear();
            uint id = 0;
            foreach(var i in dic)
            {
                i.Value.ID = id;
                data.SecInfo.Records.Add(id++, i.Value);
                ((MainForm)Node.TreeView.FindForm()).GenTreeNode(i.Value, this);
            }
            Node.TreeView.EndUpdate();
        }
    }
}
