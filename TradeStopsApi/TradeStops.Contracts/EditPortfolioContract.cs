using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with portfolio fields to patch
    /// </summary>
    public class EditPortfolioContract
    {
        /// <summary>
        /// (optional) New portfolio name
        /// </summary>
        public Optional<string> Name { get; set; }

        /// <summary>
        /// (optional) New notes for the portfolio.
        /// </summary>
        public Optional<string> Notes { get; set; }

        /// <summary>
        /// (optional) New portfolio cash value.
        /// </summary>
        public Optional<decimal> Cash { get; set; }

        /// <summary>
        /// (optional) New type of the portfolio.
        /// </summary>
        public Optional<PortfolioTypes> Type { get; set; }

        /// <summary>
        /// (optional) Remove the portfolio from the account (soft delete).
        /// </summary>
        public Optional<bool> Delisted { get; set; }

        /// <summary>
        /// (optional) New currency identifier for the portfolio.
        /// </summary>
        public Optional<int> CurrencyId { get; set; }

        /// <summary>
        /// (optional) Portfolio entry commission.
        /// </summary>
        public Optional<decimal> EntryCommission { get; set; }

        /// <summary>
        /// (optional) Portfolio exit commission.
        /// </summary>
        public Optional<decimal> ExitCommission { get; set; }

        /// <summary>
        /// (optional) Flag indicating whether to use the cross сourse for commissions
        /// </summary>
        public Optional<bool> UseCrossCourseForCommission { get; set; }
    }
}
