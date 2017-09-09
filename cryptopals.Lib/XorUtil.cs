namespace cryptopals.Lib
{
    public static class XorUtil
    {
        public static byte[] Xor(byte[] inputBytes, byte[] key)
        {
            var keyIndex = 0;
            var maxKeyIndex = key.Length;
            var xord = new byte[inputBytes.Length];
            for (int i = 0; i < inputBytes.Length; i++)
            {
                if (keyIndex >= maxKeyIndex)
                {
                    keyIndex = 0;
                }
                xord[i] = (byte)(inputBytes[i] ^ key[keyIndex++]);
            }
            return xord;
        }
    }
}
