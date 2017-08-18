using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Core.Exception;
using Trady.Core.Period;

namespace Trady.Core
{
    public static class CandleExtension
    {
        public static decimal GetUpperShadow(this Candle candle) => candle.Open < candle.Close ? candle.High - candle.Close : candle.High - candle.Open;

        public static decimal GetLowerShadow(this Candle candle) => candle.Open < candle.Close ? candle.Open - candle.Low : candle.Close - candle.Low;

        public static decimal GetBody(this Candle candle) => Math.Abs(candle.Open - candle.Close);

        public static bool IsBullish(this Candle candle) => candle.Open - candle.Close > 0;

        public static bool IsBearish(this Candle candle) => candle.Open - candle.Close < 0;

        #region candle list transformation

        public static IEnumerable<Candle> Transform<TSourcePeriod, TTargetPeriod>(this IEnumerable<Candle> candles)
            where TSourcePeriod : IPeriod
            where TTargetPeriod : IPeriod
        {
            if (typeof(TSourcePeriod).Equals(typeof(TTargetPeriod)))
                return candles;

            if (!IsTimeframesValid<TSourcePeriod>(candles, out Candle err))
                throw new InvalidTimeframeException(err.DateTime);

            if (!IsTransformationValid<TSourcePeriod, TTargetPeriod>())
                throw new InvalidTransformationException(typeof(TSourcePeriod), typeof(TTargetPeriod));

            var outCandles = new List<Candle>();
            var periodInstance = Activator.CreateInstance<TTargetPeriod>();

            DateTime startTime = candles.First().DateTime;
            while (startTime <= candles.Last().DateTime)
            {
                var nextStartTime = periodInstance.NextTimestamp(startTime);
                if (periodInstance.IsTimestamp(startTime))
                {
                    var outCandle = ComputeCandle(candles, startTime, nextStartTime);
                    if (outCandle != null)
                        outCandles.Add(outCandle);
                }
                startTime = nextStartTime;
            }

            return outCandles;
        }

        static bool IsTimeframesValid<TPeriod>(IEnumerable<Candle> candles, out Candle err)
            where TPeriod : IPeriod
        {
            var periodInstance = Activator.CreateInstance<TPeriod>();
            err = default(Candle);
            for (int i = 0; i < candles.Count() - 1; i++)
            {
                var candleEndTime = periodInstance.NextTimestamp(candles.ElementAt(i).DateTime);
                if (candleEndTime > candles.ElementAt(i + 1).DateTime)
                {
                    err = candles.ElementAt(i);
                    return false;
                }
            }
            return true;
        }

        static bool IsTransformationValid<TSourcePeriod, TTargetPeriod>()
            where TSourcePeriod : IPeriod
            where TTargetPeriod : IPeriod
        {
            var inputInstance = Activator.CreateInstance<TSourcePeriod>();
            var outputInstance = Activator.CreateInstance<TTargetPeriod>();

            if (inputInstance is IIntradayPeriod input)
                return !(outputInstance is IIntradayPeriod output) || (input.NumberOfSecond < output.NumberOfSecond);

            var input2 = inputInstance as IInterdayPeriod;
            if (outputInstance is IIntradayPeriod output2)
                return false;

            var output3 = outputInstance as IInterdayPeriod;
            if (input2.OrderOfTransformation >= output3.OrderOfTransformation)
                return false;

            return true;
        }

        static Candle ComputeCandle(IEnumerable<Candle> candles, DateTime startTime, DateTime endTime)
        {
            var candle = candles.Where(c => c.DateTime >= startTime && c.DateTime < endTime);
            if (candle.Any())
            {
                var dateTime = candle.First().DateTime;
                var open = candle.First().Open;
                var high = candle.Max(stick => stick.High);
                var low = candle.Min(stick => stick.Low);
                var close = candle.Last().Close;
                var volume = candle.Sum(stick => stick.Volume);
                return new Candle(dateTime, open, high, low, close, volume);
            }
            return null;
        }

        #endregion candle list transformation
    }
}