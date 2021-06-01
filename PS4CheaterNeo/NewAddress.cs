using libdebug;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static PS4CheaterNeo.SectionTool;

namespace PS4CheaterNeo
{
    public partial class NewAddress : Form
    {
        readonly Main mainForm;
        bool isCheckAddressBox;

        Button AddOffsetBtn;
        Button DelOffsetBtn;
        List<TextBox> OffsetBoxList;
        List<Label> OffsetLabelList;

        public ulong Address { get; private set; }
        public ulong Value { get; private set; }
        public ScanType CheatType { get; private set; }
        public bool IsLock { get; private set; }
        public bool IsPointer { get; private set; }
        public string Descriptioin { get; private set; }
        public List<long> OffsetList { get; private set; }
        public Section AddrSection { get; private set; }
        public Section BaseSection { get; private set; }
        public NewAddress(Main mainForm, Section section, ulong address, ScanType scanType, ulong value, bool cheatLock, string cheatDesc, bool isEdit) : 
            this(mainForm, section, null, address, scanType, value, cheatLock, cheatDesc, null, isEdit)
        {
        }
        public NewAddress(Main mainForm, Section addrSection, Section baseSection, ulong address, ScanType scanType, ulong value, bool cheatLock, string cheatDesc, List<long> offsetList, bool isEdit)
        {
            InitializeComponent();

            if (mainForm.ProcessName == "") throw new Exception("No Process currently");

            AddOffsetBtn = new Button();
            DelOffsetBtn = new Button();
            OffsetBoxList = new List<TextBox>();
            OffsetLabelList = new List<Label>();

            ProcessMap pMap = PS4Tool.GetProcessMaps(mainForm.ProcessName);
            mainForm.sectionTool.InitSectionList(pMap);

            this.mainForm = mainForm;
            AddrSection = addrSection;

            Address = address;
            CheatType = scanType;
            Value = value;
            IsLock = cheatLock;
            Descriptioin = cheatDesc;
            IsPointer = offsetList != null;

            AddressBox.Text = Address.ToString("X");
            ScanTypeBox.SelectedIndex = ScanTypeBox.FindStringExact(CheatType.GetDescription()); //ToString().Replace("_", " "));
            ValueBox.Text = Value != 0 ? ScanTool.ULongToString(CheatType, Value) : "0";
            LockBox.Checked = IsLock;
            DescriptionBox.Text = Descriptioin;
            PointerBox.Checked = IsPointer;
            if (IsPointer)
            {
                BaseSection = baseSection;
                if (AddrSection == null) AddrSection = BaseSection;
                OffsetList = new List<long>(offsetList);
                ValueBox.Enabled = false;
            }
            if (isEdit)
            {
                AddressBox.ReadOnly = true;
                if (!IsPointer) PointerBox.Enabled = false;
            }
            isCheckAddressBox = true;
        }

        private void NewAddress_Load(object sender, EventArgs e)
        {
            foreach (ScanType filterEnum in (ScanType[])Enum.GetValues(typeof(ScanType)))
            {
                if (filterEnum == ScanType.Group) continue;
                string scanTypeStr = filterEnum.GetDescription();// filterEnum.ToString().Replace("_", " ");
                ComboboxItem item = new ComboboxItem(scanTypeStr, filterEnum);
                ScanTypeBox.Items.Add(item);
                if (filterEnum == CheatType) ScanTypeBox.SelectedItem = item;
            }

            if (OffsetList != null && OffsetList.Count > 0)
            {
                foreach (long offset in OffsetList)
                {
                    AddOffsetBtn.PerformClick();
                    OffsetBoxList[OffsetBoxList.Count - 1].Text = offset.ToString("X");
                }
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Address = ulong.Parse(AddressBox.Text, System.Globalization.NumberStyles.HexNumber);
                CheatType = (ScanType)((ComboboxItem)(ScanTypeBox.SelectedItem)).Value;
                Value = ScanTool.ValueStringToULong(CheatType, ValueBox.Text);
                ulong valueULong = ScanTool.ValueStringToULong(CheatType, ValueBox.Text);
                IsLock = LockBox.Checked;
                Descriptioin = DescriptionBox.Text;

                if (!AddressBox.ReadOnly) mainForm.AddToCheatGrid(AddrSection, Address - AddrSection.Start, CheatType, valueULong, IsLock, Descriptioin, IsPointer, OffsetList);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddressBox_Leave(object sender, EventArgs e)
        {
            if (!isCheckAddressBox) return;
            try
            {
                Address = ulong.Parse(AddressBox.Text, System.Globalization.NumberStyles.HexNumber);
                int SID = mainForm.sectionTool.GetSectionID(Address);
                if (SID == -1) throw new Exception("Address verification failed");

                AddrSection = mainForm.sectionTool.GetSection(SID);
            }
            catch
            {
                //MessageBox.Show(exception.Message, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void PointerBox_CheckedChanged(object sender, EventArgs e)
        {

            Point savePosition = SaveBtn.Location;
            savePosition.Y = PointerBox.Location.Y + PointerBox.Height + 5;

            Point cancelPosition = CloseBtn.Location;
            cancelPosition.Y = PointerBox.Location.Y + PointerBox.Height + 5;

            if (PointerBox.Checked)
            {
                OffsetList = new List<long>();
                IsPointer = true;
                savePosition.Y += 30;
                cancelPosition.Y += 30;

                AddOffsetBtn.Text = "Add Offset";
                AddOffsetBtn.BackColor = this.BackColor;
                AddOffsetBtn.ForeColor = this.ForeColor;
                AddOffsetBtn.FlatStyle = FlatStyle.Flat;
                AddOffsetBtn.Size = SaveBtn.Size;
                AddOffsetBtn.Location = SaveBtn.Location;
                AddOffsetBtn.Click -= AddOffset_Click;
                AddOffsetBtn.Click += AddOffset_Click;
                this.Controls.Add(AddOffsetBtn);

                DelOffsetBtn.Text = "Del Offset";
                DelOffsetBtn.BackColor = this.BackColor;
                DelOffsetBtn.ForeColor = this.ForeColor;
                DelOffsetBtn.FlatStyle = FlatStyle.Flat;
                DelOffsetBtn.Size = CloseBtn.Size;
                DelOffsetBtn.Location = CloseBtn.Location;
                DelOffsetBtn.Click -= DelOffset_Click;
                DelOffsetBtn.Click += DelOffset_Click;
                this.Controls.Add(DelOffsetBtn);

                AddressBox.Enabled = false;
            }
            else
            {
                IsPointer = false;

                this.Controls.Remove(DelOffsetBtn);
                this.Controls.Remove(AddOffsetBtn);

                for (int i = 0; i < OffsetBoxList.Count; ++i)
                {
                    this.Controls.Remove(OffsetBoxList[i]);
                    this.Controls.Remove(OffsetLabelList[i]);
                }

                OffsetLabelList.Clear();
                OffsetBoxList.Clear();


                AddressBox.Enabled = true;
            }

            SaveBtn.Location = savePosition;
            CloseBtn.Location = cancelPosition;
            this.Height = savePosition.Y + SaveBtn.Height + 50;
        }

        private void DelOffset_Click(object sender, EventArgs e)
        {
            if (OffsetLabelList.Count == 0) return;

            TextBox textBox = OffsetBoxList[OffsetLabelList.Count - 1];
            this.Controls.Remove(textBox);
            OffsetBoxList.RemoveAt(OffsetLabelList.Count - 1);

            Label label = OffsetLabelList[OffsetLabelList.Count - 1];
            this.Controls.Remove(label);
            OffsetLabelList.RemoveAt(OffsetLabelList.Count - 1);

            Point addOffsetPosition = AddOffsetBtn.Location;
            addOffsetPosition.Y -= 30;
            AddOffsetBtn.Location = addOffsetPosition;

            Point delOffsetPosition = DelOffsetBtn.Location;
            delOffsetPosition.Y -= 30;
            DelOffsetBtn.Location = delOffsetPosition;

            Point savePosition = SaveBtn.Location;
            savePosition.Y -= 30;
            SaveBtn.Location = savePosition;

            Point cancelPosition = CloseBtn.Location;
            cancelPosition.Y -= 30;
            CloseBtn.Location = cancelPosition;

            this.Height -= 30;
        }

        private void AddOffset_Click(object sender, EventArgs e)
        {
            TextBox textBox = new TextBox();
            textBox.Text = "0";
            textBox.Size = AddOffsetBtn.Size;
            textBox.Location = AddOffsetBtn.Location;
            textBox.ForeColor = this.ForeColor;
            textBox.BackColor = this.BackColor;
            this.Controls.Add(textBox);
            OffsetBoxList.Add(textBox);

            Label label = new Label();
            label.Text = "";
            label.Size = DelOffsetBtn.Size;
            label.Location = DelOffsetBtn.Location;
            label.ForeColor = this.ForeColor;
            label.BackColor = this.BackColor;
            this.Controls.Add(label);
            OffsetLabelList.Add(label);

            Point addOffsetPosition = AddOffsetBtn.Location;
            addOffsetPosition.Y += 30;
            AddOffsetBtn.Location = addOffsetPosition;

            Point delOffsetPosition = DelOffsetBtn.Location;
            delOffsetPosition.Y += 30;
            DelOffsetBtn.Location = delOffsetPosition;

            Point savePosition = SaveBtn.Location;
            savePosition.Y += 30;
            SaveBtn.Location = savePosition;

            Point cancelPosition = CloseBtn.Location;
            cancelPosition.Y += 30;
            CloseBtn.Location = cancelPosition;

            this.Height += 30;
        }

        private void RefreshPointerChecker_Tick(object sender, EventArgs e)
        {
            if (!IsPointer || OffsetList == null) return;
            if (!IsHandleCreated) return;

            try
            {
                CheatType = (ScanType)((ComboboxItem)(ScanTypeBox.SelectedItem)).Value;

                long baseAddress = 0;

                for (int idx = 0; idx < OffsetBoxList.Count; ++idx)
                {
                    long address = long.Parse(OffsetBoxList[idx].Text, System.Globalization.NumberStyles.HexNumber);

                    if (idx == 0 && address == 0) break;
                    else if (idx == 0 && BaseSection == null) BaseSection = mainForm.sectionTool.GetSection(mainForm.sectionTool.GetSectionID((ulong)address));

                    if (BaseSection == null) break;

                    if (OffsetBoxList.Count > OffsetList.Count) OffsetList.Add(address);
                    else OffsetList[idx] = address;

                    if (idx != OffsetBoxList.Count - 1)
                    {
                        if (AddrSection == null) AddrSection = mainForm.sectionTool.GetSection(mainForm.sectionTool.GetSectionID((ulong)(address + baseAddress)));
                        byte[] nextAddress = PS4Tool.ReadMemory(AddrSection.PID, (ulong)(address + baseAddress), 8);
                        baseAddress = BitConverter.ToInt64(nextAddress, 0);
                        OffsetLabelList[idx].Text = baseAddress.ToString("X");
                    }
                    else
                    {
                        if (address == 0 && baseAddress == 0) continue;
                        byte[] data = PS4Tool.ReadMemory(BaseSection.PID, (ulong)(address + baseAddress), ScanTool.ScanTypeLengthDict[CheatType]);
                        OffsetLabelList[idx].Text = ScanTool.BytesToString(CheatType, data);
                        AddressBox.Text = (address + baseAddress).ToString("X");
                        if (!ValueBox.Enabled)
                        {
                            ValueBox.Enabled = true;
                            ValueBox.Text = OffsetLabelList[idx].Text;
                        }
                    }
                }
            }
            catch
            {

            }
        }
    }
}
