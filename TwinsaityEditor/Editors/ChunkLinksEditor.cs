using OpenTK;
using System;
using System.Windows.Forms;
using TwinsaityEditor.Utils;
using Twinsanity;

namespace TwinsaityEditor
{
    public partial class ChunkLinksEditor : Form
    {
        private ChunkLinksController controller;
        ChunkLinks.ChunkLink link;

        private int pos_i, areap_i, u1_i, u2_i;
        private bool ignore_value_change;

        public ChunkLinksEditor(ChunkLinksController c)
        {
            controller = c;
            InitializeComponent();
            PopulateList();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void PopulateList()
        {
            listBox1.Items.Clear();
            foreach (var i in controller.Data.Links)
            {
                listBox1.Items.Add(new string(i.Path));
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;
            ignore_value_change = true;

            link = controller.Data.Links[listBox1.SelectedIndex];
            groupBox1.Enabled = groupBox2.Enabled = groupBox3.Enabled = true;

            textBox1.Text = new string(link.Path);

            comboBox1.SelectedIndex = link.Type;

            textBox2.Text = Convert.ToString(link.Flags, 16).ToUpper();

            if (groupBox6.Enabled = (link.Flags & 0x80000) != 0)
            {
                GetLoadWallPos();
            }

            if (groupBox9.Enabled = groupBox8.Enabled = groupBox7.Enabled = link.Type == 1 || link.Type == 3)
            {
                GetLoadAreaPos();
                GetAreaMatrix1Pos();
                GetAreaMatrix2Pos();
            }
            Vector3 chunk_position = ExtractPosition(link.ChunkMatrix);
            Vector3 chunk_scale = ExtractScale(link.ChunkMatrix);
            Vector3 chunk_rotation = ExtractEulerRotation(link.ChunkMatrix, chunk_scale);
            ChunkPosX.Text = chunk_position.X.ToString();
            ChunkPosY.Text = chunk_position.Y.ToString();
            ChunkPosZ.Text = chunk_position.Z.ToString();
            ChunkScaleX.Text = chunk_scale.X.ToString();
            ChunkScaleY.Text = chunk_scale.Y.ToString();
            ChunkScaleZ.Text = chunk_scale.Z.ToString();
            ChunkRotationX.Text = chunk_rotation.X.ToString();
            ChunkRotationY.Text = chunk_rotation.Y.ToString();
            ChunkRotationZ.Text = chunk_rotation.Z.ToString();

            Vector3 object_position = ExtractPosition(link.ObjectMatrix);
            Vector3 object_scale = ExtractScale(link.ObjectMatrix);
            Vector3 object_rotation = ExtractEulerRotation(link.ObjectMatrix, object_scale);
            ObjectPosX.Text = object_position.X.ToString();
            ObjectPosY.Text = object_position.Y.ToString();
            ObjectPosZ.Text = object_position.Z.ToString();
            ObjectScaleX.Text = object_scale.X.ToString();
            ObjectScaleY.Text = object_scale.Y.ToString();
            ObjectScaleZ.Text = object_scale.Z.ToString();
            ObjectRotationX.Text = object_rotation.X.ToString();
            ObjectRotationY.Text = object_rotation.Y.ToString();
            ObjectRotationZ.Text = object_rotation.Z.ToString();
            ignore_value_change = false;
            return;
        }

        private void GetLoadWallPos()
        {
            numericUpDown27.Value = (decimal)link.LoadWall[pos_i].X;
            numericUpDown26.Value = (decimal)link.LoadWall[pos_i].Y;
            numericUpDown25.Value = (decimal)link.LoadWall[pos_i].Z;
            numericUpDown24.Value = (decimal)link.LoadWall[pos_i].W;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pos_i <= 0)
                return;
            label21.Text = $"{pos_i--} / 4";
            GetLoadWallPos();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pos_i >= 3)
                return;
            label21.Text = $"{++pos_i+1} / 4";
            GetLoadWallPos();
        }

        private void GetLoadAreaPos()
        {
            if (link.TreeRoot != null)
            {
                ChunkLinks.ChunkLink.LinkTree Link = (ChunkLinks.ChunkLink.LinkTree)link.TreeRoot;
                numericUpDown21.Value = (decimal)Link.LoadArea[areap_i].X;
                numericUpDown28.Value = (decimal)Link.LoadArea[areap_i].Y;
                numericUpDown23.Value = (decimal)Link.LoadArea[areap_i].Z;
                numericUpDown22.Value = (decimal)Link.LoadArea[areap_i].W;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (areap_i <= 0)
                return;
            label22.Text = $"{areap_i--} / 8";
            GetLoadAreaPos();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (u1_i <= 0)
                return;
            label27.Text = $"{u1_i--} / 6";
            GetAreaMatrix1Pos();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (u1_i >= 5)
                return;
            label27.Text = $"{++u1_i + 1} / 6";
            GetAreaMatrix1Pos();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (u2_i <= 0)
                return;
            label32.Text = $"{u2_i--} / 6";
            GetAreaMatrix2Pos();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (u2_i >= 5)
                return;
            label32.Text = $"{++u2_i + 1} / 6";
            GetAreaMatrix2Pos();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            UpdateObjectMatrix();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (areap_i >= 7)
                return;
            label22.Text = $"{++areap_i + 1} / 8";
            GetLoadAreaPos();
        }

        private void GetAreaMatrix1Pos()
        {
            if (link.TreeRoot != null)
            {
                ChunkLinks.ChunkLink.LinkTree Link = (ChunkLinks.ChunkLink.LinkTree)link.TreeRoot;
                numericUpDown29.Value = (decimal)Link.AreaMatrix[u1_i].X;
                numericUpDown32.Value = (decimal)Link.AreaMatrix[u1_i].Y;
                numericUpDown31.Value = (decimal)Link.AreaMatrix[u1_i].Z;
                numericUpDown30.Value = (decimal)Link.AreaMatrix[u1_i].W;
            }
        }

        private void numericUpDown20_ValueChanged(object sender, EventArgs e)
        {
            UpdateChunkMatrix();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* todo: rework for new structure
            listBox1.Items.Add("link");
            ChunkLinks.ChunkLink link = new ChunkLinks.ChunkLink { Path = "link".ToCharArray(), ObjectMatrix = new Pos[4], ChunkMatrix = new Pos[4], LoadWall = new Pos[4], LoadArea = new Pos[8], AreaMatrix = new Pos[6], UnknownMatrix = new Pos[6] };
            link.ObjectMatrix[3].W = link.ChunkMatrix[3].W =
                link.LoadWall[0].W = link.LoadWall[1].W = link.LoadWall[2].W = link.LoadWall[3].W =
                link.LoadArea[0].W = link.LoadArea[1].W = link.LoadArea[2].W = link.LoadArea[3].W =
                link.LoadArea[4].W = link.LoadArea[5].W = link.LoadArea[6].W = link.LoadArea[7].W =
                link.UnknownMatrix[0].W = link.UnknownMatrix[1].W = link.UnknownMatrix[2].W = link.UnknownMatrix[3].W = link.UnknownMatrix[4].W = link.UnknownMatrix[5].W = 1;
            link.Unknown = new short[15] { 0, 0, 8, 12, 6, 3, 3, 128, 224, 272, 320, 326, 356, 380, 0 };
            link.Bytes = new byte[60] { 0, 5, 10, 15, 20, 25, 4, 2, 3, 1, 0, 4, 4, 5, 3, 2, 4, 6, 7, 5, 4, 4, 0, 1, 7, 6, 4, 3, 5, 7, 1, 4, 4, 2, 0, 6, 0, 1, 1, 3, 3, 2, 2, 0, 3, 5, 5, 4, 4, 2, 5, 7, 7, 6, 6, 4, 7, 1, 0, 6, };
            controller.Data.Links.Add(link);
            controller.UpdateText();
            */
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sel_i = listBox1.SelectedIndex;
            if (sel_i == -1)
                return;
            controller.Data.Links.RemoveAt(sel_i);
            listBox1.Items.RemoveAt(sel_i);
            if (sel_i >= listBox1.Items.Count) sel_i = listBox1.Items.Count - 1;
            listBox1.SelectedIndex = sel_i;
            if (listBox1.Items.Count == 0)
                groupBox1.Enabled = groupBox2.Enabled = groupBox3.Enabled = groupBox6.Enabled = groupBox9.Enabled = groupBox8.Enabled = groupBox7.Enabled = false;
            controller.UpdateText();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            link.Path = textBox1.Text.ToCharArray();
            controller.Data.Links[listBox1.SelectedIndex] = link;
            listBox1.Items[listBox1.SelectedIndex] = textBox1.Text;
            controller.UpdateText();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ignore_value_change) return;
            link.Type = comboBox1.SelectedIndex;
            controller.Data.Links[listBox1.SelectedIndex] = link;
            if (groupBox9.Enabled = groupBox8.Enabled = groupBox7.Enabled = link.Type == 1 || link.Type == 3)
            {
                GetLoadAreaPos();
                GetAreaMatrix1Pos();
                GetAreaMatrix2Pos();
            }
            controller.UpdateText();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            uint o;
            if (uint.TryParse(textBox2.Text, System.Globalization.NumberStyles.HexNumber, null, out o))
                link.Flags = o;
            controller.Data.Links[listBox1.SelectedIndex] = link;
            if (groupBox6.Enabled = (link.Flags & 0x80000) != 0)
            {
                GetLoadWallPos();
            }
            controller.UpdateText();
        }

        private void numericUpDown27_ValueChanged(object sender, EventArgs e)
        {
            Pos pos = link.LoadWall[pos_i];
            pos.X = (float)numericUpDown27.Value;
            controller.Data.Links[listBox1.SelectedIndex].LoadWall[pos_i] = pos;
        }

        private void numericUpDown26_ValueChanged(object sender, EventArgs e)
        {
            Pos pos = link.LoadWall[pos_i];
            pos.Y = (float)numericUpDown26.Value;
            controller.Data.Links[listBox1.SelectedIndex].LoadWall[pos_i] = pos;
        }

        private void numericUpDown25_ValueChanged(object sender, EventArgs e)
        {
            Pos pos = link.LoadWall[pos_i];
            pos.Z = (float)numericUpDown25.Value;
            controller.Data.Links[listBox1.SelectedIndex].LoadWall[pos_i] = pos;
        }

        private void numericUpDown23_ValueChanged(object sender, EventArgs e)
        {
            if (link.TreeRoot != null)
            {
                ChunkLinks.ChunkLink.LinkTree Link = (ChunkLinks.ChunkLink.LinkTree)link.TreeRoot;
                Pos pos = Link.LoadArea[areap_i];
                pos.Z = (float)numericUpDown23.Value;
                Link.LoadArea[areap_i] = pos;
            }
        }

        private void numericUpDown28_ValueChanged(object sender, EventArgs e)
        {
            if (link.TreeRoot != null)
            {
                ChunkLinks.ChunkLink.LinkTree Link = (ChunkLinks.ChunkLink.LinkTree)link.TreeRoot;
                Pos pos = Link.LoadArea[areap_i];
                pos.Y = (float)numericUpDown28.Value;
                Link.LoadArea[areap_i] = pos;
            }
        }

        private void numericUpDown21_ValueChanged(object sender, EventArgs e)
        {
            if (link.TreeRoot != null)
            {
                ChunkLinks.ChunkLink.LinkTree Link = (ChunkLinks.ChunkLink.LinkTree)link.TreeRoot;
                Pos pos = Link.LoadArea[areap_i];
                pos.X = (float)numericUpDown21.Value;
                Link.LoadArea[areap_i] = pos;
            }
        }

        private void numericUpDown29_ValueChanged(object sender, EventArgs e)
        {
            if (link.TreeRoot != null)
            {
                ChunkLinks.ChunkLink.LinkTree Link = (ChunkLinks.ChunkLink.LinkTree)link.TreeRoot;
                Pos pos = Link.AreaMatrix[u1_i];
                pos.X = (float)numericUpDown29.Value;
                Link.AreaMatrix[u1_i] = pos;
            }
        }

        private void numericUpDown32_ValueChanged(object sender, EventArgs e)
        {
            if (link.TreeRoot != null)
            {
                ChunkLinks.ChunkLink.LinkTree Link = (ChunkLinks.ChunkLink.LinkTree)link.TreeRoot;
                Pos pos = Link.AreaMatrix[u1_i];
                pos.Y = (float)numericUpDown32.Value;
                Link.AreaMatrix[u1_i] = pos;
            }
        }

        private void numericUpDown31_ValueChanged(object sender, EventArgs e)
        {
            if (link.TreeRoot != null)
            {
                ChunkLinks.ChunkLink.LinkTree Link = (ChunkLinks.ChunkLink.LinkTree)link.TreeRoot;
                Pos pos = Link.AreaMatrix[u1_i];
                pos.Z = (float)numericUpDown31.Value;
                Link.AreaMatrix[u1_i] = pos;
            }
        }

        private void numericUpDown30_ValueChanged(object sender, EventArgs e)
        {
            if (link.TreeRoot != null)
            {
                ChunkLinks.ChunkLink.LinkTree Link = (ChunkLinks.ChunkLink.LinkTree)link.TreeRoot;
                Pos pos = Link.AreaMatrix[u1_i];
                pos.W = (float)numericUpDown30.Value;
                Link.AreaMatrix[u1_i] = pos;
            }
        }

        private void numericUpDown33_ValueChanged(object sender, EventArgs e)
        {
            if (link.TreeRoot != null)
            {
                ChunkLinks.ChunkLink.LinkTree Link = (ChunkLinks.ChunkLink.LinkTree)link.TreeRoot;
                Pos pos = Link.UnknownMatrix[u2_i];
                pos.X = (float)numericUpDown33.Value;
                Link.UnknownMatrix[u2_i] = pos;
            }
        }

        private void numericUpDown36_ValueChanged(object sender, EventArgs e)
        {
            if (link.TreeRoot != null)
            {
                ChunkLinks.ChunkLink.LinkTree Link = (ChunkLinks.ChunkLink.LinkTree)link.TreeRoot;
                Pos pos = Link.UnknownMatrix[u2_i];
                pos.Y = (float)numericUpDown36.Value;
                Link.UnknownMatrix[u2_i] = pos;
            }
        }

        private void numericUpDown35_ValueChanged(object sender, EventArgs e)
        {
            if (link.TreeRoot != null)
            {
                ChunkLinks.ChunkLink.LinkTree Link = (ChunkLinks.ChunkLink.LinkTree)link.TreeRoot;
                Pos pos = Link.UnknownMatrix[u2_i];
                pos.Z = (float)numericUpDown35.Value;
                Link.UnknownMatrix[u2_i] = pos;
            }
        }

        private void numericUpDown24_ValueChanged(object sender, EventArgs e)
        {
            Pos pos = link.LoadWall[pos_i];
            pos.W = (float)numericUpDown24.Value;
            controller.Data.Links[listBox1.SelectedIndex].LoadWall[pos_i] = pos;
        }

        private void numericUpDown22_ValueChanged(object sender, EventArgs e)
        {
            if (link.TreeRoot != null)
            {
                ChunkLinks.ChunkLink.LinkTree Link = (ChunkLinks.ChunkLink.LinkTree)link.TreeRoot;
                Pos pos = Link.LoadArea[areap_i];
                pos.W = (float)numericUpDown22.Value;
                Link.LoadArea[areap_i] = pos;
            }
        }

        private void numericUpDown34_ValueChanged(object sender, EventArgs e)
        {
            if (link.TreeRoot != null)
            {
                ChunkLinks.ChunkLink.LinkTree Link = (ChunkLinks.ChunkLink.LinkTree)link.TreeRoot;
                Pos pos = Link.UnknownMatrix[u2_i];
                pos.W = (float)numericUpDown34.Value;
                Link.UnknownMatrix[u2_i] = pos;
            }
        }

        private void ChunkLinksEditor_Load(object sender, EventArgs e)
        {

        }

        private void GetAreaMatrix2Pos()
        {
            if (link.TreeRoot != null)
            {
                ChunkLinks.ChunkLink.LinkTree Link = (ChunkLinks.ChunkLink.LinkTree)link.TreeRoot;
                numericUpDown33.Value = (decimal)Link.UnknownMatrix[u2_i].X;
                numericUpDown36.Value = (decimal)Link.UnknownMatrix[u2_i].Y;
                numericUpDown35.Value = (decimal)Link.UnknownMatrix[u2_i].Z;
                numericUpDown34.Value = (decimal)Link.UnknownMatrix[u2_i].W;
            }
        }

        private void UpdateObjectMatrix()
        {
            if (ignore_value_change) return;
            Vector3 ObjectPosition = new Vector3(float.Parse(ObjectPosX.Text), float.Parse(ObjectPosY.Text), float.Parse(ObjectPosZ.Text));
            Vector3 ObjectScale = new Vector3(float.Parse(ObjectScaleX.Text), float.Parse(ObjectScaleY.Text), float.Parse(ObjectScaleZ.Text));
            Vector3 ObjectEulerRotation = new Vector3(float.Parse(ObjectRotationX.Text), float.Parse(ObjectRotationY.Text), float.Parse(ObjectRotationZ.Text));

            link.ObjectMatrix = ComposeMatrix(ObjectPosition, ObjectScale, ObjectEulerRotation);

            controller.Data.Links[listBox1.SelectedIndex] = link;
            return;
        }

        private void UpdateChunkMatrix()
        {
            if (ignore_value_change) return;
            Vector3 ChunkPosition = new Vector3(float.Parse(ChunkPosX.Text), float.Parse(ChunkPosY.Text), float.Parse(ChunkPosZ.Text));
            Vector3 ChunkScale = new Vector3(float.Parse(ChunkScaleX.Text), float.Parse(ChunkScaleY.Text), float.Parse(ChunkScaleZ.Text));
            Vector3 ChunkEulerRotation = new Vector3(float.Parse(ChunkRotationX.Text), float.Parse(ChunkRotationY.Text), float.Parse(ChunkRotationZ.Text));

            link.ChunkMatrix = ComposeMatrix(ChunkPosition, ChunkScale, ChunkEulerRotation);
            controller.Data.Links[listBox1.SelectedIndex] = link;
            return;
        }

        private Vector3 ExtractPosition(Pos[] matrix)
        {
            Vector3 pos = new Vector3();
            pos.X = matrix[0].W;
            pos.Y = matrix[1].W;
            pos.Z = matrix[2].W;
            return pos;
        }
        private Vector3 ExtractScale(Pos[] matrix)
        {
            Vector3 scale = new Vector3();
            Vector3 scaleX = new Vector3();
            Vector3 scaleY = new Vector3();
            Vector3 scaleZ = new Vector3();
            scaleX.X = matrix[0].X;
            scaleX.Y = matrix[1].X;
            scaleX.Z = matrix[2].X;
            scaleY.X = matrix[0].Y;
            scaleY.Y = matrix[1].Y;
            scaleY.Z = matrix[2].Y;
            scaleZ.X = matrix[0].Z;
            scaleZ.Y = matrix[1].Z;
            scaleZ.Z = matrix[2].Z;
            scale.X = scaleX.Length;
            scale.Y = scaleY.Length;
            scale.Z = scaleZ.Length;
            return scale;
        }

        private const float EPSILON = 0.000000001f;
        private Vector3 ExtractEulerRotation(Pos[] matrix, Vector3 scale)
        {
            
            Matrix3 rotationMatrix = new Matrix3();

            rotationMatrix.Row0.X = matrix[0].X / scale.X;
            rotationMatrix.Row1.X = matrix[1].X / scale.X;
            rotationMatrix.Row2.X = matrix[2].X / scale.X;

            rotationMatrix.Row0.Y = matrix[0].Y / scale.Y;
            rotationMatrix.Row1.Y = matrix[1].Y / scale.Y;
            rotationMatrix.Row2.Y = matrix[2].Y / scale.Y;

            rotationMatrix.Row0.Z = matrix[0].Z / scale.Z;
            rotationMatrix.Row1.Z = matrix[1].Z / scale.Z;
            rotationMatrix.Row2.Z = matrix[2].Z / scale.Z;

            Quaternion rotation = Quaternion.FromMatrix(rotationMatrix);
            Vector3 angles = new Vector3();
            /*
             * TODO get angles
            angles.X = (float)Math.Atan2(2.0f * (rotation.X * rotation.Y + rotation.Z * rotation.W), 1.0f - 2.0f * (rotation.Y * rotation.Y + rotation.Z * rotation.Z));
            angles.Y = (float)Math.Asin(2.0f * (rotation.X * rotation.Z - rotation.W * rotation.Y));
            angles.Z = (float)Math.Atan2(2.0f * (rotation.X * rotation.W + rotation.Y * rotation.Z), 1.0f - 2.0f * (rotation.Z * rotation.Z + rotation.W * rotation.W));
            */

            return new Vector3(angles.X * 180f / (float)Math.PI, angles.Y * 180f / (float)Math.PI, angles.Z * 180f / (float)Math.PI);
        }

        private Pos[] ComposeMatrix(Vector3 Position, Vector3 Scale, Vector3 EulerRotation)
        {
            Matrix4 translation = CreateTranslationMatrix(Position);
            Matrix4 scale = CreateScaleMatrix(Scale);
            Matrix4 rotation = CreateRotationMatrix(new Vector3(EulerRotation.X * (float)Math.PI / 180.0f, 
                                                                   EulerRotation.Y * (float)Math.PI / 180.0f,
                                                                   EulerRotation.Z * (float)Math.PI / 180.0f));
            Matrix4 transform = translation * rotation * scale;
            return new Pos[4]
            {
                new Pos(transform.Row0.X, transform.Row0.Y, transform.Row0.Z, transform.Row0.W),
                new Pos(transform.Row1.X, transform.Row1.Y, transform.Row1.Z, transform.Row1.W),
                new Pos(transform.Row2.X, transform.Row2.Y, transform.Row2.Z, transform.Row2.W),
                new Pos(transform.Row3.X, transform.Row3.Y, transform.Row3.Z, transform.Row3.W)
            };
        }



        private void ProcessMatrixChange(TextBox sender,  Action updateAction)
        {
            if (!ignore_value_change)
            {
                try
                {
                    float.Parse(sender.Text, System.Globalization.CultureInfo.InvariantCulture);
                    sender.BackColor = System.Drawing.Color.White;
                    updateAction.Invoke();
                }
                catch
                {
                    sender.BackColor = System.Drawing.Color.Red;
                }
            }
        }
        private void ObjectPosX_TextChanged(object sender, EventArgs e)
        {
            ProcessMatrixChange((TextBox)sender, UpdateObjectMatrix);
        }
        private void ObjectPosY_TextChanged(object sender, EventArgs e)
        {
            ProcessMatrixChange((TextBox)sender, UpdateObjectMatrix);
        }

        private void ObjectPosZ_TextChanged(object sender, EventArgs e)
        {
            ProcessMatrixChange((TextBox)sender, UpdateObjectMatrix);
        }

        private void ObjectScaleX_TextChanged(object sender, EventArgs e)
        {
            ProcessMatrixChange((TextBox)sender, UpdateObjectMatrix);
        }

        private void ObjectScaleY_TextChanged(object sender, EventArgs e)
        {
            ProcessMatrixChange((TextBox)sender, UpdateObjectMatrix);
        }

        private void ObjectScaleZ_TextChanged(object sender, EventArgs e)
        {
            ProcessMatrixChange((TextBox)sender, UpdateObjectMatrix);
        }

        private void ObjectRotationX_TextChanged(object sender, EventArgs e)
        {
            ProcessMatrixChange((TextBox)sender, UpdateObjectMatrix);
        }

        private void ObjectRotationY_TextChanged(object sender, EventArgs e)
        {
            ProcessMatrixChange((TextBox)sender, UpdateObjectMatrix);
        }

        private void ObjectRotationZ_TextChanged(object sender, EventArgs e)
        {
            ProcessMatrixChange((TextBox)sender, UpdateObjectMatrix);
        }

        private void ChunkPosX_TextChanged(object sender, EventArgs e)
        {
            ProcessMatrixChange((TextBox)sender, UpdateChunkMatrix);
        }

        private void ChunkPosY_TextChanged(object sender, EventArgs e)
        {
            ProcessMatrixChange((TextBox)sender, UpdateChunkMatrix);
        }

        private void ChunkPosZ_TextChanged(object sender, EventArgs e)
        {
            ProcessMatrixChange((TextBox)sender, UpdateChunkMatrix);
        }

        private void ChunkScaleX_TextChanged(object sender, EventArgs e)
        {
            ProcessMatrixChange((TextBox)sender, UpdateChunkMatrix);
        }

        private void ChunkScaleY_TextChanged(object sender, EventArgs e)
        {
            ProcessMatrixChange((TextBox)sender, UpdateChunkMatrix);
        }

        private void ChunkScaleZ_TextChanged(object sender, EventArgs e)
        {
            ProcessMatrixChange((TextBox)sender, UpdateChunkMatrix);
        }

        private void ChunkRotationX_TextChanged(object sender, EventArgs e)
        {
            ProcessMatrixChange((TextBox)sender, UpdateChunkMatrix);
        }

        private void ChunkRotationY_TextChanged(object sender, EventArgs e)
        {
            ProcessMatrixChange((TextBox)sender, UpdateChunkMatrix);
        }

        private void ChunkRotationZ_TextChanged(object sender, EventArgs e)
        {
            ProcessMatrixChange((TextBox)sender, UpdateChunkMatrix);
        }

        private Matrix4 CreateTranslationMatrix(Vector3 Position)
        {
            Matrix4 matrix = Matrix4.CreateTranslation(Position);
            return matrix;
        }
        private Matrix4 CreateScaleMatrix(Vector3 Scale)
        {
            Matrix4 matrix = Matrix4.CreateScale(Scale);
            return matrix;
        }
        private Matrix4 CreateRotationMatrix(Vector3 Rotation)
        {
            return Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(Rotation));
        }
    }
}
