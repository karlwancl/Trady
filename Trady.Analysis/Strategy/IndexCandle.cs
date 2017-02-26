using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern;
using Trady.Analysis.Pattern.Indicator;
using Trady.Core;

namespace Trady.Analysis.Strategy
{
    public class IndexCandle : Candle
    {
        private Equity _equity;
        private int _index;

        public IndexCandle(Equity equity, int index)
            : base(equity[index].DateTime, equity[index].Open, equity[index].High, equity[index].Low, equity[index].Close, equity[index].Volume)
        {
            _equity = equity;
            _index = index;
        }

        public Equity Equity => _equity;

        public int Index => _index;

        public IndexCandle Prev => _index - 1 >= 0 ? new IndexCandle(_equity, _index - 1) : null;

        public IndexCandle Next => _index + 1 < _equity.Count ? new IndexCandle(_equity, _index + 1) : null;

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
            => _equity.GetOrCreateAnalytic<IsHighestPrice>(periodCount).ComputeByIndex(_index).State == Match.IsMatched;

        public bool IsLowest(int periodCount)
            => _equity.GetOrCreateAnalytic<IsLowestPrice>(periodCount).ComputeByIndex(_index).State == Match.IsMatched;

        public bool IsRsiOverbought(int periodCount)
            => _equity.GetOrCreateAnalytic<RelativeStrengthIndexOvertrade>(periodCount).ComputeByIndex(_index).State == Overtrade.Overbought;

        public bool IsRsiOversold(int periodCount)
            => _equity.GetOrCreateAnalytic<RelativeStrengthIndexOvertrade>(periodCount).ComputeByIndex(_index).State == Overtrade.Oversold;

        public bool IsFastStoOverbought(int periodCount, int smaPeriodCount)
            => _equity.GetOrCreateAnalytic<StochasticsOvertrade.Fast>(periodCount, smaPeriodCount).ComputeByIndex(_index).State == Overtrade.SeverelyOverbought;

        public bool IsFullStoOverbought(int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => _equity.GetOrCreateAnalytic<StochasticsOvertrade.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(_index).State == Overtrade.SeverelyOverbought;

        public bool IsSlowStoOverbought(int periodCount, int smaPeriodCountD)
            => _equity.GetOrCreateAnalytic<StochasticsOvertrade.Slow>(periodCount, smaPeriodCountD).ComputeByIndex(_index).State == Overtrade.SeverelyOverbought;

        public bool IsFastStoOversold(int periodCount, int smaPeriodCount)
            => _equity.GetOrCreateAnalytic<StochasticsOvertrade.Fast>(periodCount, smaPeriodCount).ComputeByIndex(_index).State == Overtrade.SeverelyOversold;

        public bool IsFullStoOversold(int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => _equity.GetOrCreateAnalytic<StochasticsOvertrade.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(_index).State == Overtrade.SeverelyOversold;

        public bool IsSlowStoOversold(int periodCount, int smaPeriodCountD)
            => _equity.GetOrCreateAnalytic<StochasticsOvertrade.Slow>(periodCount, smaPeriodCountD).ComputeByIndex(_index).State == Overtrade.SeverelyOversold;

        public bool IsAboveSma(int periodCount)
            => _equity.GetOrCreateAnalytic<IsAboveSimpleMovingAverage>(periodCount).ComputeByIndex(_index).State == Match.IsMatched;

        public bool IsAboveEma(int periodCount)
            => _equity.GetOrCreateAnalytic<IsAboveExponentialMovingAverage>(periodCount).ComputeByIndex(_index).State == Match.IsMatched;

        public bool IsSmaBullish(int periodCount)
            => _equity.GetOrCreateAnalytic<SimpleMovingAverageTrend>(periodCount).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsSmaBearish(int periodCount)
            => _equity.GetOrCreateAnalytic<SimpleMovingAverageTrend>(periodCount).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsSmaOscBullish(int periodCount1, int periodCount2)
            => _equity.GetOrCreateAnalytic<SimpleMovingAverageOscillatorTrend>(periodCount1, periodCount2).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsSmaOscBearish(int periodCount1, int periodCount2)
            => _equity.GetOrCreateAnalytic<SimpleMovingAverageOscillatorTrend>(periodCount1, periodCount2).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsEmaBullish(int periodCount)
            => _equity.GetOrCreateAnalytic<ExponentialMovingAverageTrend>(periodCount).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsEmaBearish(int periodCount)
            => _equity.GetOrCreateAnalytic<ExponentialMovingAverageTrend>(periodCount).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsEmaOscBullish(int periodCount1, int periodCount2)
            => _equity.GetOrCreateAnalytic<ExponentialMovingAverageOscillatorTrend>(periodCount1, periodCount2).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsEmaOscBearish(int periodCount1, int periodCount2)
            => _equity.GetOrCreateAnalytic<ExponentialMovingAverageOscillatorTrend>(periodCount1, periodCount2).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsMacdOscBullish(int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => _equity.GetOrCreateAnalytic<MovingAverageConvergenceDivergenceOscillatorTrend>(emaPeriodCount1, emaPeriodCount2, demPeriodCount).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsMacdOscBearish(int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => _equity.GetOrCreateAnalytic<MovingAverageConvergenceDivergenceOscillatorTrend>(emaPeriodCount1, emaPeriodCount2, demPeriodCount).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsFastStoOscBullish(int periodCount, int smaPeriodCount)
            => _equity.GetOrCreateAnalytic<StochasticsOscillatorTrend.Fast>(periodCount, smaPeriodCount).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsFastStoOscBearish(int periodCount, int smaPeriodCount)
            => _equity.GetOrCreateAnalytic<StochasticsOscillatorTrend.Fast>(periodCount, smaPeriodCount).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsFullStoOscBullish(int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => _equity.GetOrCreateAnalytic<StochasticsOscillatorTrend.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsFullStoOscBearish(int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => _equity.GetOrCreateAnalytic<StochasticsOscillatorTrend.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsSlowStoOscBullish(int periodCount, int smaPeriodCountD)
            => _equity.GetOrCreateAnalytic<StochasticsOscillatorTrend.Slow>(periodCount, smaPeriodCountD).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsSlowStoOscBearish(int periodCount, int smaPeriodCountD)
            => _equity.GetOrCreateAnalytic<StochasticsOscillatorTrend.Slow>(periodCount, smaPeriodCountD).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsSmaBullishCross(int periodCount1, int periodCount2)
            => _equity.GetOrCreateAnalytic<SimpleMovingAverageCrossover>(periodCount1, periodCount2).ComputeByIndex(_index).State == Crossover.BullishCrossover;

        public bool IsSmaBearishCross(int periodCount1, int periodCount2)
            => _equity.GetOrCreateAnalytic<SimpleMovingAverageCrossover>(periodCount1, periodCount2).ComputeByIndex(_index).State == Crossover.BearishCrossover;

        public bool IsEmaBullishCross(int periodCount1, int periodCount2)
            => _equity.GetOrCreateAnalytic<ExponentialMovingAverageCrossover>(periodCount1, periodCount2).ComputeByIndex(_index).State == Crossover.BullishCrossover;

        public bool IsEmaBearishCross(int periodCount1, int periodCount2)
            => _equity.GetOrCreateAnalytic<ExponentialMovingAverageCrossover>(periodCount1, periodCount2).ComputeByIndex(_index).State == Crossover.BearishCrossover;

        public bool IsMacdBullishCross(int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => _equity.GetOrCreateAnalytic<MovingAverageConvergenceDivergenceCrossover>(emaPeriodCount1, emaPeriodCount2, demPeriodCount).ComputeByIndex(_index).State == Crossover.BullishCrossover;

        public bool IsMacdBearishCross(int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => _equity.GetOrCreateAnalytic<MovingAverageConvergenceDivergenceCrossover>(emaPeriodCount1, emaPeriodCount2, demPeriodCount).ComputeByIndex(_index).State == Crossover.BearishCrossover;

        public bool IsFastStoBullishCross(int periodCount, int smaPeriodCount)
            => _equity.GetOrCreateAnalytic<StochasticsCrossover.Fast>(periodCount, smaPeriodCount).ComputeByIndex(_index).State == Crossover.BullishCrossover;

        public bool IsFastStoBearishCross(int periodCount, int smaPeriodCount)
            => _equity.GetOrCreateAnalytic<StochasticsCrossover.Fast>(periodCount, smaPeriodCount).ComputeByIndex(_index).State == Crossover.BearishCrossover;

        public bool IsFullStoBullishCross(int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => _equity.GetOrCreateAnalytic<StochasticsCrossover.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(_index).State == Crossover.BullishCrossover;

        public bool IsFullStoBearishCross(int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => _equity.GetOrCreateAnalytic<StochasticsCrossover.Full>(periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(_index).State == Crossover.BearishCrossover;

        public bool IsSlowStoBullishCross(int periodCount, int smaPeriodCountD)
            => _equity.GetOrCreateAnalytic<StochasticsCrossover.Slow>(periodCount, smaPeriodCountD).ComputeByIndex(_index).State == Crossover.BullishCrossover;

        public bool IsSlowStoBearishCross(int periodCount, int smaPeriodCountD)
            => _equity.GetOrCreateAnalytic<StochasticsCrossover.Slow>(periodCount, smaPeriodCountD).ComputeByIndex(_index).State == Crossover.BearishCrossover;
    }
}