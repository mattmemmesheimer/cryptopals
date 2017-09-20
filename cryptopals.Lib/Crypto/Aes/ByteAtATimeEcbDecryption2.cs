using System;
using System.Collections.Generic;
using System.Linq;
using cryptopals.Lib.Extensions;
using cryptopals.Lib.Text;

namespace cryptopals.Lib.Crypto.Aes
{
    public static class ByteAtATimeEcbDecryption2
    {
        #region Constants

        private static readonly byte[] Key =
        {
            0xCD, 0xFE, 0xD3, 0xD6, 0x52, 0x7C, 0xE5, 0x42,
            0xBF, 0x4D, 0x67, 0xB4, 0xE0, 0x46, 0x89, 0x97
        };

        private static int BlockSizeBytes = 16;

        private static int MaxPrefixLength = 64;

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

        public static byte[] Decrypt()
        {
            // Determine block size of encryption oracle
            var blockSize = FindBlockSize();
            // Determine prefix size of encryption oracle
            var prefixSize = FindPrefixSize(blockSize);

            var knownBytes = new List<byte>();
            var blockIndex = (int) Math.Ceiling((double) prefixSize/blockSize);
            while (true)
            {
                var b = DecryptNextByte(blockSize, prefixSize, knownBytes, blockIndex);
                if (b == -1)
                {
                    break;
                }
                knownBytes.Add((byte)b);
                if (knownBytes.Count % blockSize == 0)
                {
                    blockIndex++;
                }
            }
            var unpadded = PaddingUtil.RemovePad(knownBytes.ToArray());
            return unpadded;
        }

        private static int FindPrefixSize(int blockSize)
        {
            // Find the number of blocks where the prefix fills the entire block
            var fullPrefixBlockCount = FindFullPrefixBlockCount(blockSize);
            // Find the size of the prefix that partially fills a block
            var partialPrefixBlockSize = FindPartialPrefixBlockSize(blockSize);
            var prefixSize = fullPrefixBlockCount * blockSize + partialPrefixBlockSize;
            return prefixSize;
        }

        private static int FindFullPrefixBlockCount(int blockSize)
        {
            var x1 = EncryptionOracle(new byte[0]);
            var x2 = EncryptionOracle(new byte[]{0x00});
            var blocks1 = x1.Chunks(blockSize).ToArray();
            var blocks2 = x2.Chunks(blockSize).ToArray();
            var index = 0;
            for (; index < blocks1.Length; index++)
            {
                var b1 = blocks1[index];
                var b2 = blocks2[index];
                if (!b1.SequenceEqual(b2))
                {
                    break;
                }
            }
            return index;
        }

        private static int FindPartialPrefixBlockSize(int blockSize)
        {
            // Append repeating bytes to prefix until we get two repeated blocks in a row
            for (int i = 0; i < blockSize; i++)
            {
                var repeatingBytes = RepeatingBytes(blockSize * 2 + i);
                var encrypted = EncryptionOracle(repeatingBytes);
                if (HasRepeatedBlocks(encrypted, blockSize))
                {
                    return blockSize - i;
                }
            }
            return -1;
        }

        private static byte[] EncryptionOracle(byte[] data)
        {
            if (_randomPrefix == null || _randomPrefix.Length == 0)
            {
                var r = new Random();
                var prefixLength = r.Next(1, MaxPrefixLength + 1);
                _randomPrefix = SecureRng.GenerateRandomBytes(prefixLength);
            }
            var combined = new List<byte>();
            combined.AddRange(_randomPrefix);
            combined.AddRange(data);
            combined.AddRange(Convert.FromBase64String(EncodedSuffix));
            var padded = PaddingUtil.Pad(combined.ToArray(), BlockSizeBytes);
            return AesEcb.Encrypt(Key, padded);
        }

        private static int DecryptNextByte(int blockSize, int prefixSize, List<byte> knownBytes,
            int blockIndex)
        {
            string key;
            IEnumerable<byte> block;
            // Create a padding of repeating bytes
            var repeatingBytesLength = blockSize - prefixSize % blockSize;
            repeatingBytesLength += blockSize - knownBytes.Count % blockSize - 1;
            var repeatingBytes = RepeatingBytes(repeatingBytesLength);
            // Combine the repeating bytes and the known bytes
            var data = new List<byte>();
            data.AddRange(repeatingBytes);
            data.AddRange(knownBytes);
            data.Add(0x00);
            var dataArray = data.ToArray();
            // Calculate a dictionary of each possible byte value
            var results = new Dictionary<string, int>();
            for (int i = 0; i < 256; i++)
            {
                dataArray[dataArray.Length - 1] = (byte)i;
                var encryptedWithPrefix = EncryptionOracle(dataArray);
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

        private static bool HasRepeatedBlocks(byte[] data, int blockSize)
        {
            var blocks = Chunks(data, blockSize);
            for (int i = 0; i < blocks.Count - 1; i++)
            {
                if (blocks[i].SequenceEqual(blocks[i + 1]))
                {
                    return true;
                }
            }
            return false;
        }

        private static List<byte[]> Chunks(byte[] data, int chunkSize)
        {
            var chunks = new List<byte[]>();
            for (int i = 0; i < data.Length; i+= chunkSize)
            {
                chunks.Add(new byte[chunkSize]);
                Array.Copy(data, i, chunks[i/chunkSize], 0, chunkSize);
            }
            return chunks;
        }

        private static byte[] _randomPrefix;
    }
}
