using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Lit.Db.Framework;

namespace Lit.Db
{
    public static class DbBindingExtension
    {
        #region IDbTableBinding general

        /// <summary>
        /// Gets a table name.
        /// </summary>
        public static string GetSqlTableName(this IDbTableBinding table)
        {
            return table.Setup.Naming.GetSqlTableName(table.TableName);
        }

        /// <summary>
        /// Get table stored procedure name.
        /// </summary>
        public static string GetTableSpName(this IDbTableBinding binding, StoredProcedureFunction spFunc)
        {
            return binding.Setup.Naming.GetStoredProcedureName(binding.TableName, spFunc);
        }

        /// <summary>
        /// Get table stored procedure name as needed to be included in a SQL sentence.
        /// </summary>
        public static string GetSqlTableSpName(this IDbTableBinding binding, StoredProcedureFunction spFunc)
        {
            var spName = binding.GetTableSpName(spFunc);
            return binding.Setup.Naming.GetSqlSpName(spName);
        }
        /// <summary>
        /// Check constraints consistency.
        /// </summary>
        public static void CheckConstraints(this IDbTableBinding binding)
        {
            binding.ForeignKeys?.ForEach(fk => binding.CheckForeignKey(fk));
        }

        /// <summary>
        /// Check foreign key validity.
        /// </summary>
        private static void CheckForeignKey(this IDbTableBinding binding, IDbTableForeignKeyAttribute fk)
        {
            var binding2 = binding.Setup.GetTableBinding(fk.PrimaryTableTemplate);

            if (binding2.PrimaryKey == null)
            {
                throw new ArgumentException($"Invalid foreign key definition in table [{binding.TableName}], remote table has no primary key");
            }

            if (binding2.PrimaryKey.PropertyNames.Length != fk.PropertyNames.Length)
            {
                throw new ArgumentException($"Invalid foreign key definition in table [{binding.TableName}], local fields count [{fk.PropertyNames.Length}] do not match with foreign fields count [{binding2.PrimaryKey.PropertyNames.Length}]");
            }
        }

        /// <summary>
        /// Check if a table has columns of a certain type.
        /// </summary>
        public static bool HasColumns(this IDbTableBinding binding, DbColumnsSelection selection)
        {
            return binding.Columns.Any(c => IsSelected(binding, c, selection));
        }

        /// <summary>
        /// Get a column binding.
        /// </summary>
        public static IDbColumnBinding GetColumn(this IDbTableBinding binding, DbColumnsSelection selection)
        {
            var col = binding.FindColumn(selection);
            if (col == null)
            {
                throw new ArgumentException($"Invalid column of kind {selection} in table [{binding.TableName}]");
            }

            return col;
        }

        /// <summary>
        /// Finds a column binding.
        /// </summary>
        public static IDbColumnBinding FindColumn(this IDbTableBinding binding, DbColumnsSelection selection)
        {
            return binding.Columns?.FirstOrDefault(c => IsSelected(binding, c, selection));
        }

        /// <summary>
        /// Get a column binding.
        /// </summary>
        public static IDbColumnBinding GetColumn(this IDbTableBinding binding, string propertyName)
        {
            var col = binding.FindColumn(propertyName);
            if (col == null)
            {
                throw new ArgumentException($"Invalid column {propertyName} in table [{binding.TableName}]");
            }

            return col;
        }

        /// <summary>
        /// Finds a column binding.
        /// </summary>
        public static IDbColumnBinding FindColumn(this IDbTableBinding binding, string propertyName)
        {
            return binding.Columns?.FirstOrDefault(i => i.PropertyName == propertyName);
        }

        /// <summary>
        /// Get the list of columns defined in a key.
        /// </summary>
        public static string AggregateText(this IDbTableBinding binding, IDbTableKeyAttribute key, string separator, Func<IDbColumnBinding, string> action)
        {
            return binding.AggregateText(key.PropertyNames, separator, action);
        }

        /// <summary>
        /// Get an array of column names.
        /// </summary>
        public static string AggregateText(this IDbTableBinding binding, string[] propertyNames, string separator, Func<IDbColumnBinding, string> action)
        {
            return propertyNames.Select(n => binding.GetColumn(n)).AggregateText(separator, action);
        }

        /// <summary>
        /// Get an aggregated text from a columns selection.
        /// </summary>
        public static string AggregateText(this IDbTableBinding binding, DbColumnsSelection selection, string separator, Func<IDbColumnBinding, string> action)
        {
            if (selection != DbColumnsSelection.None)
            {
                return binding.Columns.Where(c => IsSelected(binding, c, selection)).AggregateText(separator, action);
            }

            return string.Empty;
        }

        /// <summary>
        /// Get an aggregated text from a columns.
        /// </summary>
        public static string AggregateText(this IEnumerable<IDbColumnBinding> columns, string separator, Func<IDbColumnBinding, string> action)
        {
            var sb = new StringBuilder();
            columns.ForEach(c => sb.ConditionalAppend(separator, action(c)));
            return sb.ToString();
        }

        /// <summary>
        /// Maps an action to every column.
        /// </summary>
        public static void MapColumns(this IDbTableBinding binding, DbColumnsSelection selection, Action<IDbColumnBinding> action)
        {
            binding.Columns.Where(c => IsSelected(binding, c, selection)).ForEach(c => action(c));
        }

        /// <summary>
        /// Check whether if a column matches a selection criteria or not.
        /// </summary>
        public static bool IsSelected(IDbTableBinding binding, IDbColumnBinding column, DbColumnsSelection selection)
        {
            switch (selection)
            {
                case DbColumnsSelection.None:
                    return false;

                case DbColumnsSelection.AutoInc:
                    return column.IsAutoIncrement;

                case DbColumnsSelection.NonAutoInc:
                    return !column.IsAutoIncrement;

                case DbColumnsSelection.PrimaryKey:
                    return IsInPrimaryKey(binding, column);

                case DbColumnsSelection.NonPrimaryKey:
                    return !IsInPrimaryKey(binding, column);

                case DbColumnsSelection.UniqueKey:
                    return IsInUniqueKey(binding, column);

                case DbColumnsSelection.All:
                default:
                    return true;
            }
        }

        private static bool IsInPrimaryKey(IDbTableBinding binding, IDbColumnBinding column)
        {
            return column.KeyConstraint == DbKeyConstraint.PrimaryKey
                || column.KeyConstraint == DbKeyConstraint.PrimaryForeignKey
                || binding.PrimaryKey.PropertyNames.Contains(column.PropertyName);
        }

        private static bool IsInUniqueKey(IDbTableBinding binding, IDbColumnBinding column)
        {
            return column.KeyConstraint == DbKeyConstraint.UniqueKey
                || binding.UniqueKeys.Count == 1 && binding.UniqueKeys.Any(uk => uk.PropertyNames.Contains(column.PropertyName));
        }

        #endregion

        #region IDbTableBinding

        /// <summary>
        /// Set table stored procedure parameters.
        /// </summary>
        public static void SetTableSpInputParameters<T>(this IDbTableBinding binding, DbCommand cmd, T instance, StoredProcedureFunction spFunc)
        {
            var columns = DbArchitectureHelper.GetTableSpParameters(spFunc);
            binding.MapColumns(columns, c => c.SetInputParameters(cmd, instance));
        }

        /// <summary>
        /// Load results returned from stored procedure.
        /// </summary>
        public static bool LoadResults(this IDbTableBinding binding, IDbDataAccess db, DbDataReader reader, object instance)
        {
            DbHelper.SetDbRef(instance, db);

            reader.Read();

            if (reader.HasRows)
            {
                binding.Columns?.ForEach(c => c.GetOutputField(reader, instance));
                return true;
            }

            return false;
        }

        #endregion

        #region IDbCommandBinding

        /// <summary>
        /// Assigns all input parameters on the command.
        /// </summary>
        public static void SetInputParameters(this IDbCommandBinding binding, DbCommand cmd, object instance)
        {
            binding.Parameters?.ForEach(b => b.SetInputParameters(cmd, instance));
        }

        /// <summary>
        /// Assigns all input parameters on the command.
        /// </summary>
        public static string SetInputParameters(this IDbCommandBinding binding, string query, object instance)
        {
            binding.Parameters?.ForEach(b => b.SetInputParameters(ref query, instance));
            return query;
        }

        /// <summary>
        /// Assigns all output parameters on the template instance.
        /// </summary>
        public static void GetOutputParameters(this IDbCommandBinding binding, DbCommand cmd, object instance)
        {
            binding.Parameters?.ForEach(b => b.GetOutputParameters(cmd, instance));
        }

        /// <summary>
        /// Load results returned from stored procedure.
        /// </summary>
        public static void LoadResults(this IDbCommandBinding binding, IDbDataAccess db, DbDataReader reader, object instance)
        {
            var recordsetCount = binding.RecordsetCount;
            if (recordsetCount == 0)
            {
                reader.Read();
                binding.GetOutputFields(db, reader, instance);
            }
            else
            {
                for (var index = 0; index < recordsetCount; index++)
                {
                    if (index > 0 && !reader.NextResult())
                    {
                        throw new ArgumentException($"Unable to load recordset index {index}");
                    }

                    var rsBinding = binding.Recordsets?.FirstOrDefault(b => b.Attributes.Index == index);
                    if (rsBinding != null)
                    {
                        rsBinding.LoadResults(db, reader, instance);
                    }
                    else
                    {
                        var rBinding = binding.Records?.FirstOrDefault(b => b.Attributes.Index == index);
                        if (rBinding != null)
                        {
                            rBinding.LoadResults(db, reader, instance);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get output fields.
        /// </summary>
        public static void GetOutputFields(this IDbCommandBinding binding, IDbDataAccess db, DbDataReader reader, object instance)
        {
            DbHelper.SetDbRef(instance, db);

            if (reader.HasRows)
            {
                binding.Fields?.ForEach(b => b.GetOutputField(reader, instance));
            }
        }

        #endregion

        #region IDbColumnBinding

        /// <summary>
        /// Get a column name.
        /// </summary>
        public static string GetSqlColumnName(this IDbColumnBinding column)
        {
            return column.Setup.Naming.GetSqlColumnName(column.ColumnName);
        }

        /// <summary>
        /// Get a parameter name.
        /// </summary>
        public static string GetSqlParamName(this IDbColumnBinding column)
        {
            return column.Setup.Naming.GetSqlParamName(column.SpParamName);
        }

        /// <summary>
        /// Get a column type.
        /// </summary>
        public static string GetSqlColumnType(this IDbColumnBinding column)
        {
            return column.Setup.Naming.GetSqlType(column.DataType, column.ColumnType, column.ColumnSize);
        }

        /// <summary>
        /// Assigns input parameters.
        /// </summary>
        public static void SetInputParameters(this IDbColumnBinding binding, DbCommand cmd, object instance)
        {
            DbHelper.SetSqlParameter(cmd, binding.SpParamName, binding.GetValue(instance), false);
        }

        /// <summary>
        /// Get output field.
        /// </summary>
        public static void GetOutputField(this IDbColumnBinding binding, DbDataReader reader, object instance)
        {
            binding.SetValue(instance, reader[binding.ColumnName]);
        }

        #endregion

        #region IDbFieldBinding

        /// <summary>
        /// Get output field.
        /// </summary>
        public static void GetOutputField(this IDbFieldBinding binding, DbDataReader reader, object instance)
        {
            try
            {
                binding.SetValue(instance, reader[binding.FieldName]);
            }
            catch
            {
                if (!binding.Attributes.IsOptional)
                {
                    throw;
                }
            }
        }

        #endregion

        #region IDbParameterBinding

        /// <summary>
        /// Assigns input parameters.
        /// </summary>
        public static void SetInputParameters(this IDbParameterBinding binding, ref string text, object instance)
        {
            var attr = binding.Attributes;
            if (attr.IsInput)
            {
                string valueGetter() => binding.GetValue(instance).ToString();
                DbHelper.SetSqlParameter(ref text, binding.SpParamName, valueGetter, attr.IsOptional);
            }
        }

        /// <summary>
        /// Assigns input parameters.
        /// </summary>
        public static void SetInputParameters(this IDbParameterBinding binding, DbCommand cmd, object instance)
        {
            var attr = binding.Attributes;
            if (attr.IsInput)
            {
                try
                {
                    DbHelper.SetSqlParameter(cmd, binding.SpParamName, binding.GetValue(instance), attr.IsOptional);
                }
                catch
                {
                    if (!attr.IsOptional)
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Get output parameters.
        /// </summary>
        public static void GetOutputParameters(this IDbParameterBinding binding, DbCommand cmd, object instance)
        {
            var attr = binding.Attributes;
            if (attr.IsOutput)
            {
                try
                {
                    binding.SetValue(instance, DbHelper.GetSqlParameter(cmd, binding.SpParamName));
                }
                catch
                {
                    if (!attr.IsOptional)
                    {
                        throw;
                    }
                }
            }
        }

        #endregion

        #region IDbRecordBinding

        /// <summary>
        /// Load the current recordset.
        /// </summary>
        public static void LoadResults(this IDbRecordBinding binding, IDbDataAccess db, DbDataReader reader, object instance)
        {
            var attr = binding.Attributes;
            var list = LoadSqlRecordset(db, reader, binding.BindingType, attr.AllowMultipleRecords ? 1 : 2) as IList;

            switch (list.Count)
            {
                case 0:
                    binding.SetValue(instance, null);
                    break;

                case 1:
                    binding.SetValue(instance, list[0]);
                    break;

                default:
                    throw new ArgumentException($"Stored procedure returned more than one record and a single one was expected, at index {attr.Index}");
            }
        }

        #endregion

        #region IDbRecordsetBinding

        /// <summary>
        /// Load the current recordset.
        /// </summary>
        public static void LoadResults(this IDbRecordsetBinding binding, IDbDataAccess db, DbDataReader reader, object instance)
        {
            var list = LoadSqlRecordset(db, reader, binding.BindingType, int.MaxValue);
            binding.SetValue(instance, list);
        }

        #endregion

        /// <summary>
        /// Builds a generic list and loads a recordset in it.
        /// </summary>
        private static object LoadSqlRecordset(IDbDataAccess db, DbDataReader reader, Type type, int maxCount)
        {
            var binding = db.Setup.GetCommandBinding(type);
            var listType = typeof(List<>).MakeGenericType(type);
            var result = Activator.CreateInstance(listType);
            var list = result as IList;

            for (var ri = 0; ri < maxCount && reader.Read(); ri++)
            {
                var record = Activator.CreateInstance(type);
                binding.GetOutputFields(db, reader, record);
                list.Add(record);
            }

            return result;
        }
    }
}
