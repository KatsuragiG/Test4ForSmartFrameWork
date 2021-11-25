using System;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class PriceLinePointContract
    {
        /// <summary>
        /// Price date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Currency sign.
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// Opening  price for the given Trade Date.
        /// </summary>
        public decimal Open { get; set; }

        /// <summary>
        /// Closing price for the given Trade Date.
        /// </summary>
        public decimal Close { get; set; }

        /// <summary>
        /// Highest price for the given Trade Date.
        /// </summary>
        public decimal High { get; set; }

        /// <summary>
        /// Lowest price for the given Trade Date.
        /// </summary>
        public decimal Low { get; set; }
    }
}