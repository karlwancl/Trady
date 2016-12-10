using System;
using System.Collections.Generic;
using System.Text;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public static class IndicatorExtension
    {
        public static IndicatorResultTimeSeries<AccumulationDistributionLine.IndicatorResult> AccumDist(this Equity equity)
            => new AccumulationDistributionLine(equity).Compute();

        public static IndicatorResultTimeSeries<AverageDirectionalIndex.IndicatorResult> Adx(this Equity equity, int periodCount)
            => new AverageDirectionalIndex(equity, periodCount).Compute();

        public static IndicatorResultTimeSeries<AverageTrueRange.IndicatorResult> Atr(this Equity equity, int periodCount)
            => new AverageTrueRange(equity, periodCount).Compute();

        public static IndicatorResultTimeSeries<BollingerBands.IndicatorResult> Bb(this Equity equity, int periodCount, int sdCount)
            => new BollingerBands(equity, periodCount, sdCount).Compute();

        public static IndicatorResultTimeSeries<ClosePriceChange.IndicatorResult> PriceChange(this Equity equity)
            => new ClosePriceChange(equity).Compute();

        public static IndicatorResultTimeSeries<ExponentialMovingAverage.IndicatorResult> Ema(this Equity equity, int periodCount)
            => new ExponentialMovingAverage(equity, periodCount).Compute();

        public static IndicatorResultTimeSeries<ExponentialMovingAverageOscillator.IndicatorResult> EmaOsc(this Equity equity, int periodCount1, int periodCount2)
            => new ExponentialMovingAverageOscillator(equity, periodCount1, periodCount2).Compute();

        public static IndicatorResultTimeSeries<HighestHigh.IndicatorResult> HighestHigh(this Equity equity, int periodCount)
            => new HighestHigh(equity, periodCount).Compute();

        public static IndicatorResultTimeSeries<LowestLow.IndicatorResult> LowestLow(this Equity equity, int periodCount)
            => new LowestLow(equity, periodCount).Compute();

        public static IndicatorResultTimeSeries<MovingAverageConvergenceDivergence.IndicatorResult> Macd(this Equity equity, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            => new MovingAverageConvergenceDivergence(equity, emaPeriodCount1, emaPeriodCount2, demPeriodCount).Compute();

        public static IndicatorResultTimeSeries<OnBalanceVolume.IndicatorResult> Obv(this Equity equity)
            => new OnBalanceVolume(equity).Compute();

        public static IndicatorResultTimeSeries<RawStochasticsValue.IndicatorResult> Rsv(this Equity equity, int periodCount)
            => new RawStochasticsValue(equity, periodCount).Compute();

        public static IndicatorResultTimeSeries<RelativeStrength.IndicatorResult> Rs(this Equity equity, int periodCount)
            => new RelativeStrength(equity, periodCount).Compute();

        public static IndicatorResultTimeSeries<RelativeStrengthIndex.IndicatorResult> Rsi(this Equity equity, int periodCount)
            => new RelativeStrengthIndex(equity, periodCount).Compute();

        public static IndicatorResultTimeSeries<SimpleMovingAverage.IndicatorResult> Sma(this Equity equity, int periodCount)
            => new SimpleMovingAverage(equity, periodCount).Compute();

        public static IndicatorResultTimeSeries<SimpleMovingAverageOscillator.IndicatorResult> SmaOsc(this Equity equity, int periodCount1, int periodCount2)
            => new SimpleMovingAverageOscillator(equity, periodCount1, periodCount2).Compute();

        public static IndicatorResultTimeSeries<Stochastics.IndicatorResult> FastSto(this Equity equity, int periodCount, int smaPeriodCount)
            => new Stochastics.Fast(equity, periodCount, smaPeriodCount).Compute();

        public static IndicatorResultTimeSeries<Stochastics.IndicatorResult> FullSto(this Equity equity, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
            => new Stochastics.Full(equity, periodCount, smaPeriodCountK, smaPeriodCountD).Compute();

        public static IndicatorResultTimeSeries<Stochastics.IndicatorResult> SlowSto(this Equity equity, int periodCount, int smaPeriodCountD)
            => new Stochastics.Slow(equity, periodCount, smaPeriodCountD).Compute();
    }
}
