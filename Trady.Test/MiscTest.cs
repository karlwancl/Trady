using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Trady.Core;
using Trady.Core.Period;
using Trady.Importer;
using Trady.Analysis.Backtest;
using Trady.Analysis;
using AFunc = System.Func<System.Collections.Generic.IReadOnlyList<Trady.Core.Candle>, int, decimal?>;

namespace Trady.Test
{
    [TestClass]
    public class MiscTest
    {
        public async Task<IEnumerable<Candle>> ImportCandlesAsync()
        {
            var csvImporter = new CsvImporter("fb.csv", new CultureInfo("en-US"));
            return await csvImporter.ImportAsync("fb");
        }

		[TestMethod]
		public async Task TestFuncAnalyzableAsync()
		{
			var candles = await ImportCandlesAsync();

			AFunc aFunc = (c, i) => c[i].Close;
			var a = aFunc.AsAnalyzable(candles);
			var aResult = a.Sma(30, candles.Count() - 1);

			var results = candles.Sma(30);
			var result = results[candles.Count() - 1];
			Assert.AreEqual(aResult.Tick.Value, result.Tick.Value);
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

        private const string logPath = "backtest.txt";

        [TestMethod]
        public async Task TestBacktestAsync()
        {
            var fullCandles = await ImportCandlesAsync();
            var candles = fullCandles.Where(c => c.DateTime < new DateTime(2013, 1, 1));

            var buyRule = Rule.Create(ic => ic.IsMacdBullishCross(12, 26, 9));
            var sellRule = Rule.Create(ic => ic.IsMacdBearishCross(12, 26, 9));

            var runner = new Builder()
                .Add(candles)
                .Buy(buyRule)
                .Sell(sellRule)
                .Build();

            var file = File.Create(logPath);
            file.Dispose();

            runner.OnBought += Backtest_OnBought;
            runner.OnSold += Backtest_Onsold;

            var result = runner.RunAsync(10000).Result;
            var expecteds = new List<Transaction>
            {
                new Transaction(candles, 19, new DateTime(2012, 6, 15), TransactionType.Buy, 350, 9979.5m),
                new Transaction(candles, 40, new DateTime(2012, 7, 17), TransactionType.Sell, 350, 9967m),
                new Transaction(candles, 62, new DateTime(2012, 8, 16), TransactionType.Buy, 488, 9975.720488m),
                new Transaction(candles, 99, new DateTime(2012, 10, 9), TransactionType.Sell, 488, 9949.319512m),
                new Transaction(candles, 111, new DateTime(2012, 10, 25), TransactionType.Buy, 427, 9945.830427m),
                new Transaction(candles, 120, new DateTime(2012, 11,9), TransactionType.Sell, 427, 8521.919573m),
                new Transaction(candles, 124, new DateTime(2012, 11, 15), TransactionType.Buy, 382, 8534.88m),
                new Transaction(candles, 145, new DateTime(2012, 12, 17), TransactionType.Sell, 382, 10225.14m)
            };

            foreach (var expected in expecteds)
                Assert.IsTrue(result.Transactions.Any(t => t.Equals(expected)));

            Assert.IsTrue(result.TotalPrincipal == 10000m);
            Assert.IsTrue(result.TotalCorrectedBalance == 10227.44817m);
            Assert.IsTrue(result.TotalCorrectedProfitLossRatio == 0.022744817m);
        }

        private void Backtest_Onsold(IEnumerable<Candle> candles, int index, DateTime dateTime, decimal sellPrice, int quantity, decimal absCashFlow, decimal currentCashAmount, decimal plRatio)
        {
            File.AppendAllLines(logPath, new string[] { $"{index}({dateTime:yyyyMMdd}), Sell {candles.GetHashCode()}@{sellPrice} * {quantity}: {absCashFlow}, plRatio: {plRatio * 100:0.##}%, currentCashAmount: {currentCashAmount}" });
        }

        private void Backtest_OnBought(IEnumerable<Candle> candles, int index, DateTime dateTime, decimal buyPrice, int quantity, decimal absCashFlow, decimal currentCashAmount)
        {
            File.AppendAllLines(logPath, new string[] { $"{index}({dateTime:yyyyMMdd}), Buy {candles.GetHashCode()}@{buyPrice} * {quantity}: {absCashFlow}, currentCashAmount: {currentCashAmount}" });
        }
    }
}