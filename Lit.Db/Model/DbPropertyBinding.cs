using System;
using System.Collections.Generic;
using System.Reflection;
using Lit.DataType;

namespace Lit.Db.Model
{
    /// <summary>
    /// Db property binding.
    /// </summary>
    public interface IDbPropertyBinding<TA> : IPropertyBinding
    {
        /// <summary>
        /// Property information.
        /// </summary>
        PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Database data type.
        /// </summary>
        DbDataType DataType { get; }

        /// <summary>
        /// Attributes.
        /// </summary>
        TA Attributes { get; }
    }

    /// <summary>
    /// Binding to a database property (parameter/field).
    /// </summary>
    internal abstract class DbPropertyBinding<TC, TP, TA> : PropertyBinding<TC, TP>, IDbPropertyBinding<TA>
        where TC : class
        where TA : Attribute
    {
        /// <summary>
        /// Database data type.
        /// </summary>
        public DbDataType DataType => dataType;

        private readonly DbDataType dataType;

        /// <summary>
        /// Attributes.
        /// </summary>
        public TA Attributes => attr;

        private readonly TA attr;

        protected readonly IDbSetup Setup;

        #region Constructor

        protected DbPropertyBinding(IDbSetup setup, PropertyInfo propInfo, TA attr, bool? isNullableForced = null)
            : base(propInfo, true, true, isNullableForced)
        {
            this.Setup = setup;
            this.attr = attr;

            switch (Mode)
            {
                case BindingMode.None:
                default:
                    dataType = DbDataType.Unknown;
                    break;

                case BindingMode.Scalar:
                    dataType = GetDataType(BindingType);
                    break;

                case BindingMode.Class:
                case BindingMode.List:
                case BindingMode.Dictionary:
                    dataType = DbDataType.Json;
                    break;
            }

            if (dataType == DbDataType.Unknown)
            {
                throw new ArgumentException($"Property {this} of type [{propInfo.PropertyType.Name}] has an unknown binding mode");
            }
        }

        #endregion

        /// <summary>
        /// Value decoding from property type.
        /// </summary>
        protected override object DecodePropertyValue(TP value)
        {
            try
            {
                if (value == null)
                {
                    return DBNull.Value;
                }

                return Setup.Translation.ToDb(dataType, BindingType, value);
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
            try
            {
                if (value == null || value is DBNull)
                {
                    return default;
                }

                return (TP)Setup.Translation.FromDb(dataType, BindingType, value);
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
