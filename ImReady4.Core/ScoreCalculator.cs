using ImReady4.Core.Extensions;
using ImReady4.Core.Model;

namespace ImReady4.Core
{
    public class ScoreCalculator
    {
        private const int BASELINE_HISTORY_DURATION = 30;

        public List<ScoreResult> CalculateScores(List<Reading> readings)
        {
            readings = readings.OrderByDescending(r => r.DateOfReading).ToList();

            var log_RMSSD = readings.Select(r => Decimal.Round(r.RMSSD, 4))
                                    .Select(v => Math.Round(20 * DecimalMath.Log(v), 4)).ToList();
            
            var log_SDNN = readings.Select(r => Decimal.Round(r.SDNN, 4))
                                    .Select(v => Math.Round(20 * DecimalMath.Log(v), 4)).ToList();

            int iteratorLimit = GetLowestNumber(readings.Count, BASELINE_HISTORY_DURATION);

            decimal[] rMSSD_scores = new decimal[iteratorLimit];
            decimal[] SDNN_scores = new decimal[iteratorLimit];
            decimal[] rhr_scores = new decimal[iteratorLimit];

            for (int i = 0; i < iteratorLimit; i++)
            {
                int lastToTake = GetLowestNumber(i + BASELINE_HISTORY_DURATION, readings.Count);

                List<decimal> set;
                (decimal mean, decimal std) mean_and_std;

                set = log_RMSSD[i..lastToTake].ToList();
                mean_and_std = GetMeanAndSTDFromSet(set);
                rMSSD_scores[i] = Math.Round((log_RMSSD[i] - mean_and_std.mean) / mean_and_std.std, 4);

                set = log_SDNN[i..lastToTake].ToList();
                mean_and_std = GetMeanAndSTDFromSet(set);
                SDNN_scores[i] = Math.Round((log_SDNN[i] - mean_and_std.mean) / mean_and_std.std, 4);

                set = readings.Select(b => b.RestingHR)
                                .ToList()[i..lastToTake];
                mean_and_std = GetMeanAndSTDFromSet(set);
                rhr_scores[i] = Math.Round((readings[i].RestingHR - mean_and_std.mean) / mean_and_std.std, 4);
            }

            List<ScoreResult> scores = new List<ScoreResult>();

            for (int i = 0; i < BASELINE_HISTORY_DURATION; i++)
            {
                scores.Add(GetScore(readings[i].DateOfReading, rMSSD_scores[i], rhr_scores[i]));
            }

            return scores;
        }

        private ScoreResult GetScore(DateTime dateOfReading, decimal rMSSD_score, decimal RHR_score)
        {
            string score;
            string detail;
            int code;

            decimal one_seven = new decimal(1.7);

            if ((RHR_score <= 1) && (RHR_score > -1) && (rMSSD_score > 1))
            {
                score = "HIT";
                detail = "Ready for Intensive Training";
                code = 1;
            }
            else if (RHR_score <= -2 && rMSSD_score >= -1 && rMSSD_score < 0)
            {
                score = "LIT";
                detail = "Low intensity training";
                code = 2;
            }
            else if (RHR_score <= -2 && rMSSD_score >= 0)
            {
                score = "LIT!";
                detail = "Keep calm! Acute fatigue signs";
                code = 3;
            }
            else if (RHR_score < one_seven && rMSSD_score >= -1)
            {
                score = "Normal";
                detail = "Go on! Train as planned.";
                code = 4;
            }
            else if (rMSSD_score >= -1)
            {
                score = "LIT";
                detail = "Low intensity training";
                code = 2;
            }
            else if (RHR_score <= -2)
            {
                score = "Rest";
                detail = "Time to recover Avoid overtraining";
                code = 5;
            }
            else if (RHR_score <= one_seven)
            {
                score = "LIT";
                detail = "Low intensity training Recovery is not complete";
                code = 3;
            }
            else
            {
                score = "REST!";
                detail = "Be careful! Illness or stress detected";
                code = 6;
            }

            return new ScoreResult(dateOfReading, score, detail, code);
        }

        private (decimal mean, decimal std) GetMeanAndSTDFromSet(List<decimal> set)
        {
            var mean = Math.Round(set.Average(), 4);
            decimal std = DecimalMath.Sqrt(set.Average(v => DecimalMath.Power(v - mean, 2)));
            std = Math.Round(std, 4);

            return (mean, std);
        }

        private int GetLowestNumber(int num1, int num2)
        {
            return (num1 > num2 ? num2 : num1);
        }
    }
}