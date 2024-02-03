using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS4CheaterNeo
{
    public partial class PointerFinder : Form
    {
        readonly Main mainForm;
        readonly string processName;
        readonly SectionTool sectionTool;
        ListViewItemComparer listViewItemComparer;
        Dictionary<uint, List<Pointer>> addrPointerDict;
        Dictionary<uint, List<Pointer>> valuePointerDict;
        List<((uint BaseSID, uint BasePos, List<long> Offsets) pointer, List<Pointer> pathPointers)> pointerResults;
        List<ListViewItem> pointerItems;

        public PointerFinder(Main mainForm, ulong address, ScanType scanType)
        {
            this.Font = mainForm.Font;
            InitializeComponent();
            ApplyUI(mainForm.langJson);
            listViewItemComparer = new ListViewItemComparer();
            pointerItems = new List<ListViewItem>();
            if (!Properties.Settings.Default.CollapsibleContainer.Value) SplitContainer1.SplitterButtonStyle = ButtonStyle.None;
            this.mainForm = mainForm;
            sectionTool = new SectionTool(mainForm);
            processName = mainForm.ProcessName;
            pointerResults = new List<((uint BaseSID, uint BasePos, List<long> Offsets) pointer, List<Pointer> pathPointers)>();
            AddressBox.Text = address.ToString("X");
            PointerListView
                .GetType()
                .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(PointerListView, true, null);

            ScanType[] scanTypes = (ScanType[])Enum.GetValues(typeof(ScanType));
            for (int idx = 0; idx < scanTypes.Length; idx++)
            {
                ScanType filterEnum = scanTypes[idx];
                ScanTypeBox.Items.Add(new ComboItem(filterEnum.GetDescription(), filterEnum));
                if (scanType == filterEnum) ScanTypeBox.SelectedIndex = ScanTypeBox.Items.Count - 1;
            }
            IsFilterBox.Checked = Properties.Settings.Default.FilterQuery.Value;
            IsFilterSizeBox.Checked = Properties.Settings.Default.FilterSizeQuery.Value;
        }

        public void ApplyUI(LanguageJson langJson)
        {
            try
            {
                if (langJson != null)
                {
                    LevelLabel.Text        = langJson.PointerFinderForm.LevelLabel;
                    AddressLabel.Text      = langJson.PointerFinderForm.AddressLabel;
                    ScanBtn.Text           = langJson.PointerFinderForm.ScanBtn;
                    NewBtn.Text            = langJson.PointerFinderForm.NewBtn;
                    MaxRangeLabel.Text     = langJson.PointerFinderForm.MaxRangeLabel;
                    IsInitScan.Text        = langJson.PointerFinderForm.IsInitScan;
                    FastScanBox.Text       = langJson.PointerFinderForm.FastScanBox;
                    NegativeOffsetBox.Text = langJson.PointerFinderForm.NegativeOffsetBox;
                    IsFilterBox.Text       = langJson.PointerFinderForm.IsFilterBox;
                    FilterRuleBtn.Text     = langJson.PointerFinderForm.FilterRuleBtn;
                    IsFilterSizeBox.Text   = langJson.PointerFinderForm.IsFilterSizeBox;
                    FilterSizeRuleBtn.Text = langJson.PointerFinderForm.FilterSizeRuleBtn;
                    SaveBtn.Text           = langJson.PointerFinderForm.SaveBtn;
                    LoadBtn.Text           = langJson.PointerFinderForm.LoadBtn;

                    PointerListViewAddToCheatGrid.Text = langJson.PointerFinderForm.PointerListViewAddToCheatGrid;
                    PointerListViewSelectAll.Text      = langJson.PointerFinderForm.PointerListViewSelectAll;
                }
            }
            catch (Exception ex)
            {
                InputBox.MsgBox("Apply UI language Exception", "", ex.Message, 100);
            }
            try
            {
                Opacity = Properties.Settings.Default.UIOpacity.Value;

                ForeColor = Properties.Settings.Default.UiForeColor.Value; //Color.White;
                BackColor = Properties.Settings.Default.UiBackColor.Value; //Color.FromArgb(36, 36, 36);
                statusStrip1.BackColor = Properties.Settings.Default.PointerFinderStatusStrip1BackColor.Value; //Color.DimGray;
                ScanBtn.BackColor = Properties.Settings.Default.PointerFinderScanBtnBackColor.Value; //Color.SteelBlue;

                SplitContainer1.Panel1.ForeColor = ForeColor;
                PointerListView.ForeColor        = ForeColor;
                PointerListView.BackColor        = BackColor;
                ScanTypeBox.ForeColor            = ForeColor;
                ScanTypeBox.BackColor            = BackColor;
                MaxRangeUpDown.ForeColor         = ForeColor;
                MaxRangeUpDown.BackColor         = BackColor;
                LevelUpdown.ForeColor            = ForeColor;
                LevelUpdown.BackColor            = BackColor;
                AddressBox.ForeColor             = ForeColor;
                AddressBox.BackColor             = BackColor;
                FilterSizeRuleBtn.BackColor      = BackColor;
                NewBtn.BackColor                 = BackColor;
                LoadBtn.BackColor                = BackColor;
                SaveBtn.BackColor                = BackColor;
                FilterRuleBtn.BackColor          = BackColor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Source + ":ApplyUI", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
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
            else if (pointerItems.Count > 0 && 
                MessageBox.Show("Still in the find pointer, Do you want to close PointerFinder?", "PointerFinder", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) e.Cancel = true;

            if (!e.Cancel)
            {
                PointerListView.VirtualListSize = 0;
                PointerListView.VirtualMode = false;
                pointerItems.Clear();
                pointerItems = null;
                PointerListView.Clear();
                PointerListView = null;
            }

            GC.Collect();
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (addrPointerDict == null || addrPointerDict.Count == 0)
            {
                ToolStripMsg.Text = String.Format("address-value list dictionary is empty.");
                return;
            }

            SaveDialog.Filter = "Pointer files (*.txt)|*.txt";
            SaveDialog.FilterIndex = 1;
            SaveDialog.RestoreDirectory = true;

            if (SaveDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                Section[] sections = sectionTool.GetSectionSortByAddr(addrPointerDict.Keys);
                List<string> lines = new List<string>();
                AddressBox.Text = Regex.Replace(AddressBox.Text, "[^0-9a-fA-F]", "");
                lines.Add($"FindAddress: {AddressBox.Text}({ulong.Parse(AddressBox.Text, NumberStyles.HexNumber)})");
                for (int sIdx = 0; sIdx < sections.Length; sIdx++)
                {
                    Section addrSection = sections[sIdx];
                    List<Pointer> addrList = addrPointerDict[addrSection.SID];
                    for (int idx = 0; idx < addrList.Count; idx++)
                    {
                        Pointer pointer = addrList[idx];
                        Section valueSection = sectionTool.GetSection(pointer.ValueSID);
                        lines.Add(string.Format("{0:X}|{1:X}|{2}|{3}|{4:X}|{5}|{6}|{7:X}", pointer.AddrPos, pointer.ValuePos, addrSection.SID, addrSection.Name, addrSection.Prot, pointer.ValueSID, valueSection.Name, valueSection.Prot));
                    }
                }

                File.WriteAllLines(SaveDialog.FileName, lines);
                GC.Collect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save memory failed, " + ex.Message, ex.Source + ":SaveBtn_Click", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
                sectionTool.InitSections(processName);
                System.Diagnostics.Stopwatch ticker = System.Diagnostics.Stopwatch.StartNew();
                if (addrPointerDict != null) addrPointerDict.Clear();
                if (valuePointerDict != null) valuePointerDict.Clear();
                addrPointerDict = new Dictionary<uint, List<Pointer>>();
                valuePointerDict = new Dictionary<uint, List<Pointer>>();
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
                        uint addrSID = uint.Parse(elems[2]);
                        uint valueSID = uint.Parse(elems[5]);
                        string addrName = elems[3];
                        string valueName = elems[6];
                        uint addrProt = uint.Parse(elems[4], NumberStyles.HexNumber);
                        uint valueProt = uint.Parse(elems[7], NumberStyles.HexNumber);
                        Section addrSection = sectionTool.GetSection(addrSID, addrName, addrProt);
                        Section valueSection = sectionTool.GetSection(valueSID, valueName, valueProt);

                        if (addrSection == null || valueSection == null) continue;

                        addrValueList.Add(new Pointer(addrSID, addrPosition, valueSID, valuePosition));
                        resultCnt++;
                    }
                }
                GC.Collect();
                if (addrValueList != null && addrValueList.Count > 0)
                {
                    uint tmpAddrSID = 0;
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
                    if (addrPointerList != null && !addrPointerDict.ContainsKey(tmpAddrSID))
                    {
                        addrPointerList.Sort((p1, p2) => p1.AddrPos.CompareTo(p2.AddrPos));
                        addrPointerDict[tmpAddrSID] = addrPointerList;
                    }

                    uint tmpValueSID = 0;
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
                    if (valuePointerList != null && !valuePointerDict.ContainsKey(tmpValueSID))
                    {
                        valuePointerList.Sort((p1, p2) => p1.ValuePos.CompareTo(p2.ValuePos));
                        valuePointerDict[tmpValueSID] = valuePointerList;
                    }
                    addrValueList = null;
                }
                GC.Collect();

                IsInitScan.Checked = false;
                ToolStripMsg.Text = String.Format("Loaded {0} address-value results, elapsed: {1:0.00}", resultCnt, ticker.Elapsed.TotalSeconds);
                ScanBtn.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save memory failed, " + ex.Message, ex.Source + ":LoadBtn_Click", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
                    int maxOffsetLevel = (int)LevelUpdown.Value;
                    int maxOffsetRange = (int)MaxRangeUpDown.Value;
                    AddressBox.Text = Regex.Replace(AddressBox.Text, "[^0-9a-fA-F]", "");
                    ulong queryAddress = ulong.Parse(AddressBox.Text, NumberStyles.HexNumber);

                    PointerListView.VirtualListSize = 0;
                    pointerItems.Clear();
                    PointerListView.Clear();
                    PointerListView.GridLines = true;
                    PointerListView.Columns.Add("Base Address", "Base Address");
                    PointerListView.Columns.Add("Base Section", "Base Section");
                    for (int i = 0; i < maxOffsetLevel; ++i) PointerListView.Columns.Add("Offset " + (i + 1), "Offset " + (i + 1));

                    ScanBtn.Text = "Stop";
                    sectionTool.InitSections(processName);

                    if (pointerSource != null) pointerSource.Dispose();
                    pointerSource = new CancellationTokenSource();
                    System.Diagnostics.Stopwatch tickerMajor = System.Diagnostics.Stopwatch.StartNew();
                    pointerTask = ScanTask(150, tickerMajor);
                    pointerTask.ContinueWith(t => TaskComparer(queryAddress, maxOffsetLevel, maxOffsetRange, 150, tickerMajor)).ContinueWith(t => TaskCompleted(tickerMajor));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Source + ":ScanBtn_Click", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

            PointerListView.VirtualListSize = 0;
            pointerItems.Clear();
            PointerListView.Clear();

            ProgBar.Value = 0;
            pointerResults.Clear();
            IsInitScan.Checked = true;
            ScanBtn.Text = "First Scan";
            GC.Collect();
        }

        private void FilterRuleBtn_Click(object sender, EventArgs e)
        {
            string sectionFilterKeys = Properties.Settings.Default.SectionFilterKeys.Value;

            if (InputBox.Show("Section Filter", "Enter the value of the filter keys", ref sectionFilterKeys) != DialogResult.OK) return;

            Properties.Settings.Default.SectionFilterKeys.Value = sectionFilterKeys;
        }

        private void FilterSizeRuleBtn_Click(object sender, EventArgs e)
        {
            uint sectionFilterSize = Properties.Settings.Default.SectionFilterSize.Value;
            string sectionFilterSizeStr = sectionFilterSize.ToString();

            if (InputBox.Show("Section Filter", "Enter the value of the filter keys", ref sectionFilterSizeStr) != DialogResult.OK) return;

            Properties.Settings.Default.SectionFilterSize.Value = uint.Parse(sectionFilterSizeStr);
        }

        #endregion

        #region PointerListView
        private void PointerListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e) => e.Item = pointerItems.Count > e.ItemIndex ? pointerItems[e.ItemIndex] : null;

        private void PointerListView_DoubleClick(object sender, EventArgs e)
        {
            List<ListViewItem> selectedItems = ListViewLVITEM.GetSelectedItems(PointerListView);
            if (selectedItems.Count != 1) return;

            ListViewItem resultItem = selectedItems[0];
            int idx = (int)resultItem.Tag;

            var pointerResult = pointerResults[idx];

            ScanType scanType = (ScanType)((ComboItem)(ScanTypeBox.SelectedItem)).Value;
            Section baseSection = sectionTool.GetSection(pointerResult.pointer.BaseSID);
            ulong baseAddress = baseSection.Start + pointerResult.pointer.BasePos;

            try
            {
                List<long> offsetList = new List<long> { (long)baseAddress };
                offsetList.AddRange(pointerResult.pointer.Offsets);
                NewAddress newAddress = new NewAddress(mainForm, null, baseSection, 0, scanType, null, false, "", offsetList, false);
                if (newAddress.ShowDialog() != DialogResult.OK) return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Source + ":PointerListView_DoubleClick", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        /// <summary>
        /// Sort VirtualMode ListView items
        /// https://stackoverflow.com/a/39505452
        /// Sort a ListView control by using a column
        /// https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/sort-listview-by-column
        /// </summary>
        private void PointerListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {// Determine if clicked column is already the column that is being sorted.
            if (listViewItemComparer.SortColumns.Contains(e.Column))
            {
                if (listViewItemComparer.SortColumns.First.Value == e.Column)
                    listViewItemComparer.Order = listViewItemComparer.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending; // Reverse the current sort direction for this column.
                else
                {
                    listViewItemComparer.SortColumns.Remove(e.Column);
                    listViewItemComparer.SortColumns.AddFirst(e.Column);
                }
            }
            else
            {
                listViewItemComparer.SortColumns.AddFirst(e.Column); // Set the column number that is to be sorted; default to ascending.
                listViewItemComparer.Order = SortOrder.Ascending;
            }
            pointerItems.Sort(listViewItemComparer);
            PointerListView.Refresh();
        }

        private void PointerListViewAddToCheatGrid_Click(object sender, EventArgs e)
        {
            List<ListViewItem> selectedItems = ListViewLVITEM.GetSelectedItems(PointerListView);
            if (selectedItems.Count == 0) return;

            Dictionary<ulong, ulong> pointerCaches = new Dictionary<ulong, ulong>();
            ScanType scanType = (ScanType)((ComboItem)(ScanTypeBox.SelectedItem)).Value;
            mainForm.CheatGridViewRowCountUpdate(false);
            for (int itemIdx = 0; itemIdx < selectedItems.Count; ++itemIdx)
            {
                try
                {
                    ListViewItem resultItem = selectedItems[itemIdx];
                    int idx = (int)resultItem.Tag;

                    var pointerResult = pointerResults[idx];

                    Section baseSection = sectionTool.GetSection(pointerResult.pointer.BaseSID);
                    ulong baseAddress = baseSection.Start + pointerResult.pointer.BasePos;
                    string msg = pointerResult.pointer.BasePos.ToString("X");
                    List<long> pointerOffsets = new List<long> { (long)baseAddress };
                    pointerResult.pointer.Offsets.ForEach(offset => {
                        msg += "_" + offset.ToString("X");
                        pointerOffsets.Add(offset);
                    });
                    mainForm.AddToCheatGrid(baseSection, 0, scanType, "0", false, msg, pointerOffsets, pointerCaches, -1, false); //FIXME oldValue is 0
                }
                catch (Exception ex)
                {
                    ToolStripMsg.Text = string.Format("Add Pointer To CheatGrid failed...{0}, {1}", ex.Message, ex.StackTrace);
                }
            }
            mainForm.CheatGridViewRowCountUpdate();
        }

        private void PointerListViewSelectAll_Click(object sender, EventArgs e)
        {
            if (PointerListView.Items.Count == 0) return;

            PointerListView.BeginUpdate();
            ListViewLVITEM.SelectAllItems(PointerListView);
            PointerListView.EndUpdate();
        }
        #endregion

        #region Task
        /// <summary>
        /// foreach all sections and take out all addresses and values, then get the list sorted by address and list by value respectively
        /// </summary>
        /// <param name="nextScanCheckNumber">necessary to obtain the latest AddrValueList again when the number of results exceeds this value</param>
        /// <param name="tickerMajor">calculate execution time</param>
        /// <returns></returns>
        private async Task<bool> ScanTask(int nextScanCheckNumber, System.Diagnostics.Stopwatch tickerMajor) => await Task.Run(() =>
        {
            try
            {
                #region InitAddrValueList
                if (addrPointerDict == null || addrPointerDict.Count == 0 || IsInitScan.Checked || pointerResults.Count > nextScanCheckNumber)
                {
                    Invoke(new MethodInvoker(() => { IsInitScan.Checked = false; }));

                    using(Mutex mutex = new Mutex())
                    {
                        bool isFilter            = IsFilterBox.Checked;
                        bool isFilterSize        = IsFilterSizeBox.Checked;
                        byte MaxQueryThreads     = Properties.Settings.Default.MaxQueryThreads.Value;
                        MaxQueryThreads          = MaxQueryThreads == (byte)0 ? (byte)1 : MaxQueryThreads;
                        string SectionFilterKeys = Properties.Settings.Default.SectionFilterKeys.Value;
                        SectionFilterKeys        = Regex.Replace(SectionFilterKeys, " *[,;] *", "|");
                        uint SectionFilterSize   = Properties.Settings.Default.SectionFilterSize.Value;
                        uint QueryBufferSize     = Properties.Settings.Default.QueryBufferSize.Value;

                        long processedMemoryLen = 0;
                        ulong minLength = QueryBufferSize * 1024 * 1024; //set the minimum read size in bytes

                        List<Pointer> addrValueList = new List<Pointer>();
                        Section[] sectionKeys   = sectionTool.GetSectionSortByAddr();
                        SemaphoreSlim semaphore = new SemaphoreSlim(MaxQueryThreads);
                        List<Task<bool>> tasks  = new List<Task<bool>>();
                        (List<(int start, int end)> rangeList, int readCnt) = GetSectionRangeList(sectionKeys, isFilter, isFilterSize, minLength, SectionFilterKeys, SectionFilterSize);

                        for (int idx = 0; idx < rangeList.Count; idx++)
                        {
                            (int start, int end) range = rangeList[idx];
                            tasks.Add(Task.Run<bool>(() =>
                            {
                                try
                                {
                                    semaphore.Wait();
                                    pointerSource.Token.ThrowIfCancellationRequested();
                                    Section sectionStart = sectionKeys[range.start];
                                    Section sectionEnd = sectionKeys[range.end];
                                    readCnt += range.end - range.start + 1;
                                    Invoke(new MethodInvoker(() =>
                                    {
                                        ProgBar.Value = (int)(((float)(readCnt) / sectionKeys.Length) * 50);
                                        ToolStripMsg.Text = string.Format("Scan elapsed:{0:0.00}s. Current: {1}/{2}, read memory...{3}MB", tickerMajor.Elapsed.TotalSeconds, readCnt, sectionKeys.Length, processedMemoryLen / (1024 * 1024));
                                    }));

                                    ulong bufferSize = sectionEnd.Start + (ulong)sectionEnd.Length - sectionStart.Start;
                                    byte[] buffer = PS4Tool.ReadMemory(sectionStart.PID, sectionStart.Start, (int)bufferSize);
                                    processedMemoryLen += buffer.Length;

                                    for (int rIdx = range.start; rIdx <= range.end; rIdx++)
                                    {
                                        Section addrSection = sectionKeys[rIdx];
                                        int scanOffset = (int)(addrSection.Start - sectionStart.Start);

                                        pointerSource.Token.ThrowIfCancellationRequested();

                                        List<Pointer> pointers = new List<Pointer>();
                                        Section valueSection = null;
                                        for (int scanIdx = scanOffset; scanIdx + 8 < scanOffset + addrSection.Length; scanIdx += 8)
                                        {
                                            pointerSource.Token.ThrowIfCancellationRequested();
                                            uint valueSID = 0;
                                            ulong mappedValue = BitConverter.ToUInt64(buffer, scanIdx);
                                            if (mappedValue > 0 && valueSection != null && mappedValue >= valueSection.Start && mappedValue <= (valueSection.Start + (ulong)valueSection.Length)) valueSID = valueSection.SID;
                                            else valueSID = sectionTool.GetSectionID(mappedValue);
                                            if (valueSID == 0) continue; //-1(int) => 0(uint)

                                            valueSection = sectionTool.GetSection(valueSID);
                                            int startOffset = scanIdx - scanOffset;
                                            pointers.Add(new Pointer(addrSection.SID, (uint)startOffset, valueSID, (uint)(mappedValue - valueSection.Start)));
                                        }
                                        mutex.WaitOne();
                                        addrValueList.AddRange(pointers);
                                        mutex.ReleaseMutex();
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
                        if (addrValueList == null || addrValueList.Count <= 0) return true;

                        if (addrPointerDict != null) addrPointerDict.Clear();
                        if (valuePointerDict != null) valuePointerDict.Clear();
                        addrPointerDict = new Dictionary<uint, List<Pointer>>();
                        valuePointerDict = new Dictionary<uint, List<Pointer>>();

                        uint tmpAddrSID = 0;
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
                        if (addrPointerList != null && !addrPointerDict.ContainsKey(tmpAddrSID))
                        {
                            addrPointerList.Sort((p1, p2) => p1.AddrPos.CompareTo(p2.AddrPos));
                            addrPointerDict[tmpAddrSID] = addrPointerList;
                        }

                        uint tmpValueSID = 0;
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
                        if (valuePointerList != null && !valuePointerDict.ContainsKey(tmpValueSID))
                        {
                            valuePointerList.Sort((p1, p2) => p1.ValuePos.CompareTo(p2.ValuePos));
                            valuePointerDict[tmpValueSID] = valuePointerList;
                        }
                        addrValueList = null;
                        GC.Collect();
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                if (ex.InnerException is OperationCanceledException)
                {
                    Invoke(new MethodInvoker(() => {
                        ProgBar.Value = 100;
                        IsInitScan.Checked = true;
                        ToolStripMsg.Text = string.Format("Scan elapsed:{0:0.00}s. ScanTask canceled. {1}", tickerMajor.Elapsed.TotalSeconds, ex.InnerException.Message);
                    }));
                }
                else MessageBox.Show(ex.ToString(), ex.Source + ":ScanTask", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally { }

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
        /// <param name="SectionFilterKeys"></param>
        /// <param name="SectionFilterSize"></param>
        /// <returns></returns>
        private (List<(int start, int end)>, int ignoreCnt) GetSectionRangeList(Section[] sectionKeys, bool isFilter, bool isFilterSize, ulong minLength, string SectionFilterKeys, uint SectionFilterSize)
        {
            List<(int start, int end)> rangeList = new List<(int start, int end)>();
            int ignoreCnt = 0;
            (int start, int end) rangeIdx = (-1, -1);
            for (int sectionIdx = 0; sectionIdx < sectionKeys.Length; sectionIdx++)
            {
                bool isContinue = false;
                Section currentSection = sectionKeys[sectionIdx];

                if (!isFilter && currentSection.Name.StartsWith("libSce") ||
                isFilter && sectionTool.SectionIsFilter(currentSection.Name, SectionFilterKeys) ||
                isFilterSize && currentSection.Length < SectionFilterSize)
                {
                    ignoreCnt++;
                    isContinue = true; //Check if section is not scanned
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
            return (rangeList, ignoreCnt);
        }

        private bool TaskComparer(ulong queryAddress, int maxOffsetLevel, int maxOffsetRange, int nextScanCheckNumber, System.Diagnostics.Stopwatch tickerMajor)
        {
            #region Comparer
            if (pointerResults.Count == 0)
            {
                Invoke(new MethodInvoker(() =>
                {
                    ProgBar.Value = 80;
                    ToolStripMsg.Text = string.Format("Scan elapsed:{0:0.00}s. perform first query...", tickerMajor.Elapsed.TotalSeconds);
                }));

                if (sectionTool.GetSectionID(queryAddress) is uint addrSID && addrSID == 0) return true; //-1(int) => 0(uint)

                Section section = sectionTool.GetSection(addrSID);
                Pointer queryPointer = new Pointer(addrSID, (uint)(queryAddress - section.Start), 0, 0);

                QueryPointerFirst(queryPointer, maxOffsetLevel, maxOffsetRange, tickerMajor);
            }
            else
            {
                Dictionary<ulong, ulong> pointerCaches = new Dictionary<ulong, ulong>();
                var newPointerResults = new List<((uint BaseSID, uint BasePos, List<long> Offsets) pointer, List<Pointer> pathPointers)>();
                for (int pIdx = 0; pIdx < pointerResults.Count; ++pIdx)
                {
                    pointerSource.Token.ThrowIfCancellationRequested();
                    if (pIdx % 1000 == 0)
                    {
                        Invoke(new MethodInvoker(() =>
                        {
                            ProgBar.Value = 50 + (int)(((float)(pIdx + 1) / pointerResults.Count) * 50);
                            ToolStripMsg.Text = string.Format("Scan elapsed:{0:0.00}s. Compare pointer results: {1}/{2}, mapped memory...", tickerMajor.Elapsed.TotalSeconds, pIdx + 1, pointerResults.Count);
                        }));
                    }

                    var pointerResult = pointerResults[pIdx];
                    var tailAddress = pointerResults.Count > nextScanCheckNumber ? ReadTailAddressByAddrPointers(pointerResult) : ReadTailAddress(pointerResult, pointerCaches);
                    if (tailAddress != queryAddress) continue;

                    newPointerResults.Add(pointerResult);
                    if (newPointerResults.Count < 20000 && pointerResult.pointer.Offsets.Count > 0) AddPointerListViewItem(newPointerResults.Count - 1, pointerResult.pathPointers, pointerResult.pointer.Offsets, maxOffsetLevel);
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
        /// find the pointer of the address closest to the query address by recursive, and calculate its offset from the query address
        /// finally, parse the query result into pointerResult
        /// Notice: when parsing pathOffsets to pointerResult.pointer.Offsets, write in reverse order
        /// </summary>
        /// <param name="queryPointer">query address</param>
        /// <param name="maxOffsetLevel">maximum offset level for this query</param>
        /// <param name="maxOffsetRange">maximum offset value range for this query</param>
        /// <param name="tickerMajor">calculate execution time</param>
        /// <param name="level">the level of the current offset value, also equals to the count of recursion times</param>
        /// <param name="pathOffsets">all paths of offset result</param>
        /// <param name="pathPointers">all paths of pointer result</param>
        private void QueryPointerFirst(Pointer queryPointer, int maxOffsetLevel, int maxOffsetRange, System.Diagnostics.Stopwatch tickerMajor, int level = 0, List<long> pathOffsets = null, List<Pointer> pathPointers = null)
        {
            pointerSource.Token.ThrowIfCancellationRequested();
            if (level > maxOffsetLevel) return;
            if (!addrPointerDict.TryGetValue(queryPointer.AddrSID, out List<Pointer> addrPointerList) || addrPointerList.Count == 0) return;
            
            Section[] sections = sectionTool.GetSectionSortByAddr(queryPointer.AddrSID, out int sIdx, new List<uint>(addrPointerDict.Keys));
            if (sIdx == -1) return;

            Section section = sections[sIdx];

            ulong queryAddress = queryPointer.AddrPos + section.Start;

            if (BinarySearchByAddress(addrPointerList, queryPointer.AddrPos, 0, addrPointerList.Count - 1) is int hitIdx && hitIdx == -1) return;

            if (pathOffsets == null) pathOffsets = new List<long>();
            if (pathPointers == null) pathPointers = new List<Pointer>();

            int counter = 0;
            bool isBreak = false;
            bool isFastScan = FastScanBox.Checked;
            bool isNegativeOffset = NegativeOffsetBox.Checked;
            const int maxPointerCount = 15;
            for (int sectionIdx = sIdx; isNegativeOffset ? sectionIdx < sections.Length : sectionIdx >= 0; sectionIdx += isNegativeOffset ? 1 : -1)
            {
                section = sectionIdx != sIdx ? sections[sectionIdx] : section;
                if (sectionIdx != sIdx && (!addrPointerDict.TryGetValue(section.SID, out addrPointerList) || addrPointerList.Count == 0)) continue;
                for (int addrIdx = sectionIdx == sIdx ? hitIdx : (isNegativeOffset ? 0 : addrPointerList.Count - 1); isNegativeOffset ? addrIdx < addrPointerList.Count : addrIdx >= 0; addrIdx += isNegativeOffset ? 1 : -1)
                {
                    pointerSource.Token.ThrowIfCancellationRequested();
                    Pointer currentAddrPointer = addrPointerList[addrIdx];
                    ulong currentAddr = currentAddrPointer.AddrPos + section.Start;
                    ulong currentOffset = queryAddress - currentAddr;
                    if (maxOffsetRange > 0 && (long)currentOffset > maxOffsetRange) break; //maxOffsetRange > 0 && (long)checkAddr + maxOffsetRange < (long)queryAddress
                    else if (maxOffsetRange < 0 && (long)currentOffset < maxOffsetRange) break; //maxOffsetRange < 0 && (long)checkAddr + maxOffsetRange > (long)queryAddress

                    List<Pointer> currentValuePointers = GetPointerListByValue(currentAddrPointer);
                    if (currentValuePointers.Count == 0) continue;

                    pathOffsets.Add((long)currentOffset); //currentOffset added at this time is for the next level to use, and will be removed after calling the next level
                    int curPointerCounter = 0;
                    bool inNewLevel = false;
                    for (int j = 0; j < currentValuePointers.Count; ++j)
                    {
                        bool inStack = false;
                        Pointer currentValuePointer = currentValuePointers[j];
                        for (int k = 0; k < pathPointers.Count; ++k) //no pathAddress at level 0
                        {
                            pointerSource.Token.ThrowIfCancellationRequested();
                            Pointer pathPointer = pathPointers[k];
                            if (pathPointer.ValuePos != currentValuePointer.ValuePos && pathPointer.AddrPos != currentValuePointer.AddrPos) continue;
                            inStack = true;
                            break;
                        }
                        if (inStack) continue;

                        inNewLevel = true;
                        if (curPointerCounter >= maxPointerCount) break;

                        ++curPointerCounter;

                        pathPointers.Add(currentValuePointer); //currentValuePointer added at this time is for the next level to use, and will be removed after calling the next level
                        QueryPointerFirst(currentValuePointer, maxOffsetLevel, maxOffsetRange, tickerMajor, level + 1, pathOffsets, pathPointers);
                        pathPointers.RemoveAt(pathPointers.Count - 1);
                    }

                    pathOffsets.RemoveAt(pathOffsets.Count - 1);

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
            if (pathPointers == null || pathPointers.Count == 0) return;

            Pointer basePointer = pathPointers[pathOffsets.Count - 1];
            Section baseSection = sectionTool.GetSection(basePointer.AddrSID);
            if (baseSection == null || isFastScan && !baseSection.Name.StartsWith("executable")) return;

            (uint BaseSID, uint BasePos, List<long> Offsets) pointer = (baseSection.SID, basePointer.AddrPos, new List<long>());
            for (int i = pathOffsets.Count - 1; i >= 0; --i) pointer.Offsets.Add(pathOffsets[i]);

            pointerResults.Add((pointer, new List<Pointer>(pathPointers)));

            if (pointerResults.Count < 20000 && pathOffsets.Count > 0) AddPointerListViewItem(pointerResults.Count - 1, pathPointers, pathOffsets, maxOffsetLevel);
            if (pointerResults.Count % 1000 == 0)
            {
                Invoke(new MethodInvoker(() =>
                {
                    ToolStripMsg.Text = string.Format("Scan elapsed:{0:0.00}s. first query: {1} results...level:{2}", tickerMajor.Elapsed.TotalSeconds, pointerResults.Count, level);
                }));
            }
        }

        /// <summary>
        /// update text of ScanBtn based on scan result
        /// </summary>
        /// <param name="tickerMajor">calculate execution time</param>
        /// <returns></returns>
        private bool TaskCompleted(System.Diagnostics.Stopwatch tickerMajor)
        {
            tickerMajor.Stop();
            Invoke(new MethodInvoker(() => {
                ProgBar.Value = 100;
                PointerListView.VirtualListSize = pointerItems.Count;
                if (pointerResults.Count == 0) ScanBtn.Text = "First Scan";
                else if (pointerResults.Count > 0) ScanBtn.Text = "Next Scan";
                ToolStripMsg.Text = string.Format("Scan elapsed:{0:0.00}s. Query pointer end, find:{1}", tickerMajor.Elapsed.TotalSeconds, pointerResults.Count);
            }));
            if (pointerSource != null) pointerSource.Dispose();
            if (pointerTask != null) pointerTask.Dispose();
            GC.Collect();
            return true;
        }

        /// <summary>
        /// add current pointers to pointerItems first and add it to PointerListView when it reaches 100 items when perform AddPointerListViewItem
        /// </summary>
        /// <param name="idx">index position of pointerResult</param>
        /// <param name="pathPointers">all paths of pointer result</param>
        /// <param name="pathOffsets">all paths of offset result</param>
        private void AddPointerListViewItem(int idx, List<Pointer> pathPointers, List<long> pathOffsets, int maxOffsetLevel)
        {
            Pointer pointer = pathPointers[pathPointers.Count - 1];
            Section section = sectionTool.GetSection(pointer.AddrSID);
            string sectionStr = $"{section.Start.ToString("X9")}-{section.Name}-{section.Prot.ToString("X")}-{section.Length / 1024}KB";

            ListViewItem item = new ListViewItem((pointer.AddrPos + section.Start).ToString("X"), 0); //Base Address
            item.Tag = idx;
            item.SubItems.Add(sectionStr); //Base Section
            for (int i = 0; i < pathOffsets.Count; ++i) item.SubItems.Add(pathOffsets[i].ToString("X"));
            if (pathOffsets.Count < maxOffsetLevel)
            { //To prevent the error "When in VirtualMode, the ListView RetrieveVirtualListItem event needs a list view SubItem for each ListView column."
                for (int i = 0; i < maxOffsetLevel - pathOffsets.Count; ++i) item.SubItems.Add("");
            }
            pointerItems.Add(item);
            Invoke(new MethodInvoker(() =>
            {
                PointerListView.VirtualListSize = pointerItems.Count;
            }));
        }

        /// <summary>
        /// get all pointers with the same value as the address of input from the list of value pointers
        /// </summary>
        /// <param name="addrPointer">query address</param>
        /// <returns>pointers with the same value as the address of input</returns>
        private List<Pointer> GetPointerListByValue(Pointer addrPointer)
        {
            List<Pointer> resultPointers = new List<Pointer>();

            if (!valuePointerDict.TryGetValue(addrPointer.AddrSID, out List<Pointer> valuePointerList)) return resultPointers;

            int index = BinarySearchByValue(valuePointerList, addrPointer.AddrPos);

            if (index < 0) return resultPointers;

            int start = index;
            for (; start > 0; --start) if (valuePointerList[start].ValuePos != addrPointer.AddrPos) break;

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
        /// read tailAddress from the temporary addrPointerList (faster but results are temporary)
        /// </summary>
        /// <param name="pointerResult">pointer result</param>
        /// <returns></returns>
        private ulong ReadTailAddressByAddrPointers(((uint BaseSID, uint BasePos, List<long> Offsets) pointer, List<Pointer> pathPointers) pointerResult)
        {
            ulong tailAddr = 0;
            Section addrSection = sectionTool.GetSection(pointerResult.pointer.BaseSID);
            if (addrSection == null) return tailAddr;
            if (!addrPointerDict.TryGetValue(addrSection.SID, out List<Pointer> addrPointerList)) return tailAddr;

            List<long> offsetList = new List<long>();
            offsetList.Add(pointerResult.pointer.BasePos); //offsetList.Add((long)(addrSection.Start + pointerResult.pointer.position));
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
                else tailAddr = (ulong)offset + headAddrPos;
            }

            return tailAddr + addrSection.Start;
        }

        /// <summary>
        /// read tailAddress from ps4 memory (slower but correct)
        /// </summary>
        /// <param name="pointerResult">pointer result</param>
        /// <param name="pointerCaches">memory caches for pointer</param>
        /// <returns></returns>
        private ulong ReadTailAddress(((uint BaseSID, uint BasePos, List<long> Offsets) pointer, List<Pointer> pathPointers) pointerResult, in Dictionary<ulong, ulong> pointerCaches)
        {
            Section baseSection = sectionTool.GetSection(pointerResult.pointer.BaseSID);
            if (baseSection == null) return 0;

            var tailAddr = PS4Tool.ReadTailAddress(baseSection.PID, baseSection.Start + pointerResult.pointer.BasePos, pointerResult.pointer.Offsets, pointerCaches);
            return tailAddr;
        }
        #endregion

        #region PointerStruct
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct Pointer
        {
            public uint AddrSID;
            public uint AddrPos;
            public uint ValueSID;
            public uint ValuePos;

            public Pointer(uint addrSID, uint addrPos, uint valueSID, uint valuePos)
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
