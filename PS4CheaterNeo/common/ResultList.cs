using System;
using System.Collections.Generic;

namespace PS4CheaterNeo
{
    /// <summary>
    /// ResultList by hurrican6 in PS4_Cheater
    /// https://github.com/hurrican6/PS4_Cheater
    /// </summary>
    public class ResultList
    {
        /// <summary>
        /// bufferSize => 0x10000 = 65536 = 4096 * 16
        /// </summary>
        private const int bufferSize = 0x1000 * 0x10;
        private List<byte[]> bufferList = new List<byte[]>();
        /// <summary>
        /// sizeof(offsetBase)
        /// </summary>
        private const int offsetSize = 4;
        /// <summary>
        /// sizeof(bitMap)
        /// </summary>
        private const int bitMapSize = 8;

        public int BufferTagOffset { get; private set; } = 0;
        public int BufferTagElemCount { get; private set; } = 0;
        public int BufferID { get; private set; } = 0;
        public int Count { get; private set; } = 0;
        public int Iterator { get; private set; } = 0;
        public int ScanTypeLength { get; private set; } = 0;
        public int ScanStep { get; private set; } = 1;

        public ResultList(int scanTypeLength, int scanStep)
        {
            bufferList.Add(new byte[bufferSize]);
            ScanTypeLength = scanTypeLength;
            ScanStep = scanStep;
        }

        private int BitCount(UInt64 data, int end)
        {
            int sum = 0;
            for (int idx = 0; idx <= end; ++idx)
            {
                if ((data & (1ul << idx)) == 0) continue;
                ++sum;
            }
            return sum;
        }

        private int BitPosition(UInt64 data, int pos)
        {
            int sum = 0;
            for (int idx = 0; idx <= 63; ++idx)
            {
                if ((data & (1ul << idx)) == 0) continue;
                if (sum == pos) return idx;
                ++sum;
            }
            return -1;
        }

        public void Add(UInt32 offsetAddr, byte[] resultBytes)
        {
            if (resultBytes.Length != ScanTypeLength) throw new Exception(string.Format("Invalid result length! ScanTypeLength:{0}, resultLength:{1}", ScanTypeLength, resultBytes.Length));

            byte[] databaseBuffer = bufferList[BufferID];

            UInt32 offsetBase = BitConverter.ToUInt32(databaseBuffer, BufferTagOffset);
            UInt64 bitMap = BitConverter.ToUInt64(databaseBuffer, BufferTagOffset + offsetSize);

            if (offsetBase > offsetAddr) throw new Exception(string.Format("Invalid Add offsetAddr! base:{0}, offsetAddr:{1}", offsetBase, offsetAddr));

            if (bitMap == 0)
            {
                offsetBase = offsetAddr;
                Buffer.BlockCopy(BitConverter.GetBytes(offsetAddr), 0, databaseBuffer, BufferTagOffset, offsetSize); //tag address base
            }

            int offsetInBitMap = (int)(offsetAddr - offsetBase) / ScanStep;
            if (offsetInBitMap < 64)
            {
                databaseBuffer[BufferTagOffset + offsetSize + offsetInBitMap / 8] |= (byte)(1 << (offsetInBitMap % 8)); //bit map
                Buffer.BlockCopy(resultBytes, 0, databaseBuffer, BufferTagOffset + offsetSize + bitMapSize + ScanTypeLength * BufferTagElemCount, ScanTypeLength);//value
                ++BufferTagElemCount;
            }
            else
            {
                BufferTagOffset += offsetSize + bitMapSize + ScanTypeLength * BufferTagElemCount;

                if (BufferTagOffset + offsetSize + bitMapSize + ScanTypeLength * 64 >= bufferSize) //Alloc new page
                {
                    bufferList.Add(new byte[bufferSize]);
                    ++BufferID;
                    BufferTagOffset = 0;
                    BufferTagElemCount = 0;
                    databaseBuffer = bufferList[BufferID];
                }
                // |→OffsetBase(4)←|→bitMap(8)←|→Value(ScanTypeLength)←|
                Buffer.BlockCopy(BitConverter.GetBytes(offsetAddr), 0, databaseBuffer, BufferTagOffset, offsetSize); //tag address base
                databaseBuffer[BufferTagOffset + offsetSize] = (byte)1; //bitMap
                Buffer.BlockCopy(resultBytes, 0, databaseBuffer, BufferTagOffset + offsetSize + bitMapSize, ScanTypeLength); //value
                BufferTagElemCount = 1;
            }
            Count++;
        }

        public void Set(byte[] newResultBytes)
        {
            byte[] databaseBuffer = bufferList[BufferID];
            Buffer.BlockCopy(newResultBytes, 0, databaseBuffer, BufferTagOffset + offsetSize + bitMapSize + ScanTypeLength * BufferTagElemCount, ScanTypeLength);
        }

        public (UInt32 offsetAddr, byte[] resultBytes) Read(int index = -1)
        {
            if (index > -1)
            {
                if (index >= Count) throw new Exception(String.Format("Invalid read result, index({0}) >= Count({1})!", index, Count));
                if (Iterator > index) Begin();
                if (index > 0) for (int idx = Iterator; idx < index; idx++) Next();
            }
            UInt32 offsetAddr;
            byte[] resultBytes = new byte[ScanTypeLength];
            byte[] databaseBuffer = bufferList[BufferID];

            UInt32 offsetBase = BitConverter.ToUInt32(databaseBuffer, BufferTagOffset);
            UInt64 bitMap = BitConverter.ToUInt64(databaseBuffer, BufferTagOffset + offsetSize);
            offsetAddr = (UInt32)(BitPosition(bitMap, BufferTagElemCount) * ScanStep) + offsetBase;
            Buffer.BlockCopy(databaseBuffer, BufferTagOffset + offsetSize + bitMapSize + ScanTypeLength * BufferTagElemCount, resultBytes, 0, ScanTypeLength);

            return (offsetAddr, resultBytes);
        }

        public void Begin()
        {
            Iterator = 0;
            BufferTagOffset = 0;
            BufferTagElemCount = 0;
            BufferID = 0;
        }

        public void Next()
        {
            ++Iterator;

            byte[] databaseBuffer = bufferList[BufferID];
            UInt64 bitMap = BitConverter.ToUInt64(databaseBuffer, BufferTagOffset + 4); //UInt32 baseOffset = BitConverter.ToUInt32(databaseBuffer, BufferTagOffset);
            ++BufferTagElemCount;

            if (BitCount(bitMap, 63) <= BufferTagElemCount)
            {
                BufferTagOffset += offsetSize + bitMapSize + ScanTypeLength * BufferTagElemCount;
                if (BufferTagOffset + offsetSize + bitMapSize + ScanTypeLength * 64 >= bufferSize)
                {
                    ++BufferID;
                    BufferTagOffset = 0;
                    BufferTagElemCount = 0;
                }
                else BufferTagElemCount = 0;
            }
        }

        public bool End() { return Iterator == Count; }

        public void Clear()
        {
            Count = 0;
            BufferTagOffset = 0;
            BufferTagElemCount = 0;
            BufferID = 0;
            bufferList.Clear();
            bufferList.Add(new byte[bufferSize]);
        }
    }
}
//using System;
//using System.Collections.Generic;

//namespace PS4CheaterNeo
//{
//    /// <summary>
//    /// ResultList by hurrican6 in PS4_Cheater
//    /// https://github.com/hurrican6/PS4_Cheater
//    /// </summary>
//    public class ResultList_
//    {
//        /// <summary>
//        /// bufferSize => 0x10000 = 65536 = 4096 * 16
//        /// </summary>
//        private const int bufferSize = 0x1000 * 0x10;
//        private List<byte[]> bufferList = new List<byte[]>();
//        /// <summary>
//        /// sizeof(UInt32) is the size of baseAddr
//        /// </summary>
//        private const int baseAddrSize = 4;
//        /// <summary>
//        /// sizeof(UInt64) is the size of bitMap
//        /// </summary>
//        private const int bitMapSize = 8;
//        /// <summary>
//        /// Size of baseAddr plus bitMap
//        /// </summary>
//        private const int headerSize = baseAddrSize + bitMapSize;
//        /// <summary>
//        /// number of bits per bitMap
//        /// </summary>
//        private const int bitMapBits = bitMapSize * 8;

//        private (int BaseAddrInBuffer, int ElementCount, int BufferID) curr = (0, 0, 0);
//        private (int BaseAddrInBuffer, int ElementCount, int BufferID) read = (0, 0, 0);

//        /// <summary>
//        /// number of data
//        /// </summary>
//        public int Count { get; private set; } = 0;

//        /// <summary>
//        /// current position of the Iterator
//        /// </summary>
//        public int Iterator { get; private set; } = 0;

//        /// <summary>
//        /// size of scanType, may be 1, 2, 4, 8
//        /// </summary>
//        public int DataLength { get; private set; } = 0;

//        /// <summary>
//        /// number of steps, may be 1, 2, 4
//        /// </summary>
//        public int Step { get; private set; } = 1;

//        /// <summary>
//        /// The step value will be affected by the alignment of the data
//        /// </summary>
//        /// <param name="dataLength">size of scanType, may be 1, 2, 4, 8</param>
//        /// <param name="step">number of steps, may be 1, 2, 4</param>
//        public ResultList_(int dataLength, int step)
//        {
//            if (dataLength == 0) throw new Exception(string.Format("Invalid dataLength! dataLength: {0}", dataLength));
//            if (step == 0) throw new Exception(string.Format("Invalid step! step: {0}", step));

//            bufferList.Add(new byte[bufferSize]);
//            DataLength = dataLength;
//            Step = step;
//        }

//        /// <summary>
//        /// find the number that has been set in bitMap.
//        /// </summary>
//        /// <param name="bitMap">bitMap</param>
//        /// <returns></returns>
//        private int BitCount(UInt64 bitMap)
//        {
//            int sum = 0;
//            for (int idx = 0; idx < bitMapBits; ++idx)
//            {
//                if ((bitMap & (1ul << idx)) == 0) continue;
//                ++sum;
//            }
//            return sum;
//        }

//        /// <summary>
//        /// find the index of target element in bitMap
//        /// </summary>
//        /// <param name="bitMap">bitMap</param>
//        /// <param name="elementPos">position of target element</param>
//        /// <returns></returns>
//        private int BitPosition(UInt64 bitMap, int elementPos)
//        {
//            int sum = 0;
//            for (int idx = 0; idx < bitMapBits; ++idx)
//            {
//                if ((bitMap & (1ul << idx)) == 0) continue;
//                if (sum == elementPos) return idx;
//                ++sum;
//            }
//            return -1;
//        }

//        /// <summary>
//        /// Store addr in bitMap, saving a lot of memory usage
//        /// 
//        /// addr of the first write is called baseAddr, addr written after the second will resolve to offsetInBitMap values.
//        /// the offsetInBitMap formula is (addr - baseAddr) / ScanStep
//        /// then set the offsetInBitMap value to BitMap, as follows
//        /// O + (I / 8) | (1 << (I % 8)), where O is the current offset(BaseOffsetInBuffer + baseAddrSize) and I is the offsetInBitMap to be inserted
//        /// 
//        /// |→baseAddr(4)←|→bitMap(8)←|→Value(ScanTypeLength) * BufferTagElemCount←|
//        /// 
//        /// Setting a bit: number |= 1UL << n;
//        /// Clearing a bit:number &= ~(1UL << n);
//        /// https://stackoverflow.com/a/47990
//        /// Does bitmap understand? Used in what scene? What’s the problem?
//        /// https://developpaper.com/does-bitmap-understand-used-in-what-scene-whats-the-problem/
//        /// Mass data processing-BitMap algorithm
//        /// https://www.programmerall.com/article/2197589673/
//        /// JDK中的BitMap實現之BitSet源碼分析
//        /// https://www.cnblogs.com/throwable/p/15759956.html
//        /// 漫畫：Bitmap算法 整合版
//        /// https://mp.weixin.qq.com/s?__biz=MzIxMjE5MTE1Nw==&mid=2653191272&idx=1&sn=9bbcd172b611b455ebfc4b7fb9a6a55e
//        /// </summary>
//        /// <param name="addr">add offset address, addr must be a multiple of scanStep</param>
//        /// <param name="data">add the value of offset address</param>
//        public void Add(UInt32 addr, byte[] data)
//        {
//            if (data.Length != DataLength) throw new Exception(string.Format("Invalid result length! ScanTypeLength:{0}, resultLength:{1}", DataLength, data.Length));

//            byte[] buffer = bufferList[curr.BufferID];

//            UInt32 baseAddr = BitConverter.ToUInt32(buffer, curr.BaseAddrInBuffer);
//            UInt64 bitMap = BitConverter.ToUInt64(buffer, curr.BaseAddrInBuffer + baseAddrSize);

//            if (baseAddr > addr) throw new Exception(string.Format("baseAddr must be less than addr, baseAddr:{0}, addr:{1}", baseAddr, addr));
//            if (bitMap == 0)
//            {
//                baseAddr = addr;
//                Buffer.BlockCopy(BitConverter.GetBytes(addr), 0, buffer, curr.BaseAddrInBuffer, baseAddrSize); //tag address base
//            }
//            int bitPosition = (int)(addr - baseAddr) / Step;
//            if (bitPosition >= bitMapBits) //Confirm whether offsetInBitMap reaches 64bit position, because the type of bitMap is UInt64
//            {
//                bitPosition = 0;
//                curr.BaseAddrInBuffer += headerSize + DataLength * curr.ElementCount;
//                if (curr.BaseAddrInBuffer + headerSize + DataLength * bitMapBits >= bufferSize) //Alloc new page, new buffer memory needs to be allocated
//                {
//                    curr.BaseAddrInBuffer = 0;
//                    curr.ElementCount = 0;
//                    bufferList.Add(new byte[bufferSize]);
//                    buffer = bufferList[++curr.BufferID];
//                }
//                Buffer.BlockCopy(BitConverter.GetBytes(addr), 0, buffer, curr.BaseAddrInBuffer, baseAddrSize); //tag address base

//                buffer[curr.BaseAddrInBuffer + baseAddrSize] = (byte)1; //bitMap
//                Buffer.BlockCopy(data, 0, buffer, curr.BaseAddrInBuffer + headerSize, DataLength); //value
//                curr.ElementCount = 1;
//            }
//            else //Indicates that addr is much larger than baseAddr, exceeding the 64bit range of bitMap, and reset baseAddr to addr in the latest address
//            { //O + (I / 8) | (1 << (I % 8)), where O is the current offset(BaseOffsetInBuffer + baseAddrSize) and I is the offsetInBitMap to be inserted
//                buffer[curr.BaseAddrInBuffer + baseAddrSize + bitPosition / 8] |= (byte)(1 << (bitPosition % 8)); //Setting bit map
//                Buffer.BlockCopy(data, 0, buffer, curr.BaseAddrInBuffer + headerSize + DataLength * curr.ElementCount, DataLength);//value
//                ++curr.ElementCount;
//            }
//            Count++;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newData"></param>
//        public void Set(byte[] newData)
//        {
//            byte[] buffer = bufferList[read.BufferID];
//            Buffer.BlockCopy(newData, 0, buffer, read.BaseAddrInBuffer + headerSize + DataLength * read.ElementCount, DataLength);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="index"></param>
//        /// <returns></returns>
//        /// <exception cref="Exception"></exception>
//        public (UInt32 addr, byte[] data) Read(int index = -1)
//        {
//            if (index > -1)
//            {
//                if (index >= Count) throw new Exception(String.Format("Invalid read result, index:{0} >= Count:{1}", index, Count));
//                if (Iterator > index || Iterator < read.ElementCount) Begin();
//                Next(index); //if (index > 0) for (int idx = Iterator; idx < index; idx++) Next();
//            }
//            UInt32 addr;
//            byte[] data = new byte[DataLength];
//            byte[] buffer = bufferList[read.BufferID];

//            UInt32 baseAddr = BitConverter.ToUInt32(buffer, read.BaseAddrInBuffer);
//            UInt64 bitMap = BitConverter.ToUInt64(buffer, read.BaseAddrInBuffer + baseAddrSize);
//            addr = (UInt32)(BitPosition(bitMap, read.ElementCount) * Step) + baseAddr;
//            Buffer.BlockCopy(buffer, read.BaseAddrInBuffer + headerSize + DataLength * read.ElementCount, data, 0, DataLength);

//            return (addr, data);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public void Begin()
//        {
//            Iterator = 0;
//            read = (0, 0, 0);
//        }

//        /// <summary>
//        /// get next data
//        /// </summary>
//        /// <param name="index"></param>
//        public void Next(int index = -1)
//        {
//            if (index >= Count) throw new Exception(String.Format("Invalid next index, index({0}) >= Count({1})!", index, Count));
//            if (index != -1 && Iterator > index || Iterator < read.ElementCount) Begin();

//            byte[] buffer = bufferList[read.BufferID];
//            UInt64 bitMap = BitConverter.ToUInt64(buffer, read.BaseAddrInBuffer + baseAddrSize);
//            var bitCount = BitCount(bitMap);
//            if (index != -1)
//            {
//                if (read.ElementCount > 0)
//                {
//                    Iterator -= read.ElementCount;
//                    read.ElementCount = 0;
//                }
//                while (index > Iterator + bitCount)
//                {
//                    Iterator += bitCount;
//                    read.ElementCount += bitCount;

//                    read.BaseAddrInBuffer += headerSize + DataLength * read.ElementCount;
//                    read.ElementCount = 0;

//                    if (read.BaseAddrInBuffer + headerSize + DataLength * bitMapBits >= bufferSize)
//                    {
//                        read.BaseAddrInBuffer = 0;
//                        buffer = bufferList[++read.BufferID];
//                    }

//                    bitMap = BitConverter.ToUInt64(buffer, read.BaseAddrInBuffer + baseAddrSize);
//                    bitCount = BitCount(bitMap);
//                }
//                if (index <= Iterator + bitCount)
//                {
//                    read.ElementCount += index - Iterator;
//                    Iterator = index;
//                    if (bitCount > read.ElementCount) return;
//                }
//            }
//            else// if (Iterator < Count - 1)
//            {
//                //byte[] buffer = bufferList[read.BufferID];
//                //UInt64 bitMap = BitConverter.ToUInt64(buffer, read.BaseAddrInBuffer + baseAddrSize);
//                ++Iterator;
//                ++read.ElementCount;
//                if (bitCount > read.ElementCount) return;
//            }

//            if (bitCount <= read.ElementCount)
//            {
//                read.BaseAddrInBuffer += headerSize + DataLength * read.ElementCount;
//                read.ElementCount = 0;

//                if (read.BaseAddrInBuffer + headerSize + DataLength * bitMapBits < bufferSize) return;

//                ++read.BufferID;
//                read.BaseAddrInBuffer = 0;
//            }
//        }

//        /// <summary>
//        /// Returns whether iterator has been reached to the end
//        /// </summary>
//        /// <returns></returns>
//        public bool End() => Iterator == Count;

//        /// <summary>
//        /// Clear ResultList data
//        /// </summary>
//        public void Clear()
//        {
//            Count = 0;
//            read = (0, 0, 0);
//            curr = (0, 0, 0);
//            bufferList.Clear();
//            bufferList.Add(new byte[bufferSize]);
//        }
//    }
//}
