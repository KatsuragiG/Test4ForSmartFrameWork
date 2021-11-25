using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for creating a dividend
    /// </summary>
    public class CreatePublishersDividendContract
    {
        /// <summary>
        /// Date when the dividend is announced.
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// Date when the dividend is payed.
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// Non adjusted value.
        /// </summary>
        public decimal NonAdjustedValue { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Indicates whether the dividend is included into related calculations or not.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Subtrade Id of dividend.
        /// </summary>
        public int SubtradeId { get; set; }
    }
}
