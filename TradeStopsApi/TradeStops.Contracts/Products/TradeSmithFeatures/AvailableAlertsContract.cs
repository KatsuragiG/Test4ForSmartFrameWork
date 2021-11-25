using System.Diagnostics.CodeAnalysis;

namespace TradeStops.Contracts
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements must be documented", Justification = "Temporary suppression")]
    public class AvailableAlertsContract
    {
        public bool PriceTarget { get; set; }
        public bool TimeBased { get; set; }
        public bool Volume { get; set; }
        public bool MovingAverage { get; set; }
        public bool TrailingStops { get; set; }
        public bool OptionCostBasis { get; set; }
        public bool UnderlyingStock { get; set; }
        public bool TimeValueExpiry { get; set; }
        public bool Fundamentals { get; set; }
        public bool Ssi { get; set; }
    }
}
