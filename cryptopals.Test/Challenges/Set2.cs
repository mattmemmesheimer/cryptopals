using System;
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

        [TestMethod]
        public void TestChallenge13()
        {
            var email1 = "my@bar.comadmin";
            var padding = 11;
            var paddingBytes = new List<byte>();
            for (int i = 0; i < padding; i++)
            {
                paddingBytes.Add((byte)padding);
            }
            var paddingStr = System.Text.Encoding.ASCII.GetString(paddingBytes.ToArray());
            var x1 = CutAndPasteEcbDecryption.EncryptProfileFor(email1 + paddingStr);
            var email2 = "aaaaa@bar.com";
            var x2 = CutAndPasteEcbDecryption.EncryptProfileFor(email2);
            var bytes = new List<byte>();
            bytes.AddRange(x2.Take(32));
            bytes.AddRange(x1.Skip(16).Take(16));
            var profile = CutAndPasteEcbDecryption.DecryptProfile(bytes.ToArray());

            Assert.IsTrue(profile.ContainsKey("role"));
            Assert.AreEqual("admin", profile["role"]);
        }

        [TestMethod]
        public void TestChallenge14()
        {
            int blockSize = ByteAtATimeEcbDecryption2.FindBlockSize();
            Assert.AreEqual(Set2Data.Challenge12BlockSizeBytes, blockSize);

            ByteAtATimeEcbDecryption2.Decrypt();

            var decrypted = ByteAtATimeEcbDecryption2.Decrypt();
            var s = System.Text.Encoding.ASCII.GetString(decrypted);
            Assert.AreEqual(Set2Data.Challenge12Solution, s);
        }
        
        [TestMethod]
        public void TestChallenge15()
        {
            var blockSize = 16;
            var str = "ICE ICE BABY";
            var bytes = System.Text.Encoding.ASCII.GetBytes(str);
            var valid = PaddingUtil.Pad(bytes, blockSize);
            Assert.IsTrue(PaddingUtil.ValidPadding(valid, blockSize));

            var invalid = new List<byte>(bytes);
            invalid.AddRange(new byte[]{0x05, 0x05, 0x05, 0x05});
            Assert.IsFalse(PaddingUtil.ValidPadding(invalid.ToArray(), blockSize));

            invalid = new List<byte>(bytes);
            invalid.AddRange(new byte[] { 0x01, 0x02, 0x03, 0x04 });
            Assert.IsFalse(PaddingUtil.ValidPadding(invalid.ToArray(), blockSize));
        }

        [TestMethod]
        public void TestMethod16()
        {
            var encrypted = AesCbcBitFlipping.EncryptionOracle(";admin=true;");
            Assert.IsFalse(AesCbcBitFlipping.IsAdmin(encrypted));

            encrypted = AesCbcBitFlipping.EncryptionOracle("AAAAAAAAAAAAAAAA:admin<true:AAAA");
            encrypted[32] ^= 1;
            encrypted[38] ^= 1;
            encrypted[43] ^= 1;
            Assert.IsTrue(AesCbcBitFlipping.IsAdmin(encrypted));
        }
    }
}
