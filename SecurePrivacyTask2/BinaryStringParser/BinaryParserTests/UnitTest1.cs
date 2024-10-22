using BinaryStringParser;

namespace BinaryStringAnalysis.Tests
{
    [TestFixture]
    public class BinaryStringTests
    {
        [Test]
        public void Test_ValidGoodBinaryStrings()
        {
            // Valid strings that follow the rules
            Assert.IsTrue(BinaryParser.IsGoodBinaryString("1100"), "Expected '1100' to be a good binary string.");
            Assert.IsTrue(BinaryParser.IsGoodBinaryString("1010"), "Expected '1010' to be a good binary string.");
            Assert.IsTrue(BinaryParser.IsGoodBinaryString("111000"), "Expected '111000' to be a good binary string.");
        }

        [Test]
        public void Test_InvalidBinaryStrings()
        {
            // Invalid strings where prefix condition or balance is violated
            Assert.IsFalse(BinaryParser.IsGoodBinaryString("1110000"), "Expected '111000' to be a bad binary string.");
            Assert.IsFalse(BinaryParser.IsGoodBinaryString("100"), "Expected '100' to be a bad binary string.");
            Assert.IsFalse(BinaryParser.IsGoodBinaryString("11100011"), "Expected '11100011' to be a bad binary string."); // This is now correct
        }

        [Test]
        public void Test_EmptyBinaryString()
        {
            // Empty string should be invalid
            Assert.IsFalse(BinaryParser.IsGoodBinaryString(""), "Expected empty string to be considered invalid.");
        }

        [Test]
        public void Test_AllOnes()
        {
            // Strings with only '1's are invalid (no 0's to balance)
            Assert.IsFalse(BinaryParser.IsGoodBinaryString("111111"), "Expected string with only '1's to be considered invalid.");
        }

        [Test]
        public void Test_AllZeroes()
        {
            // Strings with only '0's are invalid (no 1's to balance)
            Assert.IsFalse(BinaryParser.IsGoodBinaryString("000000"), "Expected string with only '0's to be considered invalid.");
        }

        [Test]
        public void Test_MixedInvalidCharacters()
        {
            // Strings with invalid characters should be considered invalid
            Assert.IsFalse(BinaryParser.IsGoodBinaryString("11002"), "Expected string with invalid characters to be considered invalid.");
            Assert.IsFalse(BinaryParser.IsGoodBinaryString("110a"), "Expected string with invalid characters to be considered invalid.");
        }

        [Test]
        public void Test_LargeBinaryString()
        {
            // Large string where the balance is off
            string largeUnbalancedString = new string('1', 10000) + new string('0', 9999);
            Assert.IsFalse(BinaryParser.IsGoodBinaryString(largeUnbalancedString), "Expected large unbalanced string to be considered invalid.");

            // Large string with valid balance and prefix condition met
            string largeValidGoodString = new string('1', 10000) + new string('0', 10000);
            Assert.IsTrue(BinaryParser.IsGoodBinaryString(largeValidGoodString), "Expected large valid good string to be considered valid.");
        }

        [Test]
        public void Test_SingleCharacterStrings()
        {
            // Single characters cannot be balanced
            Assert.IsFalse(BinaryParser.IsGoodBinaryString("0"), "Expected '0' to be considered invalid.");
            Assert.IsFalse(BinaryParser.IsGoodBinaryString("1"), "Expected '1' to be considered invalid.");
        }
    }
}
