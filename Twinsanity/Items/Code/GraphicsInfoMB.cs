using System;
using System.IO;
using System.Drawing;
using System.Text;
using System.Collections.Generic;

namespace Twinsanity
{
    public class GraphicsInfoMB : TwinsItem
    {

        public GraphicsInfo OGI;
        public List<byte[]> SurfData;
        public GHG_Actor_MB GHG;

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Data);
        }

        public override void Load(BinaryReader reader, int size)
        {
            long pre_pos = reader.BaseStream.Position;

            uint Version = reader.ReadUInt32();
            uint GHG_Size = reader.ReadUInt32(); // Size of GHG file at the end
            OGI = new GraphicsInfo();
            OGI.Load(reader, size);

            SurfData = new List<byte[]>();
            for (int i = 0; i < OGI.CollisionData.Length; i++)
            {
                SurfData.Add(reader.ReadBytes(0x10));
            }

            GHG = new GHG_Actor_MB();
            //GHG.Load(reader, (int)GHG_Size); // todo

            reader.BaseStream.Position = pre_pos;
            Data = reader.ReadBytes(size);
        }

        protected override int GetSize()
        {
            return Data.Length;
        }
    }

    public class GHG_Actor_MB : TwinsItem
    {

        protected override int GetSize()
        {
            return (int)FileSize;
        }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(Data);
        }

        // Unknown
        public List<float> Header2;
        public List<float> Header3;
        public long OffsetMatrix2;

        // Known
        private uint FileSize;
        public List<string> Locators;
        public Pos BoundingBoxVector1;
        public Pos BoundingBoxVector2;
        public List<Texture> Textures;
        public List<ExitPoint> ExitPoints;
        public List<Joint> Joints;
        public List<Layer> Layers;

        public List<Struct2> Structs2;

        public override void Load(BinaryReader reader, int size)
        {
            long StartPos = reader.BaseStream.Position;

            FileSize = reader.ReadUInt32();
            uint Zero = reader.ReadUInt32();

            uint TextureCount = reader.ReadUInt32();
            long TexOffset1 = StartPos + reader.ReadUInt32();
            uint Struct2Count = reader.ReadUInt32();
            long Struct2Offset = StartPos + reader.ReadUInt32();
            uint JointCount = reader.ReadUInt32();
            long JointOffset = StartPos + reader.ReadUInt32();
            long OffsetMatrix1 = StartPos + reader.ReadUInt32();
            OffsetMatrix2 = StartPos + reader.ReadUInt32();
            uint JointIDsCount = reader.ReadUInt32();
            long JointIDsOffset = StartPos + reader.ReadUInt32();
            long LocatorOffset = StartPos + reader.ReadUInt32();
            uint LocatorLength = reader.ReadUInt32();
            uint ExitPointIDCount = reader.ReadUInt32();
            long ExitPointIDOffset = StartPos + reader.ReadUInt32();
            uint ExitPointCount = reader.ReadUInt32();
            long ExitPointOffset = StartPos + reader.ReadUInt32();
            uint LayerCount = reader.ReadUInt32();
            long LayerOffset = StartPos + reader.ReadUInt32();

            Header2 = new List<float>();
            for (int i = 0; i < 3; i++)
            {
                Header2.Add(reader.ReadSingle());
            }

            BoundingBoxVector1 = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), 1);
            BoundingBoxVector2 = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), 1);

            Header3 = new List<float>();
            for (int i = 0; i < 5; i++)
            {
                Header3.Add(reader.ReadSingle());
            }
            //Zero = reader.ReadUInt32();
            //Zero = reader.ReadUInt32();

            // Textures
            Textures = new List<Texture>();
            if (TextureCount > 0)
            {
                reader.BaseStream.Position = TexOffset1;
                List<long> TexOffsets = new List<long>();
                for (int i = 0; i < TextureCount; i++)
                {
                    TexOffsets.Add(StartPos + reader.ReadUInt32());
                }
                for (int i = 0; i < TextureCount; i++)
                {
                    reader.BaseStream.Position = TexOffsets[i];
                    Texture tex = new Texture();
                    //tex.Read(reader);
                    Textures.Add(tex);
                }
            }

            // Struct2 todo
            Structs2 = new List<Struct2>();
            if (Struct2Count > 0)
            {
                reader.BaseStream.Position = Struct2Offset;
                List<long> Str2Offsets = new List<long>();
                for (int i = 0; i < Struct2Count; i++)
                {
                    Str2Offsets.Add(StartPos + reader.ReadUInt32());
                }
                for (int i = 0; i < Str2Offsets.Count; i++)
                {
                    reader.BaseStream.Position = Str2Offsets[i];
                    Struct2 str = new Struct2();
                    //str.Read(reader);
                    Structs2.Add(str);
                }
            }

            // Joints
            Joints = new List<Joint>();
            reader.BaseStream.Position = JointOffset;
            for (int i = 0; i < JointCount; i++)
            {
                Joint j = new Joint();
                j.Read(reader, StartPos);
                Joints.Add(j);
            }
            reader.BaseStream.Position = OffsetMatrix1; // second matrix for every joint (rest or bind pose?)
            for (int i = 0; i < JointCount; i++)
            {
                Joints[i].ReadMatrix(reader);
            }
            reader.BaseStream.Position = OffsetMatrix2; // todo?


            // List of indexes, called Joint-IDs
            reader.BaseStream.Position = JointIDsOffset;
            for (byte i = 0; i < JointIDsCount; i++)
            {
                Joints[reader.ReadByte()].ID = i;
            }

            // Exit Points
            ExitPoints = new List<ExitPoint>();
            reader.BaseStream.Position = ExitPointOffset;
            for (int i = 0; i < ExitPointCount; i++)
            {
                ExitPoint Point = new ExitPoint();
                Point.Read(reader, StartPos);
                ExitPoints.Add(Point);
            }
            // List of the exit point indexes that the game uses (zero padding to 0x10 after this list)
            reader.BaseStream.Position = ExitPointIDOffset;
            for (byte i = 0; i < ExitPointIDCount; i++)
            {
                byte index = reader.ReadByte();
                if (index != 0xFF) // only in pathtree
                {
                    ExitPoints[index].ID = i;
                }
            }

            // Locators (joints + defaultLayer? followed by exit points (POIs)) not sure if needed since joints and exit points already have pointers to these names
            Locators = new List<string>();
            reader.BaseStream.Position = LocatorOffset;
            uint CharCount = 0;
            List<char> Word = new List<char>();
            while (CharCount < LocatorLength)
            {
                char letter = reader.ReadChar();
                CharCount++;
                if (letter == '\0')
                {
                    Locators.Add(new string(Word.ToArray()));
                    Word = new List<char>();
                }
                else
                {
                    Word.Add(letter);
                }
            }

            // Layers (always one?)
            Layers = new List<Layer>();
            reader.BaseStream.Position = LayerOffset;
            for (int i = 0; i < LayerCount; i++)
            {
                Layer Lay = new Layer();
                //Lay.Read(reader, JointCount);
                Layers.Add(Lay);
            }
        }

        public class Texture
        {
            public List<Color> ColorList;
            public int Width;
            public int Height;
            public uint PixelFormat;

            public void Read(BinaryReader reader)
            {
                Width = reader.ReadUInt16();
                PixelFormat = reader.ReadUInt16(); // 1 contains palette, 0 doesn't (0: RGB24? 1: PSMT8?)
                Height = reader.ReadUInt16();
                ColorList = new List<Color>();

                switch (PixelFormat)
                {
                    case 0:
                        {
                            // RGB24
                            reader.BaseStream.Position += 0xEA;

                            byte R, G, B;
                            for (int i = 0; i < Width * Height; i++)
                            {
                                R = reader.ReadByte();
                                G = reader.ReadByte();
                                B = reader.ReadByte();
                                ColorList.Add(Color.FromArgb(255, R, G, B));
                            }
                        }
                        break;
                    case 1:
                        {
                            // PSMT8?
                            reader.BaseStream.Position += 0x7A;

                            Color[] ColorPal = new Color[0x100];
                            //reader.BaseStream.Position = 0x120;
                            byte R, G, B, A;
                            for (int i = 0; i < 0x100; i++)
                            {
                                R = reader.ReadByte();
                                G = reader.ReadByte();
                                B = reader.ReadByte();
                                A = reader.ReadByte();
                                A = (byte)(A << 1);
                                ColorPal[i] = Color.FromArgb(A, R, G, B);
                            }

                            reader.BaseStream.Position += 0x20;
                            reader.BaseStream.Position += 0xD0;

                            // Well, this works, but only until 64x64
                            //Twinsanity.Texture.SwapPalette2(ref ColorPal, Width, Height);

                            for (int i = 0; i < Width * Height; i++)
                            {
                                byte PalID = reader.ReadByte();
                                ColorList.Add(ColorPal[PalID]);
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("new format " + PixelFormat);
                        throw new NotImplementedException();
                }


            }
        }

        public class Struct2
        {
            // fixed length? of 0x208, (zero padding to 0x10 alignment between elements, but not after the last one)
            public byte[] Padding; // zero padding?

            public void Read(BinaryReader reader)
            {
                Padding = reader.ReadBytes(0x120);
            }
        }

        public class ExitPoint
        {
            public Pos[] Matrix; //4
            public byte ID;
            public string Name; // from offset
            public uint Joint;
            public uint UnkInt2; // Zero?
            public uint UnkInt3; // Zero?

            public void Read(BinaryReader reader, long StartPos)
            {
                Matrix = new Pos[4];
                for (int i = 0; i < 4; i++)
                {
                    Matrix[i] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                }
                uint NameOffset = reader.ReadUInt32();
                Joint = reader.ReadUInt32();
                UnkInt2 = reader.ReadUInt32(); // zero?
                UnkInt3 = reader.ReadUInt32(); // zero?
                long pos = reader.BaseStream.Position;

                reader.BaseStream.Position = StartPos + NameOffset;
                bool wordend = false;
                uint CharCount = 0;
                List<char> Word = new List<char>();
                while (!wordend)
                {
                    char letter = reader.ReadChar();
                    CharCount++;
                    if (letter == '\0')
                    {
                        wordend = true;
                        Name = new string(Word.ToArray());
                    }
                    else
                    {
                        Word.Add(letter);
                    }
                }

                reader.BaseStream.Position = pos;
            }
        }

        public class Joint
        {
            public Pos[] Matrix; //4
            public Pos[] Matrix2; //4
            public byte? ID;
            public string Name; // from offset
            public uint UnkInt1;
            public uint UnkInt2;
            public uint UnkInt3;
            public byte ParentJoint; // if ParentJoint == 255 then it has no parent
            public byte UnkByte;
            public byte[] UnkPad;
            public uint UnkInt5;
            public uint UnkInt6;
            public uint UnkInt7;

            public void Read(BinaryReader reader, long StartPos)
            {
                Matrix = new Pos[4];
                for (int i = 0; i < 4; i++)
                {
                    Matrix[i] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                }
                UnkInt1 = reader.ReadUInt32();
                UnkInt2 = reader.ReadUInt32();
                UnkInt3 = reader.ReadUInt32();
                uint NameOffset = reader.ReadUInt32();
                ParentJoint = reader.ReadByte();
                UnkByte = reader.ReadByte();
                UnkPad = reader.ReadBytes(2);
                UnkInt5 = reader.ReadUInt32();
                UnkInt6 = reader.ReadUInt32();
                UnkInt7 = reader.ReadUInt32();

                long pos = reader.BaseStream.Position;

                reader.BaseStream.Position = StartPos + NameOffset;
                bool wordend = false;
                uint CharCount = 0;
                List<char> Word = new List<char>();
                while (!wordend)
                {
                    char letter = reader.ReadChar();
                    CharCount++;
                    if (letter == '\0')
                    {
                        wordend = true;
                        Name = new string(Word.ToArray());
                    }
                    else
                    {
                        Word.Add(letter);
                    }
                }

                reader.BaseStream.Position = pos;
            }
            public void ReadMatrix(BinaryReader reader)
            {
                Matrix2 = new Pos[4];
                for (int i = 0; i < 4; i++)
                {
                    Matrix2[i] = new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                }
            }
        }

        public class Layer
        {
            public string LayerName;
            public List<RigidModel> RigidModels;
            public List<Skin> Skins;
            public List<BlendSkin> BlendSkins;

            public void Read(BinaryReader reader, uint JointCount, long StartPos)
            {
                RigidModels = new List<RigidModel>();
                Skins = new List<Skin>();
                BlendSkins = new List<BlendSkin>();

                long NameOffset = StartPos + reader.ReadUInt32();
                long RigidModelOffset = StartPos + reader.ReadUInt32();
                long SkinOffset = StartPos + reader.ReadUInt32();
                long UnkOffset = StartPos + reader.ReadUInt32(); // not used in any of the models so far
                long BlendSkinOffset = StartPos + reader.ReadUInt32();

                reader.BaseStream.Position = NameOffset;
                LayerName = new string(reader.ReadChars((int)0xC));

                if (RigidModelOffset != 0)
                {
                    reader.BaseStream.Position = RigidModelOffset;
                    long[] Offsets = new long[JointCount];
                    for (int i = 0; i < JointCount; i++)
                    {
                        Offsets[i] = StartPos + reader.ReadUInt32();
                    }
                    for (int d = 0; d < JointCount; d++)
                    {
                        if (Offsets[d] != 0)
                        {
                            long NextJointOffset = 0;
                            for (int j = d; j < JointCount; j++)
                            {
                                if (Offsets[j] != 0 && Offsets[j] > Offsets[d])
                                {
                                    NextJointOffset = Offsets[j];
                                    break;
                                }
                            }

                            // Model exists and is attached to joint d
                            reader.BaseStream.Position = Offsets[d];
                            uint[] SecFlags = new uint[2];
                            long[] SecOffsets = new long[2];
                            float[] SecFloats = new float[18];
                            for (int i = 0; i < SecFlags.Length; i++)
                            {
                                SecFlags[i] = reader.ReadUInt32();
                            }
                            // Offset of next model? then offset of this model
                            for (int i = 0; i < SecOffsets.Length; i++)
                            {
                                SecOffsets[i] = StartPos + reader.ReadUInt32();
                            }
                            for (int i = 0; i < SecFloats.Length; i++)
                            {
                                SecFloats[i] = reader.ReadSingle();
                            }
                            for (int x = 0; x < SecOffsets.Length; x++)
                            {
                                if (SecOffsets[x] != 0)
                                {
                                    long NextOffset = SecOffsets[x];
                                    bool LeafEnd = false;
                                    while (!LeafEnd)
                                    {
                                        reader.BaseStream.Position = NextOffset;
                                        NextOffset = reader.ReadUInt32();
                                        if (NextOffset == 0)
                                        {
                                            LeafEnd = true;
                                        }

                                        uint[] ThirdOffsets = new uint[3]; // offset next submodel, leaf ID?, offset this submodel, flag and then there's padding to 0x10?
                                        for (int t = 0; t < ThirdOffsets.Length; t++)
                                        {
                                            ThirdOffsets[t] = reader.ReadUInt32();
                                        }
                                        if (ThirdOffsets[1] != 0)
                                        {
                                            reader.BaseStream.Position = ThirdOffsets[1];
                                            RigidModel Model = new RigidModel();
                                            //Model.Read(reader, NextOffset, NextJointOffset, SkinOffset, BlendSkinOffset);
                                            Model.Joint = d;
                                            RigidModels.Add(Model);
                                        }

                                    }

                                }
                            }
                        }
                    }
                }

                if (SkinOffset != 0)
                {
                    reader.BaseStream.Position = SkinOffset;
                    byte[] Flags = reader.ReadBytes(4);
                    Flags = reader.ReadBytes(4);
                    reader.ReadUInt32();
                    long MainOffset = StartPos + reader.ReadUInt32();
                    float[] SecFloats = new float[19];
                    for (int i = 0; i < SecFloats.Length; i++)
                    {
                        SecFloats[i] = reader.ReadSingle();
                    }
                    reader.BaseStream.Position = MainOffset;

                    bool LeafEnd = false;
                    while (!LeafEnd)
                    {
                        long[] Offsets = new long[4];
                        for (int i = 0; i < Offsets.Length; i++)
                        {
                            Offsets[i] = StartPos + reader.ReadUInt32(); // second and third are flags?
                        }

                        if (Offsets[3] != 0)
                        {
                            reader.BaseStream.Position = Offsets[3];
                            Skin skin = new Skin();
                            //skin.Read(reader, Offsets[0], SkinOffset, BlendSkinOffset);
                            Skins.Add(skin);
                        }

                        if (Offsets[0] != 0)
                        {
                            reader.BaseStream.Position = Offsets[0];
                        }
                        else
                        {
                            LeafEnd = true;
                        }
                    }
                }

                if (UnkOffset != 0)
                {
                    throw new NotImplementedException();
                }

                if (BlendSkinOffset != 0)
                {
                    reader.BaseStream.Position = BlendSkinOffset;
                    byte[] Flags = reader.ReadBytes(4);
                    reader.ReadUInt32();
                    reader.ReadUInt32();
                    long MainOffset = StartPos + reader.ReadUInt32();
                    float[] SecFloats = new float[19];
                    for (int i = 0; i < SecFloats.Length; i++)
                    {
                        SecFloats[i] = reader.ReadSingle();
                    }
                    reader.BaseStream.Position = MainOffset;

                    bool LeafEnd = false;
                    while (!LeafEnd)
                    {
                        long[] Offsets = new long[4];
                        for (int i = 0; i < Offsets.Length; i++)
                        {
                            Offsets[i] = StartPos + reader.ReadUInt32(); // second and third are flags?
                        }

                        if (Offsets[3] != 0)
                        {
                            reader.BaseStream.Position = Offsets[3];
                            BlendSkin skin = new BlendSkin();
                            //skin.Read(reader, Offsets[0], SkinOffset, BlendSkinOffset);
                            BlendSkins.Add(skin);
                        }

                        if (Offsets[0] != 0)
                        {
                            reader.BaseStream.Position = Offsets[0];
                        }
                        else
                        {
                            LeafEnd = true;
                        }
                    }
                }

            }
        }

        public class RigidModel
        {
            public int Joint;
            public uint GroupCount;
            public byte[] Header; // 0x34?
            public byte[] Remain; // 0x1C? and zero padding to 0x10 alignment afterwards
            public List<Pos> Vertices = new List<Pos>();
            public List<byte[]> UVs = new List<byte[]>();
            public List<Color> Colors = new List<Color>();

            public void Read(BinaryReader reader, uint NextModelOffset, uint NextJointOffset, uint SkinOffset, uint BlendSkinOffset)
            {
                GroupCount = 0;
                Header = reader.ReadBytes(0x34);
                // todo: figure out group count and how to navigate through them, this just loads the first vertex group
                byte[] DataHeader = new byte[4];
                bool LastData = false;
                bool LastGroup = false;
                while (!LastGroup)
                {
                    if (LastData)
                    {
                        reader.BaseStream.Position += 0x18;
                        LastData = false;
                    }
                    if (reader.BaseStream.Position >= reader.BaseStream.Length - 8)
                    {
                        LastData = true;
                        LastGroup = true;
                        break;
                    }

                    // dunno how to get group count, so just guess that it ends from offset
                    if (NextModelOffset != 0 && NextModelOffset - reader.BaseStream.Position < 0x1C + 0x11)
                    {
                        // data ends and model ends before next model
                        LastData = true;
                        LastGroup = true;
                        break;
                    }
                    if (NextJointOffset != 0 && NextJointOffset - reader.BaseStream.Position < 0x1C + 0x11)
                    {
                        // ditto for next joint
                        LastData = true;
                        LastGroup = true;
                        break;
                    }
                    if (SkinOffset != 0 && SkinOffset - reader.BaseStream.Position < 0x1C + 0x11)
                    {
                        // ditto for offset
                        LastData = true;
                        LastGroup = true;
                        break;
                    }
                    if (BlendSkinOffset != 0 && BlendSkinOffset - reader.BaseStream.Position < 0x1C + 0x11)
                    {
                        // ditto for offset
                        LastData = true;
                        LastGroup = true;
                        break;
                    }

                    byte[] SectionStart = reader.ReadBytes(4);
                    if (SectionStart[1] == 0x80 || SectionStart[1] == 0xC0)  // sometimes the header is just the last 4 bytes??
                    {
                        reader.BaseStream.Position -= 4;
                    }
                    DataHeader = reader.ReadBytes(4);
                    if (DataHeader[1] == 0xC0)
                    {
                        // next group after this data
                        LastData = true;
                        GroupCount++;
                    }
                    else if (DataHeader[1] == 0x00)
                    {
                        LastData = true;
                        LastGroup = true;
                        break;
                    }
                    byte Count = DataHeader[2];
                    switch (DataHeader[0])
                    {
                        default:
                            Console.WriteLine("unknown header " + DataHeader[0]);
                            throw new Exception(); // model parsed wrong most likely
                        case 2:
                            {
                                // usually just one byte of stuff
                                for (int i = 0; i < Count; i++)
                                {
                                    reader.ReadUInt32();
                                    reader.ReadUInt32();
                                }
                            }
                            break;
                        case 3:
                            {
                                // Vertices + UVs?
                                for (int i = 0; i < Count; i++)
                                {
                                    Vertices.Add(new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), 1));
                                    UVs.Add(new byte[4] { reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte() });
                                }
                            }
                            break;
                        case 4:
                            {
                                // 8 bytes per vertex
                                for (int i = 0; i < Count; i++)
                                {

                                    byte R, G, B, A;
                                    R = reader.ReadByte();
                                    G = reader.ReadByte();
                                    B = reader.ReadByte();
                                    A = reader.ReadByte();
                                    A = (byte)(A << 1);
                                    //Colors.Add(Color.FromArgb(A, R, G, B));
                                    Colors.Add(Color.FromArgb(255, 128, 128, 128));
                                    reader.ReadUInt32();
                                }
                            }
                            break;
                        case 5:
                            {
                                // The last type (6) in model.read?
                                for (int i = 0; i < Count; i++)
                                {
                                    byte R, G, B, A;
                                    R = reader.ReadByte();
                                    G = reader.ReadByte();
                                    B = reader.ReadByte();
                                    A = reader.ReadByte();
                                    A = (byte)(A << 1);
                                    //Colors.Add(Color.FromArgb(255, 128, 128, 128));
                                }
                            }
                            break;
                    }
                }

                Remain = reader.ReadBytes(0x1C);
            }
        }

        public class Skin
        {
            public uint GroupCount;
            public byte[] Header;
            public byte[] Remain; // 0x04? and zero padding to 0x10 alignment afterwards
            public List<Pos> Vertices = new List<Pos>();
            public List<byte[]> UVs = new List<byte[]>();
            public List<Color> Colors = new List<Color>();
            public List<byte[]> UnkStruct = new List<byte[]>();
            public List<Pos> AltVerts = new List<Pos>();

            public void Read(BinaryReader reader, uint NextModelOffset, uint SkinOffset, uint BlendSkinOffset)
            {
                //Header = reader.ReadBytes(0x80); // temp

                // todo: some kind of list of one or more sub-things
                UnkStruct = new List<byte[]>();
                uint HeaderVar = reader.ReadUInt32();
                List<uint> HeaderContinue = new List<uint>() { 0x10070000, 0x30080004, 0x50000000 };
                while (HeaderContinue.Contains(HeaderVar))
                {
                    UnkStruct.Add(reader.ReadBytes(0x0C));
                    HeaderVar = reader.ReadUInt32();
                }
                reader.ReadBytes(0x0C);
                reader.BaseStream.Position += 0x34;

                bool LastGroup = false;
                bool LastData = false;
                uint Buffer;
                while (!LastGroup)
                {
                    GroupCount++;
                    byte[] SectionStart = reader.ReadBytes(4);
                    if (SectionStart[1] == 0x80 || SectionStart[1] == 0xC0)  // sometimes the header is just the last 4 bytes??
                    {
                        reader.BaseStream.Position -= 4;
                    }
                    byte[] DataHeader = reader.ReadBytes(4);

                    if (DataHeader[0] == 3)
                    {
                        for (int i = 0; i < DataHeader[2] / 2; i++)
                        {
                            Vertices.Add(new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), 1));
                            UVs.Add(new byte[4] { reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte() });
                            // float, float, flags, float (0, 1, -1)
                            float[] vars = new float[4] { reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle() };
                            AltVerts.Add(new Pos(vars[0], vars[1], vars[3], 1));
                            Colors.Add(Color.FromArgb(255, 128, 128, 128));
                        }
                    }

                    Buffer = reader.ReadUInt32();
                    List<uint> BufferNext = new List<uint>() { 0x17000000, 0x14000302 };
                    while (!BufferNext.Contains(Buffer))
                    {
                        Buffer = reader.ReadUInt32();
                    }

                    if (NextModelOffset != 0 && NextModelOffset - reader.BaseStream.Position < 0x14)
                    {
                        LastGroup = true;
                    }
                    else if (BlendSkinOffset != 0 && BlendSkinOffset - reader.BaseStream.Position < 0x14)
                    {
                        LastGroup = true;
                    }
                    else if (reader.BaseStream.Position > reader.BaseStream.Length - 0x14)
                    {
                        LastGroup = true;
                    }
                    else
                    {
                        reader.BaseStream.Position += 0x20;
                    }

                }

            }
        }

        public class BlendSkin
        {
            public uint GroupCount;
            public byte[] Header;
            public byte[] Remain;
            public List<Pos> Vertices = new List<Pos>();
            public List<byte[]> UVs = new List<byte[]>();
            public List<Color> Colors = new List<Color>();
            public List<byte[]> UnkStruct = new List<byte[]>();
            public List<Pos> AltVerts = new List<Pos>();

            public void Read(BinaryReader reader, uint NextModelOffset, uint SkinOffset, uint BlendSkinOffset)
            {
                // todo: some kind of list of one or more sub-things
                UnkStruct = new List<byte[]>();
                uint HeaderVar = reader.ReadUInt32();
                List<uint> HeaderContinue = new List<uint>() { 0x10070000, 0x30080004, 0x50000000 };
                while (HeaderContinue.Contains(HeaderVar))
                {
                    UnkStruct.Add(reader.ReadBytes(0x0C));
                    HeaderVar = reader.ReadUInt32();
                }
                reader.ReadBytes(0x0C);
                reader.BaseStream.Position += 0x30;

                bool LastGroup = false;
                bool LastData = false;
                uint Buffer;
                while (!LastGroup)
                {
                    GroupCount++;
                    byte[] SectionStart = reader.ReadBytes(4);
                    if (SectionStart[1] == 0x80 || SectionStart[1] == 0xC0)  // sometimes the header is just the last 4 bytes??
                    {
                        reader.BaseStream.Position -= 4;
                    }
                    byte[] DataHeader = reader.ReadBytes(4);

                    if (DataHeader[0] == 3)
                    {
                        for (int i = 0; i < DataHeader[2] / 2; i++)
                        {
                            Vertices.Add(new Pos(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), 1));
                            UVs.Add(new byte[4] { reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte() });
                            // float, float, flags, float (0, 1, -1)
                            float[] vars = new float[4] { reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle() };
                            AltVerts.Add(new Pos(vars[0], vars[1], vars[3], 1));
                            Colors.Add(Color.FromArgb(255, 128, 128, 128));
                        }
                    }

                    Buffer = reader.ReadUInt32();
                    List<uint> BufferNext = new List<uint>() { 0x17000000, 0x14000302 };
                    while (!BufferNext.Contains(Buffer))
                    {
                        Buffer = reader.ReadUInt32();
                    }

                    if (NextModelOffset != 0 && NextModelOffset - reader.BaseStream.Position < 0x14)
                    {
                        LastGroup = true;
                    }
                    else if (reader.BaseStream.Position > reader.BaseStream.Length - 0x14)
                    {
                        LastGroup = true;
                    }
                    else
                    {
                        reader.BaseStream.Position += 0x24;
                    }

                }

            }
        }


        public byte[] ToPLY(bool blender = false)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter ply = new StreamWriter(stream) { AutoFlush = true })
                {
                    int vertexcount = 0, polycount = 0;
                    for (int i = 0; i < Layers[0].RigidModels.Count; ++i)
                    {
                        vertexcount += Layers[0].RigidModels[i].Vertices.Count;
                        for (int f = 0; f < Layers[0].RigidModels[i].Vertices.Count - 2; ++f)
                        {
                            //if (SubModels[i].Groups[a].VData[f + 2].CONN != 128)
                            ++polycount;
                        }
                    }
                    for (int i = 0; i < Layers[0].Skins.Count; ++i)
                    {
                        vertexcount += Layers[0].Skins[i].Vertices.Count;
                        for (int f = 0; f < Layers[0].Skins[i].Vertices.Count - 3; ++f)
                        {
                            //if (SubModels[i].Groups[a].VData[f + 2].CONN != 128)
                            ++polycount;
                        }
                    }
                    ply.WriteLine("ply");
                    ply.WriteLine("format ascii 1.0");
                    ply.WriteLine("element vertex {0}", vertexcount);
                    ply.WriteLine("property float x");
                    ply.WriteLine("property float y");
                    ply.WriteLine("property float z");
                    ply.WriteLine("property uchar red");
                    ply.WriteLine("property uchar green");
                    ply.WriteLine("property uchar blue");
                    ply.WriteLine("element face {0}", polycount);
                    if (!blender)
                    {
                        ply.WriteLine("property list uchar int vertex_index");
                    }
                    else
                    {
                        ply.WriteLine("property list uchar int vertex_indices");
                    }
                    ply.WriteLine("end_header");
                    foreach (var s in Layers[0].RigidModels) //vertices
                    {
                        for (int i = 0; i < s.Vertices.Count; ++i)
                        {
                            byte red, green, blue;
                            red = s.Colors[i].R;
                            green = s.Colors[i].G;
                            blue = s.Colors[i].B;
                            //if (g.ShiteHead > 0)
                            //{
                            //    red = (byte)((g.Shit[i] & 0xFF00) >> 8);
                            //    green = (byte)((g.Shit[i] & 0xFF0000) >> 16);
                            //    blue = (byte)((g.Shit[i] & 0xFF000000) >> 24);
                            //}
                            string Line = string.Format("{0} {1} {2} {3} {4} {5}", -s.Vertices[i].X, s.Vertices[i].Y, s.Vertices[i].Z, red, green, blue);
                            Line = Line.Replace(',', '.');
                            ply.WriteLine(Line);
                        }
                    }
                    foreach (var s in Layers[0].Skins)
                    {
                        for (int i = 0; i < s.Vertices.Count; i++)
                        {
                            byte red, green, blue;
                            red = s.Colors[i].R;
                            green = s.Colors[i].G;
                            blue = s.Colors[i].B;
                            string Line = string.Format("{0} {1} {2} {3} {4} {5}", -s.Vertices[i].X, s.Vertices[i].Y, s.Vertices[i].Z, red, green, blue);
                            Line = Line.Replace(',', '.');
                            ply.WriteLine(Line);
                        }
                    }
                    vertexcount = 0;
                    foreach (var s in Layers[0].RigidModels) //polys
                    {
                        for (int i = 0; i < s.Vertices.Count - 2; ++i)
                        {
                            //if (g.VData[i + 2].CONN != 128)
                            ply.WriteLine("3 {0} {1} {2}", vertexcount + ((i & 0x1) == 0x1 ? i + 1 : i + 0), vertexcount + ((i & 0x1) == 0x1 ? i + 0 : i + 1), vertexcount + ((i & 0x1) == 0x1 ? i + 2 : i + 2));
                        }
                        vertexcount += s.Vertices.Count;
                    }
                    foreach (var s in Layers[0].Skins)
                    {
                        for (int i = 0; i < s.Vertices.Count - 3; ++i)
                        {
                            ply.WriteLine("4 {0} {1} {2} {3}", vertexcount + i, vertexcount + i + 1, vertexcount + i + 2, vertexcount + i + 3);
                            i = i + 3;
                        }
                        vertexcount += s.Vertices.Count;
                    }
                    return stream.ToArray();
                }
            }
        }
    }

}

