using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trady.Core;
using Trady.Analysis.Indicator;

namespace Trady.Test
{
    [TestClass]
    public class NewIndicatorsTest
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
            var periodCount = 30;
            var sma = candles.SimpleMovingAverage(periodCount);
            var result = sma[candles.Count - 1];
            Assert.IsTrue(136.23m.IsApproximatelyEquals(result.Value));

            var smaOtherWay = new CandleSimpleMovingAverage(candles, periodCount);
            result = smaOtherWay[candles.Count - 1];
            Assert.IsTrue(136.23m.IsApproximatelyEquals(result.Value));

            var listOfDecimals = candles.Select(c => c.Close).ToList();
            var smaAnotherWay = new DecimalSimpleMovingAverage(listOfDecimals, periodCount);
            result = smaAnotherWay[candles.Count - 1];
            Assert.IsTrue(136.23m.IsApproximatelyEquals(result.Value));

            // I don't know how much it should return and actually it doesn't matter.
            // I just want to make a point that if someone wants to do strange calculations 
            // they can be done
            var smaWay3 = new SimpleMovingAverage2<Candle>(candles, c => c.High - c.Low, periodCount);
            result = smaWay3[candles.Count - 1];
            Assert.IsNotNull(result);

        }
    }
}
