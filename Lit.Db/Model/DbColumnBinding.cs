using System;
using System.Reflection;
using Lit.DataType;

namespace Lit.Db
{
    internal static class DbColumnBinding
    {
        /// <summary>
        /// Creates an instance of a column binding.
        /// </summary>
        public static IDbColumnBinding CreateInstance(IDbSetup setup, PropertyInfo propInfo, DbColumnAttribute attr, IDbTableBinding table)
        {
            return TypeHelper.CreateInstance(typeof(DbColumnBinding<,>), new[] { propInfo.DeclaringType, propInfo.PropertyType }, setup, propInfo, attr, table) as IDbColumnBinding;
        }
    }

    /// <summary>
    /// Db field property binding.
    /// </summary>
    internal class DbColumnBinding<TC, TP> : DbPropertyBinding<TC, TP, DbColumnAttribute>, IDbColumnBinding
        where TC : class
    {
        /// <summary>
        /// Values translation (to/from db).
        /// </summary>
        protected override bool ValuesTranslation => true;

        /// <summary>
        /// Column name.
        /// </summary>
        public string ColumnName { get; private set; }

        /// <summary>
        /// Column type.
        /// </summary>
        public Type ColumnType => BindingType;

        /// <summary>
        /// Column size.
        /// </summary>
        public ulong? ColumnSize { get; private set; }

        /// <summary>
        /// Auto increment flag.
        /// </summary>
        public bool IsAutoIncrement => Attributes.AutoIncrement;

        /// <summary>
        /// Name of the related stored procedure parameter.
        /// </summary>
        public string SpParamName { get; private set; }

        /// <summary>
        /// Forced IsNullable value.
        /// </summary>
        protected override bool? IsNullableForced => Attributes.IsNullableForced;

        #region Constructor

        public DbColumnBinding(IDbSetup setup, PropertyInfo propInfo, DbColumnAttribute attr, IDbTableBinding table)
            : base(setup, propInfo, attr, true, true)
        {
            ColumnName = setup.Naming.GetColumnName(table?.TableName, propInfo, Attributes.DbName, KeyConstraint);
            ColumnSize = attr.Size;

            if (string.IsNullOrEmpty(ColumnName))
            {
                throw new ArgumentException($"Null field name in DbColumnBinding at class [{propInfo.DeclaringType.Namespace}.{propInfo.DeclaringType.Name}]");
            }

            SpParamName = setup.Naming.GetParameterName(propInfo, ColumnName, null, false, KeyConstraint);
        }

        #endregion
    }
}
