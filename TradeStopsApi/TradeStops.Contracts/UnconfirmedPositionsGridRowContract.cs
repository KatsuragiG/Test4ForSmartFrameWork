using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class UnconfirmedPositionsGridRowContract
    {
        public int UnconfirmedPositionId { get; set; }

        public decimal? PurchasePrice { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public decimal? Shares { get; set; }

        public TradeTypes TradeType { get; set; }

        public decimal? ClosePrice { get; set; }

        public decimal? CostBasis { get; set; }

        public decimal? OpenFee { get; set; }

        public int PortfolioId { get; set; }

        public string PortfolioName { get; set; }

        public decimal PortfolioCommission { get; set; }

        public bool IsSyncPortfolio { get; set; }

        public bool IsInvestmentPortfolio { get; set; }

        public string VendorHoldingId { get; set; }

        public string ParsedHoldingSymbol { get; set; }

        //TODO for Pavel: this property can be removed. VendorSymbol has the same value
        public string CustomSymbolName { get; set; }

        public string DataType { get; set; }

        public bool ChangingSharesPermission { get; set; }

        public string VendorSymbol { get; set; }

        public bool? IsCombined { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public decimal? StrikePrice { get; set; }

        public OptionTypes? OptionType { get; set; }
        
        public bool IsPossiblyClosed { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsNew { get; set; }

        public List<PositionGridIssueTypes> PositionIssues { get; set; }
    }
}
