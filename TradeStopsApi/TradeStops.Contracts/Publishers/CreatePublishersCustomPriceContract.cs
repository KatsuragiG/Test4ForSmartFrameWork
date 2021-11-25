using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for creating a custom price
    /// </summary>
    public class CreatePublishersCustomPriceContract
    {
        /// <summary>
        /// Trade date
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// Close price
        /// </summary>
        public virtual decimal ClosePrice { get; set; }
        
        /// <summary>
        /// Custom symbol id
        /// </summary>
        public virtual int CustomSymbolId { get; set; }       
    }
}