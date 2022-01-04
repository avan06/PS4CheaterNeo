using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        Dictionary<int, ResultList> resultsDict;
        Dictionary<int, List<(uint offsetAddr, byte[] resultBytes)>> resultBytesListDict; //for ScanType:Hex、String、Group
        public Query(Main mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            sectionTool = new SectionTool();
            resultsDict = new Dictionary<int, ResultList>();
            resultBytesListDict = new Dictionary<int, List<(uint offsetAddr, byte[] resultBytes)>>();
        }

        #region Event
        private void Query_Load(object sender, EventArgs e)
        {
            foreach (ScanType filterEnum in (ScanType[])Enum.GetValues(typeof(ScanType)))
                ScanTypeBox.Items.Add(new ComboboxItem(filterEnum.GetDescription(), filterEnum));
            ScanTypeBox.SelectedIndex = 2;
            GetProcessesBtn.PerformClick();
        }

        private void Query_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ResultView.Items.Count > 0 && MessageBox.Show("Still in the query, Do you want to close Query?", "Query", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) e.Cancel = true;

            resultsDict = null;
            resultBytesListDict = null;
            GC.Collect();
            Properties.Settings.Default.Save();
        }

        private void GetProcessesBtn_Click(object sender, EventArgs e)
        {
            if (!PS4Tool.Connect((string)Properties.Settings.Default["IP"], out string msg)) throw new Exception(msg);

            int selectedIdx = 0;
            ProcessesBox.Items.Clear();
            ProcessList procList = PS4Tool.GetProcessList();
            foreach (Process process in procList.processes)
            {
                int idx = this.ProcessesBox.Items.Add(new ComboboxItem(process.name, process.pid));
                if (process.name == Constant.DefaultProcess) selectedIdx = idx;
            }
            ProcessesBox.SelectedIndex = selectedIdx;
        }
        public int CompareSection(Section s1, Section s2)
        {
            return s1.Start.CompareTo(s2.Start);
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

                List<int> keys = new List<int>(sectionTool.SectionDict.Keys);
                Section[] sections = new Section[keys.Count];
                for (int sectionIdx = 0; sectionIdx < keys.Count; sectionIdx++) sections[sectionIdx] = sectionTool.SectionDict[keys[sectionIdx]];
                Array.Sort(sections, CompareSection);

                SectionView.BeginUpdate();
                for (int sectionIdx = 0; sectionIdx < sections.Length; sectionIdx++)
                {
                    Section section = sections[sectionIdx];
                    if (IsFilterBox.Checked && section.IsFilter) continue;

                    string start = String.Format("{0:X9}", section.Start);
                    int itemIdx = SectionView.Items.Count;
                    SectionView.Items.Add(sectionIdx.ToString(), start, 0);
                    SectionView.Items[itemIdx].SubItems.Add(section.Name);
                    SectionView.Items[itemIdx].SubItems.Add(section.Prot.ToString("X"));
                    SectionView.Items[itemIdx].SubItems.Add((section.Length / 1024).ToString() + "KB");
                    SectionView.Items[itemIdx].SubItems.Add(section.SID.ToString());
                    if (section.Offset != 0)
                    {
                        SectionView.Items[itemIdx].SubItems.Add(section.Offset.ToString("X"));
                    }
                    if (section.IsFilter)
                    {
                        SectionView.Items[itemIdx].Tag = "filter";
                        SectionView.Items[itemIdx].ForeColor = Color.DarkGray;
                        SectionView.Items[itemIdx].BackColor = Color.DimGray;
                    }
                    if (section.Name.StartsWith("executable"))
                    {
                        SectionView.Items[itemIdx].ForeColor = Color.GreenYellow;
                    }
                    else if (section.Name.Contains("NoName"))
                    {
                        SectionView.Items[itemIdx].ForeColor = Color.Red;
                    }
                    else if (Regex.IsMatch(section.Name, @"^\[\d+\]$"))
                    {
                        SectionView.Items[itemIdx].ForeColor = Color.HotPink;
                    }
                }
                SectionView.EndUpdate();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.StackTrace, exception.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void NewBtn_Click(object sender, EventArgs e)
        {
            if (ScanWorker.IsBusy)
            {
                if (MessageBox.Show("Still in the scanning, Do you want to stop scan?", "Scan",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) ScanWorker.CancelAsync();
                else return;
            }

            ResultView.Items.Clear();
            if (resultsDict != null) resultsDict.Clear();
            if (resultBytesListDict != null) resultBytesListDict.Clear();
            resultsDict = new Dictionary<int, ResultList>();
            resultBytesListDict = new Dictionary<int, List<(uint offsetAddr, byte[] resultBytes)>>();
            GC.Collect();

            ScanBtn.Text = "First Scan";
            ScanTypeBox.Enabled = true;
            AlignmentBox.Enabled = true;
            NewBtn.Enabled = false;
        }

        private void ScanBtn_Click(object sender, EventArgs e)
        {
            if (ScanWorker.IsBusy)
            {
                if (MessageBox.Show("Still in the scanning, Do you want to stop scan?", "Scan", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) ScanWorker.CancelAsync();
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
                Enum.TryParse(((ComboboxItem)(ScanTypeBox.SelectedItem)).Value.ToString(), out ScanType scanType);
                Enum.TryParse(CompareTypeBox.SelectedItem.ToString(), out CompareType compareType);

                var AddrMin = ulong.Parse(AddrMinBox.Text, NumberStyles.HexNumber);
                var AddrMax = ulong.Parse(AddrMaxBox.Text, NumberStyles.HexNumber);
                if (AddrMin > AddrMax && MessageBox.Show(String.Format("AddrMin({1:X}) > AddrMax({0:X})", AddrMin, AddrMax), "Scan", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK) return;

                comparerTool = new ComparerTool(scanType, compareType, value0, value1);

                ScanBtn.Text = "Stop";
                ScanWorker.RunWorkerAsync((pid, value0, value1, alignment, isFilter, scanType, compareType, AddrMin, AddrMax));

                ScanTypeBox.Enabled = false;
                AlignmentBox.Enabled = false;
                NewBtn.Enabled = true;
            }
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            ComboboxItem process = (ComboboxItem)ProcessesBox.SelectedItem;
            int pid = (int)process.Value;
            ProcessMap pMap = PS4Tool.GetProcessMaps(pid);
            if (pMap.entries == null || ResultView.Items.Count == 0) return;

            bool alignment = AlignmentBox.Checked;
            bool isFilter = IsFilterBox.Checked;

            if (RefreshWorker.IsBusy) return;
            RefreshWorker.RunWorkerAsync((pid, alignment, isFilter));
        }

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

        private void IsFilterBox_CheckedChanged(object sender, EventArgs e)
        {
            int idx = ProcessesBox.SelectedIndex;
            if (idx == -1) return;

            if (IsFilterBox.Checked)
            {
                SectionView.BeginUpdate();
                foreach (ListViewItem item in SectionView.Items)
                {
                    if ("filter".Equals(item.Tag))
                    {
                        item.Checked = false; //Ensure that MappedSectionList is not selected
                        item.Remove();
                    }
                }
                SectionView.EndUpdate();
            }
            else
            {
                ProcessesBox.SelectedIndex = 0;
                ProcessesBox.SelectedIndex = idx;
            }
        }

        private void ResultView_DoubleClick(object sender, EventArgs e)
        {
            if (ResultView.SelectedItems.Count == 1)
            {
                ListViewItem resultItem = ResultView.SelectedItems[0];
                (int sid, int resultIdx) = ((int sid, int resultIdx))resultItem.Tag;
                ScanType scanType = comparerTool.scanType;
                Section section = sectionTool.GetSection(sid);
                uint offsetAddr = 0;
                ulong resultValue = 0;
                byte[] resultBytes;
                if (resultsDict.TryGetValue(sid, out ResultList results))
                {
                    (offsetAddr, resultBytes) = results.Read(resultIdx);
                    resultValue = ScanTool.BytesToULong(resultBytes);
                } 
                else if (resultBytesListDict.TryGetValue(sid, out List<(uint offsetAddr, byte[] resultBytes)> resultBytesList))
                {
                    (offsetAddr, resultBytes) = resultBytesList[resultIdx];
                    resultValue = ScanTool.BytesToULong(resultBytes);
                    int tIdx = resultIdx % comparerTool.groupTypes.Count;
                    (scanType, _, _) = comparerTool.groupTypes[tIdx];
                }
                if (offsetAddr > 0) mainForm.AddToCheatGrid(section, offsetAddr, scanType, resultValue, false, "", false, null);
            }
        }

        private string searchSectionName = "";
        private void SectionSearchBtn_Click(object sender, EventArgs e)
        {
            if (InputBox.Show("Search", "Enter the value of the search section name", ref searchSectionName, null) != DialogResult.OK) return;

            int startIndex = 0;
            ListView.SelectedListViewItemCollection items = SectionView.SelectedItems;
            if (items.Count == 0) return;

            if (items.Count > 0 && items[items.Count - 1].Index + 1 < SectionView.Items.Count)
            {
                startIndex = items[items.Count - 1].Index + 1;
            }
            ListViewItem item = SectionView.FindItemWithText(searchSectionName, true, startIndex);
            if (item != null)
            {
                SectionView.Items[item.Index].Selected = true;
                SectionView.Items[item.Index].EnsureVisible();
            }
        }

        private void FilterRuleBtn_Click(object sender, EventArgs e)
        {
            string sectionFilterKeys = (string)Properties.Settings.Default["SectionFilterKeys"];

            if (InputBox.Show("Section Filter", "Enter the value of the filter keys", ref sectionFilterKeys, null) != DialogResult.OK) return;

            Properties.Settings.Default["SectionFilterKeys"] = sectionFilterKeys;
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
                (int sid, int resultIdx) = ((int sid, int resultIdx))resultItem.Tag;
                ScanType scanType = comparerTool.scanType;
                Section section = sectionTool.GetSection(sid);
                uint offsetAddr = 0;
                ulong resultValue = 0;
                byte[] resultBytes;
                if (resultsDict.TryGetValue(sid, out ResultList results))
                {
                    (offsetAddr, resultBytes) = results.Read(resultIdx);
                    resultValue = ScanTool.BytesToULong(resultBytes);
                } 
                else if (resultBytesListDict.TryGetValue(sid, out List<(uint offsetAddr, byte[] resultBytes)> resultBytesList))
                {
                    (offsetAddr, resultBytes) = resultBytesList[resultIdx];
                    resultValue = ScanTool.BytesToULong(resultBytes);
                    int tIdx = resultIdx % comparerTool.groupTypes.Count;
                    (scanType, _, _) = comparerTool.groupTypes[tIdx];
                }
                if (offsetAddr > 0) mainForm.AddToCheatGrid(section, offsetAddr, scanType, resultValue, false, "", false, null);
            }
        }

        private void ResultViewHexEditor_Click(object sender, EventArgs e)
        {
            if (ResultView.SelectedItems == null) return;
            if (ResultView.SelectedItems.Count != 1) return;

            ListView.SelectedListViewItemCollection items = ResultView.SelectedItems;

            var resultItem = items[0];
            (int sid, int resultIdx) = ((int sid, int resultIdx))resultItem.Tag;
            Section section = sectionTool.GetSection(sid);
            uint offsetAddr = 0;
            if (resultsDict.TryGetValue(sid, out ResultList results)) (offsetAddr, _) = results.Read(resultIdx);
            else if (resultBytesListDict.TryGetValue(sid, out List<(uint offsetAddr, byte[] resultBytes)> resultBytesList)) (offsetAddr, _) = resultBytesList[resultIdx];

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
                (int sid, int resultIdx) = ((int sid, int resultIdx))resultItem.Tag;
                Section section = sectionTool.GetSection(sid);
                uint offsetAddr = 0;
                if (resultsDict.TryGetValue(sid, out ResultList results)) (offsetAddr, _) = results.Read(resultIdx);
                else if (resultBytesListDict.TryGetValue(sid, out List<(uint offsetAddr, byte[] resultBytes)> resultBytesList)) (offsetAddr, _) = resultBytesList[resultIdx];
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
            //    ulong address = ulong.Parse(ResultView.SelectedItems[0].
            //        SubItems[(int)ResultCol.ResultListAddress].Text, NumberStyles.HexNumber);

            //    int sectionID = processManager.MappedSectionList.GetMappedSectionID(address);

            //    dump_dialog(sectionID);
            //}
        }

        private void ResultViewFindPointer_Click(object sender, EventArgs e)
        {
            if (ResultView.SelectedItems == null)
                return;

            if (ResultView.SelectedItems.Count != 1)
                return;

            try
            {
                ulong address = ulong.Parse(ResultView.SelectedItems[0].SubItems[(int)ResultCol.ResultListAddress].Text, NumberStyles.HexNumber);
                ScanType scanType = this.ParseFromDescription<ScanType>(ResultView.SelectedItems[0].SubItems[(int)ResultCol.ResultListType].Text);

                PointerFinder pointerFinder = new PointerFinder(mainForm, address, scanType);
                pointerFinder.Show();
            }
            catch
            {

            }
        }
        #endregion

        #region Worker
        //Invoke(new MethodInvoker(() => {fileListView.BeginUpdate();}));
        private void ScanWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                System.Diagnostics.Stopwatch tickerMajor = System.Diagnostics.Stopwatch.StartNew();

                (int pid, string value0, string value1, bool alignment, bool isFilter, ScanType scanType, CompareType compareType, ulong AddrMin, ulong AddrMax) =
                    ((int pid, string value0, string value1, bool alignment, bool isFilter, ScanType scanType, CompareType compareType, ulong AddrMin, ulong AddrMax))e.Argument;

                int hitCnt = 0;
                int scanStep = (comparerTool.scanType == ScanType.Hex || comparerTool.scanType == ScanType.String_) ? 1 :
                    alignment ? (comparerTool.scanTypeLength > 4 ? 4 : comparerTool.scanTypeLength) : 1;
                long processedMemoryLen = 0;
                List<int> keys = new List<int>(sectionTool.SectionDict.Keys);
                keys.Sort();
                ScanWorker.ReportProgress(1);
                int maxThreads = 3;
                SemaphoreSlim semaphore = new SemaphoreSlim(maxThreads);
                Task<(int, TimeSpan)>[] tasks = new Task<(int, TimeSpan)>[keys.Count];
                for (int sectionIdx = 0; sectionIdx < keys.Count; sectionIdx++)
                {
                    if (ScanWorker.CancellationPending) break;

                    Section section = sectionTool.SectionDict[keys[sectionIdx]];
                    tasks[sectionIdx] = Task.Run<(int, TimeSpan)>(() =>
                    {
                        if (ScanWorker.CancellationPending) return (section.SID, tickerMajor.Elapsed);
                        if ((isFilter && section.IsFilter) || !section.Check || section.Start + (ulong)section.Length < AddrMin || section.Start > AddrMax) return (section.SID, tickerMajor.Elapsed);
                        semaphore.Wait();

                        ResultList results = null;
                        List<(uint offsetAddr, byte[] resultBytes)> resultBytesList = null;

                        if (comparerTool.groupTypes == null) results = Comparer(section, scanStep, AddrMin, AddrMax);
                        else resultBytesList = ComparerGroup(section, scanStep, AddrMin, AddrMax);

                        int resultCnt = 0;
                        if (results != null && results.Count > 0)
                        {
                            resultCnt = results.Count;
                            resultsDict[section.SID] = results;
                        }
                        if (resultBytesList != null && resultBytesList.Count > 0)
                        {
                            resultCnt = resultBytesList.Count;
                            resultBytesListDict[section.SID] = resultBytesList;
                        }

                        if (resultCnt > 0) hitCnt += resultCnt;
                        else section.Check = false;

                        processedMemoryLen += section.Length;
                        ScanWorker.ReportProgress((int)(((float)processedMemoryLen / sectionTool.TotalMemorySize) * 100), (tickerMajor.Elapsed, string.Format("{0}MB, Count: {1}", processedMemoryLen / (1024 * 1024), hitCnt)));
                        semaphore.Release();
                        return (section.SID, tickerMajor.Elapsed);
                    });
                }
                Task whenTasks = Task.WhenAll(tasks);
                whenTasks.Wait();
                Invoke(new MethodInvoker(() => {
                    for (int sectionIdx = 0; sectionIdx < SectionView.Items.Count; ++sectionIdx)
                    {
                        ListViewItem sectionItem = SectionView.Items[sectionIdx];
                        if (!sectionItem.Checked) continue;

                        int sid = int.Parse(sectionItem.SubItems[(int)SectionCol.SectionViewSID].Text);
                        Section section = sectionTool.GetSection(sid);
                        if (section.Check == false) sectionItem.Checked = false;
                    }
                }));

                ScanWorker.ReportProgress(100);
                e.Result = pid;
                GC.Collect();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.StackTrace, exception.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private List<(uint offsetAddr, byte[] resultBytes)> ComparerGroup(Section section, int step, ulong AddrMin, ulong AddrMax)
        {
            if (!resultBytesListDict.TryGetValue(section.SID, out List<(uint offsetAddr, byte[] resultBytes)> resultBytesList)) resultBytesList = new List<(uint offsetAddr, byte[] resultBytes)>();
            if (ResultView.Items.Count == 0)
            {
                List<(uint offsetAddr, byte[] resultBytes)> resultNewList = new List<(uint offsetAddr, byte[] resultBytes)>();
                byte[] buffer = PS4Tool.ReadMemory(section.PID, section.Start, section.Length);
                for (int scanIdx = 0; scanIdx + comparerTool.scanTypeLength < buffer.LongLength; scanIdx += step)
                {
                    if (ScanWorker.CancellationPending) break;
                    if (section.Start + (ulong)scanIdx < AddrMin || section.Start + (ulong)scanIdx > AddrMax) continue;

                    int tmpIdx = scanIdx;
                    for (int gIdx = 0; gIdx < comparerTool.groupTypes.Count; gIdx++)
                    {
                        (ScanType scanType, int groupTypeLength, bool isAny) = comparerTool.groupTypes[gIdx];
                        if (scanIdx + groupTypeLength > buffer.LongLength)
                        {
                            resultNewList.Clear();
                            break;
                        }
                        byte[] valueBytes = comparerTool.groupValues[gIdx];
                        byte[] newValue = new byte[groupTypeLength];
                        Buffer.BlockCopy(buffer, scanIdx, newValue, 0, groupTypeLength);
                       if (isAny || ScanTool.ComparerExact(scanType, newValue, valueBytes))
                        {
                            resultNewList.Add(((uint)scanIdx, newValue));
                            scanIdx += groupTypeLength;
                        }
                        else
                        {
                            resultNewList.Clear();
                            if (scanIdx > tmpIdx) scanIdx = tmpIdx;
                            break;
                        }
                    }
                    if (resultNewList.Count > 0) resultBytesList.AddRange(resultNewList);
                    resultNewList.Clear();
                }
            }
            else
            {
                List<(uint offsetAddr, byte[] resultBytes)> resultNextBytesList = new List<(uint offsetAddr, byte[] resultBytes)>();
                byte[] buffer = null;
                if (resultBytesList.Count >= 50) buffer = PS4Tool.ReadMemory(section.PID, section.Start, section.Length);
                List<(uint offsetAddr, byte[] resultBytes)> resultNewList = new List<(uint offsetAddr, byte[] resultBytes)>();
                for (int rIdx1 = 0; rIdx1 < resultBytesList.Count; rIdx1++)
                {
                    if (ScanWorker.CancellationPending) break;

                    int gIdx = rIdx1 % comparerTool.groupTypes.Count;
                    if (gIdx == 0 && resultNewList.Count > 0)
                    {
                        resultNextBytesList.AddRange(resultNewList);
                        resultNewList.Clear();
                    }

                    (uint offsetAddr, _) = resultBytesList[rIdx1];

                    if (section.Start + offsetAddr < AddrMin || section.Start + offsetAddr > AddrMax) continue;

                    (ScanType scanType, int groupTypeLength, bool isAny) = comparerTool.groupTypes[gIdx];
                    byte[] valueBytes = comparerTool.groupValues[gIdx];
                    byte[] newValue = new byte[groupTypeLength];
                    if (resultBytesList.Count < 50) newValue = PS4Tool.ReadMemory(section.PID, offsetAddr + section.Start, groupTypeLength);
                    else Buffer.BlockCopy(buffer, (int)offsetAddr, newValue, 0, groupTypeLength);
                    if (isAny || ScanTool.ComparerExact(scanType, newValue, valueBytes)) resultNewList.Add((offsetAddr, newValue));
                    else
                    {
                        resultNewList.Clear();
                        rIdx1 += comparerTool.groupTypes.Count - gIdx - 1;
                    }
                }
                if (resultNewList.Count > 0)
                {
                    resultNextBytesList.AddRange(resultNewList);
                    resultNewList.Clear();
                }
                resultBytesList.Clear();
                resultBytesList = resultNextBytesList;
            }

            return resultBytesList;
        }

        private ResultList Comparer(Section section, int scanStep, ulong AddrMin, ulong AddrMax)
        {
            if (!resultsDict.TryGetValue(section.SID, out ResultList results)) results = new ResultList(comparerTool.scanTypeLength, scanStep);
            if (ResultView.Items.Count == 0)
            {
                byte[] buffer = PS4Tool.ReadMemory(section.PID, section.Start, section.Length);
                for (int scanIdx = 0; scanIdx + comparerTool.scanTypeLength < buffer.LongLength; scanIdx += scanStep)
                {
                    if (ScanWorker.CancellationPending) break;
                    if (section.Start + (ulong)scanIdx < AddrMin || section.Start + (ulong)scanIdx > AddrMax) continue;
                    
                    byte[] newValue = new byte[comparerTool.scanTypeLength];
                    Buffer.BlockCopy(buffer, scanIdx, newValue, 0, comparerTool.scanTypeLength);
                    if (comparerTool.value0Byte == null)
                    {
                        ulong longValue = ScanTool.BytesToULong(newValue);
                        if (ScanTool.Comparer(comparerTool, longValue, 0)) results.Add((uint)scanIdx, newValue);
                    }
                    else if (ScanTool.ComparerExact(comparerTool.scanType, newValue, comparerTool.value0Byte)) results.Add((uint)scanIdx, newValue);
                }
            }
            else
            {
                byte[] buffer = null;
                ResultList newResults;
                newResults = new ResultList(comparerTool.scanTypeLength, scanStep);
                if (results.Count >= 50) buffer = PS4Tool.ReadMemory(section.PID, section.Start, section.Length);

                for (int rIdx = 0; rIdx < results.Count; rIdx++)
                {
                    if (ScanWorker.CancellationPending) break;

                    (uint offsetAddr, byte[] oldBytes) = results.Read(rIdx);
                    if (section.Start + offsetAddr < AddrMin || section.Start + offsetAddr > AddrMax) continue;

                    ulong oldData = ScanTool.BytesToULong(oldBytes);
                    byte[] newValue = new byte[comparerTool.scanTypeLength];
                    if (results.Count < 50) newValue = PS4Tool.ReadMemory(section.PID, offsetAddr + section.Start, comparerTool.scanTypeLength);
                    else Buffer.BlockCopy(buffer, (int)offsetAddr, newValue, 0, comparerTool.scanTypeLength);

                    ulong newData = ScanTool.BytesToULong(newValue);
                    if (comparerTool.value0Byte == null)
                    {
                        if (ScanTool.Comparer(comparerTool, newData, oldData)) newResults.Add(offsetAddr, newValue);
                    }
                    else if (ScanTool.ComparerExact(comparerTool.scanType, newValue, comparerTool.value0Byte)) newResults.Add(offsetAddr, newValue);
                }
                results.Clear();
                results = newResults;
            }
            return results;
        }

        private void ScanWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ToolStripBar.Value = e.ProgressPercentage <= ToolStripBar.Maximum ? e.ProgressPercentage : ToolStripBar.Maximum;

            if (e.UserState == null) return;

            (TimeSpan tickerMajor, string msg) = ((TimeSpan tickerMajor, string msg))e.UserState;
            ToolStripMsg.Text = string.Format("Scan elapsed:{0}s. {1}", tickerMajor.TotalSeconds, msg);
        }

        private void ScanWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ToolStripMsg.Text = e.Error.ToString();
                return;
            }
            if (e.Result == null) return;
            
            if (resultsDict.Count > 0)
            {
                int count = 0;
                ResultView.Items.Clear();
                ResultView.BeginUpdate();

                List<int> keys = new List<int>(resultsDict.Keys);
                keys.Sort();
                for (int sectionIdx = 0; sectionIdx < keys.Count; sectionIdx++)
                {
                    Section section = sectionTool.SectionDict[keys[sectionIdx]];
                    resultsDict.TryGetValue(section.SID, out ResultList results);
                    if (results == null || results.Count == 0) continue;

                    Color backColor = default;
                    for (results.Begin(); !results.End(); results.Next())
                    {
                        if (++count > 0x2000) break;

                        (uint offsetAddr, byte[] oldBytes) = results.Read();
                        string typeStr, valueStr, valueHex;
                        ulong oldValue = ScanTool.BytesToULong(oldBytes);
                        typeStr = comparerTool.scanType.GetDescription();
                        valueStr = ScanTool.ULongToString(comparerTool.scanType, oldValue);
                        valueHex = ScanTool.ULongToString(comparerTool.scanType, oldValue, true);

                        int itemIdx = ResultView.Items.Count;
                        ResultView.Items.Add(offsetAddr.ToString("X8"), (offsetAddr + section.Start).ToString("X8"), 0);
                        ResultView.Items[itemIdx].Tag = (section.SID, results.Iterator);
                        ResultView.Items[itemIdx].SubItems.Add(typeStr);
                        ResultView.Items[itemIdx].SubItems.Add(valueStr);
                        ResultView.Items[itemIdx].SubItems.Add(valueHex);
                        ResultView.Items[itemIdx].SubItems.Add(string.Format("{0}_{1}_{2}_{3}", section.Start.ToString("X"), section.Name, section.Prot.ToString("X"), section.Offset.ToString("X")));
                        ResultView.Items[itemIdx].BackColor = backColor;
                    }
                }
                ResultView.EndUpdate();
                ScanTypeBox_SelectedIndexChanged(sender, null);
                ScanBtn.Text = "Next Scan";
            }
            else if (resultBytesListDict.Count > 0)
            {
                int count = 0;
                ResultView.Items.Clear();
                ResultView.BeginUpdate();

                List<int> keys = new List<int>(resultBytesListDict.Keys);
                keys.Sort();
                for (int sectionIdx = 0; sectionIdx < keys.Count; sectionIdx++)
                {
                    Section section = sectionTool.SectionDict[keys[sectionIdx]];
                    resultBytesListDict.TryGetValue(section.SID, out List<(uint offsetAddr, byte[] resultBytes)> resultBytesList);
                    if (resultBytesList == null || resultBytesList.Count == 0) continue;

                    Color backColor = default;
                    for (int resultIdx = 0; resultIdx < resultBytesList.Count; resultIdx++)
                    {
                        if (++count > 0x2000) break;

                        string valueStr, valueHex, typeStr;
                        ScanType scanType = comparerTool.scanType;
                        (uint offsetAddr, byte[] resultBytes) = resultBytesList[resultIdx];
                        if (scanType == ScanType.Group)
                        {
                            int tIdx = resultIdx % comparerTool.groupTypes.Count;
                            if (tIdx == 0)
                            {
                                if (backColor != default) backColor = default;
                                else backColor = Color.DarkSlateGray;
                            }
                            (scanType, _, _) = comparerTool.groupTypes[tIdx];
                        }
                        valueStr = ScanTool.BytesToString(scanType, resultBytes);
                        valueHex = ScanTool.BytesToString(scanType, resultBytes, true);

                        int itemIdx = ResultView.Items.Count;
                        ResultView.Items.Add(offsetAddr.ToString("X8"), (offsetAddr + section.Start).ToString("X8"), 0);
                        ResultView.Items[itemIdx].Tag = (section.SID, resultIdx);
                        ResultView.Items[itemIdx].SubItems.Add(scanType.GetDescription());
                        ResultView.Items[itemIdx].SubItems.Add(valueStr);
                        ResultView.Items[itemIdx].SubItems.Add(valueHex);
                        ResultView.Items[itemIdx].SubItems.Add(string.Format("{0}_{1}_{2}_{3}", section.Start.ToString("X"), section.Name, section.Prot.ToString("X"), section.Offset.ToString("X")));
                        ResultView.Items[itemIdx].BackColor = backColor;
                    }
                }
                ResultView.EndUpdate();
                ScanTypeBox_SelectedIndexChanged(sender, null);
                ScanBtn.Text = "Next Scan";
            }
        }

        private void RefreshWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Diagnostics.Stopwatch tickerMajor = System.Diagnostics.Stopwatch.StartNew();
            (int pid, bool alignment, bool isFilter) = ((int pid, bool alignment, bool isFilter))e.Argument;
            if (resultsDict.Count == 0 && resultBytesListDict.Count == 0) return;

            int hitCnt = 0;
            Invoke(new MethodInvoker(() =>
            {
                ResultView.Items.Clear();
                ResultView.BeginUpdate();
            }));
            List<int> keys = new List<int>(sectionTool.SectionDict.Keys);
            keys.Sort();
            for (int sectionIdx = 0; sectionIdx < keys.Count; sectionIdx++)
            {
                RefreshWorker.ReportProgress((int)(((float)(sectionIdx + 1) / keys.Count) * 100), (tickerMajor.Elapsed, string.Format("Count: {0}", hitCnt)));
                Section section = sectionTool.SectionDict[keys[sectionIdx]];
                if ((isFilter && section.IsFilter) || !section.Check) continue;

                resultsDict.TryGetValue(section.SID, out ResultList results);
                resultBytesListDict.TryGetValue(section.SID, out List<(uint offsetAddr, byte[] resultBytes)> resultBytesList);

                ScanTool.ScanTypeLengthDict.TryGetValue(comparerTool.scanType, out int scanTypeLength);

                byte[] buffer = null;
                if (scanTypeLength > 0)
                {
                    if (results.Count >= 50) buffer = PS4Tool.ReadMemory(section.PID, section.Start, section.Length);

                    for (results.Begin(); !results.End(); results.Next())
                    {
                        if (++hitCnt > 0x2000) continue;

                        (uint offsetAddr, _) = results.Read();
                        string typeStr, valueStr, valueHex;

                        int newLen = scanTypeLength;
                        byte[] newValue = new byte[newLen];
                        if (results.Count < 50) newValue = PS4Tool.ReadMemory(pid, offsetAddr + section.Start, newLen);
                        else Buffer.BlockCopy(buffer, (int)offsetAddr, newValue, 0, newLen);

                        results.Set(newValue);
                        typeStr = comparerTool.scanType.GetDescription();
                        valueStr = ScanTool.BytesToString(comparerTool.scanType, newValue);
                        valueHex = ScanTool.BytesToString(comparerTool.scanType, newValue, true);

                        Invoke(new MethodInvoker(() => {
                            int itemIdx = ResultView.Items.Count;
                            ResultView.Items.Add(offsetAddr.ToString("X8"), (offsetAddr + section.Start).ToString("X8"), 0);
                            ResultView.Items[itemIdx].Tag = (section.SID, results.Iterator);
                            ResultView.Items[itemIdx].SubItems.Add(typeStr);
                            ResultView.Items[itemIdx].SubItems.Add(valueStr);
                            ResultView.Items[itemIdx].SubItems.Add(valueHex);
                            ResultView.Items[itemIdx].SubItems.Add(string.Format("{0}_{1}_{2}_{3}", section.Start.ToString("X"), section.Name, section.Prot.ToString("X"), section.Offset.ToString("X")));
                        }));
                    }
                }
                else
                {
                    Color backColor = default;
                    if (resultBytesList.Count >= 50) buffer = PS4Tool.ReadMemory(section.PID, section.Start, section.Length);
                    for (int resultIdx = 0; resultIdx < resultBytesList.Count; resultIdx++)
                    {
                        if (++hitCnt > 0x2000) continue;

                        string valueStr, valueHex;
                        (uint offsetAddr, byte[] resultBytes) = resultBytesList[resultIdx];
                        int newLen = resultBytes.Length;
                        byte[] newValue = new byte[newLen];
                        if (resultBytesList.Count < 50) newValue = PS4Tool.ReadMemory(pid, offsetAddr + section.Start, newLen);
                        else Buffer.BlockCopy(buffer, (int)offsetAddr, newValue, 0, newLen);

                        ScanType scanType = comparerTool.scanType;
                        resultBytesList[resultIdx] = (offsetAddr, newValue);
                        if (scanType == ScanType.Group)
                        {
                            int tIdx = resultIdx % comparerTool.groupTypes.Count;
                            if (tIdx == 0)
                            {
                                if (backColor != default) backColor = default;
                                else backColor = Color.DarkSlateGray;
                            }
                            (scanType, _, _) = comparerTool.groupTypes[tIdx];
                        }
                        valueStr = ScanTool.BytesToString(scanType, resultBytes);
                        valueHex = ScanTool.BytesToString(scanType, resultBytes, true);

                        Invoke(new MethodInvoker(() => {
                            int itemIdx = ResultView.Items.Count;
                            ResultView.Items.Add(offsetAddr.ToString("X8"), (offsetAddr + section.Start).ToString("X8"), 0);
                            ResultView.Items[itemIdx].Tag = (section.SID, resultIdx);
                            ResultView.Items[itemIdx].SubItems.Add(comparerTool.scanType.GetDescription());
                            ResultView.Items[itemIdx].SubItems.Add(valueStr);
                            ResultView.Items[itemIdx].SubItems.Add(valueHex);
                            ResultView.Items[itemIdx].SubItems.Add(string.Format("{0}_{1}_{2}_{3}", section.Start.ToString("X"), section.Name, section.Prot.ToString("X"), section.Offset.ToString("X")));
                            ResultView.Items[itemIdx].BackColor = backColor;
                        }));
                    }
                }
            }
            Invoke(new MethodInvoker(() => {ResultView.EndUpdate();}));
            RefreshWorker.ReportProgress(100);
        }

        private void RefreshWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ToolStripBar.Value = e.ProgressPercentage;

            if (e.UserState == null) return;

            (TimeSpan tickerMajor, string msg) = ((TimeSpan tickerMajor, string msg))e.UserState;
            ToolStripMsg.Text = string.Format("Refresh elapsed:{0}s. {1}", tickerMajor.TotalSeconds, msg);
        }

        private void RefreshWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
        #endregion
    }
}
