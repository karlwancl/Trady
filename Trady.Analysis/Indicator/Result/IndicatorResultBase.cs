using System;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class IndicatorResultBase : TickBase
    {
        private decimal?[] _values;

        public IndicatorResultBase(DateTime dateTime, params decimal?[] values) : base(dateTime)
        {
            _values = values;
        }

        protected decimal?[] Values { get => _values; set => _values = value; }
    }
}