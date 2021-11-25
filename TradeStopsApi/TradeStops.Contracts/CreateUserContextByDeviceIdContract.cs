namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create UserContext by DeviceId
    /// </summary>
    public class CreateUserContextByDeviceIdContract
    {
        /// <summary>
        /// Device Identifier
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// Device Model Name (like iPhone 5s)
        /// </summary>
        public string DeviceName { get; set; }
    }
}
