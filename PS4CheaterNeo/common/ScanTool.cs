using libdebug;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
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
                    bytes = new byte[value.Length / 2];
                    for (int idx = 0; idx < bytes.Length; idx++)
                        bytes[idx] = Convert.ToByte(value.Substring(idx * 2, 2), 16);
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

        public static ulong BytesToULong(ScanType scanType, ref byte[] bytes)
        {
            if (bytes.Length < 8) Array.Resize(ref bytes, 8);
            ulong valueUlong = BitConverter.ToUInt64(bytes, 0);
            return valueUlong;
        }

        public static ulong ValueStringToULong(ScanType scanType, string value)
        {
            byte[] bytes = ValueStringToByte(scanType, value);
            ulong valueUlong = BytesToULong(scanType, ref bytes);

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

        public static bool Comparer(ScanType scanType, CompareType compareType, ulong newValue, ulong oldValue, ulong inputValue0, ulong inputValue1 = 0)
        {
            bool result = false;

            UInt64 newUInt64 = 0;
            UInt64 oldUInt64 = 0;
            UInt64 input0UInt64 = 0;
            UInt64 input1UInt64 = 0;

            UInt32 newUInt32 = 0;
            UInt32 oldUInt32 = 0;
            UInt32 input0UInt32 = 0;
            UInt32 input1UInt32 = 0;

            UInt16 newUInt16 = 0;
            UInt16 oldUInt16 = 0;
            UInt16 input0UInt16 = 0;
            UInt16 input1UInt16 = 0;

            byte newByte = 0;
            byte oldByte = 0;
            byte input0Byte = 0;
            byte input1Byte = 0;

            double newDouble = 0;
            double oldDouble = 0;
            double input0Double = 0;
            double input1Double = 0;

            float newFloat = 0;
            float oldFloat = 0;
            float input0Float = 0;
            float input1Float = 0;

            switch (scanType)
            {
                case ScanType.Bytes_8:
                    newUInt64 = newValue;
                    oldUInt64 = oldValue;
                    input0UInt64 = inputValue0;
                    input1UInt64 = inputValue1;
                    break;
                case ScanType.Bytes_4:
                    newUInt32 = (UInt32)newValue;
                    oldUInt32 = (UInt32)oldValue;
                    input0UInt32 = (UInt32)inputValue0;
                    input1UInt32 = (UInt32)inputValue1;
                    break;
                case ScanType.Bytes_2:
                    newUInt16 = (UInt16)newValue;
                    oldUInt16 = (UInt16)oldValue;
                    input0UInt16 = (UInt16)inputValue0;
                    input1UInt16 = (UInt16)inputValue1;
                    break;
                case ScanType.Byte_:
                    newByte = BitConverter.GetBytes(newValue)[0];
                    oldByte = BitConverter.GetBytes(oldValue)[0];
                    input0Byte = BitConverter.GetBytes(inputValue0)[0];
                    input1Byte = BitConverter.GetBytes(inputValue1)[0];
                    break;
                case ScanType.Double_:
                    newDouble = BitConverter.ToDouble(BitConverter.GetBytes(newValue), 0);
                    oldDouble = BitConverter.ToDouble(BitConverter.GetBytes(oldValue), 0);
                    input0Double = BitConverter.ToDouble(BitConverter.GetBytes(inputValue0), 0);
                    input1Double = BitConverter.ToDouble(BitConverter.GetBytes(inputValue1), 0);
                    break;
                case ScanType.Float_:
                    newFloat = BitConverter.ToSingle(BitConverter.GetBytes(newValue), 0);
                    oldFloat = BitConverter.ToSingle(BitConverter.GetBytes(oldValue), 0);
                    input0Float = BitConverter.ToSingle(BitConverter.GetBytes(inputValue0), 0);
                    input1Float = BitConverter.ToSingle(BitConverter.GetBytes(inputValue1), 0);
                    break;
                default:
                    throw new Exception("Unknown scanType type.");
            }

            if (compareType == CompareType.Exact)
            {
                switch (scanType)
                {
                    case ScanType.Bytes_8:
                        result = newUInt64 == input0UInt64;
                        break;
                    case ScanType.Bytes_4:
                        result = newUInt32 == input0UInt32;
                        break;
                    case ScanType.Bytes_2:
                        result = newUInt16 == input0UInt16;
                        break;
                    case ScanType.Byte_:
                        result = newByte == input0Byte;
                        break;
                    case ScanType.Double_:
                        result = Math.Abs(newDouble - input0Double) < 0.0001;
                        break;
                    case ScanType.Float_:
                        result = Math.Abs(newFloat - input0Float) < 0.0001;
                        break;
                    default:
                        throw new Exception("Unknown scanType type.");
                }
            }
            else if (compareType == CompareType.Fuzzy)
            {
                switch (scanType)
                {
                    case ScanType.Double_:
                        result = Math.Abs(newDouble - input0Double) < 1;
                        break;
                    case ScanType.Float_:
                        result = Math.Abs(newFloat - input0Float) < 1;
                        break;
                    default:
                        throw new Exception("Unknown scanType type.");
                }
            }
            else if (compareType == CompareType.Increased)
            {
                switch (scanType)
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
            else if (compareType == CompareType.IncreasedBy)
            {
                switch (scanType)
                {
                    case ScanType.Bytes_8:
                        result = newUInt64 == oldUInt64 + input0UInt64;
                        break;
                    case ScanType.Bytes_4:
                        result = newUInt32 == oldUInt32 + input0UInt32;
                        break;
                    case ScanType.Bytes_2:
                        result = newUInt16 == oldUInt16 + input0UInt16;
                        break;
                    case ScanType.Byte_:
                        result = newByte == oldByte + input0Byte;
                        break;
                    case ScanType.Double_:
                        result = Math.Abs(newDouble - oldDouble - input0Double) < 0.0001;
                        break;
                    case ScanType.Float_:
                        result = Math.Abs(newFloat - oldFloat - input0Float) < 0.0001;
                        break;
                    default:
                        throw new Exception("Unknown scanType type.");
                }
            }
            else if (compareType == CompareType.Decreased)
            {
                switch (scanType)
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
            else if (compareType == CompareType.DecreasedBy)
            {
                switch (scanType)
                {
                    case ScanType.Bytes_8:
                        result = newUInt64 == oldUInt64 - input0UInt64;
                        break;
                    case ScanType.Bytes_4:
                        result = newUInt32 == oldUInt32 - input0UInt32;
                        break;
                    case ScanType.Bytes_2:
                        result = newUInt16 == oldUInt16 - input0UInt16;
                        break;
                    case ScanType.Byte_:
                        result = newByte == oldByte - input0Byte;
                        break;
                    case ScanType.Double_:
                        result = Math.Abs(newDouble - oldDouble + input0Double) < 0.0001;
                        break;
                    case ScanType.Float_:
                        result = Math.Abs(newFloat - oldFloat + input0Float) < 0.0001;
                        break;
                    default:
                        throw new Exception("Unknown scanType type.");
                }
            }
            else if (compareType == CompareType.BiggerThan)
            {
                switch (scanType)
                {
                    case ScanType.Bytes_8:
                        result = newUInt64 > input0UInt64;
                        break;
                    case ScanType.Bytes_4:
                        result = newUInt32 > input0UInt32;
                        break;
                    case ScanType.Bytes_2:
                        result = newUInt16 > input0UInt16;
                        break;
                    case ScanType.Byte_:
                        result = newByte > input0Byte;
                        break;
                    case ScanType.Double_:
                        result = newDouble > input0Double;
                        break;
                    case ScanType.Float_:
                        result = newFloat > input0Float;
                        break;
                    default:
                        throw new Exception("Unknown scanType type.");
                }
            }
            else if (compareType == CompareType.SmallerThan)
            {
                switch (scanType)
                {
                    case ScanType.Bytes_8:
                        result = newUInt64 < input0UInt64;
                        break;
                    case ScanType.Bytes_4:
                        result = newUInt32 < input0UInt32;
                        break;
                    case ScanType.Bytes_2:
                        result = newUInt16 < input0UInt16;
                        break;
                    case ScanType.Byte_:
                        result = newByte < input0Byte;
                        break;
                    case ScanType.Double_:
                        result = newDouble < input0Double;
                        break;
                    case ScanType.Float_:
                        result = newFloat < input0Float;
                        break;
                    default:
                        throw new Exception("Unknown scanType type.");
                }
            }
            else if (compareType == CompareType.Changed)
            {
                switch (scanType)
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
            else if (compareType == CompareType.Unchanged)
            {
                switch (scanType)
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
            else if (compareType == CompareType.Between)
            {
                switch (scanType)
                {
                    case ScanType.Bytes_8:
                        result = newUInt64 <= input1UInt64 && newUInt64 >= input0UInt64;
                        break;
                    case ScanType.Bytes_4:
                        result = newUInt32 <= input1UInt32 && newUInt32 >= input0UInt32;
                        break;
                    case ScanType.Bytes_2:
                        result = newUInt16 <= input1UInt16 && newUInt16 >= input0UInt16;
                        break;
                    case ScanType.Byte_:
                        result = newByte <= input1Byte && newByte >= input0Byte;
                        break;
                    case ScanType.Double_:
                        result = newDouble <= input1Double && newDouble >= input0Double;
                        break;
                    case ScanType.Float_:
                        result = newFloat <= input1Float && newFloat >= input0Float;
                        break;
                    default:
                        throw new Exception("Unknown scanType type.");
                }
            }
            else if (compareType == CompareType.UnknownInitial)
            {
                switch (scanType)
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

        public static bool Comparer(ScanType scanType, byte[] newValue, byte[] inputValue0)
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
            foreach (FieldInfo field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description) return (T)field.GetValue(null);
                }
                else if (field.Name == description) return (T)field.GetValue(null);
            }

            throw new ArgumentException("ParseFromDescription Not found.", string.Format("{0}({1})", nameof(description), description));
        }
    }
}
