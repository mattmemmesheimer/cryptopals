using System;
using System.Linq;
using cryptopals.Lib.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable UnusedVariable

namespace cryptopals.Test.Text
{
    [TestClass]
    public class HexStringTest
    {
        [TestMethod]
        public void TestEmptyString()
        {
            var hexString = new HexString(string.Empty);

            Assert.AreEqual(0, hexString.Bytes.Count);
            Assert.AreEqual(string.Empty, hexString.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullString()
        {
            var hexString = new HexString((string) null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestOddLengthString()
        {
            var hexString = new HexString("a");
        }

        [TestMethod]
        public void TestEmptyArray()
        {
            var hexString = new HexString(new byte[]{ });

            Assert.AreEqual(0, hexString.Bytes.Count);
            Assert.AreEqual(string.Empty, hexString.ToString());
        }

        [TestMethod]
        public void TestBytes()
        {
            var expected = new byte[] { 0xAA, 0x05, 0x4B };
            var hexString = new HexString("aa054b");

            CollectionAssert.AreEqual(expected, hexString.Bytes.ToArray());
        }

        [TestMethod]
        public void TestString()
        {
            var expected = "aa054b";
            var hexString = new HexString(expected);

            Assert.AreEqual(expected, hexString.ToString());
        }
    }
}
