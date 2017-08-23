using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trady.Core;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;

namespace Trady.Test
{
    [TestClass]
    public class NewIndicatorsTest
    {
        async Task<IEnumerable<Candle>> ImportCandlesAsync()
        {
            var csvImporter = new Importer.CsvImporter("fb.csv", new CultureInfo("en-US"));
            return await csvImporter.ImportAsync("fb");
        }


        [TestMethod]
        public async Task TestSmaAsync()
        {
            var candles = await ImportCandlesAsync();
            var periodCount = 30;
            var sma = candles.Sma(periodCount);
            var result = sma[candles.Count() - 1];
            Assert.IsTrue(136.23m.IsApproximatelyEquals(result.Tick.Value));

            var smaOtherWay = new SimpleMovingAverage(candles, periodCount);
            result = smaOtherWay[candles.Count() - 1];
            Assert.IsTrue(136.23m.IsApproximatelyEquals(result.Tick.Value));

            var listOfDecimals = candles.Select(c => c.Close).ToList();
            var smaAnotherWay = new SimpleMovingAverageByTuple(listOfDecimals, periodCount);
            var tupleResult = smaAnotherWay[candles.Count() - 1];
            Assert.IsTrue(136.23m.IsApproximatelyEquals(tupleResult.Value));

            // I would just keep it there now, as it provides greater flexibiilty for developers to customize their input/output
            // But it seems not all indicators are meaningful to provide the mapping parameter
            // For sma, it's generic to its input, the computed result are meaningful. but for some others like rsi, it might not be the case
            // But who knows, meaning is defined by the users, we can't predict the usage by users
            // So I will just keep it here but it's open to discussion

            // I don't know how much it should return and actually it doesn't matter.
            // I just want to make a point that if someone wants to do strange calculations 
            // they can be done
            var smaWay3 = new SimpleMovingAverage<Candle, AnalyzableTick<decimal?>>(candles, c => c.High - c.Low, periodCount);
            result = smaWay3[candles.Count() - 1];
            Assert.IsNotNull(result);

        }
    }
}
