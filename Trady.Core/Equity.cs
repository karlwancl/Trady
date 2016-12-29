using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Core.Exception;
using Trady.Core.Helper;
using Trady.Core.Period;

namespace Trady.Core
{
    public class Equity : TimeSeries<Candle>
    {
        public Equity(string name, IList<Candle> candles = null, PeriodOption period = PeriodOption.Daily, int maxTickCount = 65536)
            : base(name, candles, period, maxTickCount)
        {
        }

        public Equity Transform(PeriodOption outputPeriod)
        {
            if (Period == outputPeriod)
                return this;

            if (!IsTransformValid(Period, outputPeriod))
                throw new InvalidTransformationException(Period, outputPeriod);

            var outputSeries = new Equity(Name);
            var outputPeriodInstance = outputPeriod.CreateInstance();

            DateTime periodStartTime = Ticks.First().DateTime;
            while (periodStartTime <= Ticks.Last().DateTime)
            {
                var periodNextStartTime = outputPeriodInstance.NextTimestamp(periodStartTime);
                if (outputPeriodInstance.IsTimestamp(periodStartTime))
                {
                    var candle = ComputeCandle(Ticks, periodStartTime, periodNextStartTime);
                    if (candle != null)
                        outputSeries.Add(candle);
                }
                periodStartTime = periodNextStartTime;
            }

            return outputSeries;

            bool IsTransformValid(PeriodOption inputPeriodType, PeriodOption outputPeriodType)
            {
                var inputInstance = inputPeriodType.CreateInstance();
                var outputInstance = outputPeriodType.CreateInstance();

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

            Candle ComputeCandle(IList<Candle> candles, DateTime startTime, DateTime endTime)
            {
                var candlesSubset = candles.Where(c => c.DateTime >= startTime && c.DateTime < endTime);
                if (candlesSubset.Any())
                {
                    var startTimeOfFirstCandle = candlesSubset.First().DateTime;
                    var open = candlesSubset.First().Open;
                    var high = candlesSubset.Max(stick => stick.High);
                    var low = candlesSubset.Min(stick => stick.Low);
                    var close = candlesSubset.Last().Close;
                    var volume = candlesSubset.Sum(stick => stick.Volume);
                    return new Candle(startTimeOfFirstCandle, open, high, low, close, volume);
                }
                return null;
            }
        }
    }
}
