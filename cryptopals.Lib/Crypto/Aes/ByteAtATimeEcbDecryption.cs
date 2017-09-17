using System;
using System.Collections.Generic;
using System.Linq;
using cryptopals.Lib.Text;

namespace cryptopals.Lib.Crypto.Aes
{
    public static class ByteAtATimeEcbDecryption
    {
        #region Constants

        private static readonly byte[] Key =
        {
            0xCD, 0xFE, 0xD3, 0xD6, 0x52, 0x7C, 0xE5, 0x42,
            0xBF, 0x4D, 0x67, 0xB4, 0xE0, 0x46, 0x89, 0x97
        };

        private static int BlockSizeBytes = 16;

        private static readonly string EncodedSuffix = @"Um9sbGluJyBpbiBteSA1LjAKV2l0aCBteSByYWctdG9wIGRvd24gc28gbXkg
aGFpciBjYW4gYmxvdwpUaGUgZ2lybGllcyBvbiBzdGFuZGJ5IHdhdmluZyBq
dXN0IHRvIHNheSBoaQpEaWQgeW91IHN0b3A/IE5vLCBJIGp1c3QgZHJvdmUg
YnkK";

        #endregion

        public static int FindBlockSize()
        {
            var data = new List<byte>();
            var encrypted = EncryptionOracle(data.ToArray());
            var length = encrypted.Length;
            while (true)
            {
                data.Add(0x00);
                encrypted = EncryptionOracle(data.ToArray());
                if (encrypted.Length != length)
                {
                    return encrypted.Length - length;
                }
            }
        }

        public static bool ConfirmEcbMode(int blockSize)
        {
            var b = SecureRng.GenerateRandomBytes(blockSize);
            var duplicateBlocks = new List<byte>();
            duplicateBlocks.AddRange(b);
            duplicateBlocks.AddRange(b);
            var encrypted = EncryptionOracle(duplicateBlocks.ToArray());
            return AesEcb.IsEcbEncrypted(encrypted, blockSize);
        }

        public static byte[] Decrypt()
        {
            // Determine block size of encryption oracle
            var blockSize = FindBlockSize();
            // Ensure encryption oracle is using ECB mode
            if (!ConfirmEcbMode(blockSize))
            {
                throw new InvalidOperationException("Encryption oracle is not encrypting in ECB mode.");
            }
            var knownBytes = new List<byte>();
            var blockIndex = 0;
            while(true)
            {
                var b = DecryptNextByte(blockSize, knownBytes, blockIndex);
                if (b == -1)
                {
                    break;
                }
                knownBytes.Add((byte) b);
                if (knownBytes.Count % blockSize == 0)
                {
                    blockIndex++;
                }
            }
            var unpadded = PaddingUtil.RemovePad(knownBytes.ToArray());
            return unpadded;
        }

        private static byte[] EncryptionOracle(byte[] data)
        {
            var combined = new List<byte>();
            combined.AddRange(data);
            combined.AddRange(Convert.FromBase64String(EncodedSuffix));
            var padded = PaddingUtil.Pad(combined.ToArray(), BlockSizeBytes);
            return AesEcb.Encrypt(Key, padded);
        }
        private static int DecryptNextByte(int blockSize, List<byte> knownBytes, int blockIndex)
        {
            string key;
            IEnumerable<byte> block;
            // Create a padding of repeating bytes
            var prefixLength = blockSize - knownBytes.Count % blockSize - 1;
            var repeatingBytes = RepeatingBytes(prefixLength);
            // Create the prefix of {repeatingBytes + knownBytes + byteGuess};
            var prefix = new List<byte>();
            prefix.AddRange(repeatingBytes);
            prefix.AddRange(knownBytes);
            prefix.Add(0x00);
            var prefixArray = prefix.ToArray();
            // Calculate a dictionary of each possible byte value
            var results = new Dictionary<string, int>();
            for (int i = 0; i < 256; i++)
            {
                prefixArray[prefixArray.Length - 1] = (byte) i;
                var encryptedWithPrefix = EncryptionOracle(prefixArray);
                block = encryptedWithPrefix.Skip(blockIndex * blockSize).Take(blockSize);
                key = new HexString(block).ToString();
                results[key] = i;
            }
            // Encrypt with repeating bytes
            var padded = EncryptionOracle(repeatingBytes);
            // Get the current block
            block = padded.Skip(blockIndex * blockSize).Take(blockSize);
            key = new HexString(block).ToString();
            // Look up the result
            if (results.ContainsKey(key))
            {
                return results[key];
            }
            return -1;
        }

        private static byte[] RepeatingBytes(int length)
        {
            var prefix = new List<byte>();
            for (int i = 0; i < length; i++)
            {
                prefix.Add(0x00);
            }
            return prefix.ToArray();
        }
    }
}
