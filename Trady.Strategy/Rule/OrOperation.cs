using System;
using System.Collections.Generic;
using System.Text;

namespace Trady.Strategy.Rule
{
    public class OrOperation<T> : BinaryOperationBase<T> 
    {
        public OrOperation(IRule<T> operand1, IRule<T> operand2) : base(operand1, operand2)
        {
        }

        protected override bool? Operate(bool operand1Value, bool? operand2Value = default(bool?))
        {
            if (operand1Value)
                return true;
            else if (operand2Value == null)
                return null;
            else
                return operand1Value || operand2Value.Value;
        }
    }
}
