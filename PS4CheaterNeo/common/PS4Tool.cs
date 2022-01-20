using libdebug;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace PS4CheaterNeo
{
    public static class PS4Tool
    {
        private static readonly Mutex mutex = new Mutex();
        private static PS4DBG ps4;

        /// <summary>
        /// Create a PS4DBG instance with the specified IP and connect with socket
        /// </summary>
        /// <param name="ip">specify the connection destination IP</param>
        /// <param name="msg">message returned when connection fails</param>
        /// <param name="connectTimeout">connect timeout of socket</param>
        /// <param name="sendTimeout">send timeout of socket</param>
        /// <param name="receiveTimeout">receive timeout of socket</param>
        /// <returns>return whether the connection is successful</returns>
        public static bool Connect(string ip, out string msg, int connectTimeout = 10000, int sendTimeout = 10000, int receiveTimeout = 10000)
        {
            mutex.WaitOne();
            msg = "";
            bool result = false;
            try
            {
                ps4 = new PS4DBG(ip);
                if (!ps4.IsConnected) ps4.Connect(connectTimeout, sendTimeout, receiveTimeout);
                result = true;
            }
            catch (Exception exception) { msg = exception.Message; }
            finally { mutex.ReleaseMutex(); }
            return result;
        }

        /// <summary>
        /// Used to check if it is connected
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void ConnectedCheck()
        {
            if (ps4 == null || (ps4 != null && !ps4.IsConnected)) throw new Exception("PS4DBG is not connected.");
        }

        /// <summary>
        /// get process list
        /// </summary>
        /// <returns>libdebug.ProcessList</returns>
        public static ProcessList GetProcessList()
        {
            mutex.WaitOne();
            ConnectedCheck();
            ProcessList processList = null;
            try { processList = ps4.GetProcessList(); }
            catch { }
            finally { mutex.ReleaseMutex(); }
            return processList;
        }

        /// <summary>
        /// get the process info of the specified process ID
        /// </summary>
        /// <param name="processID">specified process PID</param>
        /// <returns>libdebug.ProcessInfo</returns>
        public static ProcessInfo GetProcessInfo(int processID)
        {
            mutex.WaitOne();
            ConnectedCheck();
            ProcessInfo processInfo = new ProcessInfo();
            try { processInfo = ps4.GetProcessInfo(processID); }
            catch { }
            finally { mutex.ReleaseMutex(); }
            return processInfo;
        }

        /// <summary>
        /// get the process info of the specified process name
        /// </summary>
        /// <param name="processName">specified process name</param>
        /// <returns>libdebug.ProcessInfo</returns>
        public static ProcessInfo GetProcessInfo(string processName)
        {
            ProcessInfo processInfo = new ProcessInfo();
            ProcessList processList = GetProcessList();

            if (processList == null) return processInfo;

            for (int idx = 0; idx < processList.processes.Length; idx++)
            {
                Process process = processList.processes[idx];
                if (process.name != processName) continue;
                processInfo = GetProcessInfo(process.pid);
                break;
            }

            return processInfo;
        }

        /// <summary>
        /// get the process maps of the specified process name
        /// </summary>
        /// <param name="processName">specified process name</param>
        /// <returns>libdebug.ProcessMap</returns>
        public static ProcessMap GetProcessMaps(string processName)
        {
            ProcessInfo processInfo = GetProcessInfo(processName);

            if (processInfo.pid == 0) return null;

            return GetProcessMaps(processInfo.pid);
        }

        /// <summary>
        /// get the process maps of the specified process ID
        /// </summary>
        /// <param name="processID">specified process PID</param>
        /// <returns>libdebug.ProcessMap</returns>
        public static ProcessMap GetProcessMaps(int processID)
        {
            //if (Properties.Settings.Default.DebugMode.Value)
            //{
            //    MemoryEntry[] entries = new MemoryEntry[10];
            //    for (int idx = 0; idx < entries.Length; idx++)
            //    {
            //        entries[idx] = new MemoryEntry
            //        {
            //            name = "Debug" + idx,
            //            start = (ulong)(idx + 1) * 1000000000,
            //            end = (ulong)(idx + 1) * 1000000000 + 102400000,
            //            prot = 0x5,
            //        };
            //    }
            //    ProcessMap pMap = new ProcessMap(processID, entries);

            //    return pMap;
            //}
            mutex.WaitOne();
            ConnectedCheck();
            ProcessMap processMap = null;
            try { processMap = ps4.GetProcessMaps(processID); }
            catch { }
            finally { mutex.ReleaseMutex(); }
            return processMap;
        }

        /// <summary>
        /// Read the destination address from the pointer offsetList
        /// </summary>
        /// <param name="processID">specified process PID</param>
        /// <param name="baseAddress">base address for pointer</param>
        /// <param name="baseOffsetList">base offsets for pointers</param>
        /// <param name="pointerMemoryCaches"></param>
        /// <returns>destination address of pointer</returns>
        public static ulong ReadTailAddress(int processID, ulong baseAddress, List<long> baseOffsetList, in Dictionary<ulong, ulong> pointerMemoryCaches)
        {
            //long[] offsets = new long[baseOffsetList.Count + 1];
            //offsets[0] = (long)baseAddress;
            //for (int idx = 1; idx < offsets.Length; idx++) offsets[idx] = baseOffsetList[idx - 1];

            List<long> offsetList = new List<long>();
            offsetList.Add((long)(baseAddress));
            offsetList.AddRange(baseOffsetList);

            return ReadTailAddress(processID, offsetList, pointerMemoryCaches);
        }

        /// <summary>
        /// Read the destination address from the pointer offsetList
        /// </summary>
        /// <param name="processID">specified process PID</param>
        /// <param name="offsetList">base address and base offset for pointer</param>
        /// <param name="pointerMemoryCaches">cache the fetched destination address</param>
        /// <returns>destination address of pointer</returns>
        public static ulong ReadTailAddress(int processID, List<long> offsetList, in Dictionary<ulong, ulong> pointerMemoryCaches)
        {
            ulong targetAddr = 0;
            ulong headAddress = 0;
            for (int idx = 0; idx < offsetList.Count; ++idx)
            {
                long offset = offsetList[idx];
                ulong queryAddress = (ulong)offset + headAddress;
                if (idx != offsetList.Count - 1)
                {
                    if (pointerMemoryCaches.TryGetValue(queryAddress, out headAddress)) continue;
                    headAddress = BitConverter.ToUInt64(ReadMemory(processID, queryAddress, 8), 0);
                    pointerMemoryCaches.Add(queryAddress, headAddress);
                }
                else targetAddr = queryAddress;
            }

            return targetAddr;
        }

        /// <summary>
        /// Read the value of the address to the specified process
        /// </summary>
        /// <param name="processID">specified process PID</param>
        /// <param name="address">destination address</param>
        /// <param name="length">length of data to be read</param>
        /// <returns>value of the specified address</returns>
        public static byte[] ReadMemory(int processID, ulong address, int length)
        {
            //if (Properties.Settings.Default.DebugMode.Value)
            //{
            //    int fillLen = 4;
            //    Random rnd = new Random();
            //    byte[] buf = new byte[length];
            //    for (int idx = 0; idx + fillLen < length; ++idx)
            //    {
            //        byte[] tmpBuf = new byte[fillLen];
            //        rnd.NextBytes(tmpBuf);
            //        Buffer.BlockCopy(tmpBuf, 0, buf, idx, fillLen);
            //        idx += tmpBuf.Length;
            //    }
            //    return buf;
            //}
            mutex.WaitOne();
            ConnectedCheck();
            try
            {
                byte[] buf = ps4.ReadMemory(processID, address, length);
                return buf;
            }
            catch { }
            finally { mutex.ReleaseMutex(); }
            return new byte[length];
        }

        /// <summary>
        /// Writes the new value of the address to the specified process
        /// </summary>
        /// <param name="processID">specified process PID</param>
        /// <param name="address">destination address</param>
        /// <param name="data">new value of destination address</param>
        public static void WriteMemory(int processID, ulong address, byte[] data)
        {
            mutex.WaitOne();
            ConnectedCheck();
            try { ps4.WriteMemory(processID, address, data); }
            catch {}
            finally { mutex.ReleaseMutex(); }
        }
    }
}
