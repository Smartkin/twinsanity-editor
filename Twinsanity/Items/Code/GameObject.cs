using System;
using System.Collections.Generic;
using System.IO;

namespace Twinsanity
{
    public class GameObject : TwinsItem
    {
        /*
         * #0: (OnSpawn) 570
#1: (OnTrigger) 570
#2: (OnDamage) 578
#3: (OnTouch) None
#4: (OnHeadbutt) None
#5: (OnLand) None
#6: (OnGettingSpinAttacked) 578
#7: (OnGettingBodyslamAttacked) None
#8: (OnGettingSlideAttacked) None
#9: (OnPhysicsCollision) None
#10: (Unk10) None
         */
        private int size;
        public uint UnkBitfield { get; set; }
        public byte[] ScriptSlots { get; set; } = new byte[8]; // Pairs;Scripts;GameObjects;UInt32s;Sounds;00;00;00 (last 3 are potentially a side effect of needing object name's length to be word aligned)
        public List<UInt32> UI32 { get; set; } = new List<UInt32>();
        public List<UInt16> OGIs { get; set; } = new List<UInt16>();
        public List<UInt16> Anims { get; set; } = new List<UInt16>();
        public List<UInt16> Scripts { get; set; } = new List<UInt16>();
        public List<UInt16> Objects { get; set; } = new List<UInt16>();
        public List<UInt16> Sounds { get; set; } = new List<UInt16>();
        public uint PHeader { get; set; } // Inst;Pos;Path;00
        public uint PUI32 { get; set; }
        //private int pUi321.Length = 0;
        //private int pUi322.Length = 1;
        //private int pUi323.Length = 2;
        private List<UInt32> pUi321 = new List<UInt32>();
        private List<Single> pUi322 = new List<Single>();
        private List<UInt32> pUi323 = new List<UInt32>();
        private uint flag;
        public List<UInt16> cObjects = new List<UInt16>();
        public List<UInt16> cOGIs = new List<UInt16>();
        public List<UInt16> cAnims = new List<UInt16>();
        public List<UInt16> cCM = new List<UInt16>();
        public List<UInt16> cScripts = new List<UInt16>();
        public List<UInt16> cUnk = new List<UInt16>();
        public List<UInt16> cSounds = new List<UInt16>();
        private int scriptLen;
        private List<UInt16> scriptParams = new List<UInt16>();
        private Byte[] scriptData = new byte[0];

        public string Name { get; set; }

        public override void Save(BinaryWriter writer)
        {
            var sk = writer.BaseStream.Position;

            writer.Write(UnkBitfield);
            for (int i = 0; i < 8; ++i)
            {
                writer.Write(ScriptSlots[i]);
            }
            writer.Write(Name.Length);
            writer.Write(Name.ToCharArray(), 0, Name.Length);
            writer.Write(UI32.Count);
            for (int i = 0; i < UI32.Count; ++i)
                writer.Write(UI32[i]);
            writer.Write(OGIs.Count);
            for (int i = 0; i < OGIs.Count; ++i)
                writer.Write(OGIs[i]);
            writer.Write(Anims.Count);
            for (int i = 0; i < Anims.Count; ++i)
                writer.Write(Anims[i]);
            writer.Write(Scripts.Count);
            for (int i = 0; i < Scripts.Count; ++i)
                writer.Write(Scripts[i]);
            writer.Write(Objects.Count);
            for (int i = 0; i < Objects.Count; ++i)
                writer.Write(Objects[i]);
            writer.Write(Sounds.Count);
            for (int i = 0; i < Sounds.Count; ++i)
                writer.Write(Sounds[i]);
            PHeader = (uint)((byte)pUi321.Count
                | (pUi322.Count << 8)
                | (pUi323.Count << 16));
            if (PHeader > 0)
            {
                writer.Write(PHeader);
                writer.Write(PUI32);
                writer.Write(pUi321.Count);
                for (int i = 0; i < pUi321.Count; ++i)
                    writer.Write(pUi321[i]);
                writer.Write(pUi322.Count);
                for (int i = 0; i < pUi322.Count; ++i)
                    writer.Write(pUi322[i]);
                writer.Write(pUi323.Count);
                for (int i = 0; i < pUi323.Count; ++i)
                    writer.Write(pUi323[i]);
            }
            writer.Write(flag);
            if (flag > 0)
            {
                if ((flag & 0x01) != 0)
                {
                    writer.Write(cObjects.Count);
                    for (int i = 0; i < cObjects.Count; ++i)
                        writer.Write(cObjects[i]);
                }
                if ((flag & 0x02) != 0)
                {
                    writer.Write(cOGIs.Count);
                    for (int i = 0; i < cOGIs.Count; ++i)
                        writer.Write(cOGIs[i]);
                }
                if ((flag & 0x04) != 0)
                {
                    writer.Write(cAnims.Count);
                    for (int i = 0; i < cAnims.Count; ++i)
                        writer.Write(cAnims[i]);
                }
                if ((flag & 0x08) != 0)
                {
                    writer.Write(cCM.Count);
                    for (int i = 0; i < cCM.Count; ++i)
                        writer.Write(cCM[i]);
                }
                if ((flag & 0x10) != 0)
                {
                    writer.Write(cScripts.Count);
                    for (int i = 0; i < cScripts.Count; ++i)
                        writer.Write(cScripts[i]);
                }
                if ((flag & 0x20) != 0)
                {
                    writer.Write(cUnk.Count);
                    for (int i = 0; i < cUnk.Count; ++i)
                        writer.Write(cUnk[i]);
                }
                if ((flag & 0x40) != 0)
                {
                    writer.Write(cSounds.Count);
                    for (int i = 0; i < cSounds.Count; ++i)
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
            UI32.Clear();
            for (int i = 0; i < cnt; ++i)
                UI32.Add(reader.ReadUInt32());

            // Read OGI script slots
            cnt = reader.ReadInt32();
            OGIs.Clear();
            for (int i = 0; i < cnt; ++i)
                OGIs.Add(reader.ReadUInt16());

            // Read Animation script slots
            cnt = reader.ReadInt32();
            Anims.Clear();
            for (int i = 0; i < cnt; ++i)
                Anims.Add(reader.ReadUInt16());

            // Read Script script slots
            cnt = reader.ReadInt32();
            Scripts.Clear();
            for (int i = 0; i < cnt; ++i)
                Scripts.Add(reader.ReadUInt16());

            // Read Object script slots
            cnt = reader.ReadInt32();
            Objects.Clear();
            for (int i = 0; i < cnt; ++i)
                Objects.Add(reader.ReadUInt16());

            // Read Sound script slots
            cnt = reader.ReadInt32();
            Sounds.Clear();
            for (int i = 0; i < cnt; ++i)
                Sounds.Add(reader.ReadUInt16());

            flag = reader.ReadUInt32();
            if (flag > 255)
            {
                PHeader = flag;
                PUI32 = reader.ReadUInt32();

                cnt = reader.ReadInt32();
                pUi321.Clear();
                for (int i = 0; i < cnt; ++i)
                    pUi321.Add(reader.ReadUInt32());

                cnt = reader.ReadInt32();
                pUi322.Clear();
                for (int i = 0; i < cnt; ++i)
                    pUi322.Add(reader.ReadSingle());

                cnt = reader.ReadInt32();
                pUi323.Clear();
                for (int i = 0; i < cnt; ++i)
                    pUi323.Add(reader.ReadUInt32());
                flag = reader.ReadUInt32();
            }
            else
            {
                PHeader = 0;
                PUI32 = 0;
                pUi321.Clear();
                pUi322.Clear();
                pUi323.Clear();
            }
            // Read IDs needed for instance creation
            if (flag > 0)
            {
                if ((flag & 0x00000001) != 0)
                {
                    cnt = reader.ReadInt32();
                    cObjects.Clear();
                    for (int i = 0; i < cnt; ++i)
                        cObjects.Add(reader.ReadUInt16());
                }
                if ((flag & 0x00000002) != 0)
                {
                    cnt = reader.ReadInt32();
                    cOGIs.Clear();
                    for (int i = 0; i < cnt; ++i)
                        cOGIs.Add(reader.ReadUInt16());
                }
                if ((flag & 0x00000004) != 0)
                {
                    cnt = reader.ReadInt32();
                    cAnims.Clear();
                    for (int i = 0; i < cnt; ++i)
                        cAnims.Add(reader.ReadUInt16());
                }
                if ((flag & 0x00000008) != 0)
                {
                    cnt = reader.ReadInt32();
                    cCM.Clear();
                    for (int i = 0; i < cnt; ++i)
                        cCM.Add(reader.ReadUInt16());
                }
                if ((flag & 0x00000010) != 0)
                {
                    cnt = reader.ReadInt32();
                    cScripts.Clear();
                    for (int i = 0; i < cnt; ++i)
                        cScripts.Add(reader.ReadUInt16());
                }
                if ((flag & 0x00000020) != 0)
                {
                    cnt = reader.ReadInt32();
                    cUnk.Clear();
                    for (int i = 0; i < cnt; ++i)
                        cUnk.Add(reader.ReadUInt16());
                }
                if ((flag & 0x00000040) != 0)
                {
                    cnt = reader.ReadInt32();
                    cSounds.Clear();
                    for (int i = 0; i < cnt; ++i)
                        cSounds.Add(reader.ReadUInt16());
                }
                scriptLen = (int)reader.ReadUInt32();
                if (scriptLen > 1)
                {
                    scriptParams.Clear();
                    for (int i = 0; i < 18; ++i)
                        scriptParams.Add(reader.ReadUInt16());
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
