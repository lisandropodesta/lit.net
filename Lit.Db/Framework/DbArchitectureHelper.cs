using System;

namespace Lit.Db.Framework
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
                    prefix = "get";
                    suffix = string.Empty;
                    break;

                case StoredProcedureFunction.GetByCode:
                    prefix = "getc";
                    suffix = string.Empty;
                    break;

                case StoredProcedureFunction.ListAll:
                    prefix = "lst";
                    suffix = string.Empty;
                    break;

                case StoredProcedureFunction.Insert:
                    prefix = "ins";
                    suffix = string.Empty;
                    break;

                case StoredProcedureFunction.InsertGet:
                    prefix = "insr";
                    suffix = string.Empty;
                    break;

                case StoredProcedureFunction.Update:
                    prefix = "upd";
                    suffix = string.Empty;
                    break;

                case StoredProcedureFunction.UpdateGet:
                    prefix = "updr";
                    suffix = string.Empty;
                    break;

                case StoredProcedureFunction.Set:
                    prefix = "set";
                    suffix = string.Empty;
                    break;

                case StoredProcedureFunction.SetGet:
                    prefix = "setr";
                    suffix = string.Empty;
                    break;

                case StoredProcedureFunction.Delete:
                    prefix = "del";
                    suffix = string.Empty;
                    break;

                default:
                    throw new NotImplementedException($"Stored procedure func {spFunc}");
            }
        }
    }
}
