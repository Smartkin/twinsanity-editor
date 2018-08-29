using System;

namespace Twinsanity
{
    public class GameObject : BaseItem
    {
        public new string NodeName = "GameObject";

        public uint Class1, Class2, Class3; // ??;??;??;??;Pairs;Scripts;GameObjects;SomeShit;Sounds;00;00;00
        public string Name = "";
        public int UnkI32Number;
        public int OGINumber;
        public int AnimationNumber;
        public int ScriptNumber;
        public int GameObjectNumber;
        public int SoundNumber;
        public uint[] UnkI32;
        public ushort[] OGI;
        public ushort[] Animation;
        public ushort[] Script;
        public ushort[] _GameObject;
        public ushort[] Sound;
        public uint ParametersHeader = 131328; // Inst;Pos;Path;00
        public uint ParametersUnkI32 = 270;
        public int ParametersUnkI321Number = 0;
        public int ParametersUnkI322Number = 1;
        public int ParametersUnkI323Number = 2;
        public uint[] ParametersUnkI321;
        public float[] ParametersUnkI322 = new float[] { 1.0f };
        public uint[] ParametersUnkI323 = new uint[] { 0, 0 };
        public uint ChildFlag = 0;
        public int ChildObjectNumber;
        public int ChildOGINumber;
        public int ChildAnimationNumber;
        public int ChildID4Number;
        public int ChildScriptNumber;
        public int ChildUnknownNumber;
        public int ChildSoundNumber;
        public ushort[] ChildObject;
        public ushort[] ChildOGI;
        public ushort[] ChildAnimation;
        public ushort[] ChildID4;
        public ushort[] ChildScript;
        public ushort[] ChildUnknown;
        public ushort[] ChildSound;
        public int ScriptLength;
        public ushort[] ScriptParameters;
        public byte[] ScriptArray;

        public override void UpdateStream()
        {
            System.IO.MemoryStream NewStream = new System.IO.MemoryStream();
            System.IO.BinaryWriter NSWriter = new System.IO.BinaryWriter(NewStream);
            NSWriter.Write(Class1);
            byte b = (byte)Math.Max(OGINumber, AnimationNumber);
            Class2 = b;
            b = (byte)ScriptNumber;
            Class2 += (uint)b * 256;
            b = (byte)GameObjectNumber;
            Class2 += (uint)b * 256 * 256;
            b = (byte)UnkI32Number;
            Class2 += (uint)b * 256 * 256 * 256;
            NSWriter.Write(Class2);
            Class3 = (uint)SoundNumber;
            NSWriter.Write(Class3);
            NSWriter.Write(Name.Length);
            for (int i = 0; i <= Name.Length - 1; i++)
                NSWriter.Write(Name[i]);
            NSWriter.Write(UnkI32Number);
            for (int i = 0; i <= UnkI32Number - 1; i++)
                NSWriter.Write(UnkI32[i]);
            NSWriter.Write(OGINumber);
            for (int i = 0; i <= OGINumber - 1; i++)
                NSWriter.Write(OGI[i]);
            NSWriter.Write(AnimationNumber);
            for (int i = 0; i <= AnimationNumber - 1; i++)
                NSWriter.Write(Animation[i]);
            NSWriter.Write(ScriptNumber);
            for (int i = 0; i <= ScriptNumber - 1; i++)
                NSWriter.Write(Script[i]);
            NSWriter.Write(GameObjectNumber);
            for (int i = 0; i <= GameObjectNumber - 1; i++)
                NSWriter.Write(_GameObject[i]);
            NSWriter.Write(SoundNumber);
            for (int i = 0; i <= SoundNumber - 1; i++)
                NSWriter.Write(Sound[i]);
            b = (byte)ParametersUnkI321Number;
            ParametersHeader = b;
            b = (byte)ParametersUnkI322Number;
            ParametersHeader += (uint)b * 256;
            b = (byte)ParametersUnkI323Number;
            ParametersHeader += (uint)b * 256 * 256;
            if (ParametersHeader > 0)
            {
                NSWriter.Write(ParametersHeader);
                NSWriter.Write(ParametersUnkI32);
                NSWriter.Write(ParametersUnkI321Number);
                for (int i = 0; i <= ParametersUnkI321Number - 1; i++)
                    NSWriter.Write(ParametersUnkI321[i]);
                NSWriter.Write(ParametersUnkI322Number);
                for (int i = 0; i <= ParametersUnkI322Number - 1; i++)
                    NSWriter.Write(ParametersUnkI322[i]);
                NSWriter.Write(ParametersUnkI323Number);
                for (int i = 0; i <= ParametersUnkI323Number - 1; i++)
                    NSWriter.Write(ParametersUnkI323[i]);
            }
            NSWriter.Write(ChildFlag);
            if (ChildFlag > 0)
            {
                string str = Convert.ToString(ChildFlag, 2);
                while (str.Length < 8)
                    str = "0" + str;
                bool GameObjectFlag = (str[7] == '1');
                bool OGIFlag = (str[6] == '1');
                bool AnimationFlag = (str[5] == '1');
                bool ID4Flag = (str[4] == '1');
                bool ScriptFlag = (str[3] == '1');
                bool UnknownFlag = (str[2] == '1');
                bool SoundFlag = (str[1] == '1');
                if (GameObjectFlag)
                {
                    NSWriter.Write(ChildObjectNumber);
                    for (int i = 0; i <= ChildObjectNumber - 1; i++)
                        NSWriter.Write(ChildObject[i]);
                }
                if (OGIFlag)
                {
                    NSWriter.Write(ChildOGINumber);
                    for (int i = 0; i <= ChildOGINumber - 1; i++)
                        NSWriter.Write(ChildOGI[i]);
                }
                if (AnimationFlag)
                {
                    NSWriter.Write(ChildAnimationNumber);
                    for (int i = 0; i <= ChildAnimationNumber - 1; i++)
                        NSWriter.Write(ChildAnimation[i]);
                }
                if (ID4Flag)
                {
                    NSWriter.Write(ChildID4Number);
                    for (int i = 0; i <= ChildID4Number - 1; i++)
                        NSWriter.Write(ChildID4[i]);
                }
                if (ScriptFlag)
                {
                    NSWriter.Write(ChildScriptNumber);
                    for (int i = 0; i <= ChildScriptNumber - 1; i++)
                        NSWriter.Write(ChildScript[i]);
                }
                if (UnknownFlag)
                {
                    NSWriter.Write(ChildUnknownNumber);
                    for (int i = 0; i <= ChildUnknownNumber - 1; i++)
                        NSWriter.Write(ChildUnknown[i]);
                }
                if (SoundFlag)
                {
                    NSWriter.Write(ChildSoundNumber);
                    for (int i = 0; i <= ChildSoundNumber - 1; i++)
                        NSWriter.Write(ChildSound[i]);
                }
                NSWriter.Write(ScriptLength);
                if (ScriptLength > 1)
                {
                    for (int i = 0; i <= 17; i++)
                        NSWriter.Write(ScriptParameters[i]);
                }
                NSWriter.Write(ScriptArray);
            }
            ByteStream = NewStream;
            Size = (uint)ByteStream.Length;
        }

        protected override void DataUpdate()
        {
            System.IO.BinaryReader BSReader = new System.IO.BinaryReader(ByteStream);
            ByteStream.Position = 0;
            Class1 = BSReader.ReadUInt32();
            Class2 = BSReader.ReadUInt32();
            Class3 = BSReader.ReadUInt32();
            int Len = BSReader.ReadInt32();
            Name = new string(BSReader.ReadChars(Len));

            UnkI32Number = BSReader.ReadInt32();
            Array.Resize(ref UnkI32, UnkI32Number);
            for (int i = 0; i <= UnkI32Number - 1; i++)
                UnkI32[i] = BSReader.ReadUInt32();

            OGINumber = BSReader.ReadInt32();
            Array.Resize(ref OGI, OGINumber);
            for (int i = 0; i <= OGINumber - 1; i++)
                OGI[i] = BSReader.ReadUInt16();

            AnimationNumber = BSReader.ReadInt32();
            Array.Resize(ref Animation, AnimationNumber);
            for (int i = 0; i <= AnimationNumber - 1; i++)
                Animation[i] = BSReader.ReadUInt16();

            ScriptNumber = BSReader.ReadInt32();
            Array.Resize(ref Script, ScriptNumber);
            for (int i = 0; i <= ScriptNumber - 1; i++)
                Script[i] = BSReader.ReadUInt16();

            GameObjectNumber = BSReader.ReadInt32();
            Array.Resize(ref _GameObject, GameObjectNumber);
            for (int i = 0; i <= GameObjectNumber - 1; i++)
                _GameObject[i] = BSReader.ReadUInt16();

            SoundNumber = BSReader.ReadInt32();
            Array.Resize(ref Sound, SoundNumber);
            for (int i = 0; i <= SoundNumber - 1; i++)
                Sound[i] = BSReader.ReadUInt16();

            uint choose = BSReader.ReadUInt32();
            if (choose > 255)
            {
                ParametersHeader = choose;
                ParametersUnkI32 = BSReader.ReadUInt32();

                ParametersUnkI321Number = BSReader.ReadInt32();
                Array.Resize(ref ParametersUnkI321, ParametersUnkI321Number);
                for (int i = 0; i <= ParametersUnkI321Number - 1; i++)
                    ParametersUnkI321[i] = BSReader.ReadUInt32();

                ParametersUnkI322Number = BSReader.ReadInt32();
                Array.Resize(ref ParametersUnkI322, ParametersUnkI322Number);
                for (int i = 0; i <= ParametersUnkI322Number - 1; i++)
                    ParametersUnkI322[i] = BSReader.ReadSingle();

                ParametersUnkI323Number = BSReader.ReadInt32();
                Array.Resize(ref ParametersUnkI323, ParametersUnkI323Number);
                for (int i = 0; i <= ParametersUnkI323Number - 1; i++)
                    ParametersUnkI323[i] = BSReader.ReadUInt32();
                choose = BSReader.ReadUInt32();
            }
            else
            {
                ParametersHeader = 0;
                ParametersUnkI32 = 0;
                ParametersUnkI321Number = 0;
                ParametersUnkI321 = new uint[] { };
                ParametersUnkI322Number = 0;
                ParametersUnkI322 = new float[] { };
                ParametersUnkI323Number = 0;
                ParametersUnkI323 = new uint[] { };
            }
            ChildFlag = choose;
            if (choose > 0)
            {
                string str = Convert.ToString(ChildFlag, 2);
                while (str.Length < 8)
                    str = "0" + str;
                bool GameObjectFlag = (str[7] == '1');
                bool OGIFlag = (str[6] == '1');
                bool AnimationFlag = (str[5] == '1');
                bool ID4Flag = (str[4] == '1');
                bool ScriptFlag = (str[3] == '1');
                bool UnknownFlag = (str[2] == '1');
                bool SoundFlag = (str[1] == '1');
                if (GameObjectFlag)
                {
                    ChildObjectNumber = BSReader.ReadInt32();
                    Array.Resize(ref ChildObject, ChildObjectNumber);
                    for (int i = 0; i <= ChildObjectNumber - 1; i++)
                        ChildObject[i] = BSReader.ReadUInt16();
                }
                if (OGIFlag)
                {
                    ChildOGINumber = BSReader.ReadInt32();
                    Array.Resize(ref ChildOGI, ChildOGINumber);
                    for (int i = 0; i <= ChildOGINumber - 1; i++)
                        ChildOGI[i] = BSReader.ReadUInt16();
                }
                if (AnimationFlag)
                {
                    ChildAnimationNumber = BSReader.ReadInt32();
                    Array.Resize(ref ChildAnimation, ChildAnimationNumber);
                    for (int i = 0; i <= ChildAnimationNumber - 1; i++)
                        ChildAnimation[i] = BSReader.ReadUInt16();
                }
                if (ID4Flag)
                {
                    ChildID4Number = BSReader.ReadInt32();
                    Array.Resize(ref ChildID4, ChildID4Number);
                    for (int i = 0; i <= ChildID4Number - 1; i++)
                        ChildID4[i] = BSReader.ReadUInt16();
                }
                if (ScriptFlag)
                {
                    ChildScriptNumber = BSReader.ReadInt32();
                    Array.Resize(ref ChildScript, ChildScriptNumber);
                    for (int i = 0; i <= ChildScriptNumber - 1; i++)
                        ChildScript[i] = BSReader.ReadUInt16();
                }
                if (UnknownFlag)
                {
                    ChildUnknownNumber = BSReader.ReadInt32();
                    Array.Resize(ref ChildUnknown, ChildUnknownNumber);
                    for (int i = 0; i <= ChildUnknownNumber - 1; i++)
                        ChildUnknown[i] = BSReader.ReadUInt16();
                }
                if (SoundFlag)
                {
                    ChildSoundNumber = BSReader.ReadInt32();
                    Array.Resize(ref ChildSound, ChildSoundNumber);
                    for (int i = 0; i <= ChildSoundNumber - 1; i++)
                        ChildSound[i] = BSReader.ReadUInt16();
                }
                ScriptLength = (int)BSReader.ReadUInt32();
                if (ScriptLength > 1)
                {
                    Array.Resize(ref ScriptParameters, 18);
                    for (int i = 0; i <= 17; i++)
                        ScriptParameters[i] = BSReader.ReadUInt16();
                }
                ScriptArray = BSReader.ReadBytes((int)(ByteStream.Length - ByteStream.Position));
            }
        }
    }
}
