using System;
using System.Collections.Generic;
using Lit.Db.Attributes;
using Lit.Db.MySql;

namespace Lit.Db.Test
{
    public static class MySqlTest
    {
        public enum Engine
        {
            [DbEnumCode("InnoDB")]
            InnoDb,

            [DbEnumCode("MRG_MYISAM")]
            MrgMyISAM,

            [DbEnumCode("MEMORY")]
            Memory,

            [DbEnumCode("BLACKHOLE")]
            BlacHole,

            [DbEnumCode("MyISAM")]
            MyISAM,

            [DbEnumCode("CSV")]
            CSV,

            [DbEnumCode("ARCHIVE")]
            Archive,

            [DbEnumCode("PERFORMANCE_SCHEMA")]
            PerformanceSchema,

            [DbEnumCode("FEDERATED")]
            Federated
        }

        public class SchemaTable
        {
            [DbField]
            public string TableName { get; protected set; }

            [DbField]
            public Engine Engine { get; protected set; }
        }

        public class SchemaQuery
        {
            [DbParameter]
            public string TableSchema { get; set; }

            [DbRecordset]
            public List<SchemaTable> Tables { get; protected set; }
        }

        public static void Execute(MySqlHost db)
        {
            var query = "select * from INFORMATION_SCHEMA.TABLES where table_schema = '{{@table_schema}}'";

            var data = db.ExecuteQuery<SchemaQuery>(query, p =>
             {
                 p.TableSchema = "wikialgorithm";
             });

            foreach (var r in data.Tables)
            {
                Console.WriteLine($"Engine={r.Engine}, Table={r.TableName}");
            }
        }
    }
}
