namespace Lit.Value
{
    public static class Bits
    {
        /// <summary>
        /// Get the least significant active bit in a byte array.
        /// </summary>
        public static int GetLeastSignificantBit(byte[] data)
        {
            for (var index = 0; index < data.Length; index++)
            {
                if (data[index] != 0)
                {
                    return GetLeastSignificantBit(data[index]) + index * 8;
                }
            }

            return -1;
        }

        /// <summary>
        /// Get the most significant active bit in a byte array.
        /// </summary>
        public static int GetMostSignificantBit(byte[] data)
        {
            var index = data.Length - 1;
            while (index > 0 && data[index] == 0)
            {
                index--;
            }

            return GetMostSignificantBit(data[index]) + index * 8;
        }

        /// <summary>
        /// Get the most significant active bit in a byte.
        /// </summary>
        /// <returns>A value between 0 and 7</returns>
        public static int GetMostSignificantBit(byte data)
        {
            var value = 0;

            if ((data & 0xF0) > 0)
            {
                value += 4;
                data >>= 4;
            }

            if ((data & 0xC) > 0)
            {
                value += 2;
                data >>= 2;
            }

            if ((data & 0x2) > 0)
            {
                value += 1;
            }

            return value;
        }

        /// <summary>
        /// Get the least significant active bit in a byte.
        /// </summary>
        /// <returns>A value between 0 and 7 in case of an active bit is present. Otherwise returns -1.</returns>
        public static int GetLeastSignificantBit(byte data)
        {
            if (data == 0)
                return -1;

            var value = 0;

            if ((data & 0x0F) == 0)
            {
                value += 4;
                data >>= 4;
            }

            if ((data & 0x3) == 0)
            {
                value += 2;
                data >>= 2;
            }

            if ((data & 0x1) == 0)
            {
                value += 1;
            }

            return value;
        }
    }
}
