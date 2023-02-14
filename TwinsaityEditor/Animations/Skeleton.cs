using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinsaityEditor.Animations
{
    public class Skeleton
    {
        public List<SkeletalJoint> Joints { get; set; }
        public SkeletalPose Identity { get; set; }
        public SkeletalPose BindPose { get; set; }
        public SkeletalPose InverseBindPose { get; set; }

        public Skeleton(int jointCount)
        {
            Joints = new List<SkeletalJoint>(jointCount);
            Identity = new SkeletalPose(jointCount, Matrix4.Identity);
            BindPose = new SkeletalPose(jointCount);
            InverseBindPose = new SkeletalPose(jointCount);
        }
    }
}
