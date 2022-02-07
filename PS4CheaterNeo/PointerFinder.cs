using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
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
        Dictionary<int, List<Pointer>> addrPointerDict;
        Dictionary<int, List<Pointer>> valuePointerDict;
        List<((int AddrSID, uint AddrPos, List<long> Offsets) pointer, List<Pointer> pathAddress)> pointerResults;

        List<ListViewItem> pointerItems;
        public PointerFinder(Main mainForm, ulong address, ScanType scanType)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            sectionTool = new SectionTool();
            processName = mainForm.ProcessName;
            pointerResults = new List<((int AddrSID, uint AddrPos, List<long> Offsets) pointer, List<Pointer> pathAddress)>();
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
            IsFilterBox.Checked = Properties.Settings.Default.EnableFilterQuery.Value;
            IsFilterSizeBox.Checked = Properties.Settings.Default.EnableFilterSizeQuery.Value;
        }

        #region Event
        private void PointerFinder_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pointerTask != null && !pointerTask.IsCompleted)
            {
                if (MessageBox.Show("Still in the pointer scanning, Do you want to close PointerFinder?", "PointerFinder", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (pointerSource != null) try { pointerSource.Cancel(); } catch (Exception) { } 
                }
                else e.Cancel = true;
            }
            else if (PointerListView.Items.Count > 0 && 
                MessageBox.Show("Still in the find pointer, Do you want to close PointerFinder?", "PointerFinder", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) e.Cancel = true;
            GC.Collect();
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            SaveDialog.Filter = "Pointer files (*.txt)|*.txt";
            SaveDialog.FilterIndex = 1;
            SaveDialog.RestoreDirectory = true;

            if (SaveDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                List<int> SIDs = new List<int>(addrPointerDict.Keys);
                SIDs.Sort();
                List<string> lines = new List<string>();
                lines.Add($"FindAddress: {AddressBox.Text}({ulong.Parse(AddressBox.Text, NumberStyles.HexNumber)})");
                for (int sIdx = 0; sIdx < SIDs.Count; sIdx++)
                {
                    int addrSID = SIDs[sIdx];
                    Section addrSection = sectionTool.GetSection(addrSID);
                    List<Pointer> addrList = addrPointerDict[addrSID];
                    for (int idx = 0; idx < addrList.Count; idx++)
                    {
                        Pointer pointer = addrList[idx];
                        Section valueSection = sectionTool.GetSection(pointer.ValueSID);
                        lines.Add(string.Format("{0:X}|{1:X}|{2}|{3}|{4:X}|{5}|{6}|{7:X}", pointer.AddrPos, pointer.ValuePos, addrSID, "_", "_", pointer.ValueSID, "_", "_"));
                    }
                }

                File.WriteAllLines(SaveDialog.FileName, lines);
                GC.Collect();
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
                sectionTool.InitSectionList(processName);
                System.Diagnostics.Stopwatch ticker = System.Diagnostics.Stopwatch.StartNew();
                if (addrPointerDict != null) addrPointerDict.Clear();
                if (valuePointerDict != null) valuePointerDict.Clear();
                addrPointerDict = new Dictionary<int, List<Pointer>>();
                valuePointerDict = new Dictionary<int, List<Pointer>>();
                GC.Collect();
                int resultCnt = 0;
                List<Pointer> addrValueList = new List<Pointer>();
                using (StreamReader sr = new StreamReader(OpenDialog.FileName))
                {
                    while (sr.Peek() >= 0)
                    {
                        string line = sr.ReadLine();
                        string[] elems = line.Split(new char[] { '|' });
                        if (elems == null || elems.Length < 7) continue;

                        uint addrPosition = uint.Parse(elems[0], NumberStyles.HexNumber);
                        uint valuePosition = uint.Parse(elems[1], NumberStyles.HexNumber);
                        int addrSID = int.Parse(elems[2]);
                        int valueSID = int.Parse(elems[5]);
                        Section addrSection = sectionTool.GetSection(addrSID);
                        Section valueSection = sectionTool.GetSection(valueSID);

                        if (addrSection == null || valueSection == null) continue;

                        addrValueList.Add(new Pointer(addrSID, addrPosition, valueSID, valuePosition));
                        resultCnt++;
                    }
                }
                GC.Collect();
                if (addrValueList != null && addrValueList.Count > 0)
                {
                    int tmpAddrSID = 0;
                    List<Pointer> addrPointerList = null;
                    addrValueList.Sort((p1, p2) => p1.AddrSID.CompareTo(p2.AddrSID));
                    for (int idx = 0; idx < addrValueList.Count; idx++)
                    {
                        Pointer pointer = addrValueList[idx];
                        if (tmpAddrSID == 0 || tmpAddrSID != pointer.AddrSID)
                        {
                            if (addrPointerList != null)
                            {
                                addrPointerList.Sort((p1, p2) => p1.AddrPos.CompareTo(p2.AddrPos));
                                addrPointerDict[tmpAddrSID] = addrPointerList;
                            }
                            if (!addrPointerDict.TryGetValue(pointer.AddrSID, out addrPointerList)) addrPointerList = new List<Pointer>();
                        }
                        addrPointerList.Add(pointer);
                        tmpAddrSID = pointer.AddrSID;
                    }

                    int tmpValueSID = 0;
                    List<Pointer> valuePointerList = null;
                    addrValueList.Sort((p1, p2) => p1.ValueSID.CompareTo(p2.ValueSID));
                    for (int idx = 0; idx < addrValueList.Count; idx++)
                    {
                        Pointer pointer = addrValueList[idx];
                        if (tmpValueSID == 0 || tmpValueSID != pointer.ValueSID)
                        {
                            if (valuePointerList != null)
                            {
                                valuePointerList.Sort((p1, p2) => p1.ValuePos.CompareTo(p2.ValuePos));
                                valuePointerDict[tmpValueSID] = valuePointerList;
                            }
                            if (!valuePointerDict.TryGetValue(pointer.ValueSID, out valuePointerList)) valuePointerList = new List<Pointer>();
                        }
                        valuePointerList.Add(pointer);
                        tmpValueSID = pointer.ValueSID;
                    }
                    addrValueList = null;
                }
                GC.Collect();

                IsInitScan.Checked = false;
                ToolStripMsg.Text = String.Format("Loaded {0} address-value results, elapsed: {1}", resultCnt, ticker.Elapsed.TotalSeconds);
                ScanBtn.PerformClick();
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
            Section section = sectionTool.GetSection(pointerResult.pointer.AddrSID);
            ulong baseAddress = section.Start + (ulong)pointerResult.pointer.AddrPos;

            try
            {
                List<long> offsetList = new List<long> { (long)baseAddress };
                offsetList.AddRange(pointerResult.pointer.Offsets);
                NewAddress newAddress = new NewAddress(mainForm, null, section, 0, scanType, null, false, "", offsetList, false);
                if (newAddress.ShowDialog() != DialogResult.OK)
                    return;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\n" + exception.StackTrace, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        Task<bool> pointerTask;
        CancellationTokenSource pointerSource;
        private void ScanBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (pointerTask != null && !pointerTask.IsCompleted)
                {
                    if (MessageBox.Show("Still in the pointer scanning, Do you want to stop scan?", "PointerFinder", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (pointerSource != null) pointerSource.Cancel();
                        GC.Collect();
                    }
                }
                else if (pointerResults.Count == 0 && MessageBox.Show("Perform First Scan?", "First Scan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                else if (pointerResults.Count > 0 && MessageBox.Show("Perform Next Scan?", "Next Scan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                else
                {
                    pointerItems = new List<ListViewItem>();
                    level = (int)LevelUpdown.Value;
                    maxRange = (int)MaxRangeUpDown.Value;
                    ulong queryAddress = ulong.Parse(AddressBox.Text, NumberStyles.HexNumber);
                    PointerListView.Clear();
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
                    sectionTool.InitSectionList(processName);

                    if (pointerSource != null) pointerSource.Dispose();
                    pointerSource = new CancellationTokenSource();
                    System.Diagnostics.Stopwatch tickerMajor = System.Diagnostics.Stopwatch.StartNew();
                    pointerTask = ScanTask(150, tickerMajor);
                    pointerTask.ContinueWith(t => TaskComparer(queryAddress, range, 150, tickerMajor)).ContinueWith(t => TaskCompleted(tickerMajor));
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\n" + exception.StackTrace, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void NewBtn_Click(object sender, EventArgs e)
        {
            if (pointerTask != null && !pointerTask.IsCompleted)
            {
                if (MessageBox.Show("Still in the pointer scanning, Do you want to stop scan?", "PointerFinder", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    pointerSource.Cancel();
                    GC.Collect();
                }
                else return;
            }
            ProgBar.Value = 0;
            pointerResults.Clear();
            IsInitScan.Checked = true;
            ScanBtn.Text = "First Scan";
            GC.Collect();
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

                    Section section = sectionTool.GetSection(pointerResult.pointer.AddrSID);
                    ulong baseAddress = section.Start + (ulong)pointerResult.pointer.AddrPos;
                    string msg = pointerResult.pointer.AddrPos.ToString("X");
                    List<long> offsetList = new List<long> { (long)baseAddress };
                    pointerResult.pointer.Offsets.ForEach(offset => {
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

        private void FilterSizeRuleBtn_Click(object sender, EventArgs e)
        {
            uint sectionFilterSize = Properties.Settings.Default.SectionFilterSize.Value;
            string sectionFilterSizeStr = sectionFilterSize.ToString();

            if (InputBox.Show("Section Filter", "Enter the value of the filter keys", ref sectionFilterSizeStr, null) != DialogResult.OK) return;

            Properties.Settings.Default.SectionFilterSize.Value = uint.Parse(sectionFilterSizeStr);
        }
        #endregion


        #region Task
        //Invoke(new MethodInvoker(() => { }));
        private async Task<bool> ScanTask(int nextScanCheckNumber, System.Diagnostics.Stopwatch tickerMajor) => await Task.Run(() =>
        {
            try
            {
                #region InitPathAddrs
                if (IsInitScan.Checked || pointerResults.Count > nextScanCheckNumber)
                {
                    Invoke(new MethodInvoker(() => { IsInitScan.Checked = false; }));

                    Mutex mutex = new Mutex();
                    byte MaxQueryThreads = Properties.Settings.Default.MaxQueryThreads.Value;
                    MaxQueryThreads = MaxQueryThreads == (byte)0 ? (byte)1 : MaxQueryThreads;
                    string SectionFilterKeys = Properties.Settings.Default.SectionFilterKeys.Value;
                    uint SectionFilterSize = Properties.Settings.Default.SectionFilterSize.Value;
                    uint QueryBufferSize = Properties.Settings.Default.QueryBufferSize.Value;
                    SectionFilterKeys = Regex.Replace(SectionFilterKeys, " *[,;] *", "|");
                    
                    long processedMemoryLen = 0;
                    int readCnt = 0;
                    ulong minLength = QueryBufferSize * 1024 * 1024; //set the minimum read size in bytes

                    List<Pointer> addrValueList = new List<Pointer>();
                    Section[] sections = sectionTool.GetSectionSortByAddr();
                    SemaphoreSlim semaphore = new SemaphoreSlim(MaxQueryThreads);
                    List<Task<bool>> tasks = new List<Task<bool>>();

                    bool isFilter = IsFilterBox.Checked;
                    bool isFilterSize = IsFilterSizeBox.Checked;
                    (int start, int end) rangeIdx = (-1, -1);
                    for (int sectionIdx = 0; sectionIdx < sections.Length; sectionIdx++)
                    {
                        readCnt++;
                        bool isPerform = false;
                        bool isContinue = false;
                        bool isContinuePerform = false;
                        Section checkSection = sections[sectionIdx];
                        if (!isFilter && checkSection.Name.StartsWith("libSce")) isContinue = true;
                        if (isFilter && sectionTool.SectionIsFilter(checkSection.Name, SectionFilterKeys)) isContinue = true;
                        if (isFilterSize && checkSection.Length < SectionFilterSize) isContinue = true;

                        if (isContinue)
                        {
                            if (rangeIdx.start == -1) continue; //start and end index unchanged when not scanning
                            else //start performing a scan when the start index is set
                            {
                                checkSection = sections[rangeIdx.end];
                                isContinuePerform = true;
                            }
                        }
                        if (rangeIdx.start == -1) //set start and end index when not set
                        {
                            rangeIdx.start = sectionIdx;
                            rangeIdx.end = sectionIdx;
                        }

                        Section firstSection = sections[rangeIdx.start];
                        ulong bufferSize = checkSection.Start + (ulong)checkSection.Length - firstSection.Start;
                        if (bufferSize >= int.MaxValue) isPerform = true; //check the size of the scan to be executed, whether the scan size has been reached the upper limit

                        if (!isContinuePerform && !isPerform && bufferSize < minLength && (sectionIdx != sections.Length - 1 || rangeIdx.start == -1))
                        {
                            rangeIdx.end = sectionIdx;  //update end index
                            continue;
                        }
                        //start scanning
                        if (!isContinuePerform && !isPerform) rangeIdx.end = sectionIdx;
                        var readCnt_ = readCnt;
                        var rangeIdx_ = rangeIdx;
                        tasks.Add(Task.Run<bool>(() =>
                        {
                            semaphore.Wait();
                            pointerSource.Token.ThrowIfCancellationRequested();

                            Section lastSection = sections[rangeIdx_.end];
                            if (isPerform) bufferSize = lastSection.Start + (ulong)lastSection.Length - firstSection.Start;
                            byte[] buffer = PS4Tool.ReadMemory(firstSection.PID, firstSection.Start, (int)bufferSize);
                            processedMemoryLen += buffer.Length;
                            Invoke(new MethodInvoker(() =>
                            {
                                ProgBar.Value = (int)(((float)(readCnt_) / sections.Length) * 50);
                                ToolStripMsg.Text = string.Format("Scan elapsed:{0}s. Current: {1}/{2}, read memory...{3}MB", tickerMajor.Elapsed.TotalSeconds, readCnt_, sections.Length, processedMemoryLen / (1024 * 1024));
                            }));
                            for (int idx = rangeIdx_.start; idx <= rangeIdx_.end; idx++)
                            {
                                Section addrSection = sections[idx];
                                int scanOffset = (int)(addrSection.Start - firstSection.Start);

                                List<Pointer> pointers = new List<Pointer>();
                                Section valueSection = null;
                                for (int scanIdx = scanOffset; scanIdx + 8 < scanOffset + addrSection.Length; scanIdx += 8)
                                {
                                    pointerSource.Token.ThrowIfCancellationRequested();
                                    int valueSID = 0;
                                    ulong mappedValue = BitConverter.ToUInt64(buffer, scanIdx);
                                    if (mappedValue > 0 && valueSection != null && mappedValue >= valueSection.Start && mappedValue <= (valueSection.Start + (ulong)valueSection.Length)) valueSID = valueSection.SID;
                                    else valueSID = sectionTool.GetSectionID(mappedValue);
                                    if (valueSID == -1) continue;

                                    valueSection = sectionTool.GetSection(valueSID);
                                    int startOffset = scanIdx - scanOffset;
                                    pointers.Add(new Pointer(addrSection.SID, (uint)startOffset, valueSID, (uint)(mappedValue - valueSection.Start)));
                                }
                                mutex.WaitOne();
                                addrValueList.AddRange(pointers);
                                mutex.ReleaseMutex();
                            }
                            semaphore.Release();
                            return true;
                        }));

                        if (!isPerform) rangeIdx = (-1, -1); //initialize start and end index for non-isPerform scan
                        else
                        {
                            rangeIdx.start = sectionIdx;
                            rangeIdx.end = sectionIdx;
                        }
                    }
                    Task whenTasks = Task.WhenAll(tasks);
                    whenTasks.Wait();
                    semaphore.Dispose();
                    whenTasks.Dispose();
                    GC.Collect();
                    if (addrValueList != null && addrValueList.Count > 0)
                    {
                        if (addrPointerDict != null) addrPointerDict.Clear();
                        if (valuePointerDict != null) valuePointerDict.Clear();
                        addrPointerDict = new Dictionary<int, List<Pointer>>();
                        valuePointerDict = new Dictionary<int, List<Pointer>>();

                        int tmpAddrSID = 0;
                        List<Pointer> addrPointerList = null;
                        addrValueList.Sort((p1, p2) => p1.AddrSID.CompareTo(p2.AddrSID));
                        for (int idx = 0; idx < addrValueList.Count; idx++)
                        {
                            Pointer pointer = addrValueList[idx];
                            if (tmpAddrSID == 0 || tmpAddrSID != pointer.AddrSID)
                            {
                                if (addrPointerList != null)
                                {
                                    addrPointerList.Sort((p1, p2) => p1.AddrPos.CompareTo(p2.AddrPos));
                                    addrPointerDict[tmpAddrSID] = addrPointerList;
                                }
                                if (!addrPointerDict.TryGetValue(pointer.AddrSID, out addrPointerList)) addrPointerList = new List<Pointer>();
                            }
                            addrPointerList.Add(pointer);
                            tmpAddrSID = pointer.AddrSID;
                        }

                        int tmpValueSID = 0;
                        List<Pointer> valuePointerList = null;
                        addrValueList.Sort((p1, p2) => p1.ValueSID.CompareTo(p2.ValueSID));
                        for (int idx = 0; idx < addrValueList.Count; idx++)
                        {
                            Pointer pointer = addrValueList[idx];
                            if (tmpValueSID == 0 || tmpValueSID != pointer.ValueSID)
                            {
                                if (valuePointerList != null)
                                {
                                    valuePointerList.Sort((p1, p2) => p1.ValuePos.CompareTo(p2.ValuePos));
                                    valuePointerDict[tmpValueSID] = valuePointerList;
                                }
                                if (!valuePointerDict.TryGetValue(pointer.ValueSID, out valuePointerList)) valuePointerList = new List<Pointer>();
                            }
                            valuePointerList.Add(pointer);
                            tmpValueSID = pointer.ValueSID;
                        }
                        addrValueList = null;
                    }
                    GC.Collect();
                }
                #endregion
            }
            catch (Exception exception)
            {
                if (exception.InnerException is OperationCanceledException)
                {
                    Invoke(new MethodInvoker(() => {
                        ProgBar.Value = 100;
                        IsInitScan.Checked = true;
                        ToolStripMsg.Text = string.Format("Scan elapsed:{0}s. ScanTask canceled. {1}", tickerMajor.Elapsed.TotalSeconds, exception.InnerException.Message);
                    }));
                }
                else MessageBox.Show(exception.Message + "\n" + exception.StackTrace, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
            }
            return true;
        });

        private bool TaskComparer(ulong queryAddress, List<int> range, int nextScanCheckNumber, System.Diagnostics.Stopwatch tickerMajor)
        {
            #region Comparer
            if (pointerResults.Count == 0)
            {
                Invoke(new MethodInvoker(() =>
                {
                    ProgBar.Value = 80;
                    ToolStripMsg.Text = string.Format("Scan elapsed:{0}s. perform first query...", tickerMajor.Elapsed.TotalSeconds);
                }));

                if (sectionTool.GetSectionID(queryAddress) is int addrSID && addrSID == -1) return true;

                Section section = sectionTool.GetSection(addrSID);
                Pointer queryPointer = new Pointer(addrSID, (uint)(queryAddress - section.Start), 0, 0);

                QueryPointerFirst(queryPointer, range, tickerMajor);
            }
            else
            {
                Dictionary<ulong, ulong> pointerMemoryCaches = new Dictionary<ulong, ulong>();
                var newPointerResults = new List<((int AddrSID, uint AddrPos, List<long> Offsets) pointer, List<Pointer> pathAddress)>();
                for (int pIdx = 0; pIdx < pointerResults.Count; ++pIdx)
                {
                    pointerSource.Token.ThrowIfCancellationRequested();
                    if (pIdx % 1000 == 0)
                    {
                        Invoke(new MethodInvoker(() =>
                        {
                            ProgBar.Value = 50 + (int)(((float)(pIdx + 1) / pointerResults.Count) * 50);
                            ToolStripMsg.Text = string.Format("Scan elapsed:{0}s. Compare pointer results: {1}/{2}, mapped memory...", tickerMajor.Elapsed.TotalSeconds, pIdx + 1, pointerResults.Count);
                        }));
                    }

                    var pointerResult = pointerResults[pIdx];
                    var tailAddress = pointerResults.Count > nextScanCheckNumber ? ReadTailAddressByPathListValue(pointerResult) : ReadTailAddress(pointerResult, pointerMemoryCaches);
                    if (tailAddress != queryAddress) continue;

                    newPointerResults.Add(pointerResult);
                    if (newPointerResults.Count < 20000 && pointerResult.pointer.Offsets.Count > 0) AddPointerListViewItem(newPointerResults.Count - 1, pointerResult.pathAddress, pointerResult.pointer.Offsets);
                }

                if (newPointerResults.Count > 0 ||
                    newPointerResults.Count == 0 && MessageBox.Show("Whether to continue?", "Next Scan results are zero", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    pointerResults.Clear();
                    pointerResults = newPointerResults;
                }
            }
            #endregion
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryPointer"></param>
        /// <param name="range"></param>
        /// <param name="tickerMajor"></param>
        /// <param name="level"></param>
        /// <param name="pathOffset"></param>
        /// <param name="pathAddress"></param>
        private void QueryPointerFirst(Pointer queryPointer, List<int> range, System.Diagnostics.Stopwatch tickerMajor, int level = 0, List<long> pathOffset = null, List<Pointer> pathAddress = null)
        {
            pointerSource.Token.ThrowIfCancellationRequested();
            if (level >= range.Count) return;
            if (!addrPointerDict.TryGetValue(queryPointer.AddrSID, out List<Pointer> addrPointerList) || addrPointerList.Count == 0) return;
            
            Section[] sections = sectionTool.GetSectionSortByAddr(queryPointer.AddrSID, out int sIdx, new List<int>(addrPointerDict.Keys));
            if (sIdx == -1) return;

            Section section = sections[sIdx];

            ulong queryAddress = queryPointer.AddrPos + section.Start;

            if (BinarySearchByAddress(addrPointerList, queryPointer.AddrPos, 0, addrPointerList.Count - 1) is int hitIdx && hitIdx == -1) return;

            if (pathOffset == null) pathOffset = new List<long>();
            if (pathAddress == null) pathAddress = new List<Pointer>();

            int counter = 0;
            bool isBreak = false;
            bool isFastScan = FastScanBox.Checked;
            bool isNegativeOffset = NegativeOffsetBox.Checked;
            for (int sectionIdx = sIdx; isNegativeOffset ? sectionIdx < sections.Length : sectionIdx >= 0; sectionIdx += isNegativeOffset ? 1 : -1)
            {
                section = sectionIdx != sIdx ? sections[sectionIdx] : section;
                if (sectionIdx != sIdx && (!addrPointerDict.TryGetValue(section.SID, out addrPointerList) || addrPointerList.Count == 0)) continue;
                for (int addrIdx = sectionIdx == sIdx ? hitIdx : (isNegativeOffset ? 0 : addrPointerList.Count - 1); isNegativeOffset ? addrIdx < addrPointerList.Count : addrIdx >= 0; addrIdx += isNegativeOffset ? 1 : -1)
                {
                    pointerSource.Token.ThrowIfCancellationRequested();
                    Pointer addrPointer = addrPointerList[addrIdx];
                    ulong checkAddr = addrPointer.AddrPos + section.Start;
                    if (range[level] > 0 && (long)checkAddr + range[level] < (long)queryAddress) break;
                    else if (range[level] < 0 && (long)checkAddr + range[level] > (long)queryAddress) break;

                    List<Pointer> pointerList = GetPointerListByValue(addrPointer);
                    if (pointerList.Count == 0) continue;

                    pathOffset.Add((long)(queryAddress - checkAddr));
                    const int maxPointerCount = 15;
                    int curPointerCounter = 0;
                    bool inNewLevel = false;
                    for (int j = 0; j < pointerList.Count; ++j)
                    {
                        bool inStack = false;
                        for (int k = 0; k < pathAddress.Count; ++k)
                        {
                            pointerSource.Token.ThrowIfCancellationRequested();
                            if (pathAddress[k].ValuePos != pointerList[j].ValuePos && pathAddress[k].AddrPos != pointerList[j].AddrPos) continue;
                            inStack = true;
                            break;
                        }
                        if (inStack) continue;

                        inNewLevel = true;
                        if (curPointerCounter >= maxPointerCount) break;

                        ++curPointerCounter;

                        pathAddress.Add(pointerList[j]);
                        QueryPointerFirst(pointerList[j], range, tickerMajor, level + 1, pathOffset, pathAddress);
                        pathAddress.RemoveAt(pathAddress.Count - 1);
                    }

                    pathOffset.RemoveAt(pathOffset.Count - 1);

                    if (counter >= 1)
                    {
                        isBreak = true;
                        break;
                    }
                    if (inNewLevel) ++counter;
                }
                if (isBreak) break;
            }

            pointerSource.Token.ThrowIfCancellationRequested();
            if (pathAddress == null || pathAddress.Count == 0) return;

            Section addrSection = sectionTool.GetSection(pathAddress[pathOffset.Count - 1].AddrSID);
            if (addrSection == null) return;
            if (isFastScan && !addrSection.Name.StartsWith("executable")) return;

            uint pointerPosition = pathAddress[pathOffset.Count - 1].AddrPos;

            ((int AddrSID, uint AddrPos, List<long> Offsets) pointer, List<Pointer> pathAddress) pointerResult = ((addrSection.SID, pointerPosition, new List<long>()), new List<Pointer>(pathAddress));

            for (int i = pathOffset.Count - 1; i >= 0; --i) pointerResult.pointer.Offsets.Add(pathOffset[i]);

            pointerResults.Add(pointerResult);

            if (pointerResults.Count < 20000 && pathOffset.Count > 0) AddPointerListViewItem(pointerResults.Count - 1, pathAddress, pathOffset);
            if (pointerResults.Count % 1024 == 0)
            {
                Invoke(new MethodInvoker(() =>
                {
                    ToolStripMsg.Text = string.Format("Scan elapsed:{0}s. first query_, {1} results...level:{2}", tickerMajor.Elapsed.TotalSeconds, pointerResults.Count, level);
                }));
            }
        }

        private bool TaskCompleted(System.Diagnostics.Stopwatch tickerMajor)
        {
            tickerMajor.Stop();
            Invoke(new MethodInvoker(() => {
                ProgBar.Value = 100;
                ToolStripMsg.Text = string.Format("Scan elapsed:{0}s. Query pointer end, find:{1}", tickerMajor.Elapsed.TotalSeconds, pointerItems.Count);
                if (pointerItems.Count > 0)
                {
                    PointerListView.Items.AddRange(pointerItems.ToArray());
                    pointerItems.Clear();
                    ScanBtn.Text = "Next Scan";
                }
                else if (pointerResults.Count == 0) ScanBtn.Text = "First Scan";
                else if (pointerResults.Count > 0) ScanBtn.Text = "Next Scan";
            }));
            if (pointerSource != null) pointerSource.Dispose();
            if (pointerTask != null) pointerTask.Dispose();
            GC.Collect();
            return true;
        }

        private void AddPointerListViewItem(int idx, List<Pointer> pathAddress, List<long> pathOffset)
        {
            int itemIdx = PointerListView.Items.Count;
            Pointer pointer = pathAddress[pathAddress.Count - 1];
            Section section = sectionTool.GetSection(pointer.AddrSID);
            string sectionStr = $"{section.Start.ToString("X9")}-{section.Name}-{section.Prot.ToString("X")}-{section.Length / 1024}KB";

            ListViewItem item = new ListViewItem((pointer.AddrPos + section.Start).ToString("X"), 0); //Base Address
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

        /// <summary>
        /// get all pointers with the same address as the input from the list of value Pointers
        /// </summary>
        /// <param name="addrPointer"></param>
        /// <returns>pointers with the same address as the input</returns>
        private List<Pointer> GetPointerListByValue(Pointer addrPointer)
        {
            List<Pointer> resultPointers = new List<Pointer>();

            if (!valuePointerDict.TryGetValue(addrPointer.AddrSID, out List<Pointer> valuePointerList)) return resultPointers;

            int index = BinarySearchByValue(valuePointerList, addrPointer.AddrPos);

            if (index < 0) return resultPointers;

            int start = index;
            for (; start >= 0; --start) if (valuePointerList[start].ValuePos != addrPointer.AddrPos) break;

            bool find = false;
            for (int i = start; i < valuePointerList.Count; ++i)
            {
                Pointer valuePointer = valuePointerList[i];
                if (valuePointer.ValuePos == addrPointer.AddrPos)
                {
                    find = true;
                    resultPointers.Add(valuePointer);
                }
                else if (find) break;
            }

            return resultPointers;
        }

        /// <summary>
        /// get the index of the closest address as the input address from the address pointer list(sorted by address)
        /// </summary>
        /// <param name="addrPointerList">list of address pointers with the same SID as the query address(sorted by address)</param>
        /// <param name="addrPos">query address position</param>
        /// <param name="low">minimum index of list</param>
        /// <param name="high">maximum index of the list</param>
        /// <returns>index of the closest address from the address pointer list</returns>
        private int BinarySearchByAddress(List<Pointer> addrPointerList, uint addrPos, int low, int high)
        {
            int mid = (low + high) / 2;
            if (low > high) return -1;
            else
            {
                if (addrPointerList[mid].AddrPos == addrPos) return mid;
                else if (addrPointerList[mid].AddrPos > addrPos)
                {
                    if (mid - 1 >= 0 && addrPointerList[mid - 1].AddrPos <= addrPos) return mid - 1;
                    return BinarySearchByAddress(addrPointerList, addrPos, low, mid - 1);
                }
                else
                {
                    if (mid + 1 < addrPointerList.Count && addrPointerList[mid + 1].AddrPos >= addrPos) return mid + 1;
                    return BinarySearchByAddress(addrPointerList, addrPos, mid + 1, high);
                }
            }
        }

        /// <summary>
        /// get the index of the same address as the input address from the value pointer list(sorted by value)
        /// </summary>
        /// <param name="valuePointerList">list of value pointers with the same SID as the query address(sorted by value)</param>
        /// <param name="addrPos">query address position</param>
        /// <returns>index of the same address from the value pointer list</returns>
        private int BinarySearchByValue(List<Pointer> valuePointerList, ulong addrPos)
        {
            int low = 0;
            int high = valuePointerList.Count - 1;
            int middle;

            while (low <= high)
            {
                middle = (low + high) / 2;
                if (addrPos > valuePointerList[middle].ValuePos) low = middle + 1;
                else if (addrPos < valuePointerList[middle].ValuePos) high = middle - 1;
                else return middle;
            }

            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointerResult"></param>
        /// <returns></returns>
        private ulong ReadTailAddressByPathListValue(((int AddrSID, uint AddrPos, List<long> Offsets) pointer, List<Pointer> pathAddress) pointerResult)
        {
            ulong targetAddr = 0;
            Section addrSection = sectionTool.GetSection(pointerResult.pointer.AddrSID);
            if (addrSection == null) return targetAddr;
            if (!addrPointerDict.TryGetValue(addrSection.SID, out List<Pointer> addrPointerList)) return targetAddr;

            List<long> offsetList = new List<long>();
            offsetList.Add((long)pointerResult.pointer.AddrPos); //offsetList.Add((long)(addrSection.Start + pointerResult.pointer.position));
            offsetList.AddRange(pointerResult.pointer.Offsets);

            ulong headAddrPos = 0;
            for (int idx = 0; idx < offsetList.Count; ++idx)
            {
                long offset = offsetList[idx];
                if (idx != offsetList.Count - 1)
                {
                    int pathAddrIdx = BinarySearchByAddress(addrPointerList, (uint)((ulong)offset + headAddrPos), 0, addrPointerList.Count - 1); //BinarySearchByAddress(addrPointerList, (uint)((ulong)offset + headAddrPos - addrSection.Start), 0, addrPointerList.Count - 1);
                    if (pathAddrIdx == -1) break;

                    Pointer pointer = addrPointerList[pathAddrIdx];
                    if (pointer.ValueSID != addrSection.SID)
                    { //FIXME Check
                        if (!addrPointerDict.TryGetValue(pointer.ValueSID, out addrPointerList)) break;
                        addrSection = sectionTool.GetSection(pointer.ValueSID);
                    }
                    headAddrPos = pointer.ValuePos;// + addrSection.Start;
                }
                else targetAddr = (ulong)offset + headAddrPos;
            }

            return targetAddr + addrSection.Start;
        }

        private ulong ReadTailAddress(((int AddrSID, uint AddrPos, List<long> Offsets) pointer, List<Pointer> pathAddress) pointerResult, in Dictionary<ulong, ulong> pointerMemoryCaches)
        {
            Section section = sectionTool.GetSection(pointerResult.pointer.AddrSID);
            if (section == null) return 0;

            var targetAddr = PS4Tool.ReadTailAddress(section.PID, section.Start + pointerResult.pointer.AddrPos, pointerResult.pointer.Offsets, pointerMemoryCaches);
            return targetAddr;
        }
        #endregion

        #region PointerStruct
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct Pointer
        {
            public int AddrSID;
            public uint AddrPos;
            public int ValueSID;
            public uint ValuePos;

            public Pointer(int addrSID, uint addrPos, int valueSID, uint valuePos)
            {
                AddrSID = addrSID;
                AddrPos = addrPos;
                ValueSID = valueSID;
                ValuePos = valuePos;
            }
        }
        #endregion
    }
}
