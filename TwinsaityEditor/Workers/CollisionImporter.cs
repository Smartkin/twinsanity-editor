using System.Windows.Forms;
using System.Collections.Generic;
using Twinsanity;
using System.IO;
using System;

namespace TwinsaityEditor
{
    public partial class CollisionImporter : Form
    {
        private ColDataController controller;
        private List<ColModel> models = new List<ColModel>();

        private int vertexCount, triCount, groupCount, triggerCount;

        public CollisionImporter(ColDataController c)
        {
            controller = c;
            InitializeComponent();
            comboBox1.TextChanged += comboBox1_TextChanged;
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            int s;
            if (int.TryParse(comboBox1.Text.Split(' ')[0], out s))
            {
                ColModel model = models[listBox1.SelectedIndex];
                model.surface = s;
                models[listBox1.SelectedIndex] = model;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            vertexCount -= models[listBox1.SelectedIndex].vtx.Count;
            triCount -= models[listBox1.SelectedIndex].tris.Count;
            groupCount -= models[listBox1.SelectedIndex].groups.Count;
            models.RemoveAt(listBox1.SelectedIndex);
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            if (listBox1.Items.Count == 0)
                button3.Enabled = label4.Enabled = false;
            else
                UpdateMainLabel();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Wavefront OBJ (*.obj)|*.obj", Multiselect = true };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < ofd.FileNames.Length; ++i)
                {
                    ColModel model = new ColModel { vtx = new List<Pos>(), tris = new List<ColData.ColTri>(), groups = new List<ColData.GroupInfo>() };
                    StreamReader reader = new StreamReader(ofd.FileNames[i]);
                    while(!reader.EndOfStream)
                    {
                        string[] str = reader.ReadLine().Split(' ');
                        switch (str[0])
                        {
                            case "v":
                                model.vtx.Add(new Pos(-float.Parse(str[1].Replace(".", ".")), float.Parse(str[2].Replace(".", ".")), float.Parse(str[3].Replace(".", ".")), 1));
                                break;
                            case "f":
                                model.tris.Add(new ColData.ColTri { Vert1 = int.Parse(str[1].Split('/')[0]) - 1, Vert2 = int.Parse(str[2].Split('/')[0]) - 1, Vert3 = int.Parse(str[3].Split('/')[0]) - 1 });
                                {
                                    ColData.GroupInfo grp = model.groups[model.groups.Count - 1];
                                    ++grp.Size;
                                    model.groups[model.groups.Count - 1] = grp;
                                }
                                break;
                            case "o":
                                model.groups.Add(new ColData.GroupInfo { Offset = (uint)model.tris.Count });
                                break;
                        }
                    }
                    reader.Close();
                    listBox1.Items.Add(ofd.FileNames[i]);
                    models.Add(model);
                    vertexCount += model.vtx.Count;
                    triCount += model.tris.Count;
                    groupCount += model.groups.Count;
                }
                button3.Enabled = label4.Enabled = true;
                UpdateMainLabel();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //import
            MainForm.CloseRMViewer();
            controller.Data.Vertices.Clear();
            controller.Data.Tris.Clear();
            controller.Data.Groups.Clear();
            int vertex_off = 0, poly_off = 0;
            for (int i = 0; i < models.Count; ++i)
            {
                controller.Data.Vertices.AddRange(models[i].vtx);
                for (int j = 0; j < models[i].tris.Count; ++j)
                {
                    controller.Data.Tris.Add(new ColData.ColTri { Vert1 = models[i].tris[j].Vert1 + vertex_off,
                        Vert2 = models[i].tris[j].Vert2 + vertex_off,
                        Vert3 = models[i].tris[j].Vert3 + vertex_off,
                        Surface = models[i].surface });
                }
                vertex_off += models[i].vtx.Count;
                for (int j = 0; j < models[i].groups.Count; ++j)
                {
                    controller.Data.Groups.Add(new ColData.GroupInfo
                    {
                        Offset = (uint)(models[i].groups[j].Offset + poly_off),
                        Size = models[i].groups[j].Size
                    });
                }
                poly_off += models[i].tris.Count;
            }
            ColData.Trigger[] triggers = new ColData.Trigger[controller.Data.Groups.Count * 2];
            TreeView TriggerTree = new TreeView();
            TriggerTree.Nodes.Add("E", "0");
            int x = (int)Math.Truncate(Math.Log(controller.Data.Groups.Count, 2));
            for (int i = 0; i < x; ++i)
                ExpandLevel(TriggerTree.Nodes[0]);
            ExpandEngings(TriggerTree, (uint)(controller.Data.Groups.Count - Math.Pow(2, x)));
            int temp = 1;
            CalcIDs(TriggerTree.Nodes[0], ref temp);
            Tree2Trigger(TriggerTree.Nodes[0], triggers);
            TriggerRecalculate(triggers, controller.Data.Groups, controller.Data.Tris, controller.Data.Vertices, 0);
            controller.Data.Triggers = new List<ColData.Trigger>(triggers);
            controller.UpdateText();
            Close();
        }

        private void DoubleExpand(TreeNode Node)
        {
            Node.Nodes.Add("E", "Node");
            Node.Nodes.Add("E", "Node");
        }

        private void ExpandLevel(TreeNode Root)
        {
            TreeNode[] Nodes = Root.Nodes.Find("E", true);
            if (Root.Name == "E")
            {
                Array.Resize(ref Nodes, Nodes.Length + 1);
                Nodes[Nodes.Length - 1] = Root;
            }
            for (int i = 0; i <= Nodes.Length - 1; i++)
            {
                Nodes[i].Name = "P";
                DoubleExpand(Nodes[i]);
            }
        }
        private void ExpandEngings(TreeView Tree, uint d)
        {
            TreeNode[] Nodes = Tree.Nodes.Find("E", true);
            for (int i = 0; i < d; ++i)
            {
                Nodes[i].Name = "P";
                DoubleExpand(Nodes[i]);
            }
        }

        private void CalcIDs(TreeNode Node, ref int temp)
        {
            if (Node.Name == "P")
            {
                uint x = (uint)Node.Nodes[0].GetNodeCount(true);
                Node.Nodes[0].Text = (int.Parse(Node.Text) + 1).ToString();
                Node.Nodes[1].Text = (int.Parse(Node.Nodes[0].Text) + x + 1).ToString();
                CalcIDs(Node.Nodes[0], ref temp);
                CalcIDs(Node.Nodes[1], ref temp);
            }
            else if (Node.Name == "E")
            {
                Node.Nodes.Add("Ptr", temp.ToString());
                temp += 1;
            }
        }

        private void Tree2Trigger(TreeNode Node, ColData.Trigger[] Triggers)
        {
            if (Node.Name == "P")
            {
                Triggers[int.Parse(Node.Text)].Flag1 = int.Parse(Node.Nodes[0].Text);
                Triggers[int.Parse(Node.Text)].Flag2 = int.Parse(Node.Nodes[1].Text);
                Tree2Trigger(Node.Nodes[0], Triggers);
                Tree2Trigger(Node.Nodes[1], Triggers);
            }
            else if (Node.Name == "E")
            {
                Triggers[int.Parse(Node.Text)].Flag1 = -int.Parse(Node.Nodes[0].Text);
                Triggers[int.Parse(Node.Text)].Flag2 = -int.Parse(Node.Nodes[0].Text);
            }
        }

        private void TriggerRecalculate(ColData.Trigger[] Triggers, List<ColData.GroupInfo> Groups, List<ColData.ColTri> Indexes, List<Pos> Vertexes, int index)
        {
            if (Triggers[index].Flag1 >= 0)
            {
                TriggerRecalculate(Triggers, Groups, Indexes, Vertexes, Triggers[index].Flag1);
                TriggerRecalculate(Triggers, Groups, Indexes, Vertexes, Triggers[index].Flag2);
                Triggers[index].X1 = Math.Min(Triggers[Triggers[index].Flag1].X1, Triggers[Triggers[index].Flag2].X1);
                Triggers[index].Y1 = Math.Min(Triggers[Triggers[index].Flag1].Y1, Triggers[Triggers[index].Flag2].Y1);
                Triggers[index].Z1 = Math.Min(Triggers[Triggers[index].Flag1].Z1, Triggers[Triggers[index].Flag2].Z1);
                Triggers[index].X2 = Math.Max(Triggers[Triggers[index].Flag1].X2, Triggers[Triggers[index].Flag2].X2);
                Triggers[index].Y2 = Math.Max(Triggers[Triggers[index].Flag1].Y2, Triggers[Triggers[index].Flag2].Y2);
                Triggers[index].Z2 = Math.Max(Triggers[Triggers[index].Flag1].Z2, Triggers[Triggers[index].Flag2].Z2);
            }
            else
            {
                float x1 = 0f, x2 = 0f, y1 = 0f, y2 = 0f, z1 = 0f, z2 = 0f;
                for (int i = (int)Groups[-Triggers[index].Flag1 - 1].Offset; i < Groups[-Triggers[index].Flag1 - 1].Offset + Groups[-Triggers[index].Flag1 - 1].Size; ++i)
                {
                    Pos a = Vertexes[Indexes[i].Vert1], b = Vertexes[Indexes[i].Vert2], c = Vertexes[Indexes[i].Vert3];
                    if (x1 == 0)
                        x1 = Math.Min(a.X, Math.Min(b.X, c.X));
                    else
                        x1 = Math.Min(x1, Math.Min(a.X, Math.Min(b.X, c.X)));
                    if (y1 == 0)
                        y1 = Math.Min(a.Y, Math.Min(b.Y, c.Y));
                    else
                        y1 = Math.Min(y1, Math.Min(a.Y, Math.Min(b.Y, c.Y)));
                    if (z1 == 0)
                        z1 = Math.Min(a.Z, Math.Min(b.Z, c.Z));
                    else
                        z1 = Math.Min(z1, Math.Min(a.Z, Math.Min(b.Z, c.Z)));
                    if (x2 == 0)
                        x2 = Math.Max(a.X, Math.Max(b.X, c.X));
                    else
                        x2 = Math.Max(x2, Math.Max(a.X, Math.Min(b.X, c.X)));
                    if (y2 == 0)
                        y2 = Math.Max(a.Y, Math.Max(b.Y, c.Y));
                    else
                        y2 = Math.Max(y2, Math.Max(a.Y, Math.Min(b.Y, c.Y)));
                    if (z2 == 0)
                        z2 = Math.Max(a.Z, Math.Max(b.Z, c.Z));
                    else
                        z2 = Math.Max(z2, Math.Max(a.Z, Math.Min(b.Z, c.Z)));
                }
                Triggers[index].X1 = x1;
                Triggers[index].Y1 = y1;
                Triggers[index].Z1 = z1;
                Triggers[index].X2 = x2;
                Triggers[index].Y2 = y2;
                Triggers[index].Z2 = z2;
            }
        }

        private struct ColModel
        {
            public List<Pos> vtx;
            public List<ColData.ColTri> tris;
            public List<ColData.GroupInfo> groups;
            public int surface;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(groupBox1.Enabled = button2.Enabled = (listBox1.SelectedIndex != -1))) return;

            if (models[listBox1.SelectedIndex].surface >= comboBox1.Items.Count)
                comboBox1.Text = models[listBox1.SelectedIndex].surface.ToString();
            else
                comboBox1.Text = comboBox1.GetItemText(models[listBox1.SelectedIndex].surface);
            label3.Text = $"Vertex Count: {models[listBox1.SelectedIndex].vtx.Count} Triangle Count: {models[listBox1.SelectedIndex].tris.Count}"
                + Environment.NewLine + $"Group Count: {models[listBox1.SelectedIndex].groups.Count}";
            UpdateMainLabel();
        }

        private void UpdateMainLabel()
        {
            label4.Text = $"Trigger Count: {groupCount * 2}" + Environment.NewLine +
                $"Group Count: {groupCount}" + Environment.NewLine +
                $"Triangle Count: {triCount}" + Environment.NewLine +
                $"Vertex Count: {vertexCount}";
        }
    }
}
