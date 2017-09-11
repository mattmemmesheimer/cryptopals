using System.Collections.Generic;
using System.Linq;
using cryptopals.Lib.Extensions;
using cryptopals.Lib.Text;

namespace cryptopals.Lib.Crypto.Xor
{
    public class MultiByteXorCracker
    {
        #region Constants

        private const int MinKeySize = 2;
        private const int MaxKeySize = 40;
        private const int NumBlocksToAverage = 6;

        #endregion

        public MultiByteXorCracker(SingleByteXorCracker singleByteXorCracker)
        {
            _singleByteXorCracker = singleByteXorCracker;
        }

        public byte[] CrackKey(byte[] data)
        {
            //TODO: Improve key size guessing. Currently likely to be incorrect
            var keySize = GuessKeySize(data);
            var key = new byte[keySize];
            var transposed = Transpose(data, keySize);
            for (int i = 0; i < keySize; i++)
            {
                var hs = new HexString(transposed[i]);
                var k = _singleByteXorCracker.CrackKey(hs);
                key[i] = k;
            }
            var s = System.Text.Encoding.ASCII.GetString(key);
            return key;
        }

        private int GuessKeySize(byte[] data)
        {
            var results = new List<KeySizeDistance>();
            for (int keySizeGuess = MinKeySize; keySizeGuess <= MaxKeySize; keySizeGuess++)
            {
                var blocks = data.Batch(keySizeGuess).ToArray();
                var distances = new List<double>();
                for (int block = 0; block < NumBlocksToAverage; block++)
                {
                    var b1 = blocks[block].ToArray();
                    var b2 = blocks[block + 1].ToArray();
                    var distance = HammingCalculator.CalculateHammingDistance(b1, b2);
                    var normalized = (double)distance / keySizeGuess;
                    distances.Add(normalized);
                }
                if (distances.Count > 0)
                {
                    var average = distances.Average();
                    results.Add(new KeySizeDistance(keySizeGuess, average));
                }
            }
            var ordered = results.OrderBy(x => x.Distance).ToArray();
            return ordered.First().KeySize;
        }

        private List<List<byte>> Transpose(byte[] data, int keySize)
        {
            var transposed = new List<List<byte>>();
            for (int i = 0; i < keySize; i++)
            {
                transposed.Add(new List<byte>());
            }
            int index = 0;
            foreach (var b in data)
            {
                transposed[index++].Add(b);
                if (index >= keySize)
                {
                    index = 0;
                }
            }
            return transposed;
        }

        private class KeySizeDistance
        {
            public double Distance { get; }

            public int KeySize { get; }

            public KeySizeDistance(int keySize, double distance)
            {
                KeySize = keySize;
                Distance = distance;
            }
        }

        #region Fields
        
        private readonly SingleByteXorCracker _singleByteXorCracker;

        #endregion
    }
}
