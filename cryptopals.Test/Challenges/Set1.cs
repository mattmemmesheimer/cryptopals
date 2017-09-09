using System;
using System.Linq;
using cryptopals.Lib.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace cryptopals.Test.Challenges
{
    [TestClass]
    public class Set1
    {
        [TestMethod]
        public void Challenge1()
        {
            var input = "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d";
            var expected = "SSdtIGtpbGxpbmcgeW91ciBicmFpbiBsaWtlIGEgcG9pc29ub3VzIG11c2hyb29t";
            var hexString = new HexString(input);

            var bytes = hexString.Bytes.ToArray();
            var actual = Convert.ToBase64String(bytes);

            Assert.AreEqual(expected, actual);
        }
    }
}
