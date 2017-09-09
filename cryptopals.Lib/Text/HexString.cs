using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cryptopals.Lib.Text
{
    public class HexString
    {
        #region Properties

        public IReadOnlyList<byte> Bytes { get; }

        #endregion

        public HexString(string hex)
        {
            if (hex == null)
            {
                throw new ArgumentNullException(nameof(hex));
            }
            if (hex.Length % 2 != 0)
            {
                throw new ArgumentException(nameof(hex));
            }
            Bytes = Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToList();
        }

        public HexString(IEnumerable<byte> bytes)
        {
            Bytes = bytes.ToList();
        }

        public override string ToString()
        {
            if (Bytes.Count == 0)
            {
                return string.Empty;
            }
            var hex = new StringBuilder(Bytes.Count * 2);
            foreach (byte b in Bytes)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }
    }
}
