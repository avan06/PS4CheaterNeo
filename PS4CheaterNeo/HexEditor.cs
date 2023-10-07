using AsmJit.AssemblerContext;
using Be.Windows.Forms;
using SharpDisasm;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace PS4CheaterNeo
{
    public partial class HexEditor : Form
    {
        int Page;
        int PageCount;
        long Line;
        int Column;
        string delimitedDash;
        Dictionary<long, long> changedPosDic;
        const int PageSize = 8 * 1024 * 1024;
        readonly Main mainForm;
        readonly SectionTool sectionTool;
        Section section;
        Section[] sections;

        /// <summary>
        /// good pattern for using a Global Mutex in C#
        /// https://stackoverflow.com/a/229567
        /// </summary>
        private readonly MutexAccessRule allowEveryoneRule;
        private readonly MutexSecurity mSec;
        private readonly Mutex mutex;

        private HexEditor(Main mainForm)
        {
            this.mainForm = mainForm;
            changedPosDic = new Dictionary<long, long>();

            allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
            mSec = new MutexSecurity();
            mSec.AddAccessRule(allowEveryoneRule);
            mutex = new Mutex(false, "HexEditor", out _, mSec);

            InitializeComponent();
            ApplyUI();

            delimitedDash = Properties.Settings.Default.HexInfoDash.Value;
            if (!Properties.Settings.Default.CollapsibleContainer.Value)
            {
                SplitContainer1.SplitterButtonStyle = ButtonStyle.None;
                SplitContainer2.SplitterButtonStyle = ButtonStyle.None;
            }
            else SplitContainer2.SplitterDistance = SplitContainer2.Height - SplitContainer2.SplitterWidth + SplitContainer2.Panel2MinSize;

            try
            {
                BuiltInContextMenu builtInMenu = HexView.BuiltInContextMenu;
                HexViewMenu.Items.Add(builtInMenu.GetCopyHexToolStripMenuItem());
                HexViewMenu.Items.Add(builtInMenu.GetCopyToolStripMenuItem());
                HexViewMenu.Items.Add(builtInMenu.GetPasteToolStripMenuItem());
                HexViewMenu.Items.Add(builtInMenu.GetPasteHexToolStripMenuItem());
                HexViewMenu.Items.Add(builtInMenu.GetSelectAllToolStripMenuItem());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Source + ":HexEditor\n" + ex.ToString());
            }

            HexBox.ByteGroupingType[] byteGroupingTypes = (HexBox.ByteGroupingType[])Enum.GetValues(typeof(HexBox.ByteGroupingType));
            foreach (var byteGroupingType in byteGroupingTypes) HexViewMenuByteGroup.Items.Add(new ComboItem("GroupType:" + byteGroupingType.ToString(), byteGroupingType));
            HexViewMenuByteGroup.SelectedIndex = 0;

            for (int idx = 0; idx <= 0x10; idx++) HexViewMenuGroupSize.Items.Add("GroupSize:" + idx);
            HexViewMenuGroupSize.SelectedIndex = HexView.GroupSize;

            AutoRefreshBox.Checked = Properties.Settings.Default.AutoRefresh.Value;
            AutoRefreshTimer.Interval = (int)Properties.Settings.Default.AutoRefreshTimerInterval.Value;
            HexBox.Checked = Properties.Settings.Default.InputIsHexFormat.Value;
            LittleEndianBox.Checked = Properties.Settings.Default.UsingLittleEndian.Value;
        }

        public HexEditor(Main mainForm, SectionTool sectionTool, Section section, int baseAddr) : this(mainForm)
        {
            if (section == null || section.SID == 0) throw new ArgumentNullException("Init HexEditor failed, section is null.");
            if (baseAddr < 0) throw new ArgumentNullException("Init HexEditor failed, baseAddr is invalid.");

            this.sectionTool = sectionTool;
            sections = sectionTool.GetSectionSortByAddr();
            for (int sectionIdx = 0; sectionIdx < sections.Length; sectionIdx++)
            {
                Section tmpS = sections[sectionIdx];
                SectionBox.Items.Add(string.Format(" {0:0000}:{1:X9} 	 	 {2} Prot:{3:X2} Len:{4:X}", sectionIdx, tmpS.Start, tmpS.Name, tmpS.Prot, tmpS.Length));
                if (tmpS.SID == section.SID) SectionBox.SelectedIndex = sectionIdx;
            }

            InitPageData(section, baseAddr);
        }

        /// <summary>
        /// Initialize the contents of the PageBox menu based on the specified Section and 
        /// select the corresponding PageBox menu based on the relative address (baseAddr).
        /// </summary>
        /// <param name="section"></param>
        /// <param name="baseAddr"></param>
        private void InitPageData(Section section, int baseAddr)
        {
            this.section = section;
            Page = baseAddr / PageSize;
            Line = (baseAddr - Page * PageSize) / HexView.BytesPerLine;
            Column = (baseAddr - Page * PageSize) % HexView.BytesPerLine;

            PageCount = DivUP(section.Length, PageSize);

            PageBox.Items.Clear();
            for (int i = 0; i < PageCount; ++i)
            {
                ulong start = section.Start + (ulong)i * PageSize;
                ulong end = section.Start + (ulong)(i + 1) * PageSize;
                PageBox.Items.Add((i + 1).ToString("00") + String.Format(" {0:X8}-{1:X8}", start, end));
            }
            PageBox.SelectedIndex = PageBox.Items.Count > Page ? Page : PageBox.Items.Count - 1;
        }

        public void ApplyUI()
        {
            try
            {
                Opacity = Properties.Settings.Default.UIOpacity.Value;

                BackColor = Properties.Settings.Default.UiBackColor.Value; //Color.FromArgb(36, 36, 36);
                ForeColor = Properties.Settings.Default.UiForeColor.Value; //Color.White;
                HexView.ChangedFinishForeColor = Properties.Settings.Default.HexEditorChangedFinishForeColor.Value; //Color.LimeGreen;
                HexView.ShadowSelectionColor = Properties.Settings.Default.HexEditorShadowSelectionColor.Value; //Color.FromArgb(100, 60, 188, 255);
                HexView.ZeroBytesForeColor = Properties.Settings.Default.HexEditorZeroBytesForeColor.Value; //Color.DimGray;

                CommitBtn.ForeColor          = ForeColor;
                FindBtn.ForeColor            = ForeColor;
                AddToCheatGridBtn.ForeColor  = ForeColor;
                NextBtn.ForeColor            = ForeColor;
                PreviousBtn.ForeColor        = ForeColor;
                RefreshBtn.ForeColor         = ForeColor;
                AssemblerBtn.ForeColor       = ForeColor;
                GroupBoxAsm.ForeColor        = ForeColor;
                InfoBox4SSeparator.BackColor = ForeColor;
                InfoBoxDSeparator.BackColor  = ForeColor;
                InfoBox4Separator.BackColor  = ForeColor;

                HexView.ForeColor         = ForeColor;
                HexView.BackColor         = BackColor;
                SplitContainer1.BackColor = BackColor;
                SplitContainer2.ForeColor = ForeColor;
                InfoBox0.ForeColor        = ForeColor;
                InfoBox0.BackColor        = BackColor;
                InputBox.ForeColor        = ForeColor;
                InputBox.BackColor        = BackColor;
                PageBox.ForeColor         = ForeColor;
                PageBox.BackColor         = BackColor;
                InfoBoxB.ForeColor        = ForeColor;
                InfoBoxB.BackColor        = BackColor;
                InfoBox4S.ForeColor       = ForeColor;
                InfoBox4S.BackColor       = BackColor;
                InfoBox3S.ForeColor       = ForeColor;
                InfoBox3S.BackColor       = BackColor;
                InfoBox2S.ForeColor       = ForeColor;
                InfoBox2S.BackColor       = BackColor;
                InfoBoxF.ForeColor        = ForeColor;
                InfoBoxF.BackColor        = BackColor;
                InfoBox4U.ForeColor       = ForeColor;
                InfoBox4U.BackColor       = BackColor;
                InfoBox3U.ForeColor       = ForeColor;
                InfoBox3U.BackColor       = BackColor;
                InfoBox1U.ForeColor       = ForeColor;
                InfoBox1U.BackColor       = BackColor;
                InfoBox2U.ForeColor       = ForeColor;
                InfoBox2U.BackColor       = BackColor;
                InfoBoxD.ForeColor        = ForeColor;
                InfoBoxD.BackColor        = BackColor;
                InfoBox1S.ForeColor       = ForeColor;
                InfoBox1S.BackColor       = BackColor;
                AsmBox1.ForeColor         = ForeColor;
                AsmBox1.BackColor         = BackColor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Source + ":ApplyUI", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        #region Event
        private void HexEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            DynamicByteProvider dynaBP = HexView.ByteProvider as DynamicByteProvider;

            if (dynaBP != null && dynaBP.HasChanges() && MessageBox.Show("Byte data has changes, Do you want to close HexEditor?", "HexEditor", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) e.Cancel = true;
        }

        #region HexView
        private void HexViewByteGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            var byteGroupComboBox = (ToolStripComboBox)sender;
            ComboItem comboItem = (ComboItem)byteGroupComboBox.SelectedItem;
            HexBox.ByteGroupingType byteGroupingType = (HexBox.ByteGroupingType)comboItem.Value;
            HexView.ByteGrouping = byteGroupingType;
            if (HexViewMenuGroupSize.Items.Count > 0) HexViewMenuGroupSize.SelectedIndex = HexView.ByteGroupingSize;
        }

        private void HexViewGroupSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            var groupSizeComboBox = (ToolStripComboBox)sender;
            int groupSize = groupSizeComboBox.SelectedIndex;

            HexView.GroupSeparatorVisible = groupSize > 0;
            HexView.GroupSize = groupSize;
        }

        private void HexViewMenuCopyAddress_Click(object sender, EventArgs e) => Clipboard.SetText(string.Format("{0:X9}", HexView.SelectionStart + HexView.LineInfoOffset));

        private void HexViewMenuJumpToAddress_Click(object sender, EventArgs e)
        {
            string inputValue = "";
            if (PS4CheaterNeo.InputBox.Show("Hex View", "Please enter the memory address(hex) you'd like to jump to:", ref inputValue) != DialogResult.OK) return;

            inputValue = Regex.Replace(inputValue, "[^0-9a-fA-F]", "");
            ulong address = ulong.Parse(inputValue, NumberStyles.HexNumber);

            HexViewJumpToAddress(address);
        }

        private void HexViewMenuJumpToOffset_Click(object sender, EventArgs e)
        {
            string inputValue = "";
            if (PS4CheaterNeo.InputBox.Show("Hex View", "Please enter the relative offset(hex) you'd like to jump to from the current memory address:", ref inputValue) != DialogResult.OK) return;

            bool isNegative = false;
            inputValue = Regex.Replace(inputValue, "[^-0-9a-fA-F]", "");
            if (inputValue.StartsWith("-")) isNegative = true;
            inputValue = Regex.Replace(inputValue, "[^0-9a-fA-F]", "");
            ulong offset = ulong.Parse(inputValue, NumberStyles.HexNumber);
            ulong address = section.Start + (ulong)HexView.SelectionStart;
            address = isNegative ? address - offset : address + offset;

            HexViewJumpToAddress(address);
        }

        private void HexViewJumpToAddress(ulong address)
        {
            if (address < section.Start || address > section.Start + (uint)section.Length)
            {
                (int index, Section sectionNew) = sectionTool.GetIndexInSectionSortByAddr(address, sections);
                if (index == -1) return;

                HexView.SelectionStart = 0;
                SectionBox.SelectedIndexChanged -= new EventHandler(SectionBox_SelectedIndexChanged);
                SectionBox.SelectedIndex = index;
                InitPageData(sectionNew, (int)(address - sectionNew.Start));
                SectionBox.SelectedIndexChanged += new EventHandler(SectionBox_SelectedIndexChanged);
            }
            HexView.SelectionLength = 4;
            HexView.SelectionStart = (long)(address - section.Start);
            HexView.SelectionLength = 4;
        }

        /// <summary>
        /// BaseAddr = HexView.SelectionStart + HexView.LineInfoOffset - (long)section.Start
        /// </summary>
        private void HexView_SelectionStartChanged(object sender, EventArgs e)
        {
            if (HexView.SelectionStart < 0) return;
            if (HexView.SelectionStart >= HexView.ByteProvider.Length) return;

            DynamicByteProvider dynaBP = HexView.ByteProvider as DynamicByteProvider;

            List<byte> tmpBList = new List<byte>();
            byte info1 = 0;
            UInt16 info2 = 0;
            UInt32 info4 = 0;
            UInt64 info8 = 0;
            sbyte info1S = 0;
            Int16 info2S = 0;
            Int32 info4S = 0;
            Int64 info8S = 0;
            float infoF = 0;
            double infoD = 0;
            string hexStr = "", zeroStr = "";

            for (int idx = 0; idx <= 100; ++idx)
            {
                switch (idx)
                {
                    case 1:
                        info1 = tmpBList[0];
                        InfoBox1U.Text = InfoBox1S.Text = hexStr;
                        break;
                    case 2:
                        info2 = BitConverter.ToUInt16(tmpBList.ToArray(), 0);
                        InfoBox2U.Text = InfoBox2S.Text = hexStr;
                        break;
                    case 4:
                        byte[] value4 = tmpBList.ToArray();
                        info4 = BitConverter.ToUInt32(value4, 0);
                        if (SwapBytesBox.Checked) Array.Reverse(value4);
                        infoF = BitConverter.ToSingle(value4, 0);
                        InfoBox3U.Text = InfoBox3S.Text = hexStr;
                        break;
                    case 8:
                        byte[] value8 = tmpBList.ToArray();
                        info8 = BitConverter.ToUInt64(value8, 0);
                        if (SwapBytesBox.Checked) Array.Reverse(value8);
                        infoD = BitConverter.ToDouble(value8, 0);
                        InfoBox4U.Text = InfoBox4S.Text = hexStr;
                        break;
                }
                if (!SplitContainer2.Panel1Minimized && idx > 8) break;
                else if(HexView.SelectionStart + idx < dynaBP.Length)
                {
                    byte item = dynaBP.ReadByte(HexView.SelectionStart + idx);
                    tmpBList.Add(item);
                    string dash = hexStr.Length == 0 && zeroStr.Length == 0 ? "" : delimitedDash;
                    if (SwapBytesBox.Checked)
                    {
                        if (item == 0 && hexStr.Length == 0) zeroStr += dash + "00";
                        else
                        {
                            hexStr += zeroStr + dash + item.ToString("X2");
                            zeroStr = "";
                        }
                    }
                    else
                    {
                        if (item == 0) zeroStr = "00" + dash + zeroStr;
                        else
                        {
                            hexStr = item.ToString("X2") + dash + zeroStr + hexStr;
                            zeroStr = "";
                        }
                    }
                }
                else if (idx < 8) tmpBList.Add(0);
            }

            if (SwapBytesBox.Checked)
            {
                info2 = ScanTool.SwapBytes(info2);
                info4 = ScanTool.SwapBytes(info4);
                info8 = ScanTool.SwapBytes(info8);
            }
            info1S = (sbyte)info1;
            info2S = (Int16)info2;
            info4S = (Int32)info4;
            info8S = (Int64)info8;
            InfoBoxB.Text = Convert.ToString(info8S, 2);
            if (InfoBoxB.Text.Length % 8 != 0) InfoBoxB.Text = InfoBoxB.Text.PadLeft(InfoBoxB.Text.Length + 8 - InfoBoxB.Text.Length % 8, '0');
            for (int i = InfoBoxB.Text.Length - 8; i >= 8; i -= 8) InfoBoxB.Text = InfoBoxB.Text.Insert(i, ",");

            InfoBox0.Text   = string.Format(@"{0:X} | {1:X}+{2:X}", HexView.SelectionStart + HexView.LineInfoOffset, HexView.SelectionStart + HexView.LineInfoOffset - (long)section.Start, section.Start);
            InfoBox1U.Text  = string.Format("{0:N0} | ", info1) + InfoBox1U.Text;
            InfoBox2U.Text  = string.Format("{0:N0} | ", info2) + InfoBox2U.Text;
            InfoBox3U.Text  = string.Format("{0:N0} | ", info4) + InfoBox3U.Text;
            InfoBox4U.Text  = string.Format("{0:N0} | ", info8) + InfoBox4U.Text;
            InfoBoxF.Text   = string.Format("{0}", (double)infoF); //float which maps to System.Single simply does not have enough precision／https://stackoverflow.com/a/69849989
            InfoBoxD.Text   = string.Format("{0}", infoD);
            InfoBox1S.Text  = string.Format("{0:N0} | ", info1S) + InfoBox1S.Text;
            InfoBox2S.Text  = string.Format("{0:N0} | ", info2S) + InfoBox2S.Text;
            InfoBox3S.Text  = string.Format("{0:N0} | ", info4S) + InfoBox3S.Text;
            InfoBox4S.Text  = string.Format("{0:N0} | ", info8S) + InfoBox4S.Text;

            if (SplitContainer2.Panel1Minimized)
            {
                AsmBox1.Text = "";
                IEnumerable<Instruction> instructions = Disassembly(tmpBList.ToArray(), (ulong)(HexView.SelectionStart + HexView.LineInfoOffset));
                foreach (Instruction instruction in instructions)
                {
                    string address = instruction.Offset.ToString("X").ToUpper();
                    string byteStr = Disassembler.Translator.TranslateBytes(instruction).ToUpper();
                    string mnemonic = Disassembler.Translator.TranslateMnemonic(instruction);
                    AsmBox1.Text += String.Format("{0,9} {1,-12} {2}", address, byteStr, mnemonic) + "\r\n";
                }
            }
        }
        private IEnumerable<Instruction> Disassembly(byte[] data, ulong address)
        {
            ArchitectureMode architecture = ArchitectureMode.x86_64;
            Disassembler.Translator.IncludeAddress = true;
            Disassembler.Translator.IncludeBinary = true;
            IEnumerable<Instruction> instructions = new Disassembler(data, architecture, address, true, Vendor.Any, 0UL).Disassemble();
            foreach (Instruction instruction in instructions) yield return instruction;
        }
        #endregion

        private void SectionBox_SelectedIndexChanged(object sender, EventArgs e) => InitPageData(sections[SectionBox.SelectedIndex], 0);

        private void PageBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Page = PageBox.SelectedIndex;

            UpdateUi(Page, Line);
        }

        private void PreviousBtn_Click(object sender, EventArgs e)
        {
            if (Page <= 0)
            {
                if (SectionBox.SelectedIndex > 0) SectionBox.SelectedIndex--;
                return;
            }

            Page--;
            Line = 0;
            Column = 0;
            PageBox.SelectedIndex = Page;
        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            if (Page + 1 >= PageCount)
            {
                if (SectionBox.SelectedIndex < SectionBox.Items.Count - 1) SectionBox.SelectedIndex++;
                return;
            };

            Page++;
            Line = 0;
            Column = 0;
            PageBox.SelectedIndex = Page;
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            PageBox.SelectedIndex = Page;
            Line = HexView.CurrentLine - 1;

            if (HexView.SelectionStart > 0)
            {
                string hex = HexView.SelectionStart.ToString("X");
                char lastChar = hex[hex.Length - 1];
                Column = int.Parse(lastChar.ToString(), NumberStyles.HexNumber);
            }
            else Column = 0;
            UpdateUi(Page, Line, true);
        }

        private void CommitBtn_Click(object sender, EventArgs e)
        {
            DynamicByteProvider dynaBP = HexView.ByteProvider as DynamicByteProvider;
            if (!dynaBP.HasChanges()) return;

            byte[] buffer = dynaBP.Bytes.ToArray();
            foreach (long address in changedPosDic.Keys)
            {
                byte[] data;
                long changedLen = changedPosDic[address];
                if (changedLen <= 1) data = new byte[] { buffer[address] };
                else
                {
                    long startPos = address;
                    long endPos = address + changedLen;
                    data = new byte[endPos - startPos];
                    Array.Copy(buffer, startPos, data, 0, endPos - startPos);
                }
                PS4Tool.WriteMemory(mainForm.ProcessPid, (ulong)(address + HexView.LineInfoOffset), data);
            }
            changedPosDic.Clear();
            HexView.ChangedPosSetFinish();
        }

        private void AddToCheatGridBtn_Click(object sender, EventArgs e)
        {
            if (HexView.SelectionStart <= 0) return;

            DynamicByteProvider dynaBP = HexView.ByteProvider as DynamicByteProvider;
            ulong address = (ulong)(HexView.SelectionStart + HexView.LineInfoOffset);

            byte[] value;
            ScanType scanType;

            List<byte> tmpBList = new List<byte>();
            for (int idx = 0; idx <= 8; ++idx) tmpBList.Add(dynaBP.ReadByte(HexView.SelectionStart + idx));
            switch (HexView.SelectionLength)
            {
                case 1:
                    value = tmpBList.GetRange(0, 1).ToArray();
                    scanType = ScanType.Byte_;
                    break;
                case 2:
                    value = tmpBList.GetRange(0, 2).ToArray();
                    scanType = ScanType.Bytes_2;
                    break;
                case 4:
                    value = tmpBList.GetRange(0, 4).ToArray();
                    scanType = ScanType.Bytes_4;
                    break;
                case 8:
                    value = tmpBList.GetRange(0, 8).ToArray();
                    scanType = ScanType.Bytes_8;
                    break;
                default:
                    value = tmpBList.GetRange(0, 4).ToArray();
                    scanType = ScanType.Bytes_4;
                    break;
            }
            NewAddress newAddress = new NewAddress(mainForm, section, address, scanType, ScanTool.BytesToString(scanType, value, false, false), false, "", false);
            if (newAddress.ShowDialog() != DialogResult.OK)
                return;
        }

        private void FindBtn_Click(object sender, EventArgs e)
        {
            try
            {
                long selectionStart = HexView.SelectionStart;

                FindOptions findOptions = new FindOptions();
                findOptions.FindDirection = ForwardBox.Checked ? Direction.Forward : Direction.Backward;
                findOptions.Type = FindType.Hex;

                byte[] doubleBytes = null;
                string inputValue = InputBox.Text.Replace("0x", "").Replace(",", "");

                if (inputValue == "" && Properties.Settings.Default.AutoFindClosestChangedPosition.Value)
                {
                    List<long> changedAndFinishPosSetSet = HexView.GetChangedFinishPosList();
                    if (!changedAndFinishPosSetSet.Contains(selectionStart)) changedAndFinishPosSetSet.Add(selectionStart);
                    changedAndFinishPosSetSet.Sort();
                    int idx = changedAndFinishPosSetSet.IndexOf(selectionStart);
                    if (ForwardBox.Checked) idx++;
                    else if (idx > 0) idx--;
                    if (idx < changedAndFinishPosSetSet.Count) HexView.Select(changedAndFinishPosSetSet[idx], 1);
                    return;
                }

                if (HexBox.Checked)
                {
                    findOptions.Hex = ScanTool.ValueStringToByte(ScanType.Hex, inputValue);
                    if (LittleEndianBox.Checked) Array.Reverse(findOptions.Hex);
                }
                else if (Regex.IsMatch(inputValue, @"[-]*\d+\.\d+"))
                {
                    float.TryParse(inputValue, out float resultF);
                    double.TryParse(inputValue, out double resultD);
                    doubleBytes = ScanTool.ValueStringToByte(ScanType.Double_, inputValue);
                    if (Math.Abs(resultD - resultF) < 1) findOptions.Hex = ScanTool.ValueStringToByte(ScanType.Float_, inputValue);
                    else findOptions.Hex = doubleBytes;
                }
                else if (UInt64.TryParse(inputValue, out ulong result))
                {
                    if (result <= 0xFF) findOptions.Hex = ScanTool.ValueStringToByte(ScanType.Byte_, inputValue); //255
                    else if (result <= 0xFFFF) findOptions.Hex = ScanTool.ValueStringToByte(ScanType.Bytes_2, inputValue); //65535
                    else if (result <= 0xFFFFFFFF) findOptions.Hex = ScanTool.ValueStringToByte(ScanType.Bytes_4, inputValue); //4294967295
                    else findOptions.Hex = ScanTool.ValueStringToByte(ScanType.Bytes_8, inputValue);
                }
                else if (Int64.TryParse(inputValue, out long resultSign))
                {
                    if (resultSign < 0 && resultSign >= -0x80) findOptions.Hex = ScanTool.ValueStringToByte(ScanType.Byte_, inputValue); //-128
                    else if (resultSign < 0 && resultSign >= -0x8000) findOptions.Hex = ScanTool.ValueStringToByte(ScanType.Bytes_2, inputValue); //-32768
                    else if (resultSign < 0 && resultSign >= -0x80000000) findOptions.Hex = ScanTool.ValueStringToByte(ScanType.Bytes_4, inputValue); //-2147483648
                    else if (Math.Abs(resultSign) <= 0xFF) findOptions.Hex = ScanTool.ValueStringToByte(ScanType.Byte_, inputValue); //255
                    else if (Math.Abs(resultSign) <= 0xFFFF) findOptions.Hex = ScanTool.ValueStringToByte(ScanType.Bytes_2, inputValue); //65535
                    else if (Math.Abs(resultSign) <= 0xFFFFFFFF) findOptions.Hex = ScanTool.ValueStringToByte(ScanType.Bytes_4, inputValue); //4294967295
                    else findOptions.Hex = ScanTool.ValueStringToByte(ScanType.Bytes_8, inputValue);
                }

                if (HexView.Find(findOptions) == -1 && doubleBytes != null && findOptions.Hex.Length < 8)
                {
                    HexView.SelectionStart = selectionStart;
                    findOptions.Hex = doubleBytes;
                    HexView.Find(findOptions);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Source + ":FindBtn_Click", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void AssemblerBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string inputValue = "";
                if (PS4CheaterNeo.InputBox.Show("Assembly to bytes", "Please enter the assembly language, and the bytes value will be displayed below", ref inputValue, 100) != DialogResult.OK) return;
                if (inputValue.Trim() == "") return;
                
                AsmBox1.Text = "Assembly:\r\n" + inputValue + "\r\n\r\n";
                Assembler.CreateContext<Func<int>>();
                CodeContext<Func<int>> ctx = Assembler.CreateContext<Func<int>>();
                ctx.Emit(inputValue);
                var fn = ctx.Compile(out IntPtr fp, out int codeSize);
                byte[] managedArray = new byte[codeSize];
                Marshal.Copy(fp, managedArray, 0, codeSize);
                for (int idx = 0; idx < managedArray.Length; idx++)
                {
                    if (idx > 0 && idx % 8 == 0) AsmBox1.Text += "\r\n";
                    AsmBox1.Text += managedArray[idx].ToString("X2") + " ";
                }
            }
            catch (Exception ex)
            {
                AsmBox1.Text += string.Format("{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        private void AutoRefreshTimer_Tick(object sender, EventArgs e)
        {
            if (!AutoRefreshBox.Checked) return;

            mutex.WaitOne();
            try
            {
                RefreshBtn.PerformClick();
            }
            finally { mutex.ReleaseMutex(); }
        }

        private void AutoRefreshBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!AutoRefreshBox.Checked) AutoRefreshTimer.Stop();
            AutoRefreshTimer.Enabled = AutoRefreshBox.Checked;
        }

        private void SwapBytesBox_CheckedChanged(object sender, EventArgs e) => HexView_SelectionStartChanged(HexView, e);

        private void HexBox_CheckedChanged(object sender, EventArgs e) => LittleEndianBox.Visible = HexBox.Checked;
        #endregion

        /// <summary>
        /// Round up the result when dividing sum by div.
        /// </summary>
        /// <param name="sum"></param>
        /// <param name="div"></param>
        /// <returns></returns>
        private int DivUP(int sum, int div) => sum / div + ((sum % div != 0) ? 1 : 0);

        /// <summary>
        /// Read the PS4 memory bytes data from a specified section and corresponding page's starting address, 
        /// display it in the HexEditor, and navigate to the specified line and column address.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="line"></param>
        /// <param name="chkChangedPosSet">Confirm if you want to highlight the addresses where bytes have been changed.</param>
        private void UpdateUi(int page, long line, bool chkChangedPosSet = false)
        {
            int memSize = PageSize;
            long ScrollVpos = HexView.ScrollVpos > 0 ? HexView.ScrollVpos : 0;
            HexView.LineInfoOffset = (long)section.Start + (long)(PageSize * page);
            if (section.Length - PageSize * page < memSize) memSize = section.Length - PageSize * page;

            byte[] dst = PS4Tool.ReadMemory(mainForm.ProcessPid, section.Start + (ulong)page * PageSize, (int)memSize);

            HashSet<long> changedPosSet = new HashSet<long>();
            if (HexView.ByteProvider != null)
            {
                HexView.ByteProvider.Changed -= ByteProvider_Changed;
                if (chkChangedPosSet)
                {
                    DynamicByteProvider oldBP = (DynamicByteProvider)HexView.ByteProvider;
                    for (int idx = 0; idx < dst.Length; idx++) if (oldBP.Bytes[idx] != dst[idx]) changedPosSet.Add(idx);
                }
            }
            HexView.ByteProvider = new DynamicByteProvider(dst, changedPosSet);
            HexView.ByteProvider.Changed += ByteProvider_Changed;

            if (line != 0)
            {
                HexView.SelectionStart = line * HexView.BytesPerLine + Column;
                HexView.SelectionLength = 4;
                if (HexView.ColumnInfoVisible) line -= 2;

                HexView.ScrollByteIntoView((line + HexView.Height / (int)HexView.CharSize.Height - 1) * HexView.BytesPerLine + Column);
                if (ScrollVpos > 0) HexView.PerformScrollToLine(ScrollVpos);
            }
        }

        private void ByteProvider_Changed(object sender, EventArgs e)
        {
            if (HexView.SelectionStart < 0) return;

            if (!changedPosDic.TryGetValue(HexView.SelectionStart, out long changedLen)) changedPosDic.Add(HexView.SelectionStart, HexView.SelectionLength);
            else if (changedLen < HexView.SelectionLength) changedPosDic[HexView.SelectionStart] = HexView.SelectionLength;
        }
    }
}
