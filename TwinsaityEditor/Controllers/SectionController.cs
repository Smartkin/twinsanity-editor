using Twinsanity;
using System.Collections.Generic;

namespace TwinsaityEditor
{
    public class SectionController : Controller
    {
        public TwinsSection Data { get; set; }

        public SectionController(TwinsSection item)
        {
            Data = item;
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

        protected override string GetName()
        {
            return Data.Type + " Section [ID " + Data.ID + "]";
        }

        protected override void GenText()
        {
            TextPrev = new string[3];
            TextPrev[0] = "ID: " + Data.ID;
            TextPrev[1] = "Offset: " + Data.Offset + " Size: " + Data.Size;
            TextPrev[2] = "ContentSize: " + Data.ContentSize + " Element Count: " + Data.Records.Count;
        }

        private void Menu_ReOrderByID_Asc()
        {
            Node.TreeView.BeginUpdate();
            while (Node.Nodes.Count > 0)
                DisposeNode(Node.Nodes[0]);
            SortedDictionary<uint, int> sdic = new SortedDictionary<uint, int>(Data.RecordIDs);
            List<TwinsItem> slist = new List<TwinsItem>();
            foreach (var i in sdic)
            {
                slist.Add(Data.Records[i.Value]);
                ((MainForm)Node.TreeView.FindForm()).GenTreeNode(Data.Records[i.Value], this);
            }
            Data.Records = slist;
            Node.TreeView.EndUpdate();
            if (Data.Type == SectionType.ObjectInstance)
                RMViewer.InstancesChanged[Data.Parent.ID] = true;
        }

        private void Menu_ReOrderByID_Desc()
        {
            Node.TreeView.BeginUpdate();
            while (Node.Nodes.Count > 0)
                DisposeNode(Node.Nodes[0]);
            SortedDictionary<uint, int> sdic = new SortedDictionary<uint, int>(new Utils.DescendingComparer<uint>());
            foreach (var i in Data.RecordIDs)
                sdic.Add(i.Key, i.Value);
            List<TwinsItem> slist = new List<TwinsItem>();
            foreach (var i in sdic)
            {
                slist.Add(Data.Records[i.Value]);
                ((MainForm)Node.TreeView.FindForm()).GenTreeNode(Data.Records[i.Value], this);
            }
            Data.Records = slist;
            Node.TreeView.EndUpdate();
            if (Data.Type == SectionType.ObjectInstance)
                RMViewer.InstancesChanged[Data.Parent.ID] = true;
        }

        private void Menu_ReIDByOrder()
        {
            Node.TreeView.BeginUpdate();
            while (Node.Nodes.Count > 0)
                DisposeNode(Node.Nodes[0]);
            Data.RecordIDs.Clear();
            for (int i = 0; i < Data.Records.Count; ++i)
            {
                Data.Records[i].ID = (uint)i;
                Data.RecordIDs.Add((uint)i, i);
                ((MainForm)Node.TreeView.FindForm()).GenTreeNode(Data.Records[i], this);
            }
            Node.TreeView.EndUpdate();
            if (Data.Type == SectionType.ObjectInstance)
                RMViewer.InstancesChanged[Data.Parent.ID] = true;
        }
    }
}
