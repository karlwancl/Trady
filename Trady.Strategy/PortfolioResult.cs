using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Core;

namespace Trady.Strategy
{
    public class PortfolioResult
    {
        private decimal _principal, _premium;
        private IDictionary<Equity, IDictionary<DateTime, decimal>> _transactions;

        public PortfolioResult(decimal principal, decimal premium, IDictionary<Equity, IDictionary<DateTime, decimal>> transactions)
        {
            _principal = principal;
            _premium = premium;
            _transactions = transactions.ToDictionary(
                kvp => kvp.Key, 
                kvp => (IDictionary<DateTime, decimal>)kvp.Value.OrderBy(v => v.Key).ToDictionary(ikvp => ikvp.Key, ikvp => ikvp.Value));
        }

        public IDictionary<Equity, IDictionary<DateTime, decimal>> Transaction => _transactions;

        public int TransactionCount => _transactions.SelectMany(t => t.Value).Count();

        public double ProfitRateSum
        {
            get
            {
                double sum = 0;
                foreach (var eq in _transactions)
                {
                    for (int i = 1; i < eq.Value.Count; i++)
                    {
                        var current = Math.Abs(eq.Value.ElementAt(i).Value);
                        var previous = Math.Abs(eq.Value.ElementAt(i - 1).Value);
                        var plPercent = Convert.ToDouble((current - previous) / previous);
                        if (plPercent > 0)
                            sum += Math.Abs(plPercent);
                    }
                }
                return sum;
            }
        }

        public double LossRateSum
        {
            get
            {
                double sum = 0;
                foreach (var eq in _transactions)
                {
                    for (int i = 1; i < eq.Value.Count; i++)
                    {
                        var current = Math.Abs(eq.Value.ElementAt(i).Value);
                        var previous = Math.Abs(eq.Value.ElementAt(i - 1).Value);
                        var plPercent = Convert.ToDouble((current - previous) / previous);
                        if (plPercent < 0)
                            sum += Math.Abs(plPercent);
                    }
                }
                return sum;
            }
        }

        public double ProfitLossRatio => ProfitRateSum / (ProfitRateSum + LossRateSum);

        public decimal Principal => _principal;

        public decimal TotalPremium => _premium * TransactionCount;

        public decimal Total
        {
            get
            {
                decimal sum = 0;
                foreach (var eq in _transactions)
                {
                    if (eq.Value.Count > 1)
                        sum += Math.Abs(eq.Value.Last(kvp => kvp.Value > 0).Value);
                }
                return sum;
            }
        }

        public decimal NetProfitLoss => Total - Principal;
    }
}
