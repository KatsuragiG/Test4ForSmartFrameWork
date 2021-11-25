using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Pure Quant sources parameters.
    /// </summary>
    public class PureQuantSourcesInputContract
    {
        /// <summary>
        /// Specifies how to use multiple sources.
        /// </summary>
        public UnionTypes SourcesUnionType { get; set; }

        /// <summary>
        /// Newsletters portfolios sources.
        /// </summary>
        public List<PureQuantPortfolioInputContract> Newsletters { get; set; }

        /// <summary>
        /// Sector source.
        /// </summary>
        public List<PureQuantSourceEntityInputContract> Sectors { get; set; }

        /// <summary>
        /// Basket types sources.
        /// </summary>
        [Obsolete("Use Baskets property. Left for compatibility with old PQ results.")]
        public List<BasketTypes> BasketTypes { get; set; }

        /// <summary>
        /// Basket sources
        /// </summary>
        public List<PureQuantSourceEntityInputContract> Baskets { get; set; }

        /// <summary>
        /// Countries of exchange source.
        /// </summary>
        public List<ExchangeCountryTypes> ExchangeCountries { get; set; }

        /// <summary>
        /// List of Symbol Group IDs source.
        /// </summary>
        public List<SymbolGroupTypes> SymbolGroupIds { get; set; }

        /// <summary>
        /// User individual securities source.
        /// </summary>
        public List<PureQuantSourceEntityInputContract> IndividualSecurities { get; set; }

        /// <summary>
        /// Bypass Pure Quant filter for individual securities and always include them in the result.
        /// </summary>
        public bool BypassPureQuantFilterForIndividualSecurities { get; set; }

        /// <summary>
        /// User portfolio sources.
        /// </summary>
        public List<PureQuantPortfolioInputContract> UserPortfolios { get; set; }

        /// <summary>
        /// Additional ETFs and Mutual Fund symbols.
        /// </summary>
        public List<PureQuantSourceEntityInputContract> AdditionalFundSymbols { get; set; }

        /// <summary>
        /// Include all Fund symbol components.
        /// </summary>
        public bool IncludeFundComponents { get; set; }

        /// <summary>
        /// Ideas saved search source.
        /// </summary>
        public PureQuantSourceEntityInputContract IdeasSavedSearch { get; set; }
    }
}
