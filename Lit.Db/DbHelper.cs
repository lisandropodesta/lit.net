using System;
using System.Data.Common;

namespace Lit.Db
{
    /// <summary>
    /// Helper class.
    /// </summary>
    public static class DbHelper
    {
        /// <summary>
        /// Assigns an input parameter by name.
        /// </summary>
        public static void SetSqlParameter(ref string query, string paramName, string value, bool isOptional)
        {
            var replaceText = @"{{@" + paramName + @"}}";
            var found = false;
            int index;

            do
            {
                index = query.IndexOf(replaceText, StringComparison.OrdinalIgnoreCase);
                if (index >= 0)
                {
                    var leftPart = query.Substring(0, index);
                    var rightPart = query.Substring(index + replaceText.Length);

                    if (value.Contains("\n\t"))
                    {
                        var nlIndex = Math.Max(leftPart.LastIndexOf("\n"), 0);
                        if (nlIndex < leftPart.Length - 1)
                        {
                            value = value.Replace("\n\t", "\n" + new string(' ', leftPart.Length - 1 - nlIndex));
                        }
                    }

                    query = leftPart + value + rightPart;
                    found = true;
                }
            }
            while (index >= 0);

            if (!found && !isOptional)
            {
                throw new ArgumentException($"Parameter '{paramName}' not found on sql command");
            }
        }

        /// <summary>
        /// Assigns an input parameter by name.
        /// </summary>
        public static void SetSqlParameter(DbCommand cmd, string paramName, object value, bool isOptional)
        {
            paramName = "@" + paramName;

            if (!cmd.Parameters.Contains(paramName))
            {
                if (isOptional)
                {
                    return;
                }

                throw new ArgumentException($"Parameter '{paramName}' not found on sql command");
            }

            cmd.Parameters[paramName].Value = value;
        }

        /// <summary>
        /// Gets an output parameter by name.
        /// </summary>
        public static object GetSqlParameter(DbCommand cmd, string paramName)
        {
            paramName = "@" + paramName;

            if (!cmd.Parameters.Contains(paramName))
            {
                throw new ArgumentException($"Parameter '{paramName}' not found on sql command");
            }

            return cmd.Parameters[paramName].Value;
        }

        /// <summary>
        /// Creates an instance of a db object and optionally assigns the db reference.
        /// </summary>
        public static object CreateInstance(IDbDataAccess db, Type type, params object[] args)
        {
            var result = Activator.CreateInstance(type, args);
            SetDbRef(result, db);
            return result;
        }

        /// <summary>
        /// Assigns data access reference.
        /// </summary>
        public static T SetDbRef<T>(T obj, IDbDataAccess db)
        {
            if (obj is IDbDataAccessRef dbRef && db != null)
            {
                dbRef.Db = db;
            }

            return obj;
        }

        /// <summary>
        /// Creates an instance of a foreign key to a table.
        /// </summary>
        public static IDbForeignKeyRef<T> CreateKey<T>(IDbDataAccess db, object key, bool? isNullableForced = null)
        {
            return CreateKey(db, typeof(T), key, isNullableForced) as IDbForeignKeyRef<T>;
        }

        /// <summary>
        /// Creates an instance of a foreign key to a table.
        /// </summary>
        public static IDbForeignKeyRef CreateKey(IDbDataAccess db, Type tableType, object key, bool? isNullableForced = null)
        {
            var type = GetPrimaryKeyType(db.Setup, tableType, isNullableForced, out Type _);
            var instance = Activator.CreateInstance(type) as IDbForeignKeyRef;
            instance.Db = db;
            instance.KeyAsObject = key;
            return instance;
        }

        /// <summary>
        /// Translates a foreign key column type.
        /// </summary>
        public static Type TranslateForeignKeyType(IDbSetup setup, ref Type columnType, bool? isNullableForced)
        {
            var primaryTable = GetForeignKeyPropType(columnType);
            return primaryTable != null ? GetPrimaryKeyType(setup, primaryTable, isNullableForced, out columnType) : null;
        }

        /// <summary>
        /// Check if the property is a foreign key.
        /// </summary>
        public static Type GetForeignKeyPropType(Type propType)
        {
            return propType.IsGenericType
                && propType.GetGenericArguments().Length == 1
                && propType.GetGenericTypeDefinition() == typeof(IDbForeignKeyRef<>) ? propType.GetGenericArguments()[0] : null;
        }

        /// <summary>
        /// Get the primary key type.
        /// </summary>
        public static Type GetPrimaryKeyType(IDbSetup setup, Type tableType, bool? isNullableForced, out Type columnType)
        {
            var col = GetPrimaryKeyColumn(setup, tableType);
            columnType = col.ColumnType;
            if (col.IsNullable || isNullableForced == true)
            {
                columnType = typeof(Nullable<>).MakeGenericType(columnType);
            }

            return typeof(DbForeignKey<,>).MakeGenericType(tableType, columnType);
        }

        /// <summary>
        /// Get the primary key column type.
        /// </summary>
        public static IDbColumnBinding GetPrimaryKeyColumn(IDbSetup setup, Type tableType)
        {
            var binding = setup.GetTableBinding(tableType);
            var col = binding?.SingleColumnPrimaryKey;

            if (col == null)
            {
                throw new ArgumentException($"Unknown type/foreign key for [{tableType.Name}]");
            }

            return col;
        }
    }
}
