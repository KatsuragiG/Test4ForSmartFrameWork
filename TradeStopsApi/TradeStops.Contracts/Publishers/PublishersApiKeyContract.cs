namespace TradeStops.Contracts
{
    /// <summary>
    /// Customer api key
    /// </summary>
    public class PublishersApiKeyContract
    {
        /// <summary>
        /// Api key id
        /// </summary>
        public int ApiKeyId { get; set; }

        /// <summary>
        /// Api key value
        /// </summary>
        public string KeyString { get; set; }
    }
}
