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
        public CommandType CommandType { get; private set; }

        /// <summary>
        /// Execution mode.
        /// </summary>
        public DbExecutionMode Mode { get; private set; }

        /// <summary>
        /// Stored procedure name / query text.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Recordset referenced count.
        /// </summary>
        public int RecordsetCount => (records?.BindingList.Count ?? 0) + (recordsets?.BindingList.Count ?? 0);

        /// <summary>
        /// Maximum Recordset index referenced.
        /// </summary>
        public int MaxRecordsetIndex { get; private set; }

        /// <summary>
        /// Parameters.
        /// </summary>
        public IReadOnlyList<IDbParameterBinding> Parameters => parameters?.BindingList;

        private TypeBinding<IDbParameterBinding, DbParameterAttribute> parameters;

        /// <summary>
        /// Fields.
        /// </summary>
        public IReadOnlyList<IDbFieldBinding> Fields => fields?.BindingList;

        private TypeBinding<IDbFieldBinding, DbFieldAttribute> fields;

        /// <summary>
        /// Records.
        /// </summary>
        public IReadOnlyList<IDbRecordBinding> Records => records?.BindingList;

        private TypeBinding<IDbRecordBinding, DbRecordAttribute> records;

        /// <summary>
        /// Recordsets.
        /// </summary>
        public IReadOnlyList<IDbRecordsetBinding> Recordsets => recordsets?.BindingList;

        private TypeBinding<IDbRecordsetBinding, DbRecordsetAttribute> recordsets;

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
                Text = qattr.QueryText;
                CommandType = CommandType.Text;
            }

            if (sattr != null)
            {
                Text = setup.Naming.GetStoredProcedureName(templateType, sattr.StoredProcedureName);
                CommandType = CommandType.StoredProcedure;
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
                var typeArguments = new[] { TemplateType, propInfo.PropertyType };

                if (TypeHelper.GetAttribute<DbRecordsetAttribute>(propInfo, out var rsAttr))
                {
                    Mode = DbExecutionMode.Query;
                    AssertRecordsetIndex(rsAttr.Index);
                    AddBinding(ref recordsets, typeof(DbRecordsetBinding<,>), typeArguments, this, propInfo, rsAttr);
                }
                else if (TypeHelper.GetAttribute<DbRecordAttribute>(propInfo, out var rAttr))
                {
                    Mode = DbExecutionMode.Query;
                    AssertRecordsetIndex(rsAttr.Index);
                    AddBinding(ref records, typeof(DbRecordBinding<,>), typeArguments, this, propInfo, rAttr);
                }
                else if (TypeHelper.GetAttribute<DbFieldAttribute>(propInfo, out var fAttr)
                    || TypeHelper.GetAttribute<DbColumnAttribute>(propInfo, out var cAttr) && (fAttr = new DbFieldAttribute(cAttr)) != null)
                {
                    Mode = DbExecutionMode.Query;
                    AddBinding(ref fields, typeof(DbFieldBinding<,>), typeArguments, this, propInfo, fAttr);
                }
                else if (TypeHelper.GetAttribute<DbParameterAttribute>(propInfo, out var pAttr))
                {
                    AddBinding(ref parameters, typeof(DbParameterBinding<,>), typeArguments, this, propInfo, pAttr);
                }
            }
        }

        /// <summary>
        /// Calculate binding.
        /// </summary>
        internal void ResolveBinding()
        {
            recordsets?.BindingList.ForEach(rs => rs.CalcBindingMode());
            records?.BindingList.ForEach(r => r.CalcBindingMode());
            fields?.BindingList.ForEach(f => f.CalcBindingMode());
            parameters?.BindingList.ForEach(p => p.CalcBindingMode());
        }

        /// <summary>
        /// Asserts that resulset index is only used once.
        /// </summary>
        private void AssertRecordsetIndex(int index)
        {
            var exists = (recordsets?.BindingList.Any(i => i.Attributes.Index == index) ?? false) || (records?.BindingList.Any(i => i.Attributes.Index == index) ?? false);

            if (exists)
            {
                throw new ArgumentException($"Recordset index {index} is used more than once");
            }

            if (MaxRecordsetIndex < index)
            {
                MaxRecordsetIndex = index;
            }
        }
    }
}
