
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
            this.ResultViewMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ResultViewAddToCheatGrid = new System.Windows.Forms.ToolStripMenuItem();
            this.ResultViewSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ResultViewHexEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.ResultViewCopyAddress = new System.Windows.Forms.ToolStripMenuItem();
            this.ResultViewDump = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ResultViewFindPointer = new System.Windows.Forms.ToolStripMenuItem();
            this.SectionViewMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SectionViewHexEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.SectionViewCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.SectionViewCheckAll = new System.Windows.Forms.ToolStripMenuItem();
            this.SectionViewUnCheckAll = new System.Windows.Forms.ToolStripMenuItem();
            this.SectionViewInvertChecked = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.SectionViewCheckContains = new System.Windows.Forms.ToolStripMenuItem();
            this.SectionViewUnCheckContains = new System.Windows.Forms.ToolStripMenuItem();
            this.SectionViewTextContains = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.SectionViewCheckProt = new System.Windows.Forms.ToolStripMenuItem();
            this.SectionViewUnCheckProt = new System.Windows.Forms.ToolStripMenuItem();
            this.SectionViewTextProt = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.SectionViewCheckAllHidden = new System.Windows.Forms.ToolStripMenuItem();
            this.SectionViewUnCheckAllHidden = new System.Windows.Forms.ToolStripMenuItem();
            this.SectionViewDump = new System.Windows.Forms.ToolStripMenuItem();
            this.SectionViewImport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.SplitContainer1 = new PS4CheaterNeo.CollapsibleSplitContainer();
            this.ResultView = new System.Windows.Forms.ListView();
            this.ResultViewAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ResultViewType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ResultViewValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ResultViewHex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ResultViewSection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolStripMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.SplitContainer2 = new PS4CheaterNeo.CollapsibleSplitContainer();
            this.TableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.GetProcessesBtn = new System.Windows.Forms.Button();
            this.TableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.ProcessesBox = new PS4CheaterNeo.ComboItemBox();
            this.PauseBtn = new System.Windows.Forms.Button();
            this.SectionSearchBtn = new System.Windows.Forms.Button();
            this.ResumeBtn = new System.Windows.Forms.Button();
            this.SectionView = new System.Windows.Forms.ListView();
            this.SectionViewID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SectionViewAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SectionViewName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SectionViewProt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SectionViewLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SectionViewSID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SectionViewOffset = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SectionViewEnd = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.AddrIsFilterBox = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.AddrMaxBox = new System.Windows.Forms.TextBox();
            this.AddrMinBox = new System.Windows.Forms.TextBox();
            this.CompareFirstBox = new System.Windows.Forms.CheckBox();
            this.ToolStripBar = new System.Windows.Forms.ProgressBar();
            this.SimpleValuesBox = new System.Windows.Forms.CheckBox();
            this.SlowMotionBox = new System.Windows.Forms.CheckBox();
            this.AutoResumeBox = new System.Windows.Forms.CheckBox();
            this.AutoPauseBox = new System.Windows.Forms.CheckBox();
            this.RedoBtn = new System.Windows.Forms.Button();
            this.UndoBtn = new System.Windows.Forms.Button();
            this.CloneScanBtn = new System.Windows.Forms.Button();
            this.IsFilterBox = new System.Windows.Forms.CheckBox();
            this.IsFilterSizeBox = new System.Windows.Forms.CheckBox();
            this.ScanTypeBox = new System.Windows.Forms.ComboBox();
            this.SelectAllBox = new System.Windows.Forms.CheckBox();
            this.AlignmentBox = new System.Windows.Forms.CheckBox();
            this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ValueBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Value1Box = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.HexBox = new System.Windows.Forms.CheckBox();
            this.NotBox = new System.Windows.Forms.CheckBox();
            this.NewBtn = new System.Windows.Forms.Button();
            this.RefreshBtn = new System.Windows.Forms.Button();
            this.ScanBtn = new System.Windows.Forms.Button();
            this.CompareTypeBox = new System.Windows.Forms.ComboBox();
            this.SlowMotionTimer = new System.Windows.Forms.Timer(this.components);
            this.SaveDialog = new System.Windows.Forms.SaveFileDialog();
            this.OpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.ResultViewMenu.SuspendLayout();
            this.SectionViewMenu.SuspendLayout();
            this.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).BeginInit();
            this.SplitContainer1.Panel1.SuspendLayout();
            this.SplitContainer1.Panel2.SuspendLayout();
            this.SplitContainer1.SuspendLayout();
            this.StatusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer2)).BeginInit();
            this.SplitContainer2.Panel1.SuspendLayout();
            this.SplitContainer2.Panel2.SuspendLayout();
            this.SplitContainer2.SuspendLayout();
            this.TableLayoutPanel3.SuspendLayout();
            this.TableLayoutPanel4.SuspendLayout();
            this.TableLayoutPanel2.SuspendLayout();
            this.TableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ResultViewMenu
            // 
            this.ResultViewMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ResultViewAddToCheatGrid,
            this.ResultViewSelectAll,
            this.ToolStripSeparator1,
            this.ResultViewHexEditor,
            this.ResultViewCopyAddress,
            this.ResultViewDump,
            this.ToolStripSeparator2,
            this.ResultViewFindPointer});
            this.ResultViewMenu.Name = "ResultViewMenu";
            this.ResultViewMenu.Size = new System.Drawing.Size(177, 148);
            // 
            // ResultViewAddToCheatGrid
            // 
            this.ResultViewAddToCheatGrid.Name = "ResultViewAddToCheatGrid";
            this.ResultViewAddToCheatGrid.Size = new System.Drawing.Size(176, 22);
            this.ResultViewAddToCheatGrid.Text = "Add to Cheat Grid";
            this.ResultViewAddToCheatGrid.Click += new System.EventHandler(this.ResultViewAddToCheatGrid_Click);
            // 
            // ResultViewSelectAll
            // 
            this.ResultViewSelectAll.Name = "ResultViewSelectAll";
            this.ResultViewSelectAll.Size = new System.Drawing.Size(176, 22);
            this.ResultViewSelectAll.Text = "Select All";
            this.ResultViewSelectAll.Click += new System.EventHandler(this.ResultViewSelectAll_Click);
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
            // SectionViewMenu
            // 
            this.SectionViewMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SectionViewHexEditor,
            this.ToolStripSeparator3,
            this.SectionViewCheck,
            this.ToolStripSeparator4,
            this.SectionViewCheckAll,
            this.SectionViewUnCheckAll,
            this.SectionViewInvertChecked,
            this.ToolStripSeparator5,
            this.SectionViewCheckContains,
            this.SectionViewUnCheckContains,
            this.SectionViewTextContains,
            this.toolStripSeparator6,
            this.SectionViewCheckProt,
            this.SectionViewUnCheckProt,
            this.SectionViewTextProt,
            this.toolStripSeparator7,
            this.SectionViewCheckAllHidden,
            this.SectionViewUnCheckAllHidden,
            this.SectionViewDump,
            this.SectionViewImport,
            this.toolStripSeparator8});
            this.SectionViewMenu.Name = "SectionViewMenu";
            this.SectionViewMenu.Size = new System.Drawing.Size(208, 362);
            // 
            // SectionViewHexEditor
            // 
            this.SectionViewHexEditor.Name = "SectionViewHexEditor";
            this.SectionViewHexEditor.Size = new System.Drawing.Size(207, 22);
            this.SectionViewHexEditor.Text = "Hex Editor";
            this.SectionViewHexEditor.Click += new System.EventHandler(this.SectionViewHexEditor_Click);
            // 
            // ToolStripSeparator3
            // 
            this.ToolStripSeparator3.Name = "ToolStripSeparator3";
            this.ToolStripSeparator3.Size = new System.Drawing.Size(204, 6);
            // 
            // SectionViewCheck
            // 
            this.SectionViewCheck.Name = "SectionViewCheck";
            this.SectionViewCheck.Size = new System.Drawing.Size(207, 22);
            this.SectionViewCheck.Text = "Check";
            this.SectionViewCheck.Click += new System.EventHandler(this.SectionViewCheck_Click);
            // 
            // ToolStripSeparator4
            // 
            this.ToolStripSeparator4.Name = "ToolStripSeparator4";
            this.ToolStripSeparator4.Size = new System.Drawing.Size(204, 6);
            // 
            // SectionViewCheckAll
            // 
            this.SectionViewCheckAll.Name = "SectionViewCheckAll";
            this.SectionViewCheckAll.Size = new System.Drawing.Size(207, 22);
            this.SectionViewCheckAll.Text = "Check All";
            this.SectionViewCheckAll.Click += new System.EventHandler(this.SectionViewCheckAll_Click);
            // 
            // SectionViewUnCheckAll
            // 
            this.SectionViewUnCheckAll.Name = "SectionViewUnCheckAll";
            this.SectionViewUnCheckAll.Size = new System.Drawing.Size(207, 22);
            this.SectionViewUnCheckAll.Text = "Un-Check All";
            this.SectionViewUnCheckAll.Click += new System.EventHandler(this.SectionViewUnCheckAll_Click);
            // 
            // SectionViewInvertChecked
            // 
            this.SectionViewInvertChecked.Name = "SectionViewInvertChecked";
            this.SectionViewInvertChecked.Size = new System.Drawing.Size(207, 22);
            this.SectionViewInvertChecked.Text = "Invert Checked";
            this.SectionViewInvertChecked.Click += new System.EventHandler(this.SectionViewInvertChecked_Click);
            // 
            // ToolStripSeparator5
            // 
            this.ToolStripSeparator5.Name = "ToolStripSeparator5";
            this.ToolStripSeparator5.Size = new System.Drawing.Size(204, 6);
            // 
            // SectionViewCheckContains
            // 
            this.SectionViewCheckContains.Name = "SectionViewCheckContains";
            this.SectionViewCheckContains.Size = new System.Drawing.Size(207, 22);
            this.SectionViewCheckContains.Text = "Check that contains:";
            this.SectionViewCheckContains.Click += new System.EventHandler(this.SectionViewCheckContains_Click);
            // 
            // SectionViewUnCheckContains
            // 
            this.SectionViewUnCheckContains.Name = "SectionViewUnCheckContains";
            this.SectionViewUnCheckContains.Size = new System.Drawing.Size(207, 22);
            this.SectionViewUnCheckContains.Text = "Un-Check that contains:";
            this.SectionViewUnCheckContains.Click += new System.EventHandler(this.SectionViewUnCheckContains_Click);
            // 
            // SectionViewTextContains
            // 
            this.SectionViewTextContains.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.SectionViewTextContains.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SectionViewTextContains.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.SectionViewTextContains.Name = "SectionViewTextContains";
            this.SectionViewTextContains.Size = new System.Drawing.Size(145, 16);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(204, 6);
            // 
            // SectionViewCheckProt
            // 
            this.SectionViewCheckProt.Name = "SectionViewCheckProt";
            this.SectionViewCheckProt.Size = new System.Drawing.Size(207, 22);
            this.SectionViewCheckProt.Text = "Check that has prot:";
            this.SectionViewCheckProt.Click += new System.EventHandler(this.SectionViewCheckProt_Click);
            // 
            // SectionViewUnCheckProt
            // 
            this.SectionViewUnCheckProt.Name = "SectionViewUnCheckProt";
            this.SectionViewUnCheckProt.Size = new System.Drawing.Size(207, 22);
            this.SectionViewUnCheckProt.Text = "Un-Check that has prot:";
            this.SectionViewUnCheckProt.Click += new System.EventHandler(this.SectionViewUnCheckProt_Click);
            // 
            // SectionViewTextProt
            // 
            this.SectionViewTextProt.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.SectionViewTextProt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SectionViewTextProt.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.SectionViewTextProt.Name = "SectionViewTextProt";
            this.SectionViewTextProt.Size = new System.Drawing.Size(145, 16);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(204, 6);
            // 
            // SectionViewCheckAllHidden
            // 
            this.SectionViewCheckAllHidden.Name = "SectionViewCheckAllHidden";
            this.SectionViewCheckAllHidden.Size = new System.Drawing.Size(207, 22);
            this.SectionViewCheckAllHidden.Text = "Check All Hidden";
            this.SectionViewCheckAllHidden.Click += new System.EventHandler(this.SectionViewCheckAllHidden_Click);
            // 
            // SectionViewUnCheckAllHidden
            // 
            this.SectionViewUnCheckAllHidden.Name = "SectionViewUnCheckAllHidden";
            this.SectionViewUnCheckAllHidden.Size = new System.Drawing.Size(207, 22);
            this.SectionViewUnCheckAllHidden.Text = "Un-Check All Hidden";
            this.SectionViewUnCheckAllHidden.Click += new System.EventHandler(this.SectionViewUnCheckAllHidden_Click);
            // 
            // SectionViewDump
            // 
            this.SectionViewDump.Name = "SectionViewDump";
            this.SectionViewDump.Size = new System.Drawing.Size(207, 22);
            this.SectionViewDump.Text = "Dump";
            this.SectionViewDump.Click += new System.EventHandler(this.SectionViewDump_Click);
            // 
            // SectionViewImport
            // 
            this.SectionViewImport.Name = "SectionViewImport";
            this.SectionViewImport.Size = new System.Drawing.Size(207, 22);
            this.SectionViewImport.Text = "Import";
            this.SectionViewImport.Click += new System.EventHandler(this.SectionViewImport_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(204, 6);
            // 
            // Panel1
            // 
            this.Panel1.Controls.Add(this.SplitContainer1);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel1.Location = new System.Drawing.Point(0, 0);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(800, 487);
            this.Panel1.TabIndex = 2;
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
            this.SplitContainer1.Panel1.Controls.Add(this.ResultView);
            this.SplitContainer1.Panel1.Controls.Add(this.StatusStrip1);
            // 
            // SplitContainer1.Panel2
            // 
            this.SplitContainer1.Panel2.Controls.Add(this.SplitContainer2);
            this.SplitContainer1.Size = new System.Drawing.Size(800, 487);
            this.SplitContainer1.SplitterButtonBitmap = ((System.Drawing.Bitmap)(resources.GetObject("SplitContainer1.SplitterButtonBitmap")));
            this.SplitContainer1.SplitterButtonLocation = PS4CheaterNeo.ButtonLocation.Panel1;
            this.SplitContainer1.SplitterButtonSize = 13;
            this.SplitContainer1.SplitterButtonStyle = PS4CheaterNeo.ButtonStyle.SingleImage;
            this.SplitContainer1.SplitterDistance = 507;
            this.SplitContainer1.TabIndex = 0;
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
            this.ResultView.Size = new System.Drawing.Size(505, 463);
            this.ResultView.TabIndex = 0;
            this.ResultView.UseCompatibleStateImageBehavior = false;
            this.ResultView.View = System.Windows.Forms.View.Details;
            this.ResultView.VirtualMode = true;
            this.ResultView.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.ResultView_RetrieveVirtualItem);
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
            // StatusStrip1
            // 
            this.StatusStrip1.BackColor = System.Drawing.Color.DimGray;
            this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMsg});
            this.StatusStrip1.Location = new System.Drawing.Point(0, 463);
            this.StatusStrip1.Name = "StatusStrip1";
            this.StatusStrip1.Size = new System.Drawing.Size(505, 22);
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
            this.ToolStripMsg.Size = new System.Drawing.Size(490, 17);
            this.ToolStripMsg.Spring = true;
            this.ToolStripMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.SplitContainer2.Panel1.Controls.Add(this.TableLayoutPanel3);
            // 
            // SplitContainer2.Panel2
            // 
            this.SplitContainer2.Panel2.Controls.Add(this.CompareFirstBox);
            this.SplitContainer2.Panel2.Controls.Add(this.ToolStripBar);
            this.SplitContainer2.Panel2.Controls.Add(this.SimpleValuesBox);
            this.SplitContainer2.Panel2.Controls.Add(this.SlowMotionBox);
            this.SplitContainer2.Panel2.Controls.Add(this.AutoResumeBox);
            this.SplitContainer2.Panel2.Controls.Add(this.AutoPauseBox);
            this.SplitContainer2.Panel2.Controls.Add(this.RedoBtn);
            this.SplitContainer2.Panel2.Controls.Add(this.UndoBtn);
            this.SplitContainer2.Panel2.Controls.Add(this.CloneScanBtn);
            this.SplitContainer2.Panel2.Controls.Add(this.IsFilterBox);
            this.SplitContainer2.Panel2.Controls.Add(this.IsFilterSizeBox);
            this.SplitContainer2.Panel2.Controls.Add(this.ScanTypeBox);
            this.SplitContainer2.Panel2.Controls.Add(this.SelectAllBox);
            this.SplitContainer2.Panel2.Controls.Add(this.AlignmentBox);
            this.SplitContainer2.Panel2.Controls.Add(this.TableLayoutPanel1);
            this.SplitContainer2.Panel2.Controls.Add(this.NewBtn);
            this.SplitContainer2.Panel2.Controls.Add(this.RefreshBtn);
            this.SplitContainer2.Panel2.Controls.Add(this.ScanBtn);
            this.SplitContainer2.Panel2.Controls.Add(this.CompareTypeBox);
            this.SplitContainer2.Size = new System.Drawing.Size(289, 487);
            this.SplitContainer2.SplitterButtonBitmap = ((System.Drawing.Bitmap)(resources.GetObject("SplitContainer2.SplitterButtonBitmap")));
            this.SplitContainer2.SplitterButtonLocation = PS4CheaterNeo.ButtonLocation.Panel1;
            this.SplitContainer2.SplitterButtonPosition = PS4CheaterNeo.ButtonPosition.BottomRight;
            this.SplitContainer2.SplitterButtonSize = 13;
            this.SplitContainer2.SplitterButtonStyle = PS4CheaterNeo.ButtonStyle.SingleImage;
            this.SplitContainer2.SplitterDistance = 256;
            this.SplitContainer2.TabIndex = 0;
            // 
            // TableLayoutPanel3
            // 
            this.TableLayoutPanel3.ColumnCount = 1;
            this.TableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel3.Controls.Add(this.GetProcessesBtn, 0, 0);
            this.TableLayoutPanel3.Controls.Add(this.TableLayoutPanel4, 0, 1);
            this.TableLayoutPanel3.Controls.Add(this.SectionView, 0, 2);
            this.TableLayoutPanel3.Controls.Add(this.TableLayoutPanel2, 0, 3);
            this.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.TableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.TableLayoutPanel3.Name = "TableLayoutPanel3";
            this.TableLayoutPanel3.RowCount = 4;
            this.TableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel3.Size = new System.Drawing.Size(287, 254);
            this.TableLayoutPanel3.TabIndex = 1;
            // 
            // GetProcessesBtn
            // 
            this.GetProcessesBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.GetProcessesBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GetProcessesBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GetProcessesBtn.ForeColor = System.Drawing.Color.White;
            this.GetProcessesBtn.Location = new System.Drawing.Point(3, 3);
            this.GetProcessesBtn.Name = "GetProcessesBtn";
            this.GetProcessesBtn.Size = new System.Drawing.Size(281, 25);
            this.GetProcessesBtn.TabIndex = 1;
            this.GetProcessesBtn.Text = "Refresh Processes";
            this.GetProcessesBtn.UseVisualStyleBackColor = false;
            this.GetProcessesBtn.Click += new System.EventHandler(this.GetProcessesBtn_Click);
            // 
            // TableLayoutPanel4
            // 
            this.TableLayoutPanel4.AutoSize = true;
            this.TableLayoutPanel4.ColumnCount = 4;
            this.TableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableLayoutPanel4.Controls.Add(this.ProcessesBox, 0, 0);
            this.TableLayoutPanel4.Controls.Add(this.PauseBtn, 1, 0);
            this.TableLayoutPanel4.Controls.Add(this.SectionSearchBtn, 3, 0);
            this.TableLayoutPanel4.Controls.Add(this.ResumeBtn, 2, 0);
            this.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel4.Location = new System.Drawing.Point(0, 31);
            this.TableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.TableLayoutPanel4.Name = "TableLayoutPanel4";
            this.TableLayoutPanel4.RowCount = 1;
            this.TableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel4.Size = new System.Drawing.Size(287, 29);
            this.TableLayoutPanel4.TabIndex = 33;
            // 
            // ProcessesBox
            // 
            this.ProcessesBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ProcessesBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProcessesBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ProcessesBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProcessesBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ProcessesBox.ForeColor = System.Drawing.Color.White;
            this.ProcessesBox.FormattingEnabled = true;
            this.ProcessesBox.Location = new System.Drawing.Point(3, 3);
            this.ProcessesBox.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.ProcessesBox.Name = "ProcessesBox";
            this.ProcessesBox.Size = new System.Drawing.Size(218, 23);
            this.ProcessesBox.TabIndex = 0;
            this.ProcessesBox.SelectedIndexChanged += new System.EventHandler(this.ProcessesBox_SelectedIndexChanged);
            // 
            // PauseBtn
            // 
            this.PauseBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.PauseBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("PauseBtn.BackgroundImage")));
            this.PauseBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.PauseBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PauseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PauseBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PauseBtn.ForeColor = System.Drawing.Color.White;
            this.PauseBtn.Location = new System.Drawing.Point(224, 3);
            this.PauseBtn.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.PauseBtn.Name = "PauseBtn";
            this.PauseBtn.Size = new System.Drawing.Size(20, 23);
            this.PauseBtn.TabIndex = 31;
            this.PauseBtn.UseVisualStyleBackColor = false;
            this.PauseBtn.Click += new System.EventHandler(this.PauseBtn_Click);
            // 
            // SectionSearchBtn
            // 
            this.SectionSearchBtn.BackColor = System.Drawing.SystemColors.Desktop;
            this.SectionSearchBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SectionSearchBtn.BackgroundImage")));
            this.SectionSearchBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.SectionSearchBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SectionSearchBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SectionSearchBtn.ForeColor = System.Drawing.Color.White;
            this.SectionSearchBtn.Location = new System.Drawing.Point(264, 3);
            this.SectionSearchBtn.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.SectionSearchBtn.Name = "SectionSearchBtn";
            this.SectionSearchBtn.Size = new System.Drawing.Size(20, 23);
            this.SectionSearchBtn.TabIndex = 3;
            this.SectionSearchBtn.UseVisualStyleBackColor = false;
            this.SectionSearchBtn.Click += new System.EventHandler(this.SectionSearchBtn_Click);
            // 
            // ResumeBtn
            // 
            this.ResumeBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ResumeBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ResumeBtn.BackgroundImage")));
            this.ResumeBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ResumeBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResumeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ResumeBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResumeBtn.ForeColor = System.Drawing.Color.White;
            this.ResumeBtn.Location = new System.Drawing.Point(244, 3);
            this.ResumeBtn.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.ResumeBtn.Name = "ResumeBtn";
            this.ResumeBtn.Size = new System.Drawing.Size(20, 23);
            this.ResumeBtn.TabIndex = 32;
            this.ResumeBtn.UseVisualStyleBackColor = false;
            this.ResumeBtn.Click += new System.EventHandler(this.ResumeBtn_Click);
            // 
            // SectionView
            // 
            this.SectionView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.SectionView.CheckBoxes = true;
            this.SectionView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SectionViewID,
            this.SectionViewAddress,
            this.SectionViewName,
            this.SectionViewProt,
            this.SectionViewLength,
            this.SectionViewSID,
            this.SectionViewOffset,
            this.SectionViewEnd});
            this.SectionView.ContextMenuStrip = this.SectionViewMenu;
            this.SectionView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SectionView.ForeColor = System.Drawing.Color.White;
            this.SectionView.HideSelection = false;
            this.SectionView.Location = new System.Drawing.Point(0, 60);
            this.SectionView.Margin = new System.Windows.Forms.Padding(0);
            this.SectionView.Name = "SectionView";
            this.SectionView.Size = new System.Drawing.Size(287, 166);
            this.SectionView.TabIndex = 2;
            this.SectionView.UseCompatibleStateImageBehavior = false;
            this.SectionView.View = System.Windows.Forms.View.Details;
            this.SectionView.VirtualMode = true;
            this.SectionView.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.SectionView_RetrieveVirtualItem);
            this.SectionView.SearchForVirtualItem += new System.Windows.Forms.SearchForVirtualItemEventHandler(this.SectionView_SearchForVirtualItem);
            this.SectionView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SectionView_MouseDown);
            // 
            // SectionViewID
            // 
            this.SectionViewID.Text = "";
            this.SectionViewID.Width = 40;
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
            this.SectionViewProt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SectionViewProt.Width = 24;
            // 
            // SectionViewLength
            // 
            this.SectionViewLength.Text = "Length";
            this.SectionViewLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SectionViewLength.Width = 65;
            // 
            // SectionViewSID
            // 
            this.SectionViewSID.Text = "SID";
            this.SectionViewSID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SectionViewSID.Width = 70;
            // 
            // SectionViewOffset
            // 
            this.SectionViewOffset.Text = "Offset";
            this.SectionViewOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // SectionViewEnd
            // 
            this.SectionViewEnd.Text = "End";
            this.SectionViewEnd.Width = 85;
            // 
            // TableLayoutPanel2
            // 
            this.TableLayoutPanel2.ColumnCount = 5;
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.TableLayoutPanel2.Controls.Add(this.AddrIsFilterBox, 4, 0);
            this.TableLayoutPanel2.Controls.Add(this.label5, 2, 0);
            this.TableLayoutPanel2.Controls.Add(this.AddrMaxBox, 3, 0);
            this.TableLayoutPanel2.Controls.Add(this.AddrMinBox, 1, 0);
            this.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel2.Location = new System.Drawing.Point(0, 226);
            this.TableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.TableLayoutPanel2.Name = "TableLayoutPanel2";
            this.TableLayoutPanel2.RowCount = 1;
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel2.Size = new System.Drawing.Size(287, 28);
            this.TableLayoutPanel2.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "Min:";
            // 
            // AddrIsFilterBox
            // 
            this.AddrIsFilterBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddrIsFilterBox.AutoSize = true;
            this.AddrIsFilterBox.Location = new System.Drawing.Point(236, 3);
            this.AddrIsFilterBox.Name = "AddrIsFilterBox";
            this.AddrIsFilterBox.Size = new System.Drawing.Size(48, 16);
            this.AddrIsFilterBox.TabIndex = 21;
            this.AddrIsFilterBox.Text = "Filter";
            this.AddrIsFilterBox.UseVisualStyleBackColor = true;
            this.AddrIsFilterBox.CheckedChanged += new System.EventHandler(this.AddrIsFilterBox_CheckedChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(118, 8);
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
            this.AddrMaxBox.Location = new System.Drawing.Point(153, 3);
            this.AddrMaxBox.Name = "AddrMaxBox";
            this.AddrMaxBox.Size = new System.Drawing.Size(76, 22);
            this.AddrMaxBox.TabIndex = 19;
            this.AddrMaxBox.Leave += new System.EventHandler(this.AddrMinMaxBox_Leave);
            // 
            // AddrMinBox
            // 
            this.AddrMinBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddrMinBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.AddrMinBox.ForeColor = System.Drawing.Color.White;
            this.AddrMinBox.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.AddrMinBox.Location = new System.Drawing.Point(36, 3);
            this.AddrMinBox.Name = "AddrMinBox";
            this.AddrMinBox.Size = new System.Drawing.Size(76, 22);
            this.AddrMinBox.TabIndex = 18;
            this.AddrMinBox.Leave += new System.EventHandler(this.AddrMinMaxBox_Leave);
            // 
            // CompareFirstBox
            // 
            this.CompareFirstBox.AutoSize = true;
            this.CompareFirstBox.Enabled = false;
            this.CompareFirstBox.Location = new System.Drawing.Point(3, 85);
            this.CompareFirstBox.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.CompareFirstBox.Name = "CompareFirstBox";
            this.CompareFirstBox.Size = new System.Drawing.Size(123, 16);
            this.CompareFirstBox.TabIndex = 34;
            this.CompareFirstBox.Text = "Compare to first scan";
            this.CompareFirstBox.UseVisualStyleBackColor = true;
            // 
            // ToolStripBar
            // 
            this.ToolStripBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ToolStripBar.Location = new System.Drawing.Point(3, 213);
            this.ToolStripBar.Name = "ToolStripBar";
            this.ToolStripBar.Size = new System.Drawing.Size(283, 10);
            this.ToolStripBar.TabIndex = 16;
            // 
            // SimpleValuesBox
            // 
            this.SimpleValuesBox.AutoSize = true;
            this.SimpleValuesBox.Location = new System.Drawing.Point(205, 22);
            this.SimpleValuesBox.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.SimpleValuesBox.Name = "SimpleValuesBox";
            this.SimpleValuesBox.Size = new System.Drawing.Size(87, 16);
            this.SimpleValuesBox.TabIndex = 12;
            this.SimpleValuesBox.Text = "SimpleValues";
            this.SimpleValuesBox.UseVisualStyleBackColor = true;
            // 
            // SlowMotionBox
            // 
            this.SlowMotionBox.AutoSize = true;
            this.SlowMotionBox.ForeColor = System.Drawing.Color.White;
            this.SlowMotionBox.Location = new System.Drawing.Point(155, 22);
            this.SlowMotionBox.Name = "SlowMotionBox";
            this.SlowMotionBox.Size = new System.Drawing.Size(47, 16);
            this.SlowMotionBox.TabIndex = 33;
            this.SlowMotionBox.Text = "Slow";
            this.SlowMotionBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SlowMotionBox.UseVisualStyleBackColor = true;
            this.SlowMotionBox.CheckedChanged += new System.EventHandler(this.SlowMotionBox_CheckedChanged);
            // 
            // AutoResumeBox
            // 
            this.AutoResumeBox.AutoSize = true;
            this.AutoResumeBox.Location = new System.Drawing.Point(73, 22);
            this.AutoResumeBox.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.AutoResumeBox.Name = "AutoResumeBox";
            this.AutoResumeBox.Size = new System.Drawing.Size(84, 16);
            this.AutoResumeBox.TabIndex = 21;
            this.AutoResumeBox.Text = "AutoResume";
            this.AutoResumeBox.UseVisualStyleBackColor = true;
            // 
            // AutoPauseBox
            // 
            this.AutoPauseBox.AutoSize = true;
            this.AutoPauseBox.Location = new System.Drawing.Point(3, 22);
            this.AutoPauseBox.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.AutoPauseBox.Name = "AutoPauseBox";
            this.AutoPauseBox.Size = new System.Drawing.Size(73, 16);
            this.AutoPauseBox.TabIndex = 11;
            this.AutoPauseBox.Text = "AutoPause";
            this.AutoPauseBox.UseVisualStyleBackColor = true;
            // 
            // RedoBtn
            // 
            this.RedoBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RedoBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.RedoBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.RedoBtn.Enabled = false;
            this.RedoBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RedoBtn.Image = ((System.Drawing.Image)(resources.GetObject("RedoBtn.Image")));
            this.RedoBtn.Location = new System.Drawing.Point(261, 156);
            this.RedoBtn.Name = "RedoBtn";
            this.RedoBtn.Size = new System.Drawing.Size(24, 23);
            this.RedoBtn.TabIndex = 20;
            this.RedoBtn.UseVisualStyleBackColor = false;
            this.RedoBtn.Click += new System.EventHandler(this.RedoBtn_Click);
            // 
            // UndoBtn
            // 
            this.UndoBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UndoBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.UndoBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.UndoBtn.Enabled = false;
            this.UndoBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UndoBtn.Image = ((System.Drawing.Image)(resources.GetObject("UndoBtn.Image")));
            this.UndoBtn.Location = new System.Drawing.Point(235, 156);
            this.UndoBtn.Name = "UndoBtn";
            this.UndoBtn.Size = new System.Drawing.Size(24, 23);
            this.UndoBtn.TabIndex = 19;
            this.UndoBtn.UseVisualStyleBackColor = false;
            this.UndoBtn.Click += new System.EventHandler(this.UndoBtn_Click);
            // 
            // CloneScanBtn
            // 
            this.CloneScanBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CloneScanBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.CloneScanBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F);
            this.CloneScanBtn.ForeColor = System.Drawing.Color.White;
            this.CloneScanBtn.Location = new System.Drawing.Point(252, 0);
            this.CloneScanBtn.Name = "CloneScanBtn";
            this.CloneScanBtn.Size = new System.Drawing.Size(37, 22);
            this.CloneScanBtn.TabIndex = 2;
            this.CloneScanBtn.Text = "Clone";
            this.CloneScanBtn.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.CloneScanBtn.UseVisualStyleBackColor = false;
            this.CloneScanBtn.Click += new System.EventHandler(this.CloneScanBtn_Click);
            // 
            // IsFilterBox
            // 
            this.IsFilterBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.IsFilterBox.AutoSize = true;
            this.IsFilterBox.ForeColor = System.Drawing.Color.White;
            this.IsFilterBox.Location = new System.Drawing.Point(205, 3);
            this.IsFilterBox.Name = "IsFilterBox";
            this.IsFilterBox.Size = new System.Drawing.Size(48, 16);
            this.IsFilterBox.TabIndex = 3;
            this.IsFilterBox.Text = "Filter";
            this.IsFilterBox.UseVisualStyleBackColor = true;
            this.IsFilterBox.CheckedChanged += new System.EventHandler(this.IsFilterBox_CheckedChanged);
            // 
            // IsFilterSizeBox
            // 
            this.IsFilterSizeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.IsFilterSizeBox.AutoSize = true;
            this.IsFilterSizeBox.ForeColor = System.Drawing.Color.White;
            this.IsFilterSizeBox.Location = new System.Drawing.Point(143, 3);
            this.IsFilterSizeBox.Name = "IsFilterSizeBox";
            this.IsFilterSizeBox.Size = new System.Drawing.Size(67, 16);
            this.IsFilterSizeBox.TabIndex = 18;
            this.IsFilterSizeBox.Text = "FilterSize";
            this.IsFilterSizeBox.UseVisualStyleBackColor = true;
            this.IsFilterSizeBox.CheckedChanged += new System.EventHandler(this.IsFilterSizeBox_CheckedChanged);
            // 
            // ScanTypeBox
            // 
            this.ScanTypeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScanTypeBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ScanTypeBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ScanTypeBox.ForeColor = System.Drawing.Color.White;
            this.ScanTypeBox.FormattingEnabled = true;
            this.ScanTypeBox.Location = new System.Drawing.Point(3, 104);
            this.ScanTypeBox.Name = "ScanTypeBox";
            this.ScanTypeBox.Size = new System.Drawing.Size(283, 20);
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
            this.AlignmentBox.Location = new System.Drawing.Point(73, 3);
            this.AlignmentBox.Name = "AlignmentBox";
            this.AlignmentBox.Size = new System.Drawing.Size(73, 16);
            this.AlignmentBox.TabIndex = 1;
            this.AlignmentBox.Text = "Alignment";
            this.AlignmentBox.UseVisualStyleBackColor = true;
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
            this.TableLayoutPanel1.Controls.Add(this.ValueBox, 1, 1);
            this.TableLayoutPanel1.Controls.Add(this.label3, 2, 1);
            this.TableLayoutPanel1.Controls.Add(this.Value1Box, 3, 1);
            this.TableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.TableLayoutPanel1.Controls.Add(this.label2, 3, 0);
            this.TableLayoutPanel1.Controls.Add(this.HexBox, 0, 1);
            this.TableLayoutPanel1.Controls.Add(this.NotBox, 0, 0);
            this.TableLayoutPanel1.Location = new System.Drawing.Point(3, 41);
            this.TableLayoutPanel1.Name = "TableLayoutPanel1";
            this.TableLayoutPanel1.RowCount = 2;
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel1.Size = new System.Drawing.Size(283, 40);
            this.TableLayoutPanel1.TabIndex = 1;
            // 
            // ValueBox
            // 
            this.ValueBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ValueBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ValueBox.ForeColor = System.Drawing.Color.White;
            this.ValueBox.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.ValueBox.Location = new System.Drawing.Point(46, 19);
            this.ValueBox.Margin = new System.Windows.Forms.Padding(0);
            this.ValueBox.Name = "ValueBox";
            this.ValueBox.Size = new System.Drawing.Size(106, 22);
            this.ValueBox.TabIndex = 6;
            this.ValueBox.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(155, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 24);
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
            this.Value1Box.Location = new System.Drawing.Point(175, 19);
            this.Value1Box.Margin = new System.Windows.Forms.Padding(0);
            this.Value1Box.Name = "Value1Box";
            this.Value1Box.Size = new System.Drawing.Size(108, 22);
            this.Value1Box.TabIndex = 9;
            this.Value1Box.Text = "0";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "Value:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(178, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "Value:";
            // 
            // HexBox
            // 
            this.HexBox.AutoSize = true;
            this.HexBox.Location = new System.Drawing.Point(0, 22);
            this.HexBox.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.HexBox.Name = "HexBox";
            this.HexBox.Size = new System.Drawing.Size(43, 16);
            this.HexBox.TabIndex = 4;
            this.HexBox.Text = "Hex";
            this.HexBox.UseVisualStyleBackColor = true;
            // 
            // NotBox
            // 
            this.NotBox.AutoSize = true;
            this.NotBox.Location = new System.Drawing.Point(0, 3);
            this.NotBox.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.NotBox.Name = "NotBox";
            this.NotBox.Size = new System.Drawing.Size(41, 16);
            this.NotBox.TabIndex = 10;
            this.NotBox.Text = "Not";
            this.NotBox.UseVisualStyleBackColor = true;
            // 
            // NewBtn
            // 
            this.NewBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NewBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.NewBtn.Enabled = false;
            this.NewBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NewBtn.Location = new System.Drawing.Point(235, 186);
            this.NewBtn.Name = "NewBtn";
            this.NewBtn.Size = new System.Drawing.Size(50, 23);
            this.NewBtn.TabIndex = 17;
            this.NewBtn.Text = "New";
            this.NewBtn.UseVisualStyleBackColor = false;
            this.NewBtn.Click += new System.EventHandler(this.NewBtn_Click);
            // 
            // RefreshBtn
            // 
            this.RefreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RefreshBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.RefreshBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RefreshBtn.Location = new System.Drawing.Point(3, 186);
            this.RefreshBtn.Name = "RefreshBtn";
            this.RefreshBtn.Size = new System.Drawing.Size(226, 23);
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
            this.ScanBtn.Location = new System.Drawing.Point(3, 156);
            this.ScanBtn.Name = "ScanBtn";
            this.ScanBtn.Size = new System.Drawing.Size(226, 23);
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
            this.CompareTypeBox.Location = new System.Drawing.Point(3, 130);
            this.CompareTypeBox.Name = "CompareTypeBox";
            this.CompareTypeBox.Size = new System.Drawing.Size(283, 20);
            this.CompareTypeBox.TabIndex = 11;
            this.CompareTypeBox.SelectedIndexChanged += new System.EventHandler(this.CompareTypeBox_SelectedIndexChanged);
            // 
            // SlowMotionTimer
            // 
            this.SlowMotionTimer.Interval = 250;
            this.SlowMotionTimer.Tick += new System.EventHandler(this.SlowMotionTimer_Tick);
            // 
            // OpenDialog
            // 
            this.OpenDialog.DefaultExt = "bin";
            this.OpenDialog.Multiselect = true;
            // 
            // Query
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ClientSize = new System.Drawing.Size(800, 487);
            this.Controls.Add(this.Panel1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "Query";
            this.Text = "Query";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Query_FormClosing);
            this.Load += new System.EventHandler(this.Query_Load);
            this.ResultViewMenu.ResumeLayout(false);
            this.SectionViewMenu.ResumeLayout(false);
            this.SectionViewMenu.PerformLayout();
            this.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel1.PerformLayout();
            this.SplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).EndInit();
            this.SplitContainer1.ResumeLayout(false);
            this.StatusStrip1.ResumeLayout(false);
            this.StatusStrip1.PerformLayout();
            this.SplitContainer2.Panel1.ResumeLayout(false);
            this.SplitContainer2.Panel2.ResumeLayout(false);
            this.SplitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer2)).EndInit();
            this.SplitContainer2.ResumeLayout(false);
            this.TableLayoutPanel3.ResumeLayout(false);
            this.TableLayoutPanel3.PerformLayout();
            this.TableLayoutPanel4.ResumeLayout(false);
            this.TableLayoutPanel2.ResumeLayout(false);
            this.TableLayoutPanel2.PerformLayout();
            this.TableLayoutPanel1.ResumeLayout(false);
            this.TableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CollapsibleSplitContainer SplitContainer1;
        private CollapsibleSplitContainer SplitContainer2;
        private System.Windows.Forms.ListView ResultView;
        private System.Windows.Forms.ColumnHeader ResultViewAddress;
        private System.Windows.Forms.ColumnHeader ResultViewType;
        private System.Windows.Forms.ColumnHeader ResultViewValue;
        private System.Windows.Forms.ColumnHeader ResultViewHex;
        private System.Windows.Forms.ColumnHeader ResultViewSection;
        private System.Windows.Forms.ListView SectionView;
        private System.Windows.Forms.Button GetProcessesBtn;
        private ComboItemBox ProcessesBox;
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
        private System.Windows.Forms.Button CloneScanBtn;
        private System.Windows.Forms.CheckBox IsFilterBox;
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
        private System.Windows.Forms.TextBox AddrMinBox;
        private System.Windows.Forms.TextBox AddrMaxBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel2;
        private System.Windows.Forms.ToolStripMenuItem ResultViewCopyAddress;
        private System.Windows.Forms.CheckBox IsFilterSizeBox;
        private System.Windows.Forms.ColumnHeader SectionViewEnd;
        private System.Windows.Forms.Button PauseBtn;
        private System.Windows.Forms.Button ResumeBtn;
        private System.Windows.Forms.Timer SlowMotionTimer;
        private System.Windows.Forms.CheckBox SlowMotionBox;
        private System.Windows.Forms.SaveFileDialog SaveDialog;
        private System.Windows.Forms.ToolStripMenuItem SectionViewImport;
        private System.Windows.Forms.OpenFileDialog OpenDialog;
        private System.Windows.Forms.CheckBox AutoPauseBox;
        private System.Windows.Forms.Button RedoBtn;
        private System.Windows.Forms.Button UndoBtn;
        private System.Windows.Forms.ComboBox ScanTypeBox;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
        private System.Windows.Forms.TextBox ValueBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Value1Box;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox HexBox;
        private System.Windows.Forms.CheckBox NotBox;
        private System.Windows.Forms.Button NewBtn;
        private System.Windows.Forms.Button RefreshBtn;
        private System.Windows.Forms.Button ScanBtn;
        private System.Windows.Forms.ComboBox CompareTypeBox;
        private System.Windows.Forms.CheckBox AutoResumeBox;
        private System.Windows.Forms.CheckBox SimpleValuesBox;
        private System.Windows.Forms.ColumnHeader SectionViewID;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel4;
        private System.Windows.Forms.Button SectionSearchBtn;
        private System.Windows.Forms.ToolStripSeparator ToolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem SectionViewUnCheckAll;
        private System.Windows.Forms.ToolStripMenuItem SectionViewInvertChecked;
        private System.Windows.Forms.ToolStripSeparator ToolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem SectionViewCheckAll;
        private System.Windows.Forms.CheckBox CompareFirstBox;
        private System.Windows.Forms.ToolStripMenuItem SectionViewCheckContains;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem SectionViewUnCheckContains;
        private System.Windows.Forms.ToolStripTextBox SectionViewTextContains;
        private System.Windows.Forms.ToolStripMenuItem SectionViewCheckProt;
        private System.Windows.Forms.ToolStripMenuItem SectionViewUnCheckProt;
        private System.Windows.Forms.ToolStripTextBox SectionViewTextProt;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem ResultViewSelectAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem SectionViewCheckAllHidden;
        private System.Windows.Forms.ToolStripMenuItem SectionViewUnCheckAllHidden;
        private System.Windows.Forms.CheckBox AddrIsFilterBox;
    }
}