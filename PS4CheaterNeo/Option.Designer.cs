namespace PS4CheaterNeo
{
    partial class Option
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
            this.optionTabView1 = new OptionTabView.OptionTabView();
            this.SuspendLayout();
            // 
            // optionTabView1
            // 
            this.optionTabView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.optionTabView1.BackColorLeftView = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.optionTabView1.BorderStyleLeftView = System.Windows.Forms.BorderStyle.None;
            this.optionTabView1.ContextMenuStripLeftView = null;
            this.optionTabView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionTabView1.FloatingPointDecimalPlaces = 2;
            this.optionTabView1.FontLeftView = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.optionTabView1.ForeColor = System.Drawing.Color.White;
            this.optionTabView1.FullRowSelectLeftView = true;
            this.optionTabView1.ItemHeightLeftView = 14;
            this.optionTabView1.Location = new System.Drawing.Point(0, 0);
            this.optionTabView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.optionTabView1.Name = "optionTabView1";
            this.optionTabView1.Size = new System.Drawing.Size(548, 450);
            this.optionTabView1.TabIndex = 0;
            // 
            // Option
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 450);
            this.Controls.Add(this.optionTabView1);
            this.Name = "Option";
            this.Text = "Option";
            this.ResumeLayout(false);

        }

        #endregion

        private OptionTabView.OptionTabView optionTabView1;
    }
}