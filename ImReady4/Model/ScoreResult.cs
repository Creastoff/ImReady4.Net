namespace ImReady4.Model
{
    public class ScoreResult(DateTime dateofScore, string score, string detail, int code)
    {
        public DateTime DateOfScore { get; set; } = dateofScore;
        public string Score { get; set; } = score;
        public string Detail { get; set; } = detail;
        public int Code { get; set; } = code;
    }
}