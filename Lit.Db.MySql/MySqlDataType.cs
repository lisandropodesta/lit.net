using System;
using System.Text;
using Lit.DataType;
using Lit.Db.Attributes;
using Lit.Db.Model;

namespace Lit.Db.MySql
{
    public static class MySqlDataType
    {
        #region Constants

        // Boolean

        public const string BooleanDataType = @"BOOLEAN";

        // Integer

        public const string Int8DataType = @"TINYINT";

        public const string Int16DataType = @"SMALLINT";

        public const string Int24DataType = @"MEDIUMINT";

        public const string Int32DataType = @"INT";

        public const string Int64DataType = @"BIGINT";

        public const string DecimalDataType = @"DECIMAL";

        // Floating point

        public const string FloatDataType = @"FLOAT";

        public const string DoubleDataType = @"DOUBLE";

        // Date and time

        public const string TimeDataType = @"TIME";

        public const string DateDataType = @"DATE";

        public const string DateTimeDataType = @"DATETIME";

        public const string TimestampDataType = @"TIMESTAMP";

        // Text

        public const string CharDataType = @"CHAR";

        public const string StringDataType = @"VARCHAR";

        public const string TinyTextDataType = @"TINYTEXT";

        public const string TextDataType = "@TEXT";

        public const string MediumTextDataType = @"MEDIUMTEXT";

        public const string LongTextDataType = "@LONGTEXT";

        // Blob

        public const string TinyBlobDataType = @"TINYBLOB";

        public const string BlobDataType = "@BLOB";

        public const string MediumBlobDataType = @"MEDIUMBLOB";

        public const string LongBlobDataType = "@LONGBLOB";

        // Objects / Json

        public const string JsonDataType = @"JSON";

        // Enumerated

        public const string EnumType = @"ENUM";

        // Text/blob sizes

        public const uint Size256B = 256;

        public const uint Size64KB = 65336;

        public const uint Size16MB = 16777216;

        public const ulong Size4GB = 4294967296;

        /// <summary>
        /// Size qualifier.
        /// </summary>
        public enum SizeQualifier
        {
            Default,

            LessThan256B,

            LessThan64KB,

            LessThan16MB,

            LessThan4GB
        }

        #endregion

        /// <summary>
        /// Translates a native value to a MySql value.
        /// </summary>
        public static object TranslateToDb(DbDataType dataType, Type type, object value)
        {
            switch (dataType)
            {
                case DbDataType.Unknown:
                    return DBNull.Value;

                case DbDataType.Boolean:
                case DbDataType.Char:
                case DbDataType.UInt8:
                case DbDataType.SInt8:
                case DbDataType.UInt16:
                case DbDataType.SInt16:
                case DbDataType.UInt24:
                case DbDataType.SInt24:
                case DbDataType.UInt32:
                case DbDataType.SInt32:
                case DbDataType.UInt64:
                case DbDataType.SInt64:
                case DbDataType.Decimal:
                case DbDataType.Float:
                case DbDataType.Double:
                case DbDataType.Text:
                case DbDataType.Blob:
                case DbDataType.Enumerated:
                    return DbTranslation.ScalarToDb(type, value);

                case DbDataType.DateTime:
                    return value;

                case DbDataType.Timestamp:
                    return ((DateTimeOffset)value).DateTime;

                case DbDataType.TimeSpan:
                    return ((TimeSpan)value).Ticks;

                case DbDataType.Time:
                case DbDataType.Date:
                case DbDataType.Json:
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Translates a MySql value to a native value.
        /// </summary>
        public static object TranslateFromDb(DbDataType dataType, Type type, object value)
        {
            switch (dataType)
            {
                case DbDataType.Unknown:
                case DbDataType.Boolean:
                case DbDataType.Char:
                case DbDataType.UInt8:
                case DbDataType.SInt8:
                case DbDataType.UInt16:
                case DbDataType.SInt16:
                case DbDataType.UInt24:
                case DbDataType.SInt24:
                case DbDataType.UInt32:
                case DbDataType.SInt32:
                case DbDataType.UInt64:
                case DbDataType.SInt64:
                case DbDataType.Decimal:
                case DbDataType.Float:
                case DbDataType.Double:
                case DbDataType.Text:
                case DbDataType.Blob:
                case DbDataType.Enumerated:
                    return DbTranslation.ScalarFromDb(type, value);

                case DbDataType.DateTime:
                    return value;

                case DbDataType.Timestamp:
                    return new DateTimeOffset((DateTime)value);

                case DbDataType.TimeSpan:
                    return new TimeSpan((long)value);

                case DbDataType.Json:
                    return value;

                case DbDataType.Time:
                case DbDataType.Date:
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Translates a db data type to a MySql data type.
        /// </summary>
        public static string Translate(DbDataType dataType, Type type = null, ulong? size = null, int? precision = null)
        {
            switch (dataType)
            {
                case DbDataType.Unknown:
                    break;

                case DbDataType.Boolean:
                    return BooleanDataType;

                case DbDataType.Char:
                    return $"{CharDataType}(1)";

                case DbDataType.UInt8:
                case DbDataType.SInt8:
                    return Int8DataType;

                case DbDataType.UInt16:
                case DbDataType.SInt16:
                    return Int16DataType;

                case DbDataType.UInt24:
                case DbDataType.SInt24:
                    return Int24DataType;

                case DbDataType.UInt32:
                case DbDataType.SInt32:
                    return Int32DataType;

                case DbDataType.UInt64:
                case DbDataType.SInt64:
                    return Int64DataType;

                case DbDataType.Decimal:
                    if (size == null || precision == null)
                    {
                        throw new ArgumentException("It is necessary to specify size and precision for a DECIMAL type.");
                    }
                    return $"{DecimalDataType}({size.Value},{precision.Value})";

                case DbDataType.Float:
                    return FloatDataType;

                case DbDataType.Double:
                    return DoubleDataType;

                case DbDataType.Time:
                    return TimeDataType;

                case DbDataType.Date:
                    return DateDataType;

                case DbDataType.DateTime:
                    return DateTimeDataType;

                case DbDataType.Timestamp:
                    return DateTimeDataType;

                case DbDataType.TimeSpan:
                    return Int64DataType;

                case DbDataType.Text:
                    return $"{GetTextDataType(size)}({size ?? 255})";

                case DbDataType.Blob:
                    return GetBlobDataType(size);

                case DbDataType.Enumerated:
                    return GetEnumList(type);

                case DbDataType.Json:
                    return JsonDataType;
            }

            throw new ArgumentException($"Type [{dataType}] not supported.");
        }

        private static string GetEnumList(Type enumType)
        {
            return $"{EnumType}({GetEnumValues(enumType)})";
        }

        private static string GetEnumValues(Type enumType)
        {
            var text = new StringBuilder();
            var any = false;
            var anyAttr = false;

            foreach (var fieldInfo in enumType.GetFields())
            {
                string code;
                if (TypeHelper.GetAttribute<DbEnumCodeAttribute>(fieldInfo, out var dbCodeAttr))
                {
                    if (!anyAttr)
                    {
                        anyAttr = true;
                        text.Clear();
                        any = false;
                    }

                    code = dbCodeAttr.Code;
                }
                else if (!anyAttr)
                {
                    code = fieldInfo.Name;
                }
                else
                {
                    continue;
                }

                if (any)
                {
                    text.Append(",");
                }

                text.Append("'" + code + "'");
                any = true;
            }

            return text.ToString();
        }

        private static string GetTextDataType(ulong? size)
        {
            switch (CheckSize(size))
            {
                case SizeQualifier.Default:
                default:
                    return StringDataType;

                case SizeQualifier.LessThan256B:
                    return TinyTextDataType;

                case SizeQualifier.LessThan64KB:
                    return TextDataType;

                case SizeQualifier.LessThan16MB:
                    return MediumTextDataType;

                case SizeQualifier.LessThan4GB:
                    return LongTextDataType;
            }
        }

        private static string GetBlobDataType(ulong? size)
        {
            switch (CheckSize(size))
            {
                case SizeQualifier.LessThan256B:
                    return TinyBlobDataType;

                default:
                case SizeQualifier.Default:
                case SizeQualifier.LessThan64KB:
                    return BlobDataType;

                case SizeQualifier.LessThan16MB:
                    return MediumBlobDataType;

                case SizeQualifier.LessThan4GB:
                    return LongBlobDataType;
            }
        }

        private static SizeQualifier CheckSize(ulong? size)
        {
            if (size == null)
            {
                return SizeQualifier.Default;
            }

            if (size < Size256B)
            {
                return SizeQualifier.LessThan256B;
            }

            if (size < Size64KB)
            {
                return SizeQualifier.LessThan64KB;
            }

            if (size < Size16MB)
            {
                return SizeQualifier.LessThan16MB;
            }

            if (size < Size4GB)
            {
                return SizeQualifier.LessThan4GB;
            }

            throw new ArgumentException($"Invalid text/blob size [{size}]");
        }
    }
}
