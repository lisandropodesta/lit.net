using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Reflection;
using Lit.DataType;

namespace Lit.Db.Sql
{
    public static class SqlHelper
    {
        /// <summary>
        /// Creates a table.
        /// </summary>
        public static DataTable CreateTable(IDbBindingCache bindingCache, object data)
        {
            if (data != null)
            {
                var type = data.GetType();
                switch (TypeHelper.GetBindingMode(ref type, out _))
                {
                    case BindingMode.Class:
                        {
                            var bindingList = bindingCache.GetParametersBinding(type);
                            var table = CreateTable(bindingList);
                            AddRecord(table, bindingList, data);
                            return table;
                        }

                    case BindingMode.ClassList:
                        {
                            var bindingList = bindingCache.GetParametersBinding(type);
                            var table = CreateTable(bindingList);
                            var mi = typeof(SqlHelper).GetMethod(nameof(AddRecords), BindingFlags.Static | BindingFlags.NonPublic);
                            mi.MakeGenericMethod(type).Invoke(null, new[] { table, bindingList, data });
                            return table;
                        }
                }
            }

            throw new NotImplementedException();
        }

        private static DataTable CreateTable(IEnumerable<IDbParameterBinding> bindingList)
        {
            var table = new DataTable();

            foreach (var b in bindingList)
            {
                var type = b.BindingType;
                var column = new DataColumn(b.PropertyName, type);
                table.Columns.Add(column);
            }

            return table;
        }

        private static void AddRecords<T>(DataTable table, IEnumerable<IDbParameterBinding> bindingList, IEnumerable<T> instances)
        {
            foreach (var instance in instances)
            {
                AddRecord(table, bindingList, instance);
            }
        }

        private static void AddRecord(DataTable table, IEnumerable<IDbParameterBinding> bindingList, object instance)
        {
            var row = table.NewRow();
            var col = 0;
            foreach (var b in bindingList)
            {
                row[col++] = b.GetValue(instance);
            }

            table.Rows.Add(row);
        }
    }
}
