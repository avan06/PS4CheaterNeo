using GroupGridView;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace PS4CheaterNeo
{
    public partial class Main : Form
    {
        private Option option;
        private SendPayload sendPayload;
        private CheatJson cheatJson = null;
        private CheatTrainer cheatTrainer = null;
        private Brush cheatGridViewRowIndexForeBrush;
        private bool VerifySectionWhenLock;
        private bool VerifySectionWhenRefresh;
        private bool CheatAutoRefreshShowStatus;
        private uint CheatGridGroupRefreshThreshold;
        private (string msg, string name) processInfo = ("Current Processes: ", "");

        public class CheatRow
        {
            public Section Section_;
            public uint OffsetAddr;
            public Color ForeColor;
            public bool IsSign;
            public List<Object> Cells;
        }

        List<CheatRow> cheatGridRowList = new List<CheatRow>();

        public FontFamily UIFont { get; private set; } = null;
        public LanguageJson langJson { get; private set; } = null;
        public SectionTool sectionTool { get; private set; }
        public string GameID { get; private set; }
        public string GameVer { get; private set; }
        public Dictionary<uint, (ulong Start, ulong End, bool Valid, byte Prot, string Name)> LocalHiddenSections { get; private set; }
        public string ProcessName;
        public string ProcessContentid;
        public int ProcessPid;
        public Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; //Avoid the case where CurrentCulture.NumberFormatInfo.NumberDecimalSeparator is not "."
            if ((Properties.Settings.Default.PS4IP.Value ?? "") == "") Properties.Settings.Default.Upgrade(); //Need to get the settings again when the AssemblyVersion is changed
            InitializeComponent();
            ParseLanguageJson();
            ApplyUI();
            ToolStripLockEnable.Checked = Properties.Settings.Default.CheatLock.Value;
            ToolStripAutoRefresh.Checked = Properties.Settings.Default.CheatAutoRefresh.Value;
            CheatAutoRefreshShowStatus = Properties.Settings.Default.CheatAutoRefreshShowStatus.Value;
            AutoRefreshTimer.Interval = (int)Properties.Settings.Default.CheatAutoRefreshTimerInterval.Value;
            VerifySectionWhenLock = Properties.Settings.Default.VerifySectionWhenLock.Value;
            VerifySectionWhenRefresh = Properties.Settings.Default.VerifySectionWhenRefresh.Value;
            CheatGridGroupRefreshThreshold = Properties.Settings.Default.CheatGridGroupRefreshThreshold.Value;
            CheatGridView.GroupByEnabled = Properties.Settings.Default.CheatGridViewGroupByEnabled.Value;

            Text += " " + Application.ProductVersion; //Assembly.GetExecutingAssembly().GetName().Version.ToString(); // Assembly.GetEntryAssembly().GetName().Version.ToString();
            sectionTool = new SectionTool(this);

            CheatGridView.Rows.Clear();
            CheatGridView.RowCount = 0;
        }

        public void ParseLanguageJson()
        {
            string codes = Properties.Settings.Default.UILanguage.Value.ToString();
            string path = "languages\\LanguageFile_" + codes + ".json";

            if (!File.Exists(path)) return;

            using (StreamReader sr = new StreamReader(path))
            using (Stream stream = sr.BaseStream)
            {
                DataContractJsonSerializer deseralizer = new DataContractJsonSerializer(typeof(LanguageJson));
                langJson = (LanguageJson)deseralizer.ReadObject(stream);
            }
        }

        public void ApplyUI()
        {
            try
            {
                if (langJson != null)
                {
                    ToolStripLockEnable.Text          = langJson.MainForm.ToolStripLockEnable;
                    ToolStripAutoRefresh.Text         = langJson.MainForm.ToolStripAutoRefresh;
                    ToolStripSend.ToolTipText         = langJson.MainForm.ToolStripSendToolTipText;
                    ToolStripOpen.ToolTipText         = langJson.MainForm.ToolStripOpenToolTipText;
                    ToolStripSave.ToolTipText         = langJson.MainForm.ToolStripSaveToolTipText;
                    ToolStripNewQuery.ToolTipText     = langJson.MainForm.ToolStripNewQueryToolTipText;
                    ToolStripAdd.ToolTipText          = langJson.MainForm.ToolStripAddToolTipText;
                    ToolStripHexView.ToolTipText      = langJson.MainForm.ToolStripHexViewToolTipText;
                    ToolStripRefreshCheat.ToolTipText = langJson.MainForm.ToolStripRefreshCheatToolTipText;
                    ToolStripExpandAll.ToolTipText    = langJson.MainForm.ToolStripExpandAllToolTipText;
                    ToolStripCollapseAll.ToolTipText  = langJson.MainForm.ToolStripCollapseAllToolTipText;
                    ToolStripLockEnable.ToolTipText   = langJson.MainForm.ToolStripLockEnableToolTipText;
                    ToolStripAutoRefresh.ToolTipText  = langJson.MainForm.ToolStripAutoRefreshToolTipText;
                    ToolStripSettings.ToolTipText     = langJson.MainForm.ToolStripSettingsToolTipText;
                    processInfo.msg                   = langJson.MainForm.ToolStripProcessInfoMsg;
                    ToolStripProcessInfo.Text         = processInfo.msg + processInfo.name;

                    CheatGridViewDel.HeaderText         = langJson.MainForm.CheatGridViewDel;
                    CheatGridViewAddress.HeaderText     = langJson.MainForm.CheatGridViewAddress;
                    CheatGridViewType.HeaderText        = langJson.MainForm.CheatGridViewType;
                    CheatGridViewActive.HeaderText      = langJson.MainForm.CheatGridViewActive;
                    CheatGridViewValue.HeaderText       = langJson.MainForm.CheatGridViewValue;
                    CheatGridViewSection.HeaderText     = langJson.MainForm.CheatGridViewSection;
                    CheatGridViewSID.HeaderText         = langJson.MainForm.CheatGridViewSID;
                    CheatGridViewLock.HeaderText        = langJson.MainForm.CheatGridViewLock;
                    CheatGridViewDescription.HeaderText = langJson.MainForm.CheatGridViewDescription;

                    CheatGridMenuHexEditor.Text   = langJson.MainForm.CheatGridMenuHexEditor;
                    CheatGridMenuLock.Text        = langJson.MainForm.CheatGridMenuLock;
                    CheatGridMenuUnlock.Text      = langJson.MainForm.CheatGridMenuUnlock;
                    CheatGridMenuActive.Text      = langJson.MainForm.CheatGridMenuActive;
                    CheatGridMenuEdit.Text        = langJson.MainForm.CheatGridMenuEdit;
                    CheatGridMenuCopyAddress.Text = langJson.MainForm.CheatGridMenuCopyAddress;
                    CheatGridMenuFindPointer.Text = langJson.MainForm.CheatGridMenuFindPointer;
                    CheatGridMenuDelete.Text      = langJson.MainForm.CheatGridMenuDelete;
                }
                UIFont = Properties.Settings.Default.UIFont.Value;
                Font = new Font(UIFont, Font.Size);
            }
            catch (Exception ex)
            {
                InputBox.MsgBox("Apply UI language Exception", "", ex.Message, 100);
            }
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

                ToolStripMsg.ForeColor                              = MainCheatGridCellForeColor; //Color.White;
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
                if (cheatGridRowList.Count < CheatGridGroupRefreshThreshold && CheatGridView.GroupByEnabled) CheatGridView.GroupRefresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Source + ":ApplyUI", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        #region Event
        private void Main_Shown(object sender, EventArgs e)
        {
            ProcessName         = "";
            bool isConnected    = false;
            string PS4FWVersion = Properties.Settings.Default.PS4FWVersion.Value ?? "";
            string PS4IP        = Properties.Settings.Default.PS4IP.Value ?? "";
            ushort PS4Port      = Properties.Settings.Default.PS4Port.Value;
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

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to close PS4CheaterNeo?", "PS4CheaterNeo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }
        }

        #region ToolStrip
        private void ToolStripSend_Click(object sender, EventArgs e)
        {
            if (sendPayload == null || sendPayload.IsDisposed) sendPayload = new SendPayload(this);
            sendPayload.StartPosition = FormStartPosition.CenterParent;
            sendPayload.TopMost = true;
            sendPayload.Show();
        }

        #region ToolStripOpenAndSave
        private const string dialogFilter = "Cheat (*.cht)|*.cht|Cheat Relative (*.chtr)|*.chtr|Cheat Json (*.json)|*.json|Cheat Shn (*.shn)|*.shn";
        private void ToolStripOpen_Click(object sender, EventArgs e)
        {
            try
            {
                OpenCheatDialog.Filter = 
                    "Cheat files(*.cht;*.chtr;*.json;*.shn;*.mc4;eboot.bin)|*.cht;*.chtr;*.json;*.shn;*.mc4;eboot.bin|" + dialogFilter + "|Cheat MC4 (*.mc4)|*.mc4|eboot.bin (eboot.bin)|*.bin";
                OpenCheatDialog.AddExtension = true;
                OpenCheatDialog.RestoreDirectory = true;

                if (OpenCheatDialog.ShowDialog() != DialogResult.OK) return;

                CheatGridView.SuspendLayout();
                int count = 0;
                using (StreamReader sr = new StreamReader(OpenCheatDialog.FileName))
                {
                    if (OpenCheatDialog.FileName.ToUpper().EndsWith("JSON")) count = ParseCheatJson(sr.BaseStream);
                    else if (OpenCheatDialog.FileName.ToUpper().EndsWith("SHN")) count = ParseCheatSHN(sr.ReadToEnd());
                    else if (OpenCheatDialog.FileName.ToUpper().EndsWith("MC4")) count = ParseCheatMC4(sr);
                    else if (OpenCheatDialog.FileName.ToUpper().EndsWith("BIN")) count = ParseExecutableBIN(sr.BaseStream);
                    else
                    {
                        string cheatTexts = sr.ReadToEnd();
                        string[] cheatStrArr = cheatTexts.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None); //File.ReadAllLines(OpenCheatDialog.FileName);
                        #region cheatHeaderItems Check
                        string[] cheatHeaderItems = cheatStrArr[0].Split('|');

                        ProcessName = cheatHeaderItems[1];
                        if (!InitSections(ProcessName)) return;

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

                        string[] pVers      = productVersion.Split('.'); //1.2.3.4 => 01020304
                        int pVerChk         = int.Parse(pVers[0]) * 1000000 + int.Parse(pVers[1]) * 10000 + int.Parse(pVers[2]) * 100 + int.Parse(pVers[3]);
                        bool isSIDv1        = pVerChk < 00090505;
                        bool isRelativeV1   = pVerChk < 00090507;
                        string cheatGameID  = cheatHeaderItems[2];
                        string cheatGameVer = cheatHeaderItems[3];
                        string cheatFWVer   = cheatHeaderItems[4];
                        string PS4FWVersion = Properties.Settings.Default.PS4FWVersion.Value ?? "";
                        string FWVer        = PS4FWVersion != "" ? PS4FWVersion : Constant.Versions[0];

                        if (!LoadCheatGameValid(cheatGameID, cheatGameVer)) return;
                        if (FWVer != cheatFWVer && MessageBox.Show(string.Format("Your Firmware version({0}) is different with cheat file({1}), still load?", FWVer, cheatFWVer),
                            "FWVer", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                        #endregion
                        Dictionary<ulong, ulong> pointerCaches = new Dictionary<ulong, ulong>();
                        (uint sid, string name, uint prot, uint offsetFirst, uint offsetAddr) preData = (0, "", 0, 0, 0);
                        CheatGridViewRowCountUpdate(false);
                        for (int idx = 1; idx < cheatStrArr.Length; ++idx)
                        {
                            string cheatStr = cheatStrArr[idx];

                            if (string.IsNullOrWhiteSpace(cheatStr)) continue;

                            string[] cheatElements = cheatStr.Split(new Char[] { '|' }, 10);

                            if (cheatElements.Length < 5) continue;

                            bool isRelative        = cheatElements[5].StartsWith("+");
                            bool isRelativeV1a     = cheatElements[5].StartsWith("++");
                            if (isRelativeV1a) cheatElements[5] = cheatElements[5].Substring(1);
                            bool isPointer         = !cheatElements[5].Contains("[") && Regex.IsMatch(cheatElements[5], "[0-9A-F]+_", RegexOptions.IgnoreCase);
                            int relativeOffset     = isRelative ? int.Parse(cheatElements[5].Substring(1), NumberStyles.HexNumber) : -1;
                            uint sid               = isRelative ? preData.sid : uint.Parse(cheatElements[0]);
                            string name            = isRelative ? preData.name : cheatElements[2];
                            uint prot              = isRelative ? preData.prot : uint.Parse(cheatElements[3], NumberStyles.HexNumber);
                            ScanType cheatScanType = this.ParseFromDescription<ScanType>(cheatElements[6]);
                            bool cheatLock         = bool.Parse(cheatElements[7]);
                            string cheatValue      = cheatElements[8];
                            string cheatDesc       = cheatElements[9];
                            string onValue         = null;
                            string offValue        = null;
                            if (Regex.Match(cheatDesc, @"(.*)\|([0-9A-Fa-f]+)\|([0-9A-Fa-f]+)") is Match m1 && m1.Success)
                            {
                                cheatDesc = m1.Groups[1].Value;
                                onValue   = m1.Groups[2].Value;
                                offValue  = m1.Groups[3].Value;
                            }

                            Section section = isSIDv1 ? sectionTool.GetSectionBySIDv1(sid, name, prot) : sectionTool.GetSection(sid, name, prot);
                            if (section == null && (ToolStripLockEnable.Checked || ToolStripAutoRefresh.Checked))
                            {
                                ToolStripLockEnable.Checked = false;
                                ToolStripAutoRefresh.Checked = false;
                            }

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

                            CheatRow row = AddToCheatGrid(section, offsetAddr, cheatScanType, cheatValue, cheatLock, cheatDesc, pointerOffsets, pointerCaches, isRelative ? (int)preData.offsetAddr : -1, false, onValue, offValue);
                            if (row.Cells == null || row.Cells.Count == 0)
                            {
                                preData = (0, "", 0, 0, 0);
                                continue;
                            }
                            if (isPointer)
                            {
                                (section, offsetAddr) = (row.Section_, row.OffsetAddr);
                                sid = section.SID;
                                name = section.Name;
                                prot = section.Prot;
                            }
                            if (!isRelative) preData = (sid, name, prot, offsetAddr, 0);
                            count++;
                        }
                        CheatGridViewRowCountUpdate();
                    }
                }
                if (cheatGridRowList.Count < CheatGridGroupRefreshThreshold && CheatGridView.GroupByEnabled) CheatGridView.GroupRefresh();
                CheatGridView.ResumeLayout();
                SaveCheatDialog.FileName = OpenCheatDialog.FileName;
                if (OpenCheatDialog.FilterIndex < 6) SaveCheatDialog.FilterIndex = OpenCheatDialog.FilterIndex;
                ToolStripMsg.Text = string.Format("Successfully loaded {0} cheat items.", count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Source + ":ToolStripOpen_Click", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        #region ParseLoadCheat
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

                    sequence         = int.Parse(cheatElements[2]);
                    offsetAddrStr    = cheatElements[3];
                    section          = sequence < sections.Length ? sections[sequence] : null;
                    string cheatCode = cheatElements[6];
                    cheatLockStr     = cheatElements[7];
                    cheatDesc        = cheatElements[8];
                    string[] datas   = cheatCode.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

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
                    string[] pointerList     = cheatElements[3].Split('+');
                    string[] addressElements = pointerList[0].Split('_');
                    sequence                 = int.Parse(addressElements[1]);
                    offsetAddrStr            = addressElements[2];
                    section                  = sequence < sections.Length ? sections[sequence] : null;
                    for (int offsetIdx       = 1; offsetIdx < pointerList.Length; ++offsetIdx) offsetAddrStr += "_" + pointerList[offsetIdx];
                    scanTypeStr              = cheatElements[5];
                    cheatValue               = cheatElements[6];
                    cheatLockStr             = cheatElements[7];
                    cheatDesc                = cheatElements[8];
                }
                else
                {
                    sequence      = int.Parse(cheatElements[1]);
                    offsetAddrStr = cheatElements[2];
                    section       = sequence < sections.Length ? sections[sequence] : null;
                    scanTypeStr   = cheatElements[3];
                    cheatValue    = cheatElements[4];
                    cheatLockStr  = cheatElements[5];
                    cheatDesc     = cheatElements[6];
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

        /// <summary>
        /// Load and parse Cheat files in the @batchcode format.
        /// </summary>
        /// <param name="data"></param>
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

        /// <summary>
        /// Load a Cheat file in JSON format.
        /// </summary>
        /// <returns></returns>
        private int ParseCheatJson(Stream stream)
        {
            int count = 0;
            using (stream)
            {
                DataContractJsonSerializer deseralizer = new DataContractJsonSerializer(typeof(CheatJson));
                cheatJson = (CheatJson)deseralizer.ReadObject(stream);

                ProcessName = cheatJson.Process;
                if (!InitSections(ProcessName)) return 0;
                if (!LoadCheatGameValid(cheatJson.Id, cheatJson.Version)) return 0;

                Section section;
                Section sectionFirst = sectionTool.GetSectionSortByAddr()[0];
                CheatGridViewRowCountUpdate(false);
                foreach (CheatJson.Mod cheatMod in cheatJson.Mods)
                {
                    bool cheatLock = false;
                    ScanType cheatScanType = ScanType.Hex;
                    section = sectionFirst;
                    for (int idx = 0; idx < cheatMod.Memory.Count; idx++)
                    {
                        CheatJson.Memory memory = cheatMod.Memory[idx];
                        string cheatDesc = cheatMod.Name;
                        if (cheatMod.Memory.Count > 1) cheatDesc += string.Format("_{0:00}", idx);

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

                        AddToCheatGrid(section, (uint)offsetAddr, cheatScanType, cheatValue, cheatLock, cheatDesc, null, null, -1, false, memory.On, memory.Off);
                        count++;
                    }
                }
                CheatGridViewRowCountUpdate();
            }
            return count;
        }

        private int ParseCheatSHN(string shnXML)
        {
            int count = 0;
            using (StringReader readerXml = new StringReader(shnXML))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CheatTrainer));
                cheatTrainer = (CheatTrainer)serializer.Deserialize(readerXml);

                ProcessName = cheatTrainer.Process;
                if (!InitSections(ProcessName)) return 0;
                if (!LoadCheatGameValid(cheatTrainer.Cusa, cheatTrainer.Version)) return 0;

                Section[] sections = sectionTool.GetSectionSortByAddr();
                CheatGridViewRowCountUpdate(false);
                foreach (CheatTrainer.Cheat startUP in cheatTrainer.StartUPs)
                {
                    ParseCheatline(startUP.Text, startUP.Description, startUP.Cheatlines, sections);
                    count++;
                }
                foreach (CheatTrainer.Cheat cheat in cheatTrainer.Cheats)
                {
                    ParseCheatline(cheat.Text, cheat.Description, cheat.Cheatlines, sections);
                    count++;
                }
                CheatGridViewRowCountUpdate();
            }
            void ParseCheatline(string text, string desc, List<CheatTrainer.Cheatline> cheatlines, Section[] sections)
            {
                bool cheatLock = false;
                ScanType cheatScanType = ScanType.Hex;
                for (int idx = 0; idx < cheatlines.Count; idx++)
                {
                    CheatTrainer.Cheatline cheatline = cheatlines[idx];
                    Section section = sections[cheatline.SectionId];
                    string onValue = cheatline.ValueOn.Replace("-", "");
                    string offValue = cheatline.ValueOff.Replace("-", "");
                    ulong offsetAddr = ulong.Parse(cheatline.Offset, NumberStyles.HexNumber);

                    string cheatDesc = text;
                    if (cheatlines.Count > 1) cheatDesc += string.Format("_{0:00}", idx);

                    AddToCheatGrid(section, (uint)offsetAddr, cheatScanType, onValue, cheatLock, cheatDesc, null, null, -1, false, onValue, offValue);
                }
            }
            return count;
        }

        private int ParseCheatMC4(StreamReader reader)
        {
            /// Decrypted by bucanero/save-decrypters.
            /// https://github.com/bucanero/save-decrypters
            byte[] aes256cbcKey = Encoding.ASCII.GetBytes("304c6528f659c766110239a51cl5dd9c");
            byte[] aes256cbcIv = Encoding.ASCII.GetBytes("u@}kzW2u[u(8DWar");

            int count = 0;
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                string raw = reader.ReadToEnd();
                byte[] rawData = Convert.FromBase64String(raw);
                var decryptor = aes.CreateDecryptor(aes256cbcKey, aes256cbcIv);
                byte[] decBytes = decryptor.TransformFinalBlock(rawData, 0, rawData.Length);
                string decXml = Encoding.UTF8.GetString(decBytes);
                decXml = HttpUtility.HtmlDecode(decXml);
                decXml = Regex.Unescape(decXml);
                Console.WriteLine(decXml);
                count = ParseCheatSHN(decXml); 
            }
            return count;
        }

        private int ParseExecutableBIN(Stream stream)
        {
            if (!InitSections(Properties.Settings.Default.DefaultProcess.Value)) return 0;

            bool isModified = MessageBox.Show("Could you please confirm if the loaded \"eboot.bin\" file belongs to a modified version?\r\n\r\nYes: Modified version\r\nNo: Original version\r\n\r\n" +
                "The loaded \"eboot.bin\" here will be compared with the section memory content of the game executable, and the differing memory data between the two will be obtained.\r\n" +
                "Therefore, you must choose to load an \"eboot.bin\" with the same game ID but differing from the content of the currently running game for the comparison to be valid.", "Load eboot.bin", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

            int count = 0;
            List<(int index, int length)> diffs = new List<(int index, int length)>();
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                byte[] ebootData = ms.ToArray();

                Self.Header seHead = Self.BytesToStruct<Self.Header>(ebootData);

                if (Self.SELF_MAGIC != seHead.magic && MessageBox.Show(String.Format("Invalid Self Magic! (0x{0:X8})\n", seHead.magic), "Load eboot.bin", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK) return 0;

                Self.Entry seEntry0 = new Self.Entry();
                for (ushort seIdx = 0; seIdx < seHead.numEntries; seIdx++)
                {
                    var entryOffset = Self.SizeSHdr + Self.SizeSEntry * seIdx;
                    Byte[] dataEntry = new Byte[Self.SizeSEntry];
                    Buffer.BlockCopy(ebootData, entryOffset, dataEntry, 0, dataEntry.Length);
                    seEntry0 = Self.BytesToStruct<Self.Entry>(dataEntry);
                    if (seEntry0.props >> 20 == 0) break;
                }

                if (seEntry0.fileSz == 0 && seEntry0.props >> 20 != 0 && MessageBox.Show("The information of Segment00 in the loaded \"eboot.bin\" file cannot be parsed.", "Load eboot.bin", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK) return 0;

                Section section = sectionTool.GetSection("executable", 5);
                byte[] checkBytes = PS4Tool.ReadMemory(ProcessPid, section.Start, 48);
                byte[] executableData = PS4Tool.ReadMemory(ProcessPid, section.Start, section.Length);

                int checkLength = executableData.Length > (int)seEntry0.memSz ? (int)seEntry0.memSz : executableData.Length;
                int notEqualIdx = -1;
                for (int idx = 0; idx < checkLength; idx++)
                {
                    if (ebootData[(int)seEntry0.offs + idx] == executableData[idx])
                    {
                        if (notEqualIdx != -1)
                        {
                            diffs.Add((notEqualIdx, idx - notEqualIdx));
                            notEqualIdx = -1;
                        }
                        continue;
                    }

                    if (notEqualIdx == -1) notEqualIdx = idx;
                }

                for (int idx = 0; idx < diffs.Count; idx++)
                {
                    (int index, int length) diff = diffs[idx];
                    (int index, int length) diff2 = idx + 1 < diffs.Count ? diffs[idx + 1] : (0, 0);
                    if (diff2.index > 0 && diff2.index - (diff.index + diff.length) < 10)
                    {
                        diffs[idx] = (diff.index, (diff2.index + diff2.length) - diff.index);
                        diffs.RemoveAt(idx + 1);
                        idx--;
                        continue;
                    }
                }
                CheatGridViewRowCountUpdate(false);
                for (int idx = 0; idx < diffs.Count; idx++)
                {
                    (int index, int length) diff = diffs[idx];
                    bool cheatLock = false;
                    string cheatDesc = string.Format("index{0:000}", idx);
                    ScanType cheatScanType = ScanType.Hex;

                    byte[] onValue = new byte[diff.length];
                    byte[] offValue = new byte[diff.length];
                    Buffer.BlockCopy(ebootData, (int)seEntry0.offs + diff.index, onValue, 0, diff.length);
                    Buffer.BlockCopy(executableData, diff.index, offValue, 0, diff.length);
                    string onStr = ScanTool.BytesToString(cheatScanType, onValue, true);
                    string offStr = ScanTool.BytesToString(cheatScanType, offValue, true);
                    if (isModified) AddToCheatGrid(section, (uint)diff.index, cheatScanType, onStr, cheatLock, cheatDesc, null, null, -1, false, onStr, offStr);
                    else AddToCheatGrid(section, (uint)diff.index, cheatScanType, offStr, cheatLock, cheatDesc, null, null, -1, false, offStr, onStr);
                    count++;
                }
                CheatGridViewRowCountUpdate();

            }
            return count;
        }

        private bool LoadCheatGameValid(string cheatGameID, string cheatGameVer)
        {
            InitGameInfo();

            if (GameID != cheatGameID && MessageBox.Show(string.Format("Your Game ID({0}) is different with cheat file({1}), still load?", GameID, cheatGameID),
                "GameID", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return false;
            if (GameVer != cheatGameVer && MessageBox.Show(string.Format("Your Game version({0}) is different with cheat file({1}), still load?", GameVer, cheatGameVer),
                "GameVer", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return false;
            
            return true;
        }
        #endregion

        private void ToolStripSave_Click(object sender, EventArgs e)
        {
            if (cheatGridRowList.Count == 0) return;

            string FWVer = Properties.Settings.Default.PS4FWVersion.Value;
            InitGameInfo();
            SaveCheatDialog.Filter = "Cheat files(*.cht;*.chtr;*.json;*.shn)|*.cht;*.chtr;*.json;*.shn|" + dialogFilter;
            SaveCheatDialog.AddExtension = true;
            SaveCheatDialog.RestoreDirectory = true;
            if (string.IsNullOrWhiteSpace(SaveCheatDialog.FileName))
            {
                if (string.IsNullOrWhiteSpace(ProcessContentid))
                {
                    try
                    {
                        var processInfo = PS4Tool.GetProcessInfo(ProcessName);
                        ProcessPid = processInfo.pid;
                        ProcessContentid = processInfo.contentid;
                    }
                    catch {}
                }
                SaveCheatDialog.FileName = string.IsNullOrWhiteSpace(ProcessContentid) ? GameID : ProcessContentid;
            }

            if (SaveCheatDialog.ShowDialog() != DialogResult.OK) return;
            if (!InitSections(ProcessName)) return;

            if (SaveCheatDialog.FileName.ToUpper().EndsWith("JSON")) SaveCheatJson();
            else if (SaveCheatDialog.FileName.ToUpper().EndsWith("SHN")) SaveCheatShn();
            else
            {
                string processName = ProcessName;
                StringBuilder saveBuf = new StringBuilder($"{Application.ProductVersion}|{processName}|{GameID}|{GameVer}|{FWVer}\n");
                uint maxRelative = 0x100;
                ulong preAddr = 0;
                for (int cIdx = 0; cIdx < cheatGridRowList.Count; cIdx++)
                {
                    CheatRow row = cheatGridRowList[cIdx];

                    (Section section, uint offsetAddr) = (row.Section_, row.OffsetAddr);
                    bool isRelative = preAddr > 0 && offsetAddr > preAddr && offsetAddr - preAddr <= maxRelative;
                    bool isPointer = row.Cells[(int)ChertCol.CheatListAddress].ToString().StartsWith("P->");
                    Section newSection = section;
                    string addressStr;
                    string sectionStr = "";
                    if (isPointer)
                    {
                        uint sid = row.Cells[(int)ChertCol.CheatListSID] == null ? 0 : (uint)row.Cells[(int)ChertCol.CheatListSID];
                        string[] sectionArr = row.Cells[(int)ChertCol.CheatListSection].ToString().Split('|');
                        newSection = sectionTool.GetSection(sid, sectionArr[1], uint.Parse(sectionArr[2]));
                        addressStr = sectionArr[sectionArr.Length - 1];
                    }
                    else if (SaveCheatDialog.FilterIndex == 3 && isRelative)
                    { //Cheat Relative (*.chtr)
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

                    saveBuf.Append(string.Format("{0}|{1}|{2}|{3}|{4}|{5}",
                        sectionStr,
                        addressStr,
                        row.Cells[(int)ChertCol.CheatListType],
                        row.Cells[(int)ChertCol.CheatListLock],
                        row.Cells[(int)ChertCol.CheatListValue],
                        row.Cells[(int)ChertCol.CheatListDesc]
                        ));
                    if (!string.IsNullOrWhiteSpace((string)row.Cells[(int)ChertCol.CheatListOn]))
                        saveBuf.AppendLine(string.Format("|{0}|{1}",
                            row.Cells[(int)ChertCol.CheatListOn],
                            row.Cells[(int)ChertCol.CheatListOff]
                            ));
                    else saveBuf.AppendLine();
                    if (SaveCheatDialog.FilterIndex == 3 && !isRelative) preAddr = offsetAddr; //Cheat Relative (*.chtr)
                }

                using (var myStream = new StreamWriter(SaveCheatDialog.FileName)) myStream.Write(saveBuf.ToString());
            }

            OpenCheatDialog.FileName = SaveCheatDialog.FileName;
            OpenCheatDialog.FilterIndex = SaveCheatDialog.FilterIndex;
        }

        private void SaveCheatJson()
        {
            if (cheatJson == null || cheatJson.Id != GameID) cheatJson = new CheatJson(ProcessContentid, GameID, GameVer, ProcessName);
            else cheatJson.Mods = new List<CheatJson.Mod>();

            CheatJson.Mod modBak = null;
            Section sectionFirst = sectionTool.GetSectionSortByAddr()[0];
            for (int cIdx = 0; cIdx < cheatGridRowList.Count; cIdx++)
            {
                CheatRow row = cheatGridRowList[cIdx];

                (Section section, ulong offsetAddr) = (row.Section_, row.OffsetAddr);
                if (section.SID != sectionFirst.SID) offsetAddr += section.Start - sectionFirst.Start;

                string cheatDesc = row.Cells[(int)ChertCol.CheatListDesc].ToString();
                ScanType scanType = this.ParseFromDescription<ScanType>(row.Cells[(int)ChertCol.CheatListType].ToString());
                string on = row.Cells[(int)ChertCol.CheatListValue].ToString();
                if (scanType != ScanType.Hex)
                {
                    byte[] bytes = ScanTool.ValueStringToByte(scanType, on);
                    on = ScanTool.BytesToString(scanType, bytes, true, on.StartsWith("-"));
                    on = ScanTool.ReverseHexString(on);
                }
                string off = string.IsNullOrWhiteSpace((string)row.Cells[(int)ChertCol.CheatListOff]) ? on : row.Cells[(int)ChertCol.CheatListOff].ToString();
                if (Regex.Match(cheatDesc, @"(.*) *__ *\[ *on: *([0-9a-zA-Z]+) *off: *([0-9a-zA-Z]+)") is Match m1 && m1.Success)
                { //Attempt to restore off value from desc
                    cheatDesc = m1.Groups[1].Value;
                    off = m1.Groups[3].Value;
                }
                cheatDesc = Regex.Replace(cheatDesc, @"_\d+$", "");

                if (modBak != null && modBak.Name == cheatDesc) cheatJson.Mods[cheatJson.Mods.Count - 1].Memory.Add(new CheatJson.Memory(offsetAddr.ToString("X"), on, off));
                else
                {
                    CheatJson.Mod mod = new CheatJson.Mod(cheatDesc, "checkbox");
                    mod.Memory.Add(new CheatJson.Memory(offsetAddr.ToString("X"), on, off));

                    cheatJson.Mods.Add(mod);
                    modBak = mod;
                }
            }

            using (var myStream = new FileStream(SaveCheatDialog.FileName, FileMode.Create))
            using (var writer = JsonReaderWriterFactory.CreateJsonWriter(myStream, Encoding.UTF8, true, true, "  "))
            {
                var serializer = new DataContractJsonSerializer(typeof(CheatJson));
                serializer.WriteObject(writer, cheatJson);
                writer.Flush();
            }
        }

        private void SaveCheatShn()
        {
            if (cheatTrainer == null || cheatTrainer.Cusa != GameID) cheatTrainer = new CheatTrainer(ProcessContentid, GameID, GameVer, ProcessName);
            else
            {
                cheatTrainer.StartUPs = new List<CheatTrainer.Cheat>();
                cheatTrainer.Cheats = new List<CheatTrainer.Cheat>();
                cheatTrainer.Genress = new List<CheatTrainer.Genres>();
            }


            CheatTrainer.Cheat cheatBak = null;
            Section[] sections = sectionTool.GetSectionSortByAddr();
            for (int idx = 0; idx < sections.Length; idx++)
            {
                Section section = sections[idx];
                section.SN = idx;
            }
            for (int cIdx = 0; cIdx < cheatGridRowList.Count; cIdx++)
            {
                CheatRow row = cheatGridRowList[cIdx];

                (Section section, ulong offsetAddr) = (row.Section_, row.OffsetAddr);

                string cheatDesc = row.Cells[(int)ChertCol.CheatListDesc].ToString();
                ScanType scanType = this.ParseFromDescription<ScanType>(row.Cells[(int)ChertCol.CheatListType].ToString());
                string on = row.Cells[(int)ChertCol.CheatListValue].ToString();
                if (scanType != ScanType.Hex)
                {
                    byte[] bytes = ScanTool.ValueStringToByte(scanType, on);
                    on = ScanTool.BytesToString(scanType, bytes, true, on.StartsWith("-"));
                    on = ScanTool.ReverseHexString(on);
                }
                string off = row.Cells[(int)ChertCol.CheatListOff].ToString();
                if (Regex.Match(cheatDesc, @"(.*) *__ *\[ *on: *([0-9a-zA-Z]+) *off: *([0-9a-zA-Z]+)") is Match m1 && m1.Success)
                { //Attempt to restore off value from desc
                    cheatDesc = m1.Groups[1].Value;
                    off = m1.Groups[3].Value;
                }
                on = Regex.Replace(on, @"(\w\w)(?=\w)", @"$1-");
                off = Regex.Replace(off, @"(\w\w)(?=\w)", @"$1-");
                cheatDesc = Regex.Replace(cheatDesc, @"_\d+$", "");

                if (cheatBak != null && cheatBak.Text == cheatDesc) cheatTrainer.Cheats[cheatTrainer.Cheats.Count - 1].Cheatlines.Add(new CheatTrainer.Cheatline(offsetAddr.ToString("X"), section.SN, on, off));
                else
                {
                    CheatTrainer.Cheat cheat = new CheatTrainer.Cheat(cheatDesc);
                    cheat.Cheatlines.Add(new CheatTrainer.Cheatline(offsetAddr.ToString("X"), section.SN, on, off));

                    cheatTrainer.Cheats.Add(cheat);
                    cheatBak = cheat;
                }
            }
            XmlSerializer serializer = new XmlSerializer(typeof(CheatTrainer));
            using (var myStream = new StreamWriter(SaveCheatDialog.FileName))
            using (XmlTextWriter xmlWriter = new XmlTextWriter(myStream))
            {
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.Indentation = 2;
                serializer.Serialize(xmlWriter, cheatTrainer);
            }
        }
        #endregion

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
                if (ex.Message == "No Process currently")
                {
                    InputBox.MsgBox("ToolStripAdd_Click", ex.Message, "Process isn't connected. Please connect first");
                    return;
                }
                MessageBox.Show(ex.ToString(), ex.Source + ":ToolStripAdd_Click", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

                DataGridViewSelectedRowCollection rows = CheatGridView.SelectedRows;
                if (rows.Count == 1)
                {
                    var cheatRow = rows[0];
                    (section, offsetAddr) = ((Section section, uint offsetAddr))cheatRow.Tag;
                    inputValue = (section.Start + offsetAddr).ToString("X");
                }

                if ((ProcessName ?? "") == "") throw new Exception("No Process currently");
                if (!InitSections(ProcessName)) throw new Exception(String.Format("Process({0}): InitSections failed", ProcessName));

                if (InputBox.Show("Hex View", "Please enter the memory address(hex) you want to view", ref inputValue) != DialogResult.OK) return;

                inputValue = Regex.Replace(inputValue, "[^0-9a-fA-F]", "");
                ulong address = ulong.Parse(inputValue, NumberStyles.HexNumber);
                uint sid = sectionTool.GetSectionID(address);
                if (sid == 0) return; //-1(int) => 0(uint)

                section = sectionTool.GetSection(sid);
                offsetAddr = (uint)(address - section.Start);

                HexEditor hexEdit = new HexEditor(this, sectionTool, section, (int)offsetAddr);
                hexEdit.Show(this);
            }
            catch (Exception ex)
            {
                if (ex.Message == "No Process currently")
                {
                    InputBox.MsgBox("ToolStripHexView_Click", ex.Message, "Process isn't connected. Please connect first");
                    return;
                }
                MessageBox.Show(ex.ToString(), ex.Source + ":ToolStripHexView_Click", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void ToolStripRefreshCheat_Click(object sender, EventArgs e)
        {
            if (cheatGridRowList.Count == 0 || (refreshCheatTask != null && !refreshCheatTask.IsCompleted)) return;

            refreshCheatSource = new CancellationTokenSource();
            refreshCheatTask = RefreshCheatTask(true);
            refreshCheatTask.ContinueWith(t => {
                refreshCheatSource?.Dispose();
                refreshCheatSource = null;
                refreshCheatTask?.Dispose();
                refreshCheatTask = null;
            });
            ToolStripMsg.Text = string.Format("{0:000}%, Refresh Cheat finished.", 100);
        }

        private void ToolStripRefreshCheat_DoubleClick(object sender, EventArgs e)
        {
            if (cheatGridRowList.Count == 0) return;
            if (refreshCheatTask != null && !refreshCheatTask.IsCompleted) refreshCheatSource.Cancel();
        }

        private void ToolStripLockEnable_Click(object sender, EventArgs e)
        {
            ToolStripLockEnable.Checked = !ToolStripLockEnable.Checked;
            if (refreshLockSource != null && !ToolStripLockEnable.Checked) refreshLockSource.Cancel();
        }

        private void ToolStripLockEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (ToolStripLockEnable.Image != null) ToolStripLockEnable.Image.Dispose();
            ToolStripLockEnable.Image = CheckBoxImage(ToolStripLockEnable.Checked);
        }

        private void ToolStripAutoRefresh_Click(object sender, EventArgs e)
        {
            ToolStripAutoRefresh.Checked = !ToolStripAutoRefresh.Checked;
            if (refreshCheatSource != null && !ToolStripAutoRefresh.Checked) refreshCheatSource.Cancel();
        }

        private void ToolStripAutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            if (ToolStripAutoRefresh.Image != null) ToolStripAutoRefresh.Image.Dispose();
            ToolStripAutoRefresh.Image = CheckBoxImage(ToolStripAutoRefresh.Checked);

            if (!ToolStripAutoRefresh.Checked) AutoRefreshTimer.Stop();
            AutoRefreshTimer.Enabled = ToolStripAutoRefresh.Checked;
        }

        /// <summary>
        /// Get the Image of CheckBoxes with the status of Checked or Unchecked.
        /// </summary>
        /// <param name="isChecked">Checked or Unchecked</param>
        /// <returns>Image of CheckBoxes</returns>
        private Image CheckBoxImage(bool isChecked)
        {
            Image image = null;
            byte[] imageBytes = isChecked ?
                Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAvElEQVR4Xr3ToQoCMRzH8Z8ytFy66DOIPocyBc/iExhE8FUMYhCLGBTBM7hX0KbJqGDUYLqwOPmHgzGO222Cn7zvj4WtpJSClFLBE0sDzjlcCSHAoIn3BxQV9XsgZfj788Djc0d32cL5eYKOFY0n8RhhNUSj1rTf4J28MuP5YIGgEuTfYHfdYH1ZYRrNQDLi/IF2vQNxO1IIYsT2ATpIwWg7BNFi+4A5QtLYZUAL7Zj5PF0x/WP4oO+MX3wBcd48+jpQHgAAAAAASUVORK5CYII=") :
                Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAARUlEQVR4Xu3SoRGAMBAF0Q1z5Z0J1AmYX9tZGGYQWL6KyCtg1baquvARAJmJQxLB69hP/li3zmPBN1BgBmYgPm/7AUm4bkJfDRuTAXI3AAAAAElFTkSuQmCC");
            using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length)) image = Image.FromStream(ms, true);
            return image;
        }
        #endregion

        #region Task
        Task<bool> refreshCheatTask;
        CancellationTokenSource refreshCheatSource = null;
        private void AutoRefreshTimer_Tick(object sender, EventArgs e)
        {
            if (!(ToolStripProcessInfo.Tag is bool) && ProcessName != (string)ToolStripProcessInfo.Tag)
            {
                processInfo.name = ProcessName == "" ? "Empty" : (ProcessName + (ProcessPid > 0 ? "(" + ProcessPid + ")" : ""));
                ToolStripProcessInfo.Tag = ProcessName;
                ToolStripProcessInfo.Text = processInfo.msg + processInfo.name;
            }
            if (!ToolStripAutoRefresh.Checked || cheatGridRowList.Count == 0 || (refreshCheatTask != null && !refreshCheatTask.IsCompleted)) return;

            if (refreshCheatTask != null) refreshCheatTask.Dispose();

            if (ToolStripProcessInfo.Tag is bool && !(bool)ToolStripProcessInfo.Tag)
            {
                ToolStripAutoRefresh.Checked = false;
                ToolStripLockEnable.Checked = false;
                return;
            }

            refreshCheatSource = new CancellationTokenSource();
            refreshCheatTask = RefreshCheatTask();
            refreshCheatTask.ContinueWith(t => {
                refreshCheatSource?.Dispose();
                refreshCheatSource = null;
                refreshCheatTask?.Dispose();
                refreshCheatTask = null;
            });
        }

        private async Task<bool> RefreshCheatTask(bool isShowStatus = false) => await Task.Run(() => {
            bool isForceInitSections = DateTime.Now.Second % 3 == 0;
            System.Diagnostics.Stopwatch tickerMajor = System.Diagnostics.Stopwatch.StartNew();
            if (!InitSections(ProcessName, isForceInitSections)) return false;
            if (isForceInitSections)
            {
                VerifySectionWhenRefresh = Properties.Settings.Default.VerifySectionWhenRefresh.Value;
                CheatAutoRefreshShowStatus = Properties.Settings.Default.CheatAutoRefreshShowStatus.Value;
                AutoRefreshTimer.Interval = (int)Properties.Settings.Default.CheatAutoRefreshTimerInterval.Value;
            }

            (uint sid, string name, uint prot, uint offsetAddr) preData = (0, "", 0, 0);
            Dictionary<ulong, ulong> pointerCaches = new Dictionary<ulong, ulong>();
            int processID = -1;
            Dictionary<uint, (Section Section_, uint MinOffset, uint MaxOffset, List<(uint OffsetAddr, int Length, int CIDX, ScanType ScanType, bool IsSign)> CheatList)> cheatsDict =
                new Dictionary<uint, (Section Section_, uint MinOffset, uint MaxOffset, List<(uint OffsetAddr, int Length, int CIDX, ScanType ScanType, bool IsSign)> CheatList)>();
            List<(uint OffsetAddr, int Length, int CIDX, ScanType ScanType, bool IsSign)> CheatList = new List<(uint OffsetAddr, int Length, int CIDX, ScanType ScanType, bool IsSign)>();
            for (int cIdx = 0; cIdx < cheatGridRowList.Count; cIdx++)
            {
                refreshCheatSource.Token.ThrowIfCancellationRequested();
                try
                {
                    if (cIdx % 1000 == 0 && (CheatAutoRefreshShowStatus || isShowStatus))
                    {
                        Invoke(new MethodInvoker(() => {
                            ToolStripMsg.Text = string.Format("{0:000}%, Init Refresh Cheat elapsed:{1:0.00}s. {2}/{3}", (int)(((float)(cIdx + 1) / cheatGridRowList.Count) * 100), tickerMajor.Elapsed.TotalSeconds, cIdx + 1, cheatGridRowList.Count);
                        }));
                    }

                    CheatRow row = cheatGridRowList[cIdx];
                    (Section section, uint offsetAddr) = (row.Section_, row.OffsetAddr);
                    #region  Refresh Section and offsetAddr
                    if (VerifySectionWhenRefresh)
                    {
                        uint sid = row.Cells[(int)ChertCol.CheatListSID] == null ? 0 : (uint)row.Cells[(int)ChertCol.CheatListSID];
                        string[] sectionArr = row.Cells[(int)ChertCol.CheatListSection].ToString().Split('|');
                        string hexAddr = "";
                        bool isRelative = sectionArr[0].StartsWith("+");
                        bool isPointer = row.Cells[(int)ChertCol.CheatListAddress].ToString().StartsWith("P->");
                        if (isRelative && preData.sid == 0) section = null;
                        else (section, offsetAddr, hexAddr) = RefreshSection(sid, sectionArr, offsetAddr, isPointer, preData, pointerCaches);
                        if (section == null)
                        {
                            Invoke(new MethodInvoker(() =>
                            {
                                ToolStripMsg.Text = string.Format("RefreshCheat Failed...CheatGrid({0}), section: {1}, offsetAddr: {2:X}", cIdx, string.Join("-", sectionArr), offsetAddr);
                                if (ToolStripLockEnable.Checked || ToolStripAutoRefresh.Checked)
                                {
                                    ToolStripLockEnable.Checked = false;
                                    ToolStripAutoRefresh.Checked = false;
                                }
                            }));
                            row.ForeColor = Color.Red;
                            preData = (0, "", 0, 0);
                            continue;
                        }
                        else if (row.ForeColor == Color.Red) row.ForeColor = default;
                        if (!isRelative) preData = (section.SID, section.Name, section.Prot, offsetAddr);
                        row.Cells[(int)ChertCol.CheatListAddress] = hexAddr;
                    }
                    #endregion
                    ScanType scanType = this.ParseFromDescription<ScanType>(row.Cells[(int)ChertCol.CheatListType].ToString());
                    bool isSign = row.IsSign;
                    int scanTypeLength = 0;
                    if (scanType != ScanType.Hex && scanType != ScanType.String_) ScanTool.ScanTypeLengthDict.TryGetValue(scanType, out scanTypeLength);
                    else scanTypeLength = ScanTool.ValueStringToByte(scanType, row.Cells[(int)ChertCol.CheatListValue].ToString()).Length;

                    if (processID == -1) processID = section.PID;

                    if (!cheatsDict.TryGetValue(section.SID, out (Section Section_, uint MinOffset, uint MaxOffset, List<(uint OffsetAddr, int Length, int CIDX, ScanType ScanType, bool IsSign)> CheatList) cheatData))
                        cheatData.CheatList = new List<(uint OffsetAddr, int Length, int CIDX, ScanType ScanType, bool IsSign)>();
                    cheatData.Section_ = section;
                    if (cheatData.MinOffset > offsetAddr || cheatData.CheatList.Count == 0) cheatData.MinOffset = offsetAddr;
                    if (cheatData.MaxOffset < offsetAddr + (uint)scanTypeLength) cheatData.MaxOffset = offsetAddr + (uint)scanTypeLength;
                    cheatData.CheatList.Add((offsetAddr, scanTypeLength, cIdx, scanType, isSign));
                    cheatsDict[section.SID] = cheatData;
                    (row.Section_, row.OffsetAddr) = (section, offsetAddr);
                }
                catch (Exception) { preData = (0, "", 0, 0); }
            }

            if (ProcessPid == 0) return false;

            CheatBatchReadMemory(processID, cheatsDict, isShowStatus, tickerMajor);

            if (CheatAutoRefreshShowStatus || isShowStatus)
            {
                Invoke(new MethodInvoker(() => {
                    ToolStripMsg.Text = string.Format("{0:000}%, Refresh Cheat elapsed:{1:0.00}s", (int)(((float)cheatGridRowList.Count / cheatGridRowList.Count) * 100), tickerMajor.Elapsed.TotalSeconds);
                }));
            }

            return true;
        });

        /// <summary>
        /// Reload and update Section contents based on sid, name, and prot as conditions.
        /// </summary>
        /// <param name="sectionSid">section sid</param>
        /// <param name="sectionArr">The CheatListSection field in CheatGridView records information about the Section.</param>
        /// <param name="offsetAddr">The Tag of CheatGridView's Rows records the Section and Offset Address.</param>
        /// <param name="isPointer">Verify if it's a pointer.</param>
        /// <param name="preData">Save the previous data, which includes sid, name, prot, and offsetAddr.</param>
        /// <param name="pointerCaches">memory caches for pointer</param>
        /// <returns></returns>
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
        CancellationTokenSource refreshLockSource = null;
        private void RefreshLock_Tick(object sender, EventArgs e)
        {
            if (!(ToolStripProcessInfo.Tag is bool) && ProcessName != (string)ToolStripProcessInfo.Tag)
            {
                processInfo.name = ProcessName == "" ? "Empty" : (ProcessName + (ProcessPid > 0 ? "(" + ProcessPid + ")" : ""));
                ToolStripProcessInfo.Tag = ProcessName;
                ToolStripProcessInfo.Text = processInfo.msg + processInfo.name;
            }
            if (!ToolStripLockEnable.Checked || cheatGridRowList.Count == 0 || (refreshLockTask != null && !refreshLockTask.IsCompleted)) return;

            if (refreshLockTask != null) refreshLockTask.Dispose();

            if (ToolStripProcessInfo.Tag is bool && !(bool)ToolStripProcessInfo.Tag)
            {
                ToolStripAutoRefresh.Checked = false;
                ToolStripLockEnable.Checked = false;
                return;
            }

            refreshLockSource = new CancellationTokenSource();
            refreshLockTask = RefreshLockTask();
            refreshLockTask.ContinueWith(t => {
                refreshLockSource?.Dispose();
                refreshLockSource = null;
                refreshLockTask?.Dispose();
                refreshLockTask = null;
            });
        }

        private async Task<bool> RefreshLockTask() => await Task.Run(() => {
            bool isInitSections = false;
            if (DateTime.Now.Second % 5 == 0 && DateTime.Now.Millisecond < 500)
            {
                if (!InitSections(ProcessName)) return false;

                isInitSections = true;
                VerifySectionWhenLock = Properties.Settings.Default.VerifySectionWhenLock.Value;
            }

            (uint sid, string name, uint prot, uint offsetAddr) preData = (0, "", 0, 0);
            Dictionary<ulong, ulong> pointerCaches = new Dictionary<ulong, ulong>();
            int processID = -1;
            List<(ulong address, byte[] data)> writeData = new List<(ulong address, byte[] data)>();
            for (int cIdx = 0; cIdx < cheatGridRowList.Count; cIdx++)
            {
                refreshLockSource.Token.ThrowIfCancellationRequested();
                try
                {
                    CheatRow row = cheatGridRowList[cIdx];
                    if ((bool)row.Cells[(int)ChertCol.CheatListLock] == false)
                    {
                        preData = (0, "", 0, 0);
                        continue;
                    }
                    (Section section, uint offsetAddr) = (row.Section_, row.OffsetAddr);
                    #region Refresh Section and offsetAddr
                    if (isInitSections && VerifySectionWhenLock)
                    {
                        string hexAddr;
                        uint sid = row.Cells[(int)ChertCol.CheatListSID] == null ? 0 : (uint)row.Cells[(int)ChertCol.CheatListSID];
                        string[] sectionArr = row.Cells[(int)ChertCol.CheatListSection].ToString().Split('|');
                        bool isRelative = sectionArr[0].StartsWith("+");
                        bool isPointer = row.Cells[(int)ChertCol.CheatListAddress].ToString().StartsWith("P->");
                        if (isRelative && preData.sid == 0)
                        {
                            for (int idx = cIdx - 1; idx >= 0; idx--)
                            {
                                CheatRow checkRow = cheatGridRowList[idx];
                                uint checkSid = checkRow.Cells[(int)ChertCol.CheatListSID] == null ? 0 : (uint)checkRow.Cells[(int)ChertCol.CheatListSID];
                                string[] checkSArr = checkRow.Cells[(int)ChertCol.CheatListSection].ToString().Split('|');
                                if (checkSArr[0].StartsWith("+")) continue;

                                (Section section, uint offsetAddr) checkCheat = (checkRow.Section_, checkRow.OffsetAddr);
                                bool checkPointer = checkRow.Cells[(int)ChertCol.CheatListAddress].ToString().StartsWith("P->");
                                (Section section, uint offsetAddr, string hexAddr) checkSection = RefreshSection(checkSid, checkSArr, checkCheat.offsetAddr, checkPointer, preData, pointerCaches);
                                if (checkSection.section == null)
                                {
                                    ToolStripMsg.Text = string.Format("RefreshLock Failed...CheatGrid({0}), section: {1}, offsetAddr: {2:X}", cIdx, string.Join("-", sectionArr), offsetAddr);
                                    row.ForeColor = Color.Red;
                                    cheatGridRowList[idx] = row;
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
                            row.ForeColor = Color.Red;
                            preData = (0, "", 0, 0);
                            continue;
                        }
                        else if (row.ForeColor == Color.Red) row.ForeColor = default;
                        if (!isRelative) preData = (section.SID, section.Name, section.Prot, offsetAddr);
                        (row.Section_, row.OffsetAddr) = (section, offsetAddr);
                        row.Cells[(int)ChertCol.CheatListAddress] = hexAddr;
                    }
                    #endregion
                    ScanType scanType = this.ParseFromDescription<ScanType>(row.Cells[(int)ChertCol.CheatListType].ToString());
                    byte[] newData = ScanTool.ValueStringToByte(scanType, row.Cells[(int)ChertCol.CheatListValue].ToString());

                    if (processID == -1) processID = section.PID;
                    writeData.Add((offsetAddr + section.Start, newData));
                }
                catch (Exception) { preData = (0, "", 0, 0); }
            }

            if (ProcessPid == 0) return false;

            CheatBatchWriteMemory(10, processID, writeData, true);

            return true;
        });

        /// <summary>
        /// After splitting the readData, which contains memory positions and lengths, 
        /// into multiple chunks based on chunkSize, read PS4 memory content in batches. 
        /// Then, using the corresponding cheatList (which includes index values and scan type information), 
        /// update the newly read values in the CheatGridView.
        /// </summary>
        /// <param name="chunkSize">Size of each chunk</param>
        /// <param name="processID">specified process PID</param>
        /// <param name="cheatsDict">cheatsDict contains memory positions and lengths and index values and scan type information</param>
        /// <param name="isShowStatus">Whether to display status messages.</param>
        /// <param name="tickerMajor"></param>
        private void CheatBatchReadMemory(int processID, Dictionary<uint, (Section Section_, uint MinOffset, uint MaxOffset, List<(uint OffsetAddr, int Length, int CIDX, ScanType ScanType, bool IsSign)> CheatList)> cheatsDict,
            bool isShowStatus, System.Diagnostics.Stopwatch tickerMajor)
        {
            int count = 0;
            int firstDisplayedScrollingRowIndex = CheatGridView.FirstDisplayedScrollingRowIndex;
            bool isRefreshFinish = false;
            foreach ((Section Section_, uint MinOffset, uint MaxOffset, List<(uint OffsetAddr, int Length, int CIDX, ScanType ScanType, bool IsSign)> CheatList) cheat in cheatsDict.Values)
            {
                refreshCheatSource.Token.ThrowIfCancellationRequested();
                byte[] newDatas = PS4Tool.ReadMemory(processID, cheat.Section_.Start + cheat.MinOffset, (int)cheat.MaxOffset - (int)cheat.MinOffset);
                for (int idx = 0; idx < cheat.CheatList.Count; idx++)
                {
                    count++;
                    refreshCheatSource.Token.ThrowIfCancellationRequested();
                    (uint OffsetAddr, int Length, int CIDX, ScanType ScanType, bool IsSign) = cheat.CheatList[idx];
                    Byte[] newData = new byte[Length];
                    Buffer.BlockCopy(newDatas, (int)OffsetAddr - (int)cheat.MinOffset, newData, 0, Length);
                    Invoke(new MethodInvoker(() =>
                    {
                        cheatGridRowList[CIDX].Cells[(int)ChertCol.CheatListValue] = ScanTool.BytesToString(ScanType, newData, false, IsSign);
                        if (idx % 1000 == 0 && (CheatAutoRefreshShowStatus || isShowStatus))
                        {
                            if (idx % 50000 == 0)
                            {
                                if (firstDisplayedScrollingRowIndex != CheatGridView.FirstDisplayedScrollingRowIndex) isRefreshFinish = false;
                                if (!isRefreshFinish && CIDX > CheatGridView.FirstDisplayedScrollingRowIndex)
                                {
                                    CheatGridView.Refresh();
                                    isRefreshFinish = true;
                                }
                                firstDisplayedScrollingRowIndex = CheatGridView.FirstDisplayedScrollingRowIndex;
                            }
                            ToolStripMsg.Text = string.Format("{0:000}%, Refresh Cheat elapsed:{1:0.00}s. {2}/{3}",
                            (int)(((float)count / cheatGridRowList.Count) * 100), tickerMajor.Elapsed.TotalSeconds, count, cheatGridRowList.Count);
                        }
                    }));
                }
                Invoke(new MethodInvoker(() =>
                {
                    if (CheatAutoRefreshShowStatus || isShowStatus)
                    {
                        CheatGridView.Refresh();
                        ToolStripMsg.Text = string.Format("{0:000}%, Refresh Cheat elapsed:{1:0.00}s. {2}/{3}",
                        (int)(((float)count / cheatGridRowList.Count) * 100), tickerMajor.Elapsed.TotalSeconds, count, cheatGridRowList.Count);
                    }
                }));
            }
        }

        /// <summary>
        /// Split the writeData containing memory locations and bytes into multiple chunks based on chunkSize and write them into PS4 memory in batches.
        /// </summary>
        /// <param name="chunkSize">Size of each chunk</param>
        /// <param name="processID">specified process PID</param>
        /// <param name="writeData">writeData containing memory locations and bytes</param>
        private void CheatBatchWriteMemory(int chunkSize, int processID, List<(ulong address, byte[] data)> writeData, bool isExceptionThrow = false)
        {
            List<List<(ulong address, byte[] data)>> writeDataList = SplitList(writeData, chunkSize);

            for (int idx = 0; idx < writeDataList.Count; idx++)
            {
                refreshLockSource.Token.ThrowIfCancellationRequested();
                try
                {
                    List<(ulong address, byte[] data)> subWriteData = writeDataList[idx];
                    PS4Tool.WriteMemory(processID, subWriteData.ToArray());
                }
                catch (Exception) { if (isExceptionThrow) throw; }
            }
        }

        /// <summary>
        /// Split the list into N sublists based on the chunkSize.
        /// </summary>
        /// <param name="source">Source list</param>
        /// <param name="chunkSize">Size of each chunk</param>
        /// <returns></returns>
        private List<List<T>> SplitList<T>(List<T> source, int chunkSize)
        {
            var subLists = new List<List<T>>();

            for (int idx = 0; idx < source.Count; idx += chunkSize)
            {
                int remainingCount = source.Count - idx;
                int currentChunkSize = Math.Min(chunkSize, remainingCount);

                List<T> chunk = source.GetRange(idx, currentChunkSize);
                subLists.Add(chunk);
            }

            return subLists;
        }
        #endregion

        #region CheatGridView
        private void CheatGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (cheatGridRowList.Count <= e.RowIndex) return;

            CheatRow viewRow = cheatGridRowList[e.RowIndex];
            if (e.ColumnIndex == 0)
            {
                DataGridViewRow cheatRow = CheatGridView.Rows[e.RowIndex];
                cheatRow.Tag = (viewRow.Section_, viewRow.OffsetAddr);
                cheatRow.DefaultCellStyle.ForeColor = viewRow.ForeColor;
            }

            e.Value = viewRow.Cells[e.ColumnIndex];
        }

        private void CheatGridView_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            if (cheatGridRowList.Count <= e.RowIndex) return;
            CheatRow viewRow = cheatGridRowList[e.RowIndex];

            viewRow.Cells[e.ColumnIndex] = e.Value;
            cheatGridRowList[e.RowIndex] = viewRow;
        }
        private void CheatGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var format = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
            var bounds = new Rectangle(e.RowBounds.X + 16, e.RowBounds.Top, 48, e.RowBounds.Height);
            e.Graphics.DrawString(e.RowIndex.ToString(), Font, cheatGridViewRowIndexForeBrush, bounds, format);
        }

        private void CheatGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (e.RowIndex > cheatGridRowList.Count - 1) return;

            try
            {
                CheatRow row = cheatGridRowList[e.RowIndex];
                switch (e.ColumnIndex)
                {
                    case (int)ChertCol.CheatListEnabled:
                        CheatGridView.EndEdit();
                        (Section section, uint offsetAddr) = (row.Section_, row.OffsetAddr);
                        ScanType scanType = this.ParseFromDescription<ScanType>(row.Cells[(int)ChertCol.CheatListType].ToString());
                        byte[] data = ScanTool.ValueStringToByte(scanType, row.Cells[(int)ChertCol.CheatListValue].ToString());
                        PS4Tool.WriteMemory(section.PID, offsetAddr + section.Start, data);
                        break;
                    case (int)ChertCol.CheatListDel:
                        CheatGridView.SuspendLayout();
                        cheatGridRowList.RemoveAt(e.RowIndex);
                        CheatGridView.RowCount = cheatGridRowList.Count;
                        CheatGridView.Refresh();
                        if (cheatGridRowList.Count < CheatGridGroupRefreshThreshold && CheatGridView.GroupByEnabled) CheatGridView.GroupRefresh();
                        CheatGridView.SuspendLayout();
                        break;
                    case (int)ChertCol.CheatListLock:
                        CheatGridView.EndEdit();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Source + ":CheatGridView_CellContentClick", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void CheatGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex != (int)ChertCol.CheatListValue) return;
                if (e.RowIndex > cheatGridRowList.Count - 1) return;

                CheatRow editedRow = cheatGridRowList[e.RowIndex];
                ScanType scanType = this.ParseFromDescription<ScanType>(editedRow.Cells[(int)ChertCol.CheatListType].ToString());
                ScanTool.ValueStringToULong(scanType, (string)e.FormattedValue);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                MessageBox.Show(ex.ToString(), ex.Source + ":CellValidating", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void CheatGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != (int)ChertCol.CheatListValue) return;
            if (e.RowIndex > cheatGridRowList.Count - 1) return;

            try
            {
                CheatRow editedRow = cheatGridRowList[e.RowIndex];
                (Section section, uint offsetAddr) = (editedRow.Section_, editedRow.OffsetAddr);
                ScanType scanType = this.ParseFromDescription<ScanType>(editedRow.Cells[(int)ChertCol.CheatListType].ToString());
                byte[] data = ScanTool.ValueStringToByte(scanType, editedRow.Cells[(int)ChertCol.CheatListValue].ToString());
                PS4Tool.WriteMemory(section.PID, offsetAddr + section.Start, data);
                (editedRow.Section_, editedRow.OffsetAddr) = (section, offsetAddr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Source + ":CheatGridView_CellEndEdit", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
            if (e.RowIndex > cheatGridRowList.Count - 1) return;

            if (!Regex.IsMatch(e.Text, @"^-?[0-9][0-9,\.]*$")) return;

            CheatRow row = cheatGridRowList[e.RowIndex];

            (Section section, uint offsetAddr) = (row.Section_, row.OffsetAddr);
            ScanType scanType = this.ParseFromDescription<ScanType>(row.Cells[(int)ChertCol.CheatListType].ToString());

            byte[] newData = ScanTool.ValueStringToByte(scanType, e.Text);
            PS4Tool.WriteMemory(section.PID, offsetAddr + section.Start, newData);
        }
        #endregion

        #region CheatGridMenu
        private void CheatGridMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DataGridViewSelectedRowCollection rows = CheatGridView.SelectedRows;
            if (rows == null || rows.Count == 0) return;

            DataGridViewRow row = rows[0];
            bool isOnOffVisible = !string.IsNullOrWhiteSpace((string)row.Cells[(int)ChertCol.CheatListOn].Value);
            CheatGridMenuOnValue.Visible = isOnOffVisible;
            CheatGridMenuOffValue.Visible = isOnOffVisible;
        }

        private void CheatGridMenuHexEditor_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewSelectedRowCollection rows = CheatGridView.SelectedRows;
                if (rows == null || rows.Count == 0) return;
                if (rows.Count != 1) return;

                DataGridViewRow cheatRow = rows[0];
                (Section section, uint offsetAddr) = ((Section section, uint offsetAddr))cheatRow.Tag;

                HexEditor hexEdit = new HexEditor(this, sectionTool, section, (int)offsetAddr);
                hexEdit.Show(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Source + ":CheatGridMenuHexEditor_Click", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void CheatGridMenuLock_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rows = CheatGridView.SelectedRows;
            if (rows == null || rows.Count == 0) return;

            for (int i = 0; i < rows.Count; ++i) rows[i].Cells[(int)ChertCol.CheatListLock].Value = true;
        }

        private void CheatGridMenuUnlock_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rows = CheatGridView.SelectedRows;
            if (rows == null || rows.Count == 0) return;

            for (int i = 0; i < rows.Count; ++i) rows[i].Cells[(int)ChertCol.CheatListLock].Value = false;
        }

        private void CheatGridMenuActive_Click(object sender, EventArgs e) => CheatGridActiveValue();

        private void CheatGridMenuOnValue_Click(object sender, EventArgs e) => CheatGridActiveValue(true);

        private void CheatGridMenuOffValue_Click(object sender, EventArgs e) => CheatGridActiveValue(false);

        private void CheatGridActiveValue(bool? isOnValue = null)
        {
            DataGridViewSelectedRowCollection rows = CheatGridView.SelectedRows;
            if (rows == null || rows.Count == 0) return;

            for (int idx = 0; idx < rows.Count; ++idx)
            {
                int index = rows[idx].Index;
                CheatRow row = cheatGridRowList[index];
                (Section section, uint offsetAddr) = (row.Section_, row.OffsetAddr);
                ScanType scanType = this.ParseFromDescription<ScanType>(row.Cells[(int)ChertCol.CheatListType].ToString());
                string value;

                if (isOnValue == null) value = row.Cells[(int)ChertCol.CheatListValue].ToString();
                else
                {
                    if ((bool)isOnValue) value = row.Cells[(int)ChertCol.CheatListOn].ToString();
                    else value = row.Cells[(int)ChertCol.CheatListOff].ToString();
                    if (value != null && value.Trim() != "")
                    {
                        row.Cells[(int)ChertCol.CheatListValue] = value;
                        CheatGridView.Refresh();
                    }
                    else value = row.Cells[(int)ChertCol.CheatListValue].ToString();
                }


                byte[] data = ScanTool.ValueStringToByte(scanType, value);
                PS4Tool.WriteMemory(section.PID, offsetAddr + section.Start, data);
            }
        }

        private void CheatGridMenuEdit_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rows = CheatGridView.SelectedRows;
            if (rows == null || rows.Count == 0) return;

            string errorMsg = "";
            try
            {
                CheatRow row = default;
                if (rows.Count > 1)
                {
                    string inputValue = "";
                    if (InputBox.Show("Multiple Addresses Edit", "Please enter a value and write to multiple addresses", ref inputValue) != DialogResult.OK) return;

                    for (int idx = 0; idx < rows.Count; ++idx)
                    {
                        int index = 0;
                        try
                        {
                            index = rows[idx].Index;
                            row = cheatGridRowList[index];
                            (Section section, uint offsetAddr) checkRow = (row.Section_, row.OffsetAddr);
                            ScanType rowScanType = this.ParseFromDescription<ScanType>(row.Cells[(int)ChertCol.CheatListType].ToString());
                            byte[] rowData = ScanTool.ValueStringToByte(rowScanType, inputValue);
                            PS4Tool.WriteMemory(checkRow.section.PID, checkRow.offsetAddr + checkRow.section.Start, rowData);
                            row.Cells[(int)ChertCol.CheatListValue] = ScanTool.BytesToString(rowScanType, rowData, false, inputValue.StartsWith("-"));
                        }
                        catch (Exception ex)
                        {
                            errorMsg += string.Format("Row {0}, Exception: {1}\n", index, ex);
                            row.ForeColor = Color.Red;
                        }
                    }
                    CheatGridView.Refresh();
                    if (errorMsg != "") InputBox.MsgBox("CheatGridMenuEdit_Click Exception", "", errorMsg, 100);
                    return;
                }

                row = cheatGridRowList[rows[0].Index];
                ScanType scanType = this.ParseFromDescription<ScanType>(row.Cells[(int)ChertCol.CheatListType].ToString());
                string oldValue = row.Cells[(int)ChertCol.CheatListValue].ToString();
                (Section section, uint offsetAddr) = (row.Section_, row.OffsetAddr);

                NewAddress newAddress = null;
                bool isPointer = row.Cells[(int)ChertCol.CheatListAddress].ToString().StartsWith("P->");
                List<long> offsetList = null;
                if (isPointer)
                {
                    offsetList = new List<long>();
                    uint baseSID = row.Cells[(int)ChertCol.CheatListSID] == null ? 0 : (uint)row.Cells[(int)ChertCol.CheatListSID];
                    string[] baseSectionArr = row.Cells[(int)ChertCol.CheatListSection].ToString().Split('|');
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
                    newAddress = new NewAddress(this, section, baseSection, offsetAddr + section.Start, scanType, oldValue, (bool)row.Cells[(int)ChertCol.CheatListLock], (string)row.Cells[(int)ChertCol.CheatListDesc], offsetList, true, (string)row.Cells[(int)ChertCol.CheatListOn], (string)row.Cells[(int)ChertCol.CheatListOff]);
                }
                else newAddress = new NewAddress(this, section, offsetAddr + section.Start, scanType, oldValue, (bool)row.Cells[(int)ChertCol.CheatListLock], (string)row.Cells[(int)ChertCol.CheatListDesc], true, (string)row.Cells[(int)ChertCol.CheatListOn], (string)row.Cells[(int)ChertCol.CheatListOff]);

                if (newAddress.ShowDialog() != DialogResult.OK) return;

                (row.Section_, row.OffsetAddr) = (newAddress.AddrSection, (uint)(newAddress.Address - newAddress.AddrSection.Start));
                row.Cells[(int)ChertCol.CheatListAddress] = (newAddress.Address).ToString("X8");
                row.Cells[(int)ChertCol.CheatListType] = newAddress.CheatType.GetDescription();
                row.Cells[(int)ChertCol.CheatListLock] = newAddress.IsLock;
                row.Cells[(int)ChertCol.CheatListDesc] = newAddress.Descriptioin;
                if (newAddress.IsPointer)
                {
                    row.Cells[(int)ChertCol.CheatListAddress] = "P->" + row.Cells[(int)ChertCol.CheatListAddress];
                    string offsetStr = "|";
                    for (int idx = 0; idx < newAddress.PointerOffsets.Count; idx++)
                    {
                        long offset = newAddress.PointerOffsets[idx];
                        if (offsetStr == "|") offsetStr += (offset - (long)newAddress.BaseSection.Start).ToString("X");
                        else offsetStr += "_" + offset.ToString("X");
                    }
                    row.Cells[(int)ChertCol.CheatListSID] = newAddress.BaseSection.SID;
                    row.Cells[(int)ChertCol.CheatListSection] = string.Format("{0}|{1}|{2}|{3}|{4}", newAddress.BaseSection.Start.ToString("X"), newAddress.BaseSection.Name, newAddress.BaseSection.Prot.ToString("X"), newAddress.BaseSection.Offset.ToString("X"), offsetStr);
                }
                else
                {
                    row.Cells[(int)ChertCol.CheatListSID] = newAddress.AddrSection.SID;
                    row.Cells[(int)ChertCol.CheatListSection] = string.Format("{0}|{1}|{2}|{3}", newAddress.AddrSection.Start.ToString("X"), newAddress.AddrSection.Name, newAddress.AddrSection.Prot.ToString("X"), newAddress.AddrSection.Offset.ToString("X"));
                }

                byte[] data = ScanTool.ValueStringToByte(newAddress.CheatType, newAddress.Value);
                PS4Tool.WriteMemory(section.PID, offsetAddr + section.Start, data);
                row.Cells[(int)ChertCol.CheatListValue] = newAddress.Value;
                row.Cells[(int)ChertCol.CheatListOn] = newAddress.OnValue;
                row.Cells[(int)ChertCol.CheatListOff] = newAddress.OffValue;
                CheatGridView.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Source + ":CheatGridMenuEdit_Click", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void CheatGridMenuCopyAddress_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rows = CheatGridView.SelectedRows;
            if (rows == null || rows.Count == 0) return;

            string clipStr = "";
            for (int idx = 0; idx < rows.Count; ++idx)
            {
                int index = rows[idx].Index;
                CheatRow row = cheatGridRowList[index];
                (Section section, uint offsetAddr) = (row.Section_, row.OffsetAddr);
                if (clipStr.Length > 0) clipStr += " \n";
                clipStr += (offsetAddr + section.Start).ToString("X");
            }
            if (clipStr.Length > 0) Clipboard.SetText(clipStr);
        }

        private void CheatGridMenuFindPointer_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rows = CheatGridView.SelectedRows;
            if (rows == null || rows.Count == 0) return;
            if (rows.Count != 1) return;

            try
            {
                var cheatRow = rows[0];
                (Section section, uint offsetAddr) = ((Section section, uint offsetAddr))cheatRow.Tag;
                ScanType scanType = this.ParseFromDescription<ScanType>(cheatRow.Cells[(int)ChertCol.CheatListType].Value.ToString());

                PointerFinder pointerFinder = new PointerFinder(this, offsetAddr + section.Start, scanType);
                pointerFinder.Show();
            }
            catch { }
        }

        private void CheatGridMenuDelete_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rows = CheatGridView.SelectedRows;
            if (rows == null || rows.Count == 0) return;

            CheatGridView.SuspendLayout();
            CheatGridView.CellValidating -= CheatGridView_CellValidating;

            List<int> selectedIndex = new List<int>();
            for (int idx = 0; idx < rows.Count; idx++) selectedIndex.Add(rows[idx].Index);
            selectedIndex.Sort();
            CheatGridView.VirtualMode = false;
            CheatGridView.RowCount = 0;
            CheatGridView.Rows.Clear();

            for (int idx = selectedIndex.Count - 1; idx >= 0; idx--)
            {
                if (selectedIndex[idx] < cheatGridRowList.Count) cheatGridRowList.RemoveAt(selectedIndex[idx]);
            }
            CheatGridView.RowCount = cheatGridRowList.Count;
            CheatGridView.VirtualMode = true;
            CheatGridView.Refresh();
            if (cheatGridRowList.Count < CheatGridGroupRefreshThreshold && CheatGridView.GroupByEnabled) CheatGridView.GroupRefresh();
            CheatGridView.ResumeLayout();
            CheatGridView.CellValidating += CheatGridView_CellValidating;
            rows = null;
            selectedIndex.Clear();
            selectedIndex = null;
            GC.Collect();
        }
        #endregion

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
        /// <param name="isUpdateCheatGridViewRowCount">whether to update the row count value of the virtual mode CheatGridView</param>
        /// <param name="onValue">onValue</param>
        /// <param name="offValue">offValue</param>
        /// <returns>returns the cheatRow added this time, which can be used to change the row style</returns>
        public CheatRow AddToCheatGrid(Section section, uint offsetAddr, ScanType scanType, string oldValue, bool cheatLock = false, string cheatDesc = "", List<long> pointerOffsets = null, Dictionary<ulong, ulong> pointerCaches = null, int relativeOffset=-1, bool isUpdateCheatGridViewRowCount=true, string onValue=null, string offValue=null)
        {
            CheatRow result = new CheatRow { Cells = new List<object>() { default, default, default, default, default, default, default, default, default, default, default, } };
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

                if (isFailed) result.ForeColor = Color.Red;

                if (relativeOffset > -1) result.Cells[(int)ChertCol.CheatListSection] = "+" + relativeOffset.ToString("X");
                else
                {
                    result.Cells[(int)ChertCol.CheatListSID] = section.SID;
                    result.Cells[(int)ChertCol.CheatListSection] = string.Format("{0}|{1}|{2}|{3}", section.Start.ToString("X"), section.Name, section.Prot.ToString("X"), section.Offset.ToString("X"));
                }
                
                if (isPointer)
                {
                    result.Section_ = pointerTag.section;
                    result.OffsetAddr = pointerTag.offsetAddr;
                    result.Cells[(int)ChertCol.CheatListAddress] = "P->" + (pointerTag.offsetAddr + pointerTag.section.Start).ToString("X");
                    string offsetStr = "|";
                    for (int idx = 0; idx < pointerOffsets.Count; idx++)
                    {
                        long pointerOffset = pointerOffsets[idx];
                        if (offsetStr == "|") offsetStr += (pointerOffset - (long)section.Start).ToString("X");
                        else offsetStr += "_" + pointerOffset.ToString("X");
                    }
                    result.Cells[(int)ChertCol.CheatListSection] += offsetStr;
                    section = pointerTag.section;
                }
                else
                {
                    result.Section_ = section;
                    result.OffsetAddr = offsetAddr;
                    result.Cells[(int)ChertCol.CheatListAddress] = (offsetAddr + (section != null ? section.Start : 0)).ToString("X8");
                }
                result.IsSign = oldValue.StartsWith("-"); //IsSign
                result.Cells[(int)ChertCol.CheatListType] = scanType.GetDescription();
                result.Cells[(int)ChertCol.CheatListValue] = oldValue;
                result.Cells[(int)ChertCol.CheatListLock] = cheatLock;
                result.Cells[(int)ChertCol.CheatListDesc] = cheatDesc;
                if (onValue != null) result.Cells[(int)ChertCol.CheatListOn] = onValue;
                if (offValue != null) result.Cells[(int)ChertCol.CheatListOff] = offValue;

                cheatGridRowList.Add(result);
                if (isUpdateCheatGridViewRowCount) CheatGridView.RowCount = cheatGridRowList.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Source + ":AddToCheatGrid", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return result;
        }

        /// <summary>
        /// If isUpdateCheatGridViewRowCount is false when AddToCheatGrid is executed, 
        /// the CheatGridView's VirtualMode will be set to false. 
        /// After multiple AddToCheatGrid operations are completed, 
        /// CheatGridViewRowCountUpdate will be executed to update the CheatGridView's row count value, 
        /// and then CheatGridView's VirtualMode will be set to true.
        /// </summary>
        /// <param name="virtualMode"></param>
        public void CheatGridViewRowCountUpdate(bool virtualMode = true)
        {
            CheatGridView.RowCount = virtualMode ? cheatGridRowList.Count : 0;
            CheatGridView.VirtualMode = virtualMode;
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
        public bool InitSections(string processName, bool force = false)
        {
            libdebug.ProcessMap pMap = null;
            libdebug.ProcessInfo processInfo = PS4Tool.GetProcessInfo(processName);

            if (processInfo.pid == 0 || processInfo.name != processName)
            {
                Invoke(new MethodInvoker(() =>
                {
                    ProcessPid = 0;
                    ToolStripLockEnable.Checked = false;
                    ToolStripAutoRefresh.Checked = false;
                    ToolStripProcessInfo.Text = string.Format("ProcessInfo: Cheat file Process({0}) could not find.", processName);
                    ToolStripProcessInfo.Tag = false;
                    (GameID, GameVer) = (null, null);
                }));
                return false;
            }
            else if (ToolStripProcessInfo.Tag is bool) ToolStripProcessInfo.Tag = null; //When ProcessInfo has been queried, clear unsuccessful Tags.
            else if (!force && processInfo.pid > 0 && processInfo.pid == sectionTool.PID && sectionTool.SectionDict != null)
            {
                if (sectionTool.SectionDict.Count < 20) pMap = PS4Tool.GetProcessMaps(processInfo.pid);
                if (pMap == null || sectionTool.SectionDict.Count >= pMap.entries.Length) return true; //When the Process ID is the same, initialization will no longer be executed.
            }

            ProcessPid = processInfo.pid;
            ProcessName = processInfo.name;
            ProcessContentid = processInfo.contentid;
            if (pMap == null) sectionTool.InitSections(processInfo.pid, ProcessName);
            else sectionTool.InitSections(pMap, processInfo.pid, ProcessName);

            return true;
        }

        /// <summary>
        /// Read libSceCdlgUtilServer.sprx of PS4's SceCdlgApp to obtain the Game ID and version.
        /// </summary>
        public void InitGameInfo()
        {
            if ((GameID, GameVer) != default) return;

            string PS4FWVersion = Properties.Settings.Default.PS4FWVersion.Value ?? "";
            string FWVer = PS4FWVersion != "" ? PS4FWVersion : Constant.Versions[0];

            (GameID, GameVer) = ScanTool.GameInfo(FWVer);
        }

        /// <summary>
        /// Read the conf file corresponding to the game ID in the "sections" directory.
        /// The file contains the addresses, start and end, and whether they are valid for various Hidden Sections.
        /// Convert this information into a LocalHiddenSections Dictionary.
        /// </summary>
        public void InitLocalHiddenSections()
        {
            if (LocalHiddenSections != null) LocalHiddenSections.Clear();
            LocalHiddenSections = new Dictionary<uint, (ulong Start, ulong End, bool Valid, byte Prot, string Name)>();
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("sections{0}{1}.conf", Path.DirectorySeparatorChar, GameID));
                if (!File.Exists(filePath)) return;

                foreach (string line in File.ReadLines(filePath))
                {
                    string[] sectionInfo = line.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (sectionInfo.Length < 3) continue;
                    if (!uint.TryParse(sectionInfo[0], out uint sid)) continue;
                    ulong start = ulong.Parse(sectionInfo[1], NumberStyles.HexNumber);
                    ulong end   = ulong.Parse(sectionInfo[2], NumberStyles.HexNumber);
                    bool valid  = bool.Parse(sectionInfo[3]);
                    byte Prot   = byte.Parse(sectionInfo[4], NumberStyles.HexNumber);
                    string name = sectionInfo[5];
                    LocalHiddenSections.Add(sid, (start, end, valid, Prot, name));
                }
            }
            catch (Exception ex)
            {
                InputBox.MsgBox("Read Local HiddenSections Exception", "", ex.ToString(), 100);
            }
        }

        /// <summary>
        /// Read the contents of the LocalHiddenSections Dictionary and update them in the conf file corresponding to the game ID in the "sections" directory.
        /// </summary>
        public void UpdateLocalHiddenSections()
        {
            try
            {
                if (LocalHiddenSections == null || LocalHiddenSections.Count == 0) return;

                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("sections{0}{1}.conf", Path.DirectorySeparatorChar, GameID));
                if (!File.Exists(filePath)) return;

                StringBuilder localSectionSB = new StringBuilder();
                foreach (string line in File.ReadLines(filePath))
                {
                    string[] sectionInfo = line.Split(new char[] { '\t', ' ' }, 6, StringSplitOptions.RemoveEmptyEntries);
                    if (sectionInfo.Length < 3 || !uint.TryParse(sectionInfo[0], out uint sid))
                    {
                        localSectionSB.AppendLine(line);
                        continue;
                    }
                    ulong start = ulong.Parse(sectionInfo[1], NumberStyles.HexNumber);
                    ulong end   = ulong.Parse(sectionInfo[2], NumberStyles.HexNumber);
                    bool valid  = bool.Parse(sectionInfo[3]);
                    uint prot   = uint.Parse(sectionInfo[4], NumberStyles.HexNumber);
                    string name = sectionInfo[5];
                    if (LocalHiddenSections.TryGetValue(sid, out (ulong Start, ulong End, bool Valid, byte Prot, string Name) localSection))
                    {
                        start = localSection.Start;
                        end   = localSection.End;
                        valid = localSection.Valid;
                        LocalHiddenSections.Remove(sid);
                    }
                    string newLine = string.Format("{0:000000000}\t{1:X9}\t{2:X9}\t{3}\t{4:X2}\t{5}", sid, start, end, valid, prot, name);
                    localSectionSB.AppendLine(newLine);
                }
                if (LocalHiddenSections.Count > 0)
                {
                    foreach (KeyValuePair<uint, (ulong Start, ulong End, bool Valid, byte Prot, string Name)> kvp in LocalHiddenSections)
                    {
                        (ulong Start, ulong End, bool Valid, byte Prot, string Name) local = kvp.Value;
                        string newLine = string.Format("{0:000000000}\t{1:X9}\t{2:X9}\t{3}\t{4:X2}\t{5}", kvp.Key, local.Start, local.End, local.Valid, local.Prot, local.Name);
                        localSectionSB.AppendLine(newLine);
                    }
                    LocalHiddenSections.Clear();
                }
                string contentToWrite = localSectionSB.ToString();

                File.WriteAllText(filePath, contentToWrite);
            }
            catch (Exception ex)
            {
                InputBox.MsgBox("Write Local HiddenSection Exception", "", ex.ToString(), 100);
            }
        }
        #endregion
    }
}
