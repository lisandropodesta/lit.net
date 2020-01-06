namespace Lit.Calc
{
    public class StackOperation
    {
        public RealNumber Value { get; set; }

        public BinaryOperation Operation { get; set; }

        public Precedence Precedence { get { return attribute?.Precedence ?? default(Precedence); } }

        private readonly OperationAttribute attribute;

        public StackOperation(RealNumber value, BinaryOperation operation)
        {
            Value = value;
            Operation = operation;
            attribute = OperationAttribute.Get(operation);
        }

        public RealNumber Perform(RealNumber value)
        {
            return Value.Apply(Operation, value);
        }
    }
}
