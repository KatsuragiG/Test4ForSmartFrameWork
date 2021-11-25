namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create alert template
    /// </summary>
    public class CreateAlertTemplateContract
    {
        /// <summary>
        /// Alert template name.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Determines if the alert template is default on creating position.
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
