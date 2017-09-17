using cryptopals.Lib.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace cryptopals.Test.Crypto
{
    [TestClass]
    public class PaddingUtilTest
    {
        [TestMethod]
        public void TestPaddingInputSizeSmallerThanBlockSize()
        {
            int blockSize = 4;
            var input = new byte[] { 0x01, 0x02 };
            var expected = new byte[] { 0x01, 0x02, 0x02, 0x02 };

            var actual = PaddingUtil.Pad(input, blockSize);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestPaddingInputSizeEqualsBlockSize()
        {
            int blockSize = 5;
            var input = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
            var expected = new byte[]
            {
                0x01, 0x02, 0x03, 0x04, 0x05,
                0x05, 0x05, 0x05, 0x05, 0x05
            };

            var actual = PaddingUtil.Pad(input, blockSize);

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestPaddingInputSizeLargerThanBlockSize()
        {
            int blockSize = 4;
            var input = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
            var expected = new byte[]
            {
                0x01, 0x02, 0x03, 0x04,
                0x05, 0x03, 0x03, 0x03
            };

            var actual = PaddingUtil.Pad(input, blockSize);

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
