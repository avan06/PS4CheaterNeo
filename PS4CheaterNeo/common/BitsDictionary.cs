using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace PS4CheaterNeo
{
    /// <summary>
    /// 
    /// [Reference]
    /// 
    /// A fast selection algorithm by vigna
    /// https://sux4j.di.unimi.it/select.php (https://github.com/vigna/dsiutils)
    /// Folly: Facebook Open-source Library
    /// https://github.com/facebook/folly
    /// Succinct Range Filter (SuRF)
    /// https://github.com/efficient/SuRF
    /// Bit Twiddling Hacks
    /// http://graphics.stanford.edu/~seander/bithacks.html
    /// Hamming weight
    /// https://en.wikipedia.org/wiki/Hamming_weight
    ///
    /// set, clear, and toggle a single bit
    /// https://stackoverflow.com/a/47990
    /// JDK中的BitMap實現之BitSet源碼分析
    /// https://www.cnblogs.com/throwable/p/15759956.html
    /// Bit Hacks：關於一切位操作的魔法
    /// https://zhuanlan.zhihu.com/p/37014715
    /// Find nth SET bit in an int
    /// https://stackoverflow.com/a/7671563
    /// GCC自帶的一些builtin內建函數
    /// https://tomsworkspace.github.io/2021/02/27/GCC自带的一些builtin内建函数
    /// 漫畫：Bitmap算法 整合版
    /// https://mp.weixin.qq.com/s?__biz=MzIxMjE5MTE1Nw==&mid=2653191272&idx=1&sn=9bbcd172b611b455ebfc4b7fb9a6a55e
    /// Does bitmap understand? Used in what scene? What’s the problem?
    /// https://developpaper.com/does-bitmap-understand-used-in-what-scene-whats-the-problem/
    /// Mass data processing-BitMap algorithm
    /// https://www.programmerall.com/article/2197589673/
    /// </summary>
    public class BitsDictionary : ICollection<KeyValuePair<UInt32, byte[]>>, IEnumerable<KeyValuePair<UInt32, byte[]>>, IEnumerable, IDictionary, ICollection, IReadOnlyDictionary<UInt32, byte[]>, IReadOnlyCollection<KeyValuePair<UInt32, byte[]>>, ISerializable, IDeserializationCallback
    {
        #region Fields
        /// <summary>
        /// size of bitMap as sizeof(UInt64)
        /// </summary>
        private const int bitMapSize = 8;
        /// <summary>
        /// number of bits per bitMap
        /// </summary>
        private const int bitMapBits = bitMapSize * 8;
        /// <summary>
        /// size of baseKey as sizeof(UInt32)
        /// </summary>
        private const int baseKeySize = 4;
        /// <summary>
        /// Size of baseKey plus bitMap
        /// </summary>
        private const int headerSize = bitMapSize + baseKeySize;
        /// <summary>
        /// bufferSize => headerSize * 0x100
        /// </summary>
        private const int bufferBitSize = headerSize * 0x100;
        /// <summary>
        /// bufferSize => DataLength * 0x40000
        /// </summary>
        private readonly int bufferDataSize;
        /// <summary>
        /// this value represents how much data can be stored in a buffer
        /// </summary>
        private readonly int dataFactor;
        /// <summary>
        /// Buffers for storing BitMap
        /// </summary>
        private readonly List<byte[]> bufferBits = new List<byte[]>();
        /// <summary>
        /// Buffers for storing data
        /// </summary>
        private readonly List<byte[]> bufferDatas = new List<byte[]>();
        /// <summary>
        /// index for current status
        /// status for add data
        /// status for get data
        /// </summary>
        private ((int Index, int IndexInBit) Current,
            (int BitMapPos, int DataPos, int BufBitsID, int BufDatasID, UInt32 BaseKey, byte[] BufferBit, byte[] BufferData) Add,
            (int BitMapPos, int DataPos, int BufBitsID, int BufDatasID, UInt32 BaseKey, byte[] BufferBit, byte[] BufferData) Get) state;
        #endregion

        #region properties
        /// <summary>
        /// gets the number of elements contained in the BitsDictionary
        /// </summary>
        public int Count { get; private set; } = 0;

        /// <summary>
        /// index of current position
        /// </summary>
        public int Index { get { return state.Current.Index + state.Current.IndexInBit; } }

        /// <summary>
        /// data length, can be any length
        /// </summary>
        public int DataLength { get; private set; } = 0;

        /// <summary>
        /// number of steps, may be 1, 2, 4, 8
        /// Key's Greatest (highest) common divisor, used for Key data compression, when each Key is divisible by this number.
        /// For example, Key data is 100, 104, 108..., then Setp is 4.
        /// </summary>
        public int KeyStep { get; private set; } = 1;
        #endregion

        /// <summary>
        /// The keyStep value will be affected by the alignment of the data
        /// </summary>
        /// <param name="dataLength">Data length, can be any value</param>
        /// <param name="keyStep">number of steps, may be 1, 2, 4</param>
        public BitsDictionary(int keyStep, int dataLength)
        {
            if (keyStep == 0) throw new Exception(string.Format("Invalid keyStep: {0}", keyStep));
            if (dataLength == 0) throw new Exception(string.Format("Invalid dataLength: {0}", dataLength));

            DataLength = dataLength;
            KeyStep = keyStep;
            bufferDataSize = DataLength * 0x4000;
            dataFactor = bufferDataSize / DataLength;
            Clear();
        }

        /// <summary>
        /// add key and value data
        /// </summary>
        /// <param name="key">key, must be a multiple of keyStep</param>
        /// <param name="data">value data</param>
        public void Add(UInt32 key, byte[] data)
        {
            if (DataLength != data.Length) throw new Exception(string.Format("Invalid data length! DataLength:{0}, dataLength:{1}", DataLength, data.Length));
            if (state.Add.BufferBit[state.Add.BitMapPos] == 0)
            {
                state.Add.BaseKey = key;
                Buffer.BlockCopy(BitConverter.GetBytes(key), 0, state.Add.BufferBit, state.Add.BitMapPos + bitMapSize, baseKeySize); //tag base key
            }
            else if (state.Add.BaseKey == 0) state.Add.BaseKey = BitConverter.ToUInt32(state.Add.BufferBit, state.Add.BitMapPos + bitMapSize);
            if (state.Add.BaseKey > key) throw new Exception(string.Format("baseKey must be less than key, baseKey:{0}, key:{1}", state.Add.BaseKey, key));

            int bitPosition = (int)(key - state.Add.BaseKey) / KeyStep;
            if (bitPosition >= bitMapBits)
            {
                bitPosition = 0;
                state.Add.BitMapPos += headerSize;
                if (state.Add.BitMapPos >= bufferBitSize)
                {
                    state.Add.BitMapPos = 0;
                    bufferBits.Add(new byte[bufferBitSize]);
                    state.Add.BufferBit = bufferBits[++state.Add.BufBitsID];
                }
                state.Add.BaseKey = key;
                Buffer.BlockCopy(BitConverter.GetBytes(key), 0, state.Add.BufferBit, state.Add.BitMapPos + bitMapSize, baseKeySize); //tag base key
            }
            state.Add.BufferBit[state.Add.BitMapPos + bitPosition / 8] |= (byte)(1 << (bitPosition % 8)); //Setting bit map

            if (state.Add.DataPos + 1 > dataFactor)
            {
                bufferDatas.Add(new byte[bufferDataSize]);
                state.Add.DataPos = 0;
                state.Add.BufferData = bufferDatas[++state.Add.BufDatasID];
            }
            Buffer.BlockCopy(data, 0, state.Add.BufferData, DataLength * state.Add.DataPos, DataLength); //value
            ++state.Add.DataPos;
            ++Count;
        }

        /// <summary>
        /// write new data from current position
        /// </summary>
        /// <param name="newData">new data</param>
        public void Set(byte[] newData) => Buffer.BlockCopy(newData, 0, state.Get.BufferData, DataLength * state.Get.DataPos, DataLength);

        /// <summary>
        /// automatically obtain the next data, or obtain the data of the specified index
        /// </summary>
        /// <param name="index">use index to get specified data</param>
        /// <returns></returns>
        public (UInt32 key, byte[] data) Get(int index = -1)
        {
            if (index >= Count) throw new Exception(String.Format("Invalid read result, index({0}) >= Count({1})!", index, Count));

            int offset;
            if (index == -1)
            {
                if (state.Current.Index + state.Current.IndexInBit == Count - 1) Begin();
                offset = 1;       //when the index is next
            }
            else if (index == state.Current.Index + state.Current.IndexInBit) offset = 0;
            else offset = index - (state.Current.Index != -1 ? (state.Current.Index + state.Current.IndexInBit) : 0);

            int bitCount;
            UInt64 bits;
            bool readBaseKey = false;
            bits = BitConverter.ToUInt64(state.Get.BufferBit, state.Get.BitMapPos);
            bitCount = BitCount(bits);
            if (offset > 1)
            {
                offset -= bitCount;
                if (offset > 1)
                {
                    readBaseKey = true;
                    do
                    {
                        state.Get.BitMapPos += headerSize;
                        if (state.Get.BitMapPos >= bufferBitSize)
                        {
                            state.Get.BitMapPos = 0;
                            state.Get.BufferBit = bufferBits[++state.Get.BufBitsID];
                        }
                        bits = BitConverter.ToUInt64(state.Get.BufferBit, state.Get.BitMapPos);
                        bitCount = BitCount(bits);
                        offset -= bitCount;
                    }
                    while (offset > 0);
                }
                state.Current.IndexInBit = offset == 0 ? 0 : bitCount + offset;
                state.Current.Index = index - state.Current.IndexInBit;
            }
            else if (offset < 0)  //when index is less than current position
            {
                offset += state.Current.IndexInBit;
                if (offset < 0)
                {
                    readBaseKey = true;
                    do
                    {
                        state.Get.BitMapPos -= headerSize;
                        if (state.Get.BitMapPos < 0)
                        {
                            state.Get.BitMapPos = bufferBitSize - headerSize;
                            state.Get.BufferBit = bufferBits[--state.Get.BufBitsID];
                        }
                        bits = BitConverter.ToUInt64(state.Get.BufferBit, state.Get.BitMapPos);
                        bitCount = BitCount(bits);
                        offset += bitCount;
                    }
                    while (offset < 0);
                }
                state.Current.IndexInBit = offset;
                state.Current.Index = index - state.Current.IndexInBit;
            }
            else if (offset == 1 && (state.Current.Index + state.Current.IndexInBit) < Count - 1)
            {
                readBaseKey = true;
                if (state.Current.Index == -1)
                {
                    state.Current.Index = 0;
                    state.Current.IndexInBit = 0;
                }
                else if (++state.Current.IndexInBit >= bitCount)
                {
                    state.Current.Index += bitCount;
                    state.Current.IndexInBit = 0;
                    state.Get.BitMapPos += headerSize;
                    if (state.Get.BitMapPos >= bufferBitSize)
                    {
                        state.Get.BitMapPos = 0;
                        state.Get.BufferBit = bufferBits[++state.Get.BufBitsID];
                    }
                }
            }
            else if (offset == 0 && state.Current.Index == -1)
            {
                state.Current.Index = 0;
                state.Current.IndexInBit = 0;
            }

            if (state.Get.BaseKey == 0 || readBaseKey) state.Get.BaseKey = BitConverter.ToUInt32(state.Get.BufferBit, state.Get.BitMapPos + bitMapSize);
            int bitPos = SelectBroadword(bits, state.Current.IndexInBit);
            if (bitPos == -1) throw new Exception(String.Format("Invalid bit position(-1): BitMapPos({0}), BitCnt({1})", state.Get.BitMapPos, state.Current.IndexInBit));

            var bufDatasID = (state.Current.Index + state.Current.IndexInBit) / dataFactor;
            if (state.Get.BufDatasID != bufDatasID || state.Get.BufferData == null)
            {
                state.Get.BufDatasID = bufDatasID;
                state.Get.BufferData = bufferDatas[bufDatasID];
            }
            state.Get.DataPos = (state.Current.Index + state.Current.IndexInBit) % dataFactor;

            byte[] data = new byte[DataLength];
            UInt32 key = (UInt32)(bitPos * KeyStep) + state.Get.BaseKey;
            Buffer.BlockCopy(state.Get.BufferData, DataLength * state.Get.DataPos, data, 0, DataLength);
            return (key, data);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Begin()
        {
            state.Current = (-1, -1);
            state.Get = (0, 0, 0, 0, 0, bufferBits[0], bufferDatas[0]);
        }

        /// <summary>
        /// Clear BitsDictionary data
        /// </summary>
        public void Clear()
        {
            bufferBits.Clear();
            bufferDatas.Clear();
            bufferBits.Add(new byte[bufferBitSize]);
            bufferDatas.Add(new byte[bufferDataSize]);
            Count = 0;
            state.Current = (-1, -1);
            state.Get = (0, 0, 0, 0, 0, bufferBits[0], bufferDatas[0]);
            state.Add = (0, 0, 0, 0, 0, bufferBits[0], bufferDatas[0]);
        }

        #region TEST
        /// <summary>
        /// experimental feature with very low throughput
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add<T>(UInt32 key, T value)
        {
            byte[] data = GetBytes<T>(value);
            Add(key, data);
        }

        /// <summary>
        /// experimental feature with very low throughput
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newValue"></param>
        public void Set<T>(T newValue)
        {
            byte[] data = GetBytes<T>(newValue);
            Set(data);
        }

        /// <summary>
        /// experimental feature with very low throughput
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        public (uint key, T value) GetValue<T>(int index = -1) where T : struct
        {
            (uint key, byte[] data) = Get(index);
            T value = FromBytes<T>(data);
            return (key, value);
        }

        /// <summary>
        /// convert a structure to a byte array
        /// https://stackoverflow.com/a/35717498
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public byte[] GetBytes<T>(T value)
        {
            int size = Marshal.SizeOf(value);
            // Both managed and unmanaged buffers required.
            byte[] bytes = new byte[size];
            GCHandle handle = default;
            try
            {
                handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                Marshal.StructureToPtr<T>(value, handle.AddrOfPinnedObject(), false);
            }
            finally
            {
                if (handle.IsAllocated) handle.Free();
            }

            return bytes;
        }

        /// <summary>
        /// convert a structure to a byte array
        /// https://stackoverflow.com/a/35717498
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public T FromBytes<T>(byte[] bytes) where T : struct
        {
            T value = default;
            GCHandle handle = default;
            try
            {
                handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                value = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
            }
            finally
            {
                if (handle.IsAllocated) handle.Free();
            }

            return value;
        }
        #endregion

        #region Bit Method
        /// <summary>
        /// ~0UL / 3
        /// binary: 1 zeros,  1 ones, 
        /// 0101010101010101010101010101010101010101010101010101010101010101
        /// </summary>
        const UInt64 m01 = 0x5555555555555555;
        /// <summary>
        /// ~0UL / 5
        /// binary: 2 zeros,  2 ones
        /// 0011001100110011001100110011001100110011001100110011001100110011
        /// </summary>
        const UInt64 m02 = 0x3333333333333333;
        /// <summary>
        /// ~0UL /0x11
        /// binary: 4 zeros,  4 ones
        /// 0000111100001111000011110000111100001111000011110000111100001111
        /// </summary>
        const UInt64 m04 = 0x0f0f0f0f0f0f0f0f;
        /// <summary>
        /// ~0UL /0x101
        /// binary: 8 zeros,  8 ones
        /// 0000000011111111000000001111111100000000111111110000000011111111
        /// </summary>
        const UInt64 m08 = 0x00ff00ff00ff00ff;
        /// <summary>
        /// binary: 16 zeros,16 ones
        /// 0000000000000000111111111111111100000000000000001111111111111111
        /// </summary>
        const UInt64 m16 = 0x0000ffff0000ffff;
        /// <summary>
        /// binary: 32 zeros,32 ones
        /// 0000000000000000000000000000000011111111111111111111111111111111
        /// </summary>
        const UInt64 m32 = 0x00000000ffffffff;
        /// <summary>
        /// ~0UL /0xFF
        /// least significant bit mask
        /// each byte is 1, the sum of 256 to the power of 0,1,2,3...
        /// binary:
        /// 0000000100000001000000010000000100000001000000010000000100000001
        /// </summary>
        const UInt64 h01 = 0x0101010101010101;
        /// <summary>
        /// 0x80 * h01
        /// most significant bit mask for a 64-bit byte vector
        /// binary:
        /// 1000000010000000100000001000000010000000100000001000000010000000
        /// </summary>
        const UInt64 h80 = 0x8080808080808080;
        /// <summary>
        /// binary:
        /// 0001000100010001000100010001000100010001000100010001000100010001
        /// </summary>
        const UInt64 h11 = 0x1111111111111111;
        /// <summary>
        /// 0x80 << 56 | 0x40 << 48 | 0x20 << 40 | 0x10 << 32 | 0x8 << 24 | 0x4 << 16 | 0x2 << 8 | 0x1
        /// binary:
        /// 1000000001000000001000000001000000001000000001000000001000000001
        /// </summary>
        const UInt64 h8421 = 0x8040201008040201;

        /// <summary>
        /// count set bits in parallel
        /// return the raw result of the sum of bits
        /// </summary>
        /// <param name="bits"></param>
        /// <returns>the raw result of sum</returns>
        public ulong BitRawSum(UInt64 bits)
        {
            bits -= ((bits & 0xA * h11) >> 1);
            bits = (bits & 0x3 * h11) + ((bits >> 2) & 0x3 * h11);
            bits = (bits + (bits >> 4)) & 0xF * h01;
            return bits *= h01;
        }

        /// <summary>
        /// get the number of bits set in BitMap.
        /// </summary>
        /// <param name="bits">BitMap</param>
        /// <returns>the number of bits set in BitMap</returns>
        public int BitCount(UInt64 bits) => (int)(BitRawSum(bits) >> 56);

        /// <summary>
        /// SelectBroadword algorithm by vigna
        /// 
        /// Select the bit position (from the least-significant bit) with the given count (rank)
        /// </summary>
        /// <param name="bits">bits</param>
        /// <param name="rank">rank</param>
        /// <returns>the position in bits of given count (rank)</returns>
        public int SelectBroadword(ulong bits, int rank)
        {
            // Phase 1: sums by byte
            ulong byteSums = BitRawSum(bits);

            if (rank >= (int)(byteSums >> 56)) return -1;

            // Phase 2: compare each byte sum with rank to obtain the relevant byte
            ulong rankStep8 = (uint)rank * h01;

            int byteOffset = (int)(((((rankStep8 | h80) - byteSums) & h80) >> 7) * h01 >> 53) & ~0x7;

            // Phase 3: Locate the relevant byte and make 8 copies with incremental masks
            ulong byteRank = (ulong)rank - (((byteSums << 8) >> byteOffset) & 0xFF);

            ulong spreadBits = (bits >> byteOffset & 0xFF) * h01 & h8421;
            ulong bitSums = (((spreadBits | ((spreadBits | h80) - h01)) & h80) >> 7) * h01;

            // Compute the inside-byte location and return the sum
            ulong byteRankStep8 = byteRank * h01;

            return byteOffset + (int)(((((byteRankStep8 | h80) - bitSums) & h80) >> 7) * h01 >> 56);
        }

        /// <summary>
        /// Count bits set (rank) from the least-significant bit upto a given position
        /// 
        /// bit manipulation in a range
        /// https://stackoverflow.com/a/42601900
        /// </summary>
        /// <param name="bits">bits</param>
        /// <param name="pos">position</param>
        /// <returns>the sum of bits that are set to 1 from the least-signficant bit upto the bit at the given position</returns>
        public int BitCountLSB(UInt64 bits, uint pos)
        {
            //if (pos == 0) return 0;

            UInt64 result = bits & ~((1ul << 63 << 1) - (1ul << (int)pos + 1));
            return (int)(BitRawSum(result) >> 56); //(int)((sizeof(UInt64) - 1) * CHAR_BIT)
        }
        #endregion

        #region implement interface
        public bool IsReadOnly => false;

        public List<uint> KeyList
        {
            get
            {
                Begin();
                List<uint> keys = new List<uint>();
                for (int idx = 0; idx < Count; idx++)
                {
                    (uint key, _) = Get(idx);
                    keys.Add(key);
                }
                return keys;
            }
        }
        public List<byte[]> ValueList
        {
            get
            {
                Begin();
                List<byte[]> datas = new List<byte[]>();
                for (int idx = 0; idx < Count; idx++)
                {
                    (_, byte[] data) = Get(idx);
                    datas.Add(data);
                }
                return datas;
            }
        }

        public ICollection Keys => KeyList;

        public ICollection Values => ValueList;

        public bool IsFixedSize => false;

        public object SyncRoot => throw new NotImplementedException();

        public bool IsSynchronized => throw new NotImplementedException();

        IEnumerable<uint> IReadOnlyDictionary<uint, byte[]>.Keys => KeyList;

        IEnumerable<byte[]> IReadOnlyDictionary<uint, byte[]>.Values => ValueList;

        public byte[] this[uint key]
        {
            get
            {
                TryGetValue(key, out byte[] result);
                return result;
            }
            set => Add(key, value);
        }

        public object this[object key]
        {
            get => this[Convert.ToUInt32(key)];
            set => Add(Convert.ToUInt32(key), (byte[])value);
        }

        public void Add(KeyValuePair<uint, byte[]> item) => Add(item.Key, item.Value);

        public bool Contains(KeyValuePair<uint, byte[]> item) => ContainsKey(item.Key);

        public void CopyTo(KeyValuePair<uint, byte[]>[] array, int arrayIndex) 
        {
            if (array == null) throw new ArgumentNullException("array");
            if (arrayIndex < 0 || arrayIndex > array.Length) throw new ArgumentException("index must be non-negative and within array argument Length");
            if (array.Length - arrayIndex < Count) throw new ArgumentException("array argument plus index offset is too small");

            for (int i = 0; i < Count; i++)
            {
                (uint key, byte[] data) = Get(i);
                array[arrayIndex++] = new KeyValuePair<uint, byte[]>(key, data);
            }
        }

        public bool Remove(KeyValuePair<uint, byte[]> item) => throw new NotImplementedException();

        public IEnumerator<KeyValuePair<uint, byte[]>> GetEnumerator()
        {
            Begin();
            for (int idx = 0; idx < Count; idx++)
            {
                (uint key, byte[] data) = Get(idx);
                yield return new KeyValuePair<uint, byte[]>(key, data);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Contains(object key) => ContainsKey(Convert.ToUInt32(key));

        public void Add(object key, object value) => Add(Convert.ToUInt32(key), value);

        IDictionaryEnumerator IDictionary.GetEnumerator() => new BitsDictionaryEnumerator(GetEnumerator());
        struct BitsDictionaryEnumerator : IDictionaryEnumerator
        {
            private readonly IEnumerator<KeyValuePair<uint, byte[]>> enumerator;
            public object Key => enumerator.Current.Key;
            public object Value => enumerator.Current.Value;
            public BitsDictionaryEnumerator(IEnumerator<KeyValuePair<uint, byte[]>> enumerator) => this.enumerator = enumerator;
            public DictionaryEntry Entry => new DictionaryEntry(enumerator.Current.Key, enumerator.Current.Value);
            public object Current => Entry;
            public bool MoveNext() => enumerator.MoveNext();
            public void Reset() => enumerator.Reset();
        }

        public void Remove(object key) => throw new NotImplementedException();

        public void CopyTo(Array array, int index)
        {

            if (array == null) throw new ArgumentNullException("array");
            if (array.Rank != 1) throw new ArgumentException("array not supported rank multi dim");
            if (array.GetLowerBound(0) != 0) throw new ArgumentException("array does not have zero-based indexing");

            if (array is KeyValuePair<uint, byte[]>[] kvps) CopyTo(kvps, index);
            else
            {
                if (index < 0 || index > array.Length) throw new ArgumentException("index must be non-negative and within array argument Length");
                if (array.Length - index < Count) throw new ArgumentException("array argument plus index offset is too small");
                if (array is DictionaryEntry[] des)
                {
                    for (int i = 0; i < Count; i++)
                    {
                        (uint key, byte[] data) = Get(i);
                        des[index++] = new DictionaryEntry(key, data);
                    }
                }
                else if (array is object[] objects) for (int i = 0; i < Count; i++) objects[index++] = Get(i);
                else throw new ArgumentException("array argument is an invalid type");
            }
        }

        public bool ContainsKey(uint key)
        {
            if (state.Current.Index == -1) state.Current = (0, 0);
            if (key < state.Get.BaseKey)
            {
                do
                {
                    state.Get.BitMapPos -= headerSize;
                    if (state.Get.BitMapPos < 0)
                    {
                        if (state.Get.BufBitsID == 0) return false;
                        state.Get.BitMapPos = bufferBitSize - headerSize;
                        state.Get.BufferBit = bufferBits[--state.Get.BufBitsID];
                    }
                    ulong bitMap = BitConverter.ToUInt64(state.Get.BufferBit, state.Get.BitMapPos);
                    state.Get.BaseKey = BitConverter.ToUInt32(state.Get.BufferBit, state.Get.BitMapPos + bitMapSize);
                    long bitPosition = (key - state.Get.BaseKey) / KeyStep;
                    int bitCount = BitCount(bitMap);
                    if (bitPosition >= bitMapBits)
                    {
                        state.Current.Index -= bitCount == 0 ? 0 : bitCount;
                        state.Current.IndexInBit = 0;
                    }
                    else
                    {
                        bool result = (state.Get.BufferBit[state.Get.BitMapPos + bitPosition / 8] & (byte)(1 << ((int)bitPosition % 8))) != 0;
                        if (result)
                        {
                            var cnt = BitCountLSB(bitMap, (uint)bitPosition);
                            state.Current.Index -= bitCount == 0 ? 0 : bitCount;
                            state.Current.IndexInBit = cnt == 0 ? 0 : cnt - 1;
                        }
                        return result;
                    }

                }
                while (key < state.Get.BaseKey);
            }
            else
            {
                do
                {
                    ulong bitMap = BitConverter.ToUInt64(state.Get.BufferBit, state.Get.BitMapPos);
                    state.Get.BaseKey = BitConverter.ToUInt32(state.Get.BufferBit, state.Get.BitMapPos + bitMapSize);
                    long bitPosition = (key - state.Get.BaseKey) / KeyStep;
                    if (bitPosition >= bitMapBits)
                    {
                        int bitCount = BitCount(bitMap);
                        state.Current.Index += bitCount == 0 ? 0 : bitCount;
                        state.Current.IndexInBit = 0;

                        state.Get.BitMapPos += headerSize;
                        if (state.Get.BitMapPos >= bufferBitSize)
                        {
                            if (state.Get.BufBitsID + 1 > state.Add.BufBitsID) return false;
                            state.Get.BitMapPos = 0;
                            state.Get.BufferBit = bufferBits[++state.Get.BufBitsID];
                        }
                    }
                    else
                    {
                        bool result = (state.Get.BufferBit[state.Get.BitMapPos + bitPosition / 8] & (byte)(1 << ((int)bitPosition % 8))) != 0;
                        if (result)
                        {
                            var cnt = BitCountLSB(bitMap, (uint)bitPosition);
                            state.Current.IndexInBit = cnt == 0 ? 0 : cnt - 1;
                        }
                        return result;
                    }

                }
                while (key >= state.Get.BaseKey);
            }

            return false;
        }

        public bool TryGetValue(uint key, out byte[] value)
        {
            value = null;
            if (!ContainsKey(key)) return false;

            var bufDatasID = (state.Current.Index + state.Current.IndexInBit) / dataFactor;
            if (state.Get.BufDatasID != bufDatasID || state.Get.BufferData == null)
            {
                state.Get.BufDatasID = bufDatasID;
                state.Get.BufferData = bufferDatas[bufDatasID];
            }
            state.Get.DataPos = (state.Current.Index + state.Current.IndexInBit) % dataFactor;

            value = new byte[DataLength];
            Buffer.BlockCopy(state.Get.BufferData, DataLength * state.Get.DataPos, value, 0, DataLength);

            return true;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context) => throw new NotImplementedException();

        public void OnDeserialization(object sender) => throw new NotImplementedException();
        #endregion
    }
}
#region tmp
///// <summary>
///// find the index of target data in bitMap of read.BufferBit
///// </summary>
///// <param name="BitMapPos">position of bitMap</param>
///// <param name="DataPos">position of target data</param>
///// <returns></returns>
//private int BitPosition(int BitMapPos, int DataPos)
//{
//    if (DataPos == 0) return 0;
//    int sum = 0;
//    for (int bIdx = BitMapPos; bIdx < BitMapPos + bitMapSize; ++bIdx)
//    {
//        //for (int idx = 0; idx < 8; ++idx) //This is simpler but requires loop 8*8 times
//        //{
//        //    if ((read.BufferBit[bIdx] & (1ul << idx)) == 0) continue;
//        //    if (sum == DataPos) return idx + (bIdx - BitMapPos) * 8;
//        //    ++sum;
//        //}
//        int result = 0;
//        int tmpSum = sum;
//        switch (read.BufferBit[bIdx] & 0xF)
//        {
//            case 1://0001
//                sum += 1;
//                result = 0;
//                break;
//            case 2://0010
//                sum += 1;
//                result = 1;
//                break;
//            case 3://0011
//                sum += 2;
//                result = 1;
//                break;
//            case 4://0100
//                sum += 1;
//                result = 2;
//                break;
//            case 5://0101
//                sum += 2;
//                result = 2;
//                break;
//            case 6://0110
//                sum += 2;
//                result = 2;
//                break;
//            case 7://0111
//                sum += 3;
//                result = 2;
//                break;
//            case 8://1000
//                sum += 1;
//                result = 3;
//                break;
//            case 9://1001
//                sum += 2;
//                result = 3;
//                break;
//            case 10://1010
//                sum += 2;
//                result = 3;
//                break;
//            case 11://1011
//                sum += 3;
//                result = 3;
//                break;
//            case 12://1100
//                sum += 2;
//                result = 3;
//                break;
//            case 13://1101
//                sum += 3;
//                result = 3;
//                break;
//            case 14://1110
//                sum += 3;
//                result = 3;
//                break;
//            case 15://1111
//                sum += 4;
//                result = 3;
//                break;
//        }
//        if (sum - 1 == DataPos) return result + (bIdx - BitMapPos) * 8;
//        else if (sum - 1 > DataPos)
//        {
//            int subSum = 0;
//            for (int idx = 0; idx < 4; ++idx)
//            {
//                if ((read.BufferBit[bIdx] & (1ul << idx)) == 0) continue;
//                if (subSum + tmpSum == DataPos) return idx + (bIdx - BitMapPos) * 8;
//                ++subSum;
//            }
//        }
//        if (read.BufferBit[bIdx] < 0x10) continue;

//        tmpSum = sum;
//        switch (read.BufferBit[bIdx] & 0xF0)
//        {
//            case 0x10://0001 0000
//                sum += 1;
//                result = 4;
//                break;
//            case 0x20://0010 0000
//                sum += 1;
//                result = 5;
//                break;
//            case 0x30://0011 0000
//                sum += 2;
//                result = 5;
//                break;
//            case 0x40://0100 0000
//                sum += 1;
//                result = 6;
//                break;
//            case 0x50://0101 0000
//                sum += 2;
//                result = 6;
//                break;
//            case 0x60://0110 0000
//                sum += 2;
//                result = 6;
//                break;
//            case 0x70://0111 0000
//                sum += 3;
//                result = 6;
//                break;
//            case 0x80://1000 0000
//                sum += 1;
//                result = 7;
//                break;
//            case 0x90://1001 0000
//                sum += 2;
//                result = 7;
//                break;
//            case 0xA0://1010 0000
//                sum += 2;
//                result = 7;
//                break;
//            case 0xB0://1011 0000
//                sum += 3;
//                result = 7;
//                break;
//            case 0xC0://1100 0000
//                sum += 2;
//                result = 7;
//                break;
//            case 0xD0://1101 0000
//                sum += 3;
//                result = 7;
//                break;
//            case 0xE0://1110 0000
//                sum += 3;
//                result = 7;
//                break;
//            case 0xF0://1111 0000
//                sum += 4;
//                result = 7;
//                break;
//        }
//        if (sum - 1 == DataPos) return result + (bIdx - BitMapPos) * 8;
//        else if (sum - 1 > DataPos)
//        {
//            int subSum = 0;
//            for (int idx = 4; idx < 8; ++idx)
//            {
//                if ((read.BufferBit[bIdx] & (1ul << idx)) == 0) continue;
//                if (subSum + tmpSum == DataPos) return idx + (bIdx - BitMapPos) * 8;
//                ++subSum;
//            }
//        }
//    }
//    return -1;
//}


///// <summary>
///// find the index of target data in bitMap
///// </summary>
///// <param name="bitMap">bitMap</param>
///// <param name="DataPos">position of target data</param>
///// <returns></returns>
//public int BitPosition4(UInt64 bitMap, int DataPos)
//{
//    int sum = 0;
//    for (int idx = 0; idx < bitMapBits; ++idx)
//    {
//        if ((bitMap & (1ul << idx)) == 0) continue;
//        if (sum == DataPos) return idx;
//        ++sum;
//    }
//    return -1;
//}

///// <summary>
///// This is a naive implementation, shown for comparison,
///// and to help in understanding the better functions.
///// This algorithm uses 24 arithmetic operations (shift, add, and).
///// </summary>
///// <param name="bits"></param>
///// <returns></returns>
//public int BitCountForByte(UInt64 bits)
//{
//    bits = (byte)bits;
//    bits = (bits & m01) + (bits >> 1 & m01); //put count of each  2 bits into those  2 bits 
//    bits = (bits & m02) + (bits >> 2 & m02); //put count of each  4 bits into those  4 bits 
//    return (int)(bits % 15);
//}

//public int BitPosition5(UInt64 bits, int DataPos)
//{
//    //if (DataPos == 0) return 0;
//    int idx = 0;
//    int sum = 0;
//    do
//    {
//        var subSum = BitCountForByte(bits);
//        if (sum + subSum < DataPos + 1) bits >>= 8;
//        else
//        {
//            int cnt = 0;
//            for (int subIdx = 0; subIdx < 8; ++subIdx)
//            {
//                if ((bits & 1) != 0) ++cnt;
//                if (sum + cnt == DataPos + 1) return subIdx + idx * 8;
//                bits >>= 1;
//            }
//        }
//        sum += subSum;
//    }
//    while (++idx < 8);
//    return -1;
//}

///// <summary>
///// This is a naive implementation, shown for comparison,
///// and to help in understanding the better functions.
///// This algorithm uses 24 arithmetic operations (shift, add, and).
///// </summary>
///// <param name="bits"></param>
///// <returns></returns>
//public int BitCount2(UInt64 bits)
//{
//    bits = (bits & m01) + (bits >> 1 & m01); //put count of each  2 bits into those  2 bits 
//    bits = (bits & m02) + (bits >> 2 & m02); //put count of each  4 bits into those  4 bits 
//                                             //bits % 15 (bits len = byte)
//    bits = (bits & m04) + (bits >> 4 & m04); //put count of each  8 bits into those  8 bits 
//                                             //bits % 255 (bits len >= UInt16)
//                                             //return (int)(bits % 256);
//    bits = (bits & m08) + (bits >> 8 & m08); //put count of each 16 bits into those 16 bits 
//    bits = (bits & m16) + (bits >> 16 & m16); //put count of each 32 bits into those 32 bits 
//    bits = (bits & m32) + (bits >> 32 & m32); //put count of each 64 bits into those 64 bits 
//    return (int)bits;
//}

///// <summary>
///// Count the consecutive zero bits (trailing) on the right by binary search
///// </summary>
///// <param name="value"></param>
///// <returns></returns>
//public int CLZ64(UInt64 value)
//{
//    int result = 0;
//    if ((value & 0xFFFFFFFF00000000) == 0) { result += 32; value <<= 32; }
//    if ((value & 0xFFFF000000000000) == 0) { result += 16; value <<= 16; }
//    if ((value & 0xFF00000000000000) == 0) { result += 8; value <<= 8; }
//    if ((value & 0xF000000000000000) == 0) { result += 4; value <<= 4; }
//    if ((value & 0xC000000000000000) == 0) { result += 2; value <<= 2; }
//    if ((value & 0x8000000000000000) == 0) { result += 1; value <<= 1; }
//    return result;
//}

///// <summary>
///// Count bits set (rank) from the most-significant bit upto a given position
///// The following finds the the rank of a bit,
///// meaning it returns the sum of bits that are set to 1 from the most-signficant bit downto the bit at the given position.
///// </summary>
///// <param name="value">Compute the rank (bits set) in v from the MSB to pos</param>
///// <param name="pos">Bit position to count bits upto</param>
///// http://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetParallel
///// <returns></returns>
//public int BitCountLeft(UInt64 value, uint pos)
//{
//    // CHAR_BIT is the number of bits per byte (normally 8).
//    uint CHAR_BIT = 8;
//    //Resulting rank of bit at pos goes here
//    UInt64 result;
//    // Shift out bits after given position.
//    result = value >> (int)(sizeof(UInt64) * CHAR_BIT - pos);
//    // Count set bits in parallel.
//    result = BitRawSum(result) >> 56; //(int)((sizeof(UInt64) - 1) * CHAR_BIT)
//    return (int)result;
//}
///// <summary>
///// Select the bit position (from the most-significant bit) with the given count (rank)
///// The following 64-bit code selects the position of the rth 1 bit when counting from the left.
///// In other words if we start at the most significant bit and proceed to the right,
///// counting the number of bits set to 1 until we reach the desired rank, r, then the position where we stop is returned.
///// If the rank requested exceeds the count of bits set, then 64 is returned.
///// The code may be modified for 32-bit or counting from the right.
///// http://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetParallel
///// </summary>
///// <param name="value">Input value to find position with rank r</param>
///// <param name="rank">Input: bit's desired rank [1-64]</param>
///// <returns></returns>
//public int BitPositionLeft(UInt64 value, uint rank)
//{
//    uint result;      // Output: Resulting position of bit with rank r [1-64]
//    uint tmpCount;      // Bit count temporary.
//    UInt64 a_55555555, b_33333333, c_0F0F0F0F, d_00FF00FF; // Intermediate temporaries for bit count.
//    // Do a normal parallel bit count for a 64-bit integer,                     
//    // but store all intermediate steps.                                        
//    // a = (v & m01) + ((v >> 1) & m01);
//    a_55555555 = value - ((value >> 1) & m01);
//    // b = (a & m02) + ((a >> 2) & m02);
//    b_33333333 = (a_55555555 & m02) + ((a_55555555 >> 2) & m02);
//    // c = (b & m04) + ((b >> 4) & m04);
//    c_0F0F0F0F = (b_33333333 + (b_33333333 >> 4)) & m04;
//    // d = (c & m08) + ((c >> 8) & m08);
//    d_00FF00FF = (c_0F0F0F0F + (c_0F0F0F0F >> 8)) & m08;
//    tmpCount = (uint)(d_00FF00FF >> 32) + (uint)(d_00FF00FF >> 48);
//    // Now do branchless select!                                                
//    result = 64;
//    // if (rank > tmpCount) {result -= 32; rank -= tmpCount;}
//    result -= ((tmpCount - rank) & 256) >> 3; rank -= (tmpCount & ((tmpCount - rank) >> 8));
//    tmpCount = (uint)((d_00FF00FF >> (int)(result - 16)) & 0xff);
//    // if (rank > tmpCount) {result -= 16; rank -= tmpCount;}
//    result -= ((tmpCount - rank) & 256) >> 4; rank -= (tmpCount & ((tmpCount - rank) >> 8));
//    tmpCount = (uint)((c_0F0F0F0F >> (int)(result - 8)) & 0xf);
//    // if (rank > tmpCount) {result -= 8; rank -= tmpCount;}
//    result -= ((tmpCount - rank) & 256) >> 5; rank -= (tmpCount & ((tmpCount - rank) >> 8));
//    tmpCount = (uint)((b_33333333 >> (int)(result - 4)) & 0x7);
//    // if (rank > tmpCount) {result -= 4; rank -= tmpCount;}
//    result -= ((tmpCount - rank) & 256) >> 6; rank -= (tmpCount & ((tmpCount - rank) >> 8));
//    tmpCount = (uint)((a_55555555 >> (int)(result - 2)) & 0x3);
//    // if (rank > tmpCount) {result -= 2; rank -= tmpCount;}
//    result -= ((tmpCount - rank) & 256) >> 7; rank -= (tmpCount & ((tmpCount - rank) >> 8));
//    tmpCount = (uint)((value >> (int)(result - 1)) & 0x1);
//    // if (rank > tmpCount) result--;
//    result -= ((tmpCount - rank) & 256) >> 8;
//    result = 65 - result;
//    return (int)result;
//}

//readonly UInt64[] selectInLong = {
//    0x001000200010008, 0x001000200010003, 0x001000200010004, 0x001000200010003, 0x001000200010005, 0x001000200010003, 0x001000200010004, 0x001000200010003,
//    0x001000200010006, 0x001000200010003, 0x001000200010004, 0x001000200010003, 0x001000200010005, 0x001000200010003, 0x001000200010004, 0x001000200010003,
//    0x001000200010007, 0x001000200010003, 0x001000200010004, 0x001000200010003, 0x001000200010005, 0x001000200010003, 0x001000200010004, 0x001000200010003,
//    0x001000200010006, 0x001000200010003, 0x001000200010004, 0x001000200010003, 0x001000200010005, 0x001000200010003, 0x001000200010004, 0x001000200010003,
//    0x102020801080808, 0x102020301030308, 0x102020401040408, 0x102020301030304, 0x102020501050508, 0x102020301030305, 0x102020401040405, 0x102020301030304,
//    0x102020601060608, 0x102020301030306, 0x102020401040406, 0x102020301030304, 0x102020501050506, 0x102020301030305, 0x102020401040405, 0x102020301030304,
//    0x102020701070708, 0x102020301030307, 0x102020401040407, 0x102020301030304, 0x102020501050507, 0x102020301030305, 0x102020401040405, 0x102020301030304,
//    0x102020601060607, 0x102020301030306, 0x102020401040406, 0x102020301030304, 0x102020501050506, 0x102020301030305, 0x102020401040405, 0x102020301030304,
//    0x208080808080808, 0x203030803080808, 0x204040804080808, 0x203030403040408, 0x205050805080808, 0x203030503050508, 0x204040504050508, 0x203030403040405,
//    0x206060806080808, 0x203030603060608, 0x204040604060608, 0x203030403040406, 0x205050605060608, 0x203030503050506, 0x204040504050506, 0x203030403040405,
//    0x207070807080808, 0x203030703070708, 0x204040704070708, 0x203030403040407, 0x205050705070708, 0x203030503050507, 0x204040504050507, 0x203030403040405,
//    0x206060706070708, 0x203030603060607, 0x204040604060607, 0x203030403040406, 0x205050605060607, 0x203030503050506, 0x204040504050506, 0x203030403040405,
//    0x808080808080808, 0x308080808080808, 0x408080808080808, 0x304040804080808, 0x508080808080808, 0x305050805080808, 0x405050805080808, 0x304040504050508,
//    0x608080808080808, 0x306060806080808, 0x406060806080808, 0x304040604060608, 0x506060806080808, 0x305050605060608, 0x405050605060608, 0x304040504050506,
//    0x708080808080808, 0x307070807080808, 0x407070807080808, 0x304040704070708, 0x507070807080808, 0x305050705070708, 0x405050705070708, 0x304040504050507,
//    0x607070807080808, 0x306060706070708, 0x406060706070708, 0x304040604060607, 0x506060706070708, 0x305050605060607, 0x405050605060607, 0x304040504050506,
//    0x808080808080808, 0x808080808080808, 0x808080808080808, 0x408080808080808, 0x808080808080808, 0x508080808080808, 0x508080808080808, 0x405050805080808,
//    0x808080808080808, 0x608080808080808, 0x608080808080808, 0x406060806080808, 0x608080808080808, 0x506060806080808, 0x506060806080808, 0x405050605060608,
//    0x808080808080808, 0x708080808080808, 0x708080808080808, 0x407070807080808, 0x708080808080808, 0x507070807080808, 0x507070807080808, 0x405050705070708,
//    0x708080808080808, 0x607070807080808, 0x607070807080808, 0x406060706070708, 0x607070807080808, 0x506060706070708, 0x506060706070708, 0x405050605060607,
//    0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x508080808080808,
//    0x808080808080808, 0x808080808080808, 0x808080808080808, 0x608080808080808, 0x808080808080808, 0x608080808080808, 0x608080808080808, 0x506060806080808,
//    0x808080808080808, 0x808080808080808, 0x808080808080808, 0x708080808080808, 0x808080808080808, 0x708080808080808, 0x708080808080808, 0x507070807080808,
//    0x808080808080808, 0x708080808080808, 0x708080808080808, 0x607070807080808, 0x708080808080808, 0x607070807080808, 0x607070807080808, 0x506060706070708,
//    0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808,
//    0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x608080808080808,
//    0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x708080808080808,
//    0x808080808080808, 0x808080808080808, 0x808080808080808, 0x708080808080808, 0x808080808080808, 0x708080808080808, 0x708080808080808, 0x607070807080808,
//    0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808,
//    0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808,
//    0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808,
//    0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x808080808080808, 0x708080808080808,};

//readonly UInt64[] overflow = {
//            0x7F*h01, 0x7E*h01, 0x7D*h01, 0x7C*h01, 0x7B*h01, 0x7A*h01, 0x79*h01, 0x78*h01, 0x77*h01, 0x76*h01, 0x75*h01, 0x74*h01, 0x73*h01, 0x72*h01, 0x71*h01, 0x70*h01,
//            0x6F*h01, 0x6E*h01, 0x6D*h01, 0x6C*h01, 0x6B*h01, 0x6A*h01, 0x69*h01, 0x68*h01, 0x67*h01, 0x66*h01, 0x65*h01, 0x64*h01, 0x63*h01, 0x62*h01, 0x61*h01, 0x60*h01,
//            0x5F*h01, 0x5E*h01, 0x5D*h01, 0x5C*h01, 0x5B*h01, 0x5A*h01, 0x59*h01, 0x58*h01, 0x57*h01, 0x56*h01, 0x55*h01, 0x54*h01, 0x53*h01, 0x52*h01, 0x51*h01, 0x50*h01,
//            0x4F*h01, 0x4E*h01, 0x4D*h01, 0x4C*h01, 0x4B*h01, 0x4A*h01, 0x49*h01, 0x48*h01, 0x47*h01, 0x46*h01, 0x45*h01, 0x44*h01, 0x43*h01, 0x42*h01, 0x41*h01, 0x40*h01, };

///// <summary>
///// Required by select64
///// A fast selection algorithm
///// A precomputed table containing in position 256i + j the position of the i-th one (0 ≤ j < 8) in the binary representation of i (0 ≤ i < 256), or 8 if no such bit exists.
/////  kSelectInByte
///// https://sux4j.di.unimi.it/select.php (https://github.com/vigna/dsiutils)
/////
/////  Described in:
/////    http://dsiutils.di.unimi.it/docs/it/unimi/dsi/bits/Fast.html#selectInByte
/////
/////  A precomputed tabled containing the positions of the set bits in the binary
/////  representations of all 8-bit unsigned integers.
/////
/////  For i: [0, 256) ranging over all 8-bit unsigned integers and for j: [0, 8)
/////  ranging over all 0-based bit positions in an 8-bit unsigned integer, the
/////  table entry kSelectInByte[i][j] is the 0-based bit position of the j-th set
/////  bit in the binary representation of i, or 8 if it has fewer than j set bits.
/////
/////  Example: i: 17 (b00010001), j: [0, 8)
/////    kSelectInByte[b00010001][0] = 0
/////    kSelectInByte[b00010001][1] = 4
/////    kSelectInByte[b00010001][2] = 8
/////    ...
/////    kSelectInByte[b00010001][7] = 8
/////  https://github.com/facebook/folly/blob/main/folly/experimental/Select64.h
///// </summary>
//readonly byte[] kSelectInByte = {
//            8, 0, 1, 0, 2, 0, 1, 0, 3, 0, 1, 0, 2, 0, 1, 0, 4, 0, 1, 0, 2, 0, 1, 0, 3, 0, 1, 0, 2, 0, 1, 0, 5, 0, 1, 0, 2, 0, 1, 0, 3, 0, 1, 0, 2, 0, 1, 0, 4, 0, 1, 0, 2, 0, 1, 0, 3, 0, 1, 0, 2, 0, 1, 0,
//            6, 0, 1, 0, 2, 0, 1, 0, 3, 0, 1, 0, 2, 0, 1, 0, 4, 0, 1, 0, 2, 0, 1, 0, 3, 0, 1, 0, 2, 0, 1, 0, 5, 0, 1, 0, 2, 0, 1, 0, 3, 0, 1, 0, 2, 0, 1, 0, 4, 0, 1, 0, 2, 0, 1, 0, 3, 0, 1, 0, 2, 0, 1, 0,
//            7, 0, 1, 0, 2, 0, 1, 0, 3, 0, 1, 0, 2, 0, 1, 0, 4, 0, 1, 0, 2, 0, 1, 0, 3, 0, 1, 0, 2, 0, 1, 0, 5, 0, 1, 0, 2, 0, 1, 0, 3, 0, 1, 0, 2, 0, 1, 0, 4, 0, 1, 0, 2, 0, 1, 0, 3, 0, 1, 0, 2, 0, 1, 0,
//            6, 0, 1, 0, 2, 0, 1, 0, 3, 0, 1, 0, 2, 0, 1, 0, 4, 0, 1, 0, 2, 0, 1, 0, 3, 0, 1, 0, 2, 0, 1, 0, 5, 0, 1, 0, 2, 0, 1, 0, 3, 0, 1, 0, 2, 0, 1, 0, 4, 0, 1, 0, 2, 0, 1, 0, 3, 0, 1, 0, 2, 0, 1, 0,
//            8, 8, 8, 1, 8, 2, 2, 1, 8, 3, 3, 1, 3, 2, 2, 1, 8, 4, 4, 1, 4, 2, 2, 1, 4, 3, 3, 1, 3, 2, 2, 1, 8, 5, 5, 1, 5, 2, 2, 1, 5, 3, 3, 1, 3, 2, 2, 1, 5, 4, 4, 1, 4, 2, 2, 1, 4, 3, 3, 1, 3, 2, 2, 1,
//            8, 6, 6, 1, 6, 2, 2, 1, 6, 3, 3, 1, 3, 2, 2, 1, 6, 4, 4, 1, 4, 2, 2, 1, 4, 3, 3, 1, 3, 2, 2, 1, 6, 5, 5, 1, 5, 2, 2, 1, 5, 3, 3, 1, 3, 2, 2, 1, 5, 4, 4, 1, 4, 2, 2, 1, 4, 3, 3, 1, 3, 2, 2, 1,
//            8, 7, 7, 1, 7, 2, 2, 1, 7, 3, 3, 1, 3, 2, 2, 1, 7, 4, 4, 1, 4, 2, 2, 1, 4, 3, 3, 1, 3, 2, 2, 1, 7, 5, 5, 1, 5, 2, 2, 1, 5, 3, 3, 1, 3, 2, 2, 1, 5, 4, 4, 1, 4, 2, 2, 1, 4, 3, 3, 1, 3, 2, 2, 1,
//            7, 6, 6, 1, 6, 2, 2, 1, 6, 3, 3, 1, 3, 2, 2, 1, 6, 4, 4, 1, 4, 2, 2, 1, 4, 3, 3, 1, 3, 2, 2, 1, 6, 5, 5, 1, 5, 2, 2, 1, 5, 3, 3, 1, 3, 2, 2, 1, 5, 4, 4, 1, 4, 2, 2, 1, 4, 3, 3, 1, 3, 2, 2, 1,
//            8, 8, 8, 8, 8, 8, 8, 2, 8, 8, 8, 3, 8, 3, 3, 2, 8, 8, 8, 4, 8, 4, 4, 2, 8, 4, 4, 3, 4, 3, 3, 2, 8, 8, 8, 5, 8, 5, 5, 2, 8, 5, 5, 3, 5, 3, 3, 2, 8, 5, 5, 4, 5, 4, 4, 2, 5, 4, 4, 3, 4, 3, 3, 2,
//            8, 8, 8, 6, 8, 6, 6, 2, 8, 6, 6, 3, 6, 3, 3, 2, 8, 6, 6, 4, 6, 4, 4, 2, 6, 4, 4, 3, 4, 3, 3, 2, 8, 6, 6, 5, 6, 5, 5, 2, 6, 5, 5, 3, 5, 3, 3, 2, 6, 5, 5, 4, 5, 4, 4, 2, 5, 4, 4, 3, 4, 3, 3, 2,
//            8, 8, 8, 7, 8, 7, 7, 2, 8, 7, 7, 3, 7, 3, 3, 2, 8, 7, 7, 4, 7, 4, 4, 2, 7, 4, 4, 3, 4, 3, 3, 2, 8, 7, 7, 5, 7, 5, 5, 2, 7, 5, 5, 3, 5, 3, 3, 2, 7, 5, 5, 4, 5, 4, 4, 2, 5, 4, 4, 3, 4, 3, 3, 2,
//            8, 7, 7, 6, 7, 6, 6, 2, 7, 6, 6, 3, 6, 3, 3, 2, 7, 6, 6, 4, 6, 4, 4, 2, 6, 4, 4, 3, 4, 3, 3, 2, 7, 6, 6, 5, 6, 5, 5, 2, 6, 5, 5, 3, 5, 3, 3, 2, 6, 5, 5, 4, 5, 4, 4, 2, 5, 4, 4, 3, 4, 3, 3, 2,
//            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 3, 8, 8, 8, 8, 8, 8, 8, 4, 8, 8, 8, 4, 8, 4, 4, 3, 8, 8, 8, 8, 8, 8, 8, 5, 8, 8, 8, 5, 8, 5, 5, 3, 8, 8, 8, 5, 8, 5, 5, 4, 8, 5, 5, 4, 5, 4, 4, 3,
//            8, 8, 8, 8, 8, 8, 8, 6, 8, 8, 8, 6, 8, 6, 6, 3, 8, 8, 8, 6, 8, 6, 6, 4, 8, 6, 6, 4, 6, 4, 4, 3, 8, 8, 8, 6, 8, 6, 6, 5, 8, 6, 6, 5, 6, 5, 5, 3, 8, 6, 6, 5, 6, 5, 5, 4, 6, 5, 5, 4, 5, 4, 4, 3,
//            8, 8, 8, 8, 8, 8, 8, 7, 8, 8, 8, 7, 8, 7, 7, 3, 8, 8, 8, 7, 8, 7, 7, 4, 8, 7, 7, 4, 7, 4, 4, 3, 8, 8, 8, 7, 8, 7, 7, 5, 8, 7, 7, 5, 7, 5, 5, 3, 8, 7, 7, 5, 7, 5, 5, 4, 7, 5, 5, 4, 5, 4, 4, 3,
//            8, 8, 8, 7, 8, 7, 7, 6, 8, 7, 7, 6, 7, 6, 6, 3, 8, 7, 7, 6, 7, 6, 6, 4, 7, 6, 6, 4, 6, 4, 4, 3, 8, 7, 7, 6, 7, 6, 6, 5, 7, 6, 6, 5, 6, 5, 5, 3, 7, 6, 6, 5, 6, 5, 5, 4, 6, 5, 5, 4, 5, 4, 4, 3,
//            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 4, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 5, 8, 8, 8, 8, 8, 8, 8, 5, 8, 8, 8, 5, 8, 5, 5, 4,
//            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 6, 8, 8, 8, 8, 8, 8, 8, 6, 8, 8, 8, 6, 8, 6, 6, 4, 8, 8, 8, 8, 8, 8, 8, 6, 8, 8, 8, 6, 8, 6, 6, 5, 8, 8, 8, 6, 8, 6, 6, 5, 8, 6, 6, 5, 6, 5, 5, 4,
//            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 7, 8, 8, 8, 8, 8, 8, 8, 7, 8, 8, 8, 7, 8, 7, 7, 4, 8, 8, 8, 8, 8, 8, 8, 7, 8, 8, 8, 7, 8, 7, 7, 5, 8, 8, 8, 7, 8, 7, 7, 5, 8, 7, 7, 5, 7, 5, 5, 4,
//            8, 8, 8, 8, 8, 8, 8, 7, 8, 8, 8, 7, 8, 7, 7, 6, 8, 8, 8, 7, 8, 7, 7, 6, 8, 7, 7, 6, 7, 6, 6, 4, 8, 8, 8, 7, 8, 7, 7, 6, 8, 7, 7, 6, 7, 6, 6, 5, 8, 7, 7, 6, 7, 6, 6, 5, 7, 6, 6, 5, 6, 5, 5, 4,
//            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 5,
//            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 6, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 6, 8, 8, 8, 8, 8, 8, 8, 6, 8, 8, 8, 6, 8, 6, 6, 5,
//            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 7, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 7, 8, 8, 8, 8, 8, 8, 8, 7, 8, 8, 8, 7, 8, 7, 7, 5,
//            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 7, 8, 8, 8, 8, 8, 8, 8, 7, 8, 8, 8, 7, 8, 7, 7, 6, 8, 8, 8, 8, 8, 8, 8, 7, 8, 8, 8, 7, 8, 7, 7, 6, 8, 8, 8, 7, 8, 7, 7, 6, 8, 7, 7, 6, 7, 6, 6, 5,
//            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
//            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 6,
//            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 7,
//            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 7, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 7, 8, 8, 8, 8, 8, 8, 8, 7, 8, 8, 8, 7, 8, 7, 7, 6,
//            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
//            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
//            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
//            8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 7};
///// <summary>
///// return the raw result of the sum of bits
///// </summary>
///// <param name="bits"></param>
///// <returns>the raw result of sum</returns>
//public ulong BitRawSum(UInt64 bits)
//{
//    bits -= ((bits >> 1) & m01);                //put count of each 2 bits into those 2 bits
//    bits = (bits & m02) + ((bits >> 2) & m02);  //put count of each 4 bits into those 4 bits
//    bits = (bits + (bits >> 4)) & m04;          //put count of each 8 bits into those 8 bits
//    return bits *= h01;                         //return the raw result of the sum of bits
//}
///// <summary>
///// de Bruijn sequence
///// https://en.wikipedia.org/wiki/De_Bruijn_sequence
///// </summary>
//readonly int[] bitPerm = {
//             0,  1,  2,  7,  3, 13,  8, 19,  4, 25, 14, 28,  9, 34, 20, 40,
//             5, 17, 26, 38, 15, 46, 29, 48, 10, 31, 35, 54, 21, 50, 41, 57,
//            63,  6, 12, 18, 24, 27, 33, 39, 16, 37, 45, 47, 30, 53, 49, 56,
//            62, 11, 23, 32, 36, 44, 52, 55, 61, 22, 43, 51, 60, 42, 59, 58,};
///// <summary>
///// 0x218A392CD3D5DBF is a 64-bit deBruijn number.
///// binary: 001000011000101000111001001011001101001111010101110110111111
///// Count the consecutive zero bits (trailing) on the right in parallel (Bit Twiddling Hacks)
///// http://graphics.stanford.edu/~seander/bithacks.html#ZerosOnRightParallel
///// Reference
///// https://xr1s.me/2018/08/23/gcc-builtin-implementation
///// </summary>
///// <param name="bits"></param>
///// <returns></returns>
//public int CTZ64(UInt64 bits) => bitPerm[0x218A392CD3D5DBF * (bits & ~bits + 1) >> 58];

///// <summary>
///// Returns the position of a bit of given rank (starting from zero) by vigna
///// 
///// A fast selection algorithm
///// https://sux4j.di.unimi.it/select.php (https://github.com/vigna/dsiutils)
///// </summary>
///// <param name="bits"></param>
///// <param name="rank">smaller than the number of ones in bits; impredictable results (including exceptions) might happen if this constraint is violated.</param>
///// <returns>the position in bits of the bit of given rank</returns>
//public int Select(ulong bits, int rank)
//{
//    // Phase 1: sums by byte
//    ulong byteSums = BitRawSum64(bits);

//    if (rank >= (int)(byteSums >> 56)) return -1;

//    // Phase 2: compare each byte sum with rank to obtain the relevant byte
//    int byteOffset = BitCount((((ulong)rank * h01 | h80) - byteSums) & h80) << 3;

//    return byteOffset + kSelectInByte[(bits >> byteOffset) & 0xFF | ((ulong)rank - (((byteSums << 8) >> byteOffset) & 0xFF)) << 8];
//}

//public int Select64(ulong bits, int rank)
//{
//    // Phase 1: sums by byte
//    ulong byteSums = BitRawSum64(bits);

//    if (rank >= (int)(byteSums >> 56)) return -1;

//    // Phase 2: compare each byte sum with rank to obtain the relevant byte
//    ulong kStep8 = (ulong)rank * h01;
//    ulong geqKStep8 = (((kStep8 | h80) - byteSums) & h80);
//    int place = BitCount(geqKStep8) * 8;
//    ulong byteRank = (ulong)rank - (((byteSums << 8) >> place) & 0xFF);

//    return place + kSelectInByte[((bits >> place) & 0xFF) | (byteRank << 8)];
//}

///// <summary>
///// Select Algorithm By Gog and Petri
///// 
///// a fast selection algorithm
///// https://sux4j.di.unimi.it/select.php (https://github.com/vigna/dsiutils)
///// </summary>
///// <param name="bits"></param>
///// <param name="rank"></param>
///// <returns>the position in bits of the bit of given rank</returns>
//public int SelectGogPetri(ulong bits, int rank)
//{
//    // Phase 1: sums by byte
//    ulong byteSums = BitRawSum64(bits);

//    if (rank >= (int)(byteSums >> 56)) return -1;

//    // Phase 2: compare each byte sum with rank to obtain the relevant byte
//    int byteOffset = (CTZ64(byteSums + overflow[rank] & h80) >> 3) << 3;

//    return byteOffset + kSelectInByte[(int)(bits >> byteOffset & 0xFF) | (int)(rank - (int)(((byteSums << 8) >> byteOffset) & 0xFF)) << 8];
//}
//////////////////////////

//ToolStripMsg.Text = "";
//BitsDictionary bitsDict = new BitsDictionary(4, 16);
//ResultList resultList = new ResultList(16, 4);
//System.Diagnostics.Stopwatch tickerMajor = System.Diagnostics.Stopwatch.StartNew();

////BitCount Test
//Random rnd = new Random();
////UInt64 u1 = LongRandom(0, UInt64.MaxValue, rnd);
////UInt64 u2 = LongRandom(0, UInt64.MaxValue, rnd);
////UInt64 u3 = LongRandom(0, UInt64.MaxValue, rnd);
////UInt64 u4 = LongRandom(0, UInt64.MaxValue, rnd);
////UInt64 u5 = LongRandom(0, UInt64.MaxValue, rnd);
////tickerMajor.Restart();
////for (int i = 0; i < 500000000; i++)
////{
////    if (i % 1000 == 0)
////    {
////        u1 = LongRandom(0, UInt64.MaxValue, rnd);
////        u2 = LongRandom(0, UInt64.MaxValue, rnd);
////        u3 = LongRandom(0, UInt64.MaxValue, rnd);
////        u4 = LongRandom(0, UInt64.MaxValue, rnd);
////        u5 = LongRandom(0, UInt64.MaxValue, rnd);
////    }
////    if (i % 2 == 0) bitsDict.SelectBroadword(u1, i % 64);
////    else if (i % 3 == 0) bitsDict.SelectBroadword(u2, i % 64);
////    else if (i % 5 == 0) bitsDict.SelectBroadword(u3, i % 64);
////    else if (i % 7 == 0) bitsDict.SelectBroadword(u4, i % 64);
////    else bitsDict.SelectBroadword(u5, i % 64);
////}
////ToolStripMsg.Text += ", SelectBroadword: " + tickerMajor.Elapsed.TotalSeconds;
////tickerMajor.Restart();
//////BitRawSum64 Test
////tickerMajor.Restart();
////for (int i = 0; i < 500000000; i++)
////{
////    if (i % 1000 == 0)
////    {
////        u1 = LongRandom(0, UInt64.MaxValue, rnd);
////        u2 = LongRandom(0, UInt64.MaxValue, rnd);
////        u3 = LongRandom(0, UInt64.MaxValue, rnd);
////        u4 = LongRandom(0, UInt64.MaxValue, rnd);
////        u5 = LongRandom(0, UInt64.MaxValue, rnd);
////    }
////    if (i % 2 == 0) bitsDict.BitRawSum(u1);
////    else if (i % 3 == 0) bitsDict.BitRawSum(u2);
////    else if (i % 5 == 0) bitsDict.BitRawSum(u3);
////    else if (i % 7 == 0) bitsDict.BitRawSum(u4);
////    else bitsDict.BitRawSum(u5);
////}
////ToolStripMsg.Text += ", BitRawSum64: " + tickerMajor.Elapsed.TotalSeconds;
////tickerMajor.Restart();
//////BitRawSum64 Test

/////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////
//tickerMajor.Restart();
//Byte[] resultBytesA = new byte[] { 1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1, 2, 2, 2, 2 };
//Byte[] resultBytesB = new byte[] { 3, 3, 3, 3, 4, 4, 4, 4, 3, 3, 3, 3, 4, 4, 4, 4 };
//Byte[] resultBytesC = new byte[] { 5, 5, 5, 5, 6, 6, 6, 6, 5, 5, 5, 5, 6, 6, 6, 6 };
//Byte[] resultBytesD = new byte[] { 7, 7, 7, 7, 8, 8, 8, 8, 7, 7, 7, 7, 8, 8, 8, 8 };
//(uint offsetAddr1, byte[] resultBytes1) = (0, null);
//(uint offsetAddr2, byte[] resultBytes2) = (0, null);
//tickerMajor.Restart();
//for (int i = 0; i < 50000000; i++)
//{
//    if (i % 2 == 0) bitsDict.Add((uint)i * 44, resultBytesA);
//    else if (i % 3 == 0) bitsDict.Add((uint)i * 44, resultBytesB);
//    else if (i % 5 == 0) bitsDict.Add((uint)i * 44, resultBytesC);
//    else bitsDict.Add((uint)i * 44, resultBytesD);
//}
//ToolStripMsg.Text += ", r1 Add: " + tickerMajor.Elapsed.TotalSeconds;
//tickerMajor.Restart();
//for (int i = 0; i < 50000000; i++)
//{
//    if (i % 2 == 0) resultList.Add((uint)i * 44, resultBytesA);
//    else if (i % 3 == 0) resultList.Add((uint)i * 44, resultBytesB);
//    else if (i % 5 == 0) resultList.Add((uint)i * 44, resultBytesC);
//    else resultList.Add((uint)i * 44, resultBytesD);
//}
//ToolStripMsg.Text += ", r2 Add: " + tickerMajor.Elapsed.TotalSeconds;
//tickerMajor.Restart();//bitsDict.Begin();
//for (int idx = 0; idx < bitsDict.Count; idx++)
//{
//    if (idx % 10240 != 0) continue;
//    if (!bitsDict.ContainsKey((uint)idx * 44))
//    {
//        throw new Exception(String.Format("ContainsKey++ Test failed, key: {0}", (uint)idx * 44));
//    }
//}
//ToolStripMsg.Text += ", r1 ++ContainsKey: " + tickerMajor.Elapsed.TotalSeconds;
//bitsDict.Begin();
//tickerMajor.Restart();
//for (int idx = bitsDict.Count - 1; idx >= 0; idx--)
//{
//    if (idx % 10240 != 0) continue;
//    if (!bitsDict.ContainsKey((uint)idx * 44))
//    {
//        throw new Exception(String.Format("ContainsKey-- Test failed, key: {0}", (uint)idx * 44));
//    }
//}
//ToolStripMsg.Text += ", r1 --ContainsKey: " + tickerMajor.Elapsed.TotalSeconds;
//resultList.Begin();//bitsDict.Begin();
//tickerMajor.Restart();
//for (int idx = 0; idx < bitsDict.Count; idx++)
//{
//    //if (idx % 173 != 0) continue;
//    if (!bitsDict.TryGetValue((uint)idx * 44, out byte[] test))
//    {
//        throw new Exception(String.Format("ContainsKey++ Test failed, key: {0}", (uint)idx * 44));
//    }
//    //(offsetAddr2, resultBytes2) = resultList.Read(idx);
//    //if (offsetAddr2 != (uint)idx * 44 || !ScanTool.ComparerExact(ScanType.Hex, resultBytes2, test))
//    //{
//    //    throw new Exception(String.Format("1. idx({0}): resultMap({1}) != resultList({2})...", idx, ((uint)idx * 44).ToString("X"), offsetAddr2.ToString("X")));
//    //}
//}
//ToolStripMsg.Text += ", r1 ++TryGetValue: " + tickerMajor.Elapsed.TotalSeconds;
//resultList.Begin();//bitsDict.Begin();
//tickerMajor.Restart();
//for (int idx = bitsDict.Count - 1; idx >= 0; idx--)
//{
//    //if (idx % 510240 != 0) continue;
//    if (!bitsDict.TryGetValue((uint)idx * 44, out byte[] test))
//    {
//        throw new Exception(String.Format("ContainsKey-- Test failed, key: {0}", (uint)idx * 44));
//    }
//    //(offsetAddr2, resultBytes2) = resultList.Read(idx);
//    //if (offsetAddr2 != (uint)idx * 44 || !ScanTool.ComparerExact(ScanType.Hex, resultBytes2, test))
//    //{
//    //    throw new Exception(String.Format("1. idx({0}): resultMap({1}) != resultList({2})...", idx, ((uint)idx * 44).ToString("X"), offsetAddr2.ToString("X")));
//    //}
//}
//ToolStripMsg.Text += ", r1 --TryGetValue: " + tickerMajor.Elapsed.TotalSeconds;
//tickerMajor.Restart();
/////////////////////////////////////////////
//resultList.Begin();//bitsDict.Begin();
//for (int idx = 0; idx < bitsDict.Count; idx++)//resultList_.Begin(); !resultList_.End(); resultList_.Next())
//{
//    //if (idx % 7 != 0) continue;
//    (offsetAddr2, resultBytes2) = resultList.Read();
//    resultList.Next();
//    (offsetAddr1, resultBytes1) = bitsDict.Get(idx);
//    if (offsetAddr2 != offsetAddr1 || !ScanTool.ComparerExact(ScanType.Hex, resultBytes2, resultBytes1))
//    {
//        throw new Exception(String.Format("2. idx({0}): resultMap({1}) != resultList({2})...", idx, offsetAddr1.ToString("X"), offsetAddr2.ToString("X")));
//    }
//}
/////////////////////////////////////////////
//resultList.Begin();//bitsDict.Begin();
//for (int idx = 0; idx < bitsDict.Count; idx++)//resultList_.Begin(); !resultList_.End(); resultList_.Next())
//{
//    //if (idx % 7 != 0) continue;
//    (offsetAddr2, resultBytes2) = resultList.Read();
//    resultList.Next();
//    (offsetAddr1, resultBytes1) = bitsDict.Get();
//    if (offsetAddr2 != offsetAddr1 || !ScanTool.ComparerExact(ScanType.Hex, resultBytes2, resultBytes1))
//    {
//        throw new Exception(String.Format("1. idx({0}): resultMap({1}) != resultList({2})...", idx, offsetAddr1.ToString("X"), offsetAddr2.ToString("X")));
//    }
//}
//tickerMajor.Restart();//bitsDict.Begin();
//for (int idx = 0; idx < bitsDict.Count; idx++)
//{
//    (offsetAddr1, resultBytes1) = bitsDict.Get(idx);
//}
//ToolStripMsg.Text += ", r1 ++Get: " + tickerMajor.Elapsed.TotalSeconds;
//tickerMajor.Restart();//bitsDict.Begin();
//for (int idx = bitsDict.Count - 1; idx >= 0; idx--)
//{
//    (offsetAddr1, resultBytes1) = bitsDict.Get(idx);
//    if (idx % 5165747 == 0) Console.WriteLine(offsetAddr1);
//}
//ToolStripMsg.Text += ", r1 --Get: " + tickerMajor.Elapsed.TotalSeconds;
//tickerMajor.Restart();

//resultList.Begin();
//for (int idx = 0; idx < resultList.Count; idx++)
//{
//    (offsetAddr2, resultBytes2) = resultList.Read();
//    resultList.Next();
//}
//ToolStripMsg.Text += ", r2 Get: " + tickerMajor.Elapsed.TotalSeconds;
//tickerMajor.Restart();//bitsDict.Begin();
//for (int idx = 0; idx < bitsDict.Count; idx++)
//{
//    (offsetAddr1, resultBytes1) = bitsDict.Get();
//    bitsDict.Set(new byte[] { 8, 8, 8, 8, 9, 9, 9, 9, 8, 8, 8, 8, 9, 9, 9, 9 });
//}
//ToolStripMsg.Text += ", r1 Set: " + tickerMajor.Elapsed.TotalSeconds;
//tickerMajor.Restart();
//resultList.Begin();
//for (int idx = 0; idx < resultList.Count; idx++)
//{
//    (offsetAddr2, resultBytes2) = resultList.Read();
//    resultList.Set(new byte[] { 8, 8, 8, 8, 9, 9, 9, 9, 8, 8, 8, 8, 9, 9, 9, 9 });
//    resultList.Next();
//}
//ToolStripMsg.Text += ", r2 Set: " + tickerMajor.Elapsed.TotalSeconds;
//tickerMajor.Restart();

//////resultList.Begin();
//////bitsDict.Begin();
//////for (int idx = 0; idx < bitsDict.Count; idx++)
//////{
//////    rnd.NextBytes(resultBytesA);
//////    (offsetAddr1, resultBytes1) = bitsDict.Get();
//////    (offsetAddr2, resultBytes2) = resultList.Read();
//////    bitsDict.Set(resultBytesA);
//////    resultList.Set(resultBytesA);
//////    resultList.Next();
//////}
//resultList.Begin();//bitsDict.Begin();
//for (int idx = 0; idx < bitsDict.Count; idx++)//resultList_.Begin(); !resultList_.End(); resultList_.Next())
//{
//    //if (idx % 7 != 0) continue;
//    (offsetAddr2, resultBytes2) = resultList.Read();
//    resultList.Next();
//    (offsetAddr1, resultBytes1) = bitsDict.Get(idx);
//    if (offsetAddr2 != offsetAddr1 || !ScanTool.ComparerExact(ScanType.Hex, resultBytes2, resultBytes1))
//    {
//        throw new Exception(String.Format("2. idx({0}): resultMap({1}) != resultList({2})...", idx, offsetAddr1.ToString("X"), offsetAddr2.ToString("X")));
//    }
//}
//tickerMajor.Restart();
//ulong LongRandom(ulong min, ulong max, Random rand)
//{
//    //Working with ulong so that modulo works correctly with values > long.MaxValue
//    ulong uRange = (ulong)(max - min);

//    //Prevent a modolo bias; see https://stackoverflow.com/a/10984975/238419
//    //for more information.
//    //In the worst case, the expected number of calls is 2 (though usually it's
//    //much closer to 1) so this loop doesn't really hurt performance at all.
//    ulong ulongRand;
//    do
//    {
//        byte[] buf = new byte[8];
//        rand.NextBytes(buf);
//        ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
//    } while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

//    return (ulongRand % uRange) + min;
//}
/////////////////////////////////////////////////////////////
#endregion