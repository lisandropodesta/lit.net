﻿using System;
using System.Data;
using System.Text;
using Lit.Db.Attributes;
using Lit.Db.Framework;
using Lit.Db.Model;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// MySql create stored procedure template.
    /// </summary>
    public abstract partial class CreateStoredProcedureTemplate : MySqlTemplate
    {
        /// <summary>
        /// Stored procedure name.
        /// </summary>
        [DbParameter("name")]
        public string Name { get; set; }

        [DbParameter("parameters")]
        protected string Parameters { get { return parameters.Length == 0 ? "()" : "(\n" + parameters.ToString() + "\n)"; } set { } }

        private readonly StringBuilder parameters = new StringBuilder();

        [DbParameter("table_name")]
        public string TableName { get; set; }

        [DbParameter("filter_field", isOptional: true)]
        protected string FilterField { get; set; }

        [DbParameter("filter_param", isOptional: true)]
        protected string FilterParam { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected CreateStoredProcedureTemplate(Type tableTemplate, IDbNaming dbNaming, StoredProcedureFunction function)
        {
            var binding = DbTemplateCache.GetTable(tableTemplate, dbNaming);

            var pk = binding.FindFirstColumn(DbColumnsSelection.PrimaryKey);

            if (pk == null)
            {
                throw new Exception($"Primary key not found for table template {tableTemplate.FullName}");
            }

            TableName = binding.Text;
            Name = dbNaming.GetStoredProcedureName(TableName, function);
            FilterField = pk.FieldName;
            FilterParam = pk.SpParamName;

            Setup(tableTemplate, dbNaming, function, binding, pk);
        }

        /// <summary>
        /// Setup the template.
        /// </summary>
        protected virtual void Setup(Type tableTemplate, IDbNaming dbNaming, StoredProcedureFunction function, DbTemplateBinding binding, IDbColumnBinding pk)
        {
        }

        /// <summary>
        /// Add table columns as parameters.
        /// </summary>
        protected void AddParameters(DbTemplateBinding binding, DbColumnsSelection selection, ParameterDirection direction)
        {
            binding.MapColumns(selection, c => AddParameter(c, direction));
        }

        /// <summary>
        /// Add a table column as a parameter.
        /// </summary>
        public void AddParameter(IDbColumnBinding column, ParameterDirection direction)
        {
            AddParameter(column.SpParamName, direction, column.DataType, column.FieldType);
        }

        /// <summary>
        /// Add a parameter.
        /// </summary>
        public void AddParameter(string name, ParameterDirection direction, DbDataType dataType, Type fieldType = null)
        {
            var separator = GetSeparator(parameters.Length == 0 ? string.Empty : ",\n");
            var line = $"{GetDirection(direction)} {name} {MySqlDataType.Translate(dataType, fieldType)}";
            parameters.Append(separator + line);
        }

        /// <summary>
        /// Get a list of parameters names.
        /// </summary>
        protected string GetParametersNames(DbTemplateBinding binding, DbColumnsSelection selection)
        {
            var text = new StringBuilder();
            var separator = GetSeparator(",\n", 1);
            binding.MapColumns(selection, c => AddParameterName(text, separator, c));
            return text.ToString();
        }

        /// <summary>
        /// Add a parameter name.
        /// </summary>
        protected void AddParameterName(StringBuilder text, string separator, IDbColumnBinding column)
        {
            text.ConditionalAppend(text.Length == 0 ? string.Empty : separator, column.SpParamName);
        }

        /// <summary>
        /// Get a list of columns names.
        /// </summary>
        protected string GetColumnsNames(DbTemplateBinding binding, DbColumnsSelection selection)
        {
            var text = new StringBuilder();
            var separator = GetSeparator(",\n", 1);
            binding.MapColumns(selection, c => AddColumnName(text, separator, c));
            return text.ToString();
        }

        /// <summary>
        /// Add a column name.
        /// </summary>
        public void AddColumnName(StringBuilder text, string separator, IDbColumnBinding column)
        {
            text.ConditionalAppend(text.Length == 0 ? string.Empty : separator, column.FieldName);
        }

        /// <summary>
        /// Get a list of fields assignments.
        /// </summary>
        protected string GetFieldsAssignment(DbTemplateBinding binding, DbColumnsSelection selection)
        {
            var text = new StringBuilder();
            var separator = GetSeparator(",\n", 1);
            binding.MapColumns(selection, c => AddFieldAssignment(text, separator, c));
            return text.ToString();
        }

        /// <summary>
        /// Add a field assignment.
        /// </summary>
        public void AddFieldAssignment(StringBuilder text, string separator, IDbColumnBinding column)
        {
            text.ConditionalAppend(text.Length == 0 ? string.Empty : separator, $"{column.FieldName} = {column.SpParamName}");
        }
    }
}
