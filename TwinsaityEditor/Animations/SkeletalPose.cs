using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twinsanity;

namespace TwinsaityEditor.Animations
{
    public class SkeletalPose
    {
        private Matrix4[] boneTransformations;
        public string Name;

        public Matrix4 this[int i]
        {
            get { return boneTransformations[i]; }
            set { boneTransformations[i] = value; }
        }

        public Matrix4[] MatrixArray { get { return boneTransformations; } }

        public SkeletalPose(int count)
        {
            boneTransformations = new Matrix4[count];
        }

        public SkeletalPose(int count, Matrix4 template)
        {
            boneTransformations = new Matrix4[count];
            for (int i = 0; i < count; i++)
                boneTransformations[i] = template;
        }

        public SkeletalPose(Matrix4[] matrices)
        {
            boneTransformations = matrices;
        }

        public Vector3 Position(int boneIndex)
        {
            var tr = boneTransformations[boneIndex].ExtractTranslation();
            return new Vector3(tr.X, tr.Y, tr.Z);
        }

        public Quaternion Rotation(int boneIndex)
        {
            return boneTransformations[boneIndex].ExtractRotation();
        }

        public void Set(int boneIndex, Vector3 position, Quaternion rotation)
        {
            boneTransformations[boneIndex] = Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateTranslation(position);
        }

        public SkeletalPose Clone(string name)
        {
            return new SkeletalPose((Matrix4[])this.boneTransformations.Clone()) { Name = name };
        }
    }
}
