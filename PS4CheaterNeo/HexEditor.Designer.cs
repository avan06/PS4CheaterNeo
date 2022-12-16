namespace PS4CheaterNeo
{
    partial class HexEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HexEditor));
            this.SplitContainer1 = new PS4CheaterNeo.CollapsibleSplitContainer();
            this.HexView = new Be.Windows.Forms.HexBox();
            this.HexViewMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.HexViewMenuByteGroup = new System.Windows.Forms.ToolStripComboBox();
            this.HexViewMenuGroupSize = new System.Windows.Forms.ToolStripComboBox();
            this.HexViewMenuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.SplitContainer2 = new PS4CheaterNeo.CollapsibleSplitContainer();
            this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.InfoBox0Panel = new System.Windows.Forms.Panel();
            this.SwapBytesBox = new System.Windows.Forms.CheckBox();
            this.InfoBox0 = new System.Windows.Forms.TextBox();
            this.TableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.HexBox = new System.Windows.Forms.CheckBox();
            this.CommitBtn = new System.Windows.Forms.Button();
            this.AddToCheatGridBtn = new System.Windows.Forms.Button();
            this.TableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.NextBtn = new System.Windows.Forms.Button();
            this.PreviousBtn = new System.Windows.Forms.Button();
            this.PageBox = new System.Windows.Forms.ComboBox();
            this.TableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.RefreshBtn = new System.Windows.Forms.Button();
            this.AutoRefreshBox = new System.Windows.Forms.CheckBox();
            this.InfoBoxPanel = new System.Windows.Forms.Panel();
            this.TableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.InfoBox4SSeparator = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.InfoBoxB = new System.Windows.Forms.TextBox();
            this.InfoBoxDSeparator = new System.Windows.Forms.Panel();
            this.InfoBox4Separator = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.InfoBox4S = new System.Windows.Forms.TextBox();
            this.InfoBox3S = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.InfoBox2S = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.InfoBoxF = new System.Windows.Forms.TextBox();
            this.InfoBox4U = new System.Windows.Forms.TextBox();
            this.InfoBox3U = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.InfoBox1U = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.InfoBox2U = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.InfoBoxD = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.InfoBox1S = new System.Windows.Forms.TextBox();
            this.TableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.FindBtn = new System.Windows.Forms.Button();
            this.ForwardBox = new System.Windows.Forms.CheckBox();
            this.TableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.AssemblerBtn = new System.Windows.Forms.Button();
            this.GroupBoxAsm = new System.Windows.Forms.GroupBox();
            this.AsmBox1 = new System.Windows.Forms.RichTextBox();
            this.AutoRefreshTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).BeginInit();
            this.SplitContainer1.Panel1.SuspendLayout();
            this.SplitContainer1.Panel2.SuspendLayout();
            this.SplitContainer1.SuspendLayout();
            this.HexViewMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer2)).BeginInit();
            this.SplitContainer2.Panel1.SuspendLayout();
            this.SplitContainer2.Panel2.SuspendLayout();
            this.SplitContainer2.SuspendLayout();
            this.TableLayoutPanel1.SuspendLayout();
            this.InfoBox0Panel.SuspendLayout();
            this.TableLayoutPanel4.SuspendLayout();
            this.TableLayoutPanel2.SuspendLayout();
            this.TableLayoutPanel3.SuspendLayout();
            this.InfoBoxPanel.SuspendLayout();
            this.TableLayoutPanel5.SuspendLayout();
            this.TableLayoutPanel7.SuspendLayout();
            this.TableLayoutPanel6.SuspendLayout();
            this.GroupBoxAsm.SuspendLayout();
            this.SuspendLayout();
            // 
            // SplitContainer1
            // 
            this.SplitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer1.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer1.Name = "SplitContainer1";
            // 
            // SplitContainer1.Panel1
            // 
            this.SplitContainer1.Panel1.Controls.Add(this.HexView);
            // 
            // SplitContainer1.Panel2
            // 
            this.SplitContainer1.Panel2.Controls.Add(this.SplitContainer2);
            this.SplitContainer1.Size = new System.Drawing.Size(800, 450);
            this.SplitContainer1.SplitterButtonBitmap = ((System.Drawing.Bitmap)(resources.GetObject("SplitContainer1.SplitterButtonBitmap")));
            this.SplitContainer1.SplitterButtonLocation = PS4CheaterNeo.ButtonLocation.Panel2;
            this.SplitContainer1.SplitterButtonSize = 13;
            this.SplitContainer1.SplitterButtonStyle = PS4CheaterNeo.ButtonStyle.SingleImage;
            this.SplitContainer1.SplitterDistance = 569;
            this.SplitContainer1.TabIndex = 0;
            // 
            // HexView
            // 
            this.HexView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.HexView.ChangedFinishForeColor = System.Drawing.Color.LimeGreen;
            this.HexView.ColumnInfoVisible = true;
            this.HexView.ContextMenuStrip = this.HexViewMenu;
            this.HexView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HexView.EnableAutoChangedPosSetFinish = false;
            this.HexView.EnableCut = false;
            this.HexView.EnableDelete = false;
            this.HexView.EnableOverwritePaste = true;
            this.HexView.EnablePaste = true;
            this.HexView.EnableRetainChangedFinishPos = true;
            this.HexView.EnableRetainChangedPos = true;
            this.HexView.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.HexView.ForeColor = System.Drawing.Color.White;
            this.HexView.GroupSeparatorVisible = true;
            this.HexView.KeyDownControlCContentType = Be.Windows.Forms.HexBox.StringContentType.Hex;
            this.HexView.LineInfoOffsetLength = 10;
            this.HexView.LineInfoVisible = true;
            this.HexView.Location = new System.Drawing.Point(0, 0);
            this.HexView.Name = "HexView";
            this.HexView.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.HexView.Size = new System.Drawing.Size(567, 448);
            this.HexView.StringViewVisible = true;
            this.HexView.TabIndex = 0;
            this.HexView.UseFixedBytesPerLine = true;
            this.HexView.VScrollBarVisible = true;
            this.HexView.ZeroBytesForeColor = System.Drawing.Color.DimGray;
            this.HexView.SelectionStartChanged += new System.EventHandler(this.HexView_SelectionStartChanged);
            // 
            // HexViewMenu
            // 
            this.HexViewMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HexViewMenuByteGroup,
            this.HexViewMenuGroupSize,
            this.HexViewMenuSeparator1});
            this.HexViewMenu.Name = "HexViewMenu";
            this.HexViewMenu.Size = new System.Drawing.Size(211, 64);
            // 
            // HexViewMenuByteGroup
            // 
            this.HexViewMenuByteGroup.Name = "HexViewMenuByteGroup";
            this.HexViewMenuByteGroup.Size = new System.Drawing.Size(150, 23);
            this.HexViewMenuByteGroup.Text = "HexViewByteGroup";
            this.HexViewMenuByteGroup.SelectedIndexChanged += new System.EventHandler(this.HexViewByteGroup_SelectedIndexChanged);
            // 
            // HexViewMenuGroupSize
            // 
            this.HexViewMenuGroupSize.Name = "HexViewMenuGroupSize";
            this.HexViewMenuGroupSize.Size = new System.Drawing.Size(150, 23);
            this.HexViewMenuGroupSize.Text = "HexViewGroupSize";
            this.HexViewMenuGroupSize.SelectedIndexChanged += new System.EventHandler(this.HexViewGroupSize_SelectedIndexChanged);
            // 
            // HexViewMenuSeparator1
            // 
            this.HexViewMenuSeparator1.Name = "HexViewMenuSeparator1";
            this.HexViewMenuSeparator1.Size = new System.Drawing.Size(207, 6);
            // 
            // SplitContainer2
            // 
            this.SplitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer2.ForeColor = System.Drawing.Color.White;
            this.SplitContainer2.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.SplitContainer2.Name = "SplitContainer2";
            this.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer2.Panel1
            // 
            this.SplitContainer2.Panel1.Controls.Add(this.TableLayoutPanel1);
            // 
            // SplitContainer2.Panel2
            // 
            this.SplitContainer2.Panel2.Controls.Add(this.TableLayoutPanel6);
            this.SplitContainer2.Panel2MinSize = 1;
            this.SplitContainer2.SingleImageCollapsePanel2 = false;
            this.SplitContainer2.Size = new System.Drawing.Size(227, 450);
            this.SplitContainer2.SplitterButtonBitmap = ((System.Drawing.Bitmap)(resources.GetObject("SplitContainer2.SplitterButtonBitmap")));
            this.SplitContainer2.SplitterButtonLocation = PS4CheaterNeo.ButtonLocation.Panel1;
            this.SplitContainer2.SplitterButtonPosition = PS4CheaterNeo.ButtonPosition.BottomRight;
            this.SplitContainer2.SplitterButtonSize = 13;
            this.SplitContainer2.SplitterButtonStyle = PS4CheaterNeo.ButtonStyle.SingleImage;
            this.SplitContainer2.SplitterDistance = 374;
            this.SplitContainer2.TabIndex = 1;
            // 
            // TableLayoutPanel1
            // 
            this.TableLayoutPanel1.AutoSize = true;
            this.TableLayoutPanel1.ColumnCount = 1;
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel1.Controls.Add(this.InfoBox0Panel, 0, 7);
            this.TableLayoutPanel1.Controls.Add(this.TableLayoutPanel4, 0, 5);
            this.TableLayoutPanel1.Controls.Add(this.CommitBtn, 0, 3);
            this.TableLayoutPanel1.Controls.Add(this.AddToCheatGridBtn, 0, 4);
            this.TableLayoutPanel1.Controls.Add(this.TableLayoutPanel2, 0, 0);
            this.TableLayoutPanel1.Controls.Add(this.PageBox, 0, 1);
            this.TableLayoutPanel1.Controls.Add(this.TableLayoutPanel3, 0, 2);
            this.TableLayoutPanel1.Controls.Add(this.InfoBoxPanel, 0, 8);
            this.TableLayoutPanel1.Controls.Add(this.TableLayoutPanel7, 0, 6);
            this.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.TableLayoutPanel1.Name = "TableLayoutPanel1";
            this.TableLayoutPanel1.RowCount = 9;
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel1.Size = new System.Drawing.Size(225, 372);
            this.TableLayoutPanel1.TabIndex = 14;
            // 
            // InfoBox0Panel
            // 
            this.InfoBox0Panel.Controls.Add(this.SwapBytesBox);
            this.InfoBox0Panel.Controls.Add(this.InfoBox0);
            this.InfoBox0Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBox0Panel.Location = new System.Drawing.Point(0, 207);
            this.InfoBox0Panel.Margin = new System.Windows.Forms.Padding(0);
            this.InfoBox0Panel.Name = "InfoBox0Panel";
            this.InfoBox0Panel.Size = new System.Drawing.Size(225, 20);
            this.InfoBox0Panel.TabIndex = 14;
            // 
            // SwapBytesBox
            // 
            this.SwapBytesBox.AutoSize = true;
            this.SwapBytesBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.SwapBytesBox.Location = new System.Drawing.Point(176, 0);
            this.SwapBytesBox.Name = "SwapBytesBox";
            this.SwapBytesBox.Size = new System.Drawing.Size(49, 20);
            this.SwapBytesBox.TabIndex = 13;
            this.SwapBytesBox.Text = "Swap";
            this.SwapBytesBox.UseVisualStyleBackColor = true;
            this.SwapBytesBox.CheckedChanged += new System.EventHandler(this.SwapBytesBox_CheckedChanged);
            // 
            // InfoBox0
            // 
            this.InfoBox0.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InfoBox0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InfoBox0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoBox0.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoBox0.ForeColor = System.Drawing.Color.White;
            this.InfoBox0.Location = new System.Drawing.Point(-3, 0);
            this.InfoBox0.Margin = new System.Windows.Forms.Padding(0);
            this.InfoBox0.Name = "InfoBox0";
            this.InfoBox0.ReadOnly = true;
            this.InfoBox0.Size = new System.Drawing.Size(176, 20);
            this.InfoBox0.TabIndex = 2;
            // 
            // TableLayoutPanel4
            // 
            this.TableLayoutPanel4.AutoSize = true;
            this.TableLayoutPanel4.ColumnCount = 2;
            this.TableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.TableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel4.Controls.Add(this.InputBox, 1, 0);
            this.TableLayoutPanel4.Controls.Add(this.HexBox, 0, 0);
            this.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel4.Location = new System.Drawing.Point(0, 152);
            this.TableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.TableLayoutPanel4.Name = "TableLayoutPanel4";
            this.TableLayoutPanel4.RowCount = 1;
            this.TableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel4.Size = new System.Drawing.Size(225, 22);
            this.TableLayoutPanel4.TabIndex = 11;
            // 
            // InputBox
            // 
            this.InputBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InputBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InputBox.ForeColor = System.Drawing.Color.White;
            this.InputBox.Location = new System.Drawing.Point(65, 0);
            this.InputBox.Margin = new System.Windows.Forms.Padding(0);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(160, 22);
            this.InputBox.TabIndex = 7;
            // 
            // HexBox
            // 
            this.HexBox.AutoSize = true;
            this.HexBox.Checked = true;
            this.HexBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HexBox.Location = new System.Drawing.Point(3, 3);
            this.HexBox.Name = "HexBox";
            this.HexBox.Size = new System.Drawing.Size(43, 16);
            this.HexBox.TabIndex = 10;
            this.HexBox.Text = "Hex";
            this.HexBox.UseVisualStyleBackColor = true;
            // 
            // CommitBtn
            // 
            this.CommitBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CommitBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CommitBtn.ForeColor = System.Drawing.Color.White;
            this.CommitBtn.Location = new System.Drawing.Point(0, 90);
            this.CommitBtn.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.CommitBtn.Name = "CommitBtn";
            this.CommitBtn.Size = new System.Drawing.Size(225, 25);
            this.CommitBtn.TabIndex = 5;
            this.CommitBtn.Text = "Commit";
            this.CommitBtn.UseVisualStyleBackColor = true;
            this.CommitBtn.Click += new System.EventHandler(this.CommitBtn_Click);
            // 
            // AddToCheatGridBtn
            // 
            this.AddToCheatGridBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddToCheatGridBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddToCheatGridBtn.ForeColor = System.Drawing.Color.White;
            this.AddToCheatGridBtn.Location = new System.Drawing.Point(0, 121);
            this.AddToCheatGridBtn.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.AddToCheatGridBtn.Name = "AddToCheatGridBtn";
            this.AddToCheatGridBtn.Size = new System.Drawing.Size(225, 25);
            this.AddToCheatGridBtn.TabIndex = 6;
            this.AddToCheatGridBtn.Text = "Add To Cheat Grid";
            this.AddToCheatGridBtn.UseVisualStyleBackColor = true;
            this.AddToCheatGridBtn.Click += new System.EventHandler(this.AddToCheatGridBtn_Click);
            // 
            // TableLayoutPanel2
            // 
            this.TableLayoutPanel2.AutoSize = true;
            this.TableLayoutPanel2.ColumnCount = 2;
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel2.Controls.Add(this.NextBtn, 1, 0);
            this.TableLayoutPanel2.Controls.Add(this.PreviousBtn, 0, 0);
            this.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.TableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.TableLayoutPanel2.Name = "TableLayoutPanel2";
            this.TableLayoutPanel2.RowCount = 1;
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel2.Size = new System.Drawing.Size(225, 30);
            this.TableLayoutPanel2.TabIndex = 14;
            // 
            // NextBtn
            // 
            this.NextBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NextBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NextBtn.ForeColor = System.Drawing.Color.White;
            this.NextBtn.Location = new System.Drawing.Point(112, 3);
            this.NextBtn.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.NextBtn.Name = "NextBtn";
            this.NextBtn.Size = new System.Drawing.Size(113, 24);
            this.NextBtn.TabIndex = 1;
            this.NextBtn.Text = "Next";
            this.NextBtn.UseVisualStyleBackColor = true;
            this.NextBtn.Click += new System.EventHandler(this.NextBtn_Click);
            // 
            // PreviousBtn
            // 
            this.PreviousBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PreviousBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PreviousBtn.ForeColor = System.Drawing.Color.White;
            this.PreviousBtn.Location = new System.Drawing.Point(0, 3);
            this.PreviousBtn.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.PreviousBtn.Name = "PreviousBtn";
            this.PreviousBtn.Size = new System.Drawing.Size(112, 24);
            this.PreviousBtn.TabIndex = 0;
            this.PreviousBtn.Text = "Previous";
            this.PreviousBtn.UseVisualStyleBackColor = true;
            this.PreviousBtn.Click += new System.EventHandler(this.PreviousBtn_Click);
            // 
            // PageBox
            // 
            this.PageBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.PageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PageBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PageBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PageBox.ForeColor = System.Drawing.Color.White;
            this.PageBox.FormattingEnabled = true;
            this.PageBox.Location = new System.Drawing.Point(0, 33);
            this.PageBox.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.PageBox.Name = "PageBox";
            this.PageBox.Size = new System.Drawing.Size(225, 20);
            this.PageBox.TabIndex = 4;
            this.PageBox.SelectedIndexChanged += new System.EventHandler(this.PageBox_SelectedIndexChanged);
            // 
            // TableLayoutPanel3
            // 
            this.TableLayoutPanel3.AutoSize = true;
            this.TableLayoutPanel3.ColumnCount = 2;
            this.TableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.TableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel3.Controls.Add(this.RefreshBtn, 1, 0);
            this.TableLayoutPanel3.Controls.Add(this.AutoRefreshBox, 0, 0);
            this.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel3.Location = new System.Drawing.Point(0, 59);
            this.TableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.TableLayoutPanel3.Name = "TableLayoutPanel3";
            this.TableLayoutPanel3.RowCount = 1;
            this.TableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel3.Size = new System.Drawing.Size(225, 25);
            this.TableLayoutPanel3.TabIndex = 16;
            // 
            // RefreshBtn
            // 
            this.RefreshBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RefreshBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RefreshBtn.ForeColor = System.Drawing.Color.White;
            this.RefreshBtn.Location = new System.Drawing.Point(65, 0);
            this.RefreshBtn.Margin = new System.Windows.Forms.Padding(0);
            this.RefreshBtn.Name = "RefreshBtn";
            this.RefreshBtn.Size = new System.Drawing.Size(160, 25);
            this.RefreshBtn.TabIndex = 3;
            this.RefreshBtn.Text = "Refresh";
            this.RefreshBtn.UseVisualStyleBackColor = true;
            this.RefreshBtn.Click += new System.EventHandler(this.RefreshBtn_Click);
            // 
            // AutoRefreshBox
            // 
            this.AutoRefreshBox.AutoSize = true;
            this.AutoRefreshBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AutoRefreshBox.Location = new System.Drawing.Point(3, 3);
            this.AutoRefreshBox.Name = "AutoRefreshBox";
            this.AutoRefreshBox.Size = new System.Drawing.Size(59, 19);
            this.AutoRefreshBox.TabIndex = 12;
            this.AutoRefreshBox.Text = "Auto";
            this.AutoRefreshBox.UseVisualStyleBackColor = true;
            this.AutoRefreshBox.CheckedChanged += new System.EventHandler(this.AutoRefreshBox_CheckedChanged);
            // 
            // InfoBoxPanel
            // 
            this.InfoBoxPanel.AutoScroll = true;
            this.InfoBoxPanel.Controls.Add(this.TableLayoutPanel5);
            this.InfoBoxPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBoxPanel.Location = new System.Drawing.Point(3, 230);
            this.InfoBoxPanel.Name = "InfoBoxPanel";
            this.InfoBoxPanel.Size = new System.Drawing.Size(219, 139);
            this.InfoBoxPanel.TabIndex = 17;
            // 
            // TableLayoutPanel5
            // 
            this.TableLayoutPanel5.AutoSize = true;
            this.TableLayoutPanel5.ColumnCount = 2;
            this.TableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.TableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 152F));
            this.TableLayoutPanel5.Controls.Add(this.InfoBox4SSeparator, 0, 12);
            this.TableLayoutPanel5.Controls.Add(this.label11, 0, 13);
            this.TableLayoutPanel5.Controls.Add(this.InfoBoxB, 1, 13);
            this.TableLayoutPanel5.Controls.Add(this.InfoBoxDSeparator, 0, 7);
            this.TableLayoutPanel5.Controls.Add(this.InfoBox4Separator, 0, 4);
            this.TableLayoutPanel5.Controls.Add(this.label10, 0, 11);
            this.TableLayoutPanel5.Controls.Add(this.InfoBox4S, 2, 11);
            this.TableLayoutPanel5.Controls.Add(this.InfoBox3S, 1, 10);
            this.TableLayoutPanel5.Controls.Add(this.label9, 0, 10);
            this.TableLayoutPanel5.Controls.Add(this.InfoBox2S, 1, 9);
            this.TableLayoutPanel5.Controls.Add(this.label8, 0, 9);
            this.TableLayoutPanel5.Controls.Add(this.label6, 0, 6);
            this.TableLayoutPanel5.Controls.Add(this.InfoBoxF, 1, 5);
            this.TableLayoutPanel5.Controls.Add(this.InfoBox4U, 1, 3);
            this.TableLayoutPanel5.Controls.Add(this.InfoBox3U, 1, 2);
            this.TableLayoutPanel5.Controls.Add(this.label3, 0, 2);
            this.TableLayoutPanel5.Controls.Add(this.InfoBox1U, 1, 0);
            this.TableLayoutPanel5.Controls.Add(this.label1, 0, 0);
            this.TableLayoutPanel5.Controls.Add(this.InfoBox2U, 1, 1);
            this.TableLayoutPanel5.Controls.Add(this.label2, 0, 1);
            this.TableLayoutPanel5.Controls.Add(this.label4, 0, 3);
            this.TableLayoutPanel5.Controls.Add(this.label5, 0, 5);
            this.TableLayoutPanel5.Controls.Add(this.InfoBoxD, 1, 6);
            this.TableLayoutPanel5.Controls.Add(this.label7, 0, 8);
            this.TableLayoutPanel5.Controls.Add(this.InfoBox1S, 1, 8);
            this.TableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.TableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.TableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.TableLayoutPanel5.Name = "TableLayoutPanel5";
            this.TableLayoutPanel5.RowCount = 15;
            this.TableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.TableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.TableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.TableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel5.Size = new System.Drawing.Size(202, 255);
            this.TableLayoutPanel5.TabIndex = 13;
            // 
            // InfoBox4SSeparator
            // 
            this.InfoBox4SSeparator.BackColor = System.Drawing.Color.White;
            this.TableLayoutPanel5.SetColumnSpan(this.InfoBox4SSeparator, 2);
            this.InfoBox4SSeparator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBox4SSeparator.Location = new System.Drawing.Point(3, 213);
            this.InfoBox4SSeparator.Name = "InfoBox4SSeparator";
            this.InfoBox4SSeparator.Size = new System.Drawing.Size(196, 1);
            this.InfoBox4SSeparator.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(3, 215);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 20);
            this.label11.TabIndex = 21;
            this.label11.Text = "Binary";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InfoBoxB
            // 
            this.InfoBoxB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InfoBoxB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoBoxB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBoxB.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoBoxB.ForeColor = System.Drawing.Color.White;
            this.InfoBoxB.Location = new System.Drawing.Point(50, 215);
            this.InfoBoxB.Margin = new System.Windows.Forms.Padding(0);
            this.InfoBoxB.Name = "InfoBoxB";
            this.InfoBoxB.ReadOnly = true;
            this.InfoBoxB.Size = new System.Drawing.Size(152, 22);
            this.InfoBoxB.TabIndex = 20;
            // 
            // InfoBoxDSeparator
            // 
            this.InfoBoxDSeparator.BackColor = System.Drawing.Color.White;
            this.TableLayoutPanel5.SetColumnSpan(this.InfoBoxDSeparator, 2);
            this.InfoBoxDSeparator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBoxDSeparator.Location = new System.Drawing.Point(3, 128);
            this.InfoBoxDSeparator.Name = "InfoBoxDSeparator";
            this.InfoBoxDSeparator.Size = new System.Drawing.Size(196, 1);
            this.InfoBoxDSeparator.TabIndex = 14;
            // 
            // InfoBox4Separator
            // 
            this.InfoBox4Separator.BackColor = System.Drawing.Color.White;
            this.TableLayoutPanel5.SetColumnSpan(this.InfoBox4Separator, 2);
            this.InfoBox4Separator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBox4Separator.Location = new System.Drawing.Point(3, 83);
            this.InfoBox4Separator.Name = "InfoBox4Separator";
            this.InfoBox4Separator.Size = new System.Drawing.Size(196, 1);
            this.InfoBox4Separator.TabIndex = 14;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(3, 190);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 20);
            this.label10.TabIndex = 19;
            this.label10.Text = "Int64";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InfoBox4S
            // 
            this.InfoBox4S.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InfoBox4S.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoBox4S.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBox4S.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoBox4S.ForeColor = System.Drawing.Color.White;
            this.InfoBox4S.Location = new System.Drawing.Point(50, 190);
            this.InfoBox4S.Margin = new System.Windows.Forms.Padding(0);
            this.InfoBox4S.Name = "InfoBox4S";
            this.InfoBox4S.ReadOnly = true;
            this.InfoBox4S.Size = new System.Drawing.Size(152, 22);
            this.InfoBox4S.TabIndex = 18;
            // 
            // InfoBox3S
            // 
            this.InfoBox3S.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InfoBox3S.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoBox3S.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBox3S.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoBox3S.ForeColor = System.Drawing.Color.White;
            this.InfoBox3S.Location = new System.Drawing.Point(50, 170);
            this.InfoBox3S.Margin = new System.Windows.Forms.Padding(0);
            this.InfoBox3S.Name = "InfoBox3S";
            this.InfoBox3S.ReadOnly = true;
            this.InfoBox3S.Size = new System.Drawing.Size(152, 22);
            this.InfoBox3S.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 170);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 20);
            this.label9.TabIndex = 16;
            this.label9.Text = "Int32";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InfoBox2S
            // 
            this.InfoBox2S.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InfoBox2S.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoBox2S.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBox2S.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoBox2S.ForeColor = System.Drawing.Color.White;
            this.InfoBox2S.Location = new System.Drawing.Point(50, 150);
            this.InfoBox2S.Margin = new System.Windows.Forms.Padding(0);
            this.InfoBox2S.Name = "InfoBox2S";
            this.InfoBox2S.ReadOnly = true;
            this.InfoBox2S.Size = new System.Drawing.Size(152, 22);
            this.InfoBox2S.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(3, 150);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 20);
            this.label8.TabIndex = 14;
            this.label8.Text = "Int16";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 105);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "Double";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InfoBoxF
            // 
            this.InfoBoxF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InfoBoxF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoBoxF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBoxF.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoBoxF.ForeColor = System.Drawing.Color.White;
            this.InfoBoxF.Location = new System.Drawing.Point(50, 85);
            this.InfoBoxF.Margin = new System.Windows.Forms.Padding(0);
            this.InfoBoxF.Name = "InfoBoxF";
            this.InfoBoxF.ReadOnly = true;
            this.InfoBoxF.Size = new System.Drawing.Size(152, 22);
            this.InfoBoxF.TabIndex = 9;
            // 
            // InfoBox4U
            // 
            this.InfoBox4U.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InfoBox4U.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoBox4U.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBox4U.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoBox4U.ForeColor = System.Drawing.Color.White;
            this.InfoBox4U.Location = new System.Drawing.Point(50, 60);
            this.InfoBox4U.Margin = new System.Windows.Forms.Padding(0);
            this.InfoBox4U.Name = "InfoBox4U";
            this.InfoBox4U.ReadOnly = true;
            this.InfoBox4U.Size = new System.Drawing.Size(152, 22);
            this.InfoBox4U.TabIndex = 8;
            // 
            // InfoBox3U
            // 
            this.InfoBox3U.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InfoBox3U.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoBox3U.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBox3U.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoBox3U.ForeColor = System.Drawing.Color.White;
            this.InfoBox3U.Location = new System.Drawing.Point(50, 40);
            this.InfoBox3U.Margin = new System.Windows.Forms.Padding(0);
            this.InfoBox3U.Name = "InfoBox3U";
            this.InfoBox3U.ReadOnly = true;
            this.InfoBox3U.Size = new System.Drawing.Size(152, 22);
            this.InfoBox3U.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "UInt32";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InfoBox1U
            // 
            this.InfoBox1U.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InfoBox1U.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoBox1U.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBox1U.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoBox1U.ForeColor = System.Drawing.Color.White;
            this.InfoBox1U.Location = new System.Drawing.Point(50, 0);
            this.InfoBox1U.Margin = new System.Windows.Forms.Padding(0);
            this.InfoBox1U.Name = "InfoBox1U";
            this.InfoBox1U.ReadOnly = true;
            this.InfoBox1U.Size = new System.Drawing.Size(152, 22);
            this.InfoBox1U.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "UInt8";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InfoBox2U
            // 
            this.InfoBox2U.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InfoBox2U.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoBox2U.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBox2U.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoBox2U.ForeColor = System.Drawing.Color.White;
            this.InfoBox2U.Location = new System.Drawing.Point(50, 20);
            this.InfoBox2U.Margin = new System.Windows.Forms.Padding(0);
            this.InfoBox2U.Name = "InfoBox2U";
            this.InfoBox2U.ReadOnly = true;
            this.InfoBox2U.Size = new System.Drawing.Size(152, 22);
            this.InfoBox2U.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "UInt16";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "UInt64";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 20);
            this.label5.TabIndex = 6;
            this.label5.Text = "Float";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InfoBoxD
            // 
            this.InfoBoxD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InfoBoxD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoBoxD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBoxD.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoBoxD.ForeColor = System.Drawing.Color.White;
            this.InfoBoxD.Location = new System.Drawing.Point(50, 105);
            this.InfoBoxD.Margin = new System.Windows.Forms.Padding(0);
            this.InfoBoxD.Name = "InfoBoxD";
            this.InfoBoxD.ReadOnly = true;
            this.InfoBoxD.Size = new System.Drawing.Size(152, 22);
            this.InfoBoxD.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 130);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 20);
            this.label7.TabIndex = 12;
            this.label7.Text = "Int8";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InfoBox1S
            // 
            this.InfoBox1S.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InfoBox1S.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoBox1S.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBox1S.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoBox1S.ForeColor = System.Drawing.Color.White;
            this.InfoBox1S.Location = new System.Drawing.Point(50, 130);
            this.InfoBox1S.Margin = new System.Windows.Forms.Padding(0);
            this.InfoBox1S.Name = "InfoBox1S";
            this.InfoBox1S.ReadOnly = true;
            this.InfoBox1S.Size = new System.Drawing.Size(152, 22);
            this.InfoBox1S.TabIndex = 13;
            // 
            // TableLayoutPanel7
            // 
            this.TableLayoutPanel7.ColumnCount = 2;
            this.TableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.TableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableLayoutPanel7.Controls.Add(this.FindBtn, 1, 0);
            this.TableLayoutPanel7.Controls.Add(this.ForwardBox, 0, 0);
            this.TableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel7.Location = new System.Drawing.Point(0, 177);
            this.TableLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
            this.TableLayoutPanel7.Name = "TableLayoutPanel7";
            this.TableLayoutPanel7.RowCount = 1;
            this.TableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel7.Size = new System.Drawing.Size(225, 30);
            this.TableLayoutPanel7.TabIndex = 18;
            // 
            // FindBtn
            // 
            this.FindBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FindBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FindBtn.ForeColor = System.Drawing.Color.White;
            this.FindBtn.Location = new System.Drawing.Point(65, 3);
            this.FindBtn.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.FindBtn.Name = "FindBtn";
            this.FindBtn.Size = new System.Drawing.Size(160, 24);
            this.FindBtn.TabIndex = 8;
            this.FindBtn.Text = "Find";
            this.FindBtn.UseVisualStyleBackColor = true;
            this.FindBtn.Click += new System.EventHandler(this.FindBtn_Click);
            // 
            // ForwardBox
            // 
            this.ForwardBox.AutoSize = true;
            this.ForwardBox.Checked = true;
            this.ForwardBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ForwardBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ForwardBox.Location = new System.Drawing.Point(0, 0);
            this.ForwardBox.Margin = new System.Windows.Forms.Padding(0);
            this.ForwardBox.Name = "ForwardBox";
            this.ForwardBox.Size = new System.Drawing.Size(65, 30);
            this.ForwardBox.TabIndex = 9;
            this.ForwardBox.Text = "Forward";
            this.ForwardBox.UseVisualStyleBackColor = true;
            // 
            // TableLayoutPanel6
            // 
            this.TableLayoutPanel6.AutoSize = true;
            this.TableLayoutPanel6.ColumnCount = 1;
            this.TableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel6.Controls.Add(this.AssemblerBtn, 0, 0);
            this.TableLayoutPanel6.Controls.Add(this.GroupBoxAsm, 0, 1);
            this.TableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
            this.TableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
            this.TableLayoutPanel6.Name = "TableLayoutPanel6";
            this.TableLayoutPanel6.RowCount = 2;
            this.TableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel6.Size = new System.Drawing.Size(225, 70);
            this.TableLayoutPanel6.TabIndex = 0;
            // 
            // AssemblerBtn
            // 
            this.AssemblerBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AssemblerBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AssemblerBtn.ForeColor = System.Drawing.Color.White;
            this.AssemblerBtn.Location = new System.Drawing.Point(0, 3);
            this.AssemblerBtn.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.AssemblerBtn.Name = "AssemblerBtn";
            this.AssemblerBtn.Size = new System.Drawing.Size(225, 23);
            this.AssemblerBtn.TabIndex = 11;
            this.AssemblerBtn.Text = "AssemblerBox";
            this.AssemblerBtn.UseVisualStyleBackColor = true;
            this.AssemblerBtn.Click += new System.EventHandler(this.AssemblerBtn_Click);
            // 
            // GroupBoxAsm
            // 
            this.GroupBoxAsm.Controls.Add(this.AsmBox1);
            this.GroupBoxAsm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupBoxAsm.ForeColor = System.Drawing.Color.White;
            this.GroupBoxAsm.Location = new System.Drawing.Point(3, 32);
            this.GroupBoxAsm.Name = "GroupBoxAsm";
            this.GroupBoxAsm.Size = new System.Drawing.Size(219, 251);
            this.GroupBoxAsm.TabIndex = 9;
            this.GroupBoxAsm.TabStop = false;
            this.GroupBoxAsm.Text = "Assembler Info";
            // 
            // AsmBox1
            // 
            this.AsmBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.AsmBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AsmBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AsmBox1.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AsmBox1.ForeColor = System.Drawing.Color.White;
            this.AsmBox1.Location = new System.Drawing.Point(3, 18);
            this.AsmBox1.Name = "AsmBox1";
            this.AsmBox1.ReadOnly = true;
            this.AsmBox1.Size = new System.Drawing.Size(213, 230);
            this.AsmBox1.TabIndex = 1;
            this.AsmBox1.Text = "";
            // 
            // AutoRefreshTimer
            // 
            this.AutoRefreshTimer.Interval = 2500;
            this.AutoRefreshTimer.Tick += new System.EventHandler(this.AutoRefreshTimer_Tick);
            // 
            // HexEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.SplitContainer1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "HexEditor";
            this.Text = "HexEditor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HexEditor_FormClosing);
            this.Load += new System.EventHandler(this.HexEditor_Load);
            this.SplitContainer1.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).EndInit();
            this.SplitContainer1.ResumeLayout(false);
            this.HexViewMenu.ResumeLayout(false);
            this.SplitContainer2.Panel1.ResumeLayout(false);
            this.SplitContainer2.Panel1.PerformLayout();
            this.SplitContainer2.Panel2.ResumeLayout(false);
            this.SplitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer2)).EndInit();
            this.SplitContainer2.ResumeLayout(false);
            this.TableLayoutPanel1.ResumeLayout(false);
            this.TableLayoutPanel1.PerformLayout();
            this.InfoBox0Panel.ResumeLayout(false);
            this.InfoBox0Panel.PerformLayout();
            this.TableLayoutPanel4.ResumeLayout(false);
            this.TableLayoutPanel4.PerformLayout();
            this.TableLayoutPanel2.ResumeLayout(false);
            this.TableLayoutPanel3.ResumeLayout(false);
            this.TableLayoutPanel3.PerformLayout();
            this.InfoBoxPanel.ResumeLayout(false);
            this.InfoBoxPanel.PerformLayout();
            this.TableLayoutPanel5.ResumeLayout(false);
            this.TableLayoutPanel5.PerformLayout();
            this.TableLayoutPanel7.ResumeLayout(false);
            this.TableLayoutPanel7.PerformLayout();
            this.TableLayoutPanel6.ResumeLayout(false);
            this.GroupBoxAsm.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CollapsibleSplitContainer SplitContainer1;
        private Be.Windows.Forms.HexBox HexView;
        private System.Windows.Forms.TextBox InputBox;
        private System.Windows.Forms.GroupBox GroupBoxAsm;
        private System.Windows.Forms.CheckBox HexBox;
        private System.Windows.Forms.Button AssemblerBtn;
        private System.Windows.Forms.Timer AutoRefreshTimer;
        private System.Windows.Forms.ContextMenuStrip HexViewMenu;
        private System.Windows.Forms.ToolStripComboBox HexViewMenuGroupSize;
        private System.Windows.Forms.ToolStripComboBox HexViewMenuByteGroup;
        private System.Windows.Forms.ToolStripSeparator HexViewMenuSeparator1;
        private CollapsibleSplitContainer SplitContainer2;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox InfoBoxF;
        private System.Windows.Forms.TextBox InfoBox4U;
        private System.Windows.Forms.TextBox InfoBox3U;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox InfoBox1U;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox InfoBoxD;
        private System.Windows.Forms.TextBox InfoBox0;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
        private System.Windows.Forms.Button CommitBtn;
        private System.Windows.Forms.Button AddToCheatGridBtn;
        private System.Windows.Forms.ComboBox PageBox;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel2;
        private System.Windows.Forms.Button NextBtn;
        private System.Windows.Forms.Button PreviousBtn;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel6;
        private System.Windows.Forms.RichTextBox AsmBox1;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel3;
        private System.Windows.Forms.Button RefreshBtn;
        private System.Windows.Forms.CheckBox AutoRefreshBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox InfoBox4S;
        private System.Windows.Forms.TextBox InfoBox3S;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox InfoBox2S;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox InfoBox2U;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox InfoBox1S;
        private System.Windows.Forms.Panel InfoBoxPanel;
        private System.Windows.Forms.Panel InfoBoxDSeparator;
        private System.Windows.Forms.Panel InfoBox4Separator;
        private System.Windows.Forms.Panel InfoBox0Panel;
        private System.Windows.Forms.CheckBox SwapBytesBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox InfoBoxB;
        private System.Windows.Forms.Panel InfoBox4SSeparator;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel7;
        private System.Windows.Forms.Button FindBtn;
        private System.Windows.Forms.CheckBox ForwardBox;
    }
}