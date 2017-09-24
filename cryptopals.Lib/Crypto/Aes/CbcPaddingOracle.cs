using System;
using System.Collections.Generic;
using cryptopals.Lib.Extensions;

namespace cryptopals.Lib.Crypto.Aes
{
    public class CbcPaddingOracle
    {
        #region Constants

        public static readonly string[] Data = {
            "MDAwMDAwTm93IHRoYXQgdGhlIHBhcnR5IGlzIGp1bXBpbmc=",
            "MDAwMDAxV2l0aCB0aGUgYmFzcyBraWNrZWQgaW4gYW5kIHRoZSBWZWdhJ3MgYXJlIHB1bXBpbic=",
            "MDAwMDAyUXVpY2sgdG8gdGhlIHBvaW50LCB0byB0aGUgcG9pbnQsIG5vIGZha2luZw==",
            "MDAwMDAzQ29va2luZyBNQydzIGxpa2UgYSBwb3VuZCBvZiBiYWNvbg==",
            "MDAwMDA0QnVybmluZyAnZW0sIGlmIHlvdSBhaW4ndCBxdWljayBhbmQgbmltYmxl",
            "MDAwMDA1SSBnbyBjcmF6eSB3aGVuIEkgaGVhciBhIGN5bWJhbA==",
            "MDAwMDA2QW5kIGEgaGlnaCBoYXQgd2l0aCBhIHNvdXBlZCB1cCB0ZW1wbw==",
            "MDAwMDA3SSdtIG9uIGEgcm9sbCwgaXQncyB0aW1lIHRvIGdvIHNvbG8=",
            "MDAwMDA4b2xsaW4nIGluIG15IGZpdmUgcG9pbnQgb2g=",
            "MDAwMDA5aXRoIG15IHJhZy10b3AgZG93biBzbyBteSBoYWlyIGNhbiBibG93"
        };

        private static readonly byte[] Key =
        {
            0xCD, 0xFE, 0xD3, 0xD6, 0x52, 0x7C, 0xE5, 0x42,
            0xBF, 0x4D, 0x67, 0xB4, 0xE0, 0x46, 0x89, 0x97
        };

        private static readonly byte[] Iv =
        {
            0x00, 0x02, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
            0x00, 0x02, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07
        };

        private static readonly int BlockSizeBytes = 16;

        #endregion

        public static byte[] EncryptionOracle(out int randIndex)
        {
            var r = new Random();
            randIndex = r.Next(0, Data.Length);
            var input = Data[randIndex];
            return AesCbc.Encrypt(Key, Iv, Convert.FromBase64String(input));
        }

        public static bool IsValid(byte[] data)
        {
            var decryted = AesCbc.Decrypt(Key, Iv, data, false);
            return PaddingUtil.ValidPadding(decryted, BlockSizeBytes);
        }

        public static byte[] Decrypt(out int randIndex)
        {
            var c = EncryptionOracle(out randIndex);
            var blocks = c.Chunks(BlockSizeBytes);
            blocks.Insert(0, Iv);

            var knownBytes = new List<byte>();
            for (int i = 0; i < blocks.Count - 1; i++)
            {
                var previousBlock = blocks[i];
                var targetBlock = blocks[i + 1];
                var decrypted = Decrypt(targetBlock, previousBlock);
                knownBytes.AddRange(decrypted);
            }

            return PaddingUtil.RemovePad(knownBytes.ToArray());
        }

        private static byte[] Decrypt(byte[] targetBlock, byte[] previousBlock)
        {
            var knownBytes = new List<byte>();
            var bytes = new byte[BlockSizeBytes * 2];
            Array.Copy(previousBlock, 0, bytes, 0, BlockSizeBytes);
            Array.Copy(targetBlock, 0, bytes, BlockSizeBytes, BlockSizeBytes);
            for (int i = 0; i < BlockSizeBytes; i++)
            {
                var val = (byte) (i + 1);
                for (int k = 0; k < knownBytes.Count; k++)
                {
                    bytes[BlockSizeBytes - k - 1] =
                        (byte) (previousBlock[BlockSizeBytes - k - 1] ^ knownBytes[k] ^ val);
                }
                for (int g = 0; g < 255; g++)
                {
                    bytes[BlockSizeBytes - i - 1] =
                        (byte) (bytes[BlockSizeBytes - i - 1] ^ g ^ val);
                    //
                    if (IsValid(bytes) && !(g == val && g == 0x01))
                    {
                        knownBytes.Add((byte)g);
                        break;
                    }
                    Array.Copy(previousBlock, 0, bytes, 0, BlockSizeBytes - knownBytes.Count);
                }
                if (knownBytes.Count == 0)
                {
                    bytes[BlockSizeBytes - 1] = (byte) (previousBlock[BlockSizeBytes - 1] ^ 0x01);
                    knownBytes.Add(0x01);
                }
            }
            knownBytes.Reverse();
            return knownBytes.ToArray();
        }
    }
}
