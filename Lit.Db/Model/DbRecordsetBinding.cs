﻿using System.Reflection;

namespace Lit.Db
{
    /// <summary>
    /// Db recordset property binding.
    /// </summary>
    internal class DbRecordsetBinding<TC, TP> : DbPropertyBinding<TC, TP, DbRecordsetAttribute>, IDbRecordsetBinding
        where TC : class
    {
        /// <summary>
        /// Values translation (to/from db).
        /// </summary>
        protected override bool ValuesTranslation => false;

        #region Constructor

        public DbRecordsetBinding(IDbSetup setup, PropertyInfo propInfo, DbRecordsetAttribute attr)
            : base(setup, propInfo, attr, false, true)
        {
        }

        #endregion
    }
}
