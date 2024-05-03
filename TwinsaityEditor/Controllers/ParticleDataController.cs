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

            if (!Data.IsStandalone)
            {
                text.Add($"ID: {Data.ID}");
                text.Add($"Size: {Data.Size}");
            }
            else
            {
                text.Add($"Size: {Data.Size}");
            }
            text.Add($"Version: 0x{Data.Version:X8}");

            if (Data.IsDefault)
            {
                text.Add(string.Format("Particle Bank 1: Material ID: {0:X8}; Texture ID: {1:X8}", Data.ParticleMaterialID_1, Data.ParticleTextureID_1));
                text.Add(string.Format("Particle Bank 2: Material ID: {0:X8}; Texture ID: {1:X8}", Data.ParticleMaterialID_2, Data.ParticleTextureID_2));
                text.Add(string.Format("Particle Bank 3: Material ID: {0:X8}; Texture ID: {1:X8}", Data.ParticleMaterialID_3, Data.ParticleTextureID_3));
                text.Add(string.Format("Decal Bank: Material ID: {0:X8}; Texture ID: {1:X8}", Data.DecalMaterialID, Data.DecalTextureID));
                if (Data.isMonkeyBall)
                {
                    text.Add(string.Format("Unk 1: ID 1: {0:X8}; Material ID: {1:X8}", Data.UnkTextureID_1, Data.UnkMaterialID_1));
                    text.Add(string.Format("Unk 2: ID 2: {0:X8}; Material ID: {1:X8}", Data.UnkTextureID_2, Data.UnkMaterialID_2));
                    text.Add(string.Format("Unk 3: ID 3: {0:X8}; Material ID: {1:X8}", Data.UnkTextureID_3, Data.UnkMaterialID_3));
                }
            }

            text.Add($"Particle Definitions: {Data.ParticleTypes.Count}");
            text.Add($"Particle Instances: {Data.ParticleInstances.Count}");
            text.Add($"Remain length: {Data.Remain.Length}");
            text.Add($"Definitions:");
            for (int i = 0; i < Data.ParticleTypes.Count; i++)
            {
                ParticleData.ParticleSystemDefinition PS = Data.ParticleTypes[i];
                text.Add($"#{i} Name: {PS.Name} ");
                /*
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
                text.Add($"\tCutOn Radius: {PS.CutOnRadius} ");
                text.Add($"\tCutOff Radius: {PS.CutOffRadius} ");
                text.Add($"\tDraw CutOff: {PS.DrawCutOff} ");
                text.Add($"\tUnkFloat5: {PS.UnkFloat5} ");
                text.Add($"\tUnkFloat6: {PS.UnkFloat6} ");
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
                for (int a = 0; a < PS.UnkVecs.Length; a++)
                {
                    text.Add($"\tUnkVecs {a}: {PS.UnkVecs[a].X}; {PS.UnkVecs[a].Y}; {PS.UnkVecs[a].Z}; {PS.UnkVecs[a].W}");
                }
                for (int a = 0; a < PS.UnkLongs1.Length; a++)
                {
                    text.Add($"\tUnkLongs1 {a}: {PS.UnkLongs1[a]}");
                }
                text.Add($"\tUnkFloat27: {PS.UnkFloat27} ");
                text.Add($"\tUnkFloat28: {PS.UnkFloat28} ");
                text.Add($"\tUnkFloat29: {PS.UnkFloat29} ");
                text.Add($"\tUnkFloat30: {PS.UnkFloat30} ");
                for (int a = 0; a < PS.UnkLongs2.Length; a++)
                {
                    text.Add($"\tUnkLongs2 {a}: {PS.UnkLongs2[a]}");
                }
                for (int a = 0; a < PS.UnkLongs3.Length; a++)
                {
                    text.Add($"\tUnkLongs3 {a}: {PS.UnkLongs3[a]}");
                }
                text.Add($"\tUnkFloat31: {PS.UnkFloat31} ");
                text.Add($"\tUnkFloat32: {PS.UnkFloat32} ");
                for (int a = 0; a < PS.UnkLongs4.Length; a++)
                {
                    text.Add($"\tUnkLongs4 {a}: {PS.UnkLongs4[a]}");
                }
                for (int a = 0; a < PS.UnkLongs5.Length; a++)
                {
                    text.Add($"\tUnkLongs5 {a}: {PS.UnkLongs6[a]}");
                }
                for (int a = 0; a < PS.UnkLongs6.Length; a++)
                {
                    text.Add($"\tUnkLongs6 {a}: {PS.UnkLongs6[a]}");
                }
                text.Add($"\tUnkFloat33: {PS.UnkFloat33} ");
                text.Add($"\tUnkFloat34: {PS.UnkFloat34} ");
                text.Add($"\tUnkFloat35: {PS.UnkFloat35} ");
                text.Add($"\tUnkFloat36: {PS.UnkFloat36} ");
                for (int a = 0; a < PS.UnkLongs7.Length; a++)
                {
                    text.Add($"\tUnkLongs7 {a}: {PS.UnkLongs7[a]}");
                }
                text.Add($"\tUnkByte8: {PS.UnkByte8} ");
                text.Add($"\tUnkByte9: {PS.UnkByte9} ");
                text.Add($"\tPadAmount: {PS.padAmount} ");
                text.Add($"\tUnkFloat37: {PS.UnkFloat37} ");
                for (int a = 0; a < PS.UnkShorts.Length; a++)
                {
                    text.Add($"\tUnkShorts {a}: {PS.UnkShorts[a]}");
                }
                text.Add($"\tUnkFloat38: {PS.UnkFloat38} ");
                text.Add($"\tUnkFloat39: {PS.UnkFloat39} ");
                text.Add($"\tUnkFloat40: {PS.UnkFloat40} ");
                text.Add($"\tUnkInt: {PS.UnkInt} ");
                text.Add($"\tUnkVec3: {PS.UnkVec3.X}; {PS.UnkVec3.Y}; {PS.UnkVec3.Z}; {PS.UnkVec3.W}");
                */

            }
            text.Add($"Instances:");
            for (int i = 0; i < Data.ParticleInstances.Count; i++)
            {
                var PI = Data.ParticleInstances[i];
                text.Add($"#{i} Name: {PI.Name} Pos: {PI.Position.X}; {PI.Position.Y}; {PI.Position.Z} Rot: {PI.EmitRotX}; {PI.EmitRotY}");
                /* Not altered in vanilla chunks:
                text.Add($"#{i} Gravity Rot: {PI.GravityRotX}; {PI.GravityRotY}");
                text.Add($"#{i} Offset: {PI.Offset} UnkShort5: {PI.UnkShort5}");
                text.Add($"#{i} SwitchType: {PI.SwitchType} SwitchID: {PI.SwitchID} SwitchValue: {PI.SwitchValue}");
                text.Add($"#{i} PlaneOffset: {PI.PlaneOffset} BounceFactor: {PI.BounceFactor} ");
                text.Add($"#{i} UnkShort6: {PI.UnkShort6} UnkShort7: {PI.UnkShort7}");
                */
            }
            
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