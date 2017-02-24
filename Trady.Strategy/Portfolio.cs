using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trady.Analysis;
using Trady.Core;
using Trady.Core.Helper;
using Trady.Strategy.Rule;

namespace Trady.Strategy
{
    public class Portfolio
    {
        private IDictionary<Equity, int> _equityPairs;
        private IRule<IndexCandle> _buyRule;
        private IRule<IndexCandle> _sellRule;

        public event BuyHandler OnBought;

        public delegate void BuyHandler(int quantity, string symbol, decimal currentPrice, DateTime currentDate, decimal balance);

        public event SellHandler OnSold;

        public delegate void SellHandler(int quantity, string symbol, decimal currentPrice, DateTime currentDate, decimal balance, decimal plRatio);

        #region Builder

        public Portfolio() : this(null, null, null)
        {
        }

        private Portfolio(IDictionary<Equity, int> equityPairs, IRule<IndexCandle> buyRule, IRule<IndexCandle> sellRule)
        {
            _equityPairs = equityPairs ?? new Dictionary<Equity, int>();
            _buyRule = buyRule;
            _sellRule = sellRule;
        }

        public Portfolio Add(Equity equity, int portion = 1)
        {
            if (_equityPairs.TryGetValue(equity, out int equityPortion))
                _equityPairs[equity] = equityPortion + portion;
            else
                _equityPairs.Add(equity, portion);
            return new Portfolio(_equityPairs, _buyRule, _sellRule);
        }

        public Portfolio Buy(IRule<IndexCandle> rule)
            => new Portfolio(_equityPairs, (_buyRule?.Or(rule)) ?? rule, _sellRule);

        public Portfolio Sell(IRule<IndexCandle> rule)
            => new Portfolio(_equityPairs, _buyRule, (_sellRule?.Or(rule)) ?? rule);

        #endregion Builder

        public async Task<PortfolioResult> RunBacktestAsync(decimal principal, decimal premium = 1.0m, DateTime? startTime = null, DateTime? endTime = null)
        {
            return await Task.Factory.StartNew(() =>
            {
                if (_equityPairs == null || !_equityPairs.Any())
                    throw new ArgumentException("You should have at least one equity for calculation");

                // Initialization: transaction history, period instance, fund map, asset map
                var transactions = new List<(Equity equity, DateTime transactionDatetime, decimal amount)>();
                var pairs = UnifyEquitiesPeriod(_equityPairs);
                var periodInstance = pairs.First().Key.Period.CreateInstance();
                var balanceMap = pairs.ToDictionary(kvp => kvp.Key, kvp => principal * Convert.ToDecimal(kvp.Value * 1.0 / pairs.Sum(p => p.Value)));
                var assetMap = new Dictionary<Equity, (IndexCandle candle, int quantity)>();

                // Get the earliest start time among the equities, use it as the start time for looping. Ignore the very first record if the time is not a timestamp
                DateTime dateTimeCursor = startTime ?? pairs.Min(kvp => kvp.Key.Min(c => c.DateTime));
                if (!periodInstance.IsTimestamp(dateTimeCursor))
                    dateTimeCursor = periodInstance.NextTimestamp(dateTimeCursor);

                // Get the latest end time among the equities, use it as the end time for looping
                DateTime dateTimeEnd = endTime ?? pairs.Max(kvp => kvp.Key.Max(c => c.DateTime));

                // Loop when there is next timestamp before the end time, since we transact on next timestamp when the previous day fulfill the trade requirement
                while (periodInstance.NextTimestamp(dateTimeCursor) < dateTimeEnd)
                {
                    var nextDateTimeCursor = periodInstance.NextTimestamp(dateTimeCursor);
                    foreach (var pair in pairs)
                    {
                        // For each pair, skip if the current timestamp does not exist / the next timestamp does not exist, reason is the same as stated above
                        var candleIndex = pair.Key.ToList().FindIndexOrDefault(c => c.DateTime.Equals(dateTimeCursor));
                        var nextCandleIndex = pair.Key.ToList().FindIndexOrDefault(c => c.DateTime >= nextDateTimeCursor);
                        if (candleIndex == null || nextCandleIndex == null)
                            continue;

                        var candle = new IndexCandle(pair.Key, candleIndex.Value);
                        var nextCandle = new IndexCandle(pair.Key, nextCandleIndex.Value);
                        bool isBought = assetMap.TryGetValue(pair.Key, out var bought);

                        // Transact if current timestamp fulfill the trade requirement
                        if (!isBought && _buyRule.IsValid(candle))
                            BuyEquity(premium, transactions, balanceMap, assetMap, pair, nextCandle);
                        else if (isBought && _sellRule.IsValid(candle))
                            SellEquity(premium, transactions, balanceMap, assetMap, pair, nextCandle, bought);
                    }

                    // Move cursor to next timestamp
                    dateTimeCursor = periodInstance.NextTimestamp(dateTimeCursor);
                }
                return new PortfolioResult(principal, premium, transactions);
            }
            );
        }

        private void SellEquity(decimal premium, List<(Equity equity, DateTime transactionDatetime, decimal amount)> transactions, Dictionary<Equity, decimal> fundMap, Dictionary<Equity, (IndexCandle candle, int quantity)> assetMap, KeyValuePair<Equity, int> pair, IndexCandle nextCandle, (IndexCandle candle, int quantity) bought)
        {
            decimal plRatio = 100 * (nextCandle.Open - bought.candle.Open) / bought.candle.Open;
            decimal moneyIn = nextCandle.Open * bought.quantity - premium;

            fundMap[pair.Key] += moneyIn;
            assetMap.Remove(pair.Key);

            transactions.Add((pair.Key, nextCandle.DateTime, moneyIn));
            OnSold?.Invoke(bought.quantity, pair.Key.Name, nextCandle.Open, nextCandle.DateTime, fundMap[pair.Key], plRatio);
        }

        private void BuyEquity(decimal premium, List<(Equity equity, DateTime transactionDatetime, decimal amount)> transactions, Dictionary<Equity, decimal> fundMap, Dictionary<Equity, (IndexCandle candle, int quantity)> assetMap, KeyValuePair<Equity, int> pair, IndexCandle nextCandle)
        {
            int quantity = Convert.ToInt32(Math.Floor((fundMap[pair.Key] - premium) / nextCandle.Open));
            decimal moneyOut = nextCandle.Open * quantity + premium;

            fundMap[pair.Key] -= moneyOut;
            assetMap.Add(pair.Key, (nextCandle, quantity));

            transactions.Add((pair.Key, nextCandle.DateTime, -moneyOut));
            OnBought?.Invoke(quantity, pair.Key.Name, nextCandle.Open, nextCandle.DateTime, fundMap[pair.Key]);
        }

        private static IDictionary<Equity, int> UnifyEquitiesPeriod(IDictionary<Equity, int> equityPairs)
        {
            var output = new Dictionary<Equity, int>();
            var defaultPeriod = equityPairs.Keys.First().Period;
            foreach (var pair in equityPairs)
            {
                bool isPeriodEquals = pair.Key.Period.Equals(defaultPeriod);
                output.Add(isPeriodEquals ? pair.Key : pair.Key.Transform(defaultPeriod), pair.Value);
            }
            return output;
        }
    }
}