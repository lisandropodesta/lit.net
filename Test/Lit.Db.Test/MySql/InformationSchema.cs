using Lit.Auditing;
using Lit.Db.MySql.Schema.Information.Queries;

namespace Lit.Db.Test.MySql
{
    public static class InformationSchema
    {
        public static void Load(IDbHost db)
        {
            Audit.Message("\n  ** TABLES QUERY **");
            var data = new SchemaTables("wikialgorithm").Exec(db);

            foreach (var r in data.Tables)
            {
                Audit.Message($"  Engine={r.Engine}, Table={r.TableName}, Type={r.TableType}, RowFormat={r.RowFormat}");
            }

            Audit.Message("\n  ** COLUMNS QUERY **");

            var columnsData = new SchemaColumns("wikialgorithm").Exec(db);

            foreach (var c in columnsData.ColumnsList)
            {
                Audit.Message($"  Table={c.TableName}, Column={c.ColumnName}, DataType={c.DataType}, ColumnType={c.ColumnType}, Nullable={c.IsNullable}, Key={c.ColumnKey}");
            }
        }
    }
}
