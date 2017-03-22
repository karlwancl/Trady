using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Core.Exception;
using Trady.Core.Period;

namespace Trady.Core.Helper
{
    public static class EquityExtension
    {
        public static int? FindCandleIndexOrDefault(this Equity equity, Predicate<Candle> predicate)
        {
            int index = equity.ToList().FindIndex(predicate);
            return index == -1 ? (int?)null : index;
        }

        public static int? FindLastCandleIndexOrDefault(this Equity equity, Predicate<Candle> predicate)
        {
            int index = equity.ToList().FindLastIndex(predicate);
            return index == -1 ? (int?)null : index;
        }

        public static Equity ToEquity(this IEnumerable<Candle> candles, string name, PeriodOption period)
            => new Equity(name, candles, period);

        public static Equity Transform(this Equity sourceEquity, PeriodOption targetPeriod)
        {
            if (sourceEquity.Period == targetPeriod)
                return sourceEquity;

            if (!IsTransformationValid(sourceEquity.Period, targetPeriod))
                throw new InvalidTransformationException(sourceEquity.Period, targetPeriod);

            var candles = new List<Candle>();
            var periodInstance = targetPeriod.CreateInstance();

            DateTime startTime = sourceEquity.First().DateTime;
            while (startTime <= sourceEquity.Last().DateTime)
            {
                var nextStartTime = periodInstance.NextTimestamp(startTime);
                if (periodInstance.IsTimestamp(startTime))
                {
                    var candle = ComputeCandle(sourceEquity, startTime, nextStartTime);
                    if (candle != null)
                        candles.Add(candle);
                }
                startTime = nextStartTime;
            }

            return new Equity(sourceEquity.Name, candles, targetPeriod);
        }

        private static bool IsTransformationValid(PeriodOption inputPeriod, PeriodOption outputPeriod)
        {
            var inputInstance = inputPeriod.CreateInstance();
            var outputInstance = outputPeriod.CreateInstance();

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

        private static Candle ComputeCandle(Equity equity, DateTime startTime, DateTime endTime)
        {
            var candles = equity.Where(c => c.DateTime >= startTime && c.DateTime < endTime);
            if (candles.Any())
            {
                var dateTime = candles.First().DateTime;
                var open = candles.First().Open;
                var high = candles.Max(stick => stick.High);
                var low = candles.Min(stick => stick.Low);
                var close = candles.Last().Close;
                var volume = candles.Sum(stick => stick.Volume);
                return new Candle(dateTime, open, high, low, close, volume);
            }
            return null;
        }
    }
}