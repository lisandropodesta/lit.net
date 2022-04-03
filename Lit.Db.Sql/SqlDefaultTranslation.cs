using System;

namespace Lit.Db.Sql
{
    public class SqlDefaultTranslation : DbTranslation
    {
        public override object ToDb(IDbBindingCache bindingCache, DbDataType dataType, Type type, object value)
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
                case DbDataType.Time:
                case DbDataType.Date:
                case DbDataType.DateTime:
                case DbDataType.Timestamp:
                case DbDataType.TimeSpan:
                case DbDataType.Text:
                case DbDataType.Blob:
                case DbDataType.Enumerated:
                    return ScalarToDb(type, value);

                case DbDataType.Records:
                    return SqlHelper.CreateTable(bindingCache, value);

                case DbDataType.Json:
                default:
                    throw new NotImplementedException();
            }
        }

        public override object FromDb(IDbBindingCache bindingCache, DbDataType dataType, Type type, object value)
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
                case DbDataType.Time:
                case DbDataType.Date:
                case DbDataType.DateTime:
                case DbDataType.Timestamp:
                case DbDataType.TimeSpan:
                case DbDataType.Text:
                case DbDataType.Blob:
                case DbDataType.Enumerated:
                    return ScalarFromDb(type, value);

                case DbDataType.Json:
                    break;

                case DbDataType.Records:
                default:
                    throw new NotImplementedException();
            }

            return value;
        }
    }
}
