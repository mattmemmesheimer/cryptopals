using System.Linq;
using cryptopals.Lib.Crypto.Aes;
using cryptopals.Lib.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace cryptopals.Test.Crypto.Aes
{
    [TestClass]
    public class AesCbcTest
    {
        [TestMethod]
        public void TestEncryptOneBlock()
        {
            var key = new HexString("0102030405060708090A0B0C0D0E0F10");
            var iv = new HexString("1112131415161718191A1B1C1D1E1F20");
            var clearText = "Hello World";
            var data = System.Text.Encoding.ASCII.GetBytes(clearText);
            var expected = new HexString("E0A209AC01C9052681925942E0E5EA53");

            var actual = AesCbc.Encrypt(key.Bytes.ToArray(), iv.Bytes.ToArray(), data);

            CollectionAssert.AreEqual(expected.Bytes.ToArray(), actual);
        }

        [TestMethod]
        public void TestDecryptOneBlock()
        {
            var key = new HexString("0102030405060708090A0B0C0D0E0F10");
            var iv = new HexString("1112131415161718191A1B1C1D1E1F20");
            var data = new HexString("E0A209AC01C9052681925942E0E5EA53");
            var expected = "Hello World";

            var decrypted = AesCbc.Decrypt(key.Bytes.ToArray(), iv.Bytes.ToArray(),
                data.Bytes.ToArray());
            var actual = System.Text.Encoding.ASCII.GetString(decrypted);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestEncrypt()
        {
            var key = new HexString("0102030405060708090A0B0C0D0E0F10");
            var iv = new HexString("1112131415161718191A1B1C1D1E1F20");
            var clearText = "Hello World, let's try something longer than just one block.";
            var data = System.Text.Encoding.ASCII.GetBytes(clearText);
            var expected = new HexString("C29B1B0E628232FA5A8F77F88546DF259D8B8B4E73B5D21D9BA1EDB557CAE0A67CA855B59DC1F423A2540EC15377EF70B7750F310C8E40AE68A52E57D478BB60");

            var actual = AesCbc.Encrypt(key.Bytes.ToArray(), iv.Bytes.ToArray(), data);

            CollectionAssert.AreEqual(expected.Bytes.ToArray(), actual);
        }

        [TestMethod]
        public void TestDecrypt()
        {
            var key = new HexString("0102030405060708090A0B0C0D0E0F10");
            var iv = new HexString("1112131415161718191A1B1C1D1E1F20");
            var data = new HexString("C29B1B0E628232FA5A8F77F88546DF259D8B8B4E73B5D21D9BA1EDB557CAE0A67CA855B59DC1F423A2540EC15377EF70B7750F310C8E40AE68A52E57D478BB60");
            var expected = "Hello World, let's try something longer than just one block.";

            var decrypted = AesCbc.Decrypt(key.Bytes.ToArray(), iv.Bytes.ToArray(),
                data.Bytes.ToArray());
            var actual = System.Text.Encoding.ASCII.GetString(decrypted);

            Assert.AreEqual(expected, actual);
        }
    }
}
