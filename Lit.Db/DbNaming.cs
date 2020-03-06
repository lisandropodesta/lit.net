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
            IdPlacing = idPlacing;
            ParametersCase = StoredProceduresCase = FieldsCase = TablesCase = namingCase;
            IdText = idText;
        }

        #endregion

        /// <summary>
        /// Gets a parameter name.
        /// </summary>
        public virtual string GetParameterName(string reflectionName, string parameterName)
        {
            return TranslateName(TextSource, Scope, reflectionName, parameterName, ParametersCase, IdPlacing, IdText);
        }

        /// <summary>
        /// Gets a field name.
        /// </summary>
        public virtual string GetFieldName(string reflectionName, string fieldName)
        {
            return TranslateName(TextSource, Scope, reflectionName, fieldName, FieldsCase, IdPlacing, IdText);
        }

        /// <summary>
        /// Gets a stored procedure name.
        /// </summary>
        public virtual string GetStoredProcedureName(Type template, string spName)
        {
            return TranslateName(TextSource, Scope, template.Name, spName, StoredProceduresCase, AffixPlacing.DoNotChange, null);
        }

        /// <summary>
        /// Gets a stored procedure name.
        /// </summary>
        public virtual string GetStoredProcedureName(string tableName, StoredProcedureFunction spFunc)
        {
            DbArchitectureHelper.GetDefaultStoredProcedureAffixes(spFunc, out var prefix, out var suffix);
            var name = prefix + tableName + suffix;
            return TranslateName(TextSource, Scope, null, name, StoredProceduresCase, AffixPlacing.DoNotChange, null);
        }

        /// <summary>
        /// Gets a table name.
        /// </summary>
        public virtual string GetTableName(Type template, string tableName)
        {
            return TranslateName(TextSource, Scope, template.Name, tableName, TablesCase, AffixPlacing.DoNotChange, null);
        }

        /// <summary>
        /// Gets a table column name.
        /// </summary>
        public virtual string GetColumnName(string tableName, PropertyInfo propInfo, string fieldName)
        {
            return TranslateName(TextSource, Scope, propInfo.Name, fieldName, FieldsCase, IdPlacing, IdText);
        }

        /// <summary>
        /// Translates a name.
        /// </summary>
        public static string TranslateName(Source source, Translation scope, string reflectionName, string configurationName, Case namingCase, AffixPlacing idPlacing, params string[] affixes)
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
                name = Name.Format(name, namingCase, idPlacing, affixes);
            }

            return name;
        }
    }
}
