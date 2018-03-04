using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Analysis;
using Trady.Core.Infrastructure;
using Trady.Importer.Csv;

namespace Trady.Test
{
    [TestClass]
    public class CustomIndicatorTest
    {
        private async Task<IEnumerable<IOhlcv>> ImportIOhlcvDatasAsync()
        {
            var csvImporter = new CsvImporter("fb.csv", CultureInfo.GetCultureInfo("en-US"));
            return await csvImporter.ImportAsync("fb");
        }

        [TestMethod]
        public async Task ASimpleTest()
        {
            var candles = await ImportIOhlcvDatasAsync();

            var customIndicator = new ClosePricePercentageChangeSinceMondayOpen(candles);

            Assert.AreEqual(candles.ElementAt(10).DateTime.Date, new DateTime(2012, 6, 4));
            // 4th June 2012 is Monday. Week opens at 27.2
            Assert.AreEqual(candles.ElementAt(10).DateTime.DayOfWeek, DayOfWeek.Monday);

            // That Monday FB closed at: 26.9.
            Assert.IsTrue(customIndicator[10].Tick.Value.IsApproximatelyEquals(-1.10294m));
            // That wednesday FB closed at: 26.81.
            Assert.IsTrue(customIndicator[12].Tick.Value.IsApproximatelyEquals(-1.43382m));
            // That Friday FB closed at: 27.1.
            Assert.IsTrue(customIndicator[14].Tick.Value.IsApproximatelyEquals(-0.36764m));
        }

        public class ClosePricePercentageChangeSinceMondayOpen : AnalyzableBase<IOhlcv, IOhlcv, decimal?, AnalyzableTick<decimal?>>
        {
            public ClosePricePercentageChangeSinceMondayOpen(IEnumerable<IOhlcv> inputs)
                : base(inputs, i => i)
            {
            }

            private DateTime GetMondayFor(DateTimeOffset aDate)
            {
                int currentDay = (int)aDate.DayOfWeek;
                if (currentDay == 0)
                {
                    currentDay = 7;
                }

                int daysToSubstract = currentDay - 1;
                return aDate.DateTime.AddDays(-daysToSubstract);
            }

            protected override decimal? ComputeByIndexImpl(IReadOnlyList<IOhlcv> mappedInputs, int index)
            {
                var currentIOhlcvData = mappedInputs[index];
                var mondayOfThatWeek = GetMondayFor(currentIOhlcvData.DateTime);
                var candleForThatMonday = mappedInputs.FirstOrDefault(c => c.DateTime.Date.Equals(mondayOfThatWeek));

                if (candleForThatMonday == null)
                {
                    return null;
                }

                var openValue = candleForThatMonday.Open;

                return (currentIOhlcvData.Close - openValue) / openValue * 100;
            }
        }
    }
}
