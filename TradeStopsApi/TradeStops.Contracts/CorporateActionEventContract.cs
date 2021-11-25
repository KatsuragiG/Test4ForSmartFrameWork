using System;
using System.Diagnostics.CodeAnalysis;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class CorporateActionEventContract
    {
        public HDEventTypes EventTypeId { get; set; }
        public DateTime EventDate { get; set; }
        public int SymbolId { get; set; }

        public string Description { get; set; }
        public string AdditionalDescription { get; set; }

        public SystemEventCategories EventCategory { get; set; }

        public string Symbol { get; set; }
        public string SymbolName { get; set; }
        public DateTime? SplitTradeDate { get; set; }
        public decimal? NewShares { get; set; }
        public decimal? OldShares { get; set; }
        public DateTime? DividendTradeDate { get; set; }
        public decimal? DividendAmount { get; set; }
        public DateTime? DividendPayDate { get; set; }
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime CloseDate { get; set; }
        public int PortfolioId { get; set; }
        public string PortfolioName { get; set; }
        public PortfolioTypes PortfolioType { get; set; }
        public bool PortfolioDelisted { get; set; }
        public int UserId { get; set; }
        public string CurrencySymbol { get; set; }
        public decimal? SpinOffAmount { get; set; }
        public int? SpinOffDistributedSymbolId { get; set; }
        public decimal? StockDistributionAmount { get; set; }
    }
}