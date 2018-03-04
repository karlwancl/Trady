using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trady.Analysis.Indicator;
using Trady.Core;
using Trady.Importer;
using System.Globalization;
using Trady.Analysis;
using Trady.Analysis.Extension;
using Trady.Core.Infrastructure;
using Trady.Importer.Csv;
using System;

namespace Trady.Test
{
    [TestClass]
    public class IndicatorTest
    {
        protected async Task<IEnumerable<IOhlcv>> ImportIOhlcvDatasAsync()
        {
            // Last record: 09/18/2017
            var csvImporter = new CsvImporter("fb.csv", CultureInfo.GetCultureInfo("en-US"));

            return await csvImporter.ImportAsync("fb");
            //var yahooImporter = new YahooFinanceImporter();
            //var candles = await yahooImporter.ImportAsync("FB");
            //File.WriteAllLines("fb.csv", candles.Select(c => $"{c.DateTime.ToString("d")},{c.Open},{c.High},{c.Low},{c.Close},{c.Volume}"));
            //return candles;
        }

        [TestMethod]
        public async Task TestCciAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Cci(20)[candles.Count() - 1];
            Assert.IsTrue(result.Tick.Value.IsApproximatelyEquals(8.17m));
        }

        [TestMethod]
        public async Task TestSmiAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Smi(15, 6, 6)[candles.Count() - 1];
            Assert.IsTrue(result.Tick.Value.IsApproximatelyEquals(55.1868m));
        }

        [TestMethod]
        public async Task TestStochRsiAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.StochRsi(14)[candles.Count() - 1];
            Assert.IsTrue(0m.IsApproximatelyEquals(result.Tick.Value));

            var result2 = candles.StochRsi(14).Single(v => v.DateTime == new DateTime(2017, 9, 15));
            Assert.IsTrue(0.317m.IsApproximatelyEquals(result2.Tick.Value));
        }

        [TestMethod]
        public async Task TestNmoAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Nmo(14)[candles.Count() - 1];
            Assert.IsTrue(0.58m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestDownMomentumAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var results = new DownMomentum(candles, 20);
            Assert.IsTrue(0m.IsApproximatelyEquals(results[candles.Count() - 1].Tick.Value));

            var results2 = new DownMomentum(candles, 3);
            Assert.IsTrue(3.04m.IsApproximatelyEquals(results2[candles.Count() - 1].Tick.Value));
        }

        [TestMethod]
        public async Task TestUpMomentumAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var results = new UpMomentum(candles, 20);
            Assert.IsTrue(2.6m.IsApproximatelyEquals(results[candles.Count() - 1].Tick.Value));

            var results2 = new UpMomentum(candles, 3);
            Assert.IsTrue(0m.IsApproximatelyEquals(results2[candles.Count() - 1].Tick.Value));
        }

        [TestMethod]
        public async Task TestRmAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Rm(20, 4)[candles.Count() - 1];
            Assert.IsTrue(1.4016m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestRmiAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Rmi(20, 4)[candles.Count() - 1];
            Assert.IsTrue(58.3607m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestDymoiAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Dymoi(5, 10, 14, 30, 5)[candles.Count() - 1];
            Assert.IsTrue(48.805m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestParabolicSarAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var results = candles.Sar(0.02m, 0.2m);
            Assert.IsTrue(173.93m.IsApproximatelyEquals(results[candles.Count() - 1].Tick.Value));
            Assert.IsTrue(169.92m.IsApproximatelyEquals(results[candles.Count() - 3].Tick.Value));
            Assert.IsTrue(157.36m.IsApproximatelyEquals(results.First(r => r.DateTime == new DateTime(2017, 7, 24)).Tick.Value));
        }

		[TestMethod]
		public async Task TestMedianAsync()
		{
			var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Median(10)[candles.Count() - 1];
			Assert.IsTrue(171.8649975m.IsApproximatelyEquals(result.Tick.Value));
		}

		[TestMethod]
		public async Task TestPercentileAsync()
		{
			var candles = await ImportIOhlcvDatasAsync();
			var result = candles.Percentile(30, 0.7m)[candles.Count() - 1];
			Assert.IsTrue(171.3529969m.IsApproximatelyEquals(result.Tick.Value));

            result = candles.Percentile(10, 0.2m)[candles.Count() - 1];
            Assert.IsTrue(170.9039978m.IsApproximatelyEquals(result.Tick.Value));
		}

        [TestMethod]
        public async Task TestSmaAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Sma(30)[candles.Count() - 1];
            Assert.IsTrue(170.15m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestEmaAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Ema(30)[candles.Count() - 1];
            Assert.IsTrue(169.55m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestAccumDistAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result0 = candles.AccumDist()[candles.Count() - 1];
            var result1 = candles.AccumDist()[candles.Count() - 2];
            var result2 = candles.AccumDist()[candles.Count() - 3];
            Assert.IsTrue(result0.Tick - result1.Tick < 0 && result1.Tick - result2.Tick > 0);
        }

        [TestMethod]
        public async Task TestAroonAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Aroon(25)[candles.Count() - 1];
            Assert.IsTrue(84.0m.IsApproximatelyEquals(result.Tick.Up.Value));
            Assert.IsTrue(48.0m.IsApproximatelyEquals(result.Tick.Down.Value));
        }

        [TestMethod]
        public async Task TestAroonOscAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.AroonOsc(25)[candles.Count() - 1];
            Assert.IsTrue(36.0m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestAtrAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Atr(14)[candles.Count() - 1];
            Assert.IsTrue(2.461m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestBbAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Bb(20, 2)[candles.Count() - 1];
            Assert.IsTrue(166.15m.IsApproximatelyEquals(result.Tick.LowerBand.Value));
            Assert.IsTrue(170.42m.IsApproximatelyEquals(result.Tick.MiddleBand.Value));
            Assert.IsTrue(174.70m.IsApproximatelyEquals(result.Tick.UpperBand.Value));
        }

        [TestMethod]
        public async Task TestBbWidthAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.BbWidth(20, 2)[candles.Count() - 1];
            Assert.IsTrue(5.013m.IsApproximatelyEquals(result.Tick.Value));
        }

		[TestMethod]
		public async Task TestChandlrAsync()
		{
			var candles = await ImportIOhlcvDatasAsync();
			var result = candles.Chandlr(22, 3)[candles.Count() - 1];
			Assert.IsTrue(166.47m.IsApproximatelyEquals(result.Tick.Long.Value));
			Assert.IsTrue(172.53m.IsApproximatelyEquals(result.Tick.Short.Value));
		}

        [TestMethod]
        public async Task TestMomentumAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Mtm()[candles.Count() - 1];
            Assert.IsTrue((-1.63m).IsApproximatelyEquals(result.Tick.Value));

            result = candles.Mtm(20)[candles.Count() - 1];

            Assert.IsTrue(2.599991m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestRateOfChangeAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Roc()[candles.Count() - 1];
            Assert.IsTrue((-0.949664419m).IsApproximatelyEquals(result.Tick.Value));

            result = candles.Roc(20)[candles.Count() - 1];

            Assert.IsTrue(1.55306788m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestPdiAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Pdi(14)[candles.Count() - 1];
            Assert.IsTrue(16.36m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestMdiAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Mdi(14)[candles.Count() - 1];
            Assert.IsTrue(19.77m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestAdxAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Adx(14)[candles.Count() - 1];
            Assert.IsTrue(15.45m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestAdxrAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Adxr(14, 3)[candles.Count() - 1];
            Assert.IsTrue(16.21m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestEfficiencyRatioAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Er(10)[candles.Count() - 1];
            Assert.IsTrue(0.147253482m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestKamaAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Kama(10, 2, 30)[candles.Count() - 1];
            Assert.IsTrue(164.88m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestEmaOscAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.EmaOsc(10, 30)[candles.Count() - 1];
            Assert.IsTrue(1.83m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestHighestHighAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.HighHigh(10)[candles.Count() - 1];
            Assert.IsTrue(174m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestHighestCloseAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.HighClose(10)[candles.Count() - 1];
            Assert.IsTrue(173.509995m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestHistoricalHighestHighAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.HistHighHigh()[candles.Count() - 1];   
            Assert.IsTrue(175.490005m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestHistoricalHighestCloseAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.HistHighClose()[candles.Count() - 1];
            Assert.IsTrue(173.509995m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestLowestLowAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.LowLow(10)[candles.Count() - 1];
            Assert.IsTrue(169.339996m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestLowestCloseAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.LowClose(10)[candles.Count() - 1];
            Assert.IsTrue(170.009995m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestHistoricalLowestLowAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.HistLowLow()[candles.Count() - 1];
            Assert.IsTrue(17.549999m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestHistoricalLowestCloseAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.HistLowClose()[candles.Count() - 1];
            Assert.IsTrue(17.73m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestIchimokuCloudAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            int middlePeriodCount = 26;
            var results = candles.Ichimoku(9, middlePeriodCount, 52);

            var currResult = results.ElementAt(results.Count() - middlePeriodCount - 1);
            Assert.IsTrue(171.67m.IsApproximatelyEquals(currResult.Tick.ConversionLine.Value));
            Assert.IsTrue(169.50m.IsApproximatelyEquals(currResult.Tick.BaseLine.Value));

            var leadingResult = results.Last();
            Assert.IsTrue(170.58m.IsApproximatelyEquals(leadingResult.Tick.LeadingSpanA.Value));
            Assert.IsTrue(161.74m.IsApproximatelyEquals(leadingResult.Tick.LeadingSpanB.Value));

            var laggingResult = results.ElementAt(results.Count() - 2 * middlePeriodCount - 1);
            Assert.IsTrue(170.01m.IsApproximatelyEquals(laggingResult.Tick.LaggingSpan.Value));
        }

        [TestMethod]
        public async Task TestMmaAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Mma(30)[candles.Count() - 1];
            Assert.IsTrue(169.9556615m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestMacdAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Macd(12, 26, 9)[candles.Count() - 1];
            Assert.IsTrue(1.25m.IsApproximatelyEquals(result.Tick.MacdLine.Value));
            Assert.IsTrue(1.541m.IsApproximatelyEquals(result.Tick.SignalLine.Value));
            Assert.IsTrue((-0.291m).IsApproximatelyEquals(result.Tick.MacdHistogram.Value));
        }

		[TestMethod]
		public async Task TestMacdHistogramAsync()
		{
			var candles = await ImportIOhlcvDatasAsync();
			var result = candles.MacdHist(12, 26, 9)[candles.Count() - 1];
            Assert.IsTrue((-0.291m).IsApproximatelyEquals(result.Tick.Value));
		}

        [TestMethod]
        public async Task TestObvAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result0 = candles.Obv()[candles.Count() - 1];
            var result1 = candles.Obv()[candles.Count() - 2];
            var result2 = candles.Obv()[candles.Count() - 3];
            Assert.IsTrue(result0.Tick - result1.Tick < 0 && result1.Tick - result2.Tick > 0);
        }

        [TestMethod]
        public async Task TestRsvAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Rsv(14)[candles.Count() - 1];
            Assert.IsTrue(55.66661111m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestRsAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Rs(14)[candles.Count() - 1];
            Assert.IsTrue(1.011667673m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestRsiAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Rsi(14)[candles.Count() - 1];
            Assert.IsTrue(50.29m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestSmaOscAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.SmaOsc(10, 30)[candles.Count() - 1];
            Assert.IsTrue(1.76m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestSdAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Sd(10)[candles.Count() - 1];
            Assert.IsTrue(1.17m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestFastStoAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.FastSto(14, 3)[candles.Count() - 1];
            Assert.IsTrue(55.67m.IsApproximatelyEquals(result.Tick.K.Value));
            Assert.IsTrue(65.22m.IsApproximatelyEquals(result.Tick.D.Value));
            Assert.IsTrue(36.57m.IsApproximatelyEquals(result.Tick.J.Value));
        }

        [TestMethod]
        public async Task TestSlowStoAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.SlowSto(14, 3)[candles.Count() - 1];
			Assert.IsTrue(65.22m.IsApproximatelyEquals(result.Tick.K.Value));
			Assert.IsTrue(74.36m.IsApproximatelyEquals(result.Tick.D.Value));
			Assert.IsTrue(46.94m.IsApproximatelyEquals(result.Tick.J.Value));
        }

        [TestMethod]
        public async Task TestFullStoAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var result = candles.FullSto(14, 3, 3)[candles.Count() - 1];
            Assert.IsTrue(65.22m.IsApproximatelyEquals(result.Tick.K.Value));
            Assert.IsTrue(74.36m.IsApproximatelyEquals(result.Tick.D.Value));
            Assert.IsTrue(46.94m.IsApproximatelyEquals(result.Tick.J.Value));
        }

		[TestMethod]
		public async Task TestFastStoOscAsync()
		{
			var candles = await ImportIOhlcvDatasAsync();
			var result = candles.FastStoOsc(14, 3)[candles.Count() - 1];
            Assert.IsTrue((-9.55m).IsApproximatelyEquals(result.Tick.Value));
		}

		[TestMethod]
		public async Task TestSlowStoOscAsync()
		{
			var candles = await ImportIOhlcvDatasAsync();
			var result = candles.SlowStoOsc(14, 3)[candles.Count() - 1];
			Assert.IsTrue((-9.14m).IsApproximatelyEquals(result.Tick.Value));
		}

		[TestMethod]
		public async Task TestFullStoOscAsync()
		{
			var candles = await ImportIOhlcvDatasAsync();
			var result = candles.FullStoOsc(14, 3, 3)[candles.Count() - 1];
			Assert.IsTrue((-9.14m).IsApproximatelyEquals(result.Tick.Value));
		}
    }
}
