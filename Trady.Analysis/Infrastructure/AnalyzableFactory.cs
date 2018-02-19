using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Infrastructure
{
    internal static class AnalyzableFactory
    {
        public static TAnalyzable CreateAnalyzable<TAnalyzable, TInput>(IEnumerable<TInput> inputs, params object[] parameters)
            where TAnalyzable: IAnalyzable
        {
            // Get the default constructor for instantiation
            var ctor = typeof(TAnalyzable)
                .GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                .Where(c => c.GetParameters().Any())
                .FirstOrDefault(c => typeof(IEnumerable<TInput>).IsAssignableFrom(c.GetParameters().First().ParameterType));

            if (ctor == null)
            {
                throw new TargetInvocationException("Can't find default constructor for instantiation, please make sure that the analyzable has a constructor with IEnumerable<IOhlcvData> as the first parameter",
                    new ArgumentNullException(nameof(ctor)));
            }

            var @params = new List<object>
            {
                inputs
            };
            for (int i = 1; i < ctor.GetParameters().Count(); i++)
            {
                @params.Add(Convert.ChangeType(parameters[i - 1], ctor.GetParameters()[i].ParameterType));
            }

            return (TAnalyzable)ctor.Invoke(@params.ToArray());
        }

        public static TAnalyzable CreateAnalyzable<TAnalyzable>(IEnumerable<IOhlcv> candles, params object[] parameters)
            where TAnalyzable : IAnalyzable
            => CreateAnalyzable<TAnalyzable, IOhlcv>(candles, parameters);
    }
}
