using libdebug;
using System;
using System.Collections.Generic;
using System.Threading;

namespace PS4CheaterNeo
{
    public static class PS4Tool
    {
        private static readonly Mutex mutex = new Mutex();
        public static PS4DBG ps4;

        public static bool Connect(string ip, out string msg, int connectTimeout = 10000, int sendTimeout = 10000, int receiveTimeout = 10000)
        {
            msg = "";
            try
            {
                mutex.WaitOne();
                ps4 = new PS4DBG(ip);
                if (!ps4.IsConnected) ps4.Connect(connectTimeout, sendTimeout, receiveTimeout);
                mutex.ReleaseMutex();
                return true;
            }
            catch (Exception exception)
            {
                mutex.ReleaseMutex();
                msg = exception.Message;
                return false;
            }
        }
        public static void ConnectedCheck()
        {
            if (PS4Tool.ps4 == null || (PS4Tool.ps4 != null && !PS4Tool.ps4.IsConnected)) throw new Exception("PS4DBG is not connected.");
        }

        public static ProcessList GetProcessList()
        {
            ConnectedCheck();
            mutex.WaitOne();
            try
            {
                ProcessList processList = ps4.GetProcessList();
                mutex.ReleaseMutex();
                return processList;
            }
            catch
            {
                mutex.ReleaseMutex();
            }
            return null;
        }

        public static ProcessInfo GetProcessInfo(int processID)
        {
            ConnectedCheck();
            mutex.WaitOne();
            try
            {
                ProcessInfo processInfo = ps4.GetProcessInfo(processID);
                mutex.ReleaseMutex();
                return processInfo;
            }
            catch
            {
                mutex.ReleaseMutex();
                ProcessInfo info = new ProcessInfo();
                return info;
            }
        }

        public static ProcessInfo GetProcessInfo(string processName)
        {
            ConnectedCheck();
            ProcessInfo processInfo = new ProcessInfo();
            ProcessList processList = GetProcessList();

            if (processList == null) return processInfo;

            foreach (Process process in processList.processes)
            {
                if (process.name == processName)
                {
                    processInfo = GetProcessInfo(process.pid);
                    break;
                }
            }

            return processInfo;
        }

        public static ProcessMap GetProcessMaps(string processName)
        {
            ProcessInfo processInfo = GetProcessInfo(processName);

            if (processInfo.pid == 0) return null;

            return GetProcessMaps(processInfo.pid);
        }

        public static ProcessMap GetProcessMaps(int processID)
        {
            ConnectedCheck();
            mutex.WaitOne();
            try
            {
                ProcessMap processMap = ps4.GetProcessMaps(processID);
                mutex.ReleaseMutex();
                return processMap;
            }
            catch
            {
                mutex.ReleaseMutex();
                return null;
            }
        }

        public static ulong ReadTailAddress(int processID, ulong baseAddress, List<long> baseOffsetList, in Dictionary<ulong, ulong> pointerMemoryCaches)
        {
            List<long> offsetList = new List<long>();
            offsetList.Add((long)(baseAddress));
            offsetList.AddRange(baseOffsetList);

            return ReadTailAddress(processID, offsetList, pointerMemoryCaches);
        }

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
                    if (!pointerMemoryCaches.TryGetValue(queryAddress, out headAddress))
                    {
                        headAddress = BitConverter.ToUInt64(ReadMemory(processID, queryAddress, 8), 0);
                        pointerMemoryCaches.Add(queryAddress, headAddress);
                    }
                }
                else targetAddr = queryAddress;
            }

            return targetAddr;
        }

        public static byte[] ReadMemory(int processID, ulong address, int length)
        {
            ConnectedCheck();
            mutex.WaitOne();
            try
            {
                byte[] buf = ps4.ReadMemory(processID, address, length);
                mutex.ReleaseMutex();
                return buf;
            }
            catch
            {
                mutex.ReleaseMutex();
            }
            return new byte[length];
        }

        public static void WriteMemory(int processID, ulong address, byte[] data)
        {
            ConnectedCheck();
            mutex.WaitOne();
            try
            {
                ps4.WriteMemory(processID, address, data);
                mutex.ReleaseMutex();
            }
            catch
            {
                mutex.ReleaseMutex();
            }
        }
    }
}
