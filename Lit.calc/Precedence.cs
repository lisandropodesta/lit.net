namespace Lit.calc
{
    /// <summary>
    /// Precedence of operators. Match C# definition.
    /// </summary>
    public enum Precedence
    {
        Assignment,

        ConditionalExpression,

        LogicalOr,

        LogicalAnd,

        BitwiseInclusiveOr,

        BitwiseExclusiveOr,

        BitwiseAnd,

        Equality,

        Relational,

        BitwiseShift,

        Additive,

        Multiplicative,

        UnaryCast,

        Exponential,

        UnaryOperations
    }
}
