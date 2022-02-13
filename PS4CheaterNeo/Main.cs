using libdebug;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PS4CheaterNeo.SectionTool;

namespace PS4CheaterNeo
{
    public partial class Main : Form
    {
        private Option option;
        private SendPayload sendPayload;
        public SectionTool sectionTool { get; private set; }
        public string ProcessName;
        public Main()
        {
            if ((Properties.Settings.Default.PS4IP.Value ?? "") == "") Properties.Settings.Default.Upgrade(); //Need to get the settings again when the AssemblyVersion is changed
            InitializeComponent();
            Text += " " + Application.ProductVersion; //Assembly.GetExecutingAssembly().GetName().Version.ToString(); // Assembly.GetEntryAssembly().GetName().Version.ToString();
            sectionTool = new SectionTool();
        }
        #region Event
        private void Main_Shown(object sender, EventArgs e)
        {
            ProcessName = "";
            bool isConnected = false;
            string PS4FWVersion = Properties.Settings.Default.PS4FWVersion.Value ?? "";
            string PS4IP = Properties.Settings.Default.PS4IP.Value ?? "";
            ushort PS4Port = Properties.Settings.Default.PS4Port.Value;
            if (PS4FWVersion != "" && PS4IP != "" && PS4Port != 0)
            {
                try {isConnected = PS4Tool.Connect(PS4IP, out string msg, 1000);}
                catch (Exception){}
            }

            if (!isConnected) ToolStripSend.PerformClick();

            CheatGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            CheatGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(80, 80, 80);
            CheatGridView.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            CheatGridView.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(153, 180, 209); //ActiveCaption
            CheatGridView.EnableHeadersVisualStyles = false;
        }

        #region ToolStrip
        private void ToolStripSend_Click(object sender, EventArgs e)
        {
            if (sendPayload == null || sendPayload.IsDisposed) sendPayload = new SendPayload();
            sendPayload.StartPosition = FormStartPosition.CenterParent;
            sendPayload.TopMost = true;
            sendPayload.Show();
        }

        private void ToolStripOpen_Click(object sender, EventArgs e)
        {
            try
            {
                OpenCheatDialog.Filter = "Cheat files (*.cht)|*.cht|Cheat Relative files (*.chtr)|*.chtr";
                OpenCheatDialog.AddExtension = true;
                OpenCheatDialog.RestoreDirectory = true;

                if (OpenCheatDialog.ShowDialog() != DialogResult.OK) return;

                string[] cheatStrArr = File.ReadAllLines(OpenCheatDialog.FileName);
                #region cheatHeaderItems Check
                string[] cheatHeaderItems = cheatStrArr[0].Split('|');

                ProcessName = cheatHeaderItems[1];
                if (!InitSectionList(ProcessName)) return;

                #region ParsePS4CheatFiles
                if (cheatStrArr[0].ToUpper().Contains("ID") && cheatStrArr[0].ToUpper().Contains("VER") && cheatStrArr[0].ToUpper().Contains("FM"))
                {
                    ParseCheatFiles(ref cheatStrArr);
                    cheatHeaderItems = cheatStrArr[0].Split('|');
                }
                #endregion

                if (Application.ProductVersion != cheatHeaderItems[0] && MessageBox.Show(string.Format("PS4 Cheater Version({0}) is different with cheat file({1}), still load?", Application.ProductVersion, cheatHeaderItems[0]),
                    "ProductVersion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                string cheatGameID = cheatHeaderItems[2];
                string cheatGameVer = cheatHeaderItems[3];
                string cheatFWVer = cheatHeaderItems[4];
                string PS4FWVersion = Properties.Settings.Default.PS4FWVersion.Value ?? "";
                string FMVer = PS4FWVersion != "" ? PS4FWVersion : Constant.Versions[0];

                ScanTool.GameInfo(FMVer, out string gameID, out string gameVer);

                if (gameID != cheatGameID && MessageBox.Show(string.Format("Your Game ID({0}) is different with cheat file({1}), still load?", gameID, cheatGameID),
                    "GameID", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                if (gameVer != cheatGameVer && MessageBox.Show(string.Format("Your Game version({0}) is different with cheat file({1}), still load?", gameVer, cheatGameVer),
                    "GameVer", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                if (FMVer != cheatFWVer && MessageBox.Show(string.Format("Your Firmware version({0}) is different with cheat file({1}), still load?", FMVer, cheatFWVer),
                    "FMVer", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                #endregion
                int count = 0;
                (int sid, string name, uint prot, ulong offsetAddr) preData = (0, "", 0, 0);
                for (int idx = 1; idx < cheatStrArr.Length; ++idx)
                {
                    string cheatStr = cheatStrArr[idx];

                    if (string.IsNullOrWhiteSpace(cheatStr)) continue;

                    string[] cheatElements = cheatStr.Split('|');

                    if (cheatElements.Length < 5) continue;

                    bool isRelative = cheatElements[5].StartsWith("+");
                    bool isPointer = cheatElements[5].Contains("_");
                    int sid = isRelative ? preData.sid : int.Parse(cheatElements[0]);
                    string name = isRelative ? preData.name : cheatElements[2];
                    uint prot = isRelative ? preData.prot : uint.Parse(cheatElements[3], NumberStyles.HexNumber);
                    ScanType cheatScanType = this.ParseFromDescription<ScanType>(cheatElements[6]);
                    bool cheatLock = bool.Parse(cheatElements[7]);
                    string cheatValue = cheatElements[8];
                    string cheatDesc = cheatElements[9];
                    int relativeOffset = isRelative ? int.Parse(cheatElements[5].Substring(1), NumberStyles.HexNumber) : -1;

                    Section section = sectionTool.GetSection(sid, name, prot);
                    if (section == null && ToolStripLockEnable.Checked) ToolStripLockEnable.Checked = false;

                    ulong offsetAddr = 0;
                    List<long> offsetList = null;
                    if (isPointer)
                    {
                        offsetList = new List<long>();
                        string[] offsetArr = cheatElements[5].Split('_');
                        for (int offsetIdx = 0; offsetIdx < offsetArr.Length; ++offsetIdx)
                        {
                            string offsetStr = offsetArr[offsetIdx];
                            long offset = long.Parse(offsetStr, NumberStyles.HexNumber);
                            if (offsetIdx == 0 && section != null) offset += (long)section.Start;
                            offsetList.Add(offset);
                        }
                    }
                    else offsetAddr = isRelative ? (ulong)relativeOffset + preData.offsetAddr :
                        ulong.Parse(cheatElements[5], NumberStyles.HexNumber);

                    DataGridViewRow cheatRow = AddToCheatGrid(section, offsetAddr, cheatScanType, cheatValue, cheatLock, cheatDesc, isPointer, offsetList, relativeOffset);
                    if (cheatRow == null)
                    {
                        preData = (0, "", 0, 0);
                        continue;
                    }
                    if (isPointer)
                    {
                        (section, offsetAddr) = ((Section section, ulong offsetAddr))cheatRow.Tag;
                        sid = section.SID;
                        name = section.Name;
                        prot = section.Prot;
                    }
                    preData = (sid, name, prot, offsetAddr);
                    count++;
                }
                CheatGridView.GroupRefresh();
                SaveCheatDialog.FileName = OpenCheatDialog.FileName;
                SaveCheatDialog.FilterIndex = OpenCheatDialog.FilterIndex;

                ToolStripMsg.Text = string.Format("Successfully loaded {0} cheat items.", count);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\n" + exception.StackTrace, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        /// <summary>
        /// PS4 Cheat Files Example:
        /// 1.5|eboot.bin|ID:CUSA99999|VER:09.99|FM:672
        /// simple pointer|pointer|float|@7777777_3_3333333+50+0+8+1B0+64|data|float|999|1|DescForPointer|
        /// data|2|ABCDE|4 bytes|999|0|DescForData|30ABCDE
        /// @batchcode|data|0|0|code||offset:0x7777777 value:0x0123456789ABCDEF reset:0x0123456789ABCDEF size:8;offset:0xAABBCC value:0x0123456789 reset:0x0123456789 size:5|0|DescForBatchcode
        /// </summary>
        /// <param name="cheatStrArr"></param>
        private void ParseCheatFiles(ref string[] cheatStrArr)
        {
            Section[] sections = sectionTool.GetSectionSortByAddr();
            cheatStrArr[0] = cheatStrArr[0].Replace("ID:", "").Replace("VER:", "").Replace("FM:", "");
            for (int topIdx = 1; topIdx < cheatStrArr.Length; ++topIdx)
            {
                string cheatStr = cheatStrArr[topIdx];

                if (string.IsNullOrWhiteSpace(cheatStr)) continue;

                string[] cheatElements = cheatStr.Split('|');

                if (cheatElements.Length < 5) continue;

                bool isBatchcode = cheatElements[0].Replace("@", "") == "batchcode";
                bool isPointer = cheatElements[0].Replace("@", "") == "simple pointer";
                int sequence;
                Section section;
                string sectionStr;
                string offsetAddrStr;
                string scanTypeStr;
                string cheatLockStr;
                string cheatValue;
                string cheatDesc;
                if (isBatchcode)
                {
                    if (cheatElements[1] != "data") continue;

                    sequence = int.Parse(cheatElements[2]);
                    offsetAddrStr = cheatElements[3];
                    section = sequence < sections.Length ? sections[sequence] : null;
                    string cheatCode = cheatElements[6];
                    cheatLockStr = cheatElements[7];
                    cheatDesc = cheatElements[8];
                    string[] datas = cheatCode.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                    sectionStr = string.Format("{0}|{1}|{2}|{3}|{4}",
                       section.SID,
                       section.Start.ToString("X"),
                       section.Name,
                       section.Prot.ToString("X"),
                       section.Offset.ToString("X"));
                    List<string> cheatStrs = new List<string>();
                    for (int idx = 0; idx < datas.Length; idx++)
                    {
                        string cstr = datas[idx];
                        if (string.IsNullOrEmpty(cstr)) continue;
                        (string offset, ScanType vtype, string value) batchCode = ParseBatchCode(cstr);
                        cheatStrs.Add(string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
                            sectionStr,
                            batchCode.offset,
                            batchCode.vtype.GetDescription(),
                            cheatLockStr == "1",
                            batchCode.value,
                            cheatDesc + (datas.Length > 1 ? "_" + idx : "")));
                    }
                    if (cheatStrs.Count == 0) continue;

                    string[] cheatStrNewArr = new string[cheatStrArr.Length + cheatStrs.Count - 1];
                    for (int nIdx = 0; nIdx < cheatStrNewArr.Length; nIdx++)
                    {
                        if (nIdx < topIdx) cheatStrNewArr[nIdx] = cheatStrArr[nIdx];
                        else if (nIdx >= topIdx && nIdx < topIdx + cheatStrs.Count) cheatStrNewArr[nIdx] = cheatStrs[nIdx - topIdx];
                        else cheatStrNewArr[nIdx] = cheatStrArr[nIdx - cheatStrs.Count + 1];
                    }
                    cheatStrArr = cheatStrNewArr;
                    topIdx += cheatStrs.Count - 1;
                    continue;
                }
                if (isPointer)
                {
                    string[] pointerList = cheatElements[3].Split('+');
                    string[] addressElements = pointerList[0].Split('_');
                    sequence = int.Parse(addressElements[1]);
                    offsetAddrStr = addressElements[2];
                    section = sequence < sections.Length ? sections[sequence] : null;
                    for (int offsetIdx = 1; offsetIdx < pointerList.Length; ++offsetIdx) offsetAddrStr += "_" + pointerList[offsetIdx];
                    scanTypeStr = cheatElements[5];
                    cheatValue = cheatElements[6];
                    cheatLockStr = cheatElements[7];
                    cheatDesc = cheatElements[8];
                }
                else
                {
                    sequence = int.Parse(cheatElements[1]);
                    offsetAddrStr = cheatElements[2];
                    section = sequence < sections.Length ? sections[sequence] : null;
                    scanTypeStr = cheatElements[3];
                    cheatValue = cheatElements[4];
                    cheatLockStr = cheatElements[5];
                    cheatDesc = cheatElements[6];
                }
                sectionStr = string.Format("{0}|{1}|{2}|{3}|{4}",
                   section.SID,
                   section.Start.ToString("X"),
                   section.Name,
                   section.Prot.ToString("X"),
                   section.Offset.ToString("X"));

                cheatStrArr[topIdx] = string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
                    sectionStr,
                    offsetAddrStr,
                    scanTypeStr.Replace("bytes", "Bytes").Replace("float", "Float").Replace("double", "Double").Replace("string", "String").Replace("hex", "Hex"),
                    cheatLockStr == "1",
                    cheatValue,
                    cheatDesc);
            }
        }

        private (string offset, ScanType vtype, string value) ParseBatchCode(string data)
        {
            int size = 0;
            (string offset, ScanType vtype, string value) code = ("0", ScanType.Byte_, "");
            string[] elements = data.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for (int idx = 0; idx < elements.Length; idx++)
            {
                string[] buff = elements[idx].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                (string key, string value) = (buff[0], buff[1]);

                if (key == "offset") code.offset = value.StartsWith("0x") ? value.Substring(2) : decimal.Parse(value).ToString("X");
                else if (key == "size") size = value.StartsWith("0x") ? (int)Convert.ToInt64(value, 16) : (int)decimal.Parse(value);
                else if (key == "value") code.value = value;
                else if (key == "vtype")
                {
                    if (value == "float") code.vtype = ScanType.Float_;
                    else if (value == "double") code.vtype = ScanType.Double_;
                    else if (value == "hex") code.vtype = ScanType.Hex;
                }
            }

            if (size < 1 && code.value.StartsWith("0x"))
            {
                string hexStr = code.value.Substring(2);
                size = (hexStr.Length + 1) / 2;
                size = size > 2 ? (size + 3) / 4 * 4 : size;
            }
            size = size < 1 ? 4 : size;

            if (ScanType.Byte_.Equals(code.vtype))
            {
                if (size == 1) code.vtype = ScanType.Byte_;
                else if (size == 2) code.vtype = ScanType.Bytes_2;
                else if (size == 4) code.vtype = ScanType.Bytes_4;
                else if (size == 8) code.vtype = ScanType.Bytes_8;
                else code.vtype = ScanType.Hex;
            }

            if (code.value.StartsWith("0x"))
            {
                code.value = code.value.Substring(2).PadLeft(size * 2, '0');
                if (code.vtype != ScanType.Float_ && code.vtype != ScanType.Double_ && code.vtype != ScanType.Hex)
                {
                    byte[] valueBytes = ScanTool.ValueStringToByte(ScanType.Hex, code.value);
                    code.value = ScanTool.BytesToString(code.vtype, valueBytes);
                }
            }

            return code;
        }

        private void ToolStripSave_Click(object sender, EventArgs e)
        {
            if (CheatGridView.Rows.Count == 0) return;

            SaveCheatDialog.Filter = "Cheat files (*.cht)|*.cht|Cheat Relative files (*.chtr)|*.chtr";
            SaveCheatDialog.AddExtension = true;
            SaveCheatDialog.RestoreDirectory = true;

            if (SaveCheatDialog.ShowDialog() != DialogResult.OK) return;
            if (!InitSectionList(ProcessName)) return;

            string FMVer = Constant.Versions[0];
            if (Properties.Settings.Default.PS4FWVersion.Value is string version && !string.IsNullOrWhiteSpace(version)) FMVer = version;
            ScanTool.GameInfo(FMVer, out string gameID, out string gameVer);
            string processName = ProcessName;
            string saveBuf = $"{Application.ProductVersion}|{processName}|{gameID}|{gameVer}|{FMVer}\n";

            ulong tmpAddr = 0;
            for (int cIdx = 0; cIdx < CheatGridView.Rows.Count; cIdx++)
            {
                DataGridViewRow cheatRow = CheatGridView.Rows[cIdx];

                (Section section, ulong offsetAddr) = ((Section section, ulong offsetAddr))cheatRow.Tag;
                bool isRelative = tmpAddr > 0 && offsetAddr > 0 && offsetAddr - tmpAddr < 50;
                bool isPointer = cheatRow.Cells[(int)ChertCol.CheatListAddress].Value.ToString().StartsWith("P->");
                Section newSection = section;
                string addressStr;
                string sectionStr = "";
                if (isPointer)
                {
                    string[] sectionArr = cheatRow.Cells[(int)ChertCol.CheatListSection].Value.ToString().Split('|');
                    newSection = sectionTool.GetSection(int.Parse(sectionArr[0]), sectionArr[2], uint.Parse(sectionArr[3]));
                    addressStr = sectionArr[sectionArr.Length - 1];
                }
                else if (SaveCheatDialog.FilterIndex == 2 && isRelative)
                {
                    addressStr = "+" + (offsetAddr - tmpAddr).ToString("X");
                    sectionStr = "||||";
                }
                else addressStr = offsetAddr.ToString("X");
                if (sectionStr == "") sectionStr = string.Format("{0}|{1}|{2}|{3}|{4}",
                    newSection.SID,
                    newSection.Start.ToString("X"),
                    newSection.Name,
                    newSection.Prot.ToString("X"),
                    newSection.Offset.ToString("X"));

                saveBuf += string.Format("{0}|{1}|{2}|{3}|{4}|{5}\n",
                    sectionStr,
                    addressStr,
                    cheatRow.Cells[(int)ChertCol.CheatListType].Value, //.ToString().Replace(" ", "_")
                    cheatRow.Cells[(int)ChertCol.CheatListLock].Value,
                    cheatRow.Cells[(int)ChertCol.CheatListValue].Value,
                    cheatRow.Cells[(int)ChertCol.CheatListDesc].Value
                    );
                tmpAddr = offsetAddr;
            }

            StreamWriter myStream = new StreamWriter(SaveCheatDialog.FileName);
            myStream.Write(saveBuf);
            myStream.Close();
            OpenCheatDialog.FileName = SaveCheatDialog.FileName;
            OpenCheatDialog.FilterIndex = SaveCheatDialog.FilterIndex;
        }

        private void ToolStripNewQuery_Click(object sender, EventArgs e)
        {
            Query query = new Query(this);
            query.Show();
        }

        private void ToolStripAdd_Click(object sender, EventArgs e)
        {
            try
            {
                NewAddress newAddress = new NewAddress(this, null, 0, ScanType.Bytes_4, null, false, "", false);
                if (newAddress.ShowDialog() != DialogResult.OK) return;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\n" + exception.StackTrace, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void ToolStripExpandAll_Click(object sender, EventArgs e) => CheatGridView.CollapseExpandAll(false);

        private void ToolStripCollapseAll_Click(object sender, EventArgs e) => CheatGridView.CollapseExpandAll(true);

        private void ToolStripSettings_Click(object sender, EventArgs e)
        {
            if (option == null || option.IsDisposed) option = new Option();
            option.StartPosition = FormStartPosition.CenterParent;
            option.Show();
        }

        private void ToolStripHexView_Click(object sender, EventArgs e)
        {
            try
            {
                Section section;
                ulong offsetAddr;

                string inputValue = "";

                if (CheatGridView.SelectedRows.Count == 1)
                {
                    DataGridViewSelectedRowCollection rows = CheatGridView.SelectedRows;
                    var cheatRow = rows[0];
                    (section, offsetAddr) = ((Section section, ulong offsetAddr))cheatRow.Tag;
                    inputValue = (section.Start + offsetAddr).ToString("X");
                }

                if (InputBox.Show("Hex View", "Please enter the memory address(hex) you want to view", ref inputValue) != DialogResult.OK) return;

                if ((ProcessName ?? "") == "") throw new Exception("No Process currently");
                if (!InitSectionList(ProcessName)) throw new Exception(String.Format("Process({0}): InitSectionList failed", ProcessName));

                ulong address = ulong.Parse(inputValue, NumberStyles.HexNumber);
                int sid = sectionTool.GetSectionID(address);
                if (sid == -1) return;

                section = sectionTool.GetSection(sid);
                offsetAddr = address - section.Start;

                HexEditor hexEdit = new HexEditor(this, section, (int)offsetAddr);
                hexEdit.Show(this);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\n" + exception.StackTrace, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void ToolStripRefreshCheat_Click(object sender, EventArgs e)
        {
            if (CheatGridView.Rows.Count == 0 || (refreshCheatTask != null && !refreshCheatTask.IsCompleted)) return;

            refreshCheatTask = RefreshCheatTask();
            ToolStripMsg.Text = string.Format("{0:000}%, Refresh Cheat finished.", 100);
        }
        #endregion

        #region Task
        Task<bool> refreshCheatTask;
        private async Task<bool> RefreshCheatTask() => await Task.Run(() => {
            if (!InitSectionList(ProcessName, true)) return false;

            bool VerifySectionWhenRefresh = Properties.Settings.Default.VerifySectionWhenRefresh.Value;
            System.Diagnostics.Stopwatch tickerMajor = System.Diagnostics.Stopwatch.StartNew();
            (int sid, string name, uint prot, ulong offsetAddr) preData = (0, "", 0, 0);
            Dictionary<ulong, ulong> pointerMemoryCaches = new Dictionary<ulong, ulong>();
            for (int cIdx = 0; cIdx < CheatGridView.Rows.Count; cIdx++)
            {
                try
                {
                    string msg = string.Format("{0}/{1}", cIdx + 1, CheatGridView.Rows.Count);
                    ToolStripMsg.Text = string.Format("{0:000}%, Refresh Cheat elapsed:{1}s. {2}", (int)(((float)(cIdx + 1) / CheatGridView.Rows.Count) * 100), tickerMajor.Elapsed.TotalSeconds, msg);

                    DataGridViewRow cheatRow = CheatGridView.Rows[cIdx];
                    (Section section, ulong offsetAddr) = ((Section section, ulong offsetAddr))cheatRow.Tag;
                    #region  Refresh Section and offsetAddr
                    if (VerifySectionWhenRefresh)
                    {
                        string[] sectionArr = cheatRow.Cells[(int)ChertCol.CheatListSection].Value.ToString().Split('|');
                        string hexAddr = "";
                        bool isPointer = cheatRow.Cells[(int)ChertCol.CheatListAddress].Value.ToString().StartsWith("P->");
                        if (sectionArr[0].StartsWith("+") && preData.sid == 0) section = null;
                        else (section, offsetAddr, hexAddr) = RefreshSection(sectionArr, offsetAddr, isPointer, preData, pointerMemoryCaches);
                        if (section == null)
                        {
                            ToolStripMsg.Text = string.Format("RefreshCheat Failed...CheatGrid({0}), section: {1}, offsetAddr: {2:X}", cIdx, string.Join("-", sectionArr), offsetAddr);
                            if (ToolStripLockEnable.Checked) Invoke(new MethodInvoker(() => { ToolStripLockEnable.Checked = false; }));
                            cheatRow.DefaultCellStyle.ForeColor = Color.Red;
                            preData = (0, "", 0, 0);
                            continue;
                        }
                        else if (cheatRow.DefaultCellStyle.ForeColor == Color.Red) cheatRow.DefaultCellStyle.ForeColor = default;
                        preData = (section.SID, section.Name, section.Prot, offsetAddr);
                        cheatRow.Cells[(int)ChertCol.CheatListAddress].Value = hexAddr;
                    }
                    #endregion
                    ScanType scanType = this.ParseFromDescription<ScanType>(cheatRow.Cells[(int)ChertCol.CheatListType].Value.ToString());
                    int scanTypeLength = 0;
                    if (scanType != ScanType.Hex && scanType != ScanType.String_) ScanTool.ScanTypeLengthDict.TryGetValue(scanType, out scanTypeLength);
                    else scanTypeLength = ScanTool.ValueStringToByte(scanType, cheatRow.Cells[(int)ChertCol.CheatListValue].Value.ToString()).Length;
                    byte[] newData = PS4Tool.ReadMemory(section.PID, offsetAddr + section.Start, scanTypeLength);
                    cheatRow.Tag = (section, offsetAddr);
                    cheatRow.Cells[(int)ChertCol.CheatListValue].Value = ScanTool.BytesToString(scanType, newData);
                }
                catch { preData = (0, "", 0, 0); }
            }
            return true;
        });

        private (Section section, ulong offsetAddr, string hexAddr) RefreshSection(string[] sectionArr, ulong offsetAddr, bool isPointer, (int sid, string name, uint prot, ulong offsetAddr) preData, in Dictionary<ulong, ulong> pointerMemoryCaches)
        {
            bool isRelative = sectionArr[0].StartsWith("+");
            int relativeOffset = isRelative ? int.Parse(sectionArr[0].Substring(1), NumberStyles.HexNumber) : -1;

            int sid = isRelative ? preData.sid : int.Parse(sectionArr[0]);
            string name = isRelative ? preData.name : sectionArr[2];
            uint prot = isRelative ? preData.prot : uint.Parse(sectionArr[3], NumberStyles.HexNumber);
            string hexAddr;

            Section section;
            Section refreshSection = sectionTool.GetSection(sid, name, prot);
            if (refreshSection == null) return (null, offsetAddr, "");
            if (isPointer)
            {
                string[] offsetArr = sectionArr[sectionArr.Length - 1].Split('_');
                long[] offsets = new long[offsetArr.Length];
                for (int idx = 0; idx < offsetArr.Length; ++idx)
                {
                    long offset = long.Parse(offsetArr[idx], NumberStyles.HexNumber);
                    if (idx == 0) offset += (long)refreshSection.Start;
                    offsets[idx] = offset;
                }
                ulong tailAddr = PS4Tool.ReadTailAddress(refreshSection.PID, offsets, pointerMemoryCaches);

                section = sectionTool.GetSection(sectionTool.GetSectionID(tailAddr));
                if (section == null) return (null, offsetAddr, "");

                offsetAddr = tailAddr - section.Start;
                hexAddr = "P->" + tailAddr.ToString("X8");
            }
            else
            {
                section = refreshSection;
                offsetAddr = isRelative ? (ulong)relativeOffset + preData.offsetAddr : offsetAddr;
                hexAddr = (offsetAddr + section.Start).ToString("X8");
            }

            return (section, offsetAddr, hexAddr);
        }

        Task<bool> refreshLockTask;
        private void RefreshLock_Tick(object sender, EventArgs e)
        {
            if (!(ToolStripProcessInfo.Tag is bool) && ProcessName != (string)ToolStripProcessInfo.Tag)
            {
                ToolStripProcessInfo.Tag = ProcessName;
                ToolStripProcessInfo.Text = "Current Processes: " + (ProcessName == "" ? "Empty" : ProcessName);
            }
            if (!ToolStripLockEnable.Checked || CheatGridView.Rows.Count == 0 || (refreshLockTask != null && !refreshLockTask.IsCompleted)) return;

            if (refreshLockTask != null) refreshLockTask.Dispose();

            refreshLockTask = RefreshLockTask();
        }

        private async Task<bool> RefreshLockTask() => await Task.Run(() => {
            if (DateTime.Now.Second % 5 == 0 && DateTime.Now.Millisecond < 500)
            {
                if (!InitSectionList(ProcessName)) return false;
            }


            bool VerifySectionWhenLock = Properties.Settings.Default.VerifySectionWhenLock.Value;
            (int sid, string name, uint prot, ulong offsetAddr) preData = (0, "", 0, 0);
            Dictionary<ulong, ulong> pointerMemoryCaches = new Dictionary<ulong, ulong>();
            for (int cIdx = 0; cIdx < CheatGridView.Rows.Count; cIdx++)
            {
                try
                {
                    DataGridViewRow cheatRow = CheatGridView.Rows[cIdx];
                    if ((bool)cheatRow.Cells[(int)ChertCol.CheatListLock].Value == false)
                    {
                        preData = (0, "", 0, 0);
                        continue;
                    }
                    (Section section, ulong offsetAddr) = ((Section section, ulong offsetAddr))cheatRow.Tag;
                    #region Refresh Section and offsetAddr
                    if (VerifySectionWhenLock)
                    {
                        int checkOffset = 0;
                        string hexAddr;
                        bool isPointer = cheatRow.Cells[(int)ChertCol.CheatListAddress].Value.ToString().StartsWith("P->");
                        string[] sectionArr = cheatRow.Cells[(int)ChertCol.CheatListSection].Value.ToString().Split('|');
                        if (sectionArr[0].StartsWith("+") && preData.sid == 0)
                        {
                            for (int idx = cIdx - 1; idx >= 0; idx--)
                            {
                                DataGridViewRow checkRow = CheatGridView.Rows[idx];
                                string[] checkSArr = checkRow.Cells[(int)ChertCol.CheatListSection].Value.ToString().Split('|');
                                if (checkSArr[0].StartsWith("+"))
                                {
                                    checkOffset += int.Parse(sectionArr[0].Substring(1), NumberStyles.HexNumber);
                                    continue;
                                }

                                (Section section, ulong offsetAddr) checkCheat = ((Section section, ulong offsetAddr))checkRow.Tag;
                                bool checkPointer = checkRow.Cells[(int)ChertCol.CheatListAddress].Value.ToString().StartsWith("P->");
                                (Section section, ulong offsetAddr, string hexAddr) checkSection = RefreshSection(checkSArr, checkCheat.offsetAddr, checkPointer, preData, pointerMemoryCaches);
                                preData = (checkSection.section.SID, checkSection.section.Name, checkSection.section.Prot, checkSection.offsetAddr + (ulong)checkOffset);
                                break;
                            }
                        }
                        (section, offsetAddr, hexAddr) = RefreshSection(sectionArr, offsetAddr, isPointer, preData, pointerMemoryCaches);
                        if (section == null)
                        {
                            ToolStripMsg.Text = string.Format("RefreshLock Failed...CheatGrid({0}), section: {1}, offsetAddr: {2:X}", cIdx, string.Join("-", sectionArr), offsetAddr);
                            cheatRow.DefaultCellStyle.ForeColor = Color.Red;
                            preData = (0, "", 0, 0);
                            continue;
                        }
                        else if (cheatRow.DefaultCellStyle.ForeColor == Color.Red) cheatRow.DefaultCellStyle.ForeColor = default;
                        preData = (section.SID, section.Name, section.Prot, offsetAddr);
                        cheatRow.Tag = (section, offsetAddr);
                        cheatRow.Cells[(int)ChertCol.CheatListAddress].Value = hexAddr;
                    }
                    #endregion
                    ScanType scanType = this.ParseFromDescription<ScanType>(cheatRow.Cells[(int)ChertCol.CheatListType].Value.ToString());
                    byte[] newData = ScanTool.ValueStringToByte(scanType, cheatRow.Cells[(int)ChertCol.CheatListValue].Value.ToString());
                    PS4Tool.WriteMemory(section.PID, offsetAddr + section.Start, newData);
                }
                catch { preData = (0, "", 0, 0); }
            }
            return true;
        });
        #endregion

        #region CheatGridView
        private void CheatGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var format = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
            var bounds = new Rectangle(e.RowBounds.X + 16, e.RowBounds.Top, 16, e.RowBounds.Height);
            e.Graphics.DrawString(e.RowIndex.ToString(), Font, SystemBrushes.HighlightText, bounds, format);
        }

        private void CheatGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            //if (e.RowIndex >= 0) cheatList.RemoveAt(e.RowIndex);
        }

        private void CheatGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            DataGridViewRow cheatRow = CheatGridView.Rows[e.RowIndex];
            try
            {
                switch (e.ColumnIndex)
                {
                    case (int)ChertCol.CheatListEnabled:
                        CheatGridView.EndEdit();
                        (Section section, ulong offsetAddr) = ((Section section, ulong offsetAddr))cheatRow.Tag;
                        ScanType scanType = this.ParseFromDescription<ScanType>(cheatRow.Cells[(int)ChertCol.CheatListType].Value.ToString());
                        byte[] data = ScanTool.ValueStringToByte(scanType, cheatRow.Cells[(int)ChertCol.CheatListValue].Value.ToString());
                        PS4Tool.WriteMemory(section.PID, offsetAddr + section.Start, data);
                        break;
                    case (int)ChertCol.CheatListDel:
                        CheatGridView.Rows.RemoveAt(e.RowIndex);
                        break;
                    case (int)ChertCol.CheatListLock:
                        CheatGridView.EndEdit();
                        break;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\n" + exception.StackTrace, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void CheatGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridViewRow editedRow = CheatGridView.Rows[e.RowIndex];
            if (e.ColumnIndex != (int)ChertCol.CheatListValue) return;
            try
            {
                ScanType scanType = this.ParseFromDescription<ScanType>(editedRow.Cells[(int)ChertCol.CheatListType].Value.ToString());
                ScanTool.ValueStringToULong(scanType, (string)e.FormattedValue);
            }
            catch (Exception exception)
            {
                e.Cancel = true;
                MessageBox.Show(exception.Message + "\n" + exception.StackTrace, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void CheatGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow editedRow = CheatGridView.Rows[e.RowIndex];
            if (e.ColumnIndex != (int)ChertCol.CheatListValue) return;
            try
            {
                (Section section, ulong offsetAddr) = ((Section section, ulong offsetAddr))editedRow.Tag;
                ScanType scanType = this.ParseFromDescription<ScanType>(editedRow.Cells[(int)ChertCol.CheatListType].Value.ToString());
                byte[] data = ScanTool.ValueStringToByte(scanType, editedRow.Cells[(int)ChertCol.CheatListValue].Value.ToString());
                PS4Tool.WriteMemory(section.PID, offsetAddr + section.Start, data);
                editedRow.Tag = (section, offsetAddr);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\n" + exception.StackTrace, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        #endregion

        #endregion

        #region CheatGridMenu
        private void CheatGridMenuHexEditor_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheatGridView.SelectedRows == null || CheatGridView.SelectedRows.Count == 0) return;
                if (CheatGridView.SelectedRows.Count != 1) return;

                DataGridViewSelectedRowCollection rows = CheatGridView.SelectedRows;

                var cheatRow = rows[0];
                (Section section, ulong offsetAddr) = ((Section section, ulong offsetAddr))cheatRow.Tag;

                HexEditor hexEdit = new HexEditor(this, section, (int)offsetAddr);
                hexEdit.Show(this);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\n" + exception.StackTrace, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void CheatGridMenuLock_Click(object sender, EventArgs e)
        {
            if (CheatGridView.SelectedRows == null || CheatGridView.SelectedRows.Count == 0) return;

            DataGridViewSelectedRowCollection rows = CheatGridView.SelectedRows;
            for (int i = 0; i < rows.Count; ++i) rows[i].Cells[(int)ChertCol.CheatListLock].Value = true;
        }

        private void CheatGridMenuUnlock_Click(object sender, EventArgs e)
        {
            if (CheatGridView.SelectedRows == null || CheatGridView.SelectedRows.Count == 0) return;

            DataGridViewSelectedRowCollection rows = CheatGridView.SelectedRows;
            for (int i = 0; i < rows.Count; ++i) rows[i].Cells[(int)ChertCol.CheatListLock].Value = false;
        }

        private void CheatGridMenuActive_Click(object sender, EventArgs e)
        {
            if (CheatGridView.SelectedRows == null || CheatGridView.SelectedRows.Count == 0) return;

            DataGridViewSelectedRowCollection rows = CheatGridView.SelectedRows;

            for (int i = 0; i < rows.Count; ++i)
            {
                var cheatRow = rows[i];
                (Section section, ulong offsetAddr) = ((Section section, ulong offsetAddr))cheatRow.Tag;
                ScanType scanType = this.ParseFromDescription<ScanType>(cheatRow.Cells[(int)ChertCol.CheatListType].Value.ToString());
                byte[] data = ScanTool.ValueStringToByte(scanType, cheatRow.Cells[(int)ChertCol.CheatListValue].Value.ToString());
                PS4Tool.WriteMemory(section.PID, offsetAddr + section.Start, data);
            }
        }

        private void CheatGridMenuEdit_Click(object sender, EventArgs e)
        {
            if (CheatGridView.SelectedRows == null || CheatGridView.SelectedRows.Count == 0) return;

            try
            {
                DataGridViewRow cheatRow = null;
                if (CheatGridView.SelectedRows.Count > 1)
                {
                    string inputValue = "";
                    if (InputBox.Show("Multiple Addresses Edit", "Please enter a value and write to multiple addresses", ref inputValue) != DialogResult.OK) return;
                    DataGridViewSelectedRowCollection rows = CheatGridView.SelectedRows;

                    for (int i = 0; i < rows.Count; ++i)
                    {
                        cheatRow = rows[i];
                        (Section section, ulong offsetAddr) row = ((Section section, ulong offsetAddr))cheatRow.Tag;
                        ScanType rowScanType = this.ParseFromDescription<ScanType>(cheatRow.Cells[(int)ChertCol.CheatListType].Value.ToString());
                        byte[] rowData = ScanTool.ValueStringToByte(rowScanType, inputValue);
                        PS4Tool.WriteMemory(row.section.PID, row.offsetAddr + row.section.Start, rowData);
                        cheatRow.Cells[(int)ChertCol.CheatListValue].Value = ScanTool.BytesToString(rowScanType, rowData);
                    }
                    return;
                }

                cheatRow = CheatGridView.SelectedRows[0];
                ScanType scanType = this.ParseFromDescription<ScanType>(cheatRow.Cells[(int)ChertCol.CheatListType].Value.ToString());
                string oldValue = cheatRow.Cells[(int)ChertCol.CheatListValue].Value.ToString();
                (Section section, ulong offsetAddr) = ((Section section, ulong offsetAddr))cheatRow.Tag;

                NewAddress newAddress = null;
                bool isPointer = cheatRow.Cells[(int)ChertCol.CheatListAddress].Value.ToString().StartsWith("P->");
                List<long> offsetList = null;
                if (isPointer)
                {
                    offsetList = new List<long>();
                    string[] baseSectionArr = cheatRow.Cells[(int)ChertCol.CheatListSection].Value.ToString().Split('|');
                    int baseSID = int.Parse(baseSectionArr[0]);
                    long baseSectionStart = long.Parse(baseSectionArr[1], NumberStyles.HexNumber);
                    string baseName = baseSectionArr[2];
                    uint baseProt = uint.Parse(baseSectionArr[3], NumberStyles.HexNumber);

                    Section baseSection = sectionTool.GetSection(baseSID, baseName, baseProt);
                    string[] offsetArr = baseSectionArr[baseSectionArr.Length - 1].Split('_');
                    for (int idx = 0; idx < offsetArr.Length; ++idx)
                    {
                        long offset = long.Parse(offsetArr[idx], NumberStyles.HexNumber);
                        if (idx == 0) offset += (long)baseSection.Start;
                        offsetList.Add(offset);
                    }
                    newAddress = new NewAddress(this, section, baseSection, offsetAddr + section.Start, scanType, oldValue, (bool)cheatRow.Cells[(int)ChertCol.CheatListLock].Value, (string)cheatRow.Cells[(int)ChertCol.CheatListDesc].Value, offsetList, true);
                }
                else newAddress = new NewAddress(this, section, offsetAddr + section.Start, scanType, oldValue, (bool)cheatRow.Cells[(int)ChertCol.CheatListLock].Value, (string)cheatRow.Cells[(int)ChertCol.CheatListDesc].Value, true);

                if (newAddress.ShowDialog() != DialogResult.OK) return;

                cheatRow.Tag = (newAddress.AddrSection, newAddress.Address - newAddress.AddrSection.Start);
                cheatRow.Cells[(int)ChertCol.CheatListAddress].Value = (newAddress.Address).ToString("X8");
                cheatRow.Cells[(int)ChertCol.CheatListType].Value = newAddress.CheatType.GetDescription();
                cheatRow.Cells[(int)ChertCol.CheatListLock].Value = newAddress.IsLock;
                cheatRow.Cells[(int)ChertCol.CheatListDesc].Value = newAddress.Descriptioin;
                if (newAddress.IsPointer)
                {
                    cheatRow.Cells[(int)ChertCol.CheatListAddress].Value = "P->" + cheatRow.Cells[(int)ChertCol.CheatListAddress].Value;
                    string offsetStr = "|";
                    for (int idx = 0; idx < newAddress.OffsetList.Count; idx++)
                    {
                        long offset = newAddress.OffsetList[idx];
                        if (offsetStr == "|") offsetStr += (offset - (long)newAddress.BaseSection.Start).ToString("X");
                        else offsetStr += "_" + offset.ToString("X");
                    }
                    cheatRow.Cells[(int)ChertCol.CheatListSection].Value = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", newAddress.BaseSection.SID, newAddress.BaseSection.Start.ToString("X"), newAddress.BaseSection.Name, newAddress.BaseSection.Prot.ToString("X"), newAddress.BaseSection.Offset.ToString("X"), offsetStr);
                }
                else cheatRow.Cells[(int)ChertCol.CheatListSection].Value = string.Format("{0}|{1}|{2}|{3}|{4}", newAddress.AddrSection.SID, newAddress.AddrSection.Start.ToString("X"), newAddress.AddrSection.Name, newAddress.AddrSection.Prot.ToString("X"), newAddress.AddrSection.Offset.ToString("X"));

                byte[] data = ScanTool.ValueStringToByte(scanType, newAddress.Value);
                PS4Tool.WriteMemory(section.PID, offsetAddr + section.Start, data);
                cheatRow.Cells[(int)ChertCol.CheatListValue].Value = newAddress.Value;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\n" + exception.StackTrace, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void CheatGridMenuCopyAddress_Click(object sender, EventArgs e)
        {
            if (CheatGridView.SelectedRows == null || CheatGridView.SelectedRows.Count == 0) return;

            string clipStr = "";
            DataGridViewSelectedRowCollection rows = CheatGridView.SelectedRows;
            for (int i = 0; i < rows.Count; ++i)
            {
                var cheatRow = rows[i];
                (Section section, ulong offsetAddr) = ((Section section, ulong offsetAddr))cheatRow.Tag;
                if (clipStr.Length > 0) clipStr += " \n";
                clipStr += (offsetAddr + section.Start).ToString("X");
            }
            if (clipStr.Length > 0) Clipboard.SetText(clipStr);
        }

        private void CheatGridMenuFindPointer_Click(object sender, EventArgs e)
        {
            if (CheatGridView.SelectedRows == null || CheatGridView.SelectedRows.Count == 0) return;

            if (CheatGridView.SelectedRows.Count != 1) return;

            DataGridViewSelectedRowCollection rows = CheatGridView.SelectedRows;

            try
            {
                var cheatRow = rows[0];
                (Section section, ulong offsetAddr) = ((Section section, ulong offsetAddr))cheatRow.Tag;
                ScanType scanType = this.ParseFromDescription<ScanType>(cheatRow.Cells[(int)ChertCol.CheatListType].Value.ToString());

                PointerFinder pointerFinder = new PointerFinder(this, offsetAddr + section.Start, scanType);
                pointerFinder.Show();
            }
            catch
            {

            }
        }

        private void CheatGridMenuDelete_Click(object sender, EventArgs e)
        {
            if (CheatGridView.SelectedRows == null || CheatGridView.SelectedRows.Count == 0) return;

            DataGridViewSelectedRowCollection rows = CheatGridView.SelectedRows;
            for (int i = 0; i < rows.Count; ++i) CheatGridView.Rows.Remove(rows[i]);
        }

        private void ToolStripLockEnable_Click(object sender, EventArgs e)
        {
            ToolStripLockEnable.Checked = !ToolStripLockEnable.Checked;
        }

        private void ToolStripLockEnable_CheckedChanged(object sender, EventArgs e)
        {
            byte[] checkBoxBase64;
            if (ToolStripLockEnable.Checked) checkBoxBase64 = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAvElEQVR4Xr3ToQoCMRzH8Z8ytFy66DOIPocyBc/iExhE8FUMYhCLGBTBM7hX0KbJqGDUYLqwOPmHgzGO222Cn7zvj4WtpJSClFLBE0sDzjlcCSHAoIn3BxQV9XsgZfj788Djc0d32cL5eYKOFY0n8RhhNUSj1rTf4J28MuP5YIGgEuTfYHfdYH1ZYRrNQDLi/IF2vQNxO1IIYsT2ATpIwWg7BNFi+4A5QtLYZUAL7Zj5PF0x/WP4oO+MX3wBcd48+jpQHgAAAAAASUVORK5CYII=");
            else checkBoxBase64 = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAARUlEQVR4Xu3SoRGAMBAF0Q1z5Z0J1AmYX9tZGGYQWL6KyCtg1baquvARAJmJQxLB69hP/li3zmPBN1BgBmYgPm/7AUm4bkJfDRuTAXI3AAAAAElFTkSuQmCC");
            using (MemoryStream ms = new MemoryStream(checkBoxBase64, 0, checkBoxBase64.Length)) ToolStripLockEnable.Image = Image.FromStream(ms, true);
        }
        #endregion

        #region Public Method
        /// <summary>
        /// Add the destination address to the CheatGrid
        /// </summary>
        /// <param name="section">section of the destination address</param>
        /// <param name="offsetAddr">offset address of the destination section</param>
        /// <param name="scanType">value type of the destination address</param>
        /// <param name="oldValue">value of the destination address</param>
        /// <param name="cheatLock">Whether to lock cheat</param>
        /// <param name="cheatDesc">description of cheat</param>
        /// <param name="isPointer">is it Pointer</param>
        /// <param name="offsetList">offsets of Pointer</param>
        /// <param name="relativeOffset">store address as a relative offset</param>
        /// <returns>returns the cheatRow added this time, which can be used to change the row style</returns>
        public DataGridViewRow AddToCheatGrid(Section section, ulong offsetAddr, ScanType scanType, string oldValue, bool cheatLock, string cheatDesc, bool isPointer, List<long> offsetList, int relativeOffset=-1)
        {
            DataGridViewRow cheatRow = null;
            try
            {
                bool isFailed = false;
                (Section section, ulong offsetAddr) pointerTag = (new Section(), 0);
                if (isPointer)
                {
                    var baseSection = section;
                    long baseAddress = 0;
                    for (int i = 0; i < offsetList.Count; ++i)
                    {
                        long address = offsetList[i];
                        if (i != offsetList.Count - 1)
                        {
                            byte[] nextAddress = PS4Tool.ReadMemory(section.PID, (ulong)(address + baseAddress), 8);
                            baseAddress = BitConverter.ToInt64(nextAddress, 0);
                            if (baseAddress == 0)
                            {
                                isFailed = true;
                                ToolStripMsg.Text = string.Format("Add Pointer To CheatGrid failed...NextAddress is zero, offsets:{0}, Desc:{1}", String.Join("-", offsetList.Select(p => p.ToString("X")).ToArray()), cheatDesc);
                                break;
                            }
                        }
                        else
                        {
                            int SID = sectionTool.GetSectionID((ulong)(address + baseAddress));
                            if (SID == -1)
                            {
                                isFailed = true;
                                ToolStripMsg.Text = string.Format("Add Pointer To CheatGrid failed...Section not found, address:{0:X8}, offsets:{1}, Desc:{2}", address + baseAddress, String.Join("-", offsetList.Select(p => p.ToString("X")).ToArray()), cheatDesc);
                                break;
                            }
                            pointerTag.section = sectionTool.GetSection(SID);
                            pointerTag.offsetAddr = (ulong)(address + baseAddress) - pointerTag.section.Start;
                        }
                    }
                }
                int cheatIdx = CheatGridView.Rows.Add();
                cheatRow = CheatGridView.Rows[cheatIdx];
                if (isFailed) cheatRow.DefaultCellStyle.ForeColor = Color.Red;

                if (relativeOffset > -1) cheatRow.Cells[(int)ChertCol.CheatListSection].Value = "+" + relativeOffset.ToString("X");
                else cheatRow.Cells[(int)ChertCol.CheatListSection].Value = string.Format("{0}|{1}|{2}|{3}|{4}", section.SID, section.Start.ToString("X"), section.Name, section.Prot.ToString("X"), section.Offset.ToString("X"));
                
                if (isPointer)
                {
                    cheatRow.Tag = pointerTag;
                    cheatRow.Cells[(int)ChertCol.CheatListAddress].Value = "P->" + (pointerTag.offsetAddr + pointerTag.section.Start).ToString("X");
                    string offsetStr = "|";
                    for (int idx = 0; idx < offsetList.Count; idx++)
                    {
                        long offset = offsetList[idx];
                        if (offsetStr == "|") offsetStr += (offset - (long)section.Start).ToString("X");
                        else offsetStr += "_" + offset.ToString("X");
                    }
                    cheatRow.Cells[(int)ChertCol.CheatListSection].Value += offsetStr;
                    section = pointerTag.section;
                }
                else
                {
                    cheatRow.Tag = (section, offsetAddr);
                    cheatRow.Cells[(int)ChertCol.CheatListAddress].Value = (offsetAddr + (section != null ? section.Start : 0)).ToString("X8");
                }
                cheatRow.Cells[(int)ChertCol.CheatListType].Value = scanType.GetDescription();
                cheatRow.Cells[(int)ChertCol.CheatListValue].Value = oldValue;
                cheatRow.Cells[(int)ChertCol.CheatListLock].Value = cheatLock;
                cheatRow.Cells[(int)ChertCol.CheatListDesc].Value = cheatDesc;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\n" + exception.StackTrace, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return cheatRow;
        }

        /// <summary>
        /// Initialize sections that match the selection process.
        /// 1. Show process not found in main window when pid is zero and set Tag to false.
        /// 2. The Tag has been set to false in the previous detection, and when the process can be found, the Tag will be cleared.
        /// 3. Confirm whether the Section dictionary is correct, when the detected Section count is less than 20.
        /// 4. If not the above and it has been initialized before, it will not be re-initialized.
        /// </summary>
        /// <param name="processName">initialize Sections according to the passed process name</param>
        /// <param name="force">force initialization even if Section dictionary already has data</param>
        /// <returns>return whether the initialization was successful</returns>
        public bool InitSectionList(string processName, bool force = false)
        {
            ProcessInfo processInfo = PS4Tool.GetProcessInfo(processName);

            if (processInfo.pid == 0 || processInfo.name != processName)
            {
                ToolStripProcessInfo.Text = string.Format("ProcessInfo: Cheat file Process({0}) could not find.", processName);
                ToolStripProcessInfo.Tag = false;
                return false;
            }
            else if (ToolStripProcessInfo.Tag is bool) ToolStripProcessInfo.Tag = null; //當已查詢到ProcessInfo時，清除失敗的Tag
            else if (!force && processInfo.pid > 0 && processInfo.pid == sectionTool.PID && sectionTool.SectionDict != null)
            {
                ProcessMap pMap = null;
                if (sectionTool.SectionDict.Count < 20) pMap = PS4Tool.GetProcessMaps(processInfo.pid);
                if (pMap == null || sectionTool.SectionDict.Count >= pMap.entries.Length) return true; //當Process ID相同時，不再執行初始化
            }

            ProcessName = processInfo.name;
            sectionTool.InitSectionList(processInfo.pid, ProcessName);

            return true;
        }
        #endregion
    }
}
