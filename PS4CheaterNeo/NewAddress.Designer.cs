
namespace PS4CheaterNeo
{
    partial class NewAddress
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
            this.AddressBox = new System.Windows.Forms.TextBox();
            this.AddressLabel = new System.Windows.Forms.Label();
            this.ValueLabel = new System.Windows.Forms.Label();
            this.ValueBox = new System.Windows.Forms.TextBox();
            this.TypeLabel = new System.Windows.Forms.Label();
            this.ScanTypeBox = new System.Windows.Forms.ComboBox();
            this.LockBox = new System.Windows.Forms.CheckBox();
            this.DescriptionLabel = new System.Windows.Forms.Label();
            this.DescriptionBox = new System.Windows.Forms.TextBox();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.CloseBtn = new System.Windows.Forms.Button();
            this.PointerBox = new System.Windows.Forms.CheckBox();
            this.RefreshPointerChecker = new System.Windows.Forms.Timer(this.components);
            this.TableLayoutBase = new System.Windows.Forms.TableLayoutPanel();
            this.TableLayoutBottom = new System.Windows.Forms.TableLayoutPanel();
            this.TableLayoutBottomLabel = new System.Windows.Forms.TableLayoutPanel();
            this.TableLayoutBottomBox = new System.Windows.Forms.TableLayoutPanel();
            this.OnOffBox = new System.Windows.Forms.CheckBox();
            this.TableLayoutBase.SuspendLayout();
            this.TableLayoutBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // AddressBox
            // 
            this.AddressBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.AddressBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddressBox.ForeColor = System.Drawing.Color.White;
            this.AddressBox.Location = new System.Drawing.Point(65, 3);
            this.AddressBox.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.AddressBox.Name = "AddressBox";
            this.AddressBox.Size = new System.Drawing.Size(114, 22);
            this.AddressBox.TabIndex = 0;
            this.AddressBox.Leave += new System.EventHandler(this.AddressBox_Leave);
            // 
            // AddressLabel
            // 
            this.AddressLabel.AutoSize = true;
            this.AddressLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddressLabel.ForeColor = System.Drawing.Color.White;
            this.AddressLabel.Location = new System.Drawing.Point(8, 3);
            this.AddressLabel.Margin = new System.Windows.Forms.Padding(3);
            this.AddressLabel.Name = "AddressLabel";
            this.AddressLabel.Size = new System.Drawing.Size(54, 19);
            this.AddressLabel.TabIndex = 1;
            this.AddressLabel.Text = "Address";
            this.AddressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ValueLabel
            // 
            this.ValueLabel.AutoSize = true;
            this.ValueLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ValueLabel.ForeColor = System.Drawing.Color.White;
            this.ValueLabel.Location = new System.Drawing.Point(182, 3);
            this.ValueLabel.Margin = new System.Windows.Forms.Padding(3);
            this.ValueLabel.Name = "ValueLabel";
            this.ValueLabel.Size = new System.Drawing.Size(44, 19);
            this.ValueLabel.TabIndex = 3;
            this.ValueLabel.Text = "Value";
            this.ValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ValueBox
            // 
            this.ValueBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ValueBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ValueBox.ForeColor = System.Drawing.Color.White;
            this.ValueBox.Location = new System.Drawing.Point(229, 3);
            this.ValueBox.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.ValueBox.Name = "ValueBox";
            this.ValueBox.Size = new System.Drawing.Size(115, 22);
            this.ValueBox.TabIndex = 2;
            this.ValueBox.Text = "0";
            // 
            // TypeLabel
            // 
            this.TypeLabel.AutoSize = true;
            this.TypeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TypeLabel.ForeColor = System.Drawing.Color.White;
            this.TypeLabel.Location = new System.Drawing.Point(8, 28);
            this.TypeLabel.Margin = new System.Windows.Forms.Padding(3);
            this.TypeLabel.Name = "TypeLabel";
            this.TypeLabel.Size = new System.Drawing.Size(54, 17);
            this.TypeLabel.TabIndex = 4;
            this.TypeLabel.Text = "Type";
            this.TypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ScanTypeBox
            // 
            this.ScanTypeBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ScanTypeBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScanTypeBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ScanTypeBox.ForeColor = System.Drawing.Color.White;
            this.ScanTypeBox.FormattingEnabled = true;
            this.ScanTypeBox.Location = new System.Drawing.Point(65, 28);
            this.ScanTypeBox.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.ScanTypeBox.Name = "ScanTypeBox";
            this.ScanTypeBox.Size = new System.Drawing.Size(114, 20);
            this.ScanTypeBox.TabIndex = 5;
            this.ScanTypeBox.SelectedIndexChanged += new System.EventHandler(this.ScanTypeBox_SelectedIndexChanged);
            // 
            // LockBox
            // 
            this.LockBox.AutoSize = true;
            this.LockBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.LockBox.ForeColor = System.Drawing.Color.White;
            this.LockBox.Location = new System.Drawing.Point(229, 28);
            this.LockBox.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.LockBox.Name = "LockBox";
            this.LockBox.Size = new System.Drawing.Size(48, 20);
            this.LockBox.TabIndex = 6;
            this.LockBox.Text = "Lock";
            this.LockBox.UseVisualStyleBackColor = true;
            // 
            // DescriptionLabel
            // 
            this.DescriptionLabel.AutoSize = true;
            this.DescriptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DescriptionLabel.ForeColor = System.Drawing.Color.White;
            this.DescriptionLabel.Location = new System.Drawing.Point(5, 51);
            this.DescriptionLabel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(60, 19);
            this.DescriptionLabel.TabIndex = 7;
            this.DescriptionLabel.Text = "Description";
            this.DescriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DescriptionBox
            // 
            this.DescriptionBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.TableLayoutBase.SetColumnSpan(this.DescriptionBox, 3);
            this.DescriptionBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DescriptionBox.ForeColor = System.Drawing.Color.White;
            this.DescriptionBox.Location = new System.Drawing.Point(65, 51);
            this.DescriptionBox.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.DescriptionBox.Name = "DescriptionBox";
            this.DescriptionBox.Size = new System.Drawing.Size(279, 22);
            this.DescriptionBox.TabIndex = 8;
            // 
            // SaveBtn
            // 
            this.SaveBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.SaveBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.SaveBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveBtn.ForeColor = System.Drawing.Color.White;
            this.SaveBtn.Location = new System.Drawing.Point(3, 4);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(133, 23);
            this.SaveBtn.TabIndex = 13;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.UseVisualStyleBackColor = false;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // CloseBtn
            // 
            this.CloseBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.CloseBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.CloseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseBtn.ForeColor = System.Drawing.Color.White;
            this.CloseBtn.Location = new System.Drawing.Point(142, 4);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(134, 23);
            this.CloseBtn.TabIndex = 14;
            this.CloseBtn.Text = "Close";
            this.CloseBtn.UseVisualStyleBackColor = false;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // PointerBox
            // 
            this.PointerBox.AutoSize = true;
            this.PointerBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PointerBox.ForeColor = System.Drawing.Color.White;
            this.PointerBox.Location = new System.Drawing.Point(68, 76);
            this.PointerBox.Name = "PointerBox";
            this.PointerBox.Size = new System.Drawing.Size(108, 16);
            this.PointerBox.TabIndex = 15;
            this.PointerBox.Text = "Pointer";
            this.PointerBox.UseVisualStyleBackColor = true;
            this.PointerBox.CheckedChanged += new System.EventHandler(this.PointerBox_CheckedChanged);
            // 
            // RefreshPointerChecker
            // 
            this.RefreshPointerChecker.Enabled = true;
            this.RefreshPointerChecker.Interval = 1000;
            this.RefreshPointerChecker.Tick += new System.EventHandler(this.RefreshPointerChecker_Tick);
            // 
            // TableLayoutBase
            // 
            this.TableLayoutBase.ColumnCount = 4;
            this.TableLayoutBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.TableLayoutBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.TableLayoutBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutBase.Controls.Add(this.AddressLabel, 0, 0);
            this.TableLayoutBase.Controls.Add(this.PointerBox, 1, 4);
            this.TableLayoutBase.Controls.Add(this.AddressBox, 1, 0);
            this.TableLayoutBase.Controls.Add(this.ValueLabel, 2, 0);
            this.TableLayoutBase.Controls.Add(this.ValueBox, 3, 0);
            this.TableLayoutBase.Controls.Add(this.DescriptionBox, 1, 2);
            this.TableLayoutBase.Controls.Add(this.ScanTypeBox, 1, 1);
            this.TableLayoutBase.Controls.Add(this.DescriptionLabel, 0, 2);
            this.TableLayoutBase.Controls.Add(this.LockBox, 3, 1);
            this.TableLayoutBase.Controls.Add(this.TypeLabel, 0, 1);
            this.TableLayoutBase.Controls.Add(this.TableLayoutBottom, 1, 5);
            this.TableLayoutBase.Controls.Add(this.OnOffBox, 3, 4);
            this.TableLayoutBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutBase.Location = new System.Drawing.Point(0, 0);
            this.TableLayoutBase.Margin = new System.Windows.Forms.Padding(0);
            this.TableLayoutBase.Name = "TableLayoutBase";
            this.TableLayoutBase.Padding = new System.Windows.Forms.Padding(5, 0, 20, 0);
            this.TableLayoutBase.RowCount = 6;
            this.TableLayoutBase.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutBase.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutBase.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutBase.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutBase.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutBase.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutBase.Size = new System.Drawing.Size(364, 134);
            this.TableLayoutBase.TabIndex = 16;
            // 
            // TableLayoutBottom
            // 
            this.TableLayoutBottom.ColumnCount = 2;
            this.TableLayoutBase.SetColumnSpan(this.TableLayoutBottom, 3);
            this.TableLayoutBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutBottom.Controls.Add(this.TableLayoutBottomLabel, 1, 1);
            this.TableLayoutBottom.Controls.Add(this.SaveBtn, 0, 2);
            this.TableLayoutBottom.Controls.Add(this.CloseBtn, 1, 2);
            this.TableLayoutBottom.Controls.Add(this.TableLayoutBottomBox, 0, 1);
            this.TableLayoutBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutBottom.Location = new System.Drawing.Point(65, 95);
            this.TableLayoutBottom.Margin = new System.Windows.Forms.Padding(0);
            this.TableLayoutBottom.Name = "TableLayoutBottom";
            this.TableLayoutBottom.RowCount = 3;
            this.TableLayoutBottom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutBottom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutBottom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutBottom.Size = new System.Drawing.Size(279, 41);
            this.TableLayoutBottom.TabIndex = 16;
            // 
            // TableLayoutBottomLabel
            // 
            this.TableLayoutBottomLabel.ColumnCount = 1;
            this.TableLayoutBottomLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableLayoutBottomLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutBottomLabel.Location = new System.Drawing.Point(139, 0);
            this.TableLayoutBottomLabel.Margin = new System.Windows.Forms.Padding(0);
            this.TableLayoutBottomLabel.Name = "TableLayoutBottomLabel";
            this.TableLayoutBottomLabel.RowCount = 1;
            this.TableLayoutBottomLabel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutBottomLabel.Size = new System.Drawing.Size(140, 1);
            this.TableLayoutBottomLabel.TabIndex = 16;
            // 
            // TableLayoutBottomBox
            // 
            this.TableLayoutBottomBox.ColumnCount = 1;
            this.TableLayoutBottomBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableLayoutBottomBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutBottomBox.Location = new System.Drawing.Point(0, 0);
            this.TableLayoutBottomBox.Margin = new System.Windows.Forms.Padding(0);
            this.TableLayoutBottomBox.Name = "TableLayoutBottomBox";
            this.TableLayoutBottomBox.RowCount = 1;
            this.TableLayoutBottomBox.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutBottomBox.Size = new System.Drawing.Size(139, 1);
            this.TableLayoutBottomBox.TabIndex = 15;
            // 
            // OnOffBox
            // 
            this.OnOffBox.AutoSize = true;
            this.OnOffBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OnOffBox.Location = new System.Drawing.Point(232, 76);
            this.OnOffBox.Name = "OnOffBox";
            this.OnOffBox.Size = new System.Drawing.Size(109, 16);
            this.OnOffBox.TabIndex = 17;
            this.OnOffBox.Text = "OnOffValue";
            this.OnOffBox.UseVisualStyleBackColor = true;
            this.OnOffBox.CheckedChanged += new System.EventHandler(this.OnOffBox_CheckedChanged);
            // 
            // NewAddress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ClientSize = new System.Drawing.Size(364, 134);
            this.Controls.Add(this.TableLayoutBase);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "NewAddress";
            this.Text = "NewAddress";
            this.Load += new System.EventHandler(this.NewAddress_Load);
            this.TableLayoutBase.ResumeLayout(false);
            this.TableLayoutBase.PerformLayout();
            this.TableLayoutBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox AddressBox;
        private System.Windows.Forms.Label AddressLabel;
        private System.Windows.Forms.Label ValueLabel;
        private System.Windows.Forms.TextBox ValueBox;
        private System.Windows.Forms.Label TypeLabel;
        private System.Windows.Forms.ComboBox ScanTypeBox;
        private System.Windows.Forms.CheckBox LockBox;
        private System.Windows.Forms.Label DescriptionLabel;
        private System.Windows.Forms.TextBox DescriptionBox;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.CheckBox PointerBox;
        private System.Windows.Forms.Timer RefreshPointerChecker;
        private System.Windows.Forms.TableLayoutPanel TableLayoutBase;
        private System.Windows.Forms.TableLayoutPanel TableLayoutBottom;
        private System.Windows.Forms.TableLayoutPanel TableLayoutBottomBox;
        private System.Windows.Forms.TableLayoutPanel TableLayoutBottomLabel;
        private System.Windows.Forms.CheckBox OnOffBox;
    }
}