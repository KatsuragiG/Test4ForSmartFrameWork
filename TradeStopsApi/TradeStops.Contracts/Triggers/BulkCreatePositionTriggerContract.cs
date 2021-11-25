namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create single Trigger for single Position.
    /// Contract is used in bulk-create scenarios, because to create trigger for single position we pass positionId in url
    /// </summary>
    public class BulkCreatePositionTriggerContract
    {
        /// <summary>
        /// ID of the Position.
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// Trigger to create.
        /// </summary>
        public TriggerFieldsContract Trigger { get; set; }
    }
}
