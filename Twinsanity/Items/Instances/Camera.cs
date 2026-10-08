using System.IO;
using System.Collections.Generic;
using System;

namespace Twinsanity
{
    public class Camera : TwinsItem
    {

        public uint Header { get; set; } = 1310720;
        public uint Enabled { get; set; } = 1;
        public float CheckInterval { get; set; } = 0.3f;
        public Pos[] Coords { get; set; } = new Pos[3]{
            new Pos(0,0,0,1),
            new Pos(0,0,0,1),
            new Pos(1,1,1,1),
        }; // rot/pos/size
        public uint SectionHead { get; set; } = 10;
        public List<ushort> Instances { get; set; } = new List<ushort>();

        public uint CamFlags { get; set; }
        public ushort CamSwitches { get; set; }
        public float BlendTime { get; set; } = 1f;
        public Pos TargetBoxMin { get; set; } = new Pos(0, 0, 0, 1);
        public Pos TargetBoxMax { get; set; } = new Pos(0, 0, 0, 1);
        public float FramingDistance { get; set; }
        public float FramingShare { get; set; }
        public uint FovStart { get; set; }
        public uint FovEnd { get; set; }
        public uint PitchStart { get; set; }
        public uint PitchEnd { get; set; }
        public int YawStart { get; set; }
        public int YawEnd { get; set; }
        public float DistanceStart { get; set; }
        public float DistanceEnd { get; set; }
        public float PositionFollowRate { get; set; }
        public float TargetFollowRate { get; set; }
        public uint YawSpeed { get; set; }
        public int BlendInYaw { get; set; }
        public uint BlendInPitch { get; set; }
        public float BlendInDistance { get; set; }
        public uint CameraType1 { get; set; } = 3;
        public uint CameraType2 { get; set; } = 3;
        public object[] Cameras { get; set; } = new object[2] { null, null };
        public byte Group { get; set; }

        public enum CameraType : uint
        {
            None = 3,
            Boss = 0xA19,
            Point = 0x1C02,
            Line = 0x1C03,
            Path = 0x1C04,
            NULL = 0x1C05,
            Spline = 0x1C06,
            Unk1 = 0x1C09,
            Point2 = 0x1C0B,
            Unk2 = 0x1C0C,
            Line2 = 0x1C0D,
            NULL2 = 0x1C0E,
            Zone = 0x1C0F,
        }

        public bool[] Mask
        {
            get
            {
                bool[] m = new bool[9] { false, false, false, false, false, false, false, false, false };

                for (int i = 0; i < m.Length; i++)
                {
                    if ((Enabled >> i & 0x1) != 0)
                    {
                        m[i] = true;
                    }
                }

                return m;
            }
            set
            {
                Enabled = 0;

                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i])
                    {
                        Enabled |= (uint)(1 << i);
                    }
                }
            }
        }

        public bool IsType0
        {
            get
            {
                return (Header >> 0x0 & 0x1) != 0;
            }
            set
            {
                uint mask = 1 << 0x0;
                if (value)
                    Header |= mask;
                else
                    Header &= ~mask;
            }
        }
        public bool IsType1
        {
            get
            {
                return (Header >> 0x1 & 0x1) != 0;
            }
            set
            {
                uint mask = 1 << 0x1;
                if (value)
                    Header |= mask;
                else
                    Header &= ~mask;
            }
        }
        public bool IsType2
        {
            get
            {
                return (Header >> 0x2 & 0x1) != 0;
            }
            set
            {
                uint mask = 1 << 0x2;
                if (value)
                    Header |= mask;
                else
                    Header &= ~mask;
            }
        }
        public bool IsType3
        {
            get
            {
                return (Header >> 0x3 & 0x1) != 0;
            }
            set
            {
                uint mask = 1 << 0x3;
                if (value)
                    Header |= mask;
                else
                    Header &= ~mask;
            }
        }
        public bool IsType4
        {
            get
            {
                return (Header >> 0x4 & 0x1) != 0;
            }
            set
            {
                uint mask = 1 << 0x4;
                if (value)
                    Header |= mask;
                else
                    Header &= ~mask;
            }
        }
        public bool IsType5
        {
            get
            {
                return (Header >> 0x5 & 0x1) != 0;
            }
            set
            {
                uint mask = 1 << 0x5;
                if (value)
                    Header |= mask;
                else
                    Header &= ~mask;
            }
        }
        public bool IsType6
        {
            get
            {
                return (Header >> 0x6 & 0x1) != 0;
            }
            set
            {
                uint mask = 1 << 0x6;
                if (value)
                    Header |= mask;
                else
                    Header &= ~mask;
            }
        }
        public bool IsType7
        {
            get
            {
                return (Header >> 0x7 & 0x1) != 0;
            }
            set
            {
                uint mask = 1 << 0x7;
                if (value)
                    Header |= mask;
                else
                    Header &= ~mask;
            }
        }
        public bool UnkFlag18
        {
            get
            {
                return (Header >> 0x12 & 0x1) != 0;
            }
            set
            {
                uint mask = 1 << 0x12;
                if (value)
                    Header |= mask;
                else
                    Header &= ~mask;
            }
        }
        public bool UnkFlag20
        {
            get
            {
                return (Header >> 0x14 & 0x1) != 0;
            }
            set
            {
                uint mask = 1 << 0x14;
                if (value)
                    Header |= mask;
                else
                    Header &= ~mask;
            }
        }

         public bool NotPolled
        {
            get
            {
                return (Header >> 0xC & 0x1) != 0;
            }
            set
            {
                uint mask = 1 << 0xC;
                if (value)
                    Header |= mask;
                else
                    Header &= ~mask;
            }
        }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Header);
            writer.Write(Enabled);
            writer.Write(CheckInterval);
            for (int i = 0; i < 3; ++i)
            {
                writer.Write(Coords[i].X);
                writer.Write(Coords[i].Y);
                writer.Write(Coords[i].Z);
                writer.Write(Coords[i].W);
            }
            writer.Write(Instances.Count);
            writer.Write(Instances.Count);
            writer.Write(SectionHead);
            for (int i = 0; i < Instances.Count; ++i)
                writer.Write(Instances[i]);

            writer.Write(CamFlags);
            if (ParentType != SectionType.CameraDemo)
            {
                writer.Write(CamSwitches);
            }
            writer.Write(BlendTime);
            writer.Write(TargetBoxMin.X);
            writer.Write(TargetBoxMin.Y);
            writer.Write(TargetBoxMin.Z);
            writer.Write(TargetBoxMin.W);
            writer.Write(TargetBoxMax.X);
            writer.Write(TargetBoxMax.Y);
            writer.Write(TargetBoxMax.Z);
            writer.Write(TargetBoxMax.W);

            writer.Write(FramingDistance);
            writer.Write(FramingShare);
            writer.Write(FovStart);
            writer.Write(FovEnd);
            writer.Write(PitchStart);
            writer.Write(PitchEnd);
            writer.Write(YawStart);
            writer.Write(YawEnd);
            writer.Write(DistanceStart);
            writer.Write(DistanceEnd);
            writer.Write(PositionFollowRate);
            writer.Write(TargetFollowRate);
            writer.Write(YawSpeed);
            writer.Write(BlendInYaw);
            writer.Write(BlendInPitch);
            writer.Write(BlendInDistance);

            writer.Write(CameraType1);
            writer.Write(CameraType2);

            if (ParentType != SectionType.CameraDemo)
            {
                writer.Write(Group);
            }

            if (CameraType1 != 3)
            {
                WriteCamera(writer, CameraType1, 0);
            }
            if (CameraType2 != 3)
            {
                WriteCamera(writer, CameraType2, 1);
            }

        }

        public override void Load(BinaryReader reader, int size)
        {
            Header = reader.ReadUInt32();
            Enabled = reader.ReadUInt32();
            CheckInterval = reader.ReadSingle();
            for (int i = 0; i < 3; ++i)
            {
                Coords[i] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            }
            var n = reader.ReadInt32();
            n = reader.ReadInt32();
            SectionHead = reader.ReadUInt32();
            Instances = new List<ushort>(n);
            for (int i = 0; i < n; ++i)
                Instances.Add(reader.ReadUInt16());

            CamFlags = reader.ReadUInt32();
            if (ParentType != SectionType.CameraDemo)
            {
                CamSwitches = reader.ReadUInt16();
            }
            else
            {
                CamSwitches = 0;
            }
            BlendTime = reader.ReadSingle();

            TargetBoxMin = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            TargetBoxMax = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            FramingDistance = reader.ReadSingle();
            FramingShare = reader.ReadSingle();
            FovStart = reader.ReadUInt32();
            FovEnd = reader.ReadUInt32();
            PitchStart = reader.ReadUInt32();
            PitchEnd = reader.ReadUInt32();
            YawStart = reader.ReadInt32();
            YawEnd = reader.ReadInt32();
            DistanceStart = reader.ReadSingle();
            DistanceEnd = reader.ReadSingle();
            PositionFollowRate = reader.ReadSingle();
            TargetFollowRate = reader.ReadSingle();
            YawSpeed = reader.ReadUInt32();
            BlendInYaw = reader.ReadInt32();
            BlendInPitch = reader.ReadUInt32();
            BlendInDistance = reader.ReadSingle();

            CameraType1 = reader.ReadUInt32();
            CameraType2 = reader.ReadUInt32();

            if (ParentType != SectionType.CameraDemo)
            {
                Group = reader.ReadByte();
            }
            else
            {
                Group = 0;
            }

            if (CameraType1 != 3)
            {
                ReadCamera(reader, CameraType1, 0);
            }
            if (CameraType2 != 3)
            {
                ReadCamera(reader, CameraType2, 1);
            }
        }

        private void ReadCamera(BinaryReader reader, uint Type, uint ID)
        {
            switch (Type)
            {
                default:
                    throw new NotImplementedException();
                case 0xA19:
                    Camera_Boss Camera1 = new Camera_Boss();

                    Camera1.Flags = reader.ReadUInt32();
                    Camera1.Rate = reader.ReadSingle();
                    Camera1.Offset = reader.ReadSingle();
                    Camera1.WorldToArena = new Pos[4];
                    for (int i = 0; i < Camera1.WorldToArena.Length; ++i)
                    {
                        Camera1.WorldToArena[i] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    }
                    Camera1.ArenaToWorld = new Pos[4];
                    for (int i = 0; i < Camera1.ArenaToWorld.Length; ++i)
                    {
                        Camera1.ArenaToWorld[i] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    }
                    Camera1.Orbit = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    Camera1.Curves = reader.ReadByte();
                    Camera1.RadiusShare = reader.ReadSingle();
                    Camera1.MiddleHeight = reader.ReadSingle();
                    Camera1.EdgeHeight = reader.ReadSingle();
                    Camera1.TurnLimit = reader.ReadSingle();
                    Camera1.CurveAllAxes = reader.ReadByte();

                    Cameras[ID] = Camera1;
                    break;
                case 0x1C02:
                    Camera_Point Camera2 = new Camera_Point();

                    Camera2.Flags = reader.ReadUInt32();
                    Camera2.Rate = reader.ReadSingle();
                    Camera2.Offset = reader.ReadSingle();
                    Camera2.Point = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                    Cameras[ID] = Camera2;
                    break;
                case 0x1C03:
                    Camera_Line Camera3 = new Camera_Line();

                    Camera3.Flags = reader.ReadUInt32();
                    Camera3.Rate = reader.ReadSingle();
                    Camera3.Offset = reader.ReadSingle();
                    Camera3.LineStart = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    Camera3.LineEnd = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                    Cameras[ID] = Camera3;
                    break;
                case 0x1C04:
                    Camera_Path Camera4 = new Camera_Path();

                    Camera4.Flags = reader.ReadUInt32();
                    Camera4.Rate = reader.ReadSingle();
                    Camera4.Offset = reader.ReadSingle();
                    uint VectorCount = reader.ReadUInt32();
                    Camera4.Points = new Pos[VectorCount];
                    for (int i = 0; i < VectorCount; ++i)
                    {
                        Camera4.Points[i] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    }
                    uint Parameters = reader.ReadUInt32();
                    Camera4.Lengths = new float[Parameters];
                    Camera4.Steps = new float[Parameters];
                    for (int i = 0; i < Parameters; ++i)
                    {
                        Camera4.Lengths[i] = reader.ReadSingle();
                    }
                    for (int i = 0; i < Parameters; ++i)
                    {
                        Camera4.Steps[i] = reader.ReadSingle();
                    }

                    Cameras[ID] = Camera4;
                    break;
                case 0x1C05:
                    Camera_Main Camera5 = new Camera_Main();
                    Cameras[ID] = Camera5;
                    break;
                case 0x1C06:
                    Camera_Spline Camera6 = new Camera_Spline();

                    Camera6.Flags = reader.ReadInt32();
                    Camera6.Rate = reader.ReadSingle();
                    Camera6.Offset = reader.ReadSingle();
                    Camera6.Count = reader.ReadUInt32();
                    Camera6.Step = reader.ReadSingle();
                    Camera6.Samples = new Pos[(Camera6.Count + 1) * 2];
                    for (int i = 0; i < Camera6.Samples.Length; ++i)
                    {
                        Camera6.Samples[i] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    }
                    Camera6.Lengths = new float[Camera6.Count];
                    Camera6.Steps = new float[Camera6.Count];
                    for (int i = 0; i < Camera6.Count; ++i)
                    {
                        Camera6.Lengths[i] = reader.ReadSingle();
                    }
                    for (int i = 0; i < Camera6.Count; ++i)
                    {
                        Camera6.Steps[i] = reader.ReadSingle();
                    }
                    Camera6.SplineFlags = reader.ReadInt16();

                    Cameras[ID] = Camera6;
                    break;
                case 0x1C09:
                    Camera_SplineArm Camera7 = new Camera_SplineArm();

                    Camera7.Flags = reader.ReadUInt32();
                    Camera7.Rate = reader.ReadSingle();
                    Camera7.Offset = reader.ReadSingle();

                    Cameras[ID] = Camera7;
                    break;
                case 0x1C0B:
                    Camera_Point2 Camera8 = new Camera_Point2();

                    Camera8.Flags = reader.ReadUInt32();
                    Camera8.Rate = reader.ReadSingle();
                    Camera8.Offset = reader.ReadSingle();
                    Camera8.Point = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    Camera8.Distance = reader.ReadSingle();
                    Camera8.Mode = reader.ReadByte();

                    Cameras[ID] = Camera8;
                    break;
                case 0x1C0C:
                    Camera_Orbit Camera9 = new Camera_Orbit();

                    Camera9.Flag1 = reader.ReadByte();
                    Camera9.Flag2 = reader.ReadByte();
                    Camera9.Flag3 = reader.ReadByte();
                    Camera9.Flag4 = reader.ReadByte();

                    Cameras[ID] = Camera9;
                    break;
                case 0x1C0D:
                    Camera_Line2 Camera10 = new Camera_Line2();

                    Camera10.Flags = reader.ReadUInt32();
                    Camera10.Rate = reader.ReadSingle();
                    Camera10.Offset = reader.ReadSingle();
                    Camera10.LineStart = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    Camera10.LineEnd = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    Camera10.NearDistance = reader.ReadSingle();
                    Camera10.FarDistance = reader.ReadSingle();

                    Cameras[ID] = Camera10;
                    break;
                case 0x1C0E:
                    Camera_Keyed Camera11 = new Camera_Keyed();
                    Cameras[ID] = Camera11;
                    break;
                case 0x1C0F:
                    Camera_Zone Camera12 = new Camera_Zone();

                    Camera12.CameraBox = new Pos[5];
                    for (int i = 0; i < Camera12.CameraBox.Length; i++)
                    {
                        Camera12.CameraBox[i] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    }

                    Camera12.TargetBox = new Pos[5];
                    for (int i = 0; i < Camera12.TargetBox.Length; i++)
                    {
                        Camera12.TargetBox[i] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    }

                    Cameras[ID] = Camera12;
                    break;

            }
        }

        private void WriteCamera(BinaryWriter writer, uint Type, uint ID)
        {
            switch (Type)
            {
                default:
                    throw new NotImplementedException();
                case 0xA19:
                    Camera_Boss Camera1 = (Camera_Boss)Cameras[ID];

                    writer.Write(Camera1.Flags);
                    writer.Write(Camera1.Rate);
                    writer.Write(Camera1.Offset);
                    for (int i = 0; i < Camera1.WorldToArena.Length; ++i)
                    {
                        writer.Write(Camera1.WorldToArena[i].X);
                        writer.Write(Camera1.WorldToArena[i].Y);
                        writer.Write(Camera1.WorldToArena[i].Z);
                        writer.Write(Camera1.WorldToArena[i].W);
                    }
                    for (int i = 0; i < Camera1.ArenaToWorld.Length; ++i)
                    {
                        writer.Write(Camera1.ArenaToWorld[i].X);
                        writer.Write(Camera1.ArenaToWorld[i].Y);
                        writer.Write(Camera1.ArenaToWorld[i].Z);
                        writer.Write(Camera1.ArenaToWorld[i].W);
                    }
                    writer.Write(Camera1.Orbit.X);
                    writer.Write(Camera1.Orbit.Y);
                    writer.Write(Camera1.Orbit.Z);
                    writer.Write(Camera1.Orbit.W);
                    writer.Write(Camera1.Curves);
                    writer.Write(Camera1.RadiusShare);
                    writer.Write(Camera1.MiddleHeight);
                    writer.Write(Camera1.EdgeHeight);
                    writer.Write(Camera1.TurnLimit);
                    writer.Write(Camera1.CurveAllAxes);

                    break;
                case 0x1C02:
                    Camera_Point Camera2 = (Camera_Point)Cameras[ID];

                    writer.Write(Camera2.Flags);
                    writer.Write(Camera2.Rate);
                    writer.Write(Camera2.Offset);
                    writer.Write(Camera2.Point.X);
                    writer.Write(Camera2.Point.Y);
                    writer.Write(Camera2.Point.Z);
                    writer.Write(Camera2.Point.W);

                    break;
                case 0x1C03:
                    Camera_Line Camera3 = (Camera_Line)Cameras[ID];

                    writer.Write(Camera3.Flags);
                    writer.Write(Camera3.Rate);
                    writer.Write(Camera3.Offset);
                    writer.Write(Camera3.LineStart.X);
                    writer.Write(Camera3.LineStart.Y);
                    writer.Write(Camera3.LineStart.Z);
                    writer.Write(Camera3.LineStart.W);
                    writer.Write(Camera3.LineEnd.X);
                    writer.Write(Camera3.LineEnd.Y);
                    writer.Write(Camera3.LineEnd.Z);
                    writer.Write(Camera3.LineEnd.W);

                    break;
                case 0x1C04:
                    Camera_Path Camera4 = (Camera_Path)Cameras[ID];

                    writer.Write(Camera4.Flags);
                    writer.Write(Camera4.Rate);
                    writer.Write(Camera4.Offset);
                    writer.Write(Camera4.Points.Length);
                    for (int i = 0; i < Camera4.Points.Length; ++i)
                    {
                        writer.Write(Camera4.Points[i].X);
                        writer.Write(Camera4.Points[i].Y);
                        writer.Write(Camera4.Points[i].Z);
                        writer.Write(Camera4.Points[i].W);
                    }
                    writer.Write(Camera4.Lengths.Length);
                    for (int i = 0; i < Camera4.Lengths.Length; ++i)
                    {
                        writer.Write(Camera4.Lengths[i]);
                    }
                    for (int i = 0; i < Camera4.Steps.Length; ++i)
                    {
                        writer.Write(Camera4.Steps[i]);
                    }

                    break;
                case 0x1C05:
                    break;
                case 0x1C06:
                    Camera_Spline Camera6 = (Camera_Spline)Cameras[ID];

                    writer.Write(Camera6.Flags);
                    writer.Write(Camera6.Rate);
                    writer.Write(Camera6.Offset);
                    writer.Write(Camera6.Count);
                    writer.Write(Camera6.Step);
                    for (int i = 0; i < Camera6.Samples.Length; ++i)
                    {
                        writer.Write(Camera6.Samples[i].X);
                        writer.Write(Camera6.Samples[i].Y);
                        writer.Write(Camera6.Samples[i].Z);
                        writer.Write(Camera6.Samples[i].W);
                    }
                    for (int i = 0; i < Camera6.Count; ++i)
                    {
                        writer.Write(Camera6.Lengths[i]);
                    }
                    for (int i = 0; i < Camera6.Count; ++i)
                    {
                        writer.Write(Camera6.Steps[i]);
                    }
                    writer.Write(Camera6.SplineFlags);

                    break;
                case 0x1C09:
                    Camera_SplineArm Camera7 = (Camera_SplineArm)Cameras[ID];

                    writer.Write(Camera7.Flags);
                    writer.Write(Camera7.Rate);
                    writer.Write(Camera7.Offset);

                    break;
                case 0x1C0B:
                    Camera_Point2 Camera8 = (Camera_Point2)Cameras[ID];

                    writer.Write(Camera8.Flags);
                    writer.Write(Camera8.Rate);
                    writer.Write(Camera8.Offset);
                    writer.Write(Camera8.Point.X);
                    writer.Write(Camera8.Point.Y);
                    writer.Write(Camera8.Point.Z);
                    writer.Write(Camera8.Point.W);
                    writer.Write(Camera8.Distance);
                    writer.Write(Camera8.Mode);

                    break;
                case 0x1C0C:
                    Camera_Orbit Camera9 = (Camera_Orbit)Cameras[ID];

                    writer.Write(Camera9.Flag1);
                    writer.Write(Camera9.Flag2);
                    writer.Write(Camera9.Flag3);
                    writer.Write(Camera9.Flag4);

                    break;
                case 0x1C0D:
                    Camera_Line2 Camera10 = (Camera_Line2)Cameras[ID];

                    writer.Write(Camera10.Flags);
                    writer.Write(Camera10.Rate);
                    writer.Write(Camera10.Offset);
                    writer.Write(Camera10.LineStart.X);
                    writer.Write(Camera10.LineStart.Y);
                    writer.Write(Camera10.LineStart.Z);
                    writer.Write(Camera10.LineStart.W);
                    writer.Write(Camera10.LineEnd.X);
                    writer.Write(Camera10.LineEnd.Y);
                    writer.Write(Camera10.LineEnd.Z);
                    writer.Write(Camera10.LineEnd.W);
                    writer.Write(Camera10.NearDistance);
                    writer.Write(Camera10.FarDistance);

                    break;
                case 0x1C0E:
                    break;
                case 0x1C0F:
                    Camera_Zone Camera12 = (Camera_Zone)Cameras[ID];

                    for (int i = 0; i < Camera12.CameraBox.Length; i++)
                    {
                        writer.Write(Camera12.CameraBox[i].X);
                        writer.Write(Camera12.CameraBox[i].Y);
                        writer.Write(Camera12.CameraBox[i].Z);
                        writer.Write(Camera12.CameraBox[i].W);
                    }

                    for (int i = 0; i < Camera12.TargetBox.Length; i++)
                    {
                        writer.Write(Camera12.TargetBox[i].X);
                        writer.Write(Camera12.TargetBox[i].Y);
                        writer.Write(Camera12.TargetBox[i].Z);
                        writer.Write(Camera12.TargetBox[i].W);
                    }

                    break;

            }
        }

        protected override int GetSize()
        {
            int count = 4 + 4 + 4 + (16 * 3) + 4 + 4 + 4 + (Instances.Count * 2);

            count += 2 + 2;
            if (ParentType != SectionType.CameraDemo)
            {
                count += 2;
            }

            count += 4 + 16 + 16 + (4 * 16) + 4 + 4;

            if (ParentType != SectionType.CameraDemo)
            {
                count += 1;
            }

            for (int i = 0; i < 2; i++)
            {
                if (Cameras[i] == null)
                {

                }
                else if (Cameras[i] is Camera_Boss Camera1)
                {
                    count += 4 + 4 + 4 + (Camera1.WorldToArena.Length * 16) + (Camera1.ArenaToWorld.Length * 16) +
                        16 + 1 + 4 + 4 + 4 + 4 + 1;
                }
                else if (Cameras[i] is Camera_Point Camera2)
                {
                    count += 4 + 4 + 4 + 16;
                }
                else if (Cameras[i] is Camera_Line Camera3)
                {
                    count += 4 + 4 + 4 + 16 + 16;
                }
                else if (Cameras[i] is Camera_Path Camera4)
                {
                    count += 4 + 4 + 4 + 4 + (Camera4.Points.Length * 16) + 4 + (Camera4.Lengths.Length * 4) + (Camera4.Steps.Length * 4);
                }
                else if (Cameras[i] is Camera_Main Camera5)
                {

                }
                else if (Cameras[i] is Camera_Spline Camera6)
                {
                    count += 4 + 4 + 4 + 4 + 4 + (Camera6.Samples.Length * 16) + (Camera6.Lengths.Length * 4) + (Camera6.Steps.Length * 4) + 2;
                }
                else if (Cameras[i] is Camera_SplineArm Camera7)
                {
                    count += 4 + 4 + 4;
                }
                else if (Cameras[i] is Camera_Point2 Camera8)
                {
                    count += 4 + 4 + 4 + 16 + 4 + 1;
                }
                else if (Cameras[i] is Camera_Orbit Camera9)
                {
                    count += 4;
                }
                else if (Cameras[i] is Camera_Line2 Camera10)
                {
                    count += 4 + 4 + 4 + 16 + 16 + 4 + 4;
                }
                else if (Cameras[i] is Camera_Keyed Camera11)
                {

                }
                else if (Cameras[i] is Camera_Zone Camera12)
                {
                    count += (Camera12.CameraBox.Length * 16) + 4 + 4 + 8 + (Camera12.TargetBox.Length * 16) + 4 + 4 + 8;
                }
                else
                {

                }
            }


            return count;
        }

        public class Camera_Boss // Boss Camera (Centers on Boss) 0xA19
        {
            public uint Flags;
            public float Rate;
            public float Offset;
            public Pos[] WorldToArena; // 4
            public Pos[] ArenaToWorld; // 4
            public Pos Orbit;
            public byte Curves;
            public float RadiusShare;
            public float MiddleHeight;
            public float EdgeHeight;
            public float TurnLimit;
            public byte CurveAllAxes;
        }

        public class Camera_Point // Point 0x1C02
        {
            public uint Flags;
            public float Rate;
            public float Offset;
            public Pos Point;
        }

        public class Camera_Line // Line 0x1C03
        {
            public uint Flags;
            public float Rate;
            public float Offset;
            public Pos LineStart;
            public Pos LineEnd;
        }

        public class Camera_Path // Polygonal chain 0x1C04
        {
            public uint Flags;
            public float Rate;
            public float Offset;
            public Pos[] Points; // uint vectorAmount 
            public float[] Lengths;
            public float[] Steps;
        }

        public class Camera_Main // NULL 0x1C05
        {
            // Can't be read because methods are set to NULL
        }

        public class Camera_Spline // Spline 0x1C06
        {
            public int Flags;
            public float Rate;
            public float Offset;
            public uint Count;
            public float Step;
            public Pos[] Samples; // Count * 2
            public float[] Lengths;
            public float[] Steps;
            public short SplineFlags;
        }

        public class Camera_SplineArm // Unused, spits errors, zooms out to a certain angle and stays in place while in the trigger
        {
            public uint Flags;
            public float Rate;
            public float Offset;
        }

        public class Camera_Point2 // Point (Ukafight) 0x1C0B
        {
            public uint Flags;
            public float Rate;
            public float Offset;
            public Pos Point;
            public float Distance;
            public byte Mode;
        }

        public class Camera_Orbit // Unused, fixed angle or distance from player?
        {
            public byte Flag1;
            public byte Flag2;
            public byte Flag3;
            public byte Flag4;
        }

        public class Camera_Line2 // Line (Gpa12/Throne) 0x1C0D
        {
            public uint Flags;
            public float Rate;
            public float Offset;
            public Pos LineStart;
            public Pos LineEnd;
            public float NearDistance;
            public float FarDistance;
        }

        public class Camera_Keyed // Empty 0x1C0E
        {
            // Nothing, method empty
        }

        public class Camera_Zone // Zone 0x1C0F
        {
            public Pos[] CameraBox; //5
            public Pos[] TargetBox; //5
        }
    }
}
