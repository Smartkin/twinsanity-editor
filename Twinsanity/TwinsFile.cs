using System;
using System.Collections.Generic;
using System.IO;

namespace Twinsanity
{
    public struct TwinsSubInfo
    {
        public uint Off;
        public int Size;
        public uint ID;
    }

    /// <summary>
    /// Represents a Twinsanity RM/SM file, a full pair (+ .ptl in Xbox) corresponds to a full "chunk"
    /// </summary>
    public class TwinsFile
    {
        private readonly uint magic = 0x00010001;
        private TwinsSecInfo sec_info;

        public TwinsSecInfo SecInfo { get => sec_info; set => sec_info = value; }

        //private List<TwinsSubInfo> sub_list;
        
        /// <summary>
        /// Load an RM/SM file. "rs" is a boolean that determines if the file being loaded is an RM (false) or an SM (true)
        /// </summary>
        public void LoadFile(string path, bool rs)
        {
            if (!File.Exists(path))
                return;
            sec_info.Records = new Dictionary<uint, TwinsItem>();
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(file);
            if ((sec_info.Magic = reader.ReadUInt32()) != magic)
                throw new ArgumentException("TwinsFile::LoadRM2File: Magic number is wrong.");
            var count = reader.ReadInt32();
            var sec_size = reader.ReadUInt32();
            TwinsSubInfo sub = new TwinsSubInfo();
            for (int i = 0; i < count; i++)
            {
                sub.Off = reader.ReadUInt32();
                sub.Size = reader.ReadInt32();
                sub.ID = reader.ReadUInt32();
                if (!rs)
                {
                    switch (sub.ID)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        //{
                        //    InstanceSection sec = new InstanceSection();
                        //    InstInfo.Load(ref file, ref reader);
                        //    sec_info.Sections.Add(sub.ID, sec);
                        //    break;
                        //}
                        case 10:
                        //{
                        //    CodeSectionNew sec = new CodeSectionNew();
                        //    Code.Load(ref file, ref reader);
                        //    sec_info.Sections.Add(sub.ID, sec);
                        //    break;
                        //}
                        case 11:
                            //{
                            //    GraphicsSectionNew sec = new GraphicsSectionNew();
                            //    Graphics.Load(ref file, ref reader);
                            //    sec_info.Sections.Add(sub.ID, sec);
                            //    break;
                            //}
                            {
                                TwinsSection sec = new TwinsSection();
                                sec.ID = sub.ID;
                                var sk = reader.BaseStream.Position;
                                reader.BaseStream.Position = sec.Offset = sub.Off;
                                sec.Level = 1;
                                sec.Load(reader, sub.Size);
                                reader.BaseStream.Position = sk;
                                sec_info.Records.Add(sub.ID, sec);
                                break;
                            }
                        case 9:
                            {
                                ColData rec = new ColData();
                                rec.ID = sub.ID;
                                var sk = reader.BaseStream.Position;
                                reader.BaseStream.Position = rec.Offset = sub.Off;
                                rec.Load(reader);
                                reader.BaseStream.Position = sk;
                                sec_info.Records.Add(sub.ID, rec);
                                break;
                            }
                        default:
                            {
                                TwinsItem rec = new TwinsItem();
                                rec.ID = sub.ID;
                                var sk = reader.BaseStream.Position;
                                reader.BaseStream.Position = rec.Offset = sub.Off;
                                rec.Load(reader, sub.Size);
                                reader.BaseStream.Position = sk;
                                sec_info.Records.Add(sub.ID, rec);
                                break;
                            }
                    }
                }
                else
                {
                    switch (sub.ID)
                    {
                        case 6:
                            {
                                TwinsSection sec = new TwinsSection();
                                var sk = reader.BaseStream.Position;
                                reader.BaseStream.Position = sec.Offset = sub.Off;
                                sec.Level = 1;
                                sec.Load(reader, sub.Size);
                                reader.BaseStream.Position = sk;
                                sec_info.Records.Add(sub.ID, sec);
                                break;
                            }
                        default:
                            {
                                TwinsItem rec = new TwinsItem();
                                var sk = reader.BaseStream.Position;
                                reader.BaseStream.Position = rec.Offset = sub.Off;
                                rec.Load(reader, sub.Size);
                                reader.BaseStream.Position = sk;
                                sec_info.Records.Add(sub.ID, rec);
                                break;
                            }
                    }
                }
            }
            reader.Close();
            file.Close();
        }

        /// <summary>
        /// Load the SM2 chunk
        /// </summary>
        /// <param name="Path">Path to load from</param>
        public void LoadSM2(string Path)
        {
            throw new NotImplementedException();
            /*
            if (File.Exists(Path))
            {
                FileStream file = new FileStream(Path, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(file);
                Header = reader.ReadUInt32();
                Records = (int)reader.ReadUInt32();
                Size = reader.ReadUInt32();
                Array.Resize(ref Item, Records);
                for (int i = 0; i <= Records - 1; i++)
                {
                    uint Offset = reader.ReadUInt32();
                    uint Size = reader.ReadUInt32();
                    uint ID = reader.ReadUInt32();
                    uint Pos = (uint)file.Position;
                    switch (ID)
                    {
                        case 6:
                            {
                                GraphicsSection Graphics = new GraphicsSection();
                                Graphics.Size = Size;
                                Graphics.Offset = Offset;
                                Graphics.Base = 0;
                                Graphics.ID = ID;
                                Graphics.Load(ref file, ref reader);
                                Item[i] = Graphics;
                                break;
                            }

                        case 0:
                            {
                                Scenery SCEN = new Scenery();
                                SCEN.Size = Size;
                                SCEN.Offset = Offset;
                                SCEN.Base = 0;
                                SCEN.ID = ID;
                                SCEN.Load(ref file, ref reader);
                                Item[i] = SCEN;
                                break;
                            }

                        case 5:
                            {
                                SubChunk SC = new SubChunk();
                                SC.Size = Size;
                                SC.Offset = Offset;
                                SC.Base = 0;
                                SC.ID = ID;
                                SC.Load(ref file, ref reader);
                                Item[i] = SC;
                                break;
                            }

                        default:
                            {
                                BaseItem UItem = new BaseItem();
                                UItem.Size = Size;
                                UItem.Offset = Offset;
                                UItem.Base = 0;
                                UItem.ID = ID;
                                UItem.Load(ref file, ref reader);
                                Item[i] = UItem;
                                break;
                            }
                    }
                    file.Position = Pos;
                }
                reader.Close();
                file.Close();
            }
            */
        }

        /// <summary>
        /// Create new RM2 chunk
        /// </summary>
        public void NewRM2()
        {
            throw new NotImplementedException();
            /*
            Array.Resize(ref Item, 12);
            Records = 12;
            Item[0] = new GraphicsSection();
            Item[0].ID = 11;
            Item[0].Records = 9;
            Array.Resize(ref Item[0]._Item, 9);
            for (int i = 0; i <= 8; i++)
            {
                switch (i)
                {
                    case 0:
                        {
                            Item[0][i] = new Textures();
                            break;
                        }

                    case 1:
                        {
                            Item[0][i] = new Materials();
                            break;
                        }

                    case 2:
                        {
                            Item[0][i] = new Models();
                            break;
                        }

                    case 3:
                        {
                            Item[0][i] = new GCs();
                            break;
                        }

                    case 4:
                        {
                            Item[0][i] = new ID4Models();
                            break;
                        }

                    case 5:
                        {
                            Item[0][i] = new BaseSection();
                            break;
                        }

                    case 6:
                        {
                            Item[0][i] = new GCs();
                            break;
                        }

                    case 7:
                        {
                            Item[0][i] = new BaseSection();
                            break;
                        }

                    case 8:
                        {
                            Item[0][i] = new BaseSection();
                            break;
                        }
                }
                Item[0][i].ID = (uint)i;
            }
            Array.Resize(ref Item, 12);
            Item[1] = new CodeSection();
            Item[1].ID = 10;
            Item[1].Records = 13;
            Array.Resize(ref Item[1]._Item, 13);
            for (int i = 0; i <= 12; i++)
            {
                switch (i)
                {
                    case 0:
                        {
                            Item[1][i] = new GameObjects();
                            break;
                        }

                    case 1:
                        {
                            Item[1][i] = new Scripts();
                            break;
                        }

                    case 2:
                        {
                            Item[1][i] = new Animations();
                            break;
                        }

                    case 3:
                        {
                            Item[1][i] = new OGIs();
                            break;
                        }

                    case 4:
                        {
                            Item[1][i] = new BaseSection();
                            break;
                        }

                    case 5:
                        {
                            Item[1][i] = new SoundDescriptions();
                            break;
                        }

                    case 6:
                        {
                            Item[1][i] = new Sound();
                            break;
                        }

                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                        {
                            Item[1][i] = new SoundbankDescriptions();
                            break;
                        }
                }
                Item[1][i].ID = (uint)i;
            }
            Item[2] = new BaseItem();
            Item[2].ID = 8;
            Item[3] = new ColData();
            Item[3].ID = 9;
            for (int j = 4; j <= 11; j++)
            {
                Item[j] = new InstanceInfoSection();
                Item[j].ID = (uint)j - 4;
                Item[j].Records = 9;
                Array.Resize(ref Item[j]._Item, 9);
                for (int i = 0; i <= 8; i++)
                {
                    switch (i)
                    {
                        case 0:
                            {
                                Item[j][i] = new BaseSection();
                                break;
                            }

                        case 1:
                            {
                                Item[j][i] = new Behaviors();
                                break;
                            }

                        case 2:
                            {
                                Item[j][i] = new FuckingShits();
                                break;
                            }

                        case 3:
                            {
                                Item[j][i] = new Positions();
                                break;
                            }

                        case 4:
                            {
                                Item[j][i] = new Paths();
                                break;
                            }

                        case 5:
                            {
                                Item[j][i] = new SurfaceBehaviours();
                                break;
                            }

                        case 6:
                            {
                                Item[j][i] = new Instances();
                                break;
                            }

                        case 7:
                            {
                                Item[j][i] = new Triggers();
                                break;
                            }

                        case 8:
                            {
                                Item[j][i] = new BaseSection();
                                break;
                            }
                    }
                    Item[j][i].ID = (uint)i;
                }
            }
            Recalculate();
            */
        }
        
        /// <summary>
        /// New SM2 Chunk
        /// </summary>
        /// <param name="Name">Name of the chunk</param>
        public void NewSM2(string Name)
        {
            throw new NotImplementedException();
            /*
            Records = 7;
            Array.Resize(ref Item, 7);
            Item[0] = new GraphicsSection();
            Item[0].ID = 6;
            Item[0].Records = 9;
            Array.Resize(ref Item[0]._Item, 9);
            for (int i = 0; i <= 8; i++)
            {
                switch (i)
                {
                    case 0:
                        {
                            Item[0][i] = new Textures();
                            break;
                        }

                    case 1:
                        {
                            Item[0][i] = new Materials();
                            break;
                        }

                    case 2:
                        {
                            Item[0][i] = new Models();
                            break;
                        }

                    case 3:
                        {
                            Item[0][i] = new GCs();
                            break;
                        }

                    case 4:
                        {
                            Item[0][i] = new BaseSection();
                            break;
                        }

                    case 5:
                        {
                            Item[0][i] = new BaseSection();
                            break;
                        }

                    case 6:
                        {
                            Item[0][i] = new GCs();
                            break;
                        }

                    case 7:
                        {
                            Item[0][i] = new BaseSection();
                            break;
                        }

                    case 8:
                        {
                            Item[0][i] = new BaseSection();
                            break;
                        }
                }
                Item[0][i].ID = (uint)i;
            }
            for (int i = 1; i <= 6; i++)
            {
                Item[i] = new BaseItem();
                Item[i].ID = (uint)i - 1;
            }
            MemoryStream NewStream = new MemoryStream();
            BinaryWriter NSwriter = new BinaryWriter(NewStream);
            int I32 = 196608;
            NSwriter.Write(I32);
            NSwriter.Write(Name.Length);
            for (int i = 0; i <= Name.Length - 1; i++)
                NSwriter.Write(Name[i]);
            I32 = 0;
            NSwriter.Write(I32);
            Int16 I16 = 5642;
            NSwriter.Write(I16);
            byte B = 0;
            for (int i = 0; i <= 54; i++)
                NSwriter.Write(B);
            int[] indexes = new int[] { };
            Item[1].Put_Stream(NewStream, 0, indexes);
            Recalculate();
            */
        }
        
        /// <summary>
        /// Save the chunk
        /// </summary>
        /// <param name="Path">Path to save at</param>
        public void Save(string Path)
        {
            throw new NotImplementedException();
            /*
            Recalculate();
            FileStream file = new FileStream(Path, FileMode.Create, FileAccess.Write);
            BinaryWriter Writer = new BinaryWriter(file);
            Writer.Write(Header);
            Writer.Write(Records);
            Writer.Write(Size);
            for (int i = 0; i <= Records - 1; i++)
            {
                Writer.Write(Item[i].Offset);
                Writer.Write(Item[i].Size);
                Writer.Write(Item[i].ID);
            }
            for (int i = 0; i <= Records - 1; i++)
                Item[i].Save(ref file, ref Writer);
            file.Close();
            */
        }

        /*
        /// <summary>
        /// Get the item in the chunk tree
        /// </summary>
        /// <param name="indexes">Array of indexes</param>
        /// <returns>Found object in the tree</returns>
        public BaseObject Get_Item(params int[] indexes)
        {
            if (indexes.Length <= 2)
                // If Item(indexes(indexes.Length - 1)).Size > 0 Then
                return Item[indexes[indexes.Length - 1]];
            else
                return Item[indexes[indexes.Length - 1]].Get_Item(indexes.Length - 2, indexes);
        }

        /// <summary>
        /// Put new item into the chunk tree
        /// </summary>
        /// <param name="indexes">Array of indexes</param>
        /// <param name="It">Item to put in</param>
        public void Put_Item(BaseObject It, params int[] indexes)
        {
            if (indexes.Length <= 2)
                Item[indexes[indexes.Length - 1]] = It;
            else
                Item[indexes[indexes.Length - 1]].Put_Item(It, indexes.Length - 2, indexes);
            Recalculate();
        }

        /// <summary>
        /// Get the RAM stream from the chunk tree
        /// </summary>
        /// <param name="indexes">Array of indexes</param>
        /// <returns>Found RAM stream in the tree</returns>
        public MemoryStream Get_Stream(params int[] indexes)
        {
            return Item[indexes[indexes.Length - 1]].Get_Stream(indexes.Length - 2, indexes);
        }

        /// <summary>
        /// Put new RAM stream into the chunk tree
        /// </summary>
        /// <param name="indexes">Array of indexes</param>
        /// <param name="It">The stream to put</param>
        public void Put_Stream(MemoryStream It, params int[] indexes)
        {
            Item[indexes[indexes.Length - 1]].Put_Stream(It, indexes.Length - 2, indexes);
            Recalculate();
        }

        /// <summary>
        /// Add item to the chunk tree
        /// </summary>
        /// <param name="indexes">Array of indexes</param>
        public void Add_Item(params int[] indexes)
        {
            Item[indexes[indexes.Length - 1]].Add_Item(indexes.Length - 2, indexes);
            Recalculate();
        }

        /// <summary>
        /// Delete item from the chunk tree
        /// </summary>
        /// <param name="indexes">Array of indexes</param>
        public void Delete_Item(params int[] indexes)
        {
            Item[indexes[indexes.Length - 1]].Delete_Item(indexes.Length - 2, indexes);
            Recalculate();
        }

        /// <summary>
        /// Represents a simple 4D coordinate
        /// </summary>
        /*
        public struct Coordinate4
        {
            public float X, Y, Z, W;
        }
        */
        }
    }
