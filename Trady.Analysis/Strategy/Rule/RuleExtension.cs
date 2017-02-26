using System;

namespace Trady.Analysis.Strategy.Rule
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
        public static IRule<IndexCandle> Create(this bool value)
            => new Rule<IndexCandle>(value);

        public static IRule<IndexCandle> Create(this Predicate<IndexCandle> predicate)
            => new Rule<IndexCandle>(predicate);

        public static IRule<IndexCandle> Create(this IOperation<IndexCandle> @operator)
            => new Rule<IndexCandle>(@operator);
    }
}