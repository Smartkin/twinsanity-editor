using System;

namespace Twinsanity
{
    public class SubChunk : BaseItem
    {
        public new string NodeName = "SubChunk";
        public int Chunks;
        public ChunkType[] Chunk = new ChunkType[] { };


        /////////PARENTS FUNCTION//////////
        protected override void DataUpdate()
        {
            System.IO.BinaryReader BSReader = new System.IO.BinaryReader(ByteStream);
            ByteStream.Position = 0;
            Chunks = BSReader.ReadInt32();
            Array.Resize(ref Chunk, Chunks);
            for (int i = 0; i <= Chunks - 1; i++)
            {
                ChunkType CH = new ChunkType { Unknown = new uint[] { } };
                CH.Type = BSReader.ReadUInt32();
                switch (CH.Type)
                {
                    case 0:
                        {
                            CH.Path = BSReader.ReadChars(BSReader.ReadInt32()).ToString();
                            CH.Flags = BSReader.ReadUInt32();
                            CH.PlayerTeleportScaleX = BSReader.ReadSingle();
                            CH.SomeInt1 = BSReader.ReadUInt32();
                            CH.SomeInt2 = BSReader.ReadUInt32();
                            CH.Null1 = BSReader.ReadUInt32();
                            CH.SomeInt3 = BSReader.ReadUInt32();
                            CH.PlayerTeleportScaleY = BSReader.ReadSingle();
                            CH.SomeInt4 = BSReader.ReadUInt32();
                            CH.Null2 = BSReader.ReadUInt32();
                            CH.SomeInt5 = BSReader.ReadUInt32();
                            CH.SomeInt6 = BSReader.ReadUInt32();
                            CH.PlayerTeleportScaleZ = BSReader.ReadSingle();
                            CH.Null3 = BSReader.ReadUInt32();
                            CH.UnkV4.X = BSReader.ReadSingle();
                            CH.UnkV4.Y = BSReader.ReadSingle();
                            CH.UnkV4.Z = BSReader.ReadSingle();
                            CH.UnkV4.W = BSReader.ReadSingle();
                            CH.PlayerTeleport.X = BSReader.ReadSingle();
                            CH.PlayerTeleport.Y = BSReader.ReadSingle();
                            CH.PlayerTeleport.Z = BSReader.ReadSingle();
                            CH.PlayerTeleport.W = BSReader.ReadSingle();
                            CH.VisualSubchunkScaleX = BSReader.ReadSingle();
                            CH.SomeInt7 = BSReader.ReadUInt32();
                            CH.SomeInt8 = BSReader.ReadUInt32();
                            CH.Null4 = BSReader.ReadUInt32();
                            CH.SomeInt9 = BSReader.ReadUInt32();
                            CH.VisualSubchunkScaleY = BSReader.ReadSingle();
                            CH.SomeInt10 = BSReader.ReadUInt32();
                            CH.Null5 = BSReader.ReadUInt32();
                            CH.SomeInt11 = BSReader.ReadUInt32();
                            CH.SomeInt12 = BSReader.ReadUInt32();
                            CH.VisualSubchunkScaleZ = BSReader.ReadSingle();
                            CH.Null6 = BSReader.ReadUInt32();
                            CH.VisualSubchunkLocationX = BSReader.ReadSingle();
                            CH.VisualSubchunkLocationY = BSReader.ReadSingle();
                            CH.VisualSubchunkLocationZ = BSReader.ReadSingle();
                            CH.LoadWallPoint1.X = BSReader.ReadSingle();
                            CH.LoadWallPoint1.Y = BSReader.ReadSingle();
                            CH.LoadWallPoint1.Z = BSReader.ReadSingle();
                            CH.LoadWallPoint1.W = BSReader.ReadSingle();

                            CH.LoadWallPoint2.X = BSReader.ReadSingle();
                            CH.LoadWallPoint2.Y = BSReader.ReadSingle();
                            CH.LoadWallPoint2.Z = BSReader.ReadSingle();
                            CH.LoadWallPoint2.W = BSReader.ReadSingle();

                            CH.LoadWallPoint3.X = BSReader.ReadSingle();
                            CH.LoadWallPoint3.Y = BSReader.ReadSingle();
                            CH.LoadWallPoint3.Z = BSReader.ReadSingle();
                            CH.LoadWallPoint3.W = BSReader.ReadSingle();

                            CH.LoadWallPoint4.X = BSReader.ReadSingle();
                            CH.LoadWallPoint4.Y = BSReader.ReadSingle();
                            CH.LoadWallPoint4.Z = BSReader.ReadSingle();
                            CH.LoadWallPoint4.W = BSReader.ReadSingle();
                            break;
                        }

                    case 1:
                        {
                            CH.Path = BSReader.ReadChars(BSReader.ReadInt32()).ToString();
                            CH.Flags = BSReader.ReadUInt32();
                            CH.PlayerTeleportScaleX = BSReader.ReadSingle();
                            CH.SomeInt1 = BSReader.ReadUInt32();
                            CH.SomeInt2 = BSReader.ReadUInt32();
                            CH.Null1 = BSReader.ReadUInt32();
                            CH.SomeInt3 = BSReader.ReadUInt32();
                            CH.PlayerTeleportScaleY = BSReader.ReadSingle();
                            CH.SomeInt4 = BSReader.ReadUInt32();
                            CH.Null2 = BSReader.ReadUInt32();
                            CH.SomeInt5 = BSReader.ReadUInt32();
                            CH.SomeInt6 = BSReader.ReadUInt32();
                            CH.PlayerTeleportScaleZ = BSReader.ReadSingle();
                            CH.Null3 = BSReader.ReadUInt32();
                            CH.UnkV4.X = BSReader.ReadSingle();
                            CH.UnkV4.Y = BSReader.ReadSingle();
                            CH.UnkV4.Z = BSReader.ReadSingle();
                            CH.UnkV4.W = BSReader.ReadSingle();
                            CH.PlayerTeleport.X = BSReader.ReadSingle();
                            CH.PlayerTeleport.Y = BSReader.ReadSingle();
                            CH.PlayerTeleport.Z = BSReader.ReadSingle();
                            CH.PlayerTeleport.W = BSReader.ReadSingle();
                            CH.VisualSubchunkScaleX = BSReader.ReadSingle();
                            CH.SomeInt7 = BSReader.ReadUInt32();
                            CH.SomeInt8 = BSReader.ReadUInt32();
                            CH.Null4 = BSReader.ReadUInt32();
                            CH.SomeInt9 = BSReader.ReadUInt32();
                            CH.VisualSubchunkScaleY = BSReader.ReadSingle();
                            CH.SomeInt10 = BSReader.ReadUInt32();
                            CH.Null5 = BSReader.ReadUInt32();
                            CH.SomeInt11 = BSReader.ReadUInt32();
                            CH.SomeInt12 = BSReader.ReadUInt32();
                            CH.VisualSubchunkScaleZ = BSReader.ReadSingle();
                            CH.Null6 = BSReader.ReadUInt32();
                            CH.VisualSubchunkLocationX = BSReader.ReadSingle();
                            CH.VisualSubchunkLocationY = BSReader.ReadSingle();
                            CH.VisualSubchunkLocationZ = BSReader.ReadSingle();
                            CH.LoadWallPoint1.X = BSReader.ReadSingle();
                            CH.LoadWallPoint1.Y = BSReader.ReadSingle();
                            CH.LoadWallPoint1.Z = BSReader.ReadSingle();
                            CH.LoadWallPoint1.W = BSReader.ReadSingle();

                            CH.LoadWallPoint2.X = BSReader.ReadSingle();
                            CH.LoadWallPoint2.Y = BSReader.ReadSingle();
                            CH.LoadWallPoint2.Z = BSReader.ReadSingle();
                            CH.LoadWallPoint2.W = BSReader.ReadSingle();

                            CH.LoadWallPoint3.X = BSReader.ReadSingle();
                            CH.LoadWallPoint3.Y = BSReader.ReadSingle();
                            CH.LoadWallPoint3.Z = BSReader.ReadSingle();
                            CH.LoadWallPoint3.W = BSReader.ReadSingle();

                            CH.LoadWallPoint4.X = BSReader.ReadSingle();
                            CH.LoadWallPoint4.Y = BSReader.ReadSingle();
                            CH.LoadWallPoint4.Z = BSReader.ReadSingle();
                            CH.LoadWallPoint4.W = BSReader.ReadSingle();
                            Array.Resize(ref CH.Unknown, 17);
                            CH.Null7 = BSReader.ReadUInt32();
                            for (int j = 0; j <= 16; j++)
                                CH.Unknown[j] = BSReader.ReadUInt32();
                            CH.V1.X = BSReader.ReadSingle();
                            CH.V1.Y = BSReader.ReadSingle();
                            CH.V1.Z = BSReader.ReadSingle();
                            CH.Z1 = BSReader.ReadSingle();

                            CH.V2.X = BSReader.ReadSingle();
                            CH.V2.Y = BSReader.ReadSingle();
                            CH.V2.Z = BSReader.ReadSingle();
                            CH.Y2 = BSReader.ReadSingle();

                            CH.V3.X = BSReader.ReadSingle();
                            CH.V3.Y = BSReader.ReadSingle();
                            CH.V3.Z = BSReader.ReadSingle();
                            CH.Z2 = BSReader.ReadSingle();

                            CH.V4.X = BSReader.ReadSingle();
                            CH.V4.Y = BSReader.ReadSingle();
                            CH.V4.Z = BSReader.ReadSingle();
                            CH.Y1 = BSReader.ReadSingle();

                            CH.V5.X = BSReader.ReadSingle();
                            CH.V5.Y = BSReader.ReadSingle();
                            CH.V5.Z = BSReader.ReadSingle();
                            CH.X2 = BSReader.ReadSingle();

                            CH.V6.X = BSReader.ReadSingle();
                            CH.V6.Y = BSReader.ReadSingle();
                            CH.V6.Z = BSReader.ReadSingle();
                            CH.X1 = BSReader.ReadSingle();

                            Array.Resize(ref CH.V4Arr, 6);
                            for (int j = 0; j <= 5; j++)
                            {
                                CH.V4Arr[j].X = BSReader.ReadSingle();
                                CH.V4Arr[j].Y = BSReader.ReadSingle();
                                CH.V4Arr[j].Z = BSReader.ReadSingle();
                                CH.V4Arr[j].W = BSReader.ReadSingle();
                            }
                            CH.Bytes = BSReader.ReadBytes(60);
                            break;
                        }
                }
                Chunk[i] = CH;
            }
        }

        #region STRUCTURES
        public struct Type0
        {
            public string Path;
            public uint Flags;
            public float PlayerTeleportScaleX;
            public uint SomeInt1;
            public uint SomeInt2;
            public uint Null1;
            public uint SomeInt3;
            public float PlayerTeleportScaleY;
            public uint SomeInt4;
            public uint Null2;
            public uint SomeInt5;
            public uint SomeInt6;
            public float PlayerTeleportScaleZ;
            public uint Null3;
            public RM2.Coordinate4 UnkV4;
            public RM2.Coordinate4 PlayerTeleport;
            public float VisualSubchunkScaleX;
            public uint SomeInt7;
            public uint SomeInt8;
            public uint Null4;
            public uint SomeInt9;
            public float VisualSubchunkScaleY;
            public uint SomeInt10;
            public uint Null5;
            public uint SomeInt11;
            public uint SomeInt12;
            public float VisualSubchunkScaleZ;
            public uint Null6;
            public float VisualSubchunkLocationX;
            public float VisualSubchunkLocationY;
            public float VisualSubchunkLocationZ;
            public RM2.Coordinate4 LoadWallPoint1;
            public RM2.Coordinate4 LoadWallPoint2;
            public RM2.Coordinate4 LoadWallPoint3;
            public RM2.Coordinate4 LoadWallPoint4;
        }

        public struct ChunkType
        {
            public uint Type;
            public string Path;
            public uint Flags;
            public float PlayerTeleportScaleX;
            public uint SomeInt1;
            public uint SomeInt2;
            public uint Null1;
            public uint SomeInt3;
            public float PlayerTeleportScaleY;
            public uint SomeInt4;
            public uint Null2;
            public uint SomeInt5;
            public uint SomeInt6;
            public float PlayerTeleportScaleZ;
            public uint Null3;
            public RM2.Coordinate4 UnkV4;
            public RM2.Coordinate4 PlayerTeleport;
            public float VisualSubchunkScaleX;
            public uint SomeInt7;
            public uint SomeInt8;
            public uint Null4;
            public uint SomeInt9;
            public float VisualSubchunkScaleY;
            public uint SomeInt10;
            public uint Null5;
            public uint SomeInt11;
            public uint SomeInt12;
            public float VisualSubchunkScaleZ;
            public uint Null6;
            public float VisualSubchunkLocationX;
            public float VisualSubchunkLocationY;
            public float VisualSubchunkLocationZ;
            public RM2.Coordinate4 LoadWallPoint1;
            public RM2.Coordinate4 LoadWallPoint2;
            public RM2.Coordinate4 LoadWallPoint3;
            public RM2.Coordinate4 LoadWallPoint4;
            // Type 1 stuff
            public uint Null7;
            public uint[] Unknown; // 17
            public RM2.Coordinate4 V1;
            public float Z1;
            public RM2.Coordinate4 V2;
            public float Y2;
            public RM2.Coordinate4 V3;
            public float Z2;
            public RM2.Coordinate4 V4;
            public float Y1;
            public RM2.Coordinate4 V5;
            public float X2;
            public RM2.Coordinate4 V6;
            public float X1;
            public RM2.Coordinate4[] V4Arr; // 6
            public byte[] Bytes; // 60
        }
        #endregion
    }
}
