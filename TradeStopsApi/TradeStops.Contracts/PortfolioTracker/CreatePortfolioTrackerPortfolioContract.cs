using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create portfolio tracker portfolio
    /// </summary>
    public class CreatePortfolioTrackerPortfolioContract
    {
        /// <summary>
        /// Portfolio name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// (Optional) Id of PortfoliosGroup this portfolio must be assigned to.
        /// </summary>
        public int? PortfolioGroupId { get; set; }

        /// <summary>
        /// Guru's name.
        /// </summary>
        public string Guru { get; set; }

        /// <summary>
        /// Notes for the portfolio.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Portfolio pub codes.
        /// </summary>
        public string PubCodes { get; set; }

        /// <summary>
        ///  Value of the portfolio cash.
        /// </summary>
        public decimal Cash { get; set; }

        /// <summary>
        /// Id of the portfolio currency.
        /// </summary>
        public int CurrencyId { get; set; }
    }
}
