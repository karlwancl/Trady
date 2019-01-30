using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trady.Analysis;
using Trady.Analysis.Backtest;
using Trady.Analysis.Backtest.FeeCalculators;
using Trady.Analysis.Extension;
using Trady.Analysis.Indicator;
using Trady.Core.Infrastructure;
using Trady.Core.Period;
using Trady.Importer.AlphaVantage;
using Trady.Importer.Csv;

namespace Trady.Test
{
    
    [TestClass]
    public class BacktestTest
    {
		public async Task<IEnumerable<IOhlcv>> ImportIOhlcvDatasAsync()
		{
			var csvImporter = new CsvImporter("fb.csv", CultureInfo.GetCultureInfo("en-US"));
			return await csvImporter.ImportAsync("fb");
		}

		private const string logPath = "backtest.txt";

        
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var file = File.Create(logPath);
            file.Dispose();
        }
        
		[TestMethod]
		public async Task TestBacktestAsync()
		{
			var fullIOhlcvDatas = await ImportIOhlcvDatasAsync();
			var candles = fullIOhlcvDatas.Where(c => c.DateTime < new DateTime(2013, 1, 1)).ToList();

			var buyRule = Rule.Create(ic => ic.IsMacdBullishCross(12, 26, 9));
			var sellRule = Rule.Create(ic => ic.IsMacdBearishCross(12, 26, 9));

			var runner = new Builder()
				.Add(candles)
				.Buy(buyRule)
				.Sell(sellRule)
				.Build();

			runner.OnBought += Backtest_OnBought;
			runner.OnSold += Backtest_Onsold;

			var result = runner.RunAsync(10000).Result;
			var expecteds = new List<Transaction>
			{
				new Transaction(candles, 19, new DateTime(2012, 6, 15), TransactionType.Buy, 350, 9979.5m, 1),
				new Transaction(candles, 40, new DateTime(2012, 7, 17), TransactionType.Sell, 350, 9967m, 1),
				new Transaction(candles, 62, new DateTime(2012, 8, 16), TransactionType.Buy, 488, 9975.720488m, 1),
				new Transaction(candles, 99, new DateTime(2012, 10, 9), TransactionType.Sell, 488, 9949.319512m, 1),
				new Transaction(candles, 111, new DateTime(2012, 10, 25), TransactionType.Buy, 427, 9945.830427m, 1),
				new Transaction(candles, 120, new DateTime(2012, 11,9), TransactionType.Sell, 427, 8521.919573m, 1),
				new Transaction(candles, 124, new DateTime(2012, 11, 15), TransactionType.Buy, 382, 8534.88m, 1),
				new Transaction(candles, 145, new DateTime(2012, 12, 17), TransactionType.Sell, 382, 10225.14m, 1)
			};
            
            CollectionAssert.AreEquivalent(expecteds, result.Transactions.ToList(), $"Actual collection :{Environment.NewLine}{ToString(result.Transactions)}");
		    Assert.AreEqual(10000m, result.TotalPrincipal);
		    Assert.AreEqual(10227.44817m, result.TotalCorrectedBalance);
		    Assert.AreEqual(0.022744817m, result.TotalCorrectedProfitLossRatio);
		}
        
        [TestMethod]
        public async Task TestBacktestWithPartialBaseCurrenciesAsync()
        {
            var fullIOhlcvDatas = await ImportIOhlcvDatasAsync();
            var candles = fullIOhlcvDatas.Where(c => c.DateTime < new DateTime(2013, 1, 1)).ToList();

            var buyRule = Rule.Create(ic => ic.IsMacdBullishCross(12, 26, 9));
            var sellRule = Rule.Create(ic => ic.IsMacdBearishCross(12, 26, 9));

            var runner = new Builder()
                .Add(candles)
                .BuyWithAllAvailableCash()
                .Buy(buyRule)
                .Sell(sellRule)
                .Build();

            runner.OnBought += Backtest_OnBought;
            runner.OnSold += Backtest_Onsold;

            var result = runner.RunAsync(10000).Result;
            var expecteds = new List<Transaction>
            {
                new Transaction(candles, 19, new DateTime(2012, 6, 15), TransactionType.Buy, 350.71904594878989828130480533m, 10000.000000000000000000000000m, 1),
                new Transaction(candles, 40, new DateTime(2012, 7, 17), TransactionType.Sell, 350.71904594878989828130480533m, 9987.478428621536303051560856m, 1),
                new Transaction(candles, 62, new DateTime(2012, 8, 16), TransactionType.Buy, 488.57524168523946271096370573m, 9987.478428621536303051560856m, 1),
                new Transaction(candles, 99, new DateTime(2012, 10, 9), TransactionType.Sell, 488.57524168523946271096370573m, 9961.048689386790959437087249m, 1),
                new Transaction(candles, 111, new DateTime(2012, 10, 25), TransactionType.Buy, 427.6534247201960600790479678m, 9961.048689386790959437087249m, 1),
                new Transaction(candles, 120, new DateTime(2012, 11, 9), TransactionType.Sell, 427.6534247201960600790479678m, 8534.961929761688638981737358m, 1),
                new Transaction(candles, 124, new DateTime(2012, 11, 15), TransactionType.Buy, 382.00366740204514946202942516m, 8534.961929761688638981737358m, 1),
                new Transaction(candles, 145, new DateTime(2012, 12, 17), TransactionType.Sell, 382.00366740204514946202942516m, 10225.238176352748651098527712m, 1)
            };
            
            CollectionAssert.AreEquivalent(expecteds, result.Transactions.ToList(), $"Actual collection :{Environment.NewLine}{ToString(result.Transactions)}");
            Assert.AreEqual(10000m, result.TotalPrincipal);
            Assert.AreEqual(10225.238176352748651098527712m, result.TotalCorrectedBalance);
            Assert.AreEqual(0.0225238176352748651098527712m, result.TotalCorrectedProfitLossRatio);
        }

        [TestMethod]
        public async Task TestBacktestWithFeesAsync()
        {
            var fullIOhlcvDatas = await ImportIOhlcvDatasAsync();
            var candles = fullIOhlcvDatas.Where(c => c.DateTime < new DateTime(2013, 1, 1)).ToList();

            var buyRule = Rule.Create(ic => ic.IsMacdBullishCross(12, 26, 9));
            var sellRule = Rule.Create(ic => ic.IsMacdBearishCross(12, 26, 9));

            var runner = new Builder()
                .Add(candles)
                .Calculator(new FeeCalculator(0.001m, 1, 2))
                .BuyWithAllAvailableCash()
                .Buy(buyRule)
                .Sell(sellRule)
                .Build();

            runner.OnBought += Backtest_OnBought;
            runner.OnSold += Backtest_Onsold;

            var result = runner.RunAsync(10000).Result;
            var expecteds = new List<Transaction>
            {
                new Transaction(candles, 19, new DateTime(2012, 6, 15), TransactionType.Buy, 350.36832690284110838302350052m, 10000.000000000000000000000000m, 1.35m),
                new Transaction(candles, 40, new DateTime(2012, 7, 17), TransactionType.Sell, 350.36832690284110838302350052m, 9967.512460242721851981760786m, 10.98m),
                new Transaction(candles, 62, new DateTime(2012, 8, 16), TransactionType.Buy, 487.11083467082409292102182506m, 9967.512460242721851981760786m, 1.49m),
                new Transaction(candles, 99, new DateTime(2012, 10, 9), TransactionType.Sell, 487.11083467082409292102182506m, 9921.258242395441315251706550m, 10.93m),
                new Transaction(candles, 111, new DateTime(2012, 10, 25), TransactionType.Buy, 425.51900208819423725814588172m, 9921.258242395441315251706550m, 1.43m),
                new Transaction(candles, 120, new DateTime(2012, 11, 9), TransactionType.Sell, 425.51900208819423725814588172m, 8483.866497305193532590876186m, 9.49m),
                new Transaction(candles, 124, new DateTime(2012, 11, 15), TransactionType.Buy, 379.33677846051424973403246687m, 8483.866497305193532590876186m, 1.38m),
                new Transaction(candles, 145, new DateTime(2012, 12, 17), TransactionType.Sell, 379.33677846051424973403246687m, 10143.691713828578498914669089m, 11.15m)
            };
            
            CollectionAssert.AreEquivalent(expecteds, result.Transactions.ToList(), $"Actual collection :{Environment.NewLine}{ToString(result.Transactions)}");
            Assert.AreEqual(10000m, result.TotalPrincipal);
            Assert.AreEqual(10143.691713828578498914669089m, result.TotalCorrectedBalance);
            Assert.AreEqual(0.0143691713828578498914669089m, result.TotalCorrectedProfitLossRatio);            
        }

        private string ToString(IEnumerable<Transaction> resultTransactions)
        {
            return string.Join(Environment.NewLine, resultTransactions.Select(t => $"                new Transaction(candles, {t.Index}, new DateTime({t.DateTime.Year}, {t.DateTime.Month}, {t.DateTime.Day}), TransactionType.{t.Type}, {t.Quantity}m, {t.AbsoluteCashFlow}m),"));
            //return string.Join(Environment.NewLine, resultTransactions.Select(t => t.ToString()));
        }

        private void Backtest_Onsold(IEnumerable<IOhlcv> candles, int index, DateTimeOffset dateTime, decimal sellPrice, decimal quantity, decimal absCashFlow, decimal currentCashAmount, decimal plRatio)
		{
			File.AppendAllLines(logPath, new[] { $"{index}({dateTime:yyyyMMdd}), Sell {candles.GetHashCode()}@{sellPrice} * {quantity}: {absCashFlow}, plRatio: {plRatio * 100:0.##}%, currentCashAmount: {currentCashAmount}" });
		}

		private void Backtest_OnBought(IEnumerable<IOhlcv> candles, int index, DateTimeOffset dateTime, decimal buyPrice, decimal quantity, decimal absCashFlow, decimal currentCashAmount)
		{
			File.AppendAllLines(logPath, new[] { $"{index}({dateTime:yyyyMMdd}), Buy {candles.GetHashCode()}@{buyPrice} * {quantity}: {absCashFlow}, currentCashAmount: {currentCashAmount}" });
		}

        [TestMethod]
        public async Task TestBacktestWithValidationAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();

            var buyRule = Rule.Create(ic => ic.IsEmaBullishCross(21, 55));
            var sellRule = Rule.Create(ic => ic.IsEmaBearishCross(21, 55));

            var runner = new Builder()
                .Add(candles)
                .Buy(buyRule)
                .Sell(sellRule)
                .Build();

            var result = await runner.RunAsync(1000);
            var transactionIndexes = result.Transactions.Select(t => t.Index);
            //var indexString = string.Join(",", transactionIndexes);

            foreach (var transactionIndex in transactionIndexes)
            {
                var ic = new IndexedCandle(candles, transactionIndex - 1);
                bool isBuyRuleValid = buyRule(ic);
                bool isSellRuleValid = sellRule(ic);
                Assert.IsTrue(isBuyRuleValid || isSellRuleValid);
            }
        }

        [TestMethod]
        public async Task TestIssue70()
        {
            //var importer = new AlphaVantageImporter("WUT9R5SC4IHCNA4S", OutputSize.full);
            //var fb = importer.ImportAsync("fb", startTime: new DateTime(2018, 1, 1), period: PeriodOption.PerMinute).Result;

            var importer2 = new AlphaVantageImporter("WUT9R5SC4IHCNA4S", OutputSize.full);
            var wba = importer2.ImportAsync("xom", startTime: new DateTime(2018, 1, 1), period: PeriodOption.PerMinute).Result;

            //var importer3 = new AlphaVantageImporter("WUT9R5SC4IHCNA4S", OutputSize.full);
            //var att = importer3.ImportAsync("t", startTime: new DateTime(2018, 1, 1), period: PeriodOption.PerMinute).Result;

            var buyRule = Rule.Create(c => c.IsMacdBullishCross(12, 26, 9))
                            .And(c => c.IsRsiOversold2());

            var sellRule = Rule.Create(c => c.IsMacdBearishCross(12, 26, 9))
                            .And(c => c.IsRsiOverbought2());

            var runner = new Trady.Analysis.Backtest.Builder()
                //.Add(att, 33).Add(wba, 33).Add(fb, 33)
                .Add(wba)
                .Buy(buyRule)
                .Sell(sellRule)
                //.FlatExchangeFeeRate(.05m)
                .FlatExchangeFeeRate(5)
                //.Premium(5)
                .Build();

            Assert.ThrowsException<AggregateException>(() => runner.RunAsync(10000, new DateTime(2018, 1, 1)).Result, "The FlatExchangeFee must be set to a value that is greater than equal to zero(>= 0) and less than one(< 1).Example => .25");
            //var result = runner.RunAsync(10000, new DateTime(2018, 1, 1)).Result;

            //Console.WriteLine($"Transaction count: {result.Transactions.Count():#.##}, P/L ratio: {result.TotalCorrectedProfitLossRatio * 100}%, Principal: {result.TotalPrincipal}, Total: {result.TotalCorrectedBalance}");
        }

        
    }

    public static class Signals
    {
        public static bool IsRsiOverbought2(this IIndexedOhlcv ic, int periodCount = 14)
            => ic.Get<RelativeStrengthIndex>(periodCount)[ic.Index].Tick.IsTrue(t => t >= 60);

        public static bool IsRsiOversold2(this IIndexedOhlcv ic, int periodCount = 14)
            => ic.Get<RelativeStrengthIndex>(periodCount)[ic.Index].Tick.IsTrue(t => t <= 40);
    }
}
