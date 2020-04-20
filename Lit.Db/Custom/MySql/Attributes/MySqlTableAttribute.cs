using System;

namespace Lit.Db.Custom.MySql
{
    /// <summary>
    /// Engine specification.
    /// </summary>
    public class MySqlTableAttribute : Attribute
    {
        /// <summary>
        /// Engine.
        /// </summary>
        public Engine? Engine { get; private set; }

        /// <summary>
        /// Default character set.
        /// </summary>
        public string DefaultCharset { get; private set; }

        #region Constructors

        public MySqlTableAttribute(string defaultCharset)
        {
            DefaultCharset = defaultCharset;
        }

        public MySqlTableAttribute(Engine engine, string defaultCharset = null)
        {
            Engine = engine;
            DefaultCharset = defaultCharset;
        }

        #endregion
    }
}
