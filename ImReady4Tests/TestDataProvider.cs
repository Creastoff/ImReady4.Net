using ImReady4.Model;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImReady4Tests
{
    public class TestDataProvider
    {
        private List<Reading>? _successfulTestData { get; set; } 
        private List<ScoreResult>? _expectedSuccessfulResultData { get; set; } 

        public List<Reading> SuccessfulTestData
        {
            get
            {
                if (_successfulTestData is null)
                {
                    _successfulTestData = ParseCSV<Reading>(@"SuccessfulTestData.csv", true, fields =>
                    {
                        var dt = DateTime.Parse(fields[0]);
                        var rhr = Decimal.Parse(fields[1]);
                        var rmssd = Decimal.Parse(fields[2]);
                        var sdnn = Decimal.Parse(fields[3]);

                        return new Reading(dt, rhr, rmssd, sdnn);
                    });
                }

                return _successfulTestData;
            }
        }

        public List<ScoreResult> ExpectedSuccessfulResultData
        {
            get
            {
                if (_expectedSuccessfulResultData is null)
                {
                    _expectedSuccessfulResultData = ParseCSV<ScoreResult>("SuccessfulTestDataExpectedResults.csv", false, fields =>
                    {
                        var dt = DateTime.Parse(fields[0]);
                        var code = int.Parse(fields[3]);

                        return new ScoreResult(dt, fields[1], fields[2], code);
                    });
                }

                return _expectedSuccessfulResultData;
            }
        }

        private List<T> ParseCSV<T>(string fileName, bool skipHeader, Func<string[], T> getItem)
        {
            List<T> result = new List<T>();

            using (TextFieldParser parser = new TextFieldParser(fileName))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                bool skippedHeader = false;

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    if (skipHeader && !skippedHeader)
                    {
                        skippedHeader = true;
                        continue;
                    }

                    result.Add(getItem(fields!));
                }
            }

            return result;
        }
    }
}
