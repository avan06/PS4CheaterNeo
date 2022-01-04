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
