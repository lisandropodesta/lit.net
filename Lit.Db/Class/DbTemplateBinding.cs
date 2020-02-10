using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Lit.DataType;
using Lit.Db.Attributes;

namespace Lit.Db.Class
{
    /// <summary>
    /// Stored procedure template information.
    /// </summary>
    public class DbTemplateBinding
    {
        #region Global cache

        // Templates cache
        private static readonly Dictionary<Type, DbTemplateBinding> templateBindings = new Dictionary<Type, DbTemplateBinding>();

        /// <summary>
        /// Gets the template binding information.
        /// </summary>
        public static DbTemplateBinding Get(Type type)
        {
            lock (templateBindings)
            {
                if (!templateBindings.TryGetValue(type, out var template))
                {
                    template = new DbTemplateBinding(type);
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

        private readonly List<IDbParameterBinding> parameterBindings;

        private readonly List<IDbFieldBinding> fieldBindings;

        private readonly List<IDbRecordBinding> recordBindings;

        private readonly List<IDbRecordsetBinding> recordsetBindings;

        #region Constructor

        private DbTemplateBinding(Type templateType)
        {
            this.templateType = templateType;
            mode = DbExecutionMode.NonQuery;

            var attr = TypeHelper.GetAttribute<DbStoredProcedureAttribute>(templateType);
            storedProcedureName = attr?.Name;

            foreach (var propInfo in templateType.GetProperties())
            {
                if (TypeHelper.GetAttribute<DbRecordsetAttribute>(propInfo, out var rsAttr))
                {
                    mode = DbExecutionMode.Query;
                    AssertRecordsetIndex(rsAttr.Index);
                    AddBinding(ref recordsetBindings, typeof(DbRecordsetBinding<,>), propInfo, rsAttr);
                }
                else if (TypeHelper.GetAttribute<DbRecordAttribute>(propInfo, out var rAttr))
                {
                    mode = DbExecutionMode.Query;
                    AssertRecordsetIndex(rsAttr.Index);
                    AddBinding(ref recordBindings, typeof(DbRecordBinding<,>), propInfo, rAttr);
                }
                else if (TypeHelper.GetAttribute<DbFieldAttribute>(propInfo, out var fAttr))
                {
                    mode = DbExecutionMode.Query;
                    AddBinding(ref fieldBindings, typeof(DbFieldBinding<,>), propInfo, fAttr);
                }
                else if (TypeHelper.GetAttribute<DbParameterAttribute>(propInfo, out var pAttr))
                {
                    AddBinding(ref parameterBindings, typeof(DbRecordBinding<,>), propInfo, pAttr);
                }
            }
        }

        #endregion

        /// <summary>
        /// Assigns all input parameters on the command.
        /// </summary>
        public void SetInputParameters(SqlCommand cmd, object instance)
        {
            parameterBindings?.ForEach(b => b.SetInputParameters(cmd, instance));
        }

        /// <summary>
        /// Assigns all output parameters on the template instance.
        /// </summary>
        public void GetOutputParameters(SqlCommand cmd, object instance)
        {
            parameterBindings?.ForEach(b => b.GetOutputParameters(cmd, instance));
        }

        /// <summary>
        /// Get output fields.
        /// </summary>
        public void GetOutputFields(SqlDataReader reader, object instance)
        {
            fieldBindings?.ForEach(b => b.GetOutputField(reader, instance));
        }

        /// <summary>
        /// Load results returned from stored procedure.
        /// </summary>
        public void LoadResults(SqlDataReader reader, object instance)
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
                    if (index > 0 && reader.NextResult())
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
        /// Adds a binding to a list.
        /// </summary>
        private TI AddBinding<TI, TAttr>(ref List<TI> list, Type genClassType, PropertyInfo propertyInfo, TAttr attribute)
            where TI : class
        {
            if (list == null)
            {
                list = new List<TI>();
            }

            var binding = CreateBinding<TI, TAttr>(genClassType, propertyInfo, attribute);
            list.Add(binding);
            return binding;
        }

        /// <summary>
        /// Creates a binding.
        /// </summary>
        private TI CreateBinding<TI, TAttr>(Type genClassType, PropertyInfo propertyInfo, TAttr attribute)
            where TI : class
        {
            var type = genClassType.MakeGenericType(propertyInfo.DeclaringType, propertyInfo.PropertyType);
            var binding = Activator.CreateInstance(type, new object[] { propertyInfo, attribute });
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
