using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Dividend contract
    /// </summary>
    public class PublishersDividendContract
    {
        /// <summary>
        /// Dividend id
        /// </summary>
        public int DividendId { get; set; }

        /// <summary>
        /// Type of dividend
        /// </summary>
        public PublishersDividendTypes Type { get; set; }

        /// <summary>
        /// Date when the dividend is announced.
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// Date when the dividend is payed.
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// Split adjusted value per share.
        /// </summary>
        public decimal SplitAdjustedValue { get; set; }

        /// <summary>
        /// Split adjusted drip value.
        /// </summary>
        public decimal? SplitAdjustedDripValue { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Frequency of dividend.
        /// </summary>
        public PublishersDividendFrequencyTypes DividendFrequency { get; set; }

        /// <summary>
        /// Indicates whether the dividend is included into related calculations or not.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Subtrade Id of dividend.
        /// </summary>
        public int SubtradeId { get; set; }

        /// <summary>
        /// Dividend owner id.
        /// </summary>
        public int? OwnerId { get; set; }
    }
}
