namespace cryptopals.Lib.Text
{
    public class TextScore
    {
        #region Properties

        public string Text { get; }

        public int Score { get; } 

        #endregion

        public TextScore(string text, int score)
        {
            Text = text;
            Score = score;
        }
    }
}
