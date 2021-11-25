using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Portfolio tracker portfolio
    /// </summary>
    public class PortfolioTrackerPortfolioContract
    {
        /// <summary>
        /// Unique portfolio Id.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// UTC date and time when portfolio was created.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Id of organization this portfolio belongs to.
        /// </summary>
        public PortfolioTrackerOrganizations OrganizationId { get; set; }

        /// <summary>
        /// Portfolio name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Id of portfolios group this portfolio belongs to.
        /// </summary>
        public int? PortfolioGroupId { get; set; }

        /// <summary>
        /// Indicates whether this portfolio is published in TradeSmith Gurus center or not.
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Guru name
        /// </summary>
        public string Guru { get; set; }

        /// <summary>
        /// Notes
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Portfolio pub codes.
        /// </summary>
        public string PubCodes { get; set; }

        /// <summary>
        /// Id of the portfolio currency.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Additional portfolio cash.
        /// </summary>
        public decimal Cash { get; set; }

        /// <summary>
        /// Identifies whether the portfolio has been deleted by the user (soft delete) or not.
        /// </summary>
        public bool Delisted { get; set; }
    }
}
