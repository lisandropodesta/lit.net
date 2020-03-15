using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Lit.Db.Model
{
    /// <summary>
    /// Stored procedure/query template information.
    /// </summary>
    public interface IDbCommandBinding : IDbTemplateBinding
    {
        /// <summary>
        /// Command type.
        /// </summary>
        CommandType CommandType { get; }

        /// <summary>
        /// Execution mode.
        /// </summary>
        DbExecutionMode Mode { get; }

        /// <summary>
        /// Stored procedure name / query text.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Recordset referenced count.
        /// </summary>
        int RecordsetCount { get; }

        /// <summary>
        /// Maximum Recordset index referenced.
        /// </summary>
        int MaxRecordsetIndex { get; }

        /// <summary>
        /// Parameters.
        /// </summary>
        IReadOnlyList<IDbParameterBinding> Parameters { get; }

        /// <summary>
        /// Fields.
        /// </summary>
        IReadOnlyList<IDbFieldBinding> Fields { get; }

        /// <summary>
        /// Records.
        /// </summary>
        IReadOnlyList<IDbRecordBinding> Records { get; }

        /// <summary>
        /// Recordsets.
        /// </summary>
        IReadOnlyList<IDbRecordsetBinding> Recordsets { get; }

        /// <summary>
        /// Assigns all input parameters on the command.
        /// </summary>
        void SetInputParameters(DbCommand cmd, object instance);

        /// <summary>
        /// Assigns all input parameters on the command.
        /// </summary>
        string SetInputParameters(string query, object instance);

        /// <summary>
        /// Assigns all output parameters on the template instance.
        /// </summary>
        void GetOutputParameters(DbCommand cmd, object instance);

        /// <summary>
        /// Get output fields.
        /// </summary>
        void GetOutputFields(DbDataReader reader, object instance);

        /// <summary>
        /// Load results returned from stored procedure.
        /// </summary>
        void LoadResults(DbDataReader reader, object instance);
    }
}
