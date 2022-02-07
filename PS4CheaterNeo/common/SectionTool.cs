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
        public ulong TotalMemorySize;
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
            public bool IsFilterSize;

            public override string ToString() => $"{Start:X},{(float)Length / 1024} KB,{Name},{Prot:X},{Offset:X},{IsFilter},{IsFilterSize},{Check},{PID},{SID}";
        }

        /// <summary>
        /// Reorder MemoryEntry, the order without name is higher, and then order by prot, name, start
        /// </summary>
        private int CompareMemoryEntry(MemoryEntry e1, MemoryEntry e2)
        {
            int result = 0;
            if (e1.name == "" || e2.name == "") result = e1.name.CompareTo(e2.name);
            if (result != 0) return result;

            result = e1.prot.CompareTo(e2.prot);
            if (result != 0) return result;

            result = ((e1.name == "executable" ? " " : "") + e1.name).CompareTo((e2.name == "executable" ? " " : "") + e2.name);
            if (result != 0) return result;

            result = e1.start.CompareTo(e2.start);
            return result;
        }

        /// <summary>
        /// Initialize section list
        /// </summary>
        /// <param name="processName">specified process name</param>
        /// <exception cref="Exception"></exception>
        public void InitSectionList(string processName)
        {
            ProcessInfo processInfo = PS4Tool.GetProcessInfo(processName);

            if (processInfo.pid == 0) throw new Exception(string.Format("{0}: ProcessInfo is null.", processName));

            InitSectionList(processInfo.pid, processName);
        }

        /// <summary>
        /// Initialize section list
        /// </summary>
        /// <param name="processID">specified process id</param>
        /// <param name="processName">specified process name</param>
        /// <exception cref="Exception"></exception>
        public void InitSectionList(int processID, string processName)
        {
            ProcessMap pMap = PS4Tool.GetProcessMaps(processID);

            if (pMap == null || pMap.entries == null || pMap.entries.Length == 0) throw new Exception(string.Format("{0}: Process({1}) Map is null.", processName, processID));

            InitSectionList(pMap, processID, processName);
        }

        /// <summary>
        /// Initialize section list
        /// </summary>
        /// <param name="pMap">libdebug.ProcessMap</param>
        /// <param name="processID">specified process id</param>
        /// <param name="processName">specified process name</param>
        /// <exception cref="Exception"></exception>
        public void InitSectionList(ProcessMap pMap, int processID, string processName)
        {
            if (pMap == null || pMap.entries == null || pMap.entries.Length == 0) throw new Exception(string.Format("{0}: Process({1}) Map is null.", processName, processID));

            mutex.WaitOne();
            try
            {
                string SectionFilterKeys = Properties.Settings.Default.SectionFilterKeys.Value;
                uint SectionFilterSize = Properties.Settings.Default.SectionFilterSize.Value;
                SectionFilterKeys = Regex.Replace(SectionFilterKeys, " *[,;] *", "|");
                SectionDict = new Dictionary<int, Section>();
                SectionList = new List<(int SID, ulong start, ulong end)>();
                TotalMemorySize = 0;
                PID = pMap.pid;
                int sIdx = 0;
                int protCnt = 0;
                uint protTmp = 0;
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
                        bool isFilter = SectionIsFilter(entry.name, SectionFilterKeys);

                        ulong bufferLength = 1024 * 1024 * 128;
                        if ((entry.prot & 0x5) == 0x5) bufferLength = length; //Executable section
                        if (MemoryStart == 0 || start < MemoryStart) MemoryStart = start;
                        if (MemoryEnd == 0 || end > MemoryEnd) MemoryEnd = end;
                        if (protTmp > 0 && protTmp != entry.prot)
                        { //Calculate SID value: Increase the prot count and reset sIdx to zero when the prot has changed
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
                            else if (section.Length < SectionFilterSize) section.IsFilterSize = true;

                            SectionDict.Add(section.SID, section);
                            SectionList.Add((section.SID, start, start + curLength));

                            start += curLength;
                            idx++;
                        }
                        if (idx > 99) sIdx += idx / 100;
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

        /// <summary>
        /// Check if section needs to be filtered
        /// </summary>
        /// <param name="name">specified section name</param>
        /// <param name="sectionFilterKeys">filters for Section in regex</param>
        /// <returns></returns>
        public bool SectionIsFilter(string name, string sectionFilterKeys)
        {
            bool result = false;
            if (Regex.IsMatch(name, sectionFilterKeys)) result = true;

            return result;
        }

        /// <summary>
        /// find Section with name, prot as unique keys
        /// </summary>
        /// <param name="name"></param>
        /// <param name="prot"></param>
        /// <returns></returns>
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

        /// <summary>
        /// find Section with sid as unique keys
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public Section GetSection(int sid)
        {
            SectionDict.TryGetValue(sid, out Section section);

            return section;
        }

        /// <summary>
        /// find Section with sid, name, prot as unique keys
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="name"></param>
        /// <param name="prot"></param>
        /// <returns></returns>
        public Section GetSection(int sid, string name, uint prot)
        {
            Section section = GetSection(sid);
            if (section != null && name != null && name.Length > 0 && !name.Contains(section.Name)) section = GetSection(name, prot);

            return section;
        }

        /// <summary>
        /// get its section SID by address
        /// </summary>
        /// <param name="address">query address</param>
        /// <returns>section SID</returns>
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
                if (address >= end) low = middle + 1;   //find the second half of the array
                else if (address < start) high = middle - 1;  //find the first half of the array
                else return SID;  //return SID of the specified address
            }

            return -1;
        }


        /// <summary>
        /// get Sections sorted by address
        /// </summary>
        /// <param name="collection">get only Sections of the specified SID, is optional</param>
        /// <returns>section array</returns>
        public Section[] GetSectionSortByAddr(IEnumerable<int> collection = null)
        {
            if (collection == null) collection = SectionDict.Keys;
            List<int> keys = new List<int>(collection);
            Section[] sections = new Section[keys.Count];
            for (int sectionIdx = 0; sectionIdx < keys.Count; sectionIdx++) sections[sectionIdx] = SectionDict[keys[sectionIdx]];
            Array.Sort(sections, CompareSection);

            return sections;
        }

        /// <summary>
        /// get Sections sorted by address and return the position of the given SID
        /// </summary>
        /// <param name="SID">find the position of the SID</param>
        /// <param name="idx">position index of the SID</param>
        /// <param name="collection">get only Sections of the specified SID, is optional</param>
        /// <returns>section array</returns>
        public Section[] GetSectionSortByAddr(int SID, out int idx, IEnumerable<int> collection = null)
        {
            idx = -1;
            Section[] sections = GetSectionSortByAddr(collection);

            for (int sectionIdx = 0; sectionIdx < sections.Length; sectionIdx++)
            {
                Section section = sections[sectionIdx];
                if (section.SID != SID) continue;
                idx = sectionIdx;
                break;
            }

            return sections;
        }

        /// <summary>
        /// sort by start address
        /// </summary>
        public int CompareSection(Section s1, Section s2) => s1.Start.CompareTo(s2.Start);
    }
}
