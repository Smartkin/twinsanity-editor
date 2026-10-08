using Twinsanity;
using System.Collections.Generic;
using System;

namespace TwinsaityEditor
{
    public class CameraController : ItemController
    {
        public new Camera Data { get; set; }

        public CameraController(MainForm topform, Camera item) : base(topform, item)
        {
            Data = item;
            AddMenu("Open editor", Menu_OpenEditor);
        }

        protected override string GetName()
        {
            return string.Format("Camera [ID {0}]", Data.ID);
        }

        protected override void GenText()
        {
            List<string> text = new List<string>();

            text.Add(string.Format("ID: {0:X8}", Data.ID));
            text.Add($"Size: {Data.Size}");
            text.Add($"Rotation ({Data.Coords[0].X}, {Data.Coords[0].Y}, {Data.Coords[0].Z}, {Data.Coords[0].W})");
            text.Add($"Position ({Data.Coords[1].X}, {Data.Coords[1].Y}, {Data.Coords[1].Z}, {Data.Coords[1].W})");
            text.Add($"Size ({Data.Coords[2].X}, {Data.Coords[2].Y}, {Data.Coords[2].Z}, {Data.Coords[2].W})");
            text.Add($"Header: {Data.Header} Mask: {Data.Enabled} CheckInterval: {Data.CheckInterval} SectionHead: {Data.SectionHead}");

            text.Add($"Instances: {Data.Instances.Count}");
            for (int i = 0; i < Data.Instances.Count; ++i)
            {
                string obj_name = MainFile.GetObjectName((ushort)MainFile.GetInstanceID(Data.Parent.Parent.ID, Data.Instances[i]));
                obj_name = Utils.TextUtils.TruncateObjectName(obj_name, (ushort)MainFile.GetInstanceID(Data.Parent.Parent.ID, Data.Instances[i]), "", " (Not in Objects)");

                text.Add($"Instance {Data.Instances[i]} {(obj_name != string.Empty ? $" - {obj_name}" : string.Empty)}");
            }

            text.Add("");
            text.Add($"Cam Flags: {Data.CamFlags}");
            string flag = Convert.ToString(Data.CamFlags, 2);
            if (flag.Length < 32)
            {
                while (flag.Length < 32)
                {
                    flag = "0" + flag;
                }
            }
            text.Add(flag);
            if (Data.ParentType != SectionType.CameraDemo)
            {
                text.Add($"Cam Switches: {Data.CamSwitches}");
            }
            text.Add($"BlendTime: {Data.BlendTime}");
            if (Data.ParentType != SectionType.CameraDemo)
            {
                text.Add($"Group: {Data.Group}");
            }

            text.Add("");
            text.Add($"TargetBoxMin ({Data.TargetBoxMin.X}, {Data.TargetBoxMin.Y}, {Data.TargetBoxMin.Z}, {Data.TargetBoxMin.W})");
            text.Add($"TargetBoxMax ({Data.TargetBoxMax.X}, {Data.TargetBoxMax.Y}, {Data.TargetBoxMax.Z}, {Data.TargetBoxMax.W})");
            if (true)
            {
                text.Add($"FramingDistance: {Data.FramingDistance.ToString()}");
            }
            if (true)
            {
                text.Add($"FramingShare: {Data.FramingShare.ToString()}");
            }
            if (true)
            {
                text.Add($"FovStart: {Data.FovStart.ToString()}");
            }
            if (true)
            {
                text.Add($"FovEnd: {Data.FovEnd.ToString()}");
            }
            if (true)
            {
                text.Add($"PitchStart: {Data.PitchStart.ToString()}");
            }
            if (true)
            {
                text.Add($"PitchEnd: {Data.PitchEnd.ToString()}");
            }
            if (true)
            {
                text.Add($"YawStart: {Data.YawStart.ToString()}");
            }
            if (true)
            {
                text.Add($"YawEnd: {Data.YawEnd.ToString()}");
            }
            if (true)
            {
                text.Add($"DistanceStart: {Data.DistanceStart.ToString()}");
            }
            if (true)
            {
                text.Add($"DistanceEnd: {Data.DistanceEnd.ToString()}");
            }
            if (true)
            {
                text.Add($"PositionFollowRate: {Data.PositionFollowRate.ToString()}");
            }
            if (true)
            {
                text.Add($"TargetFollowRate: {Data.TargetFollowRate.ToString()}");
            }
            if (true)
            {
                text.Add($"YawSpeed: {Data.YawSpeed.ToString()}");
            }
            if (true)
            {
                text.Add($"BlendInYaw: {Data.BlendInYaw.ToString()}");
            }
            if (true)
            {
                text.Add($"BlendInPitch: {Data.BlendInPitch.ToString()}");
            }
            if (true)
            {
                text.Add($"BlendInDistance: {Data.BlendInDistance.ToString()}");
            }
            text.Add("");

            if (Data.CameraType1 != 3)
            {
                text.Add($"Camera 1 Type: {Data.CameraType1:X};");
                GenTextCamera(Data.CameraType1, Data.Cameras[0], ref text);
            }
            else
            {
                text.Add($"Camera 1: N/A");
            }
            text.Add("");

            if (Data.CameraType2 != 3)
            {
                text.Add($"Camera 2 Type: {Data.CameraType2:X};");
                GenTextCamera(Data.CameraType2, Data.Cameras[1], ref text);
            }
            else
            {
                text.Add($"Camera 2: N/A");
            }

            TextPrev = text.ToArray();

        }

        private void GenTextCamera(uint Type, object cam, ref List<string> text)
        {
            switch (Type)
            {
                default:
                    throw new NotImplementedException();
                case 0xA19:
                    Camera.Camera_Boss Camera1 = (Camera.Camera_Boss)cam;

                    text.Add($"Boss Camera");
                    text.Add($"Flags {Camera1.Flags}");
                    text.Add($"Rate {Camera1.Rate}");
                    text.Add($"Offset {Camera1.Offset}");
                    for (int i = 0; i < Camera1.WorldToArena.Length; ++i)
                    {
                        text.Add($"WorldToArena {i}: {Camera1.WorldToArena[i].X}; {Camera1.WorldToArena[i].Y}; {Camera1.WorldToArena[i].Z}; {Camera1.WorldToArena[i].W}; ");
                    }
                    for (int i = 0; i < Camera1.WorldToArena.Length; ++i)
                    {
                        text.Add($"ArenaToWorld {i}: {Camera1.ArenaToWorld[i].X}; {Camera1.ArenaToWorld[i].Y}; {Camera1.ArenaToWorld[i].Z}; {Camera1.ArenaToWorld[i].W}; ");
                    }
                    text.Add($"Orbit: {Camera1.Orbit.X}; {Camera1.Orbit.Y}; {Camera1.Orbit.Z}; {Camera1.Orbit.W}; ");
                    text.Add($"Curves {Camera1.Curves}");
                    text.Add($"RadiusShare {Camera1.RadiusShare}");
                    text.Add($"MiddleHeight {Camera1.MiddleHeight}");
                    text.Add($"EdgeHeight {Camera1.EdgeHeight}");
                    text.Add($"TurnLimit {Camera1.TurnLimit}");
                    text.Add($"CurveAllAxes {Camera1.CurveAllAxes}");

                    break;
                case 0x1C02:
                    Camera.Camera_Point Camera2 = (Camera.Camera_Point)cam;

                    text.Add($"Camera Point");
                    text.Add($"Flags {Camera2.Flags}");
                    text.Add($"Rate {Camera2.Rate}");
                    text.Add($"Offset {Camera2.Offset}");
                    text.Add($"Point: {Camera2.Point.X}; {Camera2.Point.Y}; {Camera2.Point.Z}; {Camera2.Point.W}; ");

                    break;
                case 0x1C03:
                    Camera.Camera_Line Camera3 = (Camera.Camera_Line)cam;

                    text.Add($"Camera Line");
                    text.Add($"Flags {Camera3.Flags}");
                    text.Add($"Rate {Camera3.Rate}");
                    text.Add($"Offset {Camera3.Offset}");
                    text.Add($"LineStart: {Camera3.LineStart.X}; {Camera3.LineStart.Y}; {Camera3.LineStart.Z}; {Camera3.LineStart.W}; ");
                    text.Add($"LineEnd: {Camera3.LineEnd.X}; {Camera3.LineEnd.Y}; {Camera3.LineEnd.Z}; {Camera3.LineEnd.W}; ");

                    break;
                case 0x1C04:
                    Camera.Camera_Path Camera4 = (Camera.Camera_Path)cam;

                    text.Add($"Camera Path");
                    text.Add($"Flags {Camera4.Flags}");
                    text.Add($"Rate {Camera4.Rate}");
                    text.Add($"Offset {Camera4.Offset}");

                    for (int i = 0; i < Camera4.Points.Length; ++i)
                    {
                        text.Add($"Vector {i}: {Camera4.Points[i].X}; {Camera4.Points[i].Y}; {Camera4.Points[i].Z}; {Camera4.Points[i].W}; ");
                    }
                    for (int i = 0; i < Camera4.Lengths.Length; i++)
                    {
                        text.Add($"Param {i} Length {Camera4.Lengths[i]} Step {Camera4.Steps[i]} ");
                    }

                    break;
                case 0x1C05:
                    Camera.Camera_Main Camera5 = (Camera.Camera_Main)cam;
                    text.Add($"Main Camera");

                    break;
                case 0x1C06:
                    Camera.Camera_Spline Camera6 = (Camera.Camera_Spline)cam;

                    text.Add($"Camera Spline");
                    text.Add($"Flags {Camera6.Flags}");
                    text.Add($"Rate {Camera6.Rate}");
                    text.Add($"Offset {Camera6.Offset}");
                    text.Add($"Count {Camera6.Count}");
                    text.Add($"Step {Camera6.Step}");
                    text.Add($"SplineFlags {Camera6.SplineFlags}");

                    for (int i = 0; i < Camera6.Samples.Length; ++i)
                    {
                        text.Add($"Sample {i}: {Camera6.Samples[i].X}; {Camera6.Samples[i].Y}; {Camera6.Samples[i].Z}; {Camera6.Samples[i].W}; ");
                    }
                    for (int i = 0; i < Camera6.Lengths.Length; i++)
                    {
                        text.Add($"Param {i} Length {Camera6.Lengths[i]} Step {Camera6.Steps[i]} ");
                    }

                    break;
                case 0x1C09:
                    Camera.Camera_SplineArm Camera7 = (Camera.Camera_SplineArm)cam;

                    text.Add($"Spline Arm Camera");
                    text.Add($"Flags {Camera7.Flags}");
                    text.Add($"Rate {Camera7.Rate}");
                    text.Add($"Offset {Camera7.Offset}");

                    break;
                case 0x1C0B:
                    Camera.Camera_Point2 Camera8 = (Camera.Camera_Point2)cam;

                    text.Add($"Camera Point 2");
                    text.Add($"Flags {Camera8.Flags}");
                    text.Add($"Rate {Camera8.Rate}");
                    text.Add($"Offset {Camera8.Offset}");
                    text.Add($"Point: {Camera8.Point.X}; {Camera8.Point.Y}; {Camera8.Point.Z}; {Camera8.Point.W}; ");
                    text.Add($"Distance {Camera8.Distance}");
                    text.Add($"Mode {Camera8.Mode}");

                    break;
                case 0x1C0C:
                    Camera.Camera_Orbit Camera9 = (Camera.Camera_Orbit)cam;

                    text.Add($"Orbit Camera");
                    text.Add($"Flag1 {Camera9.Flag1}");
                    text.Add($"Flag2 {Camera9.Flag2}");
                    text.Add($"Flag3 {Camera9.Flag3}");
                    text.Add($"Flag4 {Camera9.Flag4}");

                    break;
                case 0x1C0D:
                    Camera.Camera_Line2 Camera10 = (Camera.Camera_Line2)cam;

                    text.Add($"Camera Line 2");
                    text.Add($"Flags {Camera10.Flags}");
                    text.Add($"Rate {Camera10.Rate}");
                    text.Add($"Offset {Camera10.Offset}");
                    text.Add($"LineStart: {Camera10.LineStart.X}; {Camera10.LineStart.Y}; {Camera10.LineStart.Z}; {Camera10.LineStart.W}; ");
                    text.Add($"LineEnd {Camera10.LineEnd.X}; {Camera10.LineEnd.Y}; {Camera10.LineEnd.Z}; {Camera10.LineEnd.W}; ");
                    text.Add($"NearDistance {Camera10.NearDistance}");
                    text.Add($"FarDistance {Camera10.FarDistance}");

                    break;
                case 0x1C0E:
                    Camera.Camera_Keyed Camera11 = (Camera.Camera_Keyed)cam;
                    text.Add($"Keyed Camera");

                    break;
                case 0x1C0F:
                    Camera.Camera_Zone Camera12 = (Camera.Camera_Zone)cam;

                    text.Add($"Zone Camera");

                    text.Add($"CameraBox");
                    for (int i = 0; i < Camera12.CameraBox.Length; i++)
                    {
                        text.Add($"Vector {i}: {Camera12.CameraBox[i].X}; {Camera12.CameraBox[i].Y}; {Camera12.CameraBox[i].Z}; {Camera12.CameraBox[i].W}; ");
                    }

                    text.Add($"TargetBox");
                    for (int i = 0; i < Camera12.TargetBox.Length; i++)
                    {
                        text.Add($"Vector {i}: {Camera12.TargetBox[i].X}; {Camera12.TargetBox[i].Y}; {Camera12.TargetBox[i].Z}; {Camera12.TargetBox[i].W}; ");
                    }

                    break;

            }
        }

        private void Menu_OpenEditor()
        {
            MainFile.OpenEditor((SectionController)Node.Parent.Tag);
        }
    }
}