namespace Trady.Strategy.Rule
{
    public interface IRule<T>
    {
        bool IsValid(T obj);
    }
}