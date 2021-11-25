using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Result of position size calculation by investment amount
    /// </summary>
    public class PositionSizeByInvestmentContract
    {
        /// <summary>
        /// Investment risk.
        /// </summary>
        public decimal RiskAmount { get; set; }

        /// <summary>
        /// Current VQ value for the symbol.
        /// </summary>
        public decimal VolatilityQuotient { get; set; }

        /// <summary>
        /// Amount of shares you can purchase determined by the InvestmentAmount value.
        /// </summary>
        public decimal NumberOfShares { get; set; }

        /// <summary>
        /// Latest close price of the symbol adjusted by dividends.
        /// </summary>
        public decimal LatestCloseAdjusted { get; set; }

        /// <summary>
        /// SSI values for Long Positions adjusted by dividends.
        /// </summary>
        ////[Obsolete("LongAdjSsi is obsolete. Use SsiValue instead.")]
        public SsiValueContract LongAdjSsi { get; set; }

        /// <summary>
        /// SSI values for Long and Short Positions adjusted by dividends.
        /// </summary>
        public SsiValueContract SsiValue { get; set; }

        /// <summary>
        /// Stop price value.
        /// </summary>
        public decimal StopPrice { get; set; }
    }
}
