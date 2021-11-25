using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Broker pill is a block with broker name that is displayed in 'Import Portfolio' popup
    /// </summary>
    public class BrokerPillContract
    {
        /// <summary>
        /// Broker pill ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Pill name.
        /// </summary>
        public string PillName { get; set; }

        /// <summary>
        /// Pill logo image name.
        /// </summary>
        public string PillImage { get; set; }

        /// <summary>
        /// Pill page type.
        /// </summary>
        public BrokerPillsPage PillPage { get; set; }

        /// <summary>
        /// Pill's CSS class
        /// </summary>
        public string PillIconClass { get; set; }
    }
}
