using System;

namespace Lit.Db.MySql
{
    public class MySqlDefaultTranslation : DbTranslation
    {
        public override object ToDb(IDbBindingCache bindingCache, DbDataType dataType, Type type, object value)
        {
            return MySqlDataType.TranslateToDb(dataType, type, value);
        }

        public override object FromDb(IDbBindingCache bindingCache, DbDataType dataType, Type type, object value)
        {
            return MySqlDataType.TranslateFromDb(dataType, type, value);
        }
    }
}
