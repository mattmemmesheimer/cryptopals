using System.IO;
using System.Security.Cryptography;

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
