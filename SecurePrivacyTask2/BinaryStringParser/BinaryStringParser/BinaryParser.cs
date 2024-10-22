namespace BinaryStringParser
{
    public static class BinaryParser
    {
        public static bool IsGoodBinaryString(string binaryString)
        {
            if (string.IsNullOrEmpty(binaryString))
            {
                // An empty string is considered invalid
                return false;
            }

            int countZero = 0;
            int countOne = 0;

            // Iterate through the string to check each prefix
            foreach (char bit in binaryString)
            {
                if (bit == '1')
                {
                    countOne++;
                }
                else if (bit == '0')
                {
                    countZero++;
                }
                else
                {
                    // If the string contains invalid characters, return false
                    return false;
                }

                // Condition to check that at no point should 1's be less than 0's
                if (countOne < countZero)
                {
                    return false;
                }
            }

            // Final check: The number of 1's and 0's should be equal
            return countOne == countZero;
        }

    }
}
