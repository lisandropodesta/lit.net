using System;
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
    public class DbTemplateBinding
    {
        /// <summary>
        /// Command type.
        /// </summary>
        public CommandType CommandType => commandType;

        private readonly CommandType commandType;

        /// <summary>
        /// Template data type.
        /// </summary>
        public Type TemplateType => templateType;

        private readonly Type templateType;

        /// <summary>
        /// Stored procedure name / query text.
        /// </summary>
        public string Text => text;

        private readonly string text;

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
        /// Execution mode.
        /// </summary>
        public DbExecutionMode Mode => mode;

        private readonly DbExecutionMode mode;

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
        /// Columns.
        /// </summary>
        public IReadOnlyList<IDbColumnBinding> Columns => columnBindings;

        private readonly List<IDbColumnBinding> columnBindings;

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

        internal DbTemplateBinding(Type templateType, IDbNaming dbNaming)
        {
            this.templateType = templateType;
            mode = DbExecutionMode.NonQuery;

            var qattr = TypeHelper.GetAttribute<DbQueryAttribute>(templateType, true);
            var sattr = TypeHelper.GetAttribute<DbStoredProcedureAttribute>(templateType, true);
            var tattr = TypeHelper.GetAttribute<DbTableAttribute>(templateType, true);

            if ((qattr != null ? 1 : 0) + (sattr != null ? 1 : 0) + (tattr != null ? 1 : 0) > 1)
            {
                throw new ArgumentException($"Multiple template definition in class [{templateType.FullName}]");
            }

            if (qattr != null)
            {
                text = qattr.QueryText;
                commandType = CommandType.Text;
            }

            if (sattr != null)
            {
                text = dbNaming.GetStoredProcedureName(templateType, sattr.StoredProcedureName);
                commandType = CommandType.StoredProcedure;
            }

            if (tattr != null)
            {
                text = dbNaming.GetTableName(templateType, tattr.TableName);
                commandType = CommandType.TableDirect;
            }

            foreach (var propInfo in templateType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (TypeHelper.GetAttribute<DbColumnAttribute>(propInfo, out var cAttr))
                {
                    AddBinding(ref columnBindings, typeof(DbColumnBinding<,>), propInfo, cAttr, dbNaming);
                }
                else if (TypeHelper.GetAttribute<DbRecordsetAttribute>(propInfo, out var rsAttr))
                {
                    mode = DbExecutionMode.Query;
                    AssertRecordsetIndex(rsAttr.Index);
                    AddBinding(ref recordsetBindings, typeof(DbRecordsetBinding<,>), propInfo, rsAttr, dbNaming);
                }
                else if (TypeHelper.GetAttribute<DbRecordAttribute>(propInfo, out var rAttr))
                {
                    mode = DbExecutionMode.Query;
                    AssertRecordsetIndex(rsAttr.Index);
                    AddBinding(ref recordBindings, typeof(DbRecordBinding<,>), propInfo, rAttr, dbNaming);
                }
                else if (TypeHelper.GetAttribute<DbFieldAttribute>(propInfo, out var fAttr))
                {
                    mode = DbExecutionMode.Query;
                    AddBinding(ref fieldBindings, typeof(DbFieldBinding<,>), propInfo, fAttr, dbNaming);
                }
                else if (TypeHelper.GetAttribute<DbParameterAttribute>(propInfo, out var pAttr))
                {
                    AddBinding(ref parameterBindings, typeof(DbParameterBinding<,>), propInfo, pAttr, dbNaming);
                }
            }

            switch (commandType)
            {
                case CommandType.StoredProcedure:
                case CommandType.Text:
                    if (columnBindings?.Count > 0)
                    {
                        throw new ArgumentException($"Invalid definition of DbColumnAttribute in non-table template [{templateType.Name}]");
                    }
                    break;

                case CommandType.TableDirect:
                    if ((recordsetBindings?.Count ?? 0) + (recordBindings?.Count ?? 0) + (parameterBindings?.Count ?? 0) + (fieldBindings?.Count ?? 0) > 0)
                    {
                        throw new ArgumentException("Invalid definition of non-DbColumnAttribute in table template [{templateType.Name}]");
                    }
                    break;
            }
        }

        #endregion

        /// <summary>
        /// Finds a column binding.
        /// </summary>
        public IDbColumnBinding FindColumn(string propertyName)
        {
            var propInfo = templateType.GetProperty(propertyName);
            return Columns?.FirstOrDefault(i => i.PropertyInfo == propInfo);
        }

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
        public void LoadResults(DbDataReader reader, object instance, IDbNaming dbNaming)
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
                        rsBinding.LoadResults(reader, instance, dbNaming);
                    }
                    else
                    {
                        var rBinding = recordBindings?.FirstOrDefault(b => b.Attributes.Index == index);
                        if (rBinding != null)
                        {
                            rBinding.LoadResults(reader, instance, dbNaming);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds a binding to a list.
        /// </summary>
        private TI AddBinding<TI, TAttr>(ref List<TI> list, Type genClassType, PropertyInfo propertyInfo, TAttr attribute, IDbNaming dbNaming)
            where TI : class
        {
            if (list == null)
            {
                list = new List<TI>();
            }

            var binding = CreateBinding<TI, TAttr>(genClassType, propertyInfo, attribute, dbNaming);
            list.Add(binding);
            return binding;
        }

        /// <summary>
        /// Creates a binding.
        /// </summary>
        private TI CreateBinding<TI, TAttr>(Type genClassType, PropertyInfo propertyInfo, TAttr attribute, IDbNaming dbNaming)
            where TI : class
        {
            var type = genClassType.MakeGenericType(propertyInfo.DeclaringType, propertyInfo.PropertyType);
            var binding = Activator.CreateInstance(type, new object[] { propertyInfo, attribute, dbNaming });
            return binding as TI;
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
