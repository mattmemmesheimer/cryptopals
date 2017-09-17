using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace cryptopals.Lib.Crypto.Aes
{
    public static class AesEcbCbcDetectionOracle
    {
        #region Constants

        private static readonly int MinRandBytesCount = 5;
        private static readonly int MaxRandBytesCount = 10;
        private static readonly int BlockSizeBytes = 16;
        private static readonly int KeySizeBytes = 16;
        private static readonly Random Rand = new Random(); 

        #endregion

        public static CipherMode EncryptEcbOrCbc(byte[] data, out CipherMode actualMode)
        {
            actualMode = Rand.Next(0, 2) == 0 ? CipherMode.ECB : CipherMode.CBC;
            var key = SecureRng.GenerateRandomBytes(KeySizeBytes);
            // Add prefix and suffix to data
            data = TransformData(data);
            byte[] encrypted;
            if (actualMode == CipherMode.ECB)
            {
                var padded = PaddingUtil.Pad(data, BlockSizeBytes);
                encrypted = AesEcb.Encrypt(key, padded);
            }
            else
            {
                var iv = SecureRng.GenerateRandomBytes(BlockSizeBytes);
                encrypted = AesCbc.Encrypt(key, iv, data);
            }
            var guessedMode = AesEcb.IsEcbEncrypted(encrypted, BlockSizeBytes)
                ? CipherMode.ECB
                : CipherMode.CBC;
            return guessedMode;
        }

        private static byte[] TransformData(byte[] data)
        {
            var prefixLength = Rand.Next(MinRandBytesCount, MaxRandBytesCount + 1);
            var suffixLength = Rand.Next(MinRandBytesCount, MaxRandBytesCount + 1);
            var prefix = SecureRng.GenerateRandomBytes(prefixLength);
            var suffix = SecureRng.GenerateRandomBytes(suffixLength);
            var input = new List<byte>();
            input.AddRange(prefix);
            input.AddRange(data);
            input.AddRange(suffix);
            return input.ToArray();
        }
    }
}
