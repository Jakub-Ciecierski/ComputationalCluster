using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator;
namespace StringCalculatorTest
{
    [TestClass]
    public class UnitTest1
    {
        private StringCalculator createStringCalculator()
        {
            return new StringCalculator();
        }

        [TestMethod]
        public void Calculate_EmptyString_ReturnsZero()
        {
            StringCalculator stringCalculator = createStringCalculator();

            string emptyString = "";

            int actualNumber = stringCalculator.run(emptyString);
            int expectedNumber = 0;

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void Calculate_StringNumber_IntNumber()
        {
            StringCalculator stringCalculator = createStringCalculator();

            string someString = "3";

            int actualNumber = stringCalculator.run(someString);
            int expectedNumber = 3;

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void Calculate_StringTwoNumbersCommaDelimited_IntSum()
        {
            StringCalculator stringCalculator = createStringCalculator();

            string someString = "11,32";

            int actualNumber = stringCalculator.run(someString);
            int expectedNumber = 43;

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void Calculate_StringTwoNumbersNewLineDelimited_IntSum()
        {
            StringCalculator stringCalculator = createStringCalculator();

            string someString = "1\n3";

            int actualNumber = stringCalculator.run(someString);
            int expectedNumber = 4;

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void Calculate_StringThreeNumbersAnyDelimiteer_IntSum()
        {
            StringCalculator stringCalculator = createStringCalculator();

            string someString = "1,3,5";

            int actualNumber = stringCalculator.run(someString);
            int expectedNumber = 9;

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(ArithmeticException))]
        public void Calculate_StringWithNegativeNumbersAnyDelimiteer_Throws()
        {
            StringCalculator stringCalculator = createStringCalculator();

            string someString = "-1,3,5";

            int actualNumber = stringCalculator.run(someString);
        }

        [TestMethod]
        public void Calculate_StringNumbersAnyDelimiteer_IntSumGreaterThan1000Ignored()
        {
            StringCalculator stringCalculator = createStringCalculator();

            string someString = "1001,1,3,5,1001";

            int actualNumber = stringCalculator.run(someString);
            int expectedNumber = 9;

            Assert.AreEqual(expectedNumber, actualNumber);
        }

        [TestMethod]
        public void Calculate_StringNumbersNewDefinedDelimiteer_IntSum()
        {
            StringCalculator stringCalculator = createStringCalculator();

            string someString = "//&%\n1001\n1,3%5&1,1001";

            int actualNumber = stringCalculator.run(someString);
            int expectedNumber = 10;

            Assert.AreEqual(expectedNumber, actualNumber);
        }
    }
}
