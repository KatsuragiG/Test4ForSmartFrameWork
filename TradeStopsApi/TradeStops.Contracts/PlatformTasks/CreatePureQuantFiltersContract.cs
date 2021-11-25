using System.Collections.Generic;
using TradeStops.Common.DataStructures;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Pure Quant filter parameters to create task.
    /// </summary>
    public class CreatePureQuantFiltersContract
    {
        /// <summary>
        /// Health (SSI) status filter.
        /// </summary>
        public List<SsiStatuses> HealthStatuses { get; set; }

        /// <summary>
        /// Average VQ for stocks filter.
        /// </summary>
        public DecimalFilter AverageVqThresholdForStocks { get; set; }

        /// <summary>
        /// Average VQ for cryptos filter.
        /// </summary>
        public DecimalFilter AverageVqThresholdForCryptos { get; set; }

        /// <summary>
        /// The stock must be at a gain since it most recently entered the Green State.
        /// </summary>
        public bool ApplyPositiveGainFilter { get; set; }

        /// <summary>
        /// Average Volume Shares for US stocks.
        /// </summary>
        public DecimalFilter AverageVolumeThresholdForUsStocks { get; set; }

        /// <summary>
        /// Average Volume Shares for non-US stocks.
        /// </summary>
        public DecimalFilter AverageVolumeThresholdForNonUsStocks { get; set; }

        /// <summary>
        /// Average Volume Shares for Crypto.
        /// </summary>
        public DecimalFilter AverageVolumeThresholdForCryptos { get; set; }

        /// <summary>
        /// Average Daily Volume Price($) for US stocks.
        /// </summary>
        public DecimalFilter AverageVolumeValueThresholdForUsStocks { get; set; }

        /// <summary>
        /// Average Daily Volume Price($) for non-US stocks.
        /// </summary>
        public DecimalFilter AverageVolumeValueThresholdForNonUsStocks { get; set; }

        /// <summary>
        /// Average Daily Volume Price($) for Crypto.
        /// </summary>
        public DecimalFilter AverageVolumeValueThresholdForCryptos { get; set; }

        /// <summary>
        /// Maximize Diversification.
        /// </summary>
        public bool ApplyCorrelationAlgorithm { get; set; }

        /// <summary>
        /// Market Capitalization types filter.
        /// </summary>
        public List<MarketCapTypes> MarketCapTypes { get; set; }

        /// <summary>
        /// Allow fractional shares in the result.
        /// </summary>
        public bool AllowFractionalShares { get; set; }
    }
}
