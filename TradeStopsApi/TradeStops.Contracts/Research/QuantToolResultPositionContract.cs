namespace TradeStops.Contracts
{
    /// <summary>
    /// Quant Tool result position
    /// </summary>
    public class QuantToolResultPositionContract
    {
        /// <summary>
        /// ID of the symbol.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Symbol (ticker).
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Company name.
        /// </summary>
        public string SymbolName { get; set; }

        /// <summary>
        /// Latest price of the symbol.
        /// </summary>
        public decimal LatestPrice { get; set; }

        /// <summary>
        /// Position VQ value.
        /// </summary>
        public decimal Vq { get; set; }

        /// <summary>
        /// Average VQ value fot 30 years
        /// </summary>
        public decimal Average30YearsVolatilityQuotient { get; set; }

        /// <summary>
        /// ID of the currency.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Symbol currency.
        /// </summary>
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// Recommended size share to take an equal risk per position.
        /// </summary>
        public decimal SuggestedShares { get; set; }

        /// <summary>
        /// Amount of money invested in each position of the potfolio.
        /// </summary>
        public decimal PositionSize { get; set; }

        /// <summary>
        /// Percentage of each position in the portfolio.
        /// </summary>
        public decimal PositionSizePercent { get; set; }

        /// <summary>
        /// Percentage amount of risk in the individual position compared to the value of the overall portfolio.
        /// </summary>
        public decimal PositionRisk { get; set; }

        /// <summary>
        /// Position has been included into the Risk Rebalancer Algorithm.
        /// </summary>
        public bool RebalancedPosition { get; set; }

        /// <summary>
        /// SSI values for Long adjested or unadjusted by dividends positions based on the AdjustByDividends value in the request.
        /// </summary>
        public SsiValueContract SsiValue { get; set; }

        /// <summary>
        /// Rank of the position in Quant Tool.
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// Company sector name.
        /// </summary>
        public string SectorName { get; set; }

        /// <summary>
        /// Is additional position.
        /// </summary>
        public bool IsAdditionalPosition { get; set; }
    }
}
