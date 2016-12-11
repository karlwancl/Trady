using System;
using System.Collections.Generic;
using System.Text;
using Trady.Analysis.Pattern.Indicator;
using Trady.Analysis.Pattern;
using Trady.Core;
using Trady.Analysis.Indicator;
using Trady.Strategy.Helper;

namespace Trady.Strategy
{
    public class ComputableCandle : Candle
    {
        private Equity _equity;
        private int _index;

        public ComputableCandle(Equity equity, int index) 
            : base(equity[index].DateTime, equity[index].Open, equity[index].High, equity[index].Low, equity[index].Close, equity[index].Volume)
        {
            _equity = equity;
            _index = index;
        }

        public Equity Equity => _equity;

        public int Index => _index;

        public ComputableCandle Next => _index + 1 < _equity.TickCount ? _equity.GetComputableCandleAt(_index + 1) : null;

        #region Computation

        public decimal PriceChange()
            => new ClosePriceChange(_equity).ComputeByIndex(_index).Change;

        public decimal PricePercentageChange()
            => new ClosePricePercentageChange(_equity).ComputeByIndex(_index).PercentageChange;

        #endregion  

        #region Patterns

        public bool IsEquityBullish()
            => new ClosePriceChangeTrend(_equity).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsEquityBearish()
            => new ClosePriceChangeTrend(_equity).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsAccumDistBullish()
            => new AccumulationDistributionLineTrend(_equity).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsAccumDistBearish()
            => new AccumulationDistributionLineTrend(_equity).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsObvBullish()
            => new OnBalanceVolumeTrend(_equity).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsObvBearish()
            => new OnBalanceVolumeTrend(_equity).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsInBbRange(int periodCount, int sdCount)
            => new BollingerBandsInRange(_equity, periodCount, sdCount).ComputeByIndex(_index).State == Overboundary.InRange;

        public bool IsHighest(int periodCount)
            => new IsHighestPrice(_equity, periodCount).ComputeByIndex(_index).IsMatched;

        public bool IsLowest(int periodCount)
            => new IsLowestPrice(_equity, periodCount).ComputeByIndex(_index).IsMatched;

        public bool IsRsiOverbought(int periodCount)
            => new RelativeStrengthIndexOvertrade(_equity, periodCount).ComputeByIndex(_index).State == SevereOvertrade.Overbought;

        public bool IsRsiOversold(int periodCount)
            => new RelativeStrengthIndexOvertrade(_equity, periodCount).ComputeByIndex(_index).State == SevereOvertrade.Oversold;

        public bool IsFastStoOverbought(int periodCount, int smaPeriodCount)
            => new StochasticsOvertrade.Fast(_equity, periodCount, smaPeriodCount).ComputeByIndex(_index).State == Overtrade.Overbought;

        public bool IsFullStoOverbought(int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => new StochasticsOvertrade.Full(_equity, periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(_index).State == Overtrade.Overbought;

        public bool IsSlowStoOverbought(int periodCount, int smaPeriodCountD)
            => new StochasticsOvertrade.Slow(_equity, periodCount, smaPeriodCountD).ComputeByIndex(_index).State == Overtrade.Overbought;

        public bool IsFastStoOversold(int periodCount, int smaPeriodCount)
            => new StochasticsOvertrade.Fast(_equity, periodCount, smaPeriodCount).ComputeByIndex(_index).State == Overtrade.Oversold;

        public bool IsFullStoOversold(int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => new StochasticsOvertrade.Full(_equity, periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(_index).State == Overtrade.Oversold;

        public bool IsSlowStoOversold(int periodCount, int smaPeriodCountD)
            => new StochasticsOvertrade.Slow(_equity, periodCount, smaPeriodCountD).ComputeByIndex(_index).State == Overtrade.Oversold;

        public bool IsAboveSma(int periodCount)
            => new IsAboveSimpleMovingAverage(_equity, periodCount).ComputeByIndex(_index).IsMatched;

        public bool IsAboveEma(int periodCount)
            => new IsAboveExponentialMovingAverage(_equity, periodCount).ComputeByIndex(_index).IsMatched;

        #endregion

        #region Oscillators

        public bool IsSmaOscBullish(int periodCount1, int periodCount2)
            => new SimpleMovingAverageCrossover(_equity, periodCount1, periodCount2).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsSmaOscBearish(int periodCount1, int periodCount2)
            => new SimpleMovingAverageCrossover(_equity, periodCount1, periodCount2).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsEmaOscBullish(int periodCount1, int periodCount2)
            => new ExponentialMovingAverageCrossover(_equity, periodCount1, periodCount2).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsEmaOscBearish(int periodCount1, int periodCount2)
            => new ExponentialMovingAverageCrossover(_equity, periodCount1, periodCount2).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsMacdOscBullish(int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => new MovingAverageConvergenceDivergenceCrossover(_equity, emaPeriodCount1, emaPeriodCount2, demPeriodCount).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsMacdOscBearish(int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => new MovingAverageConvergenceDivergenceCrossover(_equity, emaPeriodCount1, emaPeriodCount2, demPeriodCount).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsFastStoOscBullish(int periodCount, int smaPeriodCount)
            => new StochasticsCrossover.Fast(_equity, periodCount, smaPeriodCount).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsFastStoOscBearish(int periodCount, int smaPeriodCount)
            => new StochasticsCrossover.Fast(_equity, periodCount, smaPeriodCount).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsFullStoOscBullish(int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => new StochasticsCrossover.Full(_equity, periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsFullStoOscBearish(int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => new StochasticsCrossover.Full(_equity, periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(_index).State == Trend.Bearish;

        public bool IsSlowStoOscBullish(int periodCount, int smaPeriodCountD)
            => new StochasticsCrossover.Slow(_equity, periodCount, smaPeriodCountD).ComputeByIndex(_index).State == Trend.Bullish;

        public bool IsSlowStoOscBearish(int periodCount, int smaPeriodCountD)
            => new StochasticsCrossover.Slow(_equity, periodCount, smaPeriodCountD).ComputeByIndex(_index).State == Trend.Bearish;

        #endregion

        #region Crosses

        public bool IsSmaBullishCross(int periodCount1, int periodCount2)
        {
            var result = new SimpleMovingAverageCrossover(_equity, periodCount1, periodCount2).ComputeByIndex(_index);
            return result.IsMatched && result.State == Trend.Bullish;
        }

        public bool IsSmaBearishCross(int periodCount1, int periodCount2)
        {
            var result = new SimpleMovingAverageCrossover(_equity, periodCount1, periodCount2).ComputeByIndex(_index);
            return result.IsMatched && result.State == Trend.Bearish;
        }

        public bool IsEmaBullishCross(int periodCount1, int periodCount2)
        {
            var result = new ExponentialMovingAverageCrossover(_equity, periodCount1, periodCount2).ComputeByIndex(_index);
            return result.IsMatched && result.State == Trend.Bullish;
        }

        public bool IsEmaBearishCross(int periodCount1, int periodCount2)
        {
            var result = new ExponentialMovingAverageCrossover(_equity, periodCount1, periodCount2).ComputeByIndex(_index);
            return result.IsMatched && result.State == Trend.Bearish;
        }

        public bool IsMacdBullishCross(int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
        {
            var result = new MovingAverageConvergenceDivergenceCrossover(_equity, emaPeriodCount1, emaPeriodCount2, demPeriodCount).ComputeByIndex(_index);
            return result.IsMatched && result.State == Trend.Bullish;
        }

        public bool IsMacdBearishCross(int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
        {
            var result = new MovingAverageConvergenceDivergenceCrossover(_equity, emaPeriodCount1, emaPeriodCount2, demPeriodCount).ComputeByIndex(_index);
            return result.IsMatched && result.State == Trend.Bearish;
        }

        public bool IsFastStoBullishCross(int periodCount, int smaPeriodCount)
        {
            var result = new StochasticsCrossover.Fast(_equity, periodCount, smaPeriodCount).ComputeByIndex(_index);
            return result.IsMatched && result.State == Trend.Bullish;
        }

        public bool IsFastStoBearishCross(int periodCount, int smaPeriodCount)
        {
            var result = new StochasticsCrossover.Fast(_equity, periodCount, smaPeriodCount).ComputeByIndex(_index);
            return result.IsMatched && result.State == Trend.Bearish;
        }

        public bool IsFullStoBullishCross(int periodCount, int smaPeriodCountK, int smaPeriodCountD)
        {
            var result = new StochasticsCrossover.Full(_equity, periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(_index);
            return result.IsMatched && result.State == Trend.Bullish;
        }

        public bool IsFullStoBearishCross(int periodCount, int smaPeriodCountK, int smaPeriodCountD)
        {
            var result = new StochasticsCrossover.Full(_equity, periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(_index);
            return result.IsMatched && result.State == Trend.Bearish;
        }

        public bool IsSlowStoBullishCross(int periodCount, int smaPeriodCountD)
        {
            var result = new StochasticsCrossover.Slow(_equity, periodCount, smaPeriodCountD).ComputeByIndex(_index);
            return result.IsMatched && result.State == Trend.Bullish;
        }

        public bool IsSlowStoBearishCross(int periodCount, int smaPeriodCountD)
        {
            var result = new StochasticsCrossover.Slow(_equity, periodCount, smaPeriodCountD).ComputeByIndex(_index);
            return result.IsMatched && result.State == Trend.Bearish;
        }

        #endregion  
    }
}
