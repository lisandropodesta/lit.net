using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Lit.Db.Model;

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
        public static void SetSqlParameter(DbCommand cmd, string paramName, object value)
        {
            paramName = "@" + paramName;

            if (!cmd.Parameters.Contains(paramName))
            {
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
        /// Builds a generic list and loads a recordset in it.
        /// </summary>
        public static object LoadSqlRecordset<TS>(DbDataReader reader, Type type, int maxCount, IDbNaming dbNaming)
            where TS : DbCommand
        {
            var binding = DbTemplateBinding<TS>.Get(type, dbNaming);
            var listType = typeof(List<>).MakeGenericType(type);
            var result = Activator.CreateInstance(listType);
            var list = result as IList;

            for (var ri = 0; ri < maxCount && reader.Read(); ri++)
            {
                var record = Activator.CreateInstance(type);
                binding.GetOutputFields(reader, record);
                list.Add(record);
            }

            return result;
        }
    }
}
