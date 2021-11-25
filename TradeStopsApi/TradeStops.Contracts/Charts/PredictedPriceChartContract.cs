using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Data for predicted prices with symbol historical price line.
    /// </summary>
    public class PredictedPriceChartContract
    {
        /// <summary>
        /// Price line values for a chart.
        /// </summary>
        public List<PriceLinePointContract> PriceLine { get; set; }

        /// <summary>
        /// Predicted price areas for a chart.
        /// </summary>
        public List<PredictedPriceAreaContract> PredictedPriceAreas { get; set; }
    }
}
