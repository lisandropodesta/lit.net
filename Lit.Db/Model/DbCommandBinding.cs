﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Lit.DataType;
using Lit.Db.Attributes;

namespace Lit.Db.Model
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
        public DbExecutionMode Mode => mode;

        private readonly DbExecutionMode mode;

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

        private readonly List<IDbParameterBinding> parameterBindings;

        /// <summary>
        /// Fields.
        /// </summary>
        public IReadOnlyList<IDbFieldBinding> Fields => fieldBindings;

        private readonly List<IDbFieldBinding> fieldBindings;

        /// <summary>
        /// Records.
        /// </summary>
        public IReadOnlyList<IDbRecordBinding> Records => recordBindings;

        private readonly List<IDbRecordBinding> recordBindings;

        /// <summary>
        /// Recordsets.
        /// </summary>
        public IReadOnlyList<IDbRecordsetBinding> Recordsets => recordsetBindings;

        private readonly List<IDbRecordsetBinding> recordsetBindings;

        #region Constructor

        internal DbCommandBinding(Type templateType, IDbSetup setup)
            : base(templateType, setup)
        {
            mode = DbExecutionMode.NonQuery;

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

            foreach (var propInfo in templateType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (TypeHelper.GetAttribute<DbRecordsetAttribute>(propInfo, out var rsAttr))
                {
                    mode = DbExecutionMode.Query;
                    AssertRecordsetIndex(rsAttr.Index);
                    AddBinding(ref recordsetBindings, typeof(DbRecordsetBinding<,>), propInfo, rsAttr, setup);
                }
                else if (TypeHelper.GetAttribute<DbRecordAttribute>(propInfo, out var rAttr))
                {
                    mode = DbExecutionMode.Query;
                    AssertRecordsetIndex(rsAttr.Index);
                    AddBinding(ref recordBindings, typeof(DbRecordBinding<,>), propInfo, rAttr, setup);
                }
                else if (TypeHelper.GetAttribute<DbFieldAttribute>(propInfo, out var fAttr)
                    || TypeHelper.GetAttribute<DbColumnAttribute>(propInfo, out var cAttr) && (fAttr = new DbFieldAttribute(cAttr.DbName)) != null)
                {
                    mode = DbExecutionMode.Query;
                    AddBinding(ref fieldBindings, typeof(DbFieldBinding<,>), propInfo, fAttr, setup);
                }
                else if (TypeHelper.GetAttribute<DbParameterAttribute>(propInfo, out var pAttr))
                {
                    AddBinding(ref parameterBindings, typeof(DbParameterBinding<,>), propInfo, pAttr, setup);
                }
            }
        }

        #endregion

        /// <summary>
        /// Assigns all input parameters on the command.
        /// </summary>
        public void SetInputParameters(DbCommand cmd, object instance)
        {
            parameterBindings?.ForEach(b => b.SetInputParameters(cmd, instance));
        }

        /// <summary>
        /// Assigns all input parameters on the command.
        /// </summary>
        public string SetInputParameters(string query, object instance)
        {
            parameterBindings?.ForEach(b => b.SetInputParameters(ref query, instance));
            return query;
        }

        /// <summary>
        /// Assigns all output parameters on the template instance.
        /// </summary>
        public void GetOutputParameters(DbCommand cmd, object instance)
        {
            parameterBindings?.ForEach(b => b.GetOutputParameters(cmd, instance));
        }

        /// <summary>
        /// Get output fields.
        /// </summary>
        public void GetOutputFields(DbDataReader reader, object instance)
        {
            fieldBindings?.ForEach(b => b.GetOutputField(reader, instance));
        }

        /// <summary>
        /// Load results returned from stored procedure.
        /// </summary>
        public void LoadResults(DbDataReader reader, object instance)
        {
            var recordsetCount = RecordsetCount;
            if (recordsetCount == 0)
            {
                if (reader.Read())
                {
                    GetOutputFields(reader, instance);
                }
            }
            else
            {
                for (var index = 0; index < recordsetCount; index++)
                {
                    if (index > 0 && !reader.NextResult())
                    {
                        throw new ArgumentException($"Unable to load recordset index {index}");
                    }

                    var rsBinding = recordsetBindings?.FirstOrDefault(b => b.Attributes.Index == index);
                    if (rsBinding != null)
                    {
                        rsBinding.LoadResults(reader, instance);
                    }
                    else
                    {
                        var rBinding = recordBindings?.FirstOrDefault(b => b.Attributes.Index == index);
                        if (rBinding != null)
                        {
                            rBinding.LoadResults(reader, instance);
                        }
                    }
                }
            }
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
