
namespace PS4CheaterNeo
{
    partial class PointerFinder
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.PointerListView = new System.Windows.Forms.ListView();
            this.PointerListViewMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.PointerListViewAddToCheatGrid = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolStripMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.FilterSizeRuleBtn = new System.Windows.Forms.Button();
            this.IsFilterSizeBox = new System.Windows.Forms.CheckBox();
            this.NegativeOffsetBox = new System.Windows.Forms.CheckBox();
            this.ScanTypeBox = new System.Windows.Forms.ComboBox();
            this.NewBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.MaxRangeUpDown = new System.Windows.Forms.NumericUpDown();
            this.LevelUpdown = new System.Windows.Forms.NumericUpDown();
            this.ProgBar = new System.Windows.Forms.ProgressBar();
            this.LoadBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.FilterRuleBtn = new System.Windows.Forms.Button();
            this.IsFilterBox = new System.Windows.Forms.CheckBox();
            this.FastScanBox = new System.Windows.Forms.CheckBox();
            this.IsInitScan = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ScanBtn = new System.Windows.Forms.Button();
            this.AddressBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SaveDialog = new System.Windows.Forms.SaveFileDialog();
            this.OpenDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.PointerListViewMenu.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxRangeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LevelUpdown)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.PointerListView);
            this.splitContainer1.Panel1.Controls.Add(this.statusStrip1);
            this.splitContainer1.Panel1.ForeColor = System.Drawing.Color.White;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.FilterSizeRuleBtn);
            this.splitContainer1.Panel2.Controls.Add(this.IsFilterSizeBox);
            this.splitContainer1.Panel2.Controls.Add(this.NegativeOffsetBox);
            this.splitContainer1.Panel2.Controls.Add(this.ScanTypeBox);
            this.splitContainer1.Panel2.Controls.Add(this.NewBtn);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.MaxRangeUpDown);
            this.splitContainer1.Panel2.Controls.Add(this.LevelUpdown);
            this.splitContainer1.Panel2.Controls.Add(this.ProgBar);
            this.splitContainer1.Panel2.Controls.Add(this.LoadBtn);
            this.splitContainer1.Panel2.Controls.Add(this.SaveBtn);
            this.splitContainer1.Panel2.Controls.Add(this.FilterRuleBtn);
            this.splitContainer1.Panel2.Controls.Add(this.IsFilterBox);
            this.splitContainer1.Panel2.Controls.Add(this.FastScanBox);
            this.splitContainer1.Panel2.Controls.Add(this.IsInitScan);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.ScanBtn);
            this.splitContainer1.Panel2.Controls.Add(this.AddressBox);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 672;
            this.splitContainer1.TabIndex = 0;
            // 
            // PointerListView
            // 
            this.PointerListView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.PointerListView.ContextMenuStrip = this.PointerListViewMenu;
            this.PointerListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PointerListView.ForeColor = System.Drawing.Color.White;
            this.PointerListView.FullRowSelect = true;
            this.PointerListView.HideSelection = false;
            this.PointerListView.Location = new System.Drawing.Point(0, 0);
            this.PointerListView.Name = "PointerListView";
            this.PointerListView.Size = new System.Drawing.Size(670, 426);
            this.PointerListView.TabIndex = 3;
            this.PointerListView.UseCompatibleStateImageBehavior = false;
            this.PointerListView.View = System.Windows.Forms.View.Details;
            this.PointerListView.DoubleClick += new System.EventHandler(this.PointerListView_DoubleClick);
            // 
            // PointerListViewMenu
            // 
            this.PointerListViewMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PointerListViewAddToCheatGrid});
            this.PointerListViewMenu.Name = "contextMenuStrip1";
            this.PointerListViewMenu.Size = new System.Drawing.Size(177, 26);
            // 
            // PointerListViewAddToCheatGrid
            // 
            this.PointerListViewAddToCheatGrid.Name = "PointerListViewAddToCheatGrid";
            this.PointerListViewAddToCheatGrid.Size = new System.Drawing.Size(176, 22);
            this.PointerListViewAddToCheatGrid.Text = "Add to Cheat Grid";
            this.PointerListViewAddToCheatGrid.Click += new System.EventHandler(this.PointerListViewAddToCheatGrid_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.DimGray;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMsg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 426);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(670, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ToolStripMsg
            // 
            this.ToolStripMsg.Name = "ToolStripMsg";
            this.ToolStripMsg.Size = new System.Drawing.Size(655, 17);
            this.ToolStripMsg.Spring = true;
            this.ToolStripMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FilterSizeRuleBtn
            // 
            this.FilterSizeRuleBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.FilterSizeRuleBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FilterSizeRuleBtn.Location = new System.Drawing.Point(78, 267);
            this.FilterSizeRuleBtn.Name = "FilterSizeRuleBtn";
            this.FilterSizeRuleBtn.Size = new System.Drawing.Size(42, 23);
            this.FilterSizeRuleBtn.TabIndex = 20;
            this.FilterSizeRuleBtn.Text = "Rule";
            this.FilterSizeRuleBtn.UseVisualStyleBackColor = false;
            this.FilterSizeRuleBtn.Click += new System.EventHandler(this.FilterSizeRuleBtn_Click);
            // 
            // IsFilterSizeBox
            // 
            this.IsFilterSizeBox.AutoSize = true;
            this.IsFilterSizeBox.Checked = true;
            this.IsFilterSizeBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsFilterSizeBox.Location = new System.Drawing.Point(5, 271);
            this.IsFilterSizeBox.Name = "IsFilterSizeBox";
            this.IsFilterSizeBox.Size = new System.Drawing.Size(67, 16);
            this.IsFilterSizeBox.TabIndex = 19;
            this.IsFilterSizeBox.Text = "FilterSize";
            this.IsFilterSizeBox.UseVisualStyleBackColor = true;
            // 
            // NegativeOffsetBox
            // 
            this.NegativeOffsetBox.AutoSize = true;
            this.NegativeOffsetBox.Location = new System.Drawing.Point(5, 225);
            this.NegativeOffsetBox.Name = "NegativeOffsetBox";
            this.NegativeOffsetBox.Size = new System.Drawing.Size(93, 16);
            this.NegativeOffsetBox.TabIndex = 18;
            this.NegativeOffsetBox.Text = "NegativeOffset";
            this.NegativeOffsetBox.UseVisualStyleBackColor = true;
            // 
            // ScanTypeBox
            // 
            this.ScanTypeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScanTypeBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ScanTypeBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ScanTypeBox.ForeColor = System.Drawing.Color.White;
            this.ScanTypeBox.FormattingEnabled = true;
            this.ScanTypeBox.Location = new System.Drawing.Point(0, 84);
            this.ScanTypeBox.Name = "ScanTypeBox";
            this.ScanTypeBox.Size = new System.Drawing.Size(120, 20);
            this.ScanTypeBox.TabIndex = 17;
            // 
            // NewBtn
            // 
            this.NewBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NewBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.NewBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NewBtn.Location = new System.Drawing.Point(78, 110);
            this.NewBtn.Name = "NewBtn";
            this.NewBtn.Size = new System.Drawing.Size(42, 23);
            this.NewBtn.TabIndex = 16;
            this.NewBtn.Text = "New";
            this.NewBtn.UseVisualStyleBackColor = false;
            this.NewBtn.Click += new System.EventHandler(this.NewBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Level:";
            // 
            // MaxRangeUpDown
            // 
            this.MaxRangeUpDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.MaxRangeUpDown.ForeColor = System.Drawing.Color.White;
            this.MaxRangeUpDown.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.MaxRangeUpDown.Location = new System.Drawing.Point(0, 151);
            this.MaxRangeUpDown.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.MaxRangeUpDown.Minimum = new decimal(new int[] {
            500000,
            0,
            0,
            -2147483648});
            this.MaxRangeUpDown.Name = "MaxRangeUpDown";
            this.MaxRangeUpDown.Size = new System.Drawing.Size(120, 22);
            this.MaxRangeUpDown.TabIndex = 15;
            this.MaxRangeUpDown.Value = new decimal(new int[] {
            8192,
            0,
            0,
            0});
            // 
            // LevelUpdown
            // 
            this.LevelUpdown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.LevelUpdown.ForeColor = System.Drawing.Color.White;
            this.LevelUpdown.Location = new System.Drawing.Point(0, 15);
            this.LevelUpdown.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.LevelUpdown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.LevelUpdown.Name = "LevelUpdown";
            this.LevelUpdown.Size = new System.Drawing.Size(120, 22);
            this.LevelUpdown.TabIndex = 14;
            this.LevelUpdown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // ProgBar
            // 
            this.ProgBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgBar.Location = new System.Drawing.Point(0, 435);
            this.ProgBar.Name = "ProgBar";
            this.ProgBar.Size = new System.Drawing.Size(120, 10);
            this.ProgBar.TabIndex = 13;
            // 
            // LoadBtn
            // 
            this.LoadBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.LoadBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoadBtn.Location = new System.Drawing.Point(0, 337);
            this.LoadBtn.Name = "LoadBtn";
            this.LoadBtn.Size = new System.Drawing.Size(120, 23);
            this.LoadBtn.TabIndex = 12;
            this.LoadBtn.Text = "Load";
            this.LoadBtn.UseVisualStyleBackColor = false;
            this.LoadBtn.Click += new System.EventHandler(this.LoadBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.SaveBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveBtn.Location = new System.Drawing.Point(0, 308);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(120, 23);
            this.SaveBtn.TabIndex = 11;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.UseVisualStyleBackColor = false;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // FilterRuleBtn
            // 
            this.FilterRuleBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.FilterRuleBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FilterRuleBtn.Location = new System.Drawing.Point(78, 242);
            this.FilterRuleBtn.Name = "FilterRuleBtn";
            this.FilterRuleBtn.Size = new System.Drawing.Size(42, 23);
            this.FilterRuleBtn.TabIndex = 10;
            this.FilterRuleBtn.Text = "Rule";
            this.FilterRuleBtn.UseVisualStyleBackColor = false;
            this.FilterRuleBtn.Click += new System.EventHandler(this.FilterRuleBtn_Click);
            // 
            // IsFilterBox
            // 
            this.IsFilterBox.AutoSize = true;
            this.IsFilterBox.Checked = true;
            this.IsFilterBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsFilterBox.Location = new System.Drawing.Point(5, 249);
            this.IsFilterBox.Name = "IsFilterBox";
            this.IsFilterBox.Size = new System.Drawing.Size(48, 16);
            this.IsFilterBox.TabIndex = 9;
            this.IsFilterBox.Text = "Filter";
            this.IsFilterBox.UseVisualStyleBackColor = true;
            // 
            // FastScanBox
            // 
            this.FastScanBox.AutoSize = true;
            this.FastScanBox.Checked = true;
            this.FastScanBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FastScanBox.Location = new System.Drawing.Point(5, 203);
            this.FastScanBox.Name = "FastScanBox";
            this.FastScanBox.Size = new System.Drawing.Size(64, 16);
            this.FastScanBox.TabIndex = 7;
            this.FastScanBox.Text = "FastScan";
            this.FastScanBox.UseVisualStyleBackColor = true;
            // 
            // IsInitScan
            // 
            this.IsInitScan.AutoSize = true;
            this.IsInitScan.Checked = true;
            this.IsInitScan.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsInitScan.Location = new System.Drawing.Point(5, 180);
            this.IsInitScan.Name = "IsInitScan";
            this.IsInitScan.Size = new System.Drawing.Size(62, 16);
            this.IsInitScan.TabIndex = 6;
            this.IsInitScan.Text = "InitScan";
            this.IsInitScan.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "MaxRange:";
            // 
            // ScanBtn
            // 
            this.ScanBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScanBtn.BackColor = System.Drawing.Color.SteelBlue;
            this.ScanBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ScanBtn.Location = new System.Drawing.Point(0, 110);
            this.ScanBtn.Name = "ScanBtn";
            this.ScanBtn.Size = new System.Drawing.Size(72, 23);
            this.ScanBtn.TabIndex = 3;
            this.ScanBtn.Text = "Scan";
            this.ScanBtn.UseVisualStyleBackColor = false;
            this.ScanBtn.Click += new System.EventHandler(this.ScanBtn_Click);
            // 
            // AddressBox
            // 
            this.AddressBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddressBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.AddressBox.ForeColor = System.Drawing.Color.White;
            this.AddressBox.Location = new System.Drawing.Point(0, 56);
            this.AddressBox.Name = "AddressBox";
            this.AddressBox.Size = new System.Drawing.Size(120, 22);
            this.AddressBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Address:";
            // 
            // PointerFinder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "PointerFinder";
            this.Text = "PointerFinder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PointerFinder_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.PointerListViewMenu.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxRangeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LevelUpdown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox AddressBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ScanBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox FastScanBox;
        private System.Windows.Forms.CheckBox IsInitScan;
        private System.Windows.Forms.CheckBox IsFilterBox;
        private System.Windows.Forms.Button FilterRuleBtn;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Button LoadBtn;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ProgressBar ProgBar;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripMsg;
        private System.Windows.Forms.NumericUpDown LevelUpdown;
        private System.Windows.Forms.NumericUpDown MaxRangeUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SaveFileDialog SaveDialog;
        private System.Windows.Forms.OpenFileDialog OpenDialog;
        private System.Windows.Forms.ListView PointerListView;
        private System.Windows.Forms.Button NewBtn;
        private System.Windows.Forms.ComboBox ScanTypeBox;
        private System.Windows.Forms.ContextMenuStrip PointerListViewMenu;
        private System.Windows.Forms.ToolStripMenuItem PointerListViewAddToCheatGrid;
        private System.Windows.Forms.CheckBox NegativeOffsetBox;
        private System.Windows.Forms.Button FilterSizeRuleBtn;
        private System.Windows.Forms.CheckBox IsFilterSizeBox;
    }
}