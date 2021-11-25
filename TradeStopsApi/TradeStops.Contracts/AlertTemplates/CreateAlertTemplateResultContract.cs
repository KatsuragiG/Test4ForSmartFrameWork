using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Result of alert templates creation
    /// </summary>
    public class CreateAlertTemplateResultContract
    {
        /// <summary>
        /// Alert template
        /// </summary>
        public AlertTemplateContract Template { get; set; }

        /// <summary>
        /// List of template triggers
        /// </summary>
        public List<AlertTemplateTriggerContract> Triggers { get; set; }
    }
}
