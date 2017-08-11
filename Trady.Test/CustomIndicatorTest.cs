using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Test
{
    [TestClass]
    public class CustomIndicatorTest
    {
        async Task<IList<Candle>> ImportCandlesAsync()
        {
            var csvImporter = new Importer.CsvImporter("fb.csv", new CultureInfo("en-US"));
            return await csvImporter.ImportAsync("fb");
        }


        [TestMethod]
        public async Task ASimpleTest()
        {
            var candles = await ImportCandlesAsync();

            var customIndicator = new ClosePricePercentageChangeSinceMondayOpen(candles);

            Assert.AreEqual(candles[10].DateTime.Date, new DateTime(2012,6,4));
            // 4th June 2012 is Monday. Week opens at 27.2
            Assert.AreEqual(candles[10].DateTime.DayOfWeek, DayOfWeek.Monday);

            // That Monday FB closed at: 26.9. 
            Assert.IsTrue(customIndicator[10].Value.IsApproximatelyEquals(-1.10294m));
            // That wednesday FB closed at: 26.81. 
            Assert.IsTrue(customIndicator[12].Value.IsApproximatelyEquals(-1.43382m));
            // That Friday FB closed at: 27.1. 
            Assert.IsTrue(customIndicator[14].Value.IsApproximatelyEquals(-0.36764m));
        }

        public class ClosePricePercentageChangeSinceMondayOpen : AnalyzableBase<Candle, decimal?>
        {
            public ClosePricePercentageChangeSinceMondayOpen(IEnumerable<Candle> inputs) : base(inputs)
            {
            }

            private DateTime GetMondayFor(DateTime aDate)
            {
                int currentDay = (int)aDate.DayOfWeek;
                if (currentDay == 0)
                    currentDay = 7;

                int daysToSubstract = currentDay - 1;
                return aDate.AddDays(-daysToSubstract);
            }

            protected override decimal? ComputeByIndexImpl(IEnumerable<Candle> mappedInputs, int index)
            {
                var currentCandle = mappedInputs.ElementAt(index);
                var mondayOfThatWeek = GetMondayFor(currentCandle.DateTime);
                var candleForThatMonday = Inputs.FirstOrDefault(c => c.DateTime.Date.Equals(mondayOfThatWeek));

                if (candleForThatMonday == null)
                    return null;

                var openValue = candleForThatMonday.Open;

                return (currentCandle.Close - openValue) / openValue * 100;
            }
        }

    }
}
