using System.Collections.Generic;

namespace PS4CheaterNeo
{
    public enum SectionCol
    {
        SectionViewID,
        SectionViewAddress,
        SectionViewName,
        SectionViewProt,
        SectionViewLength,
        SectionViewSID,
        SectionViewOffset,
    }

    public enum ResultCol
    {
        ResultListAddress,
        ResultListType,
        ResultListValue,
        ResultListHex,
        ResultListSection,
    }

    public enum ChertCol
    {
        CheatListDel,
        CheatListAddress,
        CheatListType,
        CheatListEnabled,
        CheatListValue,
        CheatListSection,
        CheatListSID,
        CheatListLock,
        CheatListDesc,
    }

    public enum ColorTheme
    {
        Dark,
        Light,
    }

    partial class Constant
    {
        public static string[] Versions = new string[] {
            "9.00",
            "7.55",
            "7.02",
            "6.72",
            "5.05",
            "4.55",
            "4.05",
        };

        public static readonly Dictionary<string, Dictionary<string, object>> GameInfos = new Dictionary<string, Dictionary<string, object>>()
        {
            [""] = new Dictionary<string, object>() //default
            {
                ["ProcessName"] = "SceCdlgApp",
                ["SectionName"] = "libSceCdlgUtilServer.sprx",
                ["SectionProt"] = 3,
                ["IdOffset"] = 0XA0,
                ["VersionOffset"] = 0XC8
            },
            //["6.72"] = new Dictionary<string, object>() //custom version
            //{
            //    ["ProcessName"] = "SceCdlgApp",
            //    ["SectionName"] = "libSceCdlgUtilServer.sprx",
            //    ["SectionProt"] = 3,
            //    ["IdOffset"] = 0XA0,
            //    ["VersionOffset"] = 0XC8
            //},
        };

        public static object[] SearchByBytesFirst = new object[]
        {
            CompareType.Exact,
            CompareType.Between,
            CompareType.UnknownInitial,
            CompareType.BiggerThan,
            CompareType.SmallerThan,
        };
        public static object[] SearchByFloatFirst = new object[]
        {
            CompareType.Fuzzy,
            CompareType.Exact,
            CompareType.Between,
            CompareType.UnknownInitial,
            CompareType.BiggerThan,
            CompareType.SmallerThan,
        };
        public static object[] SearchByBytesNext = new object[]
        {
             CompareType.Exact,
             CompareType.Increased,
             CompareType.Decreased,
             CompareType.Changed,
             CompareType.Unchanged,
             CompareType.IncreasedBy,
             CompareType.DecreasedBy,
             CompareType.Between,
             CompareType.BiggerThan,
             CompareType.SmallerThan,
             CompareType.Fuzzy,
        };
        public static object[] SearchByFloatNext = new object[]
        {
             CompareType.Fuzzy,
             CompareType.Exact,
             CompareType.Increased,
             CompareType.Decreased,
             CompareType.Changed,
             CompareType.Unchanged,
             CompareType.IncreasedBy,
             CompareType.DecreasedBy,
             CompareType.Between,
             CompareType.BiggerThan,
             CompareType.SmallerThan,
        };
        public static object[] SearchByHex = new object[] { CompareType.Exact, };
    }

}
