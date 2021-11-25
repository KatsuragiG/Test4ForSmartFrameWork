namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to edit alert template
    /// </summary>
    public class EditAlertTemplateContract
    {
        /// <summary>
        /// Title of the alert template.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Determines if the alert template is default on creating position.
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
