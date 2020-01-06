namespace Lit.Calc
{
    using System.Collections.Generic;

    public class CalculatorEngine
    {
        public int PrecisionBits = 256;

        public string Debug { get; private set; }

        public string Screen { get { return Value.Format(); } }

        public CalculatorNumber Value { get; private set; }

        #region Private members

        private readonly Stack<StackOperation> stack = new Stack<StackOperation>();

        #endregion

        public CalculatorEngine(Radix radix)
        {
            CreateNewValue(radix);
        }

        public void PutKey(string key)
        {
            LocalPutKey(key);
        }

        private void LocalPutKey(string key)
        {
            key = key.ToUpper();
            switch (key)
            {
                case @"CA":
                    Value.Clear();
                    stack.Clear();
                    break;

                case @"CE":
                    Value.Clear();
                    break;

                case @".":
                case @"DOT":
                    Value.SetDecimalMark();
                    break;

                case @"0":
                case @"1":
                case @"2":
                case @"3":
                case @"4":
                case @"5":
                case @"6":
                case @"7":
                case @"8":
                case @"9":
                    Value.AddDigit(key[0] - '0');
                    break;

                case @"A":
                case @"B":
                case @"C":
                case @"D":
                case @"E":
                case @"F":
                    Value.AddDigit(10 + key[0] - 'A');
                    break;

                case @"BCK":
                    Value.DelDigit();
                    break;

                case @"BIN":
                    Value.Radix = Radix.Bin;
                    break;

                case @"OCT":
                    Value.Radix = Radix.Oct;
                    break;

                case @"DEC":
                    Value.Radix = Radix.Dec;
                    break;

                case @"HEX":
                    Value.Radix = Radix.Hex;
                    break;

                case @"+":
                case @"ADD":
                    PutBinaryOperation(BinaryOperation.Add);
                    break;

                case @"-":
                case @"SUB":
                    PutBinaryOperation(BinaryOperation.Substract);
                    break;

                case @"*":
                case @"MUL":
                    PutBinaryOperation(BinaryOperation.Multiply);
                    break;

                case @"/":
                case @"DIV":
                    PutBinaryOperation(BinaryOperation.Divide);
                    break;

                case @"SGN":
                    Value.ChangeSign();
                    break;

                case @"=":
                case @"RES":
                    Value = Reduce(Precedence.Assignment);
                    break;

                default:
                    Debug = string.Format("Key: [{0}]", key);
                    break;
            }
        }

        private void PutBinaryOperation(BinaryOperation operation)
        {
            if (stack.Count > 0)
            {
                var attr = OperationAttribute.Get(operation);
                Value = Reduce(attr.Precedence);
            }

            stack.Push(new StackOperation(Value, operation));
            CreateNewValue(Value.Radix);
        }

        private CalculatorNumber Reduce(Precedence precedence)
        {
            var number = Value;

            while (stack.Count > 0)
            {
                var tos = stack.Peek();
                if ((int)tos.Precedence < (int)precedence)
                    break;

                tos = stack.Pop();
                number = new CalculatorNumber(tos.Perform(number));
            }

            return number;
        }

        private void CreateNewValue(Radix radix)
        {
            Value = new CalculatorNumber(radix, PrecisionBits);
        }
    }
}
