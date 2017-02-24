using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Trady.Core;

namespace Trady.Analysis
{
    public abstract class IndicatorBase<TTick> : AnalyzableBase<TTick>, IIndicator, IComputable<TTick> where TTick : ITick
    {
        public IndicatorBase(Equity equity, params int[] parameters) : base(equity)
        {
            Parameters = parameters;
        }

        public int[] Parameters { get; private set; }
    }
}