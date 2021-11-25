using System.Collections.Generic;
using TradeStops.Common.DataStructures;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Input parameters for Quant Tool
    /// </summary>
    public class QuantToolInputContract
    {
        /// <summary>
        /// All calculations will be performed in this currency in case positions were provided in multiple currencies.
        /// </summary>
        public int DefaultCurrencyId { get; set; }

        /// <summary>
        /// The quant tool will equally disctribute these funds according to the risk of each position.
        /// </summary>
        public decimal InvestmentAmount { get; set; }

        /// <summary>
        /// (optional) Selected number will determine how many positions are returned and how the share sizes are determined.
        /// </summary>
        public int? NumberOfPositions { get; set; }

        /// <summary>
        /// Determines if the prices adjusted by dividends will be used in calculations.
        /// </summary>
        public bool AdjustByDividends { get; set; }

        /// <summary>
        /// Determines if the Quant Tool runs only for selected positions.
        /// </summary>
        public bool IsRebalanceByPositions { get; set; }

        /// <summary>
        /// Type of a strategy for the QuantTool.
        /// </summary>
        public QuantToolStrategyTypes StrategyType { get; set; }

        /// <summary>
        /// Array of symbols for processing.
        /// </summary>
        public List<QuantToolInputPositionContract> Positions { get; set; }

        /// <summary>
        /// (optional) Allow QuantTool to recommend fractional shares.
        /// </summary>
        public bool AllowFractionalShares { get; set; }

        /// <summary>
        /// VQ Threshold value used in the algorithm for Stocks. Recommended default value is less than 40%
        /// </summary>
        public DecimalFilter AverageVqThreshold { get; set; }

        /// <summary>
        /// VQ Threshold value used in the algorithm for Cryptos. Recommended default value is less than 80%
        /// </summary>
        public DecimalFilter AverageVqThresholdForCryptos { get; set; }

        /// <summary>
        /// Specifies if diversification algorithm should be applied.
        /// </summary>
        public bool ApplyDiversification { get; set; }

        /// <summary>
        /// Average one year Daily Volume Shares threshold value used in the algorithm for US Stocks. Recommended default value is more than 200000
        /// </summary>
        public DecimalFilter AverageVolumeThreshold { get; set; }

        /// <summary>
        /// Average one year Daily Volume Price threshold value used in the algorithm for US Stocks. Recommended default value is more than 2000000
        /// </summary>
        public DecimalFilter AverageVolumeValueThreshold { get; set; }

        /// <summary>
        /// Average one year Daily Volume Shares threshold value used in the algorithm for non-US Stocks. Recommended default value is more than 200000
        /// </summary>
        public DecimalFilter AverageVolumeThresholdForNonUsStocks { get; set; }

        /// <summary>
        /// Average one year Daily Volume Price threshold value used in the algorithm for non-US Stocks. Recommended default value is more than 2000000
        /// </summary>
        public DecimalFilter AverageVolumeValueThresholdForNonUsStocks { get; set; }

        /// <summary>
        /// Average one year Daily Volume Shares threshold value used in the algorithm for Cryptos. Recommended default value is more than 200000
        /// </summary>
        public DecimalFilter AverageVolumeThresholdForCryptos { get; set; }

        /// <summary>
        /// Average one year Daily Volume Price threshold value used in the algorithm for Cryptos. Recommended default value is more than 2000000
        /// </summary>
        public DecimalFilter AverageVolumeValueThresholdForCryptos { get; set; }

        /// <summary>
        /// Additional positions that will be included into analyzis with possibility to skip Pure Quant filter.
        /// </summary>
        public List<QuantToolInputPositionContract> AdditionalPositions { get; set; }

        /// <summary>
        /// Indicates whether Pure Quant filter must be bypassed for additional positions.
        /// </summary>
        public bool BypassPureQuantFilterForAdditionalPositions { get; set; }

        /// <summary>
        /// Include all active symbols from the following sectors.
        /// </summary>
        public List<int> SectorIds { get; set; }

        /// <summary>
        /// Filter results by provided Martet Cap types.
        /// </summary>
        public List<MarketCapTypes> MarketCapTypes { get; set; }

        /// <summary>
        /// Include all active symbols from the following contries.
        /// </summary>
        public List<ExchangeCountryTypes> ExchangeCountries { get; set; }

        /// <summary>
        /// Filter results by provided SSI types.
        /// </summary>
        public List<SsiStatuses> SsiTypes { get; set; }

        /// <summary>
        /// Specifies if positive gain since last Entry filter must be applied..
        /// </summary>
        public bool ApplyPositiveGainFilter { get; set; }

        /// <summary>
        /// Include all active symbols matching provided Stock Finder criteria.
        /// </summary>
        public StockFinderInputContract StockFinderSource { get; set; }

        /// <summary>
        /// Include provided ETF and other Fund symbols.
        /// </summary>
        public List<int> AdditionalFundSymbolIds { get; set; }

        /// <summary>
        /// Include components of provided EFT and other Fund symbols.
        /// </summary>
        public bool IncludeFundComponents { get; set; }
    }
}
