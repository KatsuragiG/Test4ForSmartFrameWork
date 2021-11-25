namespace TradeStops.Contracts
{
    /// <summary>
    /// Identifier of remote device
    /// </summary>
    public class RemoteDeviceIdContract
    {
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public RemoteDeviceIdContract()
        {
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="deviceId">Device identifier</param>
        public RemoteDeviceIdContract(string deviceId)
        {
            DeviceId = deviceId;
        }

        /// <summary>
        /// Device identifier
        /// </summary>
        public string DeviceId { get; set; }
    }
}
