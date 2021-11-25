using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class AdminPositionContract
    {
        public int PositionId { get; set; }

        public int PortfolioId { get; set; }

        [Obsolete("This contract should be used for regular positions only")]
        public PositionConfirmationType PositionConfirmationType { get; set; }

        public int SymbolId { get; set; }

        public string Symbol { get; set; }

        public string Name { get; set; }

        public TradeTypes TradeType { get; set; }

        public string Currency { get; set; }

        public string CurrencyName { get; set; }

        public decimal? SplitsAdj { get; set; }

        public decimal? PurchasePrice { get; set; }

        public decimal? PurchasePriceAdj { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public decimal? ClosePrice { get; set; }

        public DateTime? CloseDate { get; set; }

        public decimal? CloseFee { get; set; }

        public decimal? OpenFee { get; set; }

        public PositionStatusTypes? StatusType { get; set; }

        public bool Delisted { get; set; }

        public decimal? Shares { get; set; }

        public bool AdjustByDividends { get; set; }

        public string SymbolType { get; set; }

        public OptionTypes? OptionType { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public decimal? StrikePrice { get; set; }

        public string VendorHoldingId { get; set; }

        public string VendorSymbol { get; set; }
    }
}
