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
            this.SplitContainer1 = new PS4CheaterNeo.CollapsibleSplitContainer();
            this.HexView = new Be.Windows.Forms.HexBox();
            this.HexViewMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.HexViewByteGroup = new System.Windows.Forms.ToolStripComboBox();
            this.HexViewGroupSize = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.SplitContainer2 = new PS4CheaterNeo.CollapsibleSplitContainer();
            this.NextBtn = new System.Windows.Forms.Button();
            this.CommitBtn = new System.Windows.Forms.Button();
            this.PageBox = new System.Windows.Forms.ComboBox();
            this.RefreshBtn = new System.Windows.Forms.Button();
            this.PreviousBtn = new System.Windows.Forms.Button();
            this.AutoRefreshBox = new System.Windows.Forms.CheckBox();
            this.AddToCheatGridBtn = new System.Windows.Forms.Button();
            this.AssemblerBtn = new System.Windows.Forms.Button();
            this.HexBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.InfoBox = new System.Windows.Forms.RichTextBox();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.FindBtn = new System.Windows.Forms.Button();
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
            this.groupBox1.SuspendLayout();
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
            this.SplitContainer1.SplitterButtonLocation = PS4CheaterNeo.ButtonLocation.Panel2;
            this.SplitContainer1.SplitterButtonSize = 13;
            this.SplitContainer1.SplitterButtonStyle = PS4CheaterNeo.ButtonStyle.SingleImage;
            this.SplitContainer1.SplitterDistance = 600;
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
            this.HexView.LineInfoOffsetLength = 10;
            this.HexView.LineInfoVisible = true;
            this.HexView.Location = new System.Drawing.Point(0, 0);
            this.HexView.Name = "HexView";
            this.HexView.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.HexView.Size = new System.Drawing.Size(598, 448);
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
            this.HexViewByteGroup,
            this.HexViewGroupSize,
            this.toolStripSeparator1});
            this.HexViewMenu.Name = "HexViewMenu";
            this.HexViewMenu.Size = new System.Drawing.Size(211, 64);
            // 
            // HexViewByteGroup
            // 
            this.HexViewByteGroup.Name = "HexViewByteGroup";
            this.HexViewByteGroup.Size = new System.Drawing.Size(150, 23);
            this.HexViewByteGroup.Text = "HexViewByteGroup";
            this.HexViewByteGroup.SelectedIndexChanged += new System.EventHandler(this.HexViewByteGroup_SelectedIndexChanged);
            // 
            // HexViewGroupSize
            // 
            this.HexViewGroupSize.Name = "HexViewGroupSize";
            this.HexViewGroupSize.Size = new System.Drawing.Size(150, 23);
            this.HexViewGroupSize.Text = "HexViewGroupSize";
            this.HexViewGroupSize.SelectedIndexChanged += new System.EventHandler(this.HexViewGroupSize_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(207, 6);
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
            this.SplitContainer2.Panel1.Controls.Add(this.NextBtn);
            this.SplitContainer2.Panel1.Controls.Add(this.CommitBtn);
            this.SplitContainer2.Panel1.Controls.Add(this.PageBox);
            this.SplitContainer2.Panel1.Controls.Add(this.RefreshBtn);
            this.SplitContainer2.Panel1.Controls.Add(this.PreviousBtn);
            this.SplitContainer2.Panel1.Controls.Add(this.AutoRefreshBox);
            this.SplitContainer2.Panel1.Controls.Add(this.AddToCheatGridBtn);
            // 
            // SplitContainer2.Panel2
            // 
            this.SplitContainer2.Panel2.Controls.Add(this.AssemblerBtn);
            this.SplitContainer2.Panel2.Controls.Add(this.HexBox);
            this.SplitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.SplitContainer2.Panel2.Controls.Add(this.InputBox);
            this.SplitContainer2.Panel2.Controls.Add(this.FindBtn);
            this.SplitContainer2.SingleImageCollapsePanel2 = false;
            this.SplitContainer2.Size = new System.Drawing.Size(196, 450);
            this.SplitContainer2.SplitterButtonLocation = PS4CheaterNeo.ButtonLocation.Panel2;
            this.SplitContainer2.SplitterButtonPosition = PS4CheaterNeo.ButtonPosition.BottomRight;
            this.SplitContainer2.SplitterButtonSize = 13;
            this.SplitContainer2.SplitterButtonStyle = PS4CheaterNeo.ButtonStyle.SingleImage;
            this.SplitContainer2.SplitterDistance = 150;
            this.SplitContainer2.TabIndex = 1;
            // 
            // NextBtn
            // 
            this.NextBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NextBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NextBtn.ForeColor = System.Drawing.Color.White;
            this.NextBtn.Location = new System.Drawing.Point(98, 6);
            this.NextBtn.Name = "NextBtn";
            this.NextBtn.Size = new System.Drawing.Size(94, 23);
            this.NextBtn.TabIndex = 1;
            this.NextBtn.Text = "Next";
            this.NextBtn.UseVisualStyleBackColor = true;
            this.NextBtn.Click += new System.EventHandler(this.NextBtn_Click);
            // 
            // CommitBtn
            // 
            this.CommitBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CommitBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CommitBtn.ForeColor = System.Drawing.Color.White;
            this.CommitBtn.Location = new System.Drawing.Point(2, 93);
            this.CommitBtn.Name = "CommitBtn";
            this.CommitBtn.Size = new System.Drawing.Size(190, 23);
            this.CommitBtn.TabIndex = 5;
            this.CommitBtn.Text = "Commit";
            this.CommitBtn.UseVisualStyleBackColor = true;
            this.CommitBtn.Click += new System.EventHandler(this.CommitBtn_Click);
            // 
            // PageBox
            // 
            this.PageBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PageBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.PageBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PageBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PageBox.ForeColor = System.Drawing.Color.White;
            this.PageBox.FormattingEnabled = true;
            this.PageBox.Location = new System.Drawing.Point(2, 36);
            this.PageBox.Name = "PageBox";
            this.PageBox.Size = new System.Drawing.Size(194, 20);
            this.PageBox.TabIndex = 4;
            this.PageBox.SelectedIndexChanged += new System.EventHandler(this.PageBox_SelectedIndexChanged);
            // 
            // RefreshBtn
            // 
            this.RefreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RefreshBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RefreshBtn.ForeColor = System.Drawing.Color.White;
            this.RefreshBtn.Location = new System.Drawing.Point(47, 63);
            this.RefreshBtn.Name = "RefreshBtn";
            this.RefreshBtn.Size = new System.Drawing.Size(145, 23);
            this.RefreshBtn.TabIndex = 3;
            this.RefreshBtn.Text = "Refresh";
            this.RefreshBtn.UseVisualStyleBackColor = true;
            this.RefreshBtn.Click += new System.EventHandler(this.RefreshBtn_Click);
            // 
            // PreviousBtn
            // 
            this.PreviousBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PreviousBtn.ForeColor = System.Drawing.Color.White;
            this.PreviousBtn.Location = new System.Drawing.Point(2, 6);
            this.PreviousBtn.Name = "PreviousBtn";
            this.PreviousBtn.Size = new System.Drawing.Size(94, 23);
            this.PreviousBtn.TabIndex = 0;
            this.PreviousBtn.Text = "Previous";
            this.PreviousBtn.UseVisualStyleBackColor = true;
            this.PreviousBtn.Click += new System.EventHandler(this.PreviousBtn_Click);
            // 
            // AutoRefreshBox
            // 
            this.AutoRefreshBox.AutoSize = true;
            this.AutoRefreshBox.Location = new System.Drawing.Point(2, 67);
            this.AutoRefreshBox.Name = "AutoRefreshBox";
            this.AutoRefreshBox.Size = new System.Drawing.Size(47, 16);
            this.AutoRefreshBox.TabIndex = 12;
            this.AutoRefreshBox.Text = "Auto";
            this.AutoRefreshBox.UseVisualStyleBackColor = true;
            // 
            // AddToCheatGridBtn
            // 
            this.AddToCheatGridBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddToCheatGridBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddToCheatGridBtn.ForeColor = System.Drawing.Color.White;
            this.AddToCheatGridBtn.Location = new System.Drawing.Point(2, 123);
            this.AddToCheatGridBtn.Name = "AddToCheatGridBtn";
            this.AddToCheatGridBtn.Size = new System.Drawing.Size(190, 23);
            this.AddToCheatGridBtn.TabIndex = 6;
            this.AddToCheatGridBtn.Text = "Add To Cheat Grid";
            this.AddToCheatGridBtn.UseVisualStyleBackColor = true;
            this.AddToCheatGridBtn.Click += new System.EventHandler(this.AddToCheatGridBtn_Click);
            // 
            // AssemblerBtn
            // 
            this.AssemblerBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AssemblerBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AssemblerBtn.ForeColor = System.Drawing.Color.White;
            this.AssemblerBtn.Location = new System.Drawing.Point(2, 65);
            this.AssemblerBtn.Name = "AssemblerBtn";
            this.AssemblerBtn.Size = new System.Drawing.Size(190, 23);
            this.AssemblerBtn.TabIndex = 11;
            this.AssemblerBtn.Text = "AssemblerBox";
            this.AssemblerBtn.UseVisualStyleBackColor = true;
            this.AssemblerBtn.Click += new System.EventHandler(this.AssemblerBtn_Click);
            // 
            // HexBox
            // 
            this.HexBox.AutoSize = true;
            this.HexBox.Checked = true;
            this.HexBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HexBox.Location = new System.Drawing.Point(1, 9);
            this.HexBox.Name = "HexBox";
            this.HexBox.Size = new System.Drawing.Size(43, 16);
            this.HexBox.TabIndex = 10;
            this.HexBox.Text = "Hex";
            this.HexBox.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.InfoBox);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(1, 92);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(191, 199);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Memory Info";
            // 
            // InfoBox
            // 
            this.InfoBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InfoBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoBox.ForeColor = System.Drawing.Color.White;
            this.InfoBox.Location = new System.Drawing.Point(3, 18);
            this.InfoBox.Name = "InfoBox";
            this.InfoBox.Size = new System.Drawing.Size(185, 178);
            this.InfoBox.TabIndex = 1;
            this.InfoBox.Text = "";
            // 
            // InputBox
            // 
            this.InputBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputBox.Location = new System.Drawing.Point(50, 3);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(141, 22);
            this.InputBox.TabIndex = 7;
            // 
            // FindBtn
            // 
            this.FindBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FindBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FindBtn.ForeColor = System.Drawing.Color.White;
            this.FindBtn.Location = new System.Drawing.Point(2, 33);
            this.FindBtn.Name = "FindBtn";
            this.FindBtn.Size = new System.Drawing.Size(190, 23);
            this.FindBtn.TabIndex = 8;
            this.FindBtn.Text = "Find";
            this.FindBtn.UseVisualStyleBackColor = true;
            this.FindBtn.Click += new System.EventHandler(this.FindBtn_Click);
            // 
            // AutoRefreshTimer
            // 
            this.AutoRefreshTimer.Enabled = true;
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
            this.Opacity = 0.95D;
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
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CollapsibleSplitContainer SplitContainer1;
        private Be.Windows.Forms.HexBox HexView;
        private System.Windows.Forms.Button PreviousBtn;
        private System.Windows.Forms.Button RefreshBtn;
        private System.Windows.Forms.Button NextBtn;
        private System.Windows.Forms.ComboBox PageBox;
        private System.Windows.Forms.Button AddToCheatGridBtn;
        private System.Windows.Forms.Button CommitBtn;
        private System.Windows.Forms.Button FindBtn;
        private System.Windows.Forms.TextBox InputBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox HexBox;
        private System.Windows.Forms.RichTextBox InfoBox;
        private System.Windows.Forms.Button AssemblerBtn;
        private System.Windows.Forms.Timer AutoRefreshTimer;
        private System.Windows.Forms.CheckBox AutoRefreshBox;
        private System.Windows.Forms.ContextMenuStrip HexViewMenu;
        private System.Windows.Forms.ToolStripComboBox HexViewGroupSize;
        private System.Windows.Forms.ToolStripComboBox HexViewByteGroup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private CollapsibleSplitContainer SplitContainer2;
    }
}