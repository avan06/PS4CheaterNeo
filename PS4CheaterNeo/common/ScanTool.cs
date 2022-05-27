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

        public static bool ComparerExact(ScanType scanType, byte[] newValue, byte[] inputValue0)
        {
            bool result;
            switch (scanType)
            {
                case ScanType.Bytes_8:
                case ScanType.Bytes_4:
                case ScanType.Bytes_2:
                case ScanType.Byte_:
                case ScanType.Hex:
                case ScanType.String_:
                case ScanType.Group:
                    if (inputValue0.Length != newValue.Length) throw new ArgumentException("Comparer String length verification failed");
                    for (int i = 0; i < inputValue0.Length; ++i) if (inputValue0[i] != newValue[i]) return false;
                    result = true;
                    break;
                case ScanType.Double_:
                    var newDouble = BitConverter.ToDouble(newValue, 0);
                    var input0Double = BitConverter.ToDouble(inputValue0, 0);
                    result = Math.Abs(newDouble - input0Double) < 1;
                    break;
                case ScanType.Float_:
                    var newFloat = BitConverter.ToSingle(newValue, 0);
                    var input0Float = BitConverter.ToSingle(inputValue0, 0);
                    result = Math.Abs(newFloat - input0Float) < 1;
                    break;
                default:
                    throw new Exception("Unknown scanType type.");
            }

            return result;
        }

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
                        result = Math.Abs(newDouble - comparerTool.input0Double) < 0.0001;
                        break;
                    case ScanType.Float_:
                        result = Math.Abs(newFloat - comparerTool.input0Float) < 0.0001;
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
                        result = Math.Abs(newDouble - oldDouble - comparerTool.input0Double) < 0.0001;
                        break;
                    case ScanType.Float_:
                        result = Math.Abs(newFloat - oldFloat - comparerTool.input0Float) < 0.0001;
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
                        result = Math.Abs(newDouble - oldDouble + comparerTool.input0Double) < 0.0001;
                        break;
                    case ScanType.Float_:
                        result = Math.Abs(newFloat - oldFloat + comparerTool.input0Float) < 0.0001;
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
                        result = Math.Abs(newDouble - oldDouble) >= 0.0001;
                        break;
                    case ScanType.Float_:
                        result = Math.Abs(newFloat - oldFloat) >= 0.0001;
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
                        result = Math.Abs(newDouble - oldDouble) < 0.0001;
                        break;
                    case ScanType.Float_:
                        result = Math.Abs(newFloat - oldFloat) < 0.0001;
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
    }

    public class ComparerTool
    {
        public ScanType scanType { get; }
        public CompareType compareType { get; }
        public int scanTypeLength { get; }
        public int groupFirstLength { get; }
        public byte[] value0Byte { get; } //for ScanType:Hex、String
        public ulong value0Long { get; }
        public ulong value1Long { get; }
        public List<byte[]> groupValues { get; }
        public List<(ScanType scanType, int groupTypeLength, bool isAny)> groupTypes { get; }

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

        public ComparerTool(ScanType scanType, CompareType compareType, string value0, string value1)
        {
            this.scanType = scanType;
            this.compareType = compareType;

            switch (scanType)
            {
                case ScanType.Bytes_8:
                    input0UInt64 = Convert.ToUInt64(value0);
                    input1UInt64 = Convert.ToUInt64(value1);
                    break;
                case ScanType.Bytes_4:
                    input0UInt32 = Convert.ToUInt32(value0);
                    input1UInt32 = Convert.ToUInt32(value1);
                    break;
                case ScanType.Bytes_2:
                    input0UInt16 = Convert.ToUInt16(value0);
                    input1UInt16 = Convert.ToUInt16(value1);
                    break;
                case ScanType.Byte_:
                    input0Byte = Convert.ToByte(value0);
                    input1Byte = Convert.ToByte(value1);
                    break;
                case ScanType.Double_:
                    input0Double = Convert.ToDouble(value0);
                    input1Double = Convert.ToDouble(value1);
                    break;
                case ScanType.Float_:
                    input0Float = Convert.ToSingle(value0);
                    input1Float = Convert.ToSingle(value1);
                    break;
            }

            if (scanType == ScanType.Group) (groupTypes, groupValues, groupFirstLength, scanTypeLength) = ScanTool.GenerateGroupList(value0);
            else if (ScanTool.ScanTypeLengthDict.TryGetValue(scanType, out int scanTypeLength))
            {
                this.scanTypeLength = scanTypeLength;
                value0Long = ScanTool.ValueStringToULong(scanType, value0);
                if (!string.IsNullOrWhiteSpace(value1)) value1Long = ScanTool.ValueStringToULong(scanType, value1);
            }
            else
            { //for ScanType:Hex、String
                value0Byte = ScanTool.ValueStringToByte(scanType, value0);
                this.scanTypeLength = value0Byte.Length;
            }
        }
    }
}
