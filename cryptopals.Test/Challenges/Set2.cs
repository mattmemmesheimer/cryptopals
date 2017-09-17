﻿using System;
using System.Collections.Generic;
using System.Linq;
using cryptopals.Lib.Crypto;
using cryptopals.Lib.Crypto.Aes;
using cryptopals.Lib.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace cryptopals.Test.Challenges
{
    [TestClass]
    public class Set2
    {
        private static readonly int EcbCbcEncryptionOracleInputLengthBytes = 64;
        private static readonly int Challenge11TestCount = 100;

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

        [TestMethod]
        public void TestChallenge11()
        {
            var key = SecureRng.GenerateRandomBytes(16);
            var data = new List<byte>();
            for (int i = 0; i < EcbCbcEncryptionOracleInputLengthBytes; i++)
            {
                data.Add(0x00);
            }
            for (int i = 0; i < Challenge11TestCount; i++)
            {
                var guessedMode =
                    AesEcbCbcDetectionOracle.EncryptEcbOrCbc(data.ToArray(), out var actualMode);
                Assert.AreEqual(actualMode, guessedMode);
            }
        }

        [TestMethod]
        public void TestChallenge12()
        {
            int blockSize = ByteAtATimeEcbDecryption.FindBlockSize();
            Assert.AreEqual(Set2Data.Challenge12BlockSizeBytes, blockSize);
            var ecbMode = ByteAtATimeEcbDecryption.ConfirmEcbMode(blockSize);
            Assert.IsTrue(ecbMode);
            var decrypted = ByteAtATimeEcbDecryption.Decrypt();
            var s = System.Text.Encoding.ASCII.GetString(decrypted);
            Assert.AreEqual(Set2Data.Challenge12Solution, s);
        }
    }
}
