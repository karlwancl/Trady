namespace Trady.Core.Infrastructure
{
    public interface IOhlcv : ITick
    {
        decimal Open { get; set; }

        decimal High { get; set; }

        decimal Low { get; set; }

        decimal Close { get; set; }

        decimal Volume { get; set; }
    }
}
