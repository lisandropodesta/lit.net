﻿using System;
using System.Data;

namespace Lit.Db.MySql
{
    /// <summary>
    /// MySql template.
    /// </summary>
    public class MySqlTemplate : DbTemplate
    {
        #region String constants

        public const string AllColumns = "*";

        public const string NullKey = "NULL";

        public const string NotNullKey = "NOT NULL";

        public const string AutoIncrementKey = "AUTO_INCREMENT";

        public const string KeyKey = "KEY";

        public const string IndexKey = "INDEX";

        public const string ConstraintKey = "CONSTRAINT";

        public const string PrimaryKeyKey = "PRIMARY KEY";

        public const string ForeignKeyKey = "FOREIGN KEY";

        public const string UniqueKeyKey = "UNIQUE KEY";

        public const string ReferecencesKey = "REFERENCES";

        public const string DefaultKey = "DEFAULT";

        public const string IfExistsKey = "IF EXISTS";

        public const string InKey = "IN";

        public const string OutKey = "OUT";

        public const string InOutKey = "INOUT";

        #endregion

        /// <summary>
        /// Get MySql direction.
        /// </summary>
        protected string GetDirection(ParameterDirection direction)
        {
            switch (direction)
            {
                case ParameterDirection.Input:
                    return InKey;

                case ParameterDirection.InputOutput:
                    return InOutKey;

                case ParameterDirection.Output:
                    return OutKey;

                case ParameterDirection.ReturnValue:
                default:
                    throw new ArgumentException();
            }
        }
    }
}