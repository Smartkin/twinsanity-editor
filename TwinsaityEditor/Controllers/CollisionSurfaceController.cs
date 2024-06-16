using Twinsanity;
using System.Collections.Generic;
using System;

namespace TwinsaityEditor
{
    public class CollisionSurfaceController : ItemController
    {
        public new CollisionSurface Data { get; set; }

        public CollisionSurfaceController(MainForm topform, CollisionSurface item) : base(topform, item)
        {
            Data = item;
            AddMenu("Open editor", Menu_OpenEditor);
        }

        protected override string GetName()
        {
            uint surf = Data.SurfaceID;
            if (MainFile.Data.Type == TwinsFile.FileType.MonkeyBallRM && Enum.IsDefined(typeof(DefaultEnums.SurfaceTypes_MonkeyBall), surf))
            {
                return (DefaultEnums.SurfaceTypes_MonkeyBall)surf + " [ID " + Data.ID.ToString() + "]";
            }
            else if (Enum.IsDefined(typeof(DefaultEnums.SurfaceTypes), surf))
            {
                return (DefaultEnums.SurfaceTypes)surf + " [ID " + Data.ID.ToString() + "]";
            }
            else
            {
                return string.Format("Surface [ID {0:X8}]", Data.ID);
            }


        }

        protected override void GenText()
        {
            List<string> text = new List<string>();

            text.Add(string.Format("ID: {0:X8}", Data.ID));
            text.Add($"Size: {Data.Size}");

            uint surf = Data.SurfaceID;
            if (Enum.IsDefined(typeof(DefaultEnums.SurfaceTypes), surf))
            {
                text.Add($"Type: { (DefaultEnums.SurfaceTypes)surf }");
            }
            else
            {
                text.Add($"Type: { Data.SurfaceID }");
            }

            text.Add($"Flags: {Data.Flags:X8}");
            text.Add($"Sound 1: { Data.Sound_1 }");
            text.Add($"Sound 2: { Data.Sound_2 }");
            text.Add($"Particle 1: { Data.Particle_1 }");
            text.Add($"Particle 2: { Data.Particle_2 }");
            text.Add($"Sound 3: { Data.Sound_3 }");
            text.Add($"Sound 4: { Data.Sound_4 }");
            text.Add($"Particle 3: { Data.Particle_3 }");
            text.Add($"Sound 5: { Data.Sound_5 }");
            text.Add($"Sound 6: { Data.Sound_6 }");
            text.Add($"Float1: { Data.UnkFloat1 } ");
            text.Add($"Float2: { Data.UnkFloat2 } ");
            text.Add($"Float3: { Data.UnkFloat3 } ");
            text.Add($"Float4: { Data.UnkFloat4 } ");
            text.Add($"Float5: { Data.UnkFloat5 } ");
            text.Add($"Float6: { Data.UnkFloat6 } ");
            text.Add($"Float7: { Data.UnkFloat7 } ");
            text.Add($"Float8: { Data.UnkFloat8 } ");
            text.Add($"Float9: { Data.UnkFloat9 } ");
            text.Add($"Float10: { Data.UnkFloat10 } ");
            text.Add($"UnkVector: {Data.UnkVector.X}; {Data.UnkVector.Y}; {Data.UnkVector.Z}; {Data.UnkVector.W}; ");
            text.Add($"UnkBoundingBox1: {Data.UnkBoundingBox1.X}; {Data.UnkBoundingBox1.Y}; {Data.UnkBoundingBox1.Z}; {Data.UnkBoundingBox1.W}; ");
            text.Add($"UnkBoundingBox2: {Data.UnkBoundingBox2.X}; {Data.UnkBoundingBox2.Y}; {Data.UnkBoundingBox2.Z}; {Data.UnkBoundingBox2.W}; ");


            TextPrev = text.ToArray();

        }

        private void Menu_OpenEditor()
        {
            MainFile.OpenEditor((SectionController)Node.Parent.Tag);
        }
    }
}