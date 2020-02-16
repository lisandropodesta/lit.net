using System;
using System.Data.Common;
using System.Reflection;
using Lit.Db.Attributes;

namespace Lit.Db.Model
{
    /// <summary>
    /// Db field binding interface.
    /// </summary>
    internal interface IDbFieldBinding : IDbPropertyBinding<DbFieldAttribute>
    {
        /// <summary>
        /// Get output field.
        /// </summary>
        void GetOutputField(DbDataReader reader, object instance);
    }

    /// <summary>
    /// Db field property binding.
    /// </summary>
    internal class DbFieldBinding<TS, TC, TP> : DbPropertyBinding<TC, TP, DbFieldAttribute>, IDbFieldBinding
        where TS : DbCommand
        where TC : class
    {
        private readonly string fieldName;

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
