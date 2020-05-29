using System;
using System.Reflection;
using Lit.Db.Framework;
using Lit.Names;

namespace Lit.Db
{
    /// <summary>
    /// Common naming conventions.
    /// </summary>
    public class DbNaming : IDbNaming
    {
        #region Helper enums

        /// <summary>
        /// Base name source.
        /// </summary>
        public enum Source
        {
            /// <summary>
            /// Configuration is preferred but in case it is missing then usess relection (this is the default).
            /// </summary>
            Default,

            /// <summary>
            /// Only uses configuration.
            /// </summary>
            Configuration,

            /// <summary>
            /// Only uses reflection
            /// </summary>
            Reflection
        }

        /// <summary>
        /// Translation scope.
        /// </summary>
        public enum Translation
        {
            /// <summary>
            /// None source is translated.
            /// </summary>
            None,

            /// <summary>
            /// Only reflection is translated.
            /// </summary>
            Reflection,

            /// <summary>
            /// Only configuration is translated.
            /// </summary>
            Configuration,

            /// <summary>
            /// Allo sources are translated.
            /// </summary>
            All
        }

        #endregion

        /// <summary>
        /// Id text.
        /// </summary>
        public string IdText;

        /// <summary>
        /// Source of text.
        /// </summary>
        public Source TextSource;

        /// <summary>
        /// Scope of translation.
        /// </summary>
        public Translation Scope;

        /// <summary>
        /// Id mode.
        /// </summary>
        public AffixPlacing IdPlacing;

        /// <summary>
        /// Forces id on primary/foreign key columns.
        /// </summary>
        public bool ForceIdOnKeyColumn;

        /// <summary>
        /// Stored procedure affix.
        /// </summary>
        public AffixPlacing StoredProcedureAffix;

        /// <summary>
        /// Naming case for stored procedures parameters.
        /// </summary>
        public Case ParametersCase;

        /// <summary>
        /// Naming case for stored procedures.
        /// </summary>
        public Case StoredProceduresCase;

        /// <summary>
        /// Naming case for table fields.
        /// </summary>
        public Case FieldsCase;

        /// <summary>
        /// Naming case for tables.
        /// </summary>
        public Case TablesCase;

        #region Constructors

        public DbNaming()
        {
        }

        public DbNaming(AffixPlacing idPlacing, Case namingCase, string idText = null)
        {
            TextSource = Source.Default;
            Scope = Translation.Reflection;
            IdPlacing = StoredProcedureAffix = idPlacing;
            ParametersCase = StoredProceduresCase = FieldsCase = TablesCase = namingCase;
            IdText = idText;
        }

        #endregion

        /// <summary>
        /// Gets a parameter name.
        /// </summary>
        public virtual string GetParameterName(PropertyInfo propInfo, string columnName, string parameterName, DbKeyConstraint constraint = DbKeyConstraint.None)
        {
            return TranslateName(TextSource, Scope, propInfo?.Name, parameterName ?? columnName, ParametersCase, constraint, IdPlacing, IdText);
        }

        /// <summary>
        /// Gets a field name.
        /// </summary>
        public virtual string GetFieldName(PropertyInfo propInfo, string fieldName, DbKeyConstraint constraint = DbKeyConstraint.None)
        {
            return TranslateName(TextSource, Scope, propInfo?.Name, fieldName, FieldsCase, constraint, IdPlacing, IdText);
        }

        /// <summary>
        /// Gets a table column name.
        /// </summary>
        public virtual string GetColumnName(string tableName, PropertyInfo propInfo, string columnName, DbKeyConstraint constraint = DbKeyConstraint.None)
        {
            return TranslateName(TextSource, Scope, propInfo.Name, columnName, FieldsCase, constraint, IdPlacing, IdText);
        }

        /// <summary>
        /// Translates a name.
        /// </summary>
        protected virtual string TranslateName(Source source, Translation scope, string reflectionName, string configurationName, Case namingCase, DbKeyConstraint constraint, AffixPlacing idPlacing, params string[] affixes)
        {
            bool forceId;
            switch (constraint)
            {
                case DbKeyConstraint.PrimaryKey:
                case DbKeyConstraint.PrimaryForeignKey:
                case DbKeyConstraint.ForeignKey:
                    forceId = ForceIdOnKeyColumn;
                    break;

                default:
                    forceId = false;
                    break;
            }

            return TranslateName(source, scope, reflectionName, configurationName, namingCase, forceId, idPlacing, affixes);
        }

        /// <summary>
        /// Gets a stored procedure name.
        /// </summary>
        public virtual string GetStoredProcedureName(Type template, string spName)
        {
            return TranslateName(TextSource, Scope, template.Name, spName, StoredProceduresCase, false, AffixPlacing.DoNotChange, null);
        }

        /// <summary>
        /// Gets a table stored procedure name.
        /// </summary>
        public virtual string GetStoredProcedureName(string tableName, StoredProcedureFunction spFunc)
        {
            GetStoredProcedureAffixes(spFunc, out var prefix, out var suffix);

            string name;
            switch (StoredProcedureAffix)
            {
                case AffixPlacing.Prefix:
                    name = Concat(prefix, suffix, tableName);
                    break;

                case AffixPlacing.Sufix:
                    name = Concat(tableName, prefix, suffix);
                    break;

                default:
                    name = Concat(prefix, tableName, suffix);
                    break;
            }

            return TranslateName(TextSource, Scope, null, name, StoredProceduresCase, false, AffixPlacing.DoNotChange, null);
        }

        /// <summary>
        /// Get table stored procedure affixes.
        /// </summary>
        protected virtual void GetStoredProcedureAffixes(StoredProcedureFunction spFunc, out string prefix, out string suffix)
        {
            DbArchitectureHelper.GetDefaultStoredProcedureAffixes(spFunc, out prefix, out suffix);
        }

        /// <summary>
        /// Gets a table name.
        /// </summary>
        public virtual string GetTableName(Type template, string tableName)
        {
            return TranslateName(TextSource, Scope, template.Name, tableName, TablesCase, false, AffixPlacing.DoNotChange, null);
        }

        /// <summary>
        /// Translates a name.
        /// </summary>
        public static string TranslateName(Source source, Translation scope, string reflectionName, string configurationName, Case namingCase, bool forceId, AffixPlacing idPlacing, params string[] affixes)
        {
            string name;

            switch (source)
            {
                case Source.Default:
                default:
                    if (!string.IsNullOrEmpty(configurationName))
                    {
                        source = Source.Configuration;
                        name = configurationName;
                    }
                    else
                    {
                        source = Source.Reflection;
                        name = reflectionName;
                    }
                    break;

                case Source.Configuration:
                    name = configurationName;
                    break;

                case Source.Reflection:
                    name = reflectionName;
                    break;
            }

            if (scope == Translation.All
                || scope == Translation.Configuration && source == Source.Configuration
                || scope == Translation.Reflection && source == Source.Reflection)
            {
                name = Name.Format(name, namingCase, idPlacing, forceId, affixes);
            }

            return name;
        }

        private static string Concat(string a, string b, string c)
        {
            return Concat(a, Concat(b, c));
        }

        private static string Concat(string a, string b)
        {
            return a + (!string.IsNullOrEmpty(a) && !string.IsNullOrEmpty(b) ? "_" : string.Empty) + b;
        }
    }
}
