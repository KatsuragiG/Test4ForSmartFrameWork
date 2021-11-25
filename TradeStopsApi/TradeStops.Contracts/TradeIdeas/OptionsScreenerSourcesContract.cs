using System.Collections.Generic;

using TradeStops.Common.DataStructures;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to specify sources that will be used to search for options.
    /// </summary>
    public class OptionsScreenerSourcesContract
    {
        /// <summary>
        /// Include all active symbols from the following portfolios.
        /// </summary>
        public List<int> PortfolioIds { get; set; }

        /// <summary>
        /// Include all active symbols from the following newsletters.
        /// </summary>
        public List<NewslettersPortfolioKey> Newsletters { get; set; }

        /// <summary>
        /// Include all active symbols from the following sectors.
        /// </summary>
        public List<int> SectorIds { get; set; }

        /// <summary>
        /// Include all active symbols from the following sub sectors.
        /// </summary>
        public List<int> SubSectorIds { get; set; }

        /// <summary>
        /// Include all active symbols from the following industries.
        /// </summary>
        public List<int> IndustryIds { get; set; }

        /// <summary>
        ///  Include all active symbols from the following sub industries.
        /// </summary>
        public List<int> SubIndustryIds { get; set; }

        /// <summary>
        /// Include all active symbols from the following symbol groups.
        /// </summary>
        public List<SymbolGroupTypes> SymbolGroupIds { get; set; }

        /// <summary>
        /// Include all symbols from the following baskets.
        /// </summary>
        public List<int> BasketIds { get; set; }

        /// <summary>
        /// Include provided symbols.
        /// </summary>
        public List<int> IndividualSecurityIds { get; set; }

        /// <summary>
        /// Use stock symbols from all provided sources as input for screener.
        /// </summary>
        public bool IncludeStocksFromSources { get; set; }

        /// <summary>
        /// Use option symbols from all provided sources as input for screener.
        /// </summary>
        public bool IncludeOptionsFromSources { get; set; }
    }
}
