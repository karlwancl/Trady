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

namespace Trady.Test
{
    [TestClass]
    public class MiscallaneousTest
    {
        public async Task<IEnumerable<IOhlcv>> ImportIOhlcvDatasAsync()
        {
            var csvImporter = new CsvImporter("fb.csv", new CultureInfo("en-US"));
            return await csvImporter.ImportAsync("fb");
        }

		[TestMethod]
        public async Task TestRuleExecutorAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var rule = Rule.Create(ic => ic.IsAboveSma(30));
            IReadOnlyList<IOhlcv> validObjects;
            using (var ctx = new AnalyzeContext(candles))
                validObjects = new SimpleRuleExecutor(ctx, rule).Execute();
            Assert.IsTrue(validObjects.Count() == 882);
        }

        [TestMethod]
        public async Task TestTransformationAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();
            var transIOhlcvDatas = candles.Transform<Daily, Weekly>();
            var selectedIOhlcvData = transIOhlcvDatas.Where(c => c.DateTime.Equals(new DateTime(2017, 3, 13))).First();
            Assert.IsTrue(138.71m.IsApproximatelyEquals(selectedIOhlcvData.Open));
            Assert.IsTrue(140.34m.IsApproximatelyEquals(selectedIOhlcvData.High));
            Assert.IsTrue(138.49m.IsApproximatelyEquals(selectedIOhlcvData.Low));
            Assert.IsTrue(139.84m.IsApproximatelyEquals(selectedIOhlcvData.Close));
            Assert.IsTrue((77.5m * 1000000).IsApproximatelyEquals(selectedIOhlcvData.Volume));
        }
    }
}