using System;
using System.Diagnostics.CodeAnalysis;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Temporary")]
    public class BacktesterSubtaskResultTradeContract
    {
        public SymbolContract Symbol { get; set; }
        public TradeTypes? TradeType { get; set; }
        public DateTime? EntryDate { get; set; }
        public decimal? EntryPrice { get; set; }
        public DateTime? ExitDate { get; set; }
        public decimal? ExitPrice { get; set; }
        public decimal? AbsoluteProfit { get; set; } // $ Gain
        public decimal? PercentProfit { get; set; } // % Profit
        public decimal? Shares { get; set; }
        public decimal? PositionValue { get; set; }
        public int? DaysHeld { get; set; } // # of bars
    }
}
