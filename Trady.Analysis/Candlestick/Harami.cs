using System;
using System.Collections.Generic;

using Trady.Analysis.Infrastructure;
using Trady.Core;
using System.Linq;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    /// <summary>
    /// Reference: http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:candlestick_pattern_dictionary
    /// </summary>
    public class Harami<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        private BearishByTuple _bearish;
        private readonly bool _shadowsHasToBeContained;

        public Harami(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, bool containedShadows = false) : base(inputs, inputMapper)
        {
            var mappedInputs = inputs.Select(inputMapper);
            var ocs = mappedInputs.Select(i => (i.Open, i.Close));
            _bearish = new BearishByTuple(ocs);
            _shadowsHasToBeContained = containedShadows;
        }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            if (index == 0)
                return default;

            if (_bearish[index - 1] == _bearish[index])
                return false;
            
            var bodyIsContained = _bearish[index - 1] ?
                mappedInputs[index - 1].Open > mappedInputs[index].Close &&
                mappedInputs[index - 1].Close < mappedInputs[index].Open :
                mappedInputs[index - 1].Open < mappedInputs[index].Close &&
                mappedInputs[index - 1].Close > mappedInputs[index].Open;

            if (!_shadowsHasToBeContained || !bodyIsContained)
                return bodyIsContained;

            return mappedInputs[index - 1].High > mappedInputs[index].High && 
                mappedInputs[index - 1].Low < mappedInputs[index].Low;
        }
    }

    public class HaramiByTuple : Harami<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public HaramiByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, bool containedShadows = false)
            : base(inputs, i => i, containedShadows)
        {
        }
    }

    public class Harami : Harami<IOhlcv, AnalyzableTick<bool?>>
    {
        public Harami(IEnumerable<IOhlcv> inputs, bool containedShadows = false)
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), containedShadows)
        {
        }
    }
}
