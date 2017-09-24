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
    }
}
