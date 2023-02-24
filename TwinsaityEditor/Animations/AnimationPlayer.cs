using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinsaityEditor.Controllers;

namespace TwinsaityEditor.Animations
{
    public class AnimationPlayer
    {
        private AnimationController animation;
        private float time;
        private int animFrame;
        private bool playing;

        public int FPS { get; set; }
        public bool Loop { get; set; }
        public bool Playing
        {
            get => playing;
            set
            {
                if (value && Finished)
                {
                    animFrame = 0;
                    Finished = false;
                }
                playing = value;
            }
        }
        public bool Finished { get; private set; }

        public AnimationPlayer()
        {

        }

        public AnimationPlayer(AnimationController animation)
        {
            this.animation = animation;
            time = 0;
            animFrame = 0;
        }

        public void AdvanceClock(float deltaTime) // In seconds
        {
            if (!Playing || animation == null) return;

            var frameTime = 1f / FPS;
            time += deltaTime;

            while (time > frameTime)
            {
                time -= frameTime;
                animFrame++;
            }
        }

        public float[] PlayFacial()
        {
            if (animation == null || animation.Data.TotalFrames2 == 0) return new float[0];

            if (!Playing)
            {
                return animation.GetFacialAnimationTransform(animFrame, animFrame, 0);
            }

            if (animFrame + 1 >= animation.Data.TotalFrames2)
            {
                if (Loop)
                {
                    animFrame %= (animation.Data.TotalFrames2 - 1);
                }
                else
                {
                    Finished = true;
                    animFrame = animation.Data.TotalFrames2 - 1;
                    Playing = false;
                    return animation.GetFacialAnimationTransform(animFrame, animFrame, 0);
                }
            }

            var frameTime = 1f / FPS;
            var frameDisplacement = time / frameTime;
            return animation.GetFacialAnimationTransform(animFrame, animFrame + 1, frameDisplacement);
        }

        public Tuple<Matrix4, bool> Play(int joint)
        {
            if (animation == null) return new Tuple<Matrix4, bool>(Matrix4.Identity, false);

            if (!Playing)
            {
                return animation.GetMainAnimationTransform(joint, animFrame, animFrame, 0);
            }
            

            if (animFrame + 1 >= animation.Data.TotalFrames)
            {
                if (Loop)
                {
                    animFrame %= (animation.Data.TotalFrames - 1);
                }
                else
                {
                    Finished = true;
                    animFrame = animation.Data.TotalFrames - 1;
                    Playing = false;
                    return animation.GetMainAnimationTransform(joint, animFrame, animFrame, 0);
                }
            }

            var frameTime = 1f / FPS;
            var frameDisplacement = time / frameTime;
            return animation.GetMainAnimationTransform(joint, animFrame, animFrame + 1, frameDisplacement);
        }
    }
}
