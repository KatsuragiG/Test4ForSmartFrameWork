using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create portfolio
    /// </summary>
    public class CreatePortfolioContract
    {
        /// <summary>
        /// Portfolio name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// (optional) Notes for the portfolio.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        ///  (optional) Value of the portfolio cash.
        /// </summary>
        public decimal Cash { get; set; }

        /// <summary>
        /// (optional) Type of the portfolio.
        /// </summary>
        public PortfolioTypes Type { get; set; }

        /// <summary>
        /// ID of the portfolio currency.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// (optional) Portfolio entry commission.
        /// </summary>
        public decimal EntryCommission { get; set; }

        /// <summary>
        /// (optional) Portfolio exit commission.
        /// </summary>
        public decimal ExitCommission { get; set; }

        /// <summary>
        /// (optional) Determines if unique portfolio name must be generated if portfolio with the same name already exists; otherwise validation exception will be thrown
        /// </summary>
        public bool GenerateUniqueName { get; set; }

        /// <summary>
        /// (optional) Flag indicating whether to use the cross сourse for commissions
        /// </summary>
        public Optional<bool> UseCrossCourseForCommission { get; set; }
    }
}
