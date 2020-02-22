using System;
using System.Data.Common;
using System.Reflection;
using Lit.Db.Attributes;

namespace Lit.Db.Model
{
    /// <summary>
    /// Db field binding interface.
    /// </summary>
    public interface IDbFieldBinding : IDbPropertyBinding<DbFieldAttribute>
    {
        /// <summary>
        /// Field name.
        /// </summary>
        string FieldName { get; }

        /// <summary>
        /// Field type.
        /// </summary>
        Type FieldType { get; }

        /// <summary>
        /// Nullable flag.
        /// </summary>
        bool IsNullable { get; }

        /// <summary>
        /// Get output field.
        /// </summary>
        void GetOutputField(DbDataReader reader, object instance);
    }

    /// <summary>
    /// Db field property binding.
    /// </summary>
    internal class DbFieldBinding<TC, TP> : DbPropertyBinding<TC, TP, DbFieldAttribute>, IDbFieldBinding
        where TC : class
    {
        /// <summary>
        /// Field name.
        /// </summary>
        public string FieldName => fieldName;

        private readonly string fieldName;

        /// <summary>
        /// Field type.
        /// </summary>
        public Type FieldType => BindingType;

        #region Constructor

        public DbFieldBinding(PropertyInfo propInfo, DbFieldAttribute attr, IDbNaming dbNaming)
            : base(propInfo, attr)
        {
            fieldName = Attributes.FieldName;
            fieldName = dbNaming?.GetFieldName(propInfo, fieldName) ?? fieldName;

            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentException($"Null field name in DbFieldBinding at class [{propInfo.DeclaringType.Namespace}.{propInfo.DeclaringType.Name}]");
            }
        }

        #endregion

        /// <summary>
        /// Get output field.
        /// </summary>
        public void GetOutputField(DbDataReader reader, object instance)
        {
            try
            {
                SetValue(instance, reader[fieldName]);
            }
            catch
            {
                if (!Attributes.IsOptional)
                {
                    throw;
                }
            }
        }
    }
}
