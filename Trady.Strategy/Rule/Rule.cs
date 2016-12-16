using System;
using System.Collections.Generic;
using System.Text;

namespace Trady.Strategy.Rule
{
    public class Rule<T> : IRule<T>
    {
        private Predicate<T> _predicate;

        public Rule(bool boolValue)
        {
            _predicate = new Predicate<T>(t => boolValue);
        }

        public Rule(Predicate<T> predicate)
        {
            _predicate = predicate;
        }

        public Rule(IOperation<T> @operator)
        {
            _predicate = new Predicate<T>(t => @operator.Operate(t).IsValid(t));
        }

        public bool IsValid(T obj) => _predicate.Invoke(obj);
    }
}
