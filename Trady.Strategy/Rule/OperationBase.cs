using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trady.Strategy.Rule
{
    public abstract class OperationBase<T> : IOperation<T>
    {
        private IRule<T>[] _operands;

        protected OperationBase(params IRule<T>[] operands)
        {
            _operands = operands ?? throw new ArgumentNullException(nameof(operands));
        }

        protected IReadOnlyList<IRule<T>> Operands => _operands;

        public abstract IRule<T> Operate(T obj);
    }
}
