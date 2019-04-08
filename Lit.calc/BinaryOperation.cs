namespace Lit.calc
{
    /// <summary>
    /// Operations that the calculator can perform.
    /// </summary>
    public enum BinaryOperation
    {
        [Operation(Precedence.Additive)]
        Add,

        [Operation(Precedence.Additive)]
        Substract,

        [Operation(Precedence.Multiplicative)]
        Multiply,

        [Operation(Precedence.Multiplicative)]
        Divide,

        [Operation(Precedence.Multiplicative)]
        Module,

        [Operation(Precedence.Exponential)]
        Power,

        [Operation(Precedence.Exponential)]
        Log,

        [Operation(Precedence.BitwiseInclusiveOr)]
        BitwiseInclusiveOr,

        [Operation(Precedence.BitwiseExclusiveOr)]
        BitwiseExclusiveOr,

        [Operation(Precedence.BitwiseAnd)]
        BitwiseAnd,

        [Operation(Precedence.BitwiseShift)]
        BitwiseShiftLeft,

        [Operation(Precedence.BitwiseShift)]
        BitwiseShiftRight
    }
}
