using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create user notification address
    /// </summary>
    public class CreateNotificationAddressContract
    {
        /// <summary>
        ///  Address type.
        /// </summary>
        public AddressTypes AddressType { get; set; }

        /// <summary>
        /// User address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// (optional) Mobile Operator values.
        /// </summary>
        public int? MobileOperatorId { get; set; }
    }
}
