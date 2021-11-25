namespace TradeStops.Contracts
{
    /// <summary>
    /// Checklist checks results
    /// </summary>
    public class ApplyChecklistResultContract
    {
        /// <summary>
        /// Option Implied Volatility Rank check result
        /// </summary>
        public ChecklistDecimalRangeCheckContract ImpliedVolatilityRank { get; set; }

        /// <summary>
        /// Option Bid - Ask check result
        /// </summary>
        public ChecklistDecimalRangeCheckContract BidAskSpread { get; set; }

        /// <summary>
        /// Option Open Interest check result
        /// </summary>
        public ChecklistDecimalRangeCheckContract OpenInterest { get; set; }

        /// <summary>
        /// Earnings Before Expiration check result
        /// </summary>
        public ChecklistBoolCheckContract EarningsBeforeExpiration { get; set; }

        /// <summary>
        /// Dividends Before Expiration check result
        /// </summary>
        public ChecklistBoolCheckContract DividendsBeforeExpiration { get; set; }

        /// <summary>
        /// Days to Expiration check result
        /// </summary>
        public ChecklistDecimalRangeCheckContract DaysToExpiration { get; set; }

        /// <summary>
        /// Probability of Profit check result
        /// </summary>
        public ChecklistDecimalRangeCheckContract ProbabilityOfProfit { get; set; }

        /// <summary>
        /// Return on Investment (ROI) check result
        /// </summary>
        public ChecklistDecimalRangeCheckContract ReturnOnInvestment { get; set; }

        /// <summary>
        /// Volume check result
        /// </summary>
        public ChecklistDecimalRangeCheckContract Volume { get; set; }

        /// <summary>
        /// Implied Volatility (IV) check result
        /// </summary>
        public ChecklistDecimalRangeCheckContract ImpliedVolatility { get; set; }

        /// <summary>
        /// Implied Volatility (IV) Percentile check result
        /// </summary>
        public ChecklistDecimalRangeCheckContract ImpliedVolatilityPercentile { get; set; }

        /// <summary>
        /// Bid / Ask Ratio check result
        /// </summary>
        public ChecklistDecimalRangeCheckContract BidAskRatio { get; set; }

        /// <summary>
        /// Max Profit check result
        /// </summary>
        public ChecklistDecimalRangeCheckContract MaxProfit { get; set; }

        /// <summary>
        /// Max Loss check result
        /// </summary>
        public ChecklistDecimalRangeCheckContract MaxLoss { get; set; }

        /// <summary>
        /// Moneyness check result
        /// </summary>
        public ChecklistDecimalRangeCheckContract Moneyness { get; set; }

        /// <summary>
        /// Delta check result
        /// </summary>
        public ChecklistDecimalRangeCheckContract Delta { get; set; }

        /// <summary>
        /// Theta check result
        /// </summary>
        public ChecklistDecimalRangeCheckContract Theta { get; set; }

        /// <summary>
        /// Gamma check result
        /// </summary>
        public ChecklistDecimalRangeCheckContract Gamma { get; set; }

        /// <summary>
        /// Vega check result
        /// </summary>
        public ChecklistDecimalRangeCheckContract Vega { get; set; }

        /// <summary>
        /// Rho check result
        /// </summary>
        public ChecklistDecimalRangeCheckContract Rho { get; set; }

        /// <summary>
        /// Historical Volatility check result
        /// </summary>
        public ChecklistDecimalRangeCheckContract HistoricalVolatility { get; set; }

        /// <summary>
        /// Volatility Ratio check result
        /// </summary>
        public ChecklistDecimalRangeCheckContract VolatilityRatio { get; set; }
    }
}
