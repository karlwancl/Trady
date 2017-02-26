using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Strategy
{
    public class PortfolioResult
    {
        private decimal _principal, _premium;
        private IDictionary<Equity, Dictionary<DateTime, decimal>> _equitiesTransactions;
        private Func<Dictionary<DateTime, decimal>, int, decimal> _change;

        internal PortfolioResult(decimal principal, decimal premium, IList<(Equity equity, DateTime transactionDateTime, decimal amount)> equitiesTransactions)
        {
            _principal = principal;
            _premium = premium;
            _equitiesTransactions = equitiesTransactions
                .GroupBy(t => t.equity)
                .ToDictionary(t => t.Key, t => t.OrderBy(t2 => t2.transactionDateTime).ToDictionary(t2 => t2.transactionDateTime, t2 => t2.amount));
            _change = (dict, i) => Convert.ToDecimal((Math.Abs(dict.ElementAt(i).Value) - Math.Abs(dict.ElementAt(i - 1).Value)) / Math.Abs(dict.ElementAt(i - 1).Value));
        }

        public IDictionary<Equity, Dictionary<DateTime, decimal>> EquityTransactions => _equitiesTransactions;

        public int TotalTransactionsCount
        {
            get
            {
                int sum = 0;
                foreach (var equityTransaction in _equitiesTransactions)
                {
                    sum += (equityTransaction.Value.Count % 2 == 0) ?
                        equityTransaction.Value.Count : equityTransaction.Value.Count - 1;
                }
                return sum;
            }
        }

        public decimal TotalProfitRate
        {
            get
            {
                decimal sum = 0;
                foreach (var equityTransaction in _equitiesTransactions)
                {
                    for (int i = 0; i < (equityTransaction.Value.Count - 1) / 2; i++)
                    {
                        int j = 2 * i + 1;
                        decimal change = _change(equityTransaction.Value, j);
                        if (change > 0)
                            sum += Math.Abs(change);
                    }
                }
                return sum;
            }
        }

        public decimal TotalLossRate
        {
            get
            {
                decimal sum = 0;
                foreach (var equityTransaction in _equitiesTransactions)
                {
                    for (int i = 0; i < (equityTransaction.Value.Count - 1) / 2; i++)
                    {
                        int j = 2 * i + 1;
                        decimal change = _change(equityTransaction.Value, j);
                        if (change < 0)
                            sum += Math.Abs(change);
                    }
                }
                return sum;
            }
        }

        public decimal ProfitLossRatio => TotalProfitRate / (TotalProfitRate + TotalLossRate);

        public decimal Principal => _principal;

        public decimal TotalPremium => _premium * TotalTransactionsCount;

        public decimal Total
        {
            get
            {
                decimal sum = 0;
                foreach (var equityTransaction in _equitiesTransactions)
                {
                    var index = (equityTransaction.Value.Count % 2 == 0) ?
                        equityTransaction.Value.Count - 1 : equityTransaction.Value.Count - 2;

                    sum += equityTransaction.Value.ElementAt(index).Value;
                }
                return sum;
            }
        }

        public decimal ProfitLoss => Total - Principal;
    }
}