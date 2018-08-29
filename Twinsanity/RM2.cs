using System;

namespace Twinsanity
{
    /// <summary>
    /// Represents Twinsanity's chunk
    /// </summary>
    public class RM2
    {
        public uint Header;
        public uint Size;
        public int Records;
        public BaseObject[] Item;

        /// <summary>
        /// Recalculate size of the chunk
        /// </summary>
        public void Recalculate()
        {
            Size = 0;
            for (int i = 0; i <= Records - 1; i++)
            {
                Item[i].Base = 0;
                Item[i].Offset = (uint)(Size + 12 * (Records + 1));
                Size += Item[i].Recalculate();
            }
        }
        
        /// <summary>
        /// Load the RM2 chunk
        /// </summary>
        /// <param name="Path">Path to load from</param>
        public void LoadRM2(string Path)
        {
            if (System.IO.File.Exists(Path))
            {
                System.IO.FileStream File = new System.IO.FileStream(Path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                System.IO.BinaryReader Reader = new System.IO.BinaryReader(File);
                Header = Reader.ReadUInt32();
                Records = (int)Reader.ReadUInt32();
                Size = Reader.ReadUInt32();
                Array.Resize(ref Item, Records);
                for (int i = 0; i <= Records - 1; i++)
                {
                    uint Offset = Reader.ReadUInt32();
                    uint Size = Reader.ReadUInt32();
                    uint ID = Reader.ReadUInt32();
                    uint Pos = (uint)File.Position;
                    switch (ID)
                    {
                        case object _ when 0 <= ID && ID <= 7:
                            {
                                InstanceInfoSection InstInfo = new InstanceInfoSection();
                                InstInfo.Size = Size;
                                InstInfo.Offset = Offset;
                                InstInfo.Base = 0;
                                InstInfo.ID = ID;
                                InstInfo.Load(ref File, ref Reader);
                                Item[i] = InstInfo;
                                break;
                            }

                        case 10:
                            {
                                CodeSection Code = new CodeSection();
                                Code.Size = Size;
                                Code.Offset = Offset;
                                Code.Base = 0;
                                Code.ID = ID;
                                Code.Load(ref File, ref Reader);
                                Item[i] = Code;
                                break;
                            }

                        case 11:
                            {
                                GraphicsSection Graphics = new GraphicsSection();
                                Graphics.Size = Size;
                                Graphics.Offset = Offset;
                                Graphics.Base = 0;
                                Graphics.ID = ID;
                                Graphics.Load(ref File, ref Reader);
                                Item[i] = Graphics;
                                break;
                            }

                        case 9:
                            {
                                GeoData GD = new GeoData();
                                GD.Size = Size;
                                GD.Offset = Offset;
                                GD.Base = 0;
                                GD.ID = ID;
                                GD.Load(ref File, ref Reader);
                                Item[i] = GD;
                                break;
                            }

                        default:
                            {
                                BaseItem UItem = new BaseItem();
                                UItem.Size = Size;
                                UItem.Offset = Offset;
                                UItem.Base = 0;
                                UItem.ID = ID;
                                UItem.Load(ref File, ref Reader);
                                Item[i] = UItem;
                                break;
                            }
                    }
                    File.Position = Pos;
                }
                Reader.Close();
                File.Close();
            }
        }
        
        /// <summary>
        /// Load Demo version of RM2 chunk
        /// </summary>
        /// <param name="Path">Path to load from</param>
        public void LoadDemoRM2(string Path)
        {
            if (System.IO.File.Exists(Path))
            {
                System.IO.FileStream File = new System.IO.FileStream(Path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                System.IO.BinaryReader Reader = new System.IO.BinaryReader(File);
                Header = Reader.ReadUInt32();
                Records = (int)Reader.ReadUInt32();
                Size = Reader.ReadUInt32();
                Array.Resize(ref Item, Records);
                for (int i = 0; i <= Records - 1; i++)
                {
                    uint Offset = Reader.ReadUInt32();
                    uint Size = Reader.ReadUInt32();
                    uint ID = Reader.ReadUInt32();
                    uint Pos = (uint)File.Position;
                    switch (ID)
                    {
                        case object _ when 0 <= ID && ID <= 7:
                            {
                                DemoInstanceInfoSection InstInfo = new DemoInstanceInfoSection();
                                InstInfo.Size = Size;
                                InstInfo.Offset = Offset;
                                InstInfo.Base = 0;
                                InstInfo.ID = ID;
                                InstInfo.Load(ref File, ref Reader);
                                Item[i] = InstInfo;
                                break;
                            }

                        case 10:
                            {
                                DemoCodeSection Code = new DemoCodeSection();
                                Code.Size = Size;
                                Code.Offset = Offset;
                                Code.Base = 0;
                                Code.ID = ID;
                                Code.Load(ref File, ref Reader);
                                Item[i] = Code;
                                break;
                            }

                        case 11:
                            {
                                GraphicsSection Graphics = new GraphicsSection();
                                Graphics.Size = Size;
                                Graphics.Offset = Offset;
                                Graphics.Base = 0;
                                Graphics.ID = ID;
                                Graphics.Load(ref File, ref Reader);
                                Item[i] = Graphics;
                                break;
                            }

                        case 9:
                            {
                                GeoData GD = new GeoData();
                                GD.Size = Size;
                                GD.Offset = Offset;
                                GD.Base = 0;
                                GD.ID = ID;
                                GD.Load(ref File, ref Reader);
                                Item[i] = GD;
                                break;
                            }

                        default:
                            {
                                BaseItem UItem = new BaseItem();
                                UItem.Size = Size;
                                UItem.Offset = Offset;
                                UItem.Base = 0;
                                UItem.ID = ID;
                                UItem.Load(ref File, ref Reader);
                                Item[i] = UItem;
                                break;
                            }
                    }
                    File.Position = Pos;
                }
                Reader.Close();
                File.Close();
            }
        }
        
        /// <summary>
        /// Load the SM2 chunk
        /// </summary>
        /// <param name="Path">Path to load from</param>
        public void LoadSM2(string Path)
        {
            if (System.IO.File.Exists(Path))
            {
                System.IO.FileStream File = new System.IO.FileStream(Path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                System.IO.BinaryReader Reader = new System.IO.BinaryReader(File);
                Header = Reader.ReadUInt32();
                Records = (int)Reader.ReadUInt32();
                Size = Reader.ReadUInt32();
                Array.Resize(ref Item, Records);
                for (int i = 0; i <= Records - 1; i++)
                {
                    uint Offset = Reader.ReadUInt32();
                    uint Size = Reader.ReadUInt32();
                    uint ID = Reader.ReadUInt32();
                    uint Pos = (uint)File.Position;
                    switch (ID)
                    {
                        case 6:
                            {
                                GraphicsSection Graphics = new GraphicsSection();
                                Graphics.Size = Size;
                                Graphics.Offset = Offset;
                                Graphics.Base = 0;
                                Graphics.ID = ID;
                                Graphics.Load(ref File, ref Reader);
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
                                SCEN.Load(ref File, ref Reader);
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
                                SC.Load(ref File, ref Reader);
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
                                UItem.Load(ref File, ref Reader);
                                Item[i] = UItem;
                                break;
                            }
                    }
                    File.Position = Pos;
                }
                Reader.Close();
                File.Close();
            }
        }

        /// <summary>
        /// Create new RM2 chunk
        /// </summary>
        public void NewRM2()
        {
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
            Item[3] = new GeoData();
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
        }
        
        /// <summary>
        /// New SM2 Chunk
        /// </summary>
        /// <param name="Name">Name of the chunk</param>
        public void NewSM2(string Name)
        {
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
            System.IO.MemoryStream NewStream = new System.IO.MemoryStream();
            System.IO.BinaryWriter NSwriter = new System.IO.BinaryWriter(NewStream);
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
        }
        
        /// <summary>
        /// Save the chunk
        /// </summary>
        /// <param name="Path">Path to save at</param>
        public void Save(string Path)
        {
            Recalculate();
            System.IO.FileStream File = new System.IO.FileStream(Path, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            System.IO.BinaryWriter Writer = new System.IO.BinaryWriter(File);
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
                Item[i].Save(ref File, ref Writer);
            File.Close();
        }

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
        public System.IO.MemoryStream Get_Stream(params int[] indexes)
        {
            return Item[indexes[indexes.Length - 1]].Get_Stream(indexes.Length - 2, indexes);
        }

        /// <summary>
        /// Put new RAM stream into the chunk tree
        /// </summary>
        /// <param name="indexes">Array of indexes</param>
        /// <param name="It">The stream to put</param>
        public void Put_Stream(System.IO.MemoryStream It, params int[] indexes)
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
        public struct Coordinate4
        {
            public float X, Y, Z, W;
        }
    }
}
