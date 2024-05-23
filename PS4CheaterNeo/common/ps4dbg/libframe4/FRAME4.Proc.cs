using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace PS4CheaterNeo.libframe4
{
    /// <summary>
    /// Copyright by DeathRGH/libframe4-cs
	/// https://github.com/DeathRGH/libframe4-cs
    /// </summary>
	public partial class FRAME4
	{
		// proc
		// packet sizes

		// send size
		private const int CMD_PROC_READ_PACKET_SIZE    = 16;
		private const int CMD_PROC_WRITE_PACKET_SIZE   = 16;
		private const int CMD_PROC_MAPS_PACKET_SIZE    = 4;
		private const int CMD_PROC_INSTALL_PACKET_SIZE = 4;
		private const int CMD_PROC_CALL_PACKET_SIZE    = 68;
		private const int CMD_PROC_ELF_PACKET_SIZE     = 8;
		private const int CMD_PROC_PROTECT_PACKET_SIZE = 20;
		private const int CMD_PROC_SCAN_PACKET_SIZE    = 14;
		private const int CMD_PROC_INFO_PACKET_SIZE    = 4;
		private const int CMD_PROC_ALLOC_PACKET_SIZE   = 8;
		private const int CMD_PROC_FREE_PACKET_SIZE    = 16;

		private const int CMD_SCAN_START_PACKET_SIZE         = 24;
		private const int CMD_SCAN_NEXTSCAN_PACKET_SIZE      = 8;
		private const int CMD_SCAN_GET_RESULTS_PACKET_SIZE   = 4;
		private const int CMD_SCAN_COUNT_RESULTS_PACKET_SIZE = 4;
		private const int CMD_SCAN_END_PACKET_SIZE           = 4;

		private const int CMD_PROC_PRX_UNLOAD_PACKET_SIZE = 4;

		// receive size
		private const int PROC_LIST_ENTRY_SIZE = 36;
		private const int PROC_MAP_ENTRY_SIZE  = 58;
		private const int PROC_INSTALL_SIZE    = 8;
		private const int PROC_CALL_SIZE       = 12;
		private const int PROC_ELF_SIZE        = 8;
		private const int PROC_PROC_INFO_SIZE  = 188;
		private const int PROC_ALLOC_SIZE      = 8;

		private const int CMD_SCAN_COUNT_RESULTS_RESPONSE_SIZE = 8;

		private const int CMD_PROC_PRX_LOAD_RESPONSE_SIZE = 4;

		/// <summary>
		/// Get current process list
		/// </summary>
		/// <returns></returns>
		public ProcessList GetProcessList()
		{
			CheckConnected();

			SendCMDPacket(CMDS.CMD_PROC_LIST, 0);
			CheckStatus();

			// recv count
			byte[] bytes = new byte[4];
			sock.Receive(bytes, 4, SocketFlags.None);
			int number = BitConverter.ToInt32(bytes, 0);

			// recv data
			byte[] data = ReceiveData(number * PROC_LIST_ENTRY_SIZE);

			// parse data
			string[] names = new string[number];
			int[] pids = new int[number];
			for (int i = 0; i < number; i++)
			{
				int offset = i * PROC_LIST_ENTRY_SIZE;
				names[i] = ConvertASCII(data, offset);
				pids[i] = BitConverter.ToInt32(data, offset + 32);
			}

			return new ProcessList(number, names, pids);
		}

		/// <summary>
		/// Read memory
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		/// <param name="length">Data length</param>
		/// <returns></returns>
		public byte[] ReadMemory(int pid, ulong address, int length)
		{
			CheckConnected();

			SendCMDPacket(CMDS.CMD_PROC_READ, CMD_PROC_READ_PACKET_SIZE, pid, address, length);
			CheckStatus();
			return ReceiveData(length);
		}

		/// <summary>
		/// Write memory
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		/// <param name="data">Data</param>
		public void WriteMemory(int pid, ulong address, byte[] data)
		{
			CheckConnected();

			SendCMDPacket(CMDS.CMD_PROC_WRITE, CMD_PROC_WRITE_PACKET_SIZE, pid, address, data.Length);
			CheckStatus();
			SendData(data, data.Length);
			CheckStatus();
		}

		// read wrappers
		/// <summary>
		/// Read Byte
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		public Byte ReadByte(int pid, ulong address)
		{
			return ReadMemory(pid, address, sizeof(Byte))[0];
		}

		/// <summary>
		/// Read Char
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		public Char ReadChar(int pid, ulong address)
		{
			return BitConverter.ToChar(ReadMemory(pid, address, sizeof(Char)), 0);
		}

		/// <summary>
		/// Read Int16
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		public Int16 ReadInt16(int pid, ulong address)
		{
			return BitConverter.ToInt16(ReadMemory(pid, address, sizeof(Int16)), 0);
		}

		/// <summary>
		/// Read Unsigned Int16
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		public UInt16 ReadUInt16(int pid, ulong address)
		{
			return BitConverter.ToUInt16(ReadMemory(pid, address, sizeof(UInt16)), 0);
		}

		/// <summary>
		/// Read Int32
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		public Int32 ReadInt32(int pid, ulong address)
		{
			return BitConverter.ToInt32(ReadMemory(pid, address, sizeof(Int32)), 0);
		}

		/// <summary>
		/// Read Unsigned Int32
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		public UInt32 ReadUInt32(int pid, ulong address)
		{
			return BitConverter.ToUInt32(ReadMemory(pid, address, sizeof(UInt32)), 0);
		}

		/// <summary>
		/// Read Int64
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		public Int64 ReadInt64(int pid, ulong address)
		{
			return BitConverter.ToInt64(ReadMemory(pid, address, sizeof(Int64)), 0);
		}

		/// <summary>
		/// Read Unsigned Int64
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		public UInt64 ReadUInt64(int pid, ulong address)
		{
			return BitConverter.ToUInt64(ReadMemory(pid, address, sizeof(UInt64)), 0);
		}

		// write wrappers
		/// <summary>
		/// Write SByte
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		/// <param name="value">SByte to write</param>
		public void WriteSByte(int pid, ulong address, SByte value)
		{
			WriteMemory(pid, address, new byte[] { (byte)value });
		}

		// write wrappers
		/// <summary>
		/// Write Byte
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		/// <param name="value">Byte to write</param>
		public void WriteByte(int pid, ulong address, Byte value)
		{
			WriteMemory(pid, address, new byte[] { value });
		}

		/// <summary>
		/// Write Char
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		/// <param name="value">Char to write</param>
		public void WriteChar(int pid, ulong address, Char value)
		{
			WriteMemory(pid, address, BitConverter.GetBytes(value));
		}

		/// <summary>
		/// Write Int16
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		/// <param name="value">Int16 to write</param>
		public void WriteInt16(int pid, ulong address, Int16 value)
		{
			WriteMemory(pid, address, BitConverter.GetBytes(value));
		}

		/// <summary>
		/// Write Unsigned Int16
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		/// <param name="value">UInt16 to write</param>
		public void WriteUInt16(int pid, ulong address, UInt16 value)
		{
			WriteMemory(pid, address, BitConverter.GetBytes(value));
		}

		/// <summary>
		/// Write Int32
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		/// <param name="value">Int32 to write</param>
		public void WriteInt32(int pid, ulong address, Int32 value)
		{
			WriteMemory(pid, address, BitConverter.GetBytes(value));
		}

		/// <summary>
		/// Write Unsigned Int32
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		/// <param name="value">UInt32 to write</param>
		public void WriteUInt32(int pid, ulong address, UInt32 value)
		{
			WriteMemory(pid, address, BitConverter.GetBytes(value));
		}

		/// <summary>
		/// Write Int64
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		/// <param name="value">Int64 to write</param>
		public void WriteInt64(int pid, ulong address, Int64 value)
		{
			WriteMemory(pid, address, BitConverter.GetBytes(value));
		}

		/// <summary>
		/// Write Unsigned Int64
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		/// <param name="value">UInt64 to write</param>
		public void WriteUInt64(int pid, ulong address, UInt64 value)
		{
			WriteMemory(pid, address, BitConverter.GetBytes(value));
		}

		// float/double
		/// <summary>
		/// Read Single
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		public float ReadSingle(int pid, ulong address)
		{
			return BitConverter.ToSingle(ReadMemory(pid, address, sizeof(float)), 0);
		}

		/// <summary>
		/// Write Single
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		/// <param name="value">float to write</param>
		public void WriteSingle(int pid, ulong address, float value)
		{
			WriteMemory(pid, address, BitConverter.GetBytes(value));
		}

		/// <summary>
		/// Read Double
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		public double ReadDouble(int pid, ulong address)
		{
			return BitConverter.ToDouble(ReadMemory(pid, address, sizeof(double)), 0);
		}

		/// <summary>
		/// Write Double
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		/// <param name="value">double to write</param>
		public void WriteDouble(int pid, ulong address, double value)
		{
			WriteMemory(pid, address, BitConverter.GetBytes(value));
		}

		// string
		/// <summary>
		/// Read null terminated string
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		public string ReadString(int pid, ulong address)
		{
			string str = "";
			ulong i = 0;

			while (true)
			{
				byte value = ReadByte(pid, address + i);
				if (value == 0)
				{
					break;
				}

				str += Convert.ToChar(value);
				i++;
			}

			return str;
		}

		/// <summary>
		/// Read string as buffer
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		/// <param name="lenght">Buffer length</param>
		public string ReadString(int pid, ulong address, int lenght)
		{
			byte[] temp = ReadMemory(pid, address, lenght);
			string str = "";
			int i = 0;

			while (true)
			{
				byte value = temp[i];

				if (value == 0)
				{
					break;
				}

				str += Convert.ToChar(value);
				i++;
			}

			return str;
		}

		/// <summary>
		/// Write String
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Memory address</param>
		/// <param name="str">String to write</param>
		public void WriteString(int pid, ulong address, string str)
		{
			WriteMemory(pid, address, Encoding.ASCII.GetBytes(str));
			WriteByte(pid, address + (ulong)str.Length, 0);
		}

		/// <summary>
		/// Get process memory maps
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <returns></returns>
		public ProcessMap GetProcessMaps(int pid)
		{
			CheckConnected();

			SendCMDPacket(CMDS.CMD_PROC_MAPS, CMD_PROC_MAPS_PACKET_SIZE, pid);
			CheckStatus();

			// recv count
			byte[] bnumber = new byte[4];
			sock.Receive(bnumber, 4, SocketFlags.None);
			int number = BitConverter.ToInt32(bnumber, 0);

			// recv data
			byte[] data = ReceiveData(number * PROC_MAP_ENTRY_SIZE);

			// parse data
			MemoryEntry[] entries = new MemoryEntry[number];
			for (int i = 0; i < number; i++)
			{
				int offset = i * PROC_MAP_ENTRY_SIZE;
				entries[i] = new MemoryEntry
				{
					name = ConvertASCII(data, offset),
					start = BitConverter.ToUInt64(data, offset + 32),
					end = BitConverter.ToUInt64(data, offset + 40),
					offset = BitConverter.ToUInt64(data, offset + 48),
					prot = BitConverter.ToUInt16(data, offset + 56)
				};

			}

			return new ProcessMap(pid, entries);
		}

		/// <summary>
		/// Install RPC into a process, this returns a stub address that you should pass into call functions
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <returns></returns>
		public ulong InstallRPC(int pid)
		{
			CheckConnected();

			SendCMDPacket(CMDS.CMD_PROC_INSTALL, CMD_PROC_INSTALL_PACKET_SIZE, pid);
			CheckStatus();

			return BitConverter.ToUInt64(ReceiveData(PROC_INSTALL_SIZE), 0);
		}

		/// <summary>
		/// Call function (returns rax)
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="rpcstub">Stub address from InstallRPC</param>
		/// <param name="address">Address to call</param>
		/// <param name="args">Arguments array</param>
		/// <returns></returns>
		public ulong Call(int pid, ulong rpcstub, ulong address, params object[] args)
		{
			CheckConnected();

			// need to do this in a custom format
			CMDPacket packet = new CMDPacket
			{
				magic = CMD_PACKET_MAGIC,
				cmd = (uint)CMDS.CMD_PROC_CALL,
				datalen = (uint)CMD_PROC_CALL_PACKET_SIZE
			};
			SendData(GetBytesFromObject(packet), CMD_PACKET_SIZE);

			MemoryStream rs = new MemoryStream();
			rs.Write(BitConverter.GetBytes(pid), 0, sizeof(int));
			rs.Write(BitConverter.GetBytes(rpcstub), 0, sizeof(ulong));
			rs.Write(BitConverter.GetBytes(address), 0, sizeof(ulong));

			int num = 0;
			foreach (object arg in args)
			{
				byte[] bytes = new byte[8];

				switch (arg)
				{
					case char c:
						{
							byte[] tmp = BitConverter.GetBytes(c);
							Buffer.BlockCopy(tmp, 0, bytes, 0, sizeof(char));

							byte[] pad = new byte[sizeof(ulong) - sizeof(char)];
							Buffer.BlockCopy(pad, 0, bytes, sizeof(char), pad.Length);
							break;
						}
					case byte b:
						{
							byte[] tmp = BitConverter.GetBytes(b);
							Buffer.BlockCopy(tmp, 0, bytes, 0, sizeof(byte));

							byte[] pad = new byte[sizeof(ulong) - sizeof(byte)];
							Buffer.BlockCopy(pad, 0, bytes, sizeof(byte), pad.Length);
							break;
						}
					case short s:
						{
							byte[] tmp = BitConverter.GetBytes(s);
							Buffer.BlockCopy(tmp, 0, bytes, 0, sizeof(short));

							byte[] pad = new byte[sizeof(ulong) - sizeof(short)];
							Buffer.BlockCopy(pad, 0, bytes, sizeof(short), pad.Length);
							break;
						}
					case ushort us:
						{
							byte[] tmp = BitConverter.GetBytes(us);
							Buffer.BlockCopy(tmp, 0, bytes, 0, sizeof(ushort));

							byte[] pad = new byte[sizeof(ulong) - sizeof(ushort)];
							Buffer.BlockCopy(pad, 0, bytes, sizeof(ushort), pad.Length);
							break;
						}
					case int i:
						{
							byte[] tmp = BitConverter.GetBytes(i);
							Buffer.BlockCopy(tmp, 0, bytes, 0, sizeof(int));

							byte[] pad = new byte[sizeof(ulong) - sizeof(int)];
							Buffer.BlockCopy(pad, 0, bytes, sizeof(int), pad.Length);
							break;
						}
					case uint ui:
						{
							byte[] tmp = BitConverter.GetBytes(ui);
							Buffer.BlockCopy(tmp, 0, bytes, 0, sizeof(uint));

							byte[] pad = new byte[sizeof(ulong) - sizeof(uint)];
							Buffer.BlockCopy(pad, 0, bytes, sizeof(uint), pad.Length);
							break;
						}
					case long l:
						{
							byte[] tmp = BitConverter.GetBytes(l);
							Buffer.BlockCopy(tmp, 0, bytes, 0, sizeof(long));
							break;
						}
					case ulong ul:
						{
							byte[] tmp = BitConverter.GetBytes(ul);
							Buffer.BlockCopy(tmp, 0, bytes, 0, sizeof(ulong));
							break;
						}
					case float f:
						{
							byte[] tmp = BitConverter.GetBytes(f);
							Buffer.BlockCopy(tmp, 0, bytes, 0, sizeof(float));

							byte[] pad = new byte[sizeof(ulong) - sizeof(float)];
							Buffer.BlockCopy(pad, 0, bytes, sizeof(int), pad.Length);
							break;
						}
					case double d:
						{
							byte[] tmp = BitConverter.GetBytes(d);
							Buffer.BlockCopy(tmp, 0, bytes, 0, sizeof(double));
							break;
						}
				}

				rs.Write(bytes, 0, bytes.Length);
				num++;
			}

			if (num > 6)
			{
				throw new Exception("libframe4: too many arguments");
			}

			if (num < 6)
			{
				for (int i = 0; i < (6 - num); i++)
				{
					rs.Write(BitConverter.GetBytes((ulong)0), 0, sizeof(ulong));
				}
			}

			SendData(rs.ToArray(), CMD_PROC_CALL_PACKET_SIZE);
			rs.Dispose();

			CheckStatus();

			byte[] data = ReceiveData(PROC_CALL_SIZE);
			return BitConverter.ToUInt64(data, 4);
		}

		/// <summary>
		/// Load elf
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="elf">Elf</param>
		public ulong LoadElf(int pid, byte[] elf)
		{
			CheckConnected();

			SendCMDPacket(CMDS.CMD_PROC_ELF, CMD_PROC_ELF_PACKET_SIZE, pid, (uint)elf.Length);
			CheckStatus();
			SendData(elf, elf.Length);
			CheckStatus();

			return BitConverter.ToUInt64(ReceiveData(PROC_ELF_SIZE), 0);
		}

		/// <summary>
		/// Load elf
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="filename">Elf filename</param>
		public ulong LoadElf(int pid, string filename)
		{
			return LoadElf(pid, File.ReadAllBytes(filename));
		}

		public enum ScanValueType : byte
		{
			valTypeUInt8 = 0,
			valTypeInt8,
			valTypeUInt16,
			valTypeInt16,
			valTypeUInt32,
			valTypeInt32,
			valTypeUInt64,
			valTypeInt64,
			valTypeFloat,
			valTypeDouble,
			valTypeArrBytes,
			valTypeString
		}

		public enum ScanCompareType : byte
		{
			ExactValue = 0,
			FuzzyValue,
			BiggerThan,
			SmallerThan,
			ValueBetween,
			IncreasedValue,
			IncreasedValueBy,
			DecreasedValue,
			DecreasedValueBy,
			ChangedValue,
			UnchangedValue,
			UnknownInitialValue
		}

		//T extraValue = default)
		public void ScanProcess<T>(int pid, int firstScan, byte[] selectedSections, ScanCompareType compareType, T value, T? extraValue = null) where T : struct
		{
			CheckConnected();

			int typeLength = 0;
			ScanValueType valueType;
			byte[] valueBuffer, extraValueBuffer = null;

			// fill in variables
			switch (value)
			{
				case bool b:
					valueType = ScanValueType.valTypeUInt8;
					typeLength = 1;
					valueBuffer = BitConverter.GetBytes(b);
					if (extraValue != null)
						extraValueBuffer = BitConverter.GetBytes((bool)(object)extraValue);
					break;
				case sbyte sb:
					valueType = ScanValueType.valTypeInt8;
					valueBuffer = BitConverter.GetBytes(sb);
					typeLength = 1;
					if (extraValue != null)
						extraValueBuffer = BitConverter.GetBytes((sbyte)(object)extraValue);
					break;
				case byte b:
					valueType = ScanValueType.valTypeUInt8;
					valueBuffer = BitConverter.GetBytes(b);
					typeLength = 1;
					if (extraValue != null)
						extraValueBuffer = BitConverter.GetBytes((byte)(object)extraValue);
					break;
				case short s:
					valueType = ScanValueType.valTypeInt16;
					valueBuffer = BitConverter.GetBytes(s);
					typeLength = 2;
					if (extraValue != null)
						extraValueBuffer = BitConverter.GetBytes((short)(object)extraValue);
					break;
				case ushort us:
					valueType = ScanValueType.valTypeUInt16;
					valueBuffer = BitConverter.GetBytes(us);
					typeLength = 2;
					if (extraValue != null)
						extraValueBuffer = BitConverter.GetBytes((ushort)(object)extraValue);
					break;
				case int i:
					valueType = ScanValueType.valTypeInt32;
					valueBuffer = BitConverter.GetBytes(i);
					typeLength = 4;
					if (extraValue != null)
						extraValueBuffer = BitConverter.GetBytes((int)(object)extraValue);
					break;
				case uint ui:
					valueType = ScanValueType.valTypeUInt32;
					valueBuffer = BitConverter.GetBytes(ui);
					typeLength = 4;
					if (extraValue != null)
						extraValueBuffer = BitConverter.GetBytes((uint)(object)extraValue);
					break;
				case long l:
					valueType = ScanValueType.valTypeInt64;
					valueBuffer = BitConverter.GetBytes(l);
					typeLength = 8;
					if (extraValue != null)
						extraValueBuffer = BitConverter.GetBytes((long)(object)extraValue);
					break;
				case ulong ul:
					valueType = ScanValueType.valTypeUInt64;
					valueBuffer = BitConverter.GetBytes(ul);
					typeLength = 8;
					if (extraValue != null)
						extraValueBuffer = BitConverter.GetBytes((ulong)(object)extraValue);
					break;
				case float f:
					valueType = ScanValueType.valTypeFloat;
					valueBuffer = BitConverter.GetBytes(f);
					typeLength = 4;
					if (extraValue != null)
						extraValueBuffer = BitConverter.GetBytes((float)(object)extraValue);
					break;
				case double d:
					valueType = ScanValueType.valTypeDouble;
					valueBuffer = BitConverter.GetBytes(d);
					typeLength = 8;
					if (extraValue != null)
						extraValueBuffer = BitConverter.GetBytes((double)(object)extraValue);
					break;
				/*case string s:
                    valueType = ScanValueType.valTypeString;
                    valueBuffer = Encoding.ASCII.GetBytes(s);
                    typeLength = valueBuffer.Length;
                    break;*/
				case byte[] ba:
					valueType = ScanValueType.valTypeArrBytes;
					valueBuffer = ba;
					typeLength = valueBuffer.Length;
					break;
				default:
					throw new NotSupportedException("Requested scan value type is not supported! (Use byte[] instead)");

			}
			// send packet
			SendCMDPacket(CMDS.CMD_PROC_SCAN, CMD_PROC_SCAN_PACKET_SIZE, pid, firstScan, (byte)valueType, (byte)compareType, (uint)(extraValue == null ? typeLength : typeLength * 2));

			int saved = sock.ReceiveTimeout;
			sock.ReceiveTimeout = int.MaxValue;

			CheckStatus();

			SendData(valueBuffer, typeLength);
			if (extraValueBuffer != null)
			{
				SendData(extraValueBuffer, typeLength);
			}

			CheckStatus();

			if (firstScan == 1)
			{
				SendData(selectedSections, selectedSections.Length);

				CheckStatus();
			}

			sock.ReceiveTimeout = saved;
		}

		/// <summary>
		/// Changes protection on pages in range
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Address</param>
		/// <param name="length">Length</param>
		/// <param name="newprot">New protection</param>
		/// <returns></returns>
		public void ChangeProtection(int pid, ulong address, uint length, VM_PROTECTIONS newProt)
		{
			CheckConnected();

			SendCMDPacket(CMDS.CMD_PROC_PROTECT, CMD_PROC_PROTECT_PACKET_SIZE, pid, address, length, (uint)newProt);
			CheckStatus();
			try
			{
				CheckStatus();
			}
			catch { }
		}

		/// <summary>
		/// Get process information
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <returns></returns>
		public ProcessInfo GetProcessInfo(int pid)
		{
			CheckConnected();

			SendCMDPacket(CMDS.CMD_PROC_INFO, CMD_PROC_INFO_PACKET_SIZE, pid);
			CheckStatus();

			byte[] data = ReceiveData(PROC_PROC_INFO_SIZE);
			return (ProcessInfo)GetObjectFromBytes(data, typeof(ProcessInfo));
		}

		/// <summary>
		/// Allocate RWX memory in the process space
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="length">Size of memory allocation</param>
		/// <returns></returns>
		public ulong AllocateMemory(int pid, int length)
		{
			CheckConnected();

			SendCMDPacket(CMDS.CMD_PROC_ALLOC, CMD_PROC_ALLOC_PACKET_SIZE, pid, length);
			CheckStatus();
			return BitConverter.ToUInt64(ReceiveData(PROC_ALLOC_SIZE), 0);
		}

		/// <summary>
		/// Free memory in the process space
		/// </summary>
		/// <param name="pid">Process ID</param>
		/// <param name="address">Address of the memory allocation</param>
		/// <param name="length">Size of memory allocation</param>
		/// <returns></returns>
		public void FreeMemory(int pid, ulong address, int length)
		{
			CheckConnected();

			SendCMDPacket(CMDS.CMD_PROC_FREE, CMD_PROC_FREE_PACKET_SIZE, pid, address, length);
			CheckStatus();
		}

		public ulong[] GetScanResults(int pid)
		{
			CheckConnected();

			ulong resultCount = GetScanResultsCount(pid);
			if (resultCount <= 0)
				return new ulong[0];
			else if (resultCount > 1000000)
				throw new Exception("libframe4: too many scan results");

			SendCMDPacket(CMDS.CMD_PROC_SCAN_GET_RESULTS, CMD_SCAN_GET_RESULTS_PACKET_SIZE, pid);

			int saved = sock.ReceiveTimeout;
			sock.ReceiveTimeout = int.MaxValue;

			CheckStatus();

			byte[] raw = ReceiveData((int)resultCount * 8);

			sock.ReceiveTimeout = saved;

			List<ulong> resultList = new List<ulong>();
			for (int i = 0; i < raw.Length; i += 8)
				resultList.Add(BitConverter.ToUInt64(raw, i));

			return resultList.ToArray();
		}

		public ulong GetScanResultsCount(int pid)
		{
			CheckConnected();

			SendCMDPacket(CMDS.CMD_PROC_SCAN_COUNT_RESULTS, CMD_SCAN_COUNT_RESULTS_PACKET_SIZE, pid);
			CheckStatus();

			byte[] data = ReceiveData(CMD_SCAN_COUNT_RESULTS_RESPONSE_SIZE);
			return BitConverter.ToUInt64(data, 0);
		}

		public int LoadPRX(string procName, string prxPath)
		{
			CheckConnected();

			// set timeout as it was causing issues on certain processes
			// not a good solution but will have to do until we come up with our own loader
			int saved = sock.ReceiveTimeout;
			sock.ReceiveTimeout = int.MaxValue;

			byte[] procNameArray = new byte[32];
			char[] procNameChars = procName.ToCharArray();
			for (int i = 0; i < procNameChars.Length; i++)
			{
				procNameArray[i] = Convert.ToByte(procNameChars[i]);
			}

			byte[] prxPathArray = new byte[100];
			char[] prxPathChars = prxPath.ToCharArray();
			for (int i = 0; i < prxPathChars.Length; i++)
			{
				prxPathArray[i] = Convert.ToByte(prxPathChars[i]);
			}

			SendCMDPacket(CMDS.CMD_PROC_PRX_LOAD, 0);
			SendData(procNameArray, procNameArray.Length);
			SendData(prxPathArray, prxPathArray.Length);
			CheckStatus();

			byte[] data = ReceiveData(CMD_PROC_PRX_LOAD_RESPONSE_SIZE);

			// reset timeout
			sock.ReceiveTimeout = saved;

			return BitConverter.ToInt32(data, 0);
		}

		public void UnloadPRX(string procName, int prxHandle)
		{
			CheckConnected();

			// set timeout as it was causing issues on certain processes
			// not a good solution but will have to do until we come up with our own loader
			int saved = sock.ReceiveTimeout;
			sock.ReceiveTimeout = int.MaxValue;

			byte[] procNameArray = new byte[32];
			char[] procNameChars = procName.ToCharArray();
			for (int i = 0; i < procNameChars.Length; i++)
			{
				procNameArray[i] = Convert.ToByte(procNameChars[i]);
			}

			SendCMDPacket(CMDS.CMD_PROC_PRX_UNLOAD, CMD_PROC_PRX_UNLOAD_PACKET_SIZE, prxHandle);
			SendData(procNameArray, procNameArray.Length);
			CheckStatus();

			// reset timeout
			sock.ReceiveTimeout = saved;
		}

		public T ReadMemory<T>(int pid, ulong address)
		{
			if (typeof(T) == typeof(string))
			{
				string str = "";
				ulong i = 0;

				while (true)
				{
					byte value = ReadMemory(pid, address + i, sizeof(byte))[0];
					if (value == 0)
					{
						break;
					}
					str += Convert.ToChar(value);
					i++;
				}

				return (T)(object)str;
			}

			if (typeof(T) == typeof(byte[]))
			{
				throw new NotSupportedException("byte arrays are not supported, use ReadMemory(int pid, ulong address, int size)");
			}

			return (T)GetObjectFromBytes(ReadMemory(pid, address, Marshal.SizeOf(typeof(T))), typeof(T));
		}

		public void WriteMemory<T>(int pid, ulong address, T value)
		{
			if (typeof(T) == typeof(string))
			{
				WriteMemory(pid, address, Encoding.ASCII.GetBytes((string)(object)value + (char)0x0));
				return;
			}

			if (typeof(T) == typeof(byte[]))
			{
				WriteMemory(pid, address, (byte[])(object)value);
				return;
			}

			WriteMemory(pid, address, GetBytesFromObject(value));
		}
	}
}
