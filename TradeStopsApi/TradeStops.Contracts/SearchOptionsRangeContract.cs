using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to search options range.
    /// </summary>
    public class SearchOptionsRangeContract
    {
        /// <summary>
        /// Option type.
        /// </summary>
        public OptionTypes? OptionType { get; set; }

        /// <summary>
        /// Expiration month of options.
        /// </summary>
        public int? ExpirationMonth { get; set; }

        /// <summary>
        /// Expiration year of options.
        /// </summary>
        public int? ExpirationYear { get; set; }

        /// <summary>
        /// Minimum Strike price of options including minimum value in search.
        /// </summary>
        public decimal? StrikePriceMin { get; set; }

        /// <summary>
        /// Maximum Strike price of options including maximum value in search.
        /// </summary>
        public decimal? StrikePriceMax { get; set; }
    }
}
