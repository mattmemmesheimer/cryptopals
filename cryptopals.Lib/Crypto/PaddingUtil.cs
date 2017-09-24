using System;
using System.Collections.Generic;

namespace cryptopals.Lib.Crypto
{
    public static class PaddingUtil
    {
        public static byte[] Pad(byte[] input, int blockSize)
        {
            int paddingBytes = blockSize - (input.Length % blockSize);
            if (paddingBytes == 0)
            {
                paddingBytes = blockSize;
            }
            var padded = new List<byte>(input);
            var paddingByte = (byte)paddingBytes;
            for (int i = 0; i < paddingBytes; i++)
            {
                padded.Add(paddingByte);
            }
            return padded.ToArray();
        }

        public static byte[] RemovePad(byte[] input)
        {
            if (input.Length == 0)
            {
                throw new ArgumentException("Array cannot be empty", nameof(input));
            }
            var paddingByte = input[input.Length - 1];
            var paddingLength = (int)paddingByte;
            int length = input.Length - paddingLength;
            var unpadded = new byte[length];
            Array.Copy(input, unpadded, length);
            return unpadded;
        }

        public static bool ValidPadding(byte[] input, int blockSize)
        {
            if (input.Length == 0)
            {
                throw new ArgumentException("Array cannot be empty", nameof(input));
            }
            if (input.Length % blockSize != 0)
            {
                return false;
            }
            var paddingByte = input[input.Length - 1];
            if (paddingByte == 0)
            {
                return false;
            }
            var paddingLength = (int) paddingByte;
            for (int i = 0; i < paddingLength; i++)
            {
                if (input[input.Length - 1 - i] != paddingByte)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
