using System;
using System.Net.Sockets;

namespace PS4CheaterNeo.libframe4
{
    /// <summary>
    /// Copyright by DeathRGH/libframe4-cs
    /// https://github.com/DeathRGH/libframe4-cs
    /// </summary>
    public partial class FRAME4
    {
        // kernel
        // packet sizes

        // send size
        private const int CMD_KERN_READ_PACKET_SIZE = 12;
        private const int CMD_KERN_WRITE_PACKET_SIZE = 12;
        private const int CMD_KERN_RDMSR_SIZE = 4;
        private const int CMD_KERN_PHYS_READ_PACKET_SIZE = 12;

        // receive size
        private const int KERN_BASE_SIZE = 8;
        private const int KERN_MAP_ENTRY_SIZE = 58;
        private const int KERN_RDMSR_SIZE = 8;

        /// <summary>
        /// Get kernel base address
        /// </summary>
        /// <returns></returns>
        public ulong KernelBase()
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_KERN_BASE, 0);
            CheckStatus();
            return BitConverter.ToUInt64(ReceiveData(KERN_BASE_SIZE), 0);
        }

        /// <summary>
        /// Read memory from kernel
        /// </summary>
        /// <param name="address">Memory address</param>
        /// <param name="length">Data length</param>
        /// <returns></returns>
        public byte[] KernelReadMemory(ulong address, int length)
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_KERN_READ, CMD_KERN_READ_PACKET_SIZE, address, length);
            CheckStatus();
            return ReceiveData(length);
        }

        /// <summary>
        /// Write memory in kernel
        /// </summary>
        /// <param name="address">Memory address</param>
        /// <param name="data">Data</param>
        public void KernelWriteMemory(ulong address, byte[] data)
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_KERN_WRITE, CMD_KERN_WRITE_PACKET_SIZE, address, data.Length);
            CheckStatus();
            SendData(data, data.Length);
            CheckStatus();
        }

        /// <summary>
        /// Get the kernel virtual memory map
        /// </summary>
        /// <returns>The original GetKernelVmMap returned a dedicated KernelVmMap class.
        /// Since it is nearly identical (90% similar) to ProcessMap—with the only major difference being an extra pid member in the latter—
        /// I have updated GetKernelVmMap to return ProcessMap instead, setting the pid to 0.</returns>
        public ProcessMap GetKernelVmMap()
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_KERN_VM_MAP, 0);
            CheckStatus();

            // recv count
            byte[] bnumber = new byte[4];
            sock.Receive(bnumber, 4, SocketFlags.None);
            int number = BitConverter.ToInt32(bnumber, 0);

            // recv data
            byte[] data = ReceiveData(number * KERN_MAP_ENTRY_SIZE);

            // parse data
            MemoryEntry[] entries = new MemoryEntry[number];
            for (int i = 0; i < number; i++)
            {
                int offset = i * KERN_MAP_ENTRY_SIZE;
                entries[i] = new MemoryEntry
                {
                    name = ConvertASCII(data, offset),
                    start = BitConverter.ToUInt64(data, offset + 32),
                    end = BitConverter.ToUInt64(data, offset + 40),
                    offset = BitConverter.ToUInt64(data, offset + 48),
                    prot = BitConverter.ToUInt16(data, offset + 56)
                };
            }

            return new ProcessMap(0, entries);
        }

        /// <summary>
        /// Read the Machine Specific Register
        /// </summary>
        /// <param name="reg">Register</param>
        /// <returns></returns>
        public ulong ReadMSR(uint reg)
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_KERN_RDMSR, CMD_KERN_RDMSR_SIZE, reg);
            CheckStatus();

            return BitConverter.ToUInt64(ReceiveData(KERN_RDMSR_SIZE), 0);
        }

        /// <summary>
        /// Read physical memory
        /// </summary>
        /// <param name="address">Memory address</param>
        /// <param name="length">Data length</param>
        /// <returns></returns>
        public byte[] ReadPhysicalMemory(ulong address, int length)
        {
            CheckConnected();

            SendCMDPacket(CMDS.CMD_KERN_PHYS_READ, CMD_KERN_PHYS_READ_PACKET_SIZE, address, length);
            CheckStatus();

            return ReceiveData(length);
        }
    }
}
