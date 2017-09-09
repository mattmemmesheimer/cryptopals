using System;
using System.Linq;
using cryptopals.Lib;
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

        [TestMethod]
        public void Challenge2()
        {
            var a = new HexString("1c0111001f010100061a024b53535009181c");
            var b = new HexString("686974207468652062756c6c277320657965");
            var expected = "746865206b696420646f6e277420706c6179";

            var xord = XorUtil.Xor(a.Bytes.ToArray(), b.Bytes.ToArray());
            var actual = new HexString(xord).ToString();

            Assert.AreEqual(expected, actual);
        }
    }
}
