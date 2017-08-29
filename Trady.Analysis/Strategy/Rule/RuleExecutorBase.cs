using System;
using System.Collections.Generic;
using System.Linq;

namespace Trady.Analysis.Strategy.Rule
{
    public abstract class RuleExecutorBase<TInput, TIndexed, TOutput> : IRuleExecutor<TInput, TIndexed, TOutput> where TIndexed: IIndexedObject<TInput>
    {        
        protected RuleExecutorBase(Func<TIndexed, int, TOutput> outputFunc, params Func<IRule<TIndexed>>[] rules)
        {
            OutputFunc = outputFunc ?? throw new ArgumentNullException(nameof(outputFunc));
            Rules = rules ?? throw new ArgumentNullException(nameof(outputFunc));
            if (!rules.Any())
                throw new ArgumentException("You must have at least one rule to execute", nameof(rules));
        }

        public Func<IRule<TIndexed>>[] Rules { get; }

        public Func<TIndexed, int, TOutput> OutputFunc { get; }

        public IEnumerable<TOutput> Execute(IEnumerable<TInput> inputs, int? startIndex = default(int?), int? endIndex = default(int?))
        {
            var output = new List<TOutput>();
            for (int i = startIndex ?? 0; i < (endIndex ?? (inputs.Count() - 1)); i++)
			{
				var indexedCandle = IndexedObjectConstructor(inputs, i);
				for (int j = 0; j < Rules.Count(); j++)
				{
                    if (Rules[j]().IsValid(indexedCandle))
                    {
                        output.Add(OutputFunc(indexedCandle, j));
						break;
					}
				}
			}
            return output;
        }

        public abstract Func<IEnumerable<TInput>, int, TIndexed> IndexedObjectConstructor { get; }
    }
}