using System;
using System.Collections.Generic;
using System.Text;

namespace cryptopals.Lib.Text
{
    public static class KvpParser
    {
        public static Dictionary<string, string> Decode(string encoded)
        {
            if (encoded == null)
            {
                throw new ArgumentNullException(nameof(encoded));
            }
            var result = new Dictionary<string, string>();
            var split = encoded.Split(new[] {'&'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var kvp in split)
            {
                var s = kvp.Split(new []{'='}, StringSplitOptions.None);
                if (s.Length != 2 || string.IsNullOrWhiteSpace(s[0]) || s[0] == string.Empty)
                {
                    throw new FormatException("Invalid format.");
                }
                var key = s[0];
                var value = s[1];
                if (result.ContainsKey(key))
                {
                    throw new ArgumentException("An element with the same key already exists.");
                }
                result[key] = value;
            }
            return result;
        }

        public static string Encode(Dictionary<string, string> input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            if (input.Count == 0)
            {
                return string.Empty;
            }
            var sb = new StringBuilder();
            foreach (var kvp in input)
            {
                var key = kvp.Key.Replace("&", string.Empty).Replace("=", string.Empty);
                var value = kvp.Value.Replace("&", string.Empty).Replace("=", string.Empty);
                sb.Append($"{key}={value}&");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}
