﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PS4CheaterNeo.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.4.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("|1_General|1_Connect|Enter PS4 IP location")]
        public global::OptionTreeView.Option<string> PS4IP {
            get {
                return ((global::OptionTreeView.Option<string>)(this["PS4IP"]));
            }
            set {
                this["PS4IP"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("9021|1_General|1_Connect|Enter PS4 Port")]
        public global::OptionTreeView.Option<ushort> PS4Port {
            get {
                return ((global::OptionTreeView.Option<ushort>)(this["PS4Port"]));
            }
            set {
                this["PS4Port"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("|1_General|1_SendPayload|Enter PS4 FW Version (Confirm the fw version only when p" +
            "erform sendpayload)")]
        public global::OptionTreeView.Option<string> PS4FWVersion {
            get {
                return ((global::OptionTreeView.Option<string>)(this["PS4FWVersion"]));
            }
            set {
                this["PS4FWVersion"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True|1_General|2_UI|Determines whether to enable collapsible split container ui i" +
            "n Query and HexEditor and PointerFinder windows. Default enabled")]
        public global::OptionTreeView.Option<bool> CollapsibleContainer {
            get {
                return ((global::OptionTreeView.Option<bool>)(this["CollapsibleContainer"]));
            }
            set {
                this["CollapsibleContainer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.95|1_General|2_UI|Determines the opacity of the window, the maximum is 1 (opaqu" +
            "e), the default is 0.95|0.2|1")]
        public global::OptionTreeView.Option<float> UIOpacity {
            get {
                return ((global::OptionTreeView.Option<float>)(this["UIOpacity"]));
            }
            set {
                this["UIOpacity"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True|2_Cheat|Cheat|Determines whether to enable cheat lock in the main window. De" +
            "fault enabled")]
        public global::OptionTreeView.Option<bool> CheatLock {
            get {
                return ((global::OptionTreeView.Option<bool>)(this["CheatLock"]));
            }
            set {
                this["CheatLock"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True|2_Cheat|Cheat|Determine whether to enable verifying Section values when lock" +
            "ing cheat items. Default enabled")]
        public global::OptionTreeView.Option<bool> VerifySectionWhenLock {
            get {
                return ((global::OptionTreeView.Option<bool>)(this["VerifySectionWhenLock"]));
            }
            set {
                this["VerifySectionWhenLock"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True|2_Cheat|Cheat|Determine whether to enable verifying Section values when refr" +
            "eshing the cheat list. Default enabled")]
        public global::OptionTreeView.Option<bool> VerifySectionWhenRefresh {
            get {
                return ((global::OptionTreeView.Option<bool>)(this["VerifySectionWhenRefresh"]));
            }
            set {
                this["VerifySectionWhenRefresh"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True|2_Cheat|Cheat|Determine whether to automatically write to PS4 when editing c" +
            "heat values in UpDown")]
        public global::OptionTreeView.Option<bool> CheatCellDirtyValueCommit {
            get {
                return ((global::OptionTreeView.Option<bool>)(this["CheatCellDirtyValueCommit"]));
            }
            set {
                this["CheatCellDirtyValueCommit"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True|3_Query|1_Query|Determine whether to enable automatic perform get processes " +
            "when opening the Query window. Default enabled")]
        public global::OptionTreeView.Option<bool> AutoPerformGetProcesses {
            get {
                return ((global::OptionTreeView.Option<bool>)(this["AutoPerformGetProcesses"]));
            }
            set {
                this["AutoPerformGetProcesses"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("eboot.bin|3_Query|1_Query|Set the default selected program when perform get proce" +
            "sses. Default is eboot.bin")]
        public global::OptionTreeView.Option<string> DefaultProcess {
            get {
                return ((global::OptionTreeView.Option<string>)(this["DefaultProcess"]));
            }
            set {
                this["DefaultProcess"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3|3_Query|1_Query|Enter the number of threads to use when querying. Default is 3 " +
            "threads")]
        public global::OptionTreeView.Option<byte> MaxQueryThreads {
            get {
                return ((global::OptionTreeView.Option<byte>)(this["MaxQueryThreads"]));
            }
            set {
                this["MaxQueryThreads"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"50|3_Query|1_Query|Set the minimum buffer size (in MB) in querying and pointerFinder, enter 0 to not use buffer, setting this value to 0 is better when the total number of Sections in the game is low. If the game has more than a thousand Sections, Buffer must be set")]
        public global::OptionTreeView.Option<uint> QueryBufferSize {
            get {
                return ((global::OptionTreeView.Option<uint>)(this["QueryBufferSize"]));
            }
            set {
                this["QueryBufferSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("50|3_Query|1_Query|Access value directly by address when the number of query resu" +
            "lts for the same Section is less than this factor, used to control whether to re" +
            "ad Section data completely, or directly access the value by address. Default val" +
            "ue is 50")]
        public global::OptionTreeView.Option<sbyte> MinResultAccessFactor {
            get {
                return ((global::OptionTreeView.Option<sbyte>)(this["MinResultAccessFactor"]));
            }
            set {
                this["MinResultAccessFactor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True|3_Query|1_Query|Determines whether to enable undo scan(revert to the previou" +
            "s scan result), if enabled, more memory needs to be used during scanning. Defaul" +
            "t enable")]
        public global::OptionTreeView.Option<bool> UndoScan {
            get {
                return ((global::OptionTreeView.Option<bool>)(this["UndoScan"]));
            }
            set {
                this["UndoScan"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False|3_Query|1_Query|Determines whether to automatically pause the game when sta" +
            "rting the scan in query. Default disabled")]
        public global::OptionTreeView.Option<bool> ScanAutoPause {
            get {
                return ((global::OptionTreeView.Option<bool>)(this["ScanAutoPause"]));
            }
            set {
                this["ScanAutoPause"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False|3_Query|1_Query|Determines whether to automatically resume the game when th" +
            "e scan is complete in query. Default disabled")]
        public global::OptionTreeView.Option<bool> ScanAutoResume {
            get {
                return ((global::OptionTreeView.Option<bool>)(this["ScanAutoResume"]));
            }
            set {
                this["ScanAutoResume"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True|3_Query|1_Query|Determines whether to show search size message when FirstSca" +
            "n. Default enabled")]
        public global::OptionTreeView.Option<bool> ShowSearchSizeFirstScan {
            get {
                return ((global::OptionTreeView.Option<bool>)(this["ShowSearchSizeFirstScan"]));
            }
            set {
                this["ShowSearchSizeFirstScan"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True|3_Query|2_Floating|Determines whether to make the calculation result of Floa" +
            "ting(float, double) completely exact in query window, there can be 0.0001 differ" +
            "ence in the old mechanism. Default enabled")]
        public global::OptionTreeView.Option<bool> FloatingResultExact {
            get {
                return ((global::OptionTreeView.Option<bool>)(this["FloatingResultExact"]));
            }
            set {
                this["FloatingResultExact"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("11|3_Query|2_Floating|Determine the exponents value of the simple value of floati" +
            "ng. Cheat Engine is set to 11 (2 to the 11th power = 2^11 = plus or minus 2048)." +
            " Default value is 11")]
        public global::OptionTreeView.Option<byte> FloatingSimpleValueExponents {
            get {
                return ((global::OptionTreeView.Option<byte>)(this["FloatingSimpleValueExponents"]));
            }
            set {
                this["FloatingSimpleValueExponents"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True|3_Query|3_Filter|Determine whether to enable filtering Sections when opening" +
            " the query window. Default enabled")]
        public global::OptionTreeView.Option<bool> FilterQuery {
            get {
                return ((global::OptionTreeView.Option<bool>)(this["FilterQuery"]));
            }
            set {
                this["FilterQuery"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("libSce,libc.prx,SceShell,SceLib,SceNp,SceVoice,SceFios,libkernel,SceVdec|3_Query|" +
            "3_Filter|Enter the filter value, the filter will be set here when listing Sectio" +
            "ns")]
        public global::OptionTreeView.Option<string> SectionFilterKeys {
            get {
                return ((global::OptionTreeView.Option<string>)(this["SectionFilterKeys"]));
            }
            set {
                this["SectionFilterKeys"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False|3_Query|3_Filter|Determine whether to enable filtering Sections by size whe" +
            "n opening the query window. Default disabled")]
        public global::OptionTreeView.Option<bool> FilterSizeQuery {
            get {
                return ((global::OptionTreeView.Option<bool>)(this["FilterSizeQuery"]));
            }
            set {
                this["FilterSizeQuery"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("204800|3_Query|3_Filter|Filter out when section size is less than this value(unit" +
            " is bytes)")]
        public global::OptionTreeView.Option<uint> SectionFilterSize {
            get {
                return ((global::OptionTreeView.Option<uint>)(this["SectionFilterSize"]));
            }
            set {
                this["SectionFilterSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0x2000|3_Query|4_Result|Enter the maximum number of displayed query results. will" +
            " only affect the number of results displayed in the ResultView. Default value is" +
            " 8192")]
        public global::OptionTreeView.Option<uint> MaxResultShow {
            get {
                return ((global::OptionTreeView.Option<uint>)(this["MaxResultShow"]));
            }
            set {
                this["MaxResultShow"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False|4_HexEditor|Hex|Determines whether to enable Auto Refresh in HexEditor. Def" +
            "ault disabled")]
        public global::OptionTreeView.Option<bool> AutoRefresh {
            get {
                return ((global::OptionTreeView.Option<bool>)(this["AutoRefresh"]));
            }
            set {
                this["AutoRefresh"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2500|4_HexEditor|Hex|Determines the Interval of AutoRefreshTimer when AutoRefresh" +
            " is enabled, in milliseconds, HexEditor needs to be restarted after this value i" +
            "s changed. Default 2500")]
        public global::OptionTreeView.Option<uint> AutoRefreshTimerInterval {
            get {
                return ((global::OptionTreeView.Option<uint>)(this["AutoRefreshTimerInterval"]));
            }
            set {
                this["AutoRefreshTimerInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("-|4_HexEditor|Hex|Determines the delimited dash value that displays the current H" +
            "ex value in the HexEditor. Default \"-\"")]
        public global::OptionTreeView.Option<string> HexInfoDash {
            get {
                return ((global::OptionTreeView.Option<string>)(this["HexInfoDash"]));
            }
            set {
                this["HexInfoDash"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0|5_UI|1_Color Theme|Determine color theme")]
        public global::OptionTreeView.Option<PS4CheaterNeo.ColorTheme> ColorTheme {
            get {
                return ((global::OptionTreeView.Option<PS4CheaterNeo.ColorTheme>)(this["ColorTheme"]));
            }
            set {
                this["ColorTheme"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("White|5_UI|1_Color Ui|Determine the UI foreground color")]
        public global::OptionTreeView.Option<System.Drawing.Color> UiForeColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["UiForeColor"]));
            }
            set {
                this["UiForeColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("242424|5_UI|1_Color Ui|Determine the UI background color")]
        public global::OptionTreeView.Option<System.Drawing.Color> UiBackColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["UiBackColor"]));
            }
            set {
                this["UiBackColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ControlText|5_UI|2_Color Main|Determine the Main foreground color")]
        public global::OptionTreeView.Option<System.Drawing.Color> MainForeColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["MainForeColor"]));
            }
            set {
                this["MainForeColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ControlDarkDark|5_UI|2_Color Main|Determine the Main background color")]
        public global::OptionTreeView.Option<System.Drawing.Color> MainBackColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["MainBackColor"]));
            }
            set {
                this["MainBackColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("FF99B4D1|5_UI|2_Color Main|Determine the Main ToolStrip1 background color")]
        public global::OptionTreeView.Option<System.Drawing.Color> MainToolStrip1BackColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["MainToolStrip1BackColor"]));
            }
            set {
                this["MainToolStrip1BackColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("White|5_UI|2_Color Main|Determine the Main CheatGridView RowIndex foreground colo" +
            "r")]
        public global::OptionTreeView.Option<System.Drawing.Color> MainCheatGridViewRowIndexForeColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["MainCheatGridViewRowIndexForeColor"]));
            }
            set {
                this["MainCheatGridViewRowIndexForeColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DimGray|5_UI|2_Color Main|Determine the Main CheatGridView background color")]
        public global::OptionTreeView.Option<System.Drawing.Color> MainCheatGridViewBackgroundColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["MainCheatGridViewBackgroundColor"]));
            }
            set {
                this["MainCheatGridViewBackgroundColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("647381|5_UI|2_Color Main|Determine the Main CheatGridView base row color")]
        public global::OptionTreeView.Option<System.Drawing.Color> MainCheatGridViewBaseRowColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["MainCheatGridViewBaseRowColor"]));
            }
            set {
                this["MainCheatGridViewBaseRowColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Silver|5_UI|2_Color Main|Determine the Main CheatGridView grid color")]
        public global::OptionTreeView.Option<System.Drawing.Color> MainCheatGridViewGridColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["MainCheatGridViewGridColor"]));
            }
            set {
                this["MainCheatGridViewGridColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("White|5_UI|2_Color Main|Determine the Main dataGridViewCellStyle foreground color" +
            "")]
        public global::OptionTreeView.Option<System.Drawing.Color> MainCheatGridCellForeColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["MainCheatGridCellForeColor"]));
            }
            set {
                this["MainCheatGridCellForeColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("404040|5_UI|2_Color Main|Determine the Main dataGridViewCellStyle background colo" +
            "r")]
        public global::OptionTreeView.Option<System.Drawing.Color> MainCheatGridCellBackColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["MainCheatGridCellBackColor"]));
            }
            set {
                this["MainCheatGridCellBackColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DimGray|5_UI|3_Color Query|Determine the Query StatusStrip1 background color")]
        public global::OptionTreeView.Option<System.Drawing.Color> QueryStatusStrip1BackColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["QueryStatusStrip1BackColor"]));
            }
            set {
                this["QueryStatusStrip1BackColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Silver|5_UI|3_Color Query|Determine the AlignmentBox foreground color")]
        public global::OptionTreeView.Option<System.Drawing.Color> QueryAlignmentBoxForeColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["QueryAlignmentBoxForeColor"]));
            }
            set {
                this["QueryAlignmentBoxForeColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("SteelBlue|5_UI|3_Color Query|Determine the Query ScanBtn background color")]
        public global::OptionTreeView.Option<System.Drawing.Color> QueryScanBtnBackColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["QueryScanBtnBackColor"]));
            }
            set {
                this["QueryScanBtnBackColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DarkGray|5_UI|3_Color Query|Determine the Query SectionView Filter foreground col" +
            "or")]
        public global::OptionTreeView.Option<System.Drawing.Color> QuerySectionViewFilterForeColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["QuerySectionViewFilterForeColor"]));
            }
            set {
                this["QuerySectionViewFilterForeColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DimGray|5_UI|3_Color Query|Determine the Query SectionView Filter background colo" +
            "r")]
        public global::OptionTreeView.Option<System.Drawing.Color> QuerySectionViewFilterBackColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["QuerySectionViewFilterBackColor"]));
            }
            set {
                this["QuerySectionViewFilterBackColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DarkCyan|5_UI|3_Color Query|Determine the Query SectionView FilterSize foreground" +
            " color")]
        public global::OptionTreeView.Option<System.Drawing.Color> QuerySectionViewFilterSizeForeColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["QuerySectionViewFilterSizeForeColor"]));
            }
            set {
                this["QuerySectionViewFilterSizeForeColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DarkSlateGray|5_UI|3_Color Query|Determine the Query SectionView FilterSize backg" +
            "round color")]
        public global::OptionTreeView.Option<System.Drawing.Color> QuerySectionViewFilterSizeBackColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["QuerySectionViewFilterSizeBackColor"]));
            }
            set {
                this["QuerySectionViewFilterSizeBackColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("GreenYellow|5_UI|3_Color Query|Determine the Query SectionView executable foregro" +
            "und color")]
        public global::OptionTreeView.Option<System.Drawing.Color> QuerySectionViewExecutableForeColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["QuerySectionViewExecutableForeColor"]));
            }
            set {
                this["QuerySectionViewExecutableForeColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Red|5_UI|3_Color Query|Determine the Query SectionView NoName foreground color")]
        public global::OptionTreeView.Option<System.Drawing.Color> QuerySectionViewNoNameForeColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["QuerySectionViewNoNameForeColor"]));
            }
            set {
                this["QuerySectionViewNoNameForeColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("HotPink|5_UI|3_Color Query|Determine the Query SectionView NoName2 foreground col" +
            "or")]
        public global::OptionTreeView.Option<System.Drawing.Color> QuerySectionViewNoName2ForeColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["QuerySectionViewNoName2ForeColor"]));
            }
            set {
                this["QuerySectionViewNoName2ForeColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DarkSlateGray|5_UI|3_Color Query|Determine the Query SectionView ItemCheck1 backg" +
            "round color")]
        public global::OptionTreeView.Option<System.Drawing.Color> QuerySectionViewItemCheck1BackColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["QuerySectionViewItemCheck1BackColor"]));
            }
            set {
                this["QuerySectionViewItemCheck1BackColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DarkGreen|5_UI|3_Color Query|Determine the Query SectionView ItemCheck2 backgroun" +
            "d color")]
        public global::OptionTreeView.Option<System.Drawing.Color> QuerySectionViewItemCheck2BackColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["QuerySectionViewItemCheck2BackColor"]));
            }
            set {
                this["QuerySectionViewItemCheck2BackColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("LimeGreen|5_UI|4_Color HexEditor|Determine the HexEditor ChangedFinish foreground" +
            " color")]
        public global::OptionTreeView.Option<System.Drawing.Color> HexEditorChangedFinishForeColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["HexEditorChangedFinishForeColor"]));
            }
            set {
                this["HexEditorChangedFinishForeColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("643CBCFF|5_UI|4_Color HexEditor|Determine the HexEditor ShadowSelection color")]
        public global::OptionTreeView.Option<System.Drawing.Color> HexEditorShadowSelectionColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["HexEditorShadowSelectionColor"]));
            }
            set {
                this["HexEditorShadowSelectionColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DimGray|5_UI|4_Color HexEditor|Determine the HexEditor ZeroBytes foreground color" +
            "")]
        public global::OptionTreeView.Option<System.Drawing.Color> HexEditorZeroBytesForeColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["HexEditorZeroBytesForeColor"]));
            }
            set {
                this["HexEditorZeroBytesForeColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DimGray|5_UI|5_Color PointerFinder|Determine the PointerFinder statusStrip1 backg" +
            "round color")]
        public global::OptionTreeView.Option<System.Drawing.Color> PointerFinderStatusStrip1BackColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["PointerFinderStatusStrip1BackColor"]));
            }
            set {
                this["PointerFinderStatusStrip1BackColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("SteelBlue|5_UI|5_Color PointerFinder|Determine the PointerFinder ScanBtn backgrou" +
            "nd color")]
        public global::OptionTreeView.Option<System.Drawing.Color> PointerFinderScanBtnBackColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["PointerFinderScanBtnBackColor"]));
            }
            set {
                this["PointerFinderScanBtnBackColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Silver|5_UI|6_Color SendPayload|Determine the SendPayload statusStrip1 background" +
            " color")]
        public global::OptionTreeView.Option<System.Drawing.Color> SendPayloadStatusStrip1BackColor {
            get {
                return ((global::OptionTreeView.Option<System.Drawing.Color>)(this["SendPayloadStatusStrip1BackColor"]));
            }
            set {
                this["SendPayloadStatusStrip1BackColor"] = value;
            }
        }
    }
}
