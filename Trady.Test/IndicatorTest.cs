using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Test
{
    [TestClass]
    public class IndicatorTest
    {
        public async Task<Equity> ImportEquityAsync()
        {
            var csvImporter = new Importer.CsvImporter("fb.csv");
            return await csvImporter.ImportAsync("fb");
        }

        [TestMethod]
        public async Task TestSmaAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new SimpleMovingAverage(equity, 30);
            Console.WriteLine($"Sma={equity.Sma(30)[equity.Count - 1].Sma}");
            var result = indicator.ComputeByIndex(equity.Count - 1).Sma;
            Assert.IsTrue(136.23m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestEmaAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new ExponentialMovingAverage(equity, 30);
            var result = indicator.ComputeByIndex(equity.Count - 1).Ema;
            Assert.IsTrue(136.09m.IsApproximatelyEquals(result.Value));
        }

        [TestMethod]
        public async Task TestAccumDistAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new AccumulationDistributionLine(equity);
            var result0 = indicator.ComputeByIndex(equity.Count - 1).AccumDist;
            var result1 = indicator.ComputeByIndex(equity.Count - 2).AccumDist;
            var result2 = indicator.ComputeByIndex(equity.Count - 3).AccumDist;
            Assert.IsTrue(result0 - result1 > 0 && result1 - result2 < 0);
        }

        [TestMethod]
        public async Task TestAroonAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new Aroon(equity, 25);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(96.0m.IsApproximatelyEquals(result.Up.Value));
            Assert.IsTrue(8.0m.IsApproximatelyEquals(result.Down.Value));
        }

        [TestMethod]
        public async Task TestAroonOscAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new AroonOscillator(equity, 25);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(88.0m.IsApproximatelyEquals(result.Osc.Value));
        }

        [TestMethod]
        public async Task TestAtrAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new AverageTrueRange(equity, 14);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(1.372m.IsApproximatelyEquals(result.Atr.Value));
        }

        [TestMethod]
        public async Task TestBbAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new BollingerBands(equity, 20, 2);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(134.04m.IsApproximatelyEquals(result.LowerBand.Value));
            Assert.IsTrue(137.59m.IsApproximatelyEquals(result.MiddleBand.Value));
            Assert.IsTrue(141.14m.IsApproximatelyEquals(result.UpperBand.Value));
        }

        [TestMethod]
        public async Task TestBbWidthAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new BollingerBandWidth(equity, 20, 2);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(5.165m.IsApproximatelyEquals(result.BandWidth.Value));
        }

        [TestMethod]
        public async Task TestChandlrAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new ChandelierExit(equity, 22, 3);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(135.69m.IsApproximatelyEquals(result.Long.Value));
            Assert.IsTrue(137.55m.IsApproximatelyEquals(result.Short.Value));
        }

        [TestMethod]
        public async Task TestClosePriceChangedAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new ClosePriceChange(equity);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(0.16m.IsApproximatelyEquals(result.Change.Value));
        }

        [TestMethod]
        public async Task TestClosePricePercentageChangedAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new ClosePricePercentageChange(equity);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(0.1145m.IsApproximatelyEquals(result.PercentageChange.Value));
        }

        [TestMethod]
        public async Task TestDmiAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new DirectionalMovementIndex(equity, 14);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(29.50m.IsApproximatelyEquals(result.Pdi.Value));
            Assert.IsTrue(7.91m.IsApproximatelyEquals(result.Mdi.Value));
            Assert.IsTrue(57.57m.IsApproximatelyEquals(result.Adx.Value));
        }

        [TestMethod]
        public async Task TestAdxrAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new AverageDirectionalMovementIndexRating(equity, 14, 3);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(56.73m.IsApproximatelyEquals(result.Adxr.Value));
        }

        [TestMethod]
        public async Task TestEfficiencyRatioAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new EfficiencyRatio(equity, 10);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(0.706m.IsApproximatelyEquals(result.Er.Value));
        }

        [TestMethod]
        public async Task TestKamaAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new KaufmanAdaptiveMovingAverage(equity, 10, 2, 30);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(138.91m.IsApproximatelyEquals(result.Kama.Value));
        }

        [TestMethod]
        public async Task TestEmaOscAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new ExponentialMovingAverageOscillator(equity, 10, 30);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(2.94m.IsApproximatelyEquals(result.Osc.Value));
        }

        [TestMethod]
        public async Task TestHighestHighAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new HighestHigh(equity, 10);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(140.34m.IsApproximatelyEquals(result.HighestHigh.Value));
        }

        [TestMethod]
        public async Task TestLowestLowAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new LowestLow(equity, 10);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(136.99m.IsApproximatelyEquals(result.LowestLow.Value));
        }

        [TestMethod]
        public async Task TestIchimokuCloudAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new IchimokuCloud(equity, 9, 26, 52);
            var results = indicator.Compute(null, null);

            File.WriteAllText("result.csv", string.Join("\n", results.Select(r => $"{r.DateTime},{r.ConversionLine},{r.BaseLine},{r.LeadingSpanA},{r.LeadingSpanB},{r.LaggingSpan}")));

            var currResult = results.Single(r => r.DateTime.Equals(new DateTime(2017, 3, 20)));
            Assert.IsTrue(138.70m.IsApproximatelyEquals(currResult.ConversionLine.Value));
            Assert.IsTrue(136.45m.IsApproximatelyEquals(currResult.BaseLine.Value));

            var leadingResult = results.Single(r => r.DateTime.Equals(new DateTime(2017, 4, 25)));
            Assert.IsTrue(137.57m.IsApproximatelyEquals(leadingResult.LeadingSpanA.Value));
            Assert.IsTrue(128.82m.IsApproximatelyEquals(leadingResult.LeadingSpanB.Value));

            var laggingResult = results.Single(r => r.DateTime.Equals(new DateTime(2017, 2, 10)));
            Assert.IsTrue(139.94m.IsApproximatelyEquals(laggingResult.LaggingSpan.Value));
        }

        [TestMethod]
        public async Task TestMemaAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new ModifiedExponentialMovingAverage(equity, 30);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(131.85m.IsApproximatelyEquals(result.Ema.Value));
        }

        [TestMethod]
        public async Task TestMacdAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new MovingAverageConvergenceDivergence(equity, 12, 26, 9);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(2.065m.IsApproximatelyEquals(result.Dif.Value));
            Assert.IsTrue(2.118m.IsApproximatelyEquals(result.Dem.Value));
            Assert.IsTrue((-0.053m).IsApproximatelyEquals(result.Osc.Value));
        }

        [TestMethod]
        public async Task TestObvAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new OnBalanceVolume(equity);
            var result0 = indicator.ComputeByIndex(equity.Count - 1);
            var result1 = indicator.ComputeByIndex(equity.Count - 2);
            var result2 = indicator.ComputeByIndex(equity.Count - 3);
            Assert.IsTrue(result0.Obv - result1.Obv > 0 && result1.Obv - result2.Obv < 0);
        }

        [TestMethod]
        public async Task TestRsvAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new RawStochasticsValue(equity, 14);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(90.61m.IsApproximatelyEquals(result.Rsv.Value));
        }

        [TestMethod]
        public async Task TestRsAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new RelativeStrength(equity, 14);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(2.66m.IsApproximatelyEquals(result.Rs.Value));
        }

        [TestMethod]
        public async Task TestRsiAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new RelativeStrengthIndex(equity, 14);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(73.03m.IsApproximatelyEquals(result.Rsi.Value));
        }

        [TestMethod]
        public async Task TestSmaOscAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new SimpleMovingAverageOscillator(equity, 10, 30);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(2.82m.IsApproximatelyEquals(result.Osc.Value));
        }

        [TestMethod]
        public async Task TestSdAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new StandardDeviation(equity, 10);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(0.93m.IsApproximatelyEquals(result.Sd.Value));
        }

        [TestMethod]
        public async Task TestFastStoAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new Stochastics.Fast(equity, 14, 3);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(90.61m.IsApproximatelyEquals(result.K.Value));
            Assert.IsTrue(92.31m.IsApproximatelyEquals(result.D.Value));
            Assert.IsTrue(87.21m.IsApproximatelyEquals(result.J.Value));
        }

        [TestMethod]
        public async Task TestSlowStoAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new Stochastics.Slow(equity, 14, 3);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(92.31m.IsApproximatelyEquals(result.K.Value));
            Assert.IsTrue(93.25m.IsApproximatelyEquals(result.D.Value));
            Assert.IsTrue(90.43m.IsApproximatelyEquals(result.J.Value));
        }

        [TestMethod]
        public async Task TestFullStoAsync()
        {
            var equity = await ImportEquityAsync();
            var indicator = new Stochastics.Full(equity, 14, 3, 3);
            var result = indicator.ComputeByIndex(equity.Count - 1);
            Assert.IsTrue(92.31m.IsApproximatelyEquals(result.K.Value));
            Assert.IsTrue(93.25m.IsApproximatelyEquals(result.D.Value));
            Assert.IsTrue(90.43m.IsApproximatelyEquals(result.J.Value));
        }
    }
}