using System.Collections.Generic;
using System.Data;

namespace Lit.Db
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
    }
}
