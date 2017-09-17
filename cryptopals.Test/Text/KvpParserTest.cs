using System;
using System.Collections.Generic;
using cryptopals.Lib.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace cryptopals.Test.Text
{
    [TestClass]
    public class KvpParserTest
    {
        [TestMethod]
        public void TestDecodeEmpty()
        {
            var encoded = "";
            var result = KvpParser.Decode(encoded);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void TestDecodeEmptyValue()
        {
            var encoded = "a=&b=";
            var result = KvpParser.Decode(encoded);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(string.Empty, result["a"]);
            Assert.AreEqual(string.Empty, result["b"]);
        }

        [ExpectedException(typeof(FormatException))]
        [TestMethod]
        public void TestDecodeInvalidFormatEmptyKey()
        {
            var encoded = "=";
            KvpParser.Decode(encoded);
        }

        [ExpectedException(typeof(FormatException))]
        [TestMethod]
        public void TestDecodeInvalidFormatWhitespaceKey()
        {
            var encoded = " =";
            KvpParser.Decode(encoded);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void TestDecodeNull()
        {
            KvpParser.Decode(null);
        }

        [TestMethod]
        public void TestDecodeSingle()
        {
            var encoded = "key=val";
            var result = KvpParser.Decode(encoded);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(result["key"], "val");
        }

        [TestMethod]
        public void TestDecodeMultiple()
        {
            var encoded = "key1=val1&key2=val2&key3=val3";
            var result = KvpParser.Decode(encoded);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(result["key1"], "val1");
            Assert.AreEqual(result["key2"], "val2");
            Assert.AreEqual(result["key3"], "val3");
        }

        [TestMethod]
        public void TestEncodeSingle()
        {
            var obj = new Dictionary<string, string>
            {
                { "key", "value" }
            };

            var encoded = KvpParser.Encode(obj);

            Assert.AreEqual("key=value", encoded);
        }

        [TestMethod]
        public void TestEncodeMultiple()
        {
            var obj = new Dictionary<string, string>
            {
                { "key1", "val1" },
                { "key2", "val2" },
                { "key3", "val3" }
            };

            var encoded = KvpParser.Encode(obj);

            Assert.AreEqual("key1=val1&key2=val2&key3=val3", encoded);
        }

        [TestMethod]
        public void TestEncodeEmpty()
        {
            var obj = new Dictionary<string, string>();

            var encoded = KvpParser.Encode(obj);

            Assert.AreEqual(string.Empty, encoded);
        }

        [TestMethod]
        public void TestEncodeRemoveInvalidChars()
        {
            var obj = new Dictionary<string, string>
            {
                { "key1=", "=val1" },
                { "key2&", "val2&" }
            };

            var encoded = KvpParser.Encode(obj);

            Assert.AreEqual("key1=val1&key2=val2", encoded);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void TestEncodeNull()
        {
            KvpParser.Encode(null);
        }
    }
}
