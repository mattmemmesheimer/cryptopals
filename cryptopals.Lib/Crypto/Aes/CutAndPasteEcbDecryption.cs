using System.Collections.Generic;
using cryptopals.Lib.Text;

namespace cryptopals.Lib.Crypto.Aes
{
    public static class CutAndPasteEcbDecryption
    {
        #region Constants

        private static readonly byte[] Key =
        {
            0x50, 0xAE, 0x36, 0xDD, 0x52, 0x3B, 0xEC, 0x49,
            0xBF, 0x2B, 0x1E, 0xB4, 0xEA, 0xBC, 0x89, 0xD8
        };

        private static int BlockSizeBytes = 16;

        #endregion

        public static string ProfileFor(string email)
        {
            var profile = new Dictionary<string, string>
            {
                { "email", email },
                { "uid", "10" },
                { "role", "user" }
            };
            return KvpParser.Encode(profile);
        }

        public static byte[] EncryptProfileFor(string email)
        {
            var profile = ProfileFor(email);
            var bytes = System.Text.Encoding.ASCII.GetBytes(profile);
            var padded = PaddingUtil.Pad(bytes, BlockSizeBytes);
            return AesEcb.Encrypt(Key, padded);
        }

        public static Dictionary<string, string> DecryptProfile(byte[] encrypted)
        {
            var decrypted = AesEcb.Decrypt(Key, encrypted);
            var encoded = PaddingUtil.RemovePad(decrypted);
            var decoded = KvpParser.Decode(System.Text.Encoding.ASCII.GetString(encoded));
            return decoded;
        }
    }
}
