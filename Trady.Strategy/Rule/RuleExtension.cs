using System;
using System.Collections.Generic;
using System.Text;

namespace Trady.Strategy.Rule
{
    public static class RuleExtension
    {
        public static IRule<T> Not<T>(this IRule<T> rule)
            => new Rule<T>(new NotOperation<T>(rule));

        public static IRule<T> And<T>(this IRule<T> rule, bool value)
            => new Rule<T>(new AndOperation<T>(rule, new Rule<T>(value)));

        public static IRule<T> And<T>(this IRule<T> rule, Predicate<T> predicate)
            => new Rule<T>(new AndOperation<T>(rule, new Rule<T>(predicate)));

        public static IRule<T> And<T>(this IRule<T> rule, IOperation<T> @operator)
            => new Rule<T>(new AndOperation<T>(rule, new Rule<T>(@operator)));

        public static IRule<T> And<T>(this IRule<T> rule1, IRule<T> rule2)
            => new Rule<T>(new AndOperation<T>(rule1, rule2));

        public static IRule<T> Or<T>(this IRule<T> rule, bool value)
            => new Rule<T>(new OrOperation<T>(rule, new Rule<T>(value)));

        public static IRule<T> Or<T>(this IRule<T> rule, Predicate<T> predicate)
            => new Rule<T>(new OrOperation<T>(rule, new Rule<T>(predicate)));

        public static IRule<T> Or<T>(this IRule<T> rule, IOperation<T> @operator)
            => new Rule<T>(new OrOperation<T>(rule, new Rule<T>(@operator)));

        public static IRule<T> Or<T>(this IRule<T> rule1, IRule<T> rule2)
            => new Rule<T>(new OrOperation<T>(rule1, rule2));
    }

    public static class Rule
    {
        public static IRule<ComputableCandle> Create(this bool value)
            => new Rule<ComputableCandle>(value);

        public static IRule<ComputableCandle> Create(this Predicate<ComputableCandle> predicate)
            => new Rule<ComputableCandle>(predicate);

        public static IRule<ComputableCandle> Create(this IOperation<ComputableCandle> @operator)
            => new Rule<ComputableCandle>(@operator);
    }
}
