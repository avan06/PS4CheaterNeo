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
            this.optionTreeView1 = new OptionTreeView.OptionTreeView();
            this.SuspendLayout();
            // 
            // optionTreeView1
            // 
            this.optionTreeView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.optionTreeView1.BackColorLeftView = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.optionTreeView1.BorderStyleLeftView = System.Windows.Forms.BorderStyle.None;
            this.optionTreeView1.ContextMenuStripLeftView = null;
            this.optionTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionTreeView1.FloatingPointDecimalPlaces = 2;
            this.optionTreeView1.FontLeftView = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.optionTreeView1.ForeColor = System.Drawing.Color.White;
            this.optionTreeView1.FullRowSelectLeftView = true;
            this.optionTreeView1.ItemHeightLeftView = 14;
            this.optionTreeView1.Location = new System.Drawing.Point(0, 0);
            this.optionTreeView1.Margin = new System.Windows.Forms.Padding(4);
            this.optionTreeView1.Name = "optionTreeView1";
            this.optionTreeView1.OptionLeftCollapsed = false;
            this.optionTreeView1.OptionLeftMinSize = 25;
            this.optionTreeView1.OptionRightCollapsed = false;
            this.optionTreeView1.OptionRightMinSize = 25;
            this.optionTreeView1.Size = new System.Drawing.Size(548, 450);
            this.optionTreeView1.SortGroupBeforeUnderline = true;
            this.optionTreeView1.SortTreeBeforeUnderline = true;
            this.optionTreeView1.SplitterDistance = 160;
            this.optionTreeView1.SplitterIncrement = 1;
            this.optionTreeView1.SplitterWidth = 4;
            this.optionTreeView1.TabIndex = 0;
            // 
            // Option
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 450);
            this.Controls.Add(this.optionTreeView1);
            this.Name = "Option";
            this.Text = "Option";
            this.ResumeLayout(false);

        }

        #endregion

        private OptionTreeView.OptionTreeView optionTreeView1;
    }
}