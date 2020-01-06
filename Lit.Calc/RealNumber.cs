namespace Lit.Calc
{
    using System;
    using System.Numerics;

    public class RealNumber
    {
        private Radix radix;

        private BigInteger significand;

        #region Public properties

        public Radix Radix { get { return radix; } set { SetRadix(value); } }

        public BigInteger Significand { get { return significand; } }

        public int Exponent { get; protected set; }

        public int PrecisionBits { get; private set; }

        public bool HasDecimals { get { return Exponent < 0; } }

        public int SignificandSign { get { return !significand.IsZero ? significand.Sign : 0; } }

        #endregion

        public RealNumber(Radix radix, int precisionBits)
        {
            this.radix = radix;
            PrecisionBits = precisionBits;
        }

        public RealNumber(RealNumber number)
        {
            radix = number.Radix;
            significand = number.Significand;
            Exponent = number.Exponent;
            PrecisionBits = number.PrecisionBits;
        }

        public virtual void AddDigit(int digit)
        {
            significand = significand * (int)Radix + digit;

            if (Exponent < 0)
            {
                Exponent--;
            }
        }

        public virtual void DelDigit()
        {
            if (Exponent > 0)
            {
                Exponent--;
            }
            else
            {
                if (Exponent < 0)
                {
                    Exponent++;
                }

                significand /= (int)Radix;
            }
        }

        public void Negate()
        {
            significand = -significand;
        }

        public RealNumber Apply(BinaryOperation operation, RealNumber value)
        {
            value.Radix = Radix;

            switch (operation)
            {
                case BinaryOperation.Multiply:
                    return Multiply(value);

                case BinaryOperation.Divide:
                    return Divide(value);

                case BinaryOperation.Module:
                    return Module(value);

                case BinaryOperation.Add:
                    return Add(value);

                case BinaryOperation.Substract:
                    return Substract(value);

                default:
                    throw new ArgumentException(string.Format("Invalid operation {0}", operation));
            }
        }

        public RealNumber Multiply(RealNumber value)
        {
            significand *= value.Significand;
            Exponent += value.Exponent;
            FitToPrecision();
            return this;
        }

        public RealNumber Divide(RealNumber value)
        {
            significand /= value.Significand;
            Exponent -= value.Exponent;
            FitToPrecision();
            return this;
        }

        public RealNumber Module(RealNumber value)
        {
            if (value.HasDecimals)
            {
                throw new ArgumentException("Unable to get module with a non-integer divisor.");
            }

            if (value.Exponent > 0)
            {
                throw new NotImplementedException("Not implemented module operation with a divisor that has an exponent.");
            }

            significand %= value.Significand;
            Exponent = 0;
            return this;
        }

        public RealNumber Add(RealNumber value)
        {
            if (EqualizeSignificand(value))
            {
                significand += value.Significand;
                FitToPrecision();
            }

            return this;
        }

        public RealNumber Substract(RealNumber value)
        {
            if (EqualizeSignificand(value))
            {
                significand -= value.Significand;
                FitToPrecision();
            }

            return this;
        }

        private bool EqualizeSignificand(RealNumber value)
        {
            if (Exponent != value.Exponent)
            {
                var digits1 = NumericHelper.GetRequiredDigits(significand, Radix);
                var digits2 = NumericHelper.GetRequiredDigits(value.Significand, Radix);
                var msd1 = digits1 + Exponent;
                var msd2 = digits2 + value.Exponent;

                var firstIsBigger = msd1 >= msd2;

                var digitsh = firstIsBigger ? digits1 : digits2;
                var digitsl = firstIsBigger ? digits2 : digits1;
                var exph = firstIsBigger ? Exponent : value.Exponent;
                var expl = firstIsBigger ? value.Exponent : Exponent;
                var msdh = firstIsBigger ? msd1 : msd2;
                var msdl = firstIsBigger ? msd2 : msd1;

                var maxPrecisionDigits = (int)Math.Truncate(PrecisionBits / NumericHelper.GetBitsPerDigit(Radix));

                // In case there is no overlap, then the bigger value prevails
                if (msdh > msdl + maxPrecisionDigits)
                {
                    if (firstIsBigger)
                        return false;

                    significand = 0;
                    Exponent = value.Exponent;
                }
                else
                {
                    // TODO: Verificar maximo desplazamiento
                    // Check if there is necessary to shift the bigger value
                    /*if (digitsh < maxPrecisionDigits - 1 && exph > expl)
                    {
                        var shift = Math.Min(maxPrecisionDigits - 1 - digitsh, exph - expl);
                        throw new NotImplementedException("Revisar");
                    }*/

                    if (Exponent < value.Exponent)
                    {
                        value.ShiftExponent(Exponent - value.Exponent);
                    }
                    else
                    {
                        ShiftExponent(value.Exponent - Exponent);
                    }
                }
            }

            return true;
        }

        public virtual void Clear()
        {
            significand = 0;
            Exponent = 0;
        }

        private void SetRadix(Radix newRadix)
        {
            if (radix != newRadix)
            {
                var newExponent = GetTargetExponent(newRadix);

                GetRadixChangeFactor(radix, Exponent, newRadix, newExponent, out BigInteger numerator, out BigInteger denominator);

                significand = (significand * numerator + denominator / 2) / denominator;

                radix = newRadix;
                Exponent = newExponent;

                TrimNonSignificantZeros();
            }
        }

        private int GetTargetExponent(Radix newRadix)
        {
            if (Exponent == 0)
                return 0;

            var bitsPerDigit0 = NumericHelper.GetBitsPerDigit(radix);
            var bitsPerDigit1 = NumericHelper.GetBitsPerDigit(newRadix);
            var newExponent = Exponent * bitsPerDigit0 / bitsPerDigit1;
            return (int)Math.Round(newExponent);
        }

        private void TrimNonSignificantZeros()
        {
            if (Exponent < 0)
            {
                Exponent += NumericHelper.TrimZeroDigitsAtRight(ref significand, radix, -Exponent);
            }
        }

        /// <summary>
        /// Calculates the required factor to convert a number from expressed in radix0 to the same 
        /// number expressed in radix1. That is ( radix0 ^ exponent0 ) / ( radix1 ^ exponent1 )
        /// </summary>
        private static void GetRadixChangeFactor(Radix radix0, int exponent0, Radix radix1, int exponent1, out BigInteger numerator, out BigInteger denominator)
        {
            numerator = new BigInteger(1);
            denominator = new BigInteger(1);

            var v0 = NumericHelper.GetPower(radix0, Math.Abs(exponent0));
            if (exponent0 > 0)
            {
                numerator *= v0;
            }
            else
            {
                denominator *= v0;
            }

            var v1 = NumericHelper.GetPower(radix1, Math.Abs(exponent1));
            if (exponent1 < 0)
            {
                numerator *= v1;
            }
            else
            {
                denominator *= v1;
            }
        }

        private void ShiftExponent(int shift)
        {
            if (shift != 0)
            {
                if (shift > 0)
                {
                    significand /= NumericHelper.GetPower(Radix, shift);
                }
                else
                {
                    significand *= NumericHelper.GetPower(Radix, -shift);
                }

                Exponent += shift;
            }
        }

        private void FitToPrecision()
        {
            var bits = NumericHelper.GetRequiredDigits(significand, Radix.Bin);
            if (bits > PrecisionBits)
            {
                var bitsPerDigit = NumericHelper.GetBitsPerDigit(Radix);
                var trimDigits = (int)Math.Truncate((bits - PrecisionBits + bitsPerDigit - 1) / bitsPerDigit);

                significand /= NumericHelper.GetPower(Radix, trimDigits);
                Exponent += trimDigits;
            }
        }
    }
}
