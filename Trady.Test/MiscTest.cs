using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trady.Core;
using Trady.Core.Period;

namespace Trady.Test
{
    [TestClass]
    public class MiscTest
    {
        public async Task<IList<Candle>> ImportCandlesAsync()
        {
            var csvImporter = new Importer.CsvImporter("fb.csv");
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

        //[TestMethod]
        //public void TestPercentile()
        //{
        //    var list = new List<decimal> { 15, 20, 35, 40, 50 };
        //    decimal? result;

        //    result = Percentile(list, list.Count(), list.Count() - 1, 0.5m);
        //    Assert.IsTrue(result == 35);

        //    result = Percentile(list, list.Count(), list.Count() - 1, 0.4m);
        //    Assert.IsTrue(result == 29);

        //    result = Percentile(list, list.Count(), list.Count() - 1, 1m);
        //    Assert.IsTrue(result == 50);
        //}

        //public decimal? Percentile(IList<decimal> values, int periodCount, int index, decimal percentile)
        //{
        //    if (percentile < 0 || percentile > 1)
        //        throw new ArgumentException("Percentile should be between 0 and 1", nameof(percentile));

        //    if (index < periodCount - 1)
        //        return null;

        //    var subset = values.Skip(index - periodCount + 1).Take(periodCount).OrderBy(v => v).ToList();
        //    var idx = percentile * (subset.Count - 1) + 1;

        //    if (idx == 1) return subset[0];
        //    if (idx == subset.Count) return subset.Last();

        //    return subset[(int)idx - 1] + (subset[(int)idx] - subset[(int)idx - 1]) * (idx - (int)idx);
        //}
    }
}