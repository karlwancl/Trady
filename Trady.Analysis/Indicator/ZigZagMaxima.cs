using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    //http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:zigzag
    public class ZigZagMaxima<TInput, TOutput> : AnalyzableBase<TInput, decimal, (decimal? Close, int CalculationIndex)?, TOutput>
    {
        private decimal _threshold;
        private List<decimal> turningPoints = new List<decimal>();
        public ZigZagMaxima(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, decimal threshold = 0.03m) : base(inputs, inputMapper)
        {
            _threshold = threshold;
        }
        
        protected override (decimal? Close, int CalculationIndex)? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
        {
            if (index == 0)
                return null;
            
            if (index == mappedInputs.Count - 1)
                return null;
            
            var currentClose = mappedInputs[index];

            var futureCloses = mappedInputs.Skip(index + 1).Cast<decimal?>().ToList();
            var endIndex = futureCloses.FindIndex(c => c > currentClose);
            if (endIndex != -1)
                futureCloses = futureCloses.Take(endIndex).ToList();

            var historicalCloses = mappedInputs.Take(index).Cast<decimal?>().ToList();
            var startIndex = historicalCloses.FindLastIndex(c => c >= currentClose) + 1;
            if (startIndex != -1)
                historicalCloses = historicalCloses.Skip(startIndex).ToList();

            var futureLow = futureCloses.FirstOrDefault(c => (currentClose - c) / currentClose > _threshold);
            var previousLow = historicalCloses.LastOrDefault(c => currentClose > c && (currentClose - c) / currentClose > _threshold);
            if (futureLow.HasValue && previousLow.HasValue)
            {
                return (currentClose, index + futureCloses.FindIndex(c => (currentClose - c) / currentClose > _threshold) + 1);
            }
            return null;
        }
    }

    public class ZigZagMaximaByCloses : ZigZagMaxima<decimal, (decimal? Close, int CalculationIndex)?>
    {
        public ZigZagMaximaByCloses(IEnumerable<decimal> inputs, decimal threshold = 0.03m)
            : base(inputs, i => i, threshold)
        {
        }
    }

    public class ZigZagMaxima : ZigZagMaxima<IOhlcv, AnalyzableTick<(decimal? Close, int CalculationIndex)?>>
    {
        public ZigZagMaxima(IEnumerable<IOhlcv> inputs, decimal threshold = 0.03m)
            : base(inputs, i => i.Close, threshold)
        {
        }
    }
}
