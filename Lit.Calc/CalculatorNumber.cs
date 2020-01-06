namespace Lit.Calc
{
    using System.Numerics;

    public class CalculatorNumber : RealNumber
    {
        #region Internal variables

        private bool isNegative;

        private bool decimalMark;

        #endregion

        #region Public properties

        public int Sign { get { return SignificandSign != 0 ? SignificandSign : isNegative ? -1 : 0; } set { SetSign(value); } }

        #endregion

        public CalculatorNumber(RealNumber realNumber)
            : base(realNumber)
        {
        }

        public CalculatorNumber(Radix radix, int precisionBits)
            : base(radix, precisionBits)
        {
        }

        public override void Clear()
        {
            decimalMark = false;
            isNegative = false;
        }

        public void ChangeSign()
        {
            SetSign(isNegative ? 1 : -1);
        }

        public void SetDecimalMark()
        {
            decimalMark = true;
        }

        public override void AddDigit(int digit)
        {
            if (digit < (int)Radix)
            {
                base.AddDigit(digit);

                if (Exponent >= 0 && decimalMark)
                {
                    Exponent--;
                }
            }
        }

        public override void DelDigit()
        {
            if (Exponent == 0 && decimalMark)
            {
                decimalMark = false;
            }
            else if (Exponent == 0 && Significand.IsZero)
            {
                isNegative = false;
            }
            else
            {
                base.DelDigit();
            }
        }

        public string Format()
        {
            return Format(Radix);
        }

        public string Format(Radix fmtRadix)
        {
            var negative = Significand.Sign < 0;
            NumericHelper.Decode(fmtRadix, PrecisionBits, Significand, Exponent,
                out BigInteger integerPart, out BigInteger fractionalPart, out int integerDigits, out int fractionalDigits, out int exp);

            var text = negative ? "-" : "" + NumericHelper.ToString(integerPart, fmtRadix, 0);
            if (Exponent != 0)
            {
                text += "." + NumericHelper.ToString(fractionalPart, fmtRadix, fractionalDigits);
            }
            else if (decimalMark)
            {
                text += ".";
            }

            if (exp != 0)
            {
                text += " e" + exp.ToString();
            }

            return text;
        }

        private void SetSign(int value)
        {
            isNegative = value < 0;
            var sign = Significand.Sign;

            if (sign > 0 && isNegative || sign < 0 && !isNegative)
            {
                Negate();
            }
        }
    }
}
