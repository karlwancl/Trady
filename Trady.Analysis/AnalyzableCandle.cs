using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern;
using Trady.Analysis.Pattern.Indicator;
using Trady.Core;

namespace Trady.Analysis
{
    public partial class AnalyzableCandle : Candle, IAnalyzable
    {
        private Equity _equity;
        private int _index;

        public AnalyzableCandle(Equity equity, int index)
            : base(equity[index].DateTime, equity[index].Open, equity[index].High, equity[index].Low, equity[index].Close, equity[index].Volume)
        {
            _equity = equity;
            _index = index;
        }

        public Equity Equity => _equity;

        public int Index => _index;

        public AnalyzableCandle Prev => _index - 1 >= 0 ? new AnalyzableCandle(_equity, _index - 1) : null;

        public AnalyzableCandle Next => _index + 1 < _equity.Count ? new AnalyzableCandle(_equity, _index + 1) : null;

        public decimal? PriceChange()
            => _equity.GetOrCreateAnalytic<ClosePriceChange>().ComputeByIndex(_index).Change;

        public decimal? PricePercentageChange()
            => _equity.GetOrCreateAnalytic<ClosePricePercentageChange>().ComputeByIndex(_index).PercentageChange;

        public bool IsEquityBullish()
            => _equity.GetOrCreateAnalytic<ClosePriceChangeTrend>().ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsEquityBearish()
            => _equity.GetOrCreateAnalytic<ClosePriceChangeTrend>().ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsAccumDistBullish()
            => _equity.GetOrCreateAnalytic<AccumulationDistributionLineTrend>().ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsAccumDistBearish()
            => _equity.GetOrCreateAnalytic<AccumulationDistributionLineTrend>().ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsObvBullish()
            => _equity.GetOrCreateAnalytic<OnBalanceVolumeTrend>().ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsObvBearish()
            => _equity.GetOrCreateAnalytic<OnBalanceVolumeTrend>().ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsInBbRange(int periodCount, int sdCount)
            => _equity.GetOrCreateAnalytic<BollingerBandsInRange>(periodCount, sdCount).ComputeByIndex(_index).State == Overboundary.InRange;

        public bool IsHighest(int periodCount)
        {
            var isMatched = _equity.GetOrCreateAnalytic<IsHighestPrice>(periodCount).ComputeByIndex(_index).IsMatched;
            return isMatched.HasValue && isMatched.Value;
        }

        public bool IsLowest(int periodCount)
        {
            var isMatched = _equity.GetOrCreateAnalytic<IsLowestPrice>(periodCount).ComputeByIndex(_index).IsMatched;
            return isMatched.HasValue && isMatched.Value;
        }

        public bool IsRsiOverbought(int periodCount)
            => _equity.GetOrCreateAnalytic<RelativeStrengthIndexOvertrade>(periodCount).ComputeByIndex(_index).State == SevereOvertrade.Overbought;

        public bool IsRsiOversold(int periodCount)
            => _equity.GetOrCreateAnalytic<RelativeStrengthIndexOvertrade>(periodCount).ComputeByIndex(_index).State == SevereOvertrade.Oversold;

        public bool IsFastStoOverbought(int periodCount, int smaPeriodCount)
            => _equity.GetOrCreateAnalytic<StochasticsOvertrade.Fast>(periodCount, smaPeriodCount).ComputeByIndex(_index).State == Overtrade.Overbought;

        public bool IsFullStoOverbought(int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => _equity.GetOrCreateAnalytic<StochasticsOvertrade.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(_index).State == Overtrade.Overbought;

        public bool IsSlowStoOverbought(int periodCount, int smaPeriodCountD)
            => _equity.GetOrCreateAnalytic<StochasticsOvertrade.Slow>(periodCount, smaPeriodCountD).ComputeByIndex(_index).State == Overtrade.Overbought;

        public bool IsFastStoOversold(int periodCount, int smaPeriodCount)
            => _equity.GetOrCreateAnalytic<StochasticsOvertrade.Fast>(periodCount, smaPeriodCount).ComputeByIndex(_index).State == Overtrade.Oversold;

        public bool IsFullStoOversold(int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => _equity.GetOrCreateAnalytic<StochasticsOvertrade.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(_index).State == Overtrade.Oversold;

        public bool IsSlowStoOversold(int periodCount, int smaPeriodCountD)
            => _equity.GetOrCreateAnalytic<StochasticsOvertrade.Slow>(periodCount, smaPeriodCountD).ComputeByIndex(_index).State == Overtrade.Oversold;

        public bool IsAboveSma(int periodCount)
        {
            var isMatched = _equity.GetOrCreateAnalytic<IsAboveSimpleMovingAverage>(periodCount).ComputeByIndex(_index).IsMatched;
            return isMatched.HasValue && isMatched.Value;
        }

        public bool IsAboveEma(int periodCount)
        {
            var isMatched = _equity.GetOrCreateAnalytic<IsAboveExponentialMovingAverage>(periodCount).ComputeByIndex(_index).IsMatched;
            return isMatched.HasValue && isMatched.Value;
        }

        public bool IsSmaBullish(int periodCount)
            => _equity.GetOrCreateAnalytic<SimpleMovingAverageTrend>(periodCount).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsSmaBearish(int periodCount)
            => _equity.GetOrCreateAnalytic<SimpleMovingAverageTrend>(periodCount).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsSmaOscBullish(int periodCount1, int periodCount2)
            => _equity.GetOrCreateAnalytic<SimpleMovingAverageCrossover>(periodCount1, periodCount2).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsSmaOscBearish(int periodCount1, int periodCount2)
            => _equity.GetOrCreateAnalytic<SimpleMovingAverageCrossover>(periodCount1, periodCount2).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsEmaBullish(int periodCount)
            => _equity.GetOrCreateAnalytic<ExponentialMovingAverageTrend>(periodCount).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsEmaBearish(int periodCount)
            => _equity.GetOrCreateAnalytic<ExponentialMovingAverageTrend>(periodCount).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsEmaOscBullish(int periodCount1, int periodCount2)
            => _equity.GetOrCreateAnalytic<ExponentialMovingAverageCrossover>(periodCount1, periodCount2).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsEmaOscBearish(int periodCount1, int periodCount2)
            => _equity.GetOrCreateAnalytic<ExponentialMovingAverageCrossover>(periodCount1, periodCount2).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsMacdOscBullish(int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => _equity.GetOrCreateAnalytic<MovingAverageConvergenceDivergenceCrossover>(emaPeriodCount1, emaPeriodCount2, demPeriodCount).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsMacdOscBearish(int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => _equity.GetOrCreateAnalytic<MovingAverageConvergenceDivergenceCrossover>(emaPeriodCount1, emaPeriodCount2, demPeriodCount).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsFastStoOscBullish(int periodCount, int smaPeriodCount)
            => _equity.GetOrCreateAnalytic<StochasticsCrossover.Fast>(periodCount, smaPeriodCount).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsFastStoOscBearish(int periodCount, int smaPeriodCount)
            => _equity.GetOrCreateAnalytic<StochasticsCrossover.Fast>(periodCount, smaPeriodCount).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsFullStoOscBullish(int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => _equity.GetOrCreateAnalytic<StochasticsCrossover.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsFullStoOscBearish(int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => _equity.GetOrCreateAnalytic<StochasticsCrossover.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsSlowStoOscBullish(int periodCount, int smaPeriodCountD)
            => _equity.GetOrCreateAnalytic<StochasticsCrossover.Slow>(periodCount, smaPeriodCountD).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsSlowStoOscBearish(int periodCount, int smaPeriodCountD)
            => _equity.GetOrCreateAnalytic<StochasticsCrossover.Slow>(periodCount, smaPeriodCountD).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsSmaBullishCross(int periodCount1, int periodCount2)
        {
            var result = _equity.GetOrCreateAnalytic<SimpleMovingAverageCrossover>(periodCount1, periodCount2).ComputeByIndex(_index);
            return result.IsMatched.HasValue && result.IsMatched.Value && result.State == Trend.Bullish;
        }

        public bool IsSmaBearishCross(int periodCount1, int periodCount2)
        {
            var result = _equity.GetOrCreateAnalytic<SimpleMovingAverageCrossover>(periodCount1, periodCount2).ComputeByIndex(_index);
            return result.IsMatched.HasValue && result.IsMatched.Value && result.State == Trend.Bearish;
        }

        public bool IsEmaBullishCross(int periodCount1, int periodCount2)
        {
            var result = _equity.GetOrCreateAnalytic<ExponentialMovingAverageCrossover>(periodCount1, periodCount2).ComputeByIndex(_index);
            return result.IsMatched.HasValue && result.IsMatched.Value && result.State == Trend.Bullish;
        }

        public bool IsEmaBearishCross(int periodCount1, int periodCount2)
        {
            var result = _equity.GetOrCreateAnalytic<ExponentialMovingAverageCrossover>(periodCount1, periodCount2).ComputeByIndex(_index);
            return result.IsMatched.HasValue && result.IsMatched.Value && result.State == Trend.Bearish;
        }

        public bool IsMacdBullishCross(int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
        {
            var result = _equity.GetOrCreateAnalytic<MovingAverageConvergenceDivergenceCrossover>(emaPeriodCount1, emaPeriodCount2, demPeriodCount).ComputeByIndex(_index);
            return result.IsMatched.HasValue && result.IsMatched.Value && result.State == Trend.Bullish;
        }

        public bool IsMacdBearishCross(int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
        {
            var result = _equity.GetOrCreateAnalytic<MovingAverageConvergenceDivergenceCrossover>(emaPeriodCount1, emaPeriodCount2, demPeriodCount).ComputeByIndex(_index);
            return result.IsMatched.HasValue && result.IsMatched.Value && result.State == Trend.Bearish;
        }

        public bool IsFastStoBullishCross(int periodCount, int smaPeriodCount)
        {
            var result = _equity.GetOrCreateAnalytic<StochasticsCrossover.Fast>(periodCount, smaPeriodCount).ComputeByIndex(_index);
            return result.IsMatched.HasValue && result.IsMatched.Value && result.State == Trend.Bullish;
        }

        public bool IsFastStoBearishCross(int periodCount, int smaPeriodCount)
        {
            var result = _equity.GetOrCreateAnalytic<StochasticsCrossover.Fast>(periodCount, smaPeriodCount).ComputeByIndex(_index);
            return result.IsMatched.HasValue && result.IsMatched.Value && result.State == Trend.Bearish;
        }

        public bool IsFullStoBullishCross(int periodCount, int smaPeriodCountK, int smaPeriodCountD)
        {
            var result = _equity.GetOrCreateAnalytic<StochasticsCrossover.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(_index);
            return result.IsMatched.HasValue && result.IsMatched.Value && result.State == Trend.Bullish;
        }

        public bool IsFullStoBearishCross(int periodCount, int smaPeriodCountK, int smaPeriodCountD)
        {
            var result = _equity.GetOrCreateAnalytic<StochasticsCrossover.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(_index);
            return result.IsMatched.HasValue && result.IsMatched.Value && result.State == Trend.Bearish;
        }

        public bool IsSlowStoBullishCross(int periodCount, int smaPeriodCountD)
        {
            var result = _equity.GetOrCreateAnalytic<StochasticsCrossover.Slow>(periodCount, smaPeriodCountD).ComputeByIndex(_index);
            return result.IsMatched.HasValue && result.IsMatched.Value && result.State == Trend.Bullish;
        }

        public bool IsSlowStoBearishCross(int periodCount, int smaPeriodCountD)
        {
            var result = _equity.GetOrCreateAnalytic<StochasticsCrossover.Slow>(periodCount, smaPeriodCountD).ComputeByIndex(_index);
            return result.IsMatched.HasValue && result.IsMatched.Value && result.State == Trend.Bearish;
        }
    }
}