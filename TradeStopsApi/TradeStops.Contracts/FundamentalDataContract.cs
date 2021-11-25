namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class FundamentalDataContract
    {
        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Enterprise value and FQ.
        /// </summary>
        public long? EnterpriseValueAndFQ { get; set; }

        /// <summary>
        /// Enterprise value EBITD and TTM.
        /// </summary>
        public decimal? EnterpriseValueEBITDAndTTM { get; set; }

        /// <summary>
        /// Enterprise value to revenue.
        /// </summary>
        public decimal? EnterpriseValueToRevenue { get; set; }

        /// <summary>
        /// Company GICS industry name.
        /// </summary>
        public string GICSIndustryName { get; set; }
        
        /// <summary>
        /// Company industry name.
        /// </summary>
        public string IndustryName { get; set; }

        /// <summary>
        /// Last annual EPS.
        /// </summary>
        public decimal? LastAnnualEPS { get; set; }

        /// <summary>
        /// Last annual Net income.
        /// </summary>
        public decimal? LastAnnualNetIncome { get; set; }

        /// <summary>
        /// Last annual revenue.
        /// </summary>
        public decimal? LastAnnualRevenue { get; set; }

        /// <summary>
        /// Last annual total assets.
        /// </summary>
        public decimal? LastAnnualTotalAssets { get; set; }

        /// <summary>
        /// Market Cap.
        /// </summary>
        public decimal? MarketCap { get; set; }

        public int? NumberOfEmployees { get; set; }

        /// <summary>
        /// Price book and FQ.
        /// </summary>
        public decimal? PriceBookAndFQ { get; set; }

        /// <summary>
        /// Price earnings and TTM.
        /// </summary>
        public decimal? PriceEarningsAndTTM { get; set; }

        /// <summary>
        /// Price earnings to growth and TTM.
        /// </summary>
        public decimal? PriceEarningsToGrowthAndTTM { get; set; }

        /// <summary>
        /// Price revenue and TTM.
        /// </summary>
        public decimal? PriceRevenueAndTTM { get; set; }

        /// <summary>
        /// Average Volume.
        /// </summary>
        public decimal? AverageVolume { get; set; }

        /// <summary>
        /// Sector name.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// GICS Sector name.
        /// </summary>
        public string GICSSectorName { get; set; }
        
        /// <summary>
        /// Sector name.
        /// </summary>
        public string SectorName { get; set; }

        /// <summary>
        /// Total shares outstanding.
        /// </summary>
        public long? TotalSharesOutstanding { get; set; }

        /// <summary>
        /// GICS sub sector name.
        /// </summary>
        public string GICSIndustryGroupName { get; set; }

        /// <summary>
        /// GICS sub industry name.
        /// </summary>
        public string GICSSubIndustryName { get; set; }
    }
}
