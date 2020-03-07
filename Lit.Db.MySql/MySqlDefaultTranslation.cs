using System;
using Lit.Db.Model;

namespace Lit.Db.MySql
{
    public class MySqlDefaultTranslation : DbTranslation
    {
        public override object ToDb(DbDataType dataType, Type type, object value)
        {
            return MySqlDataType.TranslateToDb(dataType, type, value);
        }

        public override object FromDb(DbDataType dataType, Type type, object value)
        {
            return MySqlDataType.TranslateFromDb(dataType, type, value);
        }
    }
}
