using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Predicted price values.
    /// </summary>
    public class PredictedPriceAreaContract
    {
        /// <summary>
        /// Predicted price area values.
        /// </summary>
        public List<ChartAreaPointContract> PredictedPriceArea { get; set; }

        /// <summary>
        /// Probability level of predicted prices.
        /// </summary>
        public PriceProbabilityLevelTypes PriceProbabilityLevel { get; set; }
    }
}
