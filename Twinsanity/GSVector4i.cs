using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Twinsanity.Util
{
    public unsafe class GSVector4i
    {
        static Internal128[] m_xff = new Internal128[17];
        static Internal128[] m_x0f = new Internal128[17];

        Internal128 vec = new Internal128();

        [StructLayout(LayoutKind.Explicit)]
        struct Internal128
        {
            [FieldOffset(0)]
            public int x;
            [FieldOffset(4)]
            public int y;
            [FieldOffset(8)]
            public int z;
            [FieldOffset(12)]
            public int w;
            [FieldOffset(0)]
            public int r;
            [FieldOffset(4)]
            public int g;
            [FieldOffset(8)]
            public int b;
            [FieldOffset(12)]
            public int a;
            [FieldOffset(0)]
            public int left;
            [FieldOffset(4)]
            public int top;
            [FieldOffset(8)]
            public int right;
            [FieldOffset(12)]
            public int bottom;
            [FieldOffset(0)]
            public fixed int v[4];
            [FieldOffset(0)]
            public fixed float f32[4];
            [FieldOffset(0)]
            public fixed sbyte i8[16];
            [FieldOffset(0)]
            public fixed short i16[8];
            [FieldOffset(0)]
            public fixed int i32[4];
            [FieldOffset(0)]
            public fixed long i64[2];
            [FieldOffset(0)]
            public fixed byte u8[16];
            [FieldOffset(0)]
            public fixed ushort u16[8];
            [FieldOffset(0)]
            public fixed uint u32[4];
            [FieldOffset(0)]
            public fixed ulong u64[2];
        }
    
        public GSVector4i()
        {
            vec.x = 0;
            vec.y = 0;
            vec.z = 0;
            vec.w = 0;
        }

        public static GSVector4i Read(BinaryReader reader)
        {
            var vec = new GSVector4i();
            var byteVec = reader.ReadBytes(16);
            fixed (byte* b = byteVec, dst = vec.vec.u8)
            {
                byte* pd = dst;
                byte* ps = b;
                for (var i = 0; i < 16; ++i)
                {
                    *pd = *ps;
                    pd++;
                    ps++;
                }
            }
            return vec;
        }

        public static void sw64(ref GSVector4i a, ref GSVector4i b, ref GSVector4i c, ref GSVector4i d)
        {
            GSVector4i e = (GSVector4i)a.MemberwiseClone();
            GSVector4i f = (GSVector4i)c.MemberwiseClone();

            a = e.upl64(b);
            c = e.uph64(b);
            b = f.upl64(d);
            d = f.uph64(d);
        }

        public GSVector4i upl64(GSVector4i a)
        {
            var result = new GSVector4i();

            result.vec.u64[0] = this.vec.u64[0];
            result.vec.u64[1] = a.vec.u64[0];

            return result;
        }

        public GSVector4i uph64(GSVector4i a)
        {
            var result = new GSVector4i();

            result.vec.u64[0] = this.vec.u64[1];
            result.vec.u64[1] = a.vec.u64[1];

            return result;
        }

        public void Write(BinaryWriter writer)
        {
            fixed (byte* ps = vec.u8)
            {
                byte* pSourse = ps;
                for (var i = 0; i < 16; ++i)
                {
                    writer.Write(*pSourse);
                    pSourse++;
                }
            }
        }
    }
}
