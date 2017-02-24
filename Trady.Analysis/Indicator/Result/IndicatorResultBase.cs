using System;
using System.Collections.Generic;
using System.Text;
using Trady.Core;

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
