using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Test
{
    [TestClass]
    public class IndicatorTest
    {
        async Task<IList<Candle>> ImportCandlesAsync()
        {
            var csvImporter = new Importer.CsvImporter("fb.csv", new CultureInfo("en-US"));
            return await csvImporter.ImportAsync("fb");
        }

        [TestMethod]
        public async Task TestSmaAsync()
        {
            var candles = await ImportCandlesAsync();
            var sma = new SimpleMovingAverage(candles, 30);
            var result = sma[candles.Count - 1];
            Assert.IsTrue(136.23m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestEmaAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new ExponentialMovingAverage(candles, 30);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(136.09m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestAccumDistAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new AccumulationDistributionLine(candles);
            var result0 = indicator[candles.Count - 1];
            var result1 = indicator[candles.Count - 2];
            var result2 = indicator[candles.Count - 3];
            Assert.IsTrue(result0 - result1 > 0 && result1 - result2 < 0);
        }

        [TestMethod]
        public async Task TestAroonAsync()
        {
            var equity = await ImportCandlesAsync();
            var candles = new Aroon(equity, 25);
            var result = candles[equity.Count - 1];
            Assert.IsTrue(96.0m.IsApproximatelyEquals(result.Up.Value));
            Assert.IsTrue(8.0m.IsApproximatelyEquals(result.Down.Value));
        }

        [TestMethod]
        public async Task TestAroonOscAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new AroonOscillator(candles, 25);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(88.0m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestAtrAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new AverageTrueRange(candles, 14);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(1.372m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestBbAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new BollingerBands(candles, 20, 2);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(134.04m.IsApproximatelyEquals(result.LowerBand.Value));
            Assert.IsTrue(137.59m.IsApproximatelyEquals(result.MiddleBand.Value));
            Assert.IsTrue(141.14m.IsApproximatelyEquals(result.UpperBand.Value));
        }

        [TestMethod]
        public async Task TestBbWidthAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new BollingerBandWidth(candles, 20, 2);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(5.165m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestChandlrAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new ChandelierExit(candles, 22, 3);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(135.69m.IsApproximatelyEquals(result.Long.Value));
            Assert.IsTrue(137.55m.IsApproximatelyEquals(result.Short.Value));
        }

        [TestMethod]
        public async Task TestClosePriceChangedAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new ClosePriceChange(candles);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(0.16m.IsApproximatelyEquals(result.Value));

            indicator = new ClosePriceChange(candles, 20);
            result = indicator[candles.Count - 1];
            Assert.IsTrue(6.41m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestClosePricePercentageChangedAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new ClosePricePercentageChange(candles);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(0.1145m.IsApproximatelyEquals(result.Value));

            indicator = new ClosePricePercentageChange(candles, 20);
            result = indicator[candles.Count - 1];
            Assert.IsTrue(4.800m.IsApproximatelyEquals(result.Value));

        }

        [TestMethod]
        public async Task TestPdiAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new PlusDirectionalIndicator(candles, 14);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(29.50m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestMdiAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new MinusDirectionalIndicator(candles, 14);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(7.91m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestAdxAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new AverageDirectionalIndex(candles, 14);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(57.57m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestAdxrAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new AverageDirectionalIndexRating(candles, 14, 3);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(56.73m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestEfficiencyRatioAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new EfficiencyRatio(candles, 10);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(0.706m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestKamaAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new KaufmanAdaptiveMovingAverage(candles, 10, 2, 30);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(138.91m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestEmaOscAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new ExponentialMovingAverageOscillator(candles, 10, 30);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(2.94m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestHighestHighAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new HighestHigh(candles, 10);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(140.34m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestLowestLowAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new LowestLow(candles, 10);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(136.99m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestIchimokuCloudAsync()
        {
            var candles = await ImportCandlesAsync();
            int middlePeriodCount = 26;
            var indicator = new IchimokuCloud(candles, 9, middlePeriodCount, 52);
            var results = indicator.Compute();

            var currResult = results[results.Count - middlePeriodCount - 1];
            Assert.IsTrue(138.70m.IsApproximatelyEquals(currResult.ConversionLine.Value));
            Assert.IsTrue(136.45m.IsApproximatelyEquals(currResult.BaseLine.Value));

            var leadingResult = results.Last();
            Assert.IsTrue(137.57m.IsApproximatelyEquals(leadingResult.LeadingSpanA.Value));
            Assert.IsTrue(128.82m.IsApproximatelyEquals(leadingResult.LeadingSpanB.Value));

            var laggingResult = results[results.Count - 2 * middlePeriodCount - 1];
            Assert.IsTrue(139.94m.IsApproximatelyEquals(laggingResult.LaggingSpan.Value));
        }

        [TestMethod]
        public async Task TestMemaAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new ModifiedExponentialMovingAverage(candles, 30);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(131.85m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestMacdAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new MovingAverageConvergenceDivergence(candles, 12, 26, 9);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(2.065m.IsApproximatelyEquals(result.MacdLine.Value));
            Assert.IsTrue(2.118m.IsApproximatelyEquals(result.SignalLine.Value));
            Assert.IsTrue((-0.053m).IsApproximatelyEquals(result.MacdHistogram.Value));
        }

        [TestMethod]
        public async Task TestObvAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new OnBalanceVolume(candles);
            var result0 = indicator[candles.Count - 1];
            var result1 = indicator[candles.Count - 2];
            var result2 = indicator[candles.Count - 3];
            Assert.IsTrue(result0 - result1 > 0 && result1 - result2 < 0);
        }

        [TestMethod]
        public async Task TestRsvAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new RawStochasticsValue(candles, 14);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(90.61m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestRsAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new RelativeStrength(candles, 14);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(2.66m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestRsiAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new RelativeStrengthIndex(candles, 14);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(73.03m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestSmaOscAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new SimpleMovingAverageOscillator(candles, 10, 30);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(2.82m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestSdAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new StandardDeviation(candles, 10);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(0.93m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestFastStoAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new Stochastics.Fast(candles, 14, 3);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(90.61m.IsApproximatelyEquals(result.K.Value));
            Assert.IsTrue(92.31m.IsApproximatelyEquals(result.D.Value));
            Assert.IsTrue(87.21m.IsApproximatelyEquals(result.J.Value));
        }

        [TestMethod]
        public async Task TestSlowStoAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new Stochastics.Slow(candles, 14, 3);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(92.31m.IsApproximatelyEquals(result.K.Value));
            Assert.IsTrue(93.25m.IsApproximatelyEquals(result.D.Value));
            Assert.IsTrue(90.43m.IsApproximatelyEquals(result.J.Value));
        }

        [TestMethod]
        public async Task TestFullStoAsync()
        {
            var candles = await ImportCandlesAsync();
            var indicator = new Stochastics.Full(candles, 14, 3, 3);
            var result = indicator[candles.Count - 1];
            Assert.IsTrue(92.31m.IsApproximatelyEquals(result.K.Value));
            Assert.IsTrue(93.25m.IsApproximatelyEquals(result.D.Value));
            Assert.IsTrue(90.43m.IsApproximatelyEquals(result.J.Value));
        }
    }
}