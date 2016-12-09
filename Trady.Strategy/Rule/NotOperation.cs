using System;
using System.Collections.Generic;
using System.Text;

namespace Trady.Strategy.Rule
{
    public class NotOperation<T> : UnaryOperationBase<T>
    {
        public NotOperation(IRule<T> operand) : base(operand)
        {
        }

        protected override bool Operate(bool operandValue)
            => !operandValue;
    }
}
