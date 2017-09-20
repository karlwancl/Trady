using System;
using System.Collections.Generic;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class Func0Analyzable<TInput, TOutput> : NumericAnalyzableBase<TInput, TInput, TOutput>, IFuncAnalyzable<TOutput> 
    {
        private readonly Func<IReadOnlyList<TInput>, int, IAnalyzeContext<TInput>, decimal?> _func;
        private readonly IAnalyzeContext<TInput> _ctx;

        public Func0Analyzable(IEnumerable<TInput> inputs) : base(inputs, i => i)
        {
        }

        private Func0Analyzable(IEnumerable<TInput> inputs, Func<IReadOnlyList<TInput>, int, IAnalyzeContext<TInput>, decimal?> func)
            : this(inputs)
        {
            _func = func;
            _ctx = new AnalyzeContext<TInput>(inputs);
        }

        public Func0Analyzable<TInput, TOutput> Init(Func<IReadOnlyList<TInput>, int, IAnalyzeContext<TInput>, decimal?> func)
            => new Func0Analyzable<TInput, TOutput>(_mappedInputs, func);

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<TInput> mappedInputs, int index)
        {
            if (_func == null || _ctx == null)
                throw new NullReferenceException("No func is found for the analyzable, please ensure you have called Init method to init");

            return _func(mappedInputs, index, _ctx);
        }

		public void Dispose()
		{
			_ctx?.Dispose();
		}
    }

    public class Func0Analyzable : Func0Analyzable<Candle, AnalyzableTick<decimal?>>
    {
        public Func0Analyzable(IEnumerable<Candle> inputs) : base(inputs)
        {
        }
    }


    public class Func1Analyzable<TInput, TOutput> : NumericAnalyzableBase<TInput, TInput, TOutput>, IFuncAnalyzable<TOutput>
	{
		private readonly Func<IReadOnlyList<TInput>, int, decimal, IAnalyzeContext<TInput>, decimal?> _func;
		private readonly IAnalyzeContext<TInput> _ctx;

		public decimal Parameter { get; }

		public Func1Analyzable(IEnumerable<TInput> inputs, decimal parameter) : base(inputs, i => i)
		{
			Parameter = parameter;
		}

		private Func1Analyzable(IEnumerable<TInput> inputs, Func<IReadOnlyList<TInput>, int, decimal, IAnalyzeContext<TInput>, decimal?> func, decimal parameter)
			: this(inputs, parameter)
		{
			_func = func;
            _ctx = new AnalyzeContext<TInput>(inputs);
		}

		public Func1Analyzable<TInput, TOutput> Init(Func<IReadOnlyList<TInput>, int, decimal, IAnalyzeContext<TInput>, decimal?> func)
			=> new Func1Analyzable<TInput, TOutput>(_mappedInputs, func, Parameter);

		protected override decimal? ComputeByIndexImpl(IReadOnlyList<TInput> mappedInputs, int index)
		{
			if (_func == null || _ctx == null)
				throw new NullReferenceException("No func is found for the analyzable, please ensure you have called Init method to init");

			return _func(mappedInputs, index, Parameter, _ctx);
		}

        public void Dispose()
        {
            _ctx?.Dispose();
        }
    }

    public class Func1Analyzable : Func1Analyzable<Candle, AnalyzableTick<decimal?>>
    {
        public Func1Analyzable(IEnumerable<Candle> inputs, decimal parameter) : base(inputs, parameter)
        {
        }
    }

    public class Func2Analyzable<TInput, TOutput> : NumericAnalyzableBase<TInput, TInput, TOutput>, IFuncAnalyzable<TOutput>
	{
		private readonly Func<IReadOnlyList<TInput>, int, decimal, decimal, IAnalyzeContext<TInput>, decimal?> _func;
		private readonly IAnalyzeContext<TInput> _ctx;

		public decimal Parameter0 { get; }

		public decimal Parameter1 { get; }

		public Func2Analyzable(IEnumerable<TInput> inputs, decimal parameter0, decimal parameter1) : base(inputs, i => i)
		{
			Parameter0 = parameter0;
			Parameter1 = parameter1;
		}

		private Func2Analyzable(IEnumerable<TInput> inputs, Func<IReadOnlyList<TInput>, int, decimal, decimal, IAnalyzeContext<TInput>, decimal?> func, decimal parameter0, decimal parameter1)
			: this(inputs, parameter0, parameter1)
		{
			_func = func;
            _ctx = new AnalyzeContext<TInput>(inputs);
		}

		public Func2Analyzable<TInput, TOutput> Init(Func<IReadOnlyList<TInput>, int, decimal, decimal, IAnalyzeContext<TInput>, decimal?> func)
			=> new Func2Analyzable<TInput, TOutput>(_mappedInputs, func, Parameter0, Parameter1);

		protected override decimal? ComputeByIndexImpl(IReadOnlyList<TInput> mappedInputs, int index)
		{
			if (_func == null || _ctx == null)
				throw new NullReferenceException("No func is found for the analyzable, please ensure you have called Init method to init");

			return _func(mappedInputs, index, Parameter0, Parameter1, _ctx);
		}

        public void Dispose()
        {
            _ctx?.Dispose();
        }
    }

    public class Func2Analyzable : Func2Analyzable<Candle, AnalyzableTick<decimal?>>
    {
        public Func2Analyzable(IEnumerable<Candle> inputs, decimal parameter0, decimal parameter1) : base(inputs, parameter0, parameter1)
        {
        }
    }

    public class Func3Analyzable<TInput, TOutput> : NumericAnalyzableBase<TInput, TInput, TOutput>, IFuncAnalyzable<TOutput>
	{
		private readonly Func<IReadOnlyList<TInput>, int, decimal, decimal, decimal, IAnalyzeContext<TInput>, decimal?> _func;
		private readonly IAnalyzeContext<TInput> _ctx;

		public decimal Parameter0 { get; }

		public decimal Parameter1 { get; }

		public decimal Parameter2 { get; }

		public Func3Analyzable(IEnumerable<TInput> inputs, decimal parameter0, decimal parameter1, decimal parameter2) : base(inputs, i => i)
		{
			Parameter0 = parameter0;
			Parameter1 = parameter1;
			Parameter2 = parameter2;
		}

		private Func3Analyzable(IEnumerable<TInput> inputs, Func<IReadOnlyList<TInput>, int, decimal, decimal, decimal, IAnalyzeContext<TInput>, decimal?> func,
								decimal parameter0, decimal parameter1, decimal parameter2)
			: this(inputs, parameter0, parameter1, parameter2)
		{
			_func = func;
            _ctx = new AnalyzeContext<TInput>(inputs);
		}

		public Func3Analyzable<TInput, TOutput> Init(Func<IReadOnlyList<TInput>, int, decimal, decimal, decimal, IAnalyzeContext<TInput>, decimal?> func)
			=> new Func3Analyzable<TInput, TOutput>(_mappedInputs, func, Parameter0, Parameter1, Parameter2);

		protected override decimal? ComputeByIndexImpl(IReadOnlyList<TInput> mappedInputs, int index)
		{
			if (_func == null)
				throw new NullReferenceException("No func is found for the analyzable, please ensure you have called Init method to init");

			return _func(mappedInputs, index, Parameter0, Parameter1, Parameter2, _ctx);
		}

        public void Dispose()
        {
            _ctx?.Dispose();
        }
    }

    public class Func3Analyzable : Func3Analyzable<Candle, AnalyzableTick<decimal?>>
    {
        public Func3Analyzable(IEnumerable<Candle> inputs, decimal parameter0, decimal parameter1, decimal parameter2) : base(inputs, parameter0, parameter1, parameter2)
        {
        }
    }
}
