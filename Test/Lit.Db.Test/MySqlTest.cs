using System;
using Lit.Db.MySql;
using Lit.Db.MySql.Schema.Information.Queries;

namespace Lit.Db.Test
{
    public static class MySqlTest
    {
        public static void Execute(MySqlHost db)
        {
            Console.WriteLine("\n*** TABLES QUERY ***");
            var data = new SchemaTables(db, "wikialgorithm");

            foreach (var r in data.Tables)
            {
                Console.WriteLine($"Engine={r.Engine}, Table={r.TableName}, Type={r.TableType}, RowFormat={r.RowFormat}");
            }

            Console.WriteLine("\n*** COLUMNS QUERY ***");

            var columnsData = new SchemaColumns(db, "wikialgorithm");

            foreach (var c in columnsData.Columns)
            {
                Console.WriteLine($"Table={c.TableName}, Column={c.ColumnName}, DataTye={c.DataType}, ColumnType={c.ColumnType}, Nullable={c.IsNullable}, Key={c.ColumnKey}");
            }
        }
    }
}
