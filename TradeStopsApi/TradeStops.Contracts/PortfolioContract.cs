using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Portfolio
    /// </summary>
    public class PortfolioContract
    {
        /// <summary>
        /// Unique portfolio ID.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Portfolio name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User's notes for the current portfolio.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Additional portfolio cash.
        /// </summary>
        public decimal Cash { get; set; }

        /// <summary>
        /// PortfolioTypes value in the in TradeStops API.
        /// </summary>
        public PortfolioTypes Type { get; set; }

        /// <summary>
        /// Property to identify whether the portfolio has been deleted by the user (soft delete) or not.
        /// </summary>
        public bool Delisted { get; set; }

        /// <summary>
        /// ID of the portfolio currency.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Property to identify the default portfolio for re-entry alerts.
        /// </summary>
        public bool IsReEntryDefault { get; set; }

        /// <summary>
        /// Portfolio entry commission.
        /// </summary>
        public decimal EntryCommission { get; set; }

        /// <summary>
        /// Portfolio exit commission.
        /// </summary>
        public decimal ExitCommission { get; set; }

        /// <summary>
        /// Internal TradeStops value.
        /// </summary>
        public bool HideHistory { get; set; } // we don't need it or need only internally

        /// <summary>
        /// Flag indicating whether to use the cross сourse for commissions
        /// </summary>
        public bool UseCrossCourseForCommission { get; set; }

        /// <summary>
        /// Portfolio creation date.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// All fields below are applicable only for synchronized portfolios.
        /// </summary>
        public ImportedPortfolioFields ImportedPortfolio { get; set; }
    }
}
