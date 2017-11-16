using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Core.Exception;
using Trady.Core.Infrastructure;
using Trady.Core.Period;

namespace Trady.Core
{
    public static class IOhlcvDataExtension
    {
        public static decimal GetUpperShadow(this IOhlcv candle) => candle.Open < candle.Close ? candle.High - candle.Close : candle.High - candle.Open;

        public static decimal GetLowerShadow(this IOhlcv candle) => candle.Open < candle.Close ? candle.Open - candle.Low : candle.Close - candle.Low;

        public static decimal GetBody(this IOhlcv candle) => Math.Abs(candle.Open - candle.Close);

        public static bool IsBull(this IOhlcv candle) => candle.Open - candle.Close > 0;

        public static bool IsBear(this IOhlcv candle) => candle.Open - candle.Close < 0;

        #region candle list transformation

        public static IReadOnlyList<IOhlcv> Transform<TSourcePeriod, TTargetPeriod>(this IEnumerable<IOhlcv> candles)
            where TSourcePeriod : IPeriod
            where TTargetPeriod : IPeriod
        {
            if (typeof(TSourcePeriod).Equals(typeof(TTargetPeriod)))
                return candles.ToList();

            if (!IsTimeframesValid<TSourcePeriod>(candles, out IOhlcv err))
                throw new InvalidTimeframeException(err.DateTime);

            if (!IsTransformationValid<TSourcePeriod, TTargetPeriod>())
                throw new InvalidTransformationException(typeof(TSourcePeriod), typeof(TTargetPeriod));

            var outIOhlcvDatas = new List<IOhlcv>();
            var periodInstance = Activator.CreateInstance<TTargetPeriod>();

            var startTime = candles.First().DateTime;
            while (startTime <= candles.Last().DateTime)
            {
                var nextStartTime = periodInstance.NextTimestamp(startTime);
                if (periodInstance.IsTimestamp(startTime))
                {
                    var outIOhlcvData = ComputeIOhlcvData(candles, startTime, nextStartTime);
                    if (outIOhlcvData != null)
                        outIOhlcvDatas.Add(outIOhlcvData);
                }
                startTime = nextStartTime;
            }

            return outIOhlcvDatas;
        }

        private static bool IsTimeframesValid<TPeriod>(IEnumerable<IOhlcv> candles, out IOhlcv err)
            where TPeriod : IPeriod
        {
            var periodInstance = Activator.CreateInstance<TPeriod>();
            err = default(IOhlcv);
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

        private static bool IsTransformationValid<TSourcePeriod, TTargetPeriod>()
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

        private static IOhlcv ComputeIOhlcvData(IEnumerable<IOhlcv> candles, DateTimeOffset startTime, DateTimeOffset endTime)
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