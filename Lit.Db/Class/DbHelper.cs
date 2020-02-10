using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Lit.Db.Class
{
    /// <summary>
    /// Helper class.
    /// </summary>
    public static class DbHelper
    {
        /// <summary>
        /// Assigns an input parameter by name.
        /// </summary>
        public static void SetSqlParameter(SqlCommand cmd, string paramName, object value)
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
        public static object GetSqlParameter(SqlCommand cmd, string paramName)
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
        public static object LoadSqlRecordset(SqlDataReader reader, Type type, int maxCount)
        {
            var binding = DbTemplateBinding.Get(type);
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
