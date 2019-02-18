namespace Lit.Test.Ui.Demo
{
    using System;

    public class CalculatorStack
    {
        public string Screen { get { return nextLevel?.Screen ?? inputText; } }

        public double Value { get { return sign * (integerValue + (double)fractionalValue / decimalWeight); } }

        #region Private members

        private int radix;

        private int sign;

        private int integerValue;

        private int fractionalValue;

        private int decimalWeight;

        private string inputText;

        private CalculatorStack nextLevel;

        #endregion

        public CalculatorStack(int radix)
        {
            sign = 1;
            this.radix = radix;
        }

        public void PutKey(string key)
        {
            switch (key.ToUpper())
            {
                case @".":
                case @"DOT":
                    PutDot();
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
                case @"A":
                case @"B":
                case @"C":
                case @"D":
                case @"E":
                case @"F":
                    break;

                case @"BIN":
                    SetRadix(2);
                    break;

                case @"OCT":
                    break;

                case @"DEC":
                    break;

                case @"HEX":
                    break;

                case @"+":
                case @"ADD":
                    break;

                case @"-":
                case @"SUB":
                    break;

                case @"*":
                case @"MUL":
                    break;

                case @"/":
                case @"DIV":
                    break;

                case @"SGN":
                    ChangeSign();
                    break;

                case @"=":
                case @"RES":
                    break;
            }
        }

        public void PutDot()
        {
            if (decimalWeight == 0)
            {
                decimalWeight = radix;
                inputText += ".";
            }
        }

        public void PutDigit(int digit)
        {
            if (digit < radix)
            {
                if (decimalWeight > 0)
                {
                    fractionalValue = fractionalValue * radix + digit;
                    decimalWeight *= radix;
                }
                else
                {
                    integerValue = integerValue * radix + digit;
                }

                inputText += '0' + digit;
            }
        }

        public void ChangeSign()
        {
            sign = -sign;
            inputText = sign == -1 ? "-" + inputText : inputText.Substring(1);
        }

        public void SetRadix(int newRadix)
        {
            if (radix != newRadix)
            {
                var newFractionalDigits = (int)Math.Ceiling(Math.Log10(decimalWeight) / Math.Log10(newRadix));
                var newDecimalWeight = (int)Math.Pow(newRadix, newFractionalDigits);
                fractionalValue = (int)Math.Round((double)fractionalValue * newDecimalWeight / decimalWeight);
                decimalWeight = newDecimalWeight;
                radix = newRadix;

                UpdateText();
            }
        }

        private void UpdateText()
        {
            inputText = (sign < 0 ? "-" : "") + Convert.ToString(integerValue, radix);
            if (decimalWeight > 0)
            {
                var zc = (int)Math.Truncate((Math.Log10(decimalWeight) - Math.Log10(fractionalValue)) / Math.Log10(radix));
                inputText += "." + new String('0', zc) + Convert.ToString(fractionalValue, radix);
            }
        }
    }
}
