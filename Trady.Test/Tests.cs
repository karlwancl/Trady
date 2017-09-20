using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Trady.Core;
using Trady.Core.Period;
using Trady.Importer;
using Trady.Analysis;

namespace Trady.Test
{
    [TestClass]
    public class Tests
    {
        public async Task<IEnumerable<Candle>> ImportCandlesAsync()
        {
            var csvImporter = new CsvImporter("fb.csv", new CultureInfo("en-US"));
            return await csvImporter.ImportAsync("fb");
        }

		[TestMethod]
        public async Task TestRuleExecutorAsync()
        {
            var candles = await ImportCandlesAsync();
            var rule = Rule.Create(ic => ic.IsAboveSma(30));
            IReadOnlyList<Candle> validObjects;
            using (var ctx = new AnalyzeContext(candles))
                validObjects = new SimpleRuleExecutor(ctx, rule).Execute();
            Assert.IsTrue(validObjects.Count() == 882);
        }

        [TestMethod]
        public async Task TestTransformationAsync()
        {
            var candles = await ImportCandlesAsync();
            var transCandles = candles.Transform<Daily, Weekly>();
            var selectedCandle = transCandles.Where(c => c.DateTime.Equals(new DateTime(2017, 3, 13))).First();
            Assert.IsTrue(138.71m.IsApproximatelyEquals(selectedCandle.Open));
            Assert.IsTrue(140.34m.IsApproximatelyEquals(selectedCandle.High));
            Assert.IsTrue(138.49m.IsApproximatelyEquals(selectedCandle.Low));
            Assert.IsTrue(139.84m.IsApproximatelyEquals(selectedCandle.Close));
            Assert.IsTrue((77.5m * 1000000).IsApproximatelyEquals(selectedCandle.Volume));
        }
    }
}