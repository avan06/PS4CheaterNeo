
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.HexView = new Be.Windows.Forms.HexBox();
            this.HexBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.InfoBox = new System.Windows.Forms.TextBox();
            this.FindBtn = new System.Windows.Forms.Button();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.AddToCheatGridBtn = new System.Windows.Forms.Button();
            this.CommitBtn = new System.Windows.Forms.Button();
            this.PageBox = new System.Windows.Forms.ComboBox();
            this.RefreshBtn = new System.Windows.Forms.Button();
            this.NextBtn = new System.Windows.Forms.Button();
            this.PreviousBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.HexView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.HexBox);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Controls.Add(this.FindBtn);
            this.splitContainer1.Panel2.Controls.Add(this.InputBox);
            this.splitContainer1.Panel2.Controls.Add(this.AddToCheatGridBtn);
            this.splitContainer1.Panel2.Controls.Add(this.CommitBtn);
            this.splitContainer1.Panel2.Controls.Add(this.PageBox);
            this.splitContainer1.Panel2.Controls.Add(this.RefreshBtn);
            this.splitContainer1.Panel2.Controls.Add(this.NextBtn);
            this.splitContainer1.Panel2.Controls.Add(this.PreviousBtn);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 600;
            this.splitContainer1.TabIndex = 0;
            // 
            // HexView
            // 
            this.HexView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.HexView.ChangedFinishForeColor = System.Drawing.Color.LimeGreen;
            this.HexView.ColumnInfoVisible = true;
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
            // HexBox
            // 
            this.HexBox.AutoSize = true;
            this.HexBox.Checked = true;
            this.HexBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HexBox.Location = new System.Drawing.Point(3, 187);
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
            this.groupBox1.Controls.Add(this.InfoBox);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(3, 240);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(188, 205);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Memory Info";
            // 
            // InfoBox
            // 
            this.InfoBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InfoBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.InfoBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBox.ForeColor = System.Drawing.Color.White;
            this.InfoBox.Location = new System.Drawing.Point(3, 18);
            this.InfoBox.Multiline = true;
            this.InfoBox.Name = "InfoBox";
            this.InfoBox.Size = new System.Drawing.Size(182, 184);
            this.InfoBox.TabIndex = 0;
            // 
            // FindBtn
            // 
            this.FindBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FindBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FindBtn.ForeColor = System.Drawing.Color.White;
            this.FindBtn.Location = new System.Drawing.Point(3, 211);
            this.FindBtn.Name = "FindBtn";
            this.FindBtn.Size = new System.Drawing.Size(190, 23);
            this.FindBtn.TabIndex = 8;
            this.FindBtn.Text = "Find";
            this.FindBtn.UseVisualStyleBackColor = true;
            this.FindBtn.Click += new System.EventHandler(this.FindBtn_Click);
            // 
            // InputBox
            // 
            this.InputBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputBox.Location = new System.Drawing.Point(52, 181);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(141, 22);
            this.InputBox.TabIndex = 7;
            // 
            // AddToCheatGridBtn
            // 
            this.AddToCheatGridBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddToCheatGridBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddToCheatGridBtn.ForeColor = System.Drawing.Color.White;
            this.AddToCheatGridBtn.Location = new System.Drawing.Point(3, 150);
            this.AddToCheatGridBtn.Name = "AddToCheatGridBtn";
            this.AddToCheatGridBtn.Size = new System.Drawing.Size(190, 23);
            this.AddToCheatGridBtn.TabIndex = 6;
            this.AddToCheatGridBtn.Text = "Add To Cheat Grid";
            this.AddToCheatGridBtn.UseVisualStyleBackColor = true;
            this.AddToCheatGridBtn.Click += new System.EventHandler(this.AddToCheatGridBtn_Click);
            // 
            // CommitBtn
            // 
            this.CommitBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CommitBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CommitBtn.ForeColor = System.Drawing.Color.White;
            this.CommitBtn.Location = new System.Drawing.Point(3, 120);
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
            this.PageBox.Location = new System.Drawing.Point(3, 63);
            this.PageBox.Name = "PageBox";
            this.PageBox.Size = new System.Drawing.Size(190, 20);
            this.PageBox.TabIndex = 4;
            this.PageBox.SelectedIndexChanged += new System.EventHandler(this.PageBox_SelectedIndexChanged);
            // 
            // RefreshBtn
            // 
            this.RefreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RefreshBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RefreshBtn.ForeColor = System.Drawing.Color.White;
            this.RefreshBtn.Location = new System.Drawing.Point(3, 90);
            this.RefreshBtn.Name = "RefreshBtn";
            this.RefreshBtn.Size = new System.Drawing.Size(190, 23);
            this.RefreshBtn.TabIndex = 3;
            this.RefreshBtn.Text = "Refresh";
            this.RefreshBtn.UseVisualStyleBackColor = true;
            this.RefreshBtn.Click += new System.EventHandler(this.RefreshBtn_Click);
            // 
            // NextBtn
            // 
            this.NextBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NextBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NextBtn.ForeColor = System.Drawing.Color.White;
            this.NextBtn.Location = new System.Drawing.Point(3, 33);
            this.NextBtn.Name = "NextBtn";
            this.NextBtn.Size = new System.Drawing.Size(190, 23);
            this.NextBtn.TabIndex = 1;
            this.NextBtn.Text = "Next";
            this.NextBtn.UseVisualStyleBackColor = true;
            this.NextBtn.Click += new System.EventHandler(this.NextBtn_Click);
            // 
            // PreviousBtn
            // 
            this.PreviousBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PreviousBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PreviousBtn.ForeColor = System.Drawing.Color.White;
            this.PreviousBtn.Location = new System.Drawing.Point(3, 3);
            this.PreviousBtn.Name = "PreviousBtn";
            this.PreviousBtn.Size = new System.Drawing.Size(190, 23);
            this.PreviousBtn.TabIndex = 0;
            this.PreviousBtn.Text = "Previous";
            this.PreviousBtn.UseVisualStyleBackColor = true;
            this.PreviousBtn.Click += new System.EventHandler(this.PreviousBtn_Click);
            // 
            // HexEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "HexEditor";
            this.Opacity = 0.95D;
            this.Text = "HexEditor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HexEditor_FormClosing);
            this.Load += new System.EventHandler(this.HexEditor_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
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
        private System.Windows.Forms.TextBox InfoBox;
        private System.Windows.Forms.CheckBox HexBox;
    }
}