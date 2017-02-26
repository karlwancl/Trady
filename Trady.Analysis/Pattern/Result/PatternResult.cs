using System;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Pattern
{
    public class PatternResult<TState> : TickBase
    {
        private TState _state;

        public PatternResult(DateTime dateTime, TState state) : base(dateTime)
        {
            _state = state;
        }

        public TState State => _state;
    }
}