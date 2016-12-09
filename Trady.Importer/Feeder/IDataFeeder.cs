using System;
using System.Collections.Generic;
using Trady.Core;

namespace Trady.Importer.Feeder
{
    public interface IDataFeeder
    {
        TimeSpan Interval { get; set; }

        string[] Symbols { get; set; }

        IObservable<IList<Candle>> WhenCandleFed();
    }

    public abstract class DataFeederBase : IDataFeeder
    {
        protected DataFeederBase()
        {
        }

        public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(1);

        public string[] Symbols { get; set; }

        public IObservable<IList<Candle>> WhenCandleFed()
        {
            //return Observable
            //    .Interval(Interval)
            //    .Select(t => GetCandles());
            throw new NotImplementedException();
        }

        public abstract IList<Candle> GetCandles(DateTime startDate, DateTime endDate = default(DateTime));
    }
}
