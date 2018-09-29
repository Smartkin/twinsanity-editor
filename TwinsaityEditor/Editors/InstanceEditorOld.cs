using System.Windows.Forms;
using System;

namespace TwinsaityEditor
{
    public partial class InstanceEditor
    {

        private TwinsanityEditorForm twinsanityEditorForm;

        public InstanceEditor(TwinsanityEditorForm TEF)
        {
            twinsanityEditorForm = TEF;
            InitializeComponent();
        }

        private int IISIndex;
        public void UpdateTree(ref Twinsanity.Instances INSTs, int Index)
        {
            InstanceTree.BeginUpdate();
            for (int i = 0; i <= INSTs._Item.Length - 1; i++)
                InstanceTree.Nodes.Add("ID: " + INSTs._Item[i].ID.ToString());
            InstanceTree.EndUpdate();
            IISIndex = Index;
        }
        public void UpdateInstance(int index)
        {
            Twinsanity.Instance INST = (Twinsanity.Instance)twinsanityEditorForm.LevelData.Get_Item(TwinsanityEditorForm.CalculateIndexes(twinsanityEditorForm.TreeView1.Nodes[0].Nodes[IISIndex].Nodes[6].Nodes[index]));
            Twinsanity.GameObjects Objects = (Twinsanity.GameObjects)twinsanityEditorForm.LevelData.Item[1]._Item[0];
            Twinsanity.GameObject GO = null;
            for (int i = 0; i <= Objects._Item.Length - 1; i++)
            {
                if (Objects._Item[i].ID == INST.ObjectID)
                {
                    GO = (Twinsanity.GameObject)Objects._Item[i];
                    break;
                }
            }
            if (!(GO == null))
                this.Text = "Object " + GO.Name + " (ID: " + GO.ID.ToString() + ")";
            else
                this.Text = "Object is not defined";
            InstanceID.Text = INST.ID.ToString();
            ObjectID.Text = INST.ObjectID.ToString();
            InstanceX.Text = INST.X.ToString();
            InstanceY.Text = INST.Y.ToString();
            InstanceZ.Text = INST.Z.ToString();
            RotationX.Value = INST.RX;
            RotationY.Value = INST.RY;
            RotationZ.Value = INST.RZ;
            UpdateRotations();
            Flags.Text = INST.UnkI32.ToString();
            Satan.Text = INST.AfterOID.ToString();
            Some.Items.Clear();
            Floats.Items.Clear();
            Integers.Items.Clear();
            for (int i = 0; i <= INST.UnkI321Number - 1; i++)
                Some.Items.Add(INST.UnkI321[i]);
            for (int i = 0; i <= INST.UnkI322Number - 1; i++)
                Floats.Items.Add(INST.UnkI322[i]);
            for (int i = 0; i <= INST.UnkI323Number - 1; i++)
                Integers.Items.Add(INST.UnkI323[i]);
            Position.Items.Clear();
            Path.Items.Clear();
            Instance.Items.Clear();
            for (int i = 0; i <= INST.Size1 - 1; i++)
                Position.Items.Add(INST.Something1[i]);
            for (int i = 0; i <= INST.Size2 - 1; i++)
                Path.Items.Add(INST.Something2[i]);
            for (int i = 0; i <= INST.Size3 - 1; i++)
                Instance.Items.Add(INST.Something3[i]);
        }
        private void UpdateRotations()
        {
            Label6.Text = "RotationX: " + (RotationX.Value * 360 / (double)65536).ToString("{0.00}");
            Label7.Text = "RotationY: " + (RotationY.Value * 360 / (double)65536).ToString("{0.00}");
            Label8.Text = "RotationZ: " + (RotationZ.Value * 360 / (double)65536).ToString("{0.00}");
            RotXText.Text = RotationX.Value.ToString();
            RotYText.Text = RotationY.Value.ToString();
            RotZText.Text = RotationZ.Value.ToString();
        }

        private void RotationX_Scroll(object sender, EventArgs e)
        {
            UpdateRotations();
        }

        private void RotationY_Scroll(object sender, EventArgs e)
        {
            UpdateRotations();
        }

        private void RotationZ_Scroll(object sender, EventArgs e)
        {
            UpdateRotations();
        }

        private void InstanceTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdateInstance(InstanceTree.SelectedNode.Index);
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            ApplyInstance(InstanceTree.SelectedNode.Index);
        }
        public void ApplyInstance(int index)
        {
            Twinsanity.Instance INST = new Twinsanity.Instance();
            INST.ID = uint.Parse(InstanceID.Text);
            INST.ObjectID = UInt16.Parse(ObjectID.Text);
            INST.X = float.Parse(InstanceX.Text);
            INST.Y = float.Parse(InstanceY.Text);
            INST.Z = float.Parse(InstanceZ.Text);
            INST.RX = (ushort)RotationX.Value;
            INST.RY = (ushort)RotationY.Value;
            INST.RZ = (ushort)RotationZ.Value;
            INST.UnkI32 = uint.Parse(Flags.Text);
            INST.AfterOID = uint.Parse(Satan.Text);
            INST.UnkI321Number = Some.Items.Count;
            INST.UnkI322Number = Floats.Items.Count;
            INST.UnkI323Number = Integers.Items.Count;
            INST.Size1 = Position.Items.Count;
            INST.Size2 = Path.Items.Count;
            INST.Size3 = Instance.Items.Count;
            Array.Resize(ref INST.UnkI321, INST.UnkI321Number);
            Array.Resize(ref INST.UnkI322, INST.UnkI322Number);
            Array.Resize(ref INST.UnkI323, INST.UnkI323Number);
            Array.Resize(ref INST.Something1, INST.Size1);
            Array.Resize(ref INST.Something2, INST.Size2);
            Array.Resize(ref INST.Something3, INST.Size3);
            for (int i = 0; i <= INST.UnkI321Number - 1; i++)
                INST.UnkI321[i] = (uint)Some.Items[i];
            for (int i = 0; i <= INST.UnkI322Number - 1; i++)
                INST.UnkI322[i] = (float)Floats.Items[i];
            for (int i = 0; i <= INST.UnkI323Number - 1; i++)
                INST.UnkI323[i] = (uint)Integers.Items[i];
            for (int i = 0; i <= INST.Size1 - 1; i++)
                INST.Something1[i] = (ushort)Position.Items[i];
            for (int i = 0; i <= INST.Size2 - 1; i++)
                INST.Something2[i] = (ushort)Path.Items[i];
            for (int i = 0; i <= INST.Size3 - 1; i++)
                INST.Something3[i] = (ushort)Instance.Items[i];
            twinsanityEditorForm.LevelData.Put_Item(INST, TwinsanityEditorForm.CalculateIndexes(twinsanityEditorForm.TreeView1.Nodes[0].Nodes[IISIndex].Nodes[6].Nodes[index]));
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            UpdateInstance(InstanceTree.SelectedNode.Index);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (Some.SelectedIndex >= 0)
            {
                int i = Some.SelectedIndex;
                Some.Items.RemoveAt(Some.SelectedIndex);
                if (i >= Some.Items.Count)
                    i = Some.Items.Count - 1;
                if (Some.Items.Count >= 0)
                    Some.SelectedIndex = i;
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            if (Floats.SelectedIndex >= 0)
            {
                int i = Floats.SelectedIndex;
                Floats.Items.RemoveAt(Floats.SelectedIndex);
                if (i >= Floats.Items.Count)
                    i = Floats.Items.Count - 1;
                if (Floats.Items.Count >= 0)
                    Floats.SelectedIndex = i;
            }
            Floats.Items.RemoveAt(Floats.SelectedIndex);
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            if (Integers.SelectedIndex >= 0)
            {
                int i = Integers.SelectedIndex;
                Integers.Items.RemoveAt(Integers.SelectedIndex);
                if (i >= Integers.Items.Count)
                    i = Integers.Items.Count - 1;
                if (Integers.Items.Count >= 0)
                    Integers.SelectedIndex = i;
            }
            Integers.Items.RemoveAt(Integers.SelectedIndex);
        }

        private void Button17_Click(object sender, EventArgs e)
        {
            if (Position.SelectedIndex >= 0)
            {
                int i = Position.SelectedIndex;
                Position.Items.RemoveAt(Position.SelectedIndex);
                if (i >= Position.Items.Count)
                    i = Position.Items.Count - 1;
                if (Position.Items.Count >= 0)
                    Position.SelectedIndex = i;
            }
            Position.Items.RemoveAt(Position.SelectedIndex);
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            if (Some.SelectedIndex >= 0)
            {
                int i = Some.SelectedIndex;
                Some.Items.RemoveAt(Some.SelectedIndex);
                if (i >= Some.Items.Count)
                    i = Some.Items.Count - 1;
                if (Some.Items.Count >= 0)
                    Some.SelectedIndex = i;
            }
            Path.Items.RemoveAt(Path.SelectedIndex);
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            if (Instance.SelectedIndex >= 0)
            {
                int i = Instance.SelectedIndex;
                Instance.Items.RemoveAt(Instance.SelectedIndex);
                if (i >= Instance.Items.Count)
                    i = Instance.Items.Count - 1;
                if (Instance.Items.Count >= 0)
                    Instance.SelectedIndex = i;
            }
            Instance.Items.RemoveAt(Instance.SelectedIndex);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Some.Items.Add(uint.Parse(SomeVal.Text));
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            Floats.Items.Add(float.Parse(FloatVal.Text));
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            Integers.Items.Add(uint.Parse(IntegerVal.Text));
        }

        private void Button19_Click(object sender, EventArgs e)
        {
            Position.Items.Add(uint.Parse(PositionVal.Text));
        }

        private void Button16_Click(object sender, EventArgs e)
        {
            Path.Items.Add(uint.Parse(PathVal.Text));
        }

        private void Button13_Click(object sender, EventArgs e)
        {
            Instance.Items.Add(uint.Parse(InstanceVal.Text));
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Some.Items[Some.SelectedIndex] = uint.Parse(SomeVal.Text);
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            Floats.Items[Floats.SelectedIndex] = float.Parse(FloatVal.Text);
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            Integers.Items[Integers.SelectedIndex] = uint.Parse(IntegerVal.Text);
        }

        private void Button18_Click(object sender, EventArgs e)
        {
            Position.Items[Position.SelectedIndex] = uint.Parse(PositionVal.Text);
        }

        private void Button15_Click(object sender, EventArgs e)
        {
            Path.Items[Path.SelectedIndex] = uint.Parse(PathVal.Text);
        }

        private void Button12_Click(object sender, EventArgs e)
        {
            Instance.Items[Instance.SelectedIndex] = uint.Parse(InstanceVal.Text);
        }

        private void Some_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateIndexes();
        }
        private void UpdateIndexes()
        {
            Label12.Text = "Some " + Some.SelectedIndex.ToString();
            Label13.Text = "Float " + Floats.SelectedIndex.ToString();
            Label14.Text = "Integer " + Integers.SelectedIndex.ToString();
            Label15.Text = "Position " + Position.SelectedIndex.ToString();
            Label16.Text = "Path " + Path.SelectedIndex.ToString();
            Label17.Text = "Instance " + Instance.SelectedIndex.ToString();
        }

        private void Floats_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateIndexes();
        }

        private void Integers_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateIndexes();
        }

        private void Position_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateIndexes();
        }

        private void Path_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateIndexes();
        }

        private void Instance_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateIndexes();
        }

        private void RotXText_TextChanged(object sender, EventArgs e)
        {
            try
            {
                RotationX.Value = UInt16.Parse(RotXText.Text);
            }
            catch (Exception ex)
            {
                RotationX.Value = 0;
            }
            UpdateRotations();
        }

        private void RotYText_TextChanged(object sender, EventArgs e)
        {
            try
            {
                RotationY.Value = UInt16.Parse(RotYText.Text);
            }
            catch (Exception ex)
            {
                RotationY.Value = 0;
            }
            UpdateRotations();
        }

        private void RotZText_TextChanged(object sender, EventArgs e)
        {
            try
            {
                RotationZ.Value = UInt16.Parse(RotZText.Text);
            }
            catch (Exception ex)
            {
                RotationZ.Value = 0;
            }
            UpdateRotations();
        }

        private void InstanceEditor_Load(object sender, EventArgs e)
        {
        }
    }
}
