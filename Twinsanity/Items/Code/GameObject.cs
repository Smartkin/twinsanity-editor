using System;
using System.IO;

namespace Twinsanity
{
    public class GameObject : TwinsItem
    {
        private int size;
        
        public uint Class1 { get; set; } // ??;??;??;??
        public uint Class2 { get; set; } // Pairs;Scripts;GameObjects;SomeShit
        public uint Class3 { get; set; } // Sounds;00;00;00
        public uint UnkBitfield { get; set; }
        public byte[] ScriptSlots { get; set; } = new byte[8]; // Pairs;Scripts;GameObjects;UInt32s;Sounds;00;00;00 (last 3 are potentially a side effect of needing object name's length to be word aligned)
        public uint[] UI32 { get; set; }
        public ushort[] OGIs { get; set; } = new ushort[0];
        public ushort[] Anims { get; set; } = new ushort[0];
        public ushort[] Scripts { get; set; } = new ushort[0];
        public ushort[] Objects { get; set; } = new ushort[0];
        public ushort[] Sounds { get; set; } = new ushort[0];
        public uint PHeader { get; set; } // Inst;Pos;Path;00
        public uint PUI32 { get; set; }
        //private int pUi321.Length = 0;
        //private int pUi322.Length = 1;
        //private int pUi323.Length = 2;
        private uint[] pUi321 = new uint[0];
        private float[] pUi322 = new float[0];
        private uint[] pUi323 = new uint[0];
        private uint flag;
        public ushort[] cObjects = new ushort[0];
        public ushort[] cOGIs = new ushort[0];
        public ushort[] cAnims = new ushort[0];
        public ushort[] cCM = new ushort[0];
        public ushort[] cScripts = new ushort[0];
        public ushort[] cUnk = new ushort[0];
        public ushort[] cSounds = new ushort[0];
        private int scriptLen;
        private ushort[] scriptParams = new ushort[0];
        private byte[] scriptData = new byte[0];

        public string Name { get; set; }

        public override void Save(BinaryWriter writer)
        {
            var sk = writer.BaseStream.Position;

            writer.Write(Class1);
            Class2 = (uint)((byte)Math.Max(OGIs.Length, Anims.Length)
                | ((byte)Scripts.Length << 8)
                | ((byte)Objects.Length << 16)
                | ((byte)UI32.Length << 24));
            writer.Write(Class2);
            Class3 = (uint)Sounds.Length;
            writer.Write(Class3);
            writer.Write(Name.Length);
            writer.Write(Name.ToCharArray(), 0, Name.Length);
            writer.Write(UI32.Length);
            for (int i = 0; i < UI32.Length; ++i)
                writer.Write(UI32[i]);
            writer.Write(OGIs.Length);
            for (int i = 0; i < OGIs.Length; ++i)
                writer.Write(OGIs[i]);
            writer.Write(Anims.Length);
            for (int i = 0; i < Anims.Length; ++i)
                writer.Write(Anims[i]);
            writer.Write(Scripts.Length);
            for (int i = 0; i < Scripts.Length; ++i)
                writer.Write(Scripts[i]);
            writer.Write(Objects.Length);
            for (int i = 0; i < Objects.Length; ++i)
                writer.Write(Objects[i]);
            writer.Write(Sounds.Length);
            for (int i = 0; i < Sounds.Length; ++i)
                writer.Write(Sounds[i]);
            PHeader = (uint)((byte)pUi321.Length
                | (pUi322.Length << 8)
                | (pUi323.Length << 16));
            if (PHeader > 0)
            {
                writer.Write(PHeader);
                writer.Write(PUI32);
                writer.Write(pUi321.Length);
                for (int i = 0; i < pUi321.Length; ++i)
                    writer.Write(pUi321[i]);
                writer.Write(pUi322.Length);
                for (int i = 0; i < pUi322.Length; ++i)
                    writer.Write(pUi322[i]);
                writer.Write(pUi323.Length);
                for (int i = 0; i < pUi323.Length; ++i)
                    writer.Write(pUi323[i]);
            }
            writer.Write(flag);
            if (flag > 0)
            {
                if ((flag & 0x01) != 0)
                {
                    writer.Write(cObjects.Length);
                    for (int i = 0; i < cObjects.Length; ++i)
                        writer.Write(cObjects[i]);
                }
                if ((flag & 0x02) != 0)
                {
                    writer.Write(cOGIs.Length);
                    for (int i = 0; i < cOGIs.Length; ++i)
                        writer.Write(cOGIs[i]);
                }
                if ((flag & 0x04) != 0)
                {
                    writer.Write(cAnims.Length);
                    for (int i = 0; i < cAnims.Length; ++i)
                        writer.Write(cAnims[i]);
                }
                if ((flag & 0x08) != 0)
                {
                    writer.Write(cCM.Length);
                    for (int i = 0; i < cCM.Length; ++i)
                        writer.Write(cCM[i]);
                }
                if ((flag & 0x10) != 0)
                {
                    writer.Write(cScripts.Length);
                    for (int i = 0; i < cScripts.Length; ++i)
                        writer.Write(cScripts[i]);
                }
                if ((flag & 0x20) != 0)
                {
                    writer.Write(cUnk.Length);
                    for (int i = 0; i < cUnk.Length; ++i)
                        writer.Write(cUnk[i]);
                }
                if ((flag & 0x40) != 0)
                {
                    writer.Write(cSounds.Length);
                    for (int i = 0; i < cSounds.Length; ++i)
                        writer.Write(cSounds[i]);
                }
                writer.Write(scriptLen);
                if (scriptLen > 1)
                {
                    for (int i = 0; i < 18; ++i)
                        writer.Write(scriptParams[i]);
                }
                writer.Write(scriptData);
            }
            size = (int)(writer.BaseStream.Position - sk);
        }

        public override void Load(BinaryReader reader, int size)
        {
            var sk = reader.BaseStream.Position;

            UnkBitfield = reader.ReadUInt32();
            for (int i = 0; i < 8; ++i)
            {
                ScriptSlots[i] = reader.ReadByte();
            }

            //Class1 = reader.ReadUInt32();
            //Class2 = reader.ReadUInt32();
            //Class3 = reader.ReadUInt32();
            var len = reader.ReadInt32();
            Name = new string(reader.ReadChars(len));

            // Read UInt32 script slots
            var cnt = reader.ReadInt32();
            UI32 = new uint[cnt];
            for (int i = 0; i < cnt; ++i)
                UI32[i] = reader.ReadUInt32();

            // Read OGI script slots
            cnt = reader.ReadInt32();
            OGIs = new ushort[cnt];
            for (int i = 0; i < cnt; ++i)
                OGIs[i] = reader.ReadUInt16();

            // Read Animation script slots
            cnt = reader.ReadInt32();
            Anims = new ushort[cnt];
            for (int i = 0; i < cnt; ++i)
                Anims[i] = reader.ReadUInt16();

            // Read Script script slots
            cnt = reader.ReadInt32();
            Scripts = new ushort[cnt];
            for (int i = 0; i < cnt; ++i)
                Scripts[i] = reader.ReadUInt16();

            // Read Object script slots
            cnt = reader.ReadInt32();
            Objects = new ushort[cnt];
            for (int i = 0; i < cnt; ++i)
                Objects[i] = reader.ReadUInt16();

            // Read Sound script slots
            cnt = reader.ReadInt32();
            Sounds = new ushort[cnt];
            for (int i = 0; i < cnt; ++i)
                Sounds[i] = reader.ReadUInt16();

            flag = reader.ReadUInt32();
            if (flag > 255)
            {
                PHeader = flag;
                PUI32 = reader.ReadUInt32();

                cnt = reader.ReadInt32();
                pUi321 = new uint[cnt];
                for (int i = 0; i < cnt; ++i)
                    pUi321[i] = reader.ReadUInt32();

                cnt = reader.ReadInt32();
                pUi322 = new float[cnt];
                for (int i = 0; i < cnt; ++i)
                    pUi322[i] = reader.ReadSingle();

                cnt = reader.ReadInt32();
                pUi323 = new uint[cnt];
                for (int i = 0; i < cnt; ++i)
                    pUi323[i] = reader.ReadUInt32();
                flag = reader.ReadUInt32();
            }
            else
            {
                PHeader = 0;
                PUI32 = 0;
                pUi321 = new uint[] { };
                pUi322 = new float[] { };
                pUi323 = new uint[] { };
            }
            // Read IDs needed for instance creation
            if (flag > 0)
            {
                if ((flag & 0x00000001) != 0)
                {
                    cnt = reader.ReadInt32();
                    cObjects = new ushort[cnt];
                    for (int i = 0; i < cnt; ++i)
                        cObjects[i] = reader.ReadUInt16();
                }
                if ((flag & 0x00000002) != 0)
                {
                    cnt = reader.ReadInt32();
                    cOGIs = new ushort[cnt];
                    for (int i = 0; i < cnt; ++i)
                        cOGIs[i] = reader.ReadUInt16();
                }
                if ((flag & 0x00000004) != 0)
                {
                    cnt = reader.ReadInt32();
                    cAnims = new ushort[cnt];
                    for (int i = 0; i < cnt; ++i)
                        cAnims[i] = reader.ReadUInt16();
                }
                if ((flag & 0x00000008) != 0)
                {
                    cnt = reader.ReadInt32();
                    cCM = new ushort[cnt];
                    for (int i = 0; i < cnt; ++i)
                        cCM[i] = reader.ReadUInt16();
                }
                if ((flag & 0x00000010) != 0)
                {
                    cnt = reader.ReadInt32();
                    cScripts = new ushort[cnt];
                    for (int i = 0; i < cnt; ++i)
                        cScripts[i] = reader.ReadUInt16();
                }
                if ((flag & 0x00000020) != 0)
                {
                    cnt = reader.ReadInt32();
                    cUnk = new ushort[cnt];
                    for (int i = 0; i < cnt; ++i)
                        cUnk[i] = reader.ReadUInt16();
                }
                if ((flag & 0x00000040) != 0)
                {
                    cnt = reader.ReadInt32();
                    cSounds = new ushort[cnt];
                    for (int i = 0; i < cnt; ++i)
                        cSounds[i] = reader.ReadUInt16();
                }
                scriptLen = (int)reader.ReadUInt32();
                if (scriptLen > 1)
                {
                    scriptParams = new ushort[18];
                    for (int i = 0; i < 18; ++i)
                        scriptParams[i] = reader.ReadUInt16();
                }
                scriptData = reader.ReadBytes((int)(size - (reader.BaseStream.Position - sk)));
            }
            this.size = size;
        }

        protected override int GetSize()
        {
            return size;
        }
    }
}
