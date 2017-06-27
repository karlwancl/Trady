using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Infrastructure
{
	public abstract class AnalyzableWrapperBase<TAnalyzable, TInput, TOutput> : AnalyzableBase<Candle, AnalyzableTick<TOutput>> where TAnalyzable : IAnalyzable
	{
		readonly TAnalyzable _analyzable;

		protected AnalyzableWrapperBase(IList<Candle> candles, params object[] @params) : base(candles)
		{
			var inputs = candles.Select(MappingFunction).ToList();
			var ctor = _analyzable.GetType().GetTypeInfo().DeclaredConstructors.First(); // For demo only, needs change on implementation
			_analyzable = (TAnalyzable)ctor.Invoke(inputs, @params);    // For demo only, needs further test
		}

		protected override AnalyzableTick<TOutput> ComputeByIndexImpl(int index)
			=> new AnalyzableTick<TOutput>(Inputs[index].DateTime, (TOutput)_analyzable.ComputeByIndex(index));

        protected abstract Func<Candle, TInput> MappingFunction { get; }
	}
}
