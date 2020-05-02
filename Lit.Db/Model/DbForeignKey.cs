using System;

namespace Lit.Db
{
    /// <summary>
    /// Foreign key property.
    /// </summary>
    public class DbForeignKey<TR, TK> : IDbForeignKeyRef<TR>
        where TR : class, new()
    {
        /// <summary>
        /// Data access reference.
        /// </summary>
        public IDbDataAccess Db { get; set; }

        /// <summary>
        /// Key value.
        /// </summary>
        public TK Key { get => key; set => SetKey(value); }

        private TK key;

        /// <summary>
        /// Key value as object
        /// </summary>
        object IDbForeignKeyRef.KeyAsObject { get => Key; set { Key = value != null ? (TK)value : default; } }

        /// <summary>
        /// Primary record.
        /// </summary>
        public TR Record => GetRecord();

        private WeakReference<TR> weakReference;

        #region Constructor

        public DbForeignKey()
        {
        }

        #endregion

        /// <summary>
        /// Key update.
        /// </summary>
        private void SetKey(TK value)
        {
            if (key == null && value != null || key != null && !key.Equals(value))
            {
                key = value;
                weakReference = null;
            }
        }

        /// <summary>
        /// Tries to load the record.
        /// </summary>
        private TR GetRecord()
        {
            TR record = null;
            if (!weakReference?.TryGetTarget(out record) ?? true)
            {
                var db = Db;
                if (db != null)
                {
                    record = db.Get<TR, TK>(key);
                    if (weakReference == null)
                    {
                        weakReference = new WeakReference<TR>(record);
                    }
                    else
                    {
                        weakReference.SetTarget(record);
                    }
                }
            }

            return record;
        }

        /// <summary>
        /// ToString gets the key value.
        /// </summary>
        public override string ToString()
        {
            return Key.ToString();
        }
    }
}
