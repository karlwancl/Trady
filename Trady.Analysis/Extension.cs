using System;
using System.Collections.Generic;
using System.Text;
using Trady.Analysis.Pattern.Indicator;
using Trady.Core;

namespace Trady.Analysis
{
    public static class Extension
    {
        #region Patterns

        public static bool IsBullish(this Equity series, int index)
            => new BullishClosePriceChange(series).ComputeByIndex(index).IsMatched;

        public static bool IsBearish(this Equity series, int index)
            => new BearishClosePriceChange(series).ComputeByIndex(index).IsMatched;

        public static bool IsAccumDistBullish(this Equity series, int index)
            => new BullishAccumulationDistributionLine(series).ComputeByIndex(index).IsMatched;

        public static bool IsAccumDistBearish(this Equity series, int index)
            => new BearishAccumulationDistributionLine(series).ComputeByIndex(index).IsMatched;

        public static bool IsObvBullish(this Equity series, int index)
            => new BullishOnBalanceVolume(series).ComputeByIndex(index).IsMatched;

        public static bool IsObvBearish(this Equity series, int index)
            => new BearishOnBalanceVolume(series).ComputeByIndex(index).IsMatched;

        public static bool IsInBbRange(this Equity series, int periodCount, int sdCount, int index)
            => new InRangeBollingerBands(series, periodCount, sdCount).ComputeByIndex(index).IsMatched;

        public static bool IsHighest(this Equity series, int periodCount, int index)
            => new IsHighestPrice(series, periodCount).ComputeByIndex(index).IsMatched;

        public static bool IsLowest(this Equity series, int periodCount, int index)
            => new IsLowestPrice(series, periodCount).ComputeByIndex(index).IsMatched;

        public static bool IsRsiOverbought(this Equity series, int periodCount, bool isUse80, int index)
            => new OverboughtRelativeStrengthIndex(series, periodCount, isUse80).ComputeByIndex(index).IsMatched;

        public static bool IsRsiOversold(this Equity series, int periodCount, bool isUse20, int index)
            => new OversoldRelativeStrengthIndex(series, periodCount, isUse20).ComputeByIndex(index).IsMatched;

        public static bool IsFastStoOverbought(this Equity series, int periodCount, int smaPeriodCount, int index)
            => new OverboughtStochastics.Fast(series, periodCount, smaPeriodCount).ComputeByIndex(index).IsMatched;

        public static bool IsFullStoOverbought(this Equity series, int periodCount, int smaPeriodCountK, int smaPeriodCountD, int index)
            => new OverboughtStochastics.Full(series, periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(index).IsMatched;

        public static bool IsSlowStoOverbought(this Equity series, int periodCount, int smaPeriodCountD, int index)
            => new OverboughtStochastics.Slow(series, periodCount, smaPeriodCountD).ComputeByIndex(index).IsMatched;

        public static bool IsFastStoOversold(this Equity series, int periodCount, int smaPeriodCount, int index)
            => new OversoldStochastics.Fast(series, periodCount, smaPeriodCount).ComputeByIndex(index).IsMatched;

        public static bool IsFullStoOversold(this Equity series, int periodCount, int smaPeriodCountK, int smaPeriodCountD, int index)
            => new OversoldStochastics.Full(series, periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(index).IsMatched;

        public static bool IsSlowStoOversold(this Equity series, int periodCount, int smaPeriodCountD, int index)
            => new OversoldStochastics.Slow(series, periodCount, smaPeriodCountD).ComputeByIndex(index).IsMatched;

        #endregion

        #region Oscillators

        public static bool IsSmaOscBullish(this Equity series, int periodCount1, int periodCount2, int index)
            => new SimpleMovingAverageCrossover(series, periodCount1, periodCount2).ComputeByIndex(index).IsBullish;

        public static bool IsSmaOscBearish(this Equity series, int periodCount1, int periodCount2, int index)
            => new SimpleMovingAverageCrossover(series, periodCount1, periodCount2).ComputeByIndex(index).IsBearish;

        public static bool IsEmaOscBullish(this Equity series, int periodCount1, int periodCount2, int index)
            => new ExponentialMovingAverageCrossover(series, periodCount1, periodCount2).ComputeByIndex(index).IsBullish;

        public static bool IsEmaOscBearish(this Equity series, int periodCount1, int periodCount2, int index)
            => new ExponentialMovingAverageCrossover(series, periodCount1, periodCount2).ComputeByIndex(index).IsBearish;

        public static bool IsMacdOscBullish(this Equity series, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount, int index)
            => new MovingAverageConvergenceDivergenceCrossover(series, emaPeriodCount1, emaPeriodCount2, demPeriodCount).ComputeByIndex(index).IsBullish;

        public static bool IsMacdOscBearish(this Equity series, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount, int index)
            => new MovingAverageConvergenceDivergenceCrossover(series, emaPeriodCount1, emaPeriodCount2, demPeriodCount).ComputeByIndex(index).IsBearish;

        public static bool IsFastStoOscBullish(this Equity series, int periodCount, int smaPeriodCount, int index)
            => new StochasticsCrossover.Fast(series, periodCount, smaPeriodCount).ComputeByIndex(index).IsBullish;

        public static bool IsFastStoOscBearish(this Equity series, int periodCount, int smaPeriodCount, int index)
            => new StochasticsCrossover.Fast(series, periodCount, smaPeriodCount).ComputeByIndex(index).IsBearish;

        public static bool IsFullStoOscBullish(this Equity series, int periodCount, int smaPeriodCountK, int smaPeriodCountD, int index)
            => new StochasticsCrossover.Full(series, periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(index).IsBullish;

        public static bool IsFullStoOscBearish(this Equity series, int periodCount, int smaPeriodCountK, int smaPeriodCountD, int index)
            => new StochasticsCrossover.Full(series, periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(index).IsBearish;

        public static bool IsSlowStoOscBullish(this Equity series, int periodCount, int smaPeriodCountD, int index)
            => new StochasticsCrossover.Slow(series, periodCount, smaPeriodCountD).ComputeByIndex(index).IsBullish;

        public static bool IsSlowStoOscBearish(this Equity series, int periodCount, int smaPeriodCountD, int index)
            => new StochasticsCrossover.Slow(series, periodCount, smaPeriodCountD).ComputeByIndex(index).IsBearish;

        #endregion

        #region Crosses

        public static bool IsSmaBullishCross(this Equity series, int periodCount1, int periodCount2, int index)
        {
            var result = new SimpleMovingAverageCrossover(series, periodCount1, periodCount2).ComputeByIndex(index);
            return result.IsMatched && result.IsBullish;
        }

        public static bool IsSmaBearishCross(this Equity series, int periodCount1, int periodCount2, int index)
        {
            var result = new SimpleMovingAverageCrossover(series, periodCount1, periodCount2).ComputeByIndex(index);
            return result.IsMatched && result.IsBearish;
        }

        public static bool IsEmaBullishCross(this Equity series, int periodCount1, int periodCount2, int index)
        {
            var result = new ExponentialMovingAverageCrossover(series, periodCount1, periodCount2).ComputeByIndex(index);
            return result.IsMatched && result.IsBullish;
        }

        public static bool IsEmaBearishCross(this Equity series, int periodCount1, int periodCount2, int index)
        {
            var result = new ExponentialMovingAverageCrossover(series, periodCount1, periodCount2).ComputeByIndex(index);
            return result.IsMatched && result.IsBearish;
        }

        public static bool IsMacdBullishCross(this Equity series, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount, int index)
        {
            var result = new MovingAverageConvergenceDivergenceCrossover(series, emaPeriodCount1, emaPeriodCount2, demPeriodCount).ComputeByIndex(index);
            return result.IsMatched && result.IsBullish;
        }

        public static bool IsMacdBearishCross(this Equity series, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount, int index)
        {
            var result = new MovingAverageConvergenceDivergenceCrossover(series, emaPeriodCount1, emaPeriodCount2, demPeriodCount).ComputeByIndex(index);
            return result.IsMatched && result.IsBearish;
        }

        public static bool IsFastStoBullishCross(this Equity series, int periodCount, int smaPeriodCount, int index)
        {
            var result = new StochasticsCrossover.Fast(series, periodCount, smaPeriodCount).ComputeByIndex(index);
            return result.IsMatched && result.IsBullish;
        }

        public static bool IsFastStoBearishCross(this Equity series, int periodCount, int smaPeriodCount, int index)
        {
            var result = new StochasticsCrossover.Fast(series, periodCount, smaPeriodCount).ComputeByIndex(index);
            return result.IsMatched && result.IsBearish;
        }

        public static bool IsFullStoBullishCross(this Equity series, int periodCount, int smaPeriodCountK, int smaPeriodCountD, int index)
        {
            var result = new StochasticsCrossover.Full(series, periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(index);
            return result.IsMatched && result.IsBullish;
        }

        public static bool IsFullStoBearishCross(this Equity series, int periodCount, int smaPeriodCountK, int smaPeriodCountD, int index)
        {
            var result = new StochasticsCrossover.Full(series, periodCount, smaPeriodCountK, smaPeriodCountD).ComputeByIndex(index);
            return result.IsMatched && result.IsBearish;
        }

        public static bool IsSlowStoBullishCross(this Equity series, int periodCount, int smaPeriodCountD, int index)
        {
            var result = new StochasticsCrossover.Slow(series, periodCount, smaPeriodCountD).ComputeByIndex(index);
            return result.IsMatched && result.IsBullish;
        }

        public static bool IsSlowStoBearishCross(this Equity series, int periodCount, int smaPeriodCountD, int index)
        {
            var result = new StochasticsCrossover.Slow(series, periodCount, smaPeriodCountD).ComputeByIndex(index);
            return result.IsMatched && result.IsBearish;
        }

        #endregion  
    }
}
