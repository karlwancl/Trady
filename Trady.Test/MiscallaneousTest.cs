using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Trady.Core;
using Trady.Core.Period;
using Trady.Importer;
using Trady.Analysis;
using Trady.Analysis.Extension;
using Trady.Core.Infrastructure;
using Trady.Importer.Csv;
using Newtonsoft.Json;
using Trady.Analysis.Indicator;
using System.IO;
using System.Diagnostics;

namespace Trady.Test
{
    [TestClass]
    public class MiscallaneousTest
    {
        public async Task<IEnumerable<IOhlcv>> ImportIOhlcvDatasAsync()
        {
            var csvImporter = new CsvImporter("fb.csv", CultureInfo.GetCultureInfo("en-US"));
            return await csvImporter.ImportAsync("fb");
        }
        [TestMethod]

        public void TransformFromTrades()
        {
            int days = 3;
            int hours = days * 24;
            int minutes = hours * 60;
            int seconds = minutes * 60;

            var _tradeData = new ITickTrade[seconds];
            var d = new DateTimeOffset(new DateTime(2019, 1, 1));
            for (int i = 0; i < seconds; i++)
            {
                _tradeData[i] = new Trade(d.AddSeconds(i), 1, 1);
            }

            var candles1m = _tradeData.TransformToCandles<PerMinute>();
            Assert.IsTrue(candles1m.Any());
            Assert.IsTrue(candles1m.All(c => c.Volume == seconds / minutes));

            var candles1h = _tradeData.TransformToCandles<Hourly>();
            Assert.IsTrue(candles1h.Any());
            Assert.IsTrue(candles1h.All(c => c.Volume == seconds / hours));

            var candlesD = _tradeData.TransformToCandles<Daily>();
            Assert.IsTrue(candlesD.Any());
            Assert.IsTrue(candlesD.All(c => c.Volume == seconds / days));

        }

        [TestMethod]
        public async Task TestRuleExecutorAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var rule = Rule.Create(ic => ic.IsAboveSma(30));
            IReadOnlyList<IOhlcv> validObjects;
            using (var ctx = new AnalyzeContext(candles))
            {
                validObjects = new SimpleRuleExecutor(ctx, rule).Execute();
            }

            Assert.IsTrue(validObjects.Count == 882);
        }

        [TestMethod]
        public async Task TestRuleExecutorWithVerifyAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();

            void Verify(Predicate<IIndexedOhlcv> rule)
            {
                IReadOnlyList<IIndexedOhlcv> list;

                // Get all indexed candle who fulfill the rule
                using (var ctx = new AnalyzeContext(candles))
                {
                    var executor = new SimpleRuleExecutor(ctx, rule);
                    list = executor.Execute();
                }

                var indexList = list.Select(l => l.Index);
                //var indexString = string.Join(",", indexList);

                // Verify if fulfilled indexed candle is true or not
                foreach (var index in indexList)
                {
                    var ic = new IndexedCandle(candles, index);
                    Assert.IsTrue(rule(ic));
                }
            }

            bool buyRule(IIndexedOhlcv ic) => ic.IsEmaBullishCross(21, 55);
            Verify(buyRule);

            bool sellRule(IIndexedOhlcv ic) => ic.IsEmaBearishCross(21, 55);
            Verify(sellRule);
        }

        [TestMethod]
        public async Task TestTransformationAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var transIOhlcvDatas = candles.Transform<Daily, Weekly>();
            var selectedIOhlcvData = transIOhlcvDatas.First(c => c.DateTime.Equals(new DateTime(2017, 3, 13)));
            Assert.IsTrue(138.71m.IsApproximatelyEquals(selectedIOhlcvData.Open));
            Assert.IsTrue(140.34m.IsApproximatelyEquals(selectedIOhlcvData.High));
            Assert.IsTrue(138.49m.IsApproximatelyEquals(selectedIOhlcvData.Low));
            Assert.IsTrue(139.84m.IsApproximatelyEquals(selectedIOhlcvData.Close));
            Assert.IsTrue((77.5m * 1000000).IsApproximatelyEquals(selectedIOhlcvData.Volume));
        }

        /// <summary>
        /// Test case for issue #44: https://github.com/lppkarl/Trady/issues/44
        /// </summary>
        [TestMethod]
        public void TestRsiDivideByZero()
        {
            var jsonStr = "[0.00000397,0.00000398,0.00000399,0.00000399,0.00000398,0.00000398,0.00000398,0.00000399,0.00000397,0.00000398,0.00000397,0.00000399,0.00000398,0.00000397,0.00000398,0.00000398,0.00000398,0.00000398,0.00000398,0.00000398,0.00000398,0.00000398,0.00000397,0.00000398,0.00000398,0.00000398,0.00000397,0.00000397,0.00000397,0.00000398,0.00000398,0.00000397,0.00000398,0.00000398,0.00000398,0.00000398,0.00000398,0.00000397,0.00000397,0.00000397,0.00000397,0.00000397,0.00000398,0.00000397,0.00000398,0.00000397,0.00000397,0.00000398,0.00000397,0.00000398,0.00000398,0.00000397,0.00000397,0.00000397,0.00000398,0.00000396,0.00000396,0.00000397,0.00000396,0.00000396,0.00000395,0.00000396,0.00000396,0.00000395,0.00000396,0.00000396,0.00000396,0.00000396,0.00000396,0.00000396,0.00000396,0.00000396,0.00000395,0.00000395,0.00000394,0.00000393,0.00000393,0.00000394,0.00000393,0.00000393,0.00000393,0.00000394,0.00000393,0.00000394,0.00000393,0.00000392,0.00000393,0.00000392,0.00000394,0.00000395,0.00000394,0.00000395,0.00000395,0.00000395,0.00000394,0.00000394,0.00000393,0.00000391,0.00000391,0.00000391,0.00000392,0.00000390,0.00000391,0.00000389,0.00000390,0.00000389,0.00000389,0.00000390,0.00000389,0.00000388,0.00000388,0.00000389,0.00000388,0.00000389,0.00000392,0.00000394,0.00000395,0.00000395,0.00000394,0.00000395,0.00000395,0.00000394,0.00000394,0.00000394,0.00000393,0.00000391,0.00000391,0.00000391,0.00000389,0.00000389,0.00000388,0.00000390,0.00000389,0.00000389,0.00000393,0.00000392,0.00000392,0.00000392,0.00000392,0.00000392,0.00000392,0.00000392,0.00000392,0.00000391,0.00000393,0.00000393,0.00000392,0.00000393,0.00000394,0.00000393,0.00000394,0.00000393,0.00000392,0.00000392,0.00000392,0.00000391,0.00000392,0.00000391,0.00000392,0.00000391,0.00000391,0.00000391,0.00000392,0.00000392,0.00000393,0.00000394,0.00000393,0.00000394,0.00000393,0.00000394,0.00000393,0.00000395,0.00000394,0.00000394,0.00000394,0.00000394,0.00000394,0.00000393,0.00000394,0.00000395,0.00000393,0.00000394,0.00000394,0.00000393,0.00000394,0.00000392,0.00000393,0.00000392,0.00000392,0.00000392,0.00000393,0.00000392,0.00000393,0.00000392,0.00000393,0.00000393,0.00000393,0.00000393,0.00000392,0.00000392,0.00000392,0.00000393,0.00000393,0.00000393,0.00000392,0.00000392,0.00000392,0.00000392,0.00000392,0.00000392,0.00000392,0.00000391,0.00000391,0.00000392,0.00000391,0.00000392,0.00000391,0.00000391,0.00000391,0.00000391,0.00000391,0.00000391,0.00000391,0.00000391,0.00000391,0.00000391,0.00000390,0.00000390,0.00000391,0.00000390,0.00000390,0.00000389,0.00000390,0.00000390,0.00000390,0.00000390,0.00000391,0.00000391,0.00000391,0.00000390,0.00000390,0.00000390,0.00000391,0.00000391,0.00000391,0.00000390,0.00000390,0.00000390,0.00000390,0.00000390,0.00000391,0.00000392,0.00000391,0.00000393,0.00000393,0.00000393,0.00000394,0.00000393,0.00000392,0.00000394,0.00000392,0.00000393,0.00000393,0.00000391,0.00000393,0.00000393,0.00000392,0.00000392,0.00000393,0.00000391,0.00000392,0.00000392,0.00000391,0.00000391,0.00000393,0.00000392,0.00000392,0.00000392,0.00000393,0.00000393,0.00000392,0.00000392,0.00000392,0.00000393,0.00000392,0.00000393,0.00000392,0.00000392,0.00000393,0.00000393,0.00000393,0.00000394,0.00000393,0.00000393,0.00000394,0.00000393,0.00000393,0.00000392,0.00000393,0.00000393,0.00000393,0.00000393,0.00000394,0.00000393,0.00000392,0.00000393,0.00000393,0.00000394,0.00000393,0.00000392,0.00000393,0.00000392,0.00000393,0.00000393,0.00000392,0.00000393,0.00000393,0.00000392,0.00000393,0.00000393,0.00000393,0.00000392,0.00000392,0.00000393,0.00000392,0.00000393,0.00000392,0.00000393,0.00000391,0.00000392,0.00000392,0.00000393,0.00000392,0.00000392,0.00000393,0.00000392,0.00000392,0.00000393,0.00000392,0.00000393,0.00000392,0.00000393,0.00000392,0.00000392,0.00000393,0.00000392,0.00000391,0.00000392,0.00000392,0.00000392,0.00000392,0.00000392,0.00000392,0.00000392,0.00000392,0.00000392,0.00000393,0.00000392,0.00000393,0.00000392,0.00000393,0.00000392,0.00000393,0.00000392,0.00000392,0.00000391,0.00000393,0.00000392,0.00000393,0.00000393,0.00000393,0.00000392,0.00000392,0.00000392,0.00000392,0.00000392,0.00000392,0.00000393,0.00000393,0.00000392,0.00000393,0.00000393,0.00000392,0.00000392,0.00000392,0.00000393,0.00000393,0.00000394,0.00000393,0.00000394,0.00000393,0.00000393,0.00000394,0.00000394,0.00000394,0.00000394,0.00000393,0.00000394,0.00000394,0.00000393,0.00000392,0.00000394,0.00000393,0.00000392,0.00000393,0.00000393,0.00000394,0.00000394,0.00000393,0.00000393,0.00000393,0.00000393,0.00000393,0.00000393,0.00000393,0.00000392,0.00000393,0.00000393,0.00000392,0.00000393,0.00000392,0.00000392,0.00000393,0.00000392,0.00000392,0.00000393,0.00000393,0.00000392,0.00000391,0.00000392,0.00000391,0.00000391,0.00000392,0.00000392,0.00000392,0.00000391,0.00000391,0.00000392,0.00000391,0.00000392,0.00000391,0.00000390,0.00000392,0.00000391,0.00000392,0.00000391,0.00000391,0.00000392,0.00000392,0.00000391,0.00000392,0.00000391,0.00000391,0.00000391,0.00000391,0.00000392,0.00000392,0.00000391,0.00000392,0.00000390,0.00000391,0.00000391,0.00000392,0.00000392,0.00000392,0.00000392,0.00000391,0.00000391,0.00000391,0.00000391,0.00000391,0.00000392,0.00000392,0.00000392,0.00000391,0.00000392,0.00000393,0.00000393,0.00000393,0.00000392,0.00000392,0.00000392,0.00000393,0.00000392,0.00000393,0.00000392,0.00000393,0.00000393,0.00000393,0.00000393,0.00000393,0.00000393,0.00000393,0.00000393,0.00000394,0.00000393,0.00000393,0.00000394,0.00000393,0.00000393]";
            var inputs = JsonConvert.DeserializeObject<List<decimal?>>(jsonStr);

            var rsi = new RelativeStrengthIndexByTuple(inputs, 2); // 2 can be interchanged for 3 as well. 8 gives no errors
            var rsiResults = rsi.Compute();
            Assert.IsTrue(rsiResults?.Any() ?? false);
        }

        /// <summary>
        /// Test case for issue #53: https://github.com/lppkarl/Trady/issues/53
        /// </summary>
        [TestMethod]
        public async Task TestBullishCrossAlwaysFalseAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var rule = Rule.Create(ic => ic.IsEmaBullishCross(10, 30));
            IReadOnlyList<IOhlcv> validObjects;
            using (var ctx = new AnalyzeContext(candles))
            {
                validObjects = new SimpleRuleExecutor(ctx, rule).Execute();
            }

            Assert.IsTrue(validObjects.Count == 21);
        }
    }
}
