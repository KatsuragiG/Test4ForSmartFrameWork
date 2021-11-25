using System;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class PositionStatsContract
    {
        public int PositionId { get; set; }

        public DateTime? HighestCloseDate { get; set; }
        public decimal? HighestClosePrice { get; set; }

        public DateTime? HighestHighDate { get; set; }
        public decimal? HighestHighPrice { get; set; }

        public DateTime? LowestCloseDate { get; set; }
        public decimal? LowestClosePrice { get; set; }

        public DateTime? LowestLowDate { get; set; }
        public decimal? LowestLowPrice { get; set; }

        public decimal? MaxGainPercent { get; set; }
        public decimal? MinGainPercent { get; set; }

        public decimal? MovingAvg10 { get; set; }
        public decimal? MovingAvg200 { get; set; }
        public decimal? MovingAvg50 { get; set; }

        public DateTime? HighestIntradayDate { get; set; }
        public decimal? HighestIntradayPrice { get; set; }

        public DateTime? LowestIntradayDate { get; set; }
        public decimal? LowestIntradayPrice { get; set; }
    }
}
