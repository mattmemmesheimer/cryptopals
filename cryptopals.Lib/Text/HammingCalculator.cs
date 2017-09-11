using System.Linq;
using System.Text;

namespace cryptopals.Lib.Text
{
    public static class HammingCalculator
    {
        public static int CalculateHammingDistance(string x, string y)
        {
            var xBytes = Encoding.ASCII.GetBytes(x);
            var yBytes = Encoding.ASCII.GetBytes(y);
            return CalculateHammingDistance(xBytes, yBytes);
        }
        
        public static int CalculateHammingDistance(byte[] x, byte[] y)
        {
            return x.Select((t, i) => CalculateHammingDistance(t, y[i])).Sum();
        }
        
        public static int CalculateHammingDistance(uint x, uint y)
        {
            int distance = 0;
            uint val = x ^ y;
            while (val != 0)
            {
                distance++;
                val &= val - 1;
            }
            return distance;
        }
    }
}
