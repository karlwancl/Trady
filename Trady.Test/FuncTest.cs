using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trady.Analysis;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Importer;
using AFunc = System.Func<System.Collections.Generic.IReadOnlyList<Trady.Core.Candle>, int, Trady.Core.Infrastructure.IAnalyzeContext<Trady.Core.Candle>, decimal?>;

namespace Trady.Test
{
    [TestClass]
    public class FuncTest
    {
		public async Task<IEnumerable<Candle>> ImportCandlesAsync()
		{
			var csvImporter = new CsvImporter("fb.csv", new CultureInfo("en-US"));
			return await csvImporter.ImportAsync("fb");
		}
         
        [TestMethod]
        public async Task TestGetFuncFromContextAsync()
        {
			var candles = await ImportCandlesAsync();

            FuncRegistry.Register("msma", "var sma = ctx.Get<SimpleMovingAverage>(10); return sma[i].Tick;");
            FuncRegistry.Register("msma2", (c, i, p0, ctx) => ctx.Get<SimpleMovingAverage>(p0)[i].Tick);

            var result = candles.Func("msma", 10m)[candles.Count() - 1];
            var result2 = candles.Func("msma2", 10m)[candles.Count() - 1];
            var actual = candles.Sma(10)[candles.Count() - 1];

            Assert.AreEqual(result.Tick, actual.Tick);
            Assert.AreEqual(result2.Tick, actual.Tick);
		}

        [TestMethod]
        public async Task TestGetFromContextAsync()
        {
			var candles = await ImportCandlesAsync();

            var result = candles.Func((c, i, ctx) =>
            {
                var ema = ctx.Get<ExponentialMovingAverage>(30);
                return ema[i].Tick;
            })[candles.Count() - 1];

            var emaResult = candles.Ema(30)[candles.Count() - 1];

            Assert.AreEqual(result.Tick, emaResult.Tick);
		}

		[TestMethod]
		public async Task TestSimpleOperationAsync()
		{
			var candles = await ImportCandlesAsync();

			AFunc aFunc = (c, i, _) => c[i].Close;
			var a = aFunc.AsAnalyzable(candles);
			var aResult = a.Sma(30, candles.Count() - 1);

			var results = candles.Sma(30);
			var result = results[candles.Count() - 1];
			Assert.AreEqual(aResult.Tick.Value, result.Tick.Value);
		}
    }
}
