using System;

namespace Lit.Db.PostgreSQL
{
    public class PostgresDefaultTranslation : DbTranslation
    {
        public override object ToDb(IDbBindingCache bindingCache, DbDataType dataType, Type type, object value)
        {
            return PostgresDataType.TranslateToDb(dataType, type, value);
        }

        public override object FromDb(IDbBindingCache bindingCache, DbDataType dataType, Type type, object value)
        {
            return PostgresDataType.TranslateFromDb(dataType, type, value);
        }
    }
}
