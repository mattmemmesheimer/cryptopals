using System;
using cryptopals.Lib.Crypto.Aes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace cryptopals.Test.Challenges
{
    [TestClass]
    public class Set3
    {
        [TestMethod]
        public void TestChallenge17()
        {
            var decrypted = CbcPaddingOracle.Decrypt(out var randIndex);
            var expected = Convert.FromBase64String(CbcPaddingOracle.Data[randIndex]);
            
            CollectionAssert.AreEqual(expected, decrypted);
        }

        [TestMethod]
        public void TestChallenge18()
        {
            var encrypted = Convert.FromBase64String(Set3Data.Challenge18Input);
            var keyStr = "YELLOW SUBMARINE";
            var key = System.Text.Encoding.ASCII.GetBytes(keyStr);
            ulong nonce = 0;

            var decrypted = AesCtr.Decrypt(key, nonce, encrypted);
            var actual = System.Text.Encoding.ASCII.GetString(decrypted);

            Assert.AreEqual(Set3Data.Challenge18Solution, actual);

            // Encrypt and verify against input data
            var enc = AesCtr.Encrypt(key, nonce, decrypted);
            CollectionAssert.AreEqual(encrypted, enc);
        }
    }
}
