using System.Collections.Generic;

namespace Lit.Db
{
    /// <summary>
    /// Data access definition.
    /// </summary>
    public interface IDbDataAccess : IDbHost
    {
        /// <summary>
        /// Gets a record by id.
        /// </summary>
        T Get<T, TID>(TID id)
            where T : new();

        /// <summary>
        /// Finds a record by code.
        /// </summary>
        T Find<T>(string code)
            where T : new();

        /// <summary>
        /// Deletes a record by id.
        /// </summary>
        void Delete<T, TID>(TID id)
            where T : new();

        /// <summary>
        /// Gets a record by primary key.
        /// </summary>
        bool Get<T>(T record);

        /// <summary>
        /// Finds a record by unique key.
        /// </summary>
        bool Find<T>(T record);

        /// <summary>
        /// Inserts a record.
        /// </summary>
        void Insert<T>(T record);

        /// <summary>
        /// Updates a record.
        /// </summary>
        void Update<T>(T record);

        /// <summary>
        /// Inserts or updates a record.
        /// </summary>
        void Store<T>(T record);

        /// <summary>
        /// Deletes a record.
        /// </summary>
        void Delete<T>(T record);

        /// <summary>
        /// List all records.
        /// </summary>
        List<T> List<T>();

        /// <summary>
        /// Sets the record id.
        /// </summary>
        TID GetId<T, TID>(T record);

        /// <summary>
        /// Gets the record code.
        /// </summary>
        string GetCode<T>(T record);

        /// <summary>
        /// Sets the record id.
        /// </summary>
        void SetId<T, TID>(T record, TID id);

        /// <summary>
        /// Sets the record code.
        /// </summary>
        void SetCode<T>(T record, string code);
    }
}
