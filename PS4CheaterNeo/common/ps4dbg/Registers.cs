using System.Runtime.InteropServices;

namespace PS4CheaterNeo
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct regs
    {
        public ulong  r_r15;     // General-purpose register r15, used for storing general data
        public ulong  r_r14;     // General-purpose register r14, used for storing general data
        public ulong  r_r13;     // General-purpose register r13, used for storing general data
        public ulong  r_r12;     // General-purpose register r12, used for storing general data
        public ulong  r_r11;     // General-purpose register r11, used for storing general data
        public ulong  r_r10;     // General-purpose register r10, used for storing general data
        public ulong  r_r9;      // General-purpose register r9, used for storing general data
        public ulong  r_r8;      // General-purpose register r8, used for storing general data
        public ulong  r_rdi;     // General-purpose register rdi, typically used for the first function parameter
        public ulong  r_rsi;     // General-purpose register rsi, typically used for the second function parameter
        public ulong  r_rbp;     // Base pointer register rbp, typically used for the stack pointer
        public ulong  r_rbx;     // Base register rbx, typically used for storing general data
        public ulong  r_rdx;     // General-purpose register rdx, used for storing general data
        public ulong  r_rcx;     // General-purpose register rcx, used for storing general data
        public ulong  r_rax;     // General-purpose register rax, used for storing general data
        public uint   r_trapno;  // Trap number, used to identify events or system calls causing a trap
        public ushort r_fs;      // General segment register fs, used to store segment address
        public ushort r_gs;      // General segment register gs, used to store segment address
        public uint   r_err;     // Error number, used to identify error events
        public ushort r_es;      // General segment register es, used to store segment address
        public ushort r_ds;      // General segment register ds, used to store segment address
        public ulong  r_rip;     // Instruction pointer register rip, stores the address of the next instruction to be executed
        public ulong  r_cs;      // Code segment register cs, stores the address of the code segment
        public ulong  r_rflags;  // Flags register rflags, stores the processor's status flags
        public ulong  r_rsp;     // Stack pointer register rsp, stores the address of the current stack
        public ulong  r_ss;      // Stack segment register ss, stores the address of the stack segment
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct envxmm
    {
        public ushort en_cw; /* control word (16bits) */
        public ushort en_sw; /* status word (16bits) */
        public byte   en_tw; /* tag word (8bits) */
        public byte   en_zero;
        public ushort en_opcode; /* opcode last executed (11 bits ) */
        public ulong  en_rip; /* floating point instruction pointer */
        public ulong  en_rdp; /* floating operand pointer */
        public uint   en_mxcsr; /* SSE sontorol/status register */
        public uint   en_mxcsr_mask; /* valid bits in mxcsr */
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct acc
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] fp_bytes;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        private byte[] fp_pad;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct xmmacc
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] xmm_bytes;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ymmacc
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] ymm_bytes;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct xstate_hdr
    {
        public ulong xstate_bv;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        private byte[] xstate_rsrv0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        private byte[] xstate_rsrv;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct savefpu_xstate
    {
        public xstate_hdr sx_hd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public ymmacc[] sx_ymm;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 64)]
    public struct fpregs
    {
        public envxmm svn_env;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public acc[] sv_fp;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public xmmacc[] sv_xmm;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 96)]
        private byte[] sv_pad;
        public savefpu_xstate sv_xstate;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct dbregs
    {
        public ulong dr0;
        public ulong dr1;
        public ulong dr2;
        public ulong dr3;
        public ulong dr4;
        public ulong dr5;
        public ulong dr6;
        public ulong dr7;
        public ulong dr8;
        public ulong dr9;
        public ulong dr10;
        public ulong dr11;
        public ulong dr12;
        public ulong dr13;
        public ulong dr14;
        public ulong dr15;
    }
}
