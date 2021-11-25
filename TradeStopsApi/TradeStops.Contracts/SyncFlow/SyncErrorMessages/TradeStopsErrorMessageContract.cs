namespace TradeStops.Contracts
{
    /// <summary>
    /// Yodlee TradeStops error message
    /// </summary>
    public class TradeStopsErrorMessageContract
    {
        /// <summary>
        ///  Tradestops error title for yodlee error.
        /// </summary>
        public string ErrorTitle { get; set; }

        /// <summary>
        ///  Tradestops error message for yodlee error.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
