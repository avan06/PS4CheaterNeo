using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace PS4CheaterNeo.libdebug
{
    /// <summary>
    /// Copyright by jogolden/ps4debug
    /// https://github.com/jogolden/ps4debug
    /// </summary>
    public partial class PS4DBG
    {
        //debug
        // packet sizes
        //send size
        private const int CMD_DEBUG_ATTACH_PACKET_SIZE    = 4;
        private const int CMD_DEBUG_BREAKPT_PACKET_SIZE   = 16;
        private const int CMD_DEBUG_WATCHPT_PACKET_SIZE   = 24;
        private const int CMD_DEBUG_STOPTHR_PACKET_SIZE   = 4;
        private const int CMD_DEBUG_RESUMETHR_PACKET_SIZE = 4;
        private const int CMD_DEBUG_GETREGS_PACKET_SIZE   = 4;
        private const int CMD_DEBUG_SETREGS_PACKET_SIZE   = 8;
        private const int CMD_DEBUG_STOPGO_PACKET_SIZE    = 4;
        private const int CMD_DEBUG_THRINFO_PACKET_SIZE   = 4;

        private const int CMD_DEBUG_EXT_STOPGO_PACKET_SIZE = 5; //ps4debug ext fw by ctn123

        //receive size
        private const int DEBUG_INTERRUPT_SIZE = 0x4A0;
        private const int DEBUG_THRINFO_SIZE   = 40;
        private const int DEBUG_REGS_SIZE      = 0xB0;
        private const int DEBUG_FPREGS_SIZE    = 0x340;
        private const int DEBUG_DBGREGS_SIZE   = 0x80;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct DebuggerInterruptPacket
        {
            public uint lwpid;
            public uint status;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
            public string tdname;
            public regs reg64;
            public fpregs savefpu;
            public dbregs dbreg64;
        }

        private Socket debuggerServer;
        private Socket debuggerClient;
        private void DebuggerThread(object obj)
        {
            DebuggerInterruptCallback callback = null;
            if (obj != null) callback = (DebuggerInterruptCallback)obj;

            IPAddress ip = IPAddress.Parse("0.0.0.0");
            IPEndPoint endpoint = new IPEndPoint(ip, PS4DBG_DEBUG_PORT);

            using (debuggerServer = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                debuggerServer.Bind(endpoint);
                debuggerServer.Listen(0);

                IsDebugging = true;

                using (debuggerClient = debuggerServer.Accept())
                {
                    debuggerClient.NoDelay = true;
                    debuggerClient.Blocking = false;

                    if (obj != null)
                    {
                        while (IsDebugging)
                        {
                            if (debuggerClient.Available == DEBUG_INTERRUPT_SIZE)
                            {
                                byte[] data = new byte[DEBUG_INTERRUPT_SIZE];
                                int bytes = debuggerClient.Receive(data, DEBUG_INTERRUPT_SIZE, SocketFlags.None);
                                if (bytes == DEBUG_INTERRUPT_SIZE)
                                {
                                    DebuggerInterruptPacket packet = (DebuggerInterruptPacket)GetObjectFromBytes(data, typeof(DebuggerInterruptPacket));
                                    callback(packet.lwpid, packet.status, packet.tdname, packet.reg64, packet.savefpu, packet.dbreg64);
                                }
                            }
                            Thread.Sleep(100);
                        }
                    }
                }

                if (debuggerServer.Connected) debuggerServer.Shutdown(SocketShutdown.Both);
                debuggerServer.Close();
            }
        }

        public int AttachPID { get; private set; }
        /// <summary>
        /// Attach the debugger
        /// </summary>
        /// <param name="pid">Process ID</param>
        /// <param name="callback">DebuggerInterruptCallback implementation</param>
        /// <returns></returns>
        public void AttachDebugger(int pid, DebuggerInterruptCallback callback)
        {
            CheckConnected();

            if (IsDebugging || debugThread != null)
            {
                throw new Exception("libdbg: debugger already running?");
            }

            IsDebugging = false;

            debugThread = new Thread(DebuggerThread) { IsBackground = true };
            debugThread.Start(callback);

            // wait until server is started
            while (!IsDebugging) Thread.Sleep(100);

            SendCMDPacket(CMDS.CMD_DEBUG_ATTACH, CMD_DEBUG_ATTACH_PACKET_SIZE, pid);
            CheckStatus();
            AttachPID = pid;
        }

        /// <summary>
        /// Detach the debugger
        /// </summary>
        /// <returns></returns>
        public void DetachDebugger()
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_DEBUG_DETACH, 0);
            CheckStatus();

            if (IsDebugging && debugThread != null)
            {
                IsDebugging = false;

                debugThread.Join();
                debugThread = null;
            }
        }

        /// <summary>
        /// DetachDebugger will be executed only when AttachPID still exists, otherwise close socket and debugThread
        /// </summary>
        public void TryDetachDebugger()
        {
            if (!IsDebugging) return;

            CheckConnected();

            ProcessInfo processInfo = GetProcessInfo(AttachPID);
            if (IsDebugging && processInfo.pid == AttachPID) DetachDebugger();
            else
            {
                if (IsDebugging) IsDebugging = false;
                if (debuggerServer != null)
                {
                    debuggerServer.Close();
                    debuggerServer.Dispose();
                    debuggerServer = null;
                }
                if (debuggerClient != null)
                {
                    debuggerClient.Close();
                    debuggerClient.Dispose();
                    debuggerClient = null;
                }
                if (debugThread != null)
                {
                    debugThread.Abort();
                    debugThread = null;
                }
            }
        }

        /// <summary>
        /// Stop the current process
        /// </summary>
        /// <returns></returns>
        public void ProcessStop()
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_STOPGO, CMD_DEBUG_STOPGO_PACKET_SIZE, 1);
            CheckStatus();
        }

        /// <summary>
        /// Kill the current process, it will detach before doing so
        /// </summary>
        /// <returns></returns>
        public void ProcessKill()
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_STOPGO, CMD_DEBUG_STOPGO_PACKET_SIZE, 2);
            CheckStatus();
        }

        /// <summary>
        /// Resume the current process
        /// </summary>
        /// <returns></returns>
        public void ProcessResume()
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_STOPGO, CMD_DEBUG_STOPGO_PACKET_SIZE, 0);
            CheckStatus();
        }

        /// <summary>
        /// ps4debug ext fw by ctn123
        /// </summary>
        /// <param name="pid"></param>
        public void ProcessExtStop(int pid)
        {
            if (ExtFWVersion <= 0) return;

            try
            {
                CheckConnected();

                SendCMDPacket(CMDS.CMD_DEBUG_EXT_STOPGO, CMD_DEBUG_EXT_STOPGO_PACKET_SIZE, (uint)pid, (byte)1);
                CheckStatus();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// ps4debug ext fw by ctn123
        /// </summary>
        /// <param name="pid"></param>
        public void ProcessExtResume(int pid)
        {
            if (ExtFWVersion <= 0) return;

            try
            {
                CheckConnected();

                SendCMDPacket(CMDS.CMD_DEBUG_EXT_STOPGO, CMD_DEBUG_EXT_STOPGO_PACKET_SIZE, (uint)pid, (byte)0);
                CheckStatus();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// ps4debug ext fw by ctn123
        /// </summary>
        /// <param name="pid"></param>
        public void ProcessExtKill(int pid)
        {
            if (ExtFWVersion <= 0) return;

            try
            {
                CheckConnected();

                SendCMDPacket(CMDS.CMD_DEBUG_EXT_STOPGO, CMD_DEBUG_EXT_STOPGO_PACKET_SIZE, (uint)pid, (byte)2);
                CheckStatus();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Change breakpoint, to remove said breakpoint send the same index but disable it (address is ignored)
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="enabled">Enabled</param>
        /// <param name="address">Address</param>
        /// <returns></returns>
        public void ChangeBreakpoint(int index, bool enabled, ulong address)
        {
            CheckConnected();
            CheckDebugging();

            if (index >= MAX_BREAKPOINTS)
            {
                throw new Exception("libdbg: breakpoint index out of range");
            }

            SendCMDPacket(CMDS.CMD_DEBUG_BREAKPT, CMD_DEBUG_BREAKPT_PACKET_SIZE, index, Convert.ToInt32(enabled), address);
            CheckStatus();
        }

        /// <summary>
        /// Change watchpoint
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="enabled">Enabled</param>
        /// <param name="length">Length</param>
        /// <param name="breaktype">Break type</param>
        /// <param name="address">Address</param>
        /// <returns></returns>
        public void ChangeWatchpoint(int index, bool enabled, WATCHPT_LENGTH length, WATCHPT_BREAKTYPE breaktype, ulong address)
        {
            CheckConnected();
            CheckDebugging();

            if (index >= MAX_WATCHPOINTS)
            {
                throw new Exception("libdbg: watchpoint index out of range");
            }

            SendCMDPacket(CMDS.CMD_DEBUG_WATCHPT, CMD_DEBUG_WATCHPT_PACKET_SIZE, index, Convert.ToInt32(enabled), (uint)length, (uint)breaktype, address);
            CheckStatus();
        }

        /// <summary>
        /// Get a list of threads from the current process
        /// </summary>
        /// <returns></returns>
        public uint[] GetThreadList()
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_THREADS, 0);
            CheckStatus();

            byte[] data = new byte[sizeof(int)];
            sock.Receive(data, sizeof(int), SocketFlags.None);
            int number = BitConverter.ToInt32(data, 0);

            byte[] threads = ReceiveData(number * sizeof(uint));
            uint[] thrlist = new uint[number];
            for (int i = 0; i < number; i++)
            {
                thrlist[i] = BitConverter.ToUInt32(threads, i * sizeof(uint));
            }

            return thrlist;
        }

        /// <summary>
        /// Get thread information
        /// </summary>
        /// <returns></returns>
        /// <param name="lwpid">Thread identifier</param>
        public ThreadInfo GetThreadInfo(uint lwpid)
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_THRINFO, CMD_DEBUG_THRINFO_PACKET_SIZE, lwpid);
            CheckStatus();

            return (ThreadInfo)GetObjectFromBytes(ReceiveData(DEBUG_THRINFO_SIZE), typeof(ThreadInfo));
        }

        /// <summary>
        /// Stop a thread from running
        /// </summary>
        /// <param name="lwpid">Thread id</param>
        /// <returns></returns>
        public void StopThread(uint lwpid)
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_STOPTHR, CMD_DEBUG_STOPTHR_PACKET_SIZE, lwpid);
            CheckStatus();
        }

        /// <summary>
        /// Resume a thread from being stopped
        /// </summary>
        /// <param name="lwpid">Thread id</param>
        /// <returns></returns>
        public void ResumeThread(uint lwpid)
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_RESUMETHR, CMD_DEBUG_RESUMETHR_PACKET_SIZE, lwpid);
            CheckStatus();
        }

        /// <summary>
        /// Get registers from thread
        /// </summary>
        /// <param name="lwpid">Thread id</param>
        /// <returns></returns>
        public regs GetRegisters(uint lwpid)
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_GETREGS, CMD_DEBUG_GETREGS_PACKET_SIZE, lwpid);
            CheckStatus();

            return (regs)GetObjectFromBytes(ReceiveData(DEBUG_REGS_SIZE), typeof(regs));
        }

        /// <summary>
        /// Set thread registers
        /// </summary>
        /// <param name="lwpid">Thread id</param>
        /// <param name="regs">Register data</param>
        /// <returns></returns>
        public void SetRegisters(uint lwpid, regs regs)
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_SETREGS, CMD_DEBUG_SETREGS_PACKET_SIZE, lwpid, DEBUG_REGS_SIZE);
            CheckStatus();
            SendData(GetBytesFromObject(regs), DEBUG_REGS_SIZE);
            CheckStatus();
        }

        /// <summary>
        /// Get floating point registers from thread
        /// </summary>
        /// <param name="lwpid">Thread id</param>
        /// <returns></returns>
        public fpregs GetFloatRegisters(uint lwpid)
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_GETFPREGS, CMD_DEBUG_GETREGS_PACKET_SIZE, lwpid);
            CheckStatus();

            return (fpregs)GetObjectFromBytes(ReceiveData(DEBUG_FPREGS_SIZE), typeof(fpregs));
        }

        /// <summary>
        /// Set floating point thread registers
        /// </summary>
        /// <param name="lwpid">Thread id</param>
        /// <param name="fpregs">Floating point register data</param>
        /// <returns></returns>
        public void SetFloatRegisters(uint lwpid, fpregs fpregs)
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_SETFPREGS, CMD_DEBUG_SETREGS_PACKET_SIZE, lwpid, DEBUG_FPREGS_SIZE);
            CheckStatus();
            SendData(GetBytesFromObject(fpregs), DEBUG_FPREGS_SIZE);
            CheckStatus();
        }

        /// <summary>
        /// Get debug registers from thread
        /// </summary>
        /// <param name="lwpid">Thread id</param>
        /// <returns></returns>
        public dbregs GetDebugRegisters(uint lwpid)
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_GETDBGREGS, CMD_DEBUG_GETREGS_PACKET_SIZE, lwpid);
            CheckStatus();

            return (dbregs)GetObjectFromBytes(ReceiveData(DEBUG_DBGREGS_SIZE), typeof(dbregs));
        }

        /// <summary>
        /// Set debug thread registers
        /// </summary>
        /// <param name="lwpid">Thread id</param>
        /// <param name="dbregs">debug register data</param>
        /// <returns></returns>
        public void SetDebugRegisters(uint lwpid, dbregs dbregs)
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_SETDBGREGS, CMD_DEBUG_SETREGS_PACKET_SIZE, lwpid, DEBUG_DBGREGS_SIZE);
            CheckStatus();
            SendData(GetBytesFromObject(dbregs), DEBUG_DBGREGS_SIZE);
            CheckStatus();
        }

        /// <summary>
        /// Executes a single instruction
        /// </summary>
        public void SingleStep()
        {
            CheckConnected();
            CheckDebugging();

            SendCMDPacket(CMDS.CMD_DEBUG_SINGLESTEP, 0);
            CheckStatus();
        }
    }
}