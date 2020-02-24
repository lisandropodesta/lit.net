using System;
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

        [DbParameter("primary_key", isOptional: true)]
        public string PrimaryKey { get; set; }

        [DbParameter("primary_key_param", isOptional: true)]
        public string PrimaryKeyParam { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected CreateStoredProcedureTemplate(Type tableTemplate, IDbNaming dbNaming, StoredProcedureFunction function)
        {
            var binding = DbTemplateCache.GetTable(tableTemplate, dbNaming);

            var pk = binding.Columns.FirstOrDefault(c => IsSelected(c, ParametersSelection.PrimaryKey));
            if (pk == null)
            {
                throw new Exception($"Primary key not found for template table {tableTemplate.FullName}");
            }

            TableName = binding.Text;
            Name = dbNaming.GetStoredProcedureName(TableName, function);
            PrimaryKey = pk.FieldName;
            PrimaryKeyParam = dbNaming.GetParameterName(pk.FieldName, null);

            Setup(dbNaming, binding, pk);
        }

        /// <summary>
        /// Setup the template.
        /// </summary>
        protected virtual void Setup(IDbNaming dbNaming, DbTemplateBinding binding, IDbColumnBinding pk)
        {
        }

        /// <summary>
        /// Parameters seleccion.
        /// </summary>
        protected enum ParametersSelection
        {
            All,

            PrimaryKey,

            NonPrimaryKey
        }

        /// <summary>
        /// Add table columns as parameters.
        /// </summary>
        protected void AddParameters(IEnumerable<IDbColumnBinding> columns, ParametersSelection selection, ParameterDirection direction, IDbNaming dbNaming)
        {
            columns.Where(c => IsSelected(c, selection)).ForEach(c => AddParameter(c, direction, dbNaming));
        }

        /// <summary>
        /// Add a table column as a parameter.
        /// </summary>
        public void AddParameter(IDbColumnBinding column, ParameterDirection direction, IDbNaming dbNaming)
        {
            var paramName = dbNaming.GetParameterName(column.FieldName, null);
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
        /// Add columns names.
        /// </summary>
        protected void AddColumns(StringBuilder text, ParametersSelection selection, string separator, IReadOnlyList<IDbColumnBinding> columns)
        {
            columns.Where(c => IsSelected(c, selection)).ForEach(c => AddColumn(text, separator, c));
        }

        /// <summary>
        /// Add a column name.
        /// </summary>
        public void AddColumn(StringBuilder text, string separator, IDbColumnBinding column)
        {
            text.ConditionalAppend(text.Length == 0 ? string.Empty : separator, column.FieldName);
        }

        /// Check whether if a column matches a selection criteria or not.
        /// </summary>
        protected bool IsSelected(IDbColumnBinding column, ParametersSelection selection)
        {
            switch (selection)
            {
                case ParametersSelection.PrimaryKey:
                    return column.KeyConstraint == DbKeyConstraint.PrimaryKey;

                case ParametersSelection.NonPrimaryKey:
                    return column.KeyConstraint != DbKeyConstraint.PrimaryKey;

                case ParametersSelection.All:
                default:
                    return true;
            }
        }
    }
}
