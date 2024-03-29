﻿using System.Reflection;

namespace Lit.Db
{
    /// <summary>
    /// Db record property binding.
    /// </summary>
    internal class DbRecordBinding<TC, TP> : DbPropertyBinding<TC, TP, DbRecordAttribute>, IDbRecordBinding
        where TC : class
    {
        /// <summary>
        /// Values translation (to/from db).
        /// </summary>
        protected override bool ValuesTranslation => false;

        #region Constructor

        public DbRecordBinding(IDbSetup setup, PropertyInfo propInfo, DbRecordAttribute attr)
            : base(setup, propInfo, attr, false, true)
        {
        }

        #endregion
    }
}
