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
            this.HexViewByteGroup = new System.Windows.Forms.ToolStripComboBox();
            this.HexViewGroupSize = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.SplitContainer2 = new PS4CheaterNeo.CollapsibleSplitContainer();
            this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.TableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.InfoBox5 = new System.Windows.Forms.TextBox();
            this.InfoBox4 = new System.Windows.Forms.TextBox();
            this.InfoBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.InfoBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.InfoBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.InfoBox6 = new System.Windows.Forms.TextBox();
            this.TableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.HexBox = new System.Windows.Forms.CheckBox();
            this.InfoBox0 = new System.Windows.Forms.TextBox();
            this.CommitBtn = new System.Windows.Forms.Button();
            this.FindBtn = new System.Windows.Forms.Button();
            this.AddToCheatGridBtn = new System.Windows.Forms.Button();
            this.TableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.NextBtn = new System.Windows.Forms.Button();
            this.PreviousBtn = new System.Windows.Forms.Button();
            this.PageBox = new System.Windows.Forms.ComboBox();
            this.TableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.RefreshBtn = new System.Windows.Forms.Button();
            this.AutoRefreshBox = new System.Windows.Forms.CheckBox();
            this.TableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.AssemblerBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
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
            this.TableLayoutPanel5.SuspendLayout();
            this.TableLayoutPanel4.SuspendLayout();
            this.TableLayoutPanel2.SuspendLayout();
            this.TableLayoutPanel3.SuspendLayout();
            this.TableLayoutPanel6.SuspendLayout();
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
            this.SplitContainer1.SplitterButtonBitmap = ((System.Drawing.Bitmap)(resources.GetObject("SplitContainer1.SplitterButtonBitmap")));
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
            this.SplitContainer2.Size = new System.Drawing.Size(196, 450);
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
            this.TableLayoutPanel1.Controls.Add(this.TableLayoutPanel5, 0, 9);
            this.TableLayoutPanel1.Controls.Add(this.TableLayoutPanel4, 0, 5);
            this.TableLayoutPanel1.Controls.Add(this.InfoBox0, 0, 8);
            this.TableLayoutPanel1.Controls.Add(this.CommitBtn, 0, 3);
            this.TableLayoutPanel1.Controls.Add(this.FindBtn, 0, 6);
            this.TableLayoutPanel1.Controls.Add(this.AddToCheatGridBtn, 0, 4);
            this.TableLayoutPanel1.Controls.Add(this.TableLayoutPanel2, 0, 0);
            this.TableLayoutPanel1.Controls.Add(this.PageBox, 0, 1);
            this.TableLayoutPanel1.Controls.Add(this.TableLayoutPanel3, 0, 2);
            this.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.TableLayoutPanel1.Name = "TableLayoutPanel1";
            this.TableLayoutPanel1.RowCount = 10;
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel1.Size = new System.Drawing.Size(194, 372);
            this.TableLayoutPanel1.TabIndex = 14;
            // 
            // TableLayoutPanel5
            // 
            this.TableLayoutPanel5.AutoScroll = true;
            this.TableLayoutPanel5.AutoSize = true;
            this.TableLayoutPanel5.ColumnCount = 2;
            this.TableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.TableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 144F));
            this.TableLayoutPanel5.Controls.Add(this.label6, 0, 5);
            this.TableLayoutPanel5.Controls.Add(this.InfoBox5, 1, 4);
            this.TableLayoutPanel5.Controls.Add(this.InfoBox4, 1, 3);
            this.TableLayoutPanel5.Controls.Add(this.InfoBox3, 1, 2);
            this.TableLayoutPanel5.Controls.Add(this.label3, 0, 2);
            this.TableLayoutPanel5.Controls.Add(this.InfoBox1, 1, 0);
            this.TableLayoutPanel5.Controls.Add(this.label1, 0, 0);
            this.TableLayoutPanel5.Controls.Add(this.InfoBox2, 1, 1);
            this.TableLayoutPanel5.Controls.Add(this.label2, 0, 1);
            this.TableLayoutPanel5.Controls.Add(this.label4, 0, 3);
            this.TableLayoutPanel5.Controls.Add(this.label5, 0, 4);
            this.TableLayoutPanel5.Controls.Add(this.InfoBox6, 1, 5);
            this.TableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel5.Location = new System.Drawing.Point(0, 228);
            this.TableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.TableLayoutPanel5.Name = "TableLayoutPanel5";
            this.TableLayoutPanel5.RowCount = 7;
            this.TableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel5.Size = new System.Drawing.Size(194, 144);
            this.TableLayoutPanel5.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "Double";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InfoBox5
            // 
            this.InfoBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InfoBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBox5.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoBox5.ForeColor = System.Drawing.Color.White;
            this.InfoBox5.Location = new System.Drawing.Point(50, 80);
            this.InfoBox5.Margin = new System.Windows.Forms.Padding(0);
            this.InfoBox5.Name = "InfoBox5";
            this.InfoBox5.ReadOnly = true;
            this.InfoBox5.Size = new System.Drawing.Size(144, 22);
            this.InfoBox5.TabIndex = 9;
            // 
            // InfoBox4
            // 
            this.InfoBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InfoBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBox4.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoBox4.ForeColor = System.Drawing.Color.White;
            this.InfoBox4.Location = new System.Drawing.Point(50, 60);
            this.InfoBox4.Margin = new System.Windows.Forms.Padding(0);
            this.InfoBox4.Name = "InfoBox4";
            this.InfoBox4.ReadOnly = true;
            this.InfoBox4.Size = new System.Drawing.Size(144, 22);
            this.InfoBox4.TabIndex = 8;
            // 
            // InfoBox3
            // 
            this.InfoBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InfoBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBox3.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoBox3.ForeColor = System.Drawing.Color.White;
            this.InfoBox3.Location = new System.Drawing.Point(50, 40);
            this.InfoBox3.Margin = new System.Windows.Forms.Padding(0);
            this.InfoBox3.Name = "InfoBox3";
            this.InfoBox3.ReadOnly = true;
            this.InfoBox3.Size = new System.Drawing.Size(144, 22);
            this.InfoBox3.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "4 Byte";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InfoBox1
            // 
            this.InfoBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InfoBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBox1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoBox1.ForeColor = System.Drawing.Color.White;
            this.InfoBox1.Location = new System.Drawing.Point(50, 0);
            this.InfoBox1.Margin = new System.Windows.Forms.Padding(0);
            this.InfoBox1.Name = "InfoBox1";
            this.InfoBox1.ReadOnly = true;
            this.InfoBox1.Size = new System.Drawing.Size(144, 22);
            this.InfoBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Byte";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InfoBox2
            // 
            this.InfoBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InfoBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBox2.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoBox2.ForeColor = System.Drawing.Color.White;
            this.InfoBox2.Location = new System.Drawing.Point(50, 20);
            this.InfoBox2.Margin = new System.Windows.Forms.Padding(0);
            this.InfoBox2.Name = "InfoBox2";
            this.InfoBox2.ReadOnly = true;
            this.InfoBox2.Size = new System.Drawing.Size(144, 22);
            this.InfoBox2.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "2 Byte";
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
            this.label4.Text = "8 Byte";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 20);
            this.label5.TabIndex = 6;
            this.label5.Text = "Float";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InfoBox6
            // 
            this.InfoBox6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InfoBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBox6.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoBox6.ForeColor = System.Drawing.Color.White;
            this.InfoBox6.Location = new System.Drawing.Point(50, 100);
            this.InfoBox6.Margin = new System.Windows.Forms.Padding(0);
            this.InfoBox6.Name = "InfoBox6";
            this.InfoBox6.ReadOnly = true;
            this.InfoBox6.Size = new System.Drawing.Size(144, 22);
            this.InfoBox6.TabIndex = 11;
            // 
            // TableLayoutPanel4
            // 
            this.TableLayoutPanel4.AutoSize = true;
            this.TableLayoutPanel4.ColumnCount = 2;
            this.TableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.TableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel4.Controls.Add(this.InputBox, 1, 0);
            this.TableLayoutPanel4.Controls.Add(this.HexBox, 0, 0);
            this.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel4.Location = new System.Drawing.Point(0, 152);
            this.TableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.TableLayoutPanel4.Name = "TableLayoutPanel4";
            this.TableLayoutPanel4.RowCount = 1;
            this.TableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel4.Size = new System.Drawing.Size(194, 22);
            this.TableLayoutPanel4.TabIndex = 11;
            // 
            // InputBox
            // 
            this.InputBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InputBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InputBox.ForeColor = System.Drawing.Color.White;
            this.InputBox.Location = new System.Drawing.Point(55, 0);
            this.InputBox.Margin = new System.Windows.Forms.Padding(0);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(139, 22);
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
            // InfoBox0
            // 
            this.InfoBox0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.InfoBox0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoBox0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoBox0.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoBox0.ForeColor = System.Drawing.Color.White;
            this.InfoBox0.Location = new System.Drawing.Point(0, 208);
            this.InfoBox0.Margin = new System.Windows.Forms.Padding(0);
            this.InfoBox0.Name = "InfoBox0";
            this.InfoBox0.ReadOnly = true;
            this.InfoBox0.Size = new System.Drawing.Size(194, 20);
            this.InfoBox0.TabIndex = 2;
            // 
            // CommitBtn
            // 
            this.CommitBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CommitBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CommitBtn.ForeColor = System.Drawing.Color.White;
            this.CommitBtn.Location = new System.Drawing.Point(0, 90);
            this.CommitBtn.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.CommitBtn.Name = "CommitBtn";
            this.CommitBtn.Size = new System.Drawing.Size(194, 25);
            this.CommitBtn.TabIndex = 5;
            this.CommitBtn.Text = "Commit";
            this.CommitBtn.UseVisualStyleBackColor = true;
            this.CommitBtn.Click += new System.EventHandler(this.CommitBtn_Click);
            // 
            // FindBtn
            // 
            this.FindBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FindBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FindBtn.ForeColor = System.Drawing.Color.White;
            this.FindBtn.Location = new System.Drawing.Point(0, 180);
            this.FindBtn.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.FindBtn.Name = "FindBtn";
            this.FindBtn.Size = new System.Drawing.Size(194, 25);
            this.FindBtn.TabIndex = 8;
            this.FindBtn.Text = "Find";
            this.FindBtn.UseVisualStyleBackColor = true;
            this.FindBtn.Click += new System.EventHandler(this.FindBtn_Click);
            // 
            // AddToCheatGridBtn
            // 
            this.AddToCheatGridBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddToCheatGridBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddToCheatGridBtn.ForeColor = System.Drawing.Color.White;
            this.AddToCheatGridBtn.Location = new System.Drawing.Point(0, 121);
            this.AddToCheatGridBtn.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.AddToCheatGridBtn.Name = "AddToCheatGridBtn";
            this.AddToCheatGridBtn.Size = new System.Drawing.Size(194, 25);
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
            this.TableLayoutPanel2.Size = new System.Drawing.Size(194, 30);
            this.TableLayoutPanel2.TabIndex = 14;
            // 
            // NextBtn
            // 
            this.NextBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NextBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NextBtn.ForeColor = System.Drawing.Color.White;
            this.NextBtn.Location = new System.Drawing.Point(97, 3);
            this.NextBtn.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.NextBtn.Name = "NextBtn";
            this.NextBtn.Size = new System.Drawing.Size(97, 24);
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
            this.PreviousBtn.Size = new System.Drawing.Size(97, 24);
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
            this.PageBox.Size = new System.Drawing.Size(194, 20);
            this.PageBox.TabIndex = 4;
            this.PageBox.SelectedIndexChanged += new System.EventHandler(this.PageBox_SelectedIndexChanged);
            // 
            // TableLayoutPanel3
            // 
            this.TableLayoutPanel3.AutoSize = true;
            this.TableLayoutPanel3.ColumnCount = 2;
            this.TableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.TableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel3.Controls.Add(this.RefreshBtn, 1, 0);
            this.TableLayoutPanel3.Controls.Add(this.AutoRefreshBox, 0, 0);
            this.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel3.Location = new System.Drawing.Point(0, 59);
            this.TableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.TableLayoutPanel3.Name = "TableLayoutPanel3";
            this.TableLayoutPanel3.RowCount = 1;
            this.TableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel3.Size = new System.Drawing.Size(194, 25);
            this.TableLayoutPanel3.TabIndex = 16;
            // 
            // RefreshBtn
            // 
            this.RefreshBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RefreshBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RefreshBtn.ForeColor = System.Drawing.Color.White;
            this.RefreshBtn.Location = new System.Drawing.Point(55, 0);
            this.RefreshBtn.Margin = new System.Windows.Forms.Padding(0);
            this.RefreshBtn.Name = "RefreshBtn";
            this.RefreshBtn.Size = new System.Drawing.Size(139, 25);
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
            this.AutoRefreshBox.Size = new System.Drawing.Size(49, 19);
            this.AutoRefreshBox.TabIndex = 12;
            this.AutoRefreshBox.Text = "Auto";
            this.AutoRefreshBox.UseVisualStyleBackColor = true;
            this.AutoRefreshBox.CheckedChanged += new System.EventHandler(this.AutoRefreshBox_CheckedChanged);
            // 
            // TableLayoutPanel6
            // 
            this.TableLayoutPanel6.AutoSize = true;
            this.TableLayoutPanel6.ColumnCount = 1;
            this.TableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel6.Controls.Add(this.AssemblerBtn, 0, 0);
            this.TableLayoutPanel6.Controls.Add(this.groupBox1, 0, 1);
            this.TableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
            this.TableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
            this.TableLayoutPanel6.Name = "TableLayoutPanel6";
            this.TableLayoutPanel6.RowCount = 2;
            this.TableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel6.Size = new System.Drawing.Size(194, 70);
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
            this.AssemblerBtn.Size = new System.Drawing.Size(194, 23);
            this.AssemblerBtn.TabIndex = 11;
            this.AssemblerBtn.Text = "AssemblerBox";
            this.AssemblerBtn.UseVisualStyleBackColor = true;
            this.AssemblerBtn.Click += new System.EventHandler(this.AssemblerBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.AsmBox1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(3, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(188, 251);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Assembler Info";
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
            this.AsmBox1.Size = new System.Drawing.Size(182, 230);
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
            this.TableLayoutPanel1.ResumeLayout(false);
            this.TableLayoutPanel1.PerformLayout();
            this.TableLayoutPanel5.ResumeLayout(false);
            this.TableLayoutPanel5.PerformLayout();
            this.TableLayoutPanel4.ResumeLayout(false);
            this.TableLayoutPanel4.PerformLayout();
            this.TableLayoutPanel2.ResumeLayout(false);
            this.TableLayoutPanel3.ResumeLayout(false);
            this.TableLayoutPanel3.PerformLayout();
            this.TableLayoutPanel6.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CollapsibleSplitContainer SplitContainer1;
        private Be.Windows.Forms.HexBox HexView;
        private System.Windows.Forms.TextBox InputBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox HexBox;
        private System.Windows.Forms.Button AssemblerBtn;
        private System.Windows.Forms.Timer AutoRefreshTimer;
        private System.Windows.Forms.ContextMenuStrip HexViewMenu;
        private System.Windows.Forms.ToolStripComboBox HexViewGroupSize;
        private System.Windows.Forms.ToolStripComboBox HexViewByteGroup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private CollapsibleSplitContainer SplitContainer2;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox InfoBox5;
        private System.Windows.Forms.TextBox InfoBox4;
        private System.Windows.Forms.TextBox InfoBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox InfoBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox InfoBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox InfoBox6;
        private System.Windows.Forms.TextBox InfoBox0;
        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
        private System.Windows.Forms.Button CommitBtn;
        private System.Windows.Forms.Button FindBtn;
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
    }
}