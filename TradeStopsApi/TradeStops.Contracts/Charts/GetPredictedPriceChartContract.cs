using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Values to get predicted price lines.
    /// </summary>
    public class GetPredictedPriceChartContract
    {
        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Adjust symbols prices history by dividends.
        /// </summary>
        public bool AdjustByDividends { get; set; }

        /// <summary>
        /// List of price probability levels to calculate prediction.
        /// </summary>
        public List<PriceProbabilityLevelTypes> PriceProbabilityLevels { get; set; }

        /// <summary>
        /// Predict prices till the date.
        /// </summary>
        public DateTime PredictionToDate { get; set; }

        /// <summary>
        /// Get price line from the date.
        /// </summary>
        public DateTime PriceLineFromDate { get; set; }
    }
}
