using System.Collections.Generic;
using TradeStops.Common.DataStructures;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Pure Quant sources parameters to create task.
    /// </summary>
    public class CreatePureQuantSourcesContract
    {
        /// <summary>
        /// Specifies how to use multiple sources.
        /// </summary>
        public UnionTypes SourcesUnionType { get; set; }

        /// <summary>
        /// Newsletters portfolios sources.
        /// </summary>
        public List<NewslettersPortfolioKey> Newsletters { get; set; }

        /// <summary>
        /// Sector ids source.
        /// </summary>
        public List<int> SectorsIds { get; set; }

        /// <summary>
        /// Basket ids source.
        /// </summary>
        public List<int> BasketIds { get; set; }

        /// <summary>
        /// Countries of exchange source.
        /// </summary>
        public List<ExchangeCountryTypes> ExchangeCountries { get; set; }

        /// <summary>
        /// List of Symbol Group IDs source.
        /// </summary>
        public List<SymbolGroupTypes> SymbolGroupIds { get; set; }

        /// <summary>
        /// User individual securities ids source.
        /// </summary>
        public List<int> IndividualSecurityIds { get; set; }

        /// <summary>
        /// Bypass Pure Quant filter for individual securities and always include them in the result.
        /// </summary>
        public bool BypassPureQuantFilterForIndividualSecurities { get; set; }

        /// <summary>
        /// User portfolio ids sources.
        /// </summary>
        public List<int> UserPortfolios { get; set; }

        /// <summary>
        /// Additional ETFs and Mutual Fund symbol ids.
        /// </summary>
        public List<int> AdditionalFundSymbolIds { get; set; }

        /// <summary>
        /// Include all Fund symbol components.
        /// </summary>
        public bool IncludeFundComponents { get; set; }

        /// <summary>
        /// Ideas saved search id source.
        /// </summary>
        public int? IdeasSavedSearchId { get; set; }
    }
}
