
namespace PS4CheaterNeo
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.CheatGridMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CheatGridMenuHexEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.CheatGridMenuLock = new System.Windows.Forms.ToolStripMenuItem();
            this.CheatGridMenuUnlock = new System.Windows.Forms.ToolStripMenuItem();
            this.CheatGridMenuActive = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.CheatGridMenuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.CheatGridMenuCopyAddress = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.CheatGridMenuFindPointer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.CheatGridMenuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripSend = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripOpen = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripNewQuery = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripAdd = new System.Windows.Forms.ToolStripButton();
            this.ToolStripHexView = new System.Windows.Forms.ToolStripButton();
            this.ToolStripRefreshCheat = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripProcessInfo = new System.Windows.Forms.ToolStripLabel();
            this.ToolStripExpandAll = new System.Windows.Forms.ToolStripButton();
            this.ToolStripCollapseAll = new System.Windows.Forms.ToolStripButton();
            this.ToolStripLockEnable = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripAutoRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripSettings = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolStripMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CheatGridView = new GroupGridView.GroupGridView();
            this.CheatGridViewDel = new System.Windows.Forms.DataGridViewButtonColumn();
            this.CheatGridViewAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheatGridViewType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheatGridViewActive = new System.Windows.Forms.DataGridViewButtonColumn();
            this.CheatGridViewValue = new GroupGridView.DataGridViewUpDownColumn();
            this.CheatGridViewSection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheatGridViewSID = new GroupGridView.DataGridViewUpDownColumn();
            this.CheatGridViewLock = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CheatGridViewDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RefreshLock = new System.Windows.Forms.Timer(this.components);
            this.OpenCheatDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveCheatDialog = new System.Windows.Forms.SaveFileDialog();
            this.AutoRefreshTimer = new System.Windows.Forms.Timer(this.components);
            this.CheatGridMenu.SuspendLayout();
            this.ToolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CheatGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // CheatGridMenu
            // 
            this.CheatGridMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CheatGridMenuHexEditor,
            this.toolStripSeparator2,
            this.CheatGridMenuLock,
            this.CheatGridMenuUnlock,
            this.CheatGridMenuActive,
            this.toolStripSeparator6,
            this.CheatGridMenuEdit,
            this.CheatGridMenuCopyAddress,
            this.toolStripSeparator7,
            this.CheatGridMenuFindPointer,
            this.toolStripSeparator8,
            this.CheatGridMenuDelete});
            this.CheatGridMenu.Name = "CheatGridMenu";
            this.CheatGridMenu.Size = new System.Drawing.Size(153, 204);
            // 
            // CheatGridMenuHexEditor
            // 
            this.CheatGridMenuHexEditor.Name = "CheatGridMenuHexEditor";
            this.CheatGridMenuHexEditor.Size = new System.Drawing.Size(152, 22);
            this.CheatGridMenuHexEditor.Text = "Hex Editor";
            this.CheatGridMenuHexEditor.Click += new System.EventHandler(this.CheatGridMenuHexEditor_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // CheatGridMenuLock
            // 
            this.CheatGridMenuLock.Name = "CheatGridMenuLock";
            this.CheatGridMenuLock.Size = new System.Drawing.Size(152, 22);
            this.CheatGridMenuLock.Text = "Lock";
            this.CheatGridMenuLock.Click += new System.EventHandler(this.CheatGridMenuLock_Click);
            // 
            // CheatGridMenuUnlock
            // 
            this.CheatGridMenuUnlock.Name = "CheatGridMenuUnlock";
            this.CheatGridMenuUnlock.Size = new System.Drawing.Size(152, 22);
            this.CheatGridMenuUnlock.Text = "Unlock";
            this.CheatGridMenuUnlock.Click += new System.EventHandler(this.CheatGridMenuUnlock_Click);
            // 
            // CheatGridMenuActive
            // 
            this.CheatGridMenuActive.Name = "CheatGridMenuActive";
            this.CheatGridMenuActive.Size = new System.Drawing.Size(152, 22);
            this.CheatGridMenuActive.Text = "Active";
            this.CheatGridMenuActive.Click += new System.EventHandler(this.CheatGridMenuActive_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(149, 6);
            // 
            // CheatGridMenuEdit
            // 
            this.CheatGridMenuEdit.Name = "CheatGridMenuEdit";
            this.CheatGridMenuEdit.Size = new System.Drawing.Size(152, 22);
            this.CheatGridMenuEdit.Text = "Edit";
            this.CheatGridMenuEdit.Click += new System.EventHandler(this.CheatGridMenuEdit_Click);
            // 
            // CheatGridMenuCopyAddress
            // 
            this.CheatGridMenuCopyAddress.Name = "CheatGridMenuCopyAddress";
            this.CheatGridMenuCopyAddress.Size = new System.Drawing.Size(152, 22);
            this.CheatGridMenuCopyAddress.Text = "Copy Address";
            this.CheatGridMenuCopyAddress.Click += new System.EventHandler(this.CheatGridMenuCopyAddress_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(149, 6);
            // 
            // CheatGridMenuFindPointer
            // 
            this.CheatGridMenuFindPointer.Name = "CheatGridMenuFindPointer";
            this.CheatGridMenuFindPointer.Size = new System.Drawing.Size(152, 22);
            this.CheatGridMenuFindPointer.Text = "Find Pointer";
            this.CheatGridMenuFindPointer.Click += new System.EventHandler(this.CheatGridMenuFindPointer_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(149, 6);
            // 
            // CheatGridMenuDelete
            // 
            this.CheatGridMenuDelete.Name = "CheatGridMenuDelete";
            this.CheatGridMenuDelete.Size = new System.Drawing.Size(152, 22);
            this.CheatGridMenuDelete.Text = "Delete";
            this.CheatGridMenuDelete.Click += new System.EventHandler(this.CheatGridMenuDelete_Click);
            // 
            // ToolStrip1
            // 
            this.ToolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(180)))), ((int)(((byte)(209)))));
            this.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.ToolStripSend,
            this.toolStripSeparator9,
            this.ToolStripOpen,
            this.ToolStripSave,
            this.toolStripSeparator10,
            this.ToolStripNewQuery,
            this.toolStripSeparator11,
            this.ToolStripAdd,
            this.ToolStripHexView,
            this.ToolStripRefreshCheat,
            this.toolStripSeparator12,
            this.ToolStripProcessInfo,
            this.ToolStripExpandAll,
            this.ToolStripCollapseAll,
            this.ToolStripLockEnable,
            this.toolStripSeparator3,
            this.ToolStripAutoRefresh,
            this.toolStripSeparator4,
            this.ToolStripSettings});
            this.ToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip1.Name = "ToolStrip1";
            this.ToolStrip1.Size = new System.Drawing.Size(800, 25);
            this.ToolStrip1.TabIndex = 1;
            this.ToolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolStripSend
            // 
            this.ToolStripSend.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripSend.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripSend.Image")));
            this.ToolStripSend.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripSend.Name = "ToolStripSend";
            this.ToolStripSend.Size = new System.Drawing.Size(23, 22);
            this.ToolStripSend.Text = "Send";
            this.ToolStripSend.ToolTipText = "Send";
            this.ToolStripSend.Click += new System.EventHandler(this.ToolStripSend_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolStripOpen
            // 
            this.ToolStripOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripOpen.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripOpen.Image")));
            this.ToolStripOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripOpen.Name = "ToolStripOpen";
            this.ToolStripOpen.Size = new System.Drawing.Size(23, 22);
            this.ToolStripOpen.Text = "Open";
            this.ToolStripOpen.Click += new System.EventHandler(this.ToolStripOpen_Click);
            // 
            // ToolStripSave
            // 
            this.ToolStripSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripSave.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripSave.Image")));
            this.ToolStripSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripSave.Name = "ToolStripSave";
            this.ToolStripSave.Size = new System.Drawing.Size(23, 22);
            this.ToolStripSave.Text = "Save";
            this.ToolStripSave.Click += new System.EventHandler(this.ToolStripSave_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolStripNewQuery
            // 
            this.ToolStripNewQuery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripNewQuery.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripNewQuery.Image")));
            this.ToolStripNewQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripNewQuery.Name = "ToolStripNewQuery";
            this.ToolStripNewQuery.Size = new System.Drawing.Size(23, 22);
            this.ToolStripNewQuery.Text = "Query";
            this.ToolStripNewQuery.ToolTipText = "Query";
            this.ToolStripNewQuery.Click += new System.EventHandler(this.ToolStripNewQuery_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolStripAdd
            // 
            this.ToolStripAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripAdd.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripAdd.Image")));
            this.ToolStripAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripAdd.Name = "ToolStripAdd";
            this.ToolStripAdd.Size = new System.Drawing.Size(23, 22);
            this.ToolStripAdd.Text = "Add";
            this.ToolStripAdd.ToolTipText = "Add";
            this.ToolStripAdd.Click += new System.EventHandler(this.ToolStripAdd_Click);
            // 
            // ToolStripHexView
            // 
            this.ToolStripHexView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripHexView.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripHexView.Image")));
            this.ToolStripHexView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripHexView.Name = "ToolStripHexView";
            this.ToolStripHexView.Size = new System.Drawing.Size(23, 22);
            this.ToolStripHexView.Text = "HexView";
            this.ToolStripHexView.ToolTipText = "HexView";
            this.ToolStripHexView.Click += new System.EventHandler(this.ToolStripHexView_Click);
            // 
            // ToolStripRefreshCheat
            // 
            this.ToolStripRefreshCheat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripRefreshCheat.DoubleClickEnabled = true;
            this.ToolStripRefreshCheat.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripRefreshCheat.Image")));
            this.ToolStripRefreshCheat.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripRefreshCheat.Name = "ToolStripRefreshCheat";
            this.ToolStripRefreshCheat.Size = new System.Drawing.Size(23, 22);
            this.ToolStripRefreshCheat.Text = "RefreshCheat";
            this.ToolStripRefreshCheat.Click += new System.EventHandler(this.ToolStripRefreshCheat_Click);
            this.ToolStripRefreshCheat.DoubleClick += new System.EventHandler(this.ToolStripRefreshCheat_DoubleClick);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolStripProcessInfo
            // 
            this.ToolStripProcessInfo.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ToolStripProcessInfo.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold);
            this.ToolStripProcessInfo.Name = "ToolStripProcessInfo";
            this.ToolStripProcessInfo.Size = new System.Drawing.Size(131, 22);
            this.ToolStripProcessInfo.Text = "ToolStripProcessInfo";
            this.ToolStripProcessInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ToolStripExpandAll
            // 
            this.ToolStripExpandAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripExpandAll.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripExpandAll.Image")));
            this.ToolStripExpandAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripExpandAll.Name = "ToolStripExpandAll";
            this.ToolStripExpandAll.Size = new System.Drawing.Size(23, 22);
            this.ToolStripExpandAll.Text = "ExpandAll";
            this.ToolStripExpandAll.ToolTipText = "ExpandAll";
            this.ToolStripExpandAll.Click += new System.EventHandler(this.ToolStripExpandAll_Click);
            // 
            // ToolStripCollapseAll
            // 
            this.ToolStripCollapseAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripCollapseAll.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripCollapseAll.Image")));
            this.ToolStripCollapseAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripCollapseAll.Name = "ToolStripCollapseAll";
            this.ToolStripCollapseAll.Size = new System.Drawing.Size(23, 22);
            this.ToolStripCollapseAll.Text = "CollapseAll";
            this.ToolStripCollapseAll.ToolTipText = "CollapseAll";
            this.ToolStripCollapseAll.Click += new System.EventHandler(this.ToolStripCollapseAll_Click);
            // 
            // ToolStripLockEnable
            // 
            this.ToolStripLockEnable.Checked = true;
            this.ToolStripLockEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolStripLockEnable.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripLockEnable.Image")));
            this.ToolStripLockEnable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripLockEnable.Name = "ToolStripLockEnable";
            this.ToolStripLockEnable.Size = new System.Drawing.Size(92, 22);
            this.ToolStripLockEnable.Text = "LockEnable";
            this.ToolStripLockEnable.ToolTipText = "Whether to enable cheat lock";
            this.ToolStripLockEnable.CheckedChanged += new System.EventHandler(this.ToolStripLockEnable_CheckedChanged);
            this.ToolStripLockEnable.Click += new System.EventHandler(this.ToolStripLockEnable_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolStripAutoRefresh
            // 
            this.ToolStripAutoRefresh.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripAutoRefresh.Image")));
            this.ToolStripAutoRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripAutoRefresh.Name = "ToolStripAutoRefresh";
            this.ToolStripAutoRefresh.Size = new System.Drawing.Size(96, 22);
            this.ToolStripAutoRefresh.Text = "AutoRefresh";
            this.ToolStripAutoRefresh.ToolTipText = "Whether to enable auto refresh";
            this.ToolStripAutoRefresh.CheckedChanged += new System.EventHandler(this.ToolStripAutoRefresh_CheckedChanged);
            this.ToolStripAutoRefresh.Click += new System.EventHandler(this.ToolStripAutoRefresh_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolStripSettings
            // 
            this.ToolStripSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripSettings.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripSettings.Image")));
            this.ToolStripSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripSettings.Name = "ToolStripSettings";
            this.ToolStripSettings.Size = new System.Drawing.Size(23, 22);
            this.ToolStripSettings.Text = "Settings";
            this.ToolStripSettings.ToolTipText = "Settings";
            this.ToolStripSettings.Click += new System.EventHandler(this.ToolStripSettings_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMsg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ToolStripMsg
            // 
            this.ToolStripMsg.AutoToolTip = true;
            this.ToolStripMsg.ForeColor = System.Drawing.Color.White;
            this.ToolStripMsg.Name = "ToolStripMsg";
            this.ToolStripMsg.Size = new System.Drawing.Size(785, 17);
            this.ToolStripMsg.Spring = true;
            this.ToolStripMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CheatGridView);
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Controls.Add(this.ToolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 450);
            this.panel1.TabIndex = 0;
            // 
            // CheatGridView
            // 
            this.CheatGridView.AllowUserToAddRows = false;
            this.CheatGridView.AllowUserToResizeRows = false;
            this.CheatGridView.BackgroundColor = System.Drawing.SystemColors.ControlDarkDark;
            this.CheatGridView.BaseRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(115)))), ((int)(((byte)(129)))));
            this.CheatGridView.BaseRowColorEnabled = true;
            this.CheatGridView.BaseRowColorInterleaved = true;
            this.CheatGridView.BaseRowGroupOrder = GroupGridView.GroupGridView.Order.Ascending;
            this.CheatGridView.BaseRowGroupOrderColumn = -1;
            this.CheatGridView.BaseRowSingleGroupEnabled = false;
            this.CheatGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CheatGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CheatGridViewDel,
            this.CheatGridViewAddress,
            this.CheatGridViewType,
            this.CheatGridViewActive,
            this.CheatGridViewValue,
            this.CheatGridViewSection,
            this.CheatGridViewSID,
            this.CheatGridViewLock,
            this.CheatGridViewDescription});
            this.CheatGridView.ContextMenuStrip = this.CheatGridMenu;
            this.CheatGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CheatGridView.GridColor = System.Drawing.Color.Silver;
            this.CheatGridView.GroupByColumns = ((System.Collections.Generic.List<int>)(resources.GetObject("CheatGridView.GroupByColumns")));
            this.CheatGridView.GroupByColumnsOnlyBeforeUnderlineEnabled = true;
            this.CheatGridView.GroupByEnabled = true;
            this.CheatGridView.GroupByNonsequenceEnabled = false;
            this.CheatGridView.GroupByNullValueEnabled = true;
            this.CheatGridView.Location = new System.Drawing.Point(0, 25);
            this.CheatGridView.Name = "CheatGridView";
            this.CheatGridView.RowHeadersCollapse = ((System.Drawing.Bitmap)(resources.GetObject("CheatGridView.RowHeadersCollapse")));
            this.CheatGridView.RowHeadersExpand = ((System.Drawing.Bitmap)(resources.GetObject("CheatGridView.RowHeadersExpand")));
            this.CheatGridView.RowHeadersSeparater = ((System.Drawing.Bitmap)(resources.GetObject("CheatGridView.RowHeadersSeparater")));
            this.CheatGridView.RowHeadersSeparaterEnd = ((System.Drawing.Bitmap)(resources.GetObject("CheatGridView.RowHeadersSeparaterEnd")));
            this.CheatGridView.RowHeadersSeparaterWireEnabled = true;
            this.CheatGridView.RowHeadersWidth = 16;
            this.CheatGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.CheatGridView.RowTemplate.Height = 24;
            this.CheatGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CheatGridView.Size = new System.Drawing.Size(800, 403);
            this.CheatGridView.TabIndex = 2;
            this.CheatGridView.TabStop = false;
            this.CheatGridView.TopLeftHeaderButtonEnabled = true;
            this.CheatGridView.TopLeftHeaderCollapseAll = ((System.Drawing.Bitmap)(resources.GetObject("CheatGridView.TopLeftHeaderCollapseAll")));
            this.CheatGridView.TopLeftHeaderExpandAll = ((System.Drawing.Bitmap)(resources.GetObject("CheatGridView.TopLeftHeaderExpandAll")));
            this.CheatGridView.VirtualMode = true;
            this.CheatGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CheatGridView_CellContentClick);
            this.CheatGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.CheatGridView_CellEndEdit);
            this.CheatGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.CheatGridView_CellValidating);
            this.CheatGridView.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.CheatGridView_CellValueNeeded);
            this.CheatGridView.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.CheatGridView_CellValuePushed);
            this.CheatGridView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.CheatGridView_EditingControlShowing);
            this.CheatGridView.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.CheatGridView_RowPostPaint);
            // 
            // CheatGridViewDel
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.NullValue = "Delete";
            this.CheatGridViewDel.DefaultCellStyle = dataGridViewCellStyle1;
            this.CheatGridViewDel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CheatGridViewDel.HeaderText = "Delete";
            this.CheatGridViewDel.Name = "CheatGridViewDel";
            this.CheatGridViewDel.Width = 50;
            // 
            // CheatGridViewAddress
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            this.CheatGridViewAddress.DefaultCellStyle = dataGridViewCellStyle2;
            this.CheatGridViewAddress.HeaderText = "Address";
            this.CheatGridViewAddress.Name = "CheatGridViewAddress";
            this.CheatGridViewAddress.ReadOnly = true;
            this.CheatGridViewAddress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CheatGridViewType
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            this.CheatGridViewType.DefaultCellStyle = dataGridViewCellStyle3;
            this.CheatGridViewType.HeaderText = "Type";
            this.CheatGridViewType.Name = "CheatGridViewType";
            this.CheatGridViewType.ReadOnly = true;
            this.CheatGridViewType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CheatGridViewType.Width = 60;
            // 
            // CheatGridViewActive
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.NullValue = "X";
            this.CheatGridViewActive.DefaultCellStyle = dataGridViewCellStyle4;
            this.CheatGridViewActive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CheatGridViewActive.HeaderText = "X";
            this.CheatGridViewActive.Name = "CheatGridViewActive";
            this.CheatGridViewActive.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CheatGridViewActive.Text = "X";
            this.CheatGridViewActive.Width = 25;
            // 
            // CheatGridViewValue
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            this.CheatGridViewValue.DefaultCellStyle = dataGridViewCellStyle5;
            this.CheatGridViewValue.HeaderText = "Value";
            this.CheatGridViewValue.Name = "CheatGridViewValue";
            // 
            // CheatGridViewSection
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            this.CheatGridViewSection.DefaultCellStyle = dataGridViewCellStyle6;
            this.CheatGridViewSection.HeaderText = "Section";
            this.CheatGridViewSection.Name = "CheatGridViewSection";
            this.CheatGridViewSection.ReadOnly = true;
            this.CheatGridViewSection.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CheatGridViewSID
            // 
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            this.CheatGridViewSID.DefaultCellStyle = dataGridViewCellStyle7;
            this.CheatGridViewSID.HeaderText = "SID";
            this.CheatGridViewSID.Name = "CheatGridViewSID";
            this.CheatGridViewSID.ReadOnly = true;
            this.CheatGridViewSID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CheatGridViewSID.Width = 20;
            // 
            // CheatGridViewLock
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.NullValue = false;
            this.CheatGridViewLock.DefaultCellStyle = dataGridViewCellStyle8;
            this.CheatGridViewLock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CheatGridViewLock.HeaderText = "Lock";
            this.CheatGridViewLock.Name = "CheatGridViewLock";
            this.CheatGridViewLock.Width = 35;
            // 
            // CheatGridViewDescription
            // 
            this.CheatGridViewDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.White;
            this.CheatGridViewDescription.DefaultCellStyle = dataGridViewCellStyle9;
            this.CheatGridViewDescription.HeaderText = "Description";
            this.CheatGridViewDescription.Name = "CheatGridViewDescription";
            this.CheatGridViewDescription.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // RefreshLock
            // 
            this.RefreshLock.Enabled = true;
            this.RefreshLock.Interval = 500;
            this.RefreshLock.Tick += new System.EventHandler(this.RefreshLock_Tick);
            // 
            // AutoRefreshTimer
            // 
            this.AutoRefreshTimer.Interval = 2500;
            this.AutoRefreshTimer.Tick += new System.EventHandler(this.AutoRefreshTimer_Tick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Name = "Main";
            this.Text = "PS4 Cheater Neo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Shown += new System.EventHandler(this.Main_Shown);
            this.CheatGridMenu.ResumeLayout(false);
            this.ToolStrip1.ResumeLayout(false);
            this.ToolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CheatGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private GroupGridView.GroupGridView CheatGridView;
        private System.Windows.Forms.ToolStrip ToolStrip1;
        private System.Windows.Forms.ToolStripButton ToolStripSend;
        private System.Windows.Forms.ToolStripButton ToolStripNewQuery;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripMsg;
        private System.Windows.Forms.Timer RefreshLock;
        private System.Windows.Forms.ToolStripButton ToolStripOpen;
        private System.Windows.Forms.ToolStripButton ToolStripSave;
        private System.Windows.Forms.OpenFileDialog OpenCheatDialog;
        private System.Windows.Forms.SaveFileDialog SaveCheatDialog;
        private System.Windows.Forms.ToolStripButton ToolStripRefreshCheat;
        private System.Windows.Forms.ContextMenuStrip CheatGridMenu;
        private System.Windows.Forms.ToolStripMenuItem CheatGridMenuHexEditor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem CheatGridMenuLock;
        private System.Windows.Forms.ToolStripMenuItem CheatGridMenuUnlock;
        private System.Windows.Forms.ToolStripMenuItem CheatGridMenuActive;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem CheatGridMenuEdit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem CheatGridMenuFindPointer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem CheatGridMenuDelete;
        private System.Windows.Forms.ToolStripButton ToolStripAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripButton ToolStripLockEnable;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripLabel ToolStripProcessInfo;
        private System.Windows.Forms.ToolStripButton ToolStripCollapseAll;
        private System.Windows.Forms.ToolStripButton ToolStripExpandAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ToolStripSettings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton ToolStripHexView;
        private System.Windows.Forms.ToolStripMenuItem CheatGridMenuCopyAddress;
        private System.Windows.Forms.DataGridViewButtonColumn CheatGridViewDel;
        private System.Windows.Forms.DataGridViewTextBoxColumn CheatGridViewAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn CheatGridViewType;
        private System.Windows.Forms.DataGridViewButtonColumn CheatGridViewActive;
        private GroupGridView.DataGridViewUpDownColumn CheatGridViewValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn CheatGridViewSection;
        private GroupGridView.DataGridViewUpDownColumn CheatGridViewSID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CheatGridViewLock;
        private System.Windows.Forms.DataGridViewTextBoxColumn CheatGridViewDescription;
        private System.Windows.Forms.Timer AutoRefreshTimer;
        private System.Windows.Forms.ToolStripButton ToolStripAutoRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    }
}

