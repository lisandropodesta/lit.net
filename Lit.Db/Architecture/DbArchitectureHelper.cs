using System;

namespace Lit.Db.Architecture
{
    /// <summary>
    /// Db architecture helper.
    /// </summary>
    public static class DbArchitectureHelper
    {
        /// <summary>
        /// Get default prefix and suffix for a stored procedure function.
        /// </summary>
        public static void GetDefaultStoredProcedureAffixes(StoredProcedureFunction spFunc, out string prefix, out string suffix)
        {
            switch (spFunc)
            {
                case StoredProcedureFunction.Get:
                    prefix = "get_";
                    suffix = string.Empty;
                    break;

                case StoredProcedureFunction.GetByCode:
                    prefix = "get_";
                    suffix = "_by_code";
                    break;

                case StoredProcedureFunction.ListAll:
                    prefix = "lst_";
                    suffix = string.Empty;
                    break;

                case StoredProcedureFunction.Insert:
                    prefix = "ins_";
                    suffix = string.Empty;
                    break;

                case StoredProcedureFunction.Update:
                    prefix = "upd_";
                    suffix = string.Empty;
                    break;

                case StoredProcedureFunction.Set:
                    prefix = "set_";
                    suffix = string.Empty;
                    break;

                case StoredProcedureFunction.Delete:
                    prefix = "del_";
                    suffix = string.Empty;
                    break;

                default:
                    throw new NotImplementedException($"Stored procedure func {spFunc}");
            }
        }
    }
}
