using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Twinsanity.VIF;
using System.Linq;

namespace Twinsanity
{
    public class Model : TwinsItem
    {
        public long ItemSize { get; set; }

        public List<SubModel> SubModels { get; set; } = new List<SubModel>();

        public override void Load(BinaryReader reader, int size)
        {
            var sk = reader.BaseStream.Position;
            var count = reader.ReadInt32();

            SubModels.Clear();
            for (int i = 0; i < count; i++)
            {
                var sub = new SubModel();

                sub.VertexCount = (int)reader.ReadUInt32();
                int vertexLen = reader.ReadInt32();
                sub.VifCode = reader.ReadBytes(vertexLen);
                int blobLen = reader.ReadInt32();
                sub.UnusedBlob = reader.ReadBytes(blobLen);

                sub.Vertexes = CalculateData(sub);

                SubModels.Add(sub);
            }

            ItemSize = size;

            //Console.WriteLine("end pos: " + (reader.BaseStream.Position - sk) + " target: " + size);

            //Remain = reader.ReadBytes((size) - (int)(reader.BaseStream.Position - sk));

        }

        [Flags]
        private enum FieldsPresent
        {
            Vertex = 0,
            UV_Color = 1,
            Normals = 2,
            EmitColors = 4
        }

        public List<VertexData> CalculateData(SubModel model)
        {
            var vertexes = new List<VertexData>();

            var interpreter = VIFInterpreter.InterpretCode(model.VifCode);
            var data = interpreter.GetMem();
            var Vertexes = new List<Vector4>();
            var UVW = new List<Vector4>();
            var EmitColor = new List<Vector4>();
            var Colors = new List<Color>();
            var Normals = new List<Vector4>();
            var Connection = new List<bool>();
            var index = 0;
            for (var i = 0; i < data.Count;)
            {
                var verts = (data[i][0].GetBinaryX() & 0xFF);
                var fieldsPresent = FieldsPresent.Vertex;
                var outputAddr = interpreter.GetAddressOutput();
                var fields = 0;
                foreach (var addr in outputAddr[index++])
                {
                    switch (addr)
                    {
                        case 0x3:
                            fieldsPresent |= FieldsPresent.Vertex;
                            fields++;
                            break;
                        case 0x4:
                            fieldsPresent |= FieldsPresent.UV_Color;
                            fields++;
                            break;
                        case 0x5:
                            fieldsPresent |= FieldsPresent.Normals;
                            fields++;
                            break;
                        case 0x6:
                            fieldsPresent |= FieldsPresent.EmitColors;
                            fields++;
                            break;
                    }
                    if (i + fields + 2 >= data.Count)
                        break;

                }
                Vertexes.AddRange(data[i + 2].Where((v) => v != null));
                if (fieldsPresent.HasFlag(FieldsPresent.UV_Color))
                {
                    var uv_con = data[i + 3].Where((v) => v != null);
                    foreach (var e in uv_con)
                    {
                        var conn = (e.GetBinaryW() & 0xFF00) >> 8;
                        Connection.Add(conn == 128 ? false : true);
                        var r = Math.Min(e.GetBinaryX() & 0xFF, 255);
                        var g = Math.Min(e.GetBinaryY() & 0xFF, 255);
                        var b = Math.Min(e.GetBinaryZ() & 0xFF, 255);
                        var a = (e.GetBinaryW() & 0xFF) << 1;

                        Color col = new Color((byte)r, (byte)g, (byte)b, (byte)a);
                        Colors.Add(col);

                        Vector4 uv = new Vector4(e);
                        uv.SetBinaryX(uv.GetBinaryX() & 0xFFFFFF00);
                        uv.SetBinaryY(uv.GetBinaryY() & 0xFFFFFF00);
                        uv.SetBinaryZ(uv.GetBinaryZ() & 0xFFFFFF00);
                        uv.Y = 1 - uv.Y;
                        UVW.Add(uv);
                    }
                }
                if (fieldsPresent.HasFlag(FieldsPresent.Normals))
                {
                    foreach (var e in data[i + 4])
                    {
                        if (e == null)
                            break;
                        Normals.Add(new Vector4(e.X, e.Y, e.Z, 1.0f));
                    }
                }
                if (fieldsPresent.HasFlag(FieldsPresent.EmitColors))
                {
                    foreach (var e in data[i + fields + 1])
                    {
                        if (e == null)
                            break;
                        Vector4 emit = new Vector4(e);
                        emit.X = (emit.GetBinaryX() & 0xFF);// / 256.0f;
                        emit.Y = (emit.GetBinaryY() & 0xFF);// / 256.0f;
                        emit.Z = (emit.GetBinaryZ() & 0xFF);// / 256.0f;
                        emit.W = (emit.GetBinaryW() & 0xFF);// / 256.0f;
                        EmitColor.Add(emit);
                    }
                }
                i += fields + 2;
                TrimList(UVW, Vertexes.Count);
                TrimList(EmitColor, Vertexes.Count);
                TrimList(Normals, Vertexes.Count, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
            }

            for (int i = 0; i < Vertexes.Count; i++)
            {
                var vertData = new VertexData();
                vertData.X = Vertexes[i].X;
                vertData.Y = Vertexes[i].Y;
                vertData.Z = Vertexes[i].Z;
                vertData.U = UVW[i].X;
                vertData.V = UVW[i].Y;
                vertData.R = Colors[i].R;
                vertData.G = Colors[i].G;
                vertData.B = Colors[i].B;
                vertData.A = Colors[i].A;
                vertData.ER = (byte)EmitColor[i].X;
                vertData.EG = (byte)EmitColor[i].Y;
                vertData.EB = (byte)EmitColor[i].Z;
                vertData.EA = (byte)EmitColor[i].W;
                vertData.Conn = Connection[i];
                vertexes.Add(vertData);
            }

            return vertexes;
        }

        private void TrimList(List<Vector4> list, Int32 desiredLength, Vector4 defaultValue = null)
        {
            if (list != null)
            {
                if (list.Count > desiredLength)
                {
                    list.RemoveRange(desiredLength, list.Count - desiredLength);
                }
                while (list.Count < desiredLength)
                {
                    if (defaultValue != null)
                    {
                        list.Add(new Vector4(defaultValue));
                    }
                    else
                    {
                        list.Add(new Vector4());
                    }
                }
            }
        }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(SubModels.Count);
            for (int i = 0; i < SubModels.Count; ++i)
            {
                var sub = SubModels[i];
                writer.Write(sub.VertexCount);
                writer.Write(sub.VifCode.Length);
                writer.Write(sub.VifCode);
                writer.Write(sub.UnusedBlob.Length);
                writer.Write(sub.UnusedBlob);
            }
        }

        protected override int GetSize()
        {
            return (int)ItemSize;
        }

        public void Import()
        {
            throw new NotImplementedException();
            /*
            SubModels = RawData.Length;
            Array.Resize(ref SubModel, SubModels);
            for (int i = 0; i <= SubModels - 1; i++)
            {
                var Groups = Math.Ceiling(RawData[i].Length / (double)33);
                SubModel[i].VertexCount = RawData[i].Length;
                SubModel[i].BlockSize = (uint)(12 + Groups * (48 + 16) + RawData[i].Length * (12 + 16 + 12 + 4) + 20);
                SubModel[i].k = (ushort)((SubModel[i].BlockSize - 20) / 16);
                SubModel[i].c = 24576;
                SubModel[i].Null1 = 0;
                SubModel[i].Something = 0x70000FA;
                SubModel[i].Null2 = 0;
                Array.Resize(ref SubModel[i].Group, (int)Groups);
                uint offset = 0;
                for (int j = 0; j <= SubModel[i].Group.Length - 1; j++)
                {
                    {
                        var withBlock = SubModel[i].Group[j];
                        withBlock.SomeNum1 = 0x6C018000;
                        if (j < SubModel[i].Group.Length - 1)
                            withBlock.VertexCount = 33;
                        else
                            withBlock.VertexCount = (byte)(RawData[i].Length - 33 * (SubModel[i].Group.Length - 1));
                        Array.Resize(ref withBlock.Vertex, withBlock.VertexCount);
                        Array.Resize(ref withBlock.VertexData, withBlock.VertexCount);
                        Array.Resize(ref withBlock.UV, withBlock.VertexCount);
                        Array.Resize(ref withBlock.Shit, withBlock.VertexCount);
                        withBlock.Some80h = 128;
                        withBlock.Null1 = 0;
                        withBlock.SomeNum2 = 0x30024000;
                        withBlock.SomeNum3 = 0x512;
                        withBlock.Null2 = 0;
                        withBlock.Signature1 = 0x1000101;
                        withBlock.SomeShit1 = 0x64018001;
                        withBlock.SomeShit2 = (uint)withBlock.VertexCount * 4;
                        withBlock.SomeShit3 = (uint)withBlock.VertexCount + withBlock.Some80h << 8;
                        withBlock.Signature2 = 0x1000104;
                        withBlock.VertHead = 0x68008003 + (uint)withBlock.VertexCount * 65536;  // 0x0380XX68
                        withBlock.VDataHead = 0x6C008004 + (uint)withBlock.VertexCount * 65536;
                        withBlock.UVHead = 0x68008005 + (uint)withBlock.VertexCount * 65536;
                        withBlock.ShiteHead = 0x6E008006 + (uint)withBlock.VertexCount * 65536;
                        for (int k = 0; k <= withBlock.VertexCount - 1; k++)
                        {
                            withBlock.Vertex[k].X = RawData[i][k + offset].X;
                            withBlock.Vertex[k].Y = RawData[i][k + offset].Y;
                            withBlock.Vertex[k].Z = RawData[i][k + offset].Z;
                            withBlock.VertexData[k].X = RawData[i][k + offset].U;
                            withBlock.VertexData[k].Y = RawData[i][k + offset].V;
                            withBlock.VertexData[k].Z = RawData[i][k + offset].W;
                            withBlock.VertexData[k].SomeByte = 127;
                            withBlock.VertexData[k].CONN = RawData[i][k + offset].CONN == true ? (byte)0 : (byte)128;
                            withBlock.UV[k].X = RawData[i][k + offset].Nx;
                            withBlock.UV[k].Y = RawData[i][k + offset].Ny;
                            withBlock.UV[k].Z = RawData[i][k + offset].Nz;
                            withBlock.Shit[k] = RawData[i][k + offset].Diffuse;
                        }
                        offset += withBlock.VertexCount;
                        withBlock.EndSignature1 = 0x14000000;
                        withBlock.EndSignature2 = 0x1000101;
                        Array.Resize(ref withBlock.leftovers, 20);
                        SubModel[i].Group[j] = withBlock;
                    }
                }
            }
            UpdateStream();*/
        }

        public byte[] ToPLY()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter ply = new StreamWriter(stream) { AutoFlush = true })
                {
                    int vertexcount = 0, polycount = 0;
                    for (int i = 0; i < SubModels.Count; ++i)
                    {
                        vertexcount += SubModels[i].Vertexes.Count;
                        for (int f = 0; f < SubModels[i].Vertexes.Count - 2; ++f)
                        {
                            if (SubModels[i].Vertexes[f + 2].Conn)
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
                    ply.WriteLine("property list uchar int vertex_index");
                    ply.WriteLine("end_header");
                    foreach (var s in SubModels) //vertices
                    {
                        foreach (var g in s.Vertexes)
                        {
                            byte red, green, blue;
                            red = (byte)(g.R * 256);
                            green = (byte)(g.G * 256);
                            blue = (byte)(g.B * 256);
                            //if (g.ShiteHead > 0)
                            //{
                            //    red = (byte)((g.Shit[i] & 0xFF00) >> 8);
                            //    green = (byte)((g.Shit[i] & 0xFF0000) >> 16);
                            //    blue = (byte)((g.Shit[i] & 0xFF000000) >> 24);
                            //}
                            ply.WriteLine("{0} {1} {2} {3} {4} {5}", -g.X, g.Y, g.Z, red, green, blue);
                        }
                    }
                    vertexcount = 0;
                    foreach (var s in SubModels) //polys
                    {
                        for (int i = 0; i < s.Vertexes.Count - 2; ++i)
                        {
                            if (s.Vertexes[i].Conn)
                                ply.WriteLine("3 {0} {1} {2}", vertexcount + ((i & 0x1) == 0x1 ? i + 1 : i + 0), vertexcount + ((i & 0x1) == 0x1 ? i + 0 : i + 1), vertexcount + ((i & 0x1) == 0x1 ? i + 2 : i + 2));
                        }
                        vertexcount += s.Vertexes.Count;
                    }
                    return stream.ToArray();
                }
            }
        }

        #region STRUCTURES
        public struct SubModel
        {
            // Primary Header
            public int VertexCount;
            public Byte[] VifCode { get; set; }
            public Byte[] UnusedBlob { get; set; }
            public List<VertexData> Vertexes;
        }
        public struct VertexData
        {
            public float X, Y, Z;
            public float U, V;
            public byte R, G, B, A;
            public byte ER, EG, EB, EA; // Emit colors
            public bool Conn;
        }
        #endregion
    }
}
