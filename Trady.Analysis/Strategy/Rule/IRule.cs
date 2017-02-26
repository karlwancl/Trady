namespace Trady.Analysis.Strategy.Rule
{
    public interface IRule<T>
    {
        bool IsValid(T obj);
    }
}