namespace Trady.Analysis.Strategy.Rule
{
    public class AndOperation<T> : BinaryOperationBase<T>
    {
        public AndOperation(IRule<T> operand1, IRule<T> operand2) : base(operand1, operand2)
        {
        }

        protected override bool? Operate(bool operand1Value, bool? operand2Value = default(bool?))
        {
            if (!operand1Value)
                return false;
            else if (operand2Value == null)
                return null;
            else
                return operand1Value & operand2Value.Value;
        }
    }
}