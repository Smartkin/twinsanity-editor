using Twinsanity;
using System.Collections.Generic;

namespace TwinsaityEditor
{
    public class SectionController : Controller
    {
        private TwinsSection data;

        public SectionController(TwinsSection item)
        {
            data = item;
            if (item.Type != SectionType.Texture && item.Type != SectionType.TextureX
                && item.Type != SectionType.Material && item.Type != SectionType.Mesh
                && item.Type != SectionType.MeshX && item.Type != SectionType.Model
                && item.Type != SectionType.ArmatureModel && item.Type != SectionType.ActorModel
                && item.Type != SectionType.StaticModel && item.Type != SectionType.SpecialModel
                && item.Type != SectionType.Skybox)
            {
                AddMenu("Re-order by ID (asc.)", Menu_ReOrderByID_Asc);
                if (item.Type == SectionType.ObjectInstance)
                    AddMenu("Re-ID by order", Menu_ReIDByOrder);
            }
            else
            {
                AddMenu("Re-order by ID (desc.)", Menu_ReOrderByID_Desc);
            }
        }

        public override string GetName()
        {
            return data.Type + " Section [ID " + data.ID + "]";
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

        private void Menu_ReOrderByID_Desc()
        {
            Node.TreeView.BeginUpdate();
            while (Node.Nodes.Count > 0)
                DisposeNode(Node.Nodes[0]);
            SortedDictionary<uint, TwinsItem> sdic = new SortedDictionary<uint, TwinsItem>(new Utils.DescendingComparer<uint>());
            foreach (var i in data.SecInfo.Records)
                sdic.Add(i.Key, i.Value);
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
