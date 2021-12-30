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
        Dictionary<int, (ScanType scanType, List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)> results, List<(ScanType scanType, int groupTypeLength, bool isAny)> groupTypes)> resultListDict;
        public Query(Main mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            sectionTool = new SectionTool();
            resultListDict = new Dictionary<int, (ScanType scanType, List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)> results, List<(ScanType scanType, int groupTypeLength, bool isAny)> groupTypes)>();
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
                MessageBox.Show("SectionList init failed, " + exception.Message, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void NewBtn_Click(object sender, EventArgs e)
        {
            ResultView.Items.Clear();
            ScanBtn.Text = "First Scan";
            if (resultListDict != null) resultListDict.Clear();
            resultListDict = new Dictionary<int, (ScanType scanType, List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)> results, List<(ScanType scanType, int groupTypeLength, bool isAny)> groupTypes)>();
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

                ScanBtn.Text = "Stop";
                ScanWorker.RunWorkerAsync((pid, value0, value1, alignment, isFilter, scanType, compareType, AddrMin, AddrMax));
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
                (ScanType scanType, List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)> results, List<(ScanType scanType, int groupTypeLength, bool isAny)> groupTypes) = resultListDict[sid];
                (ulong mappedAddr, ulong resultValue, byte[] resultBytes) = results[resultIdx];
                Section section = sectionTool.GetSection(sid);
                mainForm.AddToCheatGrid(section, mappedAddr, scanType, resultValue, false, "", false, null);
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
                (ScanType scanType, List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)> results, List<(ScanType scanType, int groupTypeLength, bool isAny)> groupTypes) = resultListDict[sid];
                (ulong mappedAddr, ulong resultValue, byte[] resultBytes) = results[resultIdx];
                Section section = sectionTool.GetSection(sid);
                mainForm.AddToCheatGrid(section, mappedAddr, scanType, resultValue, false, "", false, null);
            }
        }

        private void ResultViewHexEditor_Click(object sender, EventArgs e)
        {
            if (ResultView.SelectedItems == null) return;

            if (ResultView.SelectedItems.Count != 1) return;

            ListView.SelectedListViewItemCollection items = ResultView.SelectedItems;

            var resultItem = items[0];
            (int sid, int resultIdx) = ((int sid, int resultIdx))resultItem.Tag;
            (ScanType scanType, List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)> results, List<(ScanType scanType, int groupTypeLength, bool isAny)> groupTypes) = resultListDict[sid];
            (ulong mappedAddr, ulong resultValue, byte[] resultBytes) = results[resultIdx];
            Section section = sectionTool.GetSection(sid);

            HexEditor hexEdit = new HexEditor(mainForm, section, (int)mappedAddr);
            hexEdit.Show(this);
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

                int scanTypeLength = 0;
                byte[] value0Byte = null;
                ulong value0Long = 0, value1Long = 0;
                List<byte[]> groupValues = null;
                List<(ScanType scanType, int groupTypeLength, bool isAny)> groupTypes = null;
                if (scanType == ScanType.Group) (groupTypes, groupValues) = generateGroupList(value0, out scanTypeLength);
                else if (ScanTool.ScanTypeLengthDict.TryGetValue(scanType, out scanTypeLength))
                {
                    value0Long = ScanTool.ValueStringToULong(scanType, value0);
                    if (!string.IsNullOrWhiteSpace(value1)) value1Long = ScanTool.ValueStringToULong(scanType, value1);
                }
                else
                {
                    value0Byte = ScanTool.ValueStringToByte(scanType, value0); //for ScanType:Hex、String
                    scanTypeLength = value0Byte.Length;
                }

                int hitCnt = 0;
                int step = alignment ? scanTypeLength : 1;
                long processedMemoryLen = 0;
                List<int> keys = new List<int>(sectionTool.SectionDict.Keys);
                keys.Sort();
                ScanWorker.ReportProgress(1);
                SemaphoreSlim semaphore = new SemaphoreSlim(2);
                Task<(int, TimeSpan)>[] tasks = new Task<(int, TimeSpan)>[keys.Count];
                for (int sectionIdx = 0; sectionIdx < keys.Count; sectionIdx++)
                {
                    if (ScanWorker.CancellationPending) break;

                    Section section = sectionTool.SectionDict[keys[sectionIdx]];
                    tasks[sectionIdx] = Task.Run<(int, TimeSpan)>(() =>
                    {
                        if ((isFilter && section.IsFilter) || !section.Check || section.Start + (ulong)section.Length < AddrMin || section.Start > AddrMax) return (section.SID, tickerMajor.Elapsed);
                        semaphore.Wait();

                        (ScanType scanType, List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)> resultList, List<(ScanType scanType, int groupTypeLength, bool isAny)> groupTypes) result;

                        if (groupTypes == null) result = Comparer(section, scanType, compareType, scanTypeLength, step, value0Byte, value0Long, value1Long, AddrMin, AddrMax);
                        else result = ComparerGroup(section, scanTypeLength, step, groupTypes, groupValues, AddrMin, AddrMax);

                        resultListDict[section.SID] = result;
                        if (result.resultList.Count == 0) section.Check = false;
                        else hitCnt += result.resultList.Count;
                        processedMemoryLen += section.Length;
                        ScanWorker.ReportProgress((int)(((float)processedMemoryLen / sectionTool.TotalMemorySize) * 100), (tickerMajor.Elapsed, string.Format("{0}MB, Count: {1}", processedMemoryLen / (1024 * 1024), hitCnt)));
                        semaphore.Release();
                        return (section.SID, tickerMajor.Elapsed);
                    });
                }
                Task<(int, TimeSpan)[]> whenTasks = Task.WhenAll(tasks);
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
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private (List<(ScanType scanType, int groupTypeLength, bool isAny)> groupTypes, List<byte[]> groupValues) generateGroupList(string value0, out int scanTypeLength)
        {
            scanTypeLength = 1;
            var groupTypes = new List<(ScanType scanType, int groupTypeLength, bool isAny)>();
            var groupValues = new List<byte[]>();
            var cmd = new Dictionary<string, string>();
            value0 = value0.ToUpper().Trim();
            value0 = Regex.Replace(value0, @" *([,:]) *", "$1"); //Remove useless whitespace
            value0 = Regex.Replace(value0, @" +", " "); //Remove excess whitespace
            int start = 0;
            for (int idx = 0; idx <= value0.Length; ++idx) //Parse input value
            {
                if (idx == value0.Length && start < value0.Length) cmd["scanVal"] = value0.Substring(start, value0.Length - start); //Get the last scanVal
                else if (value0[idx].Equals(':')) //Get typeKey value
                {
                    cmd["typeKey"] = value0.Substring(start, idx - start);
                    start = idx + 1;
                }
                else if (Regex.IsMatch(value0[idx].ToString(), "[, ]")) //Get scanVal value
                {
                    cmd["scanVal"] = value0.Substring(start, idx - start);
                    start = idx + 1;
                }

                
                if (cmd.TryGetValue("scanVal", out string scanVal))
                {
                    ScanType scanType;
                    cmd.TryGetValue("typeKey", out string typeKey);

                    if (typeKey == "1") scanType = ScanType.Byte_;
                    else if (typeKey == "2") scanType = ScanType.Bytes_2;
                    else if (typeKey == "4") scanType = ScanType.Bytes_4;
                    else if (typeKey == "8") scanType = ScanType.Bytes_8;
                    else if (typeKey == "F") scanType = ScanType.Float_;
                    else if (typeKey == "D") scanType = ScanType.Double_;
                    else if (typeKey == "H") scanType = ScanType.Hex;
                    else scanType = ScanType.Bytes_4;

                    bool isAny = false;
                    byte[] valueBytes;
                    if (scanVal == "*" || scanVal == "?")
                    {
                        isAny = true;
                        ScanTool.ScanTypeLengthDict.TryGetValue(scanType, out int anyLength);
                        valueBytes = new byte[anyLength];
                    }
                    else valueBytes = ScanTool.ValueStringToByte(scanType, cmd["scanVal"]);

                    if (!ScanTool.ScanTypeLengthDict.TryGetValue(scanType, out int groupTypeLength)) groupTypeLength = valueBytes.Length;
                    else if (groupTypes.Count == 0) scanTypeLength = groupTypeLength;

                    groupTypes.Add((scanType, groupTypeLength, isAny));
                    groupValues.Add(valueBytes);
                    cmd = new Dictionary<string, string>();
                }
            }

            return (groupTypes, groupValues);
        }
        private (ScanType scanType, List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)> resultList, List<(ScanType scanType, int groupTypeLength, bool isAny)> groupTypes) ComparerGroup(
            Section section, int scanTypeLength, int step, List<(ScanType scanType, int groupTypeLength, bool isAny)> groupTypes, List<byte[]> groupValues, ulong AddrMin, ulong AddrMax)
        {
            (ScanType scanType, List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)> resultList, List<(ScanType scanType, int groupTypeLength, bool isAny)> groupTypes) result;
            if (!resultListDict.TryGetValue(section.SID, out result))
            {
                result.scanType = ScanType.Group;
                result.resultList = new List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)>();
                result.groupTypes = new List<(ScanType scanType, int groupTypeLength, bool isAny)>();
            }

            List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)> resultList = new List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)>();
            if (ResultView.Items.Count == 0)
            {
                List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)> resultNewList = new List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)>();
                byte[] buffer = PS4Tool.ReadMemory(section.PID, section.Start, section.Length);
                for (int scanIdx = 0; scanIdx + scanTypeLength < buffer.LongLength; scanIdx += step)
                {
                    if (ScanWorker.CancellationPending) break;
                    if (section.Start + (ulong)scanIdx < AddrMin || section.Start + (ulong)scanIdx > AddrMax) continue;

                    int tmpIdx = scanIdx; //+ scanTypeLength;
                    for (int gIdx = 0; gIdx < groupTypes.Count; gIdx++)
                    {
                        (ScanType scanType, int groupTypeLength, bool isAny) = groupTypes[gIdx];
                        if (scanIdx + groupTypeLength > buffer.LongLength)
                        {
                            resultNewList.Clear();
                            break;
                        }
                        byte[] valueBytes = groupValues[gIdx];
                        byte[] newValue = new byte[groupTypeLength];
                        Buffer.BlockCopy(buffer, scanIdx, newValue, 0, groupTypeLength);
                       if (isAny || ScanTool.Comparer(scanType, newValue, valueBytes))
                        {
                            resultNewList.Add(((ulong)scanIdx, 0, newValue));
                            scanIdx += groupTypeLength;
                        }
                        else
                        {
                            resultNewList.Clear();
                            if (scanIdx > tmpIdx) scanIdx = tmpIdx;
                            break;
                        }
                    }
                    if (resultNewList.Count > 0) result.resultList.AddRange(resultNewList);
                    resultNewList.Clear();
                }
                if (result.resultList.Count > 0) result.groupTypes = groupTypes;
            }
            else
            {
                byte[] buffer = null;
                if (result.resultList.Count >= 50) buffer = PS4Tool.ReadMemory(section.PID, section.Start, section.Length);
                List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)> resultNewList = new List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)>();

                for (int rIdx1 = 0; rIdx1 < result.resultList.Count; rIdx1++)
                {
                    if (ScanWorker.CancellationPending) break;

                    int gIdx = rIdx1 % groupTypes.Count;
                    if (gIdx == 0 && resultNewList.Count > 0)
                    {
                        resultList.AddRange(resultNewList);
                        resultNewList.Clear();
                    }

                    (ulong mappedAddr, _, _) = result.resultList[rIdx1];

                    if (section.Start + mappedAddr < AddrMin || section.Start + mappedAddr > AddrMax) continue;

                    (ScanType scanType, int groupTypeLength, bool isAny) = groupTypes[gIdx];
                    byte[] valueBytes = groupValues[gIdx];
                    byte[] newValue = new byte[groupTypeLength];
                    if (result.resultList.Count < 50) newValue = PS4Tool.ReadMemory(section.PID, mappedAddr + section.Start, groupTypeLength);
                    else Buffer.BlockCopy(buffer, (int)mappedAddr, newValue, 0, groupTypeLength);
                    if (isAny || ScanTool.Comparer(scanType, newValue, valueBytes)) resultNewList.Add((mappedAddr, 0, newValue));
                    else
                    {
                        resultNewList.Clear();
                        rIdx1 += groupTypes.Count - gIdx - 1;
                    }
                }
                if (resultNewList.Count > 0)
                {
                    resultList.AddRange(resultNewList);
                    resultNewList.Clear();
                }
                result.resultList.Clear();
                result.resultList = resultList;
            }

            return result;
        }

        private (ScanType scanType, List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)> resultList, List<(ScanType scanType, int groupTypeLength, bool isAny)> groupTypes) Comparer(
            Section section, ScanType scanType, CompareType compareType, int scanTypeLength, int step, byte[] value0Byte, ulong value0Long, ulong value1Long, ulong AddrMin, ulong AddrMax)
        {
            (ScanType scanType, List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)> resultList, List<(ScanType scanType, int groupTypeLength, bool isAny)> groupTypes) result;
            if (!resultListDict.TryGetValue(section.SID, out result))
            {
                result.scanType = scanType;
                result.resultList = new List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)>();
            }

            if (ResultView.Items.Count == 0)
            {
                byte[] buffer = PS4Tool.ReadMemory(section.PID, section.Start, section.Length);
                for (int scanIdx = 0; scanIdx + scanTypeLength < buffer.LongLength; scanIdx += step)
                {
                    if (ScanWorker.CancellationPending) break;
                    if (section.Start + (ulong)scanIdx < AddrMin || section.Start + (ulong)scanIdx > AddrMax) continue;
                    
                    byte[] newValue = new byte[scanTypeLength];
                    Buffer.BlockCopy(buffer, scanIdx, newValue, 0, scanTypeLength);
                    if (value0Byte == null)
                    {
                        ulong longValue = ScanTool.BytesToULong(scanType, ref newValue);
                        if (ScanTool.Comparer(scanType, compareType, longValue, 0, value0Long, value1Long)) result.resultList.Add(((ulong)scanIdx, longValue, null));
                    }
                    else if (ScanTool.Comparer(scanType, newValue, value0Byte)) result.resultList.Add(((ulong)scanIdx, 0, value0Byte));
                }
            }
            else
            {
                byte[] buffer = null;
                List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)> resultNewList = new List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)>();
                if (result.resultList.Count >= 50) buffer = PS4Tool.ReadMemory(section.PID, section.Start, section.Length);

                for (int rIdx1 = 0; rIdx1 < result.resultList.Count; rIdx1++)
                {
                    if (ScanWorker.CancellationPending) break;

                    (ulong mappedAddr, ulong resultValue, byte[] resultBytes) = result.resultList[rIdx1];

                    if (section.Start + mappedAddr < AddrMin || section.Start + mappedAddr > AddrMax) continue;

                    byte[] newValue = new byte[scanTypeLength];
                    if (result.resultList.Count < 50) newValue = PS4Tool.ReadMemory(section.PID, mappedAddr + section.Start, scanTypeLength);
                    else Buffer.BlockCopy(buffer, (int)mappedAddr, newValue, 0, scanTypeLength);

                    if (value0Byte == null)
                    {
                        ulong longValue = ScanTool.BytesToULong(scanType, ref newValue);
                        if (ScanTool.Comparer(scanType, compareType, longValue, resultValue, value0Long, value1Long)) resultNewList.Add((mappedAddr, longValue, null)); //hitCnt++;
                    }
                    else if (ScanTool.Comparer(scanType, newValue, value0Byte)) resultNewList.Add((mappedAddr, 0, value0Byte)); //hitCnt++;
                }
                result.resultList.Clear();
                result.resultList = resultNewList;
            }
            return result;
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
            
            if (resultListDict.Count > 0)
            {
                int count = 0;
                ResultView.Items.Clear();
                ResultView.BeginUpdate();

                List<int> keys = new List<int>(resultListDict.Keys);
                keys.Sort();
                for (int sectionIdx = 0; sectionIdx < keys.Count; sectionIdx++)
                {
                    Section section = sectionTool.SectionDict[keys[sectionIdx]];
                    if (!resultListDict.TryGetValue(section.SID, out (ScanType scanType, List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)> results, List<(ScanType scanType, int groupTypeLength, bool isAny)> groupTypes) result)) resultListDict.TryGetValue(section.SID, out result);

                    Color backColor = default;
                    for (int resultIdx = 0; resultIdx < result.results.Count; resultIdx++)
                    {
                        if (++count > 0x2000) break;

                        string valueStr, valueHex, typeStr;
                        (ulong mappedAddr, ulong resultValue, byte[] resultBytes) = result.results[resultIdx];
                        if (result.scanType == ScanType.Group)
                        {
                            int tIdx = resultIdx % result.groupTypes.Count;
                            if (tIdx == 0)
                            {
                                if (backColor != default) backColor = default;
                                else backColor = Color.DarkSlateGray;
                            }
                            (ScanType scanType, int groupTypeLength, bool isAny) = result.groupTypes[tIdx];
                            typeStr = scanType.GetDescription();
                            valueStr = ScanTool.BytesToString(scanType, resultBytes);
                            valueHex = ScanTool.BytesToString(scanType, resultBytes, true);
                        }
                        else if (resultBytes == null)
                        {
                            typeStr = result.scanType.GetDescription();
                            valueStr = ScanTool.ULongToString(result.scanType, resultValue);
                            valueHex = ScanTool.ULongToString(result.scanType, resultValue, true);
                        }
                        else
                        {
                            typeStr = result.scanType.GetDescription();
                            valueStr = ScanTool.BytesToString(result.scanType, resultBytes);
                            valueHex = ScanTool.BytesToString(result.scanType, resultBytes, true);
                        }
                        int itemIdx = ResultView.Items.Count;
                        ResultView.Items.Add(mappedAddr.ToString("X8"), (mappedAddr + section.Start).ToString("X8"), 0);
                        ResultView.Items[itemIdx].Tag = (section.SID, resultIdx);
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
            else
            {

            }
        }

        private void RefreshWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Diagnostics.Stopwatch tickerMajor = System.Diagnostics.Stopwatch.StartNew();
            (int pid, bool alignment, bool isFilter) = ((int pid, bool alignment, bool isFilter))e.Argument;
            if (resultListDict.Count == 0) return;

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

                if (!resultListDict.TryGetValue(section.SID, out (ScanType scanType, List<(ulong mappedAddr, ulong resultValue, byte[] resultBytes)> results, List<(ScanType scanType, int groupTypeLength, bool isAny)> groupTypes) result)) resultListDict.TryGetValue(section.SID, out result);

                ScanTool.ScanTypeLengthDict.TryGetValue(result.scanType, out int scanTypeLength);

                byte[] buffer = null;
                if (result.results.Count >= 50) buffer = PS4Tool.ReadMemory(section.PID, section.Start, section.Length);

                for (int resultIdx = 0; resultIdx < result.results.Count; resultIdx++)
                {
                    if (++hitCnt > 0x2000) continue;

                    string valueStr, valueHex;
                    (ulong mappedAddr, ulong resultValue, byte[] resultBytes) = result.results[resultIdx];
                    int newLen = scanTypeLength > 0 ? scanTypeLength : resultBytes.Length;
                    byte[] newValue = new byte[newLen];
                    if (result.results.Count < 50) newValue = PS4Tool.ReadMemory(pid, mappedAddr + section.Start, newLen);
                    else Buffer.BlockCopy(buffer, (int)mappedAddr, newValue, 0, newLen);

                    if (scanTypeLength > 0)
                    {
                        ulong longValue = ScanTool.BytesToULong(result.scanType, ref newValue);
                        result.results[resultIdx] = (mappedAddr, longValue, null);
                        valueStr = ScanTool.ULongToString(result.scanType, longValue);
                        valueHex = ScanTool.ULongToString(result.scanType, longValue, true);
                    }
                    else
                    {
                        result.results[resultIdx] = (mappedAddr, 0, newValue);
                        valueStr = ScanTool.BytesToString(result.scanType, newValue);
                        valueHex = ScanTool.BytesToString(result.scanType, newValue, true);
                    }

                    Invoke(new MethodInvoker(() => {
                        int itemIdx = ResultView.Items.Count;
                        ResultView.Items.Add(mappedAddr.ToString("X8"), (mappedAddr + section.Start).ToString("X8"), 0);
                        ResultView.Items[itemIdx].Tag = (section.SID, resultIdx);
                        ResultView.Items[itemIdx].SubItems.Add(result.scanType.GetDescription());
                        ResultView.Items[itemIdx].SubItems.Add(valueStr);
                        ResultView.Items[itemIdx].SubItems.Add(valueHex);
                        ResultView.Items[itemIdx].SubItems.Add(string.Format("{0}_{1}_{2}_{3}", section.Start.ToString("X"), section.Name, section.Prot.ToString("X"), section.Offset.ToString("X")));
                    }));
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
