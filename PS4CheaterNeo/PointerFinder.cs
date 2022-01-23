using libdebug;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static PS4CheaterNeo.SectionTool;

namespace PS4CheaterNeo
{
    public partial class PointerFinder : Form
    {
        readonly Main mainForm;
        readonly string processName;
        readonly SectionTool sectionTool;
        int level;
        int maxRange;
        List<int> range;
        List<((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM)> pathAddrList;
        List<((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM)> pathAddrListOrderByValue;
        List<((int SID, string name, uint prot, int position, List<long> offsets) pointer,
            List<((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM)> pathAddress)> pointerResults;
        List<ListViewItem> pointerItems;
        public PointerFinder(Main mainForm, ulong address, ScanType scanType)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            sectionTool = new SectionTool();
            processName = mainForm.ProcessName;
            pointerResults = new List<((int SID, string name, uint prot, int position, List<long> offsets) pointer, List<((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM)> pathAddress)>();
            AddressBox.Text = address.ToString("X");
            PointerListView
                .GetType()
                .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(PointerListView, true, null);

            ScanType[] scanTypes = (ScanType[])Enum.GetValues(typeof(ScanType));
            for (int idx = 0; idx < scanTypes.Length; idx++)
            {
                ScanType filterEnum = scanTypes[idx];
                ScanTypeBox.Items.Add(new ComboboxItem(filterEnum.GetDescription(), filterEnum));
                if (scanType == filterEnum) ScanTypeBox.SelectedIndex = ScanTypeBox.Items.Count - 1;
            }
            
        }

        #region Event
        private void PointerFinder_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (PointerListView.Items.Count > 0 && 
                MessageBox.Show("Still in the find pointer, Do you want to close PointerFinder?", "PointerFinder", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) e.Cancel = true;
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            SaveDialog.Filter = "Pointer files (*.txt)|*.txt";
            SaveDialog.FilterIndex = 1;
            SaveDialog.RestoreDirectory = true;

            if (SaveDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                List<string> lines = new List<string>();
                lines.Add($"FindAddress: {AddressBox.Text}({ulong.Parse(AddressBox.Text, NumberStyles.HexNumber)})");
                for (int idx = 0; idx < pathAddrList.Count; ++idx)
                {
                    ((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM) = pathAddrList[idx];
                    lines.Add(string.Format("{0:X}|{1:X}|{2}|{3}|{4:X}|{5}|{6}|{7:X}", addrM.position, valueM.position, addrM.SID, addrM.name, addrM.prot, valueM.SID, valueM.name, valueM.prot));
                }

                File.WriteAllLines(SaveDialog.FileName, lines);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Save memory failed, " + exception.Message, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void LoadBtn_Click(object sender, EventArgs e)
        {
            OpenDialog.Filter = "Pointer files (*.txt)|*.txt";
            OpenDialog.FilterIndex = 1;
            OpenDialog.RestoreDirectory = true;

            if (OpenDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                string[] lines = File.ReadAllLines(OpenDialog.FileName);
                if (lines.Length == 0) return;

                sectionTool.InitSectionList(processName);

                System.Diagnostics.Stopwatch ticker = System.Diagnostics.Stopwatch.StartNew();
                pathAddrList = new List<((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM)>();

                for (int i = 1; i < lines.Length; ++i)
                {
                    string[] elems = lines[i].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (elems == null || elems.Length < 7) continue;

                    int addrPosition = int.Parse(elems[0], NumberStyles.HexNumber);
                    int valuePosition = int.Parse(elems[1], NumberStyles.HexNumber);

                    int addrSID = int.Parse(elems[2]);
                    string addrName = elems[3];
                    uint addrProt = uint.Parse(elems[4], NumberStyles.HexNumber);
                    Section addrSection = sectionTool.GetSection(addrSID, addrName, addrProt);

                    int valueSID = int.Parse(elems[5]);
                    string valueName = elems[6];
                    uint valueProt = uint.Parse(elems[7], NumberStyles.HexNumber);
                    Section valueSection = sectionTool.GetSection(valueSID, valueName, valueProt);

                    if (addrSection == null || valueSection == null) continue;

                    pathAddrList.Add(((addrSection.SID, addrSection.Name, addrSection.Prot, addrPosition, (ulong)addrPosition + addrSection.Start),
                        (valueSection.SID, valueSection.Name, valueSection.Prot, valuePosition, (ulong)valuePosition + valueSection.Start)));
                }
                pathAddrList.Sort((m1, m2) => m1.addrM.addr.CompareTo(m2.addrM.addr));
                pathAddrListOrderByValue = new List<((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM)>(pathAddrList);
                pathAddrListOrderByValue.Sort((m1, m2) => { 
                    if (m1.valueM.value == m2.valueM.value) return m1.addrM.addr.CompareTo(m2.addrM.addr);
                    else return m1.valueM.value.CompareTo(m2.valueM.value);
                });

                IsInitScan.Checked = false;
                ScanBtn.PerformClick();
                ToolStripMsg.Text = pathAddrList.Count + " results, elapsed:" + ticker.Elapsed.TotalSeconds;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Save memory failed, " + exception.Message, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void PointerListView_DoubleClick(object sender, EventArgs e)
        {
            if (PointerListView.SelectedItems.Count != 1) return;

            ListViewItem resultItem = PointerListView.SelectedItems[0];
            int idx = (int)resultItem.Tag;

            var pointerResult = pointerResults[idx];

            ScanType scanType = (ScanType)((ComboboxItem)(ScanTypeBox.SelectedItem)).Value;
            Section section = sectionTool.GetSection(pointerResult.pointer.SID, pointerResult.pointer.name, pointerResult.pointer.prot);
            ulong baseAddress = section.Start + (ulong)pointerResult.pointer.position;

            try
            {
                List<long> offsetList = new List<long> { (long)baseAddress };
                offsetList.AddRange(pointerResult.pointer.offsets);
                NewAddress newAddress = new NewAddress(mainForm, null, section, 0, scanType, null, false, "", offsetList, false);
                if (newAddress.ShowDialog() != DialogResult.OK)
                    return;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\n" + exception.StackTrace, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void ScanBtn_Click(object sender, EventArgs e)
        {
            if (PointerFinderWorker.IsBusy)
            {
                if (MessageBox.Show("Still in the pointer scanning, Do you want to stop scan?", "PointerFinder", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    PointerFinderWorker.CancelAsync();
            }
            else if (pointerResults.Count == 0 && MessageBox.Show("Perform First Scan?", "First Scan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            else if (pointerResults.Count > 0 && MessageBox.Show("Perform Next Scan?", "Next Scan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            else
            {
                pointerItems = new List<ListViewItem>();
                level = (int)LevelUpdown.Value;
                maxRange = (int)MaxRangeUpDown.Value;
                ulong queryAddress = ulong.Parse(AddressBox.Text, NumberStyles.HexNumber);
                PointerListView.Items.Clear();
                PointerListView.GridLines = true;
                PointerListView.Columns.Add("Base Address", "Base Address");
                PointerListView.Columns.Add("Base Section", "Base Section");
                range = new List<int>();
                for (int i = 0; i < level; ++i)
                {
                    range.Add(maxRange);
                    PointerListView.Columns.Add("Offset " + (i + 1), "Offset " + (i + 1));
                }


                ScanBtn.Text = "Stop";
                PointerFinderWorker.RunWorkerAsync((queryAddress, range));
            }
        }

        private void NewBtn_Click(object sender, EventArgs e)
        {
            pointerResults.Clear();
            IsInitScan.Checked = true;
            ScanBtn.Text = "First Scan";
        }

        private void PointerListViewAddToCheatGrid_Click(object sender, EventArgs e)
        {
            if (PointerListView.SelectedItems.Count == 0) return;

            ScanType scanType = (ScanType)((ComboboxItem)(ScanTypeBox.SelectedItem)).Value;
            ListView.SelectedListViewItemCollection items = PointerListView.SelectedItems;
            for (int itemIdx = 0; itemIdx < items.Count; ++itemIdx)
            {
                try
                {
                    ListViewItem resultItem = items[itemIdx];
                    int idx = (int)resultItem.Tag;

                    var pointerResult = pointerResults[idx];

                    Section section = sectionTool.GetSection(pointerResult.pointer.SID, pointerResult.pointer.name, pointerResult.pointer.prot);
                    ulong baseAddress = section.Start + (ulong)pointerResult.pointer.position;
                    string msg = pointerResult.pointer.position.ToString("X");
                    List<long> offsetList = new List<long> { (long)baseAddress };
                    pointerResult.pointer.offsets.ForEach(offset => {
                        msg += "_" + offset.ToString("X");
                        offsetList.Add(offset);
                    });
                    mainForm.AddToCheatGrid(section, 0, scanType, "0", false, msg, true, offsetList); //FIXME oldValue is 0
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message + "\n" + exception.StackTrace, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void FilterRuleBtn_Click(object sender, EventArgs e)
        {
            string sectionFilterKeys = Properties.Settings.Default.SectionFilterKeys.Value;

            if (InputBox.Show("Section Filter", "Enter the value of the filter keys", ref sectionFilterKeys, null) != DialogResult.OK) return;

            Properties.Settings.Default.SectionFilterKeys.Value = sectionFilterKeys;
        }
        #endregion

        #region Worker
        private void PointerFinderWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int nextScanCheckNumber = 150;
            System.Diagnostics.Stopwatch tickerMajor = System.Diagnostics.Stopwatch.StartNew();
            PointerFinderWorker.ReportProgress(0);
            sectionTool.InitSectionList(processName);

            (ulong queryAddress, List<int> range) = ((ulong queryAddress, List<int> range))e.Argument;
            if (IsInitScan.Checked || pointerResults.Count > nextScanCheckNumber)
            {
                string sectionFilterKeys = Properties.Settings.Default.SectionFilterKeys.Value;
                sectionFilterKeys = Regex.Replace(sectionFilterKeys, " *[,;] *", "|");
                Invoke(new MethodInvoker(() => { IsInitScan.Checked = false; }));
                var pathAddrList = new List<((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM)>();
                var pathAddrListOrderByValue = new List<((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM)>();
                int hitCnt = 0;
                List<int> keys = new List<int>(sectionTool.SectionDict.Keys);
                keys.Sort();

                for (int sectionIdx = 0; sectionIdx < keys.Count; sectionIdx++)
                {
                    if (PointerFinderWorker.CancellationPending) return;

                    PointerFinderWorker.ReportProgress((int)(((float)(sectionIdx + 1) / keys.Count) * 50), (tickerMajor.Elapsed, string.Format("Section ID: {0}/{1}, read memory...", hitCnt, keys.Count)));
                    Section section = sectionTool.SectionDict[keys[sectionIdx]];

                    if (IsFilterBox.Checked && sectionTool.SectionIsFilter(section.Name, sectionFilterKeys)) continue;
                    if (!IsFilterBox.Checked && section.Name.StartsWith("libSce")) continue;

                    byte[] buffer = PS4Tool.ReadMemory(section.PID, section.Start, section.Length);
                    PointerFinderWorker.ReportProgress((int)(((float)(sectionIdx + 1) / keys.Count) * 50), (tickerMajor.Elapsed, string.Format("Section ID: {0}/{1}, mapped memory...", hitCnt, keys.Count)));
                    hitCnt++;

                    for (int scanIdx = 0; scanIdx + 8 < buffer.LongLength; scanIdx += 8)
                    {
                        byte[] newValue = new byte[8];
                        Buffer.BlockCopy(buffer, scanIdx, newValue, 0, 8);
                        ulong mappedValue = BitConverter.ToUInt64(newValue, 0);
                        int valueSID = mappedValue > 0 ? sectionTool.GetSectionID(mappedValue) : -1;
                        if (valueSID == -1) continue;
                        Section valueSection = sectionTool.GetSection(valueSID);
                        pathAddrList.Add(((section.SID, section.Name, section.Prot, scanIdx, section.Start + (ulong)scanIdx), (valueSection.SID, valueSection.Name, valueSection.Prot, (int)(mappedValue - valueSection.Start), mappedValue)));
                    }
                }

                if (pathAddrList != null && pathAddrList.Count > 0)
                {
                    pathAddrList.Sort((m1, m2) => m1.addrM.addr.CompareTo(m2.addrM.addr));
                    pathAddrListOrderByValue = new List<((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM)>(pathAddrList);
                    pathAddrListOrderByValue.Sort((m1, m2) => {
                        if (m1.valueM.value == m2.valueM.value) return m1.addrM.addr.CompareTo(m2.addrM.addr);
                        else return m1.valueM.value.CompareTo(m2.valueM.value);
                    });
                    this.pathAddrList = pathAddrList;
                    this.pathAddrListOrderByValue = pathAddrListOrderByValue;
                }
            }
            else if (pathAddrList != null && pathAddrList.Count > 0)
            {
                for (int idx = 0; idx < pathAddrList.Count; idx++)
                {
                    if (idx % 1000 == 0) PointerFinderWorker.ReportProgress((int)(((float)(idx + 1) / pathAddrList.Count) * 50), (tickerMajor.Elapsed, string.Format("Determine PathAddr List: {0}/{1}", idx+1, pathAddrList.Count)));

                    ((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM) = pathAddrList[idx];
                    Section addrSection = sectionTool.GetSection(addrM.SID, addrM.name, addrM.prot);
                    Section valueSection = sectionTool.GetSection(valueM.SID, valueM.name, valueM.prot);
                    if (addrSection == null || valueSection == null) continue;

                    pathAddrList[idx] = ((addrSection.SID, addrSection.Name, addrSection.Prot, addrM.position, (ulong)addrM.position + addrSection.Start),
                        (valueSection.SID, valueSection.Name, valueSection.Prot, valueM.position, (ulong)valueM.position + valueSection.Start));
                }
                pathAddrListOrderByValue = new List<((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM)>(pathAddrList);
                pathAddrListOrderByValue.Sort((m1, m2) =>
                {
                    if (m1.valueM.value == m2.valueM.value) return m1.addrM.addr.CompareTo(m2.addrM.addr);
                    else return m1.valueM.value.CompareTo(m2.valueM.value);
                });
            }

            if (pointerResults.Count == 0) QueryPointerFirst(queryAddress, range);
            else
            {

                Dictionary<ulong, ulong> pointerMemoryCaches = new Dictionary<ulong, ulong>();
                var newPointerResults = new List<((int SID, string name, uint prot, int position, List<long> offsets) pointer, 
                    List<((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM)> pathAddress)>();
                for (int pIdx = 0; pIdx < pointerResults.Count; ++pIdx)
                {
                    if (pIdx % 1000 == 0) PointerFinderWorker.ReportProgress(50 + (int)(((float)(pIdx + 1) / pointerResults.Count) * 50), (tickerMajor.Elapsed, string.Format("Compare pointer results: {0}/{1}, mapped memory...", pIdx + 1, pointerResults.Count)));

                    var pointerResult = pointerResults[pIdx];
                    var tailAddress = pointerResults.Count > nextScanCheckNumber ? ReadTailAddressByPathListValue(pointerResult) : ReadTailAddress(pointerResult, pointerMemoryCaches);
                    if (tailAddress != queryAddress) continue;

                    newPointerResults.Add(pointerResult);
                    if (newPointerResults.Count < 20000 && pointerResult.pointer.offsets.Count > 0) AddPointerListViewItem(newPointerResults.Count - 1, pointerResult.pathAddress, pointerResult.pointer.offsets);
                }

                if (newPointerResults.Count > 0 ||
                    newPointerResults.Count == 0 && MessageBox.Show("Whether to continue?", "Next Scan results are zero", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    pointerResults.Clear();
                    pointerResults = newPointerResults;
                }
            }

            PointerFinderWorker.ReportProgress(100, (tickerMajor.Elapsed, string.Format("Query pointer end, find:{0}", pointerResults.Count)));
        }

        private void QueryPointerFirst(ulong queryAddress, List<int> range, int level=0, List<long> pathOffset=null, 
            List<((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM)> pathAddress=null)
        {
            if (PointerFinderWorker.CancellationPending) return;
            if (level >= range.Count) return;
            if (pathOffset == null) pathOffset = new List<long>();
            if (pathAddress == null) pathAddress = new List<((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM)>();

            int hitIdx = GetPointerByAddress(queryAddress, out _);
            int counter = 0;

            for (int addrIdx = hitIdx; NegativeOffsetBox.Checked ? addrIdx < pathAddrList.Count : addrIdx >= 0; addrIdx += NegativeOffsetBox.Checked ? 1 : -1)
            {
                if (PointerFinderWorker.CancellationPending) break;
                else if (range[level] > 0 && (long)pathAddrList[addrIdx].addrM.addr + range[level] < (long)queryAddress) break;
                else if (range[level] < 0 && (long)pathAddrList[addrIdx].addrM.addr + range[level] > (long)queryAddress) break;

                var pointerList = GetPointerListByValue(pathAddrList[addrIdx].addrM.addr);
                if (pointerList.Count == 0) continue;

                pathOffset.Add((long)(queryAddress - pathAddrList[addrIdx].addrM.addr));
                const int maxPointerCount = 15;
                int curPointerCounter = 0;
                bool inNewLevel = false;
                for (int j = 0; j < pointerList.Count; ++j)
                {
                    bool inStack = false;
                    for (int k = 0; k < pathAddress.Count; ++k)
                    {
                        if (pathAddress[k].valueM.value == pointerList[j].valueM.value || pathAddress[k].addrM.addr == pointerList[j].addrM.addr)
                        {
                            inStack = true;
                            break;
                        }
                    }
                    if (inStack) continue;

                    inNewLevel = true;
                    if (curPointerCounter >= maxPointerCount) break;

                    ++curPointerCounter;

                    pathAddress.Add(pointerList[j]);
                    QueryPointerFirst(pointerList[j].addrM.addr, range, level + 1, pathOffset, pathAddress);
                    pathAddress.RemoveAt(pathAddress.Count - 1);
                }

                pathOffset.RemoveAt(pathOffset.Count - 1);

                if (counter >= 1) break;
                if (inNewLevel) ++counter;
            }

            if (PointerFinderWorker.CancellationPending) return;
            if (pathAddress == null || pathAddress.Count == 0) return;

            Section addrSection = sectionTool.GetSection(pathAddress[pathOffset.Count - 1].addrM.SID, pathAddress[pathOffset.Count - 1].addrM.name, pathAddress[pathOffset.Count - 1].addrM.prot);
            if (addrSection == null) return;
            if (FastScaBox.Checked && !addrSection.Name.StartsWith("executable")) return;

            int pointerPosition = (int)(pathAddress[pathOffset.Count - 1].addrM.addr - addrSection.Start);

            ((int SID, string name, uint prot, int position, List<long> offsets) pointer, 
                List<((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM)> pathAddress) pointerResult =
                ((addrSection.SID, addrSection.Name, addrSection.Prot, pointerPosition, new List<long>()),
                new List<((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM)>(pathAddress));

            for (int i = pathOffset.Count - 1; i >= 0; --i) pointerResult.pointer.offsets.Add(pathOffset[i]);

            pointerResults.Add(pointerResult);

            if (pointerResults.Count < 20000 && pathOffset.Count > 0) AddPointerListViewItem(pointerResults.Count - 1, pathAddress, pathOffset);
            if (pointerResults.Count % 1024 == 0) ToolStripMsg.Text = $"{pointerResults.Count} results";
        }

        private void AddPointerListViewItem(int idx, List<((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM)> pathAddress, List<long> pathOffset)
        {
            int itemIdx = PointerListView.Items.Count;
            ((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM) = pathAddress[pathAddress.Count - 1];
            Section section = sectionTool.GetSection(addrM.SID, addrM.name, addrM.prot);
            string sectionStr = $"{section.Start.ToString("X9")}-{section.Name}-{section.Prot.ToString("X")}-{section.Length / 1024}KB";

            ListViewItem item = new ListViewItem(addrM.addr.ToString("X"), 0); //Base Address
            item.Tag = idx;
            item.SubItems.Add(sectionStr); //Base Section
            for (int i = 0; i < pathOffset.Count; ++i) item.SubItems.Add(pathOffset[i].ToString("X"));
            pointerItems.Add(item);
            Invoke(new MethodInvoker(() =>
            {
                if (pointerItems.Count % 100 == 0)
                {
                    PointerListView.Items.AddRange(pointerItems.ToArray());
                    pointerItems.Clear();
                }
            }));
        }

        private int GetPointerByAddress(ulong queryAddress, out ((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM) pointer)
        {
            pointer = ((0, "", 0, 0, 0), (0, "", 0, 0, 0));
            int index = BinarySearchByAddress(queryAddress, 0, pathAddrList.Count - 1);
            if (index < 0) return index;

            pointer = pathAddrList[index];
            return index;
        }

        private List<((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM)> GetPointerListByValue(ulong pointerAddrM)
        {
            var pointerList = new List<((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM)>();
            int index = BinarySearchByValue(pointerAddrM);

            if (index < 0) return pointerList;

            int start = index;
            for (; start >= 0; --start) if (pathAddrListOrderByValue[start].valueM.value != pointerAddrM) break;

            bool find = false;
            for (int i = start; i < pathAddrListOrderByValue.Count; ++i)
            {
                if (pathAddrListOrderByValue[i].valueM.value == pointerAddrM)
                {
                    find = true;
                    pointerList.Add(pathAddrListOrderByValue[i]);
                }
                else if (find) break;
            }

            return pointerList;
        }

        private int BinarySearchByAddress(ulong queryAddress, int low, int high)
        {
            int mid = (low + high) / 2;
            if (low > high) return -1;
            else
            {
                if (pathAddrList[mid].addrM.addr == queryAddress) return mid;
                else if (pathAddrList[mid].addrM.addr > queryAddress)
                {
                    if (mid - 1 >= 0 && pathAddrList[mid - 1].addrM.addr <= queryAddress) return mid - 1;
                    return BinarySearchByAddress(queryAddress, low, mid - 1);
                }
                else
                {
                    if (mid + 1 < pathAddrList.Count && pathAddrList[mid + 1].addrM.addr >= queryAddress) return mid + 1;
                    return BinarySearchByAddress(queryAddress, mid + 1, high);
                }
            }
        }
        private int BinarySearchByValue(ulong pointerAddrM)
        {
            int low = 0;
            int high = pathAddrListOrderByValue.Count - 1;
            int middle;

            while (low <= high)
            {
                middle = (low + high) / 2;
                if (pointerAddrM > pathAddrListOrderByValue[middle].valueM.value) low = middle + 1;
                else if (pointerAddrM < pathAddrListOrderByValue[middle].valueM.value) high = middle - 1;
                else return middle;
            }

            return -1;
        }

        private ulong ReadTailAddressByPathListValue(((int SID, string name, uint prot, int position, List<long> offsets) pointer,
            List<((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM)> pathAddress) pointerResult)
        {
            ulong targetAddr = 0;
            Section section = sectionTool.GetSection(pointerResult.pointer.SID, pointerResult.pointer.name, pointerResult.pointer.prot);
            if (section == null) return targetAddr;

            List<long> offsetList = new List<long>();
            offsetList.Add((long)(section.Start + (ulong)pointerResult.pointer.position));
            offsetList.AddRange(pointerResult.pointer.offsets);

            ulong headAddress = 0;
            for (int idx = 0; idx < offsetList.Count; ++idx)
            {
                long offset = offsetList[idx];
                if (idx != offsetList.Count - 1)
                {
                    int pathAddrIdx = BinarySearchByAddress((ulong)offset + headAddress, 0, pathAddrList.Count - 1); //int pathValueIdx = BinarySearchByValue((ulong)offset + headAddress);
                    if (pathAddrIdx == -1) break;
                    headAddress = pathAddrList[pathAddrIdx].valueM.value;
                }
                else targetAddr = (ulong)offset + headAddress;
            }

            return targetAddr;
        }

        private ulong ReadTailAddress(((int SID, string name, uint prot, int position, List<long> offsets) pointer,
            List<((int SID, string name, uint prot, int position, ulong addr) addrM, (int SID, string name, uint prot, int position, ulong value) valueM)> pathAddress) pointerResult, in Dictionary<ulong, ulong> pointerMemoryCaches)
        {
            Section section = sectionTool.GetSection(pointerResult.pointer.SID, pointerResult.pointer.name, pointerResult.pointer.prot);
            if (section == null) return 0;

            var targetAddr = PS4Tool.ReadTailAddress(section.PID, section.Start + (ulong)pointerResult.pointer.position, pointerResult.pointer.offsets, pointerMemoryCaches);
            return targetAddr;
        }

        private void PointerFinderWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgBar.Value = e.ProgressPercentage;

            if (e.UserState == null) return;

            if (e.UserState is ValueTuple<TimeSpan , string >)
            {
                (TimeSpan elapsed, string msg) = ((TimeSpan elapsed, string msg))e.UserState;
                ToolStripMsg.Text = string.Format("Scan elapsed:{0}s. {1}", elapsed.TotalSeconds, msg);
            }
        }

        private void PointerFinderWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (pointerItems.Count > 0)
            {
                PointerListView.Items.AddRange(pointerItems.ToArray());
                pointerItems.Clear();
                ScanBtn.Text = "Next Scan";
            }
            else if (pointerResults.Count == 0) ScanBtn.Text = "First Scan";
            else if (pointerResults.Count > 0) ScanBtn.Text = "Next Scan";
        }
        #endregion
    }
}
