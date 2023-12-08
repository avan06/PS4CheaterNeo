using System.Drawing;
using System.Windows.Forms;

namespace PS4CheaterNeo
{
    public partial class Option : Form
    {
        readonly Main mainForm;
        readonly ColorTheme colorTheme;
        public Option(Main mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();

            colorTheme = Properties.Settings.Default.ColorTheme.Value;
            Opacity = Properties.Settings.Default.UIOpacity.Value;
            optionTreeView1.ForeColor = Properties.Settings.Default.UiForeColor.Value;
            optionTreeView1.BackColor = Properties.Settings.Default.UiBackColor.Value;
            optionTreeView1.ForeColorLeftView = Properties.Settings.Default.UiForeColor.Value;
            optionTreeView1.BackColorLeftView = Properties.Settings.Default.UiBackColor.Value;
            optionTreeView1.DisplayChangesListWhenSaving = Properties.Settings.Default.DisplayChangesListWhenSaving.Value;

            optionTreeView1.InitSettings(Properties.Settings.Default);
        }

        private void Option_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Properties.Settings.Default.ColorTheme.Value == ColorTheme.Dark && colorTheme != ColorTheme.Dark)
            {
                Properties.Settings.Default.UiForeColor.Value = Color.White;
                Properties.Settings.Default.UiBackColor.Value = Color.FromArgb(36, 36, 36);

                Properties.Settings.Default.MainForeColor.Value                      = Color.Black;
                Properties.Settings.Default.MainBackColor.Value                      = Color.FromArgb(105, 105, 105);
                Properties.Settings.Default.MainToolStrip1BackColor.Value            = Color.FromArgb(153, 180, 209);
                Properties.Settings.Default.MainCheatGridViewRowIndexForeColor.Value = Color.White; //SystemBrushes.HighlightText
                Properties.Settings.Default.MainCheatGridViewBackgroundColor.Value   = Color.DimGray;
                Properties.Settings.Default.MainCheatGridViewBaseRowColor.Value      = Color.FromArgb(100, 115, 129);
                Properties.Settings.Default.MainCheatGridViewGridColor.Value         = Color.Silver;
                Properties.Settings.Default.MainCheatGridCellForeColor.Value         = Color.White;
                Properties.Settings.Default.MainCheatGridCellBackColor.Value         = Color.FromArgb(64, 64, 64);

                Properties.Settings.Default.QueryStatusStrip1BackColor.Value          = Color.DimGray;
                Properties.Settings.Default.QueryAlignmentBoxForeColor.Value          = Color.Silver;
                Properties.Settings.Default.QueryScanBtnBackColor.Value               = Color.SteelBlue;
                Properties.Settings.Default.QuerySectionViewFilterForeColor.Value     = Color.DarkGray;
                Properties.Settings.Default.QuerySectionViewFilterBackColor.Value     = Color.DimGray;
                Properties.Settings.Default.QuerySectionViewFilterSizeForeColor.Value = Color.DarkCyan;
                Properties.Settings.Default.QuerySectionViewFilterSizeBackColor.Value = Color.DarkSlateGray;
                Properties.Settings.Default.QuerySectionViewExecutableForeColor.Value = Color.GreenYellow;
                Properties.Settings.Default.QuerySectionViewNoNameForeColor.Value     = Color.Red;
                Properties.Settings.Default.QuerySectionViewNoName2ForeColor.Value    = Color.HotPink;
                Properties.Settings.Default.QuerySectionViewHiddenForeColor.Value     = Color.Firebrick;
                Properties.Settings.Default.QuerySectionViewItemCheck1BackColor.Value = Color.DarkSlateGray;
                Properties.Settings.Default.QuerySectionViewItemCheck2BackColor.Value = Color.DarkGreen;

                Properties.Settings.Default.HexEditorChangedFinishForeColor.Value = Color.LimeGreen;
                Properties.Settings.Default.HexEditorShadowSelectionColor.Value   = Color.FromArgb(100, 60, 188, 255);
                Properties.Settings.Default.HexEditorZeroBytesForeColor.Value     = Color.DimGray;

                Properties.Settings.Default.PointerFinderStatusStrip1BackColor.Value = Color.DimGray;
                Properties.Settings.Default.PointerFinderScanBtnBackColor.Value      = Color.SteelBlue;

                Properties.Settings.Default.SendPayloadStatusStrip1BackColor.Value = Color.Silver;

                Properties.Settings.Default.Save();
            }
            else if (Properties.Settings.Default.ColorTheme.Value == ColorTheme.Light && colorTheme != ColorTheme.Light)
            {
                Properties.Settings.Default.UiForeColor.Value = Color.Black;
                Properties.Settings.Default.UiBackColor.Value = Color.FromArgb(250, 250, 250);

                Properties.Settings.Default.MainForeColor.Value                      = Color.Black;
                Properties.Settings.Default.MainBackColor.Value                      = Color.White;
                Properties.Settings.Default.MainToolStrip1BackColor.Value            = Color.FromArgb(204, 229, 255);
                Properties.Settings.Default.MainCheatGridViewRowIndexForeColor.Value = Color.Black;
                Properties.Settings.Default.MainCheatGridViewBackgroundColor.Value   = Color.FromArgb(240, 240, 240);
                Properties.Settings.Default.MainCheatGridViewBaseRowColor.Value      = Color.FromArgb(255, 229, 204);
                Properties.Settings.Default.MainCheatGridViewGridColor.Value         = Color.FromArgb(0, 102, 102);
                Properties.Settings.Default.MainCheatGridCellForeColor.Value         = Color.Black;
                Properties.Settings.Default.MainCheatGridCellBackColor.Value         = Color.FromArgb(204, 255, 229);

                Properties.Settings.Default.QueryStatusStrip1BackColor.Value          = Color.FromArgb(200, 200, 200);
                Properties.Settings.Default.QueryAlignmentBoxForeColor.Value          = Color.Silver;
                Properties.Settings.Default.QueryScanBtnBackColor.Value               = Color.DeepSkyBlue;
                Properties.Settings.Default.QuerySectionViewFilterForeColor.Value     = Color.Gray;
                Properties.Settings.Default.QuerySectionViewFilterBackColor.Value     = Color.Silver;
                Properties.Settings.Default.QuerySectionViewFilterSizeForeColor.Value = Color.DarkCyan;
                Properties.Settings.Default.QuerySectionViewFilterSizeBackColor.Value = Color.Silver;
                Properties.Settings.Default.QuerySectionViewExecutableForeColor.Value = Color.FromArgb(0, 210, 0);
                Properties.Settings.Default.QuerySectionViewNoNameForeColor.Value     = Color.Red;
                Properties.Settings.Default.QuerySectionViewNoName2ForeColor.Value    = Color.FromArgb(255, 51, 153);
                Properties.Settings.Default.QuerySectionViewHiddenForeColor.Value     = Color.Firebrick;
                Properties.Settings.Default.QuerySectionViewItemCheck1BackColor.Value = Color.FromArgb(204, 255, 255);
                Properties.Settings.Default.QuerySectionViewItemCheck2BackColor.Value = Color.FromArgb(204, 255, 229);

                Properties.Settings.Default.HexEditorChangedFinishForeColor.Value = Color.Green;
                Properties.Settings.Default.HexEditorShadowSelectionColor.Value   = Color.FromArgb(100, 60, 188, 255);
                Properties.Settings.Default.HexEditorZeroBytesForeColor.Value     = Color.DimGray;

                Properties.Settings.Default.PointerFinderStatusStrip1BackColor.Value = Color.FromArgb(200, 200, 200);
                Properties.Settings.Default.PointerFinderScanBtnBackColor.Value      = Color.DeepSkyBlue;

                Properties.Settings.Default.SendPayloadStatusStrip1BackColor.Value = Color.FromArgb(200, 200, 200);

                Properties.Settings.Default.Save();
            }

            mainForm.ApplyUI();
        }
    }
}
