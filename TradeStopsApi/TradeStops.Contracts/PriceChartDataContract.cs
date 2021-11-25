using System.Collections.Generic;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class PriceChartDataContract
    {
        /// <summary>
        /// Price line values for a chart.
        /// </summary>
        public IEnumerable<PriceLinePointContract> PriceLine { get; set; }

        /// <summary>
        /// Timing turn areas for a chart.
        /// </summary>
        public IEnumerable<TimingTurnAreaContract> TimingTurnAreas { get; set; }

        /// <summary>
        /// Chart item values.
        /// </summary>
        public List<PriceChartItemContract> ChartItems { get; set; }

        /// <summary>
        /// Currency sign.
        /// </summary>
        public string CurrencySign { get; set; }
    }
}