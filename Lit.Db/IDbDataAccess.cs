using System.Collections.Generic;
using Lit.Db.Framework.Entities;

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
        T Get<T>(int id)
            where T : DbTableTemplate, new();

        /// <summary>
        /// Gets a record by id.
        /// </summary>
        T Get<T>(long id)
            where T : DbTableTemplate, new();

        /// <summary>
        /// Finds a record by code.
        /// </summary>
        T Find<T>(string code)
            where T : DbTableTemplate, new();

        /// <summary>
        /// Deletes a record by id.
        /// </summary>
        void Delete<T>(int id)
            where T : DbTableTemplate, new();

        /// <summary>
        /// Gets a record by primary key.
        /// </summary>
        void Get<T>(T record);

        /// <summary>
        /// Finds a record by unique key.
        /// </summary>
        void Find<T>(T record);

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
    }
}
