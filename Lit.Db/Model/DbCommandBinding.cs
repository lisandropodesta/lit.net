using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Lit.DataType;

namespace Lit.Db
{
    /// <summary>
    /// Stored procedure/query template information.
    /// </summary>
    internal class DbCommandBinding : DbTemplateBinding, IDbCommandBinding
    {
        /// <summary>
        /// Command type.
        /// </summary>
        public CommandType CommandType => commandType;

        private readonly CommandType commandType;

        /// <summary>
        /// Execution mode.
        /// </summary>
        public DbExecutionMode Mode { get; private set; }

        /// <summary>
        /// Stored procedure name / query text.
        /// </summary>
        public string Text => name;

        private readonly string name;

        /// <summary>
        /// Recordset referenced count.
        /// </summary>
        public int RecordsetCount => (recordBindings?.Count ?? 0) + (recordsetBindings?.Count ?? 0);

        /// <summary>
        /// Maximum Recordset index referenced.
        /// </summary>
        public int MaxRecordsetIndex => maxRecordsetIndex;

        private int maxRecordsetIndex;

        /// <summary>
        /// Parameters.
        /// </summary>
        public IReadOnlyList<IDbParameterBinding> Parameters => parameterBindings;

        private List<IDbParameterBinding> parameterBindings;

        /// <summary>
        /// Fields.
        /// </summary>
        public IReadOnlyList<IDbFieldBinding> Fields => fieldBindings;

        private List<IDbFieldBinding> fieldBindings;

        /// <summary>
        /// Records.
        /// </summary>
        public IReadOnlyList<IDbRecordBinding> Records => recordBindings;

        private List<IDbRecordBinding> recordBindings;

        /// <summary>
        /// Recordsets.
        /// </summary>
        public IReadOnlyList<IDbRecordsetBinding> Recordsets => recordsetBindings;

        private List<IDbRecordsetBinding> recordsetBindings;

        #region Constructor

        internal DbCommandBinding(Type templateType, IDbSetup setup)
            : base(templateType, setup)
        {
            Mode = DbExecutionMode.NonQuery;

            var qattr = TypeHelper.GetAttribute<DbQueryAttribute>(templateType, true);
            var sattr = TypeHelper.GetAttribute<DbStoredProcedureAttribute>(templateType, true);

            if ((qattr != null ? 1 : 0) + (sattr != null ? 1 : 0) > 1)
            {
                throw new ArgumentException($"Multiple template definition in class [{templateType.FullName}]");
            }

            if (qattr != null)
            {
                name = qattr.QueryText;
                commandType = CommandType.Text;
            }

            if (sattr != null)
            {
                name = setup.Naming.GetStoredProcedureName(templateType, sattr.StoredProcedureName);
                commandType = CommandType.StoredProcedure;
            }

            AddProperties();
        }

        #endregion

        /// <summary>
        /// Add properties defined in the template.
        /// </summary>
        private void AddProperties()
        {
            foreach (var propInfo in TemplateType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (TypeHelper.GetAttribute<DbRecordsetAttribute>(propInfo, out var rsAttr))
                {
                    Mode = DbExecutionMode.Query;
                    AssertRecordsetIndex(rsAttr.Index);
                    AddBinding(ref recordsetBindings, typeof(DbRecordsetBinding<,>), propInfo, rsAttr);
                }
                else if (TypeHelper.GetAttribute<DbRecordAttribute>(propInfo, out var rAttr))
                {
                    Mode = DbExecutionMode.Query;
                    AssertRecordsetIndex(rsAttr.Index);
                    AddBinding(ref recordBindings, typeof(DbRecordBinding<,>), propInfo, rAttr);
                }
                else if (TypeHelper.GetAttribute<DbFieldAttribute>(propInfo, out var fAttr)
                    || TypeHelper.GetAttribute<DbColumnAttribute>(propInfo, out var cAttr) && (fAttr = new DbFieldAttribute(cAttr)) != null)
                {
                    Mode = DbExecutionMode.Query;
                    AddBinding(ref fieldBindings, typeof(DbFieldBinding<,>), propInfo, fAttr);
                }
                else if (TypeHelper.GetAttribute<DbParameterAttribute>(propInfo, out var pAttr))
                {
                    AddBinding(ref parameterBindings, typeof(DbParameterBinding<,>), propInfo, pAttr);
                }
            }
        }

        /// <summary>
        /// Calculate binding.
        /// </summary>
        internal void ResolveBinding()
        {
            recordsetBindings?.ForEach(rs => rs.CalcBindingMode());
            recordBindings?.ForEach(r => r.CalcBindingMode());
            fieldBindings?.ForEach(f => f.CalcBindingMode());
            parameterBindings?.ForEach(p => p.CalcBindingMode());
        }

        /// <summary>
        /// Asserts that resulset index is only used once.
        /// </summary>
        private void AssertRecordsetIndex(int index)
        {
            var exists = (recordsetBindings?.Any(i => i.Attributes.Index == index) ?? false) || (recordBindings?.Any(i => i.Attributes.Index == index) ?? false);

            if (exists)
            {
                throw new ArgumentException($"Recordset index {index} is used more than once");
            }

            if (maxRecordsetIndex < index)
            {
                maxRecordsetIndex = index;
            }
        }
    }
}
