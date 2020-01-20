using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Core.Infrastructure;
using Trady.Core.Period;

namespace Trady.Core
{
    public static class TradeExtensions
    {
        public static IReadOnlyList<IOhlcv> ToCandles<TTargetPeriod>(this IEnumerable<ITickTrade> trades) where TTargetPeriod:IPeriod
        {
            var offset = trades.Any() ? trades.First().DateTime.Offset.Hours : 0;
            var tradesAggregated = trades.OrderBy(t => t.DateTime).ToList().GroupBy(date => new
            {
                date.DateTime.Year,
                date.DateTime.Month,
                date.DateTime.Day,
                date.DateTime.Hour,
                date.DateTime.Minute
            }).Select(c=>
                    new Candle(new DateTimeOffset(new DateTime(c.Key.Year,c.Key.Month,c.Key.Day,c.Key.Hour,c.Key.Minute,0),TimeSpan.FromHours(offset)),
                    c.First().Price,
                    c.Max(p=>p.Price),
                    c.Min(p=>p.Price),
                    c.Last().Price,
                    c.Sum(v=>v.Volume)));
            if(typeof(TTargetPeriod) is PerMinute)
            {
                return tradesAggregated.ToList();
            }
            else
            {
                return tradesAggregated.Transform<PerMinute, TTargetPeriod>();
            }
        }
    }
}
