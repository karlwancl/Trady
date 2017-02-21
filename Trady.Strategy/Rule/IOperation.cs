namespace Trady.Strategy.Rule
{
    public interface IOperation<T>
    {
        IRule<T> Operate(T obj);
    }
}