using System.Collections.Generic;

namespace cryptopals.Lib.Crypto.Aes
{
    public class AesCbcBitFlipping
    {
        #region Constants

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

        private static readonly string Prefix = "comment1=cooking%20MCs;userdata=";

        private static readonly string Suffix = ";comment2=%20like%20a%20pound%20of%20bacon";

        private static readonly int BlockSizeBytes = 16;

        #endregion

        public static byte[] EncryptionOracle(string input)
        {
            var encoded = input.Replace(";", "%3B").Replace("=", "%3D");
            var b1 = System.Text.Encoding.ASCII.GetBytes(Prefix);
            var b2 = System.Text.Encoding.ASCII.GetBytes(Suffix);
            var bytes = new List<byte>();
            bytes.AddRange(b1);
            bytes.AddRange(System.Text.Encoding.ASCII.GetBytes(encoded));
            bytes.AddRange(b2);
            return AesCbc.Encrypt(Key, Iv, bytes.ToArray());
        }

        public static bool IsAdmin(byte[] data)
        {
            var decrypted = AesCbc.Decrypt(Key, Iv, data);
            var str = System.Text.Encoding.ASCII.GetString(decrypted);
            return str.Contains(";admin=true;");
        }
    }
}
