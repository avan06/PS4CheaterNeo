namespace PS4CheaterNeo
{
    #region Debug.cs
    /// <summary>
    /// Debugger interrupt callback
    /// </summary>
    /// <param name="lwpid">Thread identifier</param>
    /// <param name="status">status</param>
    /// <param name="tdname">Thread name</param>
    /// <param name="regs">Registers</param>
    /// <param name="fpregs">Floating point registers</param>
    /// <param name="dbregs">Debug registers</param>
    public delegate void DebuggerInterruptCallback(uint lwpid, uint status, string tdname, regs regs, fpregs fpregs, dbregs dbregs);
    #endregion

    #region Enums
    public enum VM_PROTECTIONS : uint
    {
        VM_PROT_NONE = 0x00,
        VM_PROT_READ = 0x01,
        VM_PROT_WRITE = 0x02,
        VM_PROT_EXECUTE = 0x04,
        VM_PROT_DEFAULT = VM_PROT_READ | VM_PROT_WRITE,
        VM_PROT_READEXEC = VM_PROT_READ | VM_PROT_EXECUTE,
        VM_PROT_ALL = VM_PROT_READ | VM_PROT_WRITE | VM_PROT_EXECUTE,
        VM_PROT_NO_CHANGE = 0x08,
        VM_PROT_COPY = 0x10,
        VM_PROT_WANTS_COPY = 0x10
    };
    public enum WATCHPT_LENGTH : uint
    {
        DBREG_DR7_LEN_1 = 0x00, /* 1 byte length */
        DBREG_DR7_LEN_2 = 0x01,
        DBREG_DR7_LEN_4 = 0x03,
        DBREG_DR7_LEN_8 = 0x02,
    };
    public enum WATCHPT_BREAKTYPE : uint
    {
        DBREG_DR7_EXEC = 0x00,  /* break on execute       */
        DBREG_DR7_WRONLY = 0x01,    /* break on write         */
        DBREG_DR7_RDWR = 0x03,  /* break on read or write */
    };
    #endregion

    //========================================================================================================================================================

    public interface IPS4DBG
    {
        bool IsConnected { get; }
        bool IsDebugging { get; }
        string Version { get; }
        bool Connect(int connectTimeout = 10000, int sendTimeout = 10000, int receiveTimeout = 10000, bool getVersion = false);
        bool Disconnect();
        string GetConsoleDebugVersion();
        string GetLibraryDebugVersion();

        #region Proc.cs
        ulong AllocateMemory(int pid, int length);
        ulong Call(int pid, ulong rpcstub, ulong address, params object[] args);
        void ChangeProtection(int pid, ulong address, uint length, VM_PROTECTIONS newProt);
        void FreeMemory(int pid, ulong address, int length);
        ProcessInfo GetProcessInfo(int pid);
        ProcessList GetProcessList();
        ProcessMap GetProcessMaps(int pid);
        ulong InstallRPC(int pid);
        byte[] ReadMemory(int pid, ulong address, int length);
        T ReadMemory<T>(int pid, ulong address);
        byte ReadByte(int pid, ulong address);
        char ReadChar(int pid, ulong address);
        double ReadDouble(int pid, ulong address);
        short ReadInt16(int pid, ulong address);
        int ReadInt32(int pid, ulong address);
        long ReadInt64(int pid, ulong address);
        float ReadSingle(int pid, ulong address);
        string ReadString(int pid, ulong address);
        string ReadString(int pid, ulong address, int lenght);
        ushort ReadUInt16(int pid, ulong address);
        uint ReadUInt32(int pid, ulong address);
        ulong ReadUInt64(int pid, ulong address);
        void WriteMemory(int pid, ulong address, byte[] data);
        void WriteMemory<T>(int pid, ulong address, T value);
        void WriteByte(int pid, ulong address, byte value);
        void WriteChar(int pid, ulong address, char value);
        void WriteDouble(int pid, ulong address, double value);
        void WriteInt16(int pid, ulong address, short value);
        void WriteInt32(int pid, ulong address, int value);
        void WriteInt64(int pid, ulong address, long value);
        void WriteSByte(int pid, ulong address, sbyte value);
        void WriteSingle(int pid, ulong address, float value);
        void WriteString(int pid, ulong address, string str);
        void WriteUInt16(int pid, ulong address, ushort value);
        void WriteUInt32(int pid, ulong address, uint value);
        void WriteUInt64(int pid, ulong address, ulong value);
        #endregion

        #region Console.cs
        void Notify(int messageType, string message);
        void Print(string str);
        void Reboot();
        #endregion

        #region Debug.cs
        int AttachPID { get; }
        void AttachDebugger(int pid, DebuggerInterruptCallback callback);
        void ChangeBreakpoint(int index, bool enabled, ulong address);
        void ChangeWatchpoint(int index, bool enabled, WATCHPT_LENGTH length, WATCHPT_BREAKTYPE breaktype, ulong address);
        void DetachDebugger();
        dbregs GetDebugRegisters(uint lwpid);
        fpregs GetFloatRegisters(uint lwpid);
        regs GetRegisters(uint lwpid);
        ThreadInfo GetThreadInfo(uint lwpid);
        uint[] GetThreadList();
        void ProcessKill();
        void ProcessResume();
        void ProcessStop();
        void ProcessExtKill(int pid);
        void ProcessExtResume(int pid);
        void ProcessExtStop(int pid);
        void ResumeThread(uint lwpid);
        void SetDebugRegisters(uint lwpid, dbregs dbregs);
        void SetFloatRegisters(uint lwpid, fpregs fpregs);
        void SetRegisters(uint lwpid, regs regs);
        void SingleStep();
        void StopThread(uint lwpid);
        void TryDetachDebugger();
        #endregion

        #region Kernel
        ulong KernelBase();
        byte[] KernelReadMemory(ulong address, int length);
        void KernelWriteMemory(ulong address, byte[] data);
        #endregion

        //========================================================================================================================================================
        #region libdebug
        //PS4DBG.cs
        //int ExtFWVersion { get; }
        //string ExtPS4DBGVersion { get; }
        //string GetExtDBGVersion();
        //int GetExtFWVersion();
        //void LoadElf(int pid, byte[] elf);
        //void LoadElf(int pid, string filename);
        //List<ulong> ScanProcess<T>(int pid, PS4DBG.ScanCompareType compareType, T value, T extraValue = default);

        //PS4DBG.Console.cs
        //void GetConsoleInformation();

        //PS4DBG.Proc.cs
        //(PS4DBG.ScanValueType valueType, int typeLength, byte[] valueBuffer, byte[] extraValueBuffer) InitValueData<T>(T value, object extraValue = null);

        #endregion
        //========================================================================================================================================================
        #region libframe4
        //FRAME4.cs
        //void UnloadPayload();

        //FRAME4.Proc.cs
        //ulong[] GetScanResults(int pid);
        //ulong GetScanResultsCount(int pid);
        //ulong LoadElf(int pid, byte[] elf);
        //ulong LoadElf(int pid, string filename);
        //int LoadPRX(string procName, string prxPath);
        //void UnloadPRX(string procName, int prxHandle);
        //void ScanProcess<T>(int pid, int firstScan, byte[] selectedSections, FRAME4.ScanCompareType compareType, T value, T? extraValue = null) where T : struct;

        //FRAME4.Console.cs
        //ConsoleInfo GetConsoleInformation();

        //FRAME4.Console.cs
        //string GetPSID()
        //void SetFanThresholdCelsius(byte temperature)
        //void SetFanThresholdFahrenheit(byte temperature)
        #endregion
    }
}