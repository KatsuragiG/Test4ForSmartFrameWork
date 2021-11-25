using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Custom price contract
    /// </summary>
    public class PublishersCustomPriceContract
    {
        /// <summary>
        /// Custom price id
        /// </summary>
        public virtual int CustomPriceId { get; set; }

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

        /// <summary>
        /// Owner id
        /// </summary>
        public virtual int? OwnerId { get; set; }
    }
}
