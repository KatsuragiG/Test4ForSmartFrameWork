namespace TradeStops.Contracts
{
    /// <summary>
    /// Alert template trigger
    /// </summary>
    public class AlertTemplateTriggerContract
    {
        /// <summary>
        /// Id of the trigger type assigned to the alert template.
        /// </summary>
        public int AlertTemplateTriggerTypeId { get; set; }

        /// <summary>
        /// Alert template Id.
        /// </summary>
        public int AlertTemplateId { get; set; }

        /// <summary>
        /// Type of Position Trigger.
        /// </summary>
        public TriggerTypeContract TriggerType { get; set; }

        /// <summary>
        /// Alert template Id.
        /// </summary>
        public TriggerFieldsContract Trigger { get; set; }

        /// <summary>
        /// Trigger description. Default currency is used to generate this string.
        /// </summary>
        public string TriggerDescription { get; set; }
    }
}
