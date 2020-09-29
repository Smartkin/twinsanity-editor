using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Twinsanity;

namespace TwinsaityEditor
{
    public partial class AnimationEditor : Form
    {
        private SectionController controller;
        private Animation animation;
        private Animation.BoneSettings BoneSettings;
        private Animation.BoneSettings BoneSettings2;
        private Animation.Timeline Timelines;
        private Animation.Timeline Timeline2;
        private Animation.EndTransformations Transformation;
        private Animation.EndTransformations Transformation2;

        public AnimationEditor(SectionController c)
        {
            controller = c;
            InitializeComponent();
            Text = $"Animation editor";
            PopulateList();
        }

        private void PopulateList()
        {
            lbAnimations.SelectedIndex = -1;
            lbAnimations.Items.Clear();
            foreach (Animation anim in controller.Data.Records)
            {
                lbAnimations.Items.Add($"Animation ID {anim.ID}");
            }
        }
        private void PopulateWithAnimData(IList list, IList data, String name)
        {
            list.Clear();
            var index = 1;
            foreach (var d in data)
            {
                list.Add($"{name} {index++}");
            }
        }

        private void lbAnimations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbAnimations.SelectedIndex == -1) return;
            BoneSettings = null;
            BoneSettings2 = null;
            Timelines = null;
            Timeline2 = null;
            Transformation = null;
            Transformation2 = null;
            tbDisB1.Text = "";
            tbDisB2.Text = "";
            tbDisB3.Text = "";
            tbDisB4.Text = "";
            tbDisB5.Text = "";
            tbDisB6.Text = "";
            tbDisB7.Text = "";
            tbDisB8.Text = "";
            tbDis2B1.Text = "";
            tbDis2B2.Text = "";
            tbDis2B3.Text = "";
            tbDis2B4.Text = "";
            tbDis2B5.Text = "";
            tbDis2B6.Text = "";
            tbDis2B7.Text = "";
            tbDis2B8.Text = "";
            tbScaleB1.Text = "";
            tbScaleB2.Text = "";
            tbScale2B1.Text = "";
            tbScale2B2.Text = "";
            tbRotationBytes.Text = "";
            tbRotation2Bytes.Text = "";
            animation = (Animation)controller.Data.Records[lbAnimations.SelectedIndex];
            PopulateWithAnimData(lbDisplacements.Items, animation.BonesSettings, "Bone setting");
            PopulateWithAnimData(lbScales.Items, animation.Transformations, "Transformation");
            PopulateWithAnimData(lbRotations.Items, animation.Timelines, "Timeline");
            PopulateWithAnimData(lbDisplacements2.Items, animation.BonesSettings2, "Bone setting");
            PopulateWithAnimData(lbScales2.Items, animation.Transformations2, "Transformation");
            PopulateWithAnimData(lbRotations2.Items, animation.Timelines2, "Timeline");
        }

        private void lbDisplacements_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list = (ListBox)sender;
            if (list.SelectedIndex == -1) return;

            Animation.BoneSettings displacement = animation.BonesSettings[list.SelectedIndex];
            BoneSettings = displacement;
            tbDisB1.Text = displacement.Unknown[0].ToString();
            tbDisB2.Text = displacement.Unknown[1].ToString();
            tbDisB3.Text = displacement.Unknown[2].ToString();
            tbDisB4.Text = displacement.Unknown[3].ToString();
            tbDisB5.Text = displacement.Unknown[4].ToString();
            tbDisB6.Text = displacement.Unknown[5].ToString();
            tbDisB7.Text = displacement.Unknown[6].ToString();
            tbDisB8.Text = displacement.Unknown[7].ToString();
        }

        private void lbScales_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list = (ListBox)sender;
            if (list.SelectedIndex == -1) return;

            Animation.EndTransformations scale = animation.Transformations[list.SelectedIndex];
            Transformation = scale;
            tbScaleB1.Text = scale.Unknown[0].ToString();
            tbScaleB2.Text = scale.Unknown[1].ToString();
        }

        private void lbRotations_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list = (ListBox)sender;
            if (list.SelectedIndex == -1) return;

            Animation.Timeline rotation = animation.Timelines[list.SelectedIndex];
            Timelines = rotation;
            tbRotationBytes.Text = "";
            for (int i = 0; i < rotation.Unknown.Length; i++)
            {
                var b = rotation.Unknown[i];
                tbRotationBytes.Text += b.ToString("X2");
                if (i != rotation.Unknown.Length - 1)
                {
                    tbRotationBytes.Text += " ";
                }
            }
        }

        private void lbDisplacements2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list = (ListBox)sender;
            if (list.SelectedIndex == -1) return;

            Animation.BoneSettings displacement = animation.BonesSettings2[list.SelectedIndex];
            BoneSettings2 = displacement;
            tbDis2B1.Text = displacement.Unknown[0].ToString();
            tbDis2B2.Text = displacement.Unknown[1].ToString();
            tbDis2B3.Text = displacement.Unknown[2].ToString();
            tbDis2B4.Text = displacement.Unknown[3].ToString();
            tbDis2B5.Text = displacement.Unknown[4].ToString();
            tbDis2B6.Text = displacement.Unknown[5].ToString();
            tbDis2B7.Text = displacement.Unknown[6].ToString();
            tbDis2B8.Text = displacement.Unknown[7].ToString();
        }

        private void lbScales2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list = (ListBox)sender;
            if (list.SelectedIndex == -1) return;

            Animation.EndTransformations scale = animation.Transformations2[list.SelectedIndex];
            Transformation2 = scale;
            tbScale2B1.Text = scale.Unknown[0].ToString();
            tbScale2B2.Text = scale.Unknown[1].ToString();
        }

        private void lbRotations2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list = (ListBox)sender;
            if (list.SelectedIndex == -1) return;

            Animation.Timeline rotation = animation.Timelines2[list.SelectedIndex];
            Timeline2 = rotation;
            tbRotation2Bytes.Text = "";
            for (int i = 0; i < rotation.Unknown.Length; i++)
            {
                var b = rotation.Unknown[i];
                tbRotation2Bytes.Text += b.ToString("X2");
                if (i != rotation.Unknown.Length - 1)
                {
                    tbRotation2Bytes.Text += " ";
                }
            }
        }

        private void tbDisB1_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Byte.TryParse(tb.Text, out Byte result) || BoneSettings == null) return;
            BoneSettings.Unknown[0] = result;
        }

        private void tbDisB2_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Byte.TryParse(tb.Text, out Byte result) || BoneSettings == null) return;
            BoneSettings.Unknown[1] = result;
        }

        private void tbDisB3_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Byte.TryParse(tb.Text, out Byte result) || BoneSettings == null) return;
            BoneSettings.Unknown[2] = result;
        }

        private void tbDisB4_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Byte.TryParse(tb.Text, out Byte result) || BoneSettings == null) return;
            BoneSettings.Unknown[3] = result;
        }

        private void tbDisB5_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Byte.TryParse(tb.Text, out Byte result) || BoneSettings == null) return;
            BoneSettings.Unknown[4] = result;
        }

        private void tbDisB6_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Byte.TryParse(tb.Text, out Byte result) || BoneSettings == null) return;
            BoneSettings.Unknown[5] = result;
        }

        private void tbDisB7_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Byte.TryParse(tb.Text, out Byte result) || BoneSettings == null) return;
            BoneSettings.Unknown[6] = result;
        }

        private void tbDisB8_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Byte.TryParse(tb.Text, out Byte result) || BoneSettings == null) return;
            BoneSettings.Unknown[7] = result;
        }

        private void tbScaleB1_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Byte.TryParse(tb.Text, out Byte result) || Transformation == null) return;
            Transformation.Unknown[0] = result;
        }

        private void tbScaleB2_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Byte.TryParse(tb.Text, out Byte result) || Transformation == null) return;
            Transformation.Unknown[1] = result;
        }

        private void tbRotationBytes_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (Timelines == null) return;
            var bytes = tb.Text.Split(' ');
            var newBytes = new List<Byte>();
            foreach (var b in bytes)
            {
                if (!Byte.TryParse(b, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out Byte result)) return;
                newBytes.Add(result);
            }
            if (newBytes.Count != Timelines.Unknown.Length) return;
            Timelines.Unknown = newBytes.ToArray();
        }

        private void tbDis2B1_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Byte.TryParse(tb.Text, out Byte result) || BoneSettings2 == null) return;
            BoneSettings2.Unknown[0] = result;
        }

        private void tbDis2B2_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Byte.TryParse(tb.Text, out Byte result) || BoneSettings2 == null) return;
            BoneSettings2.Unknown[1] = result;
        }

        private void tbDis2B3_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Byte.TryParse(tb.Text, out Byte result) || BoneSettings2 == null) return;
            BoneSettings2.Unknown[2] = result;
        }

        private void tbDis2B4_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Byte.TryParse(tb.Text, out Byte result) || BoneSettings2 == null) return;
            BoneSettings2.Unknown[3] = result;
        }

        private void tbDis2B5_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Byte.TryParse(tb.Text, out Byte result) || BoneSettings2 == null) return;
            BoneSettings2.Unknown[4] = result;
        }

        private void tbDis2B6_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Byte.TryParse(tb.Text, out Byte result) || BoneSettings2 == null) return;
            BoneSettings2.Unknown[5] = result;
        }

        private void tbDis2B7_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Byte.TryParse(tb.Text, out Byte result) || BoneSettings2 == null) return;
            BoneSettings2.Unknown[6] = result;
        }

        private void tbDis2B8_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Byte.TryParse(tb.Text, out Byte result) || BoneSettings2 == null) return;
            BoneSettings2.Unknown[7] = result;
        }

        private void tbScale2B1_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Byte.TryParse(tb.Text, out Byte result) || Transformation2 == null) return;
            Transformation2.Unknown[0] = result;
        }

        private void tbScale2B2_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (!Byte.TryParse(tb.Text, out Byte result) || Transformation2 == null) return;
            Transformation2.Unknown[1] = result;
        }

        private void tbRotation2Bytes_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (Timeline2 == null) return;
            var bytes = tb.Text.Split(' ');
            var newBytes = new List<Byte>();
            foreach (var b in bytes)
            {
                if (!Byte.TryParse(b, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out Byte result)) return;
                newBytes.Add(result);
            }
            if (newBytes.Count != Timeline2.Unknown.Length) return;
            Timeline2.Unknown = newBytes.ToArray();
        }
    }
}
