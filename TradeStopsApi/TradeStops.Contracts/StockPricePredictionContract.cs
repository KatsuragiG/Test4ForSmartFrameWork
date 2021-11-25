using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Predicted stock price values.
    /// </summary>
    public class StockPricePredictionContract
    {
        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Date of prediction.
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// Upper boundary of the predicted value for a very low probability level.
        /// </summary>
        public decimal Upper10BoundAdj { get; set; }

        /// <summary>
        /// Lower boundary of the predicted value for a very low probability level.
        /// </summary>
        public decimal Lower10BoundAdj { get; set; }

        /// <summary>
        /// Upper boundary of the predicted value for a low probability level.
        /// </summary>
        public decimal Upper35BoundAdj { get; set; }

        /// <summary>
        /// Lower boundary of the predicted value for a low probability level.
        /// </summary>
        public decimal Lower35BoundAdj { get; set; }

        /// <summary>
        /// Upper boundary of the predicted value for a medium probability level.
        /// </summary>
        public decimal Upper70BoundAdj { get; set; }

        /// <summary>
        /// Lower boundary of the predicted value for a medium probability level.
        /// </summary>
        public decimal Lower70BoundAdj { get; set; }

        /// <summary>
        /// Upper boundary of the predicted value for a high probability level.
        /// </summary>
        public decimal Upper90BoundAdj { get; set; }

        /// <summary>
        /// Lower boundary of the predicted value for a high probability level.
        /// </summary>
        public decimal Lower90BoundAdj { get; set; }
    }
}
