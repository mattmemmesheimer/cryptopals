using System;
using System.Linq;
using cryptopals.Lib.Crypto.Aes;
using cryptopals.Lib.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace cryptopals.Test.Challenges
{
    [TestClass]
    public class Set2
    {
        [TestMethod]
        public void TestChallenge10()
        {
            var keyStr = "YELLOW SUBMARINE";
            var key = System.Text.Encoding.ASCII.GetBytes(keyStr);
            var iv = new HexString("0000000000000000");
            var data = Convert.FromBase64String(Set2Data.Challenge10Input);

            var decrypted = AesCbc.Decrypt(key, iv.Bytes.ToArray(), data);
            var actual = System.Text.Encoding.ASCII.GetString(decrypted);

            Assert.AreEqual(Set2Data.Challenge10Solution, actual);
        }
    }
}
