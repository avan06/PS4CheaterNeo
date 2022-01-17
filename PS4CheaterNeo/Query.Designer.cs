
namespace PS4CheaterNeo
{
    partial class Query
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Query));
            this.SplitContainer1 = new System.Windows.Forms.SplitContainer();
            this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolStripMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.ResultView = new System.Windows.Forms.ListView();
            this.ResultViewAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ResultViewType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ResultViewValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ResultViewHex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ResultViewSection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ResultViewMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ResultViewAddToCheatGrid = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ResultViewHexEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.ResultViewCopyAddress = new System.Windows.Forms.ToolStripMenuItem();
            this.ResultViewDump = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ResultViewFindPointer = new System.Windows.Forms.ToolStripMenuItem();
            this.SplitContainer2 = new System.Windows.Forms.SplitContainer();
            this.SectionSearchBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.AddrMaxBox = new System.Windows.Forms.TextBox();
            this.AddrMinBox = new System.Windows.Forms.TextBox();
            this.SectionView = new System.Windows.Forms.ListView();
            this.SectionViewAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SectionViewName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SectionViewProt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SectionViewLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SectionViewSID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SectionViewOffset = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SectionViewMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SectionViewHexEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.SectionViewDump = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.SectionViewCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.GetProcessesBtn = new System.Windows.Forms.Button();
            this.ProcessesBox = new System.Windows.Forms.ComboBox();
            this.FilterRuleBtn = new System.Windows.Forms.Button();
            this.ScanTypeBox = new System.Windows.Forms.ComboBox();
            this.SelectAllBox = new System.Windows.Forms.CheckBox();
            this.AlignmentBox = new System.Windows.Forms.CheckBox();
            this.IsFilterBox = new System.Windows.Forms.CheckBox();
            this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.HexBox = new System.Windows.Forms.CheckBox();
            this.ValueBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Value1Box = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.NewBtn = new System.Windows.Forms.Button();
            this.ToolStripBar = new System.Windows.Forms.ProgressBar();
            this.RefreshBtn = new System.Windows.Forms.Button();
            this.ScanBtn = new System.Windows.Forms.Button();
            this.CompareTypeBox = new System.Windows.Forms.ComboBox();
            this.Panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).BeginInit();
            this.SplitContainer1.Panel1.SuspendLayout();
            this.SplitContainer1.Panel2.SuspendLayout();
            this.SplitContainer1.SuspendLayout();
            this.StatusStrip1.SuspendLayout();
            this.ResultViewMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer2)).BeginInit();
            this.SplitContainer2.Panel1.SuspendLayout();
            this.SplitContainer2.Panel2.SuspendLayout();
            this.SplitContainer2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SectionViewMenu.SuspendLayout();
            this.TableLayoutPanel1.SuspendLayout();
            this.Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SplitContainer1
            // 
            this.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer1.ForeColor = System.Drawing.Color.White;
            this.SplitContainer1.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer1.Name = "SplitContainer1";
            // 
            // SplitContainer1.Panel1
            // 
            this.SplitContainer1.Panel1.Controls.Add(this.StatusStrip1);
            this.SplitContainer1.Panel1.Controls.Add(this.ResultView);
            // 
            // SplitContainer1.Panel2
            // 
            this.SplitContainer1.Panel2.Controls.Add(this.SplitContainer2);
            this.SplitContainer1.Size = new System.Drawing.Size(800, 450);
            this.SplitContainer1.SplitterDistance = 530;
            this.SplitContainer1.TabIndex = 0;
            // 
            // StatusStrip1
            // 
            this.StatusStrip1.BackColor = System.Drawing.Color.DimGray;
            this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMsg});
            this.StatusStrip1.Location = new System.Drawing.Point(0, 426);
            this.StatusStrip1.Name = "StatusStrip1";
            this.StatusStrip1.Size = new System.Drawing.Size(528, 22);
            this.StatusStrip1.SizingGrip = false;
            this.StatusStrip1.TabIndex = 0;
            this.StatusStrip1.Text = "statusStrip1";
            // 
            // ToolStripMsg
            // 
            this.ToolStripMsg.AutoToolTip = true;
            this.ToolStripMsg.BackColor = System.Drawing.Color.Transparent;
            this.ToolStripMsg.ForeColor = System.Drawing.Color.White;
            this.ToolStripMsg.Name = "ToolStripMsg";
            this.ToolStripMsg.Size = new System.Drawing.Size(513, 17);
            this.ToolStripMsg.Spring = true;
            this.ToolStripMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ResultView
            // 
            this.ResultView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ResultView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ResultViewAddress,
            this.ResultViewType,
            this.ResultViewValue,
            this.ResultViewHex,
            this.ResultViewSection});
            this.ResultView.ContextMenuStrip = this.ResultViewMenu;
            this.ResultView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultView.ForeColor = System.Drawing.Color.White;
            this.ResultView.FullRowSelect = true;
            this.ResultView.HideSelection = false;
            this.ResultView.Location = new System.Drawing.Point(0, 0);
            this.ResultView.Name = "ResultView";
            this.ResultView.Size = new System.Drawing.Size(528, 448);
            this.ResultView.TabIndex = 0;
            this.ResultView.UseCompatibleStateImageBehavior = false;
            this.ResultView.View = System.Windows.Forms.View.Details;
            this.ResultView.DoubleClick += new System.EventHandler(this.ResultView_DoubleClick);
            // 
            // ResultViewAddress
            // 
            this.ResultViewAddress.Text = "Address";
            this.ResultViewAddress.Width = 120;
            // 
            // ResultViewType
            // 
            this.ResultViewType.Text = "Type";
            // 
            // ResultViewValue
            // 
            this.ResultViewValue.Text = "Value";
            // 
            // ResultViewHex
            // 
            this.ResultViewHex.Text = "Hex";
            this.ResultViewHex.Width = 120;
            // 
            // ResultViewSection
            // 
            this.ResultViewSection.Text = "Section";
            this.ResultViewSection.Width = 250;
            // 
            // ResultViewMenu
            // 
            this.ResultViewMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ResultViewAddToCheatGrid,
            this.ToolStripSeparator1,
            this.ResultViewHexEditor,
            this.ResultViewCopyAddress,
            this.ResultViewDump,
            this.ToolStripSeparator2,
            this.ResultViewFindPointer});
            this.ResultViewMenu.Name = "ResultViewMenu";
            this.ResultViewMenu.Size = new System.Drawing.Size(177, 126);
            // 
            // ResultViewAddToCheatGrid
            // 
            this.ResultViewAddToCheatGrid.Name = "ResultViewAddToCheatGrid";
            this.ResultViewAddToCheatGrid.Size = new System.Drawing.Size(176, 22);
            this.ResultViewAddToCheatGrid.Text = "Add to Cheat Grid";
            this.ResultViewAddToCheatGrid.Click += new System.EventHandler(this.ResultViewAddToCheatGrid_Click);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(173, 6);
            // 
            // ResultViewHexEditor
            // 
            this.ResultViewHexEditor.Name = "ResultViewHexEditor";
            this.ResultViewHexEditor.Size = new System.Drawing.Size(176, 22);
            this.ResultViewHexEditor.Text = "Hex Editor";
            this.ResultViewHexEditor.Click += new System.EventHandler(this.ResultViewHexEditor_Click);
            // 
            // ResultViewCopyAddress
            // 
            this.ResultViewCopyAddress.Name = "ResultViewCopyAddress";
            this.ResultViewCopyAddress.Size = new System.Drawing.Size(176, 22);
            this.ResultViewCopyAddress.Text = "Copy Address";
            this.ResultViewCopyAddress.Click += new System.EventHandler(this.ResultViewCopyAddress_Click);
            // 
            // ResultViewDump
            // 
            this.ResultViewDump.Enabled = false;
            this.ResultViewDump.Name = "ResultViewDump";
            this.ResultViewDump.Size = new System.Drawing.Size(176, 22);
            this.ResultViewDump.Text = "Dump";
            this.ResultViewDump.Click += new System.EventHandler(this.ResultViewDump_Click);
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(173, 6);
            // 
            // ResultViewFindPointer
            // 
            this.ResultViewFindPointer.Name = "ResultViewFindPointer";
            this.ResultViewFindPointer.Size = new System.Drawing.Size(176, 22);
            this.ResultViewFindPointer.Text = "Find Pointer";
            this.ResultViewFindPointer.Click += new System.EventHandler(this.ResultViewFindPointer_Click);
            // 
            // SplitContainer2
            // 
            this.SplitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer2.ForeColor = System.Drawing.Color.White;
            this.SplitContainer2.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer2.Name = "SplitContainer2";
            this.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer2.Panel1
            // 
            this.SplitContainer2.Panel1.Controls.Add(this.SectionSearchBtn);
            this.SplitContainer2.Panel1.Controls.Add(this.tableLayoutPanel2);
            this.SplitContainer2.Panel1.Controls.Add(this.SectionView);
            this.SplitContainer2.Panel1.Controls.Add(this.GetProcessesBtn);
            this.SplitContainer2.Panel1.Controls.Add(this.ProcessesBox);
            // 
            // SplitContainer2.Panel2
            // 
            this.SplitContainer2.Panel2.Controls.Add(this.FilterRuleBtn);
            this.SplitContainer2.Panel2.Controls.Add(this.ScanTypeBox);
            this.SplitContainer2.Panel2.Controls.Add(this.SelectAllBox);
            this.SplitContainer2.Panel2.Controls.Add(this.AlignmentBox);
            this.SplitContainer2.Panel2.Controls.Add(this.IsFilterBox);
            this.SplitContainer2.Panel2.Controls.Add(this.TableLayoutPanel1);
            this.SplitContainer2.Panel2.Controls.Add(this.NewBtn);
            this.SplitContainer2.Panel2.Controls.Add(this.ToolStripBar);
            this.SplitContainer2.Panel2.Controls.Add(this.RefreshBtn);
            this.SplitContainer2.Panel2.Controls.Add(this.ScanBtn);
            this.SplitContainer2.Panel2.Controls.Add(this.CompareTypeBox);
            this.SplitContainer2.Size = new System.Drawing.Size(266, 450);
            this.SplitContainer2.SplitterDistance = 255;
            this.SplitContainer2.TabIndex = 0;
            // 
            // SectionSearchBtn
            // 
            this.SectionSearchBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SectionSearchBtn.BackColor = System.Drawing.SystemColors.Desktop;
            this.SectionSearchBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SectionSearchBtn.BackgroundImage")));
            this.SectionSearchBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.SectionSearchBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SectionSearchBtn.ForeColor = System.Drawing.Color.White;
            this.SectionSearchBtn.Location = new System.Drawing.Point(228, 32);
            this.SectionSearchBtn.Name = "SectionSearchBtn";
            this.SectionSearchBtn.Size = new System.Drawing.Size(33, 20);
            this.SectionSearchBtn.TabIndex = 3;
            this.SectionSearchBtn.UseVisualStyleBackColor = false;
            this.SectionSearchBtn.Click += new System.EventHandler(this.SectionSearchBtn_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label5, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.AddrMaxBox, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.AddrMinBox, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(-1, 222);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(264, 28);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "Min:";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(135, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 20;
            this.label5.Text = "Max:";
            // 
            // AddrMaxBox
            // 
            this.AddrMaxBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddrMaxBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.AddrMaxBox.ForeColor = System.Drawing.Color.White;
            this.AddrMaxBox.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.AddrMaxBox.Location = new System.Drawing.Point(170, 3);
            this.AddrMaxBox.Name = "AddrMaxBox";
            this.AddrMaxBox.Size = new System.Drawing.Size(91, 22);
            this.AddrMaxBox.TabIndex = 19;
            // 
            // AddrMinBox
            // 
            this.AddrMinBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddrMinBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.AddrMinBox.ForeColor = System.Drawing.Color.White;
            this.AddrMinBox.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.AddrMinBox.Location = new System.Drawing.Point(38, 3);
            this.AddrMinBox.Name = "AddrMinBox";
            this.AddrMinBox.Size = new System.Drawing.Size(91, 22);
            this.AddrMinBox.TabIndex = 18;
            // 
            // SectionView
            // 
            this.SectionView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SectionView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.SectionView.CheckBoxes = true;
            this.SectionView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SectionViewAddress,
            this.SectionViewName,
            this.SectionViewProt,
            this.SectionViewLength,
            this.SectionViewSID,
            this.SectionViewOffset});
            this.SectionView.ContextMenuStrip = this.SectionViewMenu;
            this.SectionView.ForeColor = System.Drawing.Color.White;
            this.SectionView.HideSelection = false;
            this.SectionView.Location = new System.Drawing.Point(3, 58);
            this.SectionView.Name = "SectionView";
            this.SectionView.Size = new System.Drawing.Size(260, 164);
            this.SectionView.TabIndex = 2;
            this.SectionView.UseCompatibleStateImageBehavior = false;
            this.SectionView.View = System.Windows.Forms.View.Details;
            this.SectionView.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.SectionView_ItemCheck);
            // 
            // SectionViewAddress
            // 
            this.SectionViewAddress.Text = "Address";
            this.SectionViewAddress.Width = 85;
            // 
            // SectionViewName
            // 
            this.SectionViewName.Text = "Name";
            this.SectionViewName.Width = 95;
            // 
            // SectionViewProt
            // 
            this.SectionViewProt.Text = "Prot";
            this.SectionViewProt.Width = 24;
            // 
            // SectionViewLength
            // 
            this.SectionViewLength.Text = "Length";
            this.SectionViewLength.Width = 65;
            // 
            // SectionViewSID
            // 
            this.SectionViewSID.Text = "ID";
            this.SectionViewSID.Width = 0;
            // 
            // SectionViewOffset
            // 
            this.SectionViewOffset.Text = "Offset";
            // 
            // SectionViewMenu
            // 
            this.SectionViewMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SectionViewHexEditor,
            this.SectionViewDump,
            this.ToolStripSeparator3,
            this.SectionViewCheck});
            this.SectionViewMenu.Name = "SectionViewMenu";
            this.SectionViewMenu.Size = new System.Drawing.Size(134, 76);
            // 
            // SectionViewHexEditor
            // 
            this.SectionViewHexEditor.Name = "SectionViewHexEditor";
            this.SectionViewHexEditor.Size = new System.Drawing.Size(133, 22);
            this.SectionViewHexEditor.Text = "Hex Editor";
            this.SectionViewHexEditor.Click += new System.EventHandler(this.SectionViewHexEditor_Click);
            // 
            // SectionViewDump
            // 
            this.SectionViewDump.Enabled = false;
            this.SectionViewDump.Name = "SectionViewDump";
            this.SectionViewDump.Size = new System.Drawing.Size(133, 22);
            this.SectionViewDump.Text = "Dump";
            this.SectionViewDump.Click += new System.EventHandler(this.SectionViewDump_Click);
            // 
            // ToolStripSeparator3
            // 
            this.ToolStripSeparator3.Name = "ToolStripSeparator3";
            this.ToolStripSeparator3.Size = new System.Drawing.Size(130, 6);
            // 
            // SectionViewCheck
            // 
            this.SectionViewCheck.Name = "SectionViewCheck";
            this.SectionViewCheck.Size = new System.Drawing.Size(133, 22);
            this.SectionViewCheck.Text = "Check";
            this.SectionViewCheck.Click += new System.EventHandler(this.SectionViewCheck_Click);
            // 
            // GetProcessesBtn
            // 
            this.GetProcessesBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GetProcessesBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.GetProcessesBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GetProcessesBtn.ForeColor = System.Drawing.Color.White;
            this.GetProcessesBtn.Location = new System.Drawing.Point(3, 3);
            this.GetProcessesBtn.Name = "GetProcessesBtn";
            this.GetProcessesBtn.Size = new System.Drawing.Size(257, 23);
            this.GetProcessesBtn.TabIndex = 1;
            this.GetProcessesBtn.Text = "Refresh Processes";
            this.GetProcessesBtn.UseVisualStyleBackColor = false;
            this.GetProcessesBtn.Click += new System.EventHandler(this.GetProcessesBtn_Click);
            // 
            // ProcessesBox
            // 
            this.ProcessesBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProcessesBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ProcessesBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProcessesBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ProcessesBox.ForeColor = System.Drawing.Color.White;
            this.ProcessesBox.FormattingEnabled = true;
            this.ProcessesBox.Location = new System.Drawing.Point(3, 32);
            this.ProcessesBox.Name = "ProcessesBox";
            this.ProcessesBox.Size = new System.Drawing.Size(221, 20);
            this.ProcessesBox.TabIndex = 0;
            this.ProcessesBox.SelectedIndexChanged += new System.EventHandler(this.ProcessesBox_SelectedIndexChanged);
            // 
            // FilterRuleBtn
            // 
            this.FilterRuleBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FilterRuleBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.FilterRuleBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FilterRuleBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.FilterRuleBtn.ForeColor = System.Drawing.Color.White;
            this.FilterRuleBtn.Location = new System.Drawing.Point(228, 0);
            this.FilterRuleBtn.Name = "FilterRuleBtn";
            this.FilterRuleBtn.Size = new System.Drawing.Size(35, 22);
            this.FilterRuleBtn.TabIndex = 2;
            this.FilterRuleBtn.Text = "Rule";
            this.FilterRuleBtn.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.FilterRuleBtn.UseVisualStyleBackColor = false;
            this.FilterRuleBtn.Click += new System.EventHandler(this.FilterRuleBtn_Click);
            // 
            // ScanTypeBox
            // 
            this.ScanTypeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScanTypeBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ScanTypeBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ScanTypeBox.ForeColor = System.Drawing.Color.White;
            this.ScanTypeBox.FormattingEnabled = true;
            this.ScanTypeBox.Location = new System.Drawing.Point(3, 64);
            this.ScanTypeBox.Name = "ScanTypeBox";
            this.ScanTypeBox.Size = new System.Drawing.Size(260, 20);
            this.ScanTypeBox.TabIndex = 10;
            this.ScanTypeBox.SelectedIndexChanged += new System.EventHandler(this.ScanTypeBox_SelectedIndexChanged);
            // 
            // SelectAllBox
            // 
            this.SelectAllBox.AutoSize = true;
            this.SelectAllBox.ForeColor = System.Drawing.Color.White;
            this.SelectAllBox.Location = new System.Drawing.Point(3, 3);
            this.SelectAllBox.Name = "SelectAllBox";
            this.SelectAllBox.Size = new System.Drawing.Size(68, 16);
            this.SelectAllBox.TabIndex = 0;
            this.SelectAllBox.Text = "Select All";
            this.SelectAllBox.UseVisualStyleBackColor = true;
            this.SelectAllBox.CheckedChanged += new System.EventHandler(this.SelectAllBox_CheckedChanged);
            // 
            // AlignmentBox
            // 
            this.AlignmentBox.AutoSize = true;
            this.AlignmentBox.Checked = true;
            this.AlignmentBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AlignmentBox.ForeColor = System.Drawing.Color.Silver;
            this.AlignmentBox.Location = new System.Drawing.Point(72, 3);
            this.AlignmentBox.Name = "AlignmentBox";
            this.AlignmentBox.Size = new System.Drawing.Size(73, 16);
            this.AlignmentBox.TabIndex = 1;
            this.AlignmentBox.Text = "Alignment";
            this.AlignmentBox.UseVisualStyleBackColor = true;
            // 
            // IsFilterBox
            // 
            this.IsFilterBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.IsFilterBox.AutoSize = true;
            this.IsFilterBox.ForeColor = System.Drawing.Color.White;
            this.IsFilterBox.Location = new System.Drawing.Point(182, 3);
            this.IsFilterBox.Name = "IsFilterBox";
            this.IsFilterBox.Size = new System.Drawing.Size(48, 16);
            this.IsFilterBox.TabIndex = 3;
            this.IsFilterBox.Text = "Filter";
            this.IsFilterBox.UseVisualStyleBackColor = true;
            this.IsFilterBox.CheckedChanged += new System.EventHandler(this.IsFilterBox_CheckedChanged);
            // 
            // TableLayoutPanel1
            // 
            this.TableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TableLayoutPanel1.ColumnCount = 4;
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.TableLayoutPanel1.Controls.Add(this.HexBox, 0, 1);
            this.TableLayoutPanel1.Controls.Add(this.ValueBox, 1, 1);
            this.TableLayoutPanel1.Controls.Add(this.label3, 2, 1);
            this.TableLayoutPanel1.Controls.Add(this.Value1Box, 3, 1);
            this.TableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.TableLayoutPanel1.Controls.Add(this.label2, 3, 0);
            this.TableLayoutPanel1.Location = new System.Drawing.Point(3, 21);
            this.TableLayoutPanel1.Name = "TableLayoutPanel1";
            this.TableLayoutPanel1.RowCount = 2;
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel1.Size = new System.Drawing.Size(260, 40);
            this.TableLayoutPanel1.TabIndex = 1;
            // 
            // HexBox
            // 
            this.HexBox.AutoSize = true;
            this.HexBox.Location = new System.Drawing.Point(3, 15);
            this.HexBox.Name = "HexBox";
            this.HexBox.Size = new System.Drawing.Size(43, 16);
            this.HexBox.TabIndex = 4;
            this.HexBox.Text = "Hex";
            this.HexBox.UseVisualStyleBackColor = true;
            // 
            // ValueBox
            // 
            this.ValueBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ValueBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ValueBox.ForeColor = System.Drawing.Color.White;
            this.ValueBox.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ValueBox.Location = new System.Drawing.Point(52, 15);
            this.ValueBox.Name = "ValueBox";
            this.ValueBox.Size = new System.Drawing.Size(88, 22);
            this.ValueBox.TabIndex = 6;
            this.ValueBox.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(146, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 36);
            this.label3.TabIndex = 8;
            this.label3.Text = "and";
            // 
            // Value1Box
            // 
            this.Value1Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Value1Box.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.Value1Box.ForeColor = System.Drawing.Color.White;
            this.Value1Box.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.Value1Box.Location = new System.Drawing.Point(167, 15);
            this.Value1Box.Name = "Value1Box";
            this.Value1Box.Size = new System.Drawing.Size(90, 22);
            this.Value1Box.TabIndex = 9;
            this.Value1Box.Text = "0";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "Value:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(167, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "Value:";
            // 
            // NewBtn
            // 
            this.NewBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NewBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.NewBtn.Enabled = false;
            this.NewBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NewBtn.Location = new System.Drawing.Point(212, 116);
            this.NewBtn.Name = "NewBtn";
            this.NewBtn.Size = new System.Drawing.Size(50, 23);
            this.NewBtn.TabIndex = 17;
            this.NewBtn.Text = "New";
            this.NewBtn.UseVisualStyleBackColor = false;
            this.NewBtn.Click += new System.EventHandler(this.NewBtn_Click);
            // 
            // ToolStripBar
            // 
            this.ToolStripBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ToolStripBar.Location = new System.Drawing.Point(3, 177);
            this.ToolStripBar.Name = "ToolStripBar";
            this.ToolStripBar.Size = new System.Drawing.Size(260, 10);
            this.ToolStripBar.TabIndex = 16;
            // 
            // RefreshBtn
            // 
            this.RefreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RefreshBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.RefreshBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RefreshBtn.Location = new System.Drawing.Point(3, 146);
            this.RefreshBtn.Name = "RefreshBtn";
            this.RefreshBtn.Size = new System.Drawing.Size(260, 23);
            this.RefreshBtn.TabIndex = 13;
            this.RefreshBtn.Text = "Refresh";
            this.RefreshBtn.UseVisualStyleBackColor = false;
            this.RefreshBtn.Click += new System.EventHandler(this.RefreshBtn_Click);
            // 
            // ScanBtn
            // 
            this.ScanBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScanBtn.BackColor = System.Drawing.Color.SteelBlue;
            this.ScanBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ScanBtn.Location = new System.Drawing.Point(3, 116);
            this.ScanBtn.Name = "ScanBtn";
            this.ScanBtn.Size = new System.Drawing.Size(203, 23);
            this.ScanBtn.TabIndex = 12;
            this.ScanBtn.Text = "First Scan";
            this.ScanBtn.UseVisualStyleBackColor = false;
            this.ScanBtn.Click += new System.EventHandler(this.ScanBtn_Click);
            // 
            // CompareTypeBox
            // 
            this.CompareTypeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CompareTypeBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.CompareTypeBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CompareTypeBox.ForeColor = System.Drawing.Color.White;
            this.CompareTypeBox.FormattingEnabled = true;
            this.CompareTypeBox.Location = new System.Drawing.Point(3, 90);
            this.CompareTypeBox.Name = "CompareTypeBox";
            this.CompareTypeBox.Size = new System.Drawing.Size(260, 20);
            this.CompareTypeBox.TabIndex = 11;
            this.CompareTypeBox.SelectedIndexChanged += new System.EventHandler(this.CompareTypeBox_SelectedIndexChanged);
            // 
            // Panel1
            // 
            this.Panel1.Controls.Add(this.SplitContainer1);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel1.Location = new System.Drawing.Point(0, 0);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(800, 450);
            this.Panel1.TabIndex = 2;
            // 
            // Query
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Panel1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "Query";
            this.Opacity = 0.95D;
            this.Text = "Query";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Query_FormClosing);
            this.Load += new System.EventHandler(this.Query_Load);
            this.SplitContainer1.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel1.PerformLayout();
            this.SplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).EndInit();
            this.SplitContainer1.ResumeLayout(false);
            this.StatusStrip1.ResumeLayout(false);
            this.StatusStrip1.PerformLayout();
            this.ResultViewMenu.ResumeLayout(false);
            this.SplitContainer2.Panel1.ResumeLayout(false);
            this.SplitContainer2.Panel2.ResumeLayout(false);
            this.SplitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer2)).EndInit();
            this.SplitContainer2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.SectionViewMenu.ResumeLayout(false);
            this.TableLayoutPanel1.ResumeLayout(false);
            this.TableLayoutPanel1.PerformLayout();
            this.Panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer SplitContainer1;
        private System.Windows.Forms.SplitContainer SplitContainer2;
        private System.Windows.Forms.ListView ResultView;
        private System.Windows.Forms.ColumnHeader ResultViewAddress;
        private System.Windows.Forms.ColumnHeader ResultViewType;
        private System.Windows.Forms.ColumnHeader ResultViewValue;
        private System.Windows.Forms.ColumnHeader ResultViewHex;
        private System.Windows.Forms.ColumnHeader ResultViewSection;
        private System.Windows.Forms.Button SectionSearchBtn;
        private System.Windows.Forms.ListView SectionView;
        private System.Windows.Forms.Button GetProcessesBtn;
        private System.Windows.Forms.ComboBox ProcessesBox;
        private System.Windows.Forms.CheckBox SelectAllBox;
        private System.Windows.Forms.CheckBox AlignmentBox;
        private System.Windows.Forms.ColumnHeader SectionViewAddress;
        private System.Windows.Forms.ColumnHeader SectionViewName;
        private System.Windows.Forms.ColumnHeader SectionViewProt;
        private System.Windows.Forms.ColumnHeader SectionViewLength;
        private System.Windows.Forms.ColumnHeader SectionViewSID;
        private System.Windows.Forms.ColumnHeader SectionViewOffset;
        private System.Windows.Forms.Panel Panel1;
        private System.Windows.Forms.StatusStrip StatusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripMsg;
        private System.Windows.Forms.Button FilterRuleBtn;
        private System.Windows.Forms.CheckBox IsFilterBox;
        private System.Windows.Forms.CheckBox HexBox;
        private System.Windows.Forms.TextBox ValueBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Value1Box;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ScanTypeBox;
        private System.Windows.Forms.ComboBox CompareTypeBox;
        private System.Windows.Forms.Button ScanBtn;
        private System.Windows.Forms.Button RefreshBtn;
        private System.Windows.Forms.ProgressBar ToolStripBar;
        private System.Windows.Forms.ContextMenuStrip ResultViewMenu;
        private System.Windows.Forms.ToolStripMenuItem ResultViewAddToCheatGrid;
        private System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ResultViewHexEditor;
        private System.Windows.Forms.ToolStripMenuItem ResultViewDump;
        private System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem ResultViewFindPointer;
        private System.Windows.Forms.ContextMenuStrip SectionViewMenu;
        private System.Windows.Forms.ToolStripMenuItem SectionViewHexEditor;
        private System.Windows.Forms.ToolStripMenuItem SectionViewDump;
        private System.Windows.Forms.ToolStripSeparator ToolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem SectionViewCheck;
        private System.Windows.Forms.Button NewBtn;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
        private System.Windows.Forms.TextBox AddrMinBox;
        private System.Windows.Forms.TextBox AddrMaxBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ToolStripMenuItem ResultViewCopyAddress;
    }
}