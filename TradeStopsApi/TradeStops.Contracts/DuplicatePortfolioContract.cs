using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to duplicate portfolio
    /// </summary>
    public class DuplicatePortfolioContract
    {
        /// <summary>
        ///  Id of the portfolio to be duplicated.
        /// </summary>
        public int OriginalPortfolioId { get; set; }

        /// <summary>
        /// Name of the new portfolio.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// (optional) User's notes for the new Portfolio.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// (optional) Additional portfolio cash.
        /// </summary>
        public decimal Cash { get; set; }

        /// <summary>
        /// PortfolioTypes value in TradeStops API
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
        /// Flag indicating whether to use the cross сourse for commissions
        /// </summary>
        public bool UseCrossCourseForCommission { get; set; }
    }
}
