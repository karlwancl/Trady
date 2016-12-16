using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trady.Core;
using Trady.Core.Helper;
using Trady.Strategy.Helper;
using Trady.Strategy.Rule;

namespace Trady.Strategy
{
    public class Portfolio
    {
        private IDictionary<Equity, int> _pairs;
        private IRule<ComputableCandle> _buyRule;
        private IRule<ComputableCandle> _sellRule;

        public Portfolio(IDictionary<Equity, int> equityPairs, IRule<ComputableCandle> buyRule, IRule<ComputableCandle> sellRule)
        {
            _pairs = equityPairs;
            _buyRule = buyRule;
            _sellRule = sellRule;
        }

        public async Task<PortfolioResult> RunAsync(decimal principal, decimal premium = 1.0m, DateTime? startTime = null, DateTime? endTime = null)
        {
            return await Task.Factory.StartNew(() =>
            {
                if (_pairs == null || !_pairs.Any())
                    throw new ArgumentException("You should have at least one equity for calculation");

                var transactions = new List<(Equity equity, DateTime transactionDatetime, decimal amount)>();

                var _processedPairs = UnifyEquitiesPeriod(_pairs);
                var period = _processedPairs.First().Key.Period.CreateInstance();

                DateTime dateTimeCursor = startTime ?? _processedPairs.Min(kvp => kvp.Key.Min(c => c.DateTime));
                if (!period.IsTimestamp(dateTimeCursor))
                    dateTimeCursor = period.NextTimestamp(dateTimeCursor);

                DateTime dateTimeEnd = endTime ?? _processedPairs.Max(kvp => kvp.Key.Max(c => c.DateTime));

                var fundMap = _processedPairs.ToDictionary(kvp => kvp.Key, kvp => principal * Convert.ToDecimal(kvp.Value * 1.0 / _processedPairs.Sum(p => p.Value)));
                var assetMap = new Dictionary<Equity, (ComputableCandle candle, int quantity)>();

                while (period.NextTimestamp(dateTimeCursor) < dateTimeEnd)
                {
                    var nextDateTimeCursor = period.NextTimestamp(dateTimeCursor);
                    foreach (var pair in _processedPairs)
                    {
                        var candleIndex = pair.Key.FindFirstIndexOrDefault(c => c.DateTime.Equals(dateTimeCursor));
                        var nextCandleIndex = pair.Key.FindFirstIndexOrDefault(c => c.DateTime >= nextDateTimeCursor);
                        if (candleIndex == null || nextCandleIndex == null)
                            continue;

                        var candle = pair.Key.GetComputableCandleAt(candleIndex.Value);
                        var nextCandle = pair.Key.GetComputableCandleAt(nextCandleIndex.Value);
                        bool isBought = assetMap.TryGetValue(pair.Key, out var bought);

                        if (!isBought && _buyRule.IsValid(candle))
                        {
                            int quantity = Convert.ToInt32(Math.Floor((fundMap[pair.Key] - premium) / nextCandle.Open));
                            decimal moneyOut = nextCandle.Open * quantity + premium;

                            fundMap[pair.Key] -= moneyOut;
                            assetMap.Add(pair.Key, (nextCandle, quantity));

                            transactions.Add((pair.Key, nextCandle.DateTime, -moneyOut));
                            Console.WriteLine("Buy {0} units of {1} @ {2:0.##} on {3}, Fund: $ {4}", quantity, pair.Key.Name, nextCandle.Open, nextCandle.DateTime, fundMap[pair.Key]);
                        }
                        else if (isBought && _sellRule.IsValid(candle))
                        {
                            decimal pl = 100 * (nextCandle.Open - bought.candle.Open) / bought.candle.Open;
                            decimal moneyIn = nextCandle.Open * bought.quantity - premium;

                            fundMap[pair.Key] += moneyIn;
                            assetMap.Remove(pair.Key);

                            transactions.Add((pair.Key, nextCandle.DateTime, moneyIn));
                            Console.WriteLine("Sell {0} units of {1} @ {2:0.##} on {3}, P/L: {4:0.##}%, Fund: $ {5}", bought.quantity, pair.Key.Name, nextCandle.Open, nextCandle.DateTime, pl, fundMap[pair.Key]);
                            Console.WriteLine();
                        }
                    }

                    dateTimeCursor = period.NextTimestamp(dateTimeCursor);
                }
                return new PortfolioResult(principal, premium, transactions);
            }
            );
        }

        private static IDictionary<Equity, int> UnifyEquitiesPeriod(IDictionary<Equity, int> equityPairs)
        {
            var output = new Dictionary<Equity, int>();
            foreach (var pair in equityPairs)
            {
                if (!pair.Key.Period.Equals(equityPairs.Keys.First().Period))
                {
                    try
                    {
                        // Try upcast the period for calculation
                        output.Add(pair.Key.Transform(equityPairs.Keys.First().Period), pair.Value);
                    }
                    catch (Exception)
                    {
                        throw new Exception($"Upcast period fails for equity: {pair.Key.Name}, the process is terminated");
                    }
                }
                else
                    output.Add(pair.Key, pair.Value);
            }
            return output;
        }
    }
}
