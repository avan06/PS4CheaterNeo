using System;
using System.Collections.Generic;
using System.Drawing;
using libdebug;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using static PS4CheaterNeo.SectionTool;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;

namespace PS4CheaterNeo
{
    public partial class Query : Form
    {
        readonly Main mainForm;
        readonly SectionTool sectionTool;
        ComparerTool comparerTool;
        Dictionary<int, BitsDictionary> bitsDictDict;
        public Query(Main mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            sectionTool = new SectionTool();
            bitsDictDict = new Dictionary<int, BitsDictionary>();
            IsFilterBox.Checked = Properties.Settings.Default.EnableFilterQuery.Value;
            IsFilterSizeBox.Checked = Properties.Settings.Default.EnableFilterSizeQuery.Value;
        }

        #region Event
        private void Query_Load(object sender, EventArgs e)
        {
            foreach (ScanType filterEnum in (ScanType[])Enum.GetValues(typeof(ScanType)))
                ScanTypeBox.Items.Add(new ComboboxItem(filterEnum.GetDescription(), filterEnum));
            ScanTypeBox.SelectedIndex = 2;
            if (Properties.Settings.Default.AutoPerformGetProcesses.Value) GetProcessesBtn.PerformClick();
        }

        private void Query_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ResultView.Items.Count > 0 && MessageBox.Show("Still in the query, Do you want to close Query?", "Query", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }

            PS4Tool.DetachDebugger();
            bitsDictDict = null;
            GC.Collect();
            Properties.Settings.Default.Save();
        }

        private void GetProcessesBtn_Click(object sender, EventArgs e)
        {
            try
            {
                //if (Properties.Settings.Default.DebugMode.Value)
                //{
                //    ProcessesBox.SelectedIndex = ProcessesBox.Items.Add(new ComboboxItem("Debug", 999));
                //    return;
                //}
                if (!PS4Tool.Connect(Properties.Settings.Default.PS4IP.Value, out string msg)) throw new Exception(msg);

                int selectedIdx = 0;
                string DefaultProcess = Properties.Settings.Default.DefaultProcess.Value;
                ProcessesBox.Items.Clear();
                ProcessList procList = PS4Tool.GetProcessList();
                for (int pIdx = 0; pIdx < procList.processes.Length; pIdx++)
                {
                    Process process = procList.processes[pIdx];
                    int idx = ProcessesBox.Items.Add(new ComboboxItem(process.name, process.pid));
                    if (process.name == DefaultProcess) selectedIdx = idx;
                }
                ProcessesBox.SelectedIndex = selectedIdx;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\n" + exception.StackTrace, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void ProcessesBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SectionView.Items.Clear();
                ResultView.Items.Clear();

                ComboboxItem process = (ComboboxItem)ProcessesBox.SelectedItem;
                sectionTool.InitSectionList((int)process.Value, (string)process.Text);
                mainForm.ProcessName = (string)process.Text;

                Section[] sections = sectionTool.GetSectionSortByAddr();

                SectionView.BeginUpdate();
                for (int sectionIdx = 0; sectionIdx < sections.Length; sectionIdx++)
                {
                    Section section = sections[sectionIdx];
                    if ((IsFilterBox.Checked && section.IsFilter) || (IsFilterSizeBox.Checked && section.IsFilterSize)) continue;

                    string start = String.Format("{0:X9}", section.Start);
                    int itemIdx = SectionView.Items.Count;
                    SectionView.Items.Add(sectionIdx.ToString(), start, 0);
                    SectionView.Items[itemIdx].SubItems.Add(section.Name);
                    SectionView.Items[itemIdx].SubItems.Add(section.Prot.ToString("X"));
                    SectionView.Items[itemIdx].SubItems.Add((section.Length / 1024).ToString() + "KB");
                    SectionView.Items[itemIdx].SubItems.Add(section.SID.ToString());
                    if (section.Offset != 0) SectionView.Items[itemIdx].SubItems.Add(section.Offset.ToString("X"));
                    else SectionView.Items[itemIdx].SubItems.Add("");
                    SectionView.Items[itemIdx].SubItems.Add((section.Start + (ulong)section.Length).ToString("X9"));
                    if (section.IsFilter)
                    {
                        SectionView.Items[itemIdx].Tag = "filter";
                        SectionView.Items[itemIdx].ForeColor = Color.DarkGray;
                        SectionView.Items[itemIdx].BackColor = Color.DimGray;
                    }
                    else if (section.IsFilterSize)
                    {
                        SectionView.Items[itemIdx].Tag = "filterSize";
                        SectionView.Items[itemIdx].ForeColor = Color.DarkCyan;
                        SectionView.Items[itemIdx].BackColor = Color.DarkSlateGray;
                    }
                    else if (section.Name.StartsWith("executable")) SectionView.Items[itemIdx].ForeColor = Color.GreenYellow;
                    else if (section.Name.Contains("NoName")) SectionView.Items[itemIdx].ForeColor = Color.Red;
                    else if (Regex.IsMatch(section.Name, @"^\[\d+\]$")) SectionView.Items[itemIdx].ForeColor = Color.HotPink;
                }
                SectionView.EndUpdate();
            }
            catch (Exception exception)
            {
                if (!exception.Message.Contains("Map is null")) MessageBox.Show(exception.Message + "\n" + exception.StackTrace, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void NewBtn_Click(object sender, EventArgs e)
        {
            if (scanTask != null && !scanTask.IsCompleted)
            {
                if (MessageBox.Show("Still in the scanning, Do you want to stop scan?", "Scan",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) scanSource.Cancel();
                else return;
            }

            ResultView.Items.Clear();
            if (bitsDictDict != null) bitsDictDict.Clear();
            bitsDictDict = new Dictionary<int, BitsDictionary>();
            GC.Collect();

            ScanBtn.Text = "First Scan";
            ScanTypeBox.Enabled = true;
            AlignmentBox.Enabled = true;
            NewBtn.Enabled = false;
        }

        Task<bool> scanTask;
        CancellationTokenSource scanSource;
        private void ScanBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (scanTask != null && !scanTask.IsCompleted)
                {
                    if (MessageBox.Show("Still in the scanning, Do you want to stop scan?", "Scan",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) scanSource.Cancel();
                }
                else if (ResultView.Items.Count == 0 && MessageBox.Show("search size:" + (sectionTool.TotalMemorySize / (1024 * 1024)).ToString() + "MB", "First Scan",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                else
                {
                    ComboboxItem process = (ComboboxItem)ProcessesBox.SelectedItem;
                    int pid = (int)process.Value;

                    string value0 = ValueBox.Text;
                    string value1 = Value1Box.Text;
                    bool alignment = AlignmentBox.Checked;
                    bool isFilter = IsFilterBox.Checked;
                    bool isFilterSize = IsFilterSizeBox.Checked;
                    Enum.TryParse(((ComboboxItem)(ScanTypeBox.SelectedItem)).Value.ToString(), out ScanType scanType);
                    Enum.TryParse(CompareTypeBox.SelectedItem.ToString(), out CompareType compareType);

                    ulong AddrMin = ulong.Parse(AddrMinBox.Text, NumberStyles.HexNumber);
                    ulong AddrMax = ulong.Parse(AddrMaxBox.Text, NumberStyles.HexNumber);
                    if (AddrMin > AddrMax && MessageBox.Show(String.Format("AddrMin({1:X}) > AddrMax({0:X})", AddrMin, AddrMax), "Scan", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK) return;

                    comparerTool = new ComparerTool(scanType, compareType, value0, value1);

                    ScanBtn.Text = "Stop";
                    if (scanSource != null) scanSource.Dispose();
                    scanSource = new CancellationTokenSource();
                    scanTask = ScanTask(alignment, isFilter, isFilterSize, AddrMin, AddrMax);
                    scanTask.ContinueWith(t => TaskCompleted());

                    ScanTypeBox.Enabled = false;
                    AlignmentBox.Enabled = false;
                    NewBtn.Enabled = true;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\n" + exception.StackTrace, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        //Invoke(new MethodInvoker(() => { }));
        private async Task<bool> ScanTask(bool alignment, bool isFilter, bool isFilterSize, ulong AddrMin, ulong AddrMax) => await Task.Run(() =>
        {
            System.Diagnostics.Stopwatch tickerMajor = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                ulong hitCnt = 0;
                int scanStep = (comparerTool.scanType == ScanType.Hex || comparerTool.scanType == ScanType.String_) ? 1 :
                    alignment ? (comparerTool.scanTypeLength > 4 ? 4 : comparerTool.scanTypeLength) : 1;
                
                Invoke(new MethodInvoker(() => { ToolStripBar.Value = 1; }));

                Mutex mutex = new Mutex();
                byte MaxQueryThreads = Properties.Settings.Default.MaxQueryThreads.Value;
                uint QueryBufferSize = Properties.Settings.Default.QueryBufferSize.Value;
                uint SectionFilterSize = Properties.Settings.Default.SectionFilterSize.Value;
                string SectionFilterKeys = Properties.Settings.Default.SectionFilterKeys.Value;
                MaxQueryThreads = MaxQueryThreads == (byte)0 ? (byte)1 : MaxQueryThreads;
                SectionFilterKeys = Regex.Replace(SectionFilterKeys, " *[,;] *", "|");

                int readCnt = 0;
                long processedMemoryLen = 0;
                ulong minLength = QueryBufferSize * 1024 * 1024; //set the minimum read size in bytes

                Section[] sectionKeys = sectionTool.GetSectionSortByAddr();
                SemaphoreSlim semaphore = new SemaphoreSlim(MaxQueryThreads);
                List<Task<bool>> tasks = new List<Task<bool>>();

                (int start, int end) rangeIdx = (-1, -1);
                for (int sectionIdx = 0; sectionIdx < sectionKeys.Length; sectionIdx++)
                {
                    readCnt++;
                    bool isPerform = false;
                    bool isContinue = false;
                    bool isContinuePerform = false;
                    Section checkSection = sectionKeys[sectionIdx];
                    if (!checkSection.Check || isFilter && checkSection.IsFilter || isFilterSize && checkSection.IsFilterSize ||
                    checkSection.Start + (ulong)checkSection.Length < AddrMin || checkSection.Start > AddrMax) isContinue = true; //Check if section is not scanned

                    if (isContinue)
                    {
                        if (rangeIdx.start == -1) continue; //start and end index unchanged when not scanning
                        else //start performing a scan when the start index is set
                        {
                            checkSection = sectionKeys[rangeIdx.end];
                            isContinuePerform = true;
                        }
                    }
                    if (rangeIdx.start == -1) //set start and end index when not set
                    {
                        rangeIdx.start = sectionIdx;
                        rangeIdx.end = sectionIdx;
                    }

                    Section firstSection = sectionKeys[rangeIdx.start];
                    ulong bufferSize = checkSection.Start + (ulong)checkSection.Length - firstSection.Start;
                    if (bufferSize >= int.MaxValue) isPerform = true; //check the size of the scan to be executed, whether the scan size has been reached the upper limit

                    if (!isContinuePerform && !isPerform && bufferSize < minLength && (sectionIdx != sectionKeys.Length - 1 || rangeIdx.start == -1))
                    {
                        rangeIdx.end = sectionIdx; //update end index
                        continue;
                    }
                    //start scanning
                    if (!isContinuePerform && !isPerform) rangeIdx.end = sectionIdx;
                    var rangeIdx_ = rangeIdx;
                    tasks.Add(Task.Run<bool>(() =>
                    {
                        semaphore.Wait();
                        scanSource.Token.ThrowIfCancellationRequested();

                        Section lastSection = sectionKeys[rangeIdx_.end];
                        if (isPerform) bufferSize = lastSection.Start + (ulong)lastSection.Length - firstSection.Start;
                        byte[] buffer = PS4Tool.ReadMemory(firstSection.PID, firstSection.Start, (int)bufferSize);
                        for (int idx = rangeIdx_.start; idx <= rangeIdx_.end; idx++)
                        {
                            Section addrSection = sectionKeys[idx];
                            int scanOffset = (int)(addrSection.Start - firstSection.Start);

                            scanSource.Token.ThrowIfCancellationRequested();
                            Byte[] subBuffer = new Byte[addrSection.Length];
                            Buffer.BlockCopy(buffer, (int)scanOffset, subBuffer, 0, addrSection.Length);
                            BitsDictionary bitsDict = comparerTool.groupTypes == null ? Comparer(subBuffer, addrSection, scanStep, AddrMin, AddrMax) : ComparerGroup(subBuffer, addrSection, scanStep, AddrMin, AddrMax);

                            if (bitsDict != null && bitsDict.Count > 0)
                            {
                                hitCnt += (ulong)bitsDict.Count;
                                mutex.WaitOne();
                                bitsDictDict[addrSection.SID] = bitsDict;
                                mutex.ReleaseMutex();
                            }
                            else addrSection.Check = false;

                            processedMemoryLen += addrSection.Length;
                            Invoke(new MethodInvoker(() =>
                            {
                                ToolStripBar.Value = (int)(((float)processedMemoryLen / sectionTool.TotalMemorySize) * 100);
                                ToolStripMsg.Text = string.Format("Scan elapsed:{0}s. {1}MB, Count: {2}", tickerMajor.Elapsed.TotalSeconds, processedMemoryLen / (1024 * 1024), hitCnt);
                            }));
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
                Invoke(new MethodInvoker(() => {
                    for (int sectionIdx = 0; sectionIdx < SectionView.Items.Count; ++sectionIdx)
                    {
                        ListViewItem sectionItem = SectionView.Items[sectionIdx];
                        if (!sectionItem.Checked) continue;

                        int sid = int.Parse(sectionItem.SubItems[(int)SectionCol.SectionViewSID].Text);
                        Section section = sectionTool.GetSection(sid);
                        if (section.Check == false) sectionItem.Checked = false;
                    }
                    ToolStripBar.Value = 100;
                }));

                GC.Collect();
            }
            catch (Exception exception)
            {
                if (exception.InnerException is OperationCanceledException)
                {
                    Invoke(new MethodInvoker(() => {
                        ToolStripBar.Value = 100;
                        ToolStripMsg.Text = string.Format("Scan elapsed:{0}s. ScanTask canceled. {1}", tickerMajor.Elapsed.TotalSeconds, exception.InnerException.Message);
                    }));
                }
                else MessageBox.Show(exception.Message + "\n" + exception.StackTrace, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                if (scanSource != null) scanSource.Dispose();
                tickerMajor.Stop();
            }
            return true;
        });

        private BitsDictionary Comparer(Byte[] buffer, Section section, int scanStep, ulong AddrMin, ulong AddrMax)
        {
            if (!bitsDictDict.TryGetValue(section.SID, out BitsDictionary bitsDict)) bitsDict = new BitsDictionary(scanStep, comparerTool.scanTypeLength);
            if (ResultView.Items.Count == 0)
            {
                for (int scanIdx = 0; scanIdx + comparerTool.scanTypeLength < buffer.LongLength; scanIdx += scanStep)
                {
                    if (section.Start + (ulong)scanIdx < AddrMin || section.Start + (ulong)scanIdx > AddrMax) continue;
                    if (scanSource.Token.IsCancellationRequested) break;

                    byte[] newValue = new byte[comparerTool.scanTypeLength];
                    Buffer.BlockCopy(buffer, scanIdx, newValue, 0, comparerTool.scanTypeLength);
                    if (comparerTool.value0Byte == null)
                    {
                        ulong longValue = ScanTool.BytesToULong(newValue);
                        if (ScanTool.Comparer(comparerTool, longValue, 0)) bitsDict.Add((uint)scanIdx, newValue);
                    }
                    else if (ScanTool.ComparerExact(comparerTool.scanType, newValue, comparerTool.value0Byte)) bitsDict.Add((uint)scanIdx, newValue);
                }
            }
            else
            {
                BitsDictionary newBitsDict = new BitsDictionary(scanStep, comparerTool.scanTypeLength);

                bitsDict.Begin();
                for (int idx = 0; idx < bitsDict.Count; idx++)
                {
                    if (scanSource.Token.IsCancellationRequested) break;

                    (uint offsetAddr, byte[] oldBytes) = bitsDict.Get();
                    ulong address = section.Start + offsetAddr;
                    if (address < AddrMin || address > AddrMax) continue;

                    ulong oldData = ScanTool.BytesToULong(oldBytes);
                    byte[] newValue = new byte[comparerTool.scanTypeLength];
                    Buffer.BlockCopy(buffer, (int)offsetAddr, newValue, 0, comparerTool.scanTypeLength);

                    if (comparerTool.value0Byte == null)
                    {
                        ulong newData = ScanTool.BytesToULong(newValue);
                        if (ScanTool.Comparer(comparerTool, newData, oldData)) newBitsDict.Add(offsetAddr, newValue);
                    }
                    else if (ScanTool.ComparerExact(comparerTool.scanType, newValue, comparerTool.value0Byte)) newBitsDict.Add(offsetAddr, newValue);
                }
                bitsDict.Clear();
                bitsDict = newBitsDict;
            }
            return bitsDict;
        }

        private BitsDictionary ComparerGroup(Byte[] buffer, Section section, int scanStep, ulong AddrMin, ulong AddrMax)
        {
            if (!bitsDictDict.TryGetValue(section.SID, out BitsDictionary bitsDict)) bitsDict = new BitsDictionary(scanStep, comparerTool.scanTypeLength);
            if (ResultView.Items.Count == 0)
            {
                for (int scanIdx = 0; scanIdx + comparerTool.groupFirstLength < buffer.LongLength; scanIdx += scanStep)
                {
                    if (scanSource.Token.IsCancellationRequested) break;
                    if (section.Start + (ulong)scanIdx < AddrMin || section.Start + (ulong)scanIdx > AddrMax) continue;

                    int firstScanIdx = scanIdx;
                    for (int gIdx = 0; gIdx < comparerTool.groupTypes.Count; gIdx++)
                    {
                        (ScanType scanType, int groupTypeLength, bool isAny) = comparerTool.groupTypes[gIdx];
                        if (scanIdx + groupTypeLength > buffer.LongLength) break;
                        bool comparer = false;
                        if (!isAny)
                        {
                            byte[] valueBytes = comparerTool.groupValues[gIdx];
                            byte[] newGroupBytes = new byte[groupTypeLength];
                            Buffer.BlockCopy(buffer, scanIdx, newGroupBytes, 0, groupTypeLength);
                            comparer = ScanTool.ComparerExact(scanType, newGroupBytes, valueBytes);
                        }

                        if (!isAny && !comparer)
                        {
                            if (scanIdx > firstScanIdx) scanIdx = firstScanIdx;
                            break;
                        }
                        else
                        {
                            if (gIdx == comparerTool.groupTypes.Count - 1)
                            {
                                byte[] newBytes = new byte[comparerTool.scanTypeLength];
                                Buffer.BlockCopy(buffer, firstScanIdx, newBytes, 0, comparerTool.scanTypeLength);
                                bitsDict.Add((uint)firstScanIdx, newBytes);
                            }
                            scanIdx += groupTypeLength;
                        }
                    }
                }
            }
            else
            {
                BitsDictionary newBitsDict = new BitsDictionary(scanStep, comparerTool.scanTypeLength);

                bitsDict.Begin();
                for (int idx = 0; idx < bitsDict.Count; idx++)
                {
                    if (scanSource.Token.IsCancellationRequested) break;
                    (uint offsetAddr, _) = bitsDict.Get();
                    if (section.Start + offsetAddr < AddrMin || section.Start + offsetAddr > AddrMax) continue;

                    byte[] newBytes = new byte[comparerTool.scanTypeLength];
                    Buffer.BlockCopy(buffer, (int)offsetAddr, newBytes, 0, comparerTool.scanTypeLength);

                    int scanOffset = 0;
                    for (int gIdx = 0; gIdx < comparerTool.groupTypes.Count; gIdx++)
                    {
                        (ScanType scanType, int groupTypeLength, bool isAny) = comparerTool.groupTypes[gIdx];
                        byte[] valueBytes = comparerTool.groupValues[gIdx];
                        byte[] newGroupBytes = new byte[groupTypeLength];
                        Buffer.BlockCopy(newBytes, scanOffset, newGroupBytes, 0, groupTypeLength);
                        if (!isAny && !ScanTool.ComparerExact(scanType, newGroupBytes, valueBytes)) break;
                        else if (gIdx == comparerTool.groupTypes.Count - 1) newBitsDict.Add(offsetAddr, newBytes);
                        scanOffset += groupTypeLength;
                    }
                }
                bitsDict.Clear();
                bitsDict = newBitsDict;
            }

            return bitsDict;
        }

        private void TaskCompleted()
        {
            if (bitsDictDict.Count <= 0)
            {
                Invoke(new MethodInvoker(() => { NewBtn.PerformClick(); }));
                return;
            }

            int hitCnt = 0;
            uint MaxResultShow = Properties.Settings.Default.MaxResultShow.Value;
            MaxResultShow = MaxResultShow == 0 ? 0x2000 : MaxResultShow;
            Invoke(new MethodInvoker(() =>
            {
                ResultView.Items.Clear();
                ResultView.BeginUpdate();
            }));
            List<int> sectionKeys = new List<int>(bitsDictDict.Keys);
            sectionKeys.Sort();
            Color backColor = default;
            for (int sectionIdx = 0; sectionIdx < sectionKeys.Count; sectionIdx++)
            {
                Section section = sectionTool.SectionDict[sectionKeys[sectionIdx]];
                bitsDictDict.TryGetValue(section.SID, out BitsDictionary bitsDict);
                if (bitsDict == null || bitsDict.Count == 0) continue;

                bitsDict.Begin();
                for (int idx = 0; idx < bitsDict.Count; idx++)
                {
                    if (++hitCnt > MaxResultShow) break;

                    (uint offsetAddr, byte[] oldBytes) = bitsDict.Get();

                    if (comparerTool.scanType != ScanType.Group)
                    {
                        string typeStr = comparerTool.scanType.GetDescription();
                        string valueStr = ScanTool.BytesToString(comparerTool.scanType, oldBytes);
                        string valueHex = ScanTool.BytesToString(comparerTool.scanType, oldBytes, true);

                        Invoke(new MethodInvoker(() => {
                            int itemIdx = ResultView.Items.Count;
                            ResultView.Items.Add(offsetAddr.ToString("X8"), (offsetAddr + section.Start).ToString("X8"), 0);
                            ResultView.Items[itemIdx].Tag = (section.SID, bitsDict.Index);
                            ResultView.Items[itemIdx].SubItems.Add(typeStr);
                            ResultView.Items[itemIdx].SubItems.Add(valueStr);
                            ResultView.Items[itemIdx].SubItems.Add(valueHex);
                            ResultView.Items[itemIdx].SubItems.Add(string.Format("{0}_{1}_{2}_{3}", section.Start.ToString("X"), section.Name, section.Prot.ToString("X"), section.Offset.ToString("X")));
                        }));
                        continue;
                    }

                    int scanOffset = 0;
                    backColor = backColor == default ? Color.DarkSlateGray : default;
                    for (int gIdx = 0; gIdx < comparerTool.groupTypes.Count; gIdx++)
                    {
                        (ScanType scanType, int groupTypeLength, bool isAny) group = comparerTool.groupTypes[gIdx];
                        byte[] oldGroupBytes = new byte[group.groupTypeLength];
                        Buffer.BlockCopy(oldBytes, scanOffset, oldGroupBytes, 0, group.groupTypeLength);
                        string typeStr = group.scanType.GetDescription();
                        string valueStr = ScanTool.BytesToString(group.scanType, oldGroupBytes);
                        string valueHex = ScanTool.BytesToString(group.scanType, oldGroupBytes, true);

                        Invoke(new MethodInvoker(() => {
                            int groupIdx = ResultView.Items.Count;
                            ResultView.Items.Add(offsetAddr.ToString("X8"), (offsetAddr + section.Start + (uint)scanOffset).ToString("X8"), 0);
                            ResultView.Items[groupIdx].Tag = (section.SID, bitsDict.Index);
                            ResultView.Items[groupIdx].SubItems.Add(typeStr);
                            ResultView.Items[groupIdx].SubItems.Add(valueStr);
                            ResultView.Items[groupIdx].SubItems.Add(valueHex);
                            ResultView.Items[groupIdx].SubItems.Add(string.Format("{0}_{1}_{2}_{3}", section.Start.ToString("X"), section.Name, section.Prot.ToString("X"), section.Offset.ToString("X")));
                            ResultView.Items[groupIdx].BackColor = backColor;
                        }));
                        scanOffset += group.groupTypeLength;
                    }
                }
            }
            Invoke(new MethodInvoker(() => {
                ResultView.EndUpdate();
                ScanTypeBox_SelectedIndexChanged(null, null);
                ScanBtn.Text = "Next Scan";
            }));
        }

        Task<bool> refreshTask;
        CancellationTokenSource refreshSource;
        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ComboboxItem process = (ComboboxItem)ProcessesBox.SelectedItem;
                int pid = (int)process.Value;
                ProcessMap pMap = PS4Tool.GetProcessMaps(pid);
                if (pMap.entries == null || ResultView.Items.Count == 0) return;
                if (refreshTask != null && !refreshTask.IsCompleted) return;

                if (refreshSource != null) refreshSource.Dispose();
                refreshSource = new CancellationTokenSource();
                refreshTask = RefreshTask(IsFilterBox.Checked, IsFilterSizeBox.Checked);
                refreshTask.ContinueWith(t => TaskCompleted());
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\n" + exception.StackTrace, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        //Invoke(new MethodInvoker(() => { }));
        private async Task<bool> RefreshTask(bool isFilter, bool isFilterSize) => await Task.Run(() => {
            System.Diagnostics.Stopwatch tickerMajor = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                if (bitsDictDict.Count == 0) return false;

                int hitCnt = 0;
                int count = 0;

                Invoke(new MethodInvoker(() => { ToolStripBar.Value = 1; }));

                Mutex mutex = new Mutex();
                byte MaxQueryThreads = Properties.Settings.Default.MaxQueryThreads.Value;
                uint QueryBufferSize = Properties.Settings.Default.QueryBufferSize.Value;
                uint MaxResultShow = Properties.Settings.Default.MaxResultShow.Value;
                uint SectionFilterSize = Properties.Settings.Default.SectionFilterSize.Value;
                string SectionFilterKeys = Properties.Settings.Default.SectionFilterKeys.Value;
                MaxQueryThreads = MaxQueryThreads == (byte)0 ? (byte)1 : MaxQueryThreads;
                MaxResultShow = MaxResultShow == 0 ? 0x2000 : MaxResultShow;
                SectionFilterKeys = Regex.Replace(SectionFilterKeys, " *[,;] *", "|");

                int readCnt = 0;
                ulong minLength = QueryBufferSize * 1024 * 1024; //set the minimum read size in bytes

                Section[] sectionKeys = sectionTool.GetSectionSortByAddr(bitsDictDict.Keys);
                SemaphoreSlim semaphore = new SemaphoreSlim(MaxQueryThreads);
                List<Task<bool>> tasks = new List<Task<bool>>();

                (int start, int end) rangeIdx = (-1, -1);
                for (int sectionIdx = 0; sectionIdx < sectionKeys.Length; sectionIdx++)
                {
                    readCnt++;
                    bool isPerform = false;
                    bool isContinue = false;
                    bool isContinuePerform = false;
                    Section checkSection = sectionKeys[sectionIdx];
                    if (!checkSection.Check || isFilter && checkSection.IsFilter || isFilterSize && checkSection.IsFilterSize) isContinue = true; //Check if section is not scanned

                    if (isContinue)
                    {
                        if (rangeIdx.start == -1) continue; //start and end index unchanged when not scanning
                        else //start performing a scan when the start index is set
                        {
                            checkSection = sectionKeys[rangeIdx.end];
                            isContinuePerform = true;
                        }
                    }
                    if (rangeIdx.start == -1) //set start and end index when not set
                    {
                        rangeIdx.start = sectionIdx;
                        rangeIdx.end = sectionIdx;
                    }

                    Section firstSection = sectionKeys[rangeIdx.start];
                    ulong bufferSize = checkSection.Start + (ulong)checkSection.Length - firstSection.Start;
                    if (bufferSize >= int.MaxValue) isPerform = true; //check the size of the scan to be executed, whether the scan size has been reached the upper limit

                    if (!isContinuePerform && !isPerform && bufferSize < minLength && (sectionIdx != sectionKeys.Length - 1 || rangeIdx.start == -1))
                    {
                        rangeIdx.end = sectionIdx; //update end index
                        continue;
                    }
                    //start scanning
                    if (!isContinuePerform && !isPerform) rangeIdx.end = sectionIdx;
                    var rangeIdx_ = rangeIdx;
                    tasks.Add(Task.Run<bool>(() =>
                    {
                        semaphore.Wait();
                        refreshSource.Token.ThrowIfCancellationRequested();

                        Section lastSection = sectionKeys[rangeIdx_.end];
                        if (isPerform) bufferSize = lastSection.Start + (ulong)lastSection.Length - firstSection.Start;
                        byte[] buffer = PS4Tool.ReadMemory(firstSection.PID, firstSection.Start, (int)bufferSize);
                        for (int idx = rangeIdx_.start; idx <= rangeIdx_.end; idx++)
                        {
                            Section addrSection = sectionKeys[idx];
                            int scanOffset = (int)(addrSection.Start - firstSection.Start);

                            refreshSource.Token.ThrowIfCancellationRequested();
                            Byte[] subBuffer = new Byte[addrSection.Length];
                            Buffer.BlockCopy(buffer, (int)scanOffset, subBuffer, 0, addrSection.Length);
                            bitsDictDict.TryGetValue(addrSection.SID, out BitsDictionary bitsDict);
                            bitsDict.Begin();
                            for (int bIdx = 0; bIdx < bitsDict.Count; bIdx++)
                            {
                                if (++hitCnt > MaxResultShow && MaxQueryThreads == 1) continue;
                                (uint offsetAddr, _) = bitsDict.Get();
                                byte[] newBytes = new byte[comparerTool.scanTypeLength];
                                Buffer.BlockCopy(subBuffer, (int)offsetAddr, newBytes, 0, comparerTool.scanTypeLength);
                                bitsDict.Set(newBytes);
                            }
                            Invoke(new MethodInvoker(() =>
                            {
                                ToolStripBar.Value = (int)(((float)(++count) / sectionKeys.Length) * 100);
                                ToolStripMsg.Text = string.Format("Refresh elapsed:{0}s. {1}", tickerMajor.Elapsed.TotalSeconds, string.Format("Count: {0}", hitCnt));
                            }));
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
                Invoke(new MethodInvoker(() => {
                    ToolStripBar.Value = 100;
                    ToolStripMsg.Text = string.Format("Refresh elapsed:{0}s. {1}", tickerMajor.Elapsed.TotalSeconds, string.Format("Count: {0}", hitCnt));
                }));
            }
            catch (Exception exception)
            {
                if (exception.InnerException is OperationCanceledException)
                {
                    Invoke(new MethodInvoker(() => {
                        ToolStripBar.Value = 100;
                        ToolStripMsg.Text = string.Format("Refresh elapsed:{0}s. RefreshTask canceled. {1}", tickerMajor.Elapsed.TotalSeconds, exception.InnerException.Message);
                    }));
                }
                else MessageBox.Show(exception.Message + "\n" + exception.StackTrace, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                if (refreshSource != null) refreshSource.Dispose();
                tickerMajor.Stop();
            }
            return true;
        });

        private void ScanTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Object selectedCompareType = CompareTypeBox.SelectedItem;
            if (!HexBox.Enabled)
            {
                HexBox.Enabled = true;
                AlignmentBox.Enabled = true;
                AlignmentBox.Checked = true;
            }
            CompareTypeBox.Items.Clear();
            ScanType scanType = this.ParseFromDescription<ScanType>(ScanTypeBox.SelectedItem.ToString());
            switch (scanType)
            {
                case ScanType.Bytes_8:
                case ScanType.Bytes_4:
                case ScanType.Bytes_2:
                case ScanType.Byte_:
                    if (ResultView.Items.Count == 0) CompareTypeBox.Items.AddRange(Constant.SearchByBytesFirst);
                    else CompareTypeBox.Items.AddRange(Constant.SearchByBytesNext);
                    break;
                case ScanType.Double_:
                case ScanType.Float_:
                    if (ResultView.Items.Count == 0) CompareTypeBox.Items.AddRange(Constant.SearchByFloatFirst);
                    else CompareTypeBox.Items.AddRange(Constant.SearchByFloatNext);
                    break;
                case ScanType.Hex:
                case ScanType.Group:
                case ScanType.String_:
                    HexBox.Enabled = false;
                    HexBox.Checked = false;
                    if (scanType != ScanType.Group)
                    {
                        AlignmentBox.Enabled = false;
                        AlignmentBox.Checked = false;
                    }
                    CompareTypeBox.Items.AddRange(Constant.SearchByHex);
                    break;
                default:
                    throw new Exception("ScanType verification failed");
            }

            int listIdx = 0;
            int listCount = CompareTypeBox.Items.Count;

            for (; listIdx < listCount; ++listIdx) if (CompareTypeBox.Items[listIdx] == selectedCompareType) break;

            CompareTypeBox.SelectedIndex = listIdx == listCount ? 0 : listIdx;
        }

        private void CompareTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Enum.TryParse(CompareTypeBox.SelectedItem.ToString(), out CompareType compareType);
            switch (compareType)
            {
                case CompareType.Between:
                    TableLayoutPanel1.ColumnStyles[1].Width = 50;
                    TableLayoutPanel1.ColumnStyles[2].SizeType = SizeType.AutoSize;
                    TableLayoutPanel1.ColumnStyles[3].Width = 50;
                    break;
                default:
                    TableLayoutPanel1.ColumnStyles[1].Width = 100;
                    TableLayoutPanel1.ColumnStyles[2].SizeType = SizeType.Percent;
                    TableLayoutPanel1.ColumnStyles[2].Width = 0;
                    TableLayoutPanel1.ColumnStyles[3].Width = 0;
                    break;
            }
        }

        private void SectionView_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ListViewItem item = SectionView.Items[e.Index];
            int pid = sectionTool.PID;
            int sid = int.Parse(item.SubItems[(int)SectionCol.SectionViewSID].Text);
            int idx = int.Parse(item.Name);
            Section section = sectionTool.SectionDict[sid];

            section.Check = e.NewValue == CheckState.Checked;
            if (section.Check)
            {
                sectionTool.TotalMemorySize += (ulong)section.Length;
                if (AddrMinBox.Text == "") AddrMinBox.Text = section.Start.ToString("X");
                else
                {
                    var AddrMin = ulong.Parse(AddrMinBox.Text, NumberStyles.HexNumber);
                    if (section.Start < AddrMin) AddrMinBox.Text = section.Start.ToString("X");
                }
                if (AddrMaxBox.Text == "") AddrMaxBox.Text = (section.Start + (ulong)section.Length).ToString("X");
                else
                {
                    var AddrMax = ulong.Parse(AddrMaxBox.Text, NumberStyles.HexNumber);
                    if (section.Start + (ulong)section.Length > AddrMax) AddrMaxBox.Text = (section.Start + (ulong)section.Length).ToString("X");
                }
            }
            else sectionTool.TotalMemorySize -= (ulong)section.Length;

            if (item.Checked) item.BackColor = Color.DarkSlateGray;
            else item.BackColor = Color.DarkGreen;
        }

        private void SelectAllBox_CheckedChanged(object sender, EventArgs e)
        {
            bool check = SelectAllBox.Checked;
            for (int idx = 0; idx < SectionView.Items.Count; ++idx) SectionView.Items[idx].Checked = check;
            if (!check)
            {
                AddrMinBox.Text = "";
                AddrMaxBox.Text = "";
            }
        }

        private void IsFilterSizeBox_CheckedChanged(object sender, EventArgs e)
        {
            int idx = ProcessesBox.SelectedIndex;
            if (idx == -1) return;

            if (!IsFilterSizeBox.Checked)
            {
                ProcessesBox.SelectedIndex = 0;
                ProcessesBox.SelectedIndex = idx;
            }
            else
            {
                SectionView.BeginUpdate();
                for (int sIdx = 0; sIdx < SectionView.Items.Count; sIdx++)
                {
                    ListViewItem item = SectionView.Items[sIdx];
                    if (!"filterSize".Equals(item.Tag)) continue;
                    item.Checked = false; //Ensure that MappedSectionList is not selected
                    item.Remove();
                    --sIdx;
                }
                SectionView.EndUpdate();
            }
        }

        private void IsFilterBox_CheckedChanged(object sender, EventArgs e)
        {
            int idx = ProcessesBox.SelectedIndex;
            if (idx == -1) return;

            if (!IsFilterBox.Checked)
            {
                ProcessesBox.SelectedIndex = 0;
                ProcessesBox.SelectedIndex = idx;
            }
            else
            {
                SectionView.BeginUpdate();
                for (int sIdx = 0; sIdx < SectionView.Items.Count; sIdx++)
                {
                    ListViewItem item = SectionView.Items[sIdx];
                    if (!"filter".Equals(item.Tag)) continue;
                    item.Checked = false; //Ensure that MappedSectionList is not selected
                    item.Remove();
                    --sIdx;
                }
                SectionView.EndUpdate();
            }
        }

        private void ResultView_DoubleClick(object sender, EventArgs e)
        {
            if (ResultView.SelectedItems.Count == 1)
            {
                ListViewItem resultItem = ResultView.SelectedItems[0];
                (int sid, _) = ((int sid, int resultIdx))resultItem.Tag;
                Section section = sectionTool.GetSection(sid);
                ScanType scanType = this.ParseFromDescription<ScanType>(resultItem.SubItems[(int)ResultCol.ResultListType].Text);
                uint offsetAddr = (uint)(ulong.Parse(resultItem.SubItems[(int)ResultCol.ResultListAddress].Text, NumberStyles.HexNumber) - section.Start);
                string oldValue = resultItem.SubItems[(int)ResultCol.ResultListValue].Text;

                if (offsetAddr > 0) mainForm.AddToCheatGrid(section, offsetAddr, scanType, oldValue, false, "", false, null);
            }
        }

        private string searchSectionName = "";
        private void SectionSearchBtn_Click(object sender, EventArgs e)
        {
            if (InputBox.Show("Search", "Enter the value of the search section name", ref searchSectionName, null) != DialogResult.OK) return;

            int startIndex = 0;
            ListView.SelectedListViewItemCollection items = SectionView.SelectedItems;
            if (items.Count > 0 && items[items.Count - 1].Index + 1 < SectionView.Items.Count) startIndex = items[items.Count - 1].Index + 1;
            ListViewItem item = SectionView.FindItemWithText(searchSectionName, true, startIndex);
            if (item == null) return;

            SectionView.Items[item.Index].Selected = true;
            SectionView.Items[item.Index].EnsureVisible();
        }

        private void FilterRuleBtn_Click(object sender, EventArgs e)
        {
            string SectionFilterKeys = Properties.Settings.Default.SectionFilterKeys.Value;

            if (InputBox.Show("Section Filter", "Enter the value of the filter keys", ref SectionFilterKeys, null) != DialogResult.OK) return;

            Properties.Settings.Default.SectionFilterKeys.Value = SectionFilterKeys;
        }
        private void ResumeBtn_Click(object sender, EventArgs e)
        {
            processStatus = ProcessStatus.Resume;
            ComboboxItem process = (ComboboxItem)ProcessesBox.SelectedItem;
            PS4Tool.AttachDebugger((int)process.Value, (string)process.Text, processStatus);
        }

        private void PauseBtn_Click(object sender, EventArgs e)
        {
            processStatus = ProcessStatus.Pause;
            ComboboxItem process = (ComboboxItem)ProcessesBox.SelectedItem;
            PS4Tool.AttachDebugger((int)process.Value, (string)process.Text, processStatus);
        }
        #endregion

        #region SectionViewMenu
        private void SectionViewHexEditor_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items = SectionView.SelectedItems;
            if (items.Count == 0) return;

            var sectionItem = items[0];
            int sid = int.Parse(sectionItem.SubItems[(int)SectionCol.SectionViewSID].Text);
            Section section = sectionTool.GetSection(sid);

            HexEditor hexEdit = new HexEditor(mainForm, section, 0);
            hexEdit.Show(this);
        }

        private void SectionViewDump_Click(object sender, EventArgs e)
        {
            //ListView.SelectedListViewItemCollection items = SectionView.SelectedItems;
            //if (items.Count > 0)
            //{
            //    for (int idx = 0; idx < items.Count; ++idx)
            //    {
            //        dump_dialog(int.Parse(items[idx].Name));
            //    }
            //}
        }

        private void SectionViewCheck_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items = SectionView.SelectedItems;
            if (items.Count == 0) return;

            for (int idx = 0; idx < items.Count; ++idx) items[idx].Checked = !items[idx].Checked;
        }
        #endregion

        #region ResultViewMenu
        private void ResultViewAddToCheatGrid_Click(object sender, EventArgs e)
        {
            if (ResultView.SelectedItems == null) return;

            ListView.SelectedListViewItemCollection items = ResultView.SelectedItems;
            for (int i = 0; i < items.Count; ++i)
            {
                ListViewItem resultItem = items[i];
                (int sid, _) = ((int sid, int resultIdx))resultItem.Tag;
                Section section = sectionTool.GetSection(sid);
                ScanType scanType = this.ParseFromDescription<ScanType>(resultItem.SubItems[(int)ResultCol.ResultListType].Text);
                uint offsetAddr = (uint)(ulong.Parse(resultItem.SubItems[(int)ResultCol.ResultListAddress].Text, NumberStyles.HexNumber) - section.Start);
                string oldValue = resultItem.SubItems[(int)ResultCol.ResultListValue].Text;

                if (offsetAddr > 0) mainForm.AddToCheatGrid(section, offsetAddr, scanType, oldValue, false, "", false, null);
            }
        }

        private void ResultViewHexEditor_Click(object sender, EventArgs e)
        {
            if (ResultView.SelectedItems == null || ResultView.SelectedItems.Count != 1) return;

            ListView.SelectedListViewItemCollection items = ResultView.SelectedItems;
            var resultItem = items[0];
            (int sid, _) = ((int sid, int resultIdx))resultItem.Tag;
            Section section = sectionTool.GetSection(sid);
            uint offsetAddr = (uint)(ulong.Parse(resultItem.SubItems[(int)ResultCol.ResultListAddress].Text, NumberStyles.HexNumber) - section.Start);
            if (offsetAddr == 0) return;

            HexEditor hexEdit = new HexEditor(mainForm, section, (int)offsetAddr);
            hexEdit.Show(this);
        }

        private void ResultViewCopyAddress_Click(object sender, EventArgs e)
        {
            if (ResultView.SelectedItems == null || ResultView.SelectedItems.Count == 0) return;

            string clipStr = "";
            ListView.SelectedListViewItemCollection items = ResultView.SelectedItems;
            for (int i = 0; i < items.Count; ++i)
            {
                ListViewItem resultItem = items[i];
                (int sid, _) = ((int sid, int resultIdx))resultItem.Tag;
                Section section = sectionTool.GetSection(sid);
                uint offsetAddr = (uint)(ulong.Parse(resultItem.SubItems[(int)ResultCol.ResultListAddress].Text, NumberStyles.HexNumber) - section.Start);
                if (offsetAddr == 0) continue;
                if (clipStr.Length > 0) clipStr += " \n";
                clipStr += (offsetAddr + section.Start).ToString("X");
            }
            if (clipStr.Length > 0) Clipboard.SetText(clipStr);
        }

        private void ResultViewDump_Click(object sender, EventArgs e)
        {
            //if (ResultView.SelectedItems.Count == 1)
            //{
            //    ulong address = ulong.Parse(ResultView.SelectedItems[0].SubItems[(int)ResultCol.ResultListAddress].Text, NumberStyles.HexNumber);
            //    int sectionID = processManager.MappedSectionList.GetMappedSectionID(address);
            //    dump_dialog(sectionID);
            //}
        }

        private void ResultViewFindPointer_Click(object sender, EventArgs e)
        {
            if (ResultView.SelectedItems == null || ResultView.SelectedItems.Count != 1) return;

            try
            {
                ulong address = ulong.Parse(ResultView.SelectedItems[0].SubItems[(int)ResultCol.ResultListAddress].Text, NumberStyles.HexNumber);
                ScanType scanType = this.ParseFromDescription<ScanType>(ResultView.SelectedItems[0].SubItems[(int)ResultCol.ResultListType].Text);
                PointerFinder pointerFinder = new PointerFinder(mainForm, address, scanType);
                pointerFinder.Show();
            }
            catch { }
        }
        #endregion

        #region SlowMotion
        int slowMotionInterval = 200;
        ProcessStatus processStatus;
        Task<bool> slowMotionTask;
        private void SlowMotionTimer_Tick(object sender, EventArgs e)
        {
            if (processStatus == ProcessStatus.Pause) return;
            if (slowMotionTask != null && !slowMotionTask.IsCompleted) return;
            if (slowMotionTask != null) slowMotionTask.Dispose();

            slowMotionTask = SlowMotionTask();
            if (DateTime.Now.Second % 10 == 0) GC.Collect();
        }
        private async Task<bool> SlowMotionTask() => await Task.Run(() =>
        {
            Invoke(new MethodInvoker(() => { PauseBtn.PerformClick(); }));
            Thread.Sleep(slowMotionInterval);

            Invoke(new MethodInvoker(() => { ResumeBtn.PerformClick(); }));
            Thread.Sleep(slowMotionInterval > 200 ? 200 : slowMotionInterval);

            return true;
        });

        private void SlowMotionBox_CheckedChanged(object sender, EventArgs e)
        {
            if (SlowMotionBox.Checked)
            {
                string intervalStr = "200";
                ComboboxItem process = (ComboboxItem)ProcessesBox.SelectedItem;
                if (InputBox.Show("SlowMotion", "Enter the SlowMotion interval (in milliseconds, larger intervals will be slower)", ref intervalStr, null) != DialogResult.OK ||
                    !PS4Tool.AttachDebugger((int)process.Value, (string)process.Text, processStatus))
                {
                    SlowMotionBox.Checked = false;
                    return;
                }
                if (!int.TryParse(intervalStr, out slowMotionInterval)) slowMotionInterval = 200;
                if (slowMotionInterval < 100) slowMotionInterval = 100;
                SlowMotionTimer.Interval = 100;
                SlowMotionTimer.Enabled = true;
            }
            else
            {
                if (!SlowMotionTimer.Enabled) return;

                SlowMotionTimer.Enabled = false;
                ResumeBtn.PerformClick();
            }

        }
        #endregion
    }
}
