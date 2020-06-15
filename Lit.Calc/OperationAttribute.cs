namespace Lit.Calc
{
    using Lit.DataType;
    using System;

    /// <summary>
    /// Operations precedence.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class OperationAttribute : Attribute
    {
        /// <summary>
        /// Operation precedence, greater value means execution is performed before.
        /// </summary>
        public Precedence Precedence { get; set; }

        public OperationAttribute(Precedence precedence)
        {
            Precedence = precedence;
        }

        /// <summary>
        /// Get attributes from an operation.
        /// </summary>
        public static OperationAttribute Get(BinaryOperation value)
        {
            return TypeHelper.TryGetFieldAttribute<OperationAttribute, BinaryOperation>(value);
        }
    }
}
