using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Lit.DataType;
using Lit.Db.Attributes;

namespace Lit.Db.Model
{
    /// <summary>
    /// Stored procedure template information.
    /// </summary>
    internal class DbTemplateBinding<TS>
        where TS : DbCommand
    {
        #region Global cache

        // Templates cache
        private static readonly Dictionary<Type, DbTemplateBinding<TS>> templateBindings = new Dictionary<Type, DbTemplateBinding<TS>>();

        /// <summary>
        /// Gets the template binding information.
        /// </summary>
        public static DbTemplateBinding<TS> Get(Type type, IDbNaming dbNaming)
        {
            lock (templateBindings)
            {
                if (!templateBindings.TryGetValue(type, out var template))
                {
                    template = new DbTemplateBinding<TS>(type, dbNaming);
                    templateBindings.Add(type, template);
                }

                return template;
            }
        }

        #endregion

        /// <summary>
        /// Template data type.
        /// </summary>
        public Type TemplateType => templateType;

        private readonly Type templateType;

        /// <summary>
        /// Stored procedure name.
        /// </summary>
        public string StoredProcedureName => storedProcedureName;

        private string storedProcedureName;

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

        private readonly List<IDbParameterBinding<TS>> parameterBindings;

        private readonly List<IDbFieldBinding> fieldBindings;

        private readonly List<IDbRecordBinding<TS>> recordBindings;

        private readonly List<IDbRecordsetBinding> recordsetBindings;

        #region Constructor

        private DbTemplateBinding(Type templateType, IDbNaming dbNaming)
        {
            this.templateType = templateType;
            mode = DbExecutionMode.NonQuery;

            var attr = TypeHelper.GetAttribute<DbStoredProcedureAttribute>(templateType);
            storedProcedureName = attr?.StoredProcedureName;

            foreach (var propInfo in templateType.GetProperties())
            {
                if (TypeHelper.GetAttribute<DbRecordsetAttribute>(propInfo, out var rsAttr))
                {
                    mode = DbExecutionMode.Query;
                    AssertRecordsetIndex(rsAttr.Index);
                    AddBinding(ref recordsetBindings, typeof(DbRecordsetBinding<,,>), propInfo, rsAttr, dbNaming);
                }
                else if (TypeHelper.GetAttribute<DbRecordAttribute>(propInfo, out var rAttr))
                {
                    mode = DbExecutionMode.Query;
                    AssertRecordsetIndex(rsAttr.Index);
                    AddBinding(ref recordBindings, typeof(DbRecordBinding<,,>), propInfo, rAttr, dbNaming);
                }
                else if (TypeHelper.GetAttribute<DbFieldAttribute>(propInfo, out var fAttr))
                {
                    mode = DbExecutionMode.Query;
                    AddBinding(ref fieldBindings, typeof(DbFieldBinding<,,>), propInfo, fAttr, dbNaming);
                }
                else if (TypeHelper.GetAttribute<DbParameterAttribute>(propInfo, out var pAttr))
                {
                    AddBinding(ref parameterBindings, typeof(DbParameterBinding<,,>), propInfo, pAttr, dbNaming);
                }
            }
        }

        #endregion

        /// <summary>
        /// Assigns all input parameters on the command.
        /// </summary>
        public void SetInputParameters(TS cmd, object instance)
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
        public void GetOutputParameters(TS cmd, object instance)
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
            var type = genClassType.MakeGenericType(typeof(TS), propertyInfo.DeclaringType, propertyInfo.PropertyType);
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
