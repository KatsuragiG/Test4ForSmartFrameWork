using System;
using System.Collections.Generic;
using System.Text;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Authentication result for Portfolio Lite
    /// </summary>
    public class PortfolioLiteAuthenticationResultContract
    {
        /// <summary>
        ///  TradeSmith User Guid
        /// </summary>
        public Guid UserGuid { get; set; }

        /// <summary>
        /// TradeSmith User Id
        /// </summary>
        public int TradeSmithUserId { get; set; }

        /// <summary>
        /// TradeSmith portfolio lite partner Id
        /// </summary>
        public int PartnerId { get; set; }

        /// <summary>
        /// User's portfolio Id
        /// </summary>
        public int PortfolioId { get; set; }
    }
}
