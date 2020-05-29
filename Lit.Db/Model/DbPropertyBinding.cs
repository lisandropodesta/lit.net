using System;
using System.Collections.Generic;
using System.Reflection;
using Lit.DataType;

namespace Lit.Db
{
    /// <summary>
    /// Binding to a database property (parameter/field).
    /// </summary>
    internal abstract class DbPropertyBinding<TC, TP, TA> : PropertyBinding<TC, TP>, IDbPropertyBinding<TA>
        where TC : class
        where TA : Attribute
    {
        /// <summary>
        /// Values translation (to/from db).
        /// </summary>
        protected abstract bool ValuesTranslation { get; }

        /// <summary>
        /// Shortcut for property name.
        /// </summary>
        public string PropertyName => PropertyInfo.Name;

        /// <summary>
        /// Database data type.
        /// </summary>
        public DbDataType DataType { get; private set; }

        /// <summary>
        /// Attributes.
        /// </summary>
        public TA Attributes { get; private set; }

        /// <summary>
        /// Foreign key property flag.
        /// </summary>
        public bool IsForeignKeyProp { get; private set; }

        // Type of the class that will hold this property
        private Type foreignKeyPropType;

        /// <summary>
        /// Key constraint.
        /// </summary>
        public DbKeyConstraint KeyConstraint { get; private set; }

        /// <summary>
        /// Primary table template (when this property is a foreign key).
        /// </summary>
        public Type PrimaryTableTemplate { get; protected set; }

        protected readonly IDbSetup Setup;

        /// <summary>
        /// Forced IsNullable value.
        /// </summary>
        protected virtual bool? IsNullableForced => null;

        #region Constructor

        protected DbPropertyBinding(IDbTemplateBinding binding, PropertyInfo propInfo, TA attr)
            : base(propInfo, true, true)
        {
            this.Setup = binding.Setup;
            this.Attributes = attr;

            PrimaryTableTemplate = DbHelper.GetForeignKeyPropType(PropertyInfo.PropertyType);
            IsForeignKeyProp = PrimaryTableTemplate != null;
            GetKeyConstraints(attr);
        }

        #endregion

        /// <summary>
        /// Get key constraints.
        /// </summary>
        private void GetKeyConstraints(TA attr)
        {
            if (attr is DbPrimaryKeyAttribute)
            {
                if (IsNullable)
                {
                    throw new ArgumentException($"Primary key can not be nullable on property [{PropertyInfo.DeclaringType.Namespace}.{PropertyInfo.DeclaringType.Name}.{PropertyInfo.Name}]");
                }

                if (attr is DbPrimaryAndForeignKeyAttribute pfk)
                {
                    KeyConstraint = DbKeyConstraint.PrimaryForeignKey;
                    PrimaryTableTemplate = pfk.PrimaryTableTemplate;
                }
                else if (IsForeignKeyProp)
                {
                    KeyConstraint = DbKeyConstraint.ForeignKey;
                }
                else
                {
                    KeyConstraint = DbKeyConstraint.PrimaryKey;
                }
            }
            else if (attr is DbForeignKeyAttribute fk)
            {
                KeyConstraint = DbKeyConstraint.ForeignKey;
                PrimaryTableTemplate = fk.PrimaryTableTemplate;
            }
            else if (IsForeignKeyProp)
            {
                KeyConstraint = DbKeyConstraint.ForeignKey;
            }
            else if (attr is DbUniqueKeyAttribute)
            {
                KeyConstraint = DbKeyConstraint.UniqueKey;
            }
            else
            {
                KeyConstraint = DbKeyConstraint.None;
            }
        }

        /// <summary>
        /// Get the binding mode.
        /// </summary>
        protected override BindingMode GetBindingMode(ref Type bindingType, out bool isNullable)
        {
            foreignKeyPropType = DbHelper.TranslateForeignKeyType(Setup, ref bindingType, IsNullableForced);

            var mode = TypeHelper.GetBindingMode(ref bindingType, out isNullable);

            if (IsNullableForced.HasValue)
            {
                isNullable = IsNullableForced.Value;
            }

            switch (mode)
            {
                case BindingMode.None:
                default:
                    DataType = DbDataType.Unknown;
                    break;

                case BindingMode.Scalar:
                    DataType = GetDataType(bindingType);
                    break;

                case BindingMode.Class:
                case BindingMode.List:
                case BindingMode.Dictionary:
                    DataType = DbDataType.Json;
                    break;
            }

            if (DataType == DbDataType.Unknown)
            {
                throw new ArgumentException($"Property {this} of type [{PropertyName}] has an unknown binding mode");
            }

            return mode;
        }

        /// <summary>
        /// Gets the binding value from an instance.
        /// </summary>
        public override object GetValue(object instance)
        {
            if (IsForeignKeyProp)
            {
                var value = (GetRawValue(instance) as IDbForeignKeyRef)?.KeyAsObject;
                return DecodePropertyValue(value, false);
            }

            return base.GetValue(instance);
        }

        /// <summary>
        /// Sets the binding value to an instance.
        /// </summary>
        public override void SetValue(object instance, object value)
        {
            if (IsForeignKeyProp)
            {
                var fk = GetRawValue(instance) as IDbForeignKeyRef;
                if (fk == null)
                {
                    var prop = Activator.CreateInstance(foreignKeyPropType);
                    SetRawValue(instance, prop);
                    fk = prop as IDbForeignKeyRef;
                    fk.Db = (instance as IDbDataAccessRef)?.Db;
                }

                fk.KeyAsObject = EncodePropertyValue(value, false);
            }
            else
            {
                base.SetValue(instance, value);
            }
        }

        /// <summary>
        /// Value decoding from property type.
        /// </summary>
        protected override object DecodePropertyValue(TP value)
        {
            return DecodePropertyValue(value, ValuesTranslation);
        }

        /// <summary>
        /// Value decoding from property type.
        /// </summary>
        protected object DecodePropertyValue(object value, bool translate)
        {
            try
            {
                if (value == null)
                {
                    return DBNull.Value;
                }

                return translate ? Setup.Translation.ToDb(DataType, BindingType, value) : value;
            }
            catch
            {
                throw new ArgumentException($"Unable to decode property {this} of type [{PropertyInfo.PropertyType.Name}] and binding [{BindingType}].");
            }
        }

        /// <summary>
        /// Value encoding to property type.
        /// </summary>
        protected override TP EncodePropertyValue(object value)
        {
            return (TP)EncodePropertyValue(value, ValuesTranslation);
        }

        /// <summary>
        /// Value encoding to property type.
        /// </summary>
        protected object EncodePropertyValue(object value, bool translate)
        {
            try
            {
                if (value == null || value is DBNull)
                {
                    return default;
                }

                return translate ? Setup.Translation.FromDb(DataType, BindingType, value) : value;
            }
            catch
            {
                throw new ArgumentException($"Unable to encode property {this} of type [{PropertyInfo.PropertyType.Name}] and binding [{BindingType}].");
            }
        }

        /// <summary>
        /// Get supported data type code.
        /// </summary>
        private DbDataType GetDataType(Type bindingType)
        {
            if (bindingType.IsEnum)
            {
                return DbDataType.Enumerated;
            }

            return dbScalarTypes.TryGetValue(bindingType, out var dataType) ? dataType : DbDataType.Unknown;
        }

        // Supported scalar types.
        private static readonly Dictionary<Type, DbDataType> dbScalarTypes = new Dictionary<Type, DbDataType>
        {
            { typeof(bool), DbDataType.Boolean },
            { typeof(char), DbDataType.Char },

            { typeof(sbyte), DbDataType.SInt8 },
            { typeof(byte), DbDataType.UInt8 },
            { typeof(short), DbDataType.SInt16 },
            { typeof(ushort), DbDataType.UInt16 },
            { typeof(int), DbDataType.SInt32 },
            { typeof(uint), DbDataType.UInt32 },
            { typeof(long), DbDataType.SInt64 },
            { typeof(ulong), DbDataType.UInt64 },

            { typeof(decimal), DbDataType.Decimal },
            { typeof(float), DbDataType.Float },
            { typeof(double), DbDataType.Double },

            { typeof(DateTime), DbDataType.DateTime },
            { typeof(DateTimeOffset), DbDataType.Timestamp },
            { typeof(TimeSpan), DbDataType.TimeSpan },

            { typeof(string), DbDataType.Text },
            { typeof(byte[]), DbDataType.Blob }
        };
    }
}
