using System;
using System.Collections.Generic;
using System.Text;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Trigger description
    /// </summary>
    public class TriggerDescriptionContract
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public TriggerDescriptionContract()
        {
        }

        /// <summary>
        /// Parametrized ctor
        /// </summary>
        /// <param name="triggerDescription">trigger description</param>
        public TriggerDescriptionContract(string triggerDescription)
        {
            TriggerDescription = triggerDescription;
        }

        /// <summary>
        /// Trigger description that includes trigger name with applied parameters
        /// </summary>
        public string TriggerDescription { get; set; }
    }
}
