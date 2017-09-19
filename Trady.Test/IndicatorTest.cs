using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trady.Analysis.Indicator;
using Trady.Core;
using Trady.Importer;
using Trady.Analysis;
using System.IO;
using System.Globalization;

namespace Trady.Test
{
    [TestClass]
    public class IndicatorTest
    {
        protected async Task<IEnumerable<Candle>> ImportCandlesAsync()
        {
            var csvImporter = new CsvImporter("fb.csv", new CultureInfo("en-US"));
            return await csvImporter.ImportAsync("fb");
            //var yahooImporter = new YahooFinanceImporter();
            //var candles = await yahooImporter.ImportAsync("FB");
            //File.WriteAllLines("fb.csv", candles.Select(c => $"{c.DateTime.ToString("d")},{c.Open},{c.High},{c.Low},{c.Close},{c.Volume}"));
            //return candles;
        }

		[TestMethod]
		public async Task TestMedianAsync()
		{
			var candles = await ImportCandlesAsync();
            var median = new Median(candles, 10);
			var result = median[candles.Count() - 1];
			Assert.IsTrue(171.8649975m.IsApproximatelyEquals(result.Tick.Value));
		}

		[TestMethod]
		public async Task TestPercentileAsync()
		{
			var candles = await ImportCandlesAsync();
            var percentile = new Percentile(candles, 30, 0.7m);
			var result = percentile[candles.Count() - 1];
			Assert.IsTrue(171.3529969m.IsApproximatelyEquals(result.Tick.Value));

            percentile = new Percentile(candles, 10, 0.2m);
            result = percentile[candles.Count() - 1];
            Assert.IsTrue(170.9039978m.IsApproximatelyEquals(result.Tick.Value));
		}

        [TestMethod]
        public async Task TestSmaAsync()
        {
            var candles = await ImportCandlesAsync();
            var sma = new SimpleMovingAverage(candles, 30);
            var result = sma[candles.Count() - 1];
            Assert.IsTrue(170.15m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestEmaAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new ExponentialMovingAverage(candles, 30);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(169.55m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestAccumDistAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new AccumulationDistributionLine(candles);
            var result0 = indicator[candles.Count() - 1];
            var result1 = indicator[candles.Count() - 2];
            var result2 = indicator[candles.Count() - 3];
            Assert.IsTrue(result0.Tick - result1.Tick < 0 && result1.Tick - result2.Tick > 0);
        }

        [TestMethod]
        public async Task TestAroonAsync()
        {
            var equity = await ImportCandlesAsync();
            var candles = new Aroon(equity, 25);
            var result = candles[equity.Count() - 1];
            Assert.IsTrue(84.0m.IsApproximatelyEquals(result.Tick.Up.Value));
            Assert.IsTrue(48.0m.IsApproximatelyEquals(result.Tick.Down.Value));
        }

        [TestMethod]
        public async Task TestAroonOscAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new AroonOscillator(candles, 25);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(36.0m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestAtrAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new AverageTrueRange(candles, 14);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(2.461m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestBbAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new BollingerBands(candles, 20, 2);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(166.15m.IsApproximatelyEquals(result.Tick.LowerBand.Value));
            Assert.IsTrue(170.42m.IsApproximatelyEquals(result.Tick.MiddleBand.Value));
            Assert.IsTrue(174.70m.IsApproximatelyEquals(result.Tick.UpperBand.Value));
        }

        [TestMethod]
        public async Task TestBbWidthAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new BollingerBandWidth(candles, 20, 2);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(5.013m.IsApproximatelyEquals(result.Tick.Value));
        }

		[TestMethod]
		public async Task TestChandlrAsync()
		{
			var candles = await ImportCandlesAsync();
			var indicator = new ChandelierExit(candles, 22, 3);
			var result = indicator[candles.Count() - 1];
			Assert.IsTrue(166.47m.IsApproximatelyEquals(result.Tick.Long.Value));
			Assert.IsTrue(172.53m.IsApproximatelyEquals(result.Tick.Short.Value));
		}

        [TestMethod]
        public async Task TestClosePriceChangeAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new ClosePriceChange(candles);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue((-1.63m).IsApproximatelyEquals(result.Tick.Value));

            indicator = new ClosePriceChange(candles, 20);
            result = indicator[candles.Count() - 1];
            Assert.IsTrue(2.599991m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestClosePricePercentageChangeAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new ClosePricePercentageChange(candles);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue((-0.949664419m).IsApproximatelyEquals(result.Tick.Value));

            indicator = new ClosePricePercentageChange(candles, 20);
            result = indicator[candles.Count() - 1];
            Assert.IsTrue(1.55306788m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestPdiAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new PlusDirectionalIndicator(candles, 14);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(16.36m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestMdiAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new MinusDirectionalIndicator(candles, 14);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(19.77m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestAdxAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new AverageDirectionalIndex(candles, 14);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(15.45m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestAdxrAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new AverageDirectionalIndexRating(candles, 14, 3);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(16.21m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestEfficiencyRatioAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new EfficiencyRatio(candles, 10);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(0.147253482m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestKamaAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new KaufmanAdaptiveMovingAverage(candles, 10, 2, 30);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(164.88m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestEmaOscAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new ExponentialMovingAverageOscillator(candles, 10, 30);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(1.83m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestHighestHighAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new HighestHigh(candles, 10);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(174m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestHighestCloseAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new HighestClose(candles, 10);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(173.509995m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestHistoricalHighestHighAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new HistoricalHighestHigh(candles);
            var result = indicator[candles.Count() - 1];   
            Assert.IsTrue(175.490005m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestHistoricalHighestCloseAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new HistoricalHighestClose(candles);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(173.509995m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestLowestLowAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new LowestLow(candles, 10);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(169.339996m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestLowestCloseAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new LowestClose(candles, 10);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(170.009995m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestHistoricalLowestLowAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new HistoricalLowestLow(candles);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(17.549999m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestHistoricalLowestCloseAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new HistoricalLowestClose(candles);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(17.73m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestIchimokuCloudAsync()
        {
            var candles = await ImportCandlesAsync();
            int middlePeriodCount = 26;
            var indicator = new IchimokuCloud(candles, 9, middlePeriodCount, 52);
            var results = indicator.Compute();

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
            var candles = await ImportCandlesAsync();
            var indicator = new ModifiedMovingAverage(candles, 30);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(169.9556615m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestMacdAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new MovingAverageConvergenceDivergence(candles, 12, 26, 9);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(1.25m.IsApproximatelyEquals(result.Tick.MacdLine.Value));
            Assert.IsTrue(1.541m.IsApproximatelyEquals(result.Tick.SignalLine.Value));
            Assert.IsTrue((-0.291m).IsApproximatelyEquals(result.Tick.MacdHistogram.Value));
        }

		[TestMethod]
		public async Task TestMacdHistogramAsync()
		{
			var candles = await ImportCandlesAsync();
			var indicator = new MovingAverageConvergenceDivergenceHistogram(candles, 12, 26, 9);
			var result = indicator[candles.Count() - 1];
            Assert.IsTrue((-0.291m).IsApproximatelyEquals(result.Tick.Value));
		}

        [TestMethod]
        public async Task TestObvAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new OnBalanceVolume(candles);
            var result0 = indicator[candles.Count() - 1];
            var result1 = indicator[candles.Count() - 2];
            var result2 = indicator[candles.Count() - 3];
            Assert.IsTrue(result0.Tick - result1.Tick < 0 && result1.Tick - result2.Tick > 0);
        }

        [TestMethod]
        public async Task TestRsvAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new RawStochasticsValue(candles, 14);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(55.66661111m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestRsAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new RelativeStrength(candles, 14);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(1.011667673m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestRsiAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new RelativeStrengthIndex(candles, 14);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(50.29m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestSmaOscAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new SimpleMovingAverageOscillator(candles, 10, 30);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(1.76m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestSdAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new StandardDeviation(candles, 10);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(1.17m.IsApproximatelyEquals(result.Tick.Value));
        }

        [TestMethod]
        public async Task TestFastStoAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new Stochastics.Fast(candles, 14, 3);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(55.67m.IsApproximatelyEquals(result.Tick.K.Value));
            Assert.IsTrue(65.22m.IsApproximatelyEquals(result.Tick.D.Value));
            Assert.IsTrue(36.57m.IsApproximatelyEquals(result.Tick.J.Value));
        }

        [TestMethod]
        public async Task TestSlowStoAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new Stochastics.Slow(candles, 14, 3);
            var result = indicator[candles.Count() - 1];
			Assert.IsTrue(65.22m.IsApproximatelyEquals(result.Tick.K.Value));
			Assert.IsTrue(74.36m.IsApproximatelyEquals(result.Tick.D.Value));
			Assert.IsTrue(46.94m.IsApproximatelyEquals(result.Tick.J.Value));
        }

        [TestMethod]
        public async Task TestFullStoAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new Stochastics.Full(candles, 14, 3, 3);
            var result = indicator[candles.Count() - 1];
            Assert.IsTrue(65.22m.IsApproximatelyEquals(result.Tick.K.Value));
            Assert.IsTrue(74.36m.IsApproximatelyEquals(result.Tick.D.Value));
            Assert.IsTrue(46.94m.IsApproximatelyEquals(result.Tick.J.Value));
        }

		[TestMethod]
		public async Task TestFastStoOscAsync()
		{
			var candles = await ImportCandlesAsync();
			var indicator = new StochasticsOscillator.Fast(candles, 14, 3);
			var result = indicator[candles.Count() - 1];
            Assert.IsTrue((-9.55m).IsApproximatelyEquals(result.Tick.Value));
		}

		[TestMethod]
		public async Task TestSlowStoOscAsync()
		{
			var candles = await ImportCandlesAsync();
			var indicator = new StochasticsOscillator.Slow(candles, 14, 3);
			var result = indicator[candles.Count() - 1];
			Assert.IsTrue((-9.14m).IsApproximatelyEquals(result.Tick.Value));
		}

		[TestMethod]
		public async Task TestFullStoOscAsync()
		{
			var candles = await ImportCandlesAsync();
			var indicator = new StochasticsOscillator.Full(candles, 14, 3, 3);
			var result = indicator[candles.Count() - 1];
			Assert.IsTrue((-9.14m).IsApproximatelyEquals(result.Tick.Value));
		}
    }
}