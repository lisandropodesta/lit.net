namespace Lit.calc
{
    using Lit.Value;
    using System;
    using System.Numerics;

    /// <summary>
    /// Helper for numerical operations.
    /// </summary>
    public static class NumericHelper
    {
        private const string UnableToDisplayInRadix = "Unable to format value in radix {0}";

        /// <summary>
        /// Decode a number in required parts to be displayed.
        /// </summary>
        /// <param name="radix">Target display radix</param>
        /// <param name="precision">Precision bits</param>
        /// <param name="significand">Significand part</param>
        /// <param name="exponent">Exponent part</param>
        /// <param name="integerPart">Resulting integer part</param>
        /// <param name="fractionalPart">Resulting fractional part</param>
        /// <param name="integerDigits">Number of integer digits</param>
        /// <param name="fractionalDigits">Number of fractional digits</param>
        /// <param name="exp">Display exponent</param>
        public static void Decode(Radix radix, int precision, BigInteger significand, int exponent,
            out BigInteger integerPart, out BigInteger fractionalPart,
            out int integerDigits, out int fractionalDigits, out int exp)
        {
            var digits = GetRequiredDigits(significand, radix);

            if (exponent == 0)
            {
                integerPart = significand;
                fractionalPart = new BigInteger(0);
                integerDigits = digits;
                fractionalDigits = 0;
                exp = 0;
            }
            else
            {
                var exponentDec = exponent <= 0 ? 0 : Math.Min(exponent, precision - digits);
                var scientificNotation = exponent - exponentDec > 0 || exponent - exponentDec <= -precision;

                if (!scientificNotation && exponentDec > 0)
                {
                    significand *= GetPower(radix, exponentDec);
                    exponent -= exponentDec;
                    digits += exponentDec;
                }

                integerDigits = scientificNotation ? 1 : exponent + digits;
                fractionalDigits = digits - integerDigits;
                exp = exponent + fractionalDigits;

                var decimalWeight = GetPower(radix, fractionalDigits);
                integerPart = BigInteger.DivRem(significand, decimalWeight, out fractionalPart);
            }
        }

        /// <summary>
        /// Calculates and trim zero digits at right from a value when expressed in a specified radix.
        /// </summary>
        public static int TrimZeroDigitsAtRight(ref BigInteger value, Radix radix, int maxCount)
        {
            var zerosAtRight = GetZeroDigitsAtRight(value, radix, maxCount);

            if (zerosAtRight > 0)
            {
                var divisor = GetPower(radix, zerosAtRight);
                value /= divisor;
            }

            return zerosAtRight;
        }

        /// <summary>
        /// Calculate zero digits at right when expressed in a specified radix.
        /// </summary>
        public static int GetZeroDigitsAtRight(BigInteger value, Radix radix, int maxCount)
        {
            int zerosAtRight;

            switch (radix)
            {
                case Radix.Dec:
                    {
                        var v = BigInteger.Abs(value);

                        zerosAtRight = 0;
                        while (maxCount == 0 || zerosAtRight < maxCount)
                        {
                            BigInteger.DivRem(v, 10, out BigInteger remainder);
                            if (!remainder.IsZero)
                                break;

                            zerosAtRight++;
                        }

                        break;
                    }

                default:
                    {
                        zerosAtRight = GetZeroDigitsAtRight(value, (int)GetBitsPerDigit(radix));
                        break;
                    }
            }

            if (maxCount > 0 && zerosAtRight > maxCount)
            {
                zerosAtRight = maxCount;
            }

            return zerosAtRight;
        }

        private static int GetZeroDigitsAtRight(BigInteger value, int bitsPerDigit)
        {
            if (value.IsZero)
                return 0;

            var bytes = BigInteger.Abs(value).ToByteArray();
            var lsb = Bits.GetLeastSignificantBit(bytes);
            return lsb / bitsPerDigit;
        }

        /// <summary>
        /// Calculate the required number of digits to display a value in a specified radix.
        /// </summary>
        public static int GetRequiredDigits(BigInteger value, Radix radix)
        {
            switch (radix)
            {
                case Radix.Dec:
                    {
                        var v = BigInteger.Abs(value);

                        if (v < (int)radix)
                            return 1;

                        var digits = BigInteger.Log(v, (int)radix);
                        return v > 1 ? (int)Math.Ceiling(digits) : -(int)Math.Ceiling(Math.Abs(digits));
                    }

                default:
                    return GetDigitsCount(value, (int)GetBitsPerDigit(radix));
            }
        }

        private static int GetDigitsCount(BigInteger value, int bitsPerDigit)
        {
            if (value.IsZero)
                return 1;

            var bytes = BigInteger.Abs(value).ToByteArray();
            var msb = Bits.GetMostSignificantBit(bytes);
            return 1 + msb / bitsPerDigit;
        }

        /// <summary>
        /// Converts to a string with a specific base and number of digits representation.
        /// </summary>
        public static string ToString(BigInteger value, Radix radix, int digits)
        {
            var isNegative = value < 0;
            string text;

            switch (radix)
            {
                case Radix.Dec:
                    text = value.ToString();
                    if (isNegative)
                    {
                        text = text.Substring(1);
                    }
                    break;

                default:
                    text = ToNbitDigitsString(value, (int)GetBitsPerDigit(radix));
                    break;
            }

            if (digits > 0 && text.Length < digits)
            {
                text = new string('0', digits - text.Length) + text;
            }

            return (isNegative ? "-" : "") + text;
        }

        private static string ToNbitDigitsString(BigInteger value, int bitsPerDigit)
        {
            var text = "";
            var array = BigInteger.Abs(value).ToByteArray();

            var count = (array.Length + 5) / 6;
            var minDigits = 0;
            for (var i = count - 1; i >= 0; i--)
            {
                var word48 = Get48bitWord(array, i);
                var textPart = Convert.ToString(word48, 1 << bitsPerDigit).ToUpper();
                if (minDigits > 0 && textPart.Length < minDigits)
                {
                    textPart = new string('0', minDigits - textPart.Length) + textPart;
                }

                text += textPart;
                minDigits = 48 / bitsPerDigit;
            }

            return text;
        }

        private static long Get48bitWord(byte[] array, int index)
        {
            var byteIndex = index * 6;
            long value = 0;

            for (int i = byteIndex, shift = 0; i < array.Length && i < byteIndex + 6; i++, shift += 8)
            {
                value += (long)array[i] << shift;
            }

            return value;
        }

        /// <summary>
        /// Get a big power.
        /// </summary>
        public static BigInteger GetPower(Radix radix, int exponent)
        {
            return BigInteger.Pow(new BigInteger((int)radix), exponent);
        }

        /// <summary>
        /// Optimized power calculation.
        /// </summary>
        public static BigInteger GetBigPower(int baseValue, int exponent)
        {
            var result = (BigInteger)1;

            if (exponent > 0)
            {
                var term = (BigInteger)baseValue;

                for (var iteration = 1; true; iteration *= 2)
                {
                    if ((exponent & iteration) != 0)
                    {
                        result *= term;
                        exponent -= iteration;
                        if (exponent == 0)
                            break;
                    }

                    term *= term;
                }
            }

            return result;
        }

        /// <summary>
        /// Get the number of bits required for each digit of a specified radix.
        /// </summary>
        public static double GetBitsPerDigit(Radix radix)
        {
            switch (radix)
            {
                case Radix.Dec:
                    return 3.3219281;

                case Radix.Bin:
                    return 1;

                case Radix.Oct:
                    return 3;

                case Radix.Hex:
                    return 4;

                default:
                    throw new ArgumentException(string.Format(UnableToDisplayInRadix, (int)radix));
            }
        }
    }
}
