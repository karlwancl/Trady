namespace Trady.Core
{
    public interface IFixedTimeSeries<TTick> : ITimeSeries<TTick> where TTick: ITick
    {
        int MaxTickCount { get; }
    }
}
