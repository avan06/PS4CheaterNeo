using libdebug;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace PS4CheaterNeo
{
    public static class PS4Tool
    {
        private static readonly int initIdx1 = 0;
        private static readonly int initIdx2 = 2;
        private static int currentIdx1 = initIdx1;
        private static int currentIdx2 = initIdx2;
        private static int reTrySocket = 0;

        /// <summary>
        /// Good pattern for using a Global Mutex in C#
        /// https://stackoverflow.com/a/229567
        /// </summary>
        private static readonly string mutexId;
        private static readonly MutexAccessRule allowEveryoneRule;
        private static readonly MutexSecurity mSec;

        private static readonly Mutex mutex;
        private static readonly Mutex[] mutexs;
        private static readonly PS4DBG[] ps4s;
        private static System.Diagnostics.Stopwatch tickerMajor = System.Diagnostics.Stopwatch.StartNew();

        static PS4Tool()
        {
            mutexId = "PS4ToolMutex";
            allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
            mSec = new MutexSecurity();
            mSec.AddAccessRule(allowEveryoneRule);
            mutex = new Mutex(false, mutexId, out _, mSec);

            mutexs = new Mutex[4];
            ps4s = new PS4DBG[mutexs.Length];
        }

        private static int CurrentIdx1()
        {
            mutex.WaitOne();
            try
            {
                int result = currentIdx1++;
                if (currentIdx1 >= initIdx2) currentIdx1 = initIdx1;
                return result;
            }
            finally { mutex.ReleaseMutex(); }
        }

        private static int CurrentIdx2()
        {
            mutex.WaitOne();
            try
            {
                int result = currentIdx2++;
                if (currentIdx2 >= mutexs.Length) currentIdx2 = initIdx2;
                return result;
            }
            finally { mutex.ReleaseMutex(); }
        }

        /// <summary>
        /// Create a PS4DBG instance with the specified IP and connect with socket
        /// </summary>
        /// <param name="ip">specify the connection destination IP</param>
        /// <param name="msg">message returned when connection fails</param>
        /// <param name="connectTimeout">connect timeout of socket in milliseconds</param>
        /// <param name="reCreateInstance">determine whether to create a new instance if the PS4DBG object has been created</param>
        /// <param name="sendTimeout">send timeout of socket in milliseconds</param>
        /// <param name="receiveTimeout">receive timeout of socket in milliseconds</param>
        /// <returns>return whether the connection is successful</returns>
        public static bool Connect(string ip, out string msg, int connectTimeout = 10000, bool reCreateInstance = false, int sendTimeout = 10000, int receiveTimeout = 10000)
        {
            msg = "";
            bool result = false;
            try
            {
                mutex.WaitOne();
                for (int idx = 0; idx < ps4s.Length; idx++)
                {
                    if (mutexs[idx] != null) mutexs[idx].Dispose();
                    mutexs[idx] = new Mutex(false, mutexId + "_" + idx, out _, mSec);
                    if (ps4s[idx] != null && (!ps4s[idx].IsConnected || reCreateInstance))
                    {
                        try { ps4s[idx].Disconnect(); } catch (Exception ex) { Console.WriteLine(ex.Message + "\n" + ex.StackTrace); }
                        ps4s[idx] = null;
                    }
                    if (ps4s[idx] == null) ps4s[idx] = new PS4DBG(ip);

                    if (idx == 0)
                    {
                        result = ps4s[idx].IsConnected ? true : ps4s[idx].Connect(connectTimeout, sendTimeout, receiveTimeout, true);
                        if (result && ps4s[idx].ExtFWVersion == -1)
                        {
                            ps4s[idx].Disconnect();
                            ps4s[idx] = null;
                            ps4s[idx] = new PS4DBG(ip);
                            result = ps4s[idx].Connect(connectTimeout, sendTimeout, receiveTimeout);
                        }
                    }
                    else result = ps4s[idx].IsConnected ? true : ps4s[idx].Connect(connectTimeout, sendTimeout, receiveTimeout);
                }
            }
            catch (Exception ex) { msg = ex.Message; }
            finally { mutex.ReleaseMutex(); }
            return result;
        }

        /// <summary>
        /// Used to check if it is connected
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void ConnectedCheck(int idx = 0)
        {
            if (ps4s[idx] == null && !Connect(Properties.Settings.Default.PS4IP.Value, out string msg, 2000, true)) throw new Exception("PS4DBG detected an anomaly. error: " + msg); ;
            if (!ps4s[idx].IsConnected && !Connect(Properties.Settings.Default.PS4IP.Value, out msg, 2000, true)) throw new Exception("PS4DBG is not connected. error: " + msg);
        }

        /// <summary>
        /// get process list
        /// Socket errorCode 10054: connection closed by peer, this situation means that ps4debug has been abnormal, ps4 may wake up from sleep, no solution.
        /// Socket errorCode 10060: connection timed out, will try to reconnect 5 times and throw error when all fails
        /// </summary>
        /// <returns>libdebug.ProcessList</returns>
        public static ProcessList GetProcessList()
        {
            int current = CurrentIdx2();
            ProcessList processList = null;
            try
            {
                mutexs[current].WaitOne();
                ConnectedCheck(current);
                processList = ps4s[current].GetProcessList();
            }
            catch (SocketException ex)
            {
                if (tickerMajor.Elapsed.TotalSeconds >= 1.5)
                {
                    reTrySocket = tickerMajor.Elapsed.TotalSeconds < 10 ? reTrySocket + 1 : 0;
                    tickerMajor = System.Diagnostics.Stopwatch.StartNew();
                    if ((ex.ErrorCode == 10054 || ex.ErrorCode == 10060) && reTrySocket > 5) throw;
                    Connect(Properties.Settings.Default.PS4IP.Value, out string msg, 1000, true);
                }
            }
            finally { try { mutexs[current].ReleaseMutex(); } catch (Exception) { } }
            return processList;
        }

        /// <summary>
        /// get the process info of the specified process ID
        /// </summary>
        /// <param name="processID">specified process PID</param>
        /// <returns>libdebug.ProcessInfo</returns>
        public static ProcessInfo GetProcessInfo(int processID)
        {
            int current = CurrentIdx2();
            ProcessInfo processInfo = new ProcessInfo();
            try
            {
                mutexs[current].WaitOne();
                ConnectedCheck(current);
                processInfo = ps4s[current].GetProcessInfo(processID);
            }
            catch (Exception) { }
            finally { try { mutexs[current].ReleaseMutex(); } catch (Exception) { } }
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
            int current = CurrentIdx2();
            ProcessMap processMap = null;
            try
            {
                mutexs[current].WaitOne();
                ConnectedCheck(current);
                processMap = ps4s[current].GetProcessMaps(processID);
            }
            catch (SocketException ex)
            {
                if (tickerMajor.Elapsed.TotalSeconds >= 1.5)
                {
                    reTrySocket = tickerMajor.Elapsed.TotalSeconds < 10 ? reTrySocket + 1 : 0;
                    tickerMajor = System.Diagnostics.Stopwatch.StartNew();
                    if ((ex.ErrorCode == 10054 || ex.ErrorCode == 10060) && reTrySocket > 5) throw;
                    Connect(Properties.Settings.Default.PS4IP.Value, out string msg, 1000, true);
                }
            }
            finally { try { mutexs[current].ReleaseMutex(); } catch (Exception) { } }
            return processMap;
        }

        /// <summary>
        /// Read the destination address from the pointer offsetList
        /// </summary>
        /// <param name="processID">specified process PID</param>
        /// <param name="baseAddress">base address for pointer</param>
        /// <param name="baseOffsetList">base offsets for pointers</param>
        /// <param name="pointerCaches"></param>
        /// <returns>destination address of pointer</returns>
        public static ulong ReadTailAddress(int processID, ulong baseAddress, List<long> baseOffsetList, Dictionary<ulong, ulong> pointerCaches = null)
        {
            long[] pointerOffsets = new long[baseOffsetList.Count + 1];
            pointerOffsets[0] = (long)baseAddress;
            for (int idx = 1; idx < pointerOffsets.Length; idx++) pointerOffsets[idx] = baseOffsetList[idx - 1];

            return ReadTailAddress(processID, pointerOffsets, pointerCaches);
        }

        /// <summary>
        /// Read the destination address from the pointer offsetList
        /// </summary>
        /// <param name="processID">specified process PID</param>
        /// <param name="pointerOffsets">base address and base offset for pointer</param>
        /// <param name="pointerCaches">cache the fetched destination address</param>
        /// <returns>destination address of pointer</returns>
        public static ulong ReadTailAddress(int processID, long[] pointerOffsets, Dictionary<ulong, ulong> pointerCaches = null)
        {
            ulong targetAddr = 0;
            ulong headAddress = 0;
            for (int idx = 0; idx < pointerOffsets.Length; ++idx)
            {
                long offset = pointerOffsets[idx];
                ulong queryAddress = (ulong)offset + headAddress;
                if (idx != pointerOffsets.Length - 1)
                {
                    if (pointerCaches != null && pointerCaches.TryGetValue(queryAddress, out headAddress)) continue;
                    headAddress = BitConverter.ToUInt64(ReadMemory(processID, queryAddress, 8), 0);
                    if (pointerCaches != null) pointerCaches.Add(queryAddress, headAddress);
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
            int current = CurrentIdx1();
            try
            {
                mutexs[current].WaitOne();
                ConnectedCheck(current);
                byte[] buf = ps4s[current].ReadMemory(processID, address, length);
                return buf;
            }
            catch (SocketException ex)
            {
                if (tickerMajor.Elapsed.TotalSeconds >= 1.5)
                {
                    reTrySocket = tickerMajor.Elapsed.TotalSeconds < 10 ? reTrySocket + 1 : 0;
                    tickerMajor = System.Diagnostics.Stopwatch.StartNew();
                    if ((ex.ErrorCode == 10054 || ex.ErrorCode == 10060) && reTrySocket > 5) throw;
                    Connect(Properties.Settings.Default.PS4IP.Value, out string msg, 1000, true);
                }
            }
            finally { try { mutexs[current].ReleaseMutex(); } catch (Exception) { } }
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
            int current = CurrentIdx2();
            try
            {
                mutexs[current].WaitOne();
                ConnectedCheck(current);
                ps4s[current].WriteMemory(processID, address, data);
            }
            catch (Exception) {}
            finally { try { mutexs[current].ReleaseMutex(); } catch (Exception) { } }
        }

        private static ProcessStatus processStatus;

        /// <summary>
        /// specify the process to perform PS4DBG's AttachDebugger
        /// </summary>
        /// <param name="processID">process ID</param>
        /// <param name="processName">process name</param>
        /// <param name="isPause">pause or resume process</param>
        public static bool AttachDebugger(int processID, string processName, ProcessStatus newStatus)
        {
            if (ps4s[0].IsConnected && ps4s[0].ExtFWVersion > 0)
            {
                if (processStatus != newStatus)
                {
                    processStatus = newStatus;
                    if (newStatus == ProcessStatus.Pause) ps4s[0].ProcessExtStop(processID);
                    else ps4s[0].ProcessExtResume(processID);
                }
            }
            else if (ps4s[0].IsConnected && ps4s[0].IsDebugging)
            {
                if (processStatus != newStatus)
                {
                    processStatus = newStatus;
                    if (newStatus == ProcessStatus.Pause) ps4s[0].ProcessStop();
                    else ps4s[0].ProcessResume();
                }
            }
            else if (MessageBox.Show("This experimental feature requires Attach ps4 Debugging\n\n" +
                "be sure to close this window before closing the game, otherwise the PS4 will crash, continue?", "Attach warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return false;
            else
            {
                try
                {
                    mutexs[0].WaitOne();
                    ps4s[0].AttachDebugger(processID, null);
                    ps4s[0].Notify(222, "attached to " + processName);
                    processStatus = ProcessStatus.Pause;
                }
                catch (Exception) { return false; }
                finally { try { mutexs[0].ReleaseMutex(); } catch (Exception) { } }
            }
            return true;
        }

        /// <summary>
        /// perform PS4DBG's DetachDebugger
        /// After performing AttachDebugger, close the PS4 program, and then perform PS4DBG.DetachDebugger will cause the PS4 to crash
        /// </summary>
        public static void DetachDebugger(int processID = 0)
        {
            if (ps4s[0] == null) return;
            
            if (ps4s[0].IsConnected && ps4s[0].ExtFWVersion > 0 && processStatus == ProcessStatus.Pause) ps4s[0].ProcessExtResume(processID);

            if (!ps4s[0].IsDebugging) return;

            ps4s[0].TryDetachDebugger();
        }
    }

    public enum ProcessStatus
    {
        None,
        Resume,
        Pause,
    }
}
