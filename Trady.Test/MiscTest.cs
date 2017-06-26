using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Trady.Core;
using Trady.Core.Period;
using System.IO;
using Trady.Analysis.Strategy;
using Trady.Analysis.Strategy.Rule;

namespace Trady.Test
{
    [TestClass]
    public class MiscTest
    {
        public async Task<IList<Candle>> ImportCandlesAsync()
        {
            var csvImporter = new Importer.CsvImporter("fb.csv", new CultureInfo("en-US"));
            return await csvImporter.ImportAsync("fb");
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

		const string logPath = "backtest.txt";

		[TestMethod]
		public async Task TestPortfolioAsync()
		{
			var candles = await ImportCandlesAsync();

			var buyRule = Rule.Create(ic => ic.IsMacdBullishCross(12, 26, 9));
			var sellRule = Rule.Create(ic => ic.IsMacdBearishCross(12, 26, 9));

			var portfolio = new Portfolio()
				.Add(candles)
				.Buy(buyRule)
				.Sell(sellRule);

			var file = File.Create(logPath);
			file.Dispose();

			portfolio.OnBought += Portfolio_OnBought;
			portfolio.OnSold += Portfolio_OnSold;

			var result = await portfolio.RunBacktestAsync(10000);
			var expecteds = new List<Transaction>
			{
				new Transaction(candles, 19, new DateTime(2012, 6, 15), TransactionType.Buy, 350, 9977.75m),
				new Transaction(candles, 40, new DateTime(2012, 7, 17), TransactionType.Sell, 350, 9967m),
				new Transaction(candles, 62, new DateTime(2012, 8, 16), TransactionType.Buy, 488, 9975.72m),
				new Transaction(candles, 99, new DateTime(2012, 10, 9), TransactionType.Sell, 488, 9949.32m)
			};

			foreach (var expected in expecteds)
				Assert.IsTrue(result.Transactions.Any(t => t.Equals(expected)));

			Assert.IsTrue(result.TotalPrincipal == 10000m);
			Assert.IsTrue(result.TotalCorrectedBalance == 19304.775m);
			Assert.IsTrue(result.TotalCorrectedProfitLossRatio == 0.9304775m);
		}

		void Portfolio_OnSold(IList<Candle> candles, int index, DateTime dateTime, decimal sellPrice, int quantity, decimal absCashFlow, decimal currentCashAmount, decimal plRatio)
		{
			File.AppendAllLines(logPath, new string[] { $"{index}({dateTime:yyyyMMdd}), Sell {candles.GetHashCode()}@{sellPrice} * {quantity}: {absCashFlow}, plRatio: {plRatio * 100:0.##}%, currentCashAmount: {currentCashAmount}" });
		}

		void Portfolio_OnBought(IList<Candle> candles, int index, DateTime dateTime, decimal buyPrice, int quantity, decimal absCashFlow, decimal currentCashAmount)
		{
			File.AppendAllLines(logPath, new string[] { $"{index}({dateTime:yyyyMMdd}), Buy {candles.GetHashCode()}@{buyPrice} * {quantity}: {absCashFlow}, currentCashAmount: {currentCashAmount}" });
		}
    }
}