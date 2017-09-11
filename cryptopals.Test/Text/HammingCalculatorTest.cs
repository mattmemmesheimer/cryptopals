using cryptopals.Lib.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace cryptopals.Test.Text
{
    [TestClass]
    public class HammingCalculatorTest
    {
        [TestMethod]
        public void Test()
        {
            var a = "this is a test";
            var b = "wokka wokka!!!";
            var expectedDistance = 37;

            var actualDistance = HammingCalculator.CalculateHammingDistance(a, b);

            Assert.AreEqual(expectedDistance, actualDistance);
        }

        [TestMethod]
        public void Test1()
        {
            var a = "a";
            var b = "b";
            var expectedDistance = 2;

            var actualDistance = HammingCalculator.CalculateHammingDistance(a, b);

            Assert.AreEqual(expectedDistance, actualDistance);

            a = "c";
            b = "d";
            expectedDistance = 3;

            actualDistance = HammingCalculator.CalculateHammingDistance(a, b);

            Assert.AreEqual(expectedDistance, actualDistance);
        }

        [TestMethod]
        public void Test2()
        {
            var a = "Ram";
            var b = "Rom";
            var expectedDistance = 3;

            var actualDistance = HammingCalculator.CalculateHammingDistance(a, b);

            Assert.AreEqual(expectedDistance, actualDistance);


            a = "mam";
            b = "man";
            expectedDistance = 2;

            actualDistance = HammingCalculator.CalculateHammingDistance(a, b);

            Assert.AreEqual(expectedDistance, actualDistance);
        }
    }
}
