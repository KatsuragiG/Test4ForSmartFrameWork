using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TradeStops.Common.DataStructures;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Todo: remove suppression")]
    public class StockFinderInputContract
    {
        /// <summary>
        /// Number of symbols to return
        /// </summary>
        public int? NumberOfSymbolsToReturn { get; set; }

        /// <summary>
        /// Field to perform sort operation
        /// </summary>
        public string OrderByField { get; set; }

        /// <summary>
        /// Sort type, ascending or descending
        /// </summary>
        public OrderTypes OrderType { get; set; }

        /// <summary>
        /// List of ssi statuses.
        /// </summary>
        public List<SsiStatuses> SsiTypes { get; set; }

        /// <summary>
        /// TrendTypes List of trend types.
        /// </summary>
        public List<TrendTypes> TrendTypes { get; set; }

        /// <summary>
        /// List of sector IDs
        /// </summary>
        public List<int> SectorIds { get; set; }

        /// <summary>
        /// List of subsector IDs
        /// </summary>
        public List<int> SubSectorIds { get; set; }

        /// <summary>
        /// List of industry IDs
        /// </summary>
        public List<int> IndustryIds { get; set; }

        /// <summary>
        /// List of subindustry IDs
        /// </summary>
        public List<int> SubIndustryIds { get; set; }

        /// <summary>
        /// List of market types. 
        /// </summary>
        public List<MarketCapTypes> MarketCapTypes { get; set; }

        /// <summary>
        /// List of currency IDs
        /// </summary>
        public List<int> CurrencyIds { get; set; }

        /// <summary>
        /// List of strategies to apply as filter
        /// </summary>
        public List<InvestmentStrategyTypes> StrategyIds { get; set; }

        /// <summary>
        /// Specifies how to apply multiple Strategies in the filter
        /// </summary>
        public UnionTypes StrategiesUnionType { get; set; }

        /// <summary>
        /// Search for symbols that have qualified for selected strategies since provided date.
        /// </summary>
        public DateTimeFilter QualifiedForStrategiesFilter { get; set; }

        /// <summary>
        /// List for symbol groups to apply as filter
        /// </summary>
        public List<SymbolGroupTypes> SymbolGroupIds { get; set; }

        /// <summary>
        /// List for countries to apply as filter
        /// </summary>
        public List<ExchangeCountryTypes> ExchangeCountryIds { get; set; }

        /// <summary>
        /// List of timing turn strength to apply as filter
        /// </summary>
        public List<TimingTurnStrengthTypes> TurnStrength { get; set; }

        /// <summary>
        /// List of timing turn areas to apply as filter
        /// </summary>
        public List<TimingTurnAreaTypes> TurnAreaTypes { get; set; }

        /// <summary>
        /// List of timing series to apply as filter
        /// </summary>
        public List<TimingSerieTypes> TimingSerieTypes { get; set; }

        /// <summary>
        /// List of basket ids to apply as filter
        /// </summary>
        public List<int> BasketIds { get; set; }

        /// <summary>
        /// Filter by VQ value.
        /// </summary>
        public DecimalFilter VqFilter { get; set; }

        /// <summary>
        /// Filter by Price/Earnings.
        /// </summary>
        public DecimalFilter PriceEarningsFilter { get; set; }

        /// <summary>
        /// Filter by Latest Close Price.
        /// </summary>
        public DecimalFilter LatestCloseFilter { get; set; }

        /// <summary>
        /// Filter by Dividend Yield.
        /// </summary>
        public DecimalFilter DividendYieldFilter { get; set; }

        /// <summary>
        /// Filter by VQ Ratio.
        /// </summary>
        public DecimalFilter VqRatioFilter { get; set; }

        /// <summary>
        /// Filter by Entry Date
        /// </summary>
        public DateTimeFilter EntryDateFilter { get; set; }

        /// <summary>
        /// Filter by Early Entry Date
        /// </summary>
        public DateTimeFilter EarlyEntryDateFilter { get; set; }

        public StockFinderSourceTypes SourceType { get; set; }

        /// <summary>
        /// List of Newsletter Portfolios to apply as filter
        /// </summary>
        public List<NewslettersPortfolioKey> NewsletterPortfolioKeys { get; set; }

        /// <summary>
        /// Inficates whether Ideas Filter should be applied to results or not. 
        /// </summary>
        public bool EnableIdeasFilter { get; set; }

        /// <summary>
        /// List of Asset types to apply as filter
        /// </summary>
        [Obsolete("2020-11-18. Use SymbolTypes instead.")]
        public List<AssetTypes> AssetTypes { get; set; }
        
        /// <summary>
        /// List of Symbol types to apply filter
        /// </summary>
        public List<SymbolTypes> SymbolTypes { get; set; }

        /// <summary>
        /// Filter by Revenue
        /// </summary>
        public DecimalFilter RevenueFilter { get; set; }

        /// <summary>
        /// Filter by Net Income
        /// </summary>
        public DecimalFilter NetIncomeFilter { get; set; }

        /// <summary>
        /// Filter by EPS
        /// </summary>
        public DecimalFilter EpsFilter { get; set; }

        /// <summary>
        /// Filter by Total Access
        /// </summary>
        public DecimalFilter TotalAccessFilter { get; set; }

        /// <summary>
        /// Filter by Shares Outstanding
        /// </summary>
        public DecimalFilter SharesOutstandingFilter { get; set; }

        /// <summary>
        /// Filter by Enterprice Value
        /// </summary>
        public DecimalFilter EnterpriseValueFilter { get; set; }

        /// <summary>
        /// Filter by Price/Book
        /// </summary>
        public DecimalFilter PriceBookFilter { get; set; }

        /// <summary>
        /// Filter by Price/EarningsToGrowth
        /// </summary>
        public DecimalFilter PriceEarningsToGrowthFilter { get; set; }

        /// <summary>
        /// Filter by Enterprice/EBITDA
        /// </summary>
        public DecimalFilter EnterpriseEbitdaFilter { get; set; }

        /// <summary>
        /// Filter by EBITDA
        /// </summary>
        public DecimalFilter EbitdaFilter { get; set; }

        /// <summary>
        /// Filter by Enterprice/Revenue
        /// </summary>
        public DecimalFilter EnterpriseRevenueFilter { get; set; }

        /// <summary>
        /// Filter by percentage changes per day
        /// </summary>
        public DecimalFilter OneDayChangeInPercent { get; set; }

        /// <summary>
        /// Filter by percentage change for the week
        /// </summary>
        public DecimalFilter OneWeekChangeInPercent { get; set; }

        /// <summary>
        /// Filter by percentage change for the month
        /// </summary>
        public DecimalFilter OneMonthChangeInPercent { get; set; }

        /// <summary>
        /// Filter by percentage change for the year
        /// </summary>
        public DecimalFilter OneYearChangeInPercent { get; set; }

        /// <summary>
        /// Filter by percentage change over three years
        /// </summary>
        public DecimalFilter ThreeYearsChangeInPercent { get; set; }

        /// <summary>
        /// Filter by percentage change over five years
        /// </summary>
        public DecimalFilter FiveYearsChangeInPercent { get; set; }

        /// <summary>
        /// Filter by Average Vq value
        /// </summary>
        public DecimalFilter AvgVqFilter { get; set; }

        /// <summary>
        /// Filter by Volume
        /// </summary>
        public DecimalFilter VolumeFilter { get; set; }

        /// <summary>
        /// Filter by latest RSI(14) value 
        /// </summary>
        public DecimalFilter LatestRsi14Filter { get; set; }

        /// <summary>
        /// List of global ranks to apply as filter
        /// </summary>
        public List<GlobalRankTypes> GlobalRankIds { get; set; }
    }
}
