using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to edit portfolio tracker portfolio
    /// </summary>
    public class EditPortfolioTrackerPortfolioContract
    {
        /// <summary>
        /// (Optional) New portfolio name.
        /// </summary>
        public Optional<string> Name { get; set; }

        /// <summary>
        /// (Optional) New id of PortfoliosGroup this portfolio must be assigned to.
        /// </summary>
        public Optional<int?> PortfolioGroupId { get; set; }

        /// <summary>
        /// (Optional) Indicates whether portfolio should be published or not.
        /// </summary>
        public Optional<bool> Published { get; set; }

        /// <summary>
        /// (Optional) New Guru's name.
        /// </summary>
        public Optional<string> Guru { get; set; }

        /// <summary>
        /// (Optional) New notes for the portfolio.
        /// </summary>
        public Optional<string> Notes { get; set; }

        /// <summary>
        /// (Optional) New value for portfolio pub codes.
        /// </summary>
        public Optional<string> PubCodes { get; set; }

        /// <summary>
        ///  (Optional) New value of the portfolio cash.
        /// </summary>
        public Optional<decimal> Cash { get; set; }

        /// <summary>
        /// (Optional) New Id of the portfolio currency.
        /// </summary>
        public Optional<int> CurrencyId { get; set; }
    }
}
