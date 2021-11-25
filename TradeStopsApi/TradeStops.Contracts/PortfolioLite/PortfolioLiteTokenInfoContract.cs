using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Information regarding one-time Portfolio Lite token,
    /// that is used to Authenticate user in Portfolio Lite website.
    /// </summary>
    public class PortfolioLiteTokenInfoContract
    {
        /// <summary>
        /// Portfolio Lite partner ID.
        /// </summary>
        public int PartnerId { get; set; }

        /// <summary>
        /// TradeSmith User ID.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///  TradeSmith User Guid
        /// </summary>
        public Guid UserGuid { get; set; }

        /// <summary>
        /// ID of the portfolio that is used in PortfolioLite for corresponding Partner.
        /// </summary>
        public int PortfolioId { get; set; }
    }
}
