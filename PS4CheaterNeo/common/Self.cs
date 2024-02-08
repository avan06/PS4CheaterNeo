using System;
using System.Runtime.InteropServices;

namespace PS4CheaterNeo
{
    /// <summary>
    /// Selfutil.Net
    /// https://github.com/avan06/selfutil.net
    /// </summary>
    public class Self
    {
        public static readonly uint PS4_PAGE_SIZE = 0x4000;
        public static readonly uint PS4_PAGE_MASK = 0x3FFF;
        public static readonly uint SELF_MAGIC = 0x1D3D154F;

        /// <summary>
        /// SizeOF:32
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Header
        {
            public UInt32 magic;
            public byte version;
            public byte mode;
            public byte endian;
            public byte attribs;
            public UInt32 keyType;
            public UInt16 headerSize;
            public UInt16 metaSize;
            public UInt64 fileSize;
            public UInt16 numEntries;
            public UInt16 flags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] pad;
        }

        /// <summary>
        /// SizeOF:32
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Entry
        {
            public ulong props;
            public ulong offs;
            public ulong fileSz;
            public ulong memSz;
        }

        /// <summary>
        /// SizeOF:32
        /// </summary>
        public static readonly int SizeSHdr = Marshal.SizeOf(typeof(Header));
        /// <summary>
        /// SizeOF:32
        /// </summary>
        public static readonly int SizeSEntry = Marshal.SizeOf(typeof(Entry));

        public static T BytesToStruct<T>(byte[] data) where T : struct
        {
            T result = default;
            GCHandle handle = default;
            try
            {
                handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                result = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            catch { }
            finally { if (handle.IsAllocated) handle.Free(); }

            return result;
        }
    }
}
