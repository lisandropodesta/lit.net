using System;

namespace Lit.Db.Attributes
{

    /// <summary>
    /// Primary key attributes definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbForeignKeyAttribute :Attribute
    {
    }
}
