﻿using System;

namespace Lit.Db
{
    public interface IDbForeignKeyAttribute
    {
        /// <summary>
        /// Foreign table template.
        /// </summary>
        Type ForeignTableTemplate { get; }

        /// <summary>
        /// Foreign column property name.
        /// </summary>
        string ForeignColumnProperty { get; }
    }
}
