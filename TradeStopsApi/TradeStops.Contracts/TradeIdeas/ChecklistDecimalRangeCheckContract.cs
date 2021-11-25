namespace TradeStops.Contracts
{
    /// <summary>
    /// Checklist result of decimal range checks.
    /// </summary>
    public class ChecklistDecimalRangeCheckContract : ChecklistBaseCheckContract
    {
        /// <summary>
        /// Check from value
        /// </summary>
        public decimal? FromValue { get; set; }

        /// <summary>
        /// Check to value
        /// </summary>
        public decimal? ToValue { get; set; }

        /// <summary>
        /// Current check value
        /// </summary>
        public decimal? CurrentValue { get; set; }
    }
}
