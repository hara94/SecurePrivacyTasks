using BinaryStringParser;

namespace BinaryStringAnalysis
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter binary strings separated by commas:");
            string input = Console.ReadLine();

            // Check if input is null or empty
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Error: Input cannot be null or empty.");
                return;
            }

            // Split the input by commas and trim each part
            string[] testStrings = input.Split(',');

            foreach (var binaryString in testStrings)
            {
                string trimmedString = binaryString.Trim();

                // Validate that each string is not empty and contains only '0's and '1's
                if (IsValidBinaryString(trimmedString))
                {
                    bool isGood = BinaryParser.IsGoodBinaryString(trimmedString);
                    Console.WriteLine($"Binary String: {trimmedString} is {(isGood ? "Good" : "Not Good")}");
                }
                else
                {
                    Console.WriteLine($"Error: '{trimmedString}' is not a valid binary string.");
                }
            }
        }

        // Function to validate if the input string contains only '0's and '1's
        static bool IsValidBinaryString(string binaryString)
        {
            if (string.IsNullOrEmpty(binaryString))
            {
                return false; // Empty string is not valid
            }

            // Check if the string contains only '0' and '1'
            foreach (char c in binaryString)
            {
                if (c != '0' && c != '1')
                {
                    return false; // Invalid character found
                }
            }

            return true; // All characters are valid
        }
    }
}
