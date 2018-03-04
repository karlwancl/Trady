using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trady.Analysis;
using Trady.Analysis.Extension;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;
using Trady.Importer;
using Trady.Analysis.Backtest;
using Trady.Importer.Csv;

using AFunc = System.Func<System.Collections.Generic.IReadOnlyList<Trady.Core.Infrastructure.IOhlcv>, int, System.Collections.Generic.IReadOnlyList<decimal>, Trady.Core.Infrastructure.IAnalyzeContext<Trady.Core.Infrastructure.IOhlcv>, decimal?>;

namespace Trady.Test
{
    [TestClass]
    public class FuncTest
    {
		public async Task<IEnumerable<IOhlcv>> ImportIOhlcvDatasAsync()
		{
			var csvImporter = new CsvImporter("fb.csv", CultureInfo.GetCultureInfo("en-US"));
			return await csvImporter.ImportAsync("fb");
		}

        [TestMethod]
        public async Task TestGetRuleByRuleCreateAsync()
        {
            var candles = await ImportIOhlcvDatasAsync();

			RuleRegistry.Register("isabovesmax", (ic, p) => ic.Get<SimpleMovingAverage>(p[0])[ic.Index].Tick.IsTrue(t => t < ic.Close));
			RuleRegistry.Register("isbelowsmax", (ic, p) => ic.Get<SimpleMovingAverage>(p[0])[ic.Index].Tick.IsTrue(t => t > ic.Close));

            var buyRule = Rule.Create(ic => ic.Eval("isabovesmax", 30));
            var sellRule = Rule.Create(ic => ic.Eval("isbelowsmax", 30));

            var runner = new Builder()
                .Add(candles)
                .Buy(buyRule)
                .Sell(sellRule)
                .Build();

            var result = runner.Run(10000);
            Assert.IsTrue(true);
		}

        [TestMethod]
        public async Task TestGetRuleAsync()
        {
			var candles = await ImportIOhlcvDatasAsync();

            RuleRegistry.Register("isbelowsma30_2", "ic.Get<SimpleMovingAverage>(30)[ic.Index].Tick.IsTrue(t => t > ic.Close)");
            RuleRegistry.Register("isbelowsma30", (ic, p) => ic.Get<SimpleMovingAverage>(p[0])[ic.Index].Tick.IsTrue(t => t > ic.Close));

            using (var ctx = new AnalyzeContext(candles))
            {
                var result = new SimpleRuleExecutor(ctx, ctx.GetRule("isbelowsma30_2")).Execute();
                var result2 = new SimpleRuleExecutor(ctx, ctx.GetRule("isbelowsma30", 30)).Execute();
                var result3 = new SimpleRuleExecutor(ctx, ic => ic.IsBelowSma(30)).Execute();

                Assert.AreEqual(result.Count(), result3.Count());
                Assert.AreEqual(result2.Count(), result3.Count());
            }
        }

        [TestMethod]
        public async Task TestGetFuncFromContextAsync()
        {
			var candles = await ImportIOhlcvDatasAsync();

            FuncRegistry.Register("msma", "var sma = ctx.Get<SimpleMovingAverage>(10); return sma[i].Tick;");
            FuncRegistry.Register("msma2", (c, i, p, ctx) => ctx.Get<SimpleMovingAverage>(p[0])[i].Tick);

            var result = candles.Func("msma")[candles.Count() - 1];
            var result2 = candles.Func("msma2", 10)[candles.Count() - 1];
            var actual = candles.Sma(10)[candles.Count() - 1];

            Assert.AreEqual(result.Tick, actual.Tick);
            Assert.AreEqual(result2.Tick, actual.Tick);
		}

        [TestMethod]
        public async Task TestGetFromContextAsync()
        {
			var candles = await ImportIOhlcvDatasAsync();
            var result = candles.Func((c, i, _, ctx) => ctx.Get<ExponentialMovingAverage>(30)[i].Tick)[candles.Count() - 1];
            var emaResult = candles.Ema(30)[candles.Count() - 1];

            Assert.AreEqual(result.Tick, emaResult.Tick);
		}

		[TestMethod]
		public async Task TestSimpleOperationAsync()
		{
			var candles = await ImportIOhlcvDatasAsync();

			AFunc aFunc = (c, i, _, __) => c[i].Close;
			var a = aFunc.AsAnalyzable(candles);
			var aResult = a.Sma(30, candles.Count() - 1);

			var results = candles.Sma(30);
			var result = results[candles.Count() - 1];

			Assert.AreEqual(aResult.Tick.Value, result.Tick.Value);
		}
    }
}
