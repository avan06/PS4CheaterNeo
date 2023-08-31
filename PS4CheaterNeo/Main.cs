using GroupGridView;
using libdebug;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PS4CheaterNeo.SectionTool;

namespace PS4CheaterNeo
{
    public partial class Main : Form
    {
        private Option option;
        private SendPayload sendPayload;
        private CheatJson cheatJson = null;
        private Brush cheatGridViewRowIndexForeBrush;
        public SectionTool sectionTool { get; private set; }
        public string ProcessName;
        public int ProcessPid;
        public Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; //Avoid the case where CurrentCulture.NumberFormatInfo.NumberDecimalSeparator is not "."
            if ((Properties.Settings.Default.PS4IP.Value ?? "") == "") Properties.Settings.Default.Upgrade(); //Need to get the settings again when the AssemblyVersion is changed
            InitializeComponent();
            ApplyUI();
            ToolStripLockEnable.Checked = Properties.Settings.Default.CheatLock.Value;
            Text += " " + Application.ProductVersion; //Assembly.GetExecutingAssembly().GetName().Version.ToString(); // Assembly.GetEntryAssembly().GetName().Version.ToString();
            sectionTool = new SectionTool();
        }

        public void ApplyUI()
        {
            try
            {
                Opacity = Properties.Settings.Default.UIOpacity.Value;
                cheatGridViewRowIndexForeBrush = new SolidBrush(Properties.Settings.Default.MainCheatGridViewRowIndexForeColor.Value); //SystemBrushes.HighlightText

                ForeColor = Properties.Settings.Default.MainForeColor.Value; //SystemColors.ControlText;
                BackColor = Properties.Settings.Default.MainBackColor.Value; //SystemColors.ControlDarkDark;
                ToolStrip1.BackColor = Properties.Settings.Default.MainToolStrip1BackColor.Value; //Color.FromArgb(153, 180, 209);
                CheatGridView.BackgroundColor = Properties.Settings.Default.MainCheatGridViewBackgroundColor.Value; //Color.DimGray;
                CheatGridView.BaseRowColor = Properties.Settings.Default.MainCheatGridViewBaseRowColor.Value; //Color.FromArgb(100, 115, 129);
                CheatGridView.GridColor = Properties.Settings.Default.MainCheatGridViewGridColor.Value; //Color.Silver;

                var MainCheatGridCellBackColor = Properties.Settings.Default.MainCheatGridCellBackColor.Value;
                var MainCheatGridCellForeColor = Properties.Settings.Default.MainCheatGridCellForeColor.Value;

                ToolStripMsg.ForeColor = MainCheatGridCellForeColor; //Color.White;
                CheatGridViewDel.DefaultCellStyle.BackColor         = MainCheatGridCellBackColor; //Color.FromArgb(64, 64, 64);
                CheatGridViewDel.DefaultCellStyle.ForeColor         = MainCheatGridCellForeColor; //Color.White;
                CheatGridViewAddress.DefaultCellStyle.BackColor     = MainCheatGridCellBackColor; //Color.FromArgb(64, 64, 64);
                CheatGridViewAddress.DefaultCellStyle.ForeColor     = MainCheatGridCellForeColor; //Color.White;
                CheatGridViewType.DefaultCellStyle.BackColor        = MainCheatGridCellBackColor; //Color.FromArgb(64, 64, 64);
                CheatGridViewType.DefaultCellStyle.ForeColor        = MainCheatGridCellForeColor; //Color.White;
                CheatGridViewActive.DefaultCellStyle.BackColor      = MainCheatGridCellBackColor; //Color.FromArgb(64, 64, 64);
                CheatGridViewActive.DefaultCellStyle.ForeColor      = MainCheatGridCellForeColor; //Color.White;
                CheatGridViewValue.DefaultCellStyle.BackColor       = MainCheatGridCellBackColor; //Color.FromArgb(64, 64, 64);
                CheatGridViewValue.DefaultCellStyle.ForeColor       = MainCheatGridCellForeColor; //Color.White;
                CheatGridViewSection.DefaultCellStyle.BackColor     = MainCheatGridCellBackColor; //Color.FromArgb(64, 64, 64);
                CheatGridViewSection.DefaultCellStyle.ForeColor     = MainCheatGridCellForeColor; //Color.White;
                CheatGridViewSID.DefaultCellStyle.BackColor         = MainCheatGridCellBackColor; //Color.FromArgb(64, 64, 64);
                CheatGridViewSID.DefaultCellStyle.ForeColor         = MainCheatGridCellForeColor; //Color.White;
                CheatGridViewLock.DefaultCellStyle.BackColor        = MainCheatGridCellBackColor; //Color.FromArgb(64, 64, 64);
                CheatGridViewLock.DefaultCellStyle.ForeColor        = MainCheatGridCellForeColor; //Color.White;
                CheatGridViewDescription.DefaultCellStyle.BackColor = MainCheatGridCellBackColor; //Color.FromArgb(64, 64, 64);
                CheatGridViewDescription.DefaultCellStyle.ForeColor = MainCheatGridCellForeColor; //Color.White;
                CheatGridView.GroupRefresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, ex.Source + ":ApplyUI", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
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
                OpenCheatDialog.Filter = "Cheat (*.cht)|*.cht|Cheat Relative (*.chtr)|*.chtr|Cheat Json (*.json)|*.json";
                OpenCheatDialog.AddExtension = true;
                OpenCheatDialog.RestoreDirectory = true;

                if (OpenCheatDialog.ShowDialog() != DialogResult.OK) return;

                int count = 0;
                string cheatTexts = File.ReadAllText(OpenCheatDialog.FileName);
                if (OpenCheatDialog.FileName.ToUpper().EndsWith("JSON")) count = ParseCheatJson(cheatTexts);
                else
                {
                    string[] cheatStrArr = cheatTexts.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None); //File.ReadAllLines(OpenCheatDialog.FileName);
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

                    string productVersion = cheatHeaderItems[0];

                    if (Application.ProductVersion != productVersion && MessageBox.Show(string.Format("PS4 Cheater Version({0}) is different with cheat file({1}), still load?", Application.ProductVersion, productVersion),
                        "ProductVersion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                    string[] pVers = productVersion.Split('.'); //1.2.3.4 => 01020304
                    int pVerChk = int.Parse(pVers[0]) * 1000000 + int.Parse(pVers[1]) * 10000 + int.Parse(pVers[2]) * 100 + int.Parse(pVers[3]);
                    bool isSIDv1 = pVerChk < 00090505;
                    bool isRelativeV1 = pVerChk < 00090507;
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
                    Dictionary<ulong, ulong> pointerCaches = new Dictionary<ulong, ulong>();
                    (uint sid, string name, uint prot, uint offsetFirst, uint offsetAddr) preData = (0, "", 0, 0, 0);
                    for (int idx = 1; idx < cheatStrArr.Length; ++idx)
                    {
                        string cheatStr = cheatStrArr[idx];

                        if (string.IsNullOrWhiteSpace(cheatStr)) continue;

                        string[] cheatElements = cheatStr.Split('|');

                        if (cheatElements.Length < 5) continue;

                        bool isRelative = cheatElements[5].StartsWith("+");
                        bool isRelativeV1a = cheatElements[5].StartsWith("++");
                        if (isRelativeV1a) cheatElements[5] = cheatElements[5].Substring(1);
                        bool isPointer = !cheatElements[5].Contains("[") && Regex.IsMatch(cheatElements[5], "[0-9A-F]+_", RegexOptions.IgnoreCase);
                        uint sid = isRelative ? preData.sid : uint.Parse(cheatElements[0]);
                        string name = isRelative ? preData.name : cheatElements[2];
                        uint prot = isRelative ? preData.prot : uint.Parse(cheatElements[3], NumberStyles.HexNumber);
                        ScanType cheatScanType = this.ParseFromDescription<ScanType>(cheatElements[6]);
                        bool cheatLock = bool.Parse(cheatElements[7]);
                        string cheatValue = cheatElements[8];
                        string cheatDesc = cheatElements[9];
                        int relativeOffset = isRelative ? int.Parse(cheatElements[5].Substring(1), NumberStyles.HexNumber) : -1;

                        Section section = isSIDv1 ? sectionTool.GetSectionBySIDv1(sid, name, prot) : sectionTool.GetSection(sid, name, prot);
                        if (section == null && ToolStripLockEnable.Checked) ToolStripLockEnable.Checked = false;

                        uint offsetAddr = 0;
                        List<long> pointerOffsets = null;
                        if (isPointer)
                        {
                            pointerOffsets = new List<long>();
                            string[] offsetArr = cheatElements[5].Split('_');
                            for (int offsetIdx = 0; offsetIdx < offsetArr.Length; ++offsetIdx)
                            {
                                string offsetStr = offsetArr[offsetIdx];
                                long offset = long.Parse(offsetStr, NumberStyles.HexNumber);
                                if (offsetIdx == 0 && section != null) offset += (long)section.Start;
                                pointerOffsets.Add(offset);
                            }
                        }
                        else if (isRelative)
                        {
                            preData.offsetAddr = (uint)relativeOffset + (isRelativeV1 || isRelativeV1a ? preData.offsetAddr : 0);
                            offsetAddr = preData.offsetFirst + preData.offsetAddr;
                        }
                        else offsetAddr = uint.Parse(cheatElements[5], NumberStyles.HexNumber);

                        DataGridViewRow cheatRow = AddToCheatGrid(section, offsetAddr, cheatScanType, cheatValue, cheatLock, cheatDesc, pointerOffsets, pointerCaches, isRelative ? (int)preData.offsetAddr : -1);
                        if (cheatRow == null)
                        {
                            preData = (0, "", 0, 0, 0);
                            continue;
                        }
                        if (isPointer)
                        {
                            (section, offsetAddr) = ((Section section, uint offsetAddr))cheatRow.Tag;
                            sid = section.SID;
                            name = section.Name;
                            prot = section.Prot;
                        }
                        if (!isRelative) preData = (sid, name, prot, offsetAddr, 0);
                        count++;
                    }
                }

                CheatGridView.GroupRefresh();
                SaveCheatDialog.FileName = OpenCheatDialog.FileName;
                SaveCheatDialog.FilterIndex = OpenCheatDialog.FilterIndex;

                ToolStripMsg.Text = string.Format("Successfully loaded {0} cheat items.", count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, ex.Source + ":ToolStripOpen_Click", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
                    code.value = ScanTool.BytesToString(code.vtype, valueBytes, false, code.value.StartsWith("-"));
                }
            }

            return code;
        }

        private int ParseCheatJson(string cheatTexts)
        {
            int count = 0;
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(cheatTexts)))
            {
                DataContractJsonSerializer deseralizer = new DataContractJsonSerializer(typeof(CheatJson));
                cheatJson = (CheatJson)deseralizer.ReadObject(ms);

                ProcessName = cheatJson.Process;
                if (!InitSectionList(ProcessName)) return 0;

                #region cheatHeaderItems Check
                string cheatGameID = cheatJson.Id;
                string cheatGameVer = cheatJson.Version;
                string PS4FWVersion = Properties.Settings.Default.PS4FWVersion.Value ?? "";
                string FMVer = PS4FWVersion != "" ? PS4FWVersion : Constant.Versions[0];

                ScanTool.GameInfo(FMVer, out string gameID, out string gameVer);

                if (gameID != cheatGameID && MessageBox.Show(string.Format("Your Game ID({0}) is different with cheat file({1}), still load?", gameID, cheatGameID),
                    "GameID", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return 0;
                if (gameVer != cheatGameVer && MessageBox.Show(string.Format("Your Game version({0}) is different with cheat file({1}), still load?", gameVer, cheatGameVer),
                    "GameVer", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return 0;
                #endregion

                Section section;
                Section sectionFirst = sectionTool.GetSectionSortByAddr()[0];
                foreach (CheatJson.Mod cheatMod in cheatJson.Mods)
                {
                    bool cheatLock = false;
                    string cheatDesc = cheatMod.Name;
                    ScanType cheatScanType = ScanType.Hex;
                    section = sectionFirst;
                    foreach (CheatJson.Memory memory in cheatMod.Memory)
                    {
                        string cheatValue = memory.On;
                        ulong offsetAddr = ulong.Parse(memory.Offset, NumberStyles.HexNumber);
                        if (offsetAddr >= (ulong)sectionFirst.Length)
                        {
                            var sid = sectionTool.GetSectionID(offsetAddr + sectionFirst.Start);
                            if (sid == 0) section = new Section();
                            else
                            {
                                var newSection = sectionTool.GetSection(sid);
                                offsetAddr += sectionFirst.Start - newSection.Start;
                                section = newSection;
                            }
                        }

                        DataGridViewRow cheatRow = AddToCheatGrid(section, (uint)offsetAddr, cheatScanType, cheatValue, cheatLock, String.Format("{0} __ [ on: {1} off: {2} ]", cheatDesc, memory.On, memory.Off));
                        count++;
                    }
                }
            }
            return count;
        }

        private void ToolStripSave_Click(object sender, EventArgs e)
        {
            if (CheatGridView.Rows.Count == 0) return;

            string FMVer = Properties.Settings.Default.PS4FWVersion.Value;
            ScanTool.GameInfo(FMVer, out string gameID, out string gameVer);
            SaveCheatDialog.Filter = "Cheat (*.cht)|*.cht|Cheat Relative (*.chtr)|*.chtr|Cheat Json (*.json)|*.json";
            SaveCheatDialog.AddExtension = true;
            SaveCheatDialog.RestoreDirectory = true;
            if (string.IsNullOrWhiteSpace(SaveCheatDialog.FileName)) SaveCheatDialog.FileName = gameID;

            if (SaveCheatDialog.ShowDialog() != DialogResult.OK) return;
            if (!InitSectionList(ProcessName)) return;
            
            if (SaveCheatDialog.FileName.ToUpper().EndsWith("JSON"))
            {
                if (cheatJson == null || cheatJson.Id != gameID) cheatJson = new CheatJson(gameID, gameID, gameVer, ProcessName);
                else cheatJson.Mods = new List<CheatJson.Mod>();

                CheatJson.Mod modBak = null;
                Section sectionFirst = sectionTool.GetSectionSortByAddr()[0];
                for (int cIdx = 0; cIdx < CheatGridView.Rows.Count; cIdx++)
                {
                    DataGridViewRow cheatRow = CheatGridView.Rows[cIdx];

                    (Section section, ulong offsetAddr) = ((Section section, uint offsetAddr))cheatRow.Tag;
                    if (section.SID != sectionFirst.SID) offsetAddr += section.Start - sectionFirst.Start;

                    string cheatDesc = cheatRow.Cells[(int)ChertCol.CheatListDesc].Value.ToString();
                    ScanType scanType = this.ParseFromDescription<ScanType>(cheatRow.Cells[(int)ChertCol.CheatListType].Value.ToString());
                    string on = cheatRow.Cells[(int)ChertCol.CheatListValue].Value.ToString();
                    if (scanType != ScanType.Hex)
                    {
                        byte[] bytes = ScanTool.ValueStringToByte(scanType, on);
                        on = ScanTool.BytesToString(scanType, bytes, true, on.StartsWith("-"));
                        on = ScanTool.ReverseHexString(on);
                    }
                    string off = on;
                    if (Regex.Match(cheatDesc, @"(.*) *__ *\[ *on: *([0-9a-zA-Z]+) *off: *([0-9a-zA-Z]+)") is Match m1 && m1.Success)
                    { //Attempt to restore off value from desc
                        cheatDesc = m1.Groups[1].Value;
                        off = m1.Groups[3].Value;
                    }

                    if (modBak != null && modBak.Name == cheatDesc) cheatJson.Mods[cheatJson.Mods.Count - 1].Memory.Add(new CheatJson.Memory(offsetAddr.ToString("X"), on, off));
                    else
                    {
                        CheatJson.Mod mod = new CheatJson.Mod(cheatDesc, "checkbox");
                        mod.Memory.Add(new CheatJson.Memory(offsetAddr.ToString("X"), on, off));

                        cheatJson.Mods.Add(mod);
                        modBak = mod;
                    }

                }

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(CheatJson));
                using (var msObj = new MemoryStream())
                {
                    serializer.WriteObject(msObj, cheatJson);
                    var bytes = msObj.ToArray();
                    string json = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                    using (var myStream = new StreamWriter(SaveCheatDialog.FileName)) myStream.Write(json);
                }
            }
            else
            {
                string processName = ProcessName;
                string saveBuf = $"{Application.ProductVersion}|{processName}|{gameID}|{gameVer}|{FMVer}\n";
                uint maxRelative = 0x100;
                ulong preAddr = 0;
                for (int cIdx = 0; cIdx < CheatGridView.Rows.Count; cIdx++)
                {
                    DataGridViewRow cheatRow = CheatGridView.Rows[cIdx];

                    (Section section, uint offsetAddr) = ((Section section, uint offsetAddr))cheatRow.Tag;
                    bool isRelative = preAddr > 0 && offsetAddr > preAddr && offsetAddr - preAddr <= maxRelative;
                    bool isPointer = cheatRow.Cells[(int)ChertCol.CheatListAddress].Value.ToString().StartsWith("P->");
                    Section newSection = section;
                    string addressStr;
                    string sectionStr = "";
                    if (isPointer)
                    {
                        uint sid = cheatRow.Cells[(int)ChertCol.CheatListSID].Value == null ? 0 : (uint)cheatRow.Cells[(int)ChertCol.CheatListSID].Value;
                        string[] sectionArr = cheatRow.Cells[(int)ChertCol.CheatListSection].Value.ToString().Split('|');
                        newSection = sectionTool.GetSection(sid, sectionArr[1], uint.Parse(sectionArr[2]));
                        addressStr = sectionArr[sectionArr.Length - 1];
                    }
                    else if (SaveCheatDialog.FilterIndex == 2 && isRelative)
                    {
                        addressStr = "+" + (offsetAddr - preAddr).ToString("X");
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
                        cheatRow.Cells[(int)ChertCol.CheatListType].Value,
                        cheatRow.Cells[(int)ChertCol.CheatListLock].Value,
                        cheatRow.Cells[(int)ChertCol.CheatListValue].Value,
                        cheatRow.Cells[(int)ChertCol.CheatListDesc].Value
                        );
                    if (SaveCheatDialog.FilterIndex == 2 && !isRelative) preAddr = offsetAddr;
                }

                using (var myStream = new StreamWriter(SaveCheatDialog.FileName)) myStream.Write(saveBuf);
            }

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, ex.Source + ":ToolStripAdd_Click", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void ToolStripExpandAll_Click(object sender, EventArgs e) => CheatGridView.CollapseExpandAll(false);

        private void ToolStripCollapseAll_Click(object sender, EventArgs e) => CheatGridView.CollapseExpandAll(true);

        private void ToolStripSettings_Click(object sender, EventArgs e)
        {
            if (option == null || option.IsDisposed) option = new Option(this);
            option.StartPosition = FormStartPosition.CenterParent;
            option.Show();
        }

        private void ToolStripHexView_Click(object sender, EventArgs e)
        {
            try
            {
                Section section;
                uint offsetAddr;

                string inputValue = "";

                if (CheatGridView.SelectedRows.Count == 1)
                {
                    DataGridViewSelectedRowCollection rows = CheatGridView.SelectedRows;
                    var cheatRow = rows[0];
                    (section, offsetAddr) = ((Section section, uint offsetAddr))cheatRow.Tag;
                    inputValue = (section.Start + offsetAddr).ToString("X");
                }

                if (InputBox.Show("Hex View", "Please enter the memory address(hex) you want to view", ref inputValue) != DialogResult.OK) return;

                if ((ProcessName ?? "") == "") throw new Exception("No Process currently");
                if (!InitSectionList(ProcessName)) throw new Exception(String.Format("Process({0}): InitSectionList failed", ProcessName));

                ulong address = ulong.Parse(inputValue, NumberStyles.HexNumber);
                uint sid = sectionTool.GetSectionID(address);
                if (sid == 0) return; //-1(int) => 0(uint)

                section = sectionTool.GetSection(sid);
                offsetAddr = (uint)(address - section.Start);

                HexEditor hexEdit = new HexEditor(this, section, (int)offsetAddr);
                hexEdit.Show(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, ex.Source + ":ToolStripHexView_Click", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
            (uint sid, string name, uint prot, uint offsetAddr) preData = (0, "", 0, 0);
            Dictionary<ulong, ulong> pointerCaches = new Dictionary<ulong, ulong>();
            for (int cIdx = 0; cIdx < CheatGridView.Rows.Count; cIdx++)
            {
                try
                {
                    string msg = string.Format("{0}/{1}", cIdx + 1, CheatGridView.Rows.Count);
                    ToolStripMsg.Text = string.Format("{0:000}%, Refresh Cheat elapsed:{1}s. {2}", (int)(((float)(cIdx + 1) / CheatGridView.Rows.Count) * 100), tickerMajor.Elapsed.TotalSeconds, msg);

                    DataGridViewRow cheatRow = CheatGridView.Rows[cIdx];
                    (Section section, uint offsetAddr) = ((Section section, uint offsetAddr))cheatRow.Tag;
                    #region  Refresh Section and offsetAddr
                    if (VerifySectionWhenRefresh)
                    {
                        uint sid = cheatRow.Cells[(int)ChertCol.CheatListSID].Value == null ? 0 : (uint)cheatRow.Cells[(int)ChertCol.CheatListSID].Value;
                        string[] sectionArr = cheatRow.Cells[(int)ChertCol.CheatListSection].Value.ToString().Split('|');
                        string hexAddr = "";
                        bool isRelative = sectionArr[0].StartsWith("+");
                        bool isPointer = cheatRow.Cells[(int)ChertCol.CheatListAddress].Value.ToString().StartsWith("P->");
                        if (isRelative && preData.sid == 0) section = null;
                        else (section, offsetAddr, hexAddr) = RefreshSection(sid, sectionArr, offsetAddr, isPointer, preData, pointerCaches);
                        if (section == null)
                        {
                            ToolStripMsg.Text = string.Format("RefreshCheat Failed...CheatGrid({0}), section: {1}, offsetAddr: {2:X}", cIdx, string.Join("-", sectionArr), offsetAddr);
                            if (ToolStripLockEnable.Checked) Invoke(new MethodInvoker(() => { ToolStripLockEnable.Checked = false; }));
                            cheatRow.DefaultCellStyle.ForeColor = Color.Red;
                            preData = (0, "", 0, 0);
                            continue;
                        }
                        else if (cheatRow.DefaultCellStyle.ForeColor == Color.Red) cheatRow.DefaultCellStyle.ForeColor = default;
                        if (!isRelative) preData = (section.SID, section.Name, section.Prot, offsetAddr);
                        cheatRow.Cells[(int)ChertCol.CheatListAddress].Value = hexAddr;
                    }
                    #endregion
                    ScanType scanType = this.ParseFromDescription<ScanType>(cheatRow.Cells[(int)ChertCol.CheatListType].Value.ToString());
                    bool isSign = cheatRow.Cells[(int)ChertCol.CheatListType].Tag != null && (bool)cheatRow.Cells[(int)ChertCol.CheatListType].Tag;
                    int scanTypeLength = 0;
                    if (scanType != ScanType.Hex && scanType != ScanType.String_) ScanTool.ScanTypeLengthDict.TryGetValue(scanType, out scanTypeLength);
                    else scanTypeLength = ScanTool.ValueStringToByte(scanType, cheatRow.Cells[(int)ChertCol.CheatListValue].Value.ToString()).Length;
                    byte[] newData = PS4Tool.ReadMemory(section.PID, offsetAddr + section.Start, scanTypeLength);
                    cheatRow.Tag = (section, offsetAddr);
                    cheatRow.Cells[(int)ChertCol.CheatListValue].Value = ScanTool.BytesToString(scanType, newData, false, isSign);
                }
                catch (Exception) { preData = (0, "", 0, 0); }
            }
            return true;
        });

        private (Section section, uint offsetAddr, string hexAddr) RefreshSection(uint sectionSid, string[] sectionArr, uint offsetAddr, bool isPointer, (uint sid, string name, uint prot, uint offsetAddr) preData, Dictionary<ulong, ulong> pointerCaches)
        {
            bool isRelative = sectionArr[0].StartsWith("+");
            int relativeOffset = isRelative ? int.Parse(sectionArr[0].Substring(1), NumberStyles.HexNumber) : -1;

            uint sid = isRelative ? preData.sid : sectionSid;
            string name = isRelative ? preData.name : sectionArr[1];
            uint prot = isRelative ? preData.prot : uint.Parse(sectionArr[2], NumberStyles.HexNumber);
            string hexAddr;

            Section section;
            Section refreshSection = sectionTool.GetSection(sid, name, prot);
            if (refreshSection == null) return (null, offsetAddr, "");
            if (isPointer)
            {
                string[] offsetArr = sectionArr[sectionArr.Length - 1].Split('_');
                long[] pointerOffsets = new long[offsetArr.Length];
                for (int idx = 0; idx < offsetArr.Length; ++idx)
                {
                    long offset = long.Parse(offsetArr[idx], NumberStyles.HexNumber);
                    if (idx == 0) offset += (long)refreshSection.Start;
                    pointerOffsets[idx] = offset;
                }
                ulong tailAddr = PS4Tool.ReadTailAddress(refreshSection.PID, pointerOffsets, pointerCaches);

                section = sectionTool.GetSection(sectionTool.GetSectionID(tailAddr));
                if (section == null) return (null, offsetAddr, "");

                offsetAddr = (uint)(tailAddr - section.Start);
                hexAddr = "P->" + tailAddr.ToString("X8");
            }
            else
            {
                section = refreshSection;
                offsetAddr = isRelative ? (uint)relativeOffset + preData.offsetAddr : offsetAddr;
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
                ToolStripProcessInfo.Text = "Current Processes: " + (ProcessName == "" ? "Empty" : (ProcessName + (ProcessPid > 0 ? "(" + ProcessPid + ")" : "")));
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
            (uint sid, string name, uint prot, uint offsetAddr) preData = (0, "", 0, 0);
            Dictionary<ulong, ulong> pointerCaches = new Dictionary<ulong, ulong>();
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
                    (Section section, uint offsetAddr) = ((Section section, uint offsetAddr))cheatRow.Tag;
                    #region Refresh Section and offsetAddr
                    if (VerifySectionWhenLock)
                    {
                        string hexAddr;
                        uint sid = cheatRow.Cells[(int)ChertCol.CheatListSID].Value == null ? 0 : (uint)cheatRow.Cells[(int)ChertCol.CheatListSID].Value;
                        string[] sectionArr = cheatRow.Cells[(int)ChertCol.CheatListSection].Value.ToString().Split('|');
                        bool isRelative = sectionArr[0].StartsWith("+");
                        bool isPointer = cheatRow.Cells[(int)ChertCol.CheatListAddress].Value.ToString().StartsWith("P->");
                        if (isRelative && preData.sid == 0)
                        {
                            for (int idx = cIdx - 1; idx >= 0; idx--)
                            {
                                DataGridViewRow checkRow = CheatGridView.Rows[idx];
                                uint checkSid = checkRow.Cells[(int)ChertCol.CheatListSID].Value == null ? 0 : (uint)checkRow.Cells[(int)ChertCol.CheatListSID].Value;
                                string[] checkSArr = checkRow.Cells[(int)ChertCol.CheatListSection].Value.ToString().Split('|');
                                if (checkSArr[0].StartsWith("+")) continue;

                                (Section section, uint offsetAddr) checkCheat = ((Section section, uint offsetAddr))checkRow.Tag;
                                bool checkPointer = checkRow.Cells[(int)ChertCol.CheatListAddress].Value.ToString().StartsWith("P->");
                                (Section section, uint offsetAddr, string hexAddr) checkSection = RefreshSection(checkSid, checkSArr, checkCheat.offsetAddr, checkPointer, preData, pointerCaches);
                                if (checkSection.section == null)
                                {
                                    ToolStripMsg.Text = string.Format("RefreshLock Failed...CheatGrid({0}), section: {1}, offsetAddr: {2:X}", cIdx, string.Join("-", sectionArr), offsetAddr);
                                    cheatRow.DefaultCellStyle.ForeColor = Color.Red;
                                    preData = (0, "", 0, 0);
                                    break;
                                }
                                preData = (checkSection.section.SID, checkSection.section.Name, checkSection.section.Prot, checkSection.offsetAddr);
                                break;
                            }
                        }
                        (section, offsetAddr, hexAddr) = RefreshSection(sid, sectionArr, offsetAddr, isPointer, preData, pointerCaches);
                        if (section == null)
                        {
                            ToolStripMsg.Text = string.Format("RefreshLock Failed...CheatGrid({0}), section: {1}, offsetAddr: {2:X}", cIdx, string.Join("-", sectionArr), offsetAddr);
                            cheatRow.DefaultCellStyle.ForeColor = Color.Red;
                            preData = (0, "", 0, 0);
                            continue;
                        }
                        else if (cheatRow.DefaultCellStyle.ForeColor == Color.Red) cheatRow.DefaultCellStyle.ForeColor = default;
                        if (!isRelative) preData = (section.SID, section.Name, section.Prot, offsetAddr);
                        cheatRow.Tag = (section, offsetAddr);
                        cheatRow.Cells[(int)ChertCol.CheatListAddress].Value = hexAddr;
                    }
                    #endregion
                    ScanType scanType = this.ParseFromDescription<ScanType>(cheatRow.Cells[(int)ChertCol.CheatListType].Value.ToString());
                    byte[] newData = ScanTool.ValueStringToByte(scanType, cheatRow.Cells[(int)ChertCol.CheatListValue].Value.ToString());
                    PS4Tool.WriteMemory(section.PID, offsetAddr + section.Start, newData);
                }
                catch (Exception) { preData = (0, "", 0, 0); }
            }
            return true;
        });
        #endregion

        #region CheatGridView
        private void CheatGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var format = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
            var bounds = new Rectangle(e.RowBounds.X + 16, e.RowBounds.Top, 16, e.RowBounds.Height);
            e.Graphics.DrawString(e.RowIndex.ToString(), Font, cheatGridViewRowIndexForeBrush, bounds, format);
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
                        (Section section, uint offsetAddr) = ((Section section, uint offsetAddr))cheatRow.Tag;
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, ex.Source + ":CheatGridView_CellContentClick", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
            catch (Exception ex)
            {
                e.Cancel = true;
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void CheatGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow editedRow = CheatGridView.Rows[e.RowIndex];
            if (e.ColumnIndex != (int)ChertCol.CheatListValue) return;
            try
            {
                (Section section, uint offsetAddr) = ((Section section, uint offsetAddr))editedRow.Tag;
                ScanType scanType = this.ParseFromDescription<ScanType>(editedRow.Cells[(int)ChertCol.CheatListType].Value.ToString());
                byte[] data = ScanTool.ValueStringToByte(scanType, editedRow.Cells[(int)ChertCol.CheatListValue].Value.ToString());
                PS4Tool.WriteMemory(section.PID, offsetAddr + section.Start, data);
                editedRow.Tag = (section, offsetAddr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, ex.Source + ":CheatGridView_CellEndEdit", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void CheatGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            bool CheatCellDirtyValueCommit = Properties.Settings.Default.CheatCellDirtyValueCommit.Value;
            if (!CheatCellDirtyValueCommit) return;

            DataGridViewUpDownEditingControl upDownControl = CheatGridView.EditingControl as DataGridViewUpDownEditingControl;
            if (upDownControl == null) return;

            upDownControl.UpDown -= UpDownControl_UpDown;
            upDownControl.UpDown += UpDownControl_UpDown;
        }

        private void UpDownControl_UpDown(object sender, DataGridViewUpDownCellEventArgs e)
        {
            bool CheatCellDirtyValueCommit = Properties.Settings.Default.CheatCellDirtyValueCommit.Value;
            if (!CheatCellDirtyValueCommit) return;

            if (CheatGridView.CurrentCell == null || e.RowIndex < 0 || e.ColumnIndex != (int)ChertCol.CheatListValue) return;

            if (!Regex.IsMatch(e.Text, @"^-?[0-9][0-9,\.]*$")) return;

            DataGridViewRow cheatRow = CheatGridView.Rows[e.RowIndex];

            (Section section, uint offsetAddr) = ((Section section, uint offsetAddr))cheatRow.Tag;
            ScanType scanType = this.ParseFromDescription<ScanType>(cheatRow.Cells[(int)ChertCol.CheatListType].Value.ToString());

            byte[] newData = ScanTool.ValueStringToByte(scanType, e.Text);
            PS4Tool.WriteMemory(section.PID, offsetAddr + section.Start, newData);
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
                (Section section, uint offsetAddr) = ((Section section, uint offsetAddr))cheatRow.Tag;

                HexEditor hexEdit = new HexEditor(this, section, (int)offsetAddr);
                hexEdit.Show(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, ex.Source + ":CheatGridMenuHexEditor_Click", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
                (Section section, uint offsetAddr) = ((Section section, uint offsetAddr))cheatRow.Tag;
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
                        (Section section, uint offsetAddr) row = ((Section section, uint offsetAddr))cheatRow.Tag;
                        ScanType rowScanType = this.ParseFromDescription<ScanType>(cheatRow.Cells[(int)ChertCol.CheatListType].Value.ToString());
                        byte[] rowData = ScanTool.ValueStringToByte(rowScanType, inputValue);
                        PS4Tool.WriteMemory(row.section.PID, row.offsetAddr + row.section.Start, rowData);
                        cheatRow.Cells[(int)ChertCol.CheatListValue].Value = ScanTool.BytesToString(rowScanType, rowData, false, inputValue.StartsWith("-"));
                    }
                    return;
                }

                cheatRow = CheatGridView.SelectedRows[0];
                ScanType scanType = this.ParseFromDescription<ScanType>(cheatRow.Cells[(int)ChertCol.CheatListType].Value.ToString());
                string oldValue = cheatRow.Cells[(int)ChertCol.CheatListValue].Value.ToString();
                (Section section, uint offsetAddr) = ((Section section, uint offsetAddr))cheatRow.Tag;

                NewAddress newAddress = null;
                bool isPointer = cheatRow.Cells[(int)ChertCol.CheatListAddress].Value.ToString().StartsWith("P->");
                List<long> offsetList = null;
                if (isPointer)
                {
                    offsetList = new List<long>();
                    uint baseSID = cheatRow.Cells[(int)ChertCol.CheatListSID].Value == null ? 0 : (uint)cheatRow.Cells[(int)ChertCol.CheatListSID].Value;
                    string[] baseSectionArr = cheatRow.Cells[(int)ChertCol.CheatListSection].Value.ToString().Split('|');
                    long baseSectionStart = long.Parse(baseSectionArr[0], NumberStyles.HexNumber);
                    string baseName = baseSectionArr[1];
                    uint baseProt = uint.Parse(baseSectionArr[2], NumberStyles.HexNumber);

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

                cheatRow.Tag = (newAddress.AddrSection, (uint)(newAddress.Address - newAddress.AddrSection.Start));
                cheatRow.Cells[(int)ChertCol.CheatListAddress].Value = (newAddress.Address).ToString("X8");
                cheatRow.Cells[(int)ChertCol.CheatListType].Value = newAddress.CheatType.GetDescription();
                cheatRow.Cells[(int)ChertCol.CheatListLock].Value = newAddress.IsLock;
                cheatRow.Cells[(int)ChertCol.CheatListDesc].Value = newAddress.Descriptioin;
                if (newAddress.IsPointer)
                {
                    cheatRow.Cells[(int)ChertCol.CheatListAddress].Value = "P->" + cheatRow.Cells[(int)ChertCol.CheatListAddress].Value;
                    string offsetStr = "|";
                    for (int idx = 0; idx < newAddress.PointerOffsets.Count; idx++)
                    {
                        long offset = newAddress.PointerOffsets[idx];
                        if (offsetStr == "|") offsetStr += (offset - (long)newAddress.BaseSection.Start).ToString("X");
                        else offsetStr += "_" + offset.ToString("X");
                    }
                    cheatRow.Cells[(int)ChertCol.CheatListSID].Value = newAddress.BaseSection.SID;
                    cheatRow.Cells[(int)ChertCol.CheatListSection].Value = string.Format("{0}|{1}|{2}|{3}|{4}", newAddress.BaseSection.Start.ToString("X"), newAddress.BaseSection.Name, newAddress.BaseSection.Prot.ToString("X"), newAddress.BaseSection.Offset.ToString("X"), offsetStr);
                }
                else
                {
                    cheatRow.Cells[(int)ChertCol.CheatListSID].Value = newAddress.AddrSection.SID;
                    cheatRow.Cells[(int)ChertCol.CheatListSection].Value = string.Format("{0}|{1}|{2}|{3}", newAddress.AddrSection.Start.ToString("X"), newAddress.AddrSection.Name, newAddress.AddrSection.Prot.ToString("X"), newAddress.AddrSection.Offset.ToString("X"));
                }

                byte[] data = ScanTool.ValueStringToByte(newAddress.CheatType, newAddress.Value);
                PS4Tool.WriteMemory(section.PID, offsetAddr + section.Start, data);
                cheatRow.Cells[(int)ChertCol.CheatListValue].Value = newAddress.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, ex.Source + ":CheatGridMenuEdit_Click", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
                (Section section, uint offsetAddr) = ((Section section, uint offsetAddr))cheatRow.Tag;
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
                (Section section, uint offsetAddr) = ((Section section, uint offsetAddr))cheatRow.Tag;
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
        /// <param name="pointerOffsets">offsets of Pointer</param>
        /// <param name="pointerCaches">memory caches for pointer</param>
        /// <param name="relativeOffset">store address as a relative offset</param>
        /// <returns>returns the cheatRow added this time, which can be used to change the row style</returns>
        public DataGridViewRow AddToCheatGrid(Section section, uint offsetAddr, ScanType scanType, string oldValue, bool cheatLock = false, string cheatDesc = "", List<long> pointerOffsets = null, Dictionary<ulong, ulong> pointerCaches = null, int relativeOffset=-1)
        {
            DataGridViewRow cheatRow = null;
            try
            {
                bool isFailed = false;
                bool isPointer = pointerOffsets != null && pointerOffsets.Count > 0;
                (Section section, uint offsetAddr) pointerTag = (new Section(), 0);
                if (isPointer)
                {
                    var baseSection = section;
                    ulong baseAddress = 0;
                    for (int idx = 0; idx < pointerOffsets.Count; ++idx)
                    {
                        long pointerOffset = pointerOffsets[idx];
                        ulong queryAddress = (ulong)pointerOffset + baseAddress;
                        if (idx != pointerOffsets.Count - 1)
                        {
                            if (pointerCaches != null && pointerCaches.TryGetValue(queryAddress, out baseAddress)) continue;
                            baseAddress = BitConverter.ToUInt64(PS4Tool.ReadMemory(section.PID, queryAddress, 8), 0);
                            if (pointerCaches != null) pointerCaches.Add(queryAddress, baseAddress);
                            if (baseAddress == 0)
                            {
                                isFailed = true;
                                ToolStripMsg.Text = string.Format("Add Pointer To CheatGrid failed...NextAddress is zero, offsets:{0}, Desc:{1}", String.Join("-", pointerOffsets.Select(p => p.ToString("X")).ToArray()), cheatDesc);
                                break;
                            }
                        }
                        else
                        {
                            uint SID = sectionTool.GetSectionID((ulong)pointerOffset + baseAddress);
                            if (SID == 0) //-1(int) => 0(uint)
                            {
                                isFailed = true;
                                ToolStripMsg.Text = string.Format("Add Pointer To CheatGrid failed...Section not found, address:{0:X8}, offsets:{1}, Desc:{2}", (ulong)pointerOffset + baseAddress, String.Join("-", pointerOffsets.Select(p => p.ToString("X")).ToArray()), cheatDesc);
                                break;
                            }
                            pointerTag.section = sectionTool.GetSection(SID);
                            pointerTag.offsetAddr = (uint)((ulong)pointerOffset + baseAddress - pointerTag.section.Start);
                        }
                    }
                }
                int cheatIdx = CheatGridView.Rows.Add();
                cheatRow = CheatGridView.Rows[cheatIdx];
                if (isFailed) cheatRow.DefaultCellStyle.ForeColor = Color.Red;

                if (relativeOffset > -1) cheatRow.Cells[(int)ChertCol.CheatListSection].Value = "+" + relativeOffset.ToString("X");
                else
                {
                    cheatRow.Cells[(int)ChertCol.CheatListSID].Value = section.SID;
                    cheatRow.Cells[(int)ChertCol.CheatListSection].Value = string.Format("{0}|{1}|{2}|{3}", section.Start.ToString("X"), section.Name, section.Prot.ToString("X"), section.Offset.ToString("X"));
                }
                
                if (isPointer)
                {
                    cheatRow.Tag = pointerTag;
                    cheatRow.Cells[(int)ChertCol.CheatListAddress].Value = "P->" + (pointerTag.offsetAddr + pointerTag.section.Start).ToString("X");
                    string offsetStr = "|";
                    for (int idx = 0; idx < pointerOffsets.Count; idx++)
                    {
                        long pointerOffset = pointerOffsets[idx];
                        if (offsetStr == "|") offsetStr += (pointerOffset - (long)section.Start).ToString("X");
                        else offsetStr += "_" + pointerOffset.ToString("X");
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
                cheatRow.Cells[(int)ChertCol.CheatListType].Tag = oldValue.StartsWith("-"); //IsSign
                cheatRow.Cells[(int)ChertCol.CheatListValue].Value = oldValue;
                cheatRow.Cells[(int)ChertCol.CheatListLock].Value = cheatLock;
                cheatRow.Cells[(int)ChertCol.CheatListDesc].Value = cheatDesc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, ex.Source + ":AddToCheatGrid", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
                Invoke(new MethodInvoker(() =>
                {
                    ToolStripProcessInfo.Text = string.Format("ProcessInfo: Cheat file Process({0}) could not find.", processName);
                    ToolStripProcessInfo.Tag = false;
                }));
                return false;
            }
            else if (ToolStripProcessInfo.Tag is bool) ToolStripProcessInfo.Tag = null; //當已查詢到ProcessInfo時，清除失敗的Tag
            else if (!force && processInfo.pid > 0 && processInfo.pid == sectionTool.PID && sectionTool.SectionDict != null)
            {
                ProcessMap pMap = null;
                if (sectionTool.SectionDict.Count < 20) pMap = PS4Tool.GetProcessMaps(processInfo.pid);
                if (pMap == null || sectionTool.SectionDict.Count >= pMap.entries.Length) return true; //當Process ID相同時，不再執行初始化
            }

            ProcessPid = processInfo.pid;
            ProcessName = processInfo.name;
            sectionTool.InitSectionList(processInfo.pid, ProcessName);

            return true;
        }
        #endregion
    }
}
