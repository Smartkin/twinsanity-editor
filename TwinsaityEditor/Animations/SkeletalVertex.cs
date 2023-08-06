using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twinsanity;

namespace TwinsaityEditor.Animations
{
    public class SkeletalVertex
    {
        public List<SkeletalWeight> Weights;
        public Vertex Vertex;

        private const int MaxWeights = 3;

        public SkeletalVertex(Vertex v)
        {
            Vertex = v;
        }

        public float[] FloatArray()
        {
            float[] ret = new float[5 + MaxWeights * 2];
            ret[0] = Vertex.Pos.X;
            ret[1] = Vertex.Pos.Y;
            ret[2] = Vertex.Pos.Z;
            ret[3] = Vertex.Tex.X;
            ret[4] = Vertex.Tex.Y;

            for (int i = 0; i < Weights.Count; i++)
            {
                ret[5 + i] = Weights[i].BoneIndex;
                ret[5 + MaxWeights + i] = Weights[i].Bias;
            }

            return ret;
        }
    }
}
