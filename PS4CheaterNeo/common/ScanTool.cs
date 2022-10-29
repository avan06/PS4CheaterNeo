using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static PS4CheaterNeo.SectionTool;

namespace PS4CheaterNeo
{
    public enum ScanType
    {
        [Description("Byte")]
        Byte_,
        [Description("2 Bytes")]
        Bytes_2,
        [Description("4 Bytes")]
        Bytes_4,
        [Description("8 Bytes")]
        Bytes_8,
        [Description("Float")]
        Float_,
        [Description("Double")]
        Double_,
        [Description("String")]
        String_,
        Hex,
        Group,
    }

    public enum CompareType
    {
        Exact,
        Fuzzy,
        Increased,
        [Description("Increased by")]
        IncreasedBy,
        Decreased,
        [Description("Decreased by")]
        DecreasedBy,
        [Description("Bigger than")]
        BiggerThan,
        [Description("Smaller than")]
        SmallerThan,
        Changed,
        Unchanged,
        Between,
        [Description("Unknown Initial")]
        UnknownInitial,
    }

    public static class ScanTool
    {
        public static Dictionary<ScanType, int> ScanTypeLengthDict = new Dictionary<ScanType, int>()
        {
            [ScanType.Byte_] = 1,
            [ScanType.Bytes_2] = 2,
            [ScanType.Bytes_4] = 4,
            [ScanType.Bytes_8] = 8,
            [ScanType.Float_] = 4,
            [ScanType.Double_] = 8,
        };

        public static byte[] ValueStringToByte(ScanType scanType, string value)
        {
            byte[] bytes = null;

            if (value == null || value.Length == 0) return null;

            switch (scanType)
            {
                case ScanType.Bytes_8:
                    bytes = BitConverter.GetBytes(ulong.Parse(value));
                    break;
                case ScanType.Bytes_4:
                    bytes = BitConverter.GetBytes(uint.Parse(value));
                    break;
                case ScanType.Bytes_2:
                    bytes = BitConverter.GetBytes(UInt16.Parse(value));
                    break;
                case ScanType.Byte_:
                    bytes = BitConverter.GetBytes(Byte.Parse(value));
                    Array.Resize(ref bytes, 1);
                    break;
                case ScanType.Double_:
                    bytes = BitConverter.GetBytes(double.Parse(value));
                    break;
                case ScanType.Float_:
                    bytes = BitConverter.GetBytes(float.Parse(value));
                    break;
                case ScanType.Hex:
                    value = value.Replace(" ", "").Replace("-", "").Replace("_", "");
                    bytes = new byte[value.Length / 2];
                    for (int idx = 0; idx < bytes.Length; idx++) bytes[idx] = Convert.ToByte(value.Substring(idx * 2, 2), 16);
                    break;
                case ScanType.String_:
                    bytes = Encoding.Default.GetBytes(value);
                    break;
                case ScanType.Group:
                    break;
                default:
                    throw new Exception("ScanType verification failed");
            }
            return bytes;
        }

        public static ulong BytesToULong(byte[] bytes)
        {
            if (bytes.Length < 8) Array.Resize(ref bytes, 8);
            ulong valueUlong = BitConverter.ToUInt64(bytes, 0);
            return valueUlong;
        }

        public static ulong ValueStringToULong(ScanType scanType, string value)
        {
            byte[] bytes = ValueStringToByte(scanType, value);
            ulong valueUlong = BytesToULong(bytes);

            return valueUlong;
        }

        public static string BytesToString(ScanType scanType, Byte[] value, bool isHex = false)
        {
            string result = "";
            string hexFormat = "";
            switch (scanType)
            {
                case ScanType.Bytes_8:
                    if (isHex) hexFormat = "X16";
                    result = BitConverter.ToUInt64(value, 0).ToString(hexFormat);
                    break;
                case ScanType.Bytes_4:
                    if (isHex) hexFormat = "X8";
                    result = BitConverter.ToUInt32(value, 0).ToString(hexFormat);
                    break;
                case ScanType.Bytes_2:
                    if (isHex) hexFormat = "X4";
                    result = BitConverter.ToUInt16(value, 0).ToString(hexFormat);
                    break;
                case ScanType.Byte_:
                    if (isHex) hexFormat = "X2";
                    result = value[0].ToString(hexFormat);
                    break;
                case ScanType.Double_:
                    if (isHex) result = BitConverter.ToInt64(value, 0).ToString("X16");
                    else result = BitConverter.ToDouble(value, 0).ToString();
                    break;
                case ScanType.Float_:
                    if (isHex) result = BitConverter.ToInt32(value, 0).ToString("X8");
                    else result = BitConverter.ToSingle(value, 0).ToString();
                    break;
                case ScanType.Hex:
                    result = BitConverter.ToString(value).Replace("-", "");
                    break;
                case ScanType.String_:
                    if (isHex) result = BitConverter.ToString(value).Replace("-", "");
                    else result = Encoding.Default.GetString(value);
                    break;
                case ScanType.Group:
                    break;
                default:
                    throw new Exception("ScanType verification failed");
            }
            return result;
        }

        public static string ULongToString(ScanType scanType, ulong value, bool isHex = false)
        {
            byte[] valueBytes = BitConverter.GetBytes(value);
            string result = BytesToString(scanType, valueBytes, isHex);
            return result;
        }

        public static string ReverseHexString(string hex)
        {
            if (hex == null || hex.Trim().Length <= 2) return hex;

            hex = hex.Replace(" ", "").Replace("-", "").Replace("_", "");

            if (hex.Length % 2 != 0) hex = "0" + hex;

            string result = "";
            for (int idx = 0; idx <= hex.Length - 2; idx += 2) result = hex.Substring(idx, 2) + result;

            return result;
        }

        /// <summary>
        /// Floating-Point simple values only
        /// Refer to IEEE754 and cheat-engine's floatscanWithoutExponents specification
        /// IEEE Floating-Point numbers are stored as follows:
        /// The single format 32 bit has: 1 bit for sign,  8 bits for exponent, 23 bits for fraction
        /// The double format 64 bit has: 1 bit for sign, 11 bits for exponent, 52 bits for fraction
        /// https://en.wikipedia.org/wiki/IEEE_754
        /// https://github.com/cheat-engine/cheat-engine
        /// https://stackoverflow.com/a/390072
        /// https://userpages.umbc.edu/~squire/cs411_l11.html
        /// </summary>
        /// <param name="comparerTool">comparison tool</param>
        /// <param name="newValue">new value in memory</param>
        /// <param name="inputValue0">the value of the specified query</param>
        /// <param name="groupScanType">only for group scan type</param>
        public static bool ComparerExact(ComparerTool comparerTool, byte[] newValue, byte[] inputValue0, object groupScanType = null)
        {
            bool result;
            ScanType exactScanType = groupScanType == null ? comparerTool.scanType : (ScanType)groupScanType;
            switch (exactScanType)
            {
                case ScanType.Bytes_8:
                case ScanType.Bytes_4:
                case ScanType.Bytes_2:
                case ScanType.Byte_:
                case ScanType.String_:
                case ScanType.Group:
                    if (inputValue0.Length != newValue.Length) throw new ArgumentException("Comparer String length verification failed");
                    for (int idx = 0; idx < inputValue0.Length; ++idx) if (inputValue0[idx] != newValue[idx]) return false;
                    result = true;
                    break;
                case ScanType.Hex:
                    if (inputValue0.Length != newValue.Length) throw new ArgumentException("Comparer String length verification failed");
                    for (int idx = 0; idx < inputValue0.Length; ++idx)
                    {
                        if ((comparerTool.value0ByteWildcards == null || !comparerTool.value0ByteWildcards.Contains(idx)) &&
                            (inputValue0[idx] != newValue[idx])) return false;
                    }
                    result = true;
                    break;
                case ScanType.Double_:
                    var newDouble = BitConverter.ToDouble(newValue, 0);
                    var input0Double = BitConverter.ToDouble(inputValue0, 0);
                    result = Math.Abs(newDouble - input0Double) < 1;
                    if (result && comparerTool.isFloatingSimpleValues)
                    {
                        ulong newVar = BitConverter.ToUInt64(newValue, 0);
                        if (newVar > 0 && Math.Abs(1023 - (int)(((long)newVar >> 52) & 0x7ffL)) > comparerTool.floatingSimpleValueExponents) return false;
                    }
                    break;
                case ScanType.Float_:
                    var newFloat = BitConverter.ToSingle(newValue, 0);
                    var input0Float = BitConverter.ToSingle(inputValue0, 0);
                    result = Math.Abs(newFloat - input0Float) < 1;
                    if (result && comparerTool.isFloatingSimpleValues)
                    {
                        uint newVar = BitConverter.ToUInt32(newValue, 0);
                        if (newVar > 0 && Math.Abs(127 - (int)(((int)newVar >> 23) & 0xffL)) > comparerTool.floatingSimpleValueExponents) return false;
                    }
                    break;
                default:
                    throw new Exception("Unknown scanType type.");
            }

            return result;
        }

        /// <summary>
        /// Floating-Point simple values only
        /// Refer to IEEE754 and cheat-engine's floatscanWithoutExponents specification
        /// IEEE Floating-Point numbers are stored as follows:
        /// The single format 32 bit has: 1 bit for sign,  8 bits for exponent, 23 bits for fraction
        /// The double format 64 bit has: 1 bit for sign, 11 bits for exponent, 52 bits for fraction
        /// https://en.wikipedia.org/wiki/IEEE_754
        /// https://github.com/cheat-engine/cheat-engine
        /// https://stackoverflow.com/a/390072
        /// https://userpages.umbc.edu/~squire/cs411_l11.html
        /// </summary>
        /// <param name="comparerTool">comparison tool</param>
        /// <param name="newData">new value in memory</param>
        /// <param name="oldData">previous result value</param>
        public static bool Comparer(ComparerTool comparerTool, ulong newData, ulong oldData)
        {
            bool result = false;

            UInt64 newUInt64 = 0;
            UInt64 oldUInt64 = 0;

            UInt32 newUInt32 = 0;
            UInt32 oldUInt32 = 0;

            UInt16 newUInt16 = 0;
            UInt16 oldUInt16 = 0;

            byte newByte = 0;
            byte oldByte = 0;

            double newDouble = 0;
            double oldDouble = 0;

            float newFloat = 0;
            float oldFloat = 0;

            switch (comparerTool.scanType)
            {
                case ScanType.Bytes_8:
                    newUInt64 = newData;
                    oldUInt64 = oldData;
                    break;
                case ScanType.Bytes_4:
                    newUInt32 = (UInt32)newData;
                    oldUInt32 = (UInt32)oldData;
                    break;
                case ScanType.Bytes_2:
                    newUInt16 = (UInt16)newData;
                    oldUInt16 = (UInt16)oldData;
                    break;
                case ScanType.Byte_:
                    newByte = BitConverter.GetBytes(newData)[0];
                    oldByte = BitConverter.GetBytes(oldData)[0];
                    break;
                case ScanType.Double_:
                    newDouble = BitConverter.ToDouble(BitConverter.GetBytes(newData), 0);
                    oldDouble = BitConverter.ToDouble(BitConverter.GetBytes(oldData), 0);
                    break;
                case ScanType.Float_:
                    newFloat = BitConverter.ToSingle(BitConverter.GetBytes(newData), 0);
                    oldFloat = BitConverter.ToSingle(BitConverter.GetBytes(oldData), 0);
                    break;
                default:
                    throw new Exception("Unknown scanType type.");
            }

            if (comparerTool.compareType == CompareType.Exact)
            {
                switch (comparerTool.scanType)
                {
                    case ScanType.Bytes_8:
                        result = newUInt64 == comparerTool.input0UInt64;
                        break;
                    case ScanType.Bytes_4:
                        result = newUInt32 == comparerTool.input0UInt32;
                        break;
                    case ScanType.Bytes_2:
                        result = newUInt16 == comparerTool.input0UInt16;
                        break;
                    case ScanType.Byte_:
                        result = newByte == comparerTool.input0Byte;
                        break;
                    case ScanType.Double_:
                        double valD = Math.Abs(newDouble - comparerTool.input0Double);
                        result = comparerTool.enableFloatingResultExact ? valD == 0 : valD < 0.0001;
                        break;
                    case ScanType.Float_:
                        float valF = Math.Abs(newFloat - comparerTool.input0Float);
                        result = comparerTool.enableFloatingResultExact ? valF == 0 : valF < 0.0001;
                        break;
                    default:
                        throw new Exception("Unknown scanType type.");
                }
            }
            else if (comparerTool.compareType == CompareType.Fuzzy)
            {
                switch (comparerTool.scanType)
                {
                    case ScanType.Double_:
                        result = Math.Abs(newDouble - comparerTool.input0Double) < 1;
                        break;
                    case ScanType.Float_:
                        result = Math.Abs(newFloat - comparerTool.input0Float) < 1;
                        break;
                    default:
                        throw new Exception("Unknown scanType type.");
                }
            }
            else if (comparerTool.compareType == CompareType.Increased)
            {
                switch (comparerTool.scanType)
                {
                    case ScanType.Bytes_8:
                        result = newUInt64 > oldUInt64;
                        break;
                    case ScanType.Bytes_4:
                        result = newUInt32 > oldUInt32;
                        break;
                    case ScanType.Bytes_2:
                        result = newUInt16 > oldUInt16;
                        break;
                    case ScanType.Byte_:
                        result = newByte > oldByte;
                        break;
                    case ScanType.Double_:
                        result = newDouble > oldDouble;
                        break;
                    case ScanType.Float_:
                        result = newFloat > oldFloat;
                        break;
                    default:
                        throw new Exception("Unknown scanType type.");
                }
            }
            else if (comparerTool.compareType == CompareType.IncreasedBy)
            {
                switch (comparerTool.scanType)
                {
                    case ScanType.Bytes_8:
                        result = newUInt64 == oldUInt64 + comparerTool.input0UInt64;
                        break;
                    case ScanType.Bytes_4:
                        result = newUInt32 == oldUInt32 + comparerTool.input0UInt32;
                        break;
                    case ScanType.Bytes_2:
                        result = newUInt16 == oldUInt16 + comparerTool.input0UInt16;
                        break;
                    case ScanType.Byte_:
                        result = newByte == oldByte + comparerTool.input0Byte;
                        break;
                    case ScanType.Double_:
                        double valD = Math.Abs(newDouble - oldDouble - comparerTool.input0Double);
                        result = comparerTool.enableFloatingResultExact ? valD == 0 : valD < 0.0001;
                        break;
                    case ScanType.Float_:
                        float valF = Math.Abs(newFloat - oldFloat - comparerTool.input0Float);
                        result = comparerTool.enableFloatingResultExact ? valF == 0 : valF < 0.0001;
                        break;
                    default:
                        throw new Exception("Unknown scanType type.");
                }
            }
            else if (comparerTool.compareType == CompareType.Decreased)
            {
                switch (comparerTool.scanType)
                {
                    case ScanType.Bytes_8:
                        result = newUInt64 < oldUInt64;
                        break;
                    case ScanType.Bytes_4:
                        result = newUInt32 < oldUInt32;
                        break;
                    case ScanType.Bytes_2:
                        result = newUInt16 < oldUInt16;
                        break;
                    case ScanType.Byte_:
                        result = newByte < oldByte;
                        break;
                    case ScanType.Double_:
                        result = newDouble < oldDouble;
                        break;
                    case ScanType.Float_:
                        result = newFloat < oldFloat;
                        break;
                    default:
                        throw new Exception("Unknown scanType type.");
                }
            }
            else if (comparerTool.compareType == CompareType.DecreasedBy)
            {
                switch (comparerTool.scanType)
                {
                    case ScanType.Bytes_8:
                        result = newUInt64 == oldUInt64 - comparerTool.input0UInt64;
                        break;
                    case ScanType.Bytes_4:
                        result = newUInt32 == oldUInt32 - comparerTool.input0UInt32;
                        break;
                    case ScanType.Bytes_2:
                        result = newUInt16 == oldUInt16 - comparerTool.input0UInt16;
                        break;
                    case ScanType.Byte_:
                        result = newByte == oldByte - comparerTool.input0Byte;
                        break;
                    case ScanType.Double_:
                        double valD = Math.Abs(newDouble - oldDouble + comparerTool.input0Double);
                        result = comparerTool.enableFloatingResultExact ? valD == 0 : valD < 0.0001;
                        break;
                    case ScanType.Float_:
                        float valF = Math.Abs(newFloat - oldFloat + comparerTool.input0Float);
                        result = comparerTool.enableFloatingResultExact ? valF == 0 : valF < 0.0001;
                        break;
                    default:
                        throw new Exception("Unknown scanType type.");
                }
            }
            else if (comparerTool.compareType == CompareType.BiggerThan)
            {
                switch (comparerTool.scanType)
                {
                    case ScanType.Bytes_8:
                        result = newUInt64 > comparerTool.input0UInt64;
                        break;
                    case ScanType.Bytes_4:
                        result = newUInt32 > comparerTool.input0UInt32;
                        break;
                    case ScanType.Bytes_2:
                        result = newUInt16 > comparerTool.input0UInt16;
                        break;
                    case ScanType.Byte_:
                        result = newByte > comparerTool.input0Byte;
                        break;
                    case ScanType.Double_:
                        result = newDouble > comparerTool.input0Double;
                        break;
                    case ScanType.Float_:
                        result = newFloat > comparerTool.input0Float;
                        break;
                    default:
                        throw new Exception("Unknown scanType type.");
                }
            }
            else if (comparerTool.compareType == CompareType.SmallerThan)
            {
                switch (comparerTool.scanType)
                {
                    case ScanType.Bytes_8:
                        result = newUInt64 < comparerTool.input0UInt64;
                        break;
                    case ScanType.Bytes_4:
                        result = newUInt32 < comparerTool.input0UInt32;
                        break;
                    case ScanType.Bytes_2:
                        result = newUInt16 < comparerTool.input0UInt16;
                        break;
                    case ScanType.Byte_:
                        result = newByte < comparerTool.input0Byte;
                        break;
                    case ScanType.Double_:
                        result = newDouble < comparerTool.input0Double;
                        break;
                    case ScanType.Float_:
                        result = newFloat < comparerTool.input0Float;
                        break;
                    default:
                        throw new Exception("Unknown scanType type.");
                }
            }
            else if (comparerTool.compareType == CompareType.Changed)
            {
                switch (comparerTool.scanType)
                {
                    case ScanType.Bytes_8:
                        result = newUInt64 != oldUInt64;
                        break;
                    case ScanType.Bytes_4:
                        result = newUInt32 != oldUInt32;
                        break;
                    case ScanType.Bytes_2:
                        result = newUInt16 != oldUInt16;
                        break;
                    case ScanType.Byte_:
                        result = newByte != oldByte;
                        break;
                    case ScanType.Double_:
                        double valD = Math.Abs(newDouble - oldDouble);
                        result = comparerTool.enableFloatingResultExact ? valD != 0 : valD >= 0.0001;
                        break;
                    case ScanType.Float_:
                        float valF = Math.Abs(newFloat - oldFloat);
                        result = comparerTool.enableFloatingResultExact ? valF != 0 : valF >= 0.0001;
                        break;
                    default:
                        throw new Exception("Unknown scanType type.");
                }
            }
            else if (comparerTool.compareType == CompareType.Unchanged)
            {
                switch (comparerTool.scanType)
                {
                    case ScanType.Bytes_8:
                        result = newUInt64 == oldUInt64;
                        break;
                    case ScanType.Bytes_4:
                        result = newUInt32 == oldUInt32;
                        break;
                    case ScanType.Bytes_2:
                        result = newUInt16 == oldUInt16;
                        break;
                    case ScanType.Byte_:
                        result = newByte == oldByte;
                        break;
                    case ScanType.Double_:
                        Double valD = Math.Abs(newDouble - oldDouble);
                        result = comparerTool.enableFloatingResultExact ? valD == 0 : valD < 0.0001;
                        break;
                    case ScanType.Float_:
                        float valF = Math.Abs(newFloat - oldFloat);
                        result = comparerTool.enableFloatingResultExact ? valF == 0 : valF < 0.0001;
                        break;
                    default:
                        throw new Exception("Unknown scanType type.");
                }
            }
            else if (comparerTool.compareType == CompareType.Between)
            {
                switch (comparerTool.scanType)
                {
                    case ScanType.Bytes_8:
                        result = newUInt64 <= comparerTool.input1UInt64 && newUInt64 >= comparerTool.input0UInt64;
                        break;
                    case ScanType.Bytes_4:
                        result = newUInt32 <= comparerTool.input1UInt32 && newUInt32 >= comparerTool.input0UInt32;
                        break;
                    case ScanType.Bytes_2:
                        result = newUInt16 <= comparerTool.input1UInt16 && newUInt16 >= comparerTool.input0UInt16;
                        break;
                    case ScanType.Byte_:
                        result = newByte <= comparerTool.input1Byte && newByte >= comparerTool.input0Byte;
                        break;
                    case ScanType.Double_:
                        result = newDouble <= comparerTool.input1Double && newDouble >= comparerTool.input0Double;
                        break;
                    case ScanType.Float_:
                        result = newFloat <= comparerTool.input1Float && newFloat >= comparerTool.input0Float;
                        break;
                    default:
                        throw new Exception("Unknown scanType type.");
                }
            }
            else if (comparerTool.compareType == CompareType.UnknownInitial)
            {
                switch (comparerTool.scanType)
                {
                    case ScanType.Bytes_8:
                        result = newUInt64 != 0;
                        break;
                    case ScanType.Bytes_4:
                        result = newUInt32 != 0;
                        break;
                    case ScanType.Bytes_2:
                        result = newUInt16 != 0;
                        break;
                    case ScanType.Byte_:
                        result = newByte != 0;
                        break;
                    case ScanType.Double_:
                        result = newDouble != 0;
                        break;
                    case ScanType.Float_:
                        result = newFloat != 0;
                        break;
                    default:
                        throw new Exception("Unknown scanType type.");
                }
            }

            if (result && comparerTool.isFloatingSimpleValues && newData > 0)
            {
                if (comparerTool.scanType == ScanType.Double_)
                {
                    if (Math.Abs(1023 - (int)(((long)newData >> 52) & 0x7ffL)) > comparerTool.floatingSimpleValueExponents) result = false;
                }
                else if (comparerTool.scanType == ScanType.Float_)
                {
                    if (Math.Abs(127 - (int)(((int)newData >> 23) & 0xffL)) > comparerTool.floatingSimpleValueExponents) result = false;
                }
            }

            if (comparerTool.isNot &&
                comparerTool.compareType != CompareType.IncreasedBy &&
                comparerTool.compareType != CompareType.DecreasedBy &&
                comparerTool.compareType != CompareType.UnknownInitial) result ^= comparerTool.isNot; //result XOR true

            return result;
        }

        public static void GameInfo(string FMVer, out string gameID, out string gameVer)
        {
            gameID = null;
            gameVer = null;

            Dictionary<string, object> gameInfo = Constant.GameInfos[""];
            if (Constant.GameInfos.ContainsKey(FMVer)) gameInfo = Constant.GameInfos[FMVer];

            string processName = (string)gameInfo["ProcessName"];
            string sectionName = (string)gameInfo["SectionName"];
            uint sectionProt = Convert.ToUInt32(gameInfo["SectionProt"]);
            ulong idOffset = Convert.ToUInt64(gameInfo["IdOffset"]);
            ulong versionOffset = Convert.ToUInt64(gameInfo["VersionOffset"]);

            try
            {
                SectionTool sectionTool = new SectionTool();
                sectionTool.InitSectionList(processName);

                Section gameInfoSection = sectionTool.GetSection(sectionName, sectionProt);

                if (gameInfoSection == null) return;

                gameID = Encoding.Default.GetString(PS4Tool.ReadMemory(sectionTool.PID, gameInfoSection.Start + idOffset, 16));
                gameID = gameID.Trim(new char[] { '\0' });
                gameVer = Encoding.Default.GetString(PS4Tool.ReadMemory(sectionTool.PID, gameInfoSection.Start + versionOffset, 16));
                gameVer = gameVer.Trim(new char[] { '\0' });
            }
            catch
            {

            }
        }

        public static (List<(ScanType scanType, int groupTypeLength, bool isAny)> groupTypes, List<byte[]> groupValues, int groupFirstLength, int scanTypeLength) GenerateGroupList(string value0)
        {
            int scanTypeLength = 0;
            int groupFirstLength = 1;
            var groupTypes = new List<(ScanType scanType, int groupTypeLength, bool isAny)>();
            var groupValues = new List<byte[]>();
            var cmd = new Dictionary<string, string>();
            value0 = value0.ToUpper().Trim();
            value0 = Regex.Replace(value0, @" *([,:]) *", "$1"); //Remove useless whitespace
            value0 = Regex.Replace(value0, @" +", " "); //Remove excess whitespace
            int start = 0;
            for (int idx = 0; idx <= value0.Length; ++idx) //Parse input value
            {
                if (idx == value0.Length && start < value0.Length) cmd["scanVal"] = value0.Substring(start, value0.Length - start); //Get the last scanVal
                else if (value0[idx].Equals(':')) //Get typeKey value
                {
                    cmd["typeKey"] = value0.Substring(start, idx - start);
                    start = idx + 1;
                }
                else if (Regex.IsMatch(value0[idx].ToString(), "[, ]")) //Get scanVal value
                {
                    cmd["scanVal"] = value0.Substring(start, idx - start);
                    start = idx + 1;
                }

                if (cmd.TryGetValue("scanVal", out string scanVal))
                {
                    cmd.TryGetValue("typeKey", out string typeKey);
                    ScanType scanType;

                    if (typeKey == "1") scanType = ScanType.Byte_;
                    else if (typeKey == "2") scanType = ScanType.Bytes_2;
                    else if (typeKey == "4") scanType = ScanType.Bytes_4;
                    else if (typeKey == "8") scanType = ScanType.Bytes_8;
                    else if (typeKey == "F") scanType = ScanType.Float_;
                    else if (typeKey == "D") scanType = ScanType.Double_;
                    else if (typeKey == "H") scanType = ScanType.Hex;
                    else if (scanVal.EndsWith("LU") || scanVal.EndsWith("UL"))
                    {
                        scanVal = scanVal.Remove(scanVal.Length - 2);
                        scanType = ScanType.Bytes_8;
                    }
                    else if (scanVal.EndsWith("U"))
                    {
                        scanVal = scanVal.Remove(scanVal.Length - 1);
                        scanType = ScanType.Bytes_8;
                    }
                    else if (Regex.IsMatch(scanVal, @"^[\d]+\.*F"))
                    {
                        scanVal = scanVal.Remove(scanVal.Length - 1);
                        scanType = ScanType.Float_;
                    }
                    else if (Regex.IsMatch(scanVal, @"^[\d]+\.*D"))
                    {
                        scanVal = scanVal.Remove(scanVal.Length - 1);
                        scanType = ScanType.Double_;
                    }
                    else if (scanVal.StartsWith("0X"))
                    {
                        scanVal = scanVal.Substring(2);
                        scanType = ScanType.Hex;
                    }
                    else if (Regex.IsMatch(scanVal, @"[\d]+\.[\d]*")) scanType = ScanType.Float_;
                    else if (Regex.IsMatch(scanVal, @"[A-F]+")) scanType = ScanType.Hex;
                    else scanType = ScanType.Bytes_4;

                    bool isAny = false;
                    byte[] valueBytes;
                    if (scanVal == "*" || scanVal == "?")
                    {
                        isAny = true;
                        ScanTypeLengthDict.TryGetValue(scanType, out int anyLength);
                        valueBytes = new byte[anyLength];
                    }
                    else valueBytes = ValueStringToByte(scanType, scanVal);

                    if (!ScanTypeLengthDict.TryGetValue(scanType, out int groupTypeLength)) groupTypeLength = valueBytes.Length;
                    else if (groupTypes.Count == 0) groupFirstLength = groupTypeLength;
                    scanTypeLength += groupTypeLength;
                    groupTypes.Add((scanType, groupTypeLength, isAny));
                    groupValues.Add(valueBytes);
                    cmd = new Dictionary<string, string>();
                }
            }

            return (groupTypes, groupValues, groupFirstLength, scanTypeLength);
        }

        /// <summary>
        /// how to get enum description attribute
        /// https://stackoverflow.com/a/10986749
        /// </summary>
        public static string GetDescription<T>(this T source) where T : Enum
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }

        /// <summary>
        /// Get Enum from Description attribute
        /// https://stackoverflow.com/a/4367868
        /// </summary>
        public static T ParseFromDescription<T>(this Form _, string description) where T : Enum
        {
            FieldInfo[] fields = typeof(T).GetFields();
            for (int idx =0; idx < fields.Length; idx++)
            {
                FieldInfo field = fields[idx];
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description) return (T)field.GetValue(null);
                }
                else if (field.Name == description) return (T)field.GetValue(null);
            }

            throw new ArgumentException("ParseFromDescription Not found.", string.Format("{0}({1})", nameof(description), description));
        }

        /// <summary>
        /// formats a 64bit "Unsigned" value to its equivalent binary value
        /// https://stackoverflow.com/a/6986104
        /// https://stackoverflow.com/a/57755224
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToBinary(this UInt64 input)
        {
            //UInt32 low = (UInt32)(input & 0xFFFFFFFF);
            //UInt32 high = (UInt32)(input & 0xFFFFFFFF00000000 >> 32);
            //return $"{Convert.ToString(high, 2).PadLeft(32, '0')}{Convert.ToString(low, 2).PadLeft(32, '0')}";
            if (input == 0) return "".PadLeft(64, '0');
            StringBuilder b = new StringBuilder();
            while (input != 0)
            {
                b.Insert(0, ((input & 1) == 1) ? '1' : '0');
                input >>= 1;
            }
            return b.ToString().PadLeft(64, '0');
        }

        /// <summary>
        /// swap the bits using bit shift operations
        /// https://stackoverflow.com/a/19560621
        /// </summary>
        public static UInt16 SwapBytes(UInt16 x)
        {
            return (UInt16)((UInt16)((x & 0xff) << 8) | ((x >> 8) & 0xff));
        }

        /// <summary>
        /// swap the bits using bit shift operations
        /// https://stackoverflow.com/a/19560621
        /// </summary>
        public static UInt32 SwapBytes(UInt32 x)
        {
            // swap adjacent 16-bit blocks
            x = (x >> 16) | (x << 16);
            // swap adjacent 8-bit blocks
            return ((x & 0xFF00FF00) >> 8) | ((x & 0x00FF00FF) << 8);
        }

        /// <summary>
        /// swap the bits using bit shift operations
        /// https://stackoverflow.com/a/19560621
        /// </summary>
        public static UInt64 SwapBytes(UInt64 x)
        {
            // swap adjacent 32-bit blocks
            x = (x >> 32) | (x << 32);
            // swap adjacent 16-bit blocks
            x = ((x & 0xFFFF0000FFFF0000) >> 16) | ((x & 0x0000FFFF0000FFFF) << 16);
            // swap adjacent 8-bit blocks
            return ((x & 0xFF00FF00FF00FF00) >> 8) | ((x & 0x00FF00FF00FF00FF) << 8);
        }
    }

    /// <summary>
    /// comparison tool
    /// parse the value of the input query before comparing
    /// </summary>
    public class ComparerTool
    {
        /// <summary>scan type</summary>
        public ScanType scanType { get; }
        public CompareType compareType { get; }
        /// <summary>the length of the query value</summary>
        public int scanTypeLength { get; }
        /// <summary>the length of the first query value for group query</summary>
        public int groupFirstLength { get; }
        /// <summary>input value for ScanType:Hex、String</summary>
        public byte[] value0Byte { get; }
        /// <summary>Specifies the index position of wildcards when searching for hex</summary>
        public HashSet<int> value0ByteWildcards { get; }
        /// <summary>input value for comparison</summary>
        public ulong value0Long { get; }
        /// <summary>used for the second input value of compare type between</summary>
        public ulong value1Long { get; }
        /// <summary>input values for group query</summary>
        public List<byte[]> groupValues { get; }
        /// <summary>input types for group query</summary>
        public List<(ScanType scanType, int groupTypeLength, bool isAny)> groupTypes { get; }
        /// <summary>input value for comparison</summary>
        public string value0 { get; }
        /// <summary>used for the second input value of compare type between</summary>
        public string value1 { get; }
        /// <summary>whether to invert the comparison result</summary>
        public bool isNot { get; }
        /// <summary>whether to compare only simple values when the type is Floating-Point</summary>
        public bool isFloatingSimpleValues { get; }
        /// <summary>Determines whether to make the calculation result of Floating(float, double) completely exact in query window, there can be 0.0001 difference in the old mechanism. Default enabled</summary>
        public bool enableFloatingResultExact { get; }
        /// <summary>Determine the exponents value of the simple value of floating. Cheat Engine is set to 11 (2 to the 11th power = 2^11 = plus or minus 2048). Default value is 11</summary>
        public byte floatingSimpleValueExponents { get; }

        public UInt64 input0UInt64 { get; }
        public UInt64 input1UInt64 { get; }

        public UInt32 input0UInt32 { get; }
        public UInt32 input1UInt32 { get; }

        public UInt16 input0UInt16 { get; }
        public UInt16 input1UInt16 { get; }

        public byte input0Byte { get; }
        public byte input1Byte { get; }

        public double input0Double { get; }
        public double input1Double { get; }

        public float input0Float { get; }
        public float input1Float { get; }

        public ComparerTool(ScanType scanType, CompareType compareType, string value0, string value1, bool isHex, bool isNot, bool isFloatingSimpleValues, bool enableFloatingResultExact, byte floatingSimpleValueExponents)
        {
            this.scanType = scanType;
            this.compareType = compareType;
            this.value0 = value0;
            this.value1 = value1;
            this.isNot = isNot;
            this.isFloatingSimpleValues = isFloatingSimpleValues;
            this.enableFloatingResultExact = enableFloatingResultExact;
            this.floatingSimpleValueExponents = floatingSimpleValueExponents;

            switch (scanType)
            {
                case ScanType.Bytes_8:
                    input0UInt64 = (isHex && value0 != "0") ? BitConverter.ToUInt64(ScanTool.ValueStringToByte(ScanType.Hex, value0), 0) : ulong.Parse(value0.Replace(",","")); //Big-Endian
                    input1UInt64 = (isHex && value1 != "0") ? BitConverter.ToUInt64(ScanTool.ValueStringToByte(ScanType.Hex, value1), 0) : ulong.Parse(value1.Replace(",", ""));
                    //input0UInt64 = isHex ? ulong.Parse(value0, NumberStyles.HexNumber) : ulong.Parse(value0); //Little-Endian
                    //input1UInt64 = isHex ? ulong.Parse(value1, NumberStyles.HexNumber) : ulong.Parse(value1);
                    break;
                case ScanType.Bytes_4:
                    input0UInt32 = (isHex && value0 != "0") ? BitConverter.ToUInt32(ScanTool.ValueStringToByte(ScanType.Hex, value0), 0) : uint.Parse(value0.Replace(",", ""));
                    input1UInt32 = (isHex && value1 != "0") ? BitConverter.ToUInt32(ScanTool.ValueStringToByte(ScanType.Hex, value1), 0) : uint.Parse(value1.Replace(",", ""));
                    //input0UInt32 = isHex ? uint.Parse(value0, NumberStyles.HexNumber) : uint.Parse(value0);
                    //input1UInt32 = isHex ? uint.Parse(value1, NumberStyles.HexNumber) : uint.Parse(value1);
                    break;
                case ScanType.Bytes_2:
                    input0UInt16 = (isHex && value0 != "0") ? BitConverter.ToUInt16(ScanTool.ValueStringToByte(ScanType.Hex, value0), 0) : ushort.Parse(value0.Replace(",", ""));
                    input1UInt16 = (isHex && value1 != "0") ? BitConverter.ToUInt16(ScanTool.ValueStringToByte(ScanType.Hex, value1), 0) : ushort.Parse(value1.Replace(",", ""));
                    //input0UInt16 = isHex ? ushort.Parse(value0, NumberStyles.HexNumber) : ushort.Parse(value0);
                    //input1UInt16 = isHex ? ushort.Parse(value1, NumberStyles.HexNumber) : ushort.Parse(value1);
                    break;
                case ScanType.Byte_:
                    input0Byte = (isHex && value0 != "0") ? ScanTool.ValueStringToByte(ScanType.Hex, value0)[0] : byte.Parse(value0.Replace(",", ""));
                    input1Byte = (isHex && value1 != "0") ? ScanTool.ValueStringToByte(ScanType.Hex, value1)[1] : byte.Parse(value1.Replace(",", ""));
                    //input0Byte = isHex ? byte.Parse(value0, NumberStyles.HexNumber) : byte.Parse(value0);
                    //input1Byte = isHex ? byte.Parse(value1, NumberStyles.HexNumber) : byte.Parse(value1);
                    break;
                case ScanType.Double_:
                    input0Double = (isHex && value0 != "0") ? BitConverter.ToDouble(ScanTool.ValueStringToByte(ScanType.Hex, value0), 0) : double.Parse(value0.Replace(",", ""));
                    input1Double = (isHex && value1 != "0") ? BitConverter.ToDouble(ScanTool.ValueStringToByte(ScanType.Hex, value1), 0) : double.Parse(value1.Replace(",", ""));
                    break;
                case ScanType.Float_:
                    input0Float = (isHex && value0 != "0") ? BitConverter.ToSingle(ScanTool.ValueStringToByte(ScanType.Hex, value0), 0) : float.Parse(value0.Replace(",", ""));
                    input1Float = (isHex && value1 != "0") ? BitConverter.ToSingle(ScanTool.ValueStringToByte(ScanType.Hex, value1), 0) : float.Parse(value1.Replace(",", ""));
                    break;
            }

            if (scanType == ScanType.Group) (groupTypes, groupValues, groupFirstLength, scanTypeLength) = ScanTool.GenerateGroupList(value0);
            else if (!isHex && ScanTool.ScanTypeLengthDict.TryGetValue(scanType, out int scanTypeLength))
            {
                this.scanTypeLength = scanTypeLength;
                value0Long = ScanTool.ValueStringToULong(scanType, value0.Replace(",", ""));
                if (!string.IsNullOrWhiteSpace(value1)) value1Long = ScanTool.ValueStringToULong(scanType, value1.Replace(",", ""));
            }
            else
            { //for ScanType:Hex、String
                if (scanType == ScanType.Hex && !isHex)
                { //Convert decimal to hex format
                    string newValue0 = "";
                    string[] values = value0.Replace("-", " ").Replace("_", " ").Replace("?", "*").Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string val in values)
                    {
                        if (val == "*") newValue0 += "**";
                        else newValue0 += byte.Parse(val).ToString("X2");
                    }
                    this.value0 = value0 = newValue0;
                }
                if (scanType == ScanType.Hex && (value0.Contains("*") || value0.Contains("?")))
                { //Handling hex with wildcards
                    value0ByteWildcards = new HashSet<int>();

                    value0 = value0.Replace(" ", "").Replace("-", "").Replace("_", "").Replace("?", "*");
                    for (int idx = 0; idx < value0.Length / 2; idx++)
                    {
                        string str = value0.Substring(idx * 2, 2);
                        if (!str.Contains("*")) continue;
                        if (str != "**") throw new Exception("Search hex with wildcard format typo");
                        value0ByteWildcards.Add(idx);
                    }
                    value0 = value0.Replace("*", "0");
                }
                value0Byte = isHex ? ScanTool.ValueStringToByte(ScanType.Hex, value0) : ScanTool.ValueStringToByte(scanType, value0);
                this.scanTypeLength = value0Byte.Length;
            }
        }
    }
}
