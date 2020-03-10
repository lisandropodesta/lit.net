using System;

namespace Lit.Db.Attributes
{
    /// <summary>
    /// Enumerated type code.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DbEnumCodeAttribute : Attribute, IDbCode
    {
        /// <summary>
        /// String code.
        /// </summary>
        public string Code => code;

        private string code;

        public DbEnumCodeAttribute(string code)
        {
            this.code = code;
        }
    }
}
