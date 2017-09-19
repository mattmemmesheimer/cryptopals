using System;
using System.Linq;
using cryptopals.Lib.Extensions;

namespace cryptopals.Lib.Crypto.Aes
{
    public static class AesCbc
    {
        private static readonly int BlockSizeBytes = 16;

        public static byte[] Encrypt(byte[] key, byte[] iv, byte[] data)
        {
            int paddingLength = BlockSizeBytes - (data.Length % BlockSizeBytes);
            paddingLength = paddingLength == 0 ? BlockSizeBytes : paddingLength;
            int cipherTextLength = data.Length + paddingLength;
            var cipherText = new byte[cipherTextLength];
            var paddedData = PaddingUtil.Pad(data, BlockSizeBytes);
            var blocks = paddedData.Chunks(BlockSizeBytes);
            int index = 0;
            var previousBlock = iv;
            foreach (var block in blocks)
            {
                var xord = XorUtil.Xor(block.ToArray(), previousBlock);
                var encrypted = AesEcb.Encrypt(key, xord);
                Array.Copy(encrypted, 0, cipherText, index, BlockSizeBytes);
                previousBlock = encrypted;
                index += BlockSizeBytes;
            }
            return cipherText;
        }

        public static byte[] Decrypt(byte[] key, byte[] iv, byte[] data)
        {
            var blocks = data.Chunks(BlockSizeBytes);
            var clearText = new byte[data.Length];
            int index = 0;
            var previousBlock = iv;
            foreach (var block in blocks)
            {
                var blockArray = block.ToArray();
                var decrypted = AesEcb.Decrypt(key, blockArray);
                var xord = XorUtil.Xor(decrypted, previousBlock);
                Array.Copy(xord, 0, clearText, index, BlockSizeBytes);
                previousBlock = blockArray;
                index += BlockSizeBytes;
            }
            return PaddingUtil.RemovePad(clearText);
        }
    }
}
