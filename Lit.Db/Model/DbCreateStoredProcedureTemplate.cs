using System;
using Lit.Db.Framework;

namespace Lit.Db
{
    /// <summary>
    /// Stored procedure creator template.
    /// </summary>
    public abstract class DbCreateStoredProcedureTemplate : DbTemplate
    {
        /// <summary>
        /// Stored procedure name.
        /// </summary>
        [DbParameter(DoNotTranslate = true)]
        public string SqlSpName { get; set; }

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        public string SqlTableName { get; set; }

        #region Parameters definition

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string AllParamsDef => GetParamDefs(DbColumnsSelection.All);

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string AutoIncParamDef => GetParamDefs(DbColumnsSelection.AutoInc);

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string NonAutoIncParamsDef => GetParamDefs(DbColumnsSelection.NonAutoInc);

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string PrimaryKeyParamsDef => GetParamDefs(DbColumnsSelection.PrimaryKey);

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string NonPrimaryKeyParamsDef => GetParamDefs(DbColumnsSelection.NonPrimaryKey);

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string UniqueKeyParamsDef => GetParamDefs(DbColumnsSelection.UniqueKey);

        #endregion

        #region Parameters

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string AllParams => GetParamNames(DbColumnsSelection.All);

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string AutoIncParam => GetParamNames(DbColumnsSelection.AutoInc);

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string NonAutoIncParams => GetParamNames(DbColumnsSelection.NonAutoInc);

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string PrimaryKeyParams => GetParamNames(DbColumnsSelection.PrimaryKey);

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string NonPrimaryKeyParams => GetParamNames(DbColumnsSelection.NonPrimaryKey);

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string UniqueKeyParams => GetParamNames(DbColumnsSelection.UniqueKey);

        #endregion

        #region Columns

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string AllColumns => GetColumnNames(DbColumnsSelection.All);

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string AutoIncColumn => GetColumnNames(DbColumnsSelection.AutoInc);

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string NonAutoIncColumns => GetColumnNames(DbColumnsSelection.NonAutoInc);

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string PrimaryKeyColumns => GetColumnNames(DbColumnsSelection.PrimaryKey);

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string NonPrimaryKeyColumns => GetColumnNames(DbColumnsSelection.NonPrimaryKey);

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string UniqueKeyColumns => GetColumnNames(DbColumnsSelection.UniqueKey);

        #endregion

        #region Assignment set

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string AllColumsSet => GetFieldAssignments(DbColumnsSelection.All);

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string NonAutoIncColumsSet => GetFieldAssignments(DbColumnsSelection.NonAutoInc);

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string NonPrimaryColumsSet => GetFieldAssignments(DbColumnsSelection.NonPrimaryKey);

        #endregion

        #region Data types

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string AutoIncDataType => Binding.GetColumn(DbColumnsSelection.AutoInc).GetSqlColumnType();

        #endregion

        #region Conditions

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string AutoIncMatchCondition => GetMatchCondition(DbColumnsSelection.AutoInc);

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string PrimaryKeyMatchCondition => GetMatchCondition(DbColumnsSelection.PrimaryKey);

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string UniqueKeyMatchCondition => GetMatchCondition(DbColumnsSelection.UniqueKey);

        [DbParameter(isOptional: true, DoNotTranslate = true)]
        protected string RecordDoNotExistsCondition => GetRecordDoNotExistsCondition();

        #endregion

        protected Type TableTemplate;

        protected IDbTableBinding Binding;

        /// <summary>
        /// Constructor.
        /// </summary>
        protected DbCreateStoredProcedureTemplate(IDbSetup setup, Type tableTemplate, StoredProcedureFunction function) : base(setup)
        {
            TableTemplate = tableTemplate;
            Binding = setup.GetTableBinding(tableTemplate);
            SqlTableName = Binding.GetSqlTableName();
            SqlSpName = Binding.GetSqlTableSpName(function);
        }

        /// <summary>
        /// Get parameter list.
        /// </summary>
        protected abstract string GetParamDefs(DbColumnsSelection selection);

        /// <summary>
        /// Get parameter list.
        /// </summary>
        protected virtual string GetParamNames(DbColumnsSelection selection)
        {
            return GetColumnsText(selection, ",\n\t", c => c.GetSqlParamName());
        }

        /// <summary>
        /// Get a list of columns names.
        /// </summary>
        protected virtual string GetColumnNames(DbColumnsSelection selection)
        {
            return GetColumnsText(selection, ",\n\t", c => c.GetSqlColumnName());
        }

        /// <summary>
        /// Get a list of fields assignments.
        /// </summary>
        protected virtual string GetFieldAssignments(DbColumnsSelection selection)
        {
            return GetColumnsText(selection, ",\n\t", c => $"{c.GetSqlColumnName()} = {c.GetSqlParamName()}");
        }

        /// <summary>
        /// Get filter list.
        /// </summary>
        protected virtual string GetMatchCondition(DbColumnsSelection selection)
        {
            var text = GetColumnsText(selection, "\n\tAND ", c => $"{c.GetSqlColumnName()} = {c.GetSqlParamName()}");
            return !string.IsNullOrEmpty(text) ? "    " + text : text;
        }

        /// <summary>
        /// Get a list from a columns selection.
        /// </summary>
        protected string GetColumnsText(DbColumnsSelection selection, string separator, Func<IDbColumnBinding, string> action)
        {
            return Binding.AggregateText(selection, separator, action);
        }

        /// <summary>
        /// Check record existance.
        /// </summary>
        protected string GetRecordDoNotExistsCondition()
        {
            if (Binding.HasColumns(DbColumnsSelection.AutoInc))
            {
                return GetColumnsText(DbColumnsSelection.AutoInc, " AND ", c => $"COALESCE( {c.GetSqlParamName()}, 0 ) = 0");
            }

            var fields = GetColumnsText(DbColumnsSelection.PrimaryKey, ", ", c => c.GetSqlColumnName());
            var condition = GetColumnsText(DbColumnsSelection.PrimaryKey, " AND ", c => $"{c.GetSqlColumnName()} = {c.GetSqlParamName()}");
            return $"NOT EXISTS ( SELECT {fields} FROM {SqlTableName} WHERE {condition} )";
        }
    }
}
