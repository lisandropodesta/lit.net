﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Lit.Db.Architecture;
using Lit.Db.Attributes;
using Lit.Db.Model;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// MySql create stored procedure template.
    /// </summary>
    public abstract class CreateStoredProcedureTemplate : MySqlTemplate
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

            var pk = FindFirstColumn(binding, ParametersSelection.PrimaryKey);

            if (pk == null)
            {
                throw new Exception($"Primary key not found for table template {tableTemplate.FullName}");
            }

            TableName = binding.Text;
            Name = dbNaming.GetStoredProcedureName(TableName, function);
            FilterField = pk.FieldName;
            FilterParam = dbNaming.GetParameterName(pk.FieldName, null);

            Setup(tableTemplate, dbNaming, function, binding, pk);
        }

        /// <summary>
        /// Setup the template.
        /// </summary>
        protected virtual void Setup(Type tableTemplate, IDbNaming dbNaming, StoredProcedureFunction function, DbTemplateBinding binding, IDbColumnBinding pk)
        {
        }

        /// <summary>
        /// Parameters seleccion.
        /// </summary>
        protected enum ParametersSelection
        {
            /// <summary>
            /// All fields.
            /// </summary>
            All,

            /// <summary>
            /// First primary key.
            /// </summary>
            PrimaryKey,

            /// <summary>
            /// First unique key.
            /// </summary>
            UniqueKey,

            /// <summary>
            /// All non primary keys.
            /// </summary>
            NonPrimaryKey
        }

        /// <summary>
        /// Get the first column that matches with selection.
        /// </summary>
        protected IDbColumnBinding FindFirstColumn(DbTemplateBinding binding, ParametersSelection selection)
        {
            return binding.Columns.FirstOrDefault(c => IsSelected(c, selection));
        }

        /// <summary>
        /// Add table columns as parameters.
        /// </summary>
        protected void AddParameters(DbTemplateBinding binding, ParametersSelection selection, ParameterDirection direction, IDbNaming dbNaming)
        {
            binding.Columns.Where(c => IsSelected(c, selection)).ForEach(c => AddParameter(c, direction, dbNaming));
        }

        /// <summary>
        /// Add a table column as a parameter.
        /// </summary>
        public void AddParameter(IDbColumnBinding column, ParameterDirection direction, IDbNaming dbNaming)
        {
            var paramName = GetParameterName(column, dbNaming);
            AddParameter(paramName, direction, column.DataType, column.FieldType);
        }

        /// <summary>
        /// Add a parameter.
        /// </summary>
        public void AddParameter(string name, ParameterDirection direction, DbDataType dataType, Type fieldType = null)
        {
            var separator = parameters.Length == 0 ? "  " : ",\n  ";
            var line = $"{GetDirection(direction)} {name} {MySqlDataType.Translate(dataType, fieldType)}";
            parameters.Append(separator + line);
        }

        /// <summary>
        /// Get a list of parameters names.
        /// </summary>
        protected string GetParametersNames(DbTemplateBinding binding, ParametersSelection selection, IDbNaming dbNaming)
        {
            var text = new StringBuilder();
            AddParametersNames(text, ",\n    ", binding, selection, dbNaming);
            return text.ToString();
        }

        /// <summary>
        /// Add parameters names.
        /// </summary>
        protected void AddParametersNames(StringBuilder text, string separator, DbTemplateBinding binding, ParametersSelection selection, IDbNaming dbNaming)
        {
            binding.Columns.Where(c => IsSelected(c, selection)).ForEach(c => AddParameterName(text, separator, c, dbNaming));
        }

        /// <summary>
        /// Add a parameter name.
        /// </summary>
        protected void AddParameterName(StringBuilder text, string separator, IDbColumnBinding column, IDbNaming dbNaming)
        {
            text.ConditionalAppend(text.Length == 0 ? string.Empty : separator, GetParameterName(column, dbNaming));
        }

        /// <summary>
        /// Get a parameter name.
        /// </summary>
        protected string GetParameterName(IDbColumnBinding column, IDbNaming dbNaming)
        {
            return dbNaming.GetParameterName(column.FieldName, null);
        }

        /// <summary>
        /// Get a list of columns names.
        /// </summary>
        protected string GetColumnsNames(DbTemplateBinding binding, ParametersSelection selection)
        {
            var columns = new StringBuilder();
            AddColumnsNames(columns, ",\n    ", binding.Columns, selection);
            return columns.ToString();
        }

        /// <summary>
        /// Add columns names.
        /// </summary>
        protected void AddColumnsNames(StringBuilder text, string separator, IReadOnlyList<IDbColumnBinding> columns, ParametersSelection selection)
        {
            columns.Where(c => IsSelected(c, selection)).ForEach(c => AddColumnName(text, separator, c));
        }

        /// <summary>
        /// Add a column name.
        /// </summary>
        public void AddColumnName(StringBuilder text, string separator, IDbColumnBinding column)
        {
            text.ConditionalAppend(text.Length == 0 ? string.Empty : separator, column.FieldName);
        }

        /// <summary>
        /// Check whether if a column matches a selection criteria or not.
        /// </summary>
        protected bool IsSelected(IDbColumnBinding column, ParametersSelection selection)
        {
            switch (selection)
            {
                case ParametersSelection.PrimaryKey:
                    return column.KeyConstraint == DbKeyConstraint.PrimaryKey;

                case ParametersSelection.UniqueKey:
                    return column.KeyConstraint == DbKeyConstraint.UniqueKey;

                case ParametersSelection.NonPrimaryKey:
                    return column.KeyConstraint != DbKeyConstraint.PrimaryKey;

                case ParametersSelection.All:
                default:
                    return true;
            }
        }
    }
}
