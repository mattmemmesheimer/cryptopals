using System;
using System.Collections.Generic;
using cryptopals.Lib.Extensions;

namespace cryptopals.Lib.Crypto.Aes
{
    public static class AesCtr
    {
        private static readonly int BlockSizeBytes = 16;

        public static byte[] Decrypt(byte[] key, ulong nonce, byte[] data)
        {
            var nonceBytes = BitConverter.GetBytes(nonce);
            return Encrypt(key, nonceBytes, data);
        }

        public static byte[] Decrypt(byte[] key, byte[] nonce, byte[] data)
        {
            return Encrypt(key, nonce, data);
        }

        public static byte[] Encrypt(byte[] key, ulong nonce, byte[] data)
        {
            var nonceBytes = BitConverter.GetBytes(nonce);
            return Encrypt(key, nonceBytes, data);
        }

        public static byte[] Encrypt(byte[] key, byte[] nonce, byte[] data)
        {
            ulong counter = 0;
            var keyStream = new byte[BlockSizeBytes];
            var blocks = data.Chunks(BlockSizeBytes);
            var xord = new List<byte>(data.Length);
            foreach (var block in blocks)
            {
                var counterBytes = BitConverter.GetBytes(counter);
                Array.Copy(nonce, keyStream, nonce.Length);
                Array.Copy(counterBytes, 0, keyStream, nonce.Length, counterBytes.Length);
                var blockKey = AesEcb.Encrypt(key, keyStream);
                xord.AddRange(XorUtil.Xor(block, blockKey));
                counter++;
            }
            return xord.ToArray();
        }
    }
}
