using System;
using System.Collections.Generic;
using System.Text;

namespace Trady.Strategy.Rule
{
    public class Rule<T> : IRule<T>
    {
        private Func<T, int, bool> _predicate;

        public Rule(bool boolValue)
        {
            _predicate = new Func<T, int, bool>((t, i) => boolValue);
        }

        public Rule(Func<T, int, bool> predicate)
        {
            _predicate = predicate;
        }

        public Rule(IOperation<T> @operator)
        {
            _predicate = new Func<T, int, bool>((t, i) => @operator.Operate(t, i).IsValid(t, i));
        }

        public bool IsValid(T obj, int index) => _predicate.Invoke(obj, index);
    }
}
