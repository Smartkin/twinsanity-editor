using System;
using System.Collections.Generic;
using Twinsanity;

namespace TwinsaityEditor
{
    public class ParticleDataController : ItemController
    {
        public new ParticleData Data { get; set; }

        public ParticleDataController(MainForm topform, ParticleData item) : base(topform, item)
        {
            Data = item;
            AddMenu("Open editor", Menu_OpenEditor);
        }

        protected override string GetName()
        {
            if (Data.IsStandalone)
            {
                return $"PTL File";
            }
            else
            {
                return $"Particle Data [ID {Data.ID}]";
            }
        }

        protected override void GenText()
        {
            List<string> text = new List<string>();

            text.Add($"Size: {Data.Size}");
            text.Add($"Version: {Data.Version}");

            if (Data.IsDefault)
            {
                text.Add(string.Format("Particle Bank 1: Material ID: {0:X8}; Texture ID: {1:X8}", Data.ParticleMaterialID_1, Data.ParticleTextureID_1));
                text.Add(string.Format("Particle Bank 2: Material ID: {0:X8}; Texture ID: {1:X8}", Data.ParticleMaterialID_2, Data.ParticleTextureID_2));
                text.Add(string.Format("Particle Bank 3: Material ID: {0:X8}; Texture ID: {1:X8}", Data.ParticleMaterialID_3, Data.ParticleTextureID_3));
                text.Add(string.Format("Decal Bank: Material ID: {0:X8}; Texture ID: {1:X8}", Data.DecalMaterialID, Data.DecalTextureID));
            }

            text.Add($"Particle Types: {Data.ParticleTypes.Count}");
            for (int i = 0; i < Data.ParticleTypes.Count; i++)
            {
                ParticleData.ParticleSystemDefinition PS = Data.ParticleTypes[i];
                text.Add($"#{i} Name: {PS.Name} ");

                if (Data.Version == 0x20) text.Add($"\tGenRate: {PS.UnkByte1} ");
                text.Add($"\tGenRate: {PS.GenRate} ");
                text.Add($"\tMax Particle Count: {PS.MaxParticleCount} ");
                text.Add($"\tUnkUShort3: {PS.UnkUShort3} ");
                text.Add($"\tEmitter Over Time: {PS.Emitter_OverTime} ");
                text.Add($"\tEmitter Over Time Random: {PS.Emitter_OverTimeRandom} ");
                text.Add($"\tEmitter Off Time: {PS.Emitter_OffTime} ");
                text.Add($"\tEmitter Off Time Random: {PS.Emitter_OffTimeRandom} ");
                text.Add($"\tGenSort: {PS.GSort} ");
                text.Add($"\tUnkByte3: {PS.UnkByte3} ");
                text.Add($"\tTexture Filtering: {PS.TextureFilter} ");
                text.Add($"\tUnkByte5: {PS.UnkByte5} ");
                text.Add($"\tUnkFloat1: {PS.UnkFloat1} ");
                if (Data.Version >= 0x6) text.Add($"\tCutOn Radius: {PS.CutOnRadius} ");
                if (Data.Version >= 0x6) text.Add($"\tCutOff Radius: {PS.CutOffRadius} ");
                if (Data.Version >= 0xA) text.Add($"\tDraw CutOff: {PS.DrawCutOff} ");
                if (Data.Version > 0x16) text.Add($"\tUnkFloat5: {PS.UnkFloat5} ");
                if (Data.Version >= 0x18) text.Add($"\tUnkFloat6: {PS.UnkFloat6} ");
                text.Add($"\tVelocity: {PS.Velocity} ");
                text.Add($"\tRandom Emit X: {PS.Random_Emit_X} ");
                text.Add($"\tRandom Emit Y: {PS.Random_Emit_Y} ");
                text.Add($"\tRandom Emit Z: {PS.Random_Emit_Z} ");
                text.Add($"\tRandom Start X: {PS.Random_Start_X} ");
                text.Add($"\tRandom Start Y: {PS.Random_Start_Y} ");
                text.Add($"\tRandom Start Z: {PS.Random_Start_Z} ");
                text.Add($"\tUnkFloat8: {PS.UnkFloat8} ");
                text.Add($"\tUnkFloat9: {PS.UnkFloat9} ");
                text.Add($"\tUnkFloat10: {PS.UnkFloat10} ");
                text.Add($"\tUnkFloat11: {PS.UnkFloat11} ");
                text.Add($"\tUnkFloat12: {PS.UnkFloat12} ");
                text.Add($"\tUnkFloat13: {PS.UnkFloat13} ");
                text.Add($"\tUnkFloat14: {PS.UnkFloat14} ");
                text.Add($"\tUnkFloat15: {PS.UnkFloat15} ");
                text.Add($"\tUnkFloat16: {PS.UnkFloat16} ");
                text.Add($"\tUnkFloat17: {PS.UnkFloat17} ");
                text.Add($"\tUnkFloat18: {PS.UnkFloat18} ");
                text.Add($"\tUnkFloat19: {PS.UnkFloat19} ");
                text.Add($"\tGravity: {PS.Gravity} ");
                text.Add($"\tParticleLifeTime: {PS.ParticleLifeTime} ");
                text.Add($"\tUnkShort8: {PS.UnkUShort8} ");
                text.Add($"\tUnkByte6: {PS.UnkByte6} ");
                text.Add($"\tUnkByte7: {PS.UnkByte7} ");
                text.Add($"\tUnkFloat22: {PS.UnkFloat22} ");
                text.Add($"\tJibberXFreq: {PS.JibberXFreq} ");
                text.Add($"\tJibberXAmp: {PS.JibberXAmp} ");
                text.Add($"\tJibberYFreq: {PS.JibberYFreq} ");
                text.Add($"\tJibberYAmp: {PS.JibberYAmp} ");
                for (int a = 0; a < PS.ColorGradient.Length; a++)
                {
                    text.Add($"\tColor {a}: Time: {PS.ColorGradient[a].X}; R: {PS.ColorGradient[a].Y}; G: {PS.ColorGradient[a].Z}; B: {PS.ColorGradient[a].W}");
                }
                for (int a = 0; a < PS.AlphaGradientTime.Length; a++)
                {
                    text.Add($"\tAlpha {a}: Time: {PS.AlphaGradientTime[a]} Value: {PS.AlphaGradientValue[a]}");
                }
                if (Data.Version >= 0x15) text.Add($"\tDistortionX: {PS.DistortionX} ");
                if (Data.Version >= 0x15) text.Add($"\tDistortionY: {PS.DistortionY} ");
                text.Add($"\tMinSize: {PS.MinSize} ");
                text.Add($"\tMaxSize: {PS.MaxSize} ");
                for (int a = 0; a < PS.SizeWidthTime.Length; a++)
                {
                    text.Add($"\tSizeWidth {a}: Time: {PS.SizeWidthTime[a]} Value: {PS.SizeWidthValue[a]}");
                }
                for (int a = 0; a < PS.SizeHeightTime.Length; a++)
                {
                    text.Add($"\tSizeHeight {a}: Time: {PS.SizeHeightTime[a]} Value: {PS.SizeHeightValue[a]}");
                }
                text.Add($"\tMinRotation (deg): {PS.MinRotation} ");
                text.Add($"\tMaxRotation (deg): {PS.MaxRotation} ");
                for (int a = 0; a < PS.RotationTime.Length; a++)
                {
                    text.Add($"\tRotation {a}: Time: {PS.RotationTime[a]} Value: {PS.RotationValue[a]} ({PS.RotationValue[a] * 360f / 65535f} deg)");
                }
                for (int a = 0; a < PS.UnkGradient1Time.Length; a++)
                {
                    text.Add($"\tUnkGrad1 {a}: Time: {PS.UnkGradient1Time[a]} Value: {PS.UnkGradient1Value[a]}");
                }
                for (int a = 0; a < PS.UnkGradient2Time.Length; a++)
                {
                    text.Add($"\tUnkGrad2 {a}: Time: {PS.UnkGradient2Time[a]} Value: {PS.UnkGradient2Value[a]}");
                }
                if (PS.TextureStartX >= 524288f)
                    text.Add($"\tTextureStartX: {PS.TextureStartX - 524288f} / {PS.TextureStartX} F2");
                else
                    text.Add($"\tTextureStartX: {PS.TextureStartX - 262144f} / {PS.TextureStartX} F1");
                if (PS.TextureStartY >= 524288f)
                    text.Add($"\tTextureStartY: {PS.TextureStartY - 524288f} / {PS.TextureStartY} F2");
                else
                    text.Add($"\tTextureStartY: {PS.TextureStartY - 262144f} / {PS.TextureStartY} F1");
                if (PS.TextureEndX >= 524288f)
                    text.Add($"\tTextureEndX: {PS.TextureEndX - 524288f} / {PS.TextureEndX} F2");
                else
                    text.Add($"\tTextureEndX: {PS.TextureEndX - 262144f} / {PS.TextureEndX} F1");
                if (PS.TextureEndY >= 524288f)
                    text.Add($"\tTextureEndY: {PS.TextureEndY - 524288f} / {PS.TextureEndY} F2");
                else
                    text.Add($"\tTextureEndY: {PS.TextureEndY - 262144f} / {PS.TextureEndY} F1");
                if (Data.Version >= 0x3)
                {
                    for (int a = 0; a < PS.CollisionTime.Length; a++)
                    {
                        text.Add($"\tCollision {a}: Time: {PS.CollisionTime[a]} Value: {PS.CollisionValue[a]}");
                    }
                }
                if (Data.Version >= 0x3) text.Add($"\tCollisionNumSpheres: {PS.CollisionNumSpheres} ");
                if (Data.Version >= 0x11) text.Add($"\tDrawFlag: {PS.DrawFlag} ");
                if (Data.Version > 0x16 && Data.Version < 0x1D) text.Add($"\tPadAmount: {PS.padAmount} ");
                if (Data.Version > 0x1B) text.Add($"\tScaleFactor: {PS.ScaleFactor} ");
                if (Data.Version >= 0x10) text.Add($"\tGhostsNum: {PS.ParticleGhostsNum}");
                if (Data.Version >= 0x10) text.Add($"\tGhostSeparation: {PS.GhostSeparation} ");
                if (Data.Version >= 0x19) text.Add($"\tStarRadialPoints: {PS.StarRadialPoints}");
                if (Data.Version >= 0x19) text.Add($"\tStarRadiusRatio: {PS.StarRadiusRatio} ");
                if (Data.Version >= 0x1A) text.Add($"\tRampTime: {PS.RampTime} ");
                if (Data.Version > 0x1A) text.Add($"\tTexture Page: {PS.TexturePage} ");
                if (Data.Version >= 0x1E) text.Add($"\tUnkVec3: {PS.UnkVec3.X}; {PS.UnkVec3.Y}; {PS.UnkVec3.Z}; {PS.UnkVec3.W}");
                if (Data.Version >= 0xB && Data.Version <= 0x15)
                {
                    for (int a = 0; a < PS.SoundIDs.Length; a++)
                    {
                        text.Add($"\tSound {a}: ID: {PS.SoundIDs[a]} Type: {PS.SoundTypes[a]} Delay: {PS.SoundDelays[a]}");
                    }
                }

            }
            text.Add($"Particle Instances: {Data.ParticleInstances.Count}");
            for (int i = 0; i < Data.ParticleInstances.Count; i++)
            {
                var PI = Data.ParticleInstances[i];
                text.Add($"#{i} Name: {PI.Name}");
                text.Add($"#{i} Pos: {PI.Position.X}; {PI.Position.Y}; {PI.Position.Z}");
                text.Add($"#{i} Rot: {PI.EmitRotX}; {PI.EmitRotY}");
                /* Not altered:
                text.Add($"#{i} Gravity Rot: {PI.GravityRotX}; {PI.GravityRotY}");
                text.Add($"#{i} Offset: {PI.Offset} UnkShort5: {PI.UnkShort5}");
                text.Add($"#{i} SwitchType: {PI.SwitchType} SwitchID: {PI.SwitchID} SwitchValue: {PI.SwitchValue}");
                text.Add($"#{i} PlaneOffset: {PI.PlaneOffset} BounceFactor: {PI.BounceFactor} ");
                text.Add($"#{i} UnkShort6: {PI.UnkShort6} UnkShort7: {PI.UnkShort7}");
                */
            }
            text.Add($"Remain length: {Data.Remain.Length}");
            /*
            string remainText = "";
            for (int i = 0; i < Data.Remain.Length; i++)
            {
                remainText += $"{Data.Remain[i]:X2}";
            }
            text.Add(remainText);
            */

            TextPrev = text.ToArray();
        }

        private void Menu_OpenEditor()
        {
            MainFile.OpenEditor((ItemController)Node.Tag);
        }
    }
}