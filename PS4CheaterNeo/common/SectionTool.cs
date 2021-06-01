using libdebug;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

namespace PS4CheaterNeo
{
    public class SectionTool
    {
        private readonly Mutex mutex = new Mutex();
        public ulong totalMemorySize;
        public int PID { get; private set; }
        public ulong MemoryStart { get; private set; }
        public ulong MemoryEnd { get; private set; }
        public Dictionary<int, Section> SectionDict { get; private set; }
        public List<(int SID, ulong start, ulong end)> SectionList { get; private set; }
        public class Section
        {
            public int PID;
            public int SID;
            public ulong Start;
            public int Length;
            public string Name;
            public bool Check;
            public uint Prot;
            public ulong Offset;
            public bool IsFilter;

            public override string ToString() => $"{Start:X},{(float)Length / 1024} KB,{Name},{Prot:X},{Offset:X},{IsFilter},{Check},{PID},{SID}";
        }

        /// <summary>
        /// 重新排序MemoryEntry，無name的排前，再依序比較prot、name、start
        /// </summary>
        private int CompareMemoryEntry(MemoryEntry e1, MemoryEntry e2)
        {
            int result = 0;
            if (e1.name == "" || e2.name == "") result = e1.name.CompareTo(e2.name);
            if (result != 0) return result;

            result = e1.prot.CompareTo(e2.prot);
            if (result != 0) return result;

            result = e1.name.CompareTo(e2.name);
            if (result != 0) return result;

            result = e1.start.CompareTo(e2.start);
            return result;
        }

        public void InitSectionList(string processName)
        {
            ProcessInfo processInfo = PS4Tool.GetProcessInfo(processName);

            if (processInfo.pid == 0) throw new Exception(string.Format("ProcessInfo({0}) is null.", processName));

            InitSectionList(processInfo.pid);
        }

        public void InitSectionList(int processID)
        {
            ProcessMap pMap = PS4Tool.GetProcessMaps(processID);

            if (pMap == null || pMap.entries == null) throw new Exception(string.Format("ProcessMap({0}) is null.", processID));

            InitSectionList(pMap);
        }

        public void InitSectionList(ProcessMap pMap)
        {
            if (pMap == null || pMap.entries == null) throw new Exception("Process Map is null.");

            mutex.WaitOne();
            try
            {
                SectionDict = new Dictionary<int, Section>();
                SectionList = new List<(int SID, ulong start, ulong end)>();
                totalMemorySize = 0;
                PID = pMap.pid;
                int sIdx = 0;
                int protCnt = 0;
                uint protTmp = 0;
                ulong bufferLength = 1024 * 1024 * 128;
                Array.Sort(pMap.entries, CompareMemoryEntry); //重新排序MemoryEntry，無name的排前，再依序比較prot、name、start
                for (int i = 0; i < pMap.entries.Length; i++)
                {
                    MemoryEntry entry = pMap.entries[i];
                    if ((entry.prot & 0x1) == 0x1)
                    {
                        int idx = 0;
                        ulong start = entry.start;
                        ulong end = entry.end;
                        ulong length = end - start;
                        bool isFilter = SectionIsFilter(entry.name);

                        if ((entry.prot & 0x5) == 0x5) bufferLength = length; //Executable section
                        if (MemoryStart == 0 || start < MemoryStart) MemoryStart = start;
                        if (MemoryEnd == 0 || end > MemoryEnd) MemoryEnd = end;
                        if (protTmp > 0 && protTmp != entry.prot)
                        {
                            protCnt++;
                            sIdx = 0;
                        }
                        while (length != 0)
                        {
                            ulong curLength = bufferLength;

                            if (curLength > length)
                            {
                                curLength = length;
                                length = 0;
                            }
                            else length -= curLength;

                            Section section = new Section();
                            section.PID = pMap.pid;
                            section.SID = (!string.IsNullOrWhiteSpace(entry.name) ? 1 : 2) * 100000000 +  protCnt * 1000000 + sIdx * 100 + idx;
                            section.Start = start;
                            section.Length = (int)curLength;
                            section.Name = entry.name + "[" + idx + "]";
                            section.Check = false;
                            section.Prot = entry.prot;
                            section.Offset = entry.offset;
                            if (isFilter) section.IsFilter = true;

                            SectionDict.Add(section.SID, section);
                            SectionList.Add((section.SID, start, start + curLength));

                            start += curLength;
                            idx++;
                            Console.WriteLine(section.ToString());
                        }
                        sIdx++;
                        protTmp = entry.prot;
                    }
                }
                SectionList.Sort((s1, s2) => s1.start.CompareTo(s2.start));
                mutex.ReleaseMutex();
            }
            catch (Exception)
            {
                mutex.ReleaseMutex();
                throw;
            }
        }

        public bool SectionIsFilter(string name)
        {
            bool result = false;
            string sectionFilterKeys = (string)Properties.Settings.Default["SectionFilterKeys"];
            sectionFilterKeys = Regex.Replace(sectionFilterKeys, " *[,;] *", "|");

            if (Regex.IsMatch(name, sectionFilterKeys)) result = true;

            return result;
        }

        public Section GetSection(string name, uint prot)
        {
            Section section = null;

            List<int> keys = new List<int>(SectionDict.Keys);
            keys.Sort();
            for (int idx = 0; idx < keys.Count; idx++)
            {
                section = SectionDict[keys[idx]];
                if ((section.Name == name || section.Name == name + "[0]") && section.Prot == prot) break;
            }
            return section;
        }

        public Section GetSection(int sid)
        {
            SectionDict.TryGetValue(sid, out Section section);

            return section;
        }

        public Section GetSection(int sid, string name, uint prot)
        {
            Section section = GetSection(sid);
            if (section != null && name != null && name.Length > 0 && !name.Contains(section.Name)) section = GetSection(name, prot);

            return section;
        }

        public int GetSectionID(ulong address)
        {
            if (MemoryStart > address || MemoryEnd < address) return -1;
            
            int low = 0;
            int high = SectionList.Count - 1;
            int middle;

            while (low <= high)
            {
                middle = (low + high) / 2;
                (int SID, ulong start, ulong end) = SectionList[middle];
                if (address >= end) low = middle + 1;   //查找数组后部分  
                else if (address < start) high = middle - 1;  //查找数组前半部分  
                else return SID;  //找到用户要查找的数字，返回下标  
            }

            return -1;
        }
    }
}
