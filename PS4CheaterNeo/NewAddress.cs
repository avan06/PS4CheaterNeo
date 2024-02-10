using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PS4CheaterNeo
{
    public partial class NewAddress : Form
    {
        readonly Main mainForm;
        bool isCheckAddressBox;

        Label OnLabel;
        Label OffLabel;
        TextBox OnBox;
        TextBox OffBox;
        Button AddOffsetBtn;
        Button DelOffsetBtn;

        public ulong Address { get; private set; }
        public string Value { get; private set; }
        public ScanType CheatType { get; private set; }
        public bool IsLock { get; private set; }
        public bool IsPointer { get; private set; }
        public string Descriptioin { get; private set; }
        public List<long> PointerOffsets { get; private set; }
        public Section AddrSection { get; private set; }
        public Section BaseSection { get; private set; }
        public string OnValue { get; private set; }
        public string OffValue { get; private set; }
        public NewAddress(Main mainForm, Section section, ulong address, ScanType scanType, string value, bool cheatLock, string cheatDesc, bool isEdit, string onValue = null, string offValue = null) : 
            this(mainForm, section, null, address, scanType, value, cheatLock, cheatDesc, null, isEdit, onValue, offValue) { }
        public NewAddress(Main mainForm, Section addrSection, Section baseSection, ulong address, ScanType scanType, string value, bool cheatLock, string cheatDesc, List<long> pointerOffsets, bool isEdit, string onValue = null, string offValue = null)
        {
            this.Font = mainForm.Font;
            InitializeComponent();
            ApplyUI(mainForm.langJson);

            if (mainForm.ProcessName == "") throw new Exception("No Process currently");

            OnLabel  = new Label();
            OffLabel = new Label();
            OnBox    = new TextBox();
            OffBox   = new TextBox();

            OnLabel.AutoSize = true;
            OnLabel.Dock = DockStyle.Fill;
            OnLabel.Margin   = AddressLabel.Margin;
            OnLabel.Name     = "OnLabel";
            OnLabel.TabIndex = 1;
            OnLabel.Text     = "On";
            OnLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            OffLabel.AutoSize  = true;
            OffLabel.Dock      = DockStyle.Fill;
            OffLabel.ForeColor = ForeColor;
            OffLabel.Margin    = ValueLabel.Margin;
            OffLabel.Name      = "OffLabel";
            OffLabel.Text      = "Off";
            OffLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            OnBox.BackColor = BackColor;
            OnBox.ForeColor = ForeColor;
            OnBox.Dock   = DockStyle.Fill;
            OnBox.Margin = ValueBox.Margin;
            OnBox.Name   = "OnBox";
            if (onValue  != null) OnBox.Text = onValue.ToString();

            OffBox.BackColor = BackColor;
            OffBox.ForeColor = ForeColor;
            OffBox.Dock   = DockStyle.Fill;
            OffBox.Margin = ValueBox.Margin;
            OffBox.Name   = "OffBox";
            if (offValue != null) OffBox.Text = offValue.ToString();

            OnOffBox.Checked = onValue != null;

            AddOffsetBtn = new Button();
            DelOffsetBtn = new Button();

            AddOffsetBtn.Text      = "Add Offset";
            AddOffsetBtn.BackColor = BackColor;
            AddOffsetBtn.ForeColor = ForeColor;
            AddOffsetBtn.FlatStyle = FlatStyle.Flat;
            AddOffsetBtn.Size      = SaveBtn.Size;
            AddOffsetBtn.Click    -= AddOffset_Click;
            AddOffsetBtn.Click    += AddOffset_Click;

            DelOffsetBtn.Text      = "Del Offset";
            DelOffsetBtn.BackColor = BackColor;
            DelOffsetBtn.ForeColor = ForeColor;
            DelOffsetBtn.FlatStyle = FlatStyle.Flat;
            DelOffsetBtn.Size      = CloseBtn.Size;
            DelOffsetBtn.Click    -= DelOffset_Click;
            DelOffsetBtn.Click    += DelOffset_Click;

            mainForm.sectionTool.InitSections(mainForm.ProcessName);

            this.mainForm = mainForm;
            AddrSection = addrSection;

            Address = address;
            CheatType = scanType;
            Value = (value ?? "") == "" ? "0" : value;
            IsLock = cheatLock;
            Descriptioin = cheatDesc;
            IsPointer = pointerOffsets != null;

            AddressBox.Text = Address.ToString("X");
            ScanTypeBox.SelectedIndex = ScanTypeBox.FindStringExact(CheatType.GetDescription());
            ValueBox.Text = Value;
            LockBox.Checked = IsLock;
            DescriptionBox.Text = Descriptioin;
            PointerBox.Checked = IsPointer;
            if (IsPointer)
            {
                BaseSection = baseSection;
                if (AddrSection == null) AddrSection = BaseSection;
                PointerOffsets = new List<long>(pointerOffsets);
                ValueBox.Enabled = false;
            }
            if (isEdit)
            {
                this.Text = "EditAddress";
                AddressBox.ReadOnly = true;
                if (!IsPointer) PointerBox.Enabled = false;
            }
            isCheckAddressBox = true;
        }

        public void ApplyUI(LanguageJson langJson)
        {
            try
            {
                if (langJson != null)
                {
                    AddressLabel.Text     = langJson.NewAddressForm.AddressLabel;
                    ValueLabel.Text       = langJson.NewAddressForm.ValueLabel;
                    TypeLabel.Text        = langJson.NewAddressForm.TypeLabel;

                    LockBox.Text          = langJson.NewAddressForm.LockBox;
                    DescriptionLabel.Text = langJson.NewAddressForm.DescriptionLabel;
                    PointerBox.Text       = langJson.NewAddressForm.PointerBox;
                    SaveBtn.Text          = langJson.NewAddressForm.SaveBtn;
                    CloseBtn.Text         = langJson.NewAddressForm.CloseBtn;
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

                AddressLabel.ForeColor     = ForeColor;
                ValueLabel.ForeColor       = ForeColor;
                TypeLabel.ForeColor        = ForeColor;
                DescriptionLabel.ForeColor = ForeColor;
                LockBox.ForeColor          = ForeColor;
                PointerBox.ForeColor       = ForeColor;
                OnOffBox.ForeColor         = ForeColor;

                AddressBox.ForeColor     = ForeColor;
                AddressBox.BackColor     = BackColor;
                ValueBox.ForeColor       = ForeColor;
                ValueBox.BackColor       = BackColor;
                ScanTypeBox.ForeColor    = ForeColor;
                ScanTypeBox.BackColor    = BackColor;
                DescriptionBox.ForeColor = ForeColor;
                DescriptionBox.BackColor = BackColor;
                SaveBtn.ForeColor        = ForeColor;
                SaveBtn.BackColor        = BackColor;
                CloseBtn.ForeColor       = ForeColor;
                CloseBtn.BackColor       = BackColor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Source + ":ApplyUI", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void NewAddress_Load(object sender, EventArgs e)
        {
            foreach (ScanType filterEnum in (ScanType[])Enum.GetValues(typeof(ScanType)))
            {
                if (filterEnum == ScanType.Group) continue;
                string scanTypeStr = filterEnum.GetDescription();
                ComboItem item = new ComboItem(scanTypeStr, filterEnum);
                ScanTypeBox.Items.Add(item);
                if (filterEnum == CheatType) ScanTypeBox.SelectedItem = item;
            }

            if (PointerOffsets != null && PointerOffsets.Count > 0)
            {
                foreach (long offset in PointerOffsets)
                {
                    AddOffsetBtn.PerformClick();
                    TableLayoutBottomBox.Controls[TableLayoutBottomBox.Controls.Count - 1].Text = offset.ToString("X");
                }
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Address = ulong.Parse(AddressBox.Text, System.Globalization.NumberStyles.HexNumber);
                CheatType = (ScanType)((ComboItem)(ScanTypeBox.SelectedItem)).Value;
                ScanTool.ValueStringToULong(CheatType, ValueBox.Text);
                Value = ValueBox.Text;
                OnValue = string.IsNullOrWhiteSpace(OnBox.Text) ? null : OnBox.Text;
                OffValue = string.IsNullOrWhiteSpace(OffBox.Text) ? null : OffBox.Text;
                IsLock = LockBox.Checked;
                Descriptioin = DescriptionBox.Text;

                if (!AddressBox.ReadOnly) mainForm.AddToCheatGrid(AddrSection, (uint)(Address - AddrSection.Start), CheatType, Value, IsLock, Descriptioin, PointerOffsets, null, -1, true, OnValue, OffValue);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Source + ":SaveBtn_Click", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void CloseBtn_Click(object sender, EventArgs e) => Close();

        private void AddressBox_Leave(object sender, EventArgs e)
        {
            if (!isCheckAddressBox) return;
            try
            {
                AddressBox.Text = Regex.Replace(AddressBox.Text, "[^0-9a-fA-F]", "");
                Address = ulong.Parse(AddressBox.Text, System.Globalization.NumberStyles.HexNumber);
                uint SID = mainForm.sectionTool.GetSectionID(Address);
                if (SID == 0) throw new Exception("Address verification failed"); //-1(int) => 0(uint)

                AddrSection = mainForm.sectionTool.GetSection(SID);
            }
            catch
            {
                //MessageBox.Show(exception.Message, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void PointerBox_CheckedChanged(object sender, EventArgs e)
        {
            if (PointerBox.Checked)
            {
                IsPointer = PointerBox.Checked;
                PointerOffsets = new List<long>();

                TableLayoutBottom.Controls.Add(AddOffsetBtn, 0, 0);
                TableLayoutBottom.Controls.Add(DelOffsetBtn, 1, 0);
                Height += AddOffsetBtn.Height;

                //When enable pointer, set Row 1 size to 100 percentage because the number of controls in Row 1 dynamically adjusts.
                TableLayoutBottom.RowStyles[1].SizeType = SizeType.Percent;
                TableLayoutBottom.RowStyles[1].Height = 100F;

                TableLayoutBottomBox.RowCount = 0;
                TableLayoutBottomBox.RowStyles.Clear();
                TableLayoutBottomBox.Controls.Clear();
                TableLayoutBottomLabel.RowCount = 0;
                TableLayoutBottomLabel.RowStyles.Clear();
                TableLayoutBottomLabel.Controls.Clear();
            }
            else
            {
                if (TableLayoutBottomBox.Height > 1) Height -= TableLayoutBottomBox.Height;
                TableLayoutBottomBox.RowCount = 0;
                TableLayoutBottomBox.RowStyles.Clear();
                TableLayoutBottomBox.Controls.Clear();
                TableLayoutBottomLabel.RowCount = 0;
                TableLayoutBottomLabel.RowStyles.Clear();
                TableLayoutBottomLabel.Controls.Clear();

                TableLayoutBottom.Controls.Remove(AddOffsetBtn);
                TableLayoutBottom.Controls.Remove(DelOffsetBtn);

                TableLayoutBottom.RowStyles[1].SizeType = SizeType.AutoSize;
                TableLayoutBottom.RowStyles[1].Height = 0;
                Height -= AddOffsetBtn.Height;
            }
        }

        private void OnOffBox_CheckedChanged(object sender, EventArgs e)
        {
            if (OnOffBox.Checked)
            {
                TableLayoutBase.Controls.Add(OnLabel, 0, 3);
                TableLayoutBase.Controls.Add(OnBox, 1, 3);
                TableLayoutBase.Controls.Add(OffLabel, 2, 3);
                TableLayoutBase.Controls.Add(OffBox, 3, 3);
                Height += OnBox.Height;
            }
            else
            {
                TableLayoutBase.Controls.Remove(OnLabel);
                TableLayoutBase.Controls.Remove(OnBox);
                TableLayoutBase.Controls.Remove(OffLabel);
                TableLayoutBase.Controls.Remove(OffBox);
                Height -= OnBox.Height;
            }
        }

        private void AddOffset_Click(object sender, EventArgs e) => SetOffsetBoxs(true);

        private void DelOffset_Click(object sender, EventArgs e) => SetOffsetBoxs(false);

        private void SetOffsetBoxs(bool isAdd)
        {
            //TableLayoutBottom's
            // Row 0 consists of AddOffset and DelOffset buttons,
            // Row 1 includes TextBox and Label controls, and
            // Row 2 features Save and Close buttons.
            if (isAdd)
            {
                TextBox textBox = new TextBox
                {
                    Text      = "0",
                    Location  = AddOffsetBtn.Location,
                    ForeColor = ForeColor,
                    BackColor = BackColor,
                    Dock      = DockStyle.Fill,
                    Margin    = new Padding(3)
                };

                TextBox label = new TextBox
                {
                    Text        = "",
                    Location    = DelOffsetBtn.Location,
                    ForeColor   = ForeColor,
                    BackColor   = BackColor,
                    Dock        = DockStyle.Fill,
                    Margin      = new Padding(3, 6, 3, 3),
                    ReadOnly    = true,
                    BorderStyle = BorderStyle.None,
            };
                PointerOffsets.Add(0);
                TableLayoutBottomBox.RowCount += 1;
                TableLayoutBottomBox.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                TableLayoutBottomBox.Controls.Add(textBox, 0, TableLayoutBottomBox.Controls.Count);
                TableLayoutBottomLabel.RowCount += 1;
                TableLayoutBottomLabel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                TableLayoutBottomLabel.Controls.Add(label, 0, TableLayoutBottomLabel.Controls.Count);

                Height += TableLayoutBottomBox.Controls[TableLayoutBottomBox.Controls.Count - 1].Height;
            }
            else
            {
                if (TableLayoutBottomBox.Controls.Count == 0) return;

                PointerOffsets.RemoveAt(PointerOffsets.Count - 1);
                Height -= TableLayoutBottomBox.Controls[TableLayoutBottomBox.Controls.Count - 1].Height;
                TableLayoutBottomBox.Controls.RemoveAt(TableLayoutBottomBox.Controls.Count - 1);
                TableLayoutBottomBox.RowCount -= 1;
                TableLayoutBottomBox.RowStyles.Remove(TableLayoutBottomBox.RowStyles[TableLayoutBottomBox.RowStyles.Count - 1]);
                TableLayoutBottomLabel.Controls.RemoveAt(TableLayoutBottomLabel.Controls.Count - 1);
                TableLayoutBottomLabel.RowCount -= 1;
                TableLayoutBottomLabel.RowStyles.Remove(TableLayoutBottomLabel.RowStyles[TableLayoutBottomLabel.RowStyles.Count - 1]);
            }
        }

        private void ScanTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsPointer || PointerOffsets != null) return;

            var newCheatType = (ScanType)((ComboItem)(ScanTypeBox.SelectedItem)).Value;
            if (CheatType == ScanType.Hex && ValueBox.Text.Length > 16)
            {
                ScanTypeBox.SelectedIndex = ScanTypeBox.FindStringExact(CheatType.GetDescription());
                return;
            }
            else if (newCheatType == ScanType.String_) return;

            try
            {
                var newValue = ScanTool.ValueStringToULong(CheatType, ValueBox.Text);
                if (newValue == 0) return;

                if (newCheatType == ScanType.Byte_ && newValue > byte.MaxValue ||
                    newCheatType == ScanType.Bytes_2 && newValue > UInt16.MaxValue ||
                    newCheatType == ScanType.Bytes_4 && newValue > UInt32.MaxValue ||
                    newCheatType == ScanType.Float_ && newValue > float.MaxValue)
                {
                    ScanTypeBox.SelectedIndex = ScanTypeBox.FindStringExact(CheatType.GetDescription());
                    return;
                }

                var newText = ScanTool.ULongToString(newCheatType, newValue);
                if (newText == "0") return;

                Value = newValue.ToString();
                ValueBox.Text = newText;

                //var newOnValue = ScanTool.ValueStringToULong(CheatType, OnBox.Text);
                //var newOnText = ScanTool.ULongToString(newCheatType, newOnValue);
                //OnValue = newOnValue.ToString();
                //OnBox.Text = newOnText;

                //var newOffValue = ScanTool.ValueStringToULong(CheatType, OffBox.Text);
                //var newOffText = ScanTool.ULongToString(newCheatType, newOffValue);
                //OffValue = newOffValue.ToString();
                //OffBox.Text = newOffText;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Source + ":ScanTypeBox_SelectedIndexChanged", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            CheatType = newCheatType;
        }

        private void RefreshPointerChecker_Tick(object sender, EventArgs e)
        {
            if (!IsPointer || PointerOffsets == null) return;
            if (!IsHandleCreated) return;

            try
            {
                var newCheatType = (ScanType)((ComboItem)(ScanTypeBox.SelectedItem)).Value;
                var changedCheatType = CheatType != newCheatType;
                if (changedCheatType) CheatType = newCheatType;

                long baseAddress = 0;

                for (int idx = 0; idx < TableLayoutBottomBox.Controls.Count; ++idx)
                {
                    long address = long.Parse(TableLayoutBottomBox.Controls[idx].Text, System.Globalization.NumberStyles.HexNumber);

                    if (idx == 0 && address == 0) break;
                    else if (idx == 0 && BaseSection == null) BaseSection = mainForm.sectionTool.GetSection(mainForm.sectionTool.GetSectionID((ulong)address));

                    if (BaseSection == null) break;

                    PointerOffsets[idx] = address;

                    if (idx != TableLayoutBottomBox.Controls.Count - 1)
                    {
                        if (AddrSection == null || AddrSection.SID == 0) AddrSection = mainForm.sectionTool.GetSection(mainForm.sectionTool.GetSectionID((ulong)(address + baseAddress)));
                        byte[] nextAddress = PS4Tool.ReadMemory(AddrSection.PID, (ulong)(address + baseAddress), 8);
                        baseAddress = BitConverter.ToInt64(nextAddress, 0);
                        TableLayoutBottomLabel.Controls[idx].Text = string.Format("=> {0:X2} +", baseAddress);
                    }
                    else
                    {
                        if (address == 0 && baseAddress == 0) continue;
                        int length = 0;
                        if (CheatType != ScanType.Hex) length = ScanTool.ScanTypeLengthDict[CheatType];
                        else if (ValueBox.Text.Length > 1) length = ValueBox.Text.Length / 2;
                        else length = 8;
                        byte[] data = PS4Tool.ReadMemory(BaseSection.PID, (ulong)(address + baseAddress), length);
                        string value = ScanTool.BytesToString(CheatType, data);
                        TableLayoutBottomLabel.Controls[idx].Text = string.Format("=> {0}", value);
                        AddressBox.Text = (address + baseAddress).ToString("X");
                        if (!ValueBox.Enabled || changedCheatType)
                        {
                            ValueBox.Enabled = true;
                            ValueBox.Text = value;
                        }
                    }
                }
            }
            catch (Exception) { }
        }
    }
}
