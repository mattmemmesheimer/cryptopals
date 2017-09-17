namespace cryptopals.Lib.Text
{
    public interface ITextScoreCalculator
    {
        TextScore CalculateScore(string text);
    }
}