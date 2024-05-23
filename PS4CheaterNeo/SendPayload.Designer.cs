
namespace PS4CheaterNeo
{
    partial class SendPayload
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
            this.VersionComboBox = new System.Windows.Forms.ComboBox();
            this.IpBox = new System.Windows.Forms.TextBox();
            this.PortBox = new System.Windows.Forms.TextBox();
            this.SendPayloadBtn = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolStripMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.OKBtn = new System.Windows.Forms.Button();
            this.VersionBox = new System.Windows.Forms.TextBox();
            this.PS4DBGTypeComboBox = new System.Windows.Forms.ComboBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // VersionComboBox
            // 
            this.VersionComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.VersionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VersionComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VersionComboBox.ForeColor = System.Drawing.Color.White;
            this.VersionComboBox.FormattingEnabled = true;
            this.VersionComboBox.Location = new System.Drawing.Point(133, 35);
            this.VersionComboBox.Name = "VersionComboBox";
            this.VersionComboBox.Size = new System.Drawing.Size(50, 20);
            this.VersionComboBox.TabIndex = 0;
            this.VersionComboBox.SelectedIndexChanged += new System.EventHandler(this.VersionComboBox_SelectedIndexChanged);
            // 
            // IpBox
            // 
            this.IpBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.IpBox.ForeColor = System.Drawing.Color.White;
            this.IpBox.Location = new System.Drawing.Point(12, 9);
            this.IpBox.Name = "IpBox";
            this.IpBox.Size = new System.Drawing.Size(120, 22);
            this.IpBox.TabIndex = 1;
            // 
            // PortBox
            // 
            this.PortBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.PortBox.ForeColor = System.Drawing.Color.White;
            this.PortBox.Location = new System.Drawing.Point(133, 9);
            this.PortBox.Name = "PortBox";
            this.PortBox.Size = new System.Drawing.Size(50, 22);
            this.PortBox.TabIndex = 2;
            this.PortBox.Text = "9021";
            // 
            // SendPayloadBtn
            // 
            this.SendPayloadBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.SendPayloadBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SendPayloadBtn.ForeColor = System.Drawing.Color.White;
            this.SendPayloadBtn.Location = new System.Drawing.Point(12, 85);
            this.SendPayloadBtn.Name = "SendPayloadBtn";
            this.SendPayloadBtn.Size = new System.Drawing.Size(171, 23);
            this.SendPayloadBtn.TabIndex = 3;
            this.SendPayloadBtn.Text = "Send Payload";
            this.SendPayloadBtn.UseVisualStyleBackColor = false;
            this.SendPayloadBtn.Click += new System.EventHandler(this.SendPayloadBtn_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.Silver;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMsg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 118);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(277, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ToolStripMsg
            // 
            this.ToolStripMsg.AutoToolTip = true;
            this.ToolStripMsg.BackColor = System.Drawing.Color.Transparent;
            this.ToolStripMsg.ForeColor = System.Drawing.Color.White;
            this.ToolStripMsg.Name = "ToolStripMsg";
            this.ToolStripMsg.Size = new System.Drawing.Size(262, 17);
            this.ToolStripMsg.Spring = true;
            this.ToolStripMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OKBtn
            // 
            this.OKBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.OKBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OKBtn.ForeColor = System.Drawing.Color.White;
            this.OKBtn.Location = new System.Drawing.Point(189, 9);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(76, 99);
            this.OKBtn.TabIndex = 5;
            this.OKBtn.Text = "Save";
            this.OKBtn.UseVisualStyleBackColor = false;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // VersionBox
            // 
            this.VersionBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.VersionBox.ForeColor = System.Drawing.Color.White;
            this.VersionBox.Location = new System.Drawing.Point(12, 34);
            this.VersionBox.Name = "VersionBox";
            this.VersionBox.Size = new System.Drawing.Size(120, 22);
            this.VersionBox.TabIndex = 6;
            // 
            // PS4DBGTypeComboBox
            // 
            this.PS4DBGTypeComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.PS4DBGTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PS4DBGTypeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PS4DBGTypeComboBox.ForeColor = System.Drawing.Color.White;
            this.PS4DBGTypeComboBox.FormattingEnabled = true;
            this.PS4DBGTypeComboBox.Location = new System.Drawing.Point(12, 59);
            this.PS4DBGTypeComboBox.Name = "PS4DBGTypeComboBox";
            this.PS4DBGTypeComboBox.Size = new System.Drawing.Size(171, 20);
            this.PS4DBGTypeComboBox.TabIndex = 7;
            // 
            // SendPayload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ClientSize = new System.Drawing.Size(277, 140);
            this.Controls.Add(this.PS4DBGTypeComboBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.VersionBox);
            this.Controls.Add(this.SendPayloadBtn);
            this.Controls.Add(this.PortBox);
            this.Controls.Add(this.IpBox);
            this.Controls.Add(this.VersionComboBox);
            this.Controls.Add(this.OKBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SendPayload";
            this.Text = "SendPayload or SaveIP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SendPayload_FormClosing);
            this.Load += new System.EventHandler(this.SendPayload_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox VersionComboBox;
        private System.Windows.Forms.TextBox IpBox;
        private System.Windows.Forms.TextBox PortBox;
        private System.Windows.Forms.Button SendPayloadBtn;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ToolStripMsg;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.TextBox VersionBox;
        private System.Windows.Forms.ComboBox PS4DBGTypeComboBox;
    }
}