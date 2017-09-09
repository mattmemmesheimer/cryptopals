using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cryptopals.Lib.Text;

namespace cryptopals.Lib.Crypto.Xor
{
    public class SingleByteXorCracker
    {
        public SingleByteXorCracker(ITextScoreCalculator textScoreCalculator)
        {
            _textScoreCalculator = textScoreCalculator;
        }

        public byte CrackKey(HexString hexString)
        {
            var scores = new Dictionary<byte, TextScore>();
            for (int key = 1; key < 127; key++)
            {
                var decrypted = XorUtil.Xor(hexString.Bytes.ToArray(), new[] { (byte)key });
                var plainText = System.Text.Encoding.ASCII.GetString(decrypted);
                scores.Add((byte)key, _textScoreCalculator.CalculateScore(plainText));
            }
            var ordered = scores.OrderByDescending(x => x.Value.Score);
            return ordered.First().Key;
        }

        public byte CrackKey(HexString[] hexStrings, out int index)
        {
            int currentMax = 0;
            index = -1;
            byte bestKey = 0;
            for (int i = 0; i < hexStrings.Length; i++)
            {
                var hexString = hexStrings[i];
                var key = CrackKey(hexString);
                var decrypted = XorUtil.Xor(hexString.Bytes.ToArray(), new[] { key });
                var plainText = System.Text.Encoding.ASCII.GetString(decrypted);
                var score = _textScoreCalculator.CalculateScore(plainText);
                if (score.Score > currentMax)
                {
                    currentMax = score.Score;
                    bestKey = key;
                    index = i;
                }
            }
            return bestKey;
        }

        private readonly ITextScoreCalculator _textScoreCalculator;
    }
}
