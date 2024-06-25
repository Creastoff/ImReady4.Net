using ImReady4;
using ImReady4.Model;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text.Json;

namespace ImReady4Tests
{
    public class ScoreCalculationTests
    {
        private ScoreCalculator _sut;
        private static TestDataProvider _testDataProvider;

        [SetUp]
        public void Setup()
        {
            _sut = new ScoreCalculator();
            _testDataProvider = new TestDataProvider();
        }

        [Test]
        public void CorrectScoreCalculated()
        {
            var testData = _testDataProvider.SuccessfulTestData.ToList();
            var reading = new Reading(DateTime.Now, 65, new decimal(38.16), new decimal(72.54));

            var calculatedScores = _sut.CalculateScores(testData);
            var expectedResults = _testDataProvider.ExpectedSuccessfulResultData.ToList();

            Assert.AreEqual(JsonSerializer.Serialize(calculatedScores), JsonSerializer.Serialize(expectedResults));
        }
    }
}