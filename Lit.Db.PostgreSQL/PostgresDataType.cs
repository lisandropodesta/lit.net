using System;
using Newtonsoft.Json;

namespace Lit.Db.PostgreSQL
{
    public static class PostgresDataType
    {
        #region Constants

        // Boolean

        public const string BooleanDataType = @"BOOLEAN";

        // Integer

        public const string Int16DataType = @"SMALLINT";

        public const string Int32DataType = @"INTEGER";

        public const string Int64DataType = @"BIGINT";

        public const string DecimalDataType = @"NUMERIC";

        // Floating point

        public const string FloatDataType = @"REAL";

        public const string DoubleDataType = @"DOUBLE PRECISION";

        // Date and time

        public const string TimeDataType = @"TIME";

        public const string DateDataType = @"DATE";

        public const string DateTimeDataType = @"TIMESTAMP";

        public const string TimestampDataType = @"TIMESTAMP WITH TIME ZONE";

        // Text

        public const string CharDataType = @"CHAR";

        public const string StringDataType = @"VARCHAR";

        public const string TextDataType = @"TEXT";

        // Blob

        public const string BlobDataType = @"BYTEA";

        // Objects / Json

        public const string JsonDataType = @"JSONB";

        // Text sizes

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
        /// Translates a native value to a PostgreSQL value.
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

                case DbDataType.Records:
                case DbDataType.Json:
                    return JsonConvert.SerializeObject(value, Formatting.None);

                case DbDataType.Time:
                case DbDataType.Date:
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Translates a PostgreSQL value to a native value.
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

                case DbDataType.Records:
                case DbDataType.Json:
                    return JsonConvert.DeserializeObject((string)value, type);

                case DbDataType.Time:
                case DbDataType.Date:
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Translates a db data type to a PostgreSQL data type.
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
                    return Int16DataType;

                case DbDataType.UInt16:
                case DbDataType.SInt16:
                    return Int16DataType;

                case DbDataType.UInt24:
                case DbDataType.SInt24:
                    return Int32DataType;

                case DbDataType.UInt32:
                case DbDataType.SInt32:
                    return Int32DataType;

                case DbDataType.UInt64:
                case DbDataType.SInt64:
                    return Int64DataType;

                case DbDataType.Decimal:
                    if (size == null || precision == null)
                    {
                        throw new ArgumentException("It is necessary to specify size and precision for a NUMERIC type.");
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
                    return TimestampDataType;

                case DbDataType.TimeSpan:
                    return Int64DataType;

                case DbDataType.Text:
                    return GetTextDataType(size);

                case DbDataType.Blob:
                    return BlobDataType;

                case DbDataType.Enumerated:
                    return StringDataType;

                case DbDataType.Records:
                case DbDataType.Json:
                    return JsonDataType;
            }

            throw new ArgumentException($"Type [{dataType}] not supported.");
        }

        private static string GetTextDataType(ulong? size)
        {
            if (!size.HasValue || size.Value > 65335)
            {
                return TextDataType;
            }

            return $"{StringDataType}({size.Value})";
        }
    }
}
