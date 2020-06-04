using System;
using System.Data;
using Lit.Db.Framework;

namespace Lit.Db.MySql.Statements.Queries
{
    /// <summary>
    /// MySql create stored procedure template.
    /// </summary>
    public abstract class CreateStoredProcedureTemplate : DbCreateStoredProcedureTemplate
    {
        #region String constants

        public const string InKey = "IN";

        public const string OutKey = "OUT";

        public const string InOutKey = "INOUT";

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        protected CreateStoredProcedureTemplate(IDbSetup setup, Type tableTemplate, StoredProcedureFunction function) : base(setup, tableTemplate, function)
        {
        }

        /// <summary>
        /// Get parameter list.
        /// </summary>
        protected override string GetParamDefs(DbColumnsSelection selection)
        {
            var text = GetColumnsText(selection, ",\n  ",
                c => $"{GetDirection(ParameterDirection.Input)} {c.GetSqlParamName()} {MySqlDataType.Translate(c.DataType, c.ColumnType)}");

            return !string.IsNullOrEmpty(text) ? "\n  " + text + "\n" : text;
        }

        /// <summary>
        /// Get MySql direction.
        /// </summary>
        protected string GetDirection(ParameterDirection direction)
        {
            switch (direction)
            {
                case ParameterDirection.Input:
                    return InKey;

                case ParameterDirection.InputOutput:
                    return InOutKey;

                case ParameterDirection.Output:
                    return OutKey;

                case ParameterDirection.ReturnValue:
                default:
                    throw new ArgumentException();
            }
        }
    }
}
