using System.Windows.Forms;
using System;

namespace TwinsaityEditor
{
    public partial class TriggerEditor
    {
        private TwinsanityEditorForm twinsanityEditorForm;

        public TriggerEditor(TwinsanityEditorForm TEF)
        {
            twinsanityEditorForm = TEF;
            InitializeComponent();
        }


        private int IISIndex;
        public void UpdateTree(ref Twinsanity.Triggers TRIGs, int Index)
        {
            TriggerTree.BeginUpdate();
            for (int i = 0; i <= TRIGs._Item.Length - 1; i++)
                TriggerTree.Nodes.Add("ID: " + TRIGs._Item[i].ID.ToString());
            TriggerTree.EndUpdate();
            IISIndex = Index;
        }
        public void UpdateTrigger(int index)
        {
            Twinsanity.Trigger TRIG = (Twinsanity.Trigger)twinsanityEditorForm.LevelData.Get_Item(TwinsanityEditorForm.CalculateIndexes(twinsanityEditorForm.TreeView1.Nodes[0].Nodes[IISIndex].Nodes[7].Nodes[index]));
            TriggerID.Text = TRIG.ID.ToString();
            Flags.Text = TRIG.SomeFlag.ToString();
            SomeNumber.Text = TRIG.SomeNumber.ToString();
            Value1.Text = TRIG.SomeUInt161.ToString();
            Value2.Text = TRIG.SomeUInt162.ToString();
            Value3.Text = TRIG.SomeUInt163.ToString();
            Value4.Text = TRIG.SomeUInt164.ToString();
            NumberValue.Text = TRIG.SomeUInt32.ToString();
            PosX.Text = TRIG.Coordinate[1].X.ToString();
            PosY.Text = TRIG.Coordinate[1].Y.ToString();
            PosZ.Text = TRIG.Coordinate[1].Z.ToString();
            SizeW.Text = TRIG.Coordinate[2].X.ToString();
            SizeH.Text = TRIG.Coordinate[2].Y.ToString();
            SizeL.Text = TRIG.Coordinate[2].Z.ToString();
            VecX.Text = TRIG.Coordinate[0].X.ToString();
            VecY.Text = TRIG.Coordinate[0].Y.ToString();
            VecZ.Text = TRIG.Coordinate[0].Z.ToString();
            Instances.Items.Clear();
            for (int i = 0; i <= TRIG.SectionSize - 1; i++)
                Instances.Items.Add(TRIG.SomeUInt16[i]);
            if (TRIG.SectionSize > 0)
                Instances.SelectedIndex = 0;
            if (Instances.SelectedIndex >= 0)
                UpdateObject(TRIG.SomeUInt16[Instances.SelectedIndex]);
            else
                Label4.Text = "Instances (Object is undefined)";
        }
        public void ApplyTrigger(int index)
        {
            Twinsanity.Trigger TRIG = new Twinsanity.Trigger();
            TRIG.ID = uint.Parse(TriggerID.Text);
            TRIG.SomeFlag = uint.Parse(Flags.Text);
            TRIG.SomeNumber = uint.Parse(SomeNumber.Text);
            TRIG.SomeUInt161 = UInt16.Parse(Value1.Text);
            TRIG.SomeUInt162 = UInt16.Parse(Value2.Text);
            TRIG.SomeUInt163 = UInt16.Parse(Value3.Text);
            TRIG.SomeUInt164 = UInt16.Parse(Value4.Text);
            TRIG.SomeUInt32 = uint.Parse(NumberValue.Text);
            TRIG.Coordinate[0].X = float.Parse(VecX.Text);
            TRIG.Coordinate[0].Y = float.Parse(VecY.Text);
            TRIG.Coordinate[0].Z = float.Parse(VecZ.Text);
            TRIG.Coordinate[1].X = float.Parse(PosX.Text);
            TRIG.Coordinate[1].Y = float.Parse(PosY.Text);
            TRIG.Coordinate[1].Z = float.Parse(PosZ.Text);
            TRIG.Coordinate[2].X = float.Parse(SizeW.Text);
            TRIG.Coordinate[2].Y = float.Parse(SizeH.Text);
            TRIG.Coordinate[2].Z = float.Parse(SizeL.Text);
            TRIG.SectionSize = Instances.Items.Count;
            Array.Resize(ref TRIG.SomeUInt16, TRIG.SectionSize);
            for (int i = 0; i <= TRIG.SectionSize - 1; i++)
                TRIG.SomeUInt16[i] = (ushort)Instances.Items[i];

            twinsanityEditorForm.LevelData.Put_Item(TRIG, TwinsanityEditorForm.CalculateIndexes(twinsanityEditorForm.TreeView1.Nodes[0].Nodes[IISIndex].Nodes[7].Nodes[index]));
        }
        private void UpdateObject(int Index)
        {
            Twinsanity.Instance INST = (Twinsanity.Instance)twinsanityEditorForm.LevelData.Get_Item(TwinsanityEditorForm.CalculateIndexes(twinsanityEditorForm.TreeView1.Nodes[0].Nodes[IISIndex].Nodes[6].Nodes[Index]));
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
                Label4.Text = "Instances (" + GO.Name + ")";
            else
                Label4.Text = "Instances (Object is undefined)";
        }

        private void TriggerTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdateTrigger(TriggerTree.SelectedNode.Index);
        }

        private void Revert_Click(object sender, EventArgs e)
        {
            UpdateTrigger(TriggerTree.SelectedNode.Index);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Instances.Items.Add(UInt16.Parse(InstanceVal.Text));
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Instances.Items[Instances.SelectedIndex] = UInt16.Parse(InstanceVal.Text);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Instances.Items.RemoveAt(Instances.SelectedIndex);
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            ApplyTrigger(TriggerTree.SelectedNode.Index);
        }

        private void Instances_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
