using System;
using System.Collections.Generic;

namespace cryptopals.Lib.Text
{
    public class BigramCalculator : ITextScoreCalculator
    {
        public TextScore CalculateScore(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            var score = 0;
            for (int i = 0; i < text.Length - 1; i++)
            {
                var x = text[i];
                var y = text[i + 1];
                var bigram = $"{x}{y}";
                if (BigramScores.ContainsKey(bigram))
                {
                    score += BigramScores[bigram];
                }
                else
                {
                    if (x < (int) AsciiByteValues.Space)
                    {
                        if (x != (int) AsciiByteValues.LineFeed)
                        {
                            score += (int) AsciiWeights.NonPrintableChar;
                        }
                    }
                    else if (char.IsLetterOrDigit(x))
                    {
                        if (char.IsLower(x))
                        {
                            score += (int) AsciiWeights.LowerCaseAlphaNumeric;
                        }
                        else
                        {
                            score += (int) AsciiWeights.UpperCaseAlphaNumeric;
                        }
                    }
                    else if (char.IsPunctuation(x))
                    {
                        score += (int) AsciiWeights.Punctuation;
                    }
                    else if (x == (int) AsciiByteValues.Space)
                    {
                        score += (int) AsciiWeights.Space;
                    }
                    else
                    {
                        score += (int) AsciiWeights.SpecialChar;
                    }
                }
            }
            return new TextScore(text, score);
        }

        private enum AsciiWeights
        {
            NonPrintableChar = -50,
            SpecialChar = -5,
            Space = 20,
            LowerCaseAlphaNumeric = 10,
            UpperCaseAlphaNumeric = 5,
            Punctuation = 3
        };

        private enum AsciiByteValues
        {
            Space = 32,
            LineFeed = 10
        };

        /// <summary>
        /// English bigram scores.
        /// </summary>
        /// <remarks>
        /// Source: https://en.wikipedia.org/wiki/Bigram
        /// </remarks>
        private static readonly Dictionary<string, int> BigramScores = new Dictionary<string, int>
        {
            {
                "th", 152
            },
            {
                "he", 128
            },
            {
                "in", 94
            },
            {
                "er", 94
            },
            {
                "an", 82
            },
            {
                "re", 68
            },
            {
                "nd", 63
            },
            {
                "at", 59
            },
            {
                "on", 57
            },
            {
                "nt", 56
            },
            {
                "ha", 56
            },
            {
                "es", 56
            },
            {
                "st", 55
            },
            {
                "en", 55
            },
            {
                "ed", 53
            },
            {
                "to", 52
            },
            {
                "it", 50
            },
            {
                "ou", 50
            },
            {
                "ea", 47
            },
            {
                "hi", 46
            },
            {
                "is", 46
            },
            {
                "or", 43
            },
            {
                "ti", 34
            },
            {
                "as", 33
            },
            {
                "te", 27
            },
            {
                "et", 19
            },
            {
                "ng", 18
            },
            {
                "of", 16
            },
            {
                "al", 9
            },
            {
                "de", 9
            },
            {
                "se", 8
            },
            {
                "le", 8
            },
            {
                "sa", 6
            },
            {
                "si", 5
            },
            {
                "ar", 4
            },
            {
                "ve", 4
            },
            {
                "ra", 4
            },
            {
                "ld", 2
            },
            {
                "ur", 2
            }
        };
    }
}
