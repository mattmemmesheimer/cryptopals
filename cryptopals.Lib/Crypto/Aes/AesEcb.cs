using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using cryptopals.Lib.Extensions;
using cryptopals.Lib.Text;

namespace cryptopals.Lib.Crypto.Aes
{
    public static class AesEcb
    {
        public static byte[] Encrypt(byte[] data, byte[] key)
        {
            using (var alg = CreateAlgorithm())
            {
                alg.Key = key;
                var encryptor = alg.CreateEncryptor();
                return Transform(data, encryptor);
            }
        }

        public static byte[] Decrypt(byte[] data, byte[] key)
        {
            using (var alg = CreateAlgorithm())
            {
                alg.Key = key;
                var decryptor = alg.CreateDecryptor();
                return Transform(data, decryptor);
            }
        }

        public static bool IsEcbEncrypted(byte[] data, int blockSize = 16)
        {
            var blocks = data.Chunks(blockSize);
            var blockList = new List<string>();
            foreach (var block in blocks)
            {
                var hs = new HexString(block);
                if (blockList.Contains(hs.ToString()))
                {
                    return true;
                }
                blockList.Add(hs.ToString());
            }
            return false;
        }

        private static SymmetricAlgorithm CreateAlgorithm()
        {
            return new RijndaelManaged
            {
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
        }

        private static byte[] Transform(byte[] bytes, ICryptoTransform transform)
        {
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                {
                    cs.Write(bytes, 0, bytes.Length);
                }
                return ms.ToArray();
            }
        }
    }
}
