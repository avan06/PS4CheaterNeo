using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;
using System.IO;

namespace PS4CheaterNeo
{
    public partial class Query : Form
    {
        readonly Main mainForm;
        readonly SectionTool sectionTool;
        readonly bool enableUndoScan;
        ComparerTool comparerTool;
        Color querySectionViewFilterForeColor;
        Color querySectionViewFilterBackColor;
        Color querySectionViewFilterSizeForeColor;
        Color querySectionViewFilterSizeBackColor;
        Color querySectionViewItemCheck1BackColor;
        Color querySectionViewItemCheck2BackColor;

        int hitCnt;
        bool enableFloatingResultExact = true;
        byte floatingSimpleValueExponents = 10;

        //Confirm whether the compare type starts with UnknownInitial
        bool isUnknownInitial = false;
        bool UnknownInitialScanDoNotSkip0   = Properties.Settings.Default.UnknownInitialScanDoNotSkip0.Value;
        bool SectionViewDetectHiddenSection = Properties.Settings.Default.SectionViewDetectHiddenSection.Value;
        bool WriteHiddenSectionConf         = Properties.Settings.Default.WriteHiddenSectionConf.Value;
        bool isCloneScan = false;

        List<ListViewItem> sectionItems = new List<ListViewItem>();
        List<ListViewItem> resultItems = new List<ListViewItem>();
        int bitsDictDictsIdx = 0;
        List<Dictionary<uint, BitsDictionary>> bitsDictDicts = new List<Dictionary<uint, BitsDictionary>>();

        public Query(Main mainForm)
        {
            InitializeComponent();
            ApplyUI();

            if (!Properties.Settings.Default.CollapsibleContainer.Value)
            {
                SplitContainer1.SplitterButtonStyle = ButtonStyle.None;
                SplitContainer2.SplitterButtonStyle = ButtonStyle.None;
            }
            this.mainForm = mainForm;

            sectionTool = new SectionTool(mainForm);
            IsFilterBox.Checked       = Properties.Settings.Default.FilterQuery.Value;
            IsFilterSizeBox.Checked   = Properties.Settings.Default.FilterSizeQuery.Value;
            enableUndoScan            = Properties.Settings.Default.UndoScan.Value;
            AutoPauseBox.Checked      = Properties.Settings.Default.ScanAutoPause.Value;
            AutoResumeBox.Checked     = Properties.Settings.Default.ScanAutoResume.Value;
            SectionView.FullRowSelect = Properties.Settings.Default.SectionViewFullRowSelect.Value;
        }

        public Query(Main mainForm, ComparerTool comparerTool = null, int bitsDictDictsIdx = 0, List<Dictionary<uint, BitsDictionary>> bitsDictDicts = null) : this(mainForm)
        {
            if (bitsDictDicts == null || bitsDictDicts.Count <= 0) return;

            this.isCloneScan = true;
            this.comparerTool = comparerTool;
            this.bitsDictDictsIdx = bitsDictDictsIdx;
            this.bitsDictDicts = new List<Dictionary<uint, BitsDictionary>>(bitsDictDicts);
        }

        public void ApplyUI()
        {
            try
            {
                Opacity = Properties.Settings.Default.UIOpacity.Value;

                ForeColor              = Properties.Settings.Default.UiForeColor.Value; //Color.White;
                BackColor              = Properties.Settings.Default.UiBackColor.Value; //Color.FromArgb(36, 36, 36);
                StatusStrip1.BackColor = Properties.Settings.Default.QueryStatusStrip1BackColor.Value; //Color.DimGray;
                AlignmentBox.ForeColor = Properties.Settings.Default.QueryAlignmentBoxForeColor.Value; //Color.Silver;
                ScanBtn.BackColor      = Properties.Settings.Default.QueryScanBtnBackColor.Value; //Color.SteelBlue;

                querySectionViewFilterForeColor     = Properties.Settings.Default.QuerySectionViewFilterForeColor.Value;
                querySectionViewFilterBackColor     = Properties.Settings.Default.QuerySectionViewFilterBackColor.Value;
                querySectionViewFilterSizeForeColor = Properties.Settings.Default.QuerySectionViewFilterSizeForeColor.Value;
                querySectionViewFilterSizeBackColor = Properties.Settings.Default.QuerySectionViewFilterSizeBackColor.Value;
                querySectionViewItemCheck1BackColor = Properties.Settings.Default.QuerySectionViewItemCheck1BackColor.Value;
                querySectionViewItemCheck2BackColor = Properties.Settings.Default.QuerySectionViewItemCheck2BackColor.Value;

                ToolStripMsg.ForeColor = ForeColor;
                SplitContainer1.ForeColor = ForeColor;
                SplitContainer2.ForeColor = ForeColor;

                SlowMotionBox.ForeColor   = ForeColor;
                SelectAllBox.ForeColor    = ForeColor;
                IsFilterSizeBox.ForeColor = ForeColor;
                IsFilterBox.ForeColor     = ForeColor;

                ResultView.ForeColor  = ForeColor;
                ResultView.BackColor  = BackColor;
                SectionView.ForeColor = ForeColor;
                SectionView.BackColor = BackColor;

                AddrMaxBox.ForeColor   = ForeColor;
                AddrMaxBox.BackColor   = BackColor;
                AddrMinBox.ForeColor   = ForeColor;
                AddrMinBox.BackColor   = BackColor;
                ProcessesBox.ForeColor = ForeColor;
                ProcessesBox.BackColor = BackColor;

                ScanTypeBox.ForeColor      = ForeColor;
                ScanTypeBox.BackColor      = BackColor;
                ValueBox.ForeColor         = ForeColor;
                ValueBox.BackColor         = BackColor;
                Value1Box.ForeColor        = ForeColor;
                Value1Box.BackColor        = BackColor;
                CompareTypeBox.ForeColor   = ForeColor;
                CompareTypeBox.BackColor   = BackColor;
                PauseBtn.ForeColor         = ForeColor;
                PauseBtn.BackColor         = BackColor;
                ResumeBtn.ForeColor        = ForeColor;
                ResumeBtn.BackColor        = BackColor;
                SectionSearchBtn.ForeColor = ForeColor;
                SectionSearchBtn.BackColor = BackColor;
                GetProcessesBtn.ForeColor  = ForeColor;
                GetProcessesBtn.BackColor  = BackColor;
                CloneScanBtn.ForeColor     = ForeColor;
                CloneScanBtn.BackColor     = BackColor;
                RedoBtn.BackColor          = BackColor;
                UndoBtn.BackColor          = BackColor;
                NewBtn.BackColor           = BackColor;
                RefreshBtn.BackColor       = BackColor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Source + ":ApplyUI", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        #region Event
        private void Query_Load(object sender, EventArgs e)
        {
            foreach (ScanType filterEnum in (ScanType[])Enum.GetValues(typeof(ScanType)))
            {
                if ((!SectionViewDetectHiddenSection || !WriteHiddenSectionConf) && filterEnum == ScanType.HiddenSections) continue;
                ScanTypeBox.Items.Add(new ComboItem(filterEnum.GetDescription(), filterEnum));
            }
            ScanTypeBox.SelectedIndex = 2;
            if (Properties.Settings.Default.AutoPerformGetProcesses.Value) GetProcessesBtn.PerformClick();
            if (isCloneScan)
            {
                SyncSections("Clone scan");
                TaskCompleted(System.Diagnostics.Stopwatch.StartNew());
                NewBtn.Enabled = true;
            }
        }

        private void Query_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ResultView.Items.Count > 0 && MessageBox.Show("Still in the query, Do you want to close Query?", "Query", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }

            SectionView.VirtualListSize = 0;
            SectionView.VirtualMode = false;
            ResultView.VirtualListSize = 0;
            ResultView.VirtualMode = false;
            sectionItems.Clear();
            sectionItems = null;
            resultItems.Clear();
            resultItems = null;
            ResultView.Clear();
            ResultView = null;
            ComboItem process = (ComboItem)ProcessesBox.SelectedItem;
            if (process != null) PS4Tool.DetachDebugger((int)process.Value);
            bitsDictDicts.Clear();
            GC.Collect();
            Properties.Settings.Default.Save();
        }

        private void GetProcessesBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!PS4Tool.Connect(Properties.Settings.Default.PS4IP.Value, out string msg)) throw new Exception(msg);

                int selectedIdx = 0;
                string DefaultProcess = Properties.Settings.Default.DefaultProcess.Value;
                ProcessesBox.Items.Clear();
                libdebug.ProcessList procList = PS4Tool.GetProcessList();
                for (int pIdx = 0; pIdx < procList.processes.Length; pIdx++)
                {
                    libdebug.Process process = procList.processes[pIdx];
                    int idx = ProcessesBox.Items.Add(new ComboItem(process.name, process.pid));
                    if (process.name == DefaultProcess) selectedIdx = idx;
                }
                ProcessesBox.SelectedIndex = selectedIdx;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Source + ":GetProcessesBtn_Click", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void ProcessesBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SectionViewCheckAllHidden.Visible   = SectionViewDetectHiddenSection;
                SectionViewUnCheckAllHidden.Visible = SectionViewDetectHiddenSection;

                SectionView.BeginUpdate();
                SectionView.VirtualListSize = 0;
                ResultView.VirtualListSize = 0;
                sectionItems.Clear();
                resultItems.Clear();
                SectionView.Items.Clear();
                ResultView.Items.Clear();
                SectionView.VirtualMode = false;

                ComboItem process = (ComboItem)ProcessesBox.SelectedItem;
                sectionTool.InitSections((int)process.Value, (string)process.Text);
                mainForm.ProcessPid = (int)process.Value;
                mainForm.ProcessName = (string)process.Text;

                Section[] sections = sectionTool.GetSectionSortByAddr();
                for (int sectionIdx = 0; sectionIdx < sections.Length; sectionIdx++)
                {
                    Section section = sections[sectionIdx];
                    if ((IsFilterBox.Checked && section.IsFilter) || (IsFilterSizeBox.Checked && section.IsFilterSize)) continue;

                    string start = String.Format("{0:X9}", section.Start);
                    ListViewItem item = new ListViewItem(sectionIdx.ToString(), 0)
                    {
                        Name = sectionIdx.ToString()
                    };
                    item.SubItems.Add(start);
                    item.SubItems.Add(section.Name);
                    item.SubItems.Add(section.Prot.ToString("X"));
                    item.SubItems.Add((section.Length / 1024).ToString() + "KB");
                    item.SubItems.Add(section.SID.ToString());
                    if (section.Offset != 0) item.SubItems.Add(section.Offset.ToString("X"));
                    else item.SubItems.Add("");
                    item.SubItems.Add((section.Start + (ulong)section.Length).ToString("X9"));
                    if (section.IsFilter)
                    {
                        item.Tag = "filter";
                        item.ForeColor = querySectionViewFilterForeColor; //Color.DarkGray;
                        item.BackColor = querySectionViewFilterBackColor; //Color.DimGray;
                    }
                    else if (section.IsFilterSize)
                    {
                        item.Tag = "filterSize";
                        item.ForeColor = querySectionViewFilterSizeForeColor; //Color.DarkCyan;
                        item.BackColor = querySectionViewFilterSizeBackColor; //Color.DarkSlateGray;
                    }
                    else if (section.Name.Contains("Hidden")) item.ForeColor = Properties.Settings.Default.QuerySectionViewHiddenForeColor.Value; //Color.Firebrick;
                    else if (section.Name.StartsWith("executable")) item.ForeColor = Properties.Settings.Default.QuerySectionViewExecutableForeColor.Value; //Color.GreenYellow;
                    else if (section.Name.Contains("NoName")) item.ForeColor = Properties.Settings.Default.QuerySectionViewNoNameForeColor.Value; //Color.Red;
                    else if (Regex.IsMatch(section.Name, @"^\[\d+\]$")) item.ForeColor = Properties.Settings.Default.QuerySectionViewNoName2ForeColor.Value; //Color.HotPink;
                    item.Checked = true;
                    sectionItems.Add(item);
                    item.Checked = false; //When ListView is in VirtualMode, you need to handle it like this to make the CheckBoxes visible.
                }

                SectionView.VirtualListSize = sectionItems.Count;
                SectionView.VirtualMode = true;
                SectionView.EndUpdate();
                ToolStripMsg.Text = string.Format("Total section: {0}, Selected section: {1}, Search size: {2}MB", sectionItems.Count, sectionTool.TotalSelected, sectionTool.TotalMemorySize / (1024 * 1024));
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("Map is null")) MessageBox.Show(ex.ToString(), ex.Source + ":ProcessesBox_SelectedIndexChanged", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void NewBtn_Click(object sender, EventArgs e)
        {
            if (scanTask != null && !scanTask.IsCompleted)
            {
                if (MessageBox.Show("Still in the scanning, Do you want to stop scan?", "Scan",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    scanSource.Cancel();
                    if (AutoResumeBox.Checked) ResumeBtn.PerformClick();
                }
                else return;
            }

            ResultView.VirtualListSize = 0;
            resultItems.Clear();
            ResultView.Items.Clear();
            bitsDictDicts.Clear();
            bitsDictDictsIdx = 0;

            comparerTool = null;
            GC.Collect();

            ScanBtn.Text = "First Scan";
            ScanTypeBox.Enabled = true;
            AlignmentBox.Enabled = true;
            AddrIsFilterBox.Enabled = true;
            ProcessesBox.Enabled = true;
            NewBtn.Enabled = false;
            UndoBtn.Enabled = false;
            RedoBtn.Enabled = false;
            isUnknownInitial = false;
            CompareFirstBox.Enabled = false;
            CompareFirstBox.Checked = false;
            ScanTypeBox_SelectedIndexChanged(null, null);
        }

        Task<bool> undoTask = null;
        private void UndoBtn_Click(object sender, EventArgs e)
        {
            if (!enableUndoScan) return;
            if (undoTask != null && !undoTask.IsCompleted) return;
            if (bitsDictDictsIdx == 0) return;

            UndoBtn.Enabled = false;
            RedoBtn.Enabled = false;

            System.Diagnostics.Stopwatch tickerMajor = System.Diagnostics.Stopwatch.StartNew();
            undoTask = UnReDO(true, tickerMajor);
            undoTask.ContinueWith(t => TaskCompleted(tickerMajor));
        }

        private void RedoBtn_Click(object sender, EventArgs e)
        {
            if (!enableUndoScan) return;
            if (undoTask != null && !undoTask.IsCompleted) return;
            if (bitsDictDictsIdx == bitsDictDicts.Count - 1) return;

            UndoBtn.Enabled = false;
            RedoBtn.Enabled = false;

            System.Diagnostics.Stopwatch tickerMajor = System.Diagnostics.Stopwatch.StartNew();
            undoTask = UnReDO(false, tickerMajor);
            undoTask.ContinueWith(t => TaskCompleted(tickerMajor));
        }

        private async Task<bool> UnReDO(bool isUnDo, System.Diagnostics.Stopwatch tickerMajor) => await Task.Run(() =>
        {
            if (!enableUndoScan) return true;

            string msg = (isUnDo ? "Undo" : "Redo") + " scan";
            if (isUnDo && bitsDictDictsIdx > 0) bitsDictDictsIdx--;
            else if (!isUnDo && bitsDictDictsIdx < bitsDictDicts.Count - 1) bitsDictDictsIdx++;
            else return false;

            SyncSections(msg);

            return true;
        });

        private void SyncSections(string msg)
        {
            Invoke(new MethodInvoker(() => { SectionView.BeginUpdate(); }));
            for (int sectionIdx = 0; sectionIdx < sectionItems.Count; ++sectionIdx)
            {
                ListViewItem sectionItem = sectionItems[sectionIdx];
                uint sid = uint.Parse(sectionItem.SubItems[(int)SectionCol.SectionViewSID].Text);

                Invoke(new MethodInvoker(() => {
                    if (bitsDictDicts[bitsDictDictsIdx].ContainsKey(sid))
                    {
                        BitsDictionary bitsDict = bitsDictDicts[bitsDictDictsIdx][sid];
                        sectionItem.Checked = bitsDict.Count > 0;
                        if (!sectionItem.Checked) bitsDictDicts[bitsDictDictsIdx].Remove(sid);
                    }
                    else sectionItem.Checked = false;
                    SectionCheckUpdate(sectionItem.Checked, sid);
                }));
            }
            Invoke(new MethodInvoker(() => {
                if (AddrMinBox.Tag != null) AddrMinBox.Text = ((ulong)AddrMinBox.Tag).ToString("X");
                if (AddrMaxBox.Tag != null) AddrMaxBox.Text = ((ulong)AddrMaxBox.Tag).ToString("X");

                ToolStripMsg.Text = string.Format("{0}, Section: {1}/{2}(selected/total)", msg, sectionTool.TotalSelected, sectionItems.Count);
                UndoBtn.Enabled = bitsDictDictsIdx > 0;
                RedoBtn.Enabled = bitsDictDictsIdx < bitsDictDicts.Count - 1;
                SectionView.EndUpdate();
            }));
        }

        Task<bool> scanTask = null;
        CancellationTokenSource scanSource = null;
        private void ScanBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ScanBtn.Text == "Stop") //scanTask != null && !scanTask.IsCompleted
                {
                    if (MessageBox.Show("Still in the scanning, Do you want to stop scan?", "Scan",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        scanSource.Cancel();
                        if (AutoResumeBox.Checked) ResumeBtn.PerformClick();
                    }
                }
                else if (Properties.Settings.Default.ShowSearchSizeFirstScan.Value && resultItems.Count == 0 && MessageBox.Show("Search size:" + (sectionTool.TotalMemorySize / (1024 * 1024)).ToString() + "MB", "First Scan",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                else
                {
                    if (AutoPauseBox.Checked) PauseBtn.PerformClick();
                    UndoBtn.Enabled = false;
                    RedoBtn.Enabled = false;
                    ComboItem process = (ComboItem)ProcessesBox.SelectedItem;
                    int pid = (int)process.Value;

                    enableFloatingResultExact = Properties.Settings.Default.FloatingResultExact.Value;
                    floatingSimpleValueExponents = Properties.Settings.Default.FloatingSimpleValueExponents.Value;
                    floatingSimpleValueExponents = (floatingSimpleValueExponents > 1 && floatingSimpleValueExponents < 51) ? (byte)(floatingSimpleValueExponents - 1) : (byte)10;

                    string value0 = ValueBox.Text.Trim();
                    string value1 = Value1Box.Text.Trim();
                    bool alignment = AlignmentBox.Checked;
                    bool isHex = HexBox.Checked;
                    bool isNot = NotBox.Checked;
                    bool isFloatingSimpleValues = SimpleValuesBox.Checked;
                    bool isFilter = IsFilterBox.Checked;
                    bool isFilterSize = IsFilterSizeBox.Checked;
                    Enum.TryParse(((ComboItem)(ScanTypeBox.SelectedItem)).Value.ToString(), out ScanType scanType);
                    Enum.TryParse(CompareTypeBox.SelectedItem.ToString(), out CompareType compareType);
                    if (scanType == ScanType.HiddenSections)
                    {
                        mainForm.InitGameInfo();
                        mainForm.InitLocalHiddenSections();
                        if (mainForm.LocalHiddenSections.Count == 0)
                        {
                            MessageBox.Show(string.Format("Hidden Sections data (sections{0}{1}.conf) was not found locally. \nYou need to enable the \"WriteHiddenSectionConf\" option and \nrestart the Query window to initialize the configuration file.", Path.DirectorySeparatorChar, mainForm.GameID), "Scan", MessageBoxButtons.OK);
                            return;
                        }
                    }

                    ulong AddrMin = ParseHexAddrText(AddrMinBox.Text);
                    ulong AddrMax = ParseHexAddrText(AddrMaxBox.Text);
                    if (AddrMin > AddrMax && MessageBox.Show(String.Format("AddrMin({1:X}) > AddrMax({0:X})", AddrMin, AddrMax), "Scan", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK) return;

                    if (value0 == "") value0 = "0";
                    if (value1 == "") value1 = "0";
                    if (isHex)
                    {
                        value0 = Regex.Replace(value0, "[^0-9a-fA-F]", "");
                        value1 = Regex.Replace(value1, "[^0-9a-fA-F]", "");
                    }

                    if (!isUnknownInitial) isUnknownInitial = compareType == CompareType.UnknownInitial;
                    comparerTool = new ComparerTool(scanType, compareType, value0, value1, isHex, isNot, isFloatingSimpleValues, enableFloatingResultExact, floatingSimpleValueExponents, isUnknownInitial);

                    if (scanType == ScanType.Hex && !isHex)
                    { //ComparerTool has converted decimal to hex
                        HexBox.Checked = true;
                        ValueBox.Text = comparerTool.Value0;
                    }

                    ScanBtn.Text = "Stop";
                    if (scanSource != null) scanSource.Dispose();
                    scanSource = new CancellationTokenSource();
                    System.Diagnostics.Stopwatch tickerMajor = System.Diagnostics.Stopwatch.StartNew();
                    scanTask = ScanTask(alignment, isFilter, isFilterSize, AddrMin, AddrMax, CompareFirstBox.Checked, tickerMajor);
                    scanTask.ContinueWith(t => {
                        if (t.Exception != null) InputBox.MsgBox("ScanTask Exception", "", t.Exception.ToString(), 100);
                    }, TaskContinuationOptions.OnlyOnFaulted)
                    .ContinueWith(t => TaskCompleted(tickerMajor))
                    .ContinueWith(t => Invoke(new MethodInvoker(() => { 
                        if (AutoResumeBox.Checked) ResumeBtn.PerformClick();
                        UndoBtn.Enabled = bitsDictDictsIdx > 0;
                        RedoBtn.Enabled = bitsDictDictsIdx < bitsDictDicts.Count - 1;
                    })))
                    .ContinueWith(t => {
                        scanSource?.Dispose();
                        scanSource = null;
                        scanTask?.Dispose();
                        scanTask = null;
                    });

                    ScanTypeBox.Enabled = false;
                    AlignmentBox.Enabled = false;
                    ProcessesBox.Enabled = false;
                    NewBtn.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("CancellationTokenSource"))
                    MessageBox.Show(ex.ToString(), ex.Source + ":ScanBtn_Click", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                if (AutoResumeBox.Checked) ResumeBtn.PerformClick();
            }
        }

        /// <summary>
        /// Convert the Hex address text to ulong.
        /// </summary>
        /// <param name="text">Hex address text</param>
        /// <returns>ulong</returns>
        private ulong ParseHexAddrText(string text)
        {
            ulong addr;
            if (text == null || text.Trim() == "") return 0;
            else if (text.Contains("+"))
            {
                var texts = text.Split('+');
                addr = ulong.Parse(texts[0].ToLower().Replace("0x", "").Trim(), NumberStyles.HexNumber);
                ulong offset = ulong.Parse(texts[1].ToLower().Replace("0x", "").Trim(), NumberStyles.HexNumber);
                addr += offset;
            }
            else if (text.Contains("-"))
            {
                var texts = text.Split('-');
                addr = ulong.Parse(texts[0].ToLower().Replace("0x", "").Trim(), NumberStyles.HexNumber);
                ulong offset = ulong.Parse(texts[1].ToLower().Replace("0x", "").Trim(), NumberStyles.HexNumber);
                addr -= offset;
            }
            else addr = ulong.Parse(text.ToLower().Replace("0x", "").Trim(), NumberStyles.HexNumber);

            return addr;
        }

        #region ScanTask
        /// <summary>
        /// ScanTask
        /// Invoke(new MethodInvoker(() => { }));
        /// </summary>
        /// <param name="alignment">Enable data alignment</param>
        /// <param name="isFilter">Enable keyword filtering for sections</param>
        /// <param name="isFilterSize">Enable size filtering for sections</param>
        /// <param name="AddrMin">Minimum section address</param>
        /// <param name="AddrMax">Maximum section address</param>
        /// <param name="isCompareFirst">Compare to first scan will compare the current addresslist and it's value to the saved value of the first scan</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<bool> ScanTask(bool alignment, bool isFilter, bool isFilterSize, ulong AddrMin, ulong AddrMax, bool isCompareFirst, System.Diagnostics.Stopwatch tickerMajor) => await Task.Run(() =>
        {
            string errInfo = "";
            try
            {
                if (bitsDictDicts.Count <= bitsDictDictsIdx) bitsDictDicts.Add(new Dictionary<uint, BitsDictionary>());
                if (enableUndoScan && bitsDictDicts.Count > bitsDictDictsIdx && bitsDictDicts[bitsDictDictsIdx].Count > 0)
                {
                    bitsDictDictsIdx++;
                    Invoke(new MethodInvoker(() => { ToolStripMsg.Text = string.Format("Scan elapsed:{0:0.00}s. Start backup of current query results", tickerMajor.Elapsed.TotalSeconds); }));

                    if (bitsDictDicts.Count > bitsDictDictsIdx) bitsDictDicts.RemoveRange(bitsDictDictsIdx, bitsDictDicts.Count - bitsDictDictsIdx);
                    bitsDictDicts.Add(new Dictionary<uint, BitsDictionary>());
                    
                    foreach (KeyValuePair<uint, BitsDictionary> bitsDict in bitsDictDicts[bitsDictDictsIdx - 1]) bitsDictDicts[bitsDictDictsIdx].Add(bitsDict.Key, (BitsDictionary)bitsDict.Value.Clone());
                    Invoke(new MethodInvoker(() => { ToolStripMsg.Text = string.Format("Scan elapsed:{0:0.00}s. Complete backup of current query results", tickerMajor.Elapsed.TotalSeconds); }));
                }

                ulong hitCnt = 0;
                int scanStep;
                if (comparerTool.ScanType_ == ScanType.Hex || comparerTool.ScanType_ == ScanType.String_ || !alignment) scanStep = 1;
                else if (comparerTool.ScanTypeLength > 4) scanStep = 4;
                else scanStep = comparerTool.ScanTypeLength;

                Invoke(new MethodInvoker(() => { ToolStripBar.Value = 1; }));

                using (Mutex mutex = new Mutex())
                {
                    byte MaxQueryThreads                = Properties.Settings.Default.MaxQueryThreads.Value;
                    uint QueryBufferSize                = Properties.Settings.Default.QueryBufferSize.Value;
                    uint SectionFilterSize              = Properties.Settings.Default.SectionFilterSize.Value;
                    string SectionFilterKeys            = Properties.Settings.Default.SectionFilterKeys.Value;
                    sbyte MinResultAccessFactor         = Properties.Settings.Default.MinResultAccessFactor.Value;
                    uint MinResultAccessFactorThreshold = Properties.Settings.Default.MinResultAccessFactorThreshold.Value;
                    MaxQueryThreads = MaxQueryThreads == (byte)0 ? (byte)1 : MaxQueryThreads;
                    SectionFilterKeys = Regex.Replace(SectionFilterKeys, " *[,;] *", "|");

                    long processedMemoryLen = 0;
                    ulong minLength = QueryBufferSize * 1024 * 1024; //set the minimum read size in bytes

                    Section[] sectionKeys   = sectionTool.GetSectionSortByAddr();
                    SemaphoreSlim semaphore = new SemaphoreSlim(MaxQueryThreads);
                    List<Task<bool>> tasks  = new List<Task<bool>>();
                    List<(int start, int end)> rangeList = GetSectionRangeList(sectionKeys, isFilter, isFilterSize, minLength, AddrMin, AddrMax, MinResultAccessFactor, MinResultAccessFactorThreshold);

                    int sectionSelectedCnt = 0;
                    int sectionFoundCnt = 0;
                    for (int idx = 0; idx < rangeList.Count; idx++)
                    {
                        var range = rangeList[idx];
                        sectionSelectedCnt += range.start == -1 ? 1 : (range.end - range.start + 1);
                        tasks.Add(Task.Run<bool>(() =>
                        {
                            try
                            {
                                semaphore.Wait();
                                scanSource.Token.ThrowIfCancellationRequested();
                                byte[] buffer = null;
                                Section sectionStart = range.start == -1 ? null : sectionKeys[range.start];
                                Section sectionEnd = sectionKeys[range.end];

                                if (sectionStart != null)
                                {
                                    ulong bufferSize = sectionEnd.Start + (ulong)sectionEnd.Length - sectionStart.Start;
                                    if (!isCompareFirst) buffer = PS4Tool.ReadMemory(sectionStart.PID, sectionStart.Start, (int)bufferSize);
                                }
                                if (range.start == -1)
                                {
                                    range.start = range.end;
                                    sectionStart = sectionEnd;
                                    if (!isCompareFirst) buffer = PS4Tool.ReadMemory(sectionEnd.PID, sectionEnd.Start, (int)sectionEnd.Length);
                                }

                                for (int rIdx = range.start; rIdx <= range.end; rIdx++)
                                {
                                    Section addrSection = sectionKeys[rIdx];
                                    int scanOffset = (int)(addrSection.Start - sectionStart.Start);

                                    scanSource.Token.ThrowIfCancellationRequested();
                                    Byte[] subBuffer = null;
                                    BitsDictionary bitsDictFirst = null;
                                    
                                    if (isCompareFirst && bitsDictDicts[0].ContainsKey(addrSection.SID)) bitsDictFirst = bitsDictDicts[0][addrSection.SID];
                                    if (!bitsDictDicts[bitsDictDictsIdx].TryGetValue(addrSection.SID, out BitsDictionary bitsDict)) bitsDict = new BitsDictionary(scanStep, comparerTool.ScanTypeLength);
                                    if (buffer != null) //bitsDict.Count == 0 || bitsDict.Count > MinResultAccessFactor)
                                    {
                                        subBuffer = new Byte[addrSection.Length];
                                        Buffer.BlockCopy(buffer, (int)scanOffset, subBuffer, 0, addrSection.Length);
                                    }

                                    bitsDict = comparerTool.GroupTypes == null ? Comparer(subBuffer, addrSection, scanStep, AddrMin, AddrMax, bitsDict, isCompareFirst, bitsDictFirst) : ComparerGroup(subBuffer, addrSection, scanStep, AddrMin, AddrMax, bitsDict, isCompareFirst, bitsDictFirst);

                                    if (bitsDict != null && bitsDict.Count > 0)
                                    {
                                        try
                                        {
                                            mutex.WaitOne();
                                            hitCnt += (ulong)bitsDict.Count;
                                            bitsDictDicts[bitsDictDictsIdx][addrSection.SID] = bitsDict;
                                            sectionFoundCnt++;
                                        }
                                        finally
                                        {
                                            mutex.ReleaseMutex();
                                        }
                                    }
                                    else addrSection.Check = false;

                                    processedMemoryLen += addrSection.Length;
                                    Invoke(new MethodInvoker(() =>
                                    {
                                        ToolStripBar.Value = (int)(((float)processedMemoryLen / sectionTool.TotalMemorySize) * 100);
                                        ToolStripMsg.Text = string.Format("Scan elapsed:{0:0.00}s. {1}MB, Count: {2}, Section: {3}/{4}/{5}(found/selected/total)", tickerMajor.Elapsed.TotalSeconds, processedMemoryLen / (1024 * 1024), hitCnt, sectionFoundCnt, sectionSelectedCnt, sectionItems.Count);
                                    }));
                                }
                            }
                            catch (Exception ex)
                            {
                                if (!(ex is OperationCanceledException))
                                {
                                    errInfo += ex.ToString() + "\n\n";
                                    throw ex;
                                }
                            }
                            finally
                            {
                                semaphore.Release();
                            }
                            return true;
                        }));
                    }
                    Task whenTasks = Task.WhenAll(tasks);
                    whenTasks.Wait();
                    semaphore.Dispose();
                    whenTasks.Dispose();
                    GC.Collect();
                    Invoke(new MethodInvoker(() => {
                        SectionView.BeginUpdate();
                        for (int sectionIdx = 0; sectionIdx < sectionItems.Count; ++sectionIdx)
                        {
                            ListViewItem sectionItem = sectionItems[sectionIdx];
                            if (!sectionItem.Checked) continue;

                            uint sid = uint.Parse(sectionItem.SubItems[(int)SectionCol.SectionViewSID].Text);
                            Section section = sectionTool.GetSection(sid);
                            if (section.Check == false)
                            {
                                sectionItem.Checked = false;
                                SectionCheckUpdate(sectionItem.Checked, section, true);
                            }
                        }
                        if (comparerTool.ScanType_ == ScanType.HiddenSections) mainForm.UpdateLocalHiddenSections();
                        SectionView.EndUpdate();
                        ToolStripBar.Value = 100;
                    }));

                    GC.Collect();
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException is OperationCanceledException)
                {
                    Invoke(new MethodInvoker(() => {
                        ToolStripBar.Value = 100;
                        ToolStripMsg.Text = string.Format("Scan elapsed:{0:0.00}s. ScanTask canceled. {1}", tickerMajor.Elapsed.TotalSeconds, ex.InnerException.Message);
                    }));
                }
                else errInfo += ex.ToString() + "\n\n";
            }
            if (errInfo != "") throw new Exception(errInfo);
            return true;
        });

        /// <summary>
        /// Filter sections based on whether they need to be processed, 
        /// and when their memory size is smaller than the queried buffer, 
        /// set consecutive sections as a group, and finally return a list that is grouped accordingly.
        /// </summary>
        /// <param name="sectionKeys"></param>
        /// <param name="isFilter"></param>
        /// <param name="isFilterSize"></param>
        /// <param name="minLength">minimum buffer size (in bytes) in querying and pointerFinder, enter 0 to not use buffer, setting this value to 0 is better when the total number of Sections in the game is low. If the game has more than a thousand Sections, Buffer must be set</param>
        /// <param name="AddrMin"></param>
        /// <param name="AddrMax"></param>
        /// <param name="MinResultAccessFactor">Access value directly by address when the number of query results for the same Section is less than this factor, used to control whether to read Section data completely, or directly access the value by address. Default value is 50</param>
        /// <param name="MinResultAccessFactorThreshold">The MinResultAccessFactor option will only take effect when the section size is greater than this value, Default size is 1048576(1MB)</param>
        /// <returns></returns>
        private List<(int start, int end)> GetSectionRangeList(Section[] sectionKeys, bool isFilter, bool isFilterSize, ulong minLength, ulong AddrMin, ulong AddrMax, sbyte MinResultAccessFactor, uint MinResultAccessFactorThreshold)
        {
            int readCnt = 0;
            List<(int start, int end)> rangeList = new List<(int start, int end)>();
            (int start, int end) rangeIdx = (-1, -1);
            for (int sectionIdx = 0; sectionIdx < sectionKeys.Length; sectionIdx++)
            {
                readCnt++;
                bool isContinue = false;
                Section currentSection = sectionKeys[sectionIdx];
                if (!currentSection.Check || isFilter && currentSection.IsFilter || isFilterSize && currentSection.IsFilterSize) isContinue = true; //Check if section is not scanned
                else if (AddrMin > 0 && AddrMax > 0 && (currentSection.Start + (ulong)currentSection.Length < AddrMin || currentSection.Start > AddrMax)) isContinue = true;
                else if (currentSection.Length > MinResultAccessFactorThreshold && bitsDictDicts.Count > bitsDictDictsIdx && bitsDictDicts[bitsDictDictsIdx].Count > 0 && bitsDictDicts[bitsDictDictsIdx].ContainsKey(currentSection.SID) && bitsDictDicts[bitsDictDictsIdx][currentSection.SID].Count is int chkCnt && chkCnt > 0 && chkCnt < MinResultAccessFactor)
                { //Access value directly by address when the number of query results for the same Section is less than this MinResultAccessFactor
                    rangeList.Add((-1, sectionIdx));
                    isContinue = true;
                }
                else if ((ulong)currentSection.Length > minLength)
                {
                    rangeList.Add((sectionIdx, sectionIdx));
                    isContinue = true;
                }

                if (isContinue)
                {
                    if (rangeIdx.start != -1) //add rangeIdx when the start index is set
                    {
                        rangeList.Add(rangeIdx);
                        rangeIdx = (-1, -1);
                    }
                    continue;
                }
                else if (rangeIdx.start == -1) rangeIdx = (sectionIdx, sectionIdx);//set start and end index when not set

                Section startSection = sectionKeys[rangeIdx.start];
                ulong bufferSize = currentSection.Start + (ulong)currentSection.Length - startSection.Start;
                if (bufferSize >= int.MaxValue) //check the size of the scan to be executed, whether the scan size has been reached the upper limit
                {
                    if (rangeIdx.start != -1) //add rangeIdx when the start index is set
                    {
                        rangeList.Add(rangeIdx);
                        rangeIdx = (-1, -1);
                    }
                    rangeIdx = (sectionIdx, sectionIdx);
                    continue;
                }
                else if (bufferSize < minLength && (sectionIdx != sectionKeys.Length - 1))
                {
                    rangeIdx.end = sectionIdx;//update end index
                    continue;
                }

                rangeIdx.end = sectionIdx;//update end index
                rangeList.Add(rangeIdx);
                rangeIdx = (-1, -1); //initialize start and end index for non-isPerform scan
            }

            return rangeList;
        }

        #region ScanComparer
        /// <summary>
        /// During the initial scan, data matching the input conditions will be read from the PS4.
        /// Subsequent scans will read data from the addresses that matched the input conditions in the previous scan.
        /// </summary>
        /// <param name="buffer">Bytes data used for this comparison</param>
        /// <param name="section">Section for this comparison</param>
        /// <param name="scanStep">Scan base size for this comparison, 1(byte), 2(bytes), 4(bytes), 8(bytes), etc.</param>
        /// <param name="AddrMin">Minimum section address</param>
        /// <param name="AddrMax">Maximum section address</param>
        /// <param name="bitsDict">Used to store the new or previous comparison results</param>
        /// <param name="isCompareFirst">Compare to first scan will compare the current addresslist and it's value to the saved value of the first scan</param>
        /// <param name="bitsDictFirst">Store the first scan results</param>
        /// <returns></returns>
        private BitsDictionary Comparer(Byte[] buffer, Section section, int scanStep, ulong AddrMin, ulong AddrMax, BitsDictionary bitsDict, bool isCompareFirst, BitsDictionary bitsDictFirst)
        {
            (ulong Start, ulong End, bool Valid, byte Prot, string Name) localHiddenSection = default;
            if (comparerTool.ScanType_ == ScanType.HiddenSections)
            { 
                if (!mainForm.LocalHiddenSections.TryGetValue(section.SID, out localHiddenSection))
                    localHiddenSection = (section.Start, section.Start + (uint)section.Length, true, (byte)section.Prot, section.Name);
            }
            if (resultItems.Count == 0)
            {
                for (int scanIdx = 0; scanIdx + comparerTool.ScanTypeLength < buffer.LongLength; scanIdx += scanStep)
                {
                    if (section.Start + (ulong)scanIdx < AddrMin || section.Start + (ulong)scanIdx > AddrMax) continue;
                    if (scanSource.Token.IsCancellationRequested) break;

                    byte[] newValue = new byte[comparerTool.ScanTypeLength];
                    Buffer.BlockCopy(buffer, scanIdx, newValue, 0, comparerTool.ScanTypeLength);
                    if (comparerTool.Value0Byte == null)
                    {
                        ulong longValue = ScanTool.BytesToULong(newValue);
                        if (ScanTool.Comparer(comparerTool, ref longValue, 0, UnknownInitialScanDoNotSkip0))
                        {
                            if (comparerTool.ScanType_ == ScanType.AutoNumeric && longValue < 0xFFFFFFFF) newValue = BitConverter.GetBytes(longValue);
                            else if (comparerTool.ScanType_ == ScanType.HiddenSections && bitsDict.Count == 0) localHiddenSection.Start = section.Start + (uint)scanIdx;
                            bitsDict.Add((uint)scanIdx, newValue);
                        }
                    }
                    else if (ScanTool.ComparerExact(comparerTool, newValue, comparerTool.Value0Byte)) bitsDict.Add((uint)scanIdx, newValue);
                }
                if (comparerTool.ScanType_ == ScanType.HiddenSections)
                {
                    if (bitsDict.Count == 0) localHiddenSection.Valid = false;
                    else
                    {
                        (uint key, byte[] _) = bitsDict.Get(bitsDict.Count - 1);
                        localHiddenSection.End = section.Start + key;
                        localHiddenSection.Prot = 7;
                    }
                }
            }
            else
            {
                BitsDictionary newBitsDict = new BitsDictionary(scanStep, comparerTool.ScanTypeLength);

                bitsDict.Begin();
                for (int idx = 0; idx < bitsDict.Count; idx++)
                {
                    if (scanSource.Token.IsCancellationRequested) break;

                    (uint offsetAddr, byte[] oldBytes) = bitsDict.Get();
                    ulong address = section.Start + offsetAddr;
                    if (address < AddrMin || address > AddrMax) continue;

                    byte[] newValue;
                    if (isCompareFirst) newValue = bitsDictFirst != null ? bitsDictFirst[offsetAddr] : new byte[8];
                    else if(buffer != null && buffer.Length > 0)
                    {
                        newValue = new byte[comparerTool.ScanTypeLength];
                        Buffer.BlockCopy(buffer, (int)offsetAddr, newValue, 0, comparerTool.ScanTypeLength);
                    }
                    else newValue = PS4Tool.ReadMemory(section.PID, address, comparerTool.ScanTypeLength);

                    if (comparerTool.Value0Byte == null)
                    {
                        ulong oldData = ScanTool.BytesToULong(oldBytes);
                        ulong newData = ScanTool.BytesToULong(newValue);
                        if (ScanTool.Comparer(comparerTool, ref newData, oldData, UnknownInitialScanDoNotSkip0))
                        {
                            if (comparerTool.ScanType_ == ScanType.AutoNumeric && newData < 0xFFFFFFFF) newValue = BitConverter.GetBytes(newData);
                            else if (comparerTool.ScanType_ == ScanType.HiddenSections && newBitsDict.Count == 0) localHiddenSection.Start = address;
                            newBitsDict.Add(offsetAddr, newValue);
                        }
                    }
                    else if (ScanTool.ComparerExact(comparerTool, newValue, comparerTool.Value0Byte)) newBitsDict.Add(offsetAddr, newValue);
                }
                if (comparerTool.ScanType_ == ScanType.HiddenSections)
                {
                    if (newBitsDict.Count == 0) localHiddenSection.Valid = false;
                    else
                    {
                        (uint key, byte[] _) = newBitsDict.Get(newBitsDict.Count - 1);
                        localHiddenSection.End = section.Start + key;
                        localHiddenSection.Prot = 7;
                    }
                }
                bitsDict.Clear();
                bitsDict = newBitsDict;
            }
            if (comparerTool.ScanType_ == ScanType.HiddenSections) mainForm.LocalHiddenSections[section.SID] = localHiddenSection;
            return bitsDict;
        }

        /// <summary>
        /// This function is only used for the ScanType "Group". 
        /// In the initial scan, data that matches the input group conditions will be read from the PS4. 
        /// In subsequent scans, data that matches the input group conditions will be read from the addresses that matched the conditions in the previous scan.
        /// </summary>
        /// <param name="buffer">Bytes data used for this comparison</param>
        /// <param name="section">Section for this comparison</param>
        /// <param name="scanStep">Scan base size for this comparison, 1(byte), 2(bytes), 4(bytes), 8(bytes), etc.</param>
        /// <param name="AddrMin">Minimum section address</param>
        /// <param name="AddrMax">Maximum section address</param>
        /// <param name="bitsDict">Used to store the new or previous comparison results</param>
        /// <param name="isCompareFirst">Compare to first scan will compare the current addresslist and it's value to the saved value of the first scan</param>
        /// <param name="bitsDictFirst">Store the first scan results</param>
        /// <returns></returns>
        private BitsDictionary ComparerGroup(Byte[] buffer, Section section, int scanStep, ulong AddrMin, ulong AddrMax, BitsDictionary bitsDict, bool isCompareFirst, BitsDictionary bitsDictFirst)
        {
            if (resultItems.Count == 0)
            {
                for (int scanIdx = 0; scanIdx + comparerTool.GroupFirstLength < buffer.LongLength; scanIdx += scanStep)
                {
                    if (scanSource.Token.IsCancellationRequested) break;
                    if (section.Start + (ulong)scanIdx < AddrMin || section.Start + (ulong)scanIdx > AddrMax) continue;

                    int firstScanIdx = scanIdx;
                    for (int gIdx = 0; gIdx < comparerTool.GroupTypes.Count; gIdx++)
                    {
                        (ScanType groupScanType, int groupTypeLength, bool isAny, _) = comparerTool.GroupTypes[gIdx];
                        if (scanIdx + groupTypeLength > buffer.LongLength) break;
                        bool comparer = false;
                        if (!isAny)
                        {
                            byte[] valueBytes = comparerTool.GroupValues[gIdx];
                            byte[] newGroupBytes = new byte[groupTypeLength];
                            Buffer.BlockCopy(buffer, scanIdx, newGroupBytes, 0, groupTypeLength);
                            comparer = ScanTool.ComparerExact(comparerTool, newGroupBytes, valueBytes, groupScanType);
                        }

                        if (!isAny && !comparer)
                        {
                            if (scanIdx > firstScanIdx) scanIdx = firstScanIdx;
                            break;
                        }
                        else
                        {
                            if (isAny && comparerTool.IsFloatingSimpleValues && (groupScanType == ScanType.Double_ || groupScanType == ScanType.Float_))
                            {
                                byte[] newGroupBytes = new byte[groupTypeLength];
                                Buffer.BlockCopy(buffer, scanIdx, newGroupBytes, 0, groupTypeLength);
                                if (groupScanType == ScanType.Double_)
                                {
                                    ulong newVar = BitConverter.ToUInt64(newGroupBytes, 0);
                                    if (newVar > 0 && Math.Abs(1023 - (int)(((long)newVar >> 52) & 0x7ffL)) > floatingSimpleValueExponents)
                                    {
                                        if (scanIdx > firstScanIdx) scanIdx = firstScanIdx;
                                        break;
                                    }
                                }
                                else if (groupScanType == ScanType.Float_)
                                {
                                    uint newVar = BitConverter.ToUInt32(newGroupBytes, 0);
                                    if (newVar > 0 && Math.Abs(127 - (int)(((int)newVar >> 23) & 0xffL)) > floatingSimpleValueExponents)
                                    {
                                        if (scanIdx > firstScanIdx) scanIdx = firstScanIdx;
                                        break;
                                    }
                                }
                            }
                            if (gIdx == comparerTool.GroupTypes.Count - 1)
                            {
                                byte[] newBytes = new byte[comparerTool.ScanTypeLength];
                                Buffer.BlockCopy(buffer, firstScanIdx, newBytes, 0, comparerTool.ScanTypeLength);
                                bitsDict.Add((uint)firstScanIdx, newBytes);
                                if (scanStep < groupTypeLength) scanIdx += groupTypeLength - scanStep;
                                break;
                            }
                            scanIdx += groupTypeLength;
                        }
                    }
                }
            }
            else
            {
                BitsDictionary newBitsDict = new BitsDictionary(scanStep, comparerTool.ScanTypeLength);

                bitsDict.Begin();
                for (int idx = 0; idx < bitsDict.Count; idx++)
                {
                    if (scanSource.Token.IsCancellationRequested) break;
                    (uint offsetAddr, _) = bitsDict.Get();
                    if (section.Start + offsetAddr < AddrMin || section.Start + offsetAddr > AddrMax) continue;

                    byte[] newBytes;
                    if (isCompareFirst) newBytes = bitsDictFirst != null ? bitsDictFirst[offsetAddr] : new byte[comparerTool.ScanTypeLength];
                    else if (buffer != null && buffer.Length > 0)
                    {
                        newBytes = new byte[comparerTool.ScanTypeLength];
                        Buffer.BlockCopy(buffer, (int)offsetAddr, newBytes, 0, comparerTool.ScanTypeLength);
                    }
                    else newBytes = PS4Tool.ReadMemory(section.PID, offsetAddr + section.Start, comparerTool.ScanTypeLength);

                    int scanOffset = 0;
                    for (int gIdx = 0; gIdx < comparerTool.GroupTypes.Count; gIdx++)
                    {
                        (ScanType groupScanType, int groupTypeLength, bool isAny, _) = comparerTool.GroupTypes[gIdx];
                        byte[] valueBytes = comparerTool.GroupValues[gIdx];
                        byte[] newGroupBytes = new byte[groupTypeLength];
                        Buffer.BlockCopy(newBytes, scanOffset, newGroupBytes, 0, groupTypeLength);
                        if (!isAny && !ScanTool.ComparerExact(comparerTool, newGroupBytes, valueBytes, groupScanType)) break;
                        else if (isAny && comparerTool.IsFloatingSimpleValues && (groupScanType == ScanType.Double_ || groupScanType == ScanType.Float_))
                        {
                            if (groupScanType == ScanType.Double_)
                            {
                                ulong newVar = BitConverter.ToUInt64(newGroupBytes, 0);
                                if (newVar > 0 && Math.Abs(1023 - (int)(((long)newVar >> 52) & 0x7ffL)) > floatingSimpleValueExponents) break;
                            }
                            else if (groupScanType == ScanType.Float_)
                            {
                                uint newVar = BitConverter.ToUInt32(newGroupBytes, 0);
                                if (newVar > 0 && Math.Abs(127 - (int)(((int)newVar >> 23) & 0xffL)) > floatingSimpleValueExponents) break;
                            }
                        }
                        if (gIdx == comparerTool.GroupTypes.Count - 1) newBitsDict.Add(offsetAddr, newBytes);
                        scanOffset += groupTypeLength;
                    }
                }
                bitsDict.Clear();
                bitsDict = newBitsDict;
            }

            return bitsDict;
        }
        #endregion

        /// <summary>
        /// After scanning or refreshing tasks are completed, display the found data in the ResultView.
        /// </summary>
        /// <param name="tickerMajor"></param>
        private void TaskCompleted(System.Diagnostics.Stopwatch tickerMajor)
        {
            try
            {
                if (bitsDictDicts[bitsDictDictsIdx].Count <= 0 && !enableUndoScan)
                {
                    Invoke(new MethodInvoker(() => { NewBtn.PerformClick(); }));
                    return;
                }
                string msg = "";
                Invoke(new MethodInvoker(() => { msg = ToolStripMsg.Text; }));

                hitCnt = 0;
                int chkHitCnt = 0;
                uint MaxResultShow = Properties.Settings.Default.MaxResultShow.Value;
                MaxResultShow = MaxResultShow == 0 ? 0x2000 : MaxResultShow;
                Invoke(new MethodInvoker(() =>
                {
                    ResultView.BeginUpdate();
                    ResultView.VirtualListSize = 0;
                    resultItems.Clear();
                    ResultView.VirtualMode = false;
                    ResultView.Items.Clear();
                }));
                List<uint> sectionKeys = new List<uint>(bitsDictDicts[bitsDictDictsIdx].Keys);
                sectionKeys.Sort();
                Color backColor = default;
                for (int sectionIdx = 0; sectionIdx < sectionKeys.Count; sectionIdx++)
                {
                    if (scanSource != null && scanSource.Token.CanBeCanceled) scanSource.Token.ThrowIfCancellationRequested();
                    if (refreshSource != null && refreshSource.Token.CanBeCanceled) refreshSource.Token.ThrowIfCancellationRequested();
                    uint dictKey = sectionKeys[sectionIdx];
                    Section section = null;
                    BitsDictionary bitsDict = null;
                    if (sectionTool.SectionDict.ContainsKey(dictKey))
                    {
                        section = sectionTool.SectionDict[sectionKeys[sectionIdx]];
                        bitsDictDicts[bitsDictDictsIdx].TryGetValue(section.SID, out bitsDict);
                    }
                    if (bitsDict == null || bitsDict.Count == 0) continue;

                    hitCnt += bitsDict.Count;

                    bitsDict.Begin();
                    for (int idx = 0; idx < bitsDict.Count; idx++)
                    {
                        if (scanSource != null && scanSource.Token.CanBeCanceled) scanSource.Token.ThrowIfCancellationRequested();
                        if (refreshSource != null && refreshSource.Token.CanBeCanceled) refreshSource.Token.ThrowIfCancellationRequested();
                        if (++chkHitCnt >= MaxResultShow) break;

                        (uint offsetAddr, byte[] oldBytes) = bitsDict.Get();

                        if (comparerTool.ScanType_ != ScanType.Group)
                        {
                            string typeStr = "";
                            string valueStr = "";
                            string valueHex = "";

                            if (comparerTool.ScanType_ == ScanType.AutoNumeric)
                            {
                                bool isHit = false;
                                string signString = "";
                                ScanType valueType = comparerTool.ScanType_;
                                if (comparerTool.AutoNumericValid.UInt)
                                {
                                    ulong valueUlong = ScanTool.BytesToULong(oldBytes);
                                    if (isUnknownInitial)
                                    {
                                        if (valueUlong <= 0xFF) valueType = ScanType.Byte_;
                                        else if (valueUlong <= 0xFFFF) valueType = ScanType.Bytes_2;
                                        else if (valueUlong <= 0xFFFFFFFF) valueType = ScanType.Bytes_4;
                                        else valueType = ScanType.Bytes_8;
                                    }
                                    else if (comparerTool.Input0UInt64 <= 0xFF) //255
                                    {
                                        valueType = ScanType.Byte_;
                                        valueUlong = BitConverter.GetBytes(valueUlong)[0];
                                        if (comparerTool.IsValue0Signed) signString = ((sbyte)valueUlong).ToString();
                                    }
                                    else if (comparerTool.Input0UInt64 <= 0xFFFF) //65535
                                    {
                                        valueType = ScanType.Bytes_2;
                                        valueUlong = (UInt16)valueUlong;
                                        if (comparerTool.IsValue0Signed) signString = ((Int16)valueUlong).ToString();
                                    }
                                    else if (comparerTool.Input0UInt64 <= 0xFFFFFFFF) //4294967295
                                    {
                                        valueType = ScanType.Bytes_4;
                                        valueUlong = (UInt32)valueUlong;
                                        if (comparerTool.IsValue0Signed) signString = ((Int32)valueUlong).ToString();
                                    }
                                    else
                                    {
                                        valueType = ScanType.Bytes_8;
                                        if (comparerTool.IsValue0Signed) signString = ((Int64)valueUlong).ToString();
                                    }

                                    if (valueUlong - comparerTool.Input0UInt64 == 0 || isUnknownInitial)
                                    {
                                        isHit = true;
                                        valueStr = comparerTool.IsValue0Signed ? signString : valueUlong.ToString();
                                    }
                                }
                                if (!isHit && comparerTool.AutoNumericValid.Double && BitConverter.ToDouble(oldBytes, 0) is double valueDouble && Math.Abs(valueDouble - comparerTool.Input0Double) < 1)
                                {
                                    valueType = ScanType.Double_;
                                    valueStr = valueDouble.ToString();
                                }
                                else if (!isHit && comparerTool.AutoNumericValid.Float && BitConverter.ToSingle(oldBytes, 0) is float valueFloat && Math.Abs(valueFloat - comparerTool.Input0Float) < 1)
                                {
                                    valueType = ScanType.Float_;
                                    valueStr = ((double)valueFloat).ToString();
                                }
                                typeStr = valueType.GetDescription();
                                valueHex = ScanTool.BytesToString(valueType, oldBytes, true, false);
                            }
                            else
                            {
                                typeStr = comparerTool.ScanType_.GetDescription();
                                valueStr = ScanTool.BytesToString(comparerTool.ScanType_, oldBytes, false, comparerTool.IsValue0Signed);
                                valueHex = ScanTool.BytesToString(comparerTool.ScanType_, oldBytes, true, false);
                            }
                            ListViewItem resultItem = new ListViewItem((offsetAddr + section.Start).ToString("X8"), 0)
                            {
                                Name = offsetAddr.ToString("X8"),
                                Tag = (section.SID, bitsDict.Index)
                            };
                            resultItem.SubItems.Add(typeStr);
                            resultItem.SubItems.Add(valueStr);
                            resultItem.SubItems.Add(valueHex);
                            resultItem.SubItems.Add(string.Format("{0}_{1}_{2}_{3}", section.Start.ToString("X"), section.Name, section.Prot.ToString("X"), section.Offset.ToString("X")));
                            resultItems.Add(resultItem);
                        }
                        else
                        {
                            int scanOffset = 0;
                            backColor = backColor == default ? Color.DarkSlateGray : default;
                            for (int gIdx = 0; gIdx < comparerTool.GroupTypes.Count; gIdx++)
                            {
                                (ScanType scanType, int groupTypeLength, bool isAny, bool isSign) group = comparerTool.GroupTypes[gIdx];
                                byte[] oldGroupBytes = new byte[group.groupTypeLength];
                                Buffer.BlockCopy(oldBytes, scanOffset, oldGroupBytes, 0, group.groupTypeLength);
                                string typeStr = group.scanType.GetDescription();
                                string valueStr = ScanTool.BytesToString(group.scanType, oldGroupBytes, false, group.isSign);
                                string valueHex = ScanTool.BytesToString(group.scanType, oldGroupBytes, true, false);

                                ListViewItem resultItem = new ListViewItem((offsetAddr + section.Start + (uint)scanOffset).ToString("X8"), 0)
                                {
                                    Name = offsetAddr.ToString("X8"),
                                    Tag = (section.SID, bitsDict.Index)
                                };
                                resultItem.SubItems.Add(typeStr);
                                resultItem.SubItems.Add(valueStr);
                                resultItem.SubItems.Add(valueHex);
                                resultItem.SubItems.Add(string.Format("{0}_{1}_{2}_{3}", section.Start.ToString("X"), section.Name, section.Prot.ToString("X"), section.Offset.ToString("X")));
                                resultItem.BackColor = backColor;
                                resultItems.Add(resultItem);
                                scanOffset += group.groupTypeLength;
                            }
                        }
                        if (chkHitCnt % 0x2000 == 0)
                        {
                            Invoke(new MethodInvoker(() =>
                            {
                                ResultView.VirtualListSize = resultItems.Count;
                                ToolStripMsg.Text = string.Format("{0}; Processed elapsed:{1:0.00}s, ListView:{2}/{3}", msg, tickerMajor.Elapsed.TotalSeconds, chkHitCnt, sectionIdx + 1);
                            }));
                        }
                    }
                    if (resultItems.Count > 0)
                    {
                        Invoke(new MethodInvoker(() =>
                        {
                            ResultView.VirtualListSize = resultItems.Count;
                            ToolStripMsg.Text = string.Format("{0}; Processed elapsed:{1:0.00}s, ListView:{2}/{3}", msg, tickerMajor.Elapsed.TotalSeconds, chkHitCnt, sectionIdx + 1);
                        }));
                    }
                }
            }
            finally
            {
                if (tickerMajor != null) tickerMajor.Stop();
                Invoke(new MethodInvoker(() => {
                    ResultView.VirtualMode = true;
                    ResultView.EndUpdate();
                    ScanTypeBox_SelectedIndexChanged(null, null);
                    ScanBtn.Text = "Next Scan";
                    if (AddrIsFilterBox.Checked) AddrIsFilterBox.Enabled = false;
                    if (!CompareFirstBox.Enabled) CompareFirstBox.Enabled = true;
                }));
            }
        }
        #endregion

        Task<bool> refreshTask = null;
        CancellationTokenSource refreshSource = null;
        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (resultItems.Count == 0) return; 
                else if (refreshTask != null && !refreshTask.IsCompleted) return;
                else
                {
                    ComboItem process = (ComboItem)ProcessesBox.SelectedItem;
                    int pid = (int)process.Value;
                    libdebug.ProcessMap pMap = PS4Tool.GetProcessMaps(pid);
                    if (pMap.entries == null) return;
                }

                if (refreshSource != null) refreshSource.Dispose();
                System.Diagnostics.Stopwatch tickerMajor = System.Diagnostics.Stopwatch.StartNew();
                refreshSource = new CancellationTokenSource();
                refreshTask = RefreshTask(IsFilterBox.Checked, IsFilterSizeBox.Checked, tickerMajor);
                refreshTask.ContinueWith(t => TaskCompleted(tickerMajor))
                    .ContinueWith(t => {
                        refreshSource?.Dispose();
                        refreshSource = null;
                        refreshTask?.Dispose();
                        refreshTask = null;
                    }); ;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Source + ":RefreshBtn_Click", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        //Invoke(new MethodInvoker(() => { }));
        private async Task<bool> RefreshTask(bool isFilter, bool isFilterSize, System.Diagnostics.Stopwatch tickerMajor) => await Task.Run(() => {
            try
            {
                if (bitsDictDicts[bitsDictDictsIdx].Count == 0) return false;

                int hitCnt = 0;
                int count = 0;

                Invoke(new MethodInvoker(() => { ToolStripBar.Value = 1; }));

                byte MaxQueryThreads                = Properties.Settings.Default.MaxQueryThreads.Value;
                uint QueryBufferSize                = Properties.Settings.Default.QueryBufferSize.Value;
                uint MaxResultShow                  = Properties.Settings.Default.MaxResultShow.Value;
                uint SectionFilterSize              = Properties.Settings.Default.SectionFilterSize.Value;
                string SectionFilterKeys            = Properties.Settings.Default.SectionFilterKeys.Value;
                sbyte MinResultAccessFactor         = Properties.Settings.Default.MinResultAccessFactor.Value;
                uint MinResultAccessFactorThreshold = Properties.Settings.Default.MinResultAccessFactorThreshold.Value;
                MaxQueryThreads = MaxQueryThreads == (byte)0 ? (byte)1 : MaxQueryThreads;
                MaxResultShow = MaxResultShow == 0 ? 0x2000 : MaxResultShow;
                SectionFilterKeys = Regex.Replace(SectionFilterKeys, " *[,;] *", "|");

                ulong minLength = QueryBufferSize * 1024 * 1024; //set the minimum read size in bytes

                Section[] sectionKeys   = sectionTool.GetSectionSortByAddr(bitsDictDicts[bitsDictDictsIdx].Keys);
                SemaphoreSlim semaphore = new SemaphoreSlim(MaxQueryThreads);
                List<Task<bool>> tasks  = new List<Task<bool>>();
                List<(int start, int end)> rangeList = GetSectionRangeList(sectionKeys, isFilter, isFilterSize, minLength, 0, 0, MinResultAccessFactor, MinResultAccessFactorThreshold);

                int sectionSelectedCnt = 0;
                for (int idx = 0; idx < rangeList.Count; idx++)
                {
                    (int start, int end) range = rangeList[idx];
                    sectionSelectedCnt += range.start == -1 ? 1 : (range.end - range.start + 1);
                    tasks.Add(Task.Run<bool>(() =>
                    {
                        try
                        {
                            semaphore.Wait();
                            refreshSource.Token.ThrowIfCancellationRequested();
                            byte[] buffer = null;
                            Section sectionStart = range.start == -1 ? null : sectionKeys[range.start];
                            Section sectionEnd = sectionKeys[range.end];

                            if (sectionStart != null)
                            {
                                ulong bufferSize = sectionEnd.Start + (ulong)sectionEnd.Length - sectionStart.Start;
                                buffer = PS4Tool.ReadMemory(sectionStart.PID, sectionStart.Start, (int)bufferSize);
                            }
                            if (range.start == -1)
                            {
                                range.start = range.end;
                                sectionStart = sectionEnd;
                                buffer = PS4Tool.ReadMemory(sectionEnd.PID, sectionEnd.Start, (int)sectionEnd.Length);
                            }

                            for (int rIdx = range.start; rIdx <= range.end; rIdx++)
                            {
                                Section addrSection = sectionKeys[rIdx];
                                int scanOffset = (int)(addrSection.Start - sectionStart.Start);

                                refreshSource.Token.ThrowIfCancellationRequested();
                                Byte[] subBuffer = null;
                                bitsDictDicts[bitsDictDictsIdx].TryGetValue(addrSection.SID, out BitsDictionary bitsDict);
                                if (buffer != null)//bitsDict.Count == 0 || bitsDict.Count > MinResultAccessFactor)
                                {
                                    subBuffer = new Byte[addrSection.Length];
                                    Buffer.BlockCopy(buffer, (int)scanOffset, subBuffer, 0, addrSection.Length);
                                }

                                bitsDict.Begin();
                                for (int bIdx = 0; bIdx < bitsDict.Count; bIdx++)
                                {
                                    if (++hitCnt > MaxResultShow && MaxQueryThreads == 1) continue;

                                    (uint offsetAddr, _) = bitsDict.Get();
                                    byte[] newBytes = null;
                                    if (subBuffer != null && subBuffer.Length > 0)
                                    {
                                        newBytes = new byte[comparerTool.ScanTypeLength];
                                        Buffer.BlockCopy(subBuffer, (int)offsetAddr, newBytes, 0, comparerTool.ScanTypeLength);
                                    }
                                    else newBytes = PS4Tool.ReadMemory(addrSection.PID, offsetAddr + addrSection.Start, comparerTool.ScanTypeLength);
                                    bitsDict.Set(newBytes);
                                }
                                Invoke(new MethodInvoker(() =>
                                {
                                    ToolStripBar.Value = (int)(((float)(++count) / sectionKeys.Length) * 100);
                                    ToolStripMsg.Text = string.Format("Refresh elapsed:{0:0.00}s. Count: {1}, Section: {2}/{3}(selected/total)", tickerMajor.Elapsed.TotalSeconds, hitCnt, sectionSelectedCnt, sectionItems.Count);
                                }));
                            }
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                        return true;
                    }));
                }
                Task whenTasks = Task.WhenAll(tasks);
                whenTasks.Wait();
                semaphore.Dispose();
                whenTasks.Dispose();
                GC.Collect();
                Invoke(new MethodInvoker(() => {
                    ToolStripBar.Value = 100;
                    ToolStripMsg.Text = string.Format("Refresh elapsed:{0:0.00}s. Count: {1}, Section: {2}/{3}(selected/total)", tickerMajor.Elapsed.TotalSeconds, hitCnt, sectionSelectedCnt, sectionItems.Count);
                }));
            }
            catch (Exception ex)
            {
                if (ex.InnerException is OperationCanceledException)
                {
                    Invoke(new MethodInvoker(() => {
                        ToolStripBar.Value = 100;
                        ToolStripMsg.Text = string.Format("Refresh elapsed:{0:0.00}s. RefreshTask canceled. {1}", tickerMajor.Elapsed.TotalSeconds, ex.InnerException.Message);
                    }));
                }
                else MessageBox.Show(ex.ToString(), ex.Source + ":RefreshTask", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
            ValueBox.Enabled = true;
            CompareTypeBox.Items.Clear();
            ScanType scanType = this.ParseFromDescription<ScanType>(ScanTypeBox.SelectedItem.ToString());
            switch (scanType)
            {
                case ScanType.Bytes_8:
                case ScanType.Bytes_4:
                case ScanType.Bytes_2:
                case ScanType.Byte_:
                    if (resultItems.Count == 0) CompareTypeBox.Items.AddRange(Constant.SearchByBytesFirst);
                    else CompareTypeBox.Items.AddRange(Constant.SearchByBytesNext);
                    break;
                case ScanType.Double_:
                case ScanType.Float_:
                case ScanType.AutoNumeric:
                    if (resultItems.Count == 0) CompareTypeBox.Items.AddRange(Constant.SearchByFloatFirst);
                    else CompareTypeBox.Items.AddRange(Constant.SearchByFloatNext);
                    break;
                case ScanType.Hex:
                case ScanType.Group:
                case ScanType.String_:
                    NotBox.Checked = false;
                    NotBox.Enabled = false;
                    if (scanType == ScanType.Hex)
                    {
                        HexBox.Enabled = true;
                        HexBox.Checked = true;
                    }
                    else
                    {
                        HexBox.Enabled = false;
                        HexBox.Checked = false;
                    }
                    AlignmentBox.Enabled = scanType != ScanType.Group ? false : true;
                    AlignmentBox.Checked = false;
                    CompareTypeBox.Items.AddRange(Constant.SearchByHex);
                    break;
                case ScanType.HiddenSections:
                    NotBox.Checked = true;
                    NotBox.Enabled = false;
                    HexBox.Enabled = false;
                    HexBox.Checked = false;
                    AlignmentBox.Enabled = true;
                    AlignmentBox.Checked = false;
                    ValueBox.Text = "0";
                    ValueBox.Enabled = false;
                    SectionViewContains(true, "Hidden");
                    CompareTypeBox.Items.AddRange(Constant.SearchByHex);
                    break;
                default:
                    throw new Exception("ScanType verification failed");
            }
            if (scanType == ScanType.Float_ || scanType == ScanType.Double_ || scanType == ScanType.Group) SimpleValuesBox.Show();
            else
            {
                SimpleValuesBox.Checked = false;
                SimpleValuesBox.Hide();
            }

            int listIdx = 0;
            int listCount = CompareTypeBox.Items.Count;
            for (; listIdx < listCount; ++listIdx)
                if (selectedCompareType != null && (CompareType)CompareTypeBox.Items[listIdx] == (CompareType)selectedCompareType) break;
            CompareTypeBox.SelectedIndex = listIdx == listCount ? 0 : listIdx;
        }

        private void CompareTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Enum.TryParse(CompareTypeBox.SelectedItem.ToString(), out CompareType compareType);

            switch (compareType)
            { //Show second input column when Between
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
            switch (compareType)
            { //Hide not checkbox when not applicable
                case CompareType.IncreasedBy:
                case CompareType.DecreasedBy:
                case CompareType.UnknownInitial:
                    NotBox.Checked = false;
                    NotBox.Enabled = false;
                    break;
                default:
                    ScanType scanType = this.ParseFromDescription<ScanType>(ScanTypeBox.SelectedItem.ToString());
                    if (scanType != ScanType.String_ && scanType != ScanType.Hex && scanType != ScanType.Group && scanType != ScanType.HiddenSections) NotBox.Enabled = true;
                    break;
            }
            switch (compareType)
            { //Hide input column when not applicable
                case CompareType.Increased:
                case CompareType.Decreased:
                case CompareType.Changed:
                case CompareType.Unchanged:
                case CompareType.UnknownInitial:
                    ValueBox.Hide();
                    break;
                default:
                    ValueBox.Show();
                    break;
            }
        }

        private void ResumeBtn_Click(object sender, EventArgs e)
        {
            processStatus = ProcessStatus.Resume;
            ComboItem process = (ComboItem)ProcessesBox.SelectedItem;
            PS4Tool.AttachDebugger((int)process.Value, (string)process.Text, processStatus);
        }

        private void PauseBtn_Click(object sender, EventArgs e)
        {
            processStatus = ProcessStatus.Pause;
            ComboItem process = (ComboItem)ProcessesBox.SelectedItem;
            PS4Tool.AttachDebugger((int)process.Value, (string)process.Text, processStatus);
        }
        #endregion

        #region SectionView
        private void SectionView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e) => e.Item = sectionItems.Count > e.ItemIndex ? sectionItems[e.ItemIndex] : null;

        private void SectionView_SearchForVirtualItem(object sender, SearchForVirtualItemEventArgs e)
        {
            string searchTerm = e.Text;
            for (int i = e.StartIndex; i < sectionItems.Count; i++)
            {
                ListViewItem item = sectionItems[i];
                if (!item.SubItems[(int)SectionCol.SectionViewName].Text.Contains(searchTerm)) continue;
                e.Index = i;
                return;
            }

            e.Index = -1;
        }

        /// <summary>
        /// When ListView is enabled with VirtualMode, the ItemCheck and ItemChecked events are not triggered.
        /// </summary>
        private void SectionView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            ListViewItem item = ((ListView)sender).GetItemAt(e.X, e.Y);
            if (item == null) return;

            ListViewItem.ListViewSubItem subitem = item.GetSubItemAt(e.X, e.Y);
            if (item.SubItems[(int)SectionCol.SectionViewID] != subitem) return;

            item.Checked = !item.Checked;

            uint sid = uint.Parse(item.SubItems[(int)SectionCol.SectionViewSID].Text);
            SectionCheckUpdate(item.Checked, sid);

            if (AddrMinBox.Tag != null) AddrMinBox.Text = ((ulong)AddrMinBox.Tag).ToString("X");
            if (AddrMaxBox.Tag != null) AddrMaxBox.Text = ((ulong)AddrMaxBox.Tag).ToString("X");
            if (scanTask == null || scanTask.IsCompleted)
                ToolStripMsg.Text = string.Format("Total section: {0}, Selected section: {1}, Search size: {2}MB", SectionView.Items.Count, sectionTool.TotalSelected, sectionTool.TotalMemorySize / (1024 * 1024));

            item.BackColor = item.Checked ? querySectionViewItemCheck1BackColor : querySectionViewItemCheck2BackColor; //Color.DarkSlateGray : Color.DarkGreen;
        }

        /// <summary>
        /// When the Checked status of SectionView is modified, 
        /// verify and synchronize the Checked status of SectionDict and 
        /// update the values of TotalSelected, TotalMemorySize, AddrMinBox, and AddrMaxBox.
        /// </summary>
        /// <param name="isChecked"></param>
        /// <param name="sid"></param>
        /// <param name="force"></param>
        private void SectionCheckUpdate(bool isChecked, uint sid, bool force = false)
        {
            Section section = sectionTool.SectionDict[sid];
            SectionCheckUpdate(isChecked, section, force);
        }

        /// <summary>
        /// When the Checked status of SectionView is modified, 
        /// verify and synchronize the Checked status of SectionDict and 
        /// update the values of TotalSelected, TotalMemorySize, AddrMinBox, and AddrMaxBox.
        /// </summary>
        /// <param name="isChecked"></param>
        /// <param name="section"></param>
        /// <param name="force"></param>
        private void SectionCheckUpdate(bool isChecked, Section section, bool force = false)
        {
            if (section.Check == isChecked && !force) return;

            section.Check = isChecked;
            if (section.Check)
            {
                sectionTool.TotalSelected += 1;
                sectionTool.TotalMemorySize += (ulong)section.Length;
                if (AddrMinBox.Tag == null)
                {
                    if (AddrMinBox.Text != "") AddrMinBox.Tag = ParseHexAddrText(AddrMinBox.Text); 
                    else AddrMinBox.Tag = section.Start;
                }
                else
                {
                    var AddrMin = (ulong)AddrMinBox.Tag;//ParseHexAddrText(AddrMinBox.Text);
                    if (section.Start < AddrMin) AddrMinBox.Tag = section.Start;
                }
                ulong sectionEnd = section.Start + (ulong)section.Length;
                if (AddrMaxBox.Tag == null)
                {
                    if (AddrMaxBox.Text != "") AddrMaxBox.Tag = ParseHexAddrText(AddrMaxBox.Text);
                    else AddrMaxBox.Tag = sectionEnd;
                }
                else
                {
                    var AddrMax = (ulong)AddrMaxBox.Tag;//ParseHexAddrText(AddrMaxBox.Text);
                    if (sectionEnd > AddrMax) AddrMaxBox.Tag = sectionEnd;
                }
            }
            else
            {
                sectionTool.TotalSelected -= 1;
                sectionTool.TotalMemorySize -= (ulong)section.Length;
            }
        }

        private void SelectAllBox_CheckedChanged(object sender, EventArgs e)
        {
            bool check = SelectAllBox.Checked;
            SectionView.BeginUpdate();
            for (int idx = 0; idx < sectionItems.Count; ++idx)
            {
                if (sectionItems[idx].Checked == check) continue;

                ListViewItem item = sectionItems[idx];
                item.Checked = check;
                uint sid = uint.Parse(item.SubItems[(int)SectionCol.SectionViewSID].Text);
                SectionCheckUpdate(check, sid);
            }
            if (AddrMinBox.Tag != null) AddrMinBox.Text = ((ulong)AddrMinBox.Tag).ToString("X");
            if (AddrMaxBox.Tag != null) AddrMaxBox.Text = ((ulong)AddrMaxBox.Tag).ToString("X");
            ToolStripMsg.Text = string.Format("Total section: {0}, Selected section: {1}, Search size: {2}MB", sectionItems.Count, sectionTool.TotalSelected, sectionTool.TotalMemorySize / (1024 * 1024));
            SectionView.EndUpdate();
            if (!check)
            {
                AddrMinBox.Tag = null;
                AddrMaxBox.Tag = null;
                AddrMinBox.Text = "";
                AddrMaxBox.Text = "";
            }
        }

        private void AddrMinMaxBox_Leave(object sender, EventArgs e)
        {
            if (!AddrIsFilterBox.Checked) return;
            FilterChecked("filterAddr", AddrIsFilterBox.Checked);
        }

        private void AddrIsFilterBox_CheckedChanged(object sender, EventArgs e)
        {
            FilterChecked("filterAddr", AddrIsFilterBox.Checked);
            if (AddrIsFilterBox.Checked && bitsDictDicts.Count > 0) AddrIsFilterBox.Enabled = false;
        }

        private void IsFilterSizeBox_CheckedChanged(object sender, EventArgs e) => FilterChecked("filterSize", IsFilterSizeBox.Checked);

        private void IsFilterBox_CheckedChanged(object sender, EventArgs e) => FilterChecked("filter", IsFilterBox.Checked);

        private void FilterChecked(string filter, bool isFilterChecked)
        {
            int idx = ProcessesBox.SelectedIndex;
            if (idx == -1) return;
            if (filter == "filterAddr" && AddrMinBox.Text.Length > 0 && AddrMaxBox.Text.Length > 0)
            {
                AddrMinBox.Tag = null;
                AddrMaxBox.Tag = null;
            }
            if (filter != "filterAddr" || !isFilterChecked) ProcessesBox_SelectedIndexChanged(ProcessesBox, null);
            if (isFilterChecked)
            {
                SectionView.BeginUpdate();
                for (int sIdx = 0; sIdx < sectionItems.Count; sIdx++)
                {
                    ListViewItem item = sectionItems[sIdx];

                    if (filter == "filterAddr" && AddrMinBox.Text.Length > 0 && AddrMaxBox.Text.Length > 0)
                    {
                        ulong start = ulong.Parse(item.SubItems[(int)SectionCol.SectionViewAddress].Text, NumberStyles.HexNumber);
                        uint length = uint.Parse(item.SubItems[(int)SectionCol.SectionViewLength].Text.Replace("KB", "")) * 1024;
                        if (AddrMinBox.Tag == null) AddrMinBox.Tag = ParseHexAddrText(AddrMinBox.Text);
                        if (AddrMaxBox.Tag == null)
                        {
                            AddrMaxBox.Tag = ParseHexAddrText(AddrMaxBox.Text);
                            if ((ulong)AddrMinBox.Tag >= (ulong)AddrMaxBox.Tag) break;
                        }
                        if ((ulong)AddrMinBox.Tag <= start && (ulong)AddrMaxBox.Tag >= start + length) continue;
                    }
                    else if (!filter.Equals(item.Tag)) continue;

                    uint sid = uint.Parse(item.SubItems[(int)SectionCol.SectionViewSID].Text);
                    SectionCheckUpdate(false, sid);
                    sectionItems.Remove(item);
                    --sIdx;
                }
                if (AddrMinBox.Tag != null) AddrMinBox.Text = ((ulong)AddrMinBox.Tag).ToString("X");
                if (AddrMaxBox.Tag != null) AddrMaxBox.Text = ((ulong)AddrMaxBox.Tag).ToString("X");
                ToolStripMsg.Text = string.Format("Total section: {0}, Selected section: {1}, Search size: {2}MB", sectionItems.Count, sectionTool.TotalSelected, sectionTool.TotalMemorySize / (1024 * 1024));
                SectionView.VirtualListSize = sectionItems.Count;
                SectionView.EndUpdate();
            }
        }

        private string searchSectionName = "";
        private void SectionSearchBtn_Click(object sender, EventArgs e)
        {
            if (InputBox.Show("Search", "Enter the value of the search section name", ref searchSectionName) != DialogResult.OK) return;

            int startIndex = 0;
            List<ListViewItem> selectedItems = ListViewLVITEM.GetSelectedItems(SectionView);
            if (selectedItems.Count > 0 && selectedItems[selectedItems.Count - 1].Index + 1 < sectionItems.Count) startIndex = selectedItems[selectedItems.Count - 1].Index + 1;
            ListViewItem item = SectionView.FindItemWithText(searchSectionName, true, startIndex);
            if (item == null) return;

            SectionView.Items[item.Index].Selected = true;
            SectionView.Items[item.Index].EnsureVisible();
        }

        private void CloneScanBtn_Click(object sender, EventArgs e)
        {
            Query query = new Query(mainForm, comparerTool, bitsDictDictsIdx, bitsDictDicts);
            query.Show();
        }

        private void SectionViewHexEditor_Click(object sender, EventArgs e)
        {
            List<ListViewItem> selectedItems = ListViewLVITEM.GetSelectedItems(SectionView);
            if (selectedItems.Count == 0) return;

            var sectionItem = selectedItems[0];
            uint sid = uint.Parse(sectionItem.SubItems[(int)SectionCol.SectionViewSID].Text);
            Section section = sectionTool.GetSection(sid);

            HexEditor hexEdit = new HexEditor(mainForm, sectionTool, section, 0);
            hexEdit.Show(this);
        }

        private void SectionViewDump_Click(object sender, EventArgs e)
        {
            List<ListViewItem> selectedItems = ListViewLVITEM.GetSelectedItems(SectionView);
            if (selectedItems.Count > 0)
            {
                SaveDialog.Filter = "Directory | directory"; //"Section binary (*.bin)|*.bin";
                SaveDialog.FilterIndex = 1;
                SaveDialog.RestoreDirectory = true;
                SaveDialog.FileName = "Save Here";

                if (SaveDialog.ShowDialog() != DialogResult.OK) return;

                double dumpSize = 0;
                string savePath = Path.GetDirectoryName(SaveDialog.FileName);
                string processName = MakeValidFileName(mainForm.ProcessName);
                //ComboItem process = (ComboItem)ProcessesBox.SelectedItem;
                for (int idx = 0; idx < selectedItems.Count; ++idx)
                {
                    string sectionAddr = MakeValidFileName(selectedItems[idx].SubItems[(int)SectionCol.SectionViewAddress].Text);
                    string sectionName = MakeValidFileName(selectedItems[idx].SubItems[(int)SectionCol.SectionViewName].Text);

                    uint sid = uint.Parse(selectedItems[idx].SubItems[(int)SectionCol.SectionViewSID].Text);
                    Section section = sectionTool.GetSection(sid);
                    byte[] subBuffer = PS4Tool.ReadMemory(section.PID, section.Start, section.Length);

                    string path = string.Format("{0}{1}{2}_{3}_{4}_{5}.bin", savePath, Path.DirectorySeparatorChar, processName, sid, sectionAddr, sectionName);
                    File.WriteAllBytes(path, subBuffer);
                    dumpSize += subBuffer.Length;
                }

                MessageBox.Show(string.Format("SectionViewDump success, dump size: {0}MB", Math.Round(dumpSize / 1024 / 1024, 2)), "SectionViewDump", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void SectionViewImport_Click(object sender, EventArgs e)
        {
            try
            {
                OpenDialog.Filter = "SectionDump binary (*.bin)|*.bin";
                OpenDialog.FilterIndex = 0;
                OpenDialog.RestoreDirectory = true;

                if (OpenDialog.ShowDialog() != DialogResult.OK) return;

                double importSize = 0;
                foreach (String file in OpenDialog.FileNames)
                {
                    byte[] bin = File.ReadAllBytes(file);
                    string name = Path.GetFileNameWithoutExtension(file);
                    string[] names = name.Split(new char[] { '_' }, 4);

                    if (names.Length < 4) continue;

                    string processName = names[0];
                    uint.TryParse(names[1], out uint sid);
                    ulong sectionAddr = ulong.Parse(names[2], NumberStyles.HexNumber);
                    string sectionName = names[3];
                    Section section = sectionTool.GetSection(sid);
                    if (section == null || section.Length != bin.Length) continue;

                    PS4Tool.WriteMemory(section.PID, section.Start, bin);
                    importSize += bin.Length;
                }

                MessageBox.Show(string.Format("SectionViewImport success, Import dump size: {0}MB", Math.Round(importSize / 1024 / 1024, 2)), "SectionViewImport", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Source + ":SectionViewImport_Click", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        /// <summary>
        /// Sanitize File Name
        /// https://stackoverflow.com/a/847251
        /// </summary>
        private string MakeValidFileName(string name)
        {
            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return Regex.Replace(name, invalidRegStr, "_");
        }

        private void SectionViewCheckAll_Click(object sender, EventArgs e) => SectionViewSelectAll(true);

        private void SectionViewUnCheckAll_Click(object sender, EventArgs e) => SectionViewSelectAll(false);

        private void SectionViewSelectAll(bool isSelectAll)
        {
            if (sectionItems.Count == 0) return;

            if (SelectAllBox.Checked != isSelectAll) SelectAllBox.Checked = isSelectAll;
            else
            {
                SectionView.BeginUpdate();
                for (int idx = 0; idx < sectionItems.Count; ++idx)
                {
                    ListViewItem item = sectionItems[idx];
                    if (item.Checked == isSelectAll) continue;

                    item.Checked = isSelectAll;
                    uint sid = uint.Parse(item.SubItems[(int)SectionCol.SectionViewSID].Text);
                    SectionCheckUpdate(isSelectAll, sid);
                }
                if (AddrMinBox.Tag != null) AddrMinBox.Text = ((ulong)AddrMinBox.Tag).ToString("X");
                if (AddrMaxBox.Tag != null) AddrMaxBox.Text = ((ulong)AddrMaxBox.Tag).ToString("X");
                ToolStripMsg.Text = string.Format("Total section: {0}, Selected section: {1}, Search size: {2}MB", sectionItems.Count, sectionTool.TotalSelected, sectionTool.TotalMemorySize / (1024 * 1024));
                SectionView.EndUpdate();
                if (!isSelectAll)
                {
                    AddrMinBox.Tag = null;
                    AddrMaxBox.Tag = null;
                    AddrMinBox.Text = "";
                    AddrMaxBox.Text = "";
                }
            }
        }

        private void SectionViewInvertChecked_Click(object sender, EventArgs e)
        {
            if (sectionItems.Count == 0) return;

            SectionView.BeginUpdate();
            for (int idx = 0; idx < sectionItems.Count; ++idx)
            {
                ListViewItem item = sectionItems[idx];
                item.Checked = !item.Checked;
                uint sid = uint.Parse(item.SubItems[(int)SectionCol.SectionViewSID].Text);
                SectionCheckUpdate(item.Checked, sid);
            }
            if (AddrMinBox.Tag != null) AddrMinBox.Text = ((ulong)AddrMinBox.Tag).ToString("X");
            if (AddrMaxBox.Tag != null) AddrMaxBox.Text = ((ulong)AddrMaxBox.Tag).ToString("X");
            ToolStripMsg.Text = string.Format("Total section: {0}, Selected section: {1}, Search size: {2}MB", sectionItems.Count, sectionTool.TotalSelected, sectionTool.TotalMemorySize / (1024 * 1024));
            SectionView.EndUpdate();
        }

        private void SectionViewCheck_Click(object sender, EventArgs e)
        {
            List<ListViewItem> selectedItems = ListViewLVITEM.GetSelectedItems(SectionView);
            if (selectedItems.Count == 0) return;

            bool? isChecked = null;
            SectionView.BeginUpdate();
            for (int idx = 0; idx < selectedItems.Count; ++idx)
            {
                ListViewItem item = selectedItems[idx];
                if (isChecked == null) isChecked = !item.Checked;
                item.Checked = (bool)isChecked;
                uint sid = uint.Parse(item.SubItems[(int)SectionCol.SectionViewSID].Text);
                SectionCheckUpdate(item.Checked, sid);
            }
            if (AddrMinBox.Tag != null) AddrMinBox.Text = ((ulong)AddrMinBox.Tag).ToString("X");
            if (AddrMaxBox.Tag != null) AddrMaxBox.Text = ((ulong)AddrMaxBox.Tag).ToString("X");
            ToolStripMsg.Text = string.Format("Total section: {0}, Selected section: {1}, Search size: {2}MB", sectionItems.Count, sectionTool.TotalSelected, sectionTool.TotalMemorySize / (1024 * 1024));
            SectionView.EndUpdate();
        }

        private void SectionViewCheckContains_Click(object sender, EventArgs e) => SectionViewContains(true, SectionViewTextContains.Text);

        private void SectionViewUnCheckContains_Click(object sender, EventArgs e) => SectionViewContains(false, SectionViewTextContains.Text);

        private void SectionViewCheckAllHidden_Click(object sender, EventArgs e) => SectionViewContains(true, "Hidden");

        private void SectionViewUnCheckAllHidden_Click(object sender, EventArgs e) => SectionViewContains(false, "Hidden");

        /// <summary>
        /// Select all sections that contain the specified text. If isContains is False, then do not select them.
        /// </summary>
        /// <param name="isContains"></param>
        private void SectionViewContains(bool isContains, string contains)
        {
            if (sectionItems.Count == 0) return;

            SectionView.BeginUpdate();
            for (int idx = 0; idx < sectionItems.Count; ++idx)
            {
                ListViewItem item = sectionItems[idx];
                uint sid = uint.Parse(item.SubItems[(int)SectionCol.SectionViewSID].Text);
                string name = item.SubItems[(int)SectionCol.SectionViewName].Text;
                item.Checked = isContains ? name.Contains(contains) : !name.Contains(contains);
                SectionCheckUpdate(item.Checked, sid);
            }
            if (AddrMinBox.Tag != null) AddrMinBox.Text = ((ulong)AddrMinBox.Tag).ToString("X");
            if (AddrMaxBox.Tag != null) AddrMaxBox.Text = ((ulong)AddrMaxBox.Tag).ToString("X");
            ToolStripMsg.Text = string.Format("Total section: {0}, Selected section: {1}, Search size: {2}MB", sectionItems.Count, sectionTool.TotalSelected, sectionTool.TotalMemorySize / (1024 * 1024));
            SectionView.EndUpdate();
        }

        private void SectionViewCheckProt_Click(object sender, EventArgs e) => SectionViewProtection(true);

        private void SectionViewUnCheckProt_Click(object sender, EventArgs e) => SectionViewProtection(false);

        /// <summary>
        /// Select all sections that match the Protection value. If isProt is False, then deselect them.
        /// </summary>
        /// <param name="isProt"></param>
        private void SectionViewProtection(bool isProt)
        {
            if (sectionItems.Count == 0) return;

            string[] sectionViewTextProts = SectionViewTextProt.Text.Split(new char[] { ',', '-', ';', '|', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            SectionView.BeginUpdate();
            for (int idx = 0; idx < sectionItems.Count; ++idx)
            {
                ListViewItem item = sectionItems[idx];
                uint sid = uint.Parse(item.SubItems[(int)SectionCol.SectionViewSID].Text);
                string prot = item.SubItems[(int)SectionCol.SectionViewProt].Text;
                for (int idxProt = 0; idxProt < sectionViewTextProts.Length; idxProt++)
                {
                    var inputProt = sectionViewTextProts[idxProt];
                    item.Checked = isProt ? prot == inputProt : prot != inputProt;
                    if (isProt && item.Checked) break;
                    else if (!isProt && !item.Checked) break;
                }
                SectionCheckUpdate(item.Checked, sid);
            }
            if (AddrMinBox.Tag != null) AddrMinBox.Text = ((ulong)AddrMinBox.Tag).ToString("X");
            if (AddrMaxBox.Tag != null) AddrMaxBox.Text = ((ulong)AddrMaxBox.Tag).ToString("X");
            ToolStripMsg.Text = string.Format("Total section: {0}, Selected section: {1}, Search size: {2}MB", sectionItems.Count, sectionTool.TotalSelected, sectionTool.TotalMemorySize / (1024 * 1024));
            SectionView.EndUpdate();
        }
        #endregion

        #region ResultViewMenu
        private void ResultView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e) => e.Item = resultItems.Count > e.ItemIndex ? resultItems[e.ItemIndex] : null;

        private void ResultView_DoubleClick(object sender, EventArgs e)
        {
            List<ListViewItem> selectedItems = ListViewLVITEM.GetSelectedItems(ResultView);
            if (selectedItems.Count != 1) return;

            ListViewItem resultItem = selectedItems[0];

            (uint sid, _) = ((uint sid, int resultIdx))resultItem.Tag;
            Section section = sectionTool.GetSection(sid);
            ScanType scanType = this.ParseFromDescription<ScanType>(resultItem.SubItems[(int)ResultCol.ResultListType].Text);
            ulong address = ulong.Parse(resultItem.SubItems[(int)ResultCol.ResultListAddress].Text, NumberStyles.HexNumber);
            if (address < section.Start) return;

            uint offsetAddr = (uint)(address - section.Start);
            string oldValue = resultItem.SubItems[(int)ResultCol.ResultListValue].Text;

            mainForm.AddToCheatGrid(section, offsetAddr, scanType, oldValue);
            selectedItems.Clear();
        }

        private void ResultViewAddToCheatGrid_Click(object sender, EventArgs e)
        {
            List<ListViewItem> selectedItems = ListViewLVITEM.GetSelectedItems(ResultView);
            if (selectedItems.Count == 0) return;

            mainForm.CheatGridViewRowCountUpdate(false);
            for (int i = 0; i < selectedItems.Count; ++i)
            {
                ListViewItem resultItem = selectedItems[i];
                (uint sid, _) = ((uint sid, int resultIdx))resultItem.Tag;
                Section section = sectionTool.GetSection(sid);
                ScanType scanType = this.ParseFromDescription<ScanType>(resultItem.SubItems[(int)ResultCol.ResultListType].Text);
                ulong address = ulong.Parse(resultItem.SubItems[(int)ResultCol.ResultListAddress].Text, NumberStyles.HexNumber);
                if (address < section.Start) continue;

                uint offsetAddr = (uint)(address - section.Start);
                string oldValue = resultItem.SubItems[(int)ResultCol.ResultListValue].Text;

                mainForm.AddToCheatGrid(section, offsetAddr, scanType, oldValue, false, "", null, null, -1, false);
            }
            mainForm.CheatGridViewRowCountUpdate();
            selectedItems.Clear();
            GC.Collect();
        }

        private void ResultViewHexEditor_Click(object sender, EventArgs e)
        {
            List<ListViewItem> selectedItems = ListViewLVITEM.GetSelectedItems(ResultView);
            if (selectedItems.Count != 1) return;

            var resultItem = selectedItems[0];
            (uint sid, _) = ((uint sid, int resultIdx))resultItem.Tag;
            Section section = sectionTool.GetSection(sid);
            uint offsetAddr = (uint)(ulong.Parse(resultItem.SubItems[(int)ResultCol.ResultListAddress].Text, NumberStyles.HexNumber) - section.Start);
            if (offsetAddr == 0) return;

            HexEditor hexEdit = new HexEditor(mainForm, sectionTool, section, (int)offsetAddr);
            hexEdit.Show(this);
        }

        private void ResultViewCopyAddress_Click(object sender, EventArgs e)
        {
            List<string> clipList = new List<string>();

            for (int i = 0; i < resultItems.Count; ++i)
            {
                ListViewItem resultItem = resultItems[i];
                if (!resultItem.Selected) continue;

                clipList.Add(resultItem.SubItems[(int)ResultCol.ResultListAddress].Text);
            }
            if (clipList.Count > 0) Clipboard.SetText(string.Join(",", clipList));
        }

        private void ResultViewDump_Click(object sender, EventArgs e)
        {
            //List<ListViewItem> selectedItems = ListViewLVITEM.GetSelectedItems(ResultView);
            //if (selectedItems.Count == 1)
            //{
            //    ulong address = ulong.Parse(ResultView.SelectedItems[0].SubItems[(int)ResultCol.ResultListAddress].Text, NumberStyles.HexNumber);
            //    int sectionID = processManager.MappedSectionList.GetMappedSectionID(address);
            //    dump_dialog(sectionID);
            //}
        }

        private void ResultViewFindPointer_Click(object sender, EventArgs e)
        {
            List<ListViewItem> selectedItems = ListViewLVITEM.GetSelectedItems(ResultView);
            if (selectedItems.Count != 1) return;

            try
            {
                ulong address = ulong.Parse(selectedItems[0].SubItems[(int)ResultCol.ResultListAddress].Text, NumberStyles.HexNumber);
                ScanType scanType = this.ParseFromDescription<ScanType>(selectedItems[0].SubItems[(int)ResultCol.ResultListType].Text);
                PointerFinder pointerFinder = new PointerFinder(mainForm, address, scanType);
                pointerFinder.Show();
            }
            catch (Exception) { }
        }

        private void ResultViewSelectAll_Click(object sender, EventArgs e)
        {
            if (ResultView.Items.Count == 0) return;

            ResultView.BeginUpdate();
            ListViewLVITEM.SelectAllItems(ResultView);
            ResultView.EndUpdate();
        }
        #endregion

        #region SlowMotion
        int slowMotionPauseInterval = 100;
        int slowMotionResumeInterval = 100;
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
            Thread.Sleep(slowMotionPauseInterval);

            Invoke(new MethodInvoker(() => { ResumeBtn.PerformClick(); }));
            Thread.Sleep(slowMotionResumeInterval);

            return true;
        });

        private void SlowMotionBox_CheckedChanged(object sender, EventArgs e)
        {
            if (SlowMotionBox.Checked)
            {
                string pauseIntervalStr = "100 100";
                string resumeIntervalStr = "100";
                ComboItem process = (ComboItem)ProcessesBox.SelectedItem;
                if (InputBox.Show("SlowMotion", "Enter the SlowMotion pause and resume interval (in milliseconds, larger intervals will be slower)", ref pauseIntervalStr) != DialogResult.OK ||
                    !PS4Tool.AttachDebugger((int)process.Value, (string)process.Text, processStatus))
                {
                    SlowMotionBox.Checked = false;
                    return;
                }
                string[] intervals = pauseIntervalStr.Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (intervals.Length > 1)
                {
                    pauseIntervalStr = intervals[0];
                    resumeIntervalStr = intervals[1];
                }
                if (!int.TryParse(pauseIntervalStr, out slowMotionPauseInterval)) slowMotionPauseInterval = 100;
                if (!int.TryParse(resumeIntervalStr, out slowMotionResumeInterval)) slowMotionResumeInterval = 100;
                if (slowMotionPauseInterval < 50) slowMotionPauseInterval = 50;
                if (slowMotionResumeInterval < 50) slowMotionResumeInterval = 50;

                SlowMotionTimer.Interval = 50;
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
